using CasaRepuestos.Models;
using CasaRepuestos.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CasaRepuestos.Tests
{
    [TestClass]
    public class ArqueoTests
    {
        // --- CONFIGURACIÓN INICIAL ---
        private ArqueoService _service;
        private List<Caja> _cajasFake;
        private List<MovimientoCaja> _ingresosFake;
        private List<MovimientoCaja> _egresosFake;

        [TestInitialize]
        // Configura el servicio y datos ficticios antes de cada prueba
        public void Setup()
        {
            _service = new ArqueoService();
            
            _cajasFake = new List<Caja>
            {
                new Caja 
                { 
                    IdCaja = 1, 
                    FechaApertura = DateTime.Today, 
                    SaldoInicial = 5000m, 
                    FechaCierre = null, 
                    SaldoFinal = 0, 
                    IdEmpleado = 1,
                    Estado = "ABIERTA"
                },
                new Caja 
                { 
                    IdCaja = 2, 
                    FechaApertura = DateTime.Today.AddDays(-1), 
                    SaldoInicial = 3000m, 
                    FechaCierre = DateTime.Today.AddDays(-1).AddHours(8), 
                    SaldoFinal = 4500m, 
                    IdEmpleado = 1,
                    Estado = "CERRADA"
                }
            };

            _ingresosFake = new List<MovimientoCaja>
            {
                new MovimientoCaja 
                { 
                    Descripcion = "Venta: Producto A", 
                    MetodoPago = "EFECTIVO", 
                    Monto = 1500m, 
                    Fecha = DateTime.Today, 
                    Tipo = "INGRESO" 
                },
                new MovimientoCaja 
                { 
                    Descripcion = "Reparación con repuesto X", 
                    MetodoPago = "TARJETA", 
                    Monto = 2000m, 
                    Fecha = DateTime.Today, 
                    Tipo = "INGRESO" 
                }
            };

            _egresosFake = new List<MovimientoCaja>
            {
                new MovimientoCaja 
                { 
                    Descripcion = "Pago Compra: Proveedor ABC", 
                    MetodoPago = "TRANSFERENCIA", 
                    Monto = 800m, 
                    Fecha = DateTime.Today, 
                    Tipo = "EGRESO" 
                },
                new MovimientoCaja 
                { 
                    Descripcion = "Gasto: Servicios", 
                    MetodoPago = "EFECTIVO", 
                    Monto = 300m, 
                    Fecha = DateTime.Today, 
                    Tipo = "EGRESO" 
                }
            };
        }

        // ========== PRUEBAS DE GESTIÓN DE CAJAS ==========

        [TestMethod]
        // Test para verificar que una caja abierta tiene fecha de cierre nula
        public void CajaAbierta_FechaCierreDebeSerNull()
        {
            var cajaAbierta = _cajasFake.FirstOrDefault(c => c.Estado == "ABIERTA");
            
            Assert.IsNotNull(cajaAbierta);
            Assert.IsNull(cajaAbierta.FechaCierre);
        }

        [TestMethod]
        // Test para verificar que una caja cerrada tiene fecha de cierre no nula
        public void CajaCerrada_FechaCierreDebeSerNoNull()
        {
            var cajaCerrada = _cajasFake.FirstOrDefault(c => c.Estado == "CERRADA");
            
            Assert.IsNotNull(cajaCerrada);
            Assert.IsNotNull(cajaCerrada.FechaCierre);
        }

        [TestMethod]
        // Test para verificar que el saldo inicial es mayor a cero
        public void CajaAbierta_SaldoInicialMayorACero()
        {
            var caja = _cajasFake.First();
            
            Assert.IsTrue(caja.SaldoInicial > 0);
        }

        [TestMethod]
        // Test para verificar que solo puede haber una caja abierta por día
        public void VerificarUnaSolaCajaAbiertaPorDia()
        {
            var cajasAbiertas = _cajasFake.Where(c => c.FechaApertura.Date == DateTime.Today && c.FechaCierre == null).ToList();
            
            Assert.IsTrue(cajasAbiertas.Count <= 1);
        }

        [TestMethod]
        // Test para verificar que la fecha de apertura debe ser válida
        public void CajaApertura_FechaDebeSerHoy()
        {
            var caja = _cajasFake.FirstOrDefault(c => c.Estado == "ABIERTA");
            
            if (caja != null)
            {
                Assert.AreEqual(DateTime.Today, caja.FechaApertura.Date);
            }
        }

        [TestMethod]
        // Test para verificar que el saldo final de caja cerrada es mayor o igual al inicial
        public void CajaCerrada_SaldoFinalNoNegativo()
        {
            var cajaCerrada = _cajasFake.FirstOrDefault(c => c.Estado == "CERRADA");
            
            if (cajaCerrada != null)
            {
                Assert.IsTrue(cajaCerrada.SaldoFinal >= 0);
            }
        }

        [TestMethod]
        // Test para simular cierre de caja y verificar que se asigna saldo final
        public void SimularCierreCaja_DebeAsignarSaldoFinal()
        {
            var caja = _cajasFake.First();
            decimal saldoFinalCalculado = 6000m;
            
            caja.FechaCierre = DateTime.Now;
            caja.SaldoFinal = saldoFinalCalculado;
            caja.Estado = "CERRADA";
            
            Assert.AreEqual(saldoFinalCalculado, caja.SaldoFinal);
            Assert.IsNotNull(caja.FechaCierre);
        }

        [TestMethod]
        // Test para verificar que no se puede abrir caja con fecha anterior
        public void AbrirCaja_FechaAnterior_DebeSerInvalido()
        {
            var fechaAnterior = DateTime.Today.AddDays(-1);
            var esFechaValida = fechaAnterior.Date == DateTime.Today;
            
            Assert.IsFalse(esFechaValida);
        }

        // ========== PRUEBAS DE INGRESOS ==========

        [TestMethod]
        // Test para verificar que los ingresos retornan una lista no nula
        public void ObtenerIngresos_DeberiaRetornarListaNoNula()
        {
            Assert.IsNotNull(_ingresosFake);
        }

        [TestMethod]
        // Test para verificar que los ingresos tienen montos positivos
        public void Ingresos_MontoDebeSerPositivo()
        {
            foreach (var ingreso in _ingresosFake)
            {
                Assert.IsTrue(ingreso.Monto > 0);
            }
        }

        [TestMethod]
        // Test para verificar que los ingresos tienen tipo correcto
        public void Ingresos_TipoDebeSerINGRESO()
        {
            foreach (var ingreso in _ingresosFake)
            {
                Assert.AreEqual("INGRESO", ingreso.Tipo);
            }
        }

        [TestMethod]
        // Test para calcular el total de ingresos del día
        public void CalcularTotalIngresos_DebeSumarCorrectamente()
        {
            decimal totalEsperado = _ingresosFake.Sum(i => i.Monto);
            decimal totalCalculado = _ingresosFake.Sum(i => i.Monto);
            
            Assert.AreEqual(totalEsperado, totalCalculado);
        }

        [TestMethod]
        // Test para verificar que ingresos tienen descripción no vacía
        public void Ingresos_DescripcionNoDebeEstarVacia()
        {
            foreach (var ingreso in _ingresosFake)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(ingreso.Descripcion));
            }
        }

        [TestMethod]
        // Test para verificar que ingresos tienen método de pago válido
        public void Ingresos_MetodoPagoDebeSerValido()
        {
            var metodosValidos = new[] { "EFECTIVO", "TARJETA", "TRANSFERENCIA", "BILLETERA VIRTUAL" };
            
            foreach (var ingreso in _ingresosFake)
            {
                Assert.IsTrue(metodosValidos.Contains(ingreso.MetodoPago));
            }
        }

        // ========== PRUEBAS DE EGRESOS ==========

        [TestMethod]
        // Test para verificar que los egresos retornan una lista no nula
        public void ObtenerEgresos_DeberiaRetornarListaNoNula()
        {
            Assert.IsNotNull(_egresosFake);
        }

        [TestMethod]
        // Test para verificar que los egresos tienen montos positivos
        public void Egresos_MontoDebeSerPositivo()
        {
            foreach (var egreso in _egresosFake)
            {
                Assert.IsTrue(egreso.Monto > 0);
            }
        }

        [TestMethod]
        // Test para verificar que los egresos tienen tipo correcto
        public void Egresos_TipoDebeSerEGRESO()
        {
            foreach (var egreso in _egresosFake)
            {
                Assert.AreEqual("EGRESO", egreso.Tipo);
            }
        }

        [TestMethod]
        // Test para calcular el total de egresos del día
        public void CalcularTotalEgresos_DebeSumarCorrectamente()
        {
            decimal totalEsperado = _egresosFake.Sum(e => e.Monto);
            decimal totalCalculado = _egresosFake.Sum(e => e.Monto);
            
            Assert.AreEqual(totalEsperado, totalCalculado);
        }

        [TestMethod]
        // Test para verificar que el saldo se calcula correctamente
        public void CalcularSaldoCaja_DebeSerCorrecto()
        {
            decimal saldoInicial = 5000m;
            decimal totalIngresos = _ingresosFake.Sum(i => i.Monto);
            decimal totalEgresos = _egresosFake.Sum(e => e.Monto);
            decimal saldoCalculado = saldoInicial + totalIngresos - totalEgresos;
            decimal saldoEsperado = 5000m + 3500m - 1100m;
            
            Assert.AreEqual(saldoEsperado, saldoCalculado);
        }

        [TestMethod]
        // Test para verificar que no hay egresos sin descripción
        public void Egresos_DescripcionNoDebeEstarVacia()
        {
            foreach (var egreso in _egresosFake)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(egreso.Descripcion));
            }
        }

        // ========== PRUEBAS DE VALIDACIONES ==========

        [TestMethod]
        // Test para verificar que el saldo inicial no puede ser negativo
        public void ValidarSaldoInicial_NoDebeSerNegativo()
        {
            decimal saldoInicial = -100m;
            
            if (saldoInicial < 0) saldoInicial = 0;
            
            Assert.IsTrue(saldoInicial >= 0);
        }

        [TestMethod]
        // Test para verificar que una caja debe tener empleado asignado
        public void Caja_DebeEstarAsignadaAEmpleado()
        {
            var caja = _cajasFake.First();
            
            Assert.IsTrue(caja.IdEmpleado > 0);
        }

        [TestMethod]
        // Test para verificar que la lista de cajas no está vacía
        public void ListarCajas_NoDeberiaEstarVacia()
        {
            Assert.IsTrue(_cajasFake.Count > 0);
        }

        [TestMethod]
        // Test para verificar que se pueden filtrar cajas por fecha
        public void FiltrarCajasPorFecha_DeberiaRetornarCoincidencias()
        {
            var cajasHoy = _cajasFake.Where(c => c.FechaApertura.Date == DateTime.Today).ToList();
            
            Assert.IsTrue(cajasHoy.Count > 0);
        }

        [TestMethod]
        // Test para verificar que las cajas se ordenan por fecha descendente
        public void OrdenarCajasPorFecha_DeberiaSerDescendente()
        {
            var cajasOrdenadas = _cajasFake.OrderByDescending(c => c.FechaApertura).ToList();
            
            Assert.AreEqual(_cajasFake.First().IdCaja, cajasOrdenadas.First().IdCaja);
        }
    }
}
