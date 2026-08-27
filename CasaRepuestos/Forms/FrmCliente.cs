using CasaRepuestos.Models;
using CasaRepuestos.Services;
using PdfSharp.Pdf.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CasaRepuestos.Forms
{
    public partial class FrmCliente : Form
    {
        private ClienteService clienteService = new ClienteService();
        private int? clienteEditandoId = null;
        private readonly string placeholderClientes = "Buscar por DNI, CUIL, Nombre o Apellido";
        private List<Cliente> listaClientes = new List<Cliente>();



        public FrmCliente()
        {
            InitializeComponent();
            AplicarEstilos();
        }

        private void FrmCliente_Load(object sender, EventArgs e)
        {
            CargarCategorias();
            CargarTipoDocumento();
            CargarClientes();

            txtBuscar.Text = placeholderClientes;
            txtBuscar.ForeColor = Color.Gray;
        }




        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Validaciones iniciales
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (e.ColumnIndex >= dgvClientes.Columns.Count) return;

            // Obtener nombre de la columna clickeada
            string nombreColumna = dgvClientes.Columns[e.ColumnIndex].Name;

            // --- ELIMINAR CLIENTE ---
            if (nombreColumna == "btnEliminar")
            {
                int id = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells["IdCliente"].Value);

                var confirmacion = MessageBox.Show(
                    "¿Eliminar cliente?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmacion == DialogResult.Yes)
                {
                    clienteService.EliminarCliente(id);
                    CargarClientes();
                }
            }
            // --- EDITAR CLIENTE ---
            else if (nombreColumna == "btnEditar")
            {
                int idCliente = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells["IdCliente"].Value);

                var cliente = clienteService.ListarClientes()
                    .FirstOrDefault(c => c.IdCliente == idCliente);

                if (cliente == null) return;

                clienteEditandoId = idCliente;

                // Cargar datos del cliente en el formulario
                txtNombre.Text = cliente.DatosPersona?.Nombre ?? string.Empty;
                txtApellido.Text = cliente.DatosPersona?.Apellido ?? string.Empty;
                txtDocumento.Text = cliente.DatosPersona?.NumeroDocumento ?? string.Empty;
                txtTelefono.Text = cliente.DatosPersona?.Telefono ?? string.Empty;
                txtEmail.Text = cliente.DatosPersona?.Email ?? string.Empty;
                txtDireccion.Text = cliente.DatosPersona?.Direccion ?? string.Empty;
                txtCuil.Text = cliente.Cuil ?? string.Empty;

                if (cliente.Categoria != null)
                    cmbCategoria.SelectedItem = cliente.Categoria;
            }
        }


        private void CargarTipoDocumento()
        {
            cmbTipoDoc.Items.Clear();
            cmbTipoDoc.Items.AddRange(new string[] { "DNI", "LC", "LE", "PASAPORTE" });
            cmbTipoDoc.SelectedIndex = 0;
        }

        private void CargarCategorias()
        {
            cmbCategoria.Items.Clear();
            cmbCategoria.Items.AddRange(new string[] {
                "CONSUMIDOR FINAL", "MONOTRIBUTISTA", "RESPONSABLE INSCRIPTO", "EXCENTO"
            });
            cmbCategoria.SelectedIndex = 0;
        }

        private void CargarClientes()
        {
            dgvClientes.AutoGenerateColumns = false;
            dgvClientes.Columns.Clear();
            dgvClientes.ReadOnly = true;
            ConfigurarGrilla();
            AgregarColumnaEliminar();
            AgregarColumnaEditar();

            listaClientes = clienteService.ListarClientes() ?? new List<Cliente>();
            dgvClientes.DataSource = listaClientes;
            dgvClientes.ClearSelection();
        }


        private void ConfigurarGrilla()
        {
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IdCliente",
                HeaderText = "Nº Cliente",
                DataPropertyName = "IdCliente",
                ReadOnly = true,
                FillWeight = 10
            });

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NombreCompleto",
                HeaderText = "Nombre y Apellido",
                DataPropertyName = "NombreCompleto",
                FillWeight = 30
            });

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NumeroDocumento",
                HeaderText = "Nº Documento",
                DataPropertyName = "NumeroDocumento",
                FillWeight = 15
            });
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Cuil",
                HeaderText = "Cuil/Cuit",
                DataPropertyName = "Cuil",
                FillWeight = 15
            });

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Telefono",
                HeaderText = "Telefono",
                DataPropertyName = "Telefono",
                FillWeight = 15
            });

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Categoria",
                HeaderText = "Categoria",
                DataPropertyName = "Categoria",
                FillWeight = 15
            });
        }

        private void AgregarColumnaEditar()
        {
            if (!dgvClientes.Columns.Contains("btnEditar"))
            {
                var col = new DataGridViewButtonColumn();
                col.Name = "btnEditar";
                col.HeaderText = "Editar";
                col.Text = "Editar";
                col.UseColumnTextForButtonValue = true;
                col.FillWeight = 8;
                dgvClientes.Columns.Add(col);
            }
        }

        private void AgregarColumnaEliminar()
        {
            if (!dgvClientes.Columns.Contains("btnEliminar"))
            {
                var col = new DataGridViewButtonColumn();
                col.Name = "btnEliminar";
                col.HeaderText = "Eliminar";
                col.Text = "Eliminar";
                col.UseColumnTextForButtonValue = true;
                col.FillWeight = 8;
                dgvClientes.Columns.Add(col);
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtDocumento.Text) ||
                string.IsNullOrWhiteSpace(txtCuil.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                MessageBox.Show("Todos los campos obligatorios deben completarse.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!txtNombre.Text.All(c => char.IsLetter(c) || c == ' '))
            {
                MessageBox.Show("El nombre solo debe contener letras y espacios.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!txtApellido.Text.All(c => char.IsLetter(c) || c == ' '))
            {
                MessageBox.Show("El apellido solo debe contener letras y espacios.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!txtDocumento.Text.All(char.IsDigit) || txtDocumento.Text.Length != 8)
            {
                MessageBox.Show("El número de documento debe contener exactamente 8 dígitos.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!txtCuil.Text.All(char.IsDigit) || txtCuil.Text.Length != 11)
            {
                MessageBox.Show("El CUIL debe contener exactamente 11 dígitos.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!txtTelefono.Text.All(char.IsDigit) || txtTelefono.Text.Length != 10)
            {
                MessageBox.Show("El teléfono debe contener exactamente 10 dígitos.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                var mail = new System.Net.Mail.MailAddress(txtEmail.Text);
            }
            catch
            {
                MessageBox.Show("Email inválido.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void Limpiar()
        {
            foreach (Control c in Controls)
                if (c is TextBox) c.Text = "";
            cmbTipoDoc.SelectedIndex = 0;
            cmbCategoria.SelectedIndex = 0;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            var persona = new Persona
            {
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                TipoDocumento = cmbTipoDoc.SelectedItem.ToString(),
                NumeroDocumento = txtDocumento.Text,
                Telefono = txtTelefono.Text,
                Email = txtEmail.Text,
                Direccion = txtDireccion.Text
            };

            var cliente = new Cliente
            {
                Categoria = cmbCategoria.SelectedItem.ToString(),
                Cuil = txtCuil.Text,
                DatosPersona = persona
            };

            if (clienteEditandoId == null)
            {
                try
                {
                    clienteService.CrearCliente(cliente);
                    MessageBox.Show("Cliente creado exitosamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al crear cliente: " + ex.Message);
                }
            }
            else
            {
                var clienteExistente = clienteService.ListarClientes()
                    .FirstOrDefault(c => c.IdCliente == clienteEditandoId.Value);

                if (clienteExistente == null)
                {
                    MessageBox.Show("No se encontró el cliente a modificar.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                cliente.IdCliente = clienteExistente.IdCliente;
                cliente.IdPersona = clienteExistente.IdPersona;
                cliente.DatosPersona.IdPersona = clienteExistente.IdPersona;

                try
                {
                    clienteService.ModificarCliente(cliente);
                    MessageBox.Show("Cliente modificado exitosamente.", "exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clienteEditandoId = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al modificar cliente: " + ex.Message);
                }
            }

            Limpiar();
            CargarClientes();
            tabControl1.SelectedTab = tabListado;
        }
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            // Evitar filtrar si está mostrando el placeholder
            if (txtBuscar.Text == placeholderClientes)
                return;

            string filtro = txtBuscar.Text.ToLower().Trim();

            // Si el campo está vacío, mostrar la lista completa
            if (string.IsNullOrWhiteSpace(filtro))
            {
                dgvClientes.DataSource = listaClientes;
                dgvClientes.ClearSelection();
                return;
            }

            // Filtrar sobre la lista en memoria, no contra la base de datos
            var filtrados = listaClientes.Where(c =>
                (c.DatosPersona?.Nombre?.ToLower().Contains(filtro) ?? false) ||
                (c.DatosPersona?.Apellido?.ToLower().Contains(filtro) ?? false) ||
                (c.DatosPersona?.NumeroDocumento?.ToLower().Contains(filtro) ?? false) ||
                (c.Cuil?.ToLower().Contains(filtro) ?? false)
            ).ToList();

            dgvClientes.DataSource = filtrados;
            dgvClientes.ClearSelection();
        }

        private void txtBuscar_Enter(object sender, EventArgs e)
        {
            if (txtBuscar.Text == placeholderClientes)
            {
                txtBuscar.Text = "";
                txtBuscar.ForeColor = Color.Black;
            }
        }

        private void txtBuscar_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                txtBuscar.Text = placeholderClientes;
                txtBuscar.ForeColor = Color.Gray;
                dgvClientes.DataSource = listaClientes; 
                dgvClientes.ClearSelection();
            }
        }


        private void btnAgregarCliente_Click(object sender, EventArgs e)
        {
            clienteEditandoId = null;
            Limpiar();

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtDocumento.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtDireccion.Clear();
            txtCuil.Clear();
            cmbTipoDoc.SelectedIndex = 0;
            cmbCategoria.SelectedIndex = 0;
            clienteEditandoId = null;

        }
        private void AplicarEstilos()
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

            // Estilos para botones específicos
            try { btnGuardar.BackColor = Color.FromArgb(34, 139, 34); btnGuardar.ForeColor = Color.White; } catch { }
            try { btnLimpiar.BackColor = Color.FromArgb(192, 57, 43); } catch { }
        }


        private void buttonVerCtasCtes_Click(object sender, EventArgs e)
        {
            new FrmCuentaCorriente().ShowDialog();
        }

       
    }
}
