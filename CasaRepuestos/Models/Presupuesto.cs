namespace CasaRepuestos.Models
{
    public class Presupuesto
    {
        public int IdPresupuesto { get; set; }
        public DateTime? Fecha { get; set; } 
        public decimal Total { get; set; }
        public string Autorizado { get; set; } = "NO";

        
        public string Estado { get; set; } = "VERIFICAR_PRECIO";

        public int IdEmpleado { get; set; }
        public int IdIngreso { get; set; }
        public string Modelo { get; set; }
        public string Falla { get; set; }


        public DateTime? FechaVencimiento { get; set; }
        public DateTime? FechaRetiro { get; set; }
        public string ClienteNombre { get; set; }
    }
}
