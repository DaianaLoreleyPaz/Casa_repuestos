using CasaRepuestos.Models;
using CasaRepuestos.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CasaRepuestos.Forms
{
    public partial class FrmInventario : Form
    {
        // servicios y variables
        private ArticuloService _articuloService = new ArticuloService();
        private List<Articulo> _todosLosArticulos; 
        private const int _stockMinimo = 10; 
        private bool esTextoPlaceholder = true;

        public FrmInventario()
        {
            InitializeComponent();

            // Estilos 
            AplicarEstilos();

            this.Load += FrmInventario_Load;

            // Asegurarse de conectar el event handler una sola vez
            dgvArticulo.CellFormatting -= DgvInventario_CellFormatting;
            dgvArticulo.CellFormatting += DgvInventario_CellFormatting;
        }

        // Evento Load del formulario
        private void FrmInventario_Load(object sender, EventArgs e)
        {
            CargarArticulo();
            MostrarPlaceholder();
            MostrarEstadisticasInventario();
        }

        private void CargarArticulo()
        {
            dgvArticulo.AutoGenerateColumns = false;
            dgvArticulo.Columns.Clear();
            dgvArticulo.ReadOnly = true;
            ConfigurarGrilla();

            _todosLosArticulos = _articuloService.ListarArticulos() ?? new List<Articulo>();
            dgvArticulo.DataSource = null;
            dgvArticulo.DataSource = _todosLosArticulos;
            // ocultar columna IdArticulo
            dgvArticulo.Columns["IdArticulo"].Visible = false;
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
                Visible = false // No mostrar ID en la grilla
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
                FillWeight = 20
            });

            dgvArticulo.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Proveedor",
                HeaderText = "Proveedor",

              
                DataPropertyName = "NombresProveedores",

                FillWeight = 20
            });
        }

        private void MostrarPlaceholder()
        {
            if (txtBuscar != null)
            {
                txtBuscar.Text = "Buscar artículo";
                txtBuscar.ForeColor = Color.Gray;
                esTextoPlaceholder = true;
            }
        }

        // Evento para filtrar artículos en tiempo real
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (txtBuscar == null) return;
            if (esTextoPlaceholder && string.IsNullOrWhiteSpace(txtBuscar.Text)) return;

            string filtro = txtBuscar.Text.ToLower().Trim();

            if (_todosLosArticulos == null)
            {
                _todosLosArticulos = _articuloService.ListarArticulos();
            }

            var filtrados = _todosLosArticulos.Where(a =>
                (a.Nombre ?? "").ToLower().Contains(filtro) ||
                (a.Marca?.Nombre ?? "").ToLower().Contains(filtro) ||
                (a.NombresProveedores ?? "").ToLower().Contains(filtro)

            ).ToList();

            dgvArticulo.DataSource = null;
            dgvArticulo.DataSource = filtrados;
            dgvArticulo.ClearSelection();
        }
        // Eventos para manejar el placeholder si se hace foco o se pierde foco
        private void txtBuscar_Enter(object sender, EventArgs e)
        {
            if (txtBuscar == null) return;

            if (esTextoPlaceholder)
            {
                txtBuscar.Text = "";
                txtBuscar.ForeColor = Color.Black;
                esTextoPlaceholder = false;
            }
        }
        // Evento Leave para restaurar el placeholder si el textbox está vacío
        private void txtBuscar_Leave(object sender, EventArgs e)
        {
            if (txtBuscar == null) return;

            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                MostrarPlaceholder();
            }
        }

        // Evento para aplicar formato condicional (muestra stock bajo)
        private void DgvInventario_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (dgvArticulo.Columns[e.ColumnIndex].Name == "Stock" && e.RowIndex >= 0)
                {
                    if (e.Value != null && int.TryParse(e.Value.ToString(), out int stock))
                    {
                        if (stock < _stockMinimo)
                        {
                            // Resalta la fila en color rojizo suave y fondo claro
                            dgvArticulo.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkRed;
                            dgvArticulo.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 235);
                        }
                        else
                        {
                            // Resetear estilo si no está bajo stock
                            dgvArticulo.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                            dgvArticulo.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                        }
                    }
                }
            }
            catch
            {
               
            }
        }

        // Muestra estadísticas del inventario como total de artículos, stock global y artículos bajo stock
        private void MostrarEstadisticasInventario()
        {
            if (_todosLosArticulos == null || !_todosLosArticulos.Any())
            {
                lblTotalArticulos.Text = "Cantidad de artículos: 0";
                lblStockGlobal.Text = "Stock global: 0";
                lblArticulosBajoStock.Text = "No hay artículos con bajo stock";
                lblArticulosBajoStock.ForeColor = SystemColors.ControlText;
                btnMostrarBajoStock.Enabled = false;
                btnGenerarPDF.Enabled = false;
                return;
            }

            int totalArticulos = _todosLosArticulos.Count;
            int stockGlobal = _todosLosArticulos.Sum(a => a.Stock);

            lblTotalArticulos.Text = $"Cantidad de artículos: {totalArticulos}";
            lblStockGlobal.Text = $"Stock global: {stockGlobal}";

            var articulosBajoStockList = _todosLosArticulos.Where(a => a.Stock < _stockMinimo).ToList();
            if (articulosBajoStockList.Any())
            {
                lblArticulosBajoStock.Text = $"Cantidad de artículos con bajo stock: {articulosBajoStockList.Count}";
                lblArticulosBajoStock.ForeColor = Color.Red;
                btnMostrarBajoStock.Enabled = true;
                btnGenerarPDF.Enabled = true;
            }
            else
            {
                lblArticulosBajoStock.Text = "No hay artículos con bajo stock";
                lblArticulosBajoStock.ForeColor = SystemColors.ControlText;
                btnMostrarBajoStock.Enabled = false;
                btnGenerarPDF.Enabled = false;
            }
        }

        // Evento para mostrar lista de artículos con bajo stock
        private void btnMostrarBajoStock_Click(object sender, EventArgs e)
        {
            if (_todosLosArticulos == null)
            {
                _todosLosArticulos = _articuloService.ListarArticulos();
            }

            var articulosFiltrados = _todosLosArticulos.Where(a => a.Stock < _stockMinimo).ToList();
            dgvArticulo.DataSource = null;
            dgvArticulo.DataSource = articulosFiltrados;
            dgvArticulo.ClearSelection();
        }
        // Evento para recargar todos los artículos
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            CargarArticulo();
            MostrarEstadisticasInventario();
        }
        // Evento para generar el reporte PDF de artículos con bajo stock
        private void btnGenerarPDF_Click(object sender, EventArgs e)
        {
            GenerarPDFBajoStock();
        }

        private void GenerarPDFBajoStock()
        {
            // --- PREPARACIÓN DE DATOS ---
            try
            {
                if (_todosLosArticulos == null) _todosLosArticulos = _articuloService.ListarArticulos();

                var articulosBajoStock = _todosLosArticulos
                    .Where(a => a.Stock < _stockMinimo)
                    .OrderBy(a => a.Stock)
                    .ToList();

                if (!articulosBajoStock.Any())
                {
                    MessageBox.Show("No hay artículos con bajo stock para generar el reporte.",
                                   "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // --- CONFIGURACIÓN DEL DOCUMENTO ---
                var doc = new Document(PageSize.A4, 40f, 40f, 40f, 40f);

                string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string nombreArchivo = $"Reporte_Bajo_Stock_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                string rutaCompleta = Path.Combine(escritorio, nombreArchivo);

                using (PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(rutaCompleta, FileMode.Create)))
                {
                    doc.Open();

                    // Tipografías
                    var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA, 16, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY);
                    var fontSubtitulo = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD);
                    var fontNormal = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                    var fontAlerta = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.RED);

                    var separator = new Chunk(new LineSeparator(1f, 100f, BaseColor.GRAY, Element.ALIGN_CENTER, -1f));

                    // --- CABECERA ---
                    doc.Add(new Paragraph("CASA DE REPUESTOS", fontSubtitulo) { Alignment = Element.ALIGN_RIGHT });
                    doc.Add(new Paragraph($"Fecha de Reporte: {DateTime.Now:dd/MM/yyyy HH:mm}", fontNormal) { Alignment = Element.ALIGN_RIGHT });

                    var titulo = new Paragraph("REPORTE DE ARTÍCULOS CON BAJO STOCK", fontTitulo)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 15f
                    };
                    doc.Add(titulo);
                    doc.Add(separator);
                    doc.Add(new Paragraph("\n"));

                    // --- TABLA DE DETALLES ---
                    PdfPTable table = new PdfPTable(4)
                    {
                        WidthPercentage = 100
                    };
                    // Anchos de columna
                    table.SetWidths(new float[] { 4f, 2f, 3f, 1f });

                    // Encabezados
                    BaseColor headerColor = new BaseColor(230, 230, 230);

                    PdfPCell cell;

                    // Descripción
                    cell = new PdfPCell(new Phrase("Artículo", fontSubtitulo)) { Padding = 5, BackgroundColor = headerColor };
                    table.AddCell(cell);
                    // Marca
                    cell = new PdfPCell(new Phrase("Marca", fontSubtitulo)) { Padding = 5, BackgroundColor = headerColor, HorizontalAlignment = Element.ALIGN_CENTER };
                    table.AddCell(cell);
                    // Proveedor
                    cell = new PdfPCell(new Phrase("Proveedor", fontSubtitulo)) { Padding = 5, BackgroundColor = headerColor };
                    table.AddCell(cell);
                    // Stock
                    cell = new PdfPCell(new Phrase("Stock", fontSubtitulo)) { Padding = 5, BackgroundColor = headerColor, HorizontalAlignment = Element.ALIGN_CENTER };
                    table.AddCell(cell);

                    // Filas de Contenido
                    foreach (var articulo in articulosBajoStock)
                    {
                        // Articulo
                        table.AddCell(new PdfPCell(new Phrase(articulo.Nombre, fontNormal)) { Padding = 4 });

                        // Marca 
                        table.AddCell(new PdfPCell(new Phrase(articulo.Marca?.Nombre ?? "N/A", fontNormal)) { Padding = 4, HorizontalAlignment = Element.ALIGN_CENTER });

                        // Proveedor 
                        table.AddCell(new PdfPCell(new Phrase(articulo.Proveedor?.RazonSocial ?? "N/A", fontNormal)) { Padding = 4 });

                        // Stock 
                        table.AddCell(new PdfPCell(new Phrase(articulo.Stock.ToString(), fontAlerta)) { Padding = 4, HorizontalAlignment = Element.ALIGN_CENTER });
                    }

                    doc.Add(table);

                    // --- PIE DE PÁGINA  ---
                    doc.Add(new Paragraph("\n"));
                    doc.Add(new Paragraph($"* Este reporte incluye {articulosBajoStock.Count} artículos cuyo stock es inferior a {_stockMinimo} unidades.", fontNormal));


                    // ---CIERRE Y APERTURA ---
                    doc.Close();
                }

                MessageBox.Show($"Reporte de Bajo Stock generado exitosamente en el Escritorio:\n{rutaCompleta}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Abrir el archivo
                Process.Start(new ProcessStartInfo(rutaCompleta) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                // En caso de error, muestra el mensaje
                MessageBox.Show($"Error fatal al generar el PDF (iTextSharp): {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarEstilos()
        {
            this.BackColor = Color.FromArgb(240, 245, 249);

            // Estilos para GroupBoxes 
            foreach (var gb in this.Controls.OfType<GroupBox>().Concat(this.Controls.OfType<Panel>().SelectMany(p => p.Controls.OfType<GroupBox>())))
            {
                gb.BackColor = Color.White;
                gb.ForeColor = Color.FromArgb(45, 66, 91);
                gb.Font = new ("Segoe UI", 10F, FontStyle.Bold); 
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
            try { btnGenerarPDF.BackColor = Color.FromArgb(192, 57, 43); } catch { }
            try { btnMostrarBajoStock.BackColor = Color.FromArgb(12, 87, 150); } catch { }

            // Estilos para Labels de las estadísticas
            
            // Estilos para DataGridView
            foreach (var dgv in new[] { dgvArticulo })
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
        
    }
}
