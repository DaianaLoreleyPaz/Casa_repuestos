using MySql.Data.MySqlClient;


namespace CasaRepuestos.Services
{
    public class LoginService
    {
        public bool ValidarCredenciales(string usuario, string contrasenia)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasenia))
                return false;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Config.Config.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM empleados WHERE usuario = @usuario AND contrasenia = @contrasenia";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@contrasenia", contrasenia);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ValidarCredenciales: {ex.Message}");
                return false;
            }
        }

        public string ObtenerRol(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return string.Empty;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Config.Config.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT rol FROM empleados WHERE usuario = @usuario";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    var result = cmd.ExecuteScalar();
                    return result?.ToString() ?? string.Empty;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public int ObtenerIdEmpleado(string usuario, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(Config.Config.ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT idempleado FROM empleados WHERE usuario = @usuario AND contrasenia = @contrasenia";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@contrasenia", password);

                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                    return 0; 
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"Error en ObtenerIdEmpleado: {ex.Message}");
                    return 0;
                }
            }
        }
    }
}
