using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CasaRepuestos.Tests
{
    [TestClass]
    public class ArticuloTests
    {
        private FakeArticuloService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new FakeArticuloService();
        }

        // --- VALIDACIONES BÁSICAS ---
        [TestMethod]
        public void CrearArticulo_SinNombre_DeberiaFallar()
        {
            var articulo = ArticuloValido();
            articulo.Nombre = "";
            Assert.ThrowsException<ArgumentException>(() => _service.CrearArticulo(articulo));
        }

        [TestMethod]
        public void CrearArticulo_PrecioNegativo_DeberiaFallar()
        {
            var articulo = ArticuloValido();
            articulo.Precio = -10;
            Assert.ThrowsException<ArgumentException>(() => _service.CrearArticulo(articulo));
        }

        [TestMethod]
        public void CrearArticulo_StockNegativo_DeberiaFallar()
        {
            var articulo = ArticuloValido();
            articulo.Stock = -1;
            Assert.ThrowsException<ArgumentException>(() => _service.CrearArticulo(articulo));
        }

        [TestMethod]
        public void CrearArticulo_SinProveedor_DeberiaFallar()
        {
            var articulo = ArticuloValido();
            articulo.IdsProveedores.Clear();
            Assert.ThrowsException<ArgumentException>(() => _service.CrearArticulo(articulo));
        }

        // --- LÓGICA DE CREACIÓN ---
        [TestMethod]
        public void CrearArticulo_Valido_DeberiaGuardarEnMemoria()
        {
            var articulo = ArticuloValido();
            var id = _service.CrearArticulo(articulo);
            Assert.AreEqual(1, id);
            Assert.AreEqual(1, _service.Articulos.Count);
        }

        // --- LÓGICA DE MODIFICACIÓN ---
        [TestMethod]
        public void ModificarArticulo_Valido_DeberiaActualizarNombre()
        {
            var articulo = ArticuloValido();
            var id = _service.CrearArticulo(articulo);

            articulo.Nombre = "Pantalla Samsung A52";
            _service.ModificarArticulo(articulo);

            var actualizado = _service.ObtenerArticuloPorId(id);
            Assert.AreEqual("Pantalla Samsung A52", actualizado.Nombre);
        }

        [TestMethod]
        public void ModificarArticulo_SinNombre_DeberiaFallar()
        {
            var articulo = ArticuloValido();
            _service.CrearArticulo(articulo);

            articulo.Nombre = "";
            Assert.ThrowsException<ArgumentException>(() => _service.ModificarArticulo(articulo));
        }

        // --- CÁLCULO DE PRECIO FINAL ---
        [TestMethod]
        public void CalcularPrecioFinal_DeberiaSerCorrecto()
        {
            decimal costo = 100m;
            decimal ganancia = 50m; // 50%
            decimal iva = 21m; // 21%

            decimal precioEsperado = _service.CalcularPrecioFinal(costo, ganancia, iva);
            Assert.AreEqual(181.50m, Math.Round(precioEsperado, 2));
        }

        // --- TEST DE PROVEEDORES ---
        [TestMethod]
        public void CrearArticulo_ConMultiplesProveedores_DeberiaGuardarseCorrectamente()
        {
            var articulo = ArticuloValido();
            articulo.IdsProveedores = new List<int> { 1, 2, 3 };

            _service.CrearArticulo(articulo);

            var guardado = _service.Articulos[0];
            Assert.AreEqual(3, guardado.IdsProveedores.Count);
        }

        // --- OBTENCIÓN ---
        [TestMethod]
        public void ObtenerArticuloPorId_DeberiaDevolverArticuloValido()
        {
            var articulo = ArticuloValido();
            _service.CrearArticulo(articulo);

            var obtenido = _service.ObtenerArticuloPorId(1);
            Assert.IsNotNull(obtenido);
            Assert.AreEqual("Pantalla Samsung A50", obtenido.Nombre);
        }

        // --- MÉTODOS AUXILIARES ---
        private Articulo ArticuloValido() => new Articulo
        {
            IdArticulo = 1,
            Nombre = "Pantalla Samsung A50",
            Precio = 1200,
            Stock = 10,
            IdMarca = 2,
            PorcentajeGanancia = 50,
            IVA = 21,
            IdsProveedores = new List<int> { 1 }
        };

        // --- CLASES FICTICIAS ---
        private class Articulo
        {
            public int IdArticulo { get; set; }
            public string Nombre { get; set; }
            public decimal Precio { get; set; }
            public int Stock { get; set; }
            public int IdMarca { get; set; }
            public decimal PorcentajeGanancia { get; set; }
            public decimal IVA { get; set; }
            public List<int> IdsProveedores { get; set; } = new();
        }

        private class FakeArticuloService
        {
            public List<Articulo> Articulos { get; } = new();

            public int CrearArticulo(Articulo a)
            {
                Validar(a);
                a.IdArticulo = Articulos.Count + 1;
                Articulos.Add(a);
                return a.IdArticulo;
            }

            public void ModificarArticulo(Articulo a)
            {
                Validar(a);
                var existente = Articulos.Find(x => x.IdArticulo == a.IdArticulo);
                if (existente == null)
                    throw new InvalidOperationException("Artículo no encontrado");

                existente.Nombre = a.Nombre;
                existente.Precio = a.Precio;
                existente.Stock = a.Stock;
                existente.IdsProveedores = new List<int>(a.IdsProveedores);
            }

            public Articulo ObtenerArticuloPorId(int id)
            {
                return Articulos.Find(a => a.IdArticulo == id);
            }

            public decimal CalcularPrecioFinal(decimal costo, decimal ganancia, decimal iva)
            {
                return costo * (1 + ganancia / 100) * (1 + iva / 100);
            }

            private void Validar(Articulo a)
            {
                if (string.IsNullOrWhiteSpace(a.Nombre))
                    throw new ArgumentException("El nombre es obligatorio");
                if (a.Precio < 0)
                    throw new ArgumentException("El precio no puede ser negativo");
                if (a.Stock < 0)
                    throw new ArgumentException("El stock no puede ser negativo");
                if (a.IdsProveedores == null || a.IdsProveedores.Count == 0)
                    throw new ArgumentException("Debe seleccionar al menos un proveedor");
            }
        }
    }
}
