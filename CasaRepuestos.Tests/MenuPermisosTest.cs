using Microsoft.VisualStudio.TestTools.UnitTesting;
using CasaRepuestos.Services;

namespace CasaRepuestos.Tests
{


    [TestClass]
    public class MenuServiceTests
    {
        public LoginService _service;

        // TEST LOGIN

        [TestInitialize]
        public void Setup()
        {
            _service = new LoginService();
        }


        [TestMethod]
        public void ValidarCredenciales_ConUsuarioVacio_DeberiaRetornarFalse()
        {

            bool resultado = _service.ValidarCredenciales("", "contraseña123");


            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void ValidarCredenciales_ConContraseniaVacia_DeberiaRetornarFalse()
        {

            bool resultado = _service.ValidarCredenciales("usuario1", "");


            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void ValidarCredenciales_ConCredencialesInvalidas_DeberiaRetornarFalse()
        {

            bool resultado = _service.ValidarCredenciales("sarasa", "1234");


            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void ObtenerRol_ConUsuarioVacio_DeberiaRetornarVacio()
        {

            string rol = _service.ObtenerRol("");


            Assert.AreEqual(string.Empty, rol);
        }

        [TestMethod]
        public void ObtenerRol_ConUsuarioInexistente_DeberiaRetornarVacio()
        {

            string rol = _service.ObtenerRol("usuarionoexiste");


            Assert.AreEqual(string.Empty, rol);
        }

        [TestMethod]
        public void ValidarCredenciales_ConCredencialesValidas_DeberiaRetornarTrue()
        {
            bool resultado = _service.ValidarCredenciales("admin", "admin123");
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void ObtenerRol_ConUsuarioExistente_DeberiaRetornarRol()
        {
            string rol = _service.ObtenerRol("admin");
            Assert.IsFalse(string.IsNullOrEmpty(rol));
        }


        // TEST MENU PERMISOS

        // Ventas: lo pueden ver CAJERO y ADMINISTRADOR
        [DataTestMethod]
        [DataRow("CAJERO", true)]
        [DataRow("ADMINISTRADOR", true)]
        [DataRow("TECNICO", false)]
        public void PuedeVerVentas_SegunRol(string rol, bool esperado)
        {
            Assert.AreEqual(esperado, MenuService.PuedeVerVentas(rol));
        }

        // Compras: solo ADMINISTRADOR
        [DataTestMethod]
        [DataRow("ADMINISTRADOR", true)]
        [DataRow("CAJERO", false)]
        [DataRow("TECNICO", false)]
        public void PuedeVerCompras_SoloAdmin(string rol, bool esperado)
        {
            Assert.AreEqual(esperado, MenuService.PuedeVerCompras(rol));
        }

        // Reparaciones: lo pueden ver TECNICO y ADMINISTRADOR
        [DataTestMethod]
        [DataRow("TECNICO", true)]
        [DataRow("ADMINISTRADOR", true)]
        [DataRow("CAJERO", false)]
        public void PuedeVerReparaciones_SegunRol(string rol, bool esperado)
        {
            Assert.AreEqual(esperado, MenuService.PuedeVerReparaciones(rol));
        }

        // Productos: solo ADMINISTRADOR
        [DataTestMethod]
        [DataRow("ADMINISTRADOR", true)]
        [DataRow("CAJERO", false)]
        [DataRow("TECNICO", false)]
        public void PuedeVerProductos_SoloAdmin(string rol, bool esperado)
        {
            Assert.AreEqual(esperado, MenuService.PuedeVerProductos(rol));
        }

        // Inventario: solo ADMINISTRADOR
        [DataTestMethod]
        [DataRow("ADMINISTRADOR", true)]
        [DataRow("CAJERO", false)]
        [DataRow("TECNICO", false)]
        public void PuedeVerInventario_SoloAdmin(string rol, bool esperado)
        {
            Assert.AreEqual(esperado, MenuService.PuedeVerInventario(rol));
        }

        // Arqueo de caja: solo ADMINISTRADOR
        [DataTestMethod]
        [DataRow("ADMINISTRADOR", true)]
        [DataRow("CAJERO", true)]
        [DataRow("TECNICO", false)]
        public void PuedeVerArqueo_SoloAdmin(string rol, bool esperado)
        {
            Assert.AreEqual(esperado, MenuService.PuedeVerArqueo(rol));
        }

        // Configuración del sistema: solo ADMINISTRADOR
        [DataTestMethod]
        [DataRow("ADMINISTRADOR", true)]
        [DataRow("CAJERO", false)]
        [DataRow("TECNICO", false)]
        public void PuedeVerConfiguracion_SoloAdmin(string rol, bool esperado)
        {
            Assert.AreEqual(esperado, MenuService.PuedeVerConfiguracion(rol));
        }

        // Marca: solo ADMINISTRADOR
        [DataTestMethod]
        [DataRow("ADMINISTRADOR", true)]
        [DataRow("CAJERO", false)]
        [DataRow("TECNICO", false)]
        public void PuedeVerMarca_SoloAdmin(string rol, bool esperado)
        {
            Assert.AreEqual(esperado, MenuService.PuedeVerMarca(rol));
        }
    }
}
