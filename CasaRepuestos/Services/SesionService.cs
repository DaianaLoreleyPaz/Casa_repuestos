using CasaRepuestos.Models;

namespace CasaRepuestos.Services
{
    public static class SesionService
    {
        public static Empleado EmpleadoLogueado { get; private set; }

        public static void IniciarSesion(int idEmpleado, string usuario, string rol)
        {
            EmpleadoLogueado = new Empleado
            {
                IdEmpleado = idEmpleado,
                Usuario = usuario,
                Rol = rol
            };
        }

        public static void CerrarSesion()
        {
            EmpleadoLogueado = null;
        }

        public static bool EstaLogueado()
        {
            return EmpleadoLogueado != null;
        }

        public static int ObtenerIdEmpleadoLogueado()
        {
            return EstaLogueado() ? EmpleadoLogueado.IdEmpleado : 0;
        }

        public static string ObtenerUsuarioLogueado()
        {
            return EstaLogueado() ? EmpleadoLogueado.Usuario : string.Empty;
        }

        public static string ObtenerRolLogueado()
        {
            return EstaLogueado() ? EmpleadoLogueado.Rol : string.Empty;
        }
    }
}