using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CasaRepuestos.Models;


namespace CasaRepuestos.Tests
{
    [TestClass]
    [DoNotParallelize]
    public class ComprasTests
    {
        private FakeComprasService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new FakeComprasService();
        }

        // --- Test para CrearCompra ---
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CrearCompra_SinProveedor_DeberiaLanzarExcepcion()
        {
            var compra = CompraValida();
            compra.IdProveedor = 0;
            _service.RegistrarIngresoDesdeOrden(compra);
        }
        // Test para CrearCompra con varios escenarios de error
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CrearCompra_SinDetalles_DeberiaLanzarExcepcion()
        {
            var compra = CompraValida();
            compra.Detalles = new List<DetalleCompra>();
            _service.RegistrarIngresoDesdeOrden(compra);
        }

        // --- Test para CrearCompra con Total negativo ---

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CrearCompra_TotalNegativo_DeberiaLanzarExcepcion()
        {
            var compra = CompraValida();
            compra.Total = -200;
            _service.RegistrarIngresoDesdeOrden(compra);
        }

        // --- Test para CrearCompra sin Fecha ---
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CrearCompra_SinFecha_DeberiaLanzarExcepcion()
        {
            var compra = CompraValida();
            compra.Fecha = default;
            _service.RegistrarIngresoDesdeOrden(compra);
        }

        // --- Test para CrearCompra con Tipo inválido ---

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CrearCompra_TipoInvalido_DeberiaLanzarExcepcion()
        {
            var compra = CompraValida();
            compra.Tipo = "";
            _service.RegistrarIngresoDesdeOrden(compra);
        }

        // --- Test para CrearCompra con Detalle con Cantidad negativa ---
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CrearCompra_Detalle_CantidadNegativa_DeberiaLanzarExcepcion()
        {
            var compra = CompraValida();
            compra.Detalles[0].Cantidad = -5;
            _service.RegistrarIngresoDesdeOrden(compra);
        }

        // --- Test para CrearCompra con Detalle con Precio negativo ---

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CrearCompra_Detalle_PrecioNegativo_DeberiaLanzarExcepcion()
        {
            var compra = CompraValida();
            compra.Detalles[0].PrecioUnitario = -1;
            _service.RegistrarIngresoDesdeOrden(compra);
        }

        // --- Test para CrearCompra con Detalle sin Artículo ---

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CrearCompra_Detalle_SinArticulo_DeberiaLanzarExcepcion()
        {
            var compra = CompraValida();
            compra.Detalles[0].IdArticulo = 0;
            _service.RegistrarIngresoDesdeOrden(compra);
        }

        // --- Test para CrearCompra exitoso y verificar aumento de stock ---
        [TestMethod]
        public void CrearCompra_StockDebeAumentar_TrasRegistrar()
        {
            var compra = CompraValida();
            
            var idCompra = _service.RegistrarIngresoDesdeOrden(compra);
            Assert.IsTrue(idCompra > 0);
            Assert.AreEqual(1, _service.GetStockArticulo(13));
        }

        // --- Test para CrearOrdenDeCompra ---
        [TestMethod]
        public void CrearOrdenCompra_ProveedorYArticuloValido_DebeCrearCorrectamente()
        {
            var orden = OrdenValida();
            var id = _service.CrearOrdenDeCompra(orden);
            Assert.IsTrue(id > 0);
        }

        // -- Test para ObtenerOrdenConDetalles ---
        [TestMethod]
        public void ObtenerOrdenConDetalles_DebeTraerDetalles()
        {
            int idOrdenExistente = _service.CrearOrdenDeCompra(OrdenValida());
            var orden = _service.GetOrdenConDetalles(idOrdenExistente);
            Assert.IsNotNull(orden);
            Assert.IsTrue(orden.Detalles.Count > 0);
        }

        // -- Test para ListarOrdenesPendientes ---
        [TestMethod]
        public void ListarOrdenesPendientes_NoDebeEstarVacio()
        {
            _service.CrearOrdenDeCompra(OrdenValida());
            var lista = _service.ListarOrdenesPendientes();
            Assert.IsNotNull(lista);
            Assert.AreEqual(1, lista.Count);
        }

        // --- Test para RegistrarIngresoDesdeOrden ---
        [TestMethod]
        public void RegistrarIngresoDesdeOrden_DebeActualizarStockYEstado()
        {
            
            int idOrden = _service.CrearOrdenDeCompra(OrdenValida());
       
            var compra = CompraDesdeOrdenValida();
            compra.IdOrdenCompra = idOrden;

            var idCompra = _service.RegistrarIngresoDesdeOrden(compra);

           
            Assert.IsTrue(idCompra > 0);
        
            Assert.AreEqual(2, _service.GetStockArticulo(12));
          
            var orden = _service.GetOrdenConDetalles(idOrden);
            Assert.AreEqual("RECIBIDA", orden.Estado);
        }

       
        

        // --- Test para ListarHistorialCompras ---
        [TestMethod]
        public void ListarHistorialCompras_Todos_DeberiaRetornarLista()
        {
            _service.RegistrarIngresoDesdeOrden(CompraValida());
            var lista = _service.ListarHistorialCompras();
            Assert.IsNotNull(lista);
            Assert.AreEqual(1, lista.Count);
        }

        // --- Test para ListarHistorialCompras filtrado por tipo ---
        [TestMethod]
        public void ListarHistorialCompras_FiltradoPorTipo()
        {
            _service.RegistrarIngresoDesdeOrden(CompraValida()); 
            var lista = _service.ListarHistorialCompras("VENTA"); 
            Assert.IsNotNull(lista);
            Assert.AreEqual(0, lista.Count); 

            var lista2 = _service.ListarHistorialCompras("REPUESTO");
            Assert.AreEqual(1, lista2.Count); 
        }

        // --- Test para ListarHistorialCompras filtrado por fechas ---

        [TestMethod]
        public void ListarHistorialCompras_FiltradoPorFechas()
        {
            _service.RegistrarIngresoDesdeOrden(CompraValida()); 
            var desde = DateTime.Today.AddMonths(-1);
            var hasta = DateTime.Today;
            var lista = _service.ListarHistorialCompras("REPUESTO", desde, hasta);
            Assert.IsNotNull(lista);
            Assert.AreEqual(1, lista.Count); 
        }

        // --- Test para ObtenerNecesidadesRepuestosConsolidadas ---
        [TestMethod]
        public void ObtenerNecesidadesRepuestosConsolidadas_NoDebeSerNula()
        {
            var lista = _service.ObtenerNecesidadesRepuestosConsolidadas();
            Assert.IsNotNull(lista);
        }

        // --- Test para CrearOrdenDeCompra sin detalles ---
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CrearOrdenCompra_SinDetalles_DeberiaLanzarExcepcion()
        {
            var orden = OrdenValida();
            orden.Detalles.Clear();
            _service.CrearOrdenDeCompra(orden);
        }

        // --- Test para CrearOrdenDeCompra sin proveedor ---
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CrearOrdenCompra_SinProveedor_DeberiaLanzarExcepcion()
        {
            var orden = OrdenValida();
            orden.IdProveedor = 0;
            _service.CrearOrdenDeCompra(orden);
        }
       

        // --- Test para ObtenerOrdenConDetalles de orden inexistente ---
        [TestMethod]
        public void GetOrdenConDetalles_OrdenInexistente_DeberiaRetornarNull()
        {
            var orden = _service.GetOrdenConDetalles(-999);
            Assert.IsNull(orden);
        }

        

        // --- MÉTODOS AUXILIARES ---
        private Compra CompraValida() => new Compra
        {
            Fecha = DateTime.Now,
            Total = 1450000,
            IdProveedor = 2,
            Tipo = "REPUESTO",
            IdOrdenCompra = 12, 
            Detalles = new List<DetalleCompra>
            {
                new DetalleCompra { IdArticulo = 13, Cantidad = 1, PrecioUnitario = 1450000 }
            }
        };

        // --- Método auxiliar para crear una orden de compra válida ---
        private OrdenCompra OrdenValida() => new OrdenCompra
        {
            FechaCreacion = DateTime.Now,
            IdProveedor = 3,
            Tipo = "VENTA",
            TotalEstimado = 50000,
            Detalles = new List<DetalleOrdenCompra>
            {
                new DetalleOrdenCompra { IdArticulo = 3, CantidadSolicitada = 2, PrecioUnitarioEstimado = 25000 }
            }
        };

        // --- Método auxiliar para crear una compra desde orden válida ---
        private Compra CompraDesdeOrdenValida() => new Compra
        {
            Fecha = DateTime.Now,
            Total = 112000,
            IdProveedor = 1,
            Tipo = "REPUESTO",
         
            Detalles = new List<DetalleCompra>
            {
                new DetalleCompra { IdArticulo = 12, Cantidad = 2, PrecioUnitario = 56000 }
            }
        };
    }

    // --- Servicio Falso para Compras ---
    public class FakeComprasService
    {
     
        private readonly List<Compra> _compras = new();
        private readonly List<OrdenCompra> _ordenes = new();
        private readonly Dictionary<int, int> _stock = new();
        private int _nextCompraId = 1;
        private int _nextOrdenId = 1;

        // --- Método para obtener el stock de un artículo ---
        public int GetStockArticulo(int idArticulo)
        {
            _stock.TryGetValue(idArticulo, out int stock);
            return stock;
        }
        // --- Método para crear una orden de compra ---
        public int CrearOrdenDeCompra(OrdenCompra orden)
        {
          
            if (orden.IdProveedor <= 0)
                throw new Exception("El proveedor es obligatorio.");
            if (orden.Detalles == null || !orden.Detalles.Any())
                throw new Exception("La orden debe tener detalles.");

            
            orden.IdOrdenCompra = _nextOrdenId++;
            orden.Estado = "PENDIENTE"; 
            _ordenes.Add(orden);
            return orden.IdOrdenCompra;
        }
        // --- Método para obtener una orden con sus detalles ---
        public OrdenCompra GetOrdenConDetalles(int idOrden)
        {
            return _ordenes.FirstOrDefault(o => o.IdOrdenCompra == idOrden);
        }
        // --- Método para listar órdenes de compra pendientes ---
        public List<OrdenCompra> ListarOrdenesPendientes()
        {
            return _ordenes.Where(o => o.Estado == "PENDIENTE").ToList();
        }


        // -- Método para registrar ingreso desde orden ---
        public int RegistrarIngresoDesdeOrden(Compra compra)
        {
            
            if (compra.IdProveedor <= 0)
                throw new Exception("El proveedor es obligatorio.");
            if (compra.Detalles == null || !compra.Detalles.Any())
                throw new Exception("La compra debe tener detalles.");
            if (compra.Total < 0)
                throw new Exception("El total no puede ser negativo.");
            if (compra.Fecha == default)
                throw new Exception("La fecha es obligatoria.");
            if (string.IsNullOrWhiteSpace(compra.Tipo))
                throw new Exception("El tipo de compra es obligatorio.");

            foreach (var det in compra.Detalles)
            {
                if (det.Cantidad <= 0)
                    throw new Exception("La cantidad del detalle no puede ser cero o negativa.");
                if (det.PrecioUnitario < 0)
                    throw new Exception("El precio del detalle no puede ser negativo.");
                if (det.IdArticulo <= 0)
                    throw new Exception("El artículo en el detalle es obligatorio.");
            }

    
            compra.IdCompra = _nextCompraId++;
            _compras.Add(compra);

            
            foreach (var det in compra.Detalles)
            {
                if (!_stock.ContainsKey(det.IdArticulo))
                    _stock[det.IdArticulo] = 0;

                _stock[det.IdArticulo] += det.Cantidad;
            }

           
            if (compra.IdOrdenCompra.HasValue && compra.IdOrdenCompra.Value > 0)
            {
                var orden = _ordenes.FirstOrDefault(o => o.IdOrdenCompra == compra.IdOrdenCompra.Value);
                if (orden != null)
                {
                    orden.Estado = "RECIBIDA";
                }
            }

            return compra.IdCompra;
        }


        // --- Método para listar historial de compras con filtros ---
        public List<Compra> ListarHistorialCompras(string tipo = null, DateTime? desde = null, DateTime? hasta = null)
        {
            var query = _compras.AsQueryable();

            if (!string.IsNullOrWhiteSpace(tipo))
                query = query.Where(c => c.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase));

            if (desde.HasValue)
                query = query.Where(c => c.Fecha >= desde.Value.Date);

            if (hasta.HasValue)
                query = query.Where(c => c.Fecha <= hasta.Value.Date.AddDays(1).AddTicks(-1)); 

            return query.ToList();
        }

        // --- Método para obtener necesidades de repuestos consolidadas ---

        public List<object> ObtenerNecesidadesRepuestosConsolidadas()
        {
            
            return new List<object> { new object() };
        }
    }
}