using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Casa_repuestos.Models;
using MySql.Data.MySqlClient;//se importa para crear la conexion con Mysql 





namespace Casa_repuestos.Services


{
   
    public class UsuarioService
    {
        private string connectionString = "server=localhost;database=casa_repuestos_db;user=DPaz;password=1234;";

        public List<Rol> ObtenerRoles()
        {
            var roles = new List<Rol>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT id_rol, tipo_rol, permisos FROM roles", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var rol = new Rol
                    {
                        IdRol = reader.GetInt32(0), // id_rol
                        TipoRol = !reader.IsDBNull(1) ? reader.GetString(1) : string.Empty, // tipo_rol
                        Permisos = !reader.IsDBNull(2) ? reader.GetString(2) : string.Empty // permisos
                    };
                    roles.Add(rol);
                }
            }
            return roles;
        }
        public void ActualizarPermisosRol(int idRol, string permisos)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new MySqlCommand("UPDATE roles SET permisos = @permisos WHERE id_rol = @idRol", conn);
                    cmd.Parameters.AddWithValue("@permisos", permisos);
                    cmd.Parameters.AddWithValue("@idRol", idRol);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar permisos del rol:\n" + ex.Message);
            }
        }

        public void CrearUsuario(Usuario usuario)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new MySqlCommand(@"INSERT INTO usuarios (nombre_usuario, contraseña, id_rol, email)
                                         VALUES (@nombre, @contrasena, @idRol, @email)", conn);
                    cmd.Parameters.AddWithValue("@nombre", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@contrasena", usuario.Contrasena);
                    cmd.Parameters.AddWithValue("@idRol", usuario.IdRol);
                    cmd.Parameters.AddWithValue("@email", usuario.Email);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el usuario:\n" + ex.Message);
            }
        }

        public void ModificarUsuario(Usuario usuario)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new MySqlCommand(@"UPDATE usuarios 
                                         SET nombre_usuario = @nombre, 
                                             contraseña = @contrasena,
                                             email = @email,
                                             id_rol = @idRol 
                                         WHERE id_usuario = @id", conn);
                    cmd.Parameters.AddWithValue("@nombre", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@contrasena", usuario.Contrasena);
                    cmd.Parameters.AddWithValue("@email", usuario.Email);
                    cmd.Parameters.AddWithValue("@idRol", usuario.IdRol);
                    cmd.Parameters.AddWithValue("@id", usuario.IdUsuario);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar usuario:\n" + ex.Message);
            }
        }
        public List<Usuario> ListarUsuarios()
        {
            var usuarios = new List<Usuario>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM usuarios", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    usuarios.Add(new Usuario
                    {
                        IdUsuario = reader.GetInt32("id_usuario"),
                        NombreUsuario = reader.GetString("nombre_usuario"),
                        Contrasena = reader.GetString("contraseña"),
                        //IdEmpleado = reader.GetInt32("id_empleado"),
                        IdRol = reader.GetInt32("id_rol"),
                        Email = reader.GetString("email")
                    });
                }
            }
            return usuarios;
        }
    }
    
}
