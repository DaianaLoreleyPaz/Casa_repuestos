using CasaRepuestos.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace CasaRepuestos.Services
{
    public class FacturaService
    {
 

        public List<Presupuesto> ListarPresupuestosFinalizados()
        {
            var presupuestos = new List<Presupuesto>();

            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            {
                string sql = @"
            SELECT IdPresupuesto, Fecha, Total, Autorizado, Estado, IdEmpleado, IdIngreso 
            FROM presupuestos 
            WHERE estado = 'FINALIZADO'";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            presupuestos.Add(new Presupuesto
                            {
                                IdPresupuesto = reader["IdPresupuesto"] != DBNull.Value ? Convert.ToInt32(reader["IdPresupuesto"]) : 0,

                                Fecha = reader["Fecha"] != DBNull.Value ? Convert.ToDateTime(reader["Fecha"]) : (DateTime?)null,


                                Total = reader["Total"] != DBNull.Value ? Convert.ToDecimal(reader["Total"]) : 0m,
                                Autorizado = reader["Autorizado"] != DBNull.Value ? reader["Autorizado"].ToString() : string.Empty,
                                Estado = reader["Estado"] != DBNull.Value ? reader["Estado"].ToString() : string.Empty,
                                IdEmpleado = reader["IdEmpleado"] != DBNull.Value ? Convert.ToInt32(reader["IdEmpleado"]) : 0,
                                IdIngreso = reader["IdIngreso"] != DBNull.Value ? Convert.ToInt32(reader["IdIngreso"]) : 0
                            });
                        }
                    }
                }
            }

            return presupuestos;
        }





        // Leer factura por ID con detalles
        public Facturas ObtenerFacturaPorId(int idFactura)
        {
            Facturas factura = null;

            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            string sql = "SELECT * FROM facturas WHERE idfactura = @ID_Factura";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ID_Factura", idFactura);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                factura = new Facturas
                {
                    ID_Factura = Convert.ToInt32(reader["idfactura"]),
                    ID_Presupuesto = reader["idpresupuesto"] != DBNull.Value ? Convert.ToInt32(reader["idpresupuesto"]) : 0,
                    Metodo_Pago = reader["metodo_pago"] != DBNull.Value ? reader["metodo_pago"].ToString() : string.Empty,
                    Descripcion_Metodo_Pago = reader["descripcion_metodopago"] != DBNull.Value ? reader["descripcion_metodopago"].ToString() : string.Empty,
                    Fecha = reader["fecha"] != DBNull.Value ? Convert.ToDateTime(reader["fecha"]) : DateTime.MinValue,
                    Total_Factura = reader["total_factura"] != DBNull.Value ? Convert.ToDecimal(reader["total_factura"]) : 0m,
                    Tipo_Factura = reader["tipo_factura"] != DBNull.Value ? reader["tipo_factura"].ToString() : string.Empty,
                    Categoria_Cliente = reader["categoria_cliente"] != DBNull.Value ? reader["categoria_cliente"].ToString() : null,
                    ID_Cuenta_Corriente = reader["idcuentacorriente"] != DBNull.Value ? Convert.ToInt32(reader["idcuentacorriente"]) : (int?)null,
                    estado = reader["estado"] != DBNull.Value ? reader["estado"].ToString() : "PENDIENTE"
                };
            }

            return factura;
        }



        // Actualizar factura (solo datos generales, no detalles)
        public void ActualizarFactura(Facturas factura)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            {

                string sql = @"UPDATE Facturas SET 
                           Metodo_Pago=@Metodo_Pago,
                           Descripcion_Metodo_Pago=@Descripcion_Metodo_Pago,
                           Fecha=@Fecha,
                           Total_Factura=@Total_Factura,
                           Tipo_Factura=@Tipo_Factura,
                           ID_Cuenta_Corriente=@ID_Cuenta_Corriente
                           WHERE ID_Factura=@ID_Factura";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Metodo_Pago", factura.Metodo_Pago);
                    cmd.Parameters.AddWithValue("@Descripcion_Metodo_Pago", factura.Descripcion_Metodo_Pago);
                    cmd.Parameters.AddWithValue("@Fecha", factura.Fecha);
                    cmd.Parameters.AddWithValue("@Total_Factura", factura.Total_Factura);
                    cmd.Parameters.AddWithValue("@Tipo_Factura", factura.Tipo_Factura);
                    cmd.Parameters.AddWithValue("@ID_Cuenta_Corriente",
                        factura.ID_Cuenta_Corriente.HasValue ? (object)factura.ID_Cuenta_Corriente.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@ID_Factura", factura.ID_Factura);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Eliminar factura con sus detalles
        public void EliminarFactura(int idFactura)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            {

                using (MySqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        // Primero eliminar detalles
                        string sqlDetalles = "DELETE FROM DetalleFactura WHERE ID_Factura=@ID_Factura";
                        using (MySqlCommand cmd = new MySqlCommand(sqlDetalles, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@ID_Factura", idFactura);
                            cmd.ExecuteNonQuery();
                        }

                        // Luego eliminar factura
                        string sqlFactura = "DELETE FROM Facturas WHERE ID_Factura=@ID_Factura";
                        using (MySqlCommand cmd = new MySqlCommand(sqlFactura, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@ID_Factura", idFactura);
                            cmd.ExecuteNonQuery();
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

        // Listar detalles de factura por ID de factura
        public List<DetalleFactura> ListarDetalles(int idFactura)
        {
            var detalles = new List<DetalleFactura>();

            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            {

                string sql = "SELECT * FROM detalle_factura WHERE idfactura=@ID_Factura";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ID_Factura", idFactura);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            detalles.Add(new DetalleFactura
                            {
                                ID_Detalle_Factura = Convert.ToInt32(reader["iddetalle_factura"]),
                                ID_Factura = Convert.ToInt32(reader["idfactura"]),
                                ID_Articulo = Convert.ToInt32(reader["idarticulo"]),
                                Cantidad = Convert.ToInt32(reader["cantidad"]),
                                subtotal = Convert.ToDecimal(reader["subtotal"])
                            });
                        }
                    }
                }
            }

            return detalles;
        }

        internal Cliente? ObtenerClientePorIDIngreso(int idIngreso) 
        {
            const string query = @"
                SELECT c.idcliente, c.categoria, c.cuil, c.idpersona,
                       p.nombre, p.apellido, p.numero_documento, p.telefono
                FROM ingresos i
                JOIN clientes c ON i.idcliente = c.idcliente
                JOIN personas p ON c.idpersona = p.idpersona
                WHERE i.idingreso = @idingreso;";

            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idingreso", idIngreso);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            if (!reader.Read()) return null;

            return new Cliente
            {
                IdCliente = reader.GetInt32("idcliente"),
                Categoria = reader.GetString("categoria"),
                Cuil = reader.GetString("cuil"),
                IdPersona = reader.GetInt32("idpersona"),
                DatosPersona = new Persona
                {
                    Nombre = reader.GetString("nombre"),
                    Apellido = reader.GetString("apellido"),
                    NumeroDocumento = reader.GetString("numero_documento"),
                    Telefono = reader.GetString("telefono")
                }
            };
        }

        internal void GuardarDetalleFactura(DetalleFactura detalle)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            {

                string sql = @"INSERT INTO detalle_factura 
                                SET cantidad=@cantidad, idarticulo=@idarticulo, idfactura=@idfactura, subtotal=@subtotal";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                    cmd.Parameters.AddWithValue("@idarticulo", detalle.ID_Articulo);
                    cmd.Parameters.AddWithValue("@idfactura", detalle.ID_Factura);
                    cmd.Parameters.AddWithValue("@subtotal", detalle.subtotal);
                    cmd.ExecuteNonQuery();

                }
            }
        }

        public List<DetallePresupuesto> ObtenerDetallesReparacion(int idpresupuesto)
        {
            var detalles = new List<DetallePresupuesto>();

            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            {
                string sql = "SELECT * FROM detalles_presupuestos WHERE idpresupuesto=@idpresupuesto";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@idpresupuesto", idpresupuesto);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            detalles.Add(new DetallePresupuesto
                            {
                                IdPresupuesto = Convert.ToInt32(reader["idpresupuesto"]),
                                IdArticulo = reader["idarticulo"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["idarticulo"]),
                                Cantidad = Convert.ToInt32(reader["cantidad"]),
                                IdServicio = (int)(reader["idservicio"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["idservicio"])),
                                //  Usar nullables para evitar errores si el campo es null
                                PrecioRepuesto = reader["precio_repuesto"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["precio_repuesto"]),
                                PrecioServicio = reader["precio_servicio"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["precio_servicio"])
                            });

                        }
                    }
                }
            }

            return detalles;
        }

        internal int? ObtenerCuentaCorriente(int idcliente)
        {
            using (var conn = new MySqlConnection(Config.Config.ConnectionString))
            {
                conn.Open();
                string query = "SELECT idcuenta_corriente FROM cuentas_corrientes WHERE idcliente = @idcliente";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    var result = cmd.ExecuteScalar();
                    return result != null && result != DBNull.Value ? Convert.ToInt32(result) : (int?)null;
                }
            }

        }

        internal int? CrearCuentaCorriente(int idcliente, decimal totalFactura)
        {
            using (var conn = new MySqlConnection(Config.Config.ConnectionString))
            {
                conn.Open();
                string query = @"INSERT INTO cuentas_corrientes (idcliente, saldo_actual)
                         VALUES (@idcliente, @saldo_actual);
                         SELECT LAST_INSERT_ID();";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    cmd.Parameters.AddWithValue("@saldo_actual", totalFactura);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int CrearFactura(Facturas factura, List<DetalleFactura>? detalles = null)
        {
            int idFactura = 0;

            using (var conn = new MySqlConnection(Config.Config.ConnectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        string sqlFactura = @"
                    INSERT INTO facturas 
                        (idpresupuesto, metodo_pago, descripcion_metodopago, fecha, total_factura, tipo_factura, categoria_cliente, idcuentacorriente, estado)
                    VALUES 
                        (@IdPresupuesto, @MetodoPago, @DescripcionMetodoPago, @Fecha, @TotalFactura, @TipoFactura, @CategoriaCliente, @IdCuentaCorriente, @estado);
                    SELECT LAST_INSERT_ID();";

                        using (var cmd = new MySqlCommand(sqlFactura, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@IdPresupuesto",
                                factura.ID_Presupuesto > 0 ? factura.ID_Presupuesto : DBNull.Value);
                            cmd.Parameters.AddWithValue("@MetodoPago", factura.Metodo_Pago);
                            cmd.Parameters.AddWithValue("@DescripcionMetodoPago", factura.Descripcion_Metodo_Pago ?? "SIN ESPECIFICAR");
                            cmd.Parameters.AddWithValue("@Fecha", factura.Fecha);
                            cmd.Parameters.AddWithValue("@TotalFactura", factura.Total_Factura);
                            cmd.Parameters.AddWithValue("@TipoFactura", factura.Tipo_Factura);
                            cmd.Parameters.AddWithValue("@CategoriaCliente", factura.Categoria_Cliente ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@estado", factura.estado);

                            if (factura.ID_Cuenta_Corriente.HasValue)
                                cmd.Parameters.AddWithValue("@IdCuentaCorriente", factura.ID_Cuenta_Corriente.Value);
                            else
                                cmd.Parameters.AddWithValue("@IdCuentaCorriente", DBNull.Value);

                            idFactura = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // Si hay detalles, los inserta
                        if (detalles != null && detalles.Count > 0)
                        {
                            foreach (var det in detalles)
                            {
                                string sqlDetalle = @"
                            INSERT INTO detalle_factura (idfactura, idarticulo, cantidad, subtotal)
                            VALUES (@IdFactura, @IdArticulo, @Cantidad, @Subtotal);";

                                using (var cmdDet = new MySqlCommand(sqlDetalle, conn, tran))
                                {
                                    cmdDet.Parameters.AddWithValue("@IdFactura", idFactura);
                                    cmdDet.Parameters.AddWithValue("@IdArticulo", det.ID_Articulo);
                                    cmdDet.Parameters.AddWithValue("@Cantidad", det.Cantidad);
                                    cmdDet.Parameters.AddWithValue("@Subtotal", det.subtotal);
                                    cmdDet.ExecuteNonQuery();
                                }
                            }
                        }

                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }

            return idFactura;
        }




        internal void ActualizarEstadoPresupuesto(int idPresupuesto, string nuevoEstado)
        {
            using (var conn = new MySqlConnection(Config.Config.ConnectionString))
            {
                conn.Open();
                string query = "UPDATE presupuestos SET estado = @estado WHERE idpresupuesto = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                    cmd.Parameters.AddWithValue("@id", idPresupuesto);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        internal Presupuesto ObtenerPresupuesto(int idPresupuesto)
        {
            using (var conn = new MySqlConnection(Config.Config.ConnectionString))
            {
                conn.Open();
                string sql = @"SELECT * FROM presupuestos WHERE idpresupuesto = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idPresupuesto);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Presupuesto
                            {
                                IdPresupuesto = reader.GetInt32("idpresupuesto"),

                                IdIngreso = reader.GetInt32("idingreso"),
                                FechaVencimiento = reader.GetDateTime("fecha_vencimiento"),
                                FechaRetiro = reader.GetDateTime("fecha_retiro"),
                                Fecha = reader.GetDateTime("fecha"),
                                Total = reader.GetDecimal("total"),
                                Autorizado = reader.GetString("autorizado"),
                                Estado = reader.GetString("estado"),
                                IdEmpleado = reader.GetInt32("idempleado")
                            };
                        }
                    }
                }
            }
            return null;
        }

        internal void ActualizarStockArticulos(List<DetalleFactura> detallesFactura)
        {
            using (var conn = new MySqlConnection(Config.Config.ConnectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = "UPDATE articulos SET stock = stock - @cantidad WHERE idarticulo = @idarticulo";
                        using (var cmd = new MySqlCommand(sql, conn, tran))
                        {
                            foreach (var detalle in detallesFactura)
                            {
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                                cmd.Parameters.AddWithValue("@idarticulo", detalle.ID_Articulo);
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

        public void MarcarFacturaComoPagada(int idFactura)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            string sql = @"UPDATE facturas SET estado = 'PAGADA' WHERE idfactura = @idFactura";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@idFactura", idFactura);
            cmd.ExecuteNonQuery();
        }

        public void CambiarEstadoYAutorizacion(int idPresupuesto)
        {
            using (var conn = new MySqlConnection(Config.Config.ConnectionString))
            {
                conn.Open();

                string sql = "UPDATE presupuestos SET estado='VERIFICAR_PRECIO', autorizado='NO' WHERE idpresupuesto=@id";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idPresupuesto);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Cliente? ObtenerClientePorIdIngreso(int idIngreso)
        {
            using var connection = new MySqlConnection(Config.Config.ConnectionString);
            connection.Open();

            string query = @"
            SELECT 
                c.idcliente, 
                c.categoria, 
                c.cuil, 
                c.idpersona,
                p.idpersona,
                p.nombre, 
                p.apellido, 
                p.tipo_documento,
                p.numero_documento, 
                p.telefono, 
                p.email, 
                p.direccion
            FROM ingresos i
            INNER JOIN clientes c ON i.idcliente = c.idcliente
            INNER JOIN personas p ON c.idpersona = p.idpersona
            WHERE i.idingreso = @idingreso";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@idingreso", idIngreso);

            using var reader = command.ExecuteReader();

            if (!reader.Read()) return null;

            var persona = new Persona
            {
                IdPersona = reader.GetInt32("idpersona"),
                Nombre = reader.GetString("nombre"),
                Apellido = reader.GetString("apellido"),
                TipoDocumento = reader.GetString("tipo_documento"),
                NumeroDocumento = reader.GetString("numero_documento"),
                Telefono = reader.GetString("telefono"),
                Email = reader.GetString("email"),
                Direccion = reader.GetString("direccion")
            };

            var cliente = new Cliente
            {
                IdCliente = reader.GetInt32("idcliente"),
                Categoria = reader.GetString("categoria"),
                Cuil = reader.GetString("cuil"),
                IdPersona = reader.GetInt32("idpersona"),
                DatosPersona = persona
            };

            return cliente;

        }
    }

}
