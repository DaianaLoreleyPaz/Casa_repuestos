using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaRepuestos.Services
{
    public class MenuService
    {

        public static bool PuedeVerVentas(string rol) =>
            rol == "CAJERO" || rol == "ADMINISTRADOR";

        public static bool PuedeVerCompras(string rol) =>
            rol == "ADMINISTRADOR";
        public static bool PuedeVerProveedor(string rol) =>
            rol == "ADMINISTRADOR";

        public static bool PuedeVerClientes(string rol) =>
            rol == "CAJERO" || rol == "ADMINISTRADOR";

        public static bool PuedeVerProductos(string rol) =>
            rol == "ADMINISTRADOR";

        public static bool PuedeVerInventario(string rol) =>
            rol == "ADMINISTRADOR";

        public static bool PuedeVerReparaciones(string rol) =>
            rol == "TECNICO" || rol == "ADMINISTRADOR";

        public static bool PuedeVerIngresos(string rol) =>
           rol == "CAJERO" || rol == "ADMINISTRADOR";

        public static bool PuedeVerServicios(string rol) =>
           rol == "ADMINISTRADOR";

        public static bool PuedeVerPresupuesto(string rol) =>
            rol == "TECNICO" || rol == "ADMINISTRADOR";

        public static bool PuedeVerArqueo(string rol) =>
            rol == "CAJERO" || rol == "ADMINISTRADOR";

        public static bool PuedeVerConfiguracion(string rol) =>
            rol == "ADMINISTRADOR";

        public static bool PuedeVerReportes(string rol) =>
            rol == "ADMINISTRADOR";
        public static bool PuedeVerMarca(string rol) =>
            rol == "ADMINISTRADOR";
    }
}