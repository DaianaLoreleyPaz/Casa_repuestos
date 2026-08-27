using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaRepuestos.Models
{
    public class OrdenCompra
    {
        public int IdOrdenCompra { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdProveedor { get; set; }
        public string Estado { get; set; }
        public decimal TotalEstimado { get; set; }
        public int? IdPresupuesto { get; set; }

        public string Tipo { get; set; } // "REPUESTO" o "STOCK"

        // Propiedades para visualización
        public string ProveedorNombre { get; set; }
        public List<DetalleOrdenCompra> Detalles { get; set; } = new List<DetalleOrdenCompra>();
    }
}
