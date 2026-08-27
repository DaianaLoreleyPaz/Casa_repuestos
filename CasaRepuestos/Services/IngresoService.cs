using CasaRepuestos.Models;
using CasaRepuestos.Config;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace CasaRepuestos.Services
{
    public class IngresoService
    {
        public void CrearIngreso(Ingreso ingreso)
        {
            using var connection = new MySqlConnection(Config.Config.ConnectionString);
            connection.Open();

            string query = @"INSERT INTO ingresos 
                (idcliente, idmarca, modelo, fecha_ingreso, falla, 
                 tipo_dispositivo, accesorios_entregados) 
                VALUES 
                (@idcliente, @idmarca, @modelo, @fecha_ingreso, @falla, 
                 @tipo_dispositivo, @accesorios_entregados)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@idcliente", ingreso.IdCliente);
            command.Parameters.AddWithValue("@idmarca", ingreso.IdMarca);
            command.Parameters.AddWithValue("@modelo", ingreso.Modelo ?? string.Empty);
            command.Parameters.AddWithValue("@fecha_ingreso", ingreso.FechaIngreso);
            command.Parameters.AddWithValue("@falla", ingreso.Falla ?? string.Empty);
            command.Parameters.AddWithValue("@tipo_dispositivo", ingreso.TipoDispositivo.ToString());
            command.Parameters.AddWithValue("@accesorios_entregados", ingreso.AccesoriosEntregados ?? string.Empty);

            command.ExecuteNonQuery();
        }

        public void ActualizarIngreso(Ingreso ingreso)
        {
            using var connection = new MySqlConnection(Config.Config.ConnectionString);
            connection.Open();

            string query = @"UPDATE ingresos SET 
                idcliente = @idcliente,
                idmarca = @idmarca,
                modelo = @modelo,
                fecha_ingreso = @fecha_ingreso,
                falla = @falla,
                tipo_dispositivo = @tipo_dispositivo,
                accesorios_entregados = @accesorios_entregados
                WHERE idingreso = @idingreso";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@idingreso", ingreso.IdIngreso);
            command.Parameters.AddWithValue("@idcliente", ingreso.IdCliente);
            command.Parameters.AddWithValue("@idmarca", ingreso.IdMarca);
            command.Parameters.AddWithValue("@modelo", ingreso.Modelo ?? string.Empty);
            command.Parameters.AddWithValue("@fecha_ingreso", ingreso.FechaIngreso);
            command.Parameters.AddWithValue("@falla", ingreso.Falla ?? string.Empty);
            command.Parameters.AddWithValue("@tipo_dispositivo", ingreso.TipoDispositivo.ToString());
            command.Parameters.AddWithValue("@accesorios_entregados", ingreso.AccesoriosEntregados ?? string.Empty);

            command.ExecuteNonQuery();
        }

        public List<Ingreso> ObtenerIngresos()
        {
            var ingresos = new List<Ingreso>();
            using var connection = new MySqlConnection(Config.Config.ConnectionString);
            connection.Open();

      
            string query = @"
        SELECT 
            i.idingreso, i.idcliente, i.idmarca, i.modelo, i.fecha_ingreso, 
            i.falla, i.tipo_dispositivo, i.accesorios_entregados
        FROM ingresos i
        LEFT JOIN presupuestos p ON i.idingreso = p.idingreso
        WHERE 
            p.idingreso IS NULL 
            OR 
            (p.autorizado = 'NO' AND p.estado != 'DEVUELTO_SIN_REPARAR')
        ORDER BY i.fecha_ingreso DESC";

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var ingreso = new Ingreso
                {
                    IdIngreso = reader.GetInt32("idingreso"),
                    IdCliente = reader.GetInt32("idcliente"),
                    IdMarca = reader.GetInt32("idmarca"),
                    Modelo = reader.IsDBNull(reader.GetOrdinal("modelo")) ? string.Empty : reader.GetString("modelo"),
                    FechaIngreso = reader.GetDateTime("fecha_ingreso"),
                    Falla = reader.IsDBNull(reader.GetOrdinal("falla")) ? string.Empty : reader.GetString("falla"),
                    TipoDispositivo = ParseTipoDispositivo(reader.IsDBNull(reader.GetOrdinal("tipo_dispositivo")) ? null : reader.GetString("tipo_dispositivo")),
                    AccesoriosEntregados = reader.IsDBNull(reader.GetOrdinal("accesorios_entregados")) ? string.Empty : reader.GetString("accesorios_entregados"),
                    Estado = Estado.RECIBIDO
                };
                ingresos.Add(ingreso);
            }
            return ingresos;
        }

        public void EliminarIngreso(int idIngreso)
        {
            using var connection = new MySqlConnection(Config.Config.ConnectionString);
            connection.Open();

            string query = "DELETE FROM ingresos WHERE idingreso = @idingreso";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@idingreso", idIngreso);
            command.ExecuteNonQuery();
        }

        public Ingreso? ObtenerIngresoPorId(int idIngreso)
        {
            using var connection = new MySqlConnection(Config.Config.ConnectionString);
            connection.Open();

            string query = @"SELECT idingreso, idcliente, idmarca, modelo, fecha_ingreso, 
                falla, tipo_dispositivo, accesorios_entregados
                FROM ingresos 
                WHERE idingreso = @idingreso";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@idingreso", idIngreso);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Ingreso
                {
                    IdIngreso = reader.GetInt32("idingreso"),
                    IdCliente = reader.GetInt32("idcliente"),
                    IdMarca = reader.GetInt32("idmarca"),
                    Modelo = reader.IsDBNull(reader.GetOrdinal("modelo")) ? string.Empty : reader.GetString("modelo"),
                    FechaIngreso = reader.GetDateTime("fecha_ingreso"),
                    Falla = reader.IsDBNull(reader.GetOrdinal("falla")) ? string.Empty : reader.GetString("falla"),
                    TipoDispositivo = ParseTipoDispositivo(reader.IsDBNull(reader.GetOrdinal("tipo_dispositivo")) ? null : reader.GetString("tipo_dispositivo")),
                    AccesoriosEntregados = reader.IsDBNull(reader.GetOrdinal("accesorios_entregados")) ? string.Empty : reader.GetString("accesorios_entregados"),
                    Estado = Estado.RECIBIDO
                };
            }

            return null;
        }

        // Métodos auxiliares para el parsing seguro de enums
        private static TipoDispositivo ParseTipoDispositivo(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return TipoDispositivo.SMARTPHONE;

            if (Enum.TryParse<TipoDispositivo>(value, true, out var result))
                return result;

            return TipoDispositivo.SMARTPHONE;
        }
    }
}