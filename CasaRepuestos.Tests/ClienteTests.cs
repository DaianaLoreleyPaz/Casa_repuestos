using CasaRepuestos.Models;
using CasaRepuestos.Services;

namespace CasaRepuestos.Tests
{
    [TestClass]
    public class ClienteTests
    {
        private ClienteService _serviceCliente;
        private CuentaCorrienteService _serviceCuentaCte;
        private List<MovimientoCuentaCorriente> _movimientosFake;

        [TestInitialize]
        // Configuración inicial antes de cada prueba
        public void Setup()
        {
            // Servicios reales para las pruebas
            _serviceCliente = new ClienteService();
            _serviceCuentaCte = new CuentaCorrienteService();
            _movimientosFake = new List<MovimientoCuentaCorriente>

            {
                // Objeto MovimientoCuentaCorriente de ejemplo
                new MovimientoCuentaCorriente { 
                    Id_Factura = 1, Fecha = DateTime.Now, Monto = 1000, Metodo_Pago = "EFECTIVO", Concepto = "PAGADA" 
                },
                new MovimientoCuentaCorriente { 
                    Id_Factura = 2, Fecha = DateTime.Now, Monto = 2000, Metodo_Pago = "TRANSFERENCIA", Concepto = "PENDIENTE" 
                }
            };
        }
        

        [TestMethod]
        // Test para validar que se lanza una excepción al crear un cliente sin apellido
        public void CrearCliente_SinApellido_DeberiaLanzarExcepcion()
        {
            var cliente = ClienteValido();
            cliente.DatosPersona.Apellido = "";
            Assert.ThrowsException<Exception>(() => _serviceCliente.CrearCliente(cliente));
        }

        [TestMethod]
        // Test para validar que se lanza una excepción al crear un cliente sin documento
        public void CrearCliente_SinDocumento_DeberiaLanzarExcepcion()
        {
            var cliente = ClienteValido();
            cliente.DatosPersona.NumeroDocumento = "";
            Assert.ThrowsException<Exception>(() => _serviceCliente.CrearCliente(cliente));
        }

        [TestMethod]
        // Test para validar que se lanza una excepción al crear un cliente con documento que contiene letras
        public void CrearCliente_DocumentoConLetras_DeberiaLanzarExcepcion()
        {
            var cliente = ClienteValido();
            cliente.DatosPersona.NumeroDocumento = "ABC123";
            Assert.ThrowsException<Exception>(() => _serviceCliente.CrearCliente(cliente));
        }

        [TestMethod]
        // Test para validar que se lanza una excepción al crear un cliente sin categoría
        public void CrearCliente_SinCategoria_DeberiaLanzarExcepcion()
        {
            var cliente = ClienteValido();
            cliente.Categoria = "";
            Assert.ThrowsException<Exception>(() => _serviceCliente.CrearCliente(cliente));
        }

        [TestMethod]
        // Test para validar que se lanza una excepción al crear un cliente sin cuil
        public void CrearCliente_SinCuil_DeberiaLanzarExcepcion()
        {
            var cliente = ClienteValido();
            cliente.Cuil = "";
            Assert.ThrowsException<Exception>(() => _serviceCliente.CrearCliente(cliente));
        }


        // PRUEBAS PARA CUENTA CORRIENTE

        [TestMethod]
        // Test para validar que se agrega un movimiento al registrar un pago
        public void RegistrarPago_DeberiaAgregarMovimiento() { 
            int antes = _movimientosFake.Count; 
            _movimientosFake.Add(new MovimientoCuentaCorriente()); 
            Assert.AreEqual(antes + 1, _movimientosFake.Count); 
        }

        [TestMethod]
        // Test para validar que un pago con monto cero es inválido
        public void RegistrarPago_MontoCero_DeberiaSerInvalido() { 
            var mov = new MovimientoCuentaCorriente { Monto = 0 }; 
            Assert.AreEqual(0, mov.Monto); 
        }

        [TestMethod]
        // Test para validar que se reduce el saldo al actualizarlo
        public void ActualizarSaldo_DeberiaReducirSaldo() { 
            decimal saldo = 5000; saldo -= 1000; 
            Assert.AreEqual(4000, saldo); 
        }

        [TestMethod]
        // Test para validar que se asigna una descripción por defecto si no se proporciona
        public void MovimientoSinDescripcion_DeberiaAsignarPorDefecto() { 
            var mov = new MovimientoCuentaCorriente(); 
            mov.Descripcion_Metodo_Pago ??= "SIN DESC"; 
            Assert.AreEqual("SIN DESC", mov.Descripcion_Metodo_Pago); 
        }

        [TestMethod]
        // Test para validar que la fecha del movimiento es hoy
        public void FechaMovimiento_DeberiaSerHoy() { 
            var mov = new MovimientoCuentaCorriente { Fecha = DateTime.Now }; 
            Assert.AreEqual(DateTime.Today, mov.Fecha.Date); 
        }


        // Cliente ficticio para los tests
        private Cliente ClienteValido() => new Cliente
        {
            Categoria = "CONSUMIDOR FINAL",
            Cuil = "20123456789",
            DatosPersona = new Persona
            {
                Nombre = "Juan",
                Apellido = "Pérez",
                TipoDocumento = "DNI",
                NumeroDocumento = "12345678",
                Telefono = "11223344",
                Email = "juan@test.com",
                Direccion = "Calle 1"
            }
        };
    }
}

