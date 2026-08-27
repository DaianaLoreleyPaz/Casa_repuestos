using CasaRepuestos.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting; 
using System.Collections.Generic; 
using System.Linq; 
using System;

namespace CasaRepuestos.Tests
{
    [TestClass]
    
    public class EmpleadosTests
    {
        // Servicio simulado para pruebas
        private FakeEmpleadoService _service;

        [TestInitialize]
        // Configuración antes de cada prueba
        public void Setup() => _service = new FakeEmpleadoService();

        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        // Prueba para crear un empleado sin usuario
        public void CrearEmpleado_SinUsuario_DeberiaLanzarExcepcion()
        {
            var emp = EmpleadoValido();// Crear un empleado válido
            emp.Usuario = "";// Establecer usuario vacío
            _service.CrearEmpleado(emp);// Intentar crear el empleado
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        // Prueba para crear un empleado sin contraseña
        public void CrearEmpleado_SinContrasenia_DeberiaLanzarExcepcion()
        {
            var emp = EmpleadoValido();// Crear un empleado válido
            emp.Contrasenia = "";// Establecer contraseña vacía
            _service.CrearEmpleado(emp);// Intentar crear el empleado
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        // Prueba para crear un empleado sin rol
        public void CrearEmpleado_SinRol_DeberiaLanzarExcepcion()
        {
            var emp = EmpleadoValido();// Crear un empleado válido
            emp.Rol = "";// Establecer rol vacío
            _service.CrearEmpleado(emp);// Intentar crear el empleado
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        // Prueba para crear un empleado sin datos de persona
        public void CrearEmpleado_SinPersona_DeberiaLanzarExcepcion()
        {
            var emp = EmpleadoValido();// Crear un empleado válido
            emp.DatosPersona = null;// Establecer datos de persona nulos
            _service.CrearEmpleado(emp);// Intentar crear el empleado
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        // Prueba para crear un empleado sin nombre en los datos de persona
        public void CrearEmpleado_SinNombrePersona_DeberiaLanzarExcepcion()
        {
            var emp = EmpleadoValido();// Crear un empleado válido
            emp.DatosPersona.Nombre = "";// Establecer nombre vacío
            _service.CrearEmpleado(emp);// Intentar crear el empleado
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        // Prueba para crear un empleado sin apellido en los datos de persona
        public void CrearEmpleado_SinApellidoPersona_DeberiaLanzarExcepcion()
        {
            var emp = EmpleadoValido();// Crear un empleado válido
            emp.DatosPersona.Apellido = "";// Establecer apellido vacío
            _service.CrearEmpleado(emp);// Intentar crear el empleado
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        // Prueba para crear un empleado sin documento en los datos de persona
        public void CrearEmpleado_SinDocumentoPersona_DeberiaLanzarExcepcion()
        {
            var emp = EmpleadoValido();// Crear un empleado válido
            emp.DatosPersona.NumeroDocumento = "";// Establecer documento vacío
            _service.CrearEmpleado(emp);// Intentar crear el empleado
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        // Prueba para crear un empleado sin email en los datos de persona
        public void CrearEmpleado_SinEmailPersona_DeberiaLanzarExcepcion()
        {
            var emp = EmpleadoValido();// Crear un empleado válido
            emp.DatosPersona.Email = "";// Establecer email vacío
            _service.CrearEmpleado(emp);// Intentar crear el empleado
        }

       
        [TestMethod]
        // Prueba para crear un empleado válido
        public void CrearEmpleado_Valido_DebeCrearCorrectamente()
        {
            var emp = EmpleadoValido();// Crear un empleado válido
            int id = _service.CrearEmpleado(emp);// Crear el empleado

            Assert.IsTrue(id > 0);// Verificar que se asignó un ID válido
            Assert.AreEqual(1, _service.ListarEmpleados().Count);// Verificar que hay un empleado en la lista
            Assert.AreEqual("usuario1", _service.ListarEmpleados().First().Usuario);// Verificar que el usuario es correcto
        }

        [TestMethod]
        // Prueba para modificar un empleado válido
        public void ModificarEmpleado_Valido_DebeActualizarDatos()
        {
           
            int id = _service.CrearEmpleado(EmpleadoValido());// Crear un empleado válido


            var empModificado = _service.ListarEmpleados().First();// Obtener el empleado creado
            empModificado.Usuario = "usuario_modificado";// Modificar el usuario
            _service.ModificarEmpleado(empModificado);// Modificar el empleado


            var empVerificar = _service.ListarEmpleados().First();// Obtener el empleado modificado
            Assert.AreEqual(1, _service.ListarEmpleados().Count);// Verificar que sigue habiendo un empleado en la lista
            Assert.AreEqual("usuario_modificado", empVerificar.Usuario);// Verificar que el usuario se actualizó correctamente
        }

        [TestMethod]
        // Prueba para eliminar un empleado válido
        public void EliminarEmpleado_Valido_DebeQuitarDeLaLista()
        {
         
            int id = _service.CrearEmpleado(EmpleadoValido());// Crear un empleado válido
            Assert.AreEqual(1, _service.ListarEmpleados().Count);// Verificar que hay un empleado en la lista


            _service.EliminarEmpleado(id);// Eliminar el empleado


            Assert.AreEqual(0, _service.ListarEmpleados().Count);// Verificar que la lista de empleados está vacía
        }

 
        private Empleado EmpleadoValido() => new Empleado// Método auxiliar para crear un empleado válido
        {
            Rol = "CAJERO",// Establecer rol
            Usuario = "usuario1",// Establecer usuario
            Contrasenia = "1234",// Establecer contraseña
            DatosPersona = new Persona// Establecer datos de persona
            {
                Nombre = "A",// Establecer nombre
                Apellido = "B",// Establecer apellido
                TipoDocumento = "DNI",// Establecer tipo de documento
                NumeroDocumento = "22222222",// Establecer número de documento
                Telefono = "111", // Establecer teléfono
                Email = "a@b.com",// Establecer email
                Direccion = "X"// Establecer dirección
            }
        };
    }


    // Servicio simulado para gestionar empleados en memoria
    public class FakeEmpleadoService
    {
        private readonly List<Empleado> _empleados = new();// Lista interna de empleados
        private int _nextId = 1;// Contador para asignar IDs únicos

        // Método para validar los datos de un empleado
        private void ValidarEmpleado(Empleado emp)
        {
            if (emp == null)// Verificar si el empleado es nulo
                throw new Exception("El empleado no puede ser nulo.");

            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(emp.Usuario))
                throw new Exception("El usuario es obligatorio.");
            if (string.IsNullOrWhiteSpace(emp.Contrasenia))// Verificar si la contraseña es nula o vacía
                throw new Exception("La contraseña es obligatoria.");
            if (string.IsNullOrWhiteSpace(emp.Rol))// Verificar si el rol es nulo o vacío
                throw new Exception("El rol es obligatorio.");

            
            if (emp.DatosPersona == null)// Verificar si los datos de la persona son nulos
                throw new Exception("Los datos de la persona son obligatorios.");
            if (string.IsNullOrWhiteSpace(emp.DatosPersona.Nombre))// Verificar si el nombre es nulo o vacío
                throw new Exception("El nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(emp.DatosPersona.Apellido))// Verificar si el apellido es nulo o vacío
                throw new Exception("El apellido es obligatorio.");
            if (string.IsNullOrWhiteSpace(emp.DatosPersona.NumeroDocumento))// Verificar si el número de documento es nulo o vacío
                throw new Exception("El documento es obligatorio.");
            if (string.IsNullOrWhiteSpace(emp.DatosPersona.Email))// Verificar si el email es nulo o vacío
                throw new Exception("El email es obligatorio.");

            
        }
        // Método para crear un nuevo empleado
        public int CrearEmpleado(Empleado emp)
        {
            
            ValidarEmpleado(emp);// Validar los datos del empleado

            // Verificar si ya existe un empleado con el mismo usuario
            if (_empleados.Any(m => m.Usuario.Equals(emp.Usuario, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("Ya existe un empleado con ese usuario");

            // Asignar un ID único y agregar el empleado a la lista
            emp.IdEmpleado = _nextId++;// Asignar ID único
            _empleados.Add(emp);// Agregar a la lista
            return emp.IdEmpleado;// Devolver el ID asignado
        }
        // Método para modificar un empleado existente
        public void ModificarEmpleado(Empleado emp)
        {
            
            ValidarEmpleado(emp);// Validar los datos del empleado

            // Buscar el empleado existente por ID
            var existente = _empleados.FirstOrDefault(m => m.IdEmpleado == emp.IdEmpleado);
            if (existente == null)// Verificar si el empleado existe
                throw new Exception("Empleado no encontrado");

            // Verificar si ya existe otro empleado con el mismo usuario
            if (_empleados.Any(m => m.Usuario.Equals(emp.Usuario, StringComparison.OrdinalIgnoreCase) && m.IdEmpleado != emp.IdEmpleado))
                throw new Exception("Ya existe otro empleado con ese usuario");

            // Actualizar los datos del empleado existente
            existente.Usuario = emp.Usuario;// Actualizar usuario
            existente.Contrasenia = emp.Contrasenia;// Actualizar contraseña
            existente.Rol = emp.Rol;// Actualizar rol
            existente.DatosPersona = emp.DatosPersona;// Actualizar datos de persona
        }
        // Método para eliminar un empleado por ID
        public void EliminarEmpleado(int id)
        {
            var existente = _empleados.FirstOrDefault(m => m.IdEmpleado == id);// Buscar el empleado existente por ID
            if (existente == null)// Verificar si el empleado existe
                throw new Exception("Empleado no encontrado");
            _empleados.Remove(existente);// Eliminar el empleado de la lista
        }
        // Método para listar todos los empleados
        public List<Empleado> ListarEmpleados()
        {
            return _empleados.ToList();// Devolver una copia de la lista de empleados
        }
    }
}