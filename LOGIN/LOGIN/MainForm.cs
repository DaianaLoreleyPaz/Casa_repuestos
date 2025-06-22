namespace LOGIN
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ConfigurarInterfazSegunRol();
        }

        private void ConfigurarInterfazSegunRol()
        {
            // Configuración básica
            this.Text = "Sistema de Reparaciones - " + Sesion.UsuarioActual.Email;

            // Label de bienvenida
            Label lblBienvenida = new Label();
            lblBienvenida.Text = $"¡Bienvenido {Sesion.UsuarioActual.Email}!";
            lblBienvenida.Font = new Font("Arial", 16);
            lblBienvenida.AutoSize = true;
            lblBienvenida.Location = new Point(50, 50);
            this.Controls.Add(lblBienvenida);

            // Botón de admin (solo visible para administradores)
            Button btnAdmin = new Button();
            btnAdmin.Text = "Administración";
            btnAdmin.Location = new Point(50, 100);
            btnAdmin.Visible = false; // Por defecto oculto

            // Configuración según rol
            if (Sesion.UsuarioActual.Rol == "Administrador")
            {
                btnAdmin.Visible = true;
                // Puedes agregar más controles aquí
            }

            this.Controls.Add(btnAdmin);

            // Botón de cierre de sesión
            Button btnCerrarSesion = new Button();
            btnCerrarSesion.Text = "Cerrar sesión";
            btnCerrarSesion.Location = new Point(50, 150);
            btnCerrarSesion.Click += (s, e) => {
                this.Close();
                new Form1().Show();
            };
            this.Controls.Add(btnCerrarSesion);
        }
    }
}