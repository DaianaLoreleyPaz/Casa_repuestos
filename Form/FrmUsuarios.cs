using Casa_repuestos.Models;
using Casa_repuestos.Services;


namespace Casa_repuestos
{
    public partial class FrmUsuarios : Form
    {
        private UsuarioService servicio = new UsuarioService();
        private int? usuarioEditandoId = null; // null si es nuevo, contiene ID si estás editando
        public FrmUsuarios()
        {
            InitializeComponent();
            ObtenerPermisosSeleccionados();
            CargarPermisosDisponibles();
            CargarRoles();
            CargarUsuarios();
        }

        private void CargarRoles()
        {
            cmbRol.DataSource = servicio.ObtenerRoles();
            cmbRol.DisplayMember = "TipoRol";  // Esto se muestra en pantalla
            cmbRol.ValueMember = "IdRol";

            cmbRol.SelectedIndexChanged += (s, e) =>
            {
                MostrarPermisosDelRol();
            };

            if (cmbRol.Items.Count > 0)
                MostrarPermisosDelRol();
        }

        private void MostrarPermisosDelRol()
        {
            if (cmbRol.SelectedItem is Rol rolSeleccionado)
            {
                string[] permisos = rolSeleccionado.Permisos.Split(',');

                for (int i = 0; i < clbPermisos.Items.Count; i++)
                {
                    string permiso = clbPermisos.Items[i].ToString();
                    clbPermisos.SetItemChecked(i, permisos.Contains(permiso));
                }
            }
        }
        private void CargarUsuarios()
        {
            dgvUsuarios.DataSource = servicio.ListarUsuarios();
        }

        private void CargarPermisosDisponibles()
        {
            clbPermisos.Items.Clear();
            string[] permisos = { "VENTAS", "COMPRAS", "PRODUCTOS", "INVENTARIOS", "REPARACIONES", "ARQUEO", "CONFIGURACION" };

            foreach (string permiso in permisos)
            {
                clbPermisos.Items.Add(permiso);
            }
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var usuario = new Usuario
            {
                NombreUsuario = txtNombre.Text,
                Email = txtEmail.Text,
                Contrasena = txtContrasena.Text,
                IdRol = (int)cmbRol.SelectedValue
            };

            if (usuarioEditandoId == null)
            {
                // Nuevo
                servicio.CrearUsuario(usuario);
                MessageBox.Show("Usuario creado correctamente");
            }
            else
            {
                usuario.IdUsuario = usuarioEditandoId.Value;
                servicio.ModificarUsuario(usuario);
                MessageBox.Show("Usuario modificado correctamente");
                usuarioEditandoId = null;
            }

            LimpiarFormulario();
            CargarUsuarios();
        }

        private string ObtenerPermisosSeleccionados()
        {
            var seleccionados = new List<string>();

            foreach (string permiso in clbPermisos.CheckedItems)
            {
                seleccionados.Add(permiso);
            }

            return string.Join(",", seleccionados);
        }
        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtEmail.Clear();
            txtContrasena.Clear();
            cmbRol.SelectedIndex = -1;
            for (int i = 0; i < clbPermisos.Items.Count; i++)
                clbPermisos.SetItemChecked(i, false);
        }
        private void CargarUsuarioEnFormulario(Usuario usuario)
        {
            usuarioEditandoId = usuario.IdUsuario;

            txtNombre.Text = usuario.NombreUsuario;
            txtEmail.Text = usuario.Email;
            txtContrasena.Text = usuario.Contrasena;
            cmbRol.SelectedValue = usuario.IdRol;

            MostrarPermisosDelRol(); // marca los permisos en el checklist
        }
        
        private void btnActualizarPermisos_Click(object sender, EventArgs e)
        {
            if (cmbRol.SelectedItem is Rol rolSeleccionado)
            {
                string permisos = ObtenerPermisosSeleccionados();
                servicio.ActualizarPermisosRol(rolSeleccionado.IdRol, permisos);
                MessageBox.Show("Permisos actualizados correctamente para el rol: " + rolSeleccionado.TipoRol);
                CargarRoles(); // Opcional: recargar el combo si querés ver los cambios reflejados
            }
            else
            {
                MessageBox.Show("Seleccione un rol para actualizar los permisos.");
            }
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dgvUsuarios.Rows[e.RowIndex];
                var usuario = new Usuario
                {
                    IdUsuario = Convert.ToInt32(fila.Cells["IdUsuario"].Value),
                    NombreUsuario = fila.Cells["NombreUsuario"].Value.ToString(),
                    Email = fila.Cells["Email"].Value.ToString(),
                    Contrasena = fila.Cells["Contrasena"].Value.ToString(),
                    IdRol = Convert.ToInt32(fila.Cells["IdRol"].Value)
                };

                CargarUsuarioEnFormulario(usuario);
            }
        }
    }

}
