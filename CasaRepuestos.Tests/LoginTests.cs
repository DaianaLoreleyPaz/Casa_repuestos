using CasaRepuestos.Models;
using CasaRepuestos.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CasaRepuestos.Tests
{
    [TestClass]
    public class LoginTests
    {
        [TestInitialize]
        // Configura el estado inicial antes de cada prueba
        public void Setup()
        {
            // Cerrar cualquier sesión previa para empezar limpio
            SesionService.CerrarSesion();
        }

        [TestCleanup]
        // Limpia después de cada prueba
        public void Cleanup()
        {
            // Cerrar sesión después de cada prueba
            SesionService.CerrarSesion();
        }

        // ========== PRUEBAS DE INICIO DE SESIÓN ==========

        [TestMethod]
        // Test para verificar que se puede iniciar sesión correctamente
        public void IniciarSesion_ConDatosValidos_DeberiaEstablecerSesion()
        {
            // Asegurar que no hay sesión activa
            SesionService.CerrarSesion();
            
            int idEmpleado = 1;
            string usuario = "admin";
            string rol = "ADMINISTRADOR";

            SesionService.IniciarSesion(idEmpleado, usuario, rol);

            Assert.IsTrue(SesionService.EstaLogueado());
            Assert.AreEqual(idEmpleado, SesionService.ObtenerIdEmpleadoLogueado());
            Assert.AreEqual(usuario, SesionService.ObtenerUsuarioLogueado());
            Assert.AreEqual(rol, SesionService.ObtenerRolLogueado());
        }

        [TestMethod]
        // Test para verificar que se puede iniciar sesión con rol CAJERO
        public void IniciarSesion_ConRolCAJERO_DeberiaEstablecerSesionCorrectamente()
        {
            // Asegurar que no hay sesión activa
            SesionService.CerrarSesion();
            
            int idEmpleado = 2;
            string usuario = "cajero1";
            string rol = "CAJERO";

            SesionService.IniciarSesion(idEmpleado, usuario, rol);

            Assert.IsTrue(SesionService.EstaLogueado());
            Assert.AreEqual(rol, SesionService.ObtenerRolLogueado());
        }

        [TestMethod]
        // Test para verificar que se puede iniciar sesión con rol TECNICO
        public void IniciarSesion_ConRolTECNICO_DeberiaEstablecerSesionCorrectamente()
        {
            // Asegurar que no hay sesión activa
            SesionService.CerrarSesion();
            
            int idEmpleado = 3;
            string usuario = "tecnico1";
            string rol = "TECNICO";

            SesionService.IniciarSesion(idEmpleado, usuario, rol);

            Assert.IsTrue(SesionService.EstaLogueado());
            Assert.AreEqual(rol, SesionService.ObtenerRolLogueado());
        }

        [TestMethod]
        // Test para verificar que el ID del empleado se guarda correctamente
        public void IniciarSesion_DeberiaGuardarIdEmpleadoCorrectamente()
        {
            // Asegurar que no hay sesión activa
            SesionService.CerrarSesion();
            
            int idEmpleadoEsperado = 10;

            SesionService.IniciarSesion(idEmpleadoEsperado, "user", "CAJERO");

            Assert.AreEqual(idEmpleadoEsperado, SesionService.ObtenerIdEmpleadoLogueado());
        }

        [TestMethod]
        // Test para verificar que el nombre de usuario se guarda correctamente
        public void IniciarSesion_DeberiaGuardarUsuarioCorrectamente()
        {
            // Asegurar que no hay sesión activa
            SesionService.CerrarSesion();
            
            string usuarioEsperado = "admin";

            SesionService.IniciarSesion(1, usuarioEsperado, "ADMINISTRADOR");

            Assert.AreEqual(usuarioEsperado, SesionService.ObtenerUsuarioLogueado());
        }

        // ========== PRUEBAS DE CIERRE DE SESIÓN ==========

        [TestMethod]
        // Test para verificar que se puede cerrar sesión correctamente
        public void CerrarSesion_DeberiaCerrarSesionActiva()
        {
            // Primero iniciar sesión
            SesionService.IniciarSesion(1, "admin", "ADMINISTRADOR");
            Assert.IsTrue(SesionService.EstaLogueado());

            // Luego cerrar sesión
            SesionService.CerrarSesion();

            Assert.IsFalse(SesionService.EstaLogueado());
        }

        [TestMethod]
        // Test para verificar que después de cerrar sesión no hay datos de usuario
        public void CerrarSesion_DeberiaLimpiarDatosUsuario()
        {
            SesionService.IniciarSesion(1, "admin", "ADMINISTRADOR");
            SesionService.CerrarSesion();

            Assert.AreEqual(0, SesionService.ObtenerIdEmpleadoLogueado());
            Assert.AreEqual(string.Empty, SesionService.ObtenerUsuarioLogueado());
            Assert.AreEqual(string.Empty, SesionService.ObtenerRolLogueado());
        }

        [TestMethod]
        // Test para verificar que se puede cerrar sesión sin tener sesión activa
        public void CerrarSesion_SinSesionActiva_NoDeberiaLanzarExcepcion()
        {
            try
            {
                SesionService.CerrarSesion();
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.Fail("No debería lanzar excepción");
            }
        }

        // ========== PRUEBAS DE ESTADO DE SESIÓN ==========

        [TestMethod]
        // Test para verificar que sin sesión activa EstaLogueado retorna false
        public void EstaLogueado_SinSesionActiva_DeberiaRetornarFalse()
        {
            Assert.IsFalse(SesionService.EstaLogueado());
        }

        [TestMethod]
        // Test para verificar que con sesión activa EstaLogueado retorna true
        public void EstaLogueado_ConSesionActiva_DeberiaRetornarTrue()
        {
            SesionService.IniciarSesion(1, "admin", "ADMINISTRADOR");

            Assert.IsTrue(SesionService.EstaLogueado());
        }

        [TestMethod]
        // Test para verificar que después de cerrar sesión EstaLogueado retorna false
        public void EstaLogueado_DespuesDeCerrarSesion_DeberiaRetornarFalse()
        {
            SesionService.IniciarSesion(1, "admin", "ADMINISTRADOR");
            SesionService.CerrarSesion();

            Assert.IsFalse(SesionService.EstaLogueado());
        }

        // ========== PRUEBAS DE OBTENCIÓN DE DATOS ==========

        [TestMethod]
        // Test para verificar que sin sesión el ID de empleado retorna 0
        public void ObtenerIdEmpleadoLogueado_SinSesion_DeberiaRetornarCero()
        {
            int idObtenido = SesionService.ObtenerIdEmpleadoLogueado();

            Assert.AreEqual(0, idObtenido);
        }

        [TestMethod]
        // Test para verificar que sin sesión el usuario retorna vacío
        public void ObtenerUsuarioLogueado_SinSesion_DeberiaRetornarVacio()
        {
            string usuarioObtenido = SesionService.ObtenerUsuarioLogueado();

            Assert.AreEqual(string.Empty, usuarioObtenido);
        }

        [TestMethod]
        // Test para verificar que sin sesión el rol retorna vacío
        public void ObtenerRolLogueado_SinSesion_DeberiaRetornarVacio()
        {
            string rolObtenido = SesionService.ObtenerRolLogueado();

            Assert.AreEqual(string.Empty, rolObtenido);
        }

        [TestMethod]
        // Test para verificar que se obtienen los datos correctos con sesión activa
        public void ObtenerDatosSesion_ConSesionActiva_DeberiaRetornarDatosCorrectos()
        {
            int idEsperado = 5;
            string usuarioEsperado = "usuario_test";
            string rolEsperado = "CAJERO";

            SesionService.IniciarSesion(idEsperado, usuarioEsperado, rolEsperado);

            Assert.AreEqual(idEsperado, SesionService.ObtenerIdEmpleadoLogueado());
            Assert.AreEqual(usuarioEsperado, SesionService.ObtenerUsuarioLogueado());
            Assert.AreEqual(rolEsperado, SesionService.ObtenerRolLogueado());
        }

        // ========== PRUEBAS DE CAMBIO DE SESIÓN ==========

        [TestMethod]
        // Test para verificar que se puede cambiar de usuario sin cerrar sesión
        public void IniciarSesion_ConSesionActiva_DeberiaReemplazarSesion()
        {
            // Primera sesión
            SesionService.IniciarSesion(1, "usuario1", "CAJERO");
            
            // Segunda sesión (reemplaza la primera)
            SesionService.IniciarSesion(2, "usuario2", "ADMINISTRADOR");

            Assert.AreEqual(2, SesionService.ObtenerIdEmpleadoLogueado());
            Assert.AreEqual("usuario2", SesionService.ObtenerUsuarioLogueado());
            Assert.AreEqual("ADMINISTRADOR", SesionService.ObtenerRolLogueado());
        }

        [TestMethod]
        // Test para verificar que al cambiar sesión se pierden los datos anteriores
        public void IniciarSesion_AlReemplazar_DeberiaPerderDatosAnteriores()
        {
            SesionService.IniciarSesion(1, "admin", "ADMINISTRADOR");
            SesionService.IniciarSesion(2, "cajero", "CAJERO");

            Assert.AreNotEqual(1, SesionService.ObtenerIdEmpleadoLogueado());
            Assert.AreNotEqual("admin", SesionService.ObtenerUsuarioLogueado());
            Assert.AreNotEqual("ADMINISTRADOR", SesionService.ObtenerRolLogueado());
        }

        // ========== PRUEBAS DE VALIDACIONES ==========

        [TestMethod]
        // Test para verificar que el rol se guarda en mayúsculas
        public void IniciarSesion_DeberiaGuardarRolCorrectamente()
        {
            string rolEsperado = "ADMINISTRADOR";

            SesionService.IniciarSesion(1, "admin", rolEsperado);

            Assert.AreEqual(rolEsperado, SesionService.ObtenerRolLogueado());
        }

        [TestMethod]
        // Test para verificar múltiples inicios y cierres de sesión
        public void MultiplesIniciosYCierres_DeberianFuncionarCorrectamente()
        {
            // Primera sesión
            SesionService.IniciarSesion(1, "user1", "CAJERO");
            Assert.IsTrue(SesionService.EstaLogueado());

            SesionService.CerrarSesion();
            Assert.IsFalse(SesionService.EstaLogueado());

            // Segunda sesión
            SesionService.IniciarSesion(2, "user2", "TECNICO");
            Assert.IsTrue(SesionService.EstaLogueado());

            SesionService.CerrarSesion();
            Assert.IsFalse(SesionService.EstaLogueado());
        }

        [TestMethod]
        // Test para verificar que el ID de empleado es un número válido
        public void IniciarSesion_ConIdValido_DeberiaSerMayorACero()
        {
            SesionService.IniciarSesion(10, "admin", "ADMINISTRADOR");

            Assert.IsTrue(SesionService.ObtenerIdEmpleadoLogueado() > 0);
        }

        [TestMethod]
        // Test para verificar consistencia de datos durante la sesión
        public void DatosSesion_DeberianSerConsistentesDuranteSesion()
        {
            SesionService.IniciarSesion(5, "usuario_test", "CAJERO");

            // Verificar múltiples veces que los datos no cambian
            int id1 = SesionService.ObtenerIdEmpleadoLogueado();
            int id2 = SesionService.ObtenerIdEmpleadoLogueado();
            string usuario1 = SesionService.ObtenerUsuarioLogueado();
            string usuario2 = SesionService.ObtenerUsuarioLogueado();

            Assert.AreEqual(id1, id2);
            Assert.AreEqual(usuario1, usuario2);
        }
    }
}
