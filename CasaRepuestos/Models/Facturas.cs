using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaRepuestos.Models
{
    public class Facturas
    {
        public int ID_Factura { get; set; }
        public int? ID_Presupuesto { get; set; }
        public String Metodo_Pago { get; set; }
        public String Descripcion_Metodo_Pago { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total_Factura { get; set; }
        public String Tipo_Factura { get; set; }
        public String? Categoria_Cliente { get; set; }
        public int? ID_Cuenta_Corriente { get; set; }
        public String estado { get; set; }
        public decimal SaldoPendiente { get; set; }


    }
}