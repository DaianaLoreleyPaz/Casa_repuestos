using CasaRepuestos.Models;
using CasaRepuestos.Services;
using System;
using System.Collections.Generic; // Necesario para List<>
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CasaRepuestos.Forms
{
    public partial class FrmReparaciones : Form
    {
        private readonly PresupuestoService _presupuestoService;
        private readonly ReparacionService _reparacionService;
        private readonly ServiciosService _servicioService = new ServiciosService();
        private List<Articulo> _articulosDisponibles = new();
        private List<Servicio> _serviciosDisponibles = new();

        //  detalles del presupuesto seleccionado (para editar)
        private List<DetallePresupuesto> _detallesActuales = new();
        private int _presupuestoSeleccionado = 0;

        public FrmReparaciones()
        {
            InitializeComponent();

            // Inicializa ambos servicios
            _presupuestoService = new PresupuestoService();
            _reparacionService = new ReparacionService();

            AplicarEstilos();
        }

        // Evento Load del formulario
        private void FrmReparaciones_Load(object sender, EventArgs e)
        {
            CargarCatalogos();
            CargarTodo();
            // Configurar DataGridView de detalles como solo lectura
            if (dgvDetalles != null)
            {
                dgvDetalles.AllowUserToAddRows = false;
                dgvDetalles.AllowUserToDeleteRows = false;
            }
            ConfigurarBotones();
        }

        private void AplicarEstilos()
        {
            this.BackColor = Color.FromArgb(240, 245, 249);

            // Aplicar estilos a GroupBoxes
            foreach (var gb in this.Controls.OfType<GroupBox>())
            {
                gb.BackColor = Color.White;
                gb.ForeColor = Color.FromArgb(45, 66, 91);
            }

            // Aplicar estilos a Botones
            foreach (var btn in this.Controls.OfType<Button>())
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Color.FromArgb(76, 132, 200);
                btn.ForeColor = Color.White;
            }

            // Estilo específico para btnMarcarReparado
            try { if (btnMarcarReparado != null) { btnMarcarReparado.BackColor = Color.FromArgb(34, 139, 34); btnMarcarReparado.ForeColor = Color.White; } } catch { }
            try { if (btnProcesarReparacion != null) { btnProcesarReparacion.BackColor = Color.FromArgb(12, 87, 150); btnProcesarReparacion.ForeColor = Color.White; } } catch { }

            foreach (var dgv in new[] { dgvEsperando, dgvEnProceso, dgvDetalles, dgvPendiente })
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
        /// <summary>
        /// Controla qué botones se ven según la pestaña activa para evitar errores lógicos.
        /// </summary>
        private void ConfigurarBotones()
        {
            // Si estamos en la pestaña "En Proceso"
            if (tabControlPrincipal.SelectedTab == tabEnProceso)
            {
                // NO tiene sentido pasar a "En Proceso" lo que ya está ahí.
                // Ocultamos el botón para evitar duplicar descuento de stock.
                if (btnProcesarReparacion != null) btnProcesarReparacion.Visible = false;

                // Aquí habilitamos el botón de Finalizar/Reparado
                if (btnMarcarReparado != null) btnMarcarReparado.Visible = true;
            }
            else
            {
                // Estamos en "Pendientes" o "Esperando Repuestos"
                // Aquí SI permitimos pasar a Proceso
                if (btnProcesarReparacion != null) btnProcesarReparacion.Visible = true;

                // Opcional: No deberíamos poder finalizar algo que no empezamos
                if (btnMarcarReparado != null) btnMarcarReparado.Visible = false;
            }
        }
        // Cargar lista de artículos y servicios
        private void CargarCatalogos()
        {
            _articulosDisponibles = _presupuestoService.GetArticulos() ?? new List<Articulo>();
            _serviciosDisponibles = _presupuestoService.GetServicios() ?? new List<Servicio>();
        }

        private void CargarTodo()
        {
            try
            {
                DataTable dt = _reparacionService.ObtenerPresupuestosConEstadoStock();

                if (!dt.Columns.Contains("stock_norm"))
                    dt.Columns.Add("stock_norm", typeof(string));

                foreach (DataRow r in dt.Rows)
                {
                    string raw = r["stock_status"]?.ToString() ?? "";
                    // Aquí se usa el método corregido de abajo
                    r["stock_norm"] = NormalizeStockStatus(raw);
                }
                var dtPendiente = dt.Clone();

                var dtEsperando = dt.Clone();
                var dtEnProceso = dt.Clone();

                // --- SWITCH CORREGIDO ---
                foreach (DataRow r in dt.Rows)
                {
                    string stock = r["stock_norm"]?.ToString() ?? "";
                    switch (stock)
                    {
                        case "PENDIENTE":
                            dtPendiente.ImportRow(r);
                            break;
                        case "ESPERA_REPUESTOS":
                        case "SIN_REPUESTOS": // Agregamos este caso aquí si quieres verlos en espera
                            dtEsperando.ImportRow(r);
                            break;

                        case "EN_PROCESO":
                            // CORREGIDO: Ahora sí va a la tabla correcta
                            dtEnProceso.ImportRow(r);
                            break;

                        

                        case "FINALIZADO":
                            // Los finalizados no se muestran en estas listas de trabajo
                            break;

                        default:
                            // Cualquier otro estado no reconocido va a pendientes
                            dtPendiente.ImportRow(r);
                            break;
                    }
                }

                dgvEsperando.DataSource = dtEsperando;
                dgvEnProceso.DataSource = dtEnProceso;
                dgvPendiente.DataSource = dtPendiente;


                // ajuste visual
                AjustarGridSimple(dgvEsperando);
                AjustarGridSimple(dgvEnProceso);
                AjustarGridSimple(dgvPendiente);


                // pintar filas por estado 
                PintarFilasSegunEstado(dgvEsperando, "ESPERA_REPUESTOS");
                PintarFilasSegunEstado(dgvEnProceso, "EN_PROCESO");
                PintarFilasSegunEstado(dgvPendiente, "PENDIENTE");


                // seleccionar primer elemento de la pestaña activa si existe
                SeleccionarPrimeraFilaSegunTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando reparaciones: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Ajusta columnas del DataGridView para mejor visualización
        private void AjustarGridSimple(DataGridView dgv)
        {
            if (dgv == null) return;

            dgv.AutoGenerateColumns = true;

            if (dgv.Columns.Count == 0) return;

            try
            {
                // 1. Configuración GENERAL: Ocultamos las columnas técnicas Y LA REDUNDANTE
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    // AQUI AGREGAMOS "stock_status" PARA QUE DESAPAREZCA SI O SI
                    if (col.Name == "stock_norm" ||
                        col.Name == "idempleado" ||
                        col.Name == "idingreso" ||
                        col.Name == "stock_status")
                    {
                        col.Visible = false;
                    }
                }

                // 2. Configuración ESPECÍFICA (Solo las que queremos ver)
                if (dgv.Columns.Contains("idpresupuesto"))
                {
                    dgv.Columns["idpresupuesto"].HeaderText = "ID";
                    dgv.Columns["idpresupuesto"].Width = 60;
                    dgv.Columns["idpresupuesto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgv.Columns.Contains("marca"))
                {
                    dgv.Columns["marca"].HeaderText = "Marca";
                    dgv.Columns["marca"].Width = 150;
                }

                if (dgv.Columns.Contains("modelo"))
                {
                    dgv.Columns["modelo"].HeaderText = "Modelo";
                    dgv.Columns["modelo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                // Nota: Ya borré el bloque que intentaba ocultar 'stock_status' abajo
                // porque ya lo manejamos arriba en el foreach.

                dgv.ClearSelection();
                dgv.CurrentCell = null;
            }
            catch
            {
                // Si falla algo visual, lo ignoramos para que el programa siga funcionando
            }
        }
        private void SeleccionarPrimeraFilaSegunTab()
        {
            // Primero determinamos cuál es la grilla activa según la pestaña
            DataGridView dgv = null;
            if (tabControlPrincipal.SelectedTab == tabEsperando) dgv = dgvEsperando;
            else if (tabControlPrincipal.SelectedTab == tabEnProceso) dgv = dgvEnProceso;
            else if (tabControlPrincipal.SelectedTab == tabPendiente) dgv = dgvPendiente;

            // Si no hay grilla activa o no se ha inicializado, salimos
            if (dgv == null) return;

            // quitar handlers antes de manipular selección
            dgvEsperando.SelectionChanged -= Grid_SelectionChanged;
            dgvEnProceso.SelectionChanged -= Grid_SelectionChanged;
            dgvPendiente.SelectionChanged -= Grid_SelectionChanged;

            if (dgv.Rows.Count > 0)
            {
                dgv.ClearSelection();
                dgv.Rows[0].Selected = true;
                CargarSeleccionadoDesdeGrid(dgv);
            }
            else
            {
                LimpiarDetalle();
            }

            // enganchar eventos nuevamente
            dgvEsperando.SelectionChanged += Grid_SelectionChanged;
            dgvEnProceso.SelectionChanged += Grid_SelectionChanged;
            dgvPendiente.SelectionChanged += Grid_SelectionChanged;
        }

        // Evento común para cambio de selección en grillas principales
        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            var dgv = sender as DataGridView;
            CargarSeleccionadoDesdeGrid(dgv);
        }

        // Obtener DataRowView del ítem seleccionado en el DataGridView
        private DataRowView GetSelectedRowView(DataGridView dgv)
        {
            if (dgv == null || dgv.SelectedRows.Count == 0) return null;
            return dgv.SelectedRows[0].DataBoundItem as DataRowView;
        }

        private void CargarSeleccionadoDesdeGrid(DataGridView dgv)
        {
            try
            {
                var drv = GetSelectedRowView(dgv);
                if (drv == null) { LimpiarDetalle(); return; }

                int id = Convert.ToInt32(drv["idpresupuesto"]);
                _presupuestoSeleccionado = id;

                // obtener ingreso (marca/modelo/falla) para mostrar detalles
                Ingreso ingreso = null;
                try
                {
                    ingreso = _presupuestoService.GetIngresoPorPresupuesto(id);
                }
                catch
                {
                    ingreso = null;
                }

                if (lblInfoModelo != null) lblInfoModelo.Text = $"Marca: {ingreso?.Marca ?? "-"} | Modelo: {ingreso?.Modelo ?? "-"}";
                if (lblInfoFalla != null) lblInfoFalla.Text = $"Falla: {ingreso?.Falla ?? "-"}";

                // cargar detalles (repuestos/servicios)
                var detalles = _presupuestoService.GetDetallesPresupuesto(id) ?? new List<DetallePresupuesto>();
                _detallesActuales = detalles;

                RecargarGridDetalles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar seleccionado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarDetalle()
        {
            if (lblInfoModelo != null) lblInfoModelo.Text = "Marca: - | Modelo: -";
            if (lblInfoFalla != null) lblInfoFalla.Text = "Falla: -";
            if (dgvDetalles != null)
            {
                dgvDetalles.DataSource = null;
                dgvDetalles.Rows.Clear();
            }
            _detallesActuales = new List<DetallePresupuesto>();
            _presupuestoSeleccionado = 0;
        }

        private void RecargarGridDetalles()
        {
            if (dgvDetalles == null) return;
            dgvDetalles.Columns.Clear();
            dgvDetalles.Rows.Clear();

            // Definición de columnas para SOLO VISUALIZACIÓN
            dgvDetalles.Columns.Add("Descripcion", "Descripción");
            dgvDetalles.Columns.Add("Cantidad", "Cantidad");
            dgvDetalles.Columns.Add("PrecioRepuesto", "Precio Repuesto");
            dgvDetalles.Columns.Add("PrecioServicio", "Precio Servicio");
            dgvDetalles.Columns.Add("Subtotal", "Subtotal");

            foreach (var d in _detallesActuales)
            {
                string descripcion = ObtenerDescripcionDetalle(d);
                decimal pr = d.PrecioRepuesto ?? 0m;
                decimal ps = d.PrecioServicio ?? 0m;
                decimal subtotal = (pr + ps) * d.Cantidad;

                dgvDetalles.Rows.Add(descripcion, d.Cantidad, pr.ToString("N2"), ps.ToString("N2"), subtotal.ToString("N2"));
            }

            // Establecer todas las celdas como NO EDITABLES
            foreach (DataGridViewColumn col in dgvDetalles.Columns)
            {
                col.ReadOnly = true;
            }

            // Ocultar cualquier columna de botón "Eliminar" si existe en el diseño
            if (dgvDetalles.Columns.Contains("Eliminar"))
                dgvDetalles.Columns["Eliminar"].Visible = false;

            dgvDetalles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }


        private string ObtenerDescripcionDetalle(DetallePresupuesto d)
        {
            // Determina si es artículo o servicio y obtiene la descripción adecuada
            if (d.IdArticulo.HasValue && d.IdArticulo.Value > 0)
            {
                var art = _articulosDisponibles.FirstOrDefault(a => a.IdArticulo == d.IdArticulo);
                return art?.Nombre ?? $"Artículo ID: {d.IdArticulo.Value}";
            }
            else if (d.IdServicio > 0)
            {
                var serv = _serviciosDisponibles.FirstOrDefault(s => s.IdServicio == d.IdServicio);
                return serv?.Descripcion ?? $"Servicio ID: {d.IdServicio}";
            }
            return "Detalle Desconocido";
        }

        // Manejar click en el botón "Marcar como Reparado"
        private void btnMarcarReparado_Click(object sender, EventArgs e)
        {
            if (_presupuestoSeleccionado == 0)
            {
                MessageBox.Show("Debe seleccionar un presupuesto para marcar como reparado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var confirm = MessageBox.Show("¿Confirmar que la reparación fue finalizada? Se descontará el stock de repuestos.", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                // Descontar stock (lanza excepción si hay faltantes)
                _reparacionService.ConsumirStockYFinalizar(_presupuestoSeleccionado);

                MessageBox.Show("Reparación marcada como FINALIZADA y stock actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarTodo();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Error de stock: {ex.Message}. Verifique el estado del stock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al finalizar la reparación: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- NORMALIZACIÓN CORREGIDA ---
        private string NormalizeStockStatus(string raw)
        {
            // CORREGIDO: Si es nulo o vacío, es PENDIENTE.
            if (string.IsNullOrWhiteSpace(raw)) return "PENDIENTE";

            string s = raw.Trim().ToUpperInvariant().Replace(" ", "_").Replace("-", "_");

            if (s == "ESPERANDO_REPUESTO" || s == "ESPERA_REPUESTO" || s.Contains("ESPERA"))
                return "ESPERA_REPUESTOS";

            if (s == "REPARADO" || s == "REPARADOS" || s == "FINALIZADO")
                return "FINALIZADO";

            if (s == "SIN_REPUESTO" || s == "NO_REPUESTOS")
                return "SIN_REPUESTOS";

            if (s == "EN_PROCESO")
                return "EN_PROCESO";

            // CORREGIDO: Eliminada la línea que forzaba PENDIENTE a EN_PROCESO.

            // Si es "PENDIENTE" o cualquier otra cosa no reconocida, se devuelve "PENDIENTE".
            return "PENDIENTE";
        }

        // Colorea filas para mejor visualización
        private void PintarFilasSegunEstado(DataGridView dgv, string estadoClave)
        {
            if (dgv == null) return;
            try
            {
                dgv.SuspendLayout();
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.IsNewRow) continue;
                    var stockNorm = "";
                    if (row.DataBoundItem is DataRowView drv && drv.Row.Table.Columns.Contains("stock_norm"))
                    {
                        stockNorm = drv["stock_norm"]?.ToString() ?? "";
                    }

                    if (estadoClave == "ESPERA_REPUESTOS" && (stockNorm == "ESPERA_REPUESTOS" || stockNorm == "SIN_REPUESTOS"))
                    {
                        row.DefaultCellStyle.BackColor = Color.LightSalmon;
                    }
                    else if (estadoClave == "EN_PROCESO" && stockNorm == "EN_PROCESO")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                    else if (estadoClave == "PENDIENTE" && stockNorm == "PENDIENTE")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightYellow; // Un color diferente para pendiente
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
            finally
            {
                dgv.ResumeLayout();
            }
        }

        private void btnProcesarReparacion_Click(object sender, EventArgs e)
        {
            // 1. Validaciones previas
            if (tabControlPrincipal.SelectedTab == tabEnProceso)
            {
                MessageBox.Show("El presupuesto seleccionado ya se encuentra en proceso de reparación.",
                                "Acción Redundante", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_presupuestoSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un presupuesto de la lista para continuar.",
                                "Selección Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Mensaje de confirmación contextual (Opcional: puedes dejar el genérico si prefieres)
                string mensajePregunta = "El sistema verificará automáticamente la disponibilidad de stock.\n¿Desea continuar?";

                var confirm = MessageBox.Show(mensajePregunta, "Verificar Stock", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) return;

                // 2. Llamada al servicio
                string resultadoEstado = _reparacionService.ProcesarInicioReparacion(_presupuestoSeleccionado);

                // 3. MENSAJES ESPECÍFICOS (Ahora con el nuevo caso)

                if (resultadoEstado == "EN_PROCESO")
                {
                    MessageBox.Show(
                        "¡Stock Encontrado!\n\n" +
                        "Se han asignado los repuestos correctamente.\n" +
                        "El presupuesto avanza a estado: EN PROCESO.",
                        "Inicio de Reparación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (resultadoEstado == "EN_PROCESO_SERVICIO")
                {
                    MessageBox.Show(
                        "¡Servicio Iniciado!\n\n" +
                        "Se inicia la mano de obra (sin consumo de stock).\n" +
                        "Estado actual: EN PROCESO.",
                        "Inicio de Servicio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (resultadoEstado == "ESPERA_REPUESTOS")
                {
                    // Este sale cuando viene de PENDIENTE -> ESPERA
                    MessageBox.Show(
                        "Stock Insuficiente.\n\n" +
                        "No hay stock disponible en este momento.\n" +
                        "El presupuesto pasa a estado: ESPERA DE REPUESTOS.",
                        "Faltante de Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (resultadoEstado == "AUN_SIN_STOCK")
                {
                    // NUEVO MENSAJE ESPECÍFICO (Cuando ya estaba esperando y sigue igual)
                    MessageBox.Show(
                        "Verificación Completada: Stock no disponible.\n\n" +
                        "Aún no se han registrado compras o ingresos de mercadería para los repuestos solicitados.\n" +
                        "El presupuesto se mantiene en espera.",
                        "Sin Novedades", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            CargarCatalogos();
            CargarTodo();
        }
        private void tabControlPrincipal_SelectedIndexChanged(object sender, EventArgs e)
        {
            SeleccionarPrimeraFilaSegunTab();
            ConfigurarBotones();
        }

        private void dgvEsperando_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var drv = GetSelectedRowView(dgvEsperando);
            if (drv != null) CargarSeleccionadoDesdeGrid(dgvEsperando);
        }

        private void dgvEnProceso_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var drv = GetSelectedRowView(dgvEnProceso);
            if (drv != null) CargarSeleccionadoDesdeGrid(dgvEnProceso);
        }

        private void dgvPendiente_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var drv = GetSelectedRowView(dgvPendiente);
            if (drv != null) CargarSeleccionadoDesdeGrid(dgvPendiente);
        }
    }
}