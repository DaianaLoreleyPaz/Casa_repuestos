namespace CasaRepuestos.Forms
{
    partial class FrmCliente
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCliente));
            tabControl1 = new TabControl();
            tabListado = new TabPage();
            groupBox2 = new GroupBox();
            txtBuscar = new TextBox();
            label10 = new Label();
            dgvClientes = new DataGridView();
            lblTitulo = new Label();
            label12 = new Label();
            btnGuardar = new Button();
            btnLimpiar = new Button();
            groupBox1 = new GroupBox();
            label9 = new Label();
            cmbCategoria = new ComboBox();
            label8 = new Label();
            txtCuil = new TextBox();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            txtEmail = new TextBox();
            txtDireccion = new TextBox();
            txtDocumento = new TextBox();
            txtTelefono = new TextBox();
            txtApellido = new TextBox();
            txtNombre = new TextBox();
            label2 = new Label();
            label1 = new Label();
            cmbTipoDoc = new ComboBox();
            label3 = new Label();
            buttonVerCtasCtes = new Button();
            tabControl1.SuspendLayout();
            tabListado.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabListado);
            tabControl1.Font = new Font("Segoe UI", 10F);
            tabControl1.Location = new Point(0, 162);
            tabControl1.Margin = new Padding(3, 4, 3, 4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(976, 861);
            tabControl1.TabIndex = 18;
            // 
            // tabListado
            // 
            tabListado.BackColor = Color.White;
            tabListado.Controls.Add(groupBox2);
            tabListado.Location = new Point(4, 32);
            tabListado.Name = "tabListado";
            tabListado.Size = new Size(968, 825);
            tabListado.TabIndex = 0;
            tabListado.Text = "Listado";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            groupBox2.Controls.Add(txtBuscar);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(dgvClientes);
            groupBox2.Location = new Point(0, 24);
            groupBox2.Margin = new Padding(3, 4, 3, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 4, 3, 4);
            groupBox2.Size = new Size(953, 739);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Listado de Clientes";
            // 
            // txtBuscar
            // 
            txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBuscar.Location = new Point(100, 30);
            txtBuscar.Margin = new Padding(3, 4, 3, 4);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(833, 30);
            txtBuscar.TabIndex = 2;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            txtBuscar.Enter += txtBuscar_Enter;
            txtBuscar.Leave += txtBuscar_Leave;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(27, 42);
            label10.Name = "label10";
            label10.Size = new Size(69, 23);
            label10.TabIndex = 8;
            label10.Text = "Buscar: ";
            // 
            // dgvClientes
            // 
            dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClientes.BackgroundColor = Color.White;
            dgvClientes.ColumnHeadersHeight = 29;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(200, 230, 255);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgvClientes.DefaultCellStyle = dataGridViewCellStyle1;
            dgvClientes.Location = new Point(6, 70);
            dgvClientes.Margin = new Padding(3, 4, 3, 4);
            dgvClientes.Name = "dgvClientes";
            dgvClientes.ReadOnly = true;
            dgvClientes.RowHeadersWidth = 51;
            dgvClientes.Size = new Size(947, 680);
            dgvClientes.TabIndex = 0;
            dgvClientes.CellClick += dgvClientes_CellClick;
            dgvClientes.CellContentClick += dgvClientes_CellClick;
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.FromArgb(45, 66, 91);
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Arial Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1626, 70);
            lblTitulo.TabIndex = 19;
            lblTitulo.Text = "MÓDULO DE CLIENTES";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            label12.Location = new Point(1118, 162);
            label12.Name = "label12";
            label12.Size = new Size(321, 37);
            label12.TabIndex = 20;
            label12.Text = "Agregar / Editar Cliente";
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(12, 87, 150);
            btnGuardar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGuardar.Image = (Image)resources.GetObject("btnGuardar.Image");
            btnGuardar.ImageAlign = ContentAlignment.MiddleLeft;
            btnGuardar.Location = new Point(1094, 714);
            btnGuardar.Margin = new Padding(3, 4, 3, 4);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(141, 53);
            btnGuardar.TabIndex = 23;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.BackColor = Color.Orange;
            btnLimpiar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLimpiar.Location = new Point(1277, 714);
            btnLimpiar.Margin = new Padding(3, 4, 3, 4);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(137, 53);
            btnLimpiar.TabIndex = 22;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = false;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.BackColor = Color.White;
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(cmbCategoria);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(txtCuil);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(txtEmail);
            groupBox1.Controls.Add(txtDireccion);
            groupBox1.Controls.Add(txtDocumento);
            groupBox1.Controls.Add(txtTelefono);
            groupBox1.Controls.Add(txtApellido);
            groupBox1.Controls.Add(txtNombre);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(cmbTipoDoc);
            groupBox1.Controls.Add(label3);
            groupBox1.Location = new Point(982, 237);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(649, 452);
            groupBox1.TabIndex = 21;
            groupBox1.TabStop = false;
            groupBox1.Text = "Datos del Cliente";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(33, 359);
            label9.Name = "label9";
            label9.Size = new Size(156, 20);
            label9.TabIndex = 0;
            label9.Text = "Seleccione Categoria: ";
            // 
            // cmbCategoria
            // 
            cmbCategoria.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbCategoria.Location = new Point(22, 394);
            cmbCategoria.Margin = new Padding(3, 4, 3, 4);
            cmbCategoria.Name = "cmbCategoria";
            cmbCategoria.Size = new Size(275, 28);
            cmbCategoria.TabIndex = 1;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(43, 202);
            label8.Name = "label8";
            label8.Size = new Size(66, 20);
            label8.TabIndex = 2;
            label8.Text = "Cuil/Cuit";
            // 
            // txtCuil
            // 
            txtCuil.Location = new Point(33, 228);
            txtCuil.Margin = new Padding(3, 4, 3, 4);
            txtCuil.Name = "txtCuil";
            txtCuil.Size = new Size(275, 27);
            txtCuil.TabIndex = 3;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(360, 280);
            label7.Name = "label7";
            label7.Size = new Size(46, 20);
            label7.TabIndex = 4;
            label7.Text = "Email";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(357, 202);
            label6.Name = "label6";
            label6.Size = new Size(67, 20);
            label6.TabIndex = 5;
            label6.Text = "Telefono";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(43, 280);
            label5.Name = "label5";
            label5.Size = new Size(72, 20);
            label5.TabIndex = 6;
            label5.Text = "Direccion";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(357, 125);
            label4.Name = "label4";
            label4.Size = new Size(137, 20);
            label4.TabIndex = 7;
            label4.Text = "Nro de Documento";
            // 
            // txtEmail
            // 
            txtEmail.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtEmail.Location = new Point(346, 306);
            txtEmail.Margin = new Padding(3, 4, 3, 4);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(265, 27);
            txtEmail.TabIndex = 16;
            // 
            // txtDireccion
            // 
            txtDireccion.Location = new Point(33, 306);
            txtDireccion.Margin = new Padding(3, 4, 3, 4);
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new Size(275, 27);
            txtDireccion.TabIndex = 17;
            // 
            // txtDocumento
            // 
            txtDocumento.Location = new Point(355, 151);
            txtDocumento.Margin = new Padding(3, 4, 3, 4);
            txtDocumento.Name = "txtDocumento";
            txtDocumento.Size = new Size(267, 27);
            txtDocumento.TabIndex = 14;
            // 
            // txtTelefono
            // 
            txtTelefono.Location = new Point(357, 228);
            txtTelefono.Margin = new Padding(3, 4, 3, 4);
            txtTelefono.Multiline = true;
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(265, 27);
            txtTelefono.TabIndex = 15;
            // 
            // txtApellido
            // 
            txtApellido.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtApellido.Location = new Point(355, 71);
            txtApellido.Margin = new Padding(3, 4, 3, 4);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(256, 27);
            txtApellido.TabIndex = 11;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(33, 73);
            txtNombre.Margin = new Padding(3, 4, 3, 4);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(275, 27);
            txtNombre.TabIndex = 13;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(357, 47);
            label2.Name = "label2";
            label2.Size = new Size(66, 20);
            label2.TabIndex = 9;
            label2.Text = "Apellido";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(43, 47);
            label1.Name = "label1";
            label1.Size = new Size(64, 20);
            label1.TabIndex = 10;
            label1.Text = "Nombre";
            // 
            // cmbTipoDoc
            // 
            cmbTipoDoc.Location = new Point(33, 151);
            cmbTipoDoc.Margin = new Padding(3, 4, 3, 4);
            cmbTipoDoc.Name = "cmbTipoDoc";
            cmbTipoDoc.Size = new Size(275, 28);
            cmbTipoDoc.TabIndex = 12;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(43, 125);
            label3.Name = "label3";
            label3.Size = new Size(142, 20);
            label3.TabIndex = 8;
            label3.Text = "Tipo de Documento";
            // 
            // buttonVerCtasCtes
            // 
            buttonVerCtasCtes.BackColor = Color.Orange;
            buttonVerCtasCtes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            buttonVerCtasCtes.Location = new Point(26, 87);
            buttonVerCtasCtes.Margin = new Padding(3, 4, 3, 4);
            buttonVerCtasCtes.Name = "buttonVerCtasCtes";
            buttonVerCtasCtes.Size = new Size(220, 53);
            buttonVerCtasCtes.TabIndex = 24;
            buttonVerCtasCtes.Text = "Cuentas Corrientes";
            buttonVerCtasCtes.UseVisualStyleBackColor = false;
            buttonVerCtasCtes.Click += buttonVerCtasCtes_Click;
            // 
            // FrmCliente
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightSteelBlue;
            ClientSize = new Size(1626, 1024);
            Controls.Add(buttonVerCtasCtes);
            Controls.Add(label12);
            Controls.Add(btnGuardar);
            Controls.Add(btnLimpiar);
            Controls.Add(groupBox1);
            Controls.Add(lblTitulo);
            Controls.Add(tabControl1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FrmCliente";
            Text = "Clientes - Casa de Repuestos";
            Load += FrmCliente_Load;
            tabControl1.ResumeLayout(false);
            tabListado.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabListado;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvClientes;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnAgregarCliente;
        private Panel panelHeader;
        private Label lblTitulo;
        private Label label12;
        private Button btnGuardar;
        private Button btnLimpiar;
        private GroupBox groupBox1;
        private Label label9;
        private ComboBox cmbCategoria;
        private Label label8;
        private TextBox txtCuil;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private TextBox txtEmail;
        private TextBox txtDireccion;
        private TextBox txtDocumento;
        private TextBox txtTelefono;
        private TextBox txtApellido;
        private TextBox txtNombre;
        private Label label2;
        private Label label1;
        private ComboBox cmbTipoDoc;
        private Label label3;
        private Button buttonVerCtasCtes;
    }
}

