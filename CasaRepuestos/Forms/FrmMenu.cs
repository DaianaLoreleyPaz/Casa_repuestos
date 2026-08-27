using CasaRepuestos.Services;


namespace CasaRepuestos.Forms
{
    public partial class FrmMenu : Form
    {
        private readonly string _userRole;
        private readonly string _userName;
        private readonly int _empleadoId;

        private Button _activeButton;
        private Panel _panelIndicator;

        public FrmMenu(string role, string usuario, int empleadoId)
        {
            InitializeComponent();
            _userRole = role;
            _userName = usuario;
            _empleadoId = empleadoId;
            SesionService.IniciarSesion(_empleadoId, _userName, _userRole);
            labelRol.Text = $"Rol: {_userRole}";
            labelUsuario.Text = $"Usuario: {_userName}";

            // permisos
            btnCliente.Visible = MenuService.PuedeVerClientes(_userRole);
            btnCompras.Visible = MenuService.PuedeVerCompras(_userRole);
            btnProveedor.Visible = MenuService.PuedeVerProveedor(_userRole);
            btnProductos.Visible = MenuService.PuedeVerProductos(_userRole);
            btnInventario.Visible = MenuService.PuedeVerInventario(_userRole);
            btnIngresos.Visible = MenuService.PuedeVerIngresos(_userRole);
            btnServicios.Visible = MenuService.PuedeVerServicios(_userRole);
            btnPresupuesto.Visible = MenuService.PuedeVerPresupuesto(_userRole);
            btnReparaciones.Visible = MenuService.PuedeVerReparaciones(_userRole);
            btnReportes.Visible = MenuService.PuedeVerReportes(_userRole);
            btnArqueo.Visible = MenuService.PuedeVerArqueo(_userRole);
            btnConfiguracion.Visible = MenuService.PuedeVerConfiguracion(_userRole);
            btnMarca.Visible = MenuService.PuedeVerMarca(_userRole);
            btnVentas.Visible = MenuService.PuedeVerVentas(_userRole);

            // Mostrar alertas administrativas si el usuario es ADMINISTRADOR
            if (_userRole == "ADMINISTRADOR")
            {
                MostrarAlertasAdministrativas();
            }
            _panelIndicator = new Panel
            {
                Size = new Size(6, 45),
                BackColor = Color.FromArgb(34, 139, 34),
                Visible = false
            };
            panelSidebar.Controls.Add(_panelIndicator);
            _panelIndicator.BringToFront();
        }

        private void CargarFormularioEnPanel(Form modulo)
        {
            try
            {
                //Cerrar y liberar el formulario anterior 
                foreach (Control ctrl in panelMain.Controls)
                {
                    if (ctrl is Form formActual)
                    {
                        formActual.Close();
                        formActual.Dispose();
                    }
                }

                panelMain.Controls.Clear();

                // Configura el nuevo formulario
                modulo.TopLevel = false;
                modulo.FormBorderStyle = FormBorderStyle.None;
                modulo.Dock = DockStyle.Fill;

                // Agregar al panel
                panelMain.Controls.Add(modulo);
                modulo.Show();
                modulo.BringToFront();
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el formulario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para activar el botón seleccionado
        private void ActivateButton(Button btn)
        {
            if (btn == null) return;

            if (_activeButton != null)
            {
                _activeButton.BackColor = Color.FromArgb(76, 132, 200);
                _activeButton.ForeColor = Color.White;
            }

            _activeButton = btn;
            _activeButton.BackColor = Color.FromArgb(34, 139, 34);
            _activeButton.ForeColor = Color.White;

            _panelIndicator.Height = _activeButton.Height;
            _panelIndicator.Top = _activeButton.Top;
            _panelIndicator.Left = 0;
            _panelIndicator.Visible = true;
            _panelIndicator.BringToFront();
        }
        // Manejadores de eventos de los botones del menú
        private void btnClientes_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            if (MenuService.PuedeVerClientes(_userRole))
                CargarFormularioEnPanel(new FrmCliente());
        }

        private void btnCompras_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            if (MenuService.PuedeVerCompras(_userRole))
                CargarFormularioEnPanel(new FrmCompras());
        }

        private void btnProveedor_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            if (MenuService.PuedeVerProveedor(_userRole))
                CargarFormularioEnPanel(new FrmProveedor());
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            if (MenuService.PuedeVerVentas(_userRole))
                CargarFormularioEnPanel(new FrmFactura());
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            if (MenuService.PuedeVerProductos(_userRole))
                CargarFormularioEnPanel(new FrmArticulo());
        }

        private void btnInventario_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            if (MenuService.PuedeVerInventario(_userRole))
                CargarFormularioEnPanel(new FrmInventario());
        }

        private void btnIngresos_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            if (MenuService.PuedeVerIngresos(_userRole))
                CargarFormularioEnPanel(new FrmIngreso());
        }

        private void btnPresupuesto_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            if (MenuService.PuedeVerPresupuesto(_userRole))
                CargarFormularioEnPanel(new FrmPresupuestos(_empleadoId, _userRole));
        }

        private void btnReparaciones_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            if (MenuService.PuedeVerReparaciones(_userRole))
                CargarFormularioEnPanel(new FrmReparaciones());
        }

        private void btnServicios_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            if (MenuService.PuedeVerServicios(_userRole))
                CargarFormularioEnPanel(new FrmServicios());
        }

        private void btnArqueo_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            if (MenuService.PuedeVerArqueo(_userRole))
                CargarFormularioEnPanel(new FrmArqueo());
        }

        private void btnConfiguracion_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            if (MenuService.PuedeVerConfiguracion(_userRole))
                CargarFormularioEnPanel(new FrmEmpleados());
        }

        private void btnMarca_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            if (MenuService.PuedeVerMarca(_userRole))
                CargarFormularioEnPanel(new FrmMarca());
        }
        private void btnReportes_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            if (MenuService.PuedeVerReportes(_userRole))
                CargarFormularioEnPanel(new FrmReportes());
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            // Verificar si el usuario es cajero o administrador 
            if (_userRole == "CAJERO" || _userRole == "ADMINISTRADOR")
            {
                // Crear instancia del servicio de arqueo
                ArqueoService arqueoService = new ArqueoService();
                
                // Verificar si hay una caja abierta hoy
                if (arqueoService.ExisteCajaAbiertaHoy())
                {
                    try
                    {
                        // Obtener la caja abierta
                        var cajaAbierta = arqueoService.ObtenerCajaAbiertaHoy();
                        if (cajaAbierta != null)
                        {
                            // Calcular el saldo final
                            decimal saldoFinal = arqueoService.ObtenerSaldoActualCaja();
                            
                            // Verificar condiciones especiales del saldo
                            if (saldoFinal < 0)
                            {
                                var respuesta = MessageBox.Show(
                                    $"El saldo final es negativo ({saldoFinal:C}). ¿Desea cerrar la caja igualmente?",
                                    "Confirmar cierre",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Warning
                                );
                                if (respuesta == DialogResult.No)
                                {
                                    // Si el usuario no desea cerrar con saldo negativo, mostrar mensaje informativo
                                    MessageBox.Show("La caja permanecerá abierta con saldo negativo.", "Información",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            
                            if (saldoFinal > 99999999.99m)
                            {
                                MessageBox.Show("El saldo final es demasiado alto. Contacte al administrador.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            
                            // Mostrar confirmación con saldo final 
                            DialogResult resultado = MessageBox.Show(
                                $"¿Está seguro de cerrar la caja con saldo final: {saldoFinal:C}?\n\n" +
                                "Si selecciona 'No', la caja permanecerá abierta y se cerrará su sesión.",
                                "Confirmar Cierre", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            
                            // Si el usuario confirma, cerrar la caja
                            if (resultado == DialogResult.Yes)
                            {
                                if (arqueoService.CerrarCaja(DateTime.Today, saldoFinal))
                                {
                                    MessageBox.Show("Caja cerrada correctamente.", "Información",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("No se pudo cerrar la caja. La sesión no se cerrará.", "Advertencia",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return; // No cerrar la sesión si no se pudo cerrar la caja
                                }
                            }
                            else
                            {
                                // Si el usuario cancela, mostrar mensaje informativo
                                MessageBox.Show("La caja permanecerá abierta. Continuando con el cierre de sesión.", "Información",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al procesar el cierre de caja: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // No cerrar la sesión si hubo un error
                    }
                }
            }
            
            // Cerrar sesión 
            SesionService.CerrarSesion();
            FrmLogin login = new FrmLogin();
            login.Show();
            this.Close();
        }

        private void MostrarAlertasAdministrativas()
        {
            try
            {
                // Inicializar ambos servicios
                PresupuestoService presupuestoService = new PresupuestoService();
                ComprasService comprasService = new ComprasService();

                // Contar Presupuestos por verificar PRECIO 
                int presupuestosVerificar = presupuestoService.ContarPresupuestosPorVerificarPrecio();

                // Contar Presupuestos que están ESPERANDO REPUESTOS
                int comprasRequeridas = comprasService.ContarPresupuestosEnEsperaDeRepuestos(); 

                string mensajeAlerta = "";

                if (presupuestosVerificar > 0)
                {
                    mensajeAlerta += $"Tienes {presupuestosVerificar} presupuesto(s) pendiente(s) de VERIFICAR PRECIO.\n";
                }

                if (comprasRequeridas > 0)
                {
                    mensajeAlerta += $"Tienes {comprasRequeridas} presupuesto(s) pendiente(s) por COMPRA DE REPUESTOS.\n";
                }

                if (!string.IsNullOrEmpty(mensajeAlerta))
                {
                    MessageBox.Show(
                        mensajeAlerta,
                        "Alertas Administrativas Pendientes",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar alertas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        
    }
}
