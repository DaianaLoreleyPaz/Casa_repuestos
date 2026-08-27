using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaRepuestos.Models
{
    public class Marca
    {
        public int IdMarca { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; } = true; 

    }
}
