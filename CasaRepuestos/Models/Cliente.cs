using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaRepuestos.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public string Cuil { get; set; } = string.Empty;
        public int IdPersona { get; set; }
        public Persona? DatosPersona { get; set; }
        public CuentaCorriente? DatosCuentaCorriente { get; set; }

        public string NombreCompleto => $"{DatosPersona?.Nombre} {DatosPersona?.Apellido}";
        public string NumeroDocumento => DatosPersona?.NumeroDocumento ?? "";
        public string Telefono => DatosPersona?.Telefono ?? "";
    }
}
