using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CasaRepuestos.Config;
using CasaRepuestos.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;


namespace CasaRepuestos.Services
{
    public class EmpleadoService
    {
        

        public List<Empleado> ListarEmpleados()
        {
            var lista = new List<Empleado>();
            using (var conn = new MySqlConnection(Config.Config.ConnectionString))
            {
                conn.Open();
                var query = @"SELECT e.idempleado, e.rol, e.usuario, e.contrasenia, e.idpersona,
                                     p.nombre, p.apellido, p.tipo_documento, p.numero_documento,
                                     p.telefono, p.email, p.direccion
                              FROM empleados e
                              INNER JOIN personas p ON e.idpersona = p.idpersona";
                var cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var empleado = new Empleado
                    {
                        IdEmpleado = reader.GetInt32("idempleado"),
                        Rol = reader.GetString("rol"),
                        Usuario = reader.GetString("usuario"),
                        Contrasenia = reader.GetString("contrasenia"),
                        IdPersona = reader.GetInt32("idpersona"),
                        DatosPersona = new Persona
                        {
                            IdPersona = reader.GetInt32("idpersona"),
                            Nombre = reader.GetString("nombre"),
                            Apellido = reader.GetString("apellido"),
                            TipoDocumento = reader.GetString("tipo_documento"),
                            NumeroDocumento = reader.GetString("numero_documento"),
                            Telefono = reader.GetString("telefono"),
                            Email = reader.GetString("email"),
                            Direccion = reader.GetString("direccion")
                        }
                    };
                    lista.Add(empleado);
                }
            }
            return lista;
        }
        public void CrearEmpleado(Empleado emp)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            if (emp.DatosPersona == null)
                throw new Exception("Los datos de la persona son obligatorios.");
            if (string.IsNullOrWhiteSpace(emp.Usuario))
                throw new Exception("El usuario es obligatorio.");
            if (string.IsNullOrWhiteSpace(emp.Contrasenia))
                throw new Exception("La contraseña es obligatoria.");
            if (string.IsNullOrWhiteSpace(emp.DatosPersona.Nombre))
                throw new Exception("El nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(emp.Rol))
                throw new Exception("El rol es obligatorio.");
            if (string.IsNullOrWhiteSpace(emp.DatosPersona.Apellido))
                throw new Exception("El apellido es obligatorio.");
            if (string.IsNullOrWhiteSpace(emp.DatosPersona?.Direccion))
                throw new Exception("La dirección es obligatoria.");

            if (string.IsNullOrWhiteSpace(emp.DatosPersona?.Telefono))
                throw new Exception("El teléfono es obligatorio.");

            if (!emp.DatosPersona.NumeroDocumento.All(char.IsDigit))
                throw new Exception("El documento solo puede contener números.");

            var existentes = ListarEmpleados();
            if (existentes.Any(e => e.Usuario == emp.Usuario))
                throw new Exception("Ya existe un empleado con ese usuario.");
            try
            {
                var mail = new System.Net.Mail.MailAddress(emp.DatosPersona.Email);
            }
            catch
            {
                throw new Exception("El email no tiene un formato válido.");
            }

            //  Insertar persona
            var insertPersona = @"INSERT INTO personas (nombre, apellido, tipo_documento, numero_documento, telefono, email, direccion)
                                  VALUES (@nombre, @apellido, @tipo_doc, @num_doc, @tel, @mail, @dir);
                                  SELECT LAST_INSERT_ID();";
            var cmdPersona = new MySqlCommand(insertPersona, conn);
            cmdPersona.Parameters.AddWithValue("@nombre", emp.DatosPersona?.Nombre);
            cmdPersona.Parameters.AddWithValue("@apellido", emp.DatosPersona?.Apellido);
            cmdPersona.Parameters.AddWithValue("@tipo_doc", emp.DatosPersona?.TipoDocumento);
            cmdPersona.Parameters.AddWithValue("@num_doc", emp.DatosPersona?.NumeroDocumento);
            cmdPersona.Parameters.AddWithValue("@tel", emp.DatosPersona?.Telefono);
            cmdPersona.Parameters.AddWithValue("@mail", emp.DatosPersona?.Email);
            cmdPersona.Parameters.AddWithValue("@dir", emp.DatosPersona?.Direccion);

            var idPersona = Convert.ToInt32(cmdPersona.ExecuteScalar());

            //  Insertar empleado
            var insertEmpleado = @"INSERT INTO empleados (rol, usuario, contrasenia, idpersona)
                                   VALUES (@rol, @usuario, @contrasena, @idPersona)";
            var cmdEmp = new MySqlCommand(insertEmpleado, conn);
            cmdEmp.Parameters.AddWithValue("@rol", emp.Rol);
            cmdEmp.Parameters.AddWithValue("@usuario", emp.Usuario);
            cmdEmp.Parameters.AddWithValue("@contrasena", emp.Contrasenia);
            cmdEmp.Parameters.AddWithValue("@idPersona", idPersona);
            cmdEmp.ExecuteNonQuery();
        }
        public void ModificarEmpleado(Empleado emp)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            var query = @"UPDATE empleados SET 
                    usuario = @usuario,
                    contrasenia = @contrasenia,
                    rol = @rol
                  WHERE idempleado = @id";

            var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@rol", emp.Rol);
            cmd.Parameters.AddWithValue("@usuario", emp.Usuario);
            cmd.Parameters.AddWithValue("@contrasenia", emp.Contrasenia);
            cmd.Parameters.AddWithValue("@id", emp.IdEmpleado);
            cmd.ExecuteNonQuery();
        }
        
        public void EliminarEmpleado(int id)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();
            var query = "DELETE FROM empleados WHERE idempleado = @id";
            var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}