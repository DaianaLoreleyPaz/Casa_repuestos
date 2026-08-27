using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaRepuestos.Models
{
    public class DetalleOrdenCompra
    {
        public int IdDetalleOrdenCompra { get; set; }
        public int IdOrdenCompra { get; set; }
        public int IdArticulo { get; set; }
        public int IdProveedor { get; set; }
        public string ProveedorNombre { get; set; }
        public int CantidadSolicitada { get; set; }
        public decimal PrecioUnitarioEstimado { get; set; }

        public string TipoIntencion {  get; set; }
        public decimal Subtotal
        {
            get { return CantidadSolicitada * PrecioUnitarioEstimado; }
        }

        // Propiedades para visualización
        public string ArticuloNombre { get; set; }
    }
}
