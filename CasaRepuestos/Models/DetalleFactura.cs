using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaRepuestos.Models
{
    public class DetalleFactura
    {
        public int ID_Detalle_Factura { get; set; } 
        public int ID_Factura { get; set; } 
        public int? ID_Articulo { get; set; } 
        public int Cantidad { get; set; }
        public decimal subtotal { get; set; }

        // Campo auxiliares 
        public string? NombreArticulo { get; set; }
        public decimal? PrecioUnitario { get; set; }


    }
}
