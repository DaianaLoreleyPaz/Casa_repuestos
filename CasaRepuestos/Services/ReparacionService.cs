using CasaRepuestos.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaRepuestos.Services
{
    public class ReparacionService
    {
        private readonly string _connectionString;

        public ReparacionService()
        {
            _connectionString = CasaRepuestos.Config.Config.ConnectionString;
        }

        /// <summary>
        ///esta funcion es usada para mostrar datos en CargarTodo() en el form 
        /// </summary>
        public DataTable ObtenerPresupuestosConEstadoStock()
        {
            var dt = new DataTable();

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                // Esta consulta solo lee los datos
                string sql = @"
                SELECT 
                    p.idpresupuesto, 
                    CONCAT(per.nombre, ' ', per.apellido) AS cliente,
                    p.fecha, 
                    p.estado,
                    p.estado AS stock_status, -- El estado ya es correcto
                    i.modelo, 
                    IFNULL(m.nombre, '') AS marca
                FROM presupuestos p
                INNER JOIN ingresos i ON p.idingreso = i.idingreso
                INNER JOIN clientes cl ON i.idcliente = cl.idcliente
                INNER JOIN personas per ON cl.idpersona = per.idpersona
                LEFT JOIN marcas m ON i.idmarca = m.idmarca
                WHERE p.estado IN ('PENDIENTE', 'ESPERA_REPUESTOS', 'EN_PROCESO', 'FINALIZADO') 
                  AND p.autorizado = 'SI'
                ORDER BY p.fecha ASC";

                using (var cmd = new MySqlCommand(sql, conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        /// <summary>
        /// (Helper) Actualiza el estado de un presupuesto, opcionalmente dentro de una transacción.
        /// </summary>
        public void ActualizarEstadoPresupuesto(int idPresupuesto, string nuevoEstado, MySqlConnection conn, MySqlTransaction tran)
        {
            // Reutiliza tu método existente, pero permite pasarle la conexión y transacción
            string sql = "UPDATE presupuestos SET estado = @estado WHERE idpresupuesto = @id AND estado != @estado";
            using (var cmd = new MySqlCommand(sql, conn, tran)) // Pasa la transacción
            {
                cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                cmd.Parameters.AddWithValue("@id", idPresupuesto);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Mueve un presupuesto a FINALIZADO y consume el 'stock_comprometido' que tenía reservado.
        /// </summary>
        public void ConsumirStockYFinalizar(int idPresupuesto)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Obtener los detalles (los repuestos necesarios)
                        string sqlDetalles = @"
                            SELECT idarticulo, cantidad 
                            FROM detalles_presupuestos 
                            WHERE idpresupuesto = @idpresupuesto AND idarticulo IS NOT NULL;";

                        using var cmdDet = new MySqlCommand(sqlDetalles, conn, tran);
                        cmdDet.Parameters.AddWithValue("@idpresupuesto", idPresupuesto);
                        using var readerDet = cmdDet.ExecuteReader();

                        var detalles = new List<(int idArticulo, int cantidad)>();
                        while (readerDet.Read())
                            detalles.Add((readerDet.GetInt32("idarticulo"), readerDet.GetInt32("cantidad")));
                        readerDet.Close();

                        if (detalles.Count > 0)
                        {
                            // 2. Consumir el stock SÓLO de 'stock_comprometido'
                            foreach (var (idArticulo, cantidadRequerida) in detalles)
                            {
                                // Bloqueamos la fila y verificamos el comprometido
                                string sqlGetStock = "SELECT COALESCE(stock_comprometido, 0) AS stock_comprometido FROM articulos WHERE idarticulo = @id FOR UPDATE;";
                                int stockComprometido = 0;

                                using (var cmdGet = new MySqlCommand(sqlGetStock, conn, tran))
                                {
                                    cmdGet.Parameters.AddWithValue("@id", idArticulo);
                                    object result = cmdGet.ExecuteScalar();
                                    if (result != null && result != DBNull.Value)
                                        stockComprometido = Convert.ToInt32(result);
                                }


                                // Consumimos ÚNICAMENTE del stock comprometido
                                string sqlConsumir = @"
                                UPDATE articulos
                                SET 
                                stock_comprometido = GREATEST(0, stock_comprometido - @cantRequerida)
                                WHERE idarticulo = @idarticulo;";

                                using var cmdUpd = new MySqlCommand(sqlConsumir, conn, tran);
                                cmdUpd.Parameters.AddWithValue("@cantRequerida", cantidadRequerida);
                                cmdUpd.Parameters.AddWithValue("@idarticulo", idArticulo);
                                cmdUpd.ExecuteNonQuery();
                            }
                        }

                        // 3. Mover el presupuesto a FINALIZADO
                        string sqlUpdatePres = @"
                            UPDATE presupuestos 
                            SET estado = 'FINALIZADO', 
                                fecha = @fechaHoy 
                            WHERE idpresupuesto = @id;";

                        using var cmdPres = new MySqlCommand(sqlUpdatePres, conn, tran);
                        cmdPres.Parameters.AddWithValue("@id", idPresupuesto);
                        cmdPres.Parameters.AddWithValue("@fechaHoy", DateTime.Now);
                        cmdPres.ExecuteNonQuery();

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new Exception($"Error al finalizar la reparación {idPresupuesto}: {ex.Message}");
                    }
                }
            }
        }


        /// <summary>
        /// (CORE) Lógica principal de asignación. Discrimina entre SERVICIOS (pasan directo) y REPUESTOS (verifican stock).
        /// </summary>
        public string ProcesarInicioReparacion(int idPresupuesto)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {

                        // ---------------------------------------------------------
                        // PASO 0: VALIDACIÓN DE SEGURIDAD
                        // ---------------------------------------------------------
                        string sqlCheckEstado = "SELECT estado FROM presupuestos WHERE idpresupuesto = @id";
                        string estadoActual = "";

                        using (var cmdCheck = new MySqlCommand(sqlCheckEstado, conn, tran))
                        {
                            cmdCheck.Parameters.AddWithValue("@id", idPresupuesto);
                            var result = cmdCheck.ExecuteScalar();
                            if (result == null) throw new Exception("El presupuesto no existe.");
                            estadoActual = result.ToString();
                        }

                        if (estadoActual == "FINALIZADO" || estadoActual == "ENTREGADO")
                            throw new InvalidOperationException($"No se puede procesar un presupuesto en estado {estadoActual}.");

                        // ---------------------------------------------------------
                        // PASO 1: OBTENER DETALLES (SERVICIOS Y REPUESTOS)
                        // Nota: Quitamos el 'IS NOT NULL' para traer TODO y filtrar en código.
                        // ---------------------------------------------------------
                        string sqlDetalles = @"
                    SELECT idarticulo, cantidad 
                    FROM detalles_presupuestos 
                    WHERE idpresupuesto = @idpresupuesto";

                        // Lista solo para los repuestos físicos que requieren stock
                        var repuestosFisicos = new List<(int idArticulo, int cantidad)>();

                        using (var cmdDet = new MySqlCommand(sqlDetalles, conn, tran))
                        {
                            cmdDet.Parameters.AddWithValue("@idpresupuesto", idPresupuesto);
                            using (var readerDet = cmdDet.ExecuteReader())
                            {
                                int colIdxArticulo = readerDet.GetOrdinal("idarticulo");
                                int colIdxCantidad = readerDet.GetOrdinal("cantidad");

                                while (readerDet.Read())
                                {
                                    // AQUI ESTÁ TU LOGICA: Verificamos si es NULL
                                    if (readerDet.IsDBNull(colIdxArticulo))
                                    {
                                        // Es un SERVICIO (Mano de obra, Formateo, etc.)
                                        // No se agrega a la lista de control de stock.
                                        continue;
                                    }

                                    // Si tiene ID, es un REPUESTO físico
                                    repuestosFisicos.Add((readerDet.GetInt32(colIdxArticulo), readerDet.GetInt32(colIdxCantidad)));
                                }
                            }
                        }

                        // ---------------------------------------------------------
                        // CASO: SOLO SERVICIOS (O Presupuesto vacío)
                        // Si la lista de repuestos quedó vacía, significa que todo eran servicios.
                        // Pasa directo a EN_PROCESO sin verificar stock.
                        // ---------------------------------------------------------
                        if (repuestosFisicos.Count == 0)
                        {
                            ActualizarEstadoPresupuesto(idPresupuesto, "EN_PROCESO", conn, tran);
                            tran.Commit();
                            return "EN_PROCESO_SERVICIO";
                        }

                        // ---------------------------------------------------------
                        // PASO 2: VERIFICACIÓN CON BLOQUEO (FOR UPDATE) - SOLO REPUESTOS
                        // ---------------------------------------------------------
                        bool hayStockParaTodo = true;

                        foreach (var (idArticulo, cantidadRequerida) in repuestosFisicos)
                        {
                            string sqlGetStock = "SELECT stock FROM articulos WHERE idarticulo = @id FOR UPDATE";

                            using (var cmdGet = new MySqlCommand(sqlGetStock, conn, tran))
                            {
                                cmdGet.Parameters.AddWithValue("@id", idArticulo);
                                object result = cmdGet.ExecuteScalar();

                                int stockActual = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;

                                if (stockActual < cantidadRequerida)
                                {
                                    hayStockParaTodo = false;
                                    // No hacemos break para asegurar que el bloqueo se mantenga consistente
                                }
                            }
                        }
                        // ---------------------------------------------------------
                        // PASO 3: EJECUCIÓN
                        // ---------------------------------------------------------
                        string estadoFinal = "";

                        if (hayStockParaTodo)
                        {
                            // A. HAY STOCK: Reservamos y avanzamos
                            foreach (var (idArticulo, cantidadRequerida) in repuestosFisicos)
                            {
                                string sqlUpdateStock = @"
                            UPDATE articulos 
                            SET stock = stock - @cant, 
                                stock_comprometido = stock_comprometido + @cant 
                            WHERE idarticulo = @id";

                                using (var cmdUpd = new MySqlCommand(sqlUpdateStock, conn, tran))
                                {
                                    cmdUpd.Parameters.AddWithValue("@id", idArticulo);
                                    cmdUpd.Parameters.AddWithValue("@cant", cantidadRequerida);
                                    cmdUpd.ExecuteNonQuery();
                                }
                            }
                            ActualizarEstadoPresupuesto(idPresupuesto, "EN_PROCESO", conn, tran);
                            estadoFinal = "EN_PROCESO";
                        }
                        else
                        {
                            // B. NO HAY STOCK
                            // Aquí hacemos la distinción inteligente:

                            if (estadoActual == "ESPERA_REPUESTOS")
                            {
                                // Si YA estaba esperando y sigue sin haber, no hacemos update a la BD
                                // (porque ya está en ese estado), pero avisamos con un código especial.
                                estadoFinal = "AUN_SIN_STOCK";
                            }
                            else
                            {
                                // Es la primera vez que falla, cambiamos el estado en BD.
                                ActualizarEstadoPresupuesto(idPresupuesto, "ESPERA_REPUESTOS", conn, tran);
                                estadoFinal = "ESPERA_REPUESTOS";
                            }
                        }

                        tran.Commit();
                        return estadoFinal;
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }
        /// <summary>
        /// (Helper) Sobrecarga pública para actualizar estado sin transacción.
        /// </summary>
        public void ActualizarEstadoPresupuesto(int idPresupuesto, string nuevoEstado)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE presupuestos SET estado = @estado WHERE idpresupuesto = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                    cmd.Parameters.AddWithValue("@id", idPresupuesto);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// (Lectura) Obtiene presupuestos para grillas (usado por FrmReparaciones).
        /// </summary>
        public List<Presupuesto> GetPresupuestosPorEstado(string estado)
        {
            var list = new List<Presupuesto>();
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                SELECT 
                    p.idpresupuesto, p.total, p.autorizado, p.estado, p.idempleado, 
                    p.idingreso, p.fechaVencimiento, i.modelo, i.falla,
                    
                    CONCAT(per.nombre, ' ', per.apellido) AS ClienteNombre

                FROM presupuestos p
                INNER JOIN ingresos i ON p.idingreso = i.idingreso
                INNER JOIN clientes c ON i.idcliente = c.idcliente 
                
                INNER JOIN personas per ON c.idpersona = per.idpersona

                WHERE p.estado = @estado AND p.autorizado = 'SI'
                ORDER BY p.idpresupuesto DESC";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@estado", estado);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            list.Add(new Presupuesto
                            {
                                IdPresupuesto = rdr.GetInt32("idpresupuesto"),
                                Total = rdr.GetDecimal("total"),
                                Autorizado = rdr.GetString("autorizado"),
                                Estado = rdr.GetString("estado"),
                                IdEmpleado = rdr.GetInt32("idempleado"),
                                IdIngreso = rdr.GetInt32("idingreso"),
                                ClienteNombre = rdr.GetString("ClienteNombre"),
                                Modelo = rdr.GetString("modelo"),
                                Falla = rdr.GetString("falla"),
                                FechaVencimiento = rdr.IsDBNull(rdr.GetOrdinal("fechaVencimiento")) ? null : (DateTime?)rdr.GetDateTime("fechaVencimiento"),
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}