namespace CasaRepuestos.Forms
{
    partial class FrmMenu
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            btnConfiguracion = new Button();
            btnArqueo = new Button();
            btnIngresos = new Button();
            btnPresupuesto = new Button();
            btnReparaciones = new Button();
            btnInventario = new Button();
            btnProductos = new Button();
            btnCompras = new Button();
            btnCliente = new Button();
            panelMain = new Panel();
            labelUsuario = new Label();
            labelRol = new Label();
            btnCerrarSesion = new Button();
            btnMarca = new Button();
            panelSidebar = new Panel();
            btnReportes = new Button();
            btnServicios = new Button();
            btnProveedor = new Button();
            btnVentas = new Button();
            labelTitulo = new Label();
            panelSidebar.SuspendLayout();
            SuspendLayout();
            // 
            // btnConfiguracion
            // 
            btnConfiguracion.BackColor = Color.FromArgb(76, 132, 200);
            btnConfiguracion.FlatAppearance.BorderSize = 0;
            btnConfiguracion.FlatStyle = FlatStyle.Flat;
            btnConfiguracion.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnConfiguracion.ForeColor = Color.White;
            btnConfiguracion.Location = new Point(20, 732);
            btnConfiguracion.Name = "btnConfiguracion";
            btnConfiguracion.Size = new Size(240, 45);
            btnConfiguracion.TabIndex = 13;
            btnConfiguracion.Text = "⚙️ CONFIGURACIÓN";
            btnConfiguracion.TextAlign = ContentAlignment.MiddleLeft;
            btnConfiguracion.UseVisualStyleBackColor = false;
            btnConfiguracion.Click += btnConfiguracion_Click;
            // 
            // btnArqueo
            // 
            btnArqueo.BackColor = Color.FromArgb(76, 132, 200);
            btnArqueo.FlatAppearance.BorderSize = 0;
            btnArqueo.FlatStyle = FlatStyle.Flat;
            btnArqueo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnArqueo.ForeColor = Color.White;
            btnArqueo.Location = new Point(20, 630);
            btnArqueo.Name = "btnArqueo";
            btnArqueo.Size = new Size(240, 45);
            btnArqueo.TabIndex = 12;
            btnArqueo.Text = "🔍 ARQUEO";
            btnArqueo.TextAlign = ContentAlignment.MiddleLeft;
            btnArqueo.UseVisualStyleBackColor = false;
            btnArqueo.Click += btnArqueo_Click;
            // 
            // btnIngresos
            // 
            btnIngresos.BackColor = Color.FromArgb(76, 132, 200);
            btnIngresos.FlatAppearance.BorderSize = 0;
            btnIngresos.FlatStyle = FlatStyle.Flat;
            btnIngresos.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnIngresos.ForeColor = Color.White;
            btnIngresos.Location = new Point(20, 430);
            btnIngresos.Name = "btnIngresos";
            btnIngresos.Size = new Size(240, 45);
            btnIngresos.TabIndex = 8;
            btnIngresos.Text = "📱 INGRESOS";
            btnIngresos.TextAlign = ContentAlignment.MiddleLeft;
            btnIngresos.UseVisualStyleBackColor = false;
            btnIngresos.Click += btnIngresos_Click;
            // 
            // btnPresupuesto
            // 
            btnPresupuesto.BackColor = Color.FromArgb(76, 132, 200);
            btnPresupuesto.FlatAppearance.BorderSize = 0;
            btnPresupuesto.FlatStyle = FlatStyle.Flat;
            btnPresupuesto.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnPresupuesto.ForeColor = Color.White;
            btnPresupuesto.Location = new Point(20, 530);
            btnPresupuesto.Name = "btnPresupuesto";
            btnPresupuesto.Size = new Size(240, 45);
            btnPresupuesto.TabIndex = 10;
            btnPresupuesto.Text = "📑 PRESUPUESTOS";
            btnPresupuesto.TextAlign = ContentAlignment.MiddleLeft;
            btnPresupuesto.UseVisualStyleBackColor = false;
            btnPresupuesto.Click += btnPresupuesto_Click;
            // 
            // btnReparaciones
            // 
            btnReparaciones.BackColor = Color.FromArgb(76, 132, 200);
            btnReparaciones.FlatAppearance.BorderSize = 0;
            btnReparaciones.FlatStyle = FlatStyle.Flat;
            btnReparaciones.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnReparaciones.ForeColor = Color.White;
            btnReparaciones.Location = new Point(20, 580);
            btnReparaciones.Name = "btnReparaciones";
            btnReparaciones.Size = new Size(240, 45);
            btnReparaciones.TabIndex = 11;
            btnReparaciones.Text = "🔧 REPARACIONES";
            btnReparaciones.TextAlign = ContentAlignment.MiddleLeft;
            btnReparaciones.UseVisualStyleBackColor = false;
            btnReparaciones.Click += btnReparaciones_Click;
            // 
            // btnInventario
            // 
            btnInventario.BackColor = Color.FromArgb(76, 132, 200);
            btnInventario.FlatAppearance.BorderSize = 0;
            btnInventario.FlatStyle = FlatStyle.Flat;
            btnInventario.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnInventario.ForeColor = Color.White;
            btnInventario.Location = new Point(20, 380);
            btnInventario.Name = "btnInventario";
            btnInventario.Size = new Size(240, 45);
            btnInventario.TabIndex = 7;
            btnInventario.Text = "📋 INVENTARIO";
            btnInventario.TextAlign = ContentAlignment.MiddleLeft;
            btnInventario.UseVisualStyleBackColor = false;
            btnInventario.Click += btnInventario_Click;
            // 
            // btnProductos
            // 
            btnProductos.BackColor = Color.FromArgb(76, 132, 200);
            btnProductos.FlatAppearance.BorderSize = 0;
            btnProductos.FlatStyle = FlatStyle.Flat;
            btnProductos.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnProductos.ForeColor = Color.White;
            btnProductos.Location = new Point(20, 330);
            btnProductos.Name = "btnProductos";
            btnProductos.Size = new Size(240, 45);
            btnProductos.TabIndex = 6;
            btnProductos.Text = "📦 PRODUCTOS";
            btnProductos.TextAlign = ContentAlignment.MiddleLeft;
            btnProductos.UseVisualStyleBackColor = false;
            btnProductos.Click += btnProductos_Click;
            // 
            // btnCompras
            // 
            btnCompras.BackColor = Color.FromArgb(76, 132, 200);
            btnCompras.FlatAppearance.BorderSize = 0;
            btnCompras.FlatStyle = FlatStyle.Flat;
            btnCompras.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnCompras.ForeColor = Color.White;
            btnCompras.Location = new Point(20, 180);
            btnCompras.Name = "btnCompras";
            btnCompras.Size = new Size(240, 45);
            btnCompras.TabIndex = 3;
            btnCompras.Text = "🛍️ COMPRAS";
            btnCompras.TextAlign = ContentAlignment.MiddleLeft;
            btnCompras.UseVisualStyleBackColor = false;
            btnCompras.Click += btnCompras_Click;
            // 
            // btnCliente
            // 
            btnCliente.BackColor = Color.FromArgb(76, 132, 200);
            btnCliente.FlatAppearance.BorderSize = 0;
            btnCliente.FlatStyle = FlatStyle.Flat;
            btnCliente.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnCliente.ForeColor = Color.White;
            btnCliente.Location = new Point(20, 80);
            btnCliente.Name = "btnCliente";
            btnCliente.Size = new Size(240, 45);
            btnCliente.TabIndex = 1;
            btnCliente.Text = "👥 CLIENTES";
            btnCliente.TextAlign = ContentAlignment.MiddleLeft;
            btnCliente.UseVisualStyleBackColor = false;
            btnCliente.Click += btnClientes_Click;
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.FromArgb(248, 249, 250);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(280, 0);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1622, 1033);
            panelMain.TabIndex = 14;
            // 
            // labelUsuario
            // 
            labelUsuario.AutoSize = true;
            labelUsuario.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            labelUsuario.ForeColor = Color.White;
            labelUsuario.Location = new Point(20, 836);
            labelUsuario.Name = "labelUsuario";
            labelUsuario.Size = new Size(75, 23);
            labelUsuario.TabIndex = 15;
            labelUsuario.Text = "Usuario:";
            // 
            // labelRol
            // 
            labelRol.AutoSize = true;
            labelRol.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            labelRol.ForeColor = Color.White;
            labelRol.Location = new Point(20, 861);
            labelRol.Name = "labelRol";
            labelRol.Size = new Size(41, 23);
            labelRol.TabIndex = 16;
            labelRol.Text = "Rol:";
            // 
            // btnCerrarSesion
            // 
            btnCerrarSesion.BackColor = Color.FromArgb(220, 53, 69);
            btnCerrarSesion.FlatAppearance.BorderSize = 0;
            btnCerrarSesion.FlatStyle = FlatStyle.Flat;
            btnCerrarSesion.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCerrarSesion.ForeColor = Color.White;
            btnCerrarSesion.Location = new Point(20, 898);
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.Size = new Size(240, 40);
            btnCerrarSesion.TabIndex = 17;
            btnCerrarSesion.Text = "🚪 Cerrar Sesión";
            btnCerrarSesion.UseVisualStyleBackColor = false;
            btnCerrarSesion.Click += btnCerrarSesion_Click;
            // 
            // btnMarca
            // 
            btnMarca.BackColor = Color.FromArgb(76, 132, 200);
            btnMarca.FlatAppearance.BorderSize = 0;
            btnMarca.FlatStyle = FlatStyle.Flat;
            btnMarca.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnMarca.ForeColor = Color.White;
            btnMarca.Location = new Point(20, 280);
            btnMarca.Name = "btnMarca";
            btnMarca.Size = new Size(240, 45);
            btnMarca.TabIndex = 5;
            btnMarca.Text = "🏷️ MARCAS";
            btnMarca.TextAlign = ContentAlignment.MiddleLeft;
            btnMarca.UseVisualStyleBackColor = false;
            btnMarca.Click += btnMarca_Click;
            // 
            // panelSidebar
            // 
            panelSidebar.BackColor = Color.FromArgb(45, 66, 91);
            panelSidebar.Controls.Add(btnReportes);
            panelSidebar.Controls.Add(btnServicios);
            panelSidebar.Controls.Add(btnProveedor);
            panelSidebar.Controls.Add(btnVentas);
            panelSidebar.Controls.Add(labelTitulo);
            panelSidebar.Controls.Add(btnCliente);
            panelSidebar.Controls.Add(btnCompras);
            panelSidebar.Controls.Add(btnMarca);
            panelSidebar.Controls.Add(btnProductos);
            panelSidebar.Controls.Add(btnInventario);
            panelSidebar.Controls.Add(btnIngresos);
            panelSidebar.Controls.Add(btnPresupuesto);
            panelSidebar.Controls.Add(btnReparaciones);
            panelSidebar.Controls.Add(btnArqueo);
            panelSidebar.Controls.Add(btnConfiguracion);
            panelSidebar.Controls.Add(labelUsuario);
            panelSidebar.Controls.Add(labelRol);
            panelSidebar.Controls.Add(btnCerrarSesion);
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Location = new Point(0, 0);
            panelSidebar.Name = "panelSidebar";
            panelSidebar.Size = new Size(280, 1033);
            panelSidebar.TabIndex = 19;
            // 
            // btnReportes
            // 
            btnReportes.BackColor = Color.FromArgb(76, 132, 200);
            btnReportes.FlatAppearance.BorderSize = 0;
            btnReportes.FlatStyle = FlatStyle.Flat;
            btnReportes.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnReportes.ForeColor = Color.White;
            btnReportes.Location = new Point(20, 681);
            btnReportes.Name = "btnReportes";
            btnReportes.Size = new Size(240, 45);
            btnReportes.TabIndex = 21;
            btnReportes.Text = "📑 REPORTES";
            btnReportes.TextAlign = ContentAlignment.MiddleLeft;
            btnReportes.UseVisualStyleBackColor = false;
            btnReportes.Click += btnReportes_Click;
            // 
            // btnServicios
            // 
            btnServicios.BackColor = Color.FromArgb(76, 132, 200);
            btnServicios.FlatAppearance.BorderSize = 0;
            btnServicios.FlatStyle = FlatStyle.Flat;
            btnServicios.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnServicios.ForeColor = Color.White;
            btnServicios.Location = new Point(20, 480);
            btnServicios.Name = "btnServicios";
            btnServicios.Size = new Size(240, 45);
            btnServicios.TabIndex = 9;
            btnServicios.Text = "💼 SERVICIOS";
            btnServicios.TextAlign = ContentAlignment.MiddleLeft;
            btnServicios.UseVisualStyleBackColor = false;
            btnServicios.Click += btnServicios_Click;
            // 
            // btnProveedor
            // 
            btnProveedor.BackColor = Color.FromArgb(76, 132, 200);
            btnProveedor.FlatAppearance.BorderSize = 0;
            btnProveedor.FlatStyle = FlatStyle.Flat;
            btnProveedor.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnProveedor.ForeColor = Color.White;
            btnProveedor.Location = new Point(20, 130);
            btnProveedor.Name = "btnProveedor";
            btnProveedor.Size = new Size(240, 45);
            btnProveedor.TabIndex = 2;
            btnProveedor.Text = "🏭 PROVEEDORES";
            btnProveedor.TextAlign = ContentAlignment.MiddleLeft;
            btnProveedor.UseVisualStyleBackColor = false;
            btnProveedor.Click += btnProveedor_Click;
            // 
            // btnVentas
            // 
            btnVentas.BackColor = Color.FromArgb(76, 132, 200);
            btnVentas.FlatAppearance.BorderSize = 0;
            btnVentas.FlatStyle = FlatStyle.Flat;
            btnVentas.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnVentas.ForeColor = Color.White;
            btnVentas.Location = new Point(20, 230);
            btnVentas.Name = "btnVentas";
            btnVentas.Size = new Size(240, 45);
            btnVentas.TabIndex = 4;
            btnVentas.Text = "\U0001f9fe VENTAS";
            btnVentas.TextAlign = ContentAlignment.MiddleLeft;
            btnVentas.UseVisualStyleBackColor = false;
            btnVentas.Click += btnVentas_Click;
            // 
            // labelTitulo
            // 
            labelTitulo.AutoSize = true;
            labelTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            labelTitulo.ForeColor = Color.White;
            labelTitulo.Location = new Point(20, 25);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(254, 37);
            labelTitulo.TabIndex = 20;
            labelTitulo.Text = "Casa de Repuestos";
            labelTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmMenu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 249, 250);
            ClientSize = new Size(1902, 1033);
            Controls.Add(panelMain);
            Controls.Add(panelSidebar);
            Name = "FrmMenu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Casa de Repuestos - Sistema de Gestión";
            WindowState = FormWindowState.Maximized;
            panelSidebar.ResumeLayout(false);
            panelSidebar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button btnConfiguracion;
        private Button btnArqueo;
        private Button btnIngresos;
        private Button btnPresupuesto;
        private Button btnReparaciones;
        private Button btnInventario;
        private Button btnProductos;
        private Button btnCompras;
        private Button btnCliente;
        private Panel panelMain;
        private Label labelUsuario;
        private Label labelRol;
        private Button btnCerrarSesion;
        private Button btnMarca;
        private Panel panelSidebar;
        private Label labelTitulo;
        private Button btnProveedor;
        private Button btnVentas;
        private Button btnServicios;
        private Button btnReportes;
    }
}
