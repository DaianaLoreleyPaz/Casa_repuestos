using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CasaRepuestos.Models;

namespace CasaRepuestos.Tests
{
    [TestClass]
    public class MarcaTests
    {
        private FakeMarcaService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new FakeMarcaService();
        }

        // --------------------------------------------------------------------
        // Crear marca nueva correctamente
        // --------------------------------------------------------------------
        [TestMethod]
        public void CrearMarca_DeberiaAgregarMarcaCorrectamente()
        {
            _service.CrearMarca(new Marca { Nombre = "Samsung" });
            Assert.IsTrue(_service.ListarMarcas().Any(m => m.Nombre == "Samsung"));
        }

        // --------------------------------------------------------------------
        // No permitir duplicados
        // --------------------------------------------------------------------
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CrearMarca_Duplicada_DeberiaLanzarExcepcion()
        {
            _service.CrearMarca(new Marca { Nombre = "Apple" });
            _service.CrearMarca(new Marca { Nombre = "Apple" });
        }

        // --------------------------------------------------------------------
        // Modificar marca existente
        // --------------------------------------------------------------------
        [TestMethod]
        public void ModificarMarca_DeberiaActualizarNombre()
        {
            _service.CrearMarca(new Marca { Nombre = "Motorola" });
            var marca = _service.ListarMarcas().First(m => m.Nombre == "Motorola");
            marca.Nombre = "Motorola Plus";
            _service.ModificarMarca(marca);
            Assert.AreEqual("Motorola Plus", _service.ListarMarcas().First().Nombre);
        }

        // --------------------------------------------------------------------
        // Eliminar marca existente
        // --------------------------------------------------------------------
        [TestMethod]
        public void EliminarMarca_DeberiaRemoverMarca()
        {
            _service.CrearMarca(new Marca { Nombre = "Xiaomi" });
            var id = _service.ListarMarcas().First().IdMarca;
            _service.EliminarMarca(id);
            Assert.AreEqual(0, _service.ListarMarcas().Count);
        }

        // --------------------------------------------------------------------
        // Eliminar marca inexistente lanza excepción
        // --------------------------------------------------------------------
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EliminarMarca_Inexistente_DeberiaLanzarExcepcion()
        {
            _service.EliminarMarca(999);
        }

        // --------------------------------------------------------------------
        //  Lista vacía al inicio
        // --------------------------------------------------------------------
        [TestMethod]
        public void ListarMarcas_SinDatos_DeberiaRetornarListaVacia()
        {
            Assert.AreEqual(0, _service.ListarMarcas().Count);
        }

        // --------------------------------------------------------------------
        // Crear varias marcas y validar conteo
        // --------------------------------------------------------------------
        [TestMethod]
        public void CrearVariasMarcas_DeberiaAumentarElConteo()
        {
            _service.CrearMarca(new Marca { Nombre = "Sony" });
            _service.CrearMarca(new Marca { Nombre = "LG" });
            _service.CrearMarca(new Marca { Nombre = "Philips" });
            Assert.AreEqual(3, _service.ListarMarcas().Count);
        }

        // --------------------------------------------------------------------
        //  Modificar marca inexistente lanza error
        // --------------------------------------------------------------------
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ModificarMarca_Inexistente_DeberiaLanzarExcepcion()
        {
            _service.ModificarMarca(new Marca { IdMarca = 999, Nombre = "Nada" });
        }

        // --------------------------------------------------------------------
        //  Crear marca con nombre vacío lanza error
        // --------------------------------------------------------------------
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CrearMarca_SinNombre_DeberiaLanzarExcepcion()
        {
            _service.CrearMarca(new Marca { Nombre = "" });
        }

        // --------------------------------------------------------------------
        //Buscar marca por ID
        // --------------------------------------------------------------------
        [TestMethod]
        public void BuscarMarcaPorId_DeberiaRetornarMarcaCorrecta()
        {
            _service.CrearMarca(new Marca { Nombre = "Nokia" });
            var marca = _service.ListarMarcas().First();
            var encontrada = _service.ListarMarcas().FirstOrDefault(m => m.IdMarca == marca.IdMarca);
            Assert.AreEqual("Nokia", encontrada.Nombre);
        }

        // ====================================================================
        // SERVICIO FAKE (en memoria, sin base de datos)
        // ====================================================================
        private class FakeMarcaService
        {
            private readonly List<Marca> _marcas = new();
            private int _nextId = 1;

            public List<Marca> ListarMarcas()
            {
                return _marcas.ToList();
            }

            public void CrearMarca(Marca marca)
            {
                if (marca == null)
                    throw new Exception("Marca inválida");
                if (string.IsNullOrWhiteSpace(marca.Nombre))
                    throw new Exception("El nombre es obligatorio");
                if (_marcas.Any(m => m.Nombre.Equals(marca.Nombre, StringComparison.OrdinalIgnoreCase)))
                    throw new Exception("Ya existe una marca con ese nombre");

                marca.IdMarca = _nextId++;
                _marcas.Add(marca);
            }

            public void ModificarMarca(Marca marca)
            {
                var existente = _marcas.FirstOrDefault(m => m.IdMarca == marca.IdMarca);
                if (existente == null)
                    throw new Exception("Marca no encontrada");

                if (_marcas.Any(m => m.Nombre.Equals(marca.Nombre, StringComparison.OrdinalIgnoreCase) && m.IdMarca != marca.IdMarca))
                    throw new Exception("Ya existe otra marca con ese nombre");

                existente.Nombre = marca.Nombre;
            }

            public void EliminarMarca(int id)
            {
                var existente = _marcas.FirstOrDefault(m => m.IdMarca == id);
                if (existente == null)
                    throw new Exception("Marca no encontrada");
                _marcas.Remove(existente);
            }

            public void Limpiar()
            {
                _marcas.Clear();
                _nextId = 1;
            }
        }
    }
}
