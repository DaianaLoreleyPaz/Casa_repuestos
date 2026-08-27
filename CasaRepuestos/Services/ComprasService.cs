using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CasaRepuestos.Models;
using MySql.Data.MySqlClient;

namespace CasaRepuestos.Services
{
    public class ComprasService
    {
        private readonly ArticuloService _articuloService = new ArticuloService();
        // ====================================================================
        // SECCIÓN 1: GESTIÓN DE ÓRDENES DE COMPRA (SIN AFECTAR STOCK)
        // ====================================================================

        

        // Valida los datos de una orden de compra antes de su creación
        private void ValidarOrdenCompra(OrdenCompra orden)
        {
            if (orden == null)// Verifica si la orden es nula
                throw new Exception("La orden no puede ser nula.");

            if (orden.IdProveedor <= 0)// Verifica si el ID del proveedor es válido
                throw new Exception("Debe seleccionar un proveedor válido.");

            if (orden.FechaCreacion == default)// Verifica si la fecha de creación es válida
                throw new Exception("Debe indicar la fecha de creación de la orden.");

            if (string.IsNullOrWhiteSpace(orden.Tipo))// Verifica si el tipo de orden es válido
                throw new Exception("Debe indicar el tipo de orden.");

            if (orden.TotalEstimado <= 0)// Verifica si el total estimado es mayor que cero
                throw new Exception("El total estimado debe ser mayor que cero.");

            if (orden.Detalles == null || orden.Detalles.Count == 0)// Verifica si hay detalles en la orden
                throw new Exception("Debe agregar al menos un detalle a la orden.");

            foreach (var detalle in orden.Detalles)// Valida cada detalle de la orden
            {
                if (detalle.IdArticulo <= 0)// Verifica si el ID del artículo es válido
                    throw new Exception("Cada detalle debe tener un artículo válido.");
                if (detalle.CantidadSolicitada <= 0)// Verifica si la cantidad solicitada es mayor que cero
                    throw new Exception("La cantidad solicitada debe ser mayor que cero.");
                if (detalle.PrecioUnitarioEstimado <= 0)// Verifica si el precio unitario estimado es mayor que cero
                    throw new Exception("El precio estimado debe ser mayor que cero.");
            }
        }
        // Crea una nueva orden de compra en la base de datos
        public int CrearOrdenDeCompra(OrdenCompra orden)
        {
            ValidarOrdenCompra(orden);// Valida la orden antes de crearla
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            using var tran = conn.BeginTransaction();// Inicia una transacción para asegurar la integridad de los datos

            try
            {
                // Inserta la orden de compra y obtiene el ID generado
                var insertOrden = @"
                INSERT INTO ordenes_compra (fecha_emision, total_estimado, idproveedor, estado, tipo)
                VALUES (@fecha, @total, @idproveedor, 'PENDIENTE', @tipo);
                SELECT LAST_INSERT_ID();";
                // Crea el comando para insertar la orden
                using var cmdOrden = new MySqlCommand(insertOrden, conn, tran);
                cmdOrden.Parameters.AddWithValue("@fecha", orden.FechaCreacion);
                cmdOrden.Parameters.AddWithValue("@total", orden.TotalEstimado);
                cmdOrden.Parameters.AddWithValue("@idproveedor", orden.IdProveedor);
                cmdOrden.Parameters.AddWithValue("@tipo", orden.Tipo);

                int idOrden = Convert.ToInt32(cmdOrden.ExecuteScalar());// Ejecuta el comando y obtiene el ID de la orden creada
                foreach (var det in orden.Detalles)// Inserta cada detalle de la orden
                {
                    // Crea el comando para insertar el detalle
                    var insertDet = @"
                        INSERT INTO detalles_orden_compra (idorden_compra, idarticulo, cantidad_solicitada, precio_unitario_estimado)
                        VALUES (@idorden, @idarticulo, @cantidad, @precio);";
                    using var cmdDet = new MySqlCommand(insertDet, conn, tran);
                    cmdDet.Parameters.AddWithValue("@idorden", idOrden);// Asigna el ID de la orden creada
                    cmdDet.Parameters.AddWithValue("@idarticulo", det.IdArticulo);// Asigna el ID del artículo
                    cmdDet.Parameters.AddWithValue("@cantidad", det.CantidadSolicitada);// Asigna la cantidad solicitada
                    cmdDet.Parameters.AddWithValue("@precio", det.PrecioUnitarioEstimado);// Asigna el precio unitario estimado
                    cmdDet.ExecuteNonQuery();
                }

                tran.Commit();
                return idOrden;// Devuelve el ID de la orden creada
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }


        


        // Obtiene una orden de compra junto con sus detalles
        public OrdenCompra GetOrdenConDetalles(int idOrdenCompra)
        {
            OrdenCompra orden = null;// Inicializa la variable para almacenar la orden
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            // Consulta SQL para obtener la orden de compra
            var queryOrden = @"
            SELECT oc.idorden_compra, oc.fecha_emision, oc.total_estimado, oc.idproveedor, oc.estado, oc.tipo, p.razon_social
            FROM ordenes_compra oc
            JOIN proveedores p ON oc.idproveedor = p.idproveedor
            WHERE oc.idorden_compra = @id";

            using var cmdOrden = new MySqlCommand(queryOrden, conn);
            cmdOrden.Parameters.AddWithValue("@id", idOrdenCompra);// Asigna el ID de la orden como parámetro
            using var readerOrden = cmdOrden.ExecuteReader();// Ejecuta el comando y obtiene el lector de datos

            if (readerOrden.Read())// Lee la orden de compra
            {
                orden = new OrdenCompra// Crea el objeto OrdenCompra
                {
                    IdOrdenCompra = readerOrden.GetInt32("idorden_compra"),// Obtiene el ID de la orden
                    FechaCreacion = readerOrden.GetDateTime("fecha_emision"),// Obtiene la fecha de emisión
                    TotalEstimado = readerOrden.GetDecimal("total_estimado"),// Obtiene el total estimado
                    IdProveedor = readerOrden.GetInt32("idproveedor"),// Obtiene el ID del proveedor
                    Estado = readerOrden.GetString("estado"),// Obtiene el estado de la orden
                    Tipo = readerOrden.GetString("tipo"),// Obtiene el tipo de la orden
                    ProveedorNombre = readerOrden.GetString("razon_social") // Obtiene la razón social del proveedor
                };
            }
            readerOrden.Close();

            if (orden != null)// Si la orden fue encontrada, obtiene sus detalles
            {
                // Consulta SQL para obtener los detalles de la orden
                var queryDetalles = @"
            SELECT d.idarticulo, a.nombre, d.cantidad_solicitada, d.precio_unitario_estimado
            FROM detalles_orden_compra d
            JOIN articulos a ON d.idarticulo = a.idarticulo
            WHERE d.idorden_compra = @id";
                using var cmdDetalles = new MySqlCommand(queryDetalles, conn);// Crea el comando para obtener los detalles
                cmdDetalles.Parameters.AddWithValue("@id", idOrdenCompra);// Asigna el ID de la orden como parámetro
                using var readerDetalles = cmdDetalles.ExecuteReader();// Ejecuta el comando y obtiene el lector de datos

                while (readerDetalles.Read())// Lee cada detalle de la orden
                {
                    orden.Detalles.Add(new DetalleOrdenCompra// Crea el objeto DetalleOrdenCompra
                    {
                        IdArticulo = readerDetalles.GetInt32("idarticulo"),// Obtiene el ID del artículo
                        ArticuloNombre = readerDetalles.GetString("nombre"),// Obtiene el nombre del artículo
                        CantidadSolicitada = readerDetalles.GetInt32("cantidad_solicitada"),// Obtiene la cantidad solicitada
                        PrecioUnitarioEstimado = readerDetalles.GetDecimal("precio_unitario_estimado")// Obtiene el precio unitario estimado
                    });
                }
            }



            return orden;// Devuelve la orden de compra con sus detalles
        }
        // Lista todas las órdenes de compra que están pendientes
        public List<OrdenCompra> ListarOrdenesPendientes()
        {
            var lista = new List<OrdenCompra>();// Inicializa la lista para almacenar las órdenes pendientes

            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            // Consulta SQL para obtener las órdenes de compra pendientes
            string query = @"
                SELECT 
                    oc.idorden_compra, oc.fecha_emision, oc.total_estimado, oc.tipo, oc.estado,
                    p.idproveedor, p.razon_social
                FROM ordenes_compra oc
                JOIN proveedores p ON oc.idproveedor = p.idproveedor
                WHERE oc.estado = 'PENDIENTE' 
                ORDER BY oc.fecha_emision DESC";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())// Lee cada orden pendiente
            {
                lista.Add(new OrdenCompra// Crea el objeto OrdenCompra
                {
                    IdOrdenCompra = reader.GetInt32("idorden_compra"),// Obtiene el ID de la orden
                    FechaCreacion = reader.GetDateTime("fecha_emision"),// Obtiene la fecha de emisión
                    TotalEstimado = reader.GetDecimal("total_estimado"),// Obtiene el total estimado
                    Tipo = reader.GetString("tipo"),// Obtiene el tipo de la orden
                    Estado = reader.GetString("estado"),// Obtiene el estado de la orden
                    IdProveedor = reader.GetInt32("idproveedor"),// Obtiene el ID del proveedor
                    ProveedorNombre = reader.GetString("razon_social") 
                });
            }

            return lista;// Devuelve la lista de órdenes pendientes
        }
        // ====================================================================
        // SECCIÓN 2: GESTIÓN DE COMPRAS 
        // ====================================================================
        // Registra la recepción de una orden de compra, crea la Compra final y ACTUALIZA EL STOCK
        // Retorna el Id de la compra registrada
        public int RegistrarIngresoDesdeOrden(Compra compraFinal)
        {
            ValidarCompra(compraFinal);// Valida los datos de la compra
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            using var tran = conn.BeginTransaction();
            int idCompra = 0;// Variable para almacenar el ID de la compra creada

            try
            {
                // Inserta la compra final en la tabla compras
                var insertCompra = @"
                INSERT INTO compras (fecha, total, idproveedor, tipo, idorden_compra, metodo_pago)
                VALUES (@fecha, @total, @idproveedor, @tipo, @idorden, @metodoPago);";
                 using (var cmdCompra = new MySqlCommand(insertCompra, conn, tran))// Crea el comando para insertar la compra
                {
                    cmdCompra.Parameters.AddWithValue("@fecha", compraFinal.Fecha);// Asigna la fecha de la compra
                    cmdCompra.Parameters.AddWithValue("@total", compraFinal.Total);// Asigna el total de la compra
                    cmdCompra.Parameters.AddWithValue("@idproveedor", compraFinal.IdProveedor);// Asigna el ID del proveedor
                    cmdCompra.Parameters.AddWithValue("@tipo", compraFinal.Tipo);// Asigna el tipo de la compra
                    cmdCompra.Parameters.AddWithValue("@idorden", (object)compraFinal.IdOrdenCompra ?? DBNull.Value);// Asigna el ID de la orden de compra si existe
                    cmdCompra.Parameters.AddWithValue("@metodoPago", compraFinal.MetodoPago);// Asigna el método de pago
                    cmdCompra.ExecuteNonQuery();
                }

                idCompra = Convert.ToInt32(new MySqlCommand("SELECT LAST_INSERT_ID();", conn, tran).ExecuteScalar());// Obtiene el ID de la compra creada

                foreach (var det in compraFinal.Detalles)// Inserta cada detalle de la compra
                {
                    // Crea el comando para insertar el detalle de la compra
                    var insertDet = @"
                    INSERT INTO detalles_compras (cantidad, precio_unitario, idcompra, idarticulo)
                    VALUES (@cant, @precio, @idcompra, @idarticulo);";
                    using (var cmdDet = new MySqlCommand(insertDet, conn, tran))
                    {
                        cmdDet.Parameters.AddWithValue("@cant", det.Cantidad);// Asigna la cantidad del detalle
                        cmdDet.Parameters.AddWithValue("@precio", det.PrecioUnitario); // Asigna el precio unitario del detalle
                        cmdDet.Parameters.AddWithValue("@idcompra", idCompra);// Asigna el ID de la compra creada
                        cmdDet.Parameters.AddWithValue("@idarticulo", det.IdArticulo);// Asigna el ID del artículo del detalle
                        cmdDet.ExecuteNonQuery();
                    }
                    // Actualiza el stock del artículo según el tipo de compra
                    if (compraFinal.Tipo != null && compraFinal.Tipo.Trim().ToUpper() == "REPUESTO")
                    {
                        // Si es repuesto, actualiza el stock comprometido
                        string updateComprometido = @"
                        UPDATE articulos
                        SET stock = COALESCE(stock, 0) + @cant
                        WHERE idarticulo = @idarticulo;";
                        using (var cmd = new MySqlCommand(updateComprometido, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@cant", det.Cantidad);// Asigna la cantidad a sumar al stock
                            cmd.Parameters.AddWithValue("@idarticulo", det.IdArticulo);// Asigna el ID del artículo
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Si es stock, actualiza el stock normal
                        string updateStock = @"
                        UPDATE articulos 
                        SET stock = stock + @cant
                        WHERE idarticulo = @idarticulo;";
                        using (var cmd = new MySqlCommand(updateStock, conn, tran))// Crea el comando para actualizar el stock
                        {
                            cmd.Parameters.AddWithValue("@cant", det.Cantidad);// Asigna la cantidad a sumar al stock
                            cmd.Parameters.AddWithValue("@idarticulo", det.IdArticulo);// Asigna el ID del artículo
                            cmd.ExecuteNonQuery();
                        }
                    }
                    // Actualiza el precio de costo del proveedor para el artículo
                    string updateCostoProveedor = @"
                        UPDATE articulo_proveedor 
                        SET precioCoste = @precioFinal 
                        WHERE idarticulo = @idarticulo AND idproveedor = @idproveedor";
                    using (var cmdCosto = new MySqlCommand(updateCostoProveedor, conn, tran))
                    {
                        cmdCosto.Parameters.AddWithValue("@precioFinal", det.PrecioUnitario);// Asigna el nuevo precio de costo
                        cmdCosto.Parameters.AddWithValue("@idarticulo", det.IdArticulo);// Asigna el ID del artículo
                        cmdCosto.Parameters.AddWithValue("@idproveedor", compraFinal.IdProveedor);// Asigna el ID del proveedor
                        cmdCosto.ExecuteNonQuery();
                    }

                    _articuloService.ActualizarPrecioCostoArticulo(det.IdArticulo, conn, tran);// Actualiza el precio de costo del artículo

                } 

                if (compraFinal.IdOrdenCompra.HasValue)// Si la compra está asociada a una orden de compra, actualiza su estado a 'RECIBIDA'
                {
                    string updateOrden = "UPDATE ordenes_compra SET estado = 'RECIBIDA' WHERE idorden_compra = @id;";// Crea el comando para actualizar el estado de la orden
                    using var cmdUpdateOrden = new MySqlCommand(updateOrden, conn, tran);
                    cmdUpdateOrden.Parameters.AddWithValue("@id", compraFinal.IdOrdenCompra.Value);// Asigna el ID de la orden de compra
                    cmdUpdateOrden.ExecuteNonQuery();
                }
                
                tran.Commit();
                return idCompra;// Devuelve el ID de la compra registrada
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new Exception($"Error en RegistrarIngresoDesdeOrden: {ex.Message}", ex);
            }
        }
        // Valida los datos de una compra antes de su registro
        private void ValidarCompra(Compra compra)
        {
            if (compra.IdProveedor <= 0)
                throw new Exception("Debe seleccionar un proveedor.");

            
            if (compra.Total < 0)// Verifica que el total no sea negativo
                throw new Exception("El total de la compra no puede ser negativo.");

            if (compra.Fecha == default)// Verifica que la fecha sea válida
                throw new Exception("La fecha de la compra es obligatoria.");

            if (string.IsNullOrWhiteSpace(compra.Tipo))// Verifica que el tipo de compra sea válido
                throw new Exception("Debe especificar el tipo de compra.");

            if (compra.Total > 0 && (compra.Detalles == null || compra.Detalles.Count == 0))// Verifica que haya detalles si el total es mayor a cero
                throw new Exception("Debe agregar al menos un detalle a la compra si el total es mayor a cero.");

            // Validamos los detalles solo si existen
            if (compra.Detalles != null)// Verifica que los detalles no sean nulos
            {
                foreach (var d in compra.Detalles)// Valida cada detalle de la compra
                {
                    if (d.IdArticulo <= 0)// Verifica que el ID del artículo sea válido
                        throw new Exception("Cada detalle debe tener un artículo válido.");

                    if (d.Cantidad <= 0)// Verifica que la cantidad sea mayor a cero
                        throw new Exception("La cantidad debe ser mayor a cero.");

                    if (d.PrecioUnitario < 0)// Verifica que el precio unitario no sea negativo
                        throw new Exception("El precio unitario no puede ser negativo.");
                }
            }
        }
        // ====================================================================
        // SECCIÓN 3: HISTORIAL DE COMPRAS
        // ====================================================================
        // Lista el historial de compras con filtros opcionales
        public List<dynamic> ListarHistorialCompras(string tipo = "", DateTime? desde = null, DateTime? hasta = null)
        {
            var lista = new List<dynamic>();
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            // Construcción dinámica de la consulta SQL con filtros
            var query = new System.Text.StringBuilder(@"
                SELECT 
                    c.idcompra,
                    c.fecha,
                    c.total,
                    c.tipo,
                    p.razon_social AS Proveedor,
                    GROUP_CONCAT(CONCAT(a.nombre, ' (', dc.cantidad, ')') SEPARATOR ', ') AS ArticulosResumen
                FROM compras c
                JOIN proveedores p ON c.idproveedor = p.idproveedor
                JOIN detalles_compras dc ON c.idcompra = dc.idcompra
                JOIN articulos a ON dc.idarticulo = a.idarticulo
                WHERE 1=1 ");

            // Construcción dinámica de los filtros
            if (!string.IsNullOrEmpty(tipo))
            {
                query.Append(" AND TRIM(UPPER(c.tipo)) = @tipo");// Filtro por tipo de compra
            }
            if (desde.HasValue)// Filtro por fecha desde
            {
                query.Append(" AND c.fecha >= @desde");
            }
            if (hasta.HasValue)// Filtro por fecha hasta
            {
                // Asegura que la fecha final incluya todo el día
                query.Append(" AND c.fecha < @hastaFinDia");
            }
            // Agrupamiento y ordenamiento de los resultados
            query.Append(@" 
                GROUP BY c.idcompra, c.fecha, c.total, c.tipo, p.razon_social
                ORDER BY c.fecha DESC, c.idcompra DESC");

            using var cmd = new MySqlCommand(query.ToString(), conn);

            // Asignar los valores a los parámetros
            if (!string.IsNullOrEmpty(tipo))// Asigna el valor del tipo si se proporcionó
            {
                cmd.Parameters.AddWithValue("@tipo", tipo.Trim().ToUpper());// Normaliza el tipo a mayúsculas y sin espacios
            }
            if (desde.HasValue)// Asigna el valor de la fecha desde si se proporcionó
            {
                cmd.Parameters.AddWithValue("@desde", desde.Value.Date);
            }
            if (hasta.HasValue)// Asigna el valor de la fecha hasta si se proporcionó
            {
                // Pasamos el día siguiente 
                cmd.Parameters.AddWithValue("@hastaFinDia", hasta.Value.Date.AddDays(1));
            }

            using var reader = cmd.ExecuteReader();// Ejecuta el comando y obtiene el lector de datos

            while (reader.Read())
            {
                lista.Add(new// Crea un objeto dinámico para cada compra en el historial
                {
                    IdCompra = reader.GetInt32("idcompra"),// Obtiene el ID de la compra
                    Fecha = reader.GetDateTime("fecha"),// Obtiene la fecha de la compra
                    Total = reader.IsDBNull(reader.GetOrdinal("total")) ? 0.0 : reader.GetDouble("total"),// Obtiene el total de la compra
                    Tipo = reader.GetString("tipo"),// Obtiene el tipo de la compra
                    Proveedor = reader.IsDBNull(reader.GetOrdinal("Proveedor")) ? "N/A" : reader.GetString("Proveedor"),// Obtiene la razón social del proveedor
                    ArticulosResumen = reader.IsDBNull(reader.GetOrdinal("ArticulosResumen")) ? "Sin Artículos" : reader.GetString("ArticulosResumen")// Obtiene el resumen de artículos comprados
                });
            }
            return lista;
        }
        // ====================================================================
        // SECCIÓN 4: NECESIDADES DE REPUESTOS
        // ====================================================================

        public List<ArticuloFaltante> ObtenerNecesidadesRepuestosConsolidadas()// Obtiene una lista consolidada de artículos faltantes para repuestos
        {
            var resultado = new List<ArticuloFaltante>();

            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            // Consulta SQL para obtener los artículos faltantes
            string query = @"
                SELECT 
                    dp.idarticulo,
                    a.nombre AS nombre_articulo,
                    a.precioCosto AS precio_unitario_sugerido,
                    (SUM(dp.cantidad) - (COALESCE(a.stock, 0) + COALESCE(a.stock_comprometido, 0))) AS cantidad_faltante
    
                    FROM detalles_presupuestos dp
                    INNER JOIN articulos a ON dp.idarticulo = a.idarticulo
                    INNER JOIN presupuestos p ON dp.idpresupuesto = p.idpresupuesto
                    WHERE
   
                    p.estado IN ('ESPERA_REPUESTOS', 'EN_PROCESO') AND  
                    dp.idarticulo IS NOT NULL          
                    GROUP BY 
                      dp.idarticulo, a.nombre, a.precioCosto, a.stock, a.stock_comprometido -- Añadido stock_comprometido al GROUP BY
                    HAVING (SUM(dp.cantidad) - (COALESCE(a.stock, 0) + COALESCE(a.stock_comprometido, 0))) > 0
                    ORDER BY a.nombre;
                        ";

            using var cmd = new MySqlCommand(query, conn);// Crea el comando para ejecutar la consulta
            using var reader = cmd.ExecuteReader();// Ejecuta el comando y obtiene el lector de datos

            while (reader.Read())// Lee cada fila del resultado
            {
                resultado.Add(new ArticuloFaltante// Crea un objeto ArticuloFaltante
                {
                    IdArticulo = reader.GetInt32("idarticulo"),// Obtiene el ID del artículo
                    NombreArticulo = reader.GetString("nombre_articulo"),// Obtiene el nombre del artículo
                    CantidadFaltante = reader.GetInt32("cantidad_faltante"),// Obtiene la cantidad faltante
                    PrecioUnitarioSugerido = reader.GetDecimal("precio_unitario_sugerido"),// Obtiene el precio unitario sugerido
                    IdProveedor = 0,// No se obtiene el proveedor en esta consulta
                    ProveedorNombre = ""// No se obtiene el proveedor en esta consulta
                });
            }

            return resultado;// Devuelve la lista de artículos faltantes
        }
        // ====================================================================
        // SECCIÓN 5: OTROS MÉTODOS DE COMPRAS
        // ====================================================================
        // Actualiza el precio de costo de un artículo para un proveedor específico
        public void ActualizarPrecioCostoProveedor(int idArticulo, int idProveedor, decimal nuevoPrecioCosto)
        {
            

            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            using var tran = conn.BeginTransaction();

            try
            {
                // Actualiza el precio de costo en la tabla articulo_proveedor
                string updateCostoProveedor = @"
            UPDATE articulo_proveedor 
            SET precioCoste = @precioFinal 
            WHERE idarticulo = @idarticulo AND idproveedor = @idproveedor";
                using (var cmdCosto = new MySqlCommand(updateCostoProveedor, conn, tran))
                {
                    cmdCosto.Parameters.AddWithValue("@precioFinal", nuevoPrecioCosto);// Asigna el nuevo precio de costo
                    cmdCosto.Parameters.AddWithValue("@idarticulo", idArticulo);// Asigna el ID del artículo
                    cmdCosto.Parameters.AddWithValue("@idproveedor", idProveedor);// Asigna el ID del proveedor


                    if (cmdCosto.ExecuteNonQuery() == 0)// Si no se actualizó ninguna fila, significa que no existe la relación
                    {
                        tran.Rollback();
                        return;
                    }
                }
                // Actualiza el precio de costo del artículo en la tabla articulos
                _articuloService.ActualizarPrecioCostoArticulo(idArticulo, conn, tran);

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new Exception($"Error al actualizar precioCosto del proveedor: {ex.Message}", ex);
            }
        }
        

        // Cuenta la cantidad de presupuestos que están en estado 'ESPERA_REPUESTOS'
        public int ContarPresupuestosEnEsperaDeRepuestos()
        {
            int count = 0;// Variable para almacenar el conteo de presupuestos
            using (var conn = new MySqlConnection(CasaRepuestos.Config.Config.ConnectionString))
            {
                conn.Open();
                // Consulta SQL para contar los presupuestos en estado 'ESPERA_REPUESTOS'
                string sql = "SELECT COUNT(*) FROM presupuestos WHERE estado = 'ESPERA_REPUESTOS'";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    object result = cmd.ExecuteScalar();
                    count = (result == null || result == DBNull.Value) ? 0 : Convert.ToInt32(result);// Asigna el resultado del conteo a la variable count
                }
            }
            return count;
        }
      
    }


}

