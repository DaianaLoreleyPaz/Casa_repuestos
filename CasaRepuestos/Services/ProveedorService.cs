using CasaRepuestos.Config;
using CasaRepuestos.Models;
using MySql.Data.MySqlClient;

namespace CasaRepuestos.Services
{
    public class ProveedorService
    {
        public List<Proveedor> ListarProveedores()
        {
            var lista = new List<Proveedor>();
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            var query = @"SELECT p.idproveedor, p.cuit, p.razon_social, per.*
                          FROM proveedores p
                          INNER JOIN personas per ON p.idpersona = per.idpersona";
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

                var proveedor = new Proveedor
                {
                    IdProveedor = reader.GetInt32("idproveedor"),
                    Cuil = reader.GetString("cuit"),
                    RazonSocial = reader.GetString("razon_social"),
                    IdPersona = persona.IdPersona,
                    DatosPersona = persona
                };

                lista.Add(proveedor);
            }

            return lista;
        }

        public void CrearProveedor(Proveedor proveedor)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            if (proveedor.DatosPersona == null)
                throw new Exception("Los datos de la persona son obligatorios.");
            if (string.IsNullOrWhiteSpace(proveedor.RazonSocial))
                throw new Exception("La razón social es obligatoria.");
            if (string.IsNullOrWhiteSpace(proveedor.Cuil) || !proveedor.Cuil.All(char.IsDigit))
                throw new Exception("El CUIT/CUIL es obligatorio y debe contener solo números.");
            if (!proveedor.DatosPersona.NumeroDocumento.All(char.IsDigit))
                throw new Exception("El documento solo puede contener números.");
            if (string.IsNullOrWhiteSpace(proveedor.DatosPersona.NumeroDocumento))
                throw new Exception("El documento es obligatorio.");


            var existentes = ListarProveedores();
            if (existentes.Any(p => p.Cuil == proveedor.Cuil))
                throw new Exception("El CUIT/CUIL ya está registrado en otro proveedor.");

            // Validar DNI único
            var checkCmd = new MySqlCommand("SELECT COUNT(*) FROM personas WHERE numero_documento = @dni", conn);
            checkCmd.Parameters.AddWithValue("@dni", proveedor.DatosPersona?.NumeroDocumento);
            var existe = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;
            if (existe)
                throw new Exception("El DNI ya está registrado.");

            // Insertar persona
            var insertPersona = @"INSERT INTO personas (nombre, apellido, tipo_documento, numero_documento, telefono, email, direccion)
                                  VALUES (@nombre, @apellido, @tipo_doc, @num_doc, @telefono, @email, @direccion);
                                  SELECT LAST_INSERT_ID();";
            var cmdPersona = new MySqlCommand(insertPersona, conn);
            cmdPersona.Parameters.AddWithValue("@nombre", proveedor.DatosPersona?.Nombre);
            cmdPersona.Parameters.AddWithValue("@apellido", proveedor.DatosPersona?.Apellido);
            cmdPersona.Parameters.AddWithValue("@tipo_doc", proveedor.DatosPersona?.TipoDocumento);
            cmdPersona.Parameters.AddWithValue("@num_doc", proveedor.DatosPersona?.NumeroDocumento);
            cmdPersona.Parameters.AddWithValue("@telefono", proveedor.DatosPersona?.Telefono);
            cmdPersona.Parameters.AddWithValue("@email", proveedor.DatosPersona?.Email);
            cmdPersona.Parameters.AddWithValue("@direccion", proveedor.DatosPersona?.Direccion);
            int idPersona = Convert.ToInt32(cmdPersona.ExecuteScalar());

            // Insertar proveedor
            var insertProveedor = @"INSERT INTO proveedores (cuit, razon_social, idpersona)
                                    VALUES (@cuit, @razon, @idpersona)";
            var cmdProveedor = new MySqlCommand(insertProveedor, conn);
            cmdProveedor.Parameters.AddWithValue("@cuit", proveedor.Cuil);
            cmdProveedor.Parameters.AddWithValue("@razon", proveedor.RazonSocial);
            cmdProveedor.Parameters.AddWithValue("@idpersona", idPersona);
            cmdProveedor.ExecuteNonQuery();
        }

        public void EliminarProveedor(int idProveedor)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            var delete = "DELETE FROM proveedores WHERE idproveedor = @id";
            var cmd = new MySqlCommand(delete, conn);
            cmd.Parameters.AddWithValue("@id", idProveedor);
            cmd.ExecuteNonQuery();
        }

        public void ModificarProveedor(Proveedor proveedor)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            // Actualizar persona
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
            cmdPersona.Parameters.AddWithValue("@nombre", proveedor.DatosPersona?.Nombre);
            cmdPersona.Parameters.AddWithValue("@apellido", proveedor.DatosPersona?.Apellido);
            cmdPersona.Parameters.AddWithValue("@tipo_doc", proveedor.DatosPersona?.TipoDocumento);
            cmdPersona.Parameters.AddWithValue("@num_doc", proveedor.DatosPersona?.NumeroDocumento);
            cmdPersona.Parameters.AddWithValue("@telefono", proveedor.DatosPersona?.Telefono);
            cmdPersona.Parameters.AddWithValue("@email", proveedor.DatosPersona?.Email);
            cmdPersona.Parameters.AddWithValue("@direccion", proveedor.DatosPersona?.Direccion);
            cmdPersona.Parameters.AddWithValue("@idpersona", proveedor.IdPersona);
            cmdPersona.ExecuteNonQuery();

            // Actualizar proveedor
            var updateProveedor = @"UPDATE proveedores 
                                    SET cuit = @cuit,
                                        razon_social = @razon
                                    WHERE idproveedor = @id";
            var cmdProveedor = new MySqlCommand(updateProveedor, conn);
            cmdProveedor.Parameters.AddWithValue("@cuit", proveedor.Cuil);
            cmdProveedor.Parameters.AddWithValue("@razon", proveedor.RazonSocial);
            cmdProveedor.Parameters.AddWithValue("@id", proveedor.IdProveedor);
            cmdProveedor.ExecuteNonQuery();
        }
    }
}