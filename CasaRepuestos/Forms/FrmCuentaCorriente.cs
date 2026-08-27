using CasaRepuestos.Models;
using CasaRepuestos.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.Data;
using System.Diagnostics;


namespace CasaRepuestos.Forms
{
    public partial class FrmCuentaCorriente : Form
    {
        // SERVICIOS
        private ClienteService clienteService = new ClienteService();
        private ArticuloService articuloService = new ArticuloService();
        private ServiciosService servicioService = new ServiciosService();
        private CuentaCorrienteService cuentaCorrienteService = new CuentaCorrienteService();
        private ArqueoService arqueoService = new ArqueoService();
        private FacturaService facturaService = new FacturaService();

        // LISTAS Y OBJETOS
        private List<Cliente> listaClientesConCuentaCte;
        private Cliente clienteSeleccionado;
        
        // Acumulador del total a pagar
        private decimal totalSaldoaPagar = 0m;

        // Conjunto para almacenar los ID de factura ya seleccionados
        private HashSet<int> facturasSeleccionadasParaPagar = new HashSet<int>();
        public FrmCuentaCorriente()
        {
            InitializeComponent();
            // Cargar la lista de clientes con cuenta corriente
            CargarClientesConCuentaCte();
            // Cargar métodos de pago
            CargarMetodosDePago();
        }

        /************************
         * MÉTODO DE PAGO 
         ************************/
        private void CargarMetodosDePago()
        {
            comboBoxMetPago.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxMetPago.Items.Clear();
            comboBoxMetPago.Items.AddRange(new object[]
            {
                "EFECTIVO",
                "TARJETA",
                "TRANSFERENCIA",
                "BILLETERA VIRTUAL"
            });
            if (comboBoxMetPago.Items.Count > 0) comboBoxMetPago.SelectedIndex = 0;
        }

        // Carga la lista de clientes con cuenta corriente y la muestra en el dataGridViewCuentasClientes
        private void CargarClientesConCuentaCte()
        {
            listaClientesConCuentaCte = clienteService.ListarCuentasCorrientes();

            dataGridViewCuentasClientes.DataSource = listaClientesConCuentaCte;

            FiltrarClienteConCtaCte(textBoxClienteConCtaCte.Text);
        }

        // Metodo para filtrar por el nombre del cliente, apellido o DNI
        private void FiltrarClienteConCtaCte(string filtro)
        {
            // Convertir el filtro a minúsculas
            string filtroLower = filtro.ToLower().Trim();

            // Aplicar condicion de filtro sobre la lista original
            var listaFiltrada = listaClientesConCuentaCte
                .Where(x =>
                    // Filtrar por Apellido, Nombre o CUIL
                    x.DatosPersona.Apellido.ToLower().Contains(filtroLower) ||
                    x.DatosPersona.Nombre.ToLower().Contains(filtroLower) ||
                    x.Cuil.Contains(filtroLower)
                )
                // Mapear solo los clientes que cumplen el criterio de filtrado
                .Select(x => new
                {
                    x.IdCliente,
                    Cliente = $"{x.DatosPersona.Apellido}, {x.DatosPersona.Nombre}",
                    x.Cuil,
                    x.Categoria,
                    Saldo = x.DatosCuentaCorriente.SaldoActual
                })
                .ToList();

            // Asignar la lista filtrada al dataGridViewCuentasClientes
            dataGridViewCuentasClientes.DataSource = listaFiltrada;

            if (dataGridViewCuentasClientes.Columns.Contains("IdCliente"))
            {
                dataGridViewCuentasClientes.Columns["IdCliente"].Visible = false;
            }
        }

        // Evento change para Filtrar
        private void textBoxClienteConCtaCte_TextChanged(object sender, EventArgs e)
        {
            FiltrarClienteConCtaCte(textBoxClienteConCtaCte.Text);
        }

        /******************************************************************************
        * SELECCIONO UN CLIENTE PARA VER SUS FACTURAS PENDIENTES EN LA CUENTA CORRIENTE 
        ******************************************************************************/
        private void dataGridViewCuentasClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var fila = dataGridViewCuentasClientes.Rows[e.RowIndex];
            int idCliente = Convert.ToInt32(fila.Cells["IdCliente"].Value);

            clienteSeleccionado = listaClientesConCuentaCte.FirstOrDefault(c => c.IdCliente == idCliente);

            if (clienteSeleccionado == null)
            {
                MessageBox.Show("No se encontró el cliente seleccionado");
                return;
            }
            // Si obtengo resultado, cargo los datos en los textbox correspondientes
            textBoxNombreCompleto.Text = clienteSeleccionado.NombreCompleto;
            textBoxSaldoActual.Text = clienteSeleccionado.DatosCuentaCorriente.SaldoActual.ToString("C2");

            totalSaldoaPagar = 0;
            facturasSeleccionadasParaPagar.Clear();
            textBoxSaldoaPagar.Text = totalSaldoaPagar.ToString("C2");

            // Cargar las facturas pendientes en el dataGridViewFacturasPendientes
            var facturasPendientes = cuentaCorrienteService
                    .ListarFacturasPendientesDeCliente(clienteSeleccionado.DatosCuentaCorriente.IdCuentaCorriente);

            // Calcular el saldo pendiente de cada factura antes de mostrarla
            foreach (var factura in facturasPendientes)
            {
                factura.SaldoPendiente = cuentaCorrienteService.ObtenerSaldoPendienteFactura(factura.ID_Factura);
            }
            // Cargar correctamente en el DataGridView
            dataGridViewFacturasPendientes.DataSource = null;
            dataGridViewFacturasPendientes.Columns.Clear();
            dataGridViewFacturasPendientes.AutoGenerateColumns = false;
            dataGridViewFacturasPendientes.DataSource = facturasPendientes;
            // Valido si las columnas ya existen para no duplicarlas
            if (dataGridViewFacturasPendientes.Columns.Count == 0)
            {
                dataGridViewFacturasPendientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ID_Factura",
                    HeaderText = "Nº Comprobante",
                    Name = "ID_Factura",
                    ReadOnly = true
                });

                dataGridViewFacturasPendientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Fecha",
                    HeaderText = "Fecha",
                    Name = "Fecha",
                    ReadOnly = true
                });

                dataGridViewFacturasPendientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Tipo_Factura",
                    HeaderText = "Tipo",
                    Name = "Tipo_Factura",
                    ReadOnly = true
                });

                dataGridViewFacturasPendientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Total_Factura",
                    HeaderText = "Total factura",
                    Name = "Total_Factura",
                    DefaultCellStyle = { Format = "C2" },
                    ReadOnly = true
                });

                dataGridViewFacturasPendientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "SaldoPendiente",
                    HeaderText = "Saldo Pendiente",
                    Name = "SaldoPendiente",
                    DefaultCellStyle = { Format = "C2" },
                    ReadOnly = true
                });

                var btnPagar = new DataGridViewButtonColumn
                {
                    Name = "btnPagar",
                    HeaderText = "Acciones",
                    Text = "Pagar",
                    UseColumnTextForButtonValue = true

                };
                dataGridViewFacturasPendientes.Columns.Add(btnPagar);

                var btnDetalles = new DataGridViewButtonColumn
                {
                    Name = "btnDetalles",
                    HeaderText = "",
                    Text = "Ver Detalles",
                    UseColumnTextForButtonValue = true
                };
                dataGridViewFacturasPendientes.Columns.Add(btnDetalles);
            }
            // Limpiar selección previa
            dataGridViewFacturasPendientes.ClearSelection();
        }

        // EVENTO CLICK EN CELDA DEL DATAGRIDVIEW DE FACTURAS PENDIENTES
        private void dataGridViewFacturasPendientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (e.ColumnIndex >= dataGridViewFacturasPendientes.Columns.Count) return;

            var columna = dataGridViewFacturasPendientes.Columns[e.ColumnIndex];
            if (columna is not DataGridViewButtonColumn) return;

            Facturas? facturaSeleccionada = dataGridViewFacturasPendientes.Rows[e.RowIndex].DataBoundItem as Facturas;
            if (facturaSeleccionada == null) return;

            if (columna.Name == "btnPagar")
            {
                FacturaAPagar(facturaSeleccionada, dataGridViewFacturasPendientes.Rows[e.RowIndex]);
            }
            else if (columna.Name == "btnDetalles")
            {
                MostrarDetallesFactura(facturaSeleccionada);
            }
        }
        // Suma la factura seleccionada al total a pagar y la marca visualmente en el DataGridView
        private void FacturaAPagar(Facturas facturaSeleccionada, DataGridViewRow fila)
        {// Verificar si la factura ya está seleccionada
            if (facturasSeleccionadasParaPagar.Contains(facturaSeleccionada.ID_Factura))
            {
                //DESELECCIONAR PAGAR   
                facturasSeleccionadasParaPagar.Remove(facturaSeleccionada.ID_Factura);
                // Restar del total a pagar
                totalSaldoaPagar -= facturaSeleccionada.SaldoPendiente;

                // Restaurar la apariencia visual
                fila.DefaultCellStyle.BackColor = Color.White;
                fila.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                ((DataGridViewButtonCell)fila.Cells["btnPagar"]).Value = "Pagar";
            }
            else
            {
                // SELECCIONAR PAGAR
                facturasSeleccionadasParaPagar.Add(facturaSeleccionada.ID_Factura);

                // Sumar al total a pagar
                totalSaldoaPagar += facturaSeleccionada.SaldoPendiente;
                // Aplicar la marca visual
                fila.DefaultCellStyle.BackColor = Color.LightGray;
                fila.DefaultCellStyle.SelectionBackColor = Color.Silver;
                ((DataGridViewButtonCell)fila.Cells["btnPagar"]).Value = "Seleccionada";
            }
            // Actualizar el saldo final una sola vez
            textBoxSaldoaPagar.Text = totalSaldoaPagar.ToString("C2");
        }

        // Metodo para mostrar los detalles de una factura en un formulario aparte
        private void MostrarDetallesFactura(Facturas factura)
        {
            try
            {
                // Obtención de datos de detalles factura
                var detallesFactura = facturaService.ListarDetalles(factura.ID_Factura);
                // Obtención de datos de detalles reparación si aplica
                List<DetallePresupuesto> detallesReparacion = new();
                // Si la factura tiene un presupuesto asociado, obtener sus detalles
                if (factura.ID_Presupuesto > 0)
                {
                    detallesReparacion = facturaService.ObtenerDetallesReparacion((int)factura.ID_Presupuesto);
                }

                // Unir todos los detalles en la nueva clase auxiliar DetalleFacturaAMostrar
                var listaFinal = new List<DetalleFacturaAMostrar>();

                // Artículos de la factura
                listaFinal.AddRange(detallesFactura.Select(d =>
                {
                    // OBTENER EL NOMBRE DEL ARTÍCULO
                    var articulo = articuloService.ObtenerArticuloPorId(d.ID_Articulo.Value);

                    return new DetalleFacturaAMostrar
                    {
                        Numero_Factura = factura.ID_Factura,
                        Articulo = d.ID_Articulo,
                        Cantidad = d.Cantidad,
                        Subtotal = d.subtotal,

                        Descripcion = articulo?.Nombre ?? $"Artículo [ID: {d.ID_Articulo}] no encontrado",
                        PrecioUnitario = (d.Cantidad > 0) ? d.subtotal / d.Cantidad : 0
                    };
                }).ToList());

                // Detalles de reparación 
                if (detallesReparacion.Any())
                {
                    listaFinal.AddRange(detallesReparacion.Select(d =>
                    {
                        string nombreItem = "";
                        // Determinar si es servicio o repuesto
                        if (d.IdServicio > 0)
                        {
                            // Obtener nombre del Servicio
                            var servicio = servicioService.ObtenerServicioPorId(d.IdServicio);
                            nombreItem = servicio?.Descripcion ?? $"Servicio [ID: {d.IdServicio}] no encontrado";
                        }
                        else if (d.IdArticulo > 0)
                        {
                            // Obtener nombre del Repuesto
                            var repuesto = articuloService.ObtenerArticuloPorId(d.IdArticulo.Value);
                            nombreItem = repuesto?.Nombre ?? $"Repuesto [ID: {d.IdArticulo}] no encontrado";
                        }
                        else
                        {
                            nombreItem = "Detalle de reparación desconocido";
                        }

                        return new DetalleFacturaAMostrar
                        {
                            Numero_Factura = factura.ID_Factura,
                            Articulo = d.IdArticulo,
                            Servicio = d.IdServicio,
                            Cantidad = d.Cantidad,
                            Subtotal = (d.PrecioServicio ?? 0) + (d.PrecioRepuesto ?? 0),
                            // Asignar el nombre del item
                            Descripcion = nombreItem,
                            PrecioUnitario = (d.Cantidad > 0)
                                            ? ((d.PrecioServicio ?? 0) + (d.PrecioRepuesto ?? 0)) / d.Cantidad
                                            : 0
                        };
                    }).ToList());
                }

                // Mostrar formulario 
                using var formDetalle = new DetalleFacturaForm(factura.ID_Factura, listaFinal);
                formDetalle.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar detalles de la factura:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /********************************************
         * GUARDAR MOVIMIENTO EN CUENTA CORRIENTE
         * ******************************************/
        private void btnGuardarMovCtaCte_Click(object sender, EventArgs e)
        {
            // Verificar si la caja está cerrada
            if (arqueoService.EsCajaDelDiaCerrada())
            {
                MessageBox.Show(
                    "No se pueden registrar pagos.\n\nLa caja del día ya ha sido cerrada.\nPara realizar transacciones, debe reabrir la caja desde el módulo de Arqueo.",
                    "Caja Cerrada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            //Verificar si no hay caja abierta
            if (!arqueoService.ExisteCajaAbiertaHoy())
            {
                MessageBox.Show(
                    "No se pueden registrar pagos.\n\nNo hay una caja abierta para el día de hoy.\nPor favor, abra la caja desde el módulo de Arqueo antes de realizar transacciones.",
                    "Caja No Abierta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (clienteSeleccionado == null)
            {
                MessageBox.Show("Debe seleccionar un cliente con cuenta corriente", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Limpia cualquier símbolo de moneda, separadores o espacios
            string textoMonto = textBoxSaldoaPagar.Text
                .Replace("$", "")
                .Replace(".", "")
                .Replace(",", ".")
                .Trim();

            // Validar que el monto sea un decimal positivo
            if (!decimal.TryParse(textoMonto, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out decimal montoDisponible)
                || montoDisponible <= 0)
            {
                MessageBox.Show("Ingrese un monto válido en 'Saldo a pagar'", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar que el monto no exceda el saldo actual del cliente
            if (comboBoxMetPago.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un método de pago", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Obtener método de pago y descripción
            string metodoPago = comboBoxMetPago.SelectedItem.ToString();
            string descripcion = textBoxDescripcionMetPago.Text.Trim();
            // Listar facturas pendientes ordenadas por fecha
            var facturasPendientes = cuentaCorrienteService
                .ListarFacturasPendientesDeCliente(clienteSeleccionado.DatosCuentaCorriente.IdCuentaCorriente)
                .OrderBy(f => f.Fecha)
                .ToList();

            if (facturasPendientes.Count == 0)
            {
                MessageBox.Show("El cliente no tiene facturas pendientes", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal montoRestante = montoDisponible;
            decimal totalPagado = 0m;
            // Recorrer las facturas pendientes y aplicar el pago
            foreach (var factura in facturasPendientes)
            {
                if (montoRestante <= 0) break;

                decimal saldoPendiente = cuentaCorrienteService.ObtenerSaldoPendienteFactura(factura.ID_Factura);
                if (saldoPendiente <= 0) continue;

                decimal pagoAplicado = Math.Min(montoRestante, saldoPendiente);

                // Registrar movimiento
                var mov = new MovimientoCuentaCorriente
                {
                    Id_Factura = factura.ID_Factura,
                    Id_Cuenta_Corriente = factura.ID_Cuenta_Corriente ?? clienteSeleccionado.DatosCuentaCorriente.IdCuentaCorriente,
                    Fecha = DateTime.Now,
                    Monto = pagoAplicado,
                    Metodo_Pago = metodoPago,
                    Descripcion_Metodo_Pago = descripcion,
                    Concepto = pagoAplicado == saldoPendiente ? "PAGO TOTAL" : "PAGO PARCIAL"
                };
                // Registrar el pago en la cuenta corriente
                cuentaCorrienteService.RegistrarPagoEnCuentaCorriente(mov);

                // Actualizar estado factura
                if (pagoAplicado == saldoPendiente)
                    facturaService.MarcarFacturaComoPagada(factura.ID_Factura);

                montoRestante -= pagoAplicado;
                totalPagado += pagoAplicado;
            }

            // Actualizar saldo cuenta corriente
            decimal nuevoSaldo = clienteSeleccionado.DatosCuentaCorriente.SaldoActual - totalPagado;
            if (nuevoSaldo < 0) nuevoSaldo = 0;
            cuentaCorrienteService.ActualizarSaldoCuentaCorriente(clienteSeleccionado.DatosCuentaCorriente.IdCuentaCorriente, nuevoSaldo);
            clienteSeleccionado.DatosCuentaCorriente.SaldoActual = nuevoSaldo;

            // Confirmación de pago registrado
            var resultado = MessageBox.Show(
                $"Pago registrado correctamente.\nTotal aplicado: {totalPagado:C2}\nSaldo actual: {nuevoSaldo:C2}" +
                $"\n\n¿Desea imprimir el comprobante?",
                "OPERACIÓN EXITOSA",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            // --- GENERAR PDF  ---
            if (resultado == DialogResult.Yes)
            {
                var factPend = cuentaCorrienteService
                    .ListarFacturasPendientesDeCliente(clienteSeleccionado.DatosCuentaCorriente.IdCuentaCorriente)
                    .ToList();

                // recalcular saldos actuales
                foreach (var f in factPend)
                    f.SaldoPendiente = cuentaCorrienteService.ObtenerSaldoPendienteFactura(f.ID_Factura);

                GenerarComprobanteEstadoCuenta(clienteSeleccionado, factPend);
            }


            // Actualizar interfaz
            textBoxSaldoActual.Text = nuevoSaldo.ToString("C2");
            textBoxSaldoaPagar.Text = "0";
            textBoxDescripcionMetPago.Clear();

            // -----------------------------------------------
            // REFRESCAR FACTURAS PENDIENTES 
            // -----------------------------------------------
            var facturasPendientesActualizadas = cuentaCorrienteService
                    .ListarFacturasPendientesDeCliente(clienteSeleccionado.DatosCuentaCorriente.IdCuentaCorriente);

            // Calcular el saldo pendiente de cada factura ANTES de mostrarlas
            foreach (var factura in facturasPendientesActualizadas)
            {
                factura.SaldoPendiente = cuentaCorrienteService.ObtenerSaldoPendienteFactura(factura.ID_Factura);
            }

            // Cargar correctamente en el DataGridView
            dataGridViewFacturasPendientes.DataSource = null;
            dataGridViewFacturasPendientes.Columns.Clear();
            dataGridViewFacturasPendientes.AutoGenerateColumns = false;
            dataGridViewFacturasPendientes.DataSource = facturasPendientesActualizadas;

            // Volver a crear las columnas
            if (dataGridViewFacturasPendientes.Columns.Count == 0)
            {
                dataGridViewFacturasPendientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ID_Factura",
                    HeaderText = "Nº Comprobante",
                    Name = "ID_Factura",
                    ReadOnly = true
                });

                dataGridViewFacturasPendientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Fecha",
                    HeaderText = "Fecha",
                    Name = "Fecha",
                    ReadOnly = true
                });

                dataGridViewFacturasPendientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Tipo_Factura",
                    HeaderText = "Tipo",
                    Name = "Tipo_Factura",
                    ReadOnly = true
                });

                dataGridViewFacturasPendientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Total_Factura",
                    HeaderText = "Total factura",
                    Name = "Total_Factura",
                    DefaultCellStyle = { Format = "C2" },
                    ReadOnly = true
                });

                dataGridViewFacturasPendientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "SaldoPendiente",
                    HeaderText = "Saldo Pendiente",
                    Name = "SaldoPendiente",
                    DefaultCellStyle = { Format = "C2" },
                    ReadOnly = true
                });

                var btnPagar = new DataGridViewButtonColumn
                {
                    Name = "btnPagar",
                    HeaderText = "Acciones",
                    Text = "Pagar",
                    UseColumnTextForButtonValue = true
                };
                dataGridViewFacturasPendientes.Columns.Add(btnPagar);

                var btnDetalles = new DataGridViewButtonColumn
                {
                    Name = "btnDetalles",
                    HeaderText = "",
                    Text = "Ver Detalles",
                    UseColumnTextForButtonValue = true
                };
                dataGridViewFacturasPendientes.Columns.Add(btnDetalles);
            }

            // Limpiar selección
            dataGridViewFacturasPendientes.ClearSelection();
        }
        // MÉTODO PARA GENERAR EL COMPROBANTE DE ESTADO DE CUENTA EN PDF
        private void GenerarComprobanteEstadoCuenta(Cliente cliente, List<Facturas> facturasPendientes)
        {
            if (cliente == null)
            {
                MessageBox.Show("Cliente no válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ---------- CONFIGURACIÓN DEL PDF ----------
            var pageSize = new iTextSharp.text.Rectangle(500f, 10000f);
            var doc = new Document(pageSize, 10f, 10f, 10f, 10f);

            string rutaDescargas = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string rutaArchivo = Path.Combine(
                rutaDescargas,
                "Downloads",
                $"EstadoCuenta_{cliente.IdCliente}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
            );

            try
            {
                using (PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(rutaArchivo, FileMode.Create)))
                {
                    doc.Open();

                    // ---------- FUENTES ----------
                    var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA, 13, iTextSharp.text.Font.BOLD);
                    var fontSubtitulo = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.BOLD);
                    var fontNormal = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL);
                    var fontHeader = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.BOLD);


                    var separator = new Chunk(new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -1f));

                    // ---------- CABECERA ----------
                    doc.Add(new Paragraph("ESTADO DE CUENTA CORRIENTE", fontTitulo) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new Paragraph($"Cliente: {cliente.NombreCompleto}", fontSubtitulo) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new Paragraph($"CUIL/CUIT: {cliente.Cuil}", fontNormal) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new Paragraph($"Fecha: {DateTime.Now:dd/MM/yyyy}", fontNormal) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(separator);

                    doc.Add(new Paragraph("\n"));

                    // ---------- TABLA IGUAL A DATAGRID ----------
                    PdfPTable tabla = new PdfPTable(5)
                    {
                        WidthPercentage = 100
                    };

                    tabla.SetWidths(new float[] { 1.5f, 2f, 1.2f, 1.5f, 1.5f });

                    BaseColor headerColor = new BaseColor(230, 230, 230);

                    // Encabezados 
                    tabla.AddCell(new PdfPCell(new Phrase("N° Comprobante", fontHeader)) { BackgroundColor = headerColor });
                    tabla.AddCell(new PdfPCell(new Phrase("Fecha", fontHeader)) { BackgroundColor = headerColor });
                    tabla.AddCell(new PdfPCell(new Phrase("Tipo", fontHeader)) { BackgroundColor = headerColor });
                    tabla.AddCell(new PdfPCell(new Phrase("Total", fontHeader)) { BackgroundColor = headerColor, HorizontalAlignment = Element.ALIGN_RIGHT });
                    tabla.AddCell(new PdfPCell(new Phrase("Saldo Pendiente", fontHeader)) { BackgroundColor = headerColor, HorizontalAlignment = Element.ALIGN_RIGHT });

                    // Filas
                    foreach (var f in facturasPendientes)
                    {
                        tabla.AddCell(new PdfPCell(new Phrase(f.ID_Factura.ToString(), fontNormal)));

                        tabla.AddCell(new PdfPCell(new Phrase(f.Fecha.ToString("dd/MM/yyyy"), fontNormal)));

                        tabla.AddCell(new PdfPCell(new Phrase(f.Tipo_Factura, fontNormal)));

                        tabla.AddCell(new PdfPCell(new Phrase(f.Total_Factura.ToString("C2"), fontNormal))
                        {
                            HorizontalAlignment = Element.ALIGN_RIGHT
                        });

                        tabla.AddCell(new PdfPCell(new Phrase(f.SaldoPendiente.ToString("C2"), fontNormal))
                        {
                            HorizontalAlignment = Element.ALIGN_RIGHT
                        });
                    }

                    doc.Add(tabla);

                    doc.Add(new Paragraph("\n"));
                    doc.Add(new Paragraph($"Saldo Total Pendiente: {cliente.DatosCuentaCorriente.SaldoActual:C2}", fontSubtitulo)
                    {
                        Alignment = Element.ALIGN_RIGHT
                    });

                    doc.Add(new Paragraph("\n"));
                    doc.Add(new Paragraph("Gracias por su pago", fontNormal) { Alignment = Element.ALIGN_CENTER });

                    doc.Close();
                }

                MessageBox.Show($"Comprobante generado en:\n{rutaArchivo}",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(new ProcessStartInfo(rutaArchivo) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar PDF:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
