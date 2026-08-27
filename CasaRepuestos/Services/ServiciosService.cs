using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using CasaRepuestos.Models;

namespace CasaRepuestos.Services
{
    public class ServiciosService
    {
        private readonly string _connectionString;

        public ServiciosService()
        {
            _connectionString = CasaRepuestos.Config.Config.ConnectionString;
        }

        public List<Articulo> ObtenerTodosArticulos()
        {
            var articulos = new List<Articulo>();
            try
            {
                using (var connection = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = @"
                SELECT 
                    a.idarticulo, a.nombre, a.codigo 
                FROM articulos a
                LEFT JOIN servicios s ON a.idarticulo = s.idarticulo
                WHERE s.idarticulo IS NULL 
                ORDER BY a.nombre";

                    using (var command = new MySql.Data.MySqlClient.MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            articulos.Add(new Articulo
                            {
                                IdArticulo = reader.GetInt32("idarticulo"),
                                Nombre = reader.GetString("nombre") 
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error al obtener artículos sin servicio asociado.", ex);
            }
            return articulos;
        }
        public List<Servicio> ObtenerTodosServicios()
        {
            var servicios = new List<Servicio>();

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = @$"SELECT idservicio, descripcion_servicio, precio, idarticulo 
                           FROM servicios 
                           ORDER BY descripcion_servicio";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            servicios.Add(new Servicio
                            {
                                IdServicio = reader.GetInt32("idservicio"),
                                Descripcion = reader.GetString("descripcion_servicio"),
                                Precio = reader.GetDecimal("precio"),
                                IdArticuloAsociado = reader.IsDBNull(reader.GetOrdinal("idarticulo"))
                                    ? (int?)null
                                    : reader.GetInt32("idarticulo")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener servicios: {ex.Message}");
            }

            return servicios;
        }

        public Servicio ObtenerServicioPorId(int idServicio)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = @"SELECT idservicio, descripcion_servicio, precio
                                 FROM servicios 
                                 WHERE idservicio = @IdServicio";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdServicio", idServicio);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Servicio
                                {
                                    IdServicio = reader.GetInt32("idservicio"),
                                    Descripcion = reader.GetString("descripcion_servicio"),
                                    Precio = reader.GetDecimal("precio")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener servicio: {ex.Message}");
            }

            return null;
        }

        public void CrearServicio(Servicio servicio)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"INSERT INTO servicios (descripcion_servicio, precio, idarticulo)
                      VALUES (@descripcion, @precio, @idarticulo);"; 

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@descripcion", servicio.Descripcion);
                    command.Parameters.AddWithValue("@precio", servicio.Precio);
                    command.Parameters.AddWithValue("@idarticulo", (object?)servicio.IdArticuloAsociado ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }


        public void ActualizarServicio(Servicio servicio)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"UPDATE servicios SET 
                        descripcion_servicio = @descripcion, 
                        precio = @precio, 
                        idarticulo = @idarticulo    
                      WHERE idservicio = @idservicio;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idservicio", servicio.IdServicio);
                    command.Parameters.AddWithValue("@descripcion", servicio.Descripcion);
                    command.Parameters.AddWithValue("@precio", servicio.Precio);
                    command.Parameters.AddWithValue("@idarticulo", (object?)servicio.IdArticuloAsociado ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool EliminarServicio(int idServicio)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    
                    var query = @"DELETE FROM servicios WHERE idservicio = @IdServicio";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdServicio", idServicio);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar servicio: {ex.Message}");
            }
        }

        public bool ExisteServicio(string descripcion, int? idExcluir = null)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = @"SELECT COUNT(*) 
                         FROM servicios 
                         WHERE descripcion_servicio = @Descripcion";

                    if (idExcluir.HasValue)
                    {
                        query += " AND idservicio != @IdExcluir";
                    }

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Descripcion", descripcion);

                        if (idExcluir.HasValue)
                        {
                            command.Parameters.AddWithValue("@IdExcluir", idExcluir.Value);
                        }

                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar existencia: {ex.Message}");
            }
        }

        public Servicio? GetServicioPorArticuloId(int idArticulo)
        {
            Servicio? servicio = null;

            string query = @"
                SELECT idservicio, descripcion_servicio, precio, idarticulo
                FROM servicios
                WHERE idarticulo = @idArticulo
                LIMIT 1;";

            using (var conn = new MySqlConnection(_connectionString))
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idArticulo", idArticulo);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            servicio = new Servicio
                            {
                                IdServicio = reader.GetInt32("idservicio"),
                                Descripcion = reader.GetString("descripcion_servicio"),
                                Precio = reader.GetDecimal("precio"),
                                IdArticuloAsociado = reader.IsDBNull(reader.GetOrdinal("idarticulo"))
                                    ? (int?)null
                                    : reader.GetInt32("idarticulo")
                            };
                        }
                    }
                }
            }
            return servicio;

        }
        public bool ExisteServicioParaArticulo(int idArticulo, int? idServicioExcluir = null)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                string query = @"SELECT COUNT(*) 
                         FROM servicios 
                         WHERE idarticulo = @idArticulo";

                if (idServicioExcluir.HasValue)
                {
                    query += " AND idservicio != @idServicioExcluir";
                }

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idArticulo", idArticulo);
                    if (idServicioExcluir.HasValue)
                        cmd.Parameters.AddWithValue("@idServicioExcluir", idServicioExcluir.Value);

                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }
    }
}