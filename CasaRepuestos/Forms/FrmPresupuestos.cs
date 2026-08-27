
using CasaRepuestos.Models;
using CasaRepuestos.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

using System.Globalization;





namespace CasaRepuestos.Forms
{
    public partial class FrmPresupuestos : Form
    {
        // Guardamos el ID y Rol del usuario actual para aplicar permisos (Admin vs Técnico).
        private int _empleadoId;
        private readonly string _userRole;
        // Instancias de los servicios para interactuar con la BD.
        private PresupuestoService _presupuestoService;
        private readonly ServiciosService _servicioService = new();
        private readonly ComprasService _comprasService = new();
        private readonly ReparacionService _reparacionService = new();
        private readonly ArticuloService _articuloService = new ArticuloService();
        // Listas en memoria para evitar consultas repetitivas a la BD mientras se usa el formulario.
        private List<Servicio> _serviciosDisponibles;
        private List<Articulo> _articulosDisponibles;
        // LISTA PRINCIPAL: Aquí se guardan los ítems del presupuesto actual antes de guardar en BD.
        private List<DetallePresupuesto> _detallesPresupuesto;
        private List<Ingreso> _todosIngresos;
        private List<Presupuesto> _todosPresupuestos;
        // Variables de control para saber si estamos editando o creando uno nuevo.
        private decimal _totalPresupuesto = 0m;
        private bool _editandoPresupuestoExistente = false;
        private int _presupuestoSeleccionadoId = 0;

        private List<string> _solicitudesDescripcion = new List<string>();
        private string _rutaArchivoSolicitudes => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PresupuestosSolicitudes.txt");



        public FrmPresupuestos() : this(0, null) { }

        // Constructor principal: Recibe quién inicia sesión para configurar el entorno.
        public FrmPresupuestos(int empleadoId, string userRole = null)
        {
            InitializeComponent();
            _empleadoId = empleadoId;
            _userRole = userRole;


            // Inicializamos las listas vacías para evitar errores de referencia nula (NullReferenceException).
            _presupuestoService = new PresupuestoService();
            _detallesPresupuesto = new List<DetallePresupuesto>();
            _serviciosDisponibles = new List<Servicio>();
            _articulosDisponibles = new List<Articulo>();
            _todosIngresos = new List<Ingreso>();
            _todosPresupuestos = new List<Presupuesto>();
            _solicitudesDescripcion = new List<string>();

            // Configuración visual de la ventana.
            this.Text = $"Presupuestos - Empleado ID: {empleadoId}";
            this.WindowState = FormWindowState.Normal;
            this.Size = new Size(1366, 768);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void FrmPresupuestos_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Configura visualmente las columnas de las grillas (ancho, moneda, ocultar IDs).
                ConfigurarColumnasGrids();
                // 2. Habilita o bloquea botones según si es ADMIN o TÉCNICO.
                AplicarPermisos();
                // 3. Estética general (colores corporativos).
                AplicarEstilos();
                // 4. Antes de mostrar nada, actualiza estados de vencimiento.
                _presupuestoService.ActualizarEstadoAutomatico();
                // 5. Carga de datos masiva desde la BD a las listas en memoria.
                CargarServicios();// Llena el ComboBox de servicios
                CargarRepuestos();// Llena el ComboBox de repuestos
                CargarIngresos();// Llena la grilla de equipos ingresados
                CargarPresupuestos();// Llena la grilla de presupuestos históricos


                dgvPresupuestos.ClearSelection();
                dgvPresupuestos.CurrentCell = null;
                dgvIngresos.ClearSelection();
                dgvIngresos.CurrentCell = null;
                // Limpia el formulario para empezar de cero.
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inicializando: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        

        #region Cargas desde BD 
        private void CargarServicios()
        {
            try
            {
                _serviciosDisponibles = _presupuestoService.GetServicios() ?? new List<Servicio>();
                cbServicios.DataSource = null;
                cbServicios.DataSource = _serviciosDisponibles;
                cbServicios.DisplayMember = "Descripcion";
                cbServicios.ValueMember = "IdServicio";
                cbServicios.SelectedIndex = -1;
                cbServicios.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar servicios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarRepuestos()
        {
            try
            {
                // Carga los repuestos disponibles desde la base de datos.
                _articulosDisponibles = _presupuestoService.GetArticulos() ?? new List<Articulo>();
                // Vincula la lista al ComboBox.
                cbRepuestos.DataSource = null;
                cbRepuestos.DataSource = _articulosDisponibles;
                cbRepuestos.DisplayMember = "Nombre";
                cbRepuestos.ValueMember = "IdArticulo";
                // Establece el índice seleccionado como -1 (ninguno seleccionado).
                cbRepuestos.SelectedIndex = -1;
                // Limpia el texto del ComboBox.
                cbRepuestos.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar repuestos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarIngresos()
        {
            try
            {
                // Obtiene todos los ingresos desde la base de datos.
                _todosIngresos = _presupuestoService.GetIngresos() ?? new List<Ingreso>();
                // Filtra los ingresos que no tienen un presupuesto asociado.
                var list = _todosIngresos.Select(i => new
                {
                    i.IdIngreso,
                    Marca = i.Marca ?? "",
                    Modelo = i.Modelo ?? "",
                    TipoDispositivo = i.TipoDispositivo.ToString(),
                    Falla = i.Falla ?? ""
                }).ToList();
                // Vincula la lista filtrada al DataGridView.
                dgvIngresos.DataSource = null;
                dgvIngresos.DataSource = list;
                dgvIngresos.ClearSelection();
                dgvIngresos.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando ingresos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarPresupuestos()
        {
            try
            {
                // Actualiza los estados automáticos antes de cargar.
                _presupuestoService.ActualizarEstadoAutomatico();
                //  Obtiene todos los presupuestos desde la base de datos.
                _todosPresupuestos = _presupuestoService.GetPresupuestos() ?? new List<Presupuesto>();
                // Filtra los presupuestos visibles según los criterios especificados.
                var presupuestosVisibles = _todosPresupuestos
                    .Where(p =>
                        p.Autorizado?.ToUpper() == "NO" && 
                        (
                         p.Estado == "VERIFICAR_PRECIO" ||
                         p.Estado == "APROBADO_ADMIN" ||
                         p.Estado == "ENVIADO_A_CLIENTE")) 
                    .ToList();
                // Proyecta los datos necesarios para mostrar en la grilla.
                var list = presupuestosVisibles.Select(p => new
                {
                    p.IdPresupuesto,
                    Fecha = p.Fecha.HasValue ? p.Fecha.Value.ToShortDateString() : "Sin Fecha",
                    Total = p.Total,
                    Autorizado = p.Autorizado,
                    Estado = p.Estado,
                    p.IdIngreso
                }).ToList();
                // Vincula la lista proyectada al DataGridView.
                dgvPresupuestos.DataSource = null;
                dgvPresupuestos.DataSource = list;

                // Limpia cualquier selección previa.
                dgvPresupuestos.ClearSelection();
                dgvPresupuestos.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando presupuestos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Columnas / estilos / utilidades UI 
        // Configura las columnas de los DataGridViews (ancho, formato, ocultar IDs).
        private void ConfigurarColumnasGrids()
        {
            dgvIngresos.AutoGenerateColumns = false;
            dgvIngresos.Columns.Clear();
            dgvIngresos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIngresos.RowHeadersVisible = false;
            dgvIngresos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 230, 255);
            dgvIngresos.DefaultCellStyle.SelectionForeColor = Color.Black;
            // Configuración de columnas
            dgvIngresos.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdIngreso", HeaderText = "ID", DataPropertyName = "IdIngreso", Width = 60 });
            dgvIngresos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Marca", HeaderText = "Marca", DataPropertyName = "Marca", Width = 120 });
            dgvIngresos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Modelo", HeaderText = "Modelo", DataPropertyName = "Modelo", Width = 160 });
            dgvIngresos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Tipo", HeaderText = "Tipo", DataPropertyName = "TipoDispositivo", Width = 120 });
            dgvIngresos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Falla", HeaderText = "Falla", DataPropertyName = "Falla", Width = 120 });
            // Configuración de selección
            dgvPresupuestos.AutoGenerateColumns = false;
            dgvPresupuestos.Columns.Clear();
            dgvPresupuestos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPresupuestos.RowHeadersVisible = false;
            // Configuración de columnas
            dgvPresupuestos.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdPresupuesto", HeaderText = "Presupuesto", DataPropertyName = "IdPresupuesto", Width = 150 });
            dgvPresupuestos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Fecha", HeaderText = "Fecha", DataPropertyName = "Fecha", Width = 220 });
            dgvPresupuestos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Total", HeaderText = "Total", DataPropertyName = "Total", Width = 210, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } });
            dgvPresupuestos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Estado", HeaderText = "Estado", DataPropertyName = "Estado", Width = 230 });
            dgvPresupuestos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Autorizado", HeaderText = "Autorizado", DataPropertyName = "Autorizado", Width = 150 });
            // Configuración de selección
            dgvDetalles.AutoGenerateColumns = false;
            dgvDetalles.Columns.Clear();
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn { Name = "Descripcion", HeaderText = "Servicio/Repuesto", Width = 350 });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PrecioCosto",
                HeaderText = "Costo ($)",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", BackColor = Color.LightYellow }
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PorcentajeGanancia",
                HeaderText = "Ganancia (%)",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", BackColor = Color.LightYellow }
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IVA",
                HeaderText = "IVA (%)",
                Width = 70,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", BackColor = Color.LightYellow }
            });

            // Columnas de Venta (Públicas)
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn { Name = "Cantidad", HeaderText = "Cant.", Width = 60 });
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn { Name = "PrecioRepuesto", HeaderText = "P. Repuesto", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } });
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn { Name = "PrecioServicio", HeaderText = "P. Servicio", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } });
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn { Name = "Subtotal", HeaderText = "Subtotal", Width = 130, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } });
            dgvDetalles.Columns.Add(new DataGridViewButtonColumn { Name = "Eliminar", HeaderText = "", Text = "X", UseColumnTextForButtonValue = true, Width = 50 });

            dgvDetalles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalles.RowHeadersVisible = false;
            dgvDetalles.AllowUserToAddRows = false;
        }
        // Aplica colores y estilos corporativos a los controles del formulario.
        private void AplicarEstilos()
        {
            this.BackColor = Color.FromArgb(240, 245, 249);
            foreach (var gb in this.Controls.OfType<GroupBox>())
            {
                gb.BackColor = Color.White;
                gb.ForeColor = Color.FromArgb(45, 66, 91);
            }
            foreach (var btn in this.Controls.OfType<Button>())
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Color.FromArgb(76, 132, 200);
                btn.ForeColor = Color.White;
            }
            try { btnImprimir.BackColor = Color.FromArgb(12, 87, 150); } catch { }
            try { btnAutorizar.BackColor = Color.FromArgb(34, 139, 34); btnAutorizar.ForeColor = Color.White; } catch { }

            foreach (var dgv in new[] { dgvIngresos, dgvPresupuestos, dgvDetalles })
            {
                dgv.BackgroundColor = Color.White;
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 66, 91);
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            }
        }

        #endregion

        #region Selecciones / carga detalle 
        // Se ejecuta al hacer clic en un equipo de la lista de "Ingresos Pendientes".
        private void dgvIngresos_SelectionChanged(object sender, EventArgs e)
        {
            // Si no hay filas seleccionadas, limpia el formulario.
            if (dgvIngresos.SelectedRows.Count == 0)
            {
                LimpiarFormulario();
                return;
            }

            try
            {
                // Obtiene el ID de la fila seleccionada y carga los datos en los TextBoxes.
                var id = Convert.ToInt32(dgvIngresos.SelectedRows[0].Cells["IdIngreso"].Value);
                var ingreso = _todosIngresos.FirstOrDefault(x => x.IdIngreso == id);
                // Si este ingreso ya tiene un presupuesto asociado, lo carga automáticamente.
                if (ingreso != null) CargarDetallesIngreso(ingreso);
            }
            catch { /* Manejo de excepciones */ }
        }
        // Se ejecuta al seleccionar un Presupuesto ya existente de la grilla inferior.
        private void dgvPresupuestos_SelectionChanged(object sender, EventArgs e)
        {
            // Si no hay filas seleccionadas, oculta el botón Autorizar y sale.
            if (dgvPresupuestos.SelectedRows.Count == 0)
            {
                btnAutorizar.Visible = false;
                return;
            }

            try
            {
                // Recuperamos el presupuesto completo usando el ID de la fila.
                var id = Convert.ToInt32(dgvPresupuestos.SelectedRows[0].Cells["IdPresupuesto"].Value);
                var presupuesto = _todosPresupuestos.FirstOrDefault(p => p.IdPresupuesto == id);// Obtener el presupuesto completo
                if (presupuesto != null)
                {
                    // Llenamos la grilla de detalles con los items de este presupuesto.
                    CargarDetallesPresupuesto(presupuesto);
                    // Activamos el modo "Edición" para hacer Updates en lugar de Inserts.
                    _editandoPresupuestoExistente = true;
                    _presupuestoSeleccionadoId = presupuesto.IdPresupuesto;
                    // Lógica visual del botón Autorizar (Cambia el texto dinámicamente).
                    btnAutorizar.Visible = true;
                    btnAutorizar.Text = (presupuesto.Autorizado?.ToUpper() == "SI") ? "❌ Desautorizar" : "✅ Autorizar";
                }
                else
                {
                    btnAutorizar.Visible = false;
                }
            }
            catch { }
        }
        // Carga los detalles del ingreso seleccionado en los TextBoxes y prepara el formulario.
        private void CargarDetallesIngreso(Ingreso ingreso)
        {
            // Llena los TextBoxes con la información del ingreso seleccionado.
            txtIdIngreso.Text = ingreso.IdIngreso.ToString();
            txtMarcaIngreso.Text = ingreso.Marca ?? "";
            txtFecha.Text = ingreso.FechaIngreso.ToShortDateString();
            txtModelo.Text = ingreso.Modelo ?? "";
            txtTipoDispositivo.Text = ingreso.TipoDispositivo.ToString();
            txtFalla.Text = ingreso.Falla ?? "";
            // Limpia los detalles previos y el total.
            _detallesPresupuesto.Clear();
            _solicitudesDescripcion.Clear();
            dgvDetalles.Rows.Clear();
            _totalPresupuesto = 0m;
            ActualizarTotal();// Refresca el total mostrado.
            // Verifica si este ingreso ya tiene un presupuesto asociado.
            var presupuestoAsociado = _todosPresupuestos.FirstOrDefault(p => p.IdIngreso == ingreso.IdIngreso);
            if (presupuestoAsociado != null)// Si existe, carga sus detalles.
            {
                // Carga los detalles del presupuesto existente.
                CargarDetallesPresupuesto(presupuestoAsociado);
                _editandoPresupuestoExistente = true;// Modo edición activado
                _presupuestoSeleccionadoId = presupuestoAsociado.IdPresupuesto;// Guarda el ID del presupuesto
                btnAutorizar.Visible = true;// Muestra el botón Autorizar
                btnAutorizar.Text = (presupuestoAsociado.Autorizado?.ToUpper() == "SI") ? "❌ Desautorizar" : "✅ Autorizar";// Cambia el texto del botón según el estado
            }
            else
            {
                // Si no existe, prepara para un nuevo presupuesto.
                _editandoPresupuestoExistente = false;
                _presupuestoSeleccionadoId = 0;// No hay presupuesto seleccionado
                btnAutorizar.Visible = false;// Oculta el botón Autorizar
            }
        }
        // Carga los detalles de un presupuesto existente en la grilla y TextBoxes.
        private void CargarDetallesPresupuesto(Presupuesto presupuesto)
        {
            try
            {
                // Cargamos los datos del ingreso asociado
                var ingreso = _presupuestoService.GetIngresoPorPresupuesto(presupuesto.IdPresupuesto);
                // Llenamos los TextBoxes con la información del ingreso
                if (ingreso != null)
                {
                    txtIdIngreso.Text = ingreso.IdIngreso.ToString();// ID del ingreso
                    txtMarcaIngreso.Text = ingreso.Marca ?? "";//   Marca del dispositivo
                    txtFecha.Text = ingreso.FechaIngreso.ToShortDateString();// Fecha de ingreso
                    txtModelo.Text = ingreso.Modelo ?? "";// Modelo del dispositivo
                    txtTipoDispositivo.Text = ingreso.TipoDispositivo.ToString();
                    txtFalla.Text = ingreso.Falla ?? "";
                }

                // Cargamos los detalles del presupuesto
                var detalles = _presupuestoService.GetDetallesPresupuesto(presupuesto.IdPresupuesto) ?? new List<DetallePresupuesto>();
                _detallesPresupuesto = detalles;// Guardamos en la lista en memoria

                dgvDetalles.Rows.Clear();// Limpiamos la grilla
                _totalPresupuesto = 0m;// Reiniciamos el total

                var solicitudes = new List<string>();// Lista para solicitudes de artículos
                try { solicitudes = LeerSolicitudesDesdeArchivo(presupuesto.IdPresupuesto); }// Leemos desde el archivo
                catch { solicitudes = new List<string>(); }// En caso de error, lista vacía

                // Llenamos la grilla 
                foreach (var detalle in _detallesPresupuesto)
                {
                    // Obtenemos la descripción completa del detalle
                    string descripcionDGV = ObtenerDescripcionDetalle(detalle);
                    decimal precioRep = detalle.PrecioRepuesto.GetValueOrDefault();// Precio del repuesto
                    decimal precioMO = detalle.PrecioServicio.GetValueOrDefault();// Precio del servicio
                    decimal subtotal = (precioRep + precioMO) * detalle.Cantidad;// Cálculo del subtotal

                    if (detalle.IdArticulo.HasValue && detalle.IdArticulo.Value > 0)
                    {
                        // Si es Repuesto, llenamos todas las columnas
                        dgvDetalles.Rows.Add(
                            descripcionDGV,
                            detalle.PrecioCosto,
                            detalle.PorcentajeGanancia,
                            detalle.IVA,
                            detalle.Cantidad,
                            precioRep,
                            precioMO,
                            subtotal
                        );
                    }
                    else
                    {
                        // Si es SOLO SERVICIO, dejamos las celdas de Admin NULAS
                        dgvDetalles.Rows.Add(
                            descripcionDGV,
                            DBNull.Value, // PrecioCosto
                            DBNull.Value, // PorcentajeGanancia
                            DBNull.Value, // IVA
                            detalle.Cantidad,
                            precioRep,
                            precioMO,
                            subtotal
                        );
                    }
                }
                // Agregamos las solicitudes al final
                foreach (var sol in solicitudes)
                {
                    // Fila especial para solicitudes
                    dgvDetalles.Rows.Add($"[SOLICITADO] {sol}", DBNull.Value, DBNull.Value, DBNull.Value, 1, 0m, 0m, 0m);
                }
                // Actualizamos el total del presupuesto
                if (presupuesto.Total > 0m) _totalPresupuesto = presupuesto.Total;

                ActualizarTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar detalles del presupuesto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Método auxiliar para obtener la descripción completa de un detalle (Repuesto + Servicio).
        private string ObtenerDescripcionDetalle(DetallePresupuesto detalle)
        {
            // Verificamos si es un Repuesto + Servicio o solo Servicio
            if (detalle.IdArticulo.HasValue)
            {
                // Es un Repuesto + Servicio
                var a = _articulosDisponibles?.FirstOrDefault(x => x.IdArticulo == detalle.IdArticulo.Value);
                var s = _serviciosDisponibles?.FirstOrDefault(x => x.IdServicio == detalle.IdServicio);// Obtener el servicio asociado
                if (a != null && s != null) return $"{a.Nombre} (Repuesto) + {s.Descripcion}";// Descripción completa
                
                else if (s != null) return $"Artículo {detalle.IdArticulo.Value} + {s.Descripcion} (Servicio)";// Artículo desconocido
                                                                                                               // Ambos desconocidos
                return $"Artículo ID:{detalle.IdArticulo.Value} + Servicio ID:{detalle.IdServicio}";// Descripción genérica
            }
            else
            {
                // Es solo un Servicio
                var s = _serviciosDisponibles?.FirstOrDefault(x => x.IdServicio == detalle.IdServicio);
                return s?.Descripcion ?? $"Servicio ID:{detalle.IdServicio}";// Descripción del servicio o genérica
            }
        }

        #endregion

        #region Agregar / Eliminar detalles 
        // Agrega un Servicio a la lista de detalles.
        private void btnAgregarServicio_Click(object sender, EventArgs e)
        {
            // Validación: Aseguramos que se haya seleccionado un servicio válido.
            if (cbServicios.SelectedValue == null || !int.TryParse(cbServicios.SelectedValue.ToString(), out int idServ))
            {
                MessageBox.Show("Seleccione un servicio válido.");
                return;
            }
            // Buscamos el servicio en la lista cargada en memoria.
            var servicio = _serviciosDisponibles.FirstOrDefault(s => s.IdServicio == idServ);
            if (servicio == null)
            {
                MessageBox.Show("Servicio no encontrado en la lista.");
                return;
            }

            // Validación: Evitamos duplicar el mismo servicio en el presupuesto.
            if (_detallesPresupuesto.Any(d => d.IdServicio == servicio.IdServicio && d.IdArticulo.GetValueOrDefault() == 0))
            {
                MessageBox.Show("Ese servicio ya fue agregado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Creamos el objeto detalle solo con el servicio (sin repuesto).
            var detalle = new DetallePresupuesto
            {
                IdServicio = servicio.IdServicio,// Solo servicio, sin repuesto
                IdArticulo = null,// No hay artículo asociado
                Cantidad = 1,// Cantidad por defecto
                PrecioRepuesto = 0,
                PrecioServicio = servicio.Precio
            };

            _detallesPresupuesto.Add(detalle);

            
            RecargarDetallesGrid();// Refrescamos la grilla de detalles
        }
        // Actualiza la información de stock cuando se selecciona un repuesto.
        private void cbRepuestos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Actualiza la etiqueta de stock según el repuesto seleccionado.
                if (cbRepuestos.SelectedItem != null)
                {
                    var art = (Articulo)cbRepuestos.SelectedItem;// Obtener el artículo seleccionado
                    lblStockInfo.Text = $"Stock disponible: {art.Stock}";// Actualizar etiqueta de stock
                }
                else lblStockInfo.Text = "Stock disponible: -";// Ningún artículo seleccionado
            }
            catch { lblStockInfo.Text = "Stock disponible: -"; }// En caso de error, mostrar guión
        }

        // Agrega un Repuesto Físico a la lista de detalles.
        private void btnAgregarRepuesto_Click(object sender, EventArgs e)
        {
            // Validación: Aseguramos que se haya seleccionado un repuesto válido.
            if (cbRepuestos.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un repuesto");
                return;
            }
            // Obtenemos el artículo seleccionado.
            var articulo = (Articulo)cbRepuestos.SelectedItem;
            //  Todo repuesto físico debe ir acompañado de un servicio de mano de obra.
            var servicioAsociado = _servicioService.GetServicioPorArticuloId(articulo.IdArticulo);
            //  Validación: Si no hay servicio asociado, mostramos un error.
            if (servicioAsociado == null)
            {
                MessageBox.Show($"El repuesto '{articulo.Nombre}' debe tener un servicio de mano de obra asociado. Configure la relación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Validación: Evitamos duplicar el mismo repuesto en el presupuesto.
            if (_detallesPresupuesto.Any(d => d.IdArticulo == articulo.IdArticulo))
            {
                MessageBox.Show("Ese repuesto ya fue agregado. No se permiten duplicados.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Obtenemos los precios necesarios.
            decimal precioArticuloVenta = (decimal)articulo.Precio;
            decimal precioServicio = servicioAsociado.Precio;// Precio del servicio asociado
            // Creamos el objeto detalle con datos del Repuesto y del Servicio asociado.
            var detalle = new DetallePresupuesto
            {
                // Datos del Repuesto
                IdArticulo = articulo.IdArticulo,
                IdServicio = servicioAsociado.IdServicio,
                Cantidad = 1,

                // Precios de VENTA (Públicos)
                PrecioRepuesto = precioArticuloVenta,
                PrecioServicio = precioServicio,

                // Datos de Cálculo (Admin)
                PrecioCosto = articulo.PrecioCosto,
                PorcentajeGanancia = articulo.PorcentajeGanancia,
                IVA = articulo.IVA
            };
            // Agregamos a la lista en memoria y refrescamos la interfaz visual.
            _detallesPresupuesto.Add(detalle);
            decimal subtotal = detalle.PrecioRepuesto.GetValueOrDefault() + detalle.PrecioServicio.GetValueOrDefault();// Cálculo del subtotal
            string descripcionDGV = $"{articulo.Nombre} (Repuesto) + {servicioAsociado.Descripcion} (Servicio)";// Descripción completa


            RecargarDetallesGrid();
        }
        // Método auxiliar que sincroniza la lista lógica (_detallesPresupuesto) con la grilla visual (dgvDetalles).
        private void RecargarDetallesGrid()
        {
         
            dgvDetalles.DataSource = null;// Desvinculamos cualquier fuente de datos previa
            dgvDetalles.Rows.Clear();// Limpiamos todas las filas existentes

            // Recorremos cada detalle en la lista lógica para agregarlo a la grilla.
            foreach (var detalle in _detallesPresupuesto)
            {
                //  Obtener la descripción 
                string descripcionDGV = ObtenerDescripcionDetalle(detalle);

                //  Cálculo de Subtotal 
                decimal subtotal = (detalle.PrecioRepuesto.GetValueOrDefault() + detalle.PrecioServicio.GetValueOrDefault()) * detalle.Cantidad;

                // Verificamos si es un Repuesto 
                if (detalle.IdArticulo.HasValue && detalle.IdArticulo.Value > 0)
                {
                    // Si es Repuesto, llenamos todas las columnas
                    dgvDetalles.Rows.Add(
                        descripcionDGV, 

                        // --- Columnas de Admin ---
                        detalle.PrecioCosto,         // Col 1: PrecioCosto
                        detalle.PorcentajeGanancia,  // Col 2: PorcentajeGanancia
                        detalle.IVA,                 // Col 3: IVA

                        // --- Columnas Públicas ---
                        detalle.Cantidad,                     // Col 4: Cantidad
                        detalle.PrecioRepuesto.GetValueOrDefault(), // Col 5: PrecioRepuesto
                        detalle.PrecioServicio.GetValueOrDefault(), // Col 6: PrecioServicio
                        subtotal                            // Col 7: Subtotal
                    );
                }
                else
                {
                    // Si es SOLO SERVICIO, dejamos las celdas de Admin NULAS
                    dgvDetalles.Rows.Add(
                        descripcionDGV,

                        DBNull.Value, // PrecioCosto
                        DBNull.Value, // PorcentajeGanancia
                        DBNull.Value, // IVA

                        detalle.Cantidad,
                        detalle.PrecioRepuesto.GetValueOrDefault(), 
                        detalle.PrecioServicio.GetValueOrDefault(),
                        subtotal
                    );
                }
            }

            //  Actualizar el total general
            ActualizarTotal();
        }
        // Maneja el evento de clic en una celda del DataGridView de detalles (por ejemplo, para eliminar un detalle).
        private void dgvDetalles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           //
        }

        #endregion

        #region Solicitar artículo 
        private List<string> LeerSolicitudesDesdeArchivo(int presupuestoId)
        {
            var res = new List<string>();
            try
            {
                if (!File.Exists(_rutaArchivoSolicitudes)) return res;
                var lines = File.ReadAllLines(_rutaArchivoSolicitudes);
                var tag = $"PresupuestoId:{presupuestoId}|";
                foreach (var l in lines)
                {
                    if (l.StartsWith(tag))
                    {
                        res.Add(l.Substring(tag.Length));
                    }
                }
            }
            catch { }
            return res;
        }

        #endregion

        #region Guardar / Limpiar / Imprimir / Autorizar
        // Guarda el presupuesto actual en la base de datos (nuevo o editado).
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validaciones básicas de entrada.
            if (string.IsNullOrEmpty(txtIdIngreso.Text))
            {
                MessageBox.Show("Seleccione un ingreso para generar el presupuesto", "Advertencia",
                     MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Debe haber al menos un detalle agregado.
            if (_detallesPresupuesto.Count == 0)
            {
                MessageBox.Show("Agregue al menos un servicio o repuesto al presupuesto", "Advertencia",
                     MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Determinamos el estado final del presupuesto.
                string estadoFinal = "VERIFICAR_PRECIO";
                // Si estamos editando, mantenemos el estado original o bloqueamos si es Técnico.
                if (_editandoPresupuestoExistente && _presupuestoSeleccionadoId > 0)
                {
                    // Obtenemos el presupuesto original para verificar su estado.
                    Presupuesto presupuestoOriginal = _presupuestoService.GetPresupuestoById(_presupuestoSeleccionadoId);
                    string estadoOriginal = presupuestoOriginal.Estado;// Estado actual en la BD

                    // Seguridad: Un técnico no puede modificar presupuestos ya enviados o aprobados.
                    if (_userRole == "TECNICO" && estadoOriginal != "VERIFICAR_PRECIO")
                    {
                        MessageBox.Show($"El presupuesto ya fue enviado o aprobado (Estado: {estadoOriginal}). No puedes modificarlo. Solo puedes modificar presupuestos en estado 'CREADO'.",
                                        "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return; 
                    }

                    
                    estadoFinal = estadoOriginal;// Mantenemos el estado original
                }

                // Construimos el objeto Presupuesto para enviar a la BD.
                var presupuesto = new Presupuesto
                {
                    //IdPresupuesto se asigna solo si es edición
                    Fecha = DateTime.Now,
                    Total = _totalPresupuesto,
                    Autorizado = "NO",
                    Estado = estadoFinal, 
                    IdEmpleado = _empleadoId,
                    IdIngreso = int.Parse(txtIdIngreso.Text)
                };
                // Decisión: UPDATE o INSERT
                if (_editandoPresupuestoExistente && _presupuestoSeleccionadoId > 0)
                {
                    // Actualizamos el presupuesto existente
                    presupuesto.IdPresupuesto = _presupuestoSeleccionadoId;
                    _presupuestoService.UpdatePresupuesto(presupuesto, _detallesPresupuesto);
                    MessageBox.Show("Presupuesto actualizado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Creamos un nuevo presupuesto
                    int nuevoId = _presupuestoService.CreatePresupuesto(presupuesto, _detallesPresupuesto);
                    MessageBox.Show($"Presupuesto #{nuevoId} creado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // Recargamos todo para reflejar cambios y limpiamos.
                CargarPresupuestos();
                CargarIngresos();
                LimpiarFormulario();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar presupuesto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();// Limpia todos los campos y resetea el estado
        }

        private void AplicarPermisos()
        {
            // Determinamos el rol del usuario actual
            bool esTecnico = string.Equals(_userRole, "TECNICO", StringComparison.OrdinalIgnoreCase);
            bool esAdmin = string.Equals(_userRole, "ADMINISTRADOR", StringComparison.OrdinalIgnoreCase);

            // --- Visibilidad de Botones Admin ---
            btnAprobarAdmin.Visible = esAdmin;
            btnEnviarCliente.Visible = !esTecnico;
          
            lblStockInfo.Visible = !esTecnico;
            dgvPresupuestos.Visible = !esTecnico;

            bool adminPuedeEditar = esAdmin;// Solo el Admin puede editar las columnas de cálculo

            // Ocultamos las columnas de cálculo si NO es Admin
            dgvDetalles.Columns["PrecioCosto"].Visible = adminPuedeEditar;
            dgvDetalles.Columns["PorcentajeGanancia"].Visible = adminPuedeEditar;
            dgvDetalles.Columns["IVA"].Visible = adminPuedeEditar;

            // El Admin puede editar
            dgvDetalles.Columns["Cantidad"].ReadOnly = !adminPuedeEditar;// Solo Admin puede editar cantidad
            dgvDetalles.Columns["PrecioRepuesto"].ReadOnly = !adminPuedeEditar;// Solo Admin puede editar precio repuesto
            dgvDetalles.Columns["PrecioServicio"].ReadOnly = !adminPuedeEditar;// Solo Admin puede editar precio servicio

            // Las columnas de cálculo SIEMPRE son editables 
            dgvDetalles.Columns["PrecioCosto"].ReadOnly = false;// Siempre editable
            dgvDetalles.Columns["PorcentajeGanancia"].ReadOnly = false;// Siempre editable
            dgvDetalles.Columns["IVA"].ReadOnly = false;// Siempre editable
        }
        // Exporta el presupuesto seleccionado a un archivo PDF en formato ticket.
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            // Verificamos que haya un presupuesto seleccionado
            if (_presupuestoSeleccionadoId > 0)
            {
                // Llama al método central con el ID que está actualmente seleccionado en la grilla
                ExportarPresupuestoPDF(_presupuestoSeleccionadoId);
            }
            else
            {
                MessageBox.Show("Seleccione un presupuesto de la grilla para imprimir.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        // Método central para exportar un presupuesto a PDF en formato ticket.
        private void ExportarPresupuestoPDF(int idPresupuestoAExportar)
        {
            // Validación del ID de presupuesto
            int idPresupuesto = idPresupuestoAExportar;
            // Si el ID es inválido, mostramos un error y salimos.
            if (idPresupuesto <= 0)
            {
                MessageBox.Show("Error: ID de presupuesto no válido para exportar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Obtenemos los datos del presupuesto y sus detalles desde el servicio.
            Presupuesto presupuesto = _presupuestoService.GetPresupuestoById(idPresupuesto);
            if (presupuesto == null)
            {
                MessageBox.Show($"No se encontraron datos para el Presupuesto N° {idPresupuesto}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Obtenemos los detalles asociados al presupuesto.
            List<DetallePresupuesto> detalles = _presupuestoService.GetDetallesPresupuesto(idPresupuesto);


            // Configuración y exportación del PDF 
            var pageSize = new iTextSharp.text.Rectangle(300f, 10000f);
            var doc = new Document(pageSize, 10f, 10f, 10f, 10f);
            // Ruta de guardado del archivo PDF en la carpeta Descargas del usuario
            string rutaDescargas = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string rutaArchivo = Path.Combine(rutaDescargas, "Downloads", $"Presupuesto_Ticket_{idPresupuesto}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            // Crear el archivo PDF
            try
            {
                // Crear el escritor de PDF
                using (PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(rutaArchivo, FileMode.Create)))
                {
                    doc.Open();

                    // Tipografías
                    var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY);
                    var fontSubtitulo = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD);
                    var fontNormal = FontFactory.GetFont(FontFactory.HELVETICA, 8);
                    var fontExtra = FontFactory.GetFont(FontFactory.HELVETICA, 6);

                    // Título
                    var titulo = new Paragraph($"PRESUPUESTO", fontTitulo)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    doc.Add(titulo);
                    doc.Add(new Chunk(new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -1f)));

                    // Información del Encabezado 
                    doc.Add(new Paragraph($"Fecha: {presupuesto.Fecha:dd/MM/yyyy}", fontNormal));
                    doc.Add(new Paragraph($"Cliente: {presupuesto.ClienteNombre ?? "N/A"}", fontNormal));
                    // Vencimiento del Presupuesto
                    string fechaVencimientoStr = presupuesto.FechaVencimiento.HasValue
                        ? presupuesto.FechaVencimiento.Value.ToString("dd/MM/yyyy")
                        : "Sin vencimiento";
                    doc.Add(new Paragraph($"Vencimiento: {fechaVencimientoStr}", fontNormal));
                    doc.Add(new Chunk(new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -1f)));


                    // Tabla de Detalles del Presupuesto 
                    PdfPTable table = new PdfPTable(4)
                    {
                        WidthPercentage = 100 
                    };
                    table.SetWidths(new float[] { 3.5f, 1f, 1.5f, 1.5f });

                    // Encabezados de la tabla
                    BaseColor headerColor = new BaseColor(230, 230, 230);
                    table.AddCell(new PdfPCell(new Phrase("Descripción", fontSubtitulo)) { Border = 0, BorderWidthBottom = 1, PaddingBottom = 3, BackgroundColor = headerColor });
                    table.AddCell(new PdfPCell(new Phrase("Cant.", fontSubtitulo)) { Border = 0, BorderWidthBottom = 1, PaddingBottom = 3, BackgroundColor = headerColor });
                    table.AddCell(new PdfPCell(new Phrase("P. Unit.", fontSubtitulo)) { Border = 0, BorderWidthBottom = 1, PaddingBottom = 3, BackgroundColor = headerColor });
                    table.AddCell(new PdfPCell(new Phrase("Subtotal", fontSubtitulo)) { Border = 0, BorderWidthBottom = 1, PaddingBottom = 3, BackgroundColor = headerColor });


                    // Contenido de la tabla (Detalles)
                    decimal totalPresupuesto = 0m;
                    foreach (var detalle in detalles)
                    {
                        string descripcion = "N/A";

                        // Obtiene la descripción usando los métodos del servicio
                        if (detalle.IdArticulo.HasValue && detalle.IdArticulo.Value > 0)
                        {
                            descripcion = _presupuestoService.GetNombreArticulo(detalle.IdArticulo.Value);
                        }// Si es solo servicio
                        else if (detalle.IdServicio > 0)
                        {// Solo servicio
                            descripcion = _presupuestoService.GetDescripcionServicio(detalle.IdServicio);
                        }

                        // Manejo de valores nulos y cálculo del subtotal
                        decimal precioRepuesto = detalle.PrecioRepuesto ?? 0m;
                        decimal precioServicio = detalle.PrecioServicio ?? 0m;

                        // En un ticket, mostramos el precio total unitario (Repuesto + Servicio)
                        decimal precioUnitarioTotal = precioRepuesto + precioServicio;
                        decimal subtotal = precioUnitarioTotal * detalle.Cantidad;
                        totalPresupuesto += subtotal;
                        // Agregamos la fila a la tabla
                        table.AddCell(new Phrase(descripcion, fontNormal));
                        table.AddCell(new Phrase(detalle.Cantidad.ToString(), fontNormal));
                        // Mostramos el precio unitario TOTAL 
                        table.AddCell(new Phrase(precioUnitarioTotal.ToString("F2"), fontNormal));
                        table.AddCell(new Phrase(subtotal.ToString("F2"), fontNormal));
                    }
                    // Agregamos la tabla al documento
                    doc.Add(table);
                    // Línea separadora
                    doc.Add(new Chunk(new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -1f)));


                    // Total del Presupuesto
                    var totalParagraph = new Paragraph($"TOTAL: ${totalPresupuesto.ToString("F2")}", fontTitulo)
                    {
                        Alignment = Element.ALIGN_RIGHT// Alineación a la derecha
                    };
                    doc.Add(totalParagraph);// Agregamos el total al documento

                    doc.Add(new Paragraph("\n"));// Espacio antes de notas adicionales


                    doc.Add(new Paragraph("La aprobación de este presupuesto es crucial antes de la Fecha de Vencimiento. Transcurrido dicho plazo, los precios y la disponibilidad de los repuestos y servicios quedan sujetos a modificación sin previo aviso.", fontExtra));


                    doc.Add(new Paragraph("Atte. Casa de Reparaciones y Repuestos", fontExtra));
                    doc.Add(new Paragraph("\n"));
                    doc.Add(new Paragraph("Gracias por su consulta.", fontSubtitulo) { Alignment = Element.ALIGN_CENTER });
                    doc.Close();
                }

                MessageBox.Show($"Ticket de presupuesto N° {idPresupuesto} exportado a: {rutaArchivo}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar a PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Autorizar / Desautorizar presupuesto
        private void btnAutorizar_Click(object sender, EventArgs e)
        {
            // Validación básica
            if (_presupuestoSeleccionadoId <= 0) return;
            var pres = _todosPresupuestos.FirstOrDefault(p => p.IdPresupuesto == _presupuestoSeleccionadoId);// Obtener el presupuesto seleccionado
            if (pres == null) return;// Seguridad adicional

            if (pres.Autorizado?.ToUpper() == "SI")// Lógica para DESAUTORIZAR
            {
                var res = MessageBox.Show($"Desea DESAUTORIZAR y volver a estado 'CREADO' el presupuesto #{pres.IdPresupuesto}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.No) return;// Cancelar si no confirma

                pres.Autorizado = "NO";// Actualizamos el estado en memoria
                _presupuestoService.UpdateAutorizado(pres.IdPresupuesto, "NO");// Actualizamos en la BD

                MessageBox.Show($"Presupuesto {pres.IdPresupuesto} desautorizado. Volvió al flujo activo.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Lógica para AUTORIZAR y pasarlo a PENDIENTE
                var res = MessageBox.Show($"Desea AUTORIZAR el presupuesto #{pres.IdPresupuesto}? Su estado cambiará a PENDIENTE.", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.No) return;
                // Actualizamos el estado en memoria y en la BD
                _presupuestoService.MarcarAutorizadoYPendiente(pres.IdPresupuesto, "SI");
       


                MessageBox.Show($"Presupuesto {pres.IdPresupuesto} AUTORIZADO", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            CargarPresupuestos(); 
            LimpiarFormulario();
        }

        #endregion

        #region Handlers botones administrativos
        // Aprobar presupuesto (Admin)
        private void BtnAprobarAdmin_Click(object sender, EventArgs e)
        {
            // Validación básica
            var pres = _todosPresupuestos.FirstOrDefault(p => p.IdPresupuesto == _presupuestoSeleccionadoId);
            if (pres == null) // Si no se encuentra el presupuesto
            {
                MessageBox.Show("No se encontró el presupuesto seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Confirmación crítica: Avisamos que se modificarán precios maestros.
            if (MessageBox.Show("¿Está seguro de APROBAR este presupuesto? \n\nEsta acción guardará los precios de VENTA en este presupuesto y actualizará permanentemente cualquier COSTO MAESTRO que haya modificado.", "Confirmación de Aprobación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            try
            {
                int costosMaestrosActualizados = 0;// Contador de costos maestros actualizados
                int serviciosMaestrosActualizados = 0;// Contador de servicios maestros actualizados

                // Recorremos cada ítem del presupuesto para ver si el Admin cambió el costo manualmente.
                foreach (var detalle in _detallesPresupuesto)
                {
                    // ---  es un REPUESTO (tiene IdArticulo) ---
                    if (detalle.IdArticulo.HasValue && detalle.IdArticulo.Value > 0)
                    {
                        // Obtenemos el artículo original de la BD (cargado en memoria)
                        var articuloDB = _articulosDisponibles.FirstOrDefault(a => a.IdArticulo == detalle.IdArticulo.Value);
                        if (articuloDB == null) continue;

                        // Usamos ToStrings para evitar errores de precisión decimal
                        if (detalle.PrecioCosto.ToString("F2") != articuloDB.PrecioCosto.ToString("F2"))
                        {
                            // Buscamos quién es el proveedor que era el más caro
                            int idProveedorMasCaro = _articuloService.ObtenerIdProveedorMasCaro(detalle.IdArticulo.Value);

                            if (idProveedorMasCaro > 0)
                            {
                                // Actualizamos el precioCosto de ESE proveedor
                                _comprasService.ActualizarPrecioCostoProveedor(
                                    detalle.IdArticulo.Value, 
                                    idProveedorMasCaro, 
                                    detalle.PrecioCosto // El nuevo costo de la grilla
                                );
                                costosMaestrosActualizados++;
                            }
                        }
                    }
                    // --- Si es un SERVICIO (sin IdArticulo) ---
                    else if (detalle.IdServicio > 0)
                    {
                        var servicioDB = _serviciosDisponibles.FirstOrDefault(s => s.IdServicio == detalle.IdServicio);// Obtener el servicio de la lista en memoria
                        if (servicioDB == null) continue;// Seguridad adicional

                        // Si el precio de venta del servicio en la grilla es diferente al de la BD
                        if (detalle.PrecioServicio.GetValueOrDefault().ToString("F2") != servicioDB.Precio.ToString("F2"))
                        {
                            // ...actualizamos el precio del servicio
                            _presupuestoService.ActualizarPrecioServicio(detalle.IdServicio, detalle.PrecioServicio.GetValueOrDefault());
                            serviciosMaestrosActualizados++;// Incrementamos el contador
                        }
                    }
                }                

        // ---. GUARDAR EL PRESUPUESTO Y DETALLES  ---
        
        // Actualiza los precios de VENTA (PrecioRepuesto, PrecioServicio) en 'detalles_presupuestos'
        _presupuestoService.ActualizarDetallesPresupuesto(_detallesPresupuesto);

        // Actualiza el 'total' en la tabla 'presupuestos'
        _presupuestoService.ActualizarTotalPresupuesto(pres.IdPresupuesto, _totalPresupuesto);

        // Cambia el estado a 'APROBADO_ADMIN'
        pres.Estado = "APROBADO_ADMIN";
        _presupuestoService.UpdateEstado(pres.IdPresupuesto, pres.Estado);//
                                                                          // Refrescamos la interfaz y limpiamos


        CargarPresupuestos();// Recargar la lista de presupuestos 
                LimpiarFormulario();  // Limpiar el formulario actual

                MessageBox.Show($"Presupuesto aprobado y guardado con éxito.\n" +
                        $"- {costosMaestrosActualizados} costos maestros de artículos actualizados.\n" +
                        $"- {serviciosMaestrosActualizados} servicios maestros actualizados.", 
                        "Aprobación Completa", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error CRÍTICO al guardar el presupuesto en la base de datos: {ex.Message}", "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

        // Enviar presupuesto al cliente
        private void BtnEnviarCliente_Click(object sender, EventArgs e)
        {
            // Validación básica
            if (_presupuestoSeleccionadoId <= 0)
            {
                MessageBox.Show("Seleccione un presupuesto.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Confirmación de envío
            var askEnvio = MessageBox.Show(
                "Confirmar envío al cliente (esto fijará la fecha de envío y el vencimiento a 7 días).",
                "Confirmar Envío",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (askEnvio == DialogResult.No) return;

            try
            {
                _presupuestoService.MarcarEnviadoACliente(_presupuestoSeleccionadoId);// Marca como enviado y fija fechas
                CargarPresupuestos();// Refresca la lista de presupuestos

                // Mensaje de Éxito y Pregunta de Impresión
                var askImprimir = MessageBox.Show(
                    "✅ Presupuesto enviado al cliente. Vencimiento seteado a 7 días.\n\n¿Desea imprimir el presupuesto ahora?",
                    "Envío Exitoso",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (askImprimir == DialogResult.Yes)
                {
                    ExportarPresupuestoPDF(_presupuestoSeleccionadoId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al enviar el presupuesto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Helpers
        private void ActualizarTotal()
        {
            decimal total = 0m;

            foreach (DataGridViewRow fila in dgvDetalles.Rows)
            {
                if (fila.Cells["Subtotal"].Value != null &&
                    decimal.TryParse(fila.Cells["Subtotal"].Value.ToString(), System.Globalization.NumberStyles.Currency, null, out decimal sub))
                {
                    total += sub;
                }
            }

            _totalPresupuesto = total;

            lblTotal.Text = $"Total: {total:C2}";
            txtTotal.Text = total.ToString("C2");
        }

        private void LimpiarFormulario()
        {
            txtIdIngreso.Clear();
            txtMarcaIngreso.Clear();
            txtModelo.Clear();
            txtTipoDispositivo.Clear();
            txtFalla.Clear();
            txtFecha.Clear();

            _detallesPresupuesto.Clear();
            dgvDetalles.Rows.Clear();
            _totalPresupuesto = 0m;
            ActualizarTotal();

            _editandoPresupuestoExistente = false;
            _presupuestoSeleccionadoId = 0;
        }

        #endregion
        // Evento al cambiar valor en la grilla de detalles
        private void dgvDetalles_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Solo el Admin puede disparar este evento
            if (string.Equals(_userRole, "TECNICO", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // Evitar errores al cargar la grilla
            if (e.RowIndex < 0 || e.RowIndex >= dgvDetalles.Rows.Count) return;

            try
            {
                var row = dgvDetalles.Rows[e.RowIndex];// Fila editada
                // Asegurarse de no fallar si la fila es nueva
                if (e.RowIndex >= _detallesPresupuesto.Count) return;

                var detalle = _detallesPresupuesto[e.RowIndex];// Objeto detalle correspondiente
                string colName = dgvDetalles.Columns[e.ColumnIndex].Name;// Nombre de la columna editada

                CultureInfo culture = CultureInfo.CurrentCulture;// Cultura actual para parseo numérico

                // ---  CÁLCULO AUTOMÁTICO ---
                if (colName == "PrecioCosto" || colName == "PorcentajeGanancia" || colName == "IVA")
                {
                    decimal.TryParse(row.Cells["PrecioCosto"].Value?.ToString() ?? "0", NumberStyles.Any, culture, out decimal costo);// Nuevo costo
                    decimal.TryParse(row.Cells["PorcentajeGanancia"].Value?.ToString() ?? "0", NumberStyles.Any, culture, out decimal ganancia);// Nueva ganancia
                    decimal.TryParse(row.Cells["IVA"].Value?.ToString() ?? "0", NumberStyles.Any, culture, out decimal iva);// Nuevo IVA

                    // Recalcular el Precio de VENTA (PrecioRepuesto)
                    decimal precioConGanancia = costo * (1 + (ganancia / 100m));
                    decimal precioFinalRepuesto = precioConGanancia * (1 + (iva / 100m));// Precio final con IVA

                    // Actualizar la celda de PrecioRepuesto
                    row.Cells["PrecioRepuesto"].Value = precioFinalRepuesto;

                    //  Actualizar el objeto en memoria
                    detalle.PrecioCosto = costo;
                    detalle.PorcentajeGanancia = ganancia;
                    detalle.IVA = iva;
                    detalle.PrecioRepuesto = precioFinalRepuesto;
                }

                // --- Obtener valores editados ---
                decimal.TryParse(row.Cells["PrecioRepuesto"].Value?.ToString() ?? "0", NumberStyles.Any, culture, out decimal precioRep);
                decimal.TryParse(row.Cells["PrecioServicio"].Value?.ToString() ?? "0", NumberStyles.Any, culture, out decimal precioSer);
                int.TryParse(row.Cells["Cantidad"].Value?.ToString() ?? "0", out int cantidad);

                // Actualizamos el objeto en memoria
                if (e.RowIndex < _detallesPresupuesto.Count)
                {
                    _detallesPresupuesto[e.RowIndex].PrecioRepuesto = precioRep;
                    _detallesPresupuesto[e.RowIndex].PrecioServicio = precioSer;
                    _detallesPresupuesto[e.RowIndex].Cantidad = cantidad;
                }

                // Recalculamos el Subtotal
                decimal subtotal = (precioRep + precioSer) * cantidad;
                row.Cells["Subtotal"].Value = subtotal.ToString("N2"); 

                // Actualizamos el Total general
                ActualizarTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error actualizando precios en grilla: {ex.Message}");
            }
        }
        // Maneja el evento de clic en una celda del DataGridView de detalles (por ejemplo, para eliminar un detalle).
        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)// Asegurarse de que no sea el encabezado
            {
                // Verificar si se hizo clic en la columna "Eliminar"
                if (dgvDetalles.Columns[e.ColumnIndex].Name == "Eliminar")
                {
                    var resultado = MessageBox.Show(
                        "¿Está seguro de eliminar este detalle del presupuesto?",
                        "Confirmar Eliminación",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (resultado == DialogResult.Yes)
                    {
                        int rowIndex = e.RowIndex;

                        if (rowIndex < _detallesPresupuesto.Count)// Seguridad adicional
                        {
                            _detallesPresupuesto.RemoveAt(rowIndex);// Eliminar del listado en memoria

                            RecargarDetallesGrid();

                        }
                    }
                }
            }
        }
    }
}
