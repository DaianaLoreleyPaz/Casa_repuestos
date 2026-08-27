using CasaRepuestos.Models;
using CasaRepuestos.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CasaRepuestos
{
    public partial class FrmEmpleados : Form
    {
        private EmpleadoService servicio = new EmpleadoService();
        private string placeholder = "Buscar por Nombre o Usuario";

        public FrmEmpleados()
        {
            InitializeComponent();

            // Aplicar estilos runtime 
            ApplyRuntimeStyles();

            // Inicializar datos
            CargarTiposDocumento();
            CargarEmpleados();
            // Configurar DataGridView
            dgvEmpleados.ReadOnly = true;
            dgvEmpleados.AllowUserToAddRows = false;
            dgvEmpleados.AllowUserToDeleteRows = false;
            dgvEmpleados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmpleados.MultiSelect = false;

            CargarRoles();

            // Eventos
            this.Load += FrmEmpleados_Load;
            dgvEmpleados.CellClick -= dgvEmpleados_CellClick;
            dgvEmpleados.CellClick += dgvEmpleados_CellClick;
        }

        private void ApplyRuntimeStyles()
        {


            this.BackColor = Color.FromArgb(240, 245, 249);

            // Estilos para GroupBoxes
            foreach (var gb in this.Controls.OfType<GroupBox>().Concat(this.Controls.OfType<Panel>().SelectMany(p => p.Controls.OfType<GroupBox>())))
            {
                gb.BackColor = Color.White;
                gb.ForeColor = Color.FromArgb(45, 66, 91);
            }

            // Estilos para Botones
            foreach (var btn in this.Controls.OfType<Button>().Concat(this.Controls.OfType<Panel>().SelectMany(p => p.Controls.OfType<Button>())))
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Color.FromArgb(76, 132, 200);
                btn.ForeColor = Color.White;
            }

            // Estilos para botones 
            try { btnGuardar.BackColor = Color.FromArgb(34, 139, 34); btnGuardar.ForeColor = Color.White; } catch { }
            try { btnLimpiar.BackColor = Color.FromArgb(192, 57, 43); } catch { }

            // Estilos para DataGridView
            foreach (var dgv in new[] { dgvEmpleados })
            {
                if (dgv != null)
                {
                    dgv.BackgroundColor = Color.White;
                    dgv.EnableHeadersVisualStyles = false;

                    // Estilo para el encabezado del DGV
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 66, 91);
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                }
            }

            try
            {
                
            }
            catch { }
        }

        private void FrmEmpleados_Load(object sender, EventArgs e)
        {
            // Asegurar placeholder al cargar
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                txtBuscar.Text = placeholder;
                txtBuscar.ForeColor = Color.Gray;
            }
        }
        private void AgregarColumnaEditar()
        {
            if (!dgvEmpleados.Columns.Contains("btnEditar"))
            {
                var colBtn = new DataGridViewButtonColumn
                {
                    Name = "btnEditar",
                    HeaderText = "Editar",
                    Text = "Editar",
                    UseColumnTextForButtonValue = true,
                    Width = 80
                };
                dgvEmpleados.Columns.Add(colBtn);
            }
        }


        private void CargarEmpleados()
        {
            try
            {
                dgvEmpleados.AutoGenerateColumns = false;
                dgvEmpleados.Columns.Clear();

                ConfigurarGrilla();            
                AgregarColumnaEditar();
                AgregarColumnaEliminar();

                var lista = servicio.ListarEmpleados() ?? Enumerable.Empty<Empleado>();
                dgvEmpleados.DataSource = lista.ToList();

                // Limpiar selección por defecto
                dgvEmpleados.ClearSelection();
                // no mostrar id empleado
                dgvEmpleados.Columns["IdEmpleado"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar empleados: " + ex.Message);
            }
        }

        private void ConfigurarGrilla()
        {
            dgvEmpleados.Columns.Clear();

            dgvEmpleados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IdEmpleado",
                HeaderText = "ID",
                DataPropertyName = "IdEmpleado",
                ReadOnly = true,
                Visible = false
            });

            dgvEmpleados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Usuario",
                HeaderText = "Usuario",
                DataPropertyName = "Usuario",
                ReadOnly = false
            });

            dgvEmpleados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Contrasenia",
                HeaderText = "Contraseña",
                DataPropertyName = "Contrasenia",
                ReadOnly = false
            });

            dgvEmpleados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Rol",
                HeaderText = "Rol",
                DataPropertyName = "Rol",
                ReadOnly = false
            });

            dgvEmpleados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NombreCompleto",
                HeaderText = "Nombre completo",
                DataPropertyName = "NombreCompleto",
                ReadOnly = false,
                Width = 220
            });
        }
        // Cargar roles en el ComboBox
        private void CargarRoles()
        {
            cmbRol.Items.Clear();
            cmbRol.Items.Add("ADMINISTRADOR");
            cmbRol.Items.Add("TECNICO");
            cmbRol.Items.Add("CAJERO");
            cmbRol.SelectedIndex = 0;
        }
        // Cargar tipos de documento en el ComboBox
        private void CargarTiposDocumento()
        {
            cmbTipoDoc.Items.Clear();
            cmbTipoDoc.Items.Add("DNI");
            cmbTipoDoc.Items.Add("LC");
            cmbTipoDoc.Items.Add("LE");
            cmbTipoDoc.Items.Add("PASAPORTE");
            cmbTipoDoc.SelectedIndex = 0;
        }
        // Validar campos del formulario
        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtNroDoc.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtDireccion.Text) ||
                string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtContrasenia.Text))
            {
                MessageBox.Show("Todos los campos obligatorios deben completarse", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!txtNombre.Text.All(c => char.IsLetter(c) || c == ' '))
            {
                MessageBox.Show("El nombre solo debe contener letras y espacios", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!txtApellido.Text.All(c => char.IsLetter(c) || c == ' '))
            {
                MessageBox.Show("El apellido solo debe contener letras y espacios", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!txtNroDoc.Text.All(char.IsDigit) || txtNroDoc.Text.Length != 8)
            {
                MessageBox.Show("El número de documento debe contener exactamente 8 dígitos.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!txtTelefono.Text.All(char.IsDigit) || txtTelefono.Text.Length != 10)
            {
                MessageBox.Show("El teléfono debe contener exactamente 10 dígitos.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                var mail = new System.Net.Mail.MailAddress(txtEmail.Text);
            }
            catch
            {
                MessageBox.Show("Email inválido.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        // Guardar empleado (crear o modificar)
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            var persona = new Persona
            {
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                TipoDocumento = cmbTipoDoc.SelectedItem?.ToString(),
                NumeroDocumento = txtNroDoc.Text,
                Telefono = txtTelefono.Text,
                Email = txtEmail.Text,
                Direccion = txtDireccion.Text
            };

            var empleado = new Empleado
            {
                Usuario = txtUsuario.Text,
                Contrasenia = txtContrasenia.Text,
                Rol = cmbRol.SelectedItem?.ToString(),
                DatosPersona = persona
            };

            // edición
            if (txtUsuario.Tag != null)
            {
                empleado.IdEmpleado = (int)txtUsuario.Tag;
                servicio.ModificarEmpleado(empleado);
                MessageBox.Show("Empleado actualizado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                servicio.CrearEmpleado(empleado);
                MessageBox.Show("Empleado creado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            txtUsuario.Tag = null; 
            LimpiarFormulario();
            CargarEmpleados();
            tabControl1.SelectedTab = tabLista;
        }


        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtContrasenia.Clear();
            txtNroDoc.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            txtEmail.Clear();
            txtUsuario.Clear();
            cmbTipoDoc.SelectedIndex = 0;
            cmbRol.SelectedIndex = 0;
        }
        // Ver lista de empleados
        private void btnVerLista_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabLista;
            CargarEmpleados();
        }


        // Agregar columna de eliminar al DataGridView
        private void AgregarColumnaEliminar()
        {
            if (!dgvEmpleados.Columns.Contains("btnEliminar"))
            {
                var colBtn = new DataGridViewButtonColumn
                {
                    Name = "btnEliminar",
                    HeaderText = "Acciones",
                    Text = "Eliminar",
                    UseColumnTextForButtonValue = true,
                    FillWeight = 12
                };
                dgvEmpleados.Columns.Add(colBtn);
            }
        }
        // Manejar clics en las celdas del DataGridView
        private void dgvEmpleados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string columnName = dgvEmpleados.Columns[e.ColumnIndex].Name;

            // --- ELIMINAR ---
            if (columnName == "btnEliminar")
            {
                int id = Convert.ToInt32(dgvEmpleados.Rows[e.RowIndex].Cells["IdEmpleado"].Value);

                if (MessageBox.Show("¿Desea eliminar este empleado?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    servicio.EliminarEmpleado(id);
                    MessageBox.Show("Empleado eliminado correctamente", "Operación exitosa",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarEmpleados();
                }
                return;
            }

            // --- EDITAR ---
            if (columnName == "btnEditar")
            {
                int idEmpleado = Convert.ToInt32(dgvEmpleados.Rows[e.RowIndex].Cells["IdEmpleado"].Value);

                var empleado = servicio.ListarEmpleados()?.FirstOrDefault(emp => emp.IdEmpleado == idEmpleado);
                if (empleado == null)
                {
                    MessageBox.Show("No se pudo cargar el empleado seleccionado");
                    return;
                }

                // Cargar datos al formulario
                txtNombre.Text = empleado.DatosPersona.Nombre;
                txtApellido.Text = empleado.DatosPersona.Apellido;
                txtNroDoc.Text = empleado.DatosPersona.NumeroDocumento;
                txtTelefono.Text = empleado.DatosPersona.Telefono;
                txtEmail.Text = empleado.DatosPersona.Email;
                txtDireccion.Text = empleado.DatosPersona.Direccion;
                cmbTipoDoc.SelectedItem = empleado.DatosPersona.TipoDocumento;

                txtUsuario.Text = empleado.Usuario;
                txtContrasenia.Text = empleado.Contrasenia;
                cmbRol.SelectedItem = empleado.Rol;

                // Guardar ID
                txtUsuario.Tag = idEmpleado;

                // Cambiar pestaña a edición
                tabControl1.SelectedTab = tabLista.Parent.Controls["tabLista"] as TabPage;
                tabControl1.SelectedTab = tabControl1.TabPages[0];

                
            }
        }

        // Buscar empleados por nombre o usuario
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (txtBuscar.Text == placeholder) return;

            string filtro = txtBuscar.Text.ToLower();
            var empleados = servicio.ListarEmpleados() ?? Enumerable.Empty<Empleado>();

            var filtrados = empleados.Where(emp =>
                (emp.DatosPersona?.Nombre?.ToLower().Contains(filtro) ?? false) ||
                (emp.Usuario?.ToLower().Contains(filtro) ?? false)).ToList();

            dgvEmpleados.DataSource = null;
            dgvEmpleados.DataSource = filtrados;
            dgvEmpleados.ClearSelection();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void txtBuscar_Enter(object sender, EventArgs e)
        {
            if (txtBuscar.Text == placeholder)
            {
                txtBuscar.Text = "";
                txtBuscar.ForeColor = Color.Black;
            }
        }

        private void txtBuscar_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                txtBuscar.Text = placeholder;
                txtBuscar.ForeColor = Color.Gray;
            }
        }
        
    }
}
