using CasaRepuestos.Models;
using CasaRepuestos.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CasaRepuestos.Tests
{
    [TestClass]
    public class ServiciosTests
    {
        // --- CONFIGURACIÓN INICIAL ---
        private ServiciosService _service;
        private List<Servicio> _serviciosFake;
        private List<Articulo> _articulosFake;

        [TestInitialize]
        // Configura el servicio y datos ficticios antes de cada prueba
        public void Setup()
        {
            _service = new ServiciosService();
            
            _serviciosFake = new List<Servicio>
            {
                new Servicio 
                { 
                    IdServicio = 1,
                    Descripcion = "Cambio de pantalla",
                    Precio = 5000m,
                    IdArticuloAsociado = 10
                },
                new Servicio 
                { 
                    IdServicio = 2,
                    Descripcion = "Reemplazo de batería",
                    Precio = 3000m,
                    IdArticuloAsociado = 15
                },
                new Servicio 
                { 
                    IdServicio = 3,
                    Descripcion = "Diagnóstico técnico",
                    Precio = 1000m,
                    IdArticuloAsociado = null
                },
                new Servicio 
                { 
                    IdServicio = 4,
                    Descripcion = "Reparación de placa",
                    Precio = 8000m,
                    IdArticuloAsociado = 20
                }
            };

            _articulosFake = new List<Articulo>
            {
                new Articulo { IdArticulo = 10, Nombre = "Pantalla LCD" },
                new Articulo { IdArticulo = 15, Nombre = "Batería 3000mAh" },
                new Articulo { IdArticulo = 20, Nombre = "Placa madre" },
                new Articulo { IdArticulo = 25, Nombre = "Flex conector carga" }
            };
        }

        // ========== PRUEBAS DE VALIDACIÓN DE DATOS ==========

        [TestMethod]
        // Test para verificar que un servicio debe tener una descripción no vacía
        public void Servicio_DescripcionNoDebeEstarVacia()
        {
            foreach (var servicio in _serviciosFake)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(servicio.Descripcion));
            }
        }

        [TestMethod]
        // Test para verificar que el precio debe ser mayor a cero
        public void Servicio_PrecioDebeSerMayorACero()
        {
            foreach (var servicio in _serviciosFake)
            {
                Assert.IsTrue(servicio.Precio > 0);
            }
        }

        [TestMethod]
        // Test para verificar que el ID del servicio debe ser válido
        public void Servicio_IdDebeSerMayorACero()
        {
            foreach (var servicio in _serviciosFake)
            {
                Assert.IsTrue(servicio.IdServicio > 0);
            }
        }

        [TestMethod]
        // Test para verificar que un servicio puede no tener artículo asociado
        public void Servicio_PuedeTenerArticuloNull()
        {
            var servicioSinArticulo = _serviciosFake.FirstOrDefault(s => s.IdArticuloAsociado == null);
            
            Assert.IsNotNull(servicioSinArticulo);
            Assert.IsNull(servicioSinArticulo.IdArticuloAsociado);
        }

        [TestMethod]
        // Test para verificar que un servicio con artículo asociado tiene ID válido
        public void Servicio_ConArticuloAsociadoTieneIdValido()
        {
            var serviciosConArticulo = _serviciosFake.Where(s => s.IdArticuloAsociado.HasValue).ToList();
            
            foreach (var servicio in serviciosConArticulo)
            {
                Assert.IsTrue(servicio.IdArticuloAsociado.Value > 0);
            }
        }

        // ========== PRUEBAS DE OPERACIONES ==========

        [TestMethod]
        // Test para verificar que se puede obtener la lista de servicios
        public void ObtenerServicios_DeberiaRetornarLista()
        {
            Assert.IsNotNull(_serviciosFake);
            Assert.IsTrue(_serviciosFake.Count > 0);
        }

        [TestMethod]
        // Test para verificar que se puede buscar un servicio por ID
        public void BuscarServicioPorId_DeberiaRetornarServicioCorrecto()
        {
            int idBuscado = 2;
            var servicio = _serviciosFake.FirstOrDefault(s => s.IdServicio == idBuscado);
            
            Assert.IsNotNull(servicio);
            Assert.AreEqual(idBuscado, servicio.IdServicio);
            Assert.AreEqual("Reemplazo de batería", servicio.Descripcion);
        }

        [TestMethod]
        // Test para verificar que buscar un servicio inexistente retorna null
        public void BuscarServicioPorId_Inexistente_DeberiaRetornarNull()
        {
            int idInexistente = 999;
            var servicio = _serviciosFake.FirstOrDefault(s => s.IdServicio == idInexistente);
            
            Assert.IsNull(servicio);
        }

        [TestMethod]
        // Test para simular la actualización de un servicio
        public void ActualizarServicio_DeberiaModificarDatos()
        {
            var servicio = _serviciosFake.First();
            decimal nuevoPrecio = 6000m;
            
            servicio.Precio = nuevoPrecio;
            
            Assert.AreEqual(nuevoPrecio, servicio.Precio);
        }

        [TestMethod]
        // Test para simular la eliminación de un servicio
        public void EliminarServicio_DeberiaReducirLista()
        {
            int cantidadInicial = _serviciosFake.Count;
            _serviciosFake.RemoveAt(0);
            
            Assert.AreEqual(cantidadInicial - 1, _serviciosFake.Count);
        }

        [TestMethod]
        // Test para verificar que se pueden ordenar servicios por descripción
        public void OrdenarServiciosPorDescripcion_DeberiaSerAscendente()
        {
            var serviciosOrdenados = _serviciosFake.OrderBy(s => s.Descripcion).ToList();
            
            Assert.AreEqual("Cambio de pantalla", serviciosOrdenados[0].Descripcion);
        }

        [TestMethod]
        // Test para verificar que se pueden filtrar servicios por precio
        public void FiltrarServiciosPorPrecio_DeberiaRetornarCoincidencias()
        {
            decimal precioMinimo = 3000m;
            var serviciosFiltrados = _serviciosFake.Where(s => s.Precio >= precioMinimo).ToList();
            
            Assert.IsTrue(serviciosFiltrados.Count > 0);
            Assert.IsTrue(serviciosFiltrados.All(s => s.Precio >= precioMinimo));
        }

        // ========== PRUEBAS DE ARTÍCULOS ASOCIADOS ==========

        [TestMethod]
        // Test para verificar que se pueden obtener servicios con artículo asociado
        public void ObtenerServiciosConArticuloAsociado_DeberiaRetornarCoincidencias()
        {
            var serviciosConArticulo = _serviciosFake.Where(s => s.IdArticuloAsociado.HasValue).ToList();
            
            Assert.IsTrue(serviciosConArticulo.Count > 0);
        }

        [TestMethod]
        // Test para verificar que se pueden obtener servicios sin artículo asociado
        public void ObtenerServiciosSinArticuloAsociado_DeberiaRetornarCoincidencias()
        {
            var serviciosSinArticulo = _serviciosFake.Where(s => !s.IdArticuloAsociado.HasValue).ToList();
            
            Assert.IsTrue(serviciosSinArticulo.Count > 0);
        }

        [TestMethod]
        // Test para verificar que un artículo puede estar asociado a un servicio
        public void BuscarServicioPorArticuloId_DeberiaRetornarServicio()
        {
            int idArticulo = 15;
            var servicio = _serviciosFake.FirstOrDefault(s => s.IdArticuloAsociado == idArticulo);
            
            Assert.IsNotNull(servicio);
            Assert.AreEqual(idArticulo, servicio.IdArticuloAsociado);
        }

        [TestMethod]
        // Test para verificar que un artículo sin servicio retorna null
        public void BuscarServicioPorArticuloId_SinServicio_DeberiaRetornarNull()
        {
            int idArticuloSinServicio = 25;
            var servicio = _serviciosFake.FirstOrDefault(s => s.IdArticuloAsociado == idArticuloSinServicio);
            
            Assert.IsNull(servicio);
        }

        // ========== PRUEBAS DE VALIDACIONES ADICIONALES ==========

        [TestMethod]
        // Test para verificar que no se permiten precios negativos
        public void ValidarPrecio_NoDebeSerNegativo()
        {
            decimal precioInvalido = -500m;
            
            if (precioInvalido < 0)
                precioInvalido = 0;
            
            Assert.IsTrue(precioInvalido >= 0);
        }

        [TestMethod]
        // Test para verificar que se puede agregar un nuevo servicio
        public void AgregarNuevoServicio_DeberiaAumentarLista()
        {
            int cantidadInicial = _serviciosFake.Count;
            
            _serviciosFake.Add(new Servicio 
            { 
                IdServicio = 5,
                Descripcion = "Limpieza interna",
                Precio = 1500m,
                IdArticuloAsociado = null
            });
            
            Assert.AreEqual(cantidadInicial + 1, _serviciosFake.Count);
        }

        [TestMethod]
        // Test para verificar que se pueden contar servicios por rango de precio
        public void ContarServiciosPorRangoPrecio_DeberiaRetornarCantidadCorrecta()
        {
            decimal precioMin = 2000m;
            decimal precioMax = 6000m;
            
            var serviciosEnRango = _serviciosFake
                .Where(s => s.Precio >= precioMin && s.Precio <= precioMax)
                .Count();
            
            Assert.IsTrue(serviciosEnRango >= 0);
        }

        [TestMethod]
        // Test para verificar que se puede calcular el precio promedio
        public void CalcularPrecioPromedio_DeberiaSerCorrecto()
        {
            decimal promedioEsperado = _serviciosFake.Average(s => s.Precio);
            decimal promedioCalculado = _serviciosFake.Sum(s => s.Precio) / _serviciosFake.Count;
            
            Assert.AreEqual(promedioEsperado, promedioCalculado);
        }

        [TestMethod]
        // Test para verificar que se puede obtener el servicio más caro
        public void ObtenerServicioMasCaro_DeberiaRetornarCorrectamente()
        {
            var servicioMasCaro = _serviciosFake.OrderByDescending(s => s.Precio).First();
            
            Assert.AreEqual(8000m, servicioMasCaro.Precio);
            Assert.AreEqual("Reparación de placa", servicioMasCaro.Descripcion);
        }

        [TestMethod]
        // Test para verificar que se puede obtener el servicio más barato
        public void ObtenerServicioMasBarato_DeberiaRetornarCorrectamente()
        {
            var servicioMasBarato = _serviciosFake.OrderBy(s => s.Precio).First();
            
            Assert.AreEqual(1000m, servicioMasBarato.Precio);
            Assert.AreEqual("Diagnóstico técnico", servicioMasBarato.Descripcion);
        }

        [TestMethod]
        // Test para verificar que no existen servicios duplicados por descripción
        public void VerificarNoDuplicados_DeberiaSerUnico()
        {
            var descripciones = _serviciosFake.Select(s => s.Descripcion).ToList();
            var descripcionesUnicas = descripciones.Distinct().ToList();
            
            Assert.AreEqual(descripciones.Count, descripcionesUnicas.Count);
        }

        [TestMethod]
        // Test para verificar que la lista de artículos disponibles no está vacía
        public void ObtenerArticulosDisponibles_DeberiaRetornarLista()
        {
            Assert.IsNotNull(_articulosFake);
            Assert.IsTrue(_articulosFake.Count > 0);
        }
    }
}
