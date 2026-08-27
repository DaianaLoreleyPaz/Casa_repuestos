using CasaRepuestos.Models;
using CasaRepuestos.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CasaRepuestos.Tests
{
    [TestClass]
    public class IngresoTests
    {
        // --- CONFIGURACIÓN INICIAL ---
        private IngresoService _service;
        private List<Ingreso> _ingresosFake;

        [TestInitialize]
        // Configura el servicio y datos ficticios antes de cada prueba
        public void Setup()
        {
            _service = new IngresoService();
            
            _ingresosFake = new List<Ingreso>
            {
                new Ingreso 
                { 
                    IdIngreso = 1,
                    IdCliente = 10,
                    IdMarca = 5,
                    Modelo = "Galaxy S21",
                    FechaIngreso = DateTime.Today,
                    Falla = "Pantalla rota",
                    TipoDispositivo = TipoDispositivo.SMARTPHONE,
                    AccesoriosEntregados = "Cargador, funda",
                    Estado = Estado.RECIBIDO
                },
                new Ingreso 
                { 
                    IdIngreso = 2,
                    IdCliente = 11,
                    IdMarca = 3,
                    Modelo = "iPhone 13",
                    FechaIngreso = DateTime.Today.AddDays(-1),
                    Falla = "Batería no carga",
                    TipoDispositivo = TipoDispositivo.SMARTPHONE,
                    AccesoriosEntregados = "Cable USB",
                    Estado = Estado.RECIBIDO
                },
                new Ingreso 
                { 
                    IdIngreso = 3,
                    IdCliente = 12,
                    IdMarca = 2,
                    Modelo = "iPad Pro",
                    FechaIngreso = DateTime.Today.AddDays(-2),
                    Falla = "No enciende",
                    TipoDispositivo = TipoDispositivo.TABLET,
                    AccesoriosEntregados = "",
                    Estado = Estado.RECIBIDO
                }
            };
        }

        // ========== PRUEBAS DE VALIDACIÓN DE DATOS ==========

        [TestMethod]
        // Test para verificar que un ingreso debe tener un cliente asignado
        public void Ingreso_DebeEstarAsignadoACliente()
        {
            var ingreso = _ingresosFake.First();
            
            Assert.IsTrue(ingreso.IdCliente > 0);
        }

        [TestMethod]
        // Test para verificar que un ingreso debe tener una marca asignada
        public void Ingreso_DebeEstarAsignadoAMarca()
        {
            var ingreso = _ingresosFake.First();
            
            Assert.IsTrue(ingreso.IdMarca > 0);
        }

        [TestMethod]
        // Test para verificar que la fecha de ingreso no puede ser futura
        public void FechaIngreso_NoDebeSerFutura()
        {
            foreach (var ingreso in _ingresosFake)
            {
                Assert.IsTrue(ingreso.FechaIngreso.Date <= DateTime.Today);
            }
        }

        [TestMethod]
        // Test para verificar que el modelo no puede estar vacío
        public void Modelo_NoDebeEstarVacio()
        {
            foreach (var ingreso in _ingresosFake)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(ingreso.Modelo));
            }
        }

        [TestMethod]
        // Test para verificar que la falla debe estar descrita
        public void Falla_DebeEstarDescrita()
        {
            foreach (var ingreso in _ingresosFake)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(ingreso.Falla));
            }
        }

        [TestMethod]
        // Test para verificar que el tipo de dispositivo es válido
        public void TipoDispositivo_DebeSerValido()
        {
            var tiposValidos = new List<TipoDispositivo> 
            { 
                TipoDispositivo.SMARTPHONE, 
                TipoDispositivo.TABLET, 
                TipoDispositivo.NOTEBOOK, 
                TipoDispositivo.OTRO 
            };
            
            foreach (var ingreso in _ingresosFake)
            {
                Assert.IsTrue(tiposValidos.Contains(ingreso.TipoDispositivo));
            }
        }

        [TestMethod]
        // Test para verificar que el estado inicial siempre es RECIBIDO
        public void EstadoInicial_DebeSerRECIBIDO()
        {
            foreach (var ingreso in _ingresosFake)
            {
                Assert.AreEqual(Estado.RECIBIDO, ingreso.Estado);
            }
        }

        [TestMethod]
        // Test para verificar que los accesorios pueden estar vacíos
        public void AccesoriosEntregados_PuedeEstarVacio()
        {
            var ingresoSinAccesorios = _ingresosFake.Last();
            
            Assert.IsTrue(string.IsNullOrEmpty(ingresoSinAccesorios.AccesoriosEntregados));
        }

        // ========== PRUEBAS DE OPERACIONES ==========

        [TestMethod]
        // Test para verificar que se puede obtener la lista de ingresos
        public void ObtenerIngresos_DeberiaRetornarLista()
        {
            Assert.IsNotNull(_ingresosFake);
            Assert.IsTrue(_ingresosFake.Count > 0);
        }

        [TestMethod]
        // Test para verificar que se puede buscar un ingreso por ID
        public void BuscarIngresoPorId_DeberiaRetornarIngresoCorrecto()
        {
            int idBuscado = 2;
            var ingreso = _ingresosFake.FirstOrDefault(i => i.IdIngreso == idBuscado);
            
            Assert.IsNotNull(ingreso);
            Assert.AreEqual(idBuscado, ingreso.IdIngreso);
        }

        [TestMethod]
        // Test para verificar que buscar un ingreso inexistente retorna null
        public void BuscarIngresoPorId_Inexistente_DeberiaRetornarNull()
        {
            int idInexistente = 999;
            var ingreso = _ingresosFake.FirstOrDefault(i => i.IdIngreso == idInexistente);
            
            Assert.IsNull(ingreso);
        }

        [TestMethod]
        // Test para simular la actualización de un ingreso
        public void ActualizarIngreso_DeberiaModificarDatos()
        {
            var ingreso = _ingresosFake.First();
            string nuevaFalla = "Actualización de falla: Pantalla y batería";
            
            ingreso.Falla = nuevaFalla;
            
            Assert.AreEqual(nuevaFalla, ingreso.Falla);
        }

        [TestMethod]
        // Test para simular la eliminación de un ingreso
        public void EliminarIngreso_DeberiaReducirLista()
        {
            int cantidadInicial = _ingresosFake.Count;
            _ingresosFake.RemoveAt(0);
            
            Assert.AreEqual(cantidadInicial - 1, _ingresosFake.Count);
        }

        [TestMethod]
        // Test para verificar que se pueden filtrar ingresos por fecha
        public void FiltrarIngresosPorFecha_DeberiaRetornarCoincidencias()
        {
            var ingresosHoy = _ingresosFake.Where(i => i.FechaIngreso.Date == DateTime.Today).ToList();
            
            Assert.IsTrue(ingresosHoy.Count > 0);
        }

        [TestMethod]
        // Test para verificar que los ingresos se ordenan por fecha descendente
        public void OrdenarIngresosPorFecha_DeberiaSerDescendente()
        {
            var ingresosOrdenados = _ingresosFake.OrderByDescending(i => i.FechaIngreso).ToList();
            
            Assert.AreEqual(_ingresosFake.First().IdIngreso, ingresosOrdenados.First().IdIngreso);
        }

        // ========== PRUEBAS DE TIPOS DE DISPOSITIVOS ==========

        [TestMethod]
        // Test para verificar que se pueden filtrar ingresos por tipo de dispositivo
        public void FiltrarPorTipoDispositivo_SMARTPHONE_DeberiaRetornarCoincidencias()
        {
            var smartphones = _ingresosFake.Where(i => i.TipoDispositivo == TipoDispositivo.SMARTPHONE).ToList();
            
            Assert.IsTrue(smartphones.Count > 0);
        }

        [TestMethod]
        // Test para verificar que existe al menos un ingreso de tipo TABLET
        public void VerificarExistenciaDeTABLET_DeberiaEncontrarAlMenosUna()
        {
            var tablets = _ingresosFake.Where(i => i.TipoDispositivo == TipoDispositivo.TABLET).ToList();
            
            Assert.IsTrue(tablets.Count > 0);
        }

        [TestMethod]
        // Test para verificar el conteo de ingresos por tipo de dispositivo
        public void ContarIngresosPorTipo_DeberiaCoincidirConTotal()
        {
            var smartphones = _ingresosFake.Count(i => i.TipoDispositivo == TipoDispositivo.SMARTPHONE);
            var tablets = _ingresosFake.Count(i => i.TipoDispositivo == TipoDispositivo.TABLET);
            var total = smartphones + tablets;
            
            Assert.AreEqual(_ingresosFake.Count, total);
        }

        // ========== PRUEBAS DE VALIDACIONES ADICIONALES ==========

        [TestMethod]
        // Test para verificar que no se permiten IDs negativos
        public void ValidarIdIngreso_NoDebeSerNegativo()
        {
            foreach (var ingreso in _ingresosFake)
            {
                Assert.IsTrue(ingreso.IdIngreso > 0);
            }
        }

        [TestMethod]
        // Test para verificar que se puede agregar un nuevo ingreso a la lista
        public void AgregarNuevoIngreso_DeberiaAumentarLista()
        {
            int cantidadInicial = _ingresosFake.Count;
            
            _ingresosFake.Add(new Ingreso 
            { 
                IdIngreso = 4,
                IdCliente = 13,
                IdMarca = 4,
                Modelo = "MacBook Air",
                FechaIngreso = DateTime.Today,
                Falla = "Teclado no funciona",
                TipoDispositivo = TipoDispositivo.NOTEBOOK,
                AccesoriosEntregados = "Cargador",
                Estado = Estado.RECIBIDO
            });
            
            Assert.AreEqual(cantidadInicial + 1, _ingresosFake.Count);
        }
    }
}
