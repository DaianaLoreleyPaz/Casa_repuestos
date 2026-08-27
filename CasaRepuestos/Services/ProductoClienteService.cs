using System.Collections.Generic;
using MySql.Data.MySqlClient;
using CasaRepuestos.Models;

namespace CasaRepuestos.Services
{
	public class ProductoClienteService
	{
		public List<ProductoCliente> ObtenerProductosClientes()
		{
			var productos = new List<ProductoCliente>();

			using (var conn = new MySqlConnection(Config.Config.ConnectionString))
			{
				string query = "SELECT * FROM productos_clientes";
				using (var cmd = new MySqlCommand(query, conn))
				{
					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							productos.Add(new ProductoCliente
							{
								IdProductoCliente = Convert.ToInt32(reader["idproducto_cliente"]),
								Nombre = reader["nombre"].ToString(),
								IdMarca = Convert.ToInt32(reader["idmarca"]),
								CondicionEquipo = reader["condicion_equipo"].ToString()
							});
						}
					}
				}
			}
			return productos;
		}
	}
}