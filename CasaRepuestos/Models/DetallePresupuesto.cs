namespace CasaRepuestos.Models
{
    public class DetallePresupuesto
    {
        public int IdDetallePresupuesto { get; set; }
        public int IdPresupuesto { get; set; }
        public int? IdArticulo { get; set; }
        public int Cantidad { get; set; } = 1;

        public int IdServicio { get; set; }
        public decimal? PrecioRepuesto { get; set; }
        public decimal? PrecioServicio { get; set; }

        

        public decimal PrecioCosto { get; set; }
        public decimal PorcentajeGanancia { get; set; }
        public decimal IVA { get; set; }
    }
}
