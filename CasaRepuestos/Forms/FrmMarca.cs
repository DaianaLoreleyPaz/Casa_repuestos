using CasaRepuestos.Models;
using CasaRepuestos.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CasaRepuestos.Forms
{
    public partial class FrmMarca : Form
    {
        private MarcaService servicioMarca = new MarcaService();
        private int? marcaEditandoId = null;
        private bool esTextoPlaceholder = true;

        public FrmMarca()
        {
            InitializeComponent();

            // Aplicar estilos runtime 
            AplicarEstilos();

            // Cargas iniciales
            CargarMarca();
            btnEditar.Visible = false;
            btnGuardar.Visible = true;

            this.Load += FrmMarca_Load;
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

            try { btnEditar.BackColor = Color.FromArgb(12, 87, 150); } catch { }
            try { btnLimpiar.BackColor = Color.FromArgb(192, 57, 43); } catch { }

            // Estilos para DataGridView
            foreach (var dgv in new[] { dgvMarca })
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

        }
        private void CargarMarca()
        {
            try
            {
                dgvMarca.AutoGenerateColumns = false;
                dgvMarca.Columns.Clear();
                dgvMarca.ReadOnly = true;
                ConfigurarGrilla();
                AgregarColumnaEliminar();
                AgregarColumnaEditar();
                dgvMarca.DataSource = null;

                var marcas = servicioMarca.ListarMarcas() ?? new List<Marca>();
                dgvMarca.DataSource = marcas;
                // no mostrar id marca
                dgvMarca.Columns["IdMarca"].Visible = false;
                dgvMarca.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar marcas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvMarca.DataSource = new List<Marca>();
            }
        }

        private void ConfigurarGrilla()
        {
            dgvMarca.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IdMarca",
                HeaderText = "ID",
                DataPropertyName = "IdMarca",
                ReadOnly = true,
                Visible = false
            });

            dgvMarca.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Nombre",
                HeaderText = "Marca",
                DataPropertyName = "Nombre"
            });
        }

        private void AgregarColumnaEditar()
        {
            if (!dgvMarca.Columns.Contains("btnEditar"))
            {
                var col = new DataGridViewButtonColumn();
                col.Name = "btnEditar";
                col.HeaderText = "Editar";
                col.Text = "Editar";
                col.UseColumnTextForButtonValue = true;
                dgvMarca.Columns.Add(col);
            }
        }

        private void AgregarColumnaEliminar()
        {
            if (!dgvMarca.Columns.Contains("btnEliminar"))
            {
                var col = new DataGridViewButtonColumn();
                col.Name = "btnEliminar";
                col.HeaderText = "Eliminar";
                col.Text = "Eliminar";
                col.UseColumnTextForButtonValue = true;
                dgvMarca.Columns.Add(col);
            }
        }

        private void dgvMarca_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            // Evita clicks fuera de celdas válidas
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            // Maneja la eliminación
            if (dgvMarca.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                int id = Convert.ToInt32(dgvMarca.Rows[e.RowIndex].Cells["IdMarca"].Value);
                if (MessageBox.Show("¿Eliminar marca?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        servicioMarca.EliminarMarca(id);
                        MessageBox.Show("Marca eliminada exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarMarca();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar marca: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (dgvMarca.Columns[e.ColumnIndex].Name == "btnEditar")
            {
                int idMarca = Convert.ToInt32(dgvMarca.Rows[e.RowIndex].Cells["IdMarca"].Value);
                var marca = servicioMarca.ListarMarcas().FirstOrDefault(a => a.IdMarca == idMarca);

                if (marca == null) return;

                marcaEditandoId = idMarca;
                txtNombre.Text = marca.Nombre;

                btnEditar.Visible = true;
                btnGuardar.Visible = false;
                
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El campo Nombre es obligatorio", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNombre.Focus();
                return false;
            }

            var marcas = servicioMarca.ListarMarcas();
            if (marcas.Any(m => m.Nombre.Equals(txtNombre.Text, StringComparison.OrdinalIgnoreCase) &&
                               (marcaEditandoId == null || m.IdMarca != marcaEditandoId.Value)))
            {
                MessageBox.Show("Ya existe una marca con ese nombre", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            var marca = new Marca
            {
                Nombre = txtNombre.Text.Trim()
            };

            try
            {
                if (marcaEditandoId == null)
                {
                    servicioMarca.CrearMarca(marca);
                    MessageBox.Show("Marca creada exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    marca.IdMarca = marcaEditandoId.Value;
                    servicioMarca.ModificarMarca(marca);
                    MessageBox.Show("Marca actualizada exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                Limpiar();
                CargarMarca();
                tabControl1.SelectedTab = tabListado;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar marca: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            btnGuardar_Click(sender, e);
        }

        private void Limpiar()
        {
            txtNombre.Clear();
            btnGuardar.Visible = true;
            btnEditar.Visible = false;
            marcaEditandoId = null;
        }

        private void btnAgregarMarca_Click(object sender, EventArgs e)
        {
            marcaEditandoId = null;
            Limpiar();
            btnGuardar.Visible = true;
            btnEditar.Visible = false;
            
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void FrmMarca_Load(object sender, EventArgs e)
        {
            try
            {
                CargarMarca();
                MostrarPlaceholder();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar formulario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarPlaceholder()
        {
            txtBuscar.Text = "Buscar por nombre de la marca";
            txtBuscar.ForeColor = Color.Gray;
            esTextoPlaceholder = true;
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (esTextoPlaceholder && string.IsNullOrWhiteSpace(txtBuscar.Text)) return;

            string filtro = txtBuscar.Text.ToLower().Trim();
            var marcas = servicioMarca.ListarMarcas();

            var filtrados = marcas.Where(a => a.Nombre.ToLower().Contains(filtro)).ToList();

            dgvMarca.DataSource = null;
            dgvMarca.DataSource = filtrados;
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
            {
                MostrarPlaceholder();
            }
        }
    }
}
