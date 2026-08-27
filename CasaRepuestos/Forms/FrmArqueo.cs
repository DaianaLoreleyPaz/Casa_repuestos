using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CasaRepuestos.Models;
using CasaRepuestos.Services;

namespace CasaRepuestos.Forms
{
    public partial class FrmArqueo : Form
    {
        private ArqueoService _arqueoService;
        private List<Caja> _cajasDisponibles;

        public FrmArqueo()
        {
            InitializeComponent();
            _arqueoService = new ArqueoService();
            _cajasDisponibles = new List<Caja>();

            // Cerrar automáticamente cajas de días anteriores
            CerrarCajasAnterioresAlIniciar();

            //Verificar rol de usuario para mostrar/ocultar el botón de reabrir caja
            VerificarPermisosUsuario();

            InicializarColumnas();
            CargarCajasDisponibles();
            VerificarEstadoCaja();
        }

        // Verificar permisos de usuario
        private void VerificarPermisosUsuario()
        {
            // Obtener el rol del usuario logueado
            string userRole = SesionService.ObtenerRolLogueado();

            // si es administrador, mostrar el botón de reabrir caja
            btnReabrirCaja.Visible = (userRole == "ADMINISTRADOR");
        }

        private void InicializarColumnas()
        {
            // Configuraciones comunes para ambos DataGridViews (ingresos y egresos)
            dgvIngresos.ReadOnly = true;
            dgvIngresos.AllowUserToAddRows = false;
            dgvIngresos.AllowUserToDeleteRows = false;
            dgvIngresos.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvIngresos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIngresos.RowHeadersVisible = false;
            dgvIngresos.CellBeginEdit += (s, e) => { e.Cancel = true; };

            dgvEgresos.ReadOnly = true;
            dgvEgresos.AllowUserToAddRows = false;
            dgvEgresos.AllowUserToDeleteRows = false;
            dgvEgresos.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvEgresos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEgresos.RowHeadersVisible = false;
            dgvEgresos.CellBeginEdit += (s, e) => { e.Cancel = true; };

            if (dgvIngresos.Columns.Count == 0)
            {
                dgvIngresos.Columns.Add("Descripcion", "Descripción");
                dgvIngresos.Columns.Add("Metodo", "Método de Pago");
                dgvIngresos.Columns.Add("Monto", "Monto ($)");
                dgvIngresos.Columns["Monto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvEgresos.Columns.Count == 0)
            {
                dgvEgresos.Columns.Add("Descripcion", "Descripción");
                dgvEgresos.Columns.Add("Metodo", "Método de Pago");
                dgvEgresos.Columns.Add("Monto", "Monto ($)");
                dgvEgresos.Columns["Monto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        //Método para cerrar cajas de días anteriores al iniciar
        private void CerrarCajasAnterioresAlIniciar()
        {
            try
            {
                // Intentar cerrar las cajas de días anteriores
                int cajasCerradas = _arqueoService.CerrarCajasAnterioresAutomaticamente();

                // Si se cerraron cajas, mostrar notificación
                if (cajasCerradas > 0)
                {
                    MessageBox.Show(
                        $" Se {(cajasCerradas == 1 ? "cerró" : "cerraron")} automáticamente {cajasCerradas} caja{(cajasCerradas == 1 ? "" : "s")} de día{(cajasCerradas == 1 ? "" : "s")} anterior{(cajasCerradas == 1 ? "" : "es")}.\n\n" +
                        $"El sistema ha calculado el saldo final de cada caja y las ha cerrado correctamente.",
                        "Cierre Automático de Cajas",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cerrar cajas anteriores: {ex.Message}");
            }
        }
        private void CargarCajasDisponibles()
        {
            try
            {
                _cajasDisponibles = _arqueoService.ObtenerTodasLasCajas() ?? new List<Caja>();
                comboCajas.Items.Clear();

                //Si no hay cajas registradas, mostramos un mensaje y salimos
                if (_cajasDisponibles.Count == 0)
                {
                    comboCajas.SelectedIndex = -1;
                    return;
                }

                // Cargar las cajas disponibles en el ComboBox
                foreach (var caja in _cajasDisponibles)
                {
                    string estado = caja.FechaCierre == null ? "Abierta" : "Cerrada";
                    string texto = $"Caja #{caja.IdCaja} - {caja.FechaApertura:dd/MM/yyyy} - {estado}";
                    comboCajas.Items.Add(texto);
                }

                //Seleccionar la caja abierta de hoy, si existe
                var cajaHoy = _cajasDisponibles.FirstOrDefault(c =>
                    c.FechaApertura.Date == DateTime.Today && c.FechaCierre == null);

                if (cajaHoy != null)
                {
                    comboCajas.SelectedIndex = _cajasDisponibles.IndexOf(cajaHoy);
                }
                else
                {
                    // Si no hay caja abierta hoy, seleccionar la primera del listado
                    comboCajas.SelectedIndex = comboCajas.Items.Count > 0 ? 0 : -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las cajas: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void VerificarEstadoCaja()
        {
            // Verificar si ya existe una caja para hoy (abierta o cerrada)
            if (_arqueoService.ExisteCajaHoy())
            {
                // Si existe una caja para hoy, verificar si está abierta
                if (!_arqueoService.ExisteCajaAbiertaHoy())
                {
                    // Existe una caja pero está cerrada, permitir verla
                    CargarDatosCajaSeleccionada();
                }
                else
                {
                    // Existe una caja abierta, cargar sus datos
                    CargarDatosCajaSeleccionada();
                }
            }
            else
            {
                // No existe ninguna caja para hoy, mostrar formulario de apertura
                MostrarFormAperturaCaja();
            }
        }

        private void MostrarFormAperturaCaja()
        {
            // Mostrar formulario de apertura de caja en un bucle hasta que se abra correctamente o se cancele
            bool cajaAbierta = false;
            bool cancelar = false;

            while (!cajaAbierta && !cancelar)
            {
                using (var formApertura = new Form())
                {
                    formApertura.Text = "Apertura de Caja";
                    formApertura.Size = new Size(500, 250);
                    formApertura.StartPosition = FormStartPosition.CenterScreen;
                    formApertura.FormBorderStyle = FormBorderStyle.FixedDialog;
                    formApertura.MaximizeBox = false;
                    formApertura.MinimizeBox = false;

                    var lblTitulo = new Label()
                    {
                        Text = "APERTURA DE CAJA DEL DÍA",
                        Font = new Font("Segoe UI", 14, FontStyle.Bold),
                        ForeColor = Color.FromArgb(45, 66, 91),
                        Location = new Point(20, 20),
                        Size = new Size(350, 30),
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    var lblMonto = new Label()
                    {
                        Text = "Monto Inicial:",
                        Font = new Font("Segoe UI", 11, FontStyle.Regular),
                        Location = new Point(50, 80),
                        Size = new Size(100, 25)
                    };

                    var txtMonto = new NumericUpDown()
                    {
                        Location = new Point(160, 80),
                        Size = new Size(150, 25),
                        Minimum = 0,
                        Maximum = 999999999999999,
                        DecimalPlaces = 2,
                        Value = 0
                    };

                    var btnAbrir = new Button()
                    {
                        Text = "ABRIR CAJA",
                        BackColor = Color.FromArgb(45, 66, 91),
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 11, FontStyle.Bold),
                        Location = new Point(120, 140),
                        Size = new Size(150, 40),
                        DialogResult = DialogResult.OK
                    };

                    var btnCancelar = new Button()
                    {
                        Text = "CANCELAR",
                        BackColor = Color.Gray,
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 11, FontStyle.Bold),
                        Location = new Point(120, 190),
                        Size = new Size(150, 40),
                        DialogResult = DialogResult.Cancel
                    };

                    btnAbrir.Click += (s, e) =>
                    {
                        if (txtMonto.Value <= 0)
                        {
                            MessageBox.Show("El monto inicial debe ser mayor a cero.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtMonto.Focus();
                            return;
                        }

                        int idEmpleado = SesionService.ObtenerIdEmpleadoLogueado();

                        if (idEmpleado <= 0)
                        {
                            MessageBox.Show("No se pudo identificar al empleado. Contacte al administrador",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (_arqueoService.AbrirCaja(DateTime.Now, txtMonto.Value, idEmpleado))
                        {
                            MessageBox.Show($"Caja abierta correctamente con ${txtMonto.Value:N2} de fondo inicial",
                                "Caja Abierta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            formApertura.DialogResult = DialogResult.OK;
                            CargarCajasDisponibles();
                            CargarDatosCajaSeleccionada();
                            cajaAbierta = true; // Salir del bucle
                        }
                        else
                        {
                            MessageBox.Show("No se pudo abrir la caja. Ya existe una caja para hoy",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };

                    formApertura.Controls.AddRange(new Control[] { lblTitulo, lblMonto, txtMonto, btnAbrir, btnCancelar });
                    formApertura.AcceptButton = btnAbrir;
                    formApertura.CancelButton = btnCancelar;

                    var resultado = formApertura.ShowDialog();

                    // Si el usuario cancela, salir del bucle
                    if (resultado == DialogResult.Cancel)
                    {
                        cancelar = true;
                    }
                }
            }
        }
        // Cargar los datos de la caja seleccionada en el ComboBox
        private void CargarDatosCajaSeleccionada()
        {
            try
            {
                if (comboCajas.SelectedIndex < 0 || comboCajas.SelectedIndex >= _cajasDisponibles.Count)
                    return;

                Caja cajaSeleccionada = _cajasDisponibles[comboCajas.SelectedIndex];
                DateTime fechaCaja = cajaSeleccionada.FechaApertura.Date;

                labelFecha.Text = $"Fecha: {fechaCaja:dd/MM/yyyy}";
                labelCaja.Text = $"Caja #{cajaSeleccionada.IdCaja} - {(cajaSeleccionada.FechaCierre == null ? "Activa" : "Cerrada")}";

                var ingresos = _arqueoService.ObtenerIngresosDelDia(fechaCaja);
                var egresos = _arqueoService.ObtenerEgresosDelDia(fechaCaja);
                decimal montoInicial = cajaSeleccionada.SaldoInicial;

                // total de ingresos de ventas y servicios
                decimal totalVentasYServicios = _arqueoService.ObtenerTotalIngresosPorFecha(fechaCaja);

                dgvIngresos.Rows.Clear();
                dgvEgresos.Rows.Clear();

                foreach (var ingreso in ingresos)
                    dgvIngresos.Rows.Add(ingreso.Descripcion, ingreso.MetodoPago, ingreso.Monto.ToString("N2"));

                foreach (var egreso in egresos)
                    dgvEgresos.Rows.Add(egreso.Descripcion, egreso.MetodoPago, egreso.Monto.ToString("N2"));

                decimal totalIngresos = ingresos.Sum(i => i.Monto) + totalVentasYServicios;
                decimal totalEgresos = egresos.Sum(e => e.Monto);

                decimal saldoFinal = montoInicial + totalVentasYServicios - totalEgresos;

                labelSaldoFinal.ForeColor = saldoFinal < 0 ? Color.IndianRed : Color.ForestGreen;

                lblMontoInicial.Text = $"${montoInicial:N2}";
                lblTotalIngresos.Text = $"${totalVentasYServicios:N2}";
                lblTotalEgresos.Text = $"${totalEgresos:N2}";
                labelSaldoFinal.Text = $"Saldo Final: {saldoFinal.ToString("C", new CultureInfo("es-AR"))}";

                labelCaja.Tag = $"(Incluye ${totalVentasYServicios:N2} de ventas y servicios)";
                // Metodo para mostrar totales por método de pago
                MostrarTotalesPorMetodoPago(ingresos, egresos, totalVentasYServicios, totalEgresos, saldoFinal);

                // Actualizar estado de los botones según el estado de la caja
                bool cajaAbierta = (cajaSeleccionada.FechaCierre == null);
                bool cajaDelDia = (fechaCaja == DateTime.Today);

                btnCerrarCaja.Enabled = (cajaAbierta && cajaDelDia);
                btnReabrirCaja.Enabled = (!cajaAbierta && cajaDelDia);
                btnRetirarDinero.Enabled = (cajaAbierta && cajaDelDia);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos de la caja: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //  Mostrar totales por método de pago
        private void MostrarTotalesPorMetodoPago(List<MovimientoCaja> ingresos, List<MovimientoCaja> egresos,
            decimal totalVentasYServicios, decimal totalEgresos, decimal saldoFinal)
        {
            flowDetalleMetodos.Controls.Clear();
            flowDetalleMetodos.WrapContents = false;
            flowDetalleMetodos.FlowDirection = FlowDirection.LeftToRight;

            // Métodos de pago comunes (sin CUENTA CORRIENTE)
            var metodosPago = new string[] { "EFECTIVO", "TARJETA", "TRANSFERENCIA", "BILLETERA VIRTUAL" };

            // Calcular totales por método de pago para ingresos
            var ingresosPorMetodo = new Dictionary<string, decimal>();
            var egresosPorMetodo = new Dictionary<string, decimal>();

            foreach (var metodo in metodosPago)
            {
                ingresosPorMetodo[metodo] = ingresos
                    .Where(i => i.MetodoPago == metodo)
                    .Sum(i => i.Monto);

                egresosPorMetodo[metodo] = egresos
                    .Where(e => e.MetodoPago == metodo)
                    .Sum(e => e.Monto);
            }

            // labels para cada método de pago
            foreach (var metodo in metodosPago)
            {
                decimal ingresoMetodo = ingresosPorMetodo[metodo];
                decimal egresoMetodo = egresosPorMetodo[metodo];
                decimal netoMetodo = ingresoMetodo - egresoMetodo;

                // Panel para cada método de pago
                var panelMetodo = new Panel()
                {
                    Size = new Size(200, 100),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.White,
                    Margin = new Padding(10)
                };

                var lblMetodo = new Label()
                {
                    Text = metodo,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.FromArgb(45, 66, 91),
                    Location = new Point(5, 5),
                    Size = new Size(190, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                var lblIngreso = new Label()
                {
                    Text = $"Ingreso: ${ingresoMetodo:N2}",
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.ForestGreen,
                    Location = new Point(5, 30),
                    Size = new Size(190, 20)
                };

                var lblEgreso = new Label()
                {
                    Text = $"Egreso: ${egresoMetodo:N2}",
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.IndianRed,
                    Location = new Point(5, 50),
                    Size = new Size(190, 20)
                };

                var lblNeto = new Label()
                {
                    Text = $"Neto: ${netoMetodo:N2}",
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = netoMetodo >= 0 ? Color.ForestGreen : Color.IndianRed,
                    Location = new Point(5, 70),
                    Size = new Size(190, 20)
                };

                panelMetodo.Controls.AddRange(new Control[] { lblMetodo, lblIngreso, lblEgreso, lblNeto });
                flowDetalleMetodos.Controls.Add(panelMetodo);
            }

            // Actualiza label de saldo final 
            ActualizarLabelSaldoFinal(totalVentasYServicios, totalEgresos, saldoFinal);
        }

        // Actualiza el label de saldo final con más información
        private void ActualizarLabelSaldoFinal(decimal totalVentasYServicios, decimal totalEgresos, decimal saldoFinal)
        {

            string textoSaldo = $"Saldo Final: {saldoFinal.ToString("C", new CultureInfo("es-AR"))}";

            var toolTip = new ToolTip();
            toolTip.SetToolTip(labelSaldoFinal,
                $"Ingresos Totales: ${totalVentasYServicios:N2}\n" +
                $"Egresos Totales: ${totalEgresos:N2}\n" +
                $"Saldo Final: ${saldoFinal:N2}");

            labelSaldoFinal.Text = textoSaldo;
            labelSaldoFinal.ForeColor = saldoFinal < 0 ? Color.IndianRed : Color.ForestGreen;
        }

        private void comboCajas_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarDatosCajaSeleccionada();
        }

        // Reabrir caja cerrada
        private void btnReabrirCaja_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboCajas.SelectedIndex < 0)
                    return;

                Caja cajaSeleccionada = _cajasDisponibles[comboCajas.SelectedIndex];

                // Solo permitir reabrir cajas cerradas
                if (cajaSeleccionada.FechaCierre == null)
                {
                    MessageBox.Show("Esta caja ya está abierta.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Solo permitir reabrir cajas del día actual
                if (cajaSeleccionada.FechaApertura.Date != DateTime.Today)
                {
                    MessageBox.Show(
                        "❌ No se puede reabrir una caja de un día anterior.\n\n" +
                        $"Fecha de la caja: {cajaSeleccionada.FechaApertura:dd/MM/yyyy}\n" +
                        $"Fecha actual: {DateTime.Today:dd/MM/yyyy}\n\n" +
                        "Solo se pueden reabrir cajas del día actual.",
                        "Fecha Inválida",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "¿Está seguro que desea reabrir esta caja? Esto permitirá continuar operando con ella",
                    "Confirmar Reapertura",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (_arqueoService.ReabrirCaja(cajaSeleccionada.FechaApertura.Date))
                    {
                        MessageBox.Show("Caja reabierta correctamente", "Información",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarCajasDisponibles();
                        // Seleccionar la caja reabierta
                        var cajaReabierta = _cajasDisponibles.FirstOrDefault(c => c.IdCaja == cajaSeleccionada.IdCaja);
                        if (cajaReabierta != null)
                        {
                            comboCajas.SelectedIndex = _cajasDisponibles.IndexOf(cajaReabierta);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se pudo reabrir la caja", "Advertencia",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al reabrir caja: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Mostrar contador de billetes
        private void btnContadorBilletes_Click(object sender, EventArgs e)
        {
            MostrarContadorBilletes();
        }
        // Método para mostrar el formulario del contador de billetes
        private void MostrarContadorBilletes()
        {
            using (var formBilletes = new Form())
            {
                formBilletes.Text = "Contador de Billetes - Argentina";
                formBilletes.Size = new Size(500, 700);
                formBilletes.StartPosition = FormStartPosition.CenterScreen;
                formBilletes.FormBorderStyle = FormBorderStyle.FixedDialog;
                formBilletes.MaximizeBox = false;
                formBilletes.MinimizeBox = false;
                formBilletes.BackColor = Color.White;


                var lblTitulo = new Label()
                {
                    Text = "CONTADOR DE BILLETES - ARGENTINA",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.FromArgb(45, 66, 91),
                    Location = new Point(20, 20),
                    Size = new Size(440, 30),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                var denominaciones = new decimal[] { 10, 20, 50, 100, 200, 500, 1000, 2000, 10000, 20000 };
                var textos = new string[] {
            "$10", "$20", "$50", "$100", "$200",
            "$500", "$1.000", "$2.000", "$10.000", "$20.000"
        };

                var labelsDenominacion = new List<Label>();
                var numericControls = new List<NumericUpDown>();
                var labelsTotal = new List<Label>();

                int yPos = 70;

                var lblTotalGeneral = new Label()
                {
                    Text = "$0",
                    Font = new Font("Segoe UI", 18, FontStyle.Bold),
                    ForeColor = Color.ForestGreen,
                    Size = new Size(150, 30),
                    TextAlign = ContentAlignment.MiddleRight
                };

                for (int i = 0; i < denominaciones.Length; i++)
                {
                    var labelDenom = new Label()
                    {
                        Text = textos[i],
                        Font = new Font("Segoe UI", 11, FontStyle.Bold),
                        ForeColor = Color.FromArgb(45, 66, 91),
                        Location = new Point(50, yPos),
                        Size = new Size(80, 25),
                        TextAlign = ContentAlignment.MiddleLeft
                    };
                    labelsDenominacion.Add(labelDenom);

                    var lblMultiplicador = new Label()
                    {
                        Text = "x",
                        Font = new Font("Segoe UI", 11, FontStyle.Regular),
                        Location = new Point(140, yPos),
                        Size = new Size(20, 25),
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    var numericControl = new NumericUpDown()
                    {
                        Location = new Point(170, yPos),
                        Size = new Size(80, 25),
                        Minimum = 0,
                        Maximum = 10000,
                        DecimalPlaces = 0,
                        Value = 0,
                        TextAlign = HorizontalAlignment.Right,
                        Font = new Font("Segoe UI", 10)
                    };
                    numericControls.Add(numericControl);

                    numericControl.Text = "";

                    var lblIgual = new Label()
                    {
                        Text = "=",
                        Font = new Font("Segoe UI", 11, FontStyle.Regular),
                        Location = new Point(260, yPos),
                        Size = new Size(20, 25),
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    var labelTotal = new Label()
                    {
                        Text = "$0",
                        Font = new Font("Segoe UI", 11, FontStyle.Bold),
                        ForeColor = Color.ForestGreen,
                        Location = new Point(290, yPos),
                        Size = new Size(120, 25),
                        TextAlign = ContentAlignment.MiddleRight
                    };
                    labelsTotal.Add(labelTotal);

                    // EVENTOS PARA CALCULAR AL ESCRIBIR
                    numericControl.KeyUp += (s, e) =>
                    {
                        CalcularTotales(numericControls.ToArray(), denominaciones, labelsTotal.ToArray(), lblTotalGeneral);
                    };

                    numericControl.KeyPress += (s, e) =>
                    {
                        // Calcular después de un pequeño delay para que se actualice el valor
                        Task.Delay(10).ContinueWith(_ =>
                        {
                            if (!formBilletes.IsDisposed)
                            {
                                formBilletes.Invoke(new Action(() =>
                                {
                                    CalcularTotales(numericControls.ToArray(), denominaciones, labelsTotal.ToArray(), lblTotalGeneral);
                                }));
                            }
                        }, TaskScheduler.Default);
                    };

                    numericControl.ValueChanged += (s, e) =>
                    {
                        CalcularTotales(numericControls.ToArray(), denominaciones, labelsTotal.ToArray(), lblTotalGeneral);
                    };

                    // Evento para manejar cuando el control obtiene el foco 
                    numericControl.Enter += (s, e) =>
                    {
                        if (numericControl.Value == 0)
                        {
                            numericControl.Text = "";
                        }
                    };

                    // Evento para cuando pierde el foco 
                    numericControl.Leave += (s, e) =>
                    {
                        if (string.IsNullOrWhiteSpace(numericControl.Text))
                        {
                            numericControl.Value = 0;
                            numericControl.Text = "";
                        }
                        // Calcular también al salir del campo
                        CalcularTotales(numericControls.ToArray(), denominaciones, labelsTotal.ToArray(), lblTotalGeneral);
                    };

                    formBilletes.Controls.AddRange(new Control[] {
                labelDenom, lblMultiplicador, numericControl, lblIgual, labelTotal
            });

                    yPos += 35;
                }


                var lineSeparator = new Label()
                {
                    BorderStyle = BorderStyle.Fixed3D,
                    Location = new Point(30, yPos + 10),
                    Size = new Size(420, 2)
                };

                // Total General - Actualiza posición
                lblTotalGeneral.Location = new Point(260, yPos + 25);

                var lblTotalTexto = new Label()
                {
                    Text = "TOTAL GENERAL:",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.FromArgb(45, 66, 91),
                    Location = new Point(50, yPos + 25),
                    Size = new Size(200, 30),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                // Botón Limpiar
                var btnLimpiar = new Button()
                {
                    Text = "LIMPIAR TODO",
                    BackColor = Color.FromArgb(80, 103, 135),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Location = new Point(80, yPos + 70),
                    Size = new Size(150, 40)
                };

                btnLimpiar.Click += (s, e) =>
                {
                    foreach (var numeric in numericControls)
                    {
                        numeric.Value = 0;
                        numeric.Text = "";
                    }
                    lblTotalGeneral.Text = "$0";

                    // Actualizar todos los labels de totales parciales
                    foreach (var labelTotal in labelsTotal)
                    {
                        labelTotal.Text = "$0";
                    }
                };

                // Botón Cerrar
                var btnCerrar = new Button()
                {
                    Text = "CERRAR",
                    BackColor = Color.FromArgb(45, 66, 91),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Location = new Point(250, yPos + 70),
                    Size = new Size(150, 40),
                    DialogResult = DialogResult.OK
                };

                formBilletes.Controls.AddRange(new Control[] {
            lblTitulo, lineSeparator, lblTotalTexto, lblTotalGeneral, btnLimpiar, btnCerrar
        });

                formBilletes.AcceptButton = btnCerrar;

                // Mostrar el formulario
                formBilletes.ShowDialog();
            }
        }

        private void CalcularTotales(NumericUpDown[] numericControls, decimal[] denominaciones, Label[] labelsTotal, Label lblTotalGeneral)
        {
            decimal totalGeneral = 0;

            for (int i = 0; i < numericControls.Length; i++)
            {
                decimal cantidad = numericControls[i].Value;
                decimal denominacion = denominaciones[i];
                decimal totalParcial = cantidad * denominacion;

                // Actualizar total parcial
                labelsTotal[i].Text = $"${totalParcial:N0}";

                totalGeneral += totalParcial;
            }

            // Actualizar total general
            lblTotalGeneral.Text = $"${totalGeneral:N0}";

            // Cambiar color según el monto
            if (totalGeneral > 0)
            {
                lblTotalGeneral.ForeColor = Color.ForestGreen;
            }
            else
            {
                lblTotalGeneral.ForeColor = Color.Gray;
            }
        }

        private void btnRetirarDinero_Click(object sender, EventArgs e)
        {
            MostrarFormRetiroDinero();
        }

        // Evento click para el botón de cierre de caja
        private void btnCerrarCaja_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboCajas.SelectedIndex < 0)
                    return;

                Caja cajaSeleccionada = _cajasDisponibles[comboCajas.SelectedIndex];

                if (cajaSeleccionada.FechaCierre != null)
                {
                    MessageBox.Show("Esta caja ya está cerrada", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (cajaSeleccionada.FechaApertura.Date != DateTime.Today)
                {
                    MessageBox.Show("Solo se puede cerrar la caja del día actual", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                decimal saldoFinal;
                if (!decimal.TryParse(labelSaldoFinal.Text.Replace("Saldo Final: ", ""),
                        NumberStyles.Currency, new CultureInfo("es-AR"), out saldoFinal))
                {
                    MessageBox.Show("No se pudo interpretar el saldo final correctamente", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (saldoFinal < 0)
                {
                    var respuesta = MessageBox.Show(
                        $"El saldo final es negativo ({saldoFinal:C}). ¿Desea cerrar la caja igualmente?",
                        "Confirmar cierre",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (respuesta == DialogResult.No)
                        return;
                }

                if (saldoFinal > 99999999.99m)
                {
                    MessageBox.Show("El saldo final es demasiado alto. Contacte al administrador", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show($"¿Está seguro de cerrar la caja con saldo final: {saldoFinal:C}?",
                    "Confirmar Cierre", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (_arqueoService.CerrarCaja(DateTime.Today, saldoFinal))
                    {
                        MessageBox.Show("Caja cerrada correctamente.", "Información",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        labelCaja.Text = "Caja Cerrada";
                        CargarCajasDisponibles();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo cerrar la caja.", "Advertencia",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar caja: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Mostrar form de retiro de dinero
        private void MostrarFormRetiroDinero()
        {
            using (var formRetiro = new Form())
            {
                formRetiro.Text = "Retirar Dinero";
                formRetiro.Size = new Size(400, 350);
                formRetiro.StartPosition = FormStartPosition.CenterScreen;
                formRetiro.FormBorderStyle = FormBorderStyle.FixedDialog;
                formRetiro.MaximizeBox = false;
                formRetiro.MinimizeBox = false;

                var lblTitulo = new Label()
                {
                    Text = "RETIRO DE DINERO",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.FromArgb(45, 66, 91),
                    Location = new Point(20, 20),
                    Size = new Size(350, 30),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                var lblDescripcion = new Label()
                {
                    Text = "Descripción:",
                    Font = new Font("Segoe UI", 11, FontStyle.Regular),
                    Location = new Point(50, 70),
                    Size = new Size(100, 25)
                };

                var txtDescripcion = new TextBox()
                {
                    Location = new Point(160, 70),
                    Size = new Size(180, 25),
                    Font = new Font("Segoe UI", 10)
                };

                var lblMonto = new Label()
                {
                    Text = "Monto:",
                    Font = new Font("Segoe UI", 11, FontStyle.Regular),
                    Location = new Point(50, 110),
                    Size = new Size(100, 25)
                };

                var txtMonto = new NumericUpDown()
                {
                    Location = new Point(160, 110),
                    Size = new Size(150, 25),
                    Minimum = 0,
                    Maximum = 9999999999999,
                    DecimalPlaces = 2,
                    Value = 0
                };

                // Hacer que el campo aparezca vacío en lugar de 0,00
                txtMonto.Text = "";

                var lblMetodoPago = new Label()
                {
                    Text = "Método de Pago:",
                    Font = new Font("Segoe UI", 11, FontStyle.Regular),
                    Location = new Point(50, 150),
                    Size = new Size(120, 25)
                };

                var comboMetodoPago = new ComboBox()
                {
                    Location = new Point(160, 150),
                    Size = new Size(150, 25),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                // Agregar métodos de pago para retiro (sin CUENTA CORRIENTE)
                comboMetodoPago.Items.AddRange(new string[] {
                    "EFECTIVO",
                    "TRANSFERENCIA",
                    "BILLETERA VIRTUAL",
                    "TARJETA"
                });

                comboMetodoPago.SelectedIndex = 0;

                var btnRetirar = new Button()
                {
                    Text = "RETIRAR DINERO",
                    BackColor = Color.FromArgb(183, 28, 28),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Location = new Point(70, 220),
                    Size = new Size(120, 40),
                    DialogResult = DialogResult.OK
                };

                var btnCancelar = new Button()
                {
                    Text = "CANCELAR",
                    BackColor = Color.Gray,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Location = new Point(200, 220),
                    Size = new Size(120, 40),
                    DialogResult = DialogResult.Cancel
                };

                btnRetirar.Click += (s, e) =>
                {
                    // Validar campos
                    if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                    {
                        MessageBox.Show("Por favor, ingrese una descripción.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (txtMonto.Value <= 0)
                    {
                        MessageBox.Show("El monto debe ser mayor a cero.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Obtener el saldo disponible por método de pago
                    DateTime fechaCaja = DateTime.Today;
                    var saldosPorMetodo = _arqueoService.ObtenerSaldosPorMetodoPago(fechaCaja);
                    string metodoPago = comboMetodoPago.SelectedItem.ToString();

                    // Verificar si hay saldo suficiente para el método de pago seleccionado
                    if (saldosPorMetodo.ContainsKey(metodoPago) && txtMonto.Value > saldosPorMetodo[metodoPago])
                    {
                        MessageBox.Show($"No se puede retirar ${txtMonto.Value:N2} en {metodoPago}.\n\n" +
                            $"El saldo disponible en {metodoPago} es: ${saldosPorMetodo[metodoPago]:N2}\n\n" +
                            "Por favor, ingrese un monto menor o igual al saldo disponible.",
                            "Saldo Insuficiente por Método de Pago",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Obtener el saldo actual de la caja
                    decimal saldoActual = _arqueoService.ObtenerSaldoActualCaja();

                    if (txtMonto.Value > saldoActual)
                    {
                        MessageBox.Show($"No se puede retirar ${txtMonto.Value:N2}.\n\n" +
                            $"El saldo disponible en caja es: ${saldoActual:N2}",
                            "Saldo Insuficiente",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Mostrar confirmación con detalles del retiro
                    string mensaje = $"¿Está seguro que desea retirar ${txtMonto.Value:N2}?\n\n" +
                                   $"Descripción: {txtDescripcion.Text}\n" +
                                   $"Método de Pago: {metodoPago}\n" +
                                   $"Saldo después del retiro: ${saldoActual - txtMonto.Value:N2}\n\n" +
                                   "Esta acción registrará el retiro como un gasto en el sistema.";

                    DialogResult resultado = MessageBox.Show(mensaje, "Confirmar Retiro",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                    {
                        // Registrar el retiro
                        if (_arqueoService.RegistrarRetiroDinero(
                            txtDescripcion.Text,
                            txtMonto.Value,
                            DateTime.Now,
                            metodoPago))
                        {
                            MessageBox.Show("Retiro registrado correctamente.", "Éxito",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            formRetiro.DialogResult = DialogResult.OK;
                            CargarDatosCajaSeleccionada();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo registrar el retiro.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                };

                formRetiro.Controls.AddRange(new Control[] {
                    lblTitulo, lblDescripcion, txtDescripcion, lblMonto, txtMonto,
                    lblMetodoPago, comboMetodoPago, btnRetirar, btnCancelar
                });

                formRetiro.AcceptButton = btnRetirar;
                formRetiro.CancelButton = btnCancelar;

                formRetiro.ShowDialog();
            }
        }

    }
}
