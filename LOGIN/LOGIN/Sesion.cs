using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIN
{
    public class Usuario
    {
        public string Email { get; set; } = string.Empty;  // Inicialización directa
        public string Rol { get; set; } = "Usuario";      // Valor por defecto
    }

    public static class Sesion
    {
        public static Usuario UsuarioActual { get; set; } = new Usuario();
    }


}
