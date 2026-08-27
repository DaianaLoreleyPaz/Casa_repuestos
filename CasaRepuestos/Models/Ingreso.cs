using System;

namespace CasaRepuestos.Models
{
    public enum TipoDispositivo
    {
        SMARTPHONE,
        TABLET,
        NOTEBOOK,
        OTRO
    }

    public class Ingreso
    {
        public int IdIngreso { get; set; }

        // FK opcionales
        public int? IdCliente { get; set; } = null;
        public int? IdMarca { get; set; } = null;
        public int? IdArticulo { get; set; } = null;

        // Info principal
        public DateTime FechaIngreso { get; set; } = DateTime.Now;
        public string Modelo { get; set; } = "";
        public string Falla { get; set; } = "";

        // Tipo de dispositivo 
        public TipoDispositivo TipoDispositivo { get; set; } = TipoDispositivo.SMARTPHONE;

        // Texto con accesorios entregados
        public string AccesoriosEntregados { get; set; } = "";

        public Estado Estado { get; set; } = Estado.RECIBIDO;

        public string Marca { get; set; } = "";
    }
}
