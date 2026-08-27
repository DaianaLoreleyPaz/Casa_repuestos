using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using CasaRepuestos.Models;

namespace CasaRepuestos.Services
{
    public class ReportesService
    {
        // Obtener reporte de compras
        public ReporteCompras ObtenerReporteCompras(DateTime fechaDesde, DateTime fechaHasta)
        {
            var reporte = new ReporteCompras
            {
                FechaDesde = fechaDesde,
                FechaHasta = fechaHasta
            };

            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                // Obtener resumen de compras
                string queryResumen = @"
                    SELECT 
                        COUNT(*) as CantidadOperaciones,
                        IFNULL(SUM(total_estimado), 0) as TotalCompras
                    FROM ordenes_compra
                    WHERE DATE(fecha_emision) BETWEEN @fechaDesde AND @fechaHasta";

                using (var cmd = new MySqlCommand(queryResumen, conn))
                {
                    cmd.Parameters.AddWithValue("@fechaDesde", fechaDesde.Date);
                    cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta.Date);

                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        reporte.CantidadOperaciones = reader.GetInt32("CantidadOperaciones");
                        reporte.TotalCompras = reader.GetDecimal("TotalCompras");
                        reporte.PromedioOperacion = reporte.CantidadOperaciones > 0 
                            ? reporte.TotalCompras / reporte.CantidadOperaciones 
                            : 0;
                    }
                }

                // Obtener compras por proveedor (ya que no hay categoría en proveedores)
                string queryCategoria = @"
                    SELECT 
                        p.razon_social as categoria,
                        COUNT(*) as cantidad,
                        IFNULL(SUM(oc.total_estimado), 0) as total
                    FROM ordenes_compra oc
                    INNER JOIN proveedores p ON oc.idproveedor = p.idproveedor
                    WHERE DATE(oc.fecha_emision) BETWEEN @fechaDesde AND @fechaHasta
                    GROUP BY p.razon_social";

                using (var cmd = new MySqlCommand(queryCategoria, conn))
                {
                    cmd.Parameters.AddWithValue("@fechaDesde", fechaDesde.Date);
                    cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta.Date);

                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string categoria = reader.IsDBNull(0) ? "Sin Categoría" : reader.GetString("categoria");
                        int cantidad = reader.GetInt32("cantidad");
                        decimal total = reader.GetDecimal("total");

                        reporte.ComprasPorCategoria[categoria] = total;
                        reporte.OperacionesPorCategoria[categoria] = cantidad;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener reporte de compras: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return reporte;
        }

        // Obtener reporte de ventas e ingresos (facturas de ventas + facturas de servicios)
        public ReporteVentas ObtenerReporteVentas(DateTime fechaDesde, DateTime fechaHasta)
        {
            var reporte = new ReporteVentas
            {
                FechaDesde = fechaDesde,
                FechaHasta = fechaHasta
            };

            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                // Obtener resumen de todas las facturas (ventas directas + servicios facturados)
                string queryResumen = @"
                    SELECT 
                        COUNT(*) as CantidadOperaciones,
                        IFNULL(SUM(total_factura), 0) as TotalVentas
                    FROM facturas
                    WHERE DATE(fecha) BETWEEN @fechaDesde AND @fechaHasta
                    
                    UNION ALL
                    
                    SELECT 
                        COUNT(*) as CantidadOperaciones,
                        IFNULL(SUM(total), 0) as TotalVentas
                    FROM presupuestos
                    WHERE DATE(fecha) BETWEEN @fechaDesde AND @fechaHasta AND estado = 'FINALIZADO'";

                decimal cantidadOperaciones = 0;
                decimal totalVentas = 0;
                
                using (var cmd = new MySqlCommand(queryResumen, conn))
                {
                    cmd.Parameters.AddWithValue("@fechaDesde", fechaDesde.Date);
                    cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta.Date);

                    using var reader = cmd.ExecuteReader();
                    // Leer resultados de facturas
                    if (reader.Read())
                    {
                        cantidadOperaciones += reader.GetInt32("CantidadOperaciones");
                        totalVentas += reader.GetDecimal("TotalVentas");
                    }
                    // Avanzar al siguiente resultado (presupuestos)
                    if (reader.NextResult() && reader.Read())
                    {
                        cantidadOperaciones += reader.GetInt32("CantidadOperaciones");
                        totalVentas += reader.GetDecimal("TotalVentas");
                    }
                }
                
                reporte.CantidadOperaciones = (int)cantidadOperaciones;
                reporte.TotalVentas = totalVentas;
                reporte.PromedioOperacion = reporte.CantidadOperaciones > 0
                    ? reporte.TotalVentas / reporte.CantidadOperaciones
                    : 0;

                // Obtener ventas por método de pago (de facturas y pagos de cuenta corriente, excluyendo presupuestos)
                string queryMetodo = @"
                    SELECT 
                        metodo_pago,
                        COUNT(*) as cantidad,
                        IFNULL(SUM(total_factura), 0) as total
                    FROM facturas
                    WHERE DATE(fecha) BETWEEN @fechaDesde AND @fechaHasta
                    GROUP BY metodo_pago
                    
                    UNION ALL
                    
                    SELECT 
                        metodo_pago,
                        COUNT(*) as cantidad,
                        IFNULL(SUM(monto), 0) as total
                    FROM movimientos_cuentas_corrientes
                    WHERE DATE(fecha_de_pago) BETWEEN @fechaDesde AND @fechaHasta
                    AND concepto IN ('PAGO TOTAL', 'PAGO PARCIAL')
                    AND metodo_pago IS NOT NULL
                    GROUP BY metodo_pago";

                var metodoPagos = new Dictionary<string, (int cantidad, decimal total)>();
                
                using (var cmd = new MySqlCommand(queryMetodo, conn))
                {
                    cmd.Parameters.AddWithValue("@fechaDesde", fechaDesde.Date);
                    cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta.Date);

                    using var reader = cmd.ExecuteReader();
                    // Leer resultados de facturas
                    while (reader.Read())
                    {
                        string metodoPago = reader.GetString("metodo_pago");
                        int cantidad = reader.GetInt32("cantidad");
                        decimal total = reader.GetDecimal("total");

                        if (metodoPagos.ContainsKey(metodoPago))
                        {
                            var existing = metodoPagos[metodoPago];
                            metodoPagos[metodoPago] = (existing.cantidad + cantidad, existing.total + total);
                        }
                        else
                        {
                            metodoPagos[metodoPago] = (cantidad, total);
                        }
                    }
                    // Avanzar al siguiente resultado (pagos de cuenta corriente)
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            string metodoPago = reader.GetString("metodo_pago");
                            int cantidad = reader.GetInt32("cantidad");
                            decimal total = reader.GetDecimal("total");

                            if (metodoPagos.ContainsKey(metodoPago))
                            {
                                var existing = metodoPagos[metodoPago];
                                metodoPagos[metodoPago] = (existing.cantidad + cantidad, existing.total + total);
                            }
                            else
                            {
                                metodoPagos[metodoPago] = (cantidad, total);
                            }
                        }
                    }
                }
                
                // Transferir datos al reporte
                foreach (var kvp in metodoPagos)
                {
                    reporte.VentasPorMetodoPago[kvp.Key] = kvp.Value.total;
                    reporte.OperacionesPorMetodoPago[kvp.Key] = kvp.Value.cantidad;
                }

                // Obtener ventas por categoría de cliente (incluyendo todas las posibles combinaciones)
                string queryTipoCliente = @"
                    SELECT 
                        COALESCE(f.categoria_cliente, c2.categoria, c3.categoria, 'CONSUMIDOR FINAL') as tipo_cliente,
                        COUNT(*) as cantidad,
                        IFNULL(SUM(f.total_factura), 0) as total
                    FROM facturas f
                    LEFT JOIN presupuestos pres ON f.idpresupuesto = pres.idpresupuesto
                    LEFT JOIN ingresos i ON pres.idingreso = i.idingreso
                    LEFT JOIN clientes c2 ON i.idcliente = c2.idcliente
                    LEFT JOIN cuentas_corrientes cc ON f.idcuentacorriente = cc.idcuenta_corriente
                    LEFT JOIN clientes c3 ON cc.idcliente = c3.idcliente
                    WHERE DATE(f.fecha) BETWEEN @fechaDesde AND @fechaHasta
                    GROUP BY COALESCE(f.categoria_cliente, c2.categoria, c3.categoria, 'CONSUMIDOR FINAL')";

                var tipoClientes = new Dictionary<string, (int cantidad, decimal total)>();
                
                using (var cmd = new MySqlCommand(queryTipoCliente, conn))
                {
                    cmd.Parameters.AddWithValue("@fechaDesde", fechaDesde.Date);
                    cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta.Date);

                    using var reader = cmd.ExecuteReader();
                    // Leer resultados de facturas con clientes
                    while (reader.Read())
                    {
                        string tipoCliente = reader.GetString("tipo_cliente");
                        int cantidad = reader.GetInt32("cantidad");
                        decimal total = reader.GetDecimal("total");

                        if (tipoClientes.ContainsKey(tipoCliente))
                        {
                            var existing = tipoClientes[tipoCliente];
                            tipoClientes[tipoCliente] = (existing.cantidad + cantidad, existing.total + total);
                        }
                        else
                        {
                            tipoClientes[tipoCliente] = (cantidad, total);
                        }
                    }
                }
                
                // Transferir datos al reporte
                foreach (var kvp in tipoClientes)
                {
                    reporte.VentasPorTipoCliente[kvp.Key] = kvp.Value.total;
                    reporte.OperacionesPorTipoCliente[kvp.Key] = kvp.Value.cantidad;
                }

                // Asegurar que todas las categorías estén presentes (aunque sean 0)
                string[] todasLasCategorias = { "CONSUMIDOR FINAL", "MONOTRIBUTISTA", "RESPONSABLE INSCRIPTO", "EXCENTO" };
                foreach (var categoria in todasLasCategorias)
                {
                    if (!reporte.VentasPorTipoCliente.ContainsKey(categoria))
                    {
                        reporte.VentasPorTipoCliente[categoria] = 0;
                        reporte.OperacionesPorTipoCliente[categoria] = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener reporte de ventas: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return reporte;
        }

        // Obtener detalles de compras
        public List<DetalleCompraReporte> ObtenerDetalleCompras(DateTime fechaDesde, DateTime fechaHasta)
        {
            var detalles = new List<DetalleCompraReporte>();

            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                string query = @"
                    SELECT 
                        oc.idorden_compra,
                        oc.fecha_emision,
                        p.razon_social as proveedor,
                        oc.total_estimado,
                        p.razon_social as categoria
                    FROM ordenes_compra oc
                    INNER JOIN proveedores p ON oc.idproveedor = p.idproveedor
                    WHERE DATE(oc.fecha_emision) BETWEEN @fechaDesde AND @fechaHasta
                    ORDER BY oc.fecha_emision DESC";

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@fechaDesde", fechaDesde.Date);
                cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta.Date);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    detalles.Add(new DetalleCompraReporte
                    {
                        IdOrdenCompra = reader.GetInt32("idorden_compra"),
                        Fecha = reader.GetDateTime("fecha_emision"),
                        Proveedor = reader.IsDBNull(reader.GetOrdinal("proveedor")) ? "Sin Proveedor" : reader.GetString("proveedor"),
                        Total = reader.GetDecimal("total_estimado"),
                        Categoria = reader.IsDBNull(reader.GetOrdinal("categoria")) ? "Sin Categoría" : reader.GetString("categoria")
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener detalle de compras: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return detalles;
        }

        // Obtener detalles de todas las facturas (ventas directas + servicios)
        public List<DetalleVentaReporte> ObtenerDetalleVentas(DateTime fechaDesde, DateTime fechaHasta)
        {
            var detalles = new List<DetalleVentaReporte>();

            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                // Obtener todas las facturas del período 
                string queryFacturas = @"
                    SELECT 
                        f.idfactura,
                        f.fecha,
                        f.total_factura,
                        f.metodo_pago,
                        f.idpresupuesto,
                        COALESCE(f.categoria_cliente, c2.categoria, c3.categoria, 'CONSUMIDOR FINAL') as tipo_cliente
                    FROM facturas f
                    LEFT JOIN presupuestos pres ON f.idpresupuesto = pres.idpresupuesto
                    LEFT JOIN ingresos i ON pres.idingreso = i.idingreso
                    LEFT JOIN clientes c2 ON i.idcliente = c2.idcliente
                    LEFT JOIN cuentas_corrientes cc ON f.idcuentacorriente = cc.idcuenta_corriente
                    LEFT JOIN clientes c3 ON cc.idcliente = c3.idcliente
                    WHERE DATE(f.fecha) BETWEEN @fechaDesde AND @fechaHasta
                    ORDER BY f.fecha";

                using var cmdFacturas = new MySqlCommand(queryFacturas, conn);
                cmdFacturas.Parameters.AddWithValue("@fechaDesde", fechaDesde.Date);
                cmdFacturas.Parameters.AddWithValue("@fechaHasta", fechaHasta.Date);

                using var readerFacturas = cmdFacturas.ExecuteReader();

                while (readerFacturas.Read())
                {
                    string metodoPago = readerFacturas.IsDBNull(readerFacturas.GetOrdinal("metodo_pago")) ? "Indefinido" : readerFacturas.GetString(readerFacturas.GetOrdinal("metodo_pago"));
                    decimal monto = readerFacturas.GetDecimal(readerFacturas.GetOrdinal("total_factura"));
                    DateTime fechaFactura = readerFacturas.GetDateTime(readerFacturas.GetOrdinal("fecha"));
                    int idFactura = readerFacturas.GetInt32(readerFacturas.GetOrdinal("idfactura"));
                    bool tienePresupuesto = !readerFacturas.IsDBNull(readerFacturas.GetOrdinal("idpresupuesto"));
                    string tipoCliente = readerFacturas.IsDBNull(readerFacturas.GetOrdinal("tipo_cliente")) ? "CONSUMIDOR FINAL" : readerFacturas.GetString(readerFacturas.GetOrdinal("tipo_cliente"));

                    // Obtener descripción completa de la factura 
                    string descripcion = ObtenerDescripcionCompletaFactura(conn, idFactura, tienePresupuesto);

                    detalles.Add(new DetalleVentaReporte
                    {
                        IdFactura = idFactura,
                        Fecha = fechaFactura,
                        Cliente = descripcion,
                        Total = monto,
                        MetodoPago = metodoPago,
                        TipoCliente = tipoCliente
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener detalles de ventas: {ex.Message}", ex);
            }

            return detalles;
        }

        // Método auxiliar para obtener la descripción completa de la factura 
        private string ObtenerDescripcionCompletaFactura(MySqlConnection conn, int idFactura, bool tienePresupuesto)
        {
            try
            {
                if (tienePresupuesto)
                {
                    return ObtenerDescripcionReparacionConProductos(conn, idFactura);
                }
                else
                {
                    return ObtenerDescripcionVentaDirecta(conn, idFactura);
                }
            }
            catch (Exception)
            {
                return tienePresupuesto ? "Reparación con productos" : "Venta de productos";
            }
        }

        // Obtener descripción para REPARACIONES (puede incluir productos de reparación y productos vendidos)
        private string ObtenerDescripcionReparacionConProductos(MySqlConnection conn, int idFactura)
        {
            var productosReparacion = new List<string>();
            var productosVendidos = new List<string>();

            // Obtener productos de la REPARACIÓN (del presupuesto)
            string queryReparacion = @"
                SELECT DISTINCT a.nombre
                FROM detalles_presupuestos dp 
                INNER JOIN articulos a ON dp.idarticulo = a.idarticulo 
                INNER JOIN presupuestos p ON dp.idpresupuesto = p.idpresupuesto
                INNER JOIN facturas f ON p.idpresupuesto = f.idpresupuesto
                WHERE f.idfactura = @idFactura
                AND dp.idarticulo IS NOT NULL";

            using (var cmd = new MySqlCommand(queryReparacion, conn))
            {
                cmd.Parameters.AddWithValue("@idFactura", idFactura);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? "Producto sin nombre" : reader.GetString(reader.GetOrdinal("nombre"));
                    productosReparacion.Add(nombre);
                }
            }

            // Obtener productos VENDIDOS ADICIONALES (del detalle_factura)
            string queryProductosVendidos = @"
                SELECT DISTINCT a.nombre
                FROM detalle_factura df 
                INNER JOIN articulos a ON df.idarticulo = a.idarticulo 
                WHERE df.idfactura = @idFactura";

            using (var cmd = new MySqlCommand(queryProductosVendidos, conn))
            {
                cmd.Parameters.AddWithValue("@idFactura", idFactura);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? "Producto sin nombre" : reader.GetString(reader.GetOrdinal("nombre"));
                    productosVendidos.Add(nombre);
                }
            }

            //Construir la descripción completa
            string descripcion = "Reparación";

            if (productosReparacion.Count > 0)
            {
                descripcion += $" con {string.Join(", ", productosReparacion)}";
            }
            else
            {
                descripcion += " (servicio técnico)";
            }

            if (productosVendidos.Count > 0)
            {
                descripcion += $" + venta: {string.Join(", ", productosVendidos)}";
            }

            return descripcion;
        }

        // Obtener descripción para VENTAS DIRECTAS
        private string ObtenerDescripcionVentaDirecta(MySqlConnection conn, int idFactura)
        {
            var productos = new List<string>();

            string queryVenta = @"
                SELECT DISTINCT a.nombre
                FROM detalle_factura df 
                INNER JOIN articulos a ON df.idarticulo = a.idarticulo 
                WHERE df.idfactura = @idFactura";

            using var cmd = new MySqlCommand(queryVenta, conn);
            cmd.Parameters.AddWithValue("@idFactura", idFactura);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? "Producto sin nombre" : reader.GetString(reader.GetOrdinal("nombre"));
                productos.Add(nombre);
            }

            return productos.Count > 0
                ? $"Venta: {string.Join(", ", productos)}"
                : "Venta: Productos varios";
        }
    }
}
