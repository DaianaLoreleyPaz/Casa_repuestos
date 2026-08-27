using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CasaRepuestos.Tests
{
    [TestClass]
    public class ReparacionTests
    {
        private FakeReparacionService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new FakeReparacionService();
        }

        // ===================================================================
        // 1 al 6: PRUEBAS BÁSICAS Y LÓGICAS
        // ===================================================================

        [TestMethod]
        public void Procesar_ConStockSuficiente_DeberiaReservarYPasarAEnProceso()
        {
            _service.AgregarArticulo(id: 1, stock: 10, stockComprometido: 0);// Artículo con stock suficiente
            var idPres = _service.CrearPresupuesto(estadoInicial: "PENDIENTE");// Nuevo presupuesto
            _service.AgregarDetalle(idPres, idArticulo: 1, idServicio: 0, cantidad: 2);// Detalle que pide 2 unidades

            string resultado = _service.ProcesarInicioReparacion(idPres);// Ejecutar procesamiento

            Assert.AreEqual("EN_PROCESO", resultado);// Verificar resultado
            var art = _service.ObtenerArticulo(1);// Obtener artículo actualizado
            Assert.AreEqual(8, art.Stock);// Verificar stock restante
            Assert.AreEqual(2, art.StockComprometido);// Verificar stock comprometido
        }
       
        [TestMethod]
        // Lógica: Sin stock suficiente
        public void Procesar_SinStock_DeberiaPasarAEsperaRepuestos()
        {
            _service.AgregarArticulo(id: 1, stock: 1, stockComprometido: 0);// Artículo con stock insuficiente
            var idPres = _service.CrearPresupuesto(estadoInicial: "PENDIENTE");// Nuevo presupuesto
            _service.AgregarDetalle(idPres, idArticulo: 1, idServicio: 0, cantidad: 5);// Detalle que pide 5 unidades

            string resultado = _service.ProcesarInicioReparacion(idPres);// Ejecutar procesamiento

            Assert.AreEqual("ESPERA_REPUESTOS", resultado);// Verificar resultado
            var art = _service.ObtenerArticulo(1);// Obtener artículo actualizado
            Assert.AreEqual(1, art.Stock); // Stock intacto// Verificar stock restante
        }

        [TestMethod]
        // Lógica: Solo servicios (sin repuestos físicos)
        public void Procesar_SoloServicios_DeberiaPasarAEnProcesoSinStock()
        {
            var idPres = _service.CrearPresupuesto(estadoInicial: "PENDIENTE");// Nuevo presupuesto
            // Detalle sin articulo (null)
            _service.AgregarDetalle(idPres, idArticulo: null, idServicio: 5, cantidad: 1);// Solo servicio

            string resultado = _service.ProcesarInicioReparacion(idPres);// Ejecutar procesamiento

            Assert.AreEqual("EN_PROCESO_SERVICIO", resultado);// Verificar resultado
            Assert.AreEqual("EN_PROCESO", _service.ObtenerPresupuesto(idPres).Estado);// Verificar estado del presupuesto
        }

        [TestMethod]
        // Lógica: Reintento de procesamiento cuando aún no hay stock
        public void Procesar_ReintentoSinStock_DeberiaRetornarAunSinStock()
        {
            _service.AgregarArticulo(id: 1, stock: 0, stockComprometido: 0);// Artículo sin stock
            var idPres = _service.CrearPresupuesto(estadoInicial: "ESPERA_REPUESTOS"); // Ya estaba en espera
            _service.AgregarDetalle(idPres, idArticulo: 1, idServicio: 0, cantidad: 1);// Detalle que pide 1 unidad

            string resultado = _service.ProcesarInicioReparacion(idPres);// Ejecutar procesamiento

            Assert.AreEqual("AUN_SIN_STOCK", resultado);// Verificar resultado
            Assert.AreEqual("ESPERA_REPUESTOS", _service.ObtenerPresupuesto(idPres).Estado);// Verificar estado del presupuesto
        }

        [TestMethod]
        // Lógica: Finalización que consume stock comprometido
        public void Finalizar_DeberiaConsumirComprometidoYCambiarEstado()
        {
            _service.AgregarArticulo(id: 1, stock: 8, stockComprometido: 2);// Artículo con stock comprometido
            var idPres = _service.CrearPresupuesto(estadoInicial: "EN_PROCESO");// Nuevo presupuesto
            _service.AgregarDetalle(idPres, idArticulo: 1, idServicio: 0, cantidad: 2);// Detalle que usó 2 unidades

            _service.ConsumirStockYFinalizar(idPres);// Ejecutar finalización

            var pres = _service.ObtenerPresupuesto(idPres);// Obtener presupuesto actualizado
            Assert.AreEqual("FINALIZADO", pres.Estado);// Verificar estado final
            var art = _service.ObtenerArticulo(1);// Obtener artículo actualizado
            Assert.AreEqual(0, art.StockComprometido); // Se liberó
        }

        [TestMethod]
        // Robustez: Presupuesto inexistente
        public void Finalizar_PresupuestoNoExiste_DeberiaLanzarExcepcion()
        {
            // Intentar finalizar un presupuesto que no existe
            Assert.ThrowsException<Exception>(() => _service.ConsumirStockYFinalizar(9999));
        }

        // ===================================================================
        // 7 al 10: NUEVAS PRUEBAS (Casos Borde y Seguridad)
        // ===================================================================

        [TestMethod]
        public void Procesar_StockExacto_DeberiaPasarAEnProceso()
        {
            // CASO BORDE: Tengo 5, piden 5. ¿Funciona o falla por < vs <=?
            _service.AgregarArticulo(id: 1, stock: 5, stockComprometido: 0);
            var idPres = _service.CrearPresupuesto(estadoInicial: "PENDIENTE");// Nuevo presupuesto
            _service.AgregarDetalle(idPres, idArticulo: 1, idServicio: 0, cantidad: 5);// Detalle que pide 5 unidades

            string resultado = _service.ProcesarInicioReparacion(idPres);// Ejecutar procesamiento

            Assert.AreEqual("EN_PROCESO", resultado);// Verificar resultado
            var art = _service.ObtenerArticulo(1);// Obtener artículo actualizado
            Assert.AreEqual(0, art.Stock, "El stock debió quedar en 0.");// Verificar stock restante
            Assert.AreEqual(5, art.StockComprometido);// Verificar stock comprometido
        }

        [TestMethod]
        // Seguridad: No permitir reprocesar algo cerrado
        public void Procesar_PresupuestoYaFinalizado_DeberiaLanzarExcepcion()
        {
            // SEGURIDAD: No permitir reprocesar algo cerrado
            var idPres = _service.CrearPresupuesto(estadoInicial: "FINALIZADO");

            Assert.ThrowsException<InvalidOperationException>(() =>// Intentar reprocesar
                _service.ProcesarInicioReparacion(idPres)
            );
        }

        [TestMethod]
        // Seguridad: Presupuesto inexistente
        public void Procesar_PresupuestoInexistente_DeberiaLanzarExcepcion()
        {
            // ROBUSTEZ: ID inválido
            Assert.ThrowsException<Exception>(() =>
                _service.ProcesarInicioReparacion(999)
            );
        }

        [TestMethod]
        // Lógica Mixta: Finalizar presupuesto que solo tenía servicios
        public void Finalizar_SoloServicios_NoDeberiaRomperStock()
        {
            // LÓGICA MIXTA: Finalizar algo que no tenía repuestos físicos
            var idPres = _service.CrearPresupuesto(estadoInicial: "EN_PROCESO");
            // Solo servicio
            _service.AgregarDetalle(idPres, idArticulo: null, idServicio: 10, cantidad: 1);

            // Ejecutamos finalización
            _service.ConsumirStockYFinalizar(idPres);

            var pres = _service.ObtenerPresupuesto(idPres);
            Assert.AreEqual("FINALIZADO", pres.Estado);
            // Si no lanzó excepción, la prueba pasa exitosamente
        }


        // ===================================================================
        // CLASES FAKE (Motor de Simulación)
        // ===================================================================
        // Simulan el comportamiento del servicio real, pero en memoria
        private class FakePresupuesto
        {
            public int IdPresupuesto { get; set; }// Identificador único
            public string Estado { get; set; }// Estado actual del presupuesto
            public List<FakeDetalle> Detalles { get; set; } = new List<FakeDetalle>();// Detalles asociados
        }

        private class FakeDetalle// Detalle de presupuesto
        {
            public int? IdArticulo { get; set; }// Puede ser null si es solo servicio
            public int IdServicio { get; set; }// Identificador del servicio
            public int Cantidad { get; set; }// Cantidad requerida
        }
        // Representa un artículo en inventario
        private class FakeArticulo
        {
            public int IdArticulo { get; set; }// Identificador único
            public int Stock { get; set; }// Stock disponible
            public int StockComprometido { get; set; }// Stock comprometido
        }
        // Servicio simulado para gestionar reparaciones
        private class FakeReparacionService
        {
            private List<FakePresupuesto> _dbPresupuestos = new List<FakePresupuesto>();// Simula tabla de presupuestos
            private List<FakeArticulo> _dbArticulos = new List<FakeArticulo>();// Simula tabla de artículos
            private int _nextId = 1;// Generador de IDs

            public void AgregarArticulo(int id, int stock, int stockComprometido)// Agrega un artículo al inventario simulado
            {
                _dbArticulos.Add(new FakeArticulo { IdArticulo = id, Stock = stock, StockComprometido = stockComprometido });// Añadir artículo
            }

            public int CrearPresupuesto(string estadoInicial)// Crea un nuevo presupuesto simulado
            {
                var p = new FakePresupuesto { IdPresupuesto = _nextId++, Estado = estadoInicial };// Nuevo presupuesto
                _dbPresupuestos.Add(p);// Añadir a la "base de datos"
                return p.IdPresupuesto;// Retornar ID generado
            }
            // Agrega un detalle a un presupuesto existente
            public void AgregarDetalle(int idPresupuesto, int? idArticulo, int idServicio, int cantidad)
            {
                var p = _dbPresupuestos.First(x => x.IdPresupuesto == idPresupuesto);// Buscar presupuesto
                p.Detalles.Add(new FakeDetalle { IdArticulo = idArticulo, IdServicio = idServicio, Cantidad = cantidad });// Añadir detalle
            }

            public FakePresupuesto ObtenerPresupuesto(int id) => _dbPresupuestos.FirstOrDefault(x => x.IdPresupuesto == id);// Obtener presupuesto por ID
            public FakeArticulo ObtenerArticulo(int id) => _dbArticulos.FirstOrDefault(x => x.IdArticulo == id);// Obtener artículo por ID

            // --- Lógica a Testear (Copia Fiel del Service Real) ---

            public string ProcesarInicioReparacion(int idPresupuesto)// Procesa el inicio de una reparación
            {
                var pres = _dbPresupuestos.FirstOrDefault(p => p.IdPresupuesto == idPresupuesto);// Buscar presupuesto
                if (pres == null) throw new Exception("Presupuesto no existe");// Validar existencia

                if (pres.Estado == "FINALIZADO" || pres.Estado == "ENTREGADO")// Validar estado
                    throw new InvalidOperationException("Estado inválido");// No se puede reprocesar

                var repuestosFisicos = pres.Detalles.Where(d => d.IdArticulo.HasValue && d.IdArticulo.Value > 0).ToList();// Filtrar repuestos físicos

                if (repuestosFisicos.Count == 0)// Solo servicios
                {
                    pres.Estado = "EN_PROCESO";// Actualizar estado
                    return "EN_PROCESO_SERVICIO";// Retornar resultado
                }

                bool hayStockParaTodo = true;// Verificar stock suficiente
                foreach (var item in repuestosFisicos)// Iterar detalles
                {
                    var articulo = _dbArticulos.FirstOrDefault(a => a.IdArticulo == item.IdArticulo);// Buscar artículo
                    int stockActual = articulo != null ? articulo.Stock : 0;// Obtener stock actual
                    if (stockActual < item.Cantidad) hayStockParaTodo = false;// Validar cantidad
                }

                if (hayStockParaTodo)
                {
                    foreach (var item in repuestosFisicos)// Reservar stock
                    {
                        var articulo = _dbArticulos.First(a => a.IdArticulo == item.IdArticulo);// Buscar artículo
                        articulo.Stock -= item.Cantidad;// Disminuir stock
                        articulo.StockComprometido += item.Cantidad;// Aumentar comprometido
                    }
                    pres.Estado = "EN_PROCESO";// Actualizar estado
                    return "EN_PROCESO";// Retornar resultado
                }
                else
                {
                    if (pres.Estado == "ESPERA_REPUESTOS") return "AUN_SIN_STOCK";// Reintento sin stock
                    else
                    {
                        pres.Estado = "ESPERA_REPUESTOS";// Actualizar estado
                        return "ESPERA_REPUESTOS";// Retornar resultado
                    }
                }
            }
            // Finaliza la reparación consumiendo el stock comprometido
            public void ConsumirStockYFinalizar(int idPresupuesto)
            {
                var pres = _dbPresupuestos.FirstOrDefault(p => p.IdPresupuesto == idPresupuesto);// Buscar presupuesto
                if (pres == null) throw new Exception("Presupuesto no existe");// Validar existencia

                var repuestosFisicos = pres.Detalles.Where(d => d.IdArticulo.HasValue && d.IdArticulo.Value > 0).ToList();// Filtrar repuestos físicos
                if (repuestosFisicos.Count > 0)// Si hay repuestos físicos
                {
                    foreach (var item in repuestosFisicos)// Iterar detalles
                    {
                        var articulo = _dbArticulos.FirstOrDefault(a => a.IdArticulo == item.IdArticulo);// Buscar artículo
                        if (articulo != null)
                            articulo.StockComprometido = Math.Max(0, articulo.StockComprometido - item.Cantidad);// Consumir stock comprometido
                    }
                }
                pres.Estado = "FINALIZADO";// Actualizar estado 
            }
        }
    }
}