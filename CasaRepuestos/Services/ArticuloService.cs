using CasaRepuestos.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CasaRepuestos.Services
{
    public class ArticuloService
    {
        public List<Articulo> ListarArticulos()
        {
            var lista = new List<Articulo>();
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

           
            var query = @"
                    SELECT 
                        a.idarticulo, a.nombre, a.precio, a.stock, 
                        COALESCE(a.stock_comprometido, 0) AS stock_comprometido,
                        a.idmarca, m.nombre AS marca_nombre,
                        COALESCE(a.precioCosto, 0) AS precioCosto, 
                        COALESCE(a.porcentajeGanancia, 0) AS porcentajeGanancia,
                        COALESCE(a.IVA, 0) AS IVA,
                        prov_list.proveedores_nombres
                    FROM articulos a
                    LEFT JOIN marcas m ON a.idmarca = m.idmarca
                    LEFT JOIN (
                        SELECT 
                            ap.idarticulo,
                            GROUP_CONCAT(DISTINCT p.razon_social SEPARATOR ', ') AS proveedores_nombres
                        FROM articulo_proveedor ap
                        JOIN proveedores p ON ap.idproveedor = p.idproveedor
                        GROUP BY ap.idarticulo
                    ) AS prov_list ON a.idarticulo = prov_list.idarticulo

                    ORDER BY a.nombre;
                    ";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var articulo = new Articulo
                {
                    IdArticulo = reader.GetInt32("idarticulo"),
                    Nombre = reader.GetString("nombre"),

                    Precio = reader.GetDecimal("precio"),

                    Stock = reader.GetInt32("stock"),
                    StockComprometido = reader.GetInt32("stock_comprometido"),
                    IdMarca = reader.GetInt32("idmarca"),
                    PrecioCosto = reader.GetDecimal("precioCosto"),
                    PorcentajeGanancia = reader.GetDecimal("porcentajeGanancia"),
                    IVA = reader.GetDecimal("IVA"),

                    Marca = new Marca
                    {
                        IdMarca = reader.GetInt32("idmarca"),
                        Nombre = reader.GetString("marca_nombre")
                    },
                    NombresProveedores = reader.IsDBNull(reader.GetOrdinal("proveedores_nombres"))
                                        ? ""
                                        : reader.GetString("proveedores_nombres")
                };
                lista.Add(articulo);
            }
            return lista;
        }


        public List<Articulo> ListarArticulosVendibles()
        {
            var lista = new List<Articulo>();
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            var query = @"
        SELECT 
            a.idarticulo, a.nombre, a.precio, a.stock, 
            COALESCE(a.stock_comprometido, 0) AS stock_comprometido,
            a.idmarca, m.nombre AS marca_nombre
        FROM articulos a
        JOIN marcas m ON a.idmarca = m.idmarca
        WHERE a.stock - COALESCE(a.stock_comprometido, 0) > 0";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var stockTotal = reader.GetInt32("stock");
                var stockComprometido = reader.GetInt32("stock_comprometido");
                int stockDisponibleVenta = stockTotal - stockComprometido;

            }

            return lista;
        }
        internal List<Articulo> ListarArticulosActivos()
        {
            var articulos = new List<Articulo>();

            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            const string query = @"
            SELECT idarticulo, nombre, precio, stock, idmarca
            FROM articulos
            WHERE activo = 1";

            using var cmd = new MySqlCommand(query, conn);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                articulos.Add(new Articulo
                {
                    IdArticulo = reader.GetInt32("idarticulo"),
                    Nombre = reader.GetString("nombre"),
                    Precio = reader.GetDecimal("precio"),
                    Stock = reader.GetInt32("stock"),
                    IdMarca = reader.GetInt32("idmarca")
                    
                });
            }

            return articulos;
        }

        
        public void CrearArticulo(Articulo articulo)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {

                var insertArticulo = @"
        INSERT INTO articulos (nombre, precio, stock, idmarca, porcentajeGanancia, IVA, precioCosto) 
        VALUES (@nombre, @precio, @stock, @idmarca, @porcentajeGanancia, @IVA, 0);
        SELECT LAST_INSERT_ID();";

                long newArticuloId;
                using (var cmdArticulo = new MySqlCommand(insertArticulo, conn, transaction))
                {
                    cmdArticulo.Parameters.AddWithValue("@nombre", articulo.Nombre);
                    cmdArticulo.Parameters.AddWithValue("@precio", articulo.Precio);
                    cmdArticulo.Parameters.AddWithValue("@stock", articulo.Stock);
                    cmdArticulo.Parameters.AddWithValue("@idmarca", articulo.IdMarca);
                    cmdArticulo.Parameters.AddWithValue("@porcentajeGanancia", articulo.PorcentajeGanancia);
                    cmdArticulo.Parameters.AddWithValue("@IVA", articulo.IVA);
                    newArticuloId = Convert.ToInt64(cmdArticulo.ExecuteScalar());
                }

                var insertRelacion = @"
            INSERT INTO articulo_proveedor (idarticulo, idproveedor, precioCoste) 
            VALUES (@idarticulo, @idproveedor, @precioCoste)";

                foreach (var provConCosto in articulo.ProveedoresConCosto)
                {
                    if (provConCosto.PrecioCoste <= 0)
                        throw new Exception($"El proveedor ID {provConCosto.IdProveedor} tiene un costo inválido.");

                    using (var cmdRelacion = new MySqlCommand(insertRelacion, conn, transaction))
                    {
                        cmdRelacion.Parameters.AddWithValue("@idarticulo", newArticuloId);
                        cmdRelacion.Parameters.AddWithValue("@idproveedor", provConCosto.IdProveedor);
                        cmdRelacion.Parameters.AddWithValue("@precioCoste", provConCosto.PrecioCoste);
                        cmdRelacion.ExecuteNonQuery();
                    }
                }

                ActualizarPrecioCostoArticulo(Convert.ToInt32(newArticuloId), conn, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void ModificarArticulo(Articulo articulo)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {

                var updateArticulo = @"
        UPDATE articulos 
        SET nombre = @nombre,
            precio = @precio,
            stock = @stock,
            idmarca = @idmarca,
            porcentajeGanancia = @porcentajeGanancia,
            IVA = @IVA
        WHERE idarticulo = @idarticulo";

                using (var cmd = new MySqlCommand(updateArticulo, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@nombre", articulo.Nombre);
                    cmd.Parameters.AddWithValue("@precio", articulo.Precio);
                    cmd.Parameters.AddWithValue("@stock", articulo.Stock);
                    cmd.Parameters.AddWithValue("@idmarca", articulo.IdMarca);
                    cmd.Parameters.AddWithValue("@idarticulo", articulo.IdArticulo);
                    cmd.Parameters.AddWithValue("@porcentajeGanancia", articulo.PorcentajeGanancia);
                    cmd.Parameters.AddWithValue("@IVA", articulo.IVA);
                    cmd.ExecuteNonQuery();
                }

                //  Borra las relaciones ANTERIORES
                var deleteRelaciones = "DELETE FROM articulo_proveedor WHERE idarticulo = @idarticulo";
                using (var cmdDelete = new MySqlCommand(deleteRelaciones, conn, transaction))
                {
                    cmdDelete.Parameters.AddWithValue("@idarticulo", articulo.IdArticulo);
                    cmdDelete.ExecuteNonQuery();
                }

                var insertRelacion = @"
            INSERT INTO articulo_proveedor (idarticulo, idproveedor, precioCoste) 
            VALUES (@idarticulo, @idproveedor, @precioCoste)";

                foreach (var provConCosto in articulo.ProveedoresConCosto)
                {
                    if (provConCosto.PrecioCoste <= 0)
                        throw new Exception($"El proveedor ID {provConCosto.IdProveedor} tiene un costo inválido.");

                    using (var cmdRelacion = new MySqlCommand(insertRelacion, conn, transaction))
                    {
                        cmdRelacion.Parameters.AddWithValue("@idarticulo", articulo.IdArticulo);
                        cmdRelacion.Parameters.AddWithValue("@idproveedor", provConCosto.IdProveedor);
                        cmdRelacion.Parameters.AddWithValue("@precioCoste", provConCosto.PrecioCoste);
                        cmdRelacion.ExecuteNonQuery();
                    }
                }

                // Actualizamos el precioCosto MAX()
                ActualizarPrecioCostoArticulo(articulo.IdArticulo, conn, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }



        public Articulo ObtenerArticuloPorId(int idArticulo)
        {
            Articulo articulo = null;
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            var query = @"
    SELECT 
        a.idarticulo, a.nombre, a.precio, a.stock, 
        COALESCE(a.stock_comprometido, 0) AS stock_comprometido,
        COALESCE(a.precioCosto, 0) AS precioCosto, 
        COALESCE(a.porcentajeGanancia, 0) AS porcentajeGanancia,
        COALESCE(a.IVA, 0) AS IVA,
        a.idmarca, m.nombre AS marca_nombre
    FROM articulos a
    LEFT JOIN marcas m ON a.idmarca = m.idmarca
    WHERE a.idarticulo = @idArticulo";

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idArticulo", idArticulo);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        articulo = new Articulo
                        {
                            IdArticulo = reader.GetInt32("idarticulo"),
                            Nombre = reader.GetString("nombre"),
                            Precio = reader.GetDecimal("precio"),
                            Stock = reader.GetInt32("stock"),
                            StockComprometido = reader.GetInt32("stock_comprometido"),
                            PrecioCosto = reader.GetDecimal("precioCosto"),
                            PorcentajeGanancia = reader.GetDecimal("porcentajeGanancia"),
                            IVA = reader.GetDecimal("IVA"),
                            IdMarca = reader.GetInt32("idmarca"),
                            Marca = new Marca
                            {
                                IdMarca = reader.GetInt32("idmarca"),
                                Nombre = reader.GetString("marca_nombre")
                            },
                            ProveedoresConCosto = new List<ArticuloProveedor>()
                        };
                    }
                } 
            }

            if (articulo != null)
            {
                var queryProveedores = @"
            SELECT idproveedor, precioCoste 
            FROM articulo_proveedor 
            WHERE idarticulo = @idArticulo";

                using (var cmdProv = new MySqlCommand(queryProveedores, conn))
                {
                    cmdProv.Parameters.AddWithValue("@idArticulo", idArticulo);
                    using (var readerProv = cmdProv.ExecuteReader())
                    {
                        while (readerProv.Read())
                        {
                            articulo.ProveedoresConCosto.Add(new ArticuloProveedor
                            {
                                IdArticulo = idArticulo,
                                IdProveedor = readerProv.GetInt32("idproveedor"),
                                PrecioCoste = readerProv.GetDecimal("precioCoste")
                            });
                        }
                    }
                }
            }

            return articulo;
        }

        public void ActualizarArticulo(Articulo articulo)
        {
            ModificarArticulo(articulo);
        }

        public void EliminarArticulo(int idArticulo)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            var delete = "DELETE FROM articulos WHERE idarticulo = @id";
            using var cmd = new MySqlCommand(delete, conn);
            cmd.Parameters.AddWithValue("@id", idArticulo);
            cmd.ExecuteNonQuery();
        }


        public bool ActualizarStock(int idArticulo, int cantidadUsada)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            // Verificar stock suficiente
            var queryVerificar = "SELECT stock FROM articulos WHERE idarticulo = @id";
            using var cmdVerificar = new MySqlCommand(queryVerificar, conn);
            cmdVerificar.Parameters.AddWithValue("@id", idArticulo);

            var stockActual = Convert.ToInt32(cmdVerificar.ExecuteScalar() ?? 0);

            if (stockActual < cantidadUsada)
            {
                throw new Exception($"Stock insuficiente. Disponible: {stockActual}, Requerido: {cantidadUsada}");
            }

            // Actualizar stock
            var queryActualizar = "UPDATE articulos SET stock = stock - @cantidad WHERE idarticulo = @id";
            using var cmdActualizar = new MySqlCommand(queryActualizar, conn);
            cmdActualizar.Parameters.AddWithValue("@cantidad", cantidadUsada);
            cmdActualizar.Parameters.AddWithValue("@id", idArticulo);

            return cmdActualizar.ExecuteNonQuery() > 0;
        }

      
        public bool VerificarStock(int idArticulo, int cantidadRequerida)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            var query = "SELECT stock FROM articulos WHERE idarticulo = @id";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", idArticulo);

            var result = cmd.ExecuteScalar();
            if (result != null)
            {
                int stockActual = Convert.ToInt32(result);
                return stockActual >= cantidadRequerida;
            }
            return false;
        }

        public List<Articulo> ObtenerTodosArticulosSimples()
        {
            var lista = new List<Articulo>();
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            var query = "SELECT idarticulo, nombre FROM articulos ORDER BY nombre;";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Articulo
                {
                    IdArticulo = reader.GetInt32("idarticulo"),
                    Nombre = reader.GetString("nombre")
                });
            }
            return lista;
        }

        public void ActualizarPrecioCostoArticulo(int idArticulo, MySqlConnection connection = null, MySqlTransaction transaction = null)
        {
            MySqlConnection conn = connection ?? new MySqlConnection(Config.Config.ConnectionString);
            bool closeConnection = (connection == null);

            try
            {
                if (closeConnection) conn.Open();

                // Obtener el precioCoste MÁS ALTO de los proveedores
                string sqlGetMaxCosto = @"
            SELECT MAX(ap.precioCoste) 
            FROM articulo_proveedor ap
            WHERE ap.idarticulo = @idArticulo";

                decimal maxPrecioCosto = 0m;
                using (var cmdGet = new MySqlCommand(sqlGetMaxCosto, conn, transaction))
                {
                    cmdGet.Parameters.AddWithValue("@idArticulo", idArticulo);
                    var result = cmdGet.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        maxPrecioCosto = Convert.ToDecimal(result);
                    }
                }

                //  Obtener la Ganancia y el IVA de este artículo
                string sqlGetFinanzas = @"
            SELECT porcentajeGanancia, IVA 
            FROM articulos 
            WHERE idarticulo = @idArticulo";

                decimal ganancia = 0m;
                decimal iva = 0m;
                using (var cmdFinanzas = new MySqlCommand(sqlGetFinanzas, conn, transaction))
                {
                    cmdFinanzas.Parameters.AddWithValue("@idArticulo", idArticulo);
                    using (var reader = cmdFinanzas.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ganancia = reader.GetDecimal("porcentajeGanancia");
                            iva = reader.GetDecimal("IVA");
                        }
                    }
                } 

                decimal precioConGanancia = maxPrecioCosto * (1 + (ganancia / 100m));
                decimal nuevoPrecioVenta = precioConGanancia * (1 + (iva / 100m));

                string sqlUpdateArticulo = @"
            UPDATE articulos
            SET 
                precioCosto = @precioCosto, -- (El MAX() de los proveedores)
                precio = @precioVenta      -- (El Precio de Venta recalculado)
            WHERE idarticulo = @idArticulo";

                using (var cmdUpdate = new MySqlCommand(sqlUpdateArticulo, conn, transaction))
                {
                    cmdUpdate.Parameters.AddWithValue("@precioCosto", maxPrecioCosto);
                    cmdUpdate.Parameters.AddWithValue("@precioVenta", nuevoPrecioVenta);
                    cmdUpdate.Parameters.AddWithValue("@idArticulo", idArticulo);
                    cmdUpdate.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el precioCosto para el artículo {idArticulo}: {ex.Message}", ex);
            }
            finally
            {
                if (closeConnection) conn.Close();
            }
        }

        public decimal ObtenerPrecioCosto(int idArticulo, int idProveedor)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            string sql = @"
        SELECT precioCoste 
        FROM articulo_proveedor 
        WHERE idarticulo = @idArticulo AND idproveedor = @idProveedor";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@idArticulo", idArticulo);
                cmd.Parameters.AddWithValue("@idProveedor", idProveedor);
                var result = cmd.ExecuteScalar();
                if (result != DBNull.Value && result != null)
                {
                    return Convert.ToDecimal(result);
                }
            }
            // Si no hay un precio específico, devuelve 0.
            return 0m;
        }

        public int ObtenerIdProveedorMasCaro(int idArticulo)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            string sql = @"
        SELECT idproveedor 
        FROM articulo_proveedor 
        WHERE idarticulo = @idArticulo 
        ORDER BY precioCoste DESC 
        LIMIT 1";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@idArticulo", idArticulo);
                var result = cmd.ExecuteScalar();
                if (result != DBNull.Value && result != null)
                {
                    return Convert.ToInt32(result);
                }
            }
            return 0; 
        }

    }

}