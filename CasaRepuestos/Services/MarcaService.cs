using CasaRepuestos.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace CasaRepuestos.Services
{
    public class MarcaService
    {
        public List<Marca> ListarMarcas()
        {
            var lista = new List<Marca>();
            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                var query = @"SELECT m.idmarca, m.nombre FROM marcas m WHERE m.activo = 1";
                var cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Marca
                    {
                        IdMarca = reader.GetInt32("idmarca"),
                        Nombre = reader.GetString("nombre")
                    });
                }
            }
            catch (MySqlException mysqlEx)
            {
                throw new Exception($"Error de base de datos: {mysqlEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar marcas: {ex.Message}");
            }
            finally
            {
                conn?.Close();
            }

            return lista;
        }

        public void CrearMarca(Marca marca)
        {
            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                // Validar si ya existe una marca con el mismo nombre
                var validarQuery = "SELECT COUNT(*) FROM marcas WHERE nombre = @nombre AND activo = 1";
                using var validarCmd = new MySqlCommand(validarQuery, conn);
                validarCmd.Parameters.AddWithValue("@nombre", marca.Nombre);

                int existe = Convert.ToInt32(validarCmd.ExecuteScalar());
                if (existe > 0)
                {
                    throw new Exception("Ya existe una marca con ese nombre");
                }

                var insertMarca = @"INSERT INTO marcas (nombre) VALUES (@nombre)";
                using var cmd = new MySqlCommand(insertMarca, conn);
                cmd.Parameters.AddWithValue("@nombre", marca.Nombre);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear marca: " + ex.Message);
            }
        }

        public void ModificarMarca(Marca marca)
        {
            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                // Validar si ya existe otra marca con el mismo nombre
                var validarQuery = "SELECT COUNT(*) FROM marcas WHERE nombre = @nombre AND idmarca != @id AND activo = 1";
                using var validarCmd = new MySqlCommand(validarQuery, conn);
                validarCmd.Parameters.AddWithValue("@nombre", marca.Nombre);
                validarCmd.Parameters.AddWithValue("@id", marca.IdMarca);

                int existe = Convert.ToInt32(validarCmd.ExecuteScalar());
                if (existe > 0)
                {
                    throw new Exception("Ya existe otra marca con ese nombre");
                }

                var update = @"UPDATE marcas SET nombre = @nombre WHERE idmarca = @idmarca";
                using var cmd = new MySqlCommand(update, conn);
                cmd.Parameters.AddWithValue("@nombre", marca.Nombre);
                cmd.Parameters.AddWithValue("@idmarca", marca.IdMarca);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar marca: " + ex.Message);
            }
        }

        public void EliminarMarca(int idMarca)
        {
            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                // Verificar si la marca está siendo usada en productos
                var verificarUso = "SELECT COUNT(*) FROM articulos WHERE idmarca = @id AND activo = 1";
                using var verificarCmd = new MySqlCommand(verificarUso, conn);
                verificarCmd.Parameters.AddWithValue("@id", idMarca);

                int enUso = Convert.ToInt32(verificarCmd.ExecuteScalar());
                if (enUso > 0)
                {
                    throw new Exception("No se puede eliminar la marca porque está siendo utilizada en artículos");
                }

                // Eliminación lógica 
                var delete = "UPDATE marcas SET activo = 0 WHERE idmarca = @id";
                using var cmd = new MySqlCommand(delete, conn);
                cmd.Parameters.AddWithValue("@id", idMarca);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar marca: " + ex.Message);
            }
        }

        public Marca? ObtenerMarcaPorId(int id)
        {
            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                var query = "SELECT idmarca, nombre FROM marcas WHERE idmarca = @id AND activo = 1";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Marca
                    {
                        IdMarca = reader.GetInt32("idmarca"),
                        Nombre = reader.GetString("nombre")
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener marca: " + ex.Message);
            }
        }
    }
}