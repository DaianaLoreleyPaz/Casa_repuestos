using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CasaRepuestos.Models;
using CasaRepuestos.Services;

namespace CasaRepuestos.Forms
{
    public partial class FrmProveedor : Form
    {
        private ProveedorService servicio = new ProveedorService();
        private int? proveedorEditandoId = null;
        private readonly string placeholderBuscar = "Buscar por CUIT, Documento o Razón Social";

        public FrmProveedor()
        {
            InitializeComponent();

            // Inicializaciones
            CargarTipoDocumento();
            CargarProveedores();

            // Aplicar estilos 
            ApplyRuntimeStyles();

            this.Load += FrmProveedor_Load;

            // Estado inicial botones
            btnEditar.Visible = false;
            btnGuardar.Visible = true;
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
            try { btnEditar.BackColor = Color.FromArgb(12, 87, 150); } catch { }
            try { btnLimpiar.BackColor = Color.FromArgb(192, 57, 43); } catch { }

            // Estilos para DataGridView
            foreach (var dgv in new[] { dgvProveedores })
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
            try
            {

            }
            catch { }
        }

        #region Cargas y utilidades
        private void CargarTipoDocumento()
        {
            cmbTipoDoc.Items.Clear();
            cmbTipoDoc.Items.AddRange(new string[] { "DNI", "LC", "LE", "PASAPORTE" });
            cmbTipoDoc.SelectedIndex = 0;
        }

        private void CargarProveedores()
        {
            try
            {
                dgvProveedores.AutoGenerateColumns = false;
                dgvProveedores.Columns.Clear();
                dgvProveedores.ReadOnly = true;
                ConfigurarGrilla();
                AgregarColumnaEliminar();
                AgregarColumnaEditar();
                dgvProveedores.DataSource = null;

                var lista = servicio.ListarProveedores() ?? new List<Proveedor>();
                dgvProveedores.DataSource = lista;
                // OCULTAR ID
                if (dgvProveedores.Columns.Contains("IdProveedor"))
                    dgvProveedores.Columns["IdProveedor"].Visible = false;

                dgvProveedores.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando proveedores: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarGrilla()
        {
            dgvProveedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IdProveedor",
                HeaderText = "Nº Proveedor",
                DataPropertyName = "IdProveedor",
                ReadOnly = true,
                FillWeight = 10
            });

            dgvProveedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RazonSocial",
                HeaderText = "Razón Social",
                DataPropertyName = "RazonSocial",
                FillWeight = 30
            });

            dgvProveedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NumeroDocumento",
                HeaderText = "Nº Documento",
                DataPropertyName = "NumeroDocumento",
                FillWeight = 15
            });

            dgvProveedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Cuil",
                HeaderText = "CUIT",
                DataPropertyName = "Cuil",
                FillWeight = 15
            });

            dgvProveedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Telefono",
                HeaderText = "Teléfono",
                DataPropertyName = "Telefono",
                FillWeight = 15
            });
        }

        private void AgregarColumnaEditar()
        {
            if (!dgvProveedores.Columns.Contains("btnEditar"))
            {
                var col = new DataGridViewButtonColumn();
                col.Name = "btnEditar";
                col.HeaderText = "Editar";
                col.Text = "Editar";
                col.UseColumnTextForButtonValue = true;
                col.FillWeight = 8;
                dgvProveedores.Columns.Add(col);
            }
        }

        private void AgregarColumnaEliminar()
        {
            if (!dgvProveedores.Columns.Contains("btnEliminar"))
            {
                var col = new DataGridViewButtonColumn();
                col.Name = "btnEliminar";
                col.HeaderText = "Eliminar";
                col.Text = "Eliminar";
                col.UseColumnTextForButtonValue = true;
                col.FillWeight = 8;
                dgvProveedores.Columns.Add(col);
            }
        }

        #endregion

        #region Validaciones y acciones

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtDocumento.Text) ||
                string.IsNullOrWhiteSpace(txtCuil.Text) ||
                string.IsNullOrWhiteSpace(txtRazonSocial.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                MessageBox.Show("Todos los campos obligatorios deben completarse.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!txtDocumento.Text.All(char.IsDigit) || txtDocumento.Text.Length != 8)
            {
                MessageBox.Show("El número de documento debe contener exactamente 8 dígitos.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!txtCuil.Text.All(char.IsDigit) || txtCuil.Text.Length != 11)
            {
                MessageBox.Show("El CUIT debe contener exactamente 11 dígitos.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            var persona = new Persona
            {
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                TipoDocumento = cmbTipoDoc.SelectedItem?.ToString() ?? "DNI",
                NumeroDocumento = txtDocumento.Text,
                Telefono = txtTelefono.Text,
                Email = txtEmail.Text,
                Direccion = txtDireccion.Text
            };

            var proveedor = new Proveedor
            {
                Cuil = txtCuil.Text,
                RazonSocial = txtRazonSocial.Text,
                DatosPersona = persona
            };

            if (proveedorEditandoId == null)
            {
                try
                {
                    servicio.CrearProveedor(proveedor);
                    MessageBox.Show("Proveedor creado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al crear proveedor: " + ex.Message);
                }
            }
            else
            {
                var proveedorExistente = servicio.ListarProveedores()
                    .FirstOrDefault(p => p.IdProveedor == proveedorEditandoId.Value);

                if (proveedorExistente == null)
                {
                    MessageBox.Show("No se encontró el proveedor a modificar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                proveedor.IdProveedor = proveedorExistente.IdProveedor;
                proveedor.IdPersona = proveedorExistente.IdPersona;
                proveedor.DatosPersona.IdPersona = proveedorExistente.IdPersona;

                try
                {
                    servicio.ModificarProveedor(proveedor);
                    MessageBox.Show("Proveedor modificado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    proveedorEditandoId = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al modificar proveedor: " + ex.Message);
                }
            }

            Limpiar();
            CargarProveedores();
            tabControl1.SelectedTab = tabListado;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            if (proveedorEditandoId == null)
            {
                MessageBox.Show("No hay proveedor seleccionado para editar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var persona = new Persona
            {
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                TipoDocumento = cmbTipoDoc.SelectedItem?.ToString() ?? "DNI",
                NumeroDocumento = txtDocumento.Text,
                Telefono = txtTelefono.Text,
                Email = txtEmail.Text,
                Direccion = txtDireccion.Text
            };

            var proveedor = new Proveedor
            {
                IdProveedor = proveedorEditandoId.Value,
                Cuil = txtCuil.Text,
                RazonSocial = txtRazonSocial.Text,
                DatosPersona = persona
            };

            var proveedorExistente = servicio.ListarProveedores()
                .FirstOrDefault(p => p.IdProveedor == proveedorEditandoId.Value);

            if (proveedorExistente == null)
            {
                MessageBox.Show("No se encontró el proveedor a modificar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            proveedor.IdPersona = proveedorExistente.IdPersona;
            proveedor.DatosPersona.IdPersona = proveedorExistente.IdPersona;

            try
            {
                servicio.ModificarProveedor(proveedor);
                MessageBox.Show("Proveedor modificado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar proveedor: " + ex.Message);
            }

            proveedorEditandoId = null;
            Limpiar();
            CargarProveedores();
            tabControl1.SelectedTab = tabListado;
        }

        private void Limpiar()
        {
            LimpiarControles(this);

            cmbTipoDoc.SelectedIndex = 0;

            btnGuardar.Visible = true;
            btnEditar.Visible = false;
            proveedorEditandoId = null;
        }

        private void LimpiarControles(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox txt)
                    txt.Clear();

                if (c is ComboBox cb)
                    cb.SelectedIndex = 0;


                if (c.HasChildren)
                    LimpiarControles(c);
            }
        }


        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();

        }

        #endregion

        #region Eventos UI

        private bool esTextoPlaceholder = true;

        private void FrmProveedor_Load(object sender, EventArgs e)
        {
            MostrarPlaceholder();
        }

        private void MostrarPlaceholder()
        {
            txtBuscar.Text = placeholderBuscar;
            txtBuscar.ForeColor = Color.Gray;
            esTextoPlaceholder = true;
        }

        private void txtBuscar_Enter(object sender, EventArgs e)
        {
            if (esTextoPlaceholder)
            {
                txtBuscar.Text = "";
                txtBuscar.ForeColor = Color.Black;
                esTextoPlaceholder = false;
            }
        }

        private void txtBuscar_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
                MostrarPlaceholder();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (esTextoPlaceholder) return;

            string filtro = txtBuscar.Text.ToLower();
            var proveedores = servicio.ListarProveedores() ?? new List<Proveedor>();

            var filtrados = proveedores.Where(p =>
                (p.RazonSocial ?? "").ToLower().Contains(filtro) ||
                (p.DatosPersona?.NumeroDocumento ?? "").ToLower().Contains(filtro) ||
                (p.Cuil ?? "").ToLower().Contains(filtro)).ToList();

            dgvProveedores.DataSource = null;
            dgvProveedores.DataSource = filtrados;
            dgvProveedores.ClearSelection();
        }


        private void dgvProveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Evita clicks fuera de celdas válidas
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;


            if (dgvProveedores.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                int id = Convert.ToInt32(dgvProveedores.Rows[e.RowIndex].Cells["IdProveedor"].Value);
                if (MessageBox.Show("¿Eliminar proveedor?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    servicio.EliminarProveedor(id);
                    CargarProveedores();
                }
            }
            else if (dgvProveedores.Columns[e.ColumnIndex].Name == "btnEditar")
            {
                int idProveedor = Convert.ToInt32(dgvProveedores.Rows[e.RowIndex].Cells["IdProveedor"].Value);

                var proveedor = servicio.ListarProveedores().FirstOrDefault(p => p.IdProveedor == idProveedor);
                if (proveedor == null) return;

                proveedorEditandoId = idProveedor;

                txtNombre.Text = proveedor.DatosPersona?.Nombre;
                txtApellido.Text = proveedor.DatosPersona?.Apellido;
                txtDocumento.Text = proveedor.DatosPersona?.NumeroDocumento;
                txtTelefono.Text = proveedor.DatosPersona?.Telefono;
                txtEmail.Text = proveedor.DatosPersona?.Email;
                txtDireccion.Text = proveedor.DatosPersona?.Direccion;
                txtCuil.Text = proveedor.Cuil;
                txtRazonSocial.Text = proveedor.RazonSocial;
                cmbTipoDoc.SelectedItem = proveedor.DatosPersona?.TipoDocumento ?? "DNI";

                // Mostrar botón Editar y ocultar Guardar
                btnEditar.Visible = true;
                btnGuardar.Visible = false;


            }
        }

        #endregion


    }
}
