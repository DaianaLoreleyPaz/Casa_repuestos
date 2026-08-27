namespace CasaRepuestos.Models
{
    public class Articulo
    {
        public int IdArticulo { get; set; }
        public string Nombre { get; set; } = "";

        public decimal Precio { get; set; } = 0.0m;
        public decimal PorcentajeGanancia { get; set; } = 0.0m;
        public decimal IVA { get; set; } = 0.0m;
        public decimal PrecioCosto { get; set; } = 0.0m;
        public int Stock { get; set; } = 0;
        public Boolean activo { get; set; } = true;
        public int IdMarca { get; set; }
        public int IdProveedor { get; set; }
        public List<ArticuloProveedor> ProveedoresConCosto { get; set; } = new List<ArticuloProveedor>();



        // Para mostrar los nombres concatenados en la grilla de artículos
        public string NombresProveedores { get; set; }
        public string NombreYPrecio
        {
            get { return $"{Nombre} - {Precio:C2}"; }
        }
        // Para facilitar el acceso a la marca y proveedor asociados
        public Marca Marca { get; set; }
        public Proveedor Proveedor { get; set; }
        public string ProveedoresResumen { get; set; }
        public int StockComprometido { get; set; }
        public string NombreMarca => Marca?.Nombre ?? "";
        public string RazonSocialProveedor => Proveedor?.RazonSocial ?? "";
    }
}
