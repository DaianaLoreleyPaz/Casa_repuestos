using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CasaRepuestos.Models;
using CasaRepuestos.Services;

namespace CasaRepuestos.Forms
{
    public partial class FrmIngreso : Form
    {
        private ClienteService _clienteService;
        private IngresoService _ingresoService;
        private PresupuestoService _presupuestoService;
        private MarcaService _marcaService;
        private List<Cliente> _clientes;
        private List<Ingreso> _ingresos;

        public FrmIngreso()
        {
            InitializeComponent();
            AplicarEstilos();
            _clienteService = new ClienteService();
            _ingresoService = new IngresoService();
            _presupuestoService = new PresupuestoService();
            _marcaService = new MarcaService();
            _clientes = new List<Cliente>();
            _ingresos = new List<Ingreso>();
        }

        private void AplicarEstilos()
        {
            this.BackColor = Color.FromArgb(240, 245, 249);

            // Estilos para Contenedores principales 
            try { panelLista.BackColor = Color.White; } catch { }
            try { panelForm.BackColor = Color.White; } catch { }
            try { panelHeader.BackColor = Color.FromArgb(240, 245, 249); } catch { }

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
            try { btnCancelar.BackColor = Color.FromArgb(192, 57, 43); } catch { }

            // Estilos para DataGridView
            foreach (var dgv in new[] { dgvIngresos })
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
        private void FrmIngreso_Load(object sender, EventArgs e)
        {
            CargarClientes();
            CargarMarcas();
            CargarIngresos();
            dtpFechaIngreso.Value = DateTime.Today;
            dtpFechaIngreso.Enabled = false;

            // Configurar DataGridView
            ConfigurarDataGridView();
        }

        private void ConfigurarDataGridView()
        {
            dgvIngresos.AutoGenerateColumns = false;
            dgvIngresos.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvIngresos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvIngresos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 66, 91);
            dgvIngresos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvIngresos.EnableHeadersVisualStyles = false;
            dgvIngresos.RowHeadersVisible = false;
            dgvIngresos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIngresos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Agregar columnas
            dgvIngresos.Columns.Clear();
            dgvIngresos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "IdIngreso",
                HeaderText = "ID",
                DataPropertyName = "IdIngreso", 
                Visible = false 
            });
            dgvIngresos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Cliente",
                HeaderText = "Cliente",
                DataPropertyName = "Cliente"
            });

            dgvIngresos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "DNI",
                HeaderText = "DNI",
                DataPropertyName = "DNI",
                Width = 120
            });

            dgvIngresos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Modelo",
                HeaderText = "Modelo",
                DataPropertyName = "Modelo",
                Width = 150
            });

            dgvIngresos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Marca",
                HeaderText = "Marca",
                DataPropertyName = "Marca",
                Width = 120
            });

            dgvIngresos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "FechaIngreso",
                HeaderText = "Fecha Ingreso",
                DataPropertyName = "FechaIngreso",
                Width = 120
            });

            dgvIngresos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Falla",
                HeaderText = "Falla",
                DataPropertyName = "Falla",
                Width = 200
            });

            dgvIngresos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "TipoDispositivo",
                HeaderText = "Tipo",
                DataPropertyName = "TipoDispositivo",
                Width = 100
            });

            dgvIngresos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Accesorios",
                HeaderText = "Accesorios",
                DataPropertyName = "AccesoriosEntregados",
                Width = 150
            });
        }
        // Cargar ingresos desde el servicio
        private void CargarIngresos()
        {
            try
            {
                _ingresos = _ingresoService.ObtenerIngresos();
                ActualizarDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar ingresos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Actualizar el DataGridView con la información combinada de ingresos y clientes
        private void ActualizarDataGridView()
        {
            var ingresosConInfoCliente = _ingresos.Select(ingreso =>
            {
                var cliente = _clientes.FirstOrDefault(c => c.IdCliente == ingreso.IdCliente);
                return new
                {
                    ingreso.IdIngreso,
                    Cliente = cliente != null ? $"{cliente.DatosPersona.Nombre} {cliente.DatosPersona.Apellido}" : "N/A",
                    DNI = cliente != null ? cliente.DatosPersona.NumeroDocumento : "N/A",
                    ingreso.Modelo,
                    Marca = _marcaService.ObtenerMarcaPorId(ingreso.IdMarca ?? 0)?.Nombre ?? "N/A",
                    ingreso.FechaIngreso,
                    ingreso.Falla,
                    ingreso.TipoDispositivo,
                    ingreso.AccesoriosEntregados
                };
            }).ToList();

            dgvIngresos.DataSource = ingresosConInfoCliente;
        }

        private void CargarClientes()
        {
            try
            {
                _clientes = _clienteService.ListarClientes();
                var clientesAutoComplete = new AutoCompleteStringCollection();
                clientesAutoComplete.AddRange(_clientes.Select(c => $"{c.DatosPersona.Nombre} {c.DatosPersona.Apellido} - {c.DatosPersona.NumeroDocumento}").ToArray());
                txtCliente.AutoCompleteCustomSource = clientesAutoComplete;
                txtCliente.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtCliente.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Cargar marcas desde el servicio
        private void CargarMarcas()
        {
            try
            {
                var marcas = _marcaService.ListarMarcas();
                cmbMarca.DataSource = marcas;
                cmbMarca.DisplayMember = "Nombre";
                cmbMarca.ValueMember = "IdMarca";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar marcas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregarCliente_Click(object sender, EventArgs e)
        {
            FrmCliente frmCliente = new FrmCliente();
            frmCliente.ShowDialog();
            CargarClientes();
        }
        // Guardar nuevo ingreso
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                try
                {
                    var clienteSeleccionado = ObtenerClienteSeleccionado();

                    if (clienteSeleccionado == null)
                    {
                        MessageBox.Show("Debe seleccionar un cliente válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var ingreso = new Ingreso
                    {
                        IdCliente = clienteSeleccionado.IdCliente,
                        IdMarca = (int)cmbMarca.SelectedValue,
                        Modelo = txtModelo.Text,
                        Falla = txtFalla.Text,
                        AccesoriosEntregados = txtAccesorios.Text,
                        FechaIngreso = dtpFechaIngreso.Value,
                        Estado = Estado.RECIBIDO,
                        TipoDispositivo = rbSmartphone.Checked ? TipoDispositivo.SMARTPHONE : TipoDispositivo.TABLET
                    };

                    _ingresoService.CrearIngreso(ingreso);
                    MessageBox.Show("Ingreso registrado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // LIMPIAR CAMPOS DESPUÉS DE GUARDAR EXITOSAMENTE
                    LimpiarCampos();
                    CargarIngresos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al registrar ingreso: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private Cliente ObtenerClienteSeleccionado()
        {
            if (string.IsNullOrEmpty(txtCliente.Text)) return null;

            // Buscar cliente por texto ingresado 
            var partes = txtCliente.Text.Split('-');
            if (partes.Length < 2) return null;

            var dni = partes[1].Trim();
            return _clientes.FirstOrDefault(c => c.DatosPersona.NumeroDocumento == dni);
        }
        // Validar campos del formulario
        private bool ValidarCampos()
        {
            if (string.IsNullOrEmpty(txtCliente.Text))
            {
                MessageBox.Show("Debe seleccionar un cliente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (cmbMarca.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar una marca", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(txtModelo.Text))
            {
                MessageBox.Show("Debe ingresar el modelo del dispositivo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(txtFalla.Text))
            {
                MessageBox.Show("Debe ingresar la falla del dispositivo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void LimpiarCampos()
        {
            txtCliente.Text = "";
            cmbMarca.SelectedIndex = -1;
            txtModelo.Text = "";
            txtFalla.Text = "";
            txtAccesorios.Text = "";
            rbSmartphone.Checked = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
        // Filtrar ingresos en tiempo real
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            FiltrarIngresos();
        }

        private void FiltrarIngresos()
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                ActualizarDataGridView();
                return;
            }

            var textoBusqueda = txtBuscar.Text.ToLower();
            var ingresosFiltrados = _ingresos.Where(ingreso =>
            {
                var cliente = _clientes.FirstOrDefault(c => c.IdCliente == ingreso.IdCliente);
                var marca = _marcaService.ObtenerMarcaPorId(ingreso.IdMarca ?? 0);

                return (cliente != null && cliente.DatosPersona.Nombre.ToLower().Contains(textoBusqueda)) ||
                       (cliente != null && cliente.DatosPersona.Apellido.ToLower().Contains(textoBusqueda)) ||
                       (cliente != null && cliente.DatosPersona.NumeroDocumento.ToLower().Contains(textoBusqueda)) ||
                       (ingreso.Modelo != null && ingreso.Modelo.ToLower().Contains(textoBusqueda)) ||
                       (marca != null && marca.Nombre.ToLower().Contains(textoBusqueda)) ||
                       (ingreso.Falla != null && ingreso.Falla.ToLower().Contains(textoBusqueda));
            }).ToList();

            var ingresosConInfoCliente = ingresosFiltrados.Select(ingreso =>
            {
                var cliente = _clientes.FirstOrDefault(c => c.IdCliente == ingreso.IdCliente);
                return new
                {
                    ingreso.IdIngreso,
                    Cliente = cliente != null ? $"{cliente.DatosPersona.Nombre} {cliente.DatosPersona.Apellido}" : "N/A",
                    DNI = cliente != null ? cliente.DatosPersona.NumeroDocumento : "N/A",
                    ingreso.Modelo,
                    Marca = _marcaService.ObtenerMarcaPorId(ingreso.IdMarca ?? 0)?.Nombre ?? "N/A",
                    ingreso.FechaIngreso,
                    ingreso.Falla,
                    ingreso.TipoDispositivo,
                    ingreso.AccesoriosEntregados
                };
            }).ToList();

            dgvIngresos.DataSource = ingresosConInfoCliente;
        }
        // Habilitar o deshabilitar el botón de registrar devolución según la selección en el DataGridView
        private void dgvIngresos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvIngresos.SelectedRows.Count > 0)
            {
                // Si se selecciona una fila, habilitamos el botón
                btnRegistrarDevolucion.Enabled = true;
            }
            else
            {
                // Si no hay nada seleccionado, lo deshabilitamos
                btnRegistrarDevolucion.Enabled = false;
            }
        }

        private void btnRegistrarDevolucion_Click(object sender, EventArgs e)
        {
            if (dgvIngresos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un ingreso de la lista para registrar su devolución.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  Obtener el IdIngreso de la fila seleccionada
            int idIngreso;
            try
            {
                // Asegurarse de que la columna IdIngreso exista y tenga valor
                if (dgvIngresos.SelectedRows[0].Cells["IdIngreso"]?.Value == null)
                {
                    MessageBox.Show("La fila seleccionada no tiene un ID de Ingreso válido.", "Error de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                idIngreso = Convert.ToInt32(dgvIngresos.SelectedRows[0].Cells["IdIngreso"].Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer el ID de Ingreso de la grilla: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // Buscar el IdPresupuesto asociado
            int? idPresupuesto = _presupuestoService.ObtenerIdPresupuestoPorIdIngreso(idIngreso);

            if (!idPresupuesto.HasValue)
            {
                MessageBox.Show("Este ingreso aún no tiene un presupuesto generado. No se puede marcar como devuelto.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Obtener el estado actual del presupuesto
            string estadoActual = _presupuestoService.ObtenerEstadoPresupuestoPorIdIngreso(idIngreso);

            if (string.IsNullOrEmpty(estadoActual))
            {
                MessageBox.Show("No se pudo encontrar el estado del presupuesto asociado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Comprobar el estado del presupuesto para permitir la devolución
            if (estadoActual.Equals("ENVIADO_A_CLIENTE", StringComparison.OrdinalIgnoreCase))
            {
                
                var confirm = MessageBox.Show(
                    "¿Confirma que el cliente está retirando este equipo SIN REPARAR?\n\nEsta acción es irreversible y registrará la fecha de retiro.",
                    "Confirmar Devolución",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.No) return;

                try
                {
                    _presupuestoService.RegistrarRetiroSinReparar(idPresupuesto.Value);
                    MessageBox.Show("Devolución registrada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarIngresos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al registrar la devolución: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(
                    "Este equipo no puede ser devuelto todavía.\n\n" +
                    $"Estado actual: {estadoActual}\n" +
                    "El presupuesto aún está siendo revisado por el Administrador o Técnico. Solo se pueden devolver equipos que hayan sido 'ENVIADO_A_CLIENTE' (y rechazados por el cliente).",
                    "Acción no permitida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
    }
}