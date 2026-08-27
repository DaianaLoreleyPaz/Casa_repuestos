using CasaRepuestos.Models;
using CasaRepuestos.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static CasaRepuestos.Forms.FrmCompras;

namespace CasaRepuestos.Forms
{
    
    public partial class FrmServicios : Form
    {
        private ServiciosService _serviciosService;
        private readonly ArticuloService _articuloService = new ArticuloService(); 
        private List<Servicio> _servicios = new List<Servicio>();
        private Servicio _servicioSeleccionado = null;
        private int? _servicioEditandoId = null;
        private bool _esTextoPlaceholder = true;
        private List<ArticuloDisplay> _articulosDisponibles = new List<ArticuloDisplay>();

        public class ArticuloDisplay
        {
            public int IdArticulo { get; set; }
            public string Nombre { get; set; }
        }
        public FrmServicios()
        {
            InitializeComponent();
            _serviciosService = new ServiciosService();


            this.Load += FrmServicios_Load;
            btnGuardar.Click += btnGuardar_Click;
            btnCancelar.Click += btnCancelar_Click;
            dgvServicios.CellClick += dgvServicios_CellClick;
            txtPrecio.KeyPress += txtPrecio_KeyPress;
            txtBuscar.Enter += txtBuscar_Enter;
            txtBuscar.Leave += txtBuscar_Leave;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            btnLimpiarArticulo.Click += btnLimpiarArticulo_Click;
        }

        private void FrmServicios_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigurarDataGridView();
                CargarServicios();
                CargarArticulos(); 
                ConfigurarControles();
                MostrarPlaceholder();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar formulario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void CargarArticulos()
        {
            try
            {
                
                var articulosRaw = _articuloService.ObtenerTodosArticulosSimples();

                _articulosDisponibles.Clear();
              
                _articulosDisponibles.Add(new ArticuloDisplay { IdArticulo = 0, Nombre = "— Sin Artículo Asociado —" });

                _articulosDisponibles.AddRange(articulosRaw.Select(a => new ArticuloDisplay
                {
                    IdArticulo = a.IdArticulo,
                    Nombre = a.Nombre
                }));

        
                cmbArticuloAsociado.DataSource = _articulosDisponibles;
                cmbArticuloAsociado.DisplayMember = "Nombre";
                cmbArticuloAsociado.ValueMember = "IdArticulo";

          
                string[] nombresArticulos = _articulosDisponibles.Select(a => a.Nombre).ToArray();

                var source = new AutoCompleteStringCollection();
                source.AddRange(nombresArticulos);

                cmbArticuloAsociado.AutoCompleteCustomSource = source;
                cmbArticuloAsociado.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbArticuloAsociado.AutoCompleteSource = AutoCompleteSource.CustomSource;

                cmbArticuloAsociado.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar artículos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarControles()
        {
            txtPrecio.Text = "0.00";

            if (cmbArticuloAsociado != null)
            {
                cmbArticuloAsociado.DropDownStyle = ComboBoxStyle.DropDown; 
            }
        }

        private void CargarServicios()
        {
            try
            {
                _servicios = _serviciosService.ObtenerTodosServicios();

             
                if (_servicios != null && _servicios.Count > 0)
                {
                    dgvServicios.DataSource = _servicios;
                }
                else
                {
                    dgvServicios.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar servicios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvServicios.DataSource = null;
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvServicios.AutoGenerateColumns = false;
            dgvServicios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvServicios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 66, 91);
            dgvServicios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvServicios.EnableHeadersVisualStyles = false;
            dgvServicios.RowHeadersVisible = false;
            dgvServicios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvServicios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvServicios.Columns.Clear();

 
            dgvServicios.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "IdServicio",
                HeaderText = "ID",
                DataPropertyName = "IdServicio",
                Width = 50,
                ReadOnly = true,
                Visible = false
            });

            dgvServicios.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Descripcion",
                HeaderText = "Descripción",
                DataPropertyName = "Descripcion",
                Width = 200,
                ReadOnly = true
            });

            dgvServicios.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Precio",
                HeaderText = "Precio",
                DataPropertyName = "Precio",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" },
                ReadOnly = true
            });

 
            AgregarColumnaEditar();
            AgregarColumnaEliminar();
        }

        private void AgregarColumnaEditar()
        {
  
            if (dgvServicios.Columns.Contains("btnEditar"))
            {
                dgvServicios.Columns.Remove("btnEditar");
            }

            var col = new DataGridViewButtonColumn();
            col.Name = "btnEditar";
            col.HeaderText = "Editar";
            col.Text = "Editar";
            col.UseColumnTextForButtonValue = true;
            dgvServicios.Columns.Add(col);
        }

        private void AgregarColumnaEliminar()
        {

            if (dgvServicios.Columns.Contains("btnEliminar"))
            {
                dgvServicios.Columns.Remove("btnEliminar");
            }

            var col = new DataGridViewButtonColumn();
            col.Name = "btnEliminar";
            col.HeaderText = "Eliminar";
            col.Text = "Eliminar";
            col.UseColumnTextForButtonValue = true;
            dgvServicios.Columns.Add(col);
        }

        private void dgvServicios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
     
            if (e.RowIndex < 0 || dgvServicios.Rows.Count == 0 || e.RowIndex >= dgvServicios.Rows.Count)
                return;

    
            if (dgvServicios.Rows[e.RowIndex].IsNewRow)
                return;

            var servicio = dgvServicios.Rows[e.RowIndex].DataBoundItem as Servicio;
            if (servicio == null)
                return;

            if (dgvServicios.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                int id = servicio.IdServicio;
                if (MessageBox.Show("¿Eliminar servicio?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        _serviciosService.EliminarServicio(id);
                        MessageBox.Show("Servicio eliminado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarServicios();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar servicio: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (dgvServicios.Columns[e.ColumnIndex].Name == "btnEditar")
            {
                _servicioEditandoId = servicio.IdServicio;
                txtDescripcion.Text = servicio.Descripcion;
                txtPrecio.Text = servicio.Precio.ToString("F2");
                int articuloId = servicio.IdArticuloAsociado.GetValueOrDefault(0);
                
                // Buscar el artículo asociado y seleccionarlo en el ComboBox
                var articuloASeleccionar = _articulosDisponibles
                    .FirstOrDefault(a => a.IdArticulo == articuloId);
                
                if (articuloASeleccionar != null)
                {
                    // Encontrar el índice del artículo en el ComboBox
                    int index = _articulosDisponibles.IndexOf(articuloASeleccionar);
                    if (index >= 0)
                    {
                        cmbArticuloAsociado.SelectedIndex = index;
                    }
                    else
                    {
                        cmbArticuloAsociado.SelectedIndex = 0;
                    }
                }
                else
                {
                    cmbArticuloAsociado.SelectedIndex = 0;
                }
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("El campo Descripción es obligatorio", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDescripcion.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("Debe ingresar un precio válido mayor a cero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPrecio.Focus();
                return false;
            }

            string descripcionActual = txtDescripcion.Text.Trim();

            if (_servicioEditandoId.HasValue)
            {
                var servicioActual = _serviciosService.ObtenerServicioPorId(_servicioEditandoId.Value);
                if (servicioActual != null && servicioActual.Descripcion.Equals(descripcionActual, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            if (_serviciosService.ExisteServicio(descripcionActual, _servicioEditandoId))
            {
                MessageBox.Show("Ya existe un servicio con esa descripción", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            if (!ValidarCampos()) return;

            int? idArticuloSeleccionado = null;

            if (cmbArticuloAsociado.SelectedItem is ArticuloDisplay selectedArticulo)
            {
                if (selectedArticulo.IdArticulo > 0)
                {
                    idArticuloSeleccionado = selectedArticulo.IdArticulo;
                }
            }
            if (idArticuloSeleccionado.HasValue && idArticuloSeleccionado.Value > 0)
            {
                bool articuloYaUsado = _serviciosService.ExisteServicioParaArticulo(
                    idArticuloSeleccionado.Value,
                    _servicioEditandoId 
                );

                if (articuloYaUsado)
                {
                    MessageBox.Show(
                        "El artículo seleccionado ya tiene un servicio asociado. No puedes asignarlo nuevamente.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }
            }

            var servicio = new Servicio
            {
                Descripcion = txtDescripcion.Text.Trim(),
                Precio = decimal.Parse(txtPrecio.Text),
                IdArticuloAsociado = idArticuloSeleccionado
            };

            try
            {
                if (_servicioEditandoId == null)
                {
                    _serviciosService.CrearServicio(servicio);
                    MessageBox.Show("Servicio creado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    servicio.IdServicio = _servicioEditandoId.Value;
                    _serviciosService.ActualizarServicio(servicio);
                    MessageBox.Show("Servicio actualizado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    try
                    {
                        var presupuestoService = new PresupuestoService();
                        int filasAfectadas = presupuestoService.ActualizarPreciosEnPresupuestosFlexibles(
                            idArticulo: null,
                            idServicio: servicio.IdServicio,
                            nuevoPrecio: servicio.Precio
                        );

                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show(
                                $"Se actualizaron {filasAfectadas} presupuestos con el nuevo precio del servicio.",
                                "Precios actualizados",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                        }
                    }
                    catch (Exception exPres)
                    {
                        MessageBox.Show(
                            "Error al actualizar precios en presupuestos: " + exPres.Message,
                            "Error de sincronización",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                    }
                }

                LimpiarCampos();
                CargarServicios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar servicio: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void LimpiarCampos()
        {
            txtDescripcion.Text = "";
            txtPrecio.Text = "0.00";
            _servicioSeleccionado = null;
            _servicioEditandoId = null;

            if (cmbArticuloAsociado != null && cmbArticuloAsociado.Items.Count > 0)
            {
                cmbArticuloAsociado.SelectedIndex = 0;
            }

            dgvServicios.ClearSelection();
        }

        private void MostrarPlaceholder()
        {
            txtBuscar.Text = "Buscar por descripción del servicio";
            txtBuscar.ForeColor = Color.Gray;
            _esTextoPlaceholder = true;
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (_esTextoPlaceholder && string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                dgvServicios.DataSource = _servicios;
                return;
            }

            string filtro = txtBuscar.Text.ToLower().Trim();
            var filtrados = _servicios.Where(s => s.Descripcion.ToLower().Contains(filtro)).ToList();
            dgvServicios.DataSource = filtrados;
        }

        private void txtBuscar_Enter(object sender, EventArgs e)
        {
            if (_esTextoPlaceholder)
            {
                txtBuscar.Text = "";
                txtBuscar.ForeColor = Color.Black;
                _esTextoPlaceholder = false;
            }
        }

        private void txtBuscar_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                MostrarPlaceholder();
                dgvServicios.DataSource = _servicios;
            }
        }

        private void btnLimpiarArticulo_Click(object sender, EventArgs e)
        {
            if (cmbArticuloAsociado.DataSource != null)
            {
                cmbArticuloAsociado.SelectedIndex = 0;
            }
        }
    }
}