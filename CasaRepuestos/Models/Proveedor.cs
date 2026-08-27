using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaRepuestos.Models
{
    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public string Cuil { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public int IdPersona { get; set; }
        public Persona? DatosPersona { get; set; }

        public string NombreCompleto => RazonSocial;
        public string NumeroDocumento => DatosPersona?.NumeroDocumento ?? "";
        public string Telefono => DatosPersona?.Telefono ?? "";
    }
}