// FrmPresupuestos.Designer.cs
namespace CasaRepuestos.Forms
{
    
    partial class FrmPresupuestos
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            btnEnviarCliente = new Button();
            btnAprobarAdmin = new Button();
            btnAutorizar = new Button();
            btnLimpiar = new Button();
            btnImprimir = new Button();
            btnGuardar = new Button();
            groupIngreso = new GroupBox();
            txtIdIngreso = new TextBox();
            txtMarcaIngreso = new TextBox();
            txtTipoDispositivo = new TextBox();
            txtFalla = new TextBox();
            txtFecha = new TextBox();
            lblIdIngreso = new Label();
            txtModelo = new TextBox();
            lblMarca = new Label();
            lblModelo = new Label();
            lblTipo = new Label();
            lblFalla = new Label();
            lblFecha = new Label();
            dgvIngresos = new DataGridView();
            groupDetalles = new GroupBox();
            btnAgregarServicio = new Button();
            cbServicios = new ComboBox();
            cbRepuestos = new ComboBox();
            btnAgregarRepuesto = new Button();
            lblStockInfo = new Label();
            dgvDetalles = new DataGridView();
            txtTotal = new TextBox();
            lblTotal = new Label();
            dgvPresupuestos = new DataGridView();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            panel1 = new Panel();
            lblTitulo = new Label();
            label1 = new Label();
            groupIngreso.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvIngresos).BeginInit();
            groupDetalles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetalles).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPresupuestos).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnEnviarCliente
            // 
            btnEnviarCliente.FlatStyle = FlatStyle.Flat;
            btnEnviarCliente.Location = new Point(867, 641);
            btnEnviarCliente.Name = "btnEnviarCliente";
            btnEnviarCliente.Size = new Size(116, 35);
            btnEnviarCliente.TabIndex = 2;
            btnEnviarCliente.Text = "Enviar Cliente";
            btnEnviarCliente.Click += BtnEnviarCliente_Click;
            // 
            // btnAprobarAdmin
            // 
            btnAprobarAdmin.FlatStyle = FlatStyle.Flat;
            btnAprobarAdmin.Location = new Point(696, 641);
            btnAprobarAdmin.Name = "btnAprobarAdmin";
            btnAprobarAdmin.Size = new Size(165, 35);
            btnAprobarAdmin.TabIndex = 1;
            btnAprobarAdmin.Text = "Aprobar Presupuesto";
            btnAprobarAdmin.Click += BtnAprobarAdmin_Click;
            // 
            // btnAutorizar
            // 
            btnAutorizar.FlatStyle = FlatStyle.Flat;
            btnAutorizar.Location = new Point(315, 638);
            btnAutorizar.Name = "btnAutorizar";
            btnAutorizar.Size = new Size(129, 38);
            btnAutorizar.TabIndex = 5;
            btnAutorizar.Text = "Autorizar";
            btnAutorizar.Click += btnAutorizar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.Location = new Point(210, 639);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(99, 36);
            btnLimpiar.TabIndex = 4;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnImprimir
            // 
            btnImprimir.FlatStyle = FlatStyle.Flat;
            btnImprimir.Location = new Point(1235, 0);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(93, 35);
            btnImprimir.TabIndex = 6;
            btnImprimir.Text = "Imprimir";
            btnImprimir.Visible = false;
            btnImprimir.Click += btnImprimir_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Location = new Point(119, 639);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(85, 35);
            btnGuardar.TabIndex = 3;
            btnGuardar.Text = "Guardar";
            btnGuardar.Click += btnGuardar_Click;
            // 
            // groupIngreso
            // 
            groupIngreso.BackColor = SystemColors.ControlLightLight;
            groupIngreso.Controls.Add(txtIdIngreso);
            groupIngreso.Controls.Add(txtMarcaIngreso);
            groupIngreso.Controls.Add(txtTipoDispositivo);
            groupIngreso.Controls.Add(txtFalla);
            groupIngreso.Controls.Add(txtFecha);
            groupIngreso.Controls.Add(lblIdIngreso);
            groupIngreso.Controls.Add(txtModelo);
            groupIngreso.Controls.Add(lblMarca);
            groupIngreso.Controls.Add(lblModelo);
            groupIngreso.Controls.Add(lblTipo);
            groupIngreso.Controls.Add(lblFalla);
            groupIngreso.Controls.Add(lblFecha);
            groupIngreso.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupIngreso.Location = new Point(99, 96);
            groupIngreso.Name = "groupIngreso";
            groupIngreso.Size = new Size(520, 240);
            groupIngreso.TabIndex = 1;
            groupIngreso.TabStop = false;
            groupIngreso.Text = "Datos del Ingreso";
            // 
            // txtIdIngreso
            // 
            txtIdIngreso.BackColor = SystemColors.ControlLightLight;
            txtIdIngreso.Location = new Point(135, 31);
            txtIdIngreso.Name = "txtIdIngreso";
            txtIdIngreso.ReadOnly = true;
            txtIdIngreso.Size = new Size(100, 27);
            txtIdIngreso.TabIndex = 0;
            // 
            // txtMarcaIngreso
            // 
            txtMarcaIngreso.BackColor = SystemColors.ControlLightLight;
            txtMarcaIngreso.Location = new Point(134, 96);
            txtMarcaIngreso.Name = "txtMarcaIngreso";
            txtMarcaIngreso.ReadOnly = true;
            txtMarcaIngreso.Size = new Size(200, 27);
            txtMarcaIngreso.TabIndex = 1;
            // 
            // txtTipoDispositivo
            // 
            txtTipoDispositivo.BackColor = SystemColors.ControlLightLight;
            txtTipoDispositivo.Location = new Point(134, 196);
            txtTipoDispositivo.Name = "txtTipoDispositivo";
            txtTipoDispositivo.ReadOnly = true;
            txtTipoDispositivo.Size = new Size(200, 27);
            txtTipoDispositivo.TabIndex = 3;
            // 
            // txtFalla
            // 
            txtFalla.BackColor = SystemColors.ControlLightLight;
            txtFalla.Location = new Point(134, 129);
            txtFalla.Name = "txtFalla";
            txtFalla.ReadOnly = true;
            txtFalla.Size = new Size(300, 27);
            txtFalla.TabIndex = 4;
            // 
            // txtFecha
            // 
            txtFecha.BackColor = SystemColors.ControlLightLight;
            txtFecha.Location = new Point(135, 163);
            txtFecha.Name = "txtFecha";
            txtFecha.ReadOnly = true;
            txtFecha.Size = new Size(150, 27);
            txtFecha.TabIndex = 5;
            // 
            // lblIdIngreso
            // 
            lblIdIngreso.Location = new Point(20, 34);
            lblIdIngreso.Name = "lblIdIngreso";
            lblIdIngreso.Size = new Size(100, 23);
            lblIdIngreso.TabIndex = 6;
            lblIdIngreso.Text = "ID Ingreso:";
            // 
            // txtModelo
            // 
            txtModelo.BackColor = SystemColors.ControlLightLight;
            txtModelo.Location = new Point(134, 64);
            txtModelo.Name = "txtModelo";
            txtModelo.ReadOnly = true;
            txtModelo.Size = new Size(200, 27);
            txtModelo.TabIndex = 2;
            // 
            // lblMarca
            // 
            lblMarca.Location = new Point(20, 100);
            lblMarca.Name = "lblMarca";
            lblMarca.Size = new Size(100, 23);
            lblMarca.TabIndex = 7;
            lblMarca.Text = "Marca:";
            // 
            // lblModelo
            // 
            lblModelo.Location = new Point(20, 68);
            lblModelo.Name = "lblModelo";
            lblModelo.Size = new Size(100, 23);
            lblModelo.TabIndex = 8;
            lblModelo.Text = "Modelo:";
            // 
            // lblTipo
            // 
            lblTipo.Location = new Point(20, 201);
            lblTipo.Name = "lblTipo";
            lblTipo.Size = new Size(100, 23);
            lblTipo.TabIndex = 9;
            lblTipo.Text = "Tipo:";
            // 
            // lblFalla
            // 
            lblFalla.Location = new Point(20, 133);
            lblFalla.Name = "lblFalla";
            lblFalla.Size = new Size(46, 23);
            lblFalla.TabIndex = 10;
            lblFalla.Text = "Falla:";
            // 
            // lblFecha
            // 
            lblFecha.Location = new Point(20, 167);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(57, 23);
            lblFecha.TabIndex = 11;
            lblFecha.Text = "Fecha:";
            // 
            // dgvIngresos
            // 
            dgvIngresos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvIngresos.ColumnHeadersHeight = 29;
            dgvIngresos.Location = new Point(27, 21);
            dgvIngresos.MultiSelect = false;
            dgvIngresos.Name = "dgvIngresos";
            dgvIngresos.ReadOnly = true;
            dgvIngresos.RowHeadersWidth = 51;
            dgvIngresos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIngresos.Size = new Size(727, 202);
            dgvIngresos.TabIndex = 2;
            dgvIngresos.CellClick += dgvIngresos_SelectionChanged;
            // 
            // groupDetalles
            // 
            groupDetalles.Controls.Add(btnAgregarServicio);
            groupDetalles.Controls.Add(cbServicios);
            groupDetalles.Controls.Add(cbRepuestos);
            groupDetalles.Controls.Add(btnAgregarRepuesto);
            groupDetalles.Controls.Add(lblStockInfo);
            groupDetalles.Controls.Add(dgvDetalles);
            groupDetalles.Controls.Add(txtTotal);
            groupDetalles.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupDetalles.Location = new Point(99, 369);
            groupDetalles.Name = "groupDetalles";
            groupDetalles.Size = new Size(1330, 250);
            groupDetalles.TabIndex = 3;
            groupDetalles.TabStop = false;
            groupDetalles.Text = "Detalles del Presupuesto";
            // 
            // btnAgregarServicio
            // 
            btnAgregarServicio.Location = new Point(758, 30);
            btnAgregarServicio.Name = "btnAgregarServicio";
            btnAgregarServicio.Size = new Size(140, 35);
            btnAgregarServicio.TabIndex = 2;
            btnAgregarServicio.Text = "Agregar Servicio";
            btnAgregarServicio.Click += btnAgregarServicio_Click;
            // 
            // cbServicios
            // 
            cbServicios.Location = new Point(493, 35);
            cbServicios.Name = "cbServicios";
            cbServicios.Size = new Size(250, 26);
            cbServicios.TabIndex = 0;
            // 
            // cbRepuestos
            // 
            cbRepuestos.Location = new Point(20, 31);
            cbRepuestos.Name = "cbRepuestos";
            cbRepuestos.Size = new Size(250, 26);
            cbRepuestos.TabIndex = 1;
            // 
            // btnAgregarRepuesto
            // 
            btnAgregarRepuesto.Location = new Point(280, 28);
            btnAgregarRepuesto.Name = "btnAgregarRepuesto";
            btnAgregarRepuesto.Size = new Size(187, 33);
            btnAgregarRepuesto.TabIndex = 3;
            btnAgregarRepuesto.Text = "Agregar Repuesto";
            btnAgregarRepuesto.Click += btnAgregarRepuesto_Click;
            // 
            // lblStockInfo
            // 
            lblStockInfo.AutoSize = true;
            lblStockInfo.Location = new Point(20, 71);
            lblStockInfo.Name = "lblStockInfo";
            lblStockInfo.Size = new Size(139, 18);
            lblStockInfo.TabIndex = 6;
            lblStockInfo.Text = "Stock disponible: -";
            // 
            // dgvDetalles
            // 
            dgvDetalles.AllowUserToAddRows = false;
            dgvDetalles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalles.ColumnHeadersHeight = 29;
            dgvDetalles.Location = new Point(18, 92);
            dgvDetalles.Name = "dgvDetalles";
            dgvDetalles.RowHeadersWidth = 51;
            dgvDetalles.Size = new Size(1290, 140);
            dgvDetalles.TabIndex = 7;
            dgvDetalles.CellContentClick += dgvDetalles_CellContentClick;
            dgvDetalles.CellValueChanged += dgvDetalles_CellValueChanged;
            // 
            // txtTotal
            // 
            txtTotal.Location = new Point(1240, 164);
            txtTotal.Name = "txtTotal";
            txtTotal.Size = new Size(30, 25);
            txtTotal.TabIndex = 5;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotal.Location = new Point(1236, 622);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(104, 23);
            lblTotal.TabIndex = 8;
            lblTotal.Text = "Total: $0,00";
            // 
            // dgvPresupuestos
            // 
            dgvPresupuestos.ColumnHeadersHeight = 29;
            dgvPresupuestos.Location = new Point(52, 41);
            dgvPresupuestos.MultiSelect = false;
            dgvPresupuestos.Name = "dgvPresupuestos";
            dgvPresupuestos.ReadOnly = true;
            dgvPresupuestos.RowHeadersWidth = 51;
            dgvPresupuestos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPresupuestos.Size = new Size(1240, 188);
            dgvPresupuestos.TabIndex = 4;
            dgvPresupuestos.CellClick += dgvPresupuestos_SelectionChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dgvPresupuestos);
            groupBox1.Controls.Add(btnImprimir);
            groupBox1.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(101, 700);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1328, 252);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "Lista de Presupuestos";
            // 
            // groupBox2
            // 
            groupBox2.BackColor = SystemColors.ControlLightLight;
            groupBox2.Controls.Add(dgvIngresos);
            groupBox2.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(675, 96);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(790, 240);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            groupBox2.Text = "Seleccione Un Ingreso";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(45, 66, 91);
            panel1.Controls.Add(lblTitulo);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1594, 80);
            panel1.TabIndex = 10;
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.FromArgb(45, 66, 91);
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Arial Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1594, 83);
            lblTitulo.TabIndex = 14;
            lblTitulo.Text = "GESTIÓN DE PRESUPUESTOS";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(628, 21);
            label1.Name = "label1";
            label1.Size = new Size(369, 37);
            label1.TabIndex = 0;
            label1.Text = "MÓDULO DE PRESUPUESTO";
            // 
            // FrmPresupuestos
            // 
            BackColor = Color.White;
            ClientSize = new Size(1594, 947);
            Controls.Add(panel1);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(btnEnviarCliente);
            Controls.Add(btnAprobarAdmin);
            Controls.Add(btnAutorizar);
            Controls.Add(btnLimpiar);
            Controls.Add(btnGuardar);
            Controls.Add(lblTotal);
            Controls.Add(groupIngreso);
            Controls.Add(groupDetalles);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "FrmPresupuestos";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestión de Presupuestos";
            Load += FrmPresupuestos_Load;
            groupIngreso.ResumeLayout(false);
            groupIngreso.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvIngresos).EndInit();
            groupDetalles.ResumeLayout(false);
            groupDetalles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetalles).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPresupuestos).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel panelButtons;
       // private System.Windows.Forms.Button btnVerificarPrecio;
        private System.Windows.Forms.Button btnAprobarAdmin;
        private System.Windows.Forms.Button btnEnviarCliente;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnAutorizar;
        private System.Windows.Forms.Button btnImprimir;

        private System.Windows.Forms.GroupBox groupIngreso;
        private System.Windows.Forms.TextBox txtIdIngreso;
        private System.Windows.Forms.TextBox txtMarcaIngreso;
        private System.Windows.Forms.TextBox txtModelo;
        private System.Windows.Forms.TextBox txtTipoDispositivo;
        private System.Windows.Forms.TextBox txtFalla;
        private System.Windows.Forms.TextBox txtFecha;
        private System.Windows.Forms.Label lblIdIngreso;
        private System.Windows.Forms.Label lblMarca;
        private System.Windows.Forms.Label lblModelo;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.Label lblFalla;
        private System.Windows.Forms.Label lblFecha;

        private System.Windows.Forms.DataGridView dgvIngresos;

        private System.Windows.Forms.GroupBox groupDetalles;
        private System.Windows.Forms.ComboBox cbServicios;
        private System.Windows.Forms.ComboBox cbRepuestos;
        private System.Windows.Forms.Button btnAgregarServicio;
        private System.Windows.Forms.Button btnAgregarRepuesto;
        private System.Windows.Forms.DataGridView dgvDetalles;
        private System.Windows.Forms.Label lblTotal;

        private System.Windows.Forms.DataGridView dgvPresupuestos;
        private TextBox txtTotal;
        private Label lblStockInfo;
        private GroupBox groupBox1;
        private Panel panelHeader;
        private Label lblTitulo;
        private GroupBox groupBox2;
        private Panel panel1;
        private Label label1;
    }
}
