using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaRepuestos.Models
{
    public class Empleado
    {
        public int IdEmpleado { get; set; }
        public string Rol { get; set; } = string.Empty; // ADMINISTRADOR, CAJERO, TECNICO
        public string Usuario { get; set; } = string.Empty;
        public string Contrasenia { get; set; } = string.Empty;
        public int IdPersona { get; set; }

        // Datos extendidos de Persona
        public Persona? DatosPersona { get; set; }
        public string NombreCompleto => $"{DatosPersona?.Nombre} {DatosPersona?.Apellido}";

    }
}
