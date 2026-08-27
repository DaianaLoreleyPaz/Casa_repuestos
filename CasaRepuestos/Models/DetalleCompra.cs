using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaRepuestos.Models
{
    public class DetalleCompra
    {
        public int IdDetalleCompra { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int IdCompra { get; set; }
        public int IdArticulo { get; set; }

        public Articulo? Articulo { get; set; }
    }

}
