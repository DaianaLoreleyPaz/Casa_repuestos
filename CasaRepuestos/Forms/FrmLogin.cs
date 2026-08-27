
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;

namespace CasaRepuestos.Forms
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();

            // Configuración inicial
            txtUser.Text = "USUARIO";
            txtUser.ForeColor = Color.Silver;

            txtPass.Text = "CONTRASEÑA";
            txtPass.ForeColor = Color.Silver;
            txtPass.UseSystemPasswordChar = false;
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            RealizarLogin();
        }

        // Método para realizar el login
        private void RealizarLogin()
        {
            // Validación para campos vacíos
            if (txtUser.Text == "USUARIO" || txtPass.Text == "CONTRASEÑA" ||
                string.IsNullOrWhiteSpace(txtUser.Text) ||
                string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("Debe ingresar credenciales válidas",
                              "Campos incompletos",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Conexión a la base de datos para verificar credenciales
                using (MySqlConnection connection = new MySqlConnection(Config.Config.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM empleados WHERE usuario = @usuario AND contrasenia = @contrasenia";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@usuario", txtUser.Text);
                    cmd.Parameters.AddWithValue("@contrasenia", txtPass.Text);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        // obtener rol
                        string roleQuery = "SELECT rol FROM empleados WHERE usuario = @usuario";
                        MySqlCommand roleCmd = new MySqlCommand(roleQuery, connection);
                        roleCmd.Parameters.AddWithValue("@usuario", txtUser.Text);
                        string rol = roleCmd.ExecuteScalar()?.ToString() ?? "USUARIO";
                        string usuario = txtUser.Text;

                        // OBTENER idempleado del usuario logueado
                        string idQuery = "SELECT idempleado FROM empleados WHERE usuario = @usuario";
                        MySqlCommand idCmd = new MySqlCommand(idQuery, connection);
                        idCmd.Parameters.AddWithValue("@usuario", txtUser.Text);
                        object obj = idCmd.ExecuteScalar();
                        int empleadoId = 0;
                        if (obj != null && obj != DBNull.Value)
                        {
                            empleadoId = Convert.ToInt32(obj);
                        }

                        this.Hide();
                        // Pasamos ahora el id del empleado al menu
                        new FrmMenu(rol, usuario, empleadoId).Show();
                    }
                    else
                    {
                        MessageBox.Show("Credenciales incorrectas",
                                      "Error de autenticación",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }
        // Eventos para los placeholders
        private void txtUser_Enter(object sender, EventArgs e)
        {
            if (txtUser.Text == "USUARIO")
            {
                txtUser.Text = "";
                txtUser.ForeColor = Color.White;
            }
        }
        // Eventos para los placeholders
        private void txtUser_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text))
            {
                txtUser.Text = "USUARIO";
                txtUser.ForeColor = Color.Silver;
            }
        }
        // Eventos para los placeholders
        private void txtPass_Enter(object sender, EventArgs e)
        {
            if (txtPass.Text == "CONTRASEÑA")
            {
                txtPass.Text = "";
                txtPass.ForeColor = Color.White;
                txtPass.UseSystemPasswordChar = true;
            }
        }

        private void txtPass_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPass.Text))
            {
                txtPass.Text = "CONTRASEÑA";
                txtPass.ForeColor = Color.Silver;
                txtPass.UseSystemPasswordChar = false;
            }
        }
        // Evento para detectar la tecla Enter en el campo de contraseña
        private void txtPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                RealizarLogin();
                e.Handled = true;
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            // Código para arrastrar el formulario si es necesario
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
            }
        }

        // Importaciones para mover el formulario
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    }
}
