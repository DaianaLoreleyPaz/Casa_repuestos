using CasaRepuestos.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CasaRepuestos.Tests
{
    [TestClass]

    public class ProveedorTests
    {
        private FakeProveedorService _service;

        [TestInitialize]
        public void Setup() => _service = new FakeProveedorService();

    

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        // Prueba para crear un proveedor sin razón social
        public void CrearProveedor_SinRazonSocial_DeberiaLanzarExcepcion()
        {
            var p = ProveedorValido();// Obtener un proveedor válido
            p.RazonSocial = "";// Establecer razón social vacía
            _service.CrearProveedor(p);// Intentar crear el proveedor
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        // Prueba para crear un proveedor sin CUIL
        public void CrearProveedor_SinCuil_DeberiaLanzarExcepcion()
        {
            var p = ProveedorValido();// Obtener un proveedor válido
            p.Cuil = "";// Establecer CUIL vacío
            _service.CrearProveedor(p);// Intentar crear el proveedor
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        // Prueba para crear un proveedor con CUIL que contiene letras
        public void CrearProveedor_CuilConLetras_DeberiaLanzarExcepcion()
        {
            var p = ProveedorValido();// Obtener un proveedor válido
            p.Cuil = "30ABC123";// Establecer CUIL con letras
            _service.CrearProveedor(p);// Intentar crear el proveedor
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        // Prueba para crear un proveedor sin datos de persona de contacto
        public void CrearProveedor_SinPersona_DeberiaLanzarExcepcion()
        {
            var p = ProveedorValido();// Obtener un proveedor válido
            p.DatosPersona = null;// Establecer datos de persona como nulos
            _service.CrearProveedor(p);// Intentar crear el proveedor
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        // Prueba para crear un proveedor sin nombre de la persona de contacto
        public void CrearProveedor_SinNombrePersona_DeberiaLanzarExcepcion()
        {
            var p = ProveedorValido();// Obtener un proveedor válido
            p.DatosPersona.Nombre = "";// Establecer nombre vacío
            _service.CrearProveedor(p);// Intentar crear el proveedor
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        // Prueba para crear un proveedor sin apellido de la persona de contacto
        public void CrearProveedor_SinApellidoPersona_DeberiaLanzarExcepcion()
        {
            var p = ProveedorValido();// Obtener un proveedor válido
            p.DatosPersona.Apellido = "";// Establecer apellido vacío
            _service.CrearProveedor(p);// Intentar crear el proveedor
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        // Prueba para crear un proveedor sin documento de la persona de contacto
        public void CrearProveedor_SinDocumentoPersona_DeberiaLanzarExcepcion()
        {
            var p = ProveedorValido();// Obtener un proveedor válido
            p.DatosPersona.NumeroDocumento = "";// Establecer documento vacío
            _service.CrearProveedor(p);// Intentar crear el proveedor
        }

    

        [TestMethod]
        // Prueba para crear un proveedor válido
        public void CrearProveedor_Valido_DebeCrearCorrectamente()
        {
            var p = ProveedorValido();// Obtener un proveedor válido
            int id = _service.CrearProveedor(p);// Crear el proveedor

            Assert.IsTrue(id > 0);// Verificar que se haya asignado un ID válido
            Assert.AreEqual(1, _service.ListarProveedores().Count);// Verificar que haya un proveedor en la lista
            Assert.AreEqual("ProveedorX", _service.ListarProveedores().First().RazonSocial);// Verificar que la razón social sea correcta
        }

        

        [TestMethod]
        // Prueba para modificar un proveedor válido
        public void ModificarProveedor_Valido_DebeActualizarDatos()
        {
           
            var p = ProveedorValido();// Obtener un proveedor válido
            int id = _service.CrearProveedor(p);// Crear el proveedor


            var pModificado = _service.ListarProveedores().First();// Obtener el proveedor creado
            pModificado.RazonSocial = "Proveedor MODIFICADO";// Modificar la razón social
            _service.ModificarProveedor(pModificado);// Actualizar el proveedor


            var pVerificar = _service.ListarProveedores().First();// Obtener el proveedor modificado
            Assert.AreEqual(1, _service.ListarProveedores().Count);// Verificar que siga habiendo un proveedor en la lista
            Assert.AreEqual("Proveedor MODIFICADO", pVerificar.RazonSocial);// Verificar que la razón social se haya actualizado
        }

        [TestMethod]
        // Prueba para eliminar un proveedor válido
        public void EliminarProveedor_Valido_DebeQuitarDeLaLista()
        {
            
            var p = ProveedorValido();// Obtener un proveedor válido
            int id = _service.CrearProveedor(p);// Crear el proveedor
            Assert.AreEqual(1, _service.ListarProveedores().Count);// Verificar que haya un proveedor en la lista


            _service.EliminarProveedor(id);// Eliminar el proveedor


            Assert.AreEqual(0, _service.ListarProveedores().Count);// Verificar que la lista esté vacía
        }




        // Método auxiliar para obtener un proveedor válido
        private Proveedor ProveedorValido() => new Proveedor
        {
            Cuil = "30111222333",// CUIL válido
            RazonSocial = "ProveedorX",// Razón social válida
            DatosPersona = new Persona// Datos de persona válidos
            {
                Nombre = "P",// Nombre válido
                Apellido = "Q",// Apellido válido
                TipoDocumento = "DNI",// Tipo de documento válido
                NumeroDocumento = "33333333",// Número de documento válido
                Telefono = "111", // Teléfono válido
                Email = "a@b.com",// Email válido
                Direccion = "Y"// Dirección válida
            }
        };
    }

    // Servicio falso para gestionar proveedores en memoria
    public class FakeProveedorService
    {
        private readonly List<Proveedor> _proveedores = new();// Lista en memoria de proveedores
        private int _nextId = 1;// ID incremental para nuevos proveedores

        // Método auxiliar para verificar si una cadena contiene letras
        private bool ContieneLetras(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;// Si la cadena es nula o vacía, no contiene letras
            return s.Any(char.IsLetter);// Verificar si algún carácter es una letra
        }

        // Método para validar los datos de un proveedor
        private void ValidarProveedor(Proveedor p)
        {
            if (p == null)// Verificar si el proveedor es nulo
                throw new Exception("El proveedor no puede ser nulo.");// Lanzar excepción si es nulo


            if (string.IsNullOrWhiteSpace(p.RazonSocial))// Verificar si la razón social está vacía
                throw new Exception("La razón social es obligatoria.");// Lanzar excepción si está vacía
            if (string.IsNullOrWhiteSpace(p.Cuil))// Verificar si el CUIL está vacío
                throw new Exception("El CUIT/CUIL es obligatorio.");// Lanzar excepción si está vacío
            if (ContieneLetras(p.Cuil))// Verificar si el CUIL contiene letras
                throw new Exception("El CUIT/CUIL no debe contener letras.");// Lanzar excepción si contiene letras


            if (p.DatosPersona == null)// Verificar si los datos de la persona de contacto son nulos
                throw new Exception("Los datos de la persona de contacto son obligatorios.");// Lanzar excepción si son nulos
            if (string.IsNullOrWhiteSpace(p.DatosPersona.Nombre))// Verificar si el nombre de la persona está vacío
                throw new Exception("El nombre de la persona es obligatorio.");// Lanzar excepción si está vacío
            if (string.IsNullOrWhiteSpace(p.DatosPersona.Apellido))// Verificar si el apellido de la persona está vacío
                throw new Exception("El apellido de la persona es obligatorio.");// Lanzar excepción si está vacío
            if (string.IsNullOrWhiteSpace(p.DatosPersona.NumeroDocumento))// Verificar si el documento de la persona está vacío
                throw new Exception("El documento de la persona es obligatorio.");// Lanzar excepción si está vacío

        }

        public int CrearProveedor(Proveedor p)
        {
          
            ValidarProveedor(p);// Validar los datos del proveedor

            // Verificar si ya existe un proveedor con la misma razón social
            if (_proveedores.Any(m => m.RazonSocial.Equals(p.RazonSocial, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("Ya existe una proveedor con esa razón social");

            // Asignar un ID único y agregar el proveedor a la lista
            p.IdProveedor = _nextId++;// Asignar ID incremental
            _proveedores.Add(p);// Agregar a la lista
            return p.IdProveedor;// Devolver el ID asignado
        }
        // Método para modificar un proveedor existente
        public void ModificarProveedor(Proveedor p)
        {
           
            ValidarProveedor(p);// Validar los datos del proveedor

            // Buscar el proveedor existente por ID
            var existente = _proveedores.FirstOrDefault(m => m.IdProveedor == p.IdProveedor);// Buscar por ID
            if (existente == null)// Verificar si el proveedor existe
                throw new Exception("Proveedor no encontrado");// Lanzar excepción si no existe

            // Verificar si ya existe otro proveedor con la misma razón social
            if (_proveedores.Any(m => m.RazonSocial.Equals(p.RazonSocial, StringComparison.OrdinalIgnoreCase) && m.IdProveedor != p.IdProveedor))
                throw new Exception("Ya existe otro proveedor con esa razón social");

            // Actualizar los datos del proveedor existente
            existente.RazonSocial = p.RazonSocial;
            existente.Cuil = p.Cuil;// Actualizar CUIL
            existente.DatosPersona = p.DatosPersona;// Actualizar datos de persona
        }
        // Método para eliminar un proveedor por ID
        public void EliminarProveedor(int id)
        {
            var existente = _proveedores.FirstOrDefault(m => m.IdProveedor == id);// Buscar por ID
            if (existente == null)// Verificar si el proveedor existe
                throw new Exception("Proveedor no encontrado");
            _proveedores.Remove(existente);// Eliminar de la lista
        }
        // Método para listar todos los proveedores
        public List<Proveedor> ListarProveedores()
        {
            return _proveedores.ToList();// Devolver una copia de la lista de proveedores
        }
    }
}