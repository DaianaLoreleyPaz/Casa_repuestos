using System;
using System.Collections.Generic;

namespace CasaRepuestos.Models
{
    public class MovimientoCaja
    {
        public string Descripcion { get; set; }
        public string MetodoPago { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } // "INGRESO" o "EGRESO"
    }

    public class Caja
    {
        public int IdCaja { get; set; }
        public DateTime FechaApertura { get; set; }
        public decimal SaldoInicial { get; set; }
        public DateTime? FechaCierre { get; set; }
        public decimal SaldoFinal { get; set; }
        public string Estado { get; set; } // "ABIERTA" o "CERRADA"
        public int IdEmpleado { get; set; }
    }
}