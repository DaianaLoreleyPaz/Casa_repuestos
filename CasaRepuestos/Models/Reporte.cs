using System;
using System.Collections.Generic;

namespace CasaRepuestos.Models
{
    public class ReporteCompras
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public decimal TotalCompras { get; set; }
        public int CantidadOperaciones { get; set; }
        public decimal PromedioOperacion { get; set; }
        public Dictionary<string, decimal> ComprasPorCategoria { get; set; }
        public Dictionary<string, int> OperacionesPorCategoria { get; set; }

        public ReporteCompras()
        {
            ComprasPorCategoria = new Dictionary<string, decimal>();
            OperacionesPorCategoria = new Dictionary<string, int>();
        }
    }

    public class ReporteVentas
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public decimal TotalVentas { get; set; }
        public int CantidadOperaciones { get; set; }
        public decimal PromedioOperacion { get; set; }
        public Dictionary<string, decimal> VentasPorMetodoPago { get; set; }
        public Dictionary<string, int> OperacionesPorMetodoPago { get; set; }
        public Dictionary<string, decimal> VentasPorTipoCliente { get; set; }
        public Dictionary<string, int> OperacionesPorTipoCliente { get; set; }

        public ReporteVentas()
        {
            VentasPorMetodoPago = new Dictionary<string, decimal>();
            OperacionesPorMetodoPago = new Dictionary<string, int>();
            VentasPorTipoCliente = new Dictionary<string, decimal>();
            OperacionesPorTipoCliente = new Dictionary<string, int>();
        }
    }

    public class DetalleCompraReporte
    {
        public int IdOrdenCompra { get; set; }
        public DateTime Fecha { get; set; }
        public string Proveedor { get; set; }
        public decimal Total { get; set; }
        public string Categoria { get; set; }
    }

    public class DetalleVentaReporte
    {
        public int IdFactura { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; }
        public string TipoCliente { get; set; }
    }
}
