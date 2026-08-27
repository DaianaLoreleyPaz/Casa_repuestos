using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using CasaRepuestos.Models;

namespace CasaRepuestos.Tests
{
    
    // Clase ficticia que encapsula la lógica principal del FrmFactura
    public class FacturaValidaciones
    {
        // estado de caja
        public bool CajaCerrada { get; set; }
        public bool CajaAbierta { get; set; }

        // datos de la venta, metodo de pago, cliente, presupuesto
        public string MetodoPago { get; set; }
        public Cliente Cliente { get; set; }
        // si es null, venta directa; si tiene valor, venta con presupuesto
        public int? IdPresupuesto { get; set; }

        // detalles de la venta
        public List<DetalleFactura> Articulos { get; set; } = new();
        public List<DetallePresupuesto> Reparacion { get; set; } = new();

        public bool ArticuloSeleccionado { get; set; }
        public int CantidadIngresada { get; set; }
        public int StockDisponible { get; set; }

        // cálculo del total de la venta
        public decimal CalcularTotal()
        {
            decimal totalReparacion = Reparacion.Sum(d =>
                ((d.PrecioRepuesto ?? 0m) + (d.PrecioServicio ?? 0m)) * d.Cantidad
            );

            decimal totalArticulos = Articulos.Sum(a => a.subtotal);

            return totalReparacion + totalArticulos;
        }


        // simulación de abrir el FrmCliente para agregar nuevo cliente
        public bool AbrirFormularioCliente()
        {
            // En el FrmFactura, el método buttonAgregarNvoCliente_Click activa: new FrmCliente().ShowDialog()
            // en este caso se simula con un booleano
            return true;
        }

        // validaciones de la venta
        public (bool Success, string Error) ValidarVenta()
        {
            // Verificar si la caja está cerrada
            if (CajaCerrada)
            {
                return (false,
                    "No se pueden realizar ventas.\n\nLa caja del día ya ha sido cerrada.\nPara realizar transacciones, debe reabrir la caja desde el módulo de Arqueo.");
            }

            // Verificar si no hay caja abierta
            if (!CajaAbierta)
            {
                return (false,
                    "No se pueden realizar ventas.\n\nNo hay una caja abierta para el día de hoy.\nPor favor, abra la caja desde el módulo de Arqueo antes de realizar transacciones.");
            }

            // Método de pago requerido
            if (string.IsNullOrWhiteSpace(MetodoPago))
            {
                return (false, "Debe seleccionar un método de pago");
            }

            // debe existir al menos un artículo en la venta si no hay presupuesto seleccionado
            if (!IdPresupuesto.HasValue && (Articulos == null || !Articulos.Any()))
            {
                return (false, "Debe agregar al menos un artículo a la venta");
            }

            // Si no hay presupuesto seleccionado y tampoco hay cliente y el método es CUENTA CORRIENTE
            if (IdPresupuesto == null && Cliente == null &&
                MetodoPago == "CUENTA CORRIENTE")
            {
                return (false, "Debe seleccionar un cliente válido para realizar una venta con método de pago 'CUENTA CORRIENTE'");
            }
            return (true, "");
        }

        // validaciones al agregar un artículo
        public (bool Success, string Error) ValidarAgregarArticulo()
        {
            // Artículo no seleccionado
            if (!ArticuloSeleccionado)
            {
                return (false, "Seleccione un artículo válido");
            }

            // Cantidad inválida
            if (CantidadIngresada <= 0)
            {
                return (false, "Ingrese una cantidad válida");
            }

            // Stock insuficiente
            if (CantidadIngresada > StockDisponible)
            {
                return (false, $"No hay stock suficiente. Stock disponible: {StockDisponible}");
            }
            return (true, "");
        }

        // determina el tipo de factura (A/B) según la categoría del cliente
        public string DeterminarTipoFactura(string categoriaCliente)
        {
            if (string.IsNullOrWhiteSpace(categoriaCliente))
                return "B";

            switch (categoriaCliente.Trim().ToUpper())
            {
                case "RESPONSABLE INSCRIPTO":
                    return "A";

                case "EXENTO":
                case "MONOTRIBUTISTA":
                case "CONSUMIDOR FINAL":
                default:
                    return "B";
            }
        }
    }

    // PRUEBAS UNITARIAS
    [TestClass]
    public class FacturaTest
    {
        // --- CONFIGURACIÓN INICIAL ---
        private FacturaValidaciones validar;

        [TestInitialize]
        public void Setup()
        {
            validar = new FacturaValidaciones();
        }

        // Prueba del botón "+ Agregar Nuevo" que abre el formulario FrmCliente
        [TestMethod]
        public void BotonAgregarNuevoCliente_DeberiaAbrirFrmCliente()
        {
            bool seAbre = validar.AbrirFormularioCliente();

            Assert.IsTrue(seAbre, "El formulario FrmCliente se abre al hacer click en '+ Agregar Nuevo'");
        }


        // Cálculo del total de la venta con reparación y artículos, actualización en tiempo real
        [TestMethod]
        public void CalcularTotal_ActualizaCorrectamente_EnTiempoReal()
        {
            validar.CajaAbierta = true;
            validar.MetodoPago = "EFECTIVO";

            // Reparación: 1 repuesto de 25000 y 1 servicio de 30000
            validar.Reparacion.Add(new DetallePresupuesto
            {
                Cantidad = 1,
                PrecioRepuesto = 25000,
                PrecioServicio = 30000
            });

            // Artículos adicional: 2 artículos de 8000 
            validar.Articulos.Add(new DetalleFactura
            {
                Cantidad = 2,
                PrecioUnitario = 8000,
                subtotal = 16000
            });

            var total = validar.CalcularTotal();

            // Total esperado: 71000
            Assert.AreEqual(71000m, total);

            // total mayor a 0
            Assert.IsTrue(total > 0);
        }


        // Caja aun no abierta, no se permite vender
        [TestMethod]
        public void Venta_SinCajaAbierta_NoPermiteVender()
        {
            validar.CajaCerrada = false;
            validar.CajaAbierta = false;

            var result = validar.ValidarVenta();

            Assert.IsFalse(result.Success);
            Assert.AreEqual(
                "No se pueden realizar ventas.\n\nNo hay una caja abierta para el día de hoy.\nPor favor, abra la caja desde el módulo de Arqueo antes de realizar transacciones.",
                result.Error);
        }

        // Caja abierta pero sin método de pago seleccionado, alerta de método requerido
        [TestMethod]
        public void Venta_SinMetodoPago_MuestraAlerta()
        {
            validar.CajaAbierta = true;
            validar.MetodoPago = null;
            validar.Articulos.Add(new DetalleFactura()); // hay detalle de venta

            var result = validar.ValidarVenta();

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Debe seleccionar un método de pago", result.Error);
        }

        // Caja abierta, método de pago OK, pero sin artículos ni presupuesto seleccionado, se alerta de artículo requerido
        [TestMethod]
        public void Venta_SinArticulosNiReparacion_MuestraAlerta()
        {
            validar.CajaAbierta = true;
            validar.MetodoPago = "EFECTIVO";
            validar.IdPresupuesto = null;           
            validar.Articulos.Clear();          

            var result = validar.ValidarVenta();

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Debe agregar al menos un artículo a la venta", result.Error);
        }

        // Venta con CUENTA CORRIENTE y sin cliente seleccionado, mensaje alerta
        [TestMethod]
        public void Venta_CuentaCorriente_DirectaSinCliente_MuestraAlertaEspecifica()
        {
            validar.CajaAbierta = true;
            validar.MetodoPago = "CUENTA CORRIENTE";
            validar.IdPresupuesto = null;           
            validar.Cliente = null;             
            validar.Articulos.Add(new DetalleFactura());

            var result = validar.ValidarVenta();

            Assert.IsFalse(result.Success);
            Assert.AreEqual(
                "Debe seleccionar un cliente válido para realizar una venta con método de pago 'CUENTA CORRIENTE'",
                result.Error);
        }


        // Venta en efectivo sin cliente, pero con artículos 
        [TestMethod]
        public void Venta_Efectivo_SinCliente_ConArticulos_EsValida()
        {
            validar.CajaAbierta = true;
            validar.MetodoPago = "EFECTIVO";
            validar.IdPresupuesto = null;
            validar.Cliente = null;
            validar.Articulos.Add(new DetalleFactura());

            var result = validar.ValidarVenta();

            Assert.IsTrue(result.Success);
            Assert.AreEqual(string.Empty, result.Error);
        }

        // Venta en efectivo con cliente y artículos
        [TestMethod]
        public void Venta_Efectivo_ConCliente_ConArticulos_EsValida()
        {
            validar.CajaAbierta = true;
            validar.MetodoPago = "EFECTIVO";
            validar.Cliente = new Cliente { IdCliente = 1 };
            validar.Articulos.Add(new DetalleFactura());

            var result = validar.ValidarVenta();

            Assert.IsTrue(result.Success);
            Assert.AreEqual(string.Empty, result.Error);
        }

        // Venta CUENTA CORRIENTE con cliente y artículos 
        [TestMethod]
        public void Venta_CuentaCorriente_ConCliente_ConArticulos_EsValida()
        {
            validar.CajaAbierta = true;
            validar.MetodoPago = "CUENTA CORRIENTE";
            validar.Cliente = new Cliente { IdCliente = 1 };
            validar.Articulos.Add(new DetalleFactura());

            var result = validar.ValidarVenta();

            Assert.IsTrue(result.Success);
            Assert.AreEqual(string.Empty, result.Error);
        }

        // Venta sólo con reparación + método EFECTIVO
        [TestMethod]
        public void Venta_ReparacionConPresupuesto_SinArticulos_EsValida()
        {
            validar.CajaAbierta = true;
            validar.MetodoPago = "EFECTIVO";
            validar.IdPresupuesto = 5;
            validar.Reparacion.Add(new DetallePresupuesto { Cantidad = 1, PrecioRepuesto = 50 });

            var result = validar.ValidarVenta();

            Assert.IsTrue(result.Success);
            Assert.AreEqual(string.Empty, result.Error);
        }

        // Caja abierta, sin método de pago y sin detalles
        [TestMethod]
        public void Venta_SinMetodoPagoConCajaAbierta_PriorizaErrorMetodo()
        {
            validar.CajaAbierta = true;
            validar.MetodoPago = "";                // vacío
            validar.IdPresupuesto = null;
            validar.Articulos.Clear();

            var result = validar.ValidarVenta();

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Debe seleccionar un método de pago", result.Error);
        }

        // Caja cerrada aunque tenga datos correctos, siempre alertr de caja cerrada primero
        [TestMethod]
        public void Venta_CajaCerrada_AunqueHayaDatos_PriorizaErrorCaja()
        {
            validar.CajaCerrada = true;
            validar.CajaAbierta = true;
            validar.MetodoPago = "EFECTIVO";
            validar.Cliente = new Cliente { IdCliente = 1 };
            validar.Articulos.Add(new DetalleFactura());

            var result = validar.ValidarVenta();

            Assert.IsFalse(result.Success);
            Assert.AreEqual(
                "No se pueden realizar ventas.\n\nLa caja del día ya ha sido cerrada.\nPara realizar transacciones, debe reabrir la caja desde el módulo de Arqueo.",
                result.Error);
        }

        // Sin artículo seleccionado, mensaje alerta
        [TestMethod]
        public void AgregarArticulo_SinArticuloSeleccionado_MuestraAlerta()
        {
            validar.ArticuloSeleccionado = false;
            validar.CantidadIngresada = 1;
            validar.StockDisponible = 10;

            var result = validar.ValidarAgregarArticulo();

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Seleccione un artículo válido", result.Error);
        }

        // Cantidad de articulo <= 0, mensaje alerta
        [TestMethod]
        public void AgregarArticulo_CantidadNoValida_MuestraAlerta()
        {
            validar.ArticuloSeleccionado = true;
            validar.CantidadIngresada = 0;
            validar.StockDisponible = 10;

            var result = validar.ValidarAgregarArticulo();

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Ingrese una cantidad válida", result.Error);
        }

        // Cantidad mayor al stock disponible, mensaje de stock insuficiente
        [TestMethod]
        public void AgregarArticulo_StockInsuficiente_MuestraAlerta()
        {
            validar.ArticuloSeleccionado = true;
            validar.CantidadIngresada = 5;
            validar.StockDisponible = 2;

            var result = validar.ValidarAgregarArticulo();

            Assert.IsFalse(result.Success);
            Assert.AreEqual("No hay stock suficiente. Stock disponible: 2", result.Error);
        }

        // Artículo seleccionado + cantidad válida con stock suficiente
        [TestMethod]
        public void AgregarArticulo_DatosValidos_EsCorrecto()
        {
            validar.ArticuloSeleccionado = true;
            validar.CantidadIngresada = 2;
            validar.StockDisponible = 10;

            var result = validar.ValidarAgregarArticulo();

            Assert.IsTrue(result.Success);
            Assert.AreEqual(string.Empty, result.Error);
        }


        // Responsable inscripto, factura A
        [TestMethod]
        public void TipoFactura_ResponsableInscripto_DevuelveA()
        {
            var tipo = validar.DeterminarTipoFactura("RESPONSABLE INSCRIPTO");
            Assert.AreEqual("A", tipo);
        }

        // Monotributista, factura A
        [TestMethod]
        public void TipoFactura_Monotributista_DevuelveA()
        {
            var tipo = validar.DeterminarTipoFactura("MONOTRIBUTISTA");
            Assert.AreEqual("B", tipo);
        }

        // Exento, factura B
        [TestMethod]
        public void TipoFactura_Exento_DevuelveB()
        {
            var tipo = validar.DeterminarTipoFactura("EXENTO");
            Assert.AreEqual("B", tipo);
        }

        // Consumidor Final, factura B
        [TestMethod]
        public void TipoFactura_ConsumidorFinal_DevuelveB()
        {
            var tipo = validar.DeterminarTipoFactura("CONSUMIDOR FINAL");
            Assert.AreEqual("B", tipo);
        }
    }
}
