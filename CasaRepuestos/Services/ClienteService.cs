using CasaRepuestos.Config;
using CasaRepuestos.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace CasaRepuestos.Services
{
    public class ClienteService
    {


        public List<Cliente> ListarClientes()
        {
            var lista = new List<Cliente>();
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            var query = @"SELECT c.idcliente, c.categoria, c.cuil, p.*
                          FROM clientes c
                          INNER JOIN personas p ON c.idpersona = p.idpersona";
            var cmd = new MySqlCommand(query, conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
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
                    IdPersona = persona.IdPersona,
                    DatosPersona = persona
                };

                lista.Add(cliente);
            }

            return lista;
        }

        public void CrearCliente(Cliente cliente)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            if (cliente.DatosPersona == null)
                throw new Exception("Los datos de la persona son obligatorios.");
            if (string.IsNullOrWhiteSpace(cliente.DatosPersona.Nombre))
                throw new Exception("El nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(cliente.DatosPersona.Apellido))
                throw new Exception("El apellido es obligatorio.");
            if (string.IsNullOrWhiteSpace(cliente.DatosPersona.NumeroDocumento))
                throw new Exception("El número de documento es obligatorio.");
            if (!cliente.DatosPersona.NumeroDocumento.All(char.IsDigit))
                throw new Exception("El documento solo puede contener números.");
            if (string.IsNullOrWhiteSpace(cliente.Cuil) || !cliente.Cuil.All(char.IsDigit))
                throw new Exception("El CUIL es obligatorio y solo debe contener números.");

            if (string.IsNullOrWhiteSpace(cliente.DatosPersona?.Telefono))
                throw new Exception("El teléfono es obligatorio.");

            if (string.IsNullOrWhiteSpace(cliente.DatosPersona?.Direccion))
                throw new Exception("La dirección es obligatoria.");

            if (string.IsNullOrWhiteSpace(cliente.Categoria)) throw new Exception("La categoría es obligatoria.");

            try
            {
                var mail = new System.Net.Mail.MailAddress(cliente.DatosPersona.Email);
            }
            catch
            {
                throw new Exception("El email no tiene un formato válido.");
            }



            // Validar DNI único
            var checkCmd = new MySqlCommand("SELECT COUNT(*) FROM personas WHERE numero_documento = @dni", conn);
            checkCmd.Parameters.AddWithValue("@dni", cliente.DatosPersona?.NumeroDocumento);
            var existe = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;
            if (existe)
                throw new Exception("El DNI ya está registrado.");

            // Insertar persona
            var insertPersona = @"INSERT INTO personas (nombre, apellido, tipo_documento, numero_documento, telefono, email, direccion)
                                  VALUES (@nombre, @apellido, @tipo_doc, @num_doc, @telefono, @email, @direccion);
                                  SELECT LAST_INSERT_ID();";
            var cmdPersona = new MySqlCommand(insertPersona, conn);
            cmdPersona.Parameters.AddWithValue("@nombre", cliente.DatosPersona?.Nombre);
            cmdPersona.Parameters.AddWithValue("@apellido", cliente.DatosPersona?.Apellido);
            cmdPersona.Parameters.AddWithValue("@tipo_doc", cliente.DatosPersona?.TipoDocumento);
            cmdPersona.Parameters.AddWithValue("@num_doc", cliente.DatosPersona?.NumeroDocumento);
            cmdPersona.Parameters.AddWithValue("@telefono", cliente.DatosPersona?.Telefono);
            cmdPersona.Parameters.AddWithValue("@email", cliente.DatosPersona?.Email);
            cmdPersona.Parameters.AddWithValue("@direccion", cliente.DatosPersona?.Direccion);
            int idPersona = Convert.ToInt32(cmdPersona.ExecuteScalar());

            // Insertar cliente
            var insertCliente = @"INSERT INTO clientes (categoria, cuil, idpersona)
                                  VALUES (@categoria, @cuil, @idpersona)";
            var cmdCliente = new MySqlCommand(insertCliente, conn);
            cmdCliente.Parameters.AddWithValue("@categoria", cliente.Categoria);
            cmdCliente.Parameters.AddWithValue("@cuil", cliente.Cuil);
            cmdCliente.Parameters.AddWithValue("@idpersona", idPersona);
            cmdCliente.ExecuteNonQuery();
        }

        public void EliminarCliente(int idCliente)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            var delete = "DELETE FROM clientes WHERE idcliente = @id";
            var cmd = new MySqlCommand(delete, conn);
            cmd.Parameters.AddWithValue("@id", idCliente);
            cmd.ExecuteNonQuery();
        }

        public void ModificarCliente(Cliente cliente)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            //  Actualizar datos en la tabla personas
            var updatePersona = @"UPDATE personas 
                          SET nombre = @nombre,
                              apellido = @apellido,
                              tipo_documento = @tipo_doc,
                              numero_documento = @num_doc,
                              telefono = @telefono,
                              email = @email,
                              direccion = @direccion
                          WHERE idpersona = @idpersona";
            var cmdPersona = new MySqlCommand(updatePersona, conn);
            cmdPersona.Parameters.AddWithValue("@nombre", cliente.DatosPersona?.Nombre);
            cmdPersona.Parameters.AddWithValue("@apellido", cliente.DatosPersona?.Apellido);
            cmdPersona.Parameters.AddWithValue("@tipo_doc", cliente.DatosPersona?.TipoDocumento);
            cmdPersona.Parameters.AddWithValue("@num_doc", cliente.DatosPersona?.NumeroDocumento);
            cmdPersona.Parameters.AddWithValue("@telefono", cliente.DatosPersona?.Telefono);
            cmdPersona.Parameters.AddWithValue("@email", cliente.DatosPersona?.Email);
            cmdPersona.Parameters.AddWithValue("@direccion", cliente.DatosPersona?.Direccion);
            cmdPersona.Parameters.AddWithValue("@idpersona", cliente.IdPersona);
            cmdPersona.ExecuteNonQuery();

            //  Actualizar datos en la tabla clientes
            var updateCliente = @"UPDATE clientes 
                          SET categoria = @categoria,
                              cuil = @cuil
                          WHERE idcliente = @id";
            var cmdCliente = new MySqlCommand(updateCliente, conn);
            cmdCliente.Parameters.AddWithValue("@categoria", cliente.Categoria);
            cmdCliente.Parameters.AddWithValue("@cuil", cliente.Cuil);
            cmdCliente.Parameters.AddWithValue("@id", cliente.IdCliente);
            cmdCliente.ExecuteNonQuery();
        }

        public Cliente? ObtenerClientePorId(int idCliente)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            var query = @"SELECT c.idcliente, c.categoria, c.cuil, p.*
                  FROM clientes c
                  INNER JOIN personas p ON c.idpersona = p.idpersona
                  WHERE c.idcliente = @idCliente";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idCliente", idCliente);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
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
                    IdPersona = persona.IdPersona,
                    DatosPersona = persona
                };

                return cliente;
            }
            return null; 
        }

        /*********************
         * CUENTAS CORRIENTES
         * *******************/

        public List<Cliente> ListarCuentasCorrientes()
        {
            var lista = new List<Cliente>();
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            var query = @"SELECT c.idcliente, c.categoria, c.cuil, p.*, cc.saldo_actual, cc.idcuenta_corriente
                          FROM cuentas_corrientes cc
                          INNER JOIN clientes c  ON cc.idcliente = c.idcliente
                          INNER JOIN personas p ON c.idpersona = p.idpersona
                          WHERE saldo_actual > 0";
            var cmd = new MySqlCommand(query, conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
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

                var cuentacorriente = new CuentaCorriente
                {
                    IdCuentaCorriente = reader.GetInt32("idcuenta_corriente"),
                    SaldoActual = reader.GetDecimal("saldo_actual"),
                };

                var cliente = new Cliente
                {
                    IdCliente = reader.GetInt32("idcliente"),
                    Categoria = reader.GetString("categoria"),
                    Cuil = reader.GetString("cuil"),
                    IdPersona = persona.IdPersona,
                    DatosPersona = persona,
                    DatosCuentaCorriente = cuentacorriente

                };

                lista.Add(cliente);
            }

            return lista;

        }


        public void ActualizarSaldoDeCuentaCorriente(int idCuentaCorriente, decimal monto, DateTime fecha)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            using var transaction = conn.BeginTransaction();
            try
            {
                // Registrar el movimiento de la cuenta corriente
                var insertMovimiento = @"
                    INSERT INTO movimientos_cuentas_corrientes (idcuentaacorriente, monto, fecha_de_pago)
                    VALUES (@idCuentaCorriente, @monto, @fecha)";

                using (var cmdMovimiento = new MySqlCommand(insertMovimiento, conn, transaction))
                {
                    cmdMovimiento.Parameters.AddWithValue("@idCuentaCorriente", idCuentaCorriente);
                    cmdMovimiento.Parameters.AddWithValue("@monto", monto);
                    cmdMovimiento.Parameters.AddWithValue("@fecha", fecha);
                    cmdMovimiento.ExecuteNonQuery();
                }

                // Actualizar el saldo en la cuenta corriente
                var updateCuentaCte = @"
                    UPDATE cuentas_corrientes
                    SET saldo_actual = saldo_actual - @monto
                    WHERE idcuenta_corriente = @idCuentaCorriente";

                using (var cmdCuentaCte = new MySqlCommand(updateCuentaCte, conn, transaction))
                {
                    cmdCuentaCte.Parameters.AddWithValue("@monto", monto);
                    cmdCuentaCte.Parameters.AddWithValue("@idCuentaCorriente", idCuentaCorriente);
                    cmdCuentaCte.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                throw new Exception("Error al actualizar la cuenta corriente: " + ex.Message);
            }
        }


    }
}