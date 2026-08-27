

namespace CasaRepuestos.Config
{
    public static class Config
    {
        public static string ConnectionString { get; } =
            "Server=localhost;" +
            "Database=casarepuestos;" +
            "Uid=tu_usuario;" +
            "Pwd=tu_contraseña;" +
            "AllowUserVariables=True;" +
            "UseAffectedRows=False;" +
            "SslMode=None;" +
            "AllowPublicKeyRetrieval=True;";
    }
}
