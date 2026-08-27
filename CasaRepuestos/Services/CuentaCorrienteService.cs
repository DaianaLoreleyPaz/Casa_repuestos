using CasaRepuestos.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaRepuestos.Services
{
    public class CuentaCorrienteService
    {
        public List<Facturas> ListarFacturasPendientesDeCliente(int idcuentacorriente)
        {
            var lista = new List<Facturas>();
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            var query = @"SELECT f.idfactura, f.total_factura, f.idpresupuesto, f.metodo_pago,
                                 f.descripcion_metodopago, f.fecha, f.tipo_factura
                          FROM facturas f
                          WHERE f.idcuentacorriente = @idcuentacorriente
                            AND f.estado='PENDIENTE'";
            var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idcuentacorriente", idcuentacorriente);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var factura = new Facturas
                {
                    ID_Factura = reader.GetInt32("idfactura"),
                    Total_Factura = reader.GetDecimal("total_factura"),
                    ID_Presupuesto = reader.IsDBNull(reader.GetOrdinal("idpresupuesto")) ? (int?)null : reader.GetInt32("idpresupuesto"),
                    Metodo_Pago = reader.GetString("metodo_pago"),
                    Descripcion_Metodo_Pago = reader.GetString("descripcion_metodopago"),
                    Fecha = reader.GetDateTime("fecha"),
                    Tipo_Factura = reader.GetString("tipo_factura"),
                };

                lista.Add(factura);
            }

            return lista;
        }

        public void RegistrarPagoEnCuentaCorriente(MovimientoCuentaCorriente movCtaCte)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            var insertMovCtaCte = @"
                        INSERT INTO movimientos_cuentas_corrientes 
                        (idcuentaacorriente, idfactura, fecha_de_pago, monto, metodo_pago, descripcion_metodopago, concepto)
                        VALUES (@idcuentaacorriente, @idfactura, @fecha_de_pago, @monto, @metodo_pago, @descripcion_metodopago, 
                                @concepto)";


            using var cmd = new MySqlCommand(insertMovCtaCte, conn);
            cmd.Parameters.AddWithValue("@idcuentaacorriente", movCtaCte.Id_Cuenta_Corriente);
            cmd.Parameters.AddWithValue("@idfactura", movCtaCte.Id_Factura);
            cmd.Parameters.AddWithValue("@fecha_de_pago", movCtaCte.Fecha);
            cmd.Parameters.AddWithValue("@monto", movCtaCte.Monto);
            cmd.Parameters.AddWithValue("@metodo_pago", movCtaCte.Metodo_Pago);
            cmd.Parameters.AddWithValue("@descripcion_metodopago",
                (object?)movCtaCte.Descripcion_Metodo_Pago ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@concepto", movCtaCte.Concepto);
            cmd.ExecuteNonQuery();




        }

        public void ActualizarSaldoCuentaCorriente(int idCuentaCorriente, decimal saldoActual)
        {
            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                string sql = @"
                            UPDATE cuentas_corrientes
                            SET saldo_actual = @saldoActual
                            WHERE idcuenta_corriente = @idCuentaCorriente";

                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@saldoActual", saldoActual);
                cmd.Parameters.AddWithValue("@idCuentaCorriente", idCuentaCorriente);

                int filasAfectadas = cmd.ExecuteNonQuery();

                if (filasAfectadas == 0)
                {
                    throw new Exception($"No se encontró la cuenta corriente con ID {idCuentaCorriente} para actualizar el saldo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el saldo de la cuenta corriente:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public decimal ObtenerSaldoCuentaCorriente(int idCuentaCorriente)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            string sql = "SELECT saldo_actual FROM cuentas_corrientes WHERE idcuenta_corriente = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", idCuentaCorriente);

            var result = cmd.ExecuteScalar();
            return result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
        }

        public decimal ObtenerSaldoPendienteFactura(int idFactura)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            // Calcular total pagado hasta el momento
            string sql = @"
                    SELECT 
                        f.total_factura - IFNULL(SUM(m.monto), 0) AS saldo_pendiente
                    FROM facturas f
                    LEFT JOIN movimientos_cuentas_corrientes m ON f.idfactura = m.idfactura
                    WHERE f.idfactura = @idFactura
                    GROUP BY f.idfactura;";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@idFactura", idFactura);

            var result = cmd.ExecuteScalar();
            return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
        }


       
    }
}



