using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LOGIN
{
    public partial class Form1 : Form
    {
        // Conexión a MySQL (cambia "tu_password" por tu contraseña real)
        private string connectionString = "Server=localhost;Database=ReparacionCelularesDB;User=root;Password=0304;";

        public Form1()
        {
            InitializeComponent();

            // Configuración inicial
            txtUser.Text = "USUARIO";
            txtUser.ForeColor = Color.DimGray;

            txtPass.Text = "CONTRASEÑA";
            txtPass.ForeColor = Color.DimGray;
            txtPass.UseSystemPasswordChar = false;

            btnLogin.Click += BtnLogin_Click;
        }
        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            // Validación de campos vacíos
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
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT Email, Rol FROM Usuarios 
                           WHERE Email = @usuario AND Contraseña = @contraseña";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@usuario", txtUser.Text);
                    cmd.Parameters.AddWithValue("@contraseña", CalcularHashSHA256(txtPass.Text));

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Manejo directo sin métodos de extensión
                            Sesion.UsuarioActual = new Usuario()
                            {
                                Email = reader.IsDBNull(reader.GetOrdinal("Email"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("Email")),
                                Rol = reader.IsDBNull(reader.GetOrdinal("Rol"))
                                    ? "Usuario" // Valor por defecto si es NULL
                                    : reader.GetString(reader.GetOrdinal("Rol"))
                            };

                            this.Hide();
                            new MainForm().Show();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void RedirectToMainForm()
        {
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.Closed += (s, args) => this.Close(); // Cierra el login cuando se cierre el MainForm
            mainForm.Show();
        }

        private void ShowErrorMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void LogError(Exception ex)
        {
            // Implementa tu propio sistema de logging aquí
            // Ejemplo: File.AppendAllText("log.txt", $"{DateTime.Now}: {ex.ToString()}{Environment.NewLine}");
        }
        // Eventos existentes (no los modifiques)
        private void txtUser_Enter(object sender, EventArgs e)
        {
            if (txtUser.Text == "USUARIO")
            {
                txtUser.Text = "";
                txtUser.ForeColor = Color.Green; // Color más visible para texto real
            }
        }
        private void txtUser_Leave(object sender, EventArgs e)
        {
            // Restaura el placeholder si está vacío
            if (string.IsNullOrWhiteSpace(txtUser.Text))
            {
                txtUser.Text = "USUARIO";
                txtUser.ForeColor = Color.DimGray; // Color para placeholder
            }
        }
        private void txtPass_Enter(object sender, EventArgs e)
        {
            if (txtPass.Text == "CONTRASEÑA")
            {
                txtPass.Text = "";
                txtPass.ForeColor = Color.Green;
                txtPass.UseSystemPasswordChar = true;
            }
        }

        private void txtPass_Leave(object sender, EventArgs e)
        {
            // Restaura el placeholder si está vacío
            if (string.IsNullOrWhiteSpace(txtPass.Text))
            {
                txtPass.Text = "CONTRASEÑA";
                txtPass.ForeColor = Color.DimGray;
                txtPass.UseSystemPasswordChar = false; // Desactiva los asteriscos
            }
        }
        private void btnCerrar_Click(object sender, EventArgs e) { Application.Exit(); }
        private void btnMinimizar_Click(object sender, EventArgs e) { this.WindowState = FormWindowState.Minimized; }
        private void Form1_MouseDown(object sender, MouseEventArgs e) { /* Tu código actual */ }

        private string CalcularHashSHA256(string input)
        {
            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}