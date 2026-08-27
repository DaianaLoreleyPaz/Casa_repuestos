using CasaRepuestos.Models;
using CasaRepuestos.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.Data;
using System.Diagnostics; 


namespace CasaRepuestos.Forms
{
    public partial class FrmFactura : Form
    {
        // variables globales service
        private readonly FacturaService _facturaService = new FacturaService();
        private readonly ArticuloService _articuloService = new ArticuloService();
        private readonly ServiciosService _serviciosService = new ServiciosService();
        private readonly PresupuestoService _presupuestoService = new PresupuestoService();
        private readonly ClienteService _clienteService = new ClienteService();
        private readonly CuentaCorrienteService _cuentaCorrienteService = new CuentaCorrienteService();
        private readonly ArqueoService _arqueoService = new ArqueoService();


        // variables globales listas
        private List<Presupuesto> listaReparacionesFinalizadas = new List<Presupuesto>();
        private List<ReparacionesFinalizadas> reparaciones = new List<ReparacionesFinalizadas>();
        private List<DetallePresupuesto> detallesReparacion = new List<DetallePresupuesto>();
        private List<Articulo> listaArticulosActivos = new List<Articulo>();
        private List<Servicio> listaServicios = new List<Servicio>();
        private List<DetalleFactura> detallesFactura = new List<DetalleFactura>();
        private List<Cliente> listaClientes = new List<Cliente>();

        // variables globales 
        private decimal totalFactura = 0;
        private int clienteSeleccionadoId = 0;
        private int? idPresupuestoSeleccionado = null;
        private Cliente? clienteActual;


        public FrmFactura()
        {
            InitializeComponent();
            // Cargo el comboBox de métodos de pago
            ConfigurarComboBoxMetPago();
            // Cargo las reparaciones finalizadas en el DataGridViewReparacionesClientes
            CargarReparacionesFinalizadas();
            //Cargar artículos en el comboBoxArticulo
            CargarArticulos();
            // Cargar servicios
            CargarServicios();
            // Refrescar detalles
            RefrescarDetalles();
            // Cargar  en comboboxClientes
            CargarClientesCombo();
            // Aplicar estilos
            AplicarEstilos();
            // Estado inicial de la interfaz
            lblModoOperacion.Text = "📋 Modo: Selección libre";
            lblModoOperacion.ForeColor = System.Drawing.Color.Black;
            AplicarEfectoDeshabilitado(dataGridViewReparacionesCliente, true);
            dataGridViewDetallesVenta.DataSource = null;
            dataGridViewDetallesVenta.Columns.Clear();
            comboBoxClientes.SelectedIndex = -1;
            comboBoxArticulo.SelectedIndex = -1;
            comboBoxMetPago.SelectedIndex = -1;

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
                btn.FlatAppearance.BorderSize = 2;
                btn.BackColor = Color.FromArgb(76, 132, 200);
                btn.ForeColor = Color.White;
            }

            // Estilos para botones específicos (Guardar y Agregar)
            try { btnGuardarVenta.BackColor = Color.FromArgb(34, 139, 34); btnGuardarVenta.ForeColor = Color.White; } catch { }
            try { btnAddArticulo.BackColor = Color.FromArgb(12, 87, 150); } catch { }
            try { buttonAgregarNvoCliente.BackColor = Color.FromArgb(12, 87, 150); } catch { }


            // Estilos para DataGridViews
            foreach (var dgv in new[] { dataGridViewReparacionesCliente, dataGridViewDetallesReparacion, dataGridViewDetallesVenta })
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
        // Estilos para controles deshabilitados
        private void AplicarEfectoDeshabilitado(Control control, bool habilitar)
        {
            if (habilitar)
            {
                control.Enabled = true;
                control.BackColor = SystemColors.Window;
                control.ForeColor = SystemColors.ControlText;
                control.Cursor = Cursors.Default;
                if (control is ComboBox combo)
                    combo.FlatStyle = FlatStyle.Standard;
            }
            else
            {
                control.Enabled = false;
                control.BackColor = Color.FromArgb(235, 235, 235);
                control.ForeColor = Color.Gray;
                control.Cursor = Cursors.No;
                if (control is ComboBox combo)
                    combo.FlatStyle = FlatStyle.Flat;
            }
        }

        // Cargar clientes para el ComboBoxClientes
        private void CargarClientesCombo()
        {
            listaClientes = _clienteService.ListarClientes() ?? new List<Cliente>();

            comboBoxClientes.DataSource = null;
            comboBoxClientes.DisplayMember = "NombreCompleto";
            comboBoxClientes.ValueMember = "IdCliente";
            comboBoxClientes.DataSource = listaClientes;

            comboBoxClientes.DropDownStyle = ComboBoxStyle.DropDown;
            comboBoxClientes.AutoCompleteMode = AutoCompleteMode.None;
        }
        // Filtrar clientes en el ComboBoxClientes mientras se escribe
        private void comboBoxClientes_TextUpdate(object sender, EventArgs e)
        {
            string texto = comboBoxClientes.Text.ToLower();

            // filtra los clientes cuyo nombre o CUIT/DNI contenga lo escrito
            var filtrados = listaClientes
                .Where(c =>
                    (!string.IsNullOrEmpty(c.NombreCompleto) && c.NombreCompleto.ToLower().Contains(texto)) ||
                    (!string.IsNullOrEmpty(c.Cuil) && c.Cuil.ToLower().Contains(texto)) ||
                    (c.DatosPersona?.NumeroDocumento ?? "").Contains(texto)
                )
                .ToList();

            if (filtrados.Count == 0)
            {
                comboBoxClientes.DroppedDown = false;
                return;
            }

            // guardar texto actual
            string textoActual = comboBoxClientes.Text;
            int pos = comboBoxClientes.SelectionStart;

            // suspender actualizaciones para evitar parpadeo
            comboBoxClientes.BeginUpdate();

            comboBoxClientes.DataSource = null;
            comboBoxClientes.DisplayMember = "NombreCompleto";
            comboBoxClientes.ValueMember = "IdCliente";
            comboBoxClientes.DataSource = filtrados;

            comboBoxClientes.DroppedDown = true; // abrir lista con resultados
            comboBoxClientes.EndUpdate();

            // restaurar el texto escrito y el cursor
            comboBoxClientes.Text = textoActual;
            comboBoxClientes.SelectionStart = pos;
            comboBoxClientes.SelectionLength = 0;
        }

        // ===============================
        // SELECCIÓN DE CLIENTE 
        // ===============================
        private void comboBoxClientes_SelectedIndexChanged(object sender, EventArgs e)
        {

            // Si había una reparación seleccionada, limpiar los detalles
            if (idPresupuestoSeleccionado.HasValue)
            {
                idPresupuestoSeleccionado = null;
                detallesReparacion.Clear();
                dataGridViewDetallesReparacion.DataSource = null;
                dataGridViewDetallesReparacion.Columns.Clear();
                labelTotal.Text = "TOTAL: $0,00";
            }
            if (comboBoxClientes.SelectedItem is not Cliente cliente)
            {
                // Si no hay cliente seleccionado, se resetea todo
                clienteSeleccionadoId = 0;
                clienteActual = null;
                idPresupuestoSeleccionado = null;
                detallesReparacion.Clear();
                detallesFactura.Clear();
                AplicarEfectoDeshabilitado(dataGridViewReparacionesCliente, true);
                dataGridViewReparacionesCliente.DataSource = null;
                dataGridViewDetallesReparacion.DataSource = null;
                lblModoOperacion.Text = "📋 Modo: Selección libre";
                lblModoOperacion.ForeColor = Color.Black;
                return;
            }

            clienteSeleccionadoId = cliente.IdCliente;
            clienteActual = cliente;
            lblModoOperacion.Text = $"🔎 Buscando reparaciones de {cliente.NombreCompleto}...";
            lblModoOperacion.ForeColor = Color.DimGray;

            // Obtener reparaciones finalizadas de ese cliente
            var reparacionesCliente = _facturaService.ListarPresupuestosFinalizados()
                .Where(p =>
                {
                    var c = _facturaService.ObtenerClientePorIDIngreso(p.IdIngreso);
                    return c != null && c.IdCliente == clienteSeleccionadoId;
                })
                .Select(p => new ReparacionesFinalizadas
                {
                    IdPresupuesto = p.IdPresupuesto,
                    Fecha = (DateTime)p.Fecha,
                    Total = p.Total,
                    ClienteNombre = cliente.NombreCompleto,
                    IdIngreso = p.IdIngreso
                })
                .ToList();

            if (reparacionesCliente.Any())
            {
                // Si hay reparaciones, mostrar en el DataGridView
                dataGridViewReparacionesCliente.DataSource = reparacionesCliente;
                if (dataGridViewReparacionesCliente.Columns["IdIngreso"] != null)
                    dataGridViewReparacionesCliente.Columns["IdIngreso"].Visible = false;
                if (dataGridViewReparacionesCliente.Columns["IdPresupuesto"] != null)
                    dataGridViewReparacionesCliente.Columns["IdPresupuesto"].Visible = false;

                AplicarEfectoDeshabilitado(dataGridViewReparacionesCliente, true);
                lblModoOperacion.Text = $"🧰 Modo: Reparaciones de {cliente.NombreCompleto}";
                lblModoOperacion.ForeColor = Color.DarkOrange;
            }
            else
            {
                // Si NO tiene reparaciones, es venta directa
                dataGridViewReparacionesCliente.DataSource = null;
                AplicarEfectoDeshabilitado(dataGridViewReparacionesCliente, false);
                lblModoOperacion.Text = $"🛒 Modo: Venta directa a {cliente.NombreCompleto}";
                lblModoOperacion.ForeColor = Color.MediumSeaGreen;
            }
        }

        // ===============================
        // BOTÓN LIMPIAR SELECCIÓN
        // ===============================
        private void btnLimpiarSeleccion_Click(object sender, EventArgs e)
        {
            clienteSeleccionadoId = 0;
            idPresupuestoSeleccionado = null;
            totalFactura = 0;
            detallesFactura.Clear();
            detallesReparacion.Clear();
            comboBoxClientes.SelectedIndex = -1;
            comboBoxClientes.Text = "";
            // Reactiva los controles de selección
            AplicarEfectoDeshabilitado(dataGridViewReparacionesCliente, true);
            AplicarEfectoDeshabilitado(buttonAgregarNvoCliente, true);
            AplicarEfectoDeshabilitado(comboBoxClientes, true);

            dataGridViewReparacionesCliente.ClearSelection();
            // Limpia y restablece los DataGridViews de detalles
            dataGridViewDetallesReparacion.DataSource = null;
            dataGridViewDetallesVenta.DataSource = null;
            dataGridViewDetallesVenta.Columns.Clear();
            // Limpieza de artículos 
            comboBoxArticulo.SelectedIndex = -1;
            textBoxCantidad.Clear();
            // Limpieza del método de pago
            comboBoxMetPago.SelectedIndex = -1;
            textBoxDescripcionMetPago.Clear();
            // Actualiza el total
            labelTotal.Text = $"TOTAL: {totalFactura:C2}";
            lblModoOperacion.Text = "📋 Modo: Selección libre";
            lblModoOperacion.ForeColor = System.Drawing.Color.Black;
        }



        // Cargar los servicios para mostrarlos en los detalles de la reparación seleccionada
        private void CargarServicios()
        {
            listaServicios = _serviciosService.ObtenerTodosServicios();
        }


        /**************************************************
         * SELECCIONO UNA REPARACION PARA VER SUS DETALLES 
         *************************************************/
        private void DataGridViewReparacionesCliente_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Validación inicial
            if (e.RowIndex < 0 || dataGridViewReparacionesCliente.Rows.Count == 0)
                return;

            var row = dataGridViewReparacionesCliente.Rows[e.RowIndex];
            if (row == null || row.Cells["IdPresupuesto"]?.Value == null)
                return;

            // Guardar el ID del presupuesto seleccionado
            int idPresupuesto = Convert.ToInt32(row.Cells["IdPresupuesto"].Value);
            idPresupuestoSeleccionado = idPresupuesto;

            // Obtener el presupuesto completo
            Presupuesto presupuestoSeleccionado = _presupuestoService.GetPresupuestoById(idPresupuesto);
            if (presupuestoSeleccionado == null)
            {
                MessageBox.Show("No se pudo obtener la información del presupuesto seleccionado",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  Obtener el cliente del ingreso vinculado
            clienteActual = _facturaService.ObtenerClientePorIdIngreso(presupuestoSeleccionado.IdIngreso);
            clienteSeleccionadoId = clienteActual?.IdCliente ?? 0;

            //  Verificar vigencia (30 días)
            //  si la reparacion esta finalizada hace mas de 30 dias vuelve a 'verificar precio' en presupuesto
            TimeSpan diferencia = DateTime.Now - (presupuestoSeleccionado.Fecha ?? DateTime.Now);
            if (diferencia.TotalDays >= 30)
            {
                _facturaService.CambiarEstadoYAutorizacion(presupuestoSeleccionado.IdPresupuesto);
                MessageBox.Show(
                    $"El Presupuesto N° {idPresupuesto} ha superado los 30 días de vigencia y requiere actualización de precios",
                    "Presupuesto vencido", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                CargarReparacionesFinalizadas();
                idPresupuestoSeleccionado = null;
                return;
            }

            // Cargar los detalles de la reparación (repuestos y servicios)
            detallesReparacion = _facturaService.ObtenerDetallesReparacion(idPresupuesto);

            if (detallesReparacion == null || detallesReparacion.Count == 0)
            {
                MessageBox.Show("El presupuesto seleccionado no tiene detalles de reparación asociados",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridViewDetallesReparacion.DataSource = null;
                CalcularTotal();
                return;
            }

            // Construir tabla de detalles para mostrar en el DataGridView de detalles de reparación
            var detallesParaMostrar = detallesReparacion.Select(d =>
            {
                var articulo = d.IdArticulo.HasValue
                    ? listaArticulosActivos.FirstOrDefault(a => a.IdArticulo == d.IdArticulo.Value)
                    : null;

                var servicio = d.IdServicio > 0
                    ? listaServicios.FirstOrDefault(s => s.IdServicio == d.IdServicio)
                    : null;

                decimal subtotal = ((d.PrecioRepuesto ?? 0) + (d.PrecioServicio ?? 0)) * d.Cantidad;

                return new
                {
                    Servicio = servicio?.Descripcion ?? "-",
                    Articulo = articulo?.Nombre ?? "-",
                    PrecioRepuesto = d.PrecioRepuesto ?? 0,
                    PrecioServicio = d.PrecioServicio ?? 0,
                    d.Cantidad,
                    Subtotal = subtotal
                };
            }).ToList();

            // Mostrar detalles y recalcular total
            dataGridViewDetallesReparacion.DataSource = detallesParaMostrar;
            CalcularTotal();

            // Habilitar siempre la carga de artículos 
            AplicarEfectoDeshabilitado(comboBoxArticulo, true);
            AplicarEfectoDeshabilitado(textBoxCantidad, true);
            AplicarEfectoDeshabilitado(btnAddArticulo, true);
        }

        /*****************************
         * MÉTODO DE PAGO 
         ****************************/
        private void ConfigurarComboBoxMetPago()
        {
            comboBoxMetPago.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxMetPago.Items.Clear();
            comboBoxMetPago.Items.AddRange(new object[]
            {
                "EFECTIVO",
                "TARJETA",
                "TRANSFERENCIA",
                "BILLETERA VIRTUAL",
                "CUENTA CORRIENTE"
            });
            if (comboBoxMetPago.Items.Count > 0) comboBoxMetPago.SelectedIndex = 0;

            comboBoxArticulo.DropDownStyle = ComboBoxStyle.DropDown;
            comboBoxArticulo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxArticulo.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        /**************************************************
         * LISTA DE REPARACIONES 
         *************************************************/
        private void CargarReparacionesFinalizadas()
        {
            listaReparacionesFinalizadas = _facturaService.ListarPresupuestosFinalizados();

            // Por cada reparacion, obtengo el nombre del cliente y creo una nueva lista de ReparacionesFinalizadas
            reparaciones = listaReparacionesFinalizadas.Select(p =>
            {
                // obtengo el idcliente a partir del idingreso
                var cliente = _facturaService.ObtenerClientePorIDIngreso(p.IdIngreso);
                var nombreCliente = cliente?.NombreCompleto ?? "Sin cliente";

                return new ReparacionesFinalizadas
                {
                    IdPresupuesto = p.IdPresupuesto,
                    Fecha = (DateTime)p.Fecha,
                    Total = p.Total,
                    ClienteNombre = nombreCliente,
                    IdIngreso = p.IdIngreso
                };
            }).ToList();
            // Asignar los datos al dataGridViewReparacionesCliente
            dataGridViewReparacionesCliente.DataSource = reparaciones;

            // Ocultar la columna ID después de asignar el DataSource
            if (dataGridViewReparacionesCliente.Columns.Contains("IdPresupuesto"))
            {
                dataGridViewReparacionesCliente.Columns["IdPresupuesto"].Visible = false;
            }

            FiltrarReparaciones(comboBoxClientes.Text);
        }
        // Metodo para filtrar las reparaciones por el nombre del cliente
        private void FiltrarReparaciones(string filtro)
        {
            var list = reparaciones
                .Where(x => string.IsNullOrWhiteSpace(filtro) ||
                            x.ClienteNombre.IndexOf(filtro.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(x => new
                {
                    x.IdPresupuesto,
                    Fecha = x.Fecha.ToString("yyyy-MM-dd"),
                    x.Total,
                    Cliente = x.ClienteNombre,
                    x.IdIngreso
                })
                .ToList();
            // muestra la lista que coinciden con el filtro
            dataGridViewReparacionesCliente.DataSource = list;
            // que no se muestre el idingreso
            if (dataGridViewReparacionesCliente.Columns["IdIngreso"] != null)
                dataGridViewReparacionesCliente.Columns["IdIngreso"].Visible = false;
        }

        private void textBoxCliente_TextChanged(object sender, EventArgs e)
        {
            FiltrarReparaciones(comboBoxClientes.Text);
        }

        // Clase privada para crear un objeto que muestre las reparaciones + el nombre del cliente
        private class ReparacionesFinalizadas
        {
            public int IdPresupuesto { get; set; }
            public DateTime Fecha { get; set; }
            public decimal Total { get; set; }
            public string ClienteNombre { get; set; }
            public int IdIngreso { get; set; }

        } //-----------------------------------

        /**************************************************
         * LISTA DE ARTICULOS 
         *************************************************/
        private void CargarArticulos()
        {

            listaArticulosActivos = _articuloService.ListarArticulosActivos();

            comboBoxArticulo.DataSource = listaArticulosActivos;
            comboBoxArticulo.DisplayMember = "Nombre";
            comboBoxArticulo.ValueMember = "IdArticulo";

            // Para buscar artículos por nombre
            var autoComplete = new AutoCompleteStringCollection();
            autoComplete.AddRange(listaArticulosActivos.Select(a => a.Nombre).ToArray());
            comboBoxArticulo.AutoCompleteCustomSource = autoComplete;
        }



        // -----AGREGAR ARTICULO AL COMBOBOX DE DETALLES DE LA FACTURA------
        private void btnAddArticulo_Click(object sender, EventArgs e)
        {
            // Validar selección de artículo
            if (comboBoxArticulo.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un artículo válido", "Artículo no válido", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar cantidad ingresada
            if (!int.TryParse(textBoxCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Ingrese una cantidad válida", "Cantidad requerida", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var articuloSeleccionado = (Articulo)comboBoxArticulo.SelectedItem;

            // Obtener el stock disponible actual para este artículo
            int stockDisponible = articuloSeleccionado.Stock;

            // Controlar si hay stock suficiente
            if (cantidad > stockDisponible)
            {
                MessageBox.Show($"No hay stock suficiente. Stock disponible: {stockDisponible}", "Stock Insuficiente", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Si el stock es suficiente:
            // Crear el detalle y agregarlo a la lista de detalles de la factura
            var detalle = new DetalleFactura
            {
                ID_Articulo = articuloSeleccionado.IdArticulo,
                Cantidad = cantidad,
                subtotal = (decimal)articuloSeleccionado.Precio * cantidad,
                PrecioUnitario = (decimal)articuloSeleccionado.Precio,
                NombreArticulo = articuloSeleccionado.Nombre
            };

            detallesFactura.Add(detalle);

            // ACTUALIZAR / DESCONTAR EL STOCK DEL OBJETO Articulo
            articuloSeleccionado.Stock -= cantidad;

            RefrescarDetalles();
            textBoxCantidad.Clear();
            comboBoxArticulo.SelectedIndex = -1;

        }

        // Refrescar el DataGridView de detalles de la factura      
        private void RefrescarDetalles()
        {

            dataGridViewDetallesVenta.DataSource = null;
            dataGridViewDetallesVenta.Columns.Clear();

            dataGridViewDetallesVenta.DataSource = detallesFactura.Select(d => new
            {
                d.NombreArticulo,
                d.Cantidad,
                d.PrecioUnitario,
                d.subtotal
            }).ToList();

            dataGridViewDetallesVenta.Columns["PrecioUnitario"].DefaultCellStyle.Format = "C2";
            dataGridViewDetallesVenta.Columns["subtotal"].DefaultCellStyle.Format = "C2";

            // Botón eliminar
            DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn
            {
                HeaderText = "Eliminar",
                Text = "X",
                Name = "Eliminar",
                UseColumnTextForButtonValue = true,
                Width = 60,
                FlatStyle = FlatStyle.Flat
            };
            dataGridViewDetallesVenta.Columns.Add(btnEliminar);

            CalcularTotal();
        }


        // Evento para manejar el clic en el botón eliminar detalle, y actualizar el stock en memoria
        private void DataGridViewDetallesVenta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar si el clic fue en la columna "Eliminar" y una fila válida
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridViewDetallesVenta.Columns["Eliminar"].Index)
            {
                // Obtener el objeto DetalleFactura que se va a eliminar antes de removerlo de la lista
                var detalleAEliminar = detallesFactura[e.RowIndex];

                // Encontrar el objeto Articulo correspondiente al detalle seleccionado
                var articuloOriginal = listaArticulosActivos.FirstOrDefault(a => a.IdArticulo == detalleAEliminar.ID_Articulo);

                if (articuloOriginal != null)
                {
                    // Devolver la cantidad inicial al stock del artículo en memoria
                    articuloOriginal.Stock += detalleAEliminar.Cantidad;
                }

                // Eliminar el detalle de la lista
                detallesFactura.RemoveAt(e.RowIndex);

                // Refrescar el DataGridView y recalcular totales
                RefrescarDetalles();
            }
        }

        private void CalcularTotal()
        {
            // Calcular el total a partir de los detalles de la reparación
            decimal totalReparacionRecalculado = detallesReparacion.Sum(d =>
            {
                // Calcular el subtotal por detalle
                decimal precioUnitarioTotal = (d.PrecioRepuesto ?? 0m) + (d.PrecioServicio ?? 0m);
                return precioUnitarioTotal * d.Cantidad;
            });

            // Sumar el total de los artículos de venta adicionales, si hay
            decimal totalArticulosExtra = detallesFactura.Sum(d => d.subtotal);

            // Calcular el total final de la factura
            totalFactura = totalReparacionRecalculado + totalArticulosExtra;

            // Actualizar el label
            labelTotal.Text = $"TOTAL: {totalFactura:C2}";
        }

        /**************************************************
        * GUARDAR FACTURA (VENTA)
        * ******************************************/


        private void btnGuardarVenta_Click(object sender, EventArgs e)
        {
            // Verificar si la caja está cerrada
            if (_arqueoService.EsCajaDelDiaCerrada())
            {
                MessageBox.Show(
                    "No se pueden realizar ventas.\n\nLa caja del día ya ha sido cerrada.\nPara realizar transacciones, debe reabrir la caja desde el módulo de Arqueo.",
                    "Caja Cerrada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Verificar si no hay caja abierta
            if (!_arqueoService.ExisteCajaAbiertaHoy())
            {
                MessageBox.Show(
                    "No se pueden realizar ventas.\n\nNo hay una caja abierta para el día de hoy.\nPor favor, abra la caja desde el módulo de Arqueo antes de realizar transacciones.",
                    "Caja No Abierta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // --- VALIDACIONES INICIALES ---
            // Método de pago seleccionado
            if (comboBoxMetPago.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un método de pago", "Método de pago requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Al menos un artículo en la venta o una reparación seleccionada
            if (!idPresupuestoSeleccionado.HasValue && (detallesFactura == null || !detallesFactura.Any()))
            {
                MessageBox.Show("Debe agregar al menos un artículo a la venta", "Artículo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            string metodoPago = comboBoxMetPago.SelectedItem.ToString();
            Cliente? cliente = null;
            int? idPresupuesto = null;

            // Obtener el cliente según el escenario (presupuesto o cliente seleccionado)
            if (idPresupuestoSeleccionado.HasValue)
            {
                idPresupuesto = idPresupuestoSeleccionado.Value;
                var presupuesto = _presupuestoService.GetPresupuestoById(idPresupuesto.Value);
                if (presupuesto == null)
                {
                    MessageBox.Show("No se encontró el presupuesto seleccionado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                cliente = _facturaService.ObtenerClientePorIdIngreso(presupuesto.IdIngreso);
            }
            else if (clienteSeleccionadoId > 0)
            {
                cliente = listaClientes.FirstOrDefault(c => c.IdCliente == clienteSeleccionadoId);
            }
            // Variable para el ID de cuenta corriente si aplica
            int? idCuentaCorriente = null;

            // Validar que haya un cliente válido o asignar 'consumidor final'
            if (idPresupuesto == null && cliente == null)
            {
                if (metodoPago.Equals("CUENTA CORRIENTE", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Debe seleccionar un cliente válido para realizar una venta con método de pago 'CUENTA CORRIENTE'",
                        "Cliente requerido", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning);
                    return;
                }

                cliente = new Cliente
                {
                    IdCliente = 0,
                    Categoria = "CONSUMIDOR FINAL",
                    Cuil = "",
                    DatosPersona = new Persona
                    {
                        Nombre = "CONSUMIDOR",
                        Apellido = "FINAL"
                    }
                };
            }
            // Si método de pago es CUENTA CORRIENTE pero no hay cliente válido
            if (metodoPago == "CUENTA CORRIENTE" && cliente == null)
            {
                MessageBox.Show("Debe seleccionar un cliente válido para usar cuenta corriente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal totalArticulos = detallesFactura.Sum(d => d.subtotal);
            decimal totalServicios = detallesReparacion.Sum(d => (d.PrecioServicio ?? 0) * d.Cantidad);
            decimal totalRepuestos = detallesReparacion.Sum(d => (d.PrecioRepuesto ?? 0) * d.Cantidad);
            this.totalFactura = totalArticulos + totalRepuestos + totalServicios;

            // Si NO hay presupuesto pero si cliente y el método es cuenta corriente
            if (metodoPago == "CUENTA CORRIENTE" && cliente != null && cliente.IdCliente > 0)
            {
                // Obtener id de la cuenta corriente o crear cuenta corriente si no existe
                idCuentaCorriente = _facturaService.ObtenerCuentaCorriente(cliente.IdCliente);

                // Si no se encontró o es inválida, se crea una nueva cuenta corriente
                if (!idCuentaCorriente.HasValue || idCuentaCorriente.Value <= 0)
                {
                    idCuentaCorriente = _facturaService.CrearCuentaCorriente(cliente.IdCliente, this.totalFactura);
                }
                else
                {
                    // Obtener el saldo actual desde la base
                    decimal saldoActual = _cuentaCorrienteService.ObtenerSaldoCuentaCorriente(idCuentaCorriente.Value);

                    // Sumar el total de la factura al saldo existente
                    decimal nuevoSaldo = saldoActual + this.totalFactura;

                    // Actualizar en la base
                    _cuentaCorrienteService.ActualizarSaldoCuentaCorriente(idCuentaCorriente.Value, nuevoSaldo);
                }
            }


            // --- DETERMINAR TIPO DE FACTURA SEGÚN CONDICIÓN DEL CLIENTE ---
            string tipoFactura;
            switch (cliente.Categoria.ToUpper())
            {
                case "RESPONSABLE INSCRIPTO":
                case "MONOTRIBUTISTA":
                    tipoFactura = "A";
                    break;
                case "EXENTO":
                case "CONSUMIDOR FINAL":
                default:
                    tipoFactura = "B";
                    break;
            }

            var factura = new Facturas
            {
                ID_Presupuesto = idPresupuesto ?? 0,
                Metodo_Pago = metodoPago,
                Descripcion_Metodo_Pago = textBoxDescripcionMetPago.Text ?? "SIN ESPECIFICAR",
                Fecha = DateTime.Now,
                Total_Factura = this.totalFactura,
                Tipo_Factura = tipoFactura,
                Categoria_Cliente = cliente?.Categoria, // Agregar la categoría del cliente
                ID_Cuenta_Corriente = idCuentaCorriente.HasValue ? idCuentaCorriente.Value : null,
                estado = idCuentaCorriente.HasValue ? "PENDIENTE" : "PAGADA"

            };

            try
            {
                // Crear la factura y obtener su ID
                int idFactura = _facturaService.CrearFactura(factura, detallesFactura);
                if (idFactura <= 0)
                    throw new Exception("Error! No existe un ID de factura válido");


                // Cambiar estado del presupuesto si hay uno
                if (idPresupuestoSeleccionado.HasValue)
                {
                    _facturaService.ActualizarEstadoPresupuesto(idPresupuestoSeleccionado.Value, "VENTA_REALIZADA");
                    idPresupuestoSeleccionado = null;
                }
                // Actualizar stock de los artículos vendidos
                _facturaService.ActualizarStockArticulos(detallesFactura);


                var resultado = MessageBox.Show(
                    $"Venta realizada correctamente.\n¿Desea imprimir el comprobante?",
                    "VENTA EXITOSA",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                // --- GENERAR PDF  ---
                if (resultado == DialogResult.Yes)
                {
                    var detallesReparacionParaPDF = new List<DetalleImprimible>();

                    // Agregar detalles de reparación al formato imprimible
                    foreach (var d in detallesReparacion)
                    {
                        var articulo = d.IdArticulo.HasValue ? listaArticulosActivos.FirstOrDefault(a => a.IdArticulo == d.IdArticulo.Value) : null;
                        var servicio = d.IdServicio > 0 ? listaServicios.FirstOrDefault(s => s.IdServicio == d.IdServicio) : null;
                        // Repuesto
                        if (d.PrecioRepuesto.HasValue && d.PrecioRepuesto.Value > 0)
                        {
                            detallesReparacionParaPDF.Add(new DetalleImprimible
                            {
                                Descripcion = $"Repuesto: {articulo?.Nombre ?? "N/A"}",
                                Cantidad = d.Cantidad,
                                PrecioUnitario = d.PrecioRepuesto.Value,
                                Subtotal = d.PrecioRepuesto.Value * d.Cantidad,
                                Tipo = "REPUESTO_REPARACION"
                            });
                        }
                        // Servicio
                        if (d.PrecioServicio.HasValue && d.PrecioServicio.Value > 0)
                        {
                            detallesReparacionParaPDF.Add(new DetalleImprimible
                            {
                                Descripcion = $"Servicio: {servicio?.Descripcion ?? "N/A"}",
                                Cantidad = 1,
                                PrecioUnitario = d.PrecioServicio.Value * d.Cantidad,
                                Subtotal = d.PrecioServicio.Value * d.Cantidad,
                                Tipo = "SERVICIO_REPARACION"
                            });
                        }
                    }
                    // Agregar detalles de artículos adicionales al formato imprimible
                    var detallesArticulosAdicionalesParaPDF = detallesFactura.Select(d => new DetalleImprimible
                    {
                        Descripcion = d.NombreArticulo,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = (decimal)d.PrecioUnitario,
                        Subtotal = d.subtotal,
                        Tipo = "ADICIONAL"
                    }).ToList();

                    factura.ID_Factura = idFactura;
                    // Generar el PDF según el método de pago
                    if (metodoPago == "CUENTA CORRIENTE")
                    {
                        GenerarComprobanteVenta(factura, cliente, detallesReparacionParaPDF, detallesArticulosAdicionalesParaPDF);
                    }
                    else
                    {
                        GenerarFacturaPDF(factura, cliente, detallesReparacionParaPDF, detallesArticulosAdicionalesParaPDF);
                    }

                }

                // --- LIMPIEZA DE INTERFAZ ---
                dataGridViewDetallesReparacion.DataSource = null;
                dataGridViewDetallesReparacion.Columns.Clear();
                dataGridViewDetallesVenta.DataSource = null;
                dataGridViewDetallesVenta.Columns.Clear();
                dataGridViewReparacionesCliente.DataSource = null;
                dataGridViewReparacionesCliente.Columns.Clear();
                detallesFactura.Clear();
                // Limpiar comboBoxes y campos
                comboBoxMetPago.SelectedIndex = -1;
                comboBoxClientes.SelectedIndex = -1;
                comboBoxArticulo.SelectedIndex = -1;

                textBoxDescripcionMetPago.Clear();
                labelTotal.Text = "TOTAL: $0,00";
                textBoxCantidad.Clear();
                lblModoOperacion.Text = "📋 Modo: Selección libre";
                lblModoOperacion.ForeColor = System.Drawing.Color.Black;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error CRÍTICO al guardar la venta y/o sus detalles:\n" + ex.Message, "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /**************************
        * IMPRIMIR FACTURA
        * ***************************/
        private void GenerarFacturaPDF(Facturas factura, Cliente cliente,
            List<DetalleImprimible> detallesReparacionParaPDF,
            List<DetalleImprimible> detallesArticulosAdicionalesParaPDF)
        {
            // VALIDACIONES INICIALES 
            if (factura == null || factura.ID_Factura <= 0)
            {
                MessageBox.Show("Factura no válida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // --- PREPARACIÓN DE DATOS DEL CLIENTE ---
            string nombreCliente = cliente?.NombreCompleto ?? "CONSUMIDOR FINAL";
            string cuitCuil = cliente?.Cuil ?? "N/A";
            string tipoFactura = factura.Tipo_Factura?.Trim().ToUpper() ?? "B";
            bool esFacturaA = tipoFactura == "A";
            decimal ivaRate = 0.21m;

            // --- CONFIGURACIÓN DEL DOCUMENTO PDF ---
            var pageSize = new iTextSharp.text.Rectangle(500f, 10000f);
            var doc = new Document(pageSize, 10f, 10f, 10f, 10f);

            // Guardar en la carpeta Descargas
            string rutaDescargas = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string rutaArchivo = Path.Combine(rutaDescargas, "Downloads",
                $"Factura_Tipo_{tipoFactura}_{factura.ID_Factura:00000000}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            try
            {
                using (PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(rutaArchivo, FileMode.Create)))
                {
                    doc.Open();

                    // Tipografías
                    var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY);
                    var fontSubtitulo = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD);
                    var fontNormal = FontFactory.GetFont(FontFactory.HELVETICA, 8);
                    var fontExtra = FontFactory.GetFont(FontFactory.HELVETICA, 6);
                    var fontTotal = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD);

                    var separator = new Chunk(new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -1f));

                    // --- CABECERA ---
                    var titulo = new Paragraph($"FACTURA TIPO {tipoFactura}", fontTitulo) { Alignment = Element.ALIGN_CENTER };
                    doc.Add(titulo);
                    doc.Add(new Paragraph($"N° {factura.ID_Factura:00000000}", fontSubtitulo) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new Paragraph("CASA DE REPUESTOS", fontTitulo) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new Paragraph("CUIT: 30-xxxxxxxx-x", fontNormal) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new Paragraph("Dirección: Calle Falsa 123 | Tel: 555-1234", fontNormal) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(separator);

                    // --- DATOS DE LA VENTA ---
                    doc.Add(new Paragraph($"Fecha: {factura.Fecha:dd/MM/yyyy}", fontNormal));
                    doc.Add(new Paragraph($"Cliente: {nombreCliente}", fontNormal));
                    if (esFacturaA)
                        doc.Add(new Paragraph($"CUIT/CUIL: {cuitCuil}", fontNormal));
                    if (cliente != null)
                        doc.Add(new Paragraph($"Categoría: {cliente?.Categoria}", fontNormal));
                    doc.Add(new Paragraph($"Método Pago: {factura.Metodo_Pago ?? "N/A"}", fontNormal));
                    doc.Add(separator);

                    // Función auxiliar para construir tablas según tipo de factura
                    void AgregarTabla(string titulo, List<DetalleImprimible> detalles)
                    {
                        if (!detalles.Any()) return;

                        doc.Add(new Paragraph(titulo, fontSubtitulo) { SpacingBefore = 6, SpacingAfter = 4 });

                        // Cantidad de columnas según tipo
                        int columnas = esFacturaA ? 5 : 4;
                        PdfPTable tabla = new PdfPTable(columnas)
                        {
                            WidthPercentage = 100
                        };

                        // Configurar anchos proporcionales
                        if (esFacturaA)
                            tabla.SetWidths(new float[] { 3.5f, 0.8f, 1.3f, 1.2f, 1.4f }); 
                        else
                            tabla.SetWidths(new float[] { 4.0f, 0.8f, 1.4f, 1.6f });       

                        BaseColor headerColor = new BaseColor(230, 230, 230);

                        // --- ENCABEZADOS ---
                        tabla.AddCell(new PdfPCell(new Phrase("Descripción", fontSubtitulo)) { BackgroundColor = headerColor });
                        tabla.AddCell(new PdfPCell(new Phrase("Cant.", fontSubtitulo)) { BackgroundColor = headerColor, HorizontalAlignment = Element.ALIGN_CENTER });

                        if (esFacturaA)
                        {
                            tabla.AddCell(new PdfPCell(new Phrase("Precio Neto", fontSubtitulo)) { BackgroundColor = headerColor, HorizontalAlignment = Element.ALIGN_RIGHT });
                            tabla.AddCell(new PdfPCell(new Phrase("IVA 21%", fontSubtitulo)) { BackgroundColor = headerColor, HorizontalAlignment = Element.ALIGN_RIGHT });
                        }
                        else
                        {
                            tabla.AddCell(new PdfPCell(new Phrase("P. Unit.", fontSubtitulo)) { BackgroundColor = headerColor, HorizontalAlignment = Element.ALIGN_RIGHT });
                        }

                        tabla.AddCell(new PdfPCell(new Phrase("Subtotal", fontSubtitulo)) { BackgroundColor = headerColor, HorizontalAlignment = Element.ALIGN_RIGHT });

                        // --- FILAS ---
                        foreach (var det in detalles)
                        {
                            decimal precioNeto = Math.Round(det.PrecioUnitario / (1 + ivaRate), 2);
                            decimal montoIva = Math.Round(det.PrecioUnitario - precioNeto, 2);
                            decimal subtotal = det.Subtotal;

                            // Descripción
                            tabla.AddCell(new PdfPCell(new Phrase(det.Descripcion, fontNormal))
                            {
                                Border = 0,
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                PaddingBottom = 3f
                            });

                            // Cantidad
                            tabla.AddCell(new PdfPCell(new Phrase(det.Cantidad.ToString(), fontNormal))
                            {
                                Border = 0,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            //  según tipo de factura A o B
                            if (esFacturaA)
                            {
                                tabla.AddCell(new PdfPCell(new Phrase(precioNeto.ToString("C2"), fontNormal)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                                tabla.AddCell(new PdfPCell(new Phrase(montoIva.ToString("C2"), fontNormal)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                            }
                            else
                            {
                                tabla.AddCell(new PdfPCell(new Phrase(det.PrecioUnitario.ToString("C2"), fontNormal)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                            }

                            // Subtotal 
                            tabla.AddCell(new PdfPCell(new Phrase(subtotal.ToString("C2"), fontNormal))
                            {
                                Border = 0,
                                HorizontalAlignment = Element.ALIGN_RIGHT
                            });
                        }

                        doc.Add(tabla);
                        doc.Add(new Paragraph("\n"));
                    }

                    // --- DETALLES ---
                    AgregarTabla("DETALLES DE LOS SERVICIOS Y REPUESTOS", detallesReparacionParaPDF);
                    AgregarTabla("ARTÍCULOS ADICIONALES", detallesArticulosAdicionalesParaPDF);

                    // --- TOTALES ---
                    if (esFacturaA)
                    {
                        decimal totalNeto = Math.Round(factura.Total_Factura / (1 + ivaRate), 2);
                        decimal totalIVA = Math.Round(factura.Total_Factura - totalNeto, 2);
                       
                        doc.Add(separator);
                        // Mostrar subtotal neto e IVA
                        doc.Add(new Paragraph($"Subtotal Neto: {totalNeto:C2}", fontNormal) { Alignment = Element.ALIGN_RIGHT });
                        doc.Add(new Paragraph($"IVA (21%): {totalIVA:C2}", fontNormal) { Alignment = Element.ALIGN_RIGHT });
                    }

                    var totalParagraph = new Paragraph($"TOTAL: {factura.Total_Factura.ToString("C2")}", fontTotal)
                    {
                        Alignment = Element.ALIGN_RIGHT,
                        SpacingBefore = 5
                    };
                    doc.Add(totalParagraph);

                    doc.Add(new Paragraph("\n"));
                    doc.Add(new Paragraph("Gracias por su compra", fontSubtitulo) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new Paragraph("Comprobante no válido como Factura oficial sin validez fiscal (Consulte al emisor)", fontExtra) { Alignment = Element.ALIGN_CENTER });

                    doc.Close();
                }

                MessageBox.Show($"Factura N° {factura.ID_Factura} exportada a: {rutaArchivo}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(new ProcessStartInfo(rutaArchivo) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el PDF de la factura: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**********************************************
        * IMPRIMIR COMPROBANTE PARA CUENTA CORRIENTE
        ********************************************/
        private void GenerarComprobanteVenta(Facturas factura, Cliente cliente, List<DetalleImprimible> detallesReparacionParaPDF, List<DetalleImprimible> detallesArticulosAdicionalesParaPDF)
        {
            // *** VALIDACIONES INICIALES ***
            if (factura == null || factura.ID_Factura <= 0)
            {
                MessageBox.Show("Comprobante no válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // --- PREPARACIÓN DE DATOS DEL CLIENTE ---
            string nombreCliente = cliente?.NombreCompleto ?? "CONSUMIDOR FINAL";
            string cuitCuil = cliente?.Cuil ?? "N/A";

            // --- CONFIGURACIÓN DEL DOCUMENTO PDF ---
            var pageSize = new iTextSharp.text.Rectangle(500f, 10000f);
            var doc = new Document(pageSize, 10f, 10f, 10f, 10f); 

            // Guardar en la carpeta Descargas 
            string rutaDescargas = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string rutaArchivo = Path.Combine(rutaDescargas, "Downloads", $"Comprobante_{factura.ID_Factura.ToString().PadLeft(8, '0')}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            try
            {
                using (PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(rutaArchivo, FileMode.Create)))
                {
                    doc.Open();

                    // Tipografías
                    var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY);
                    var fontSubtitulo = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD);
                    var fontNormal = FontFactory.GetFont(FontFactory.HELVETICA, 8);
                    var fontExtra = FontFactory.GetFont(FontFactory.HELVETICA, 6);
                    var fontTotal = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD);

                    // Separador de línea
                    var separator = new Chunk(new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -1f));

                    // --- CABECERA (TÍTULO Y DATOS EMPRESA) ---
                    var titulo = new Paragraph($"COMPROBANTE DE VENTA", fontTitulo)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    doc.Add(titulo);
                    doc.Add(new Paragraph($"N° {factura.ID_Factura.ToString().PadLeft(8, '0')}", fontSubtitulo) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new Paragraph("CASA DE REPUESTOS", fontTitulo) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new Paragraph("CUIT: 30-xxxxxxxx-x", fontNormal) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new Paragraph("Dirección: Calle Falsa 123 | Tel: 555-1234", fontNormal) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(separator);

                    // --- DATOS DE LA VENTA ---
                    doc.Add(new Paragraph($"Fecha: {factura.Fecha:dd/MM/yyyy}", fontNormal));
                    doc.Add(new Paragraph($"Cliente: {nombreCliente}", fontNormal));
                    //si es factura tipo B no mostrar cuit/cuil
                    if (factura.Tipo_Factura != "B")
                    {
                        doc.Add(new Paragraph($"CUIT/CUIL: {cuitCuil}", fontNormal));
                    }
                    // si existe un cliente, mostrar su categoría
                    if (cliente != null)
                    {
                        doc.Add(new Paragraph($"Categoría: {cliente?.Categoria}", fontNormal));
                    }
                    doc.Add(new Paragraph($"Método Pago: {factura.Metodo_Pago ?? "N/A"}", fontNormal));
                    doc.Add(separator);

                    // --- TABLA DE DETALLES ---
                    // --------------------------------------------------------------------------------
                    // SECCIÓN 1: DETALLES DE REPARACIÓN Y SERVICIOS
                    // --------------------------------------------------------------------------------
                    if (detallesReparacionParaPDF.Any())
                    {
                        doc.Add(new Paragraph("DETALLES DE LOS SERVICIOS Y REPUESTOS", fontSubtitulo) { SpacingBefore = 5, SpacingAfter = 5 });

                        PdfPTable tablaReparaciones = new PdfPTable(4)
                        {
                            WidthPercentage = 100
                        };
                        tablaReparaciones.SetWidths(new float[] { 3.5f, 1f, 1.5f, 1.5f });

                        // Encabezados
                        BaseColor headerColor = new BaseColor(230, 230, 230);
                        tablaReparaciones.AddCell(new PdfPCell(new Phrase("Descripción", fontSubtitulo)) { BackgroundColor = headerColor, BorderWidthBottom = 1 });
                        tablaReparaciones.AddCell(new PdfPCell(new Phrase("Cant.", fontSubtitulo)) { BackgroundColor = headerColor, BorderWidthBottom = 1, HorizontalAlignment = Element.ALIGN_CENTER });
                        tablaReparaciones.AddCell(new PdfPCell(new Phrase("P. Unit.", fontSubtitulo)) { BackgroundColor = headerColor, BorderWidthBottom = 1, HorizontalAlignment = Element.ALIGN_RIGHT });
                        tablaReparaciones.AddCell(new PdfPCell(new Phrase("Subtotal", fontSubtitulo)) { BackgroundColor = headerColor, BorderWidthBottom = 1, HorizontalAlignment = Element.ALIGN_RIGHT });

                        // Filas
                        foreach (var det in detallesReparacionParaPDF)
                        {
                            tablaReparaciones.AddCell(new PdfPCell(new Phrase(det.Descripcion, fontNormal)) { Border = 0 });
                            tablaReparaciones.AddCell(new PdfPCell(new Phrase(det.Cantidad.ToString(), fontNormal)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER });
                            tablaReparaciones.AddCell(new PdfPCell(new Phrase(det.PrecioUnitario.ToString("C2"), fontNormal)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                            tablaReparaciones.AddCell(new PdfPCell(new Phrase(det.Subtotal.ToString("C2"), fontNormal)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                        }

                        doc.Add(tablaReparaciones);
                        doc.Add(new Paragraph("\n"));
                    }

                    // --------------------------------------------------------------------------------
                    // SECCIÓN 2: ARTÍCULOS ADICIONALES
                    // --------------------------------------------------------------------------------
                    if (detallesArticulosAdicionalesParaPDF.Any())
                    {
                        doc.Add(new Paragraph("ARTÍCULOS", fontSubtitulo) { SpacingBefore = 5, SpacingAfter = 5 });

                        PdfPTable tablaAdicionales = new PdfPTable(4)
                        {
                            WidthPercentage = 100
                        };
                        tablaAdicionales.SetWidths(new float[] { 3.5f, 1f, 1.5f, 1.5f });

                        // Encabezados
                        BaseColor headerColor = new BaseColor(230, 230, 230);
                        tablaAdicionales.AddCell(new PdfPCell(new Phrase("Descripción", fontSubtitulo)) { BackgroundColor = headerColor, BorderWidthBottom = 1 });
                        tablaAdicionales.AddCell(new PdfPCell(new Phrase("Cant.", fontSubtitulo)) { BackgroundColor = headerColor, BorderWidthBottom = 1, HorizontalAlignment = Element.ALIGN_CENTER });
                        tablaAdicionales.AddCell(new PdfPCell(new Phrase("P. Unit.", fontSubtitulo)) { BackgroundColor = headerColor, BorderWidthBottom = 1, HorizontalAlignment = Element.ALIGN_RIGHT });
                        tablaAdicionales.AddCell(new PdfPCell(new Phrase("Subtotal", fontSubtitulo)) { BackgroundColor = headerColor, BorderWidthBottom = 1, HorizontalAlignment = Element.ALIGN_RIGHT });

                        // Filas
                        foreach (var det in detallesArticulosAdicionalesParaPDF)
                        {
                            tablaAdicionales.AddCell(new PdfPCell(new Phrase(det.Descripcion, fontNormal)) { Border = 0 });
                            tablaAdicionales.AddCell(new PdfPCell(new Phrase(det.Cantidad.ToString(), fontNormal)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER });
                            tablaAdicionales.AddCell(new PdfPCell(new Phrase(det.PrecioUnitario.ToString("C2"), fontNormal)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                            tablaAdicionales.AddCell(new PdfPCell(new Phrase(det.Subtotal.ToString("C2"), fontNormal)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                        }

                        doc.Add(tablaAdicionales);
                        doc.Add(new Paragraph("\n"));
                    }
                    doc.Add(new Paragraph("\n"));

                    // --- PIE DE PÁGINA ---
                    doc.Add(new Paragraph("Gracias por su compra", fontSubtitulo) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new Paragraph("\n"));
                    doc.Add(new Paragraph("Comprobante no válido como Factura oficial sin validez fiscal (Consulte al emisor)", fontExtra) { Alignment = Element.ALIGN_CENTER });

                    doc.Close();
                }

                MessageBox.Show($"Comprobante de venta N° {factura.ID_Factura} exportado a: {rutaArchivo}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Abrir el archivo
                Process.Start(new ProcessStartInfo(rutaArchivo) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el PDF del comprobante: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Clase DetalleImprimible para combinar detalles de reparación y artículos vendidos en el ticket
        private class DetalleImprimible
        {
            public string Descripcion { get; set; }
            public int Cantidad { get; set; }
            public decimal PrecioUnitario { get; set; }
            public decimal Subtotal { get; set; }
            public string Tipo { get; set; }
        }

        // Dirige al módulo de clientes para crear nuevo cliente
        private void buttonAgregarNvoCliente_Click(object sender, EventArgs e)
        {
            new FrmCliente().ShowDialog();

        }        
    }
}
