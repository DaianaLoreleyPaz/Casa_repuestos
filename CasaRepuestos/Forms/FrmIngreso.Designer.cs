namespace CasaRepuestos.Forms
{
    partial class FrmIngreso
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
            panelContainer = new Panel();
            splitContainer = new SplitContainer();
            panelListaContainer = new Panel();
            panelLista = new Panel();
            dgvIngresos = new DataGridView();
            panelBusqueda = new Panel();
            btnRegistrarDevolucion = new Button();
            txtBuscar = new TextBox();
            lblBuscar = new Label();
            panelFormContainer = new Panel();
            panelForm = new Panel();
            label1 = new Label();
            btnCancelar = new Button();
            btnGuardar = new Button();
            rbTablet = new RadioButton();
            rbSmartphone = new RadioButton();
            lblTipoDispositivo = new Label();
            dtpFechaIngreso = new DateTimePicker();
            lblFechaIngreso = new Label();
            txtAccesorios = new TextBox();
            lblAccesorios = new Label();
            txtFalla = new TextBox();
            lblFalla = new Label();
            txtModelo = new TextBox();
            lblModelo = new Label();
            cmbMarca = new ComboBox();
            lblMarca = new Label();
            btnAgregarCliente = new Button();
            txtCliente = new TextBox();
            lblCliente = new Label();
            panelHeader = new Panel();
            lblTitulo = new Label();
            panelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            panelListaContainer.SuspendLayout();
            panelLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvIngresos).BeginInit();
            panelBusqueda.SuspendLayout();
            panelFormContainer.SuspendLayout();
            panelForm.SuspendLayout();
            panelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // panelContainer
            // 
            panelContainer.BackColor = Color.FromArgb(248, 249, 250);
            panelContainer.Controls.Add(splitContainer);
            panelContainer.Controls.Add(panelHeader);
            panelContainer.Dock = DockStyle.Fill;
            panelContainer.Location = new Point(0, 0);
            panelContainer.Margin = new Padding(3, 4, 3, 4);
            panelContainer.Name = "panelContainer";
            panelContainer.Size = new Size(1902, 1055);
            panelContainer.TabIndex = 0;
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 80);
            splitContainer.Margin = new Padding(3, 4, 3, 4);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(panelListaContainer);
            splitContainer.Panel1MinSize = 700;
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(panelFormContainer);
            splitContainer.Panel2MinSize = 500;
            splitContainer.Size = new Size(1902, 975);
            splitContainer.SplitterDistance = 1199;
            splitContainer.SplitterWidth = 5;
            splitContainer.TabIndex = 2;
            // 
            // panelListaContainer
            // 
            panelListaContainer.Controls.Add(panelLista);
            panelListaContainer.Controls.Add(panelBusqueda);
            panelListaContainer.Dock = DockStyle.Fill;
            panelListaContainer.Location = new Point(0, 0);
            panelListaContainer.Margin = new Padding(3, 4, 3, 4);
            panelListaContainer.Name = "panelListaContainer";
            panelListaContainer.Size = new Size(1199, 975);
            panelListaContainer.TabIndex = 2;
            // 
            // panelLista
            // 
            panelLista.Controls.Add(dgvIngresos);
            panelLista.Dock = DockStyle.Fill;
            panelLista.Location = new Point(0, 75);
            panelLista.Margin = new Padding(3, 4, 3, 4);
            panelLista.Name = "panelLista";
            panelLista.Size = new Size(1199, 900);
            panelLista.TabIndex = 1;
            // 
            // dgvIngresos
            // 
            dgvIngresos.AllowUserToAddRows = false;
            dgvIngresos.AllowUserToDeleteRows = false;
            dgvIngresos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvIngresos.BackgroundColor = Color.White;
            dgvIngresos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvIngresos.Dock = DockStyle.Fill;
            dgvIngresos.Location = new Point(0, 0);
            dgvIngresos.Margin = new Padding(3, 4, 3, 4);
            dgvIngresos.Name = "dgvIngresos";
            dgvIngresos.ReadOnly = true;
            dgvIngresos.RowHeadersWidth = 51;
            dgvIngresos.RowTemplate.Height = 24;
            dgvIngresos.Size = new Size(1199, 900);
            dgvIngresos.TabIndex = 0;
            dgvIngresos.SelectionChanged += dgvIngresos_SelectionChanged;
            // 
            // panelBusqueda
            // 
            panelBusqueda.Controls.Add(btnRegistrarDevolucion);
            panelBusqueda.Controls.Add(txtBuscar);
            panelBusqueda.Controls.Add(lblBuscar);
            panelBusqueda.Dock = DockStyle.Top;
            panelBusqueda.Location = new Point(0, 0);
            panelBusqueda.Margin = new Padding(3, 4, 3, 4);
            panelBusqueda.Name = "panelBusqueda";
            panelBusqueda.Size = new Size(1199, 75);
            panelBusqueda.TabIndex = 0;
            // 
            // btnRegistrarDevolucion
            // 
            btnRegistrarDevolucion.Location = new Point(734, 17);
            btnRegistrarDevolucion.Name = "btnRegistrarDevolucion";
            btnRegistrarDevolucion.Size = new Size(174, 51);
            btnRegistrarDevolucion.TabIndex = 2;
            btnRegistrarDevolucion.Text = "Registrar Devolucion";
            btnRegistrarDevolucion.UseVisualStyleBackColor = true;
            btnRegistrarDevolucion.Click += btnRegistrarDevolucion_Click;
            // 
            // txtBuscar
            // 
            txtBuscar.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtBuscar.Location = new Point(101, 19);
            txtBuscar.Margin = new Padding(3, 4, 3, 4);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(300, 34);
            txtBuscar.TabIndex = 1;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBuscar.ForeColor = Color.FromArgb(45, 66, 91);
            lblBuscar.Location = new Point(15, 23);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(81, 28);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar:";
            // 
            // panelFormContainer
            // 
            panelFormContainer.BackColor = Color.LightSteelBlue;
            panelFormContainer.Controls.Add(panelForm);
            panelFormContainer.Location = new Point(0, 0);
            panelFormContainer.Margin = new Padding(3, 4, 3, 4);
            panelFormContainer.Name = "panelFormContainer";
            panelFormContainer.Size = new Size(698, 1211);
            panelFormContainer.TabIndex = 2;
            // 
            // panelForm
            // 
            panelForm.Anchor = AnchorStyles.None;
            panelForm.BackColor = Color.FromArgb(224, 224, 224);
            panelForm.BorderStyle = BorderStyle.FixedSingle;
            panelForm.Controls.Add(label1);
            panelForm.Controls.Add(btnCancelar);
            panelForm.Controls.Add(btnGuardar);
            panelForm.Controls.Add(rbTablet);
            panelForm.Controls.Add(rbSmartphone);
            panelForm.Controls.Add(lblTipoDispositivo);
            panelForm.Controls.Add(dtpFechaIngreso);
            panelForm.Controls.Add(lblFechaIngreso);
            panelForm.Controls.Add(txtAccesorios);
            panelForm.Controls.Add(lblAccesorios);
            panelForm.Controls.Add(txtFalla);
            panelForm.Controls.Add(lblFalla);
            panelForm.Controls.Add(txtModelo);
            panelForm.Controls.Add(lblModelo);
            panelForm.Controls.Add(cmbMarca);
            panelForm.Controls.Add(lblMarca);
            panelForm.Controls.Add(btnAgregarCliente);
            panelForm.Controls.Add(txtCliente);
            panelForm.Controls.Add(lblCliente);
            panelForm.Location = new Point(3, 23);
            panelForm.Margin = new Padding(3, 4, 3, 4);
            panelForm.Name = "panelForm";
            panelForm.Size = new Size(600, 874);
            panelForm.TabIndex = 2;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(45, 66, 91);
            label1.Location = new Point(149, 65);
            label1.Name = "label1";
            label1.Size = new Size(341, 41);
            label1.TabIndex = 19;
            label1.Text = "DETALLES DE INGRESO";
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(220, 53, 69);
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatAppearance.MouseDownBackColor = Color.FromArgb(176, 42, 55);
            btnCancelar.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 48, 62);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(350, 775);
            btnCancelar.Margin = new Padding(3, 4, 3, 4);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(150, 63);
            btnCancelar.TabIndex = 18;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(76, 132, 200);
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatAppearance.MouseDownBackColor = Color.FromArgb(57, 99, 150);
            btnGuardar.FlatAppearance.MouseOverBackColor = Color.FromArgb(91, 147, 215);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(150, 775);
            btnGuardar.Margin = new Padding(3, 4, 3, 4);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(150, 63);
            btnGuardar.TabIndex = 17;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // rbTablet
            // 
            rbTablet.AutoSize = true;
            rbTablet.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rbTablet.Location = new Point(350, 688);
            rbTablet.Margin = new Padding(3, 4, 3, 4);
            rbTablet.Name = "rbTablet";
            rbTablet.Size = new Size(85, 32);
            rbTablet.TabIndex = 16;
            rbTablet.Text = "Tablet";
            rbTablet.UseVisualStyleBackColor = true;
            // 
            // rbSmartphone
            // 
            rbSmartphone.AutoSize = true;
            rbSmartphone.Checked = true;
            rbSmartphone.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rbSmartphone.Location = new Point(150, 688);
            rbSmartphone.Margin = new Padding(3, 4, 3, 4);
            rbSmartphone.Name = "rbSmartphone";
            rbSmartphone.Size = new Size(141, 32);
            rbSmartphone.TabIndex = 15;
            rbSmartphone.TabStop = true;
            rbSmartphone.Text = "Smartphone";
            rbSmartphone.UseVisualStyleBackColor = true;
            // 
            // lblTipoDispositivo
            // 
            lblTipoDispositivo.AutoSize = true;
            lblTipoDispositivo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTipoDispositivo.ForeColor = Color.FromArgb(45, 66, 91);
            lblTipoDispositivo.Location = new Point(50, 688);
            lblTipoDispositivo.Name = "lblTipoDispositivo";
            lblTipoDispositivo.Size = new Size(59, 28);
            lblTipoDispositivo.TabIndex = 14;
            lblTipoDispositivo.Text = "Tipo:";
            // 
            // dtpFechaIngreso
            // 
            dtpFechaIngreso.Enabled = false;
            dtpFechaIngreso.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpFechaIngreso.Format = DateTimePickerFormat.Short;
            dtpFechaIngreso.Location = new Point(250, 619);
            dtpFechaIngreso.Margin = new Padding(3, 4, 3, 4);
            dtpFechaIngreso.Name = "dtpFechaIngreso";
            dtpFechaIngreso.Size = new Size(300, 34);
            dtpFechaIngreso.TabIndex = 13;
            // 
            // lblFechaIngreso
            // 
            lblFechaIngreso.AutoSize = true;
            lblFechaIngreso.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFechaIngreso.ForeColor = Color.FromArgb(45, 66, 91);
            lblFechaIngreso.Location = new Point(50, 625);
            lblFechaIngreso.Name = "lblFechaIngreso";
            lblFechaIngreso.Size = new Size(147, 28);
            lblFechaIngreso.TabIndex = 12;
            lblFechaIngreso.Text = "Fecha Ingreso:";
            // 
            // txtAccesorios
            // 
            txtAccesorios.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtAccesorios.Location = new Point(250, 496);
            txtAccesorios.Margin = new Padding(3, 4, 3, 4);
            txtAccesorios.Multiline = true;
            txtAccesorios.Name = "txtAccesorios";
            txtAccesorios.Size = new Size(300, 99);
            txtAccesorios.TabIndex = 11;
            // 
            // lblAccesorios
            // 
            lblAccesorios.AutoSize = true;
            lblAccesorios.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAccesorios.ForeColor = Color.FromArgb(45, 66, 91);
            lblAccesorios.Location = new Point(50, 500);
            lblAccesorios.Name = "lblAccesorios";
            lblAccesorios.Size = new Size(118, 28);
            lblAccesorios.TabIndex = 10;
            lblAccesorios.Text = "Accesorios:";
            // 
            // txtFalla
            // 
            txtFalla.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFalla.Location = new Point(250, 371);
            txtFalla.Margin = new Padding(3, 4, 3, 4);
            txtFalla.Multiline = true;
            txtFalla.Name = "txtFalla";
            txtFalla.Size = new Size(300, 99);
            txtFalla.TabIndex = 9;
            // 
            // lblFalla
            // 
            lblFalla.AutoSize = true;
            lblFalla.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFalla.ForeColor = Color.FromArgb(45, 66, 91);
            lblFalla.Location = new Point(50, 375);
            lblFalla.Name = "lblFalla";
            lblFalla.Size = new Size(60, 28);
            lblFalla.TabIndex = 8;
            lblFalla.Text = "Falla:";
            // 
            // txtModelo
            // 
            txtModelo.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtModelo.Location = new Point(250, 309);
            txtModelo.Margin = new Padding(3, 4, 3, 4);
            txtModelo.Name = "txtModelo";
            txtModelo.Size = new Size(300, 34);
            txtModelo.TabIndex = 7;
            // 
            // lblModelo
            // 
            lblModelo.AutoSize = true;
            lblModelo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblModelo.ForeColor = Color.FromArgb(45, 66, 91);
            lblModelo.Location = new Point(50, 312);
            lblModelo.Name = "lblModelo";
            lblModelo.Size = new Size(89, 28);
            lblModelo.TabIndex = 6;
            lblModelo.Text = "Modelo:";
            // 
            // cmbMarca
            // 
            cmbMarca.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbMarca.FormattingEnabled = true;
            cmbMarca.Location = new Point(250, 247);
            cmbMarca.Margin = new Padding(3, 4, 3, 4);
            cmbMarca.Name = "cmbMarca";
            cmbMarca.Size = new Size(300, 36);
            cmbMarca.TabIndex = 5;
            // 
            // lblMarca
            // 
            lblMarca.AutoSize = true;
            lblMarca.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMarca.ForeColor = Color.FromArgb(45, 66, 91);
            lblMarca.Location = new Point(50, 251);
            lblMarca.Name = "lblMarca";
            lblMarca.Size = new Size(76, 28);
            lblMarca.TabIndex = 4;
            lblMarca.Text = "Marca:";
            // 
            // btnAgregarCliente
            // 
            btnAgregarCliente.BackColor = Color.FromArgb(76, 132, 200);
            btnAgregarCliente.FlatAppearance.BorderSize = 0;
            btnAgregarCliente.FlatAppearance.MouseDownBackColor = Color.FromArgb(57, 99, 150);
            btnAgregarCliente.FlatAppearance.MouseOverBackColor = Color.FromArgb(91, 147, 215);
            btnAgregarCliente.FlatStyle = FlatStyle.Flat;
            btnAgregarCliente.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAgregarCliente.ForeColor = Color.White;
            btnAgregarCliente.Location = new Point(459, 184);
            btnAgregarCliente.Margin = new Padding(3, 4, 3, 4);
            btnAgregarCliente.Name = "btnAgregarCliente";
            btnAgregarCliente.Size = new Size(91, 39);
            btnAgregarCliente.TabIndex = 3;
            btnAgregarCliente.Text = "Agregar";
            btnAgregarCliente.UseVisualStyleBackColor = false;
            btnAgregarCliente.Click += btnAgregarCliente_Click;
            // 
            // txtCliente
            // 
            txtCliente.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCliente.Location = new Point(250, 184);
            txtCliente.Margin = new Padding(3, 4, 3, 4);
            txtCliente.Name = "txtCliente";
            txtCliente.Size = new Size(201, 34);
            txtCliente.TabIndex = 2;
            // 
            // lblCliente
            // 
            lblCliente.AutoSize = true;
            lblCliente.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCliente.ForeColor = Color.FromArgb(45, 66, 91);
            lblCliente.Location = new Point(50, 188);
            lblCliente.Name = "lblCliente";
            lblCliente.Size = new Size(83, 28);
            lblCliente.TabIndex = 1;
            lblCliente.Text = "Cliente:";
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(45, 66, 91);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Margin = new Padding(3, 4, 3, 4);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1902, 80);
            panelHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.FromArgb(45, 66, 91);
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Arial Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1902, 83);
            lblTitulo.TabIndex = 14;
            lblTitulo.Text = "MÓDULO DE INGRESOS";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmIngreso
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1902, 1055);
            Controls.Add(panelContainer);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FrmIngreso";
            Text = "Registro de Ingresos";
            WindowState = FormWindowState.Maximized;
            Load += FrmIngreso_Load;
            panelContainer.ResumeLayout(false);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            panelListaContainer.ResumeLayout(false);
            panelLista.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvIngresos).EndInit();
            panelBusqueda.ResumeLayout(false);
            panelBusqueda.PerformLayout();
            panelFormContainer.ResumeLayout(false);
            panelForm.ResumeLayout(false);
            panelForm.PerformLayout();
            panelHeader.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel panelListaContainer;
        private System.Windows.Forms.Panel panelLista;
        private System.Windows.Forms.DataGridView dgvIngresos;
        private System.Windows.Forms.Panel panelBusqueda;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Label lblBuscar;
        private System.Windows.Forms.Panel panelFormContainer;
        private System.Windows.Forms.Panel panelForm;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.RadioButton rbTablet;
        private System.Windows.Forms.RadioButton rbSmartphone;
        private System.Windows.Forms.Label lblTipoDispositivo;
        private System.Windows.Forms.DateTimePicker dtpFechaIngreso;
        private System.Windows.Forms.Label lblFechaIngreso;
        private System.Windows.Forms.TextBox txtAccesorios;
        private System.Windows.Forms.Label lblAccesorios;
        private System.Windows.Forms.TextBox txtFalla;
        private System.Windows.Forms.Label lblFalla;
        private System.Windows.Forms.TextBox txtModelo;
        private System.Windows.Forms.Label lblModelo;
        private System.Windows.Forms.ComboBox cmbMarca;
        private System.Windows.Forms.Label lblMarca;
        private System.Windows.Forms.Button btnAgregarCliente;
        private System.Windows.Forms.TextBox txtCliente;
        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.Label lblTituloForm;
        private Label label1;
        private Panel panelHeader;
        private Label lblTitulo;
        private Button btnRegistrarDevolucion;
    }
}