using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CasaRepuestos.Models; 
using CasaRepuestos.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;

namespace CasaRepuestos.Forms
{
    // Formulario para gestionar compras y órdenes de compra
    public partial class FrmCompras : Form
    {
    
        private readonly ComprasService _comprasService = new ComprasService();// Servicio para operaciones de compras
        private readonly ProveedorService _proveedorService = new ProveedorService();// Servicio para operaciones con proveedores
        private readonly ArticuloService _articuloService = new ArticuloService();// Servicio para operaciones con artículos
        private readonly ArqueoService _arqueoService = new ArqueoService();// Servicio para operaciones de arqueo de caja


        private List<ArticuloFaltante> _repuestosFaltantesConsolidados = new List<ArticuloFaltante>();// Lista de repuestos faltantes consolidados


        private BindingList<DetalleOrdenCompra> _detallesOrdenActual = new BindingList<DetalleOrdenCompra>();// Detalles de la orden de compra actual

        // Constructor del formulario
        public FrmCompras()
        {
            InitializeComponent();


            
            CargarProveedoresYArticulos();// Cargar proveedores y artículos
            
            ConfigurarGrillaNecesidades();// Configurar grilla de necesidades
            ConfigurarGrillaOrdenStock();// Configurar grilla de orden de stock
            ConfigurarGrillaOrdenesPendientes();// Configurar grilla de órdenes pendientes
            ConfigurarGrillaHistorial();// Configurar grilla de historial de compras
            AplicarEstilos();// Aplicar estilos visuales



            btnBuscarFaltantes.Click += btnBuscarFaltantes_Click;// Evento para buscar repuestos faltantes

            cmbFiltroTipoHistorial.Items.Add("Todos");// Filtro de tipo "Todos"
            cmbFiltroTipoHistorial.Items.Add("VENTA");// Filtro de tipo "VENTA"
            cmbFiltroTipoHistorial.Items.Add("REPUESTO");// Filtro de tipo "REPUESTO"
            cmbFiltroTipoHistorial.SelectedIndex = 0;// Seleccionar "Todos" por defecto
            btnBuscarHistorial.Click += btnBuscarHistorial_Click;// Evento para buscar en el historial
            CargarOrdenesPendientes();// Cargar órdenes pendientes
        }


        // Método para aplicar estilos visuales al formulario y sus controles
        private void AplicarEstilos()
        {

            this.BackColor = Color.FromArgb(240, 245, 249);// Color de fondo del formulario

            // Aplicar estilos a TabPages
            foreach (var tp in EncontrarControlesRecursivos<TabPage>(this))
            {
                tp.BackColor = Color.White;
            }

            // Aplicar estilos a GroupBoxes
            foreach (var gb in EncontrarControlesRecursivos<GroupBox>(this))
            {
                gb.BackColor = Color.White;
                gb.ForeColor = Color.FromArgb(45, 66, 91);
            }

            // Aplicar estilos a DataGridViews
            foreach (var dgv in EncontrarControlesRecursivos<DataGridView>(this))
            {
                if (dgv != null)
                {
                    dgv.BackgroundColor = Color.White;
                    dgv.EnableHeadersVisualStyles = false;

          
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 66, 91);
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
          
                    dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
                    dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(4);
                    dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;


                    dgv.RowsDefaultCellStyle.BackColor = Color.White;

                    dgv.RowsDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
                    dgv.RowsDefaultCellStyle.ForeColor = Color.FromArgb(40, 40, 40);
                    dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 249); 

         
                    dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                    dgv.GridColor = Color.FromArgb(221, 233, 245); 
                    dgv.BorderStyle = BorderStyle.None;
                }
            }

            // Aplicar estilos a Buttons
            foreach (var btn in EncontrarControlesRecursivos<Button>(this))
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Color.FromArgb(76, 132, 200); 
                btn.ForeColor = Color.White;

                btn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                btn.Padding = new Padding(4); 
            }

            // Estilos específicos para botones destacados
            try
            {
                btnCrearOrdenStock.BackColor = Color.FromArgb(34, 139, 34); 
                btnCrearOrdenStock.ForeColor = Color.White;
 
                btnCrearOrdenStock.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold); 
            }
            catch { }

            try
            {
                // Estilos para botones de búsqueda
                btnBuscarFaltantes.BackColor = Color.FromArgb(12, 87, 150); 
                btnBuscarHistorial.BackColor = Color.FromArgb(12, 87, 150); 
            }
            catch { }

        }
        // Método genérico para encontrar controles de un tipo específico de forma recursiva
        private IEnumerable<T> EncontrarControlesRecursivos<T>(Control controlPadre) where T : Control
        {
            var controlesEncontrados = new List<T>();// Lista para almacenar los controles encontrados

            foreach (Control ctl in controlPadre.Controls)// Iterar sobre los controles hijos
            {
              
                if (ctl is T)// Si el control es del tipo buscado
                {
                    controlesEncontrados.Add((T)ctl);// Agregar a la lista
                }

     
                if (ctl.Controls.Count > 0)// Si el control tiene hijos
                {
                    controlesEncontrados.AddRange(EncontrarControlesRecursivos<T>(ctl));// Llamada recursiva
                }
            }
            return controlesEncontrados;// Devolver la lista de controles encontrados
        }
        // Cargar proveedores y artículos en los controles correspondientes
        private void CargarProveedoresYArticulos()
        {
            try
            {
                // Se obtiene la lista completa de artículos desde el servicio
                var listaDeArticulos = _articuloService.ListarArticulos();
                // Configura el ComboBox para mostrar artículos
                cmbArticuloStock.DataSource = listaDeArticulos.OrderBy(a => a.Nombre).ToList();
                cmbArticuloStock.DisplayMember = "Nombre";       
                cmbArticuloStock.ValueMember = "IdArticulo";

                // Habilita el autocompletado
                cmbArticuloStock.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbArticuloStock.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar artículos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Configurar la grilla de la orden de stock
        private void ConfigurarGrillaOrdenStock()
        {
            dgvOrdenStock.AutoGenerateColumns = false;// Deshabilitar generación automática de columnas


            if (dgvOrdenStock.Columns["NombreArticuloCol"] is DataGridViewTextBoxColumn nombreCol)// Configurar columna de nombre de artículo
            {
                nombreCol.DataPropertyName = "ArticuloNombre";// Asignar propiedad de datos
            }

            // Eliminar columna de proveedor si existe
            if (dgvOrdenStock.Columns.Contains("ProveedorCol"))
            {
                dgvOrdenStock.Columns.Remove("ProveedorCol");
            }
            // Configurar columna de cantidad
            if (dgvOrdenStock.Columns["CantidadCol"] is DataGridViewTextBoxColumn cantidadCol)
            {
                cantidadCol.DataPropertyName = "CantidadSolicitada";
                cantidadCol.ReadOnly = false;
            }
            // Configurar columna de precio unitario
            if (dgvOrdenStock.Columns["PrecioUnitarioCol"] is DataGridViewTextBoxColumn precioCol)
            {
                precioCol.DataPropertyName = "PrecioUnitarioEstimado";
                precioCol.ReadOnly = false;
            }
            // Configurar columna de subtotal
            List<Proveedor> listaMaestraProveedores;
            try
            {
                listaMaestraProveedores = _proveedorService.ListarProveedores() ?? new List<Proveedor>();// Obtener lista de proveedores

                // Agregar opción por defecto si no existe
                if (!listaMaestraProveedores.Any(p => p.IdProveedor == 0))
                {   
                    listaMaestraProveedores.Insert(0, new Proveedor { IdProveedor = 0, RazonSocial = "[Seleccionar Proveedor]" });// Opción por defecto
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la lista maestra de proveedores: {ex.Message}", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                listaMaestraProveedores = new List<Proveedor> { new Proveedor { IdProveedor = 0, RazonSocial = "[Error al Cargar]" } };// Lista con opción de error
            }
            // Crear y configurar columna ComboBox para proveedores
            var cmbColProveedor = new DataGridViewComboBoxColumn
            {
                Name = "ProveedorCol",// Nombre de la columna
                HeaderText = "Proveedor",// Texto del encabezado
                DataPropertyName = "IdProveedor",// Propiedad de datos
                DisplayMember = "RazonSocial",// Propiedad para mostrar
                ValueMember = "IdProveedor",// Propiedad de valor

                DataSource = listaMaestraProveedores,// Asignar lista de proveedores como fuente de datos

                Width = 150,
                FlatStyle = FlatStyle.Flat
            };



            dgvOrdenStock.Columns.Insert(3, cmbColProveedor);// Insertar columna en la posición 3

            dgvOrdenStock.DataSource = _detallesOrdenActual;// Asignar la fuente de datos

            dgvOrdenStock.CellEndEdit -= dgvOrdenStock_CellEndEdit;// Desuscribir evento CellEndEdit
            dgvOrdenStock.CellEndEdit += dgvOrdenStock_CellEndEdit;// Suscribir evento CellEndEdit
            dgvOrdenStock.EditingControlShowing -= dgvOrdenStock_EditingControlShowing;// Desuscribir evento EditingControlShowing
            dgvOrdenStock.EditingControlShowing += dgvOrdenStock_EditingControlShowing;// Suscribir evento EditingControlShowing

            dgvOrdenStock.CellValueChanged -= dgvOrdenStock_CellValueChanged;// Desuscribir evento CellValueChanged
            dgvOrdenStock.CellValueChanged += dgvOrdenStock_CellValueChanged;// Suscribir evento CellValueChanged
            dgvOrdenStock.DataError -= dgvOrdenStock_DataError;// Desuscribir evento DataError
            dgvOrdenStock.DataError += dgvOrdenStock_DataError;// Suscribir evento DataError
        }
        private void ConfigurarGrillaNecesidades()// Configurar la grilla de necesidades de repuestos
        {
            dgvNecesidades.AutoGenerateColumns = false;// Deshabilitar generación automática de columnas
            dgvNecesidades.Columns.Clear();// Limpiar columnas existentes
            // Columnas de Necesidades de Repuestos
            dgvNecesidades.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "IdArticulo", HeaderText = "ID", Name = "IdArticulo", Width = 50 });
            dgvNecesidades.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreArticulo", HeaderText = "Artículo Faltante", Name = "NombreArticulo", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvNecesidades.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CantidadFaltante", HeaderText = "Cant. Faltante", Name = "CantidadFaltante", Width = 100 });

            

            var btnCol = new DataGridViewButtonColumn// Columna de botón para añadir a la orden
            {
                Text = "Añadir a Orden",// Texto del botón
                UseColumnTextForButtonValue = true,// Usar texto definido
                Name = "AnadirAOrden",// Nombre de la columna
                HeaderText = "Acción",// Texto del encabezado
                Width = 120// Ancho de la columna
            };
            dgvNecesidades.Columns.Add(btnCol);// Agregar columna de botón a la grilla
        }
        // Carga las necesidades de repuestos faltantes consolidados
        private void CargarNecesidadesRepuestos()
        {
            try
            {
                // Obtener las necesidades consolidadas desde el servicio
                _repuestosFaltantesConsolidados = _comprasService.ObtenerNecesidadesRepuestosConsolidadas();

                // Asigna la lista 
                dgvNecesidades.DataSource = _repuestosFaltantesConsolidados;

                // Actualizar el mensaje
                label5.Text = _repuestosFaltantesConsolidados.Any()
                    ? $"Repuestos Faltantes Consolidados de presupuestos ESPERA_REPUESTOS ({_repuestosFaltantesConsolidados.Count} ítems)."
                    : "No hay repuestos en estado 'ESPERA_REPUESTOS' que necesiten ser comprados.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las necesidades de repuestos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Carga la lista de órdenes pendientes 
        private void CargarOrdenesPendientes()
        {
            try
            {
                //  Listar órdenes pendientes desde el servicio
                dgvOrdenesPendientes.DataSource = _comprasService.ListarOrdenesPendientes();

                // Verificar si la columna de botón "Registrar Ingreso" ya existe
                if (!dgvOrdenesPendientes.Columns.Contains("RegistrarIngreso"))
                {
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar órdenes pendientes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Configurar la grilla de órdenes pendientes
        private void ConfigurarGrillaOrdenesPendientes()
        {
            dgvOrdenesPendientes.AutoGenerateColumns = false;// Deshabilitar generación automática de columnas

            // Columnas de Órdenes Pendientes
            dgvOrdenesPendientes.Columns.Clear();
            dgvOrdenesPendientes.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "IdOrdenCompra", HeaderText = "ID", Name = "IdOrdenCompra", Width = 60 });
            dgvOrdenesPendientes.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FechaCreacion", HeaderText = "Fecha Creación", Name = "FechaCreacion", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" } });
            dgvOrdenesPendientes.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ProveedorNombre", HeaderText = "Proveedor", Name = "ProveedorNombre", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvOrdenesPendientes.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TotalEstimado", HeaderText = "Total Estimado", Name = "TotalEstimado", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } });
            dgvOrdenesPendientes.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Tipo", HeaderText = "Tipo", Name = "Tipo", Width = 80 });

            // Botones de acción
            dgvOrdenesPendientes.Columns.Add(new DataGridViewButtonColumn { Text = "PDF", UseColumnTextForButtonValue = true, Name = "ExportarPDF", HeaderText = "PDF", Width = 60 });
            dgvOrdenesPendientes.Columns.Add(new DataGridViewButtonColumn { Text = "Recibir", UseColumnTextForButtonValue = true, Name = "RegistrarIngreso", HeaderText = "Ingreso", Width = 70 });


            // Eventos para los botones
            dgvOrdenesPendientes.CellContentClick -= dgvOrdenesPendientes_CellContentClick;
            dgvOrdenesPendientes.CellContentClick += dgvOrdenesPendientes_CellContentClick;
        }

        // Configurar la grilla del historial de compras
        private void ConfigurarGrillaHistorial()
        {
            dgvHistorial.AutoGenerateColumns = false;// Deshabilitar generación automática de columnas
            dgvHistorial.Columns.Clear();

            // Columnas de Historial
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "IdCompra", HeaderText = "ID", Name = "IdCompra", Width = 60 });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Fecha", HeaderText = "Fecha", Name = "FechaCompra", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" } });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Tipo", HeaderText = "Tipo", Name = "TipoCompra", Width = 80 });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Proveedor", HeaderText = "Proveedor", Name = "ProveedorHistorial", Width = 150 });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ArticulosResumen", HeaderText = "Artículos (Resumen)", Name = "ArticulosResumen", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Total", HeaderText = "Total", Name = "TotalCompra", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } });
        }


        // -----------------------------------------------------
        // MÉTODOS PARA ARMAR LA ORDEN DE COMPRA 
        // -----------------------------------------------------
        // Lógica para agregar ítem de stock a la orden
        private void btnAgregarItemStock_Click(object sender, EventArgs e)
        {
            // Verificamos SelectedItem 
            if (cmbArticuloStock.SelectedItem == null || (int)numCantidadStock.Value <= 0)
            {
                MessageBox.Show("Seleccione un artículo y una cantidad válida.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtenemos el objeto Articulo 
            var itemSeleccionado = (Articulo)cmbArticuloStock.SelectedItem;
            if (itemSeleccionado == null) return;

            // Llamamos a AgregarItemAOrden 
            AgregarItemAOrden(
                itemSeleccionado.IdArticulo,
                itemSeleccionado.Nombre,       
                (int)numCantidadStock.Value,
                itemSeleccionado.PrecioCosto,
                0,                         
                "",                            
                "VENTA"
            );

            numCantidadStock.Value = 1;// Reiniciamos la cantidad a 1
        }
        // Lógica para agregar repuestos faltantes a la orden desde la grilla de necesidades
        private void dgvNecesidades_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvNecesidades.Columns[e.ColumnIndex].Name != "AnadirAOrden") return;// Verificamos que sea la columna del botón

            var repuestoFaltante = dgvNecesidades.Rows[e.RowIndex].DataBoundItem as ArticuloFaltante;// Obtenemos el repuesto faltante
            if (repuestoFaltante == null) return;// Verificamos el objeto

            //  Guardamos el ID del artículo
            int idArticuloAgregado = repuestoFaltante.IdArticulo;

            // Agregamos a la orden 
            AgregarItemAOrden(
                repuestoFaltante.IdArticulo,// IdArtículo
                repuestoFaltante.NombreArticulo,// NombreArtículo
                repuestoFaltante.CantidadFaltante,// Cantidad
                repuestoFaltante.PrecioUnitarioSugerido,//
                0,      // IdProveedor
                "", // ProveedorNombre
                "REPUESTO"// TipoIntencion
            );

            //  Verificamos si se agregó correctamente
            bool agregadoExitosamente = _detallesOrdenActual.Any(item => item.IdArticulo == idArticuloAgregado);

            if (agregadoExitosamente)// Si se agregó correctamente
            {
                // Eliminamos el repuesto de la lista de necesidades 
                _repuestosFaltantesConsolidados.Remove(repuestoFaltante);

                // Refrescamos la grilla de la derecha 
                dgvNecesidades.DataSource = null;
                dgvNecesidades.DataSource = _repuestosFaltantesConsolidados;

                // Actualizamos el contador
                label5.Text = _repuestosFaltantesConsolidados.Any()
                    ? $"Repuestos Faltantes... ({_repuestosFaltantesConsolidados.Count} ítems)."
                    : "No hay repuestos que necesiten ser comprados.";
            }
        }
        // Método para agregar un ítem a la orden de compra actual
        private void AgregarItemAOrden(int idArticulo, string nombreArticulo, int cantidad, decimal precioUnitario, int idProveedor, string proveedorNombre, string tipoIntencion) // 💡 CAMBIO: decimal
        {
            if (cantidad <= 0) return;// Validar cantidad positiva

            var itemExistente = _detallesOrdenActual.FirstOrDefault(d => d.IdArticulo == idArticulo);// Buscar ítem existente por IdArtículo

            if (itemExistente != null)// Verificar duplicados
            {
                MessageBox.Show($"El artículo '{nombreArticulo}' ya está en la orden.", "Artículo Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _detallesOrdenActual.Add(new DetalleOrdenCompra// Agregar nuevo ítem a la orden
            {
                IdArticulo = idArticulo,// IdArtículo
                ArticuloNombre = nombreArticulo,// NombreArtículo
                CantidadSolicitada = cantidad,// CantidadSolicitada
                PrecioUnitarioEstimado = precioUnitario, 
                IdProveedor = 0,// IdProveedor
                ProveedorNombre = "[SELECCIONAR]",// ProveedorNombre
                TipoIntencion = tipoIntencion// TipoIntención
            });

            CalcularTotalOrden();// Recalcular total de la orden
        }
        // Método para calcular el total estimado de la orden de compra
        private void CalcularTotalOrden()
        {
            decimal total = 0m; // Inicializar total
            foreach (var detalle in _detallesOrdenActual)// Iterar sobre los detalles de la orden
            {
                total += detalle.CantidadSolicitada * detalle.PrecioUnitarioEstimado;// Sumar subtotal de cada detalle
            }

            txtTotalOrden.Text = total.ToString("C2", System.Globalization.CultureInfo.GetCultureInfo("es-AR"));// Mostrar total en formato moneda argentina
        }
        // Manejo de eventos en la grilla de la orden de stock
        private void dgvOrdenStock_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;// Validar índices

            // Eliminar ítem de la orden
            if (dgvOrdenStock.Columns[e.ColumnIndex].Name == "EliminarCol")
            {
                if (MessageBox.Show("¿Está seguro de eliminar este ítem de la orden?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var item = dgvOrdenStock.Rows[e.RowIndex].DataBoundItem as DetalleOrdenCompra;// Obtener ítem seleccionado
                    if (item != null)// Validar ítem
                    {
                        _detallesOrdenActual.Remove(item);// Eliminar ítem de la lista


                        CalcularTotalOrden();// Recalcular total de la orden

                    }
                }
            }
        }

        // -----------------------------------------------------
        // MÉTODOS PARA CREAR LA ORDEN
        // -----------------------------------------------------
        // Lógica para crear la orden de stock
        private void btnCrearOrdenStock_Click(object sender, EventArgs e)
        {
            // Verificar si la caja está cerrada
            if (_arqueoService.EsCajaDelDiaCerrada())
            {
                MessageBox.Show(
                    "No se pueden realizar compras.\n\nLa caja del día ya ha sido cerrada.\nPara realizar transacciones, debe reabrir la caja desde el módulo de Arqueo.",
                    "Caja Cerrada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Verificar si no hay caja abierta
            if (!_arqueoService.ExisteCajaAbiertaHoy())
            {
                MessageBox.Show(
                    "No se pueden realizar compras.\n\nNo hay una caja abierta para el día de hoy.\nPor favor, abra la caja desde el módulo de Arqueo antes de realizar transacciones.",
                    "Caja No Abierta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
            // Validar que haya al menos un artículo en la orden
            if (_detallesOrdenActual.Count == 0)
            {
                MessageBox.Show("Debe añadir al menos un artículo a la orden.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar que todos los artículos tengan un proveedor seleccionado
            var itemSinProveedor = _detallesOrdenActual.FirstOrDefault(d => d.IdProveedor == 0);
            if (itemSinProveedor != null)// Si hay un artículo sin proveedor
            {
                MessageBox.Show($"Debe seleccionar un proveedor para el artículo '{itemSinProveedor.ArticuloNombre}'.", "Proveedor Faltante", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("¿Confirmar la creación de la(s) Orden(es) de Compra?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            try
            {
                // Muestra un resumen antes de crear las órdenes
                var gruposPorProveedorYTipo = _detallesOrdenActual
                    .Where(d => d.IdProveedor > 0) // Filtrar solo detalles con proveedor válido
                    .GroupBy(detalle => new { detalle.IdProveedor, detalle.TipoIntencion }) // Agrupar por IdProveedor y TipoIntencion
                    .ToList();// Convertir a lista

                if (!gruposPorProveedorYTipo.Any())// Si no hay grupos válidos
                {
                    MessageBox.Show("No se pueden crear órdenes: Algún artículo agregado no tiene proveedor asignado.", "Error de Proveedor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var pdfsGenerados = new List<string>();// Lista para almacenar rutas de PDFs generados
                var fechaDeEmision = DateTime.Now;// Fecha de emisión de las órdenes
                var idsOrdenesPorProveedor = new Dictionary<int, List<int>>();// Diccionario para almacenar IDs de órdenes por proveedor
                // iterar sobre cada grupo para crear órdenes separadas
                foreach (var grupo in gruposPorProveedorYTipo)
                {
                    // Declarar y asignar el ID del proveedor del grupo
                    int idProveedorActual = grupo.Key.IdProveedor;


                    // Crear una Orden de Compra POR CADA grupo
                    var nuevaOrden = new OrdenCompra
                    {
                        FechaCreacion = fechaDeEmision,// Fecha de creación
                        IdProveedor = idProveedorActual, // Id del proveedor
                        TotalEstimado = grupo.Sum(d => d.Subtotal),// Total estimado
                        Tipo = grupo.Key.TipoIntencion,// Tipo de orden
                        Estado = "PENDIENTE",// Estado inicial
                        Detalles = grupo.ToList()// Detalles de la orden
                    };

                    int idOrdenGenerada = _comprasService.CrearOrdenDeCompra(nuevaOrden);// Crear la orden y obtener su ID

                    // idProveedorActual
                    if (!idsOrdenesPorProveedor.ContainsKey(idProveedorActual))
                    {
                        idsOrdenesPorProveedor[idProveedorActual] = new List<int>();// Inicializar lista si no existe
                    }
                    idsOrdenesPorProveedor[idProveedorActual].Add(idOrdenGenerada);// Agregar ID de la orden a la lista del proveedor
                }

                
                int documentosGenerados = 0;// Contador de documentos PDF generados
                foreach (var kvp in idsOrdenesPorProveedor)// Iterar sobre cada proveedor y sus órdenes
                {
                    var nombreProveedor = _detallesOrdenActual.First(d => d.IdProveedor == kvp.Key).ProveedorNombre;// Obtener el nombre del proveedor

                    // Llama al nuevo método para generar UN PDF por proveedor
                    ExportarOrdenesConsolidadasPDF(kvp.Value, nombreProveedor);
                    documentosGenerados++;// Incrementar contador de documentos generados
                }

                // Mostrar mensaje de éxito y limpiar
                MessageBox.Show(
                    $"¡Éxito! Se generaron {gruposPorProveedorYTipo.Count} órdenes internas, consolidadas en {documentosGenerados} documentos PDF.\n" +
                    $"Puede verlos en la pestaña 'Órdenes Pendientes'.",
                    "Órdenes Creadas",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                _detallesOrdenActual.Clear();// Limpiar detalles de la orden actual
                CalcularTotalOrden();// Recalcular total de la orden
                CargarNecesidadesRepuestos();// Recargar necesidades de repuestos
                tabControl.SelectedTab = tabOrdenesPendientes;// Cambiar a la pestaña de órdenes pendientes
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear las Órdenes de Compra: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -----------------------------------------------------
        // MÉTODOS DE PENDIENTES E HISTORIAL
        // -----------------------------------------------------
        // Método para exportar órdenes consolidadas a PDF
        private string ExportarOrdenesConsolidadasPDF(List<int> idsOrdenes, string proveedorNombre)
        {
            if (!idsOrdenes.Any()) return null;// Validar lista vacía

            try
            {
                // Obtener todos los detalles y el total
                var detallesConsolidados = new List<DetalleOrdenCompra>();
                decimal totalConsolidado = 0;
                DateTime fechaMasReciente = DateTime.MinValue;

                // Variable para almacenar el nombre del proveedor
                string nombreProveedorReal = proveedorNombre;

                foreach (int id in idsOrdenes)// Iterar sobre cada ID de orden
                {
                    var orden = _comprasService.GetOrdenConDetalles(id);// Obtener la orden con detalles
                    if (orden == null) continue;// Validar orden nula
                    // Actualizar el nombre del proveedor si es necesario
                    if (nombreProveedorReal == "[SELECCIONAR]" && !string.IsNullOrEmpty(orden.ProveedorNombre))
                    {
                        nombreProveedorReal = orden.ProveedorNombre;// Actualizar nombre del proveedor real
                    }
                    // Agregar detalles y actualizar totales
                    detallesConsolidados.AddRange(orden.Detalles);
                    totalConsolidado += orden.TotalEstimado;// Sumar al total consolidado
                    if (orden.FechaCreacion > fechaMasReciente)// Actualizar fecha más reciente
                    {
                        fechaMasReciente = orden.FechaCreacion;// Actualizar fecha más reciente
                    }
                }

                if (!detallesConsolidados.Any()) return null;// Validar detalles vacíos

                // Configuración del archivo y ruta
                string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");// Carpeta de descargas del usuario
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);// Crear carpeta si no existe

                string nombreProveedorLimpio = string.Join("_", nombreProveedorReal.Split(Path.GetInvalidFileNameChars()));// Limpiar nombre del proveedor para el archivo
                string filePath = Path.Combine(folderPath, $"OC_CONSOLIDA_{nombreProveedorLimpio}_{fechaMasReciente:yyyyMMddHHmmss}.pdf");// Ruta completa del archivo PDF

                // Generación del PDF 
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    Document doc = new Document(PageSize.A4);// Crear documento A4
                    PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    // Fuentes
                    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font fontTitulo = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font fontHeader = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                    iTextSharp.text.Font fontTexto = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.Font fontTextoBold = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD);

                    // Título
                    Paragraph titulo = new Paragraph($"Orden(es) de Compra Consolidada - Proveedor: {nombreProveedorReal}", fontTitulo);
                    titulo.Alignment = Element.ALIGN_CENTER;
                    doc.Add(titulo);
                    doc.Add(Chunk.NEWLINE);

                    // Información de Órdenes consolidadas
                    doc.Add(new Paragraph($"Fechas de Emisión: {fechaMasReciente:dd/MM/yyyy}", fontTexto));
                    doc.Add(new Paragraph($"Incluye Órdenes: {string.Join(", ", idsOrdenes)}", fontTexto));
                    doc.Add(Chunk.NEWLINE);
                    // Tabla de Detalles
                    PdfPTable table = new PdfPTable(4);
                    table.WidthPercentage = 100;
                    // Definir anchos de columnas
                    float[] anchos = { 0.5f, 0.15f, 0.15f, 0.2f };
                    table.SetWidths(anchos);

                    // Encabezados de Tabla
                    BaseColor headerColor = new BaseColor(30, 30, 30);
                    AñadirCelda(table, "Artículo", fontHeader, Element.ALIGN_CENTER, headerColor);
                    AñadirCelda(table, "Cantidad", fontHeader, Element.ALIGN_CENTER, headerColor);
                    AñadirCelda(table, "P. Unitario", fontHeader, Element.ALIGN_RIGHT, headerColor);
                    AñadirCelda(table, "Subtotal", fontHeader, Element.ALIGN_RIGHT, headerColor);
                    // Agrupar detalles por artículo para consolidar cantidades y subtotales
                    var detallesAgrupados = detallesConsolidados
                        .GroupBy(det => new { det.IdArticulo, det.ArticuloNombre, det.PrecioUnitarioEstimado })
                        .Select(g => new
                        {
                            g.Key.ArticuloNombre,
                            CantidadTotal = g.Sum(d => d.CantidadSolicitada),
                            g.Key.PrecioUnitarioEstimado,
                            SubtotalTotal = g.Sum(d => d.Subtotal) 
                        });

                    // Filas de Detalles 
                    foreach (var det in detallesAgrupados)
                    {
                        AñadirCelda(table, det.ArticuloNombre, fontTexto, Element.ALIGN_LEFT);
                        AñadirCelda(table, det.CantidadTotal.ToString(), fontTexto, Element.ALIGN_CENTER);
                        AñadirCelda(table, det.PrecioUnitarioEstimado.ToString("C2"), fontTexto, Element.ALIGN_RIGHT);
                        AñadirCelda(table, det.SubtotalTotal.ToString("C2"), fontTexto, Element.ALIGN_RIGHT);
                        ; 
                    }
                    // Agregar tabla al documento
                    doc.Add(table);
                    doc.Add(Chunk.NEWLINE);

                    // Total
                    Paragraph total = new Paragraph($"TOTAL CONSOLIDADO: {totalConsolidado:C2}", fontTextoBold);
                    total.Alignment = Element.ALIGN_RIGHT;
                    doc.Add(total);

                    doc.Close();
                }

                MessageBox.Show($"Órdenes consolidadas exportadas con éxito a:\n{filePath}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });

                return filePath;// Retornar la ruta del archivo generado
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el PDF consolidado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        // Evento al cambiar de pestaña
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabOrdenesPendientes)
            {
                CargarOrdenesPendientes();//  Cargar órdenes pendientes



                CargarNecesidadesRepuestos();// Recargar necesidades de repuestos
            }

            else if (tabControl.SelectedTab == tabHistorial)
            {
                // Cargar historial con filtros por defecto
                btnBuscarHistorial_Click(null, null);
            }
        }
        // Manejo de eventos en la grilla de órdenes pendientes
        private void dgvOrdenesPendientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;// Validar índice de fila

            int idOrden = Convert.ToInt32(dgvOrdenesPendientes.Rows[e.RowIndex].Cells["IdOrdenCompra"].Value);
            if (dgvOrdenesPendientes.Columns[e.ColumnIndex].Name == "ExportarPDF")
            {
                ExportarOrdenPDF(idOrden, abrirPdf: true);// Llamar al método para exportar a PDF
            }

            if (dgvOrdenesPendientes.Columns[e.ColumnIndex].Name == "RegistrarIngreso")// Si se hace clic en el botón "Registrar Ingreso"
            {
                var frmRecepcion = new FrmRecepcionCompra(idOrden);// Crear instancia del formulario de recepción
                frmRecepcion.ShowDialog();
                CargarOrdenesPendientes();// Recargar órdenes pendientes
                CargarNecesidadesRepuestos();// Recargar necesidades de repuestos
            }
        }
        // Método para exportar una orden de compra a PDF
        private string ExportarOrdenPDF(int idOrden, bool abrirPdf = true)
        {
            try
            {
                var orden = _comprasService.GetOrdenConDetalles(idOrden);// Obtener la orden con detalles
                if (orden == null) throw new Exception($"No se encontró la Orden N°{idOrden}.");// Validar orden nula

                // Ruta a la carpeta de Descargas del usuario
                string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                // Limpiar el nombre del proveedor para usarlo en el nombre del archivo
                string nombreProveedorLimpio = string.Join("_", orden.ProveedorNombre.Split(Path.GetInvalidFileNameChars()));
                string filePath = Path.Combine(folderPath, $"OC_{idOrden}_{nombreProveedorLimpio}.pdf");
                // Generación del PDF
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    // Crear documento PDF
                    Document doc = new Document(PageSize.A4);
                    PdfWriter.GetInstance(doc, fs);
                    doc.Open();
                    // Definir  fuentes
                    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    // Definir fuentes
                    iTextSharp.text.Font fontTitulo = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font fontHeader = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                    iTextSharp.text.Font fontTexto = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.Font fontTextoBold = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD);
                    // Título
                    Paragraph titulo = new Paragraph($"Orden de Compra N° {orden.IdOrdenCompra}", fontTitulo);
                    titulo.Alignment = Element.ALIGN_CENTER;
                    doc.Add(titulo);
                    doc.Add(Chunk.NEWLINE);

                    //  Datos del Proveedor y Fecha
                    doc.Add(new Paragraph($"Proveedor: {orden.ProveedorNombre}", fontTexto));
                    doc.Add(new Paragraph($"Fecha de Emisión: {orden.FechaCreacion:dd/MM/yyyy}", fontTexto));
                    doc.Add(Chunk.NEWLINE);

                    // Crear Tabla de Detalles
                    PdfPTable table = new PdfPTable(5); 
                    table.WidthPercentage = 100;
                    float[] anchos = { 0.1f, 0.4f, 0.15f, 0.15f, 0.2f };
                    table.SetWidths(anchos);
                    // Encabezados de Tabla
                    BaseColor headerColor = new BaseColor(30, 30, 30); 
                    AñadirCelda(table, "ID Art.", fontHeader, Element.ALIGN_CENTER, headerColor);
                    AñadirCelda(table, "Artículo", fontHeader, Element.ALIGN_CENTER, headerColor);
                    AñadirCelda(table, "Cantidad", fontHeader, Element.ALIGN_CENTER, headerColor);
                    AñadirCelda(table, "P. Unitario", fontHeader, Element.ALIGN_RIGHT, headerColor);
                    AñadirCelda(table, "Subtotal", fontHeader, Element.ALIGN_RIGHT, headerColor);

                    // Filas de Detalles
                    foreach (var det in orden.Detalles)
                    {
                        AñadirCelda(table, det.IdArticulo.ToString(), fontTexto, Element.ALIGN_CENTER);
                        AñadirCelda(table, det.ArticuloNombre, fontTexto, Element.ALIGN_LEFT);
                        AñadirCelda(table, det.CantidadSolicitada.ToString(), fontTexto, Element.ALIGN_CENTER);
                        AñadirCelda(table, det.PrecioUnitarioEstimado.ToString("C2"), fontTexto, Element.ALIGN_RIGHT);
                        // Calculamos el subtotal del detalle
                        decimal subtotalDetalle = det.CantidadSolicitada * det.PrecioUnitarioEstimado;
                        AñadirCelda(table, subtotalDetalle.ToString("C2"), fontTexto, Element.ALIGN_RIGHT);
                    }

                    // Añadir la tabla al documento
                    doc.Add(table);
                    doc.Add(Chunk.NEWLINE);

                    // Total
                    Paragraph total = new Paragraph($"Total Estimado: {orden.TotalEstimado:C2}", fontTextoBold);
                    total.Alignment = Element.ALIGN_RIGHT;
                    doc.Add(total);


                    doc.Close(); 
                }

                if (abrirPdf)
                {
                    MessageBox.Show($"Orden de Compra N°{idOrden} exportada con éxito a:\n{filePath}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                }

                return filePath; 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null; 
            }
        }
        // Método auxiliar para añadir celdas a la tabla PDF
        private void AñadirCelda(iTextSharp.text.pdf.PdfPTable table, string text, iTextSharp.text.Font font, int align, iTextSharp.text.BaseColor? bgColor = null)
        {
            iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(text, font))// Crear celda
            {
                HorizontalAlignment = align,// Alineación horizontal
                VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE,// Alineación vertical
                Padding = 5f,// Padding
                BackgroundColor = bgColor ?? iTextSharp.text.BaseColor.WHITE// Color de fondo opcional
            };
            table.AddCell(cell);// Añadir celda a la tabla
        }
        /// <summary>
        /// Lógica para buscar en el historial de compras
        /// </summary>
        
        // Lógica para buscar en el historial de compras
        private void btnBuscarHistorial_Click(object sender, EventArgs e)
        {
            try
            {
                //Obtener Filtros
                string tipoFiltro = cmbFiltroTipoHistorial.SelectedItem.ToString();

                // Convertir "Todos" a cadena vacía para el servicio
                string tipo = (tipoFiltro == "Todos") ? "" : tipoFiltro.ToUpper();

                // Fechas
                DateTime? desde = dtpDesdeHistorial.Value.Date;
                DateTime? hasta = dtpHastaHistorial.Value.Date;

                // Llamar al servicio y asignar resultados
                dgvHistorial.DataSource = _comprasService.ListarHistorialCompras(tipo, desde, hasta);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar historial: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Lógica para buscar repuestos faltantes
        /// </summary>
        
        // Lógica para buscar repuestos faltantes
        private void btnBuscarFaltantes_Click(object sender, EventArgs e)
        {
            CargarNecesidadesRepuestos();
        }
        // Manejo de edición de celdas en la grilla de la orden de stock
        private void dgvOrdenStock_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var item = dgvOrdenStock.Rows[e.RowIndex].DataBoundItem as DetalleOrdenCompra;// Obtener el ítem editado
            if (item == null) return;// Validar ítem nulo

            var currentCulture = System.Globalization.CultureInfo.CurrentCulture;// Obtener cultura actual para formato de moneda
            // Manejar edición según la columna
            try
            {
                if (dgvOrdenStock.Columns[e.ColumnIndex].Name == "CantidadCol")// Si se editó la columna de cantidad
                {
                    if (int.TryParse(Convert.ToString(dgvOrdenStock.Rows[e.RowIndex].Cells["CantidadCol"].Value), out int nuevaCantidad) && nuevaCantidad > 0)
                    {
                        item.CantidadSolicitada = nuevaCantidad;
                    }
                    else
                    {
                        MessageBox.Show("Cantidad debe ser un número entero positivo.", "Error de Cantidad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // Revertir el valor en la celda
                        dgvOrdenStock.Rows[e.RowIndex].Cells["CantidadCol"].Value = item.CantidadSolicitada;
                    }
                }
                else if (dgvOrdenStock.Columns[e.ColumnIndex].Name == "PrecioUnitarioCol")// Si se editó la columna de precio unitario
                {
                    string valorCelda = Convert.ToString(dgvOrdenStock.Rows[e.RowIndex].Cells["PrecioUnitarioCol"].Value);// Obtener valor de la celda

                    if (decimal.TryParse(valorCelda, System.Globalization.NumberStyles.Currency,// Intentar parsear el valor como moneda
                                         currentCulture, out decimal nuevoPrecio) && nuevoPrecio >= 0)
                    {
                        
                        if (item.IdProveedor <= 0)// Validar que haya un proveedor seleccionado
                        {
                            MessageBox.Show("Debe seleccionar un proveedor ANTES de modificar el precio.", "Acción Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            // Revertimos el precio al estimado (el MAX)
                            item.PrecioUnitarioEstimado = _articuloService.ObtenerArticuloPorId(item.IdArticulo).PrecioCosto;
                            dgvOrdenStock.Rows[e.RowIndex].Cells["PrecioUnitarioCol"].Value = item.PrecioUnitarioEstimado.ToString("C2", currentCulture);
                        }
                        else
                        {
                           
                            item.PrecioUnitarioEstimado = nuevoPrecio;// Actualizar precio en memoria
                            _comprasService.ActualizarPrecioCostoProveedor(item.IdArticulo, item.IdProveedor, nuevoPrecio);// Actualizar precio en la base de datos
                        }
                    }
                    else
                    {
                        MessageBox.Show("El precio ingresado no es un formato de moneda válido.", "Error de Precio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // Revertir
                        dgvOrdenStock.Rows[e.RowIndex].Cells["PrecioUnitarioCol"].Value = item.PrecioUnitarioEstimado.ToString("C2", currentCulture);// Revertir el valor en la celda
                    }
                }

                // Forzar el recálculo total y refresco de la fila
                CalcularTotalOrden();
                // Refrescar solo la fila actual para que el SubtotalCol muestre el nuevo valor
                dgvOrdenStock.InvalidateRow(e.RowIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la celda: {ex.Message}", "Error de Edición", MessageBoxButtons.OK, MessageBoxIcon.Error);
             

            }
        }
        // Manejo de la aparición del control de edición en la celda Proveedor
        private void dgvOrdenStock_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Verificamos si estamos en la columna ComboBox de Proveedor
            if (dgvOrdenStock.CurrentCell.ColumnIndex == dgvOrdenStock.Columns["ProveedorCol"].Index && e.Control is ComboBox cmbProveedorControl)
            {
                // Obtenemos el IdArticulo de la fila actual
                var detalle = dgvOrdenStock.Rows[dgvOrdenStock.CurrentRow.Index].DataBoundItem as DetalleOrdenCompra;
                if (detalle == null) return;

                try
                {
                    // Cargamos los proveedores asociados al artículo
                    var articulo = _articuloService.ObtenerArticuloPorId(detalle.IdArticulo);
                    var todosProveedores = _proveedorService.ListarProveedores() ?? new List<Proveedor>();// Obtener todos los proveedores

                    var idsProveedoresAsignados = articulo.ProveedoresConCosto// Obtener IDs de proveedores asignados al artículo
                        .Select(pc => pc.IdProveedor)
                        .ToList();

                    // Filtrar solo los proveedores asignados al artículo
                    var proveedoresDelArticulo = todosProveedores
                        .Where(p => idsProveedoresAsignados.Contains(p.IdProveedor))
                        .ToList();
                    // Agregar opción de selección si no hay proveedor asignado
                    if (detalle.IdProveedor == 0)
                    {
                        proveedoresDelArticulo.Insert(0, new Proveedor { IdProveedor = 0, RazonSocial = "[Seleccionar Proveedor]" });
                    }


                    // Configuramos el ComboBox con los proveedores filtrados
                    cmbProveedorControl.DataSource = null;
                    cmbProveedorControl.DataSource = proveedoresDelArticulo;
                    cmbProveedorControl.DisplayMember = "RazonSocial";
                    cmbProveedorControl.ValueMember = "IdProveedor";

                    // Aseguramos que el valor actual esté seleccionado
                    cmbProveedorControl.SelectedValue = detalle.IdProveedor;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar proveedores para el artículo: " + ex.Message);
                }
            }
        }
        // Manejo del cambio de valor en la celda Proveedor
        private void dgvOrdenStock_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
       
            if (e.RowIndex < 0 || e.ColumnIndex != dgvOrdenStock.Columns["ProveedorCol"].Index) return;// Validar índice y columna

            // 2. Obtener el detalle (el objeto) de la fila que cambió
            var detalle = dgvOrdenStock.Rows[e.RowIndex].DataBoundItem as DetalleOrdenCompra;
            if (detalle == null) return;
            // 3. Obtener el valor seleccionado en la celda Proveedor
            this.BeginInvoke(new Action(() =>
            {
                try
                {
                    // Obtener el valor de la celda Proveedor
                    var valorCelda = dgvOrdenStock.Rows[e.RowIndex].Cells["ProveedorCol"].Value;
                    if (valorCelda == null || valorCelda == DBNull.Value) return;
                    // Convertir el valor al IdProveedor
                    var idProveedorSeleccionado = Convert.ToInt32(valorCelda);
                    // 4. Si es válido, obtener el precio de coste para ese proveedor y artículo
                    if (idProveedorSeleccionado > 0)
                    {
                        // Pedimos el precioCoste a la BD
                        decimal precioCoste = _articuloService.ObtenerPrecioCosto(detalle.IdArticulo, idProveedorSeleccionado);

                        // Obtener el proveedor seleccionado para mostrar su nombre
                        var proveedorSeleccionado = _proveedorService.ListarProveedores()
                                                    .FirstOrDefault(p => p.IdProveedor == idProveedorSeleccionado);
                        // 5. Actualizar el detalle en memoria y la celda de PrecioUnitarioCol
                        detalle.IdProveedor = idProveedorSeleccionado;
                        detalle.ProveedorNombre = proveedorSeleccionado?.RazonSocial ?? "N/A";

                       // ACTUALIZAMOS EL PRECIO EN MEMORIA Y EN LA CELDA
                        detalle.PrecioUnitarioEstimado = precioCoste;
                        dgvOrdenStock.Rows[e.RowIndex].Cells["PrecioUnitarioCol"].Value = precioCoste.ToString("F2");

                        // Recalculamos el total
                        CalcularTotalOrden();
                        dgvOrdenStock.InvalidateRow(e.RowIndex);
                    }
                    else
                    {
                        // Si se seleccionó "[Seleccionar Proveedor]", reiniciamos valores
                        detalle.IdProveedor = 0;
                        detalle.ProveedorNombre = "[Seleccionar Proveedor]";
                        detalle.PrecioUnitarioEstimado = 0;
                        dgvOrdenStock.Rows[e.RowIndex].Cells["PrecioUnitarioCol"].Value = (0m).ToString("F2");
                        CalcularTotalOrden();
                        dgvOrdenStock.InvalidateRow(e.RowIndex);// Recalcular total y refrescar fila
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar precio del proveedor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }));
        }
        // Manejo de errores en la grilla de la orden de stock
        private void dgvOrdenStock_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.ColumnIndex == dgvOrdenStock.Columns["ProveedorCol"].Index)// Si el error es en la columna Proveedor
            {
                e.ThrowException = false;// Evitar que se lance una excepción

                e.Cancel = true;// Cancelar la edición actual

            }
        }
    }
}