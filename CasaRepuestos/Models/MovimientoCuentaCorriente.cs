using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaRepuestos.Models
{
    public class MovimientoCuentaCorriente
    {
        public int Id_Factura { get; set; }
        public int Id_Cuenta_Corriente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public String Metodo_Pago { get; set; }
        public String? Descripcion_Metodo_Pago { get; set; }
        public String Concepto { get; set; }

    }
}
