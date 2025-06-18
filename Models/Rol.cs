using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casa_repuestos.Models
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string TipoRol { get; set; } = string.Empty;
        public string Permisos { get; set; } = string.Empty;
        
    }
}
