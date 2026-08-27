using CasaRepuestos.Models;
using CasaRepuestos.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CasaRepuestos.Forms
{
    public partial class FrmArticulo : Form
    {
        private ArticuloService servicioArticulo = new ArticuloService();
        private MarcaService servicioMarca = new MarcaService();
        private ProveedorService servicioProveedor = new ProveedorService();

        // Esta variable se usa para almacenar el ID del artículo que se está editando
        private int? articuloEditandoId = null;
        private bool esTextoPlaceholder = true;

        public FrmArticulo()
        {
            InitializeComponent();

            // Aplicar estilos y ajustes runtime para coherencia con FrmMenu
            AplicarEstilos();
            txtPrecio.ReadOnly = true; 
            txtPrecio.BackColor = Color.FromArgb(230, 230, 230); 
        
            btnEditar.Visible = false; 
            btnGuardar.Visible = true;
        }

        private void CalcularPrecioFinal()
        {
            try
            {
                
                decimal ganancia = numPorcentajeGanancia.Value;

                
                decimal iva = 0m;
                string ivaSeleccionado = cmbIVA.SelectedItem?.ToString() ?? "0%";
                
                decimal.TryParse(ivaSeleccionado.Replace("%", ""), out iva);

                // Buscamos el costo máximo entre los proveedores asignados
                decimal maxCosto = 0m;
                foreach (DataGridViewRow row in dgvProveedoresCosto.Rows)
                {
                    // Solo miramos los proveedores que están marcados como asignado
                    bool asignado = Convert.ToBoolean(row.Cells["Asignado"].Value ?? false);
                    if (asignado)
                    {
                        // Leemos el valor de la celda de costo
                        decimal.TryParse(row.Cells["PrecioCoste"].Value?.ToString(), out decimal costoActual);

                        if (costoActual > maxCosto)
                        {
                            maxCosto = costoActual;
                        }
                    }
                }

                // Calcular el Precio de Venta (Precio Final)
                decimal precioConGanancia = maxCosto * (1 + (ganancia / 100m));
                decimal precioFinal = precioConGanancia * (1 + (iva / 100m));

                // Mostrarlo en el TextBox
                txtPrecio.Text = precioFinal.ToString("F2");
            }
            catch
            {
                txtPrecio.Text = "Error"; 
            }
        }


        // Evento para manejar el clic en las celdas del DataGridView
        private void dgvArticulo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; 

            // --- Lógica de Eliminar ---
            if (dgvArticulo.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                try
                {
                    // Obtener el ID y el nombre 
                    int idArticulo = Convert.ToInt32(dgvArticulo.Rows[e.RowIndex].Cells["IdArticulo"].Value);
                    string nombreArticulo = dgvArticulo.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();

                    // Mostrar confirmación
                    var confirmResult = MessageBox.Show(
                        $"¿Está seguro de que desea eliminar el artículo '{nombreArticulo}'?",
                        "Confirmar Eliminación",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (confirmResult == DialogResult.Yes)
                    {
                        // Llamar al servicio para eliminar
                        servicioArticulo.EliminarArticulo(idArticulo);
                        MessageBox.Show("Artículo eliminado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Recargar la grilla principal
                        CargarArticulo();
                    }
                }
                catch (Exception ex)
                {
                    // Manejar errores
                    MessageBox.Show("Error al eliminar el artículo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return; 
            }

            // --- Lógica de Editar ---
            if (dgvArticulo.Columns[e.ColumnIndex].Name == "btnEditar")
            {
                int idArticulo = Convert.ToInt32(dgvArticulo.Rows[e.RowIndex].Cells["IdArticulo"].Value);

                // Usamos el servicio modificado 
                var articulo = servicioArticulo.ObtenerArticuloPorId(idArticulo);
                if (articulo == null) return;

                articuloEditandoId = idArticulo;

                // Llenar campos de texto 
                txtNombre.Text = articulo.Nombre;
                txtPrecio.Text = articulo.Precio.ToString("F2");
                txtStock.Text = articulo.Stock.ToString();
                numPorcentajeGanancia.Value = articulo.PorcentajeGanancia;
                
                cmbIVA.SelectedItem = articulo.IVA.ToString("G29") + "%";

                cmbMarca.SelectedValue = articulo.IdMarca;

                // Llenar la grilla dgvProveedoresCosto
                foreach (DataGridViewRow row in dgvProveedoresCosto.Rows)
                {
                    int idProvEnGrilla = Convert.ToInt32(row.Cells["IdProveedor"].Value);

                    // Buscamos si este proveedor está asignado al artículo
                    var asignacion = articulo.ProveedoresConCosto
                        .FirstOrDefault(p => p.IdProveedor == idProvEnGrilla);

                    if (asignacion != null)
                    {
                        // Si está asignado, marcarlo y ponerle el precio
                        row.Cells["Asignado"].Value = true;
                        row.Cells["PrecioCoste"].Value = asignacion.PrecioCoste.ToString("F2");
                    }
                    else
                    {
                        // si no esta asignado, desmarcarlo y limpiar precio
                        row.Cells["Asignado"].Value = false;
                        row.Cells["PrecioCoste"].Value = null;
                    }
                }

                // Recalcular el precio final
                CalcularPrecioFinal();

                btnEditar.Visible = true;
                btnGuardar.Visible = false;
            }
        }

        // Método para cargar los artículos al iniciar el formulario
        private void CargarArticulo()
        {
            dgvArticulo.AutoGenerateColumns = false;
            dgvArticulo.Columns.Clear();
            dgvArticulo.ReadOnly = true;
            ConfigurarGrilla();
            AgregarColumnaEliminar();
            AgregarColumnaEditar();
            dgvArticulo.DataSource = null;

            var lista = servicioArticulo.ListarArticulos() ?? new List<Articulo>();
            dgvArticulo.DataSource = lista;
            // ocultar columna IdArticulo
            dgvArticulo.Columns["IdArticulo"].Visible = false;

            // Limpiar selección por defecto
            dgvArticulo.ClearSelection();
        }

        private void ConfigurarGrilla()
        {
            dgvArticulo.Columns.Clear();

            dgvArticulo.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IdArticulo",
                HeaderText = "ID",
                DataPropertyName = "IdArticulo",
                ReadOnly = true,
                Visible = false 
            });

            dgvArticulo.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Nombre",
                HeaderText = "Artículo",
                DataPropertyName = "Nombre",
                FillWeight = 30
            });

            dgvArticulo.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Precio",
                HeaderText = "Precio",
                DataPropertyName = "Precio",
                FillWeight = 15,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dgvArticulo.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Stock",
                HeaderText = "Stock",
                DataPropertyName = "Stock",
                FillWeight = 12
            });

            dgvArticulo.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Marca",
                HeaderText = "Marca",
                DataPropertyName = "NombreMarca", 
                FillWeight = 18
            });


            dgvArticulo.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Proveedor",
                HeaderText = "Proveedores",        
                DataPropertyName = "NombresProveedores",
                FillWeight = 25
            });
        }

        private void AgregarColumnaEditar()
        {
            if (!dgvArticulo.Columns.Contains("btnEditar"))
            {
                var col = new DataGridViewButtonColumn();
                col.Name = "btnEditar";
                col.HeaderText = "Editar";
                col.Text = "Editar";
                col.UseColumnTextForButtonValue = true;
                col.FillWeight = 10;
                dgvArticulo.Columns.Add(col);
            }
        }

        private void AgregarColumnaEliminar()
        {
            if (!dgvArticulo.Columns.Contains("btnEliminar"))
            {
                var col = new DataGridViewButtonColumn();
                col.Name = "btnEliminar";
                col.HeaderText = "Eliminar";
                col.Text = "Eliminar";
                col.UseColumnTextForButtonValue = true;
                col.FillWeight = 10;
                dgvArticulo.Columns.Add(col);
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

        
            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("El Precio debe ser un número válido mayor que cero", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPrecio.Focus();
                return false;
            }

 

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("El Stock debe ser un número entero válido", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtStock.Focus();
                return false;
            }

            if (cmbMarca.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar una Marca", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbMarca.Focus();
                return false;
            }

            // Validamos la grilla
            bool alMenosUnProveedor = false;
            foreach (DataGridViewRow row in dgvProveedoresCosto.Rows)
            {
                bool asignado = Convert.ToBoolean(row.Cells["Asignado"].Value ?? false);
                if (asignado)
                {
                    alMenosUnProveedor = true;
                    // Validar que el Precio de Coste sea un número válido mayor que cero
                    if (!decimal.TryParse(row.Cells["PrecioCoste"].Value?.ToString(), out decimal precioCoste) || precioCoste <= 0)
                    {
                        MessageBox.Show($"Debe ingresar un Precio de Coste válido para el proveedor '{row.Cells["RazonSocial"].Value}'", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dgvProveedoresCosto.CurrentCell = row.Cells["PrecioCoste"];
                        dgvProveedoresCosto.BeginEdit(true);
                        return false;
                    }
                }
            }

            if (!alMenosUnProveedor)
            {
                MessageBox.Show("Debe seleccionar al menos un Proveedor y asignarle un costo", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvProveedoresCosto.Focus();
                return false;
            }

            return true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            var proveedoresConCosto = new List<ArticuloProveedor>();
            string ivaSeleccionado = cmbIVA.SelectedItem.ToString().Replace("%", "");
            foreach (DataGridViewRow row in dgvProveedoresCosto.Rows)
            {
                bool asignado = Convert.ToBoolean(row.Cells["Asignado"].Value ?? false);
                if (asignado)
                {
                    proveedoresConCosto.Add(new ArticuloProveedor
                    {
                        IdProveedor = Convert.ToInt32(row.Cells["IdProveedor"].Value),
                        PrecioCoste = Convert.ToDecimal(row.Cells["PrecioCoste"].Value)
                    });
                }
            }

            var articulo = new Articulo
            {
                Nombre = txtNombre.Text,
                Precio = Convert.ToDecimal(txtPrecio.Text),
                Stock = Convert.ToInt32(txtStock.Text),
                IdMarca = (int)cmbMarca.SelectedValue,
                PorcentajeGanancia = numPorcentajeGanancia.Value, 
                IVA = Convert.ToDecimal(ivaSeleccionado),
                ProveedoresConCosto = proveedoresConCosto 
            };

            try
            {
                servicioArticulo.CrearArticulo(articulo);
                MessageBox.Show("Artículo creado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear artículo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Limpiar();
            CargarArticulo();
        }

        private void Limpiar()
        {

            txtNombre.Clear();
            txtPrecio.Clear(); 
            txtStock.Clear();

            numPorcentajeGanancia.Value = 50; 
            cmbIVA.SelectedIndex = 0;        

            cmbMarca.SelectedIndex = -1;

            //Limpiamos la grilla dgvProveedoresCosto
            foreach (DataGridViewRow row in dgvProveedoresCosto.Rows)
            {
                // Limpiamos la asignación y el precio
                if (row.Cells["Asignado"] is DataGridViewCheckBoxCell chk)
                {
                    chk.Value = false;
                }
                if (row.Cells["PrecioCoste"] is DataGridViewTextBoxCell txt)
                {
                    txt.Value = null;
                }
            }

            dgvProveedoresCosto.DataSource = null; 
            CargarProveedor(); 

            btnGuardar.Visible = true;
            btnEditar.Visible = false;
            articuloEditandoId = null;
        }

        private void btnAgregarArticulo_Click(object sender, EventArgs e)
        {
            articuloEditandoId = null;
          
            btnGuardar.Visible = true;
            btnEditar.Visible = false;

        }

        private void AplicarEstilos()
        {


            this.BackColor = Color.FromArgb(240, 245, 249);

            // Estilos para GroupBoxes
            foreach (var gb in this.Controls.OfType<GroupBox>())
            {
                gb.BackColor = Color.White;
                gb.ForeColor = Color.FromArgb(45, 66, 91);
            }

            // Estilos para Botones
            foreach (var btn in this.Controls.OfType<Button>())
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Color.FromArgb(76, 132, 200);
                btn.ForeColor = Color.White;
            }

            // Estilos para botones específicos
            try { btnGuardar.BackColor = Color.FromArgb(34, 139, 34); btnGuardar.ForeColor = Color.White; } catch { }

            try { btnEditar.BackColor = Color.FromArgb(12, 87, 150); } catch { }
            try { btnLimpiar.BackColor = Color.FromArgb(192, 57, 43); btnLimpiar.ForeColor = Color.White; } catch { }

            // Estilos para DataGridViews
            foreach (var dgv in new[] { dgvArticulo }) 
            {
                if (dgv != null)
                {
                    dgv.BackgroundColor = Color.White;
                    dgv.EnableHeadersVisualStyles = false;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 66, 91);
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                }
            }



        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
        // Evento Load del formulario, donde se inicializan los datos y controles
        private void FrmArticulo_Load(object sender, EventArgs e)
        {
            CargarArticulo();
            CargarMarca();
            CargarProveedor();
            MostrarPlaceholder();

            cmbIVA.Items.Clear();      
            cmbIVA.Items.Add("21%");
            cmbIVA.Items.Add("10.5%");
            cmbIVA.Items.Add("27%");
            cmbIVA.Items.Add("0%");
            cmbIVA.SelectedIndex = 0;


            numPorcentajeGanancia.Minimum = 0;
            numPorcentajeGanancia.Maximum = 1000; 
            numPorcentajeGanancia.DecimalPlaces = 2;
            numPorcentajeGanancia.Value = 50; 
        }
        

        private void MostrarPlaceholder()
        {
            txtBuscar.Text = "Buscar por nombre del artículo";
            txtBuscar.ForeColor = Color.Gray;
            esTextoPlaceholder = true;
        }
        // Evento TextChanged del TextBox de búsqueda para filtrar artículos
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (esTextoPlaceholder && string.IsNullOrWhiteSpace(txtBuscar.Text)) return;

            string filtro = txtBuscar.Text.ToLower().Trim();
            var articulos = servicioArticulo.ListarArticulos() ?? new List<Articulo>();

            var filtrados = articulos.Where(a =>
                (a.Nombre ?? "").ToLower().Contains(filtro) ||
                (a.Marca != null && (a.Marca.Nombre ?? "").ToLower().Contains(filtro)) ||
                (a.Proveedor != null && (a.Proveedor.RazonSocial ?? "").ToLower().Contains(filtro))
            ).ToList();

            dgvArticulo.DataSource = null;
            dgvArticulo.DataSource = filtrados;
            dgvArticulo.ClearSelection();
        }
        // Eventos Enter y Leave para manejar el placeholder en el TextBox de búsqueda
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

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            if (articuloEditandoId == null)
            {
                MessageBox.Show("No hay artículo seleccionado para editar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var proveedoresConCosto = new List<ArticuloProveedor>();
            string ivaSeleccionado = cmbIVA.SelectedItem.ToString().Replace("%", "");
            foreach (DataGridViewRow row in dgvProveedoresCosto.Rows)
            {
                bool asignado = Convert.ToBoolean(row.Cells["Asignado"].Value ?? false);
                if (asignado)
                {
                    proveedoresConCosto.Add(new ArticuloProveedor
                    {
                        IdProveedor = Convert.ToInt32(row.Cells["IdProveedor"].Value),
                        PrecioCoste = Convert.ToDecimal(row.Cells["PrecioCoste"].Value)
                    });
                }
            }

            var articulo = new Articulo
            {
                IdArticulo = articuloEditandoId.Value,
                Nombre = txtNombre.Text,
                Precio = Convert.ToDecimal(txtPrecio.Text),
                Stock = Convert.ToInt32(txtStock.Text),
                IdMarca = (int)cmbMarca.SelectedValue,
                PorcentajeGanancia = numPorcentajeGanancia.Value, 
                IVA = Convert.ToDecimal(ivaSeleccionado), 
                ProveedoresConCosto = proveedoresConCosto 
            };

            try
            {
                servicioArticulo.ModificarArticulo(articulo);
                MessageBox.Show("Artículo modificado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar artículo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            articuloEditandoId = null;
            
            CargarArticulo();
            Limpiar();
        }

        private void CargarMarca()
        {
            var marcas = servicioMarca.ListarMarcas() ?? new List<Marca>();
            cmbMarca.DataSource = marcas;
            cmbMarca.DisplayMember = "Nombre";
            cmbMarca.ValueMember = "IdMarca";
            cmbMarca.SelectedIndex = -1;
        }

        private void CargarProveedor()
        {
            var proveedores = servicioProveedor.ListarProveedores() ?? new List<Proveedor>();

            dgvProveedoresCosto.AutoGenerateColumns = false;
            dgvProveedoresCosto.Columns.Clear();

            dgvProveedoresCosto.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IdProveedor",
                DataPropertyName = "IdProveedor", 
                Visible = false
            });

            dgvProveedoresCosto.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RazonSocial",
                HeaderText = "Proveedor",
                DataPropertyName = "RazonSocial", 
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvProveedoresCosto.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "Asignado",
                HeaderText = "Asig.",
                Width = 50
            });

            var precioCol = new DataGridViewTextBoxColumn
            {
                Name = "PrecioCoste",
                HeaderText = "Costo ($)",
                Width = 100
            };
            precioCol.DefaultCellStyle.Format = "N2";
            dgvProveedoresCosto.Columns.Add(precioCol);

            dgvProveedoresCosto.DataSource = proveedores;
        }
        // Evento para recalcular el precio final cuando se edita una celda en la grilla de proveedores y costos
        private void dgvProveedoresCosto_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
            {
                CalcularPrecioFinal();
            }
        }
        // Evento para recalcular el precio final cuando cambia el porcentaje de ganancia
        private void numPorcentajeGanancia_ValueChanged(object sender, EventArgs e)
        {
            CalcularPrecioFinal();
        }
        // Evento para recalcular el precio final cuando cambia el IVA seleccionado
        private void cmbIVA_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcularPrecioFinal();
        }
    }
}

