using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaRepuestos.Models
{
    public class ArticuloProveedor
    {
        public int IdArticulo { get; set; }
        public int IdProveedor { get; set; }
        public decimal PrecioCoste { get; set; }
    }
}
