using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casa_repuestos.Models
{
    public class Usuario
    {    
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        //public int IdEmpleado { get; set; } 
        public int IdRol { get; set; }
        public string Email { get; internal set; } = string.Empty;
    }

}

