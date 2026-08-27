using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaRepuestos.Models
{
    public class ArticuloFaltante
    {
        public int IdArticulo { get; set; }
        public string NombreArticulo { get; set; }
        public int CantidadFaltante { get; set; }
        public decimal PrecioUnitarioSugerido { get; set; }

        public int IdProveedor { get; set; }
        public string ProveedorNombre { get; set; }
       
        public override string ToString() => $"{NombreArticulo} - Faltan: {CantidadFaltante} uds. (Precio Sugerido: {PrecioUnitarioSugerido:N2})";
    }

}
