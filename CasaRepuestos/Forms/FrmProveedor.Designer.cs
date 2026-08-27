namespace CasaRepuestos.Forms
{
    partial class FrmProveedor
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
            tabControl1 = new TabControl();
            tabListado = new TabPage();
            groupBox2 = new GroupBox();
            txtBuscar = new TextBox();
            label10 = new Label();
            dgvProveedores = new DataGridView();
            label12 = new Label();
            groupBox1 = new GroupBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            txtNombre = new TextBox();
            txtApellido = new TextBox();
            cmbTipoDoc = new ComboBox();
            txtDocumento = new TextBox();
            groupBox3 = new GroupBox();
            label9 = new Label();
            txtRazonSocial = new TextBox();
            label8 = new Label();
            txtCuil = new TextBox();
            label5 = new Label();
            txtDireccion = new TextBox();
            label6 = new Label();
            txtTelefono = new TextBox();
            label7 = new Label();
            txtEmail = new TextBox();
            btnGuardar = new Button();
            btnEditar = new Button();
            btnLimpiar = new Button();
            lblTitulo = new Label();
            tabControl1.SuspendLayout();
            tabListado.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProveedores).BeginInit();
            groupBox1.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabListado);
            tabControl1.Font = new Font("Segoe UI", 10F);
            tabControl1.Location = new Point(12, 66);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1009, 684);
            tabControl1.TabIndex = 1;
            // 
            // tabListado
            // 
            tabListado.BackColor = Color.White;
            tabListado.Controls.Add(groupBox2);
            tabListado.Location = new Point(4, 32);
            tabListado.Name = "tabListado";
            tabListado.Size = new Size(1001, 648);
            tabListado.TabIndex = 0;
            tabListado.Text = "Listado";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(txtBuscar);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(dgvProveedores);
            groupBox2.Location = new Point(8, 14);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(980, 631);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Listado de Proveedores";
            // 
            // txtBuscar
            // 
            txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBuscar.Location = new Point(110, 30);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(808, 30);
            txtBuscar.TabIndex = 1;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            txtBuscar.Enter += txtBuscar_Enter;
            txtBuscar.Leave += txtBuscar_Leave;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(20, 33);
            label10.Name = "label10";
            label10.Size = new Size(64, 23);
            label10.TabIndex = 2;
            label10.Text = "Buscar:";
            // 
            // dgvProveedores
            // 
            dgvProveedores.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvProveedores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProveedores.ColumnHeadersHeight = 29;
            dgvProveedores.Location = new Point(7, 92);
            dgvProveedores.Name = "dgvProveedores";
            dgvProveedores.ReadOnly = true;
            dgvProveedores.RowHeadersWidth = 51;
            dgvProveedores.Size = new Size(967, 533);
            dgvProveedores.TabIndex = 3;
            dgvProveedores.CellClick += dgvProveedores_CellClick;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label12.Location = new Point(1072, 98);
            label12.Name = "label12";
            label12.Size = new Size(325, 32);
            label12.TabIndex = 6;
            label12.Text = "Agregar / Editar Proveedor";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.BackColor = Color.FromArgb(224, 224, 224);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(txtNombre);
            groupBox1.Controls.Add(txtApellido);
            groupBox1.Controls.Add(cmbTipoDoc);
            groupBox1.Controls.Add(txtDocumento);
            groupBox1.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(1027, 142);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(636, 220);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "Datos del Responsable Proveedor";
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(16, 36);
            label1.Name = "label1";
            label1.Size = new Size(100, 23);
            label1.TabIndex = 0;
            label1.Text = "Nombre";
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(320, 30);
            label2.Name = "label2";
            label2.Size = new Size(100, 23);
            label2.TabIndex = 1;
            label2.Text = "Apellido";
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(16, 110);
            label3.Name = "label3";
            label3.Size = new Size(219, 23);
            label3.TabIndex = 2;
            label3.Text = "Tipo de Documento";
            // 
            // label4
            // 
            label4.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(320, 111);
            label4.Name = "label4";
            label4.Size = new Size(244, 23);
            label4.TabIndex = 3;
            label4.Text = "Nro de Documento";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(16, 62);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(280, 30);
            txtNombre.TabIndex = 4;
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(320, 62);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(280, 30);
            txtApellido.TabIndex = 5;
            // 
            // cmbTipoDoc
            // 
            cmbTipoDoc.Location = new Point(16, 136);
            cmbTipoDoc.Name = "cmbTipoDoc";
            cmbTipoDoc.Size = new Size(280, 31);
            cmbTipoDoc.TabIndex = 6;
            // 
            // txtDocumento
            // 
            txtDocumento.Location = new Point(320, 137);
            txtDocumento.Name = "txtDocumento";
            txtDocumento.Size = new Size(280, 30);
            txtDocumento.TabIndex = 7;
            // 
            // groupBox3
            // 
            groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox3.BackColor = Color.FromArgb(224, 224, 224);
            groupBox3.Controls.Add(label9);
            groupBox3.Controls.Add(txtRazonSocial);
            groupBox3.Controls.Add(label8);
            groupBox3.Controls.Add(txtCuil);
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(txtDireccion);
            groupBox3.Controls.Add(label6);
            groupBox3.Controls.Add(txtTelefono);
            groupBox3.Controls.Add(label7);
            groupBox3.Controls.Add(txtEmail);
            groupBox3.Location = new Point(1027, 379);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(636, 264);
            groupBox3.TabIndex = 8;
            groupBox3.TabStop = false;
            groupBox3.Text = "Datos de la empresa";
            // 
            // label9
            // 
            label9.Location = new Point(16, 44);
            label9.Name = "label9";
            label9.Size = new Size(175, 23);
            label9.TabIndex = 0;
            label9.Text = "Razón Social";
            // 
            // txtRazonSocial
            // 
            txtRazonSocial.Location = new Point(16, 73);
            txtRazonSocial.Name = "txtRazonSocial";
            txtRazonSocial.Size = new Size(280, 27);
            txtRazonSocial.TabIndex = 1;
            // 
            // label8
            // 
            label8.Location = new Point(320, 44);
            label8.Name = "label8";
            label8.Size = new Size(66, 23);
            label8.TabIndex = 2;
            label8.Text = "CUIT";
            // 
            // txtCuil
            // 
            txtCuil.Location = new Point(320, 73);
            txtCuil.Name = "txtCuil";
            txtCuil.Size = new Size(260, 27);
            txtCuil.TabIndex = 3;
            // 
            // label5
            // 
            label5.Location = new Point(16, 116);
            label5.Name = "label5";
            label5.Size = new Size(100, 23);
            label5.TabIndex = 4;
            label5.Text = "Dirección";
            // 
            // txtDireccion
            // 
            txtDireccion.Location = new Point(16, 142);
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new Size(280, 27);
            txtDireccion.TabIndex = 5;
            // 
            // label6
            // 
            label6.Location = new Point(320, 116);
            label6.Name = "label6";
            label6.Size = new Size(100, 23);
            label6.TabIndex = 6;
            label6.Text = "Teléfono";
            // 
            // txtTelefono
            // 
            txtTelefono.Location = new Point(320, 142);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(260, 27);
            txtTelefono.TabIndex = 7;
            // 
            // label7
            // 
            label7.Location = new Point(16, 190);
            label7.Name = "label7";
            label7.Size = new Size(100, 23);
            label7.TabIndex = 8;
            label7.Text = "Email";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(16, 216);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(354, 27);
            txtEmail.TabIndex = 9;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(1101, 668);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(140, 40);
            btnGuardar.TabIndex = 9;
            btnGuardar.Text = "Guardar";
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnEditar
            // 
            btnEditar.Location = new Point(1257, 668);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(140, 40);
            btnEditar.TabIndex = 10;
            btnEditar.Text = "Editar";
            btnEditar.Click += btnEditar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(1412, 668);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(140, 40);
            btnLimpiar.TabIndex = 11;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.FromArgb(45, 66, 91);
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Arial Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1710, 63);
            lblTitulo.TabIndex = 14;
            lblTitulo.Text = "MÓDULO DE PROVEEDORES";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmProveedor
            // 
            BackColor = Color.LightSteelBlue;
            ClientSize = new Size(1710, 768);
            Controls.Add(lblTitulo);
            Controls.Add(label12);
            Controls.Add(groupBox1);
            Controls.Add(groupBox3);
            Controls.Add(btnGuardar);
            Controls.Add(btnEditar);
            Controls.Add(btnLimpiar);
            Controls.Add(tabControl1);
            Name = "FrmProveedor";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Proveedores - Casa de Repuestos";
            tabControl1.ResumeLayout(false);
            tabListado.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProveedores).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabListado;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView dgvProveedores;
        private Label label12;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private ComboBox cmbTipoDoc;
        private TextBox txtDocumento;
        private GroupBox groupBox3;
        private Label label9;
        private TextBox txtRazonSocial;
        private Label label8;
        private TextBox txtCuil;
        private Label label5;
        private TextBox txtDireccion;
        private Label label6;
        private TextBox txtTelefono;
        private Label label7;
        private TextBox txtEmail;
        private Button btnGuardar;
        private Button btnEditar;
        private Button btnLimpiar;
        private Label lblTitulo;
    }
}
