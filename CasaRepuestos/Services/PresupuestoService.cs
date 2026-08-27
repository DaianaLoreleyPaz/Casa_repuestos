using CasaRepuestos.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace CasaRepuestos.Services
{
    public class PresupuestoService
    {
        private readonly string _connectionString;

        public PresupuestoService()
        {
            _connectionString = CasaRepuestos.Config.Config.ConnectionString;
        }

        #region Selects básicos (Ingresos, Servicios, Artículos, Presupuestos, Detalles)
        // Obtener ingresos sin presupuesto asociado
        public List<Ingreso> GetIngresos()
        {
            var list = new List<Ingreso>();// Lista para almacenar los ingresos
            using (var conn = new MySqlConnection(_connectionString))// Crear una conexión a la base de datos
            {
                conn.Open();
                string sql = @"
                    SELECT i.idingreso, i.fecha_ingreso, i.modelo, i.falla, i.tipo_dispositivo,
                           IFNULL(m.nombre, '') AS Marca
                    FROM ingresos i
                    LEFT JOIN marcas m ON i.idmarca = m.idmarca
                    LEFT JOIN presupuestos p ON i.idingreso = p.idingreso
                    WHERE p.idingreso IS NULL
                    ORDER BY i.fecha_ingreso DESC";
                using (var cmd = new MySqlCommand(sql, conn))// Crear un comando SQL
                using (var rdr = cmd.ExecuteReader())// Ejecutar el comando y obtener un lector de datos
                {
                    while (rdr.Read())// Leer cada fila del resultado
                    {
                        list.Add(new Ingreso// Crear un nuevo objeto Ingreso y agregarlo a la lista
                        {
                            IdIngreso = rdr.GetInt32("idingreso"),// Obtener el ID del ingreso
                            FechaIngreso = rdr.GetDateTime("fecha_ingreso"),// Obtener la fecha de ingreso
                            Modelo = rdr.IsDBNull(rdr.GetOrdinal("modelo")) ? "" : rdr.GetString("modelo"),// Obtener el modelo (verificar si es nulo)
                            Falla = rdr.IsDBNull(rdr.GetOrdinal("falla")) ? "" : rdr.GetString("falla"),// Obtener la falla (verificar si es nulo)
                            TipoDispositivo = Enum.TryParse<TipoDispositivo>(rdr["tipo_dispositivo"].ToString(), out var tipo) ? tipo : TipoDispositivo.SMARTPHONE,
                            Marca = rdr.IsDBNull(rdr.GetOrdinal("Marca")) ? "" : rdr.GetString("Marca")// Obtener la marca (verificar si es nulo)
                        });
                    }
                }
            }
            return list;// Devolver la lista de ingresos
        }
        // Obtener todos los servicios sin artículo asociado
        public List<Servicio> GetServicios()
        {
            var lista = new List<Servicio>();// Lista para almacenar los servicios
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                // Consulta SQL para obtener servicios sin artículo asociado
                string sql = @"
            SELECT idservicio, descripcion_servicio, precio 
            FROM servicios 
            WHERE idarticulo IS NULL 
            ORDER BY descripcion_servicio";

                using (var cmd = new MySqlCommand(sql, conn))// Crear un comando SQL
                using (var rdr = cmd.ExecuteReader())// Ejecutar el comando y obtener un lector de datos
                {
                    while (rdr.Read())// Leer cada fila del resultado
                    {
                        lista.Add(new Servicio// Crear un nuevo objeto Servicio y agregarlo a la lista
                        {
                            IdServicio = rdr.GetInt32("idservicio"),// Obtener el ID del servicio
                            Descripcion = rdr.GetString("descripcion_servicio"),// Obtener la descripción del servicio
                            Precio = rdr.GetDecimal("precio")// Obtener el precio del servicio
                        });
                    }
                }
            }
            return lista;
        }
        // Obtener todos los artículos que son servicios (tienen entrada en la tabla servicios)
        public List<Articulo> GetArticulos()
        {
            var lista = new List<Articulo>();// Lista para almacenar los artículos
            using (var conn = new MySqlConnection(_connectionString))// Crear una conexión a la base de datos
            {
                conn.Open();// Abrir la conexión
                // Consulta SQL para obtener artículos que son servicios
                string sql = @"
                    SELECT a.idarticulo, a.nombre, a.precio, stock, 
                           precioCosto, porcentajeGanancia, IVA 
                    FROM servicios
                    inner join articulos a on a.idarticulo=servicios.idarticulo 
                    WHERE idservicio IS NOT NULL
                    ORDER BY nombre";

                using (var cmd = new MySqlCommand(sql, conn))// Crear un comando SQL
                using (var rdr = cmd.ExecuteReader())// Ejecutar el comando y obtener un lector de datos
                {
                    while (rdr.Read())// Leer cada fila del resultado
                    {
                        lista.Add(new Articulo// Crear un nuevo objeto Artículo y agregarlo a la lista
                        {
                            IdArticulo = rdr.GetInt32("idarticulo"),// Obtener el ID del artículo
                            Nombre = rdr.GetString("nombre"),// Obtener el nombre del artículo
                            Precio = rdr.GetDecimal("precio"),// Obtener el precio del artículo
                            Stock = rdr.GetInt32("stock"),// Obtener el stock del artículo
                            PrecioCosto = rdr.GetDecimal("precioCosto"),// Obtener el precio de costo del artículo
                            PorcentajeGanancia = rdr.GetDecimal("porcentajeGanancia"),// Obtener el porcentaje de ganancia del artículo
                            IVA = rdr.GetDecimal("IVA")//   Obtener el IVA del artículo
                        });
                    }
                }
            }
            return lista;
        }
        // Obtener todos los presupuestos con información del cliente
        public List<Presupuesto> GetPresupuestos()
        {
            var list = new List<Presupuesto>();// Lista para almacenar los presupuestos
            using (var conn = new MySqlConnection(_connectionString))// Crear una conexión a la base de datos
            {
                conn.Open();
                // Consulta SQL para obtener presupuestos con información del cliente
                string sql = @"
            SELECT 
            p.idpresupuesto,
            CONCAT(per.nombre, ' ', per.apellido) AS cliente,
            p.fecha,
            p.total,
            p.autorizado,
            p.estado,
            p.fechaVencimiento,
            p.fechaRetiro,
            p.idempleado,
            p.idingreso
        FROM presupuestos p
        INNER JOIN ingresos i ON p.idingreso = i.idingreso
        INNER JOIN clientes cl ON i.idcliente = cl.idcliente
        INNER JOIN personas per ON cl.idpersona = per.idpersona
        ORDER BY p.fecha DESC";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var rdr = cmd.ExecuteReader())// Ejecutar el comando y obtener un lector de datos
                {
                    while (rdr.Read())// Leer cada fila del resultado
                    {
                        list.Add(new Presupuesto// Crear un nuevo objeto Presupuesto y agregarlo a la lista
                        {
                            IdPresupuesto = rdr.GetInt32("idpresupuesto"),// Obtener el ID del presupuesto
                            Fecha = rdr.GetDateTime("fecha"),// Obtener la fecha del presupuesto
                            Total = rdr.GetDecimal("total"),// Obtener el total del presupuesto
                            Autorizado = rdr.GetString("autorizado"),// Obtener si el presupuesto está autorizado
                            Estado = rdr.GetString("estado"),// Obtener el estado del presupuesto
                            IdEmpleado = rdr.IsDBNull(rdr.GetOrdinal("idempleado")) ? 0 : rdr.GetInt32("idempleado"),// Obtener el ID del empleado (verificar si es nulo)
                            IdIngreso = rdr.IsDBNull(rdr.GetOrdinal("idingreso")) ? 0 : rdr.GetInt32("idingreso"),// Obtener el ID del ingreso (verificar si es nulo)
                            FechaVencimiento = rdr.IsDBNull(rdr.GetOrdinal("fechaVencimiento"))
                             ? (DateTime?)null// Obtener la fecha de vencimiento (verificar si es nulo)
                             : rdr.GetDateTime("fechaVencimiento"),
                            FechaRetiro = rdr.IsDBNull(rdr.GetOrdinal("fechaRetiro"))
                            ? (DateTime?)null
                            : rdr.GetDateTime("fechaRetiro")
                        });
                    }
                }
            }
            return list;
        }
        // Obtener los detalles de un presupuesto específico, incluyendo precios maestros
        public List<DetallePresupuesto> GetDetallesPresupuesto(int idPresupuesto)
        {
            var detalles = new List<DetallePresupuesto>();// Lista para almacenar los detalles del presupuesto
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                // Consulta SQL para obtener los detalles del presupuesto con precios maestros
                string sql = @"
            SELECT 
                dp.iddetalle_presupuesto, dp.idpresupuesto, dp.idarticulo, dp.cantidad, 
                dp.precio_repuesto, dp.precio_servicio, dp.idservicio,
                COALESCE(a.precioCosto, 0) AS precioCosto,
                COALESCE(a.porcentajeGanancia, 0) AS porcentajeGanancia,
                COALESCE(a.IVA, 0) AS IVA
                
            FROM detalles_presupuestos dp
            LEFT JOIN articulos a ON dp.idarticulo = a.idarticulo
            WHERE dp.idpresupuesto = @idPresupuesto";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);// Agregar el parámetro del ID del presupuesto
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            // Crear un nuevo objeto DetallePresupuesto y agregarlo a la lista
                            detalles.Add(new DetallePresupuesto
                            {
                                
                                IdDetallePresupuesto = rdr.GetInt32("iddetalle_presupuesto"),// Obtener el ID del detalle del presupuesto
                                IdPresupuesto = rdr.GetInt32("idpresupuesto"),// Obtener el ID del presupuesto
                                IdArticulo = rdr.IsDBNull(rdr.GetOrdinal("idarticulo")) ? (int?)null : rdr.GetInt32("idarticulo"),
                                Cantidad = rdr.GetInt32("cantidad"),// Obtener la cantidad del detalle
                                PrecioRepuesto = rdr.IsDBNull(rdr.GetOrdinal("precio_repuesto")) ? (decimal?)null : rdr.GetDecimal("precio_repuesto"),
                                PrecioServicio = rdr.IsDBNull(rdr.GetOrdinal("precio_servicio")) ? (decimal?)null : rdr.GetDecimal("precio_servicio"),
                                IdServicio = rdr.IsDBNull(rdr.GetOrdinal("idservicio")) ? 0 : rdr.GetInt32("idservicio"),

                                PrecioCosto = rdr.GetDecimal("precioCosto"),// Obtener el precio de costo
                                PorcentajeGanancia = rdr.GetDecimal("porcentajeGanancia"),// Obtener el porcentaje de ganancia
                                IVA = rdr.GetDecimal("IVA")// Obtener el IVA
                            });
                        }
                    }
                }
            }
            return detalles;
        }

        #endregion

        #region Create / Update (transaccional) y UpdateAutorizado
        // Crear un nuevo presupuesto con sus detalles
        public int CreatePresupuesto(Presupuesto p, List<DetallePresupuesto> detalles)
        {
            if (detalles == null || !detalles.Any())// Validar que haya al menos un detalle
            {
                throw new Exception("El presupuesto debe tener al menos un detalle.");
            }

            if (p.Total < 0)// Validar que el total no sea negativo
            {
                throw new Exception("El total no puede ser negativo.");
            }

            if (p.IdEmpleado <= 0)// Validar que el empleado sea válido
            {
                throw new Exception("El empleado es obligatorio.");
            }

            if (p.IdIngreso <= 0)// Validar que el ingreso sea válido
            {
                throw new Exception("El ingreso es obligatorio.");
            }
            int newId = 0;// Variable para almacenar el ID del nuevo presupuesto
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // Insertar el presupuesto
                        string sqlInsertPresupuesto = @"
                    INSERT INTO presupuestos (fecha, total, autorizado, estado, idempleado, idingreso, fechaVencimiento, fechaRetiro)
                    VALUES (@fecha, @total, @autorizado, @estado, @idempleado, @idingreso, @fechaVencimiento, @fechaRetiro)";
                        using (var cmd = new MySqlCommand(sqlInsertPresupuesto, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@fecha", p.Fecha);
                            cmd.Parameters.AddWithValue("@total", p.Total);
                            cmd.Parameters.AddWithValue("@autorizado", string.IsNullOrEmpty(p.Autorizado) ? "NO" : p.Autorizado);
                            cmd.Parameters.AddWithValue("@estado", string.IsNullOrEmpty(p.Estado) ? "VERIFICAR_PRECIO" : p.Estado);
                            cmd.Parameters.AddWithValue("@idempleado", p.IdEmpleado);
                            cmd.Parameters.AddWithValue("@idingreso", p.IdIngreso);
                            cmd.Parameters.AddWithValue("@fechaVencimiento", (object?)p.FechaVencimiento ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@fechaRetiro", (object?)p.FechaRetiro ?? DBNull.Value);
                            cmd.ExecuteNonQuery();
                            newId = (int)cmd.LastInsertedId;
                        }
                        // Insertar los detalles del presupuesto
                        string sqlInsertDetalle = @"
                        INSERT INTO detalles_presupuestos (idpresupuesto, idarticulo, cantidad, precio_repuesto, precio_servicio, idservicio)
                           VALUES (@idpresupuesto, @idarticulo, @cantidad, @precio_repuesto, @precio_servicio, @idservicio)";

                        foreach (var d in detalles)// Recorrer cada detalle
                        {
                            if (d == null) continue;
                            using (var cmd = new MySqlCommand(sqlInsertDetalle, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@idpresupuesto", newId);// Usar el ID del nuevo presupuesto
                                cmd.Parameters.AddWithValue("@idarticulo", (object?)d.IdArticulo ?? DBNull.Value);// Manejar nulos para idarticulo
                                cmd.Parameters.AddWithValue("@cantidad", d.Cantidad);// Cantidad del detalle
                                cmd.Parameters.AddWithValue("@precio_repuesto", (object?)d.PrecioRepuesto ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@precio_servicio", (object?)d.PrecioServicio ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@idservicio", d.IdServicio);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
            return newId;
        }
        // Actualizar un presupuesto existente y sus detalles
        public void UpdatePresupuesto(Presupuesto presupuesto, List<DetallePresupuesto> detalles)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // Actualizar el presupuesto
                        string sqlUpdatePresupuesto = @"UPDATE presupuestos SET fecha=@fecha, total=@total, 
                    autorizado=@autorizado, estado=@estado, idempleado=@idempleado, idingreso=@idingreso,
                    fechaVencimiento=@fechaVencimiento, fechaRetiro=@fechaRetiro
                    WHERE idpresupuesto=@idpresupuesto";
                        using (var cmd = new MySqlCommand(sqlUpdatePresupuesto, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@fecha", presupuesto.Fecha);
                            cmd.Parameters.AddWithValue("@total", presupuesto.Total);
                            cmd.Parameters.AddWithValue("@autorizado", string.IsNullOrEmpty(presupuesto.Autorizado) ? "NO" : presupuesto.Autorizado);
                            cmd.Parameters.AddWithValue("@estado", string.IsNullOrEmpty(presupuesto.Estado) ? "VERIFICAR_PRECIO" : presupuesto.Estado);
                            cmd.Parameters.AddWithValue("@idempleado", presupuesto.IdEmpleado);
                            cmd.Parameters.AddWithValue("@idingreso", presupuesto.IdIngreso);
                            cmd.Parameters.AddWithValue("@fechaVencimiento", (object?)presupuesto.FechaVencimiento ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@fechaRetiro", (object?)presupuesto.FechaRetiro ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idpresupuesto", presupuesto.IdPresupuesto);
                            cmd.ExecuteNonQuery();
                        }
                        // Eliminar los detalles existentes
                        string sqlDeleteDetalles = "DELETE FROM detalles_presupuestos WHERE idpresupuesto=@idpresupuesto";
                        using (var cmd = new MySqlCommand(sqlDeleteDetalles, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@idpresupuesto", presupuesto.IdPresupuesto);
                            cmd.ExecuteNonQuery();
                        }
                        // Insertar los nuevos detalles
                        string sqlInsertDetalle = @"
                         INSERT INTO detalles_presupuestos (idpresupuesto, idarticulo, cantidad, precio_repuesto, precio_servicio, idservicio)
                           VALUES (@idpresupuesto, @idarticulo, @cantidad, @precio_repuesto, @precio_servicio, @idservicio)";
                        // Recorrer cada detalle
                        foreach (var d in detalles)
                        {
                            // Saltar detalles nulos
                            if (d == null) continue;
                            using (var cmd = new MySqlCommand(sqlInsertDetalle, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@idpresupuesto", presupuesto.IdPresupuesto);
                                cmd.Parameters.AddWithValue("@idarticulo", (object?)d.IdArticulo ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@cantidad", d.Cantidad);
                                cmd.Parameters.AddWithValue("@precio_repuesto", (object?)d.PrecioRepuesto ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@precio_servicio", (object?)d.PrecioServicio ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@idservicio", d.IdServicio);
                                cmd.ExecuteNonQuery();
                            }
                        }



                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }
        // Actualizar el campo "autorizado" de un presupuesto
        public void UpdateAutorizado(int idPresupuesto, string autorizado)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                // Consulta SQL para actualizar el campo "autorizado"
                string sql = "UPDATE presupuestos SET autorizado=@autorizado WHERE idpresupuesto=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@autorizado", autorizado);
                    cmd.Parameters.AddWithValue("@id", idPresupuesto);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Métodos de Estado (limpios y explícitos)
        public void MarcarParaVerificarPrecio(int idPresupuesto)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE presupuestos SET estado = 'VERIFICAR_PRECIO' WHERE idpresupuesto = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idPresupuesto);
                    cmd.ExecuteNonQuery();
                }
            }
        }

  
        public void MarcarAprobadoAdmin(int idPresupuesto)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE presupuestos SET estado = 'APROBADO_ADMIN' WHERE idpresupuesto = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idPresupuesto);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Presupuesto GetPresupuestoEstado(int idPresupuesto, MySqlConnection conn)
        {
      
            string sql = "SELECT fecha, estado FROM presupuestos WHERE idpresupuesto=@id";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", idPresupuesto);
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        return new Presupuesto
                        {
                            IdPresupuesto = idPresupuesto,
                 
                            Fecha = rdr.IsDBNull(0) ? (DateTime?)null : rdr.GetDateTime(0),
                            Estado = rdr.IsDBNull(1) ? "VERIFICAR_PRECIO" : rdr.GetString(1),
                        };
                    }
                }
            }
            return null;
        }

        public bool AplicarPreciosMaestros(int idPresupuesto)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                Presupuesto pres = GetPresupuestoEstado(idPresupuesto, conn);
                if (pres == null) return false;

                string estado = pres.Estado?.ToUpper() ?? "VERIFICAR_PRECIO";

                // Solo actualizamos precios si está en estos estados
                bool debeActualizar = estado == "VERIFICAR_PRECIO" || estado == "APROBADO_ADMIN";

                if (!debeActualizar)
                {
                    return false;// Aquí se detiene si no es uno de esos dos estados
                }

                bool preciosCambiaron = false;

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string sqlArticulos = @"
                    UPDATE detalles_presupuestos dp
                    INNER JOIN articulos a ON dp.idarticulo = a.idarticulo
                    SET 
                        dp.precio_repuesto = (a.precioCosto * (1 + (a.porcentajeGanancia / 100)) * (1 + (a.IVA / 100)))
                    WHERE 
                        dp.idpresupuesto = @id AND dp.idarticulo IS NOT NULL 
                        
                        AND dp.precio_repuesto != (a.precioCosto * (1 + (a.porcentajeGanancia / 100)) * (1 + (a.IVA / 100)));";

                        using (var cmdArt = new MySqlCommand(sqlArticulos, conn, trans))
                        {
                            cmdArt.Parameters.AddWithValue("@id", idPresupuesto);
                            if (cmdArt.ExecuteNonQuery() > 0)
                            {
                                preciosCambiaron = true;
                            }
                        }

                        // Lógica de actualización de SERVICIOS 
                        string sqlServicios = @"
                    UPDATE detalles_presupuestos dp
                    INNER JOIN servicios s ON dp.idservicio = s.idservicio
                    SET dp.precio_servicio = s.precio
                    WHERE dp.idpresupuesto = @id AND dp.idservicio IS NOT NULL 
                      AND dp.idarticulo IS NULL -- 💡 Solo para servicios puros
                      AND dp.precio_servicio != s.precio;";

                        using (var cmdServ = new MySqlCommand(sqlServicios, conn, trans))
                        {
                            cmdServ.Parameters.AddWithValue("@id", idPresupuesto);
                            if (cmdServ.ExecuteNonQuery() > 0)
                            {
                                preciosCambiaron = true;
                            }
                        }

                        if (preciosCambiaron)
                        {
                            // Recalcular el Total del Presupuesto 
                            string sqlRecalcularTotal = @"
                        UPDATE presupuestos p
                        INNER JOIN (
                            SELECT idpresupuesto, 
                                   SUM((IFNULL(precio_repuesto, 0) + IFNULL(precio_servicio, 0)) * cantidad) AS nuevo_total
                            FROM detalles_presupuestos
                            WHERE idpresupuesto = @id
                            GROUP BY idpresupuesto
                        ) AS dt ON p.idpresupuesto = dt.idpresupuesto
                        SET p.total = dt.nuevo_total
                        WHERE p.idpresupuesto = @id;";

                            using (var cmdTotal = new MySqlCommand(sqlRecalcularTotal, conn, trans))
                            {
                                cmdTotal.Parameters.AddWithValue("@id", idPresupuesto);
                                cmdTotal.ExecuteNonQuery();
                            }

                            trans.Commit();
                        }
                        else
                        {
                            trans.Rollback();
                        }

                        return preciosCambiaron;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw new Exception("Error al aplicar precios maestros al presupuesto: " + ex.Message);
                    }
                }
            }
        }
        //aca se acrualizan los precios tambien 
        public int ActualizarPreciosEnPresupuestosFlexibles(int? idArticulo, int idServicio, decimal nuevoPrecio)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                string estadosFlexibles = "'VERIFICAR_PRECIO', 'APROBADO_ADMIN'";// solo estos estados
                int filasActualizadas = 0;

            //aca se protegen los precios que fueron enviados al cliente
                string filtroBloqueo = @"
            AND NOT (
                p.estado = 'ENVIADO_A_CLIENTE' 
                AND p.fecha >= DATE_SUB(CURDATE(), INTERVAL 7 DAY)
            )";

                if (idArticulo.HasValue && idArticulo.Value > 0)
                {
                    //aca cambia el precio viejo por los nuevos que llegan por parametros. 
                    string sql = $@"
                UPDATE detalles_presupuestos dp
                INNER JOIN presupuestos p ON dp.idpresupuesto = p.idpresupuesto
                SET dp.precio_repuesto = @nuevoPrecio
                WHERE dp.idarticulo = @idItem
                AND p.estado IN ({estadosFlexibles})
                {filtroBloqueo};";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nuevoPrecio", nuevoPrecio);
                        cmd.Parameters.AddWithValue("@idItem", idArticulo.Value);
                        filasActualizadas = cmd.ExecuteNonQuery();
                    }
                }
                else if (idServicio > 0)
                {
                    //cambia el precio de los servicios tambien 
                    string sql = $@"
                UPDATE detalles_presupuestos dp
                INNER JOIN presupuestos p ON dp.idpresupuesto = p.idpresupuesto
                SET dp.precio_servicio = @nuevoPrecio
                WHERE dp.idservicio = @idItem
                AND p.estado IN ({estadosFlexibles})
                {filtroBloqueo};";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nuevoPrecio", nuevoPrecio);
                        cmd.Parameters.AddWithValue("@idItem", idServicio);
                        filasActualizadas = cmd.ExecuteNonQuery();
                    }
                }

                if (filasActualizadas > 0)
                {
                    
                    string sqlTotales = @"
                UPDATE presupuestos p
                INNER JOIN (
                    SELECT idpresupuesto, 
                           SUM((IFNULL(precio_repuesto, 0) + IFNULL(precio_servicio, 0)) * cantidad) AS nuevo_total
                    FROM detalles_presupuestos
                    GROUP BY idpresupuesto
                ) d ON p.idpresupuesto = d.idpresupuesto
                SET p.total = d.nuevo_total
                WHERE p.estado IN (" + estadosFlexibles + @");";

                    using (var cmd = new MySqlCommand(sqlTotales, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                return filasActualizadas;
            }
        }

        public void ActualizarDetallesPresupuesto(List<DetallePresupuesto> detalles)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
            UPDATE detalles_presupuestos
            SET precio_repuesto = @precioRepuesto, 
                precio_servicio = @precioServicio
            WHERE iddetalle_presupuesto = @idDetalle";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    foreach (var detalle in detalles)
                    {
                    
                        if (detalle.IdDetallePresupuesto > 0)
                        {
                            cmd.Parameters.Clear();
                           
                            cmd.Parameters.AddWithValue("@precioRepuesto", detalle.PrecioRepuesto ?? 0m);
                            cmd.Parameters.AddWithValue("@precioServicio", detalle.PrecioServicio ?? 0m);
                            cmd.Parameters.AddWithValue("@idDetalle", detalle.IdDetallePresupuesto);

                        
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        public void MarcarEnviadoACliente(int idPresupuesto)
        {
            var fechaEnvio = DateTime.Now.Date;
            var fechaVencimiento = fechaEnvio.AddDays(7).Date;

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    UPDATE presupuestos
                    SET estado = 'ENVIADO_A_CLIENTE',
                        fecha = @fechaEnvio,
                        fechaVencimiento = @fechaVencimiento,
                        autorizado = 'NO'
                    WHERE idpresupuesto = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idPresupuesto);
                    cmd.Parameters.AddWithValue("@fechaEnvio", fechaEnvio);
                    cmd.Parameters.AddWithValue("@fechaVencimiento", fechaVencimiento);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void MarcarFinalizado(int idPresupuesto)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE presupuestos SET estado = 'FINALIZADO' WHERE idpresupuesto = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idPresupuesto);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Lógica automática (ciclo de 7 días y verificación)
        // Método para actualizar automáticamente el estado de los presupuestos vencidos
        public void ActualizarEstadoAutomatico()
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                // Marcar presupuestos vencidos como "VERIFICAR_PRECIO"
                string sqlVolverAVerificar = @"
                    UPDATE presupuestos
                    SET estado = 'VERIFICAR_PRECIO'
                    WHERE estado = 'ENVIADO_A_CLIENTE'
                      AND fechaVencimiento IS NOT NULL
                      AND fechaVencimiento < CURDATE()
                      AND autorizado = 'NO'";
                using (var cmd = new MySqlCommand(sqlVolverAVerificar, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion  

        #region Utilidades
        // Verificar si un empleado existe en la base de datos
        //public bool EmpleadoExiste(int idEmpleado)
        //{
        //    if (idEmpleado <= 0) return false;// Validar ID inválido
        //    using (var cn = new MySqlConnection(_connectionString))// Crear conexión a la base de datos
        //    {
        //        cn.Open();
        //        // Consulta SQL para contar empleados con el ID dado
        //        using (var cmd = new MySqlCommand("SELECT COUNT(1) FROM empleados WHERE idempleado = @id", cn))
        //        {
        //            cmd.Parameters.AddWithValue("@id", idEmpleado);
        //            var count = Convert.ToInt32(cmd.ExecuteScalar());
        //            return count > 0;
        //        }
        //    }
        //}

        //public DataTable ObtenerPresupuestosAutorizados()
        //{
        //    var dt = new DataTable();
        //    using (var conn = new MySqlConnection(_connectionString))
        //    {
        //        conn.Open();
        //        string sql = @"
        //    SELECT 
        //        p.idpresupuesto,
        //        CONCAT(per.nombre, ' ', per.apellido) AS cliente,
        //        p.fecha,
        //        p.estado
        //    FROM presupuestos p
        //    INNER JOIN ingresos i ON p.idingreso = i.idingreso
        //    INNER JOIN clientes cl ON i.idcliente = cl.idcliente
        //    INNER JOIN personas per ON cl.idpersona = per.idpersona
        //    WHERE p.autorizado = 'SI' AND p.estado <> 'FINALIZADO'
        //    ORDER BY p.fecha DESC;";
        //        using (var da = new MySqlDataAdapter(sql, conn))
        //        {
        //            da.Fill(dt);
        //        }
        //    }
        //    return dt;
        //}
        // Obtener el ingreso asociado a un presupuesto
        public Ingreso GetIngresoPorPresupuesto(int idPresupuesto)
        {
            Ingreso ingreso = null;// Variable para almacenar el ingreso
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                // Consulta SQL para obtener el ingreso asociado al presupuesto
                string query = @"
            SELECT 
                i.IdIngreso, 
                i.fecha_ingreso AS FechaIngreso, 
                i.falla, 
                i.modelo AS Modelo,
                m.nombre AS Marca,
                i.tipo_dispositivo AS Tipo
            FROM presupuestos p
            INNER JOIN ingresos i ON p.idingreso = i.idingreso
            LEFT JOIN marcas m ON i.idmarca = m.idmarca
            WHERE p.idpresupuesto = @idPresupuesto;
        ";

                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ingreso = new Ingreso
                            {
                                IdIngreso = reader.GetInt32("IdIngreso"),
                                FechaIngreso = reader.GetDateTime("FechaIngreso"),
                                Falla = reader["falla"].ToString(),
                                Marca = reader["Marca"] != DBNull.Value ? reader["Marca"].ToString() : "",
                                Modelo = reader["Modelo"] != DBNull.Value ? reader["Modelo"].ToString() : "",
                                TipoDispositivo = Enum.TryParse(reader["Tipo"]?.ToString(), true, out TipoDispositivo tipo)
                                    ? tipo
                                    : TipoDispositivo.SMARTPHONE
                            };
                        }
                    }
                }
            }
            return ingreso;
        }
        // Marcar un presupuesto como autorizado y pendiente
        public void MarcarAutorizadoYPendiente(int idPresupuesto, string autorizado)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                // Consulta SQL para actualizar el presupuesto
                string sql = "UPDATE presupuestos SET autorizado=@autorizado, estado='PENDIENTE' WHERE idpresupuesto=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@autorizado", autorizado);
                    cmd.Parameters.AddWithValue("@id", idPresupuesto);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // Método para actualizar el total del presupuesto
        public void ActualizarTotalPresupuesto(int idPresupuesto, decimal nuevoTotal)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                // Consulta SQL para actualizar el total del presupuesto
                string sql = "UPDATE presupuestos SET total=@total WHERE idpresupuesto=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@total", nuevoTotal);
                    cmd.Parameters.AddWithValue("@id", idPresupuesto);
                    cmd.ExecuteNonQuery();
                }
            }
        
        }
        //public void ActualizarPrecioArticulo(int idArticulo, decimal nuevoPrecio)
        //{
        //    using (var conn = new MySqlConnection(_connectionString))
        //    {
        //        conn.Open();
        //        string sql = "UPDATE articulos SET precio=@precio WHERE idarticulo=@id";
        //        using (var cmd = new MySqlCommand(sql, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@precio", nuevoPrecio);
        //            cmd.Parameters.AddWithValue("@id", idArticulo);
        //            cmd.ExecuteNonQuery(); 
        //        }
        //    }
        //}

        // Método para actualizar el precio del servicio maestro
        public void ActualizarPrecioServicio(int idServicio, decimal nuevoPrecio)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                // Consulta SQL para actualizar el precio del servicio
                string sql = "UPDATE servicios SET precio=@precio WHERE idservicio=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@precio", nuevoPrecio);
                    cmd.Parameters.AddWithValue("@id", idServicio);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // Obtener un presupuesto por su ID
        public Presupuesto GetPresupuestoById(int idPresupuesto)
        {
            Presupuesto presupuesto = null;
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
            SELECT 
                p.idpresupuesto, p.fecha, p.total, p.autorizado, p.estado, 
                p.idempleado, p.idingreso, p.fechaVencimiento, 
                CONCAT(per.nombre, ' ', per.apellido) AS ClienteNombre 
            FROM presupuestos p
            INNER JOIN ingresos i ON p.idingreso = i.idingreso
            INNER JOIN clientes cl ON i.idcliente = cl.idcliente
            INNER JOIN personas per ON cl.idpersona = per.idpersona
            WHERE p.idpresupuesto = @id";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idPresupuesto);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            presupuesto = new Presupuesto
                            {
                                IdPresupuesto = reader.GetInt32("idpresupuesto"),// ID del presupuesto
                                Fecha = reader.GetDateTime("fecha"),// Fecha del presupuesto
                                Total = reader.GetDecimal("total"),// Total del presupuesto
                                Autorizado = reader["autorizado"].ToString(),// Estado de autorización
                                Estado = reader["estado"].ToString(),
                                IdEmpleado = reader.GetInt32("idempleado"),
                                IdIngreso = reader.GetInt32("idingreso"),

                                ClienteNombre = reader.GetString("ClienteNombre"),
                                FechaVencimiento = reader.IsDBNull(reader.GetOrdinal("fechaVencimiento"))
                                    ? (DateTime?)null
                                    : reader.GetDateTime("fechaVencimiento"),
                            };
                        }
                    }
                }
            }
            return presupuesto;
        }
        // Obtener el nombre de un artículo por su ID
        public string GetNombreArticulo(int idArticulo)
        {
            if (idArticulo <= 0) return "N/A";// Validar ID inválido

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                // Consulta SQL para obtener el nombre del artículo
                string sql = "SELECT nombre FROM articulos WHERE idarticulo = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idArticulo);
                    var result = cmd.ExecuteScalar();
                    return result?.ToString() ?? "Artículo Desconocido";
                }
            }
        }
        // Obtener la descripción de un servicio por su ID
        public string GetDescripcionServicio(int idServicio)
        {
            if (idServicio <= 0) return "N/A";// Validar ID inválido

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                // Consulta SQL para obtener la descripción del servicio
                string sql = "SELECT descripcion_servicio FROM servicios WHERE idservicio = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idServicio);
                    var result = cmd.ExecuteScalar();
                    return result?.ToString() ?? "Servicio Desconocido";
                }
            }
        }
        // Contar presupuestos en estado "VERIFICAR_PRECIO"
        public int ContarPresupuestosPorVerificarPrecio()
        {
            int count = 0;// Variable para almacenar el conteo
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            {
                conn.Open();
                // Consulta SQL para contar presupuestos en estado "VERIFICAR_PRECIO"
                string sql = "SELECT COUNT(*) FROM presupuestos WHERE estado = 'VERIFICAR_PRECIO'";
                using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn))
                {
                    object result = cmd.ExecuteScalar();
                    count = (result == null || result == DBNull.Value) ? 0 : Convert.ToInt32(result);// Obtener el conteo
                }
            }
            return count;
        }


        // Actualizar el estado de un presupuesto
        public void UpdateEstado(int idPresupuesto, string nuevoEstado)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                // Consulta SQL para actualizar el estado del presupuesto
                string sql = "UPDATE presupuestos SET estado=@estado WHERE idpresupuesto=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                    cmd.Parameters.AddWithValue("@id", idPresupuesto);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        #endregion
        // Obtener el ID del presupuesto asociado a un ingreso
        public int? ObtenerIdPresupuestoPorIdIngreso(int idIngreso)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                // Consulta SQL para obtener el ID del presupuesto
                string sql = "SELECT idpresupuesto FROM presupuestos WHERE idingreso = @idIngreso LIMIT 1";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@idIngreso", idIngreso);
                    var result = cmd.ExecuteScalar();
                    // Retornar el ID si se encuentra, de lo contrario null
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            return null;
        }
        // Registrar el retiro de un presupuesto sin reparar
        public void RegistrarRetiroSinReparar(int idPresupuesto)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                // Consulta SQL para actualizar el estado y la fecha de retiro
                string sql = @"
            UPDATE presupuestos 
            SET estado = 'DEVUELTO_SIN_REPARAR',
                fechaRetiro = @fechaAhora,
                autorizado = 'NO'
            WHERE idpresupuesto = @id";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idPresupuesto);
                    cmd.Parameters.AddWithValue("@fechaAhora", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // Obtener el estado de un presupuesto por el ID del ingreso asociado
        public string ObtenerEstadoPresupuestoPorIdIngreso(int idIngreso)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                // Consulta SQL para obtener el estado del presupuesto
                string sql = "SELECT estado FROM presupuestos WHERE idingreso = @idIngreso LIMIT 1";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@idIngreso", idIngreso);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return result.ToString();
                    }
                }
            }
            return null; 
        }
    }
}
