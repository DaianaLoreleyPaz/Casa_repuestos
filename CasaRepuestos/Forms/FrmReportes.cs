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
    public partial class FrmReportes : Form
    {
        private ReportesService _reportesService;
        private TabControl tabControl;
        private TabPage tabCompras;
        private TabPage tabVentas;

        // Controles de Compras
        private DateTimePicker dtpComprasDesde;
        private DateTimePicker dtpComprasHasta;
        private Button btnGenerarCompras;
        private Button btnLimpiarCompras;
        private Label lblTotalCompras;
        private Label lblCantidadCompras;
        private Label lblPromedioCompras;
        private Panel panelGraficoCompras;
        private DataGridView dgvDetalleCompras;

        // Controles de Ventas
        private DateTimePicker dtpVentasDesde;
        private DateTimePicker dtpVentasHasta;
        private Button btnGenerarVentas;
        private Button btnLimpiarVentas;
        private Label lblTotalVentas;
        private Label lblCantidadVentas;
        private Label lblPromedioVentas;
        private Panel panelGraficoVentas;
        private Panel panelGraficoTipoCliente;
        private DataGridView dgvDetalleVentas;

        public FrmReportes()
        {
            InitializeComponent();
            _reportesService = new ReportesService();
            InicializarControles();
            
            // Inicializar columnas de los datagrids después de la creación de los controles
            InicializarColumnasDataGrids();
        }

        private void InicializarColumnasDataGrids()
        {
            // Inicializar columnas del datagrid de compras
            if (dgvDetalleCompras != null)
            {
                dgvDetalleCompras.Columns.Clear();
                dgvDetalleCompras.Columns.Add("IdOrden", "# Orden");
                dgvDetalleCompras.Columns.Add("Fecha", "Fecha");
                dgvDetalleCompras.Columns.Add("Proveedor", "Proveedor");
                dgvDetalleCompras.Columns.Add("Total", "Total");
                dgvDetalleCompras.Columns["Total"].DefaultCellStyle.Format = "C2";
                dgvDetalleCompras.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            // Inicializar columnas del datagrid de ventas
            if (dgvDetalleVentas != null)
            {
                dgvDetalleVentas.Columns.Clear();
                dgvDetalleVentas.Columns.Add("IdFactura", "# Op");
                dgvDetalleVentas.Columns.Add("Fecha", "Fecha");
                dgvDetalleVentas.Columns.Add("Cliente", "Descripción");  // Cambiado de TipoCliente a Cliente
                dgvDetalleVentas.Columns.Add("MetodoPago", "Método de Pago");
                dgvDetalleVentas.Columns.Add("TipoCliente", "Tipo Cliente");
                dgvDetalleVentas.Columns.Add("Total", "Total");
                dgvDetalleVentas.Columns["Total"].DefaultCellStyle.Format = "C2";
                dgvDetalleVentas.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }
        
        // Método para depurar la carga de datos en el datagrid de ventas
        private void DepurarCargaDatosVentas(List<DetalleVentaReporte> detalles)
        {
            // Este método es solo para depuración y puede ser eliminado después
            Console.WriteLine($"Cantidad de detalles de ventas: {detalles?.Count ?? 0}");
            if (detalles != null && detalles.Count > 0)
            {
                foreach (var detalle in detalles.Take(5)) // Solo los primeros 5 para no sobrecargar
                {
                    Console.WriteLine($"ID: {detalle.IdFactura}, Fecha: {detalle.Fecha}, Tipo: {detalle.TipoCliente}, Método: {detalle.MetodoPago}, Total: {detalle.Total}");
                }
            }
        }

        private void InicializarControles()
        {
            this.Text = "Reportes";
            this.Size = new Size(1200, 700);

            // TabControl principal
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F)
            };

            // Tab de Compras
            tabCompras = new TabPage("Reporte de Compras");
            ConfigurarTabCompras();

            // Tab de Ventas
            tabVentas = new TabPage("Reporte de Ventas e Ingresos");
            ConfigurarTabVentas();

            tabControl.TabPages.Add(tabCompras);
            tabControl.TabPages.Add(tabVentas);

            this.Controls.Add(tabControl);
        }

        private void ConfigurarTabCompras()
        {
            // Panel superior con filtros
            var panelFiltros = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            var lblDesde = new Label
            {
                Text = "Desde:",
                Location = new Point(20, 25),
                AutoSize = true
            };

            dtpComprasDesde = new DateTimePicker
            {
                Location = new Point(80, 22),
                Width = 150,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today.AddMonths(-1)
            };

            var lblHasta = new Label
            {
                Text = "Hasta:",
                Location = new Point(250, 25),
                AutoSize = true
            };

            dtpComprasHasta = new DateTimePicker
            {
                Location = new Point(310, 22),
                Width = 150,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };

            btnGenerarCompras = new Button
            {
                Text = "Generar Reporte",
                Location = new Point(480, 20),
                Size = new Size(130, 30),
                BackColor = Color.FromArgb(45, 66, 91),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGenerarCompras.Click += BtnGenerarCompras_Click;

            btnLimpiarCompras = new Button
            {
                Text = "Limpiar Filtro",
                Location = new Point(620, 20),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLimpiarCompras.Click += BtnLimpiarCompras_Click;

            panelFiltros.Controls.AddRange(new Control[] { lblDesde, dtpComprasDesde, lblHasta, dtpComprasHasta, btnGenerarCompras, btnLimpiarCompras });

            // Panel de resumen
            var panelResumen = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.White
            };

            lblTotalCompras = new Label
            {
                Text = "Total Compras: $0.00",
                Location = new Point(20, 20),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 66, 91),
                AutoSize = true
            };

            lblCantidadCompras = new Label
            {
                Text = "Cantidad de Operaciones: 0",
                Location = new Point(20, 50),
                Font = new Font("Segoe UI", 10F),
                AutoSize = true
            };

            lblPromedioCompras = new Label
            {
                Text = "Promedio por Operación: $0.00",
                Location = new Point(300, 50),
                Font = new Font("Segoe UI", 10F),
                AutoSize = true
            };

            panelResumen.Controls.AddRange(new Control[] { lblTotalCompras, lblCantidadCompras, lblPromedioCompras });

            // Panel para gráfico visual
            panelGraficoCompras = new Panel
            {
                Dock = DockStyle.Left,
                Width = 500,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            var lblTituloGrafico = new Label
            {
                Text = "Compras por Proveedor",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 66, 91),
                Location = new Point(20, 10),
                AutoSize = true
            };
            panelGraficoCompras.Controls.Add(lblTituloGrafico);

            // DataGridView para detalles
            dgvDetalleCompras = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Las columnas se inicializan en el método InicializarColumnasDataGrids

            var splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 300
            };

            splitContainer.Panel1.Controls.Add(dgvDetalleCompras);
            splitContainer.Panel1.Controls.Add(panelGraficoCompras);
            splitContainer.Panel2.Visible = false;

            tabCompras.Controls.Add(splitContainer);
            tabCompras.Controls.Add(panelResumen);
            tabCompras.Controls.Add(panelFiltros);
        }

        private void ConfigurarTabVentas()
        {
            // Panel superior con filtros
            var panelFiltros = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            var lblDesde = new Label
            {
                Text = "Desde:",
                Location = new Point(20, 25),
                AutoSize = true
            };

            dtpVentasDesde = new DateTimePicker
            {
                Location = new Point(80, 22),
                Width = 150,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today.AddMonths(-1)
            };

            var lblHasta = new Label
            {
                Text = "Hasta:",
                Location = new Point(250, 25),
                AutoSize = true
            };

            dtpVentasHasta = new DateTimePicker
            {
                Location = new Point(310, 22),
                Width = 150,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };

            btnGenerarVentas = new Button
            {
                Text = "Generar Reporte",
                Location = new Point(480, 20),
                Size = new Size(130, 30),
                BackColor = Color.FromArgb(45, 66, 91),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGenerarVentas.Click += BtnGenerarVentas_Click;

            btnLimpiarVentas = new Button
            {
                Text = "Limpiar Filtro",
                Location = new Point(620, 20),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLimpiarVentas.Click += BtnLimpiarVentas_Click;

            panelFiltros.Controls.AddRange(new Control[] { lblDesde, dtpVentasDesde, lblHasta, dtpVentasHasta, btnGenerarVentas, btnLimpiarVentas });

            // Panel de resumen
            var panelResumen = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.White
            };

            lblTotalVentas = new Label
            {
                Text = "Total Ventas e Ingresos: $0.00",
                Location = new Point(20, 20),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(34, 139, 34),
                AutoSize = true
            };

            lblCantidadVentas = new Label
            {
                Text = "Cantidad de Operaciones: 0",
                Location = new Point(20, 50),
                Font = new Font("Segoe UI", 10F),
                AutoSize = true
            };

            lblPromedioVentas = new Label
            {
                Text = "Promedio por Operación: $0.00",
                Location = new Point(300, 50),
                Font = new Font("Segoe UI", 10F),
                AutoSize = true
            };

            panelResumen.Controls.AddRange(new Control[] { lblTotalVentas, lblCantidadVentas, lblPromedioVentas });

            // Panel para gráfico visual por método de pago
            panelGraficoVentas = new Panel
            {
                Location = new Point(20, 10),
                Size = new Size(450, 280),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            var lblTituloGraficoVentas = new Label
            {
                Text = "Ventas por Método de Pago",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 66, 91),
                Location = new Point(20, 10),
                AutoSize = true
            };
            panelGraficoVentas.Controls.Add(lblTituloGraficoVentas);

            // Panel para gráfico visual por tipo de cliente
            panelGraficoTipoCliente = new Panel
            {
                Location = new Point(490, 10),
                Size = new Size(450, 280),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            var lblTituloGraficoCliente = new Label
            {
                Text = "Ventas por Tipo de Cliente",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 66, 91),
                Location = new Point(20, 10),
                AutoSize = true
            };
            panelGraficoTipoCliente.Controls.Add(lblTituloGraficoCliente);

            // Panel contenedor de gráficos
            var panelGraficos = new Panel
            {
                Dock = DockStyle.Top,
                Height = 300,
                BackColor = Color.FromArgb(240, 240, 240)
            };
            panelGraficos.Controls.Add(panelGraficoVentas);
            panelGraficos.Controls.Add(panelGraficoTipoCliente);

            // DataGridView para detalles
            dgvDetalleVentas = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Las columnas se inicializan en el método InicializarColumnasDataGrids
            var splitContainerVentas = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 300
            };

            splitContainerVentas.Panel1.Controls.Add(panelGraficos);
            splitContainerVentas.Panel2.Controls.Add(dgvDetalleVentas);

            tabVentas.Controls.Add(splitContainerVentas);
            tabVentas.Controls.Add(panelResumen);
            tabVentas.Controls.Add(panelFiltros);
        }

        private void ActualizarGraficoCompras(ReporteCompras reporte)
        {
            // Limpiar controles anteriores (excepto el título)
            for (int i = panelGraficoCompras.Controls.Count - 1; i >= 1; i--)
            {
                panelGraficoCompras.Controls.RemoveAt(i);
            }

            if (reporte.ComprasPorCategoria.Count == 0)
            {
                var lblSinDatos = new Label
                {
                    Text = "No hay datos para mostrar",
                    Location = new Point(150, 150),
                    AutoSize = true
                };
                panelGraficoCompras.Controls.Add(lblSinDatos);
                return;
            }

            int yPos = 50;
            var colores = new[] {
                Color.FromArgb(255, 99, 132),
                Color.FromArgb(54, 162, 235),
                Color.FromArgb(255, 206, 86),
                Color.FromArgb(75, 192, 192),
                Color.FromArgb(153, 102, 255)
            };

            int idx = 0;
            foreach (var kvp in reporte.ComprasPorCategoria)
            {
                decimal porcentaje = (kvp.Value / reporte.TotalCompras) * 100;

                var pnlItem = new Panel
                {
                    Location = new Point(20, yPos),
                    Size = new Size(450, 40),
                    BorderStyle = BorderStyle.FixedSingle
                };

                var lblColor = new Label
                {
                    BackColor = colores[idx % colores.Length],
                    Location = new Point(5, 5),
                    Size = new Size(30, 30)
                };

                var lblCategoria = new Label
                {
                    Text = kvp.Key,
                    Location = new Point(45, 10),
                    Size = new Size(150, 20),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                };

                var lblMonto = new Label
                {
                    Text = $"${kvp.Value:N2} ({porcentaje:F1}%)",
                    Location = new Point(210, 10),
                    Size = new Size(150, 20),
                    Font = new Font("Segoe UI", 9F)
                };

                var lblOps = new Label
                {
                    Text = $"{reporte.OperacionesPorCategoria[kvp.Key]} operaciones",
                    Location = new Point(370, 10),
                    Size = new Size(70, 20),
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.Gray
                };

                pnlItem.Controls.AddRange(new Control[] { lblColor, lblCategoria, lblMonto, lblOps });
                panelGraficoCompras.Controls.Add(pnlItem);

                yPos += 45;
                idx++;
            }
        }

        private void ActualizarGraficoVentas(ReporteVentas reporte)
        {
            // Limpiar controles anteriores (excepto el título)
            for (int i = panelGraficoVentas.Controls.Count - 1; i >= 1; i--)
            {
                panelGraficoVentas.Controls.RemoveAt(i);
            }

            if (reporte.VentasPorMetodoPago.Count == 0)
            {
                var lblSinDatos = new Label
                {
                    Text = "No hay datos para mostrar",
                    Location = new Point(150, 150),
                    AutoSize = true
                };
                panelGraficoVentas.Controls.Add(lblSinDatos);
                return;
            }

            int yPos = 50;
            var colores = new[] {
                Color.FromArgb(255, 99, 132),
                Color.FromArgb(54, 162, 235),
                Color.FromArgb(255, 206, 86),
                Color.FromArgb(75, 192, 192),
                Color.FromArgb(153, 102, 255)
            };

            int idx = 0;
            foreach (var kvp in reporte.VentasPorMetodoPago)
            {
                decimal porcentaje = (kvp.Value / reporte.TotalVentas) * 100;

                var pnlItem = new Panel
                {
                    Location = new Point(20, yPos),
                    Size = new Size(450, 40),
                    BorderStyle = BorderStyle.FixedSingle
                };

                var lblColor = new Label
                {
                    BackColor = colores[idx % colores.Length],
                    Location = new Point(5, 5),
                    Size = new Size(30, 30)
                };

                var lblMetodo = new Label
                {
                    Text = kvp.Key,
                    Location = new Point(45, 10),
                    Size = new Size(150, 20),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                };

                var lblMonto = new Label
                {
                    Text = $"${kvp.Value:N2} ({porcentaje:F1}%)",
                    Location = new Point(210, 10),
                    Size = new Size(150, 20),
                    Font = new Font("Segoe UI", 9F)
                };

                var lblOps = new Label
                {
                    Text = $"{reporte.OperacionesPorMetodoPago[kvp.Key]} operaciones",
                    Location = new Point(370, 10),
                    Size = new Size(70, 20),
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.Gray
                };

                pnlItem.Controls.AddRange(new Control[] { lblColor, lblMetodo, lblMonto, lblOps });
                panelGraficoVentas.Controls.Add(pnlItem);

                yPos += 45;
                idx++;
            }
        }

        private void BtnGenerarCompras_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime desde = dtpComprasDesde.Value.Date;
                DateTime hasta = dtpComprasHasta.Value.Date;

                if (desde > hasta)
                {
                    MessageBox.Show("La fecha 'Desde' no puede ser mayor a la fecha 'Hasta'.",
                        "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener reporte
                var reporte = _reportesService.ObtenerReporteCompras(desde, hasta);

                // Actualizar resumen
                lblTotalCompras.Text = $"Total Compras: {reporte.TotalCompras:C2}";
                lblCantidadCompras.Text = $"Cantidad de Operaciones: {reporte.CantidadOperaciones}";
                lblPromedioCompras.Text = $"Promedio por Operación: {reporte.PromedioOperacion:C2}";

                // Actualizar gráfico visual
                ActualizarGraficoCompras(reporte);

                // Actualizar detalles
                var detalles = _reportesService.ObtenerDetalleCompras(desde, hasta);
                dgvDetalleCompras.Rows.Clear();
                
                // Verificar si hay datos
                if (detalles == null || detalles.Count == 0)
                {
                    // Agregar una fila indicando que no hay datos
                    dgvDetalleCompras.Rows.Add("Sin datos", "", "", "");
                }
                else
                {
                    foreach (var detalle in detalles)
                    {
                        dgvDetalleCompras.Rows.Add(
                            detalle.IdOrdenCompra,
                            detalle.Fecha.ToShortDateString(),
                            detalle.Proveedor,
                            detalle.Total
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar reporte: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGenerarVentas_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime desde = dtpVentasDesde.Value.Date;
                DateTime hasta = dtpVentasHasta.Value.Date;

                if (desde > hasta)
                {
                    MessageBox.Show("La fecha 'Desde' no puede ser mayor a la fecha 'Hasta'.",
                        "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener reporte
                var reporte = _reportesService.ObtenerReporteVentas(desde, hasta);

                // Actualizar resumen
                lblTotalVentas.Text = $"Total Ventas e Ingresos: {reporte.TotalVentas:C2}";
                lblCantidadVentas.Text = $"Cantidad de Operaciones: {reporte.CantidadOperaciones}";
                lblPromedioVentas.Text = $"Promedio por Operación: {reporte.PromedioOperacion:C2}";

                // Actualizar gráfico visual por método de pago
                ActualizarGraficoVentas(reporte);

                // Actualizar gráfico visual por tipo de cliente
                ActualizarGraficoTipoCliente(reporte);

                // Actualizar detalles
                var detalles = _reportesService.ObtenerDetalleVentas(desde, hasta);
                dgvDetalleVentas.Rows.Clear();
                
                // Verificar si hay datos
                if (detalles == null || detalles.Count == 0)
                {
                    // Agregar una fila indicando que no hay datos
                    dgvDetalleVentas.Rows.Add("Sin datos", "", "", "", "", "");
                }
                else
                {
                    foreach (var detalle in detalles)
                    {
                        dgvDetalleVentas.Rows.Add(
                            detalle.IdFactura,
                            detalle.Fecha.ToShortDateString(),
                            detalle.Cliente,  // Cambiado de TipoCliente a Cliente para mostrar la descripción
                            detalle.MetodoPago,
                            detalle.TipoCliente,
                            detalle.Total
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar reporte: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLimpiarCompras_Click(object sender, EventArgs e)
        {
            // Restablecer fechas
            dtpComprasDesde.Value = DateTime.Today.AddMonths(-1);
            dtpComprasHasta.Value = DateTime.Today;

            // Limpiar resumen
            lblTotalCompras.Text = "Total Compras: $0.00";
            lblCantidadCompras.Text = "Cantidad de Operaciones: 0";
            lblPromedioCompras.Text = "Promedio por Operación: $0.00";

            // Limpiar gráfico
            for (int i = panelGraficoCompras.Controls.Count - 1; i >= 1; i--)
            {
                panelGraficoCompras.Controls.RemoveAt(i);
            }

            // Limpiar detalles
            dgvDetalleCompras.Rows.Clear();
        }

        private void BtnLimpiarVentas_Click(object sender, EventArgs e)
        {
            // Restablecer fechas
            dtpVentasDesde.Value = DateTime.Today.AddMonths(-1);
            dtpVentasHasta.Value = DateTime.Today;

            // Limpiar resumen
            lblTotalVentas.Text = "Total Ventas e Ingresos: $0.00";
            lblCantidadVentas.Text = "Cantidad de Operaciones: 0";
            lblPromedioVentas.Text = "Promedio por Operación: $0.00";

            // Limpiar gráficos
            for (int i = panelGraficoVentas.Controls.Count - 1; i >= 1; i--)
            {
                panelGraficoVentas.Controls.RemoveAt(i);
            }
            for (int i = panelGraficoTipoCliente.Controls.Count - 1; i >= 1; i--)
            {
                panelGraficoTipoCliente.Controls.RemoveAt(i);
            }

            // Limpiar detalles
            dgvDetalleVentas.Rows.Clear();
        }

        private void ActualizarGraficoTipoCliente(ReporteVentas reporte)
        {
            // Limpiar controles anteriores (excepto el título)
            for (int i = panelGraficoTipoCliente.Controls.Count - 1; i >= 1; i--)
            {
                panelGraficoTipoCliente.Controls.RemoveAt(i);
            }

            if (reporte.VentasPorTipoCliente.Count == 0)
            {
                var lblSinDatos = new Label
                {
                    Text = "No hay datos para mostrar",
                    Location = new Point(150, 150),
                    AutoSize = true
                };
                panelGraficoTipoCliente.Controls.Add(lblSinDatos);
                return;
            }

            int yPos = 50;
            var colores = new[] {
                Color.FromArgb(40, 167, 69),   // Verde - CONSUMIDOR FINAL
                Color.FromArgb(255, 193, 7),   // Amarillo - MONOTRIBUTISTA
                Color.FromArgb(23, 162, 184),  // Cyan - RESPONSABLE INSCRIPTO
                Color.FromArgb(108, 117, 125), // Gris - EXCENTO
                Color.FromArgb(220, 53, 69)    // Rojo - SIN CATEGORÍA
            };

            int idx = 0;
            foreach (var kvp in reporte.VentasPorTipoCliente.OrderByDescending(x => x.Value))
            {
                if (reporte.TotalVentas == 0) continue;
                decimal porcentaje = (kvp.Value / reporte.TotalVentas) * 100;

                var pnlItem = new Panel
                {
                    Location = new Point(20, yPos),
                    Size = new Size(410, 40),
                    BorderStyle = BorderStyle.FixedSingle
                };

                var lblColor = new Label
                {
                    BackColor = colores[idx % colores.Length],
                    Location = new Point(5, 5),
                    Size = new Size(30, 30)
                };

                var lblTipo = new Label
                {
                    Text = kvp.Key,
                    Location = new Point(45, 10),
                    Size = new Size(150, 20),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                };

                var lblMonto = new Label
                {
                    Text = $"${kvp.Value:N2} ({porcentaje:F1}%)",
                    Location = new Point(210, 10),
                    Size = new Size(150, 20),
                    Font = new Font("Segoe UI", 9F)
                };

                var lblOps = new Label
                {
                    Text = $"{reporte.OperacionesPorTipoCliente[kvp.Key]} operaciones",
                    Location = new Point(340, 10),
                    Size = new Size(60, 20),
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.Gray
                };

                pnlItem.Controls.AddRange(new Control[] { lblColor, lblTipo, lblMonto, lblOps });
                panelGraficoTipoCliente.Controls.Add(pnlItem);

                yPos += 45;
                idx++;
            }
        }
    }
}
