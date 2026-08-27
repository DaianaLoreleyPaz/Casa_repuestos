using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaRepuestos.Models
{
    public class CuentaCorriente
    {
        public int IdCuentaCorriente { get; set; }
        public Decimal SaldoActual { get; set; }

        public int idcliente { get; set; }
       
    }
}
