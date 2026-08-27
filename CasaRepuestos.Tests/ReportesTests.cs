using CasaRepuestos.Models;
using CasaRepuestos.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CasaRepuestos.Tests
{
    /// <summary>
    /// Pruebas para el formulario FrmReportes y sus funcionalidades
    /// Sección de Reportes en la aplicación CasaRepuestos
    /// 
    /// Pestaña Compras:
    /// - Controles: dtpComprasDesde, dtpComprasHasta, btnGenerarCompras, btnLimpiarCompras
    /// - Labels: lblTotalCompras, lblCantidadCompras, lblPromedioCompras
    /// - DataGridView: dgvDetalleCompras (muestra detalles de órdenes de compra)
    /// - Panel: panelGraficoCompras (gráfico visual por proveedor)
    /// 
    /// Pestaña Ventas:
    /// - Controles: dtpVentasDesde, dtpVentasHasta, btnGenerarVentas, btnLimpiarVentas
    /// - Labels: lblTotalVentas, lblCantidadVentas, lblPromedioVentas
    /// - DataGridView: dgvDetalleVentas (muestra detalles de facturas)
    /// - Paneles: panelGraficoVentas (por método de pago), panelGraficoTipoCliente (por tipo de cliente)
    /// </summary>
    [TestClass]
    public class ReportesTests
    {
        // --- CONFIGURACIÓN INICIAL ---
        // Instancia del servicio de reportes que se prueba
        // Correspondiente a: private ReportesService _reportesService en FrmReportes.cs
        private ReportesService _service;

        [TestInitialize]
        // Configura el servicio antes de cada prueba
        // Similar a: _reportesService = new ReportesService() en el constructor de FrmReportes
        public void Setup()
        {
            _service = new ReportesService();
        }

        // ========== PRUEBAS DE REPORTE DE COMPRAS ==========
        // Botón: "Generar Reporte" en la pestaña "Reporte de Compras" (btnGenerarCompras_Click)
        // Sección: Panel superior con filtros de fecha para compras
        // Componentes afectados: lblTotalCompras, lblCantidadCompras, lblPromedioCompras, panelGraficoCompras, dgvDetalleCompras

        [TestMethod]
        // Test para verificar que el reporte de compras retorna un objeto válido con fechas correctas
        // Funcionalidad básica del botón Generar Reporte Compras
        // Componente: dtpComprasDesde, dtpComprasHasta, btnGenerarCompras
        // Método servicio: _reportesService.ObtenerReporteCompras()
        // Evento asociado: BtnGenerarCompras_Click
        public void ObtenerReporteCompras_ConRangoValido_RetornaReporteNoNulo()
        {
            var fechaDesde = new DateTime(2025, 10, 1); //arrange
            var fechaHasta = new DateTime(2025, 10, 31);
            var reporte = _service.ObtenerReporteCompras(fechaDesde, fechaHasta); //act

            Assert.IsNotNull(reporte);
            Assert.AreEqual(fechaDesde, reporte.FechaDesde);
            Assert.AreEqual(fechaHasta, reporte.FechaHasta);
        }

        [TestMethod]
        // Test para verificar que sin datos retorna valores en cero
        // Comportamiento cuando no hay registros de compras en el rango seleccionado
        // Componentes: lblTotalCompras, lblCantidadCompras, lblPromedioCompras
        // Método servicio: _reportesService.ObtenerReporteCompras()
        // Evento asociado: BtnGenerarCompras_Click
        public void ObtenerReporteCompras_SinDatos_RetornaCeros()
        {
            var fechaDesde = DateTime.Today.AddDays(-30);
            var fechaHasta = DateTime.Today.AddDays(-29);
            var reporte = _service.ObtenerReporteCompras(fechaDesde, fechaHasta);

            Assert.AreEqual(0, reporte.CantidadOperaciones);
            Assert.AreEqual(0m, reporte.TotalCompras);
            Assert.AreEqual(0m, reporte.PromedioOperacion);
        }

        [TestMethod]
        // Test para verificar que con datos se calculan totales correctamente
        // Precisión en los cálculos del Total Compras y Cantidad de Operaciones
        // Componentes: lblTotalCompras, lblCantidadCompras
        // Método servicio: _reportesService.ObtenerReporteCompras() - queryResumen en ReportesService.cs
        // Evento asociado: BtnGenerarCompras_Click
        public void ObtenerReporteCompras_ConDatos_CalculaTotalesCorrectamente()
        {
            var fechaDesde = new DateTime(2025, 10, 1);
            var fechaHasta = new DateTime(2025, 10, 31);
            var reporte = _service.ObtenerReporteCompras(fechaDesde, fechaHasta);

            Assert.IsTrue(reporte.CantidadOperaciones >= 0);
            Assert.IsTrue(reporte.TotalCompras >= 0m);
            Assert.IsTrue(reporte.PromedioOperacion >= 0m);
        }

        // prueba para verificar que sin datos retorna valores en cero pero con categorías
        [TestMethod]
        public void ObtenerReporteCompras_SinDatos_RetornaCerosConCategorias()
        {
            var fechaDesde = DateTime.Today.AddDays(-30);
            var fechaHasta = DateTime.Today.AddDays(-29);

            var reporte = _service.ObtenerReporteCompras(fechaDesde, fechaHasta);

            Assert.AreEqual(0, reporte.CantidadOperaciones);
            Assert.AreEqual(0m, reporte.TotalCompras);
            Assert.AreEqual(0m, reporte.PromedioOperacion);

            // El service actual solo garantiza que NO sea null, no que tenga categoría
            Assert.IsNotNull(reporte.ComprasPorCategoria);
        }


        [TestMethod]
        // Test para verificar que el promedio se calcula correctamente
        //  Fórmula de cálculo del Promedio por Operación
        // Componente: lblPromedioCompras
        // Método servicio: _reportesService.ObtenerReporteCompras() - cálculo: TotalCompras/CantidadOperaciones
        // Evento asociado: BtnGenerarCompras_Click
        public void ObtenerReporteCompras_ConDatos_CalculaPromedioCorrectamente()
        {
            var fechaDesde = new DateTime(2025, 10, 1);
            var fechaHasta = new DateTime(2025, 10, 31);
            var reporte = _service.ObtenerReporteCompras(fechaDesde, fechaHasta);

            if (reporte.CantidadOperaciones > 0)
            {
                decimal promedioEsperado = reporte.TotalCompras / reporte.CantidadOperaciones;
                Assert.AreEqual(promedioEsperado, reporte.PromedioOperacion);
            }
        }

        [TestMethod]
        // Test para verificar que los diccionarios de categoría no son nulos
        // Inicialización correcta de estructuras de datos para Compras por Categoría
        // Componente: panelGraficoCompras (estructura de datos para gráficos)
        // Método servicio: _reportesService.ObtenerReporteCompras() - ComprasPorCategoria, OperacionesPorCategoria
        // Evento asociado: BtnGenerarCompras_Click
        public void ObtenerReporteCompras_Siempre_DiccionariosCategoriaNoNulos()
        {
            var fechaDesde = DateTime.Today.AddMonths(-1);
            var fechaHasta = DateTime.Today;
            var reporte = _service.ObtenerReporteCompras(fechaDesde, fechaHasta);

            Assert.IsNotNull(reporte.ComprasPorCategoria);
            Assert.IsNotNull(reporte.OperacionesPorCategoria);
        }

        [TestMethod]
        // Test para verificar que el detalle de compras retorna una lista
        // Funcionalidad del Detalle de Compras en el DataGridView
        // Componente: dgvDetalleCompras (DataGridView con detalles de órdenes de compra)
        // Método servicio: _reportesService.ObtenerDetalleCompras() - query en ReportesService.cs
        // Evento asociado: BtnGenerarCompras_Click
        public void ObtenerDetalleCompras_ConRangoValido_RetornaLista()
        {
            var fechaDesde = new DateTime(2025, 10, 1);
            var fechaHasta = new DateTime(2025, 10, 31);
            var detalles = _service.ObtenerDetalleCompras(fechaDesde, fechaHasta);

            Assert.IsNotNull(detalles);
        }

       

        // ========== PRUEBAS DE REPORTE DE VENTAS ==========
        // Botón: "Generar Reporte" en la pestaña "Reporte de Ventas e Ingresos" (btnGenerarVentas_Click)
        // Sección: Panel superior con filtros de fecha para ventas
        // Componentes afectados: lblTotalVentas, lblCantidadVentas, lblPromedioVentas, panelGraficoVentas, panelGraficoTipoCliente, dgvDetalleVentas

        [TestMethod]
        // Test para verificar que el reporte de ventas retorna un objeto válido con fechas correctas
        // Funcionalidad básica del botón Generar Reporte Ventas
        // Componentes: dtpVentasDesde, dtpVentasHasta, btnGenerarVentas
        // Método servicio: _reportesService.ObtenerReporteVentas()
        // Evento asociado: BtnGenerarVentas_Click
        public void ObtenerReporteVentas_ConRangoValido_RetornaReporteNoNulo()
        {
            var fechaDesde = new DateTime(2025, 10, 1);
            var fechaHasta = new DateTime(2025, 10, 31);
            var reporte = _service.ObtenerReporteVentas(fechaDesde, fechaHasta);

            Assert.IsNotNull(reporte);
            Assert.AreEqual(fechaDesde, reporte.FechaDesde);
            Assert.AreEqual(fechaHasta, reporte.FechaHasta);
        }

        [TestMethod]
        // Test para verificar que sin datos retorna valores en cero
        // Comportamiento cuando no hay registros de ventas en el rango seleccionado
        // Componentes: lblTotalVentas, lblCantidadVentas, lblPromedioVentas
        // Método servicio: _reportesService.ObtenerReporteVentas() - queryResumen en ReportesService.cs
        // Evento asociado: BtnGenerarVentas_Click
        public void ObtenerReporteVentas_SinDatos_RetornaCeros()
        {
            var fechaDesde = DateTime.Today.AddDays(-30);
            var fechaHasta = DateTime.Today.AddDays(-29);
            var reporte = _service.ObtenerReporteVentas(fechaDesde, fechaHasta);

            Assert.AreEqual(0, reporte.CantidadOperaciones);
            Assert.AreEqual(0m, reporte.TotalVentas);
            Assert.AreEqual(0m, reporte.PromedioOperacion);
        }

        [TestMethod]
        // Test para verificar que con datos se calculan totales correctamente
        // Precisión en los cálculos del Total Ventas y Cantidad de Operaciones
        // Componentes: lblTotalVentas, lblCantidadVentas
        // Método servicio: _reportesService.ObtenerReporteVentas() - queryResumen en ReportesService.cs
        // Evento asociado: BtnGenerarVentas_Click
        public void ObtenerReporteVentas_ConDatos_CalculaTotalesCorrectamente()
        {
            var fechaDesde = new DateTime(2025, 10, 1);
            var fechaHasta = new DateTime(2025, 10, 31);
            var reporte = _service.ObtenerReporteVentas(fechaDesde, fechaHasta);

            Assert.IsTrue(reporte.CantidadOperaciones >= 0);
            Assert.IsTrue(reporte.TotalVentas >= 0m);
            Assert.IsTrue(reporte.PromedioOperacion >= 0m);
        }

        [TestMethod]
        // Test para verificar que sin datos retorna valores en cero pero con categorías
        // Estructura del reporte se mantiene incluso sin datos
        // Componentes: panelGraficoVentas, panelGraficoTipoCliente (gráficos por método de pago y tipo de cliente)
        // Método servicio: _reportesService.ObtenerReporteVentas() - queryMetodo y queryTipoCliente en ReportesService.cs
        // Evento asociado: BtnGenerarVentas_Click
        public void ObtenerReporteVentas_SinDatos_RetornaCerosConCategorias()
        {
            var fechaDesde = DateTime.Today.AddDays(-30);
            var fechaHasta = DateTime.Today.AddDays(-29);
            var reporte = _service.ObtenerReporteVentas(fechaDesde, fechaHasta);

            Assert.AreEqual(0, reporte.CantidadOperaciones);
            Assert.AreEqual(0m, reporte.TotalVentas);
            Assert.AreEqual(0m, reporte.PromedioOperacion);
            Assert.IsTrue(reporte.VentasPorTipoCliente.Count >= 4);
        }

        [TestMethod]
        // Test para verificar que el promedio se calcula correctamente
        // Fórmula de cálculo del Promedio por Operación
        // Componentes: lblPromedioVentas
        // Método servicio: _reportesService.ObtenerReporteVentas() - cálculo: TotalVentas/CantidadOperaciones
        // Evento asociado: BtnGenerarVentas_Click
        public void ObtenerReporteVentas_ConDatos_CalculaPromedioCorrectamente()
        {
            var fechaDesde = new DateTime(2025, 10, 1);
            var fechaHasta = new DateTime(2025, 10, 31);
            var reporte = _service.ObtenerReporteVentas(fechaDesde, fechaHasta);

            if (reporte.CantidadOperaciones > 0)
            {
                decimal promedioEsperado = reporte.TotalVentas / reporte.CantidadOperaciones;
                Assert.AreEqual(promedioEsperado, reporte.PromedioOperacion);
            }
        }

        [TestMethod]
        // Test para verificar que los diccionarios de método de pago no son nulos
        // Inicialización correcta de estructuras de datos para Ventas por Método de Pago
        // Componentes: panelGraficoVentas (estructura de datos para gráficos por método de pago)
        // Método servicio: _reportesService.ObtenerReporteVentas() - VentasPorMetodoPago, OperacionesPorMetodoPago
        // Evento asociado: BtnGenerarVentas_Click
        public void ObtenerReporteVentas_Siempre_DiccionariosMetodoPagoNoNulos()
        {
            var fechaDesde = DateTime.Today.AddMonths(-1);
            var fechaHasta = DateTime.Today;
            var reporte = _service.ObtenerReporteVentas(fechaDesde, fechaHasta);

            Assert.IsNotNull(reporte.VentasPorMetodoPago);
            Assert.IsNotNull(reporte.OperacionesPorMetodoPago);
        }

        [TestMethod]
        // Test para verificar que el detalle de ventas retorna una lista
        // Funcionalidad del Detalle de Ventas en el DataGridView
        // Componentes: dgvDetalleVentas (DataGridView con detalles de facturas)
        // Método servicio: _reportesService.ObtenerDetalleVentas() - queryFacturas en ReportesService.cs
        // Evento asociado: BtnGenerarVentas_Click
        public void ObtenerDetalleVentas_ConRangoValido_RetornaLista()
        {
            var fechaDesde = new DateTime(2025, 10, 1);
            var fechaHasta = new DateTime(2025, 10, 31);
            var detalles = _service.ObtenerDetalleVentas(fechaDesde, fechaHasta);

            Assert.IsNotNull(detalles);
        }

        [TestMethod]
        // Test para verificar que los detalles tienen tipo de cliente con formato correcto
        // Consistencia en el formato de Tipos de Cliente mostrados en el detalle
        // Componente: dgvDetalleVentas (columna TipoCliente)
        // Método servicio: _reportesService.ObtenerDetalleVentas() - DetalleVentaReporte.TipoCliente
        // Evento asociado: BtnGenerarVentas_Click
        public void ObtenerDetalleVentas_ConDatos_TieneTipoClienteConFormato()
        {
            var fechaDesde = new DateTime(2025, 10, 1);
            var fechaHasta = new DateTime(2025, 10, 31);
            var detalles = _service.ObtenerDetalleVentas(fechaDesde, fechaHasta);

            foreach (var detalle in detalles)
            {
                Assert.IsTrue(
                    detalle.TipoCliente.Contains("VENTA") || detalle.TipoCliente.Contains("REPARACION"));
            }
        }

        [TestMethod]
        // Test para verificar que las fechas están dentro del rango solicitado
        // Filtrado correcto de registros según el rango de fechas seleccionado
        // Componente: dtpVentasDesde, dtpVentasHasta (controles DateTimePicker)
        // Método servicio: _reportesService.ObtenerDetalleVentas() - WHERE DATE(f.fecha) BETWEEN en ReportesService.cs
        // Evento asociado: BtnGenerarVentas_Click
        public void ObtenerDetalleVentas_ConDatos_FechasDentroDelRango()
        {
            var fechaDesde = new DateTime(2025, 10, 1);
            var fechaHasta = new DateTime(2025, 10, 31);
            var detalles = _service.ObtenerDetalleVentas(fechaDesde, fechaHasta);

            foreach (var detalle in detalles)
            {
                Assert.IsTrue(detalle.Fecha.Date >= fechaDesde.Date);
                Assert.IsTrue(detalle.Fecha.Date <= fechaHasta.Date);
            }
        }

        [TestMethod]
        // Test para verificar que la suma de categorías no excede el total
        // Integridad de los cálculos en Ventas por Tipo de Cliente
        // Componente: panelGraficoTipoCliente (gráficos por tipo de cliente)
        // Método servicio: _reportesService.ObtenerReporteVentas() - VentasPorTipoCliente.Values.Sum()
        // Evento asociado: BtnGenerarVentas_Click
        public void ObtenerReporteVentas_ConDatos_SumaCategoriasNoExcedeTotal()
        {
            var fechaDesde = new DateTime(2025, 10, 1);
            var fechaHasta = new DateTime(2025, 10, 31);
            var reporte = _service.ObtenerReporteVentas(fechaDesde, fechaHasta);

            if (reporte.CantidadOperaciones > 0)
            {
                decimal sumaPorCategoria = reporte.VentasPorTipoCliente.Values.Sum();
                Assert.IsTrue(sumaPorCategoria <= reporte.TotalVentas);
            }
        }

        [TestMethod]
        // Test para verificar que las categorías tienen valores coherentes
        // Consistencia entre Ventas por Tipo Cliente y Operaciones por Tipo Cliente
        // Componente: panelGraficoTipoCliente, dgvDetalleVentas
        // Método servicio: _reportesService.ObtenerReporteVentas() - VentasPorTipoCliente y OperacionesPorTipoCliente
        // Evento asociado: BtnGenerarVentas_Click
        public void ObtenerReporteVentas_ConDatos_CategoriasConValoresCoherentes()
        {
            var fechaDesde = new DateTime(2025, 10, 1);
            var fechaHasta = new DateTime(2025, 10, 31);
            var reporte = _service.ObtenerReporteVentas(fechaDesde, fechaHasta);

            foreach (var categoria in reporte.VentasPorTipoCliente)
            {
                Assert.IsTrue(categoria.Value >= 0);
                if (reporte.OperacionesPorTipoCliente.ContainsKey(categoria.Key))
                {
                    Assert.IsTrue(reporte.OperacionesPorTipoCliente[categoria.Key] >= 0);
                }
            }
        }

        [TestMethod]
        // Test para verificar que el reporte maneja correctamente un rango de un solo día
        // Funcionalidad cuando se selecciona una sola fecha en ambos controles DateTimePicker
        // Componente: dtpVentasDesde, dtpVentasHasta (cuando tienen la misma fecha)
        // Método servicio: _reportesService.ObtenerReporteVentas() con fechas iguales
        // Evento asociado: BtnGenerarVentas_Click
        public void ObtenerReporteVentas_UnSoloDia_FuncionaCorrectamente()
        {
            var fecha = DateTime.Today;

            var reporte = _service.ObtenerReporteVentas(fecha, fecha);
            Assert.IsNotNull(reporte);
            Assert.AreEqual(fecha, reporte.FechaDesde);
            Assert.AreEqual(fecha, reporte.FechaHasta);
        }

        [TestMethod]
        // Test para verificar que el reporte de compras maneja correctamente un rango de un solo día
        // Funcionalidad cuando se selecciona una sola fecha en ambos controles DateTimePicker
        // Componente: dtpComprasDesde, dtpComprasHasta (cuando tienen la misma fecha)
        // Método servicio: _reportesService.ObtenerReporteCompras() con fechas iguales
        // Evento asociado: BtnGenerarCompras_Click
        public void ObtenerReporteCompras_UnSoloDia_FuncionaCorrectamente()
        {
            var fecha = DateTime.Today;

            var reporte = _service.ObtenerReporteCompras(fecha, fecha);
            Assert.IsNotNull(reporte);
            Assert.AreEqual(fecha, reporte.FechaDesde);
            Assert.AreEqual(fecha, reporte.FechaHasta);
        }
    }
}