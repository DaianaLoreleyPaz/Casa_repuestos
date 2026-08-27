using CasaRepuestos.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting; 
using System; 
using System.Collections.Generic; 
using System.Linq; 

namespace CasaRepuestos.Tests
{
    [TestClass]
    [DoNotParallelize]
    public class PresupuestosTests
    {
        // Servicio simulado para pruebas
        private FakePresupuestoService _service;

        // Constantes para IDs válidos
        private const int ID_INGRESO_VALIDO = 25;
        private const int ID_EMPLEADO_VALIDO = 6;
        private const int ID_ARTICULO_VALIDO = 9;
        private const int ID_SERVICIO_VALIDO = 12;

        [TestInitialize]
        // Configuración antes de cada prueba
        public void Setup()
        {
            // Inicializar el servicio simulado con datos válidos
            _service = new FakePresupuestoService(
                ID_INGRESO_VALIDO,
                ID_SERVICIO_VALIDO,
                ID_ARTICULO_VALIDO
            );
        }

       

        [TestMethod]
        [ExpectedException(typeof(Exception))] // Se espera una excepción
        // Prueba para crear un presupuesto sin empleado
        public void CrearPresupuesto_SinIngreso_DeberiaLanzarExcepcion()
        {
            var p = PresupuestoValido();// Crear un presupuesto válido
            var detalles = ListaDetallesValida();// Crear una lista de detalles válida
            p.IdIngreso = 0; // Establecer IdIngreso inválido


            _service.CreatePresupuesto(p, detalles);// Intentar crear el presupuesto
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        // Prueba para crear un presupuesto sin detalles
        public void CrearPresupuesto_SinDetalles_DeberiaLanzarExcepcion()
        {
            var p = PresupuestoValido();// Crear un presupuesto válido
            var detalles = new List<DetallePresupuesto>(); // Lista de detalles vacía

            _service.CreatePresupuesto(p, detalles);// Intentar crear el presupuesto
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        // Prueba para crear un presupuesto con total negativo
        public void CrearPresupuesto_TotalNegativo_DeberiaLanzarExcepcion()
        {
            var p = PresupuestoValido();// Crear un presupuesto válido
            var detalles = ListaDetallesValida();// Crear una lista de detalles válida
            p.Total = -1000; // Establecer total negativo

            _service.CreatePresupuesto(p, detalles);// Intentar crear el presupuesto
        }

       

        [TestMethod]
        // Prueba para marcar un presupuesto como aprobado por admin
        public void MarcarAprobadoAdmin_DebeCambiarEstado()
        {
           
            var p = PresupuestoValido();// Crear un presupuesto válido
            var detalles = ListaDetallesValida();// Crear una lista de detalles válida
            int id = _service.CreatePresupuesto(p, detalles);// Crear el presupuesto

            Assert.IsTrue(id > 0, "La creación falló");// Verificar que se creó correctamente


            _service.MarcarAprobadoAdmin(id);// Marcar como aprobado por admin


            var pActualizado = _service.GetPresupuestoById(id);// Obtener el presupuesto actualizado
            Assert.IsNotNull(pActualizado);// Verificar que no es nulo
            Assert.AreEqual("APROBADO_ADMIN", pActualizado.Estado);// Verificar el estado
        }

        [TestMethod]
        // Prueba para crear un presupuesto con servicio válido
        public void CrearPresupuesto_ConServicioValido_DebeCrearCorrectamente()
        {
            var p = PresupuestoValido();//
            var detalles = ListaDetallesValida();// Crear una lista de detalles válida
            // Calcular el total basado en los detalles
            var id = _service.CreatePresupuesto(p, detalles);
            Assert.IsTrue(id > 0);// Verificar que se creó correctamente


            var pCreado = _service.GetPresupuestoById(id);// Obtener el presupuesto creado
            Assert.IsNotNull(pCreado);// Verificar que no es nulo
            Assert.AreEqual(ID_INGRESO_VALIDO, pCreado.IdIngreso);// Verificar el IdIngreso
        }

        [TestMethod]
        // Prueba para obtener un presupuesto por ID inexistente
        public void GetPresupuestoById_IdInexistente_DebeRetornarNull()
        {
          
            var p = _service.GetPresupuestoById(-999);// Intentar obtener un presupuesto con ID inválido
            Assert.IsNull(p);// Verificar que retorna null
        }

        [TestMethod]
        // Prueba para obtener los detalles de un presupuesto tras crearlo
        public void GetDetallesPresupuesto_TrasCrear_DebeRetornarDetalles()
        {
            var p = PresupuestoValido();// Crear un presupuesto válido
            var detalles = ListaDetallesValida();// Crear una lista de detalles válida
            detalles.Add(DetalleRepuestoValido()); // Agregar un detalle de repuesto

            p.Total = detalles.Sum(d => (d.PrecioRepuesto ?? 0) + (d.PrecioServicio ?? 0));// Calcular el total basado en los detalles

            int id = _service.CreatePresupuesto(p, detalles);// Crear el presupuesto
            Assert.IsTrue(id > 0);// Verificar que se creó correctamente


            var detallesObtenidos = _service.GetDetallesPresupuesto(id);// Obtener los detalles del presupuesto

            Assert.IsNotNull(detallesObtenidos);// Verificar que no es nulo
            Assert.AreEqual(2, detallesObtenidos.Count);// Verificar la cantidad de detalles
        }

        [TestMethod]
        // Prueba para obtener ingresos sin presupuesto asociado
        public void GetIngresos_DebeRetornarIngresosSinPresupuesto()
        {
           
            var listaInicial = _service.GetIngresos();// Obtener la lista inicial de ingresos
            Assert.IsNotNull(listaInicial);// Verificar que no es nulo
            Assert.AreEqual(1, listaInicial.Count);// Verificar la cantidad inicial de ingresos


            _service.CreatePresupuesto(PresupuestoValido(), ListaDetallesValida());// Crear un presupuesto asociado al ingreso


            var listaFinal = _service.GetIngresos();// Obtener la lista final de ingresos
            Assert.IsNotNull(listaFinal);// Verificar que no es nulo
            Assert.AreEqual(0, listaFinal.Count);// Verificar que el ingreso ya no está disponible
        }

        [TestMethod]
        // Prueba para obtener la lista de servicios
        public void GetServicios_DebeRetornarLista()
        {
         
            var lista = _service.GetServicios();// Obtener la lista de servicios
            Assert.IsNotNull(lista);// Verificar que no es nulo
            Assert.IsTrue(lista.Any());// Verificar que hay servicios disponibles
            Assert.AreEqual(1, lista.Count);// Verificar la cantidad de servicios
        }

        [TestMethod]
        // Prueba para marcar un presupuesto como autorizado y pendiente
        public void MarcarAutorizadoYPendiente_DebeCambiarEstado()
        {
            var p = PresupuestoValido();// Crear un presupuesto válido
            var detalles = ListaDetallesValida();// Crear una lista de detalles válida
            int id = _service.CreatePresupuesto(p, detalles);// Crear el presupuesto
            Assert.IsTrue(id > 0);// Verificar que se creó correctamente

            _service.MarcarAutorizadoYPendiente(id, "SI");// Marcar como autorizado y pendiente

            var pActualizado = _service.GetPresupuestoById(id);// Obtener el presupuesto actualizado
            Assert.IsNotNull(pActualizado);// Verificar que no es nulo
            Assert.AreEqual("SI", pActualizado.Autorizado);// Verificar el campo Autorizado
            Assert.AreEqual("PENDIENTE", pActualizado.Estado);//    Verificar el estado
        }


        // Métodos auxiliares para crear datos válidos
        private Presupuesto PresupuestoValido() => new Presupuesto
        {
            Fecha = DateTime.Now,// Establecer la fecha actual
            Total = 5000,// Establecer un total válido
            Autorizado = "NO",// Establecer autorizado como "NO"
            Estado = "VERIFICAR_PRECIO",// Establecer estado inicial
            IdEmpleado = ID_EMPLEADO_VALIDO,// Establecer un IdEmpleado válido
            IdIngreso = ID_INGRESO_VALIDO// Establecer un IdIngreso válido
        };
        // Crear una lista de detalles válida
        private List<DetallePresupuesto> ListaDetallesValida()
        {
            return new List<DetallePresupuesto>// Retornar una lista con un detalle de servicio válido
            {
                DetalleServicioValido()// Agregar el detalle de servicio válido
            };
        }
        // Crear un detalle de servicio válido
        private DetallePresupuesto DetalleServicioValido() => new DetallePresupuesto
        {
            IdArticulo = null,// No es un repuesto
            IdServicio = ID_SERVICIO_VALIDO,//  Establecer un IdServicio válido
            Cantidad = 1,// Establecer cantidad
            PrecioRepuesto = 0,// No es un repuesto
            PrecioServicio = 5000// Establecer precio del servicio
        };// Crear un detalle de repuesto válido
        // Crear un detalle de repuesto válido
        private DetallePresupuesto DetalleRepuestoValido() => new DetallePresupuesto
        {
            IdArticulo = ID_ARTICULO_VALIDO,// Establecer un IdArticulo válido
            IdServicio = 16, // ID de servicio cualquiera
            Cantidad = 1,// Establecer cantidad
            PrecioRepuesto = 8000,// Establecer precio del repuesto
            PrecioServicio = 2000// Establecer precio del servicio asociado
        };
    }

    // Servicio simulado para pruebas
    public class FakePresupuestoService
    {
        
        private readonly List<Presupuesto> _presupuestos = new();// Lista interna de presupuestos
        private readonly List<DetallePresupuesto> _detalles = new();// Lista interna de detalles de presupuestos
        private readonly List<Ingreso> _ingresos = new();// Lista interna de ingresos
        private readonly List<Servicio> _servicios = new();// Lista interna de servicios
        private readonly List<Articulo> _articulos = new();// Lista interna de artículos

        private int _nextPresupuestoId = 1;// Contador para IDs de presupuestos


        public FakePresupuestoService(int idIngreso, int idServicio, int idArticulo)
        {
            
            _ingresos.Add(new Ingreso { IdIngreso = idIngreso, Falla = "Test Falla" });// Agregar un ingreso de prueba


            _servicios.Add(new Servicio { IdServicio = idServicio, Descripcion = "Test Servicio", Precio = 5000 });// Agregar un servicio de prueba 


            _articulos.Add(new Articulo { IdArticulo = idArticulo, Nombre = "Test Articulo" });// Agregar un artículo de prueba
        }

        // Método para crear un presupuesto con validaciones
        public int CreatePresupuesto(Presupuesto p, List<DetallePresupuesto> detalles)
        {
            
            if (detalles == null || !detalles.Any())// Validar que haya al menos un detalle
            {
                throw new Exception("El presupuesto debe tener al menos un detalle.");
            }
            if (p.Total < 0)// Validar que el total no sea negativo
            {
                throw new Exception("El total no puede ser negativo.");
            }
            if (p.IdEmpleado <= 0)// Validar que haya un empleado asignado
            {
                throw new Exception("El empleado es obligatorio.");
            }
            if (p.IdIngreso <= 0)// Validar que haya un ingreso asignado
            {
                throw new Exception("El ingreso es obligatorio.");
            }

            
            int newId = _nextPresupuestoId++;// Generar un nuevo ID para el presupuesto
            p.IdPresupuesto = newId;// Asignar el ID al presupuesto
            p.Estado = "VERIFICAR_PRECIO"; // Estado inicial
            _presupuestos.Add(p);// Agregar el presupuesto a la lista


            foreach (var d in detalles)// Agregar cada detalle asociado al presupuesto
            {
                d.IdPresupuesto = newId;// Asignar el ID del presupuesto al detalle
                _detalles.Add(d);// Agregar el detalle a la lista
            }

            return newId;// Retornar el ID del nuevo presupuesto
        }

        // Método para marcar un presupuesto como aprobado por admin
        public void MarcarAprobadoAdmin(int idPresupuesto)
        {
            var p = GetPresupuestoById(idPresupuesto);// Obtener el presupuesto por ID
            if (p != null)// Si se encuentra el presupuesto
            {
                p.Estado = "APROBADO_ADMIN";// Cambiar el estado a aprobado por admin
            }
            
        }
        // Método para marcar un presupuesto como autorizado y pendiente
        public void MarcarAutorizadoYPendiente(int idPresupuesto, string autorizado)
        {
            var p = GetPresupuestoById(idPresupuesto);// Obtener el presupuesto por ID
            if (p != null)// Si se encuentra el presupuesto
            {
                p.Autorizado = autorizado;// Actualizar el campo Autorizado
                p.Estado = "PENDIENTE";// Cambiar el estado a pendiente
            }
        }

        // Método para obtener un presupuesto por ID
        public Presupuesto GetPresupuestoById(int idPresupuesto)
        {
            return _presupuestos.FirstOrDefault(p => p.IdPresupuesto == idPresupuesto);// Retornar el presupuesto o null si no se encuentra
        }
        // Método para obtener los detalles de un presupuesto
        public List<DetallePresupuesto> GetDetallesPresupuesto(int idPresupuesto)
        {
            return _detalles.Where(d => d.IdPresupuesto == idPresupuesto).ToList();// Retornar la lista de detalles asociados al presupuesto
        }
        // Método para obtener ingresos sin presupuesto asociado
        public List<Ingreso> GetIngresos()
        {
            // Retornar los ingresos que no tienen un presupuesto asociado
            return _ingresos
                .Where(i => !_presupuestos.Any(p => p.IdIngreso == i.IdIngreso))
                .ToList();// Retornar la lista de ingresos sin presupuesto asociado
        }
        // Método para obtener la lista de servicios
        public List<Servicio> GetServicios()
        {
            
            return _servicios.ToList();// Retornar la lista de servicios
        }

       
    }
}