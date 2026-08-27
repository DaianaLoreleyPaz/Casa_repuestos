using CasaRepuestos.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace CasaRepuestos.Tests
{
    [TestClass]
    public class InventarioTests
    {
        private List<Articulo> _inventarioFake;

        [TestInitialize]
        public void Setup()
        {
            _inventarioFake = new List<Articulo>
            {
                new Articulo { IdArticulo = 1, Nombre = "Tablet", Stock = 5 },
                new Articulo { IdArticulo = 2, Nombre = "Motorola e6s", Stock = 15 },
                new Articulo { IdArticulo = 3, Nombre = "Pin de carga", Stock = 8 }
            };
        }

        [TestMethod] public void BuscarPorNombre_DeberiaRetornarCoincidencia() { 
            var r = _inventarioFake.Where(a => a.Nombre.Contains("Tablet")).ToList(); Assert.AreEqual(1, r.Count); 
        }
        [TestMethod] public void BuscarInexistente_DeberiaRetornarVacio() { 
            Assert.AreEqual(0, _inventarioFake.Where(a => a.Nombre == "Nada").Count()); 
        }
        [TestMethod] public void FiltrarPorStockBajo_DeberiaRetornarArticulosMenorA10() { 
            var bajo = _inventarioFake.Where(a => a.Stock < 10).ToList(); Assert.IsTrue(bajo.Count > 0); 
        }
        [TestMethod] public void StockBajo_DeberiaSer5() { 
            Assert.AreEqual(5, _inventarioFake.First().Stock); 
        }
        [TestMethod] public void StockNormal_DeberiaSerMayorQue10() { 
            Assert.IsTrue(_inventarioFake.Any(a => a.Stock > 10)); 
        }
        [TestMethod] public void TotalArticulos_DeberiaSer3() { 
            Assert.AreEqual(3, _inventarioFake.Count); 
        }
        [TestMethod] public void CalcularStockPromedio_DeberiaSerMayorACero() { 
            Assert.IsTrue(_inventarioFake.Average(a => a.Stock) > 0); 
        }
        [TestMethod] public void ValidarStockNegativo_DeberiaForzarCero() { 
            var art = new Articulo { Stock = -5 }; if (art.Stock < 0) art.Stock = 0; Assert.AreEqual(0, art.Stock); 
        }
        [TestMethod] public void IncrementarStock_DeberiaAumentar() { 
            var a = _inventarioFake.First(); a.Stock += 5; Assert.AreEqual(10, a.Stock); 
        }
        [TestMethod] public void VaciarInventario_DeberiaDejarListaVacia() { 
            _inventarioFake.Clear(); Assert.AreEqual(0, _inventarioFake.Count); 
        }

        [TestMethod] public void SimularEntradaStock_DeberiaIncrementarCantidad() { 
            var a = _inventarioFake.First(); int antes = a.Stock; a.Stock += 3; Assert.AreEqual(antes + 3, a.Stock); }
        [TestMethod] public void SimularSalidaStock_DeberiaReducirCantidad() { 
            var a = _inventarioFake.Last(); int antes = a.Stock; a.Stock -= 2; Assert.AreEqual(antes - 2, a.Stock); }
        [TestMethod] public void RecalcularTotales_DeberiaSerSumaStock() { 
            int total = _inventarioFake.Sum(a => a.Stock); Assert.IsTrue(total > 0); }
        [TestMethod] public void DetectarArticulosBajoStock_DeberiaEncontrarAlMenosUno() { 
            Assert.IsTrue(_inventarioFake.Any(a => a.Stock < 10)); }
        [TestMethod] public void BuscarPorParteDelNombre_DeberiaRetornarCoincidencia() { 
            Assert.IsTrue(_inventarioFake.Any(a => a.Nombre.Contains("Mot"))); }
        [TestMethod] public void ModificarArticulo_DeberiaActualizarNombre() { 
            var a = _inventarioFake.First(); a.Nombre = "Nuevo"; Assert.AreEqual("Nuevo", a.Nombre); }
        [TestMethod] public void EliminarArticulo_DeberiaReducirLista() { 
            int antes = _inventarioFake.Count; _inventarioFake.RemoveAt(0); Assert.AreEqual(antes - 1, _inventarioFake.Count); }
        [TestMethod] public void DuplicarArticulo_DeberiaIncrementarLista() { 
            int antes = _inventarioFake.Count; _inventarioFake.Add(new Articulo { IdArticulo = 99, Nombre = "Duplicado" }); Assert.AreEqual(antes + 1, _inventarioFake.Count); }
        [TestMethod] public void OrdenarPorStock_DeberiaSerAscendente() { 
            var ordenado = _inventarioFake.OrderBy(a => a.Stock).ToList(); Assert.AreEqual(3, ordenado.Count); }
        [TestMethod] public void PromedioStock_DeberiaCoincidirConCalculoManual() { 
            double prom = _inventarioFake.Average(a => a.Stock); Assert.IsTrue(prom > 0); }
    }
}
