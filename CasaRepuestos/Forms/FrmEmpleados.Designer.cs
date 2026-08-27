namespace CasaRepuestos
{
    partial class FrmEmpleados
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
            tabControl1 = new TabControl();
            tabLista = new TabPage();
            txtBuscar = new TextBox();
            label12 = new Label();
            label11 = new Label();
            dgvEmpleados = new DataGridView();
            btnLimpiar = new Button();
            btnGuardar = new Button();
            groupBox2 = new GroupBox();
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            txtContrasenia = new TextBox();
            cmbRol = new ComboBox();
            txtUsuario = new TextBox();
            groupBox1 = new GroupBox();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            txtApellido = new TextBox();
            cmbTipoDoc = new ComboBox();
            txtNombre = new TextBox();
            txtNroDoc = new TextBox();
            txtTelefono = new TextBox();
            txtEmail = new TextBox();
            txtDireccion = new TextBox();
            label13 = new Label();
            lblTitulo = new Label();
            tabControl1.SuspendLayout();
            tabLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmpleados).BeginInit();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabLista);
            tabControl1.Font = new Font("Segoe UI", 10F);
            tabControl1.Location = new Point(14, 88);
            tabControl1.Margin = new Padding(3, 4, 3, 4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(894, 903);
            tabControl1.TabIndex = 1;
            // 
            // tabLista
            // 
            tabLista.BackColor = Color.White;
            tabLista.Controls.Add(txtBuscar);
            tabLista.Controls.Add(label12);
            tabLista.Controls.Add(label11);
            tabLista.Controls.Add(dgvEmpleados);
            tabLista.Location = new Point(4, 32);
            tabLista.Margin = new Padding(3, 4, 3, 4);
            tabLista.Name = "tabLista";
            tabLista.Padding = new Padding(3, 4, 3, 4);
            tabLista.Size = new Size(886, 867);
            tabLista.TabIndex = 1;
            tabLista.Text = "Listado";
            // 
            // txtBuscar
            // 
            txtBuscar.Font = new Font("Segoe UI", 10F);
            txtBuscar.Location = new Point(142, 83);
            txtBuscar.Margin = new Padding(3, 4, 3, 4);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(400, 30);
            txtBuscar.TabIndex = 1;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            txtBuscar.Enter += txtBuscar_Enter;
            txtBuscar.Leave += txtBuscar_Leave;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(54, 87);
            label12.Name = "label12";
            label12.Size = new Size(69, 23);
            label12.TabIndex = 2;
            label12.Text = "Buscar: ";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label11.Location = new Point(54, 29);
            label11.Name = "label11";
            label11.Size = new Size(310, 28);
            label11.TabIndex = 3;
            label11.Text = "Lista de Empleados Registrados";
            // 
            // dgvEmpleados
            // 
            dgvEmpleados.AllowUserToAddRows = false;
            dgvEmpleados.AllowUserToDeleteRows = false;
            dgvEmpleados.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvEmpleados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvEmpleados.BackgroundColor = Color.WhiteSmoke;
            dgvEmpleados.BorderStyle = BorderStyle.Fixed3D;
            dgvEmpleados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(200, 230, 255);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgvEmpleados.DefaultCellStyle = dataGridViewCellStyle1;
            dgvEmpleados.GridColor = Color.WhiteSmoke;
            dgvEmpleados.Location = new Point(10, 132);
            dgvEmpleados.Margin = new Padding(3, 4, 3, 4);
            dgvEmpleados.Name = "dgvEmpleados";
            dgvEmpleados.RowHeadersWidth = 51;
            dgvEmpleados.Size = new Size(870, 705);
            dgvEmpleados.TabIndex = 0;
            dgvEmpleados.CellClick += dgvEmpleados_CellClick;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(1266, 802);
            btnLimpiar.Margin = new Padding(3, 4, 3, 4);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(126, 48);
            btnLimpiar.TabIndex = 6;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(993, 802);
            btnGuardar.Margin = new Padding(3, 4, 3, 4);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(160, 48);
            btnGuardar.TabIndex = 8;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = Color.White;
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(txtContrasenia);
            groupBox2.Controls.Add(cmbRol);
            groupBox2.Controls.Add(txtUsuario);
            groupBox2.Font = new Font("Segoe UI", 10F);
            groupBox2.Location = new Point(914, 539);
            groupBox2.Margin = new Padding(3, 4, 3, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 4, 3, 4);
            groupBox2.Size = new Size(702, 231);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "Datos del Usuario";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(50, 133);
            label10.Name = "label10";
            label10.Size = new Size(137, 23);
            label10.TabIndex = 0;
            label10.Text = "Seleccione el Rol";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(336, 51);
            label9.Name = "label9";
            label9.Size = new Size(97, 23);
            label9.TabIndex = 1;
            label9.Text = "Contraseña";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(50, 49);
            label8.Name = "label8";
            label8.Size = new Size(68, 23);
            label8.TabIndex = 2;
            label8.Text = "Usuario";
            // 
            // txtContrasenia
            // 
            txtContrasenia.Font = new Font("Segoe UI", 10F);
            txtContrasenia.Location = new Point(336, 77);
            txtContrasenia.Margin = new Padding(3, 4, 3, 4);
            txtContrasenia.Name = "txtContrasenia";
            txtContrasenia.Size = new Size(292, 30);
            txtContrasenia.TabIndex = 3;
            // 
            // cmbRol
            // 
            cmbRol.Font = new Font("Segoe UI", 10F);
            cmbRol.FormattingEnabled = true;
            cmbRol.Location = new Point(34, 161);
            cmbRol.Margin = new Padding(3, 4, 3, 4);
            cmbRol.Name = "cmbRol";
            cmbRol.Size = new Size(394, 31);
            cmbRol.TabIndex = 4;
            // 
            // txtUsuario
            // 
            txtUsuario.Font = new Font("Segoe UI", 10F);
            txtUsuario.Location = new Point(34, 77);
            txtUsuario.Margin = new Padding(3, 4, 3, 4);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(274, 30);
            txtUsuario.TabIndex = 5;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.White;
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(txtApellido);
            groupBox1.Controls.Add(cmbTipoDoc);
            groupBox1.Controls.Add(txtNombre);
            groupBox1.Controls.Add(txtNroDoc);
            groupBox1.Controls.Add(txtTelefono);
            groupBox1.Controls.Add(txtEmail);
            groupBox1.Controls.Add(txtDireccion);
            groupBox1.Font = new Font("Segoe UI", 10F);
            groupBox1.Location = new Point(914, 184);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(702, 347);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Datos Personales";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(363, 217);
            label7.Name = "label7";
            label7.Size = new Size(51, 23);
            label7.TabIndex = 0;
            label7.Text = "Email";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(489, 132);
            label6.Name = "label6";
            label6.Size = new Size(74, 23);
            label6.TabIndex = 1;
            label6.Text = "Telefono";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(43, 217);
            label5.Name = "label5";
            label5.Size = new Size(81, 23);
            label5.TabIndex = 2;
            label5.Text = "Direccion";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(255, 132);
            label4.Name = "label4";
            label4.Size = new Size(158, 23);
            label4.TabIndex = 3;
            label4.Text = "Nro de Documento";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(43, 132);
            label3.Name = "label3";
            label3.Size = new Size(162, 23);
            label3.TabIndex = 4;
            label3.Text = "Tipo de Documento";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(363, 55);
            label2.Name = "label2";
            label2.Size = new Size(72, 23);
            label2.TabIndex = 5;
            label2.Text = "Apellido";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(43, 53);
            label1.Name = "label1";
            label1.Size = new Size(73, 23);
            label1.TabIndex = 6;
            label1.Text = "Nombre";
            // 
            // txtApellido
            // 
            txtApellido.Font = new Font("Segoe UI", 10F);
            txtApellido.Location = new Point(363, 81);
            txtApellido.Margin = new Padding(3, 4, 3, 4);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(312, 30);
            txtApellido.TabIndex = 7;
            // 
            // cmbTipoDoc
            // 
            cmbTipoDoc.Font = new Font("Segoe UI", 10F);
            cmbTipoDoc.FormattingEnabled = true;
            cmbTipoDoc.Location = new Point(39, 158);
            cmbTipoDoc.Margin = new Padding(3, 4, 3, 4);
            cmbTipoDoc.Name = "cmbTipoDoc";
            cmbTipoDoc.Size = new Size(185, 31);
            cmbTipoDoc.TabIndex = 8;
            // 
            // txtNombre
            // 
            txtNombre.Font = new Font("Segoe UI", 10F);
            txtNombre.Location = new Point(34, 81);
            txtNombre.Margin = new Padding(3, 4, 3, 4);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(317, 30);
            txtNombre.TabIndex = 9;
            // 
            // txtNroDoc
            // 
            txtNroDoc.Font = new Font("Segoe UI", 10F);
            txtNroDoc.Location = new Point(255, 158);
            txtNroDoc.Margin = new Padding(3, 4, 3, 4);
            txtNroDoc.Name = "txtNroDoc";
            txtNroDoc.Size = new Size(216, 30);
            txtNroDoc.TabIndex = 10;
            // 
            // txtTelefono
            // 
            txtTelefono.Font = new Font("Segoe UI", 10F);
            txtTelefono.Location = new Point(489, 158);
            txtTelefono.Margin = new Padding(3, 4, 3, 4);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(186, 30);
            txtTelefono.TabIndex = 11;
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Segoe UI", 10F);
            txtEmail.Location = new Point(352, 248);
            txtEmail.Margin = new Padding(3, 4, 3, 4);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(323, 30);
            txtEmail.TabIndex = 12;
            // 
            // txtDireccion
            // 
            txtDireccion.Font = new Font("Segoe UI", 10F);
            txtDireccion.Location = new Point(40, 248);
            txtDireccion.Margin = new Padding(3, 4, 3, 4);
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new Size(295, 30);
            txtDireccion.TabIndex = 13;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label13.Location = new Point(1018, 140);
            label13.Name = "label13";
            label13.Size = new Size(365, 28);
            label13.TabIndex = 9;
            label13.Text = "Registro/Actualizacion de Empleados";
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.FromArgb(45, 66, 91);
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Arial Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1689, 83);
            lblTitulo.TabIndex = 14;
            lblTitulo.Text = "MÓDULO DE EMPLEADOS";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmEmpleados
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightSteelBlue;
            ClientSize = new Size(1689, 1024);
            Controls.Add(lblTitulo);
            Controls.Add(label13);
            Controls.Add(tabControl1);
            Controls.Add(btnLimpiar);
            Controls.Add(groupBox1);
            Controls.Add(groupBox2);
            Controls.Add(btnGuardar);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(1168, 784);
            Name = "FrmEmpleados";
            Text = "Empleados - Casa de Repuestos";
            Load += FrmEmpleados_Load;
            tabControl1.ResumeLayout(false);
            tabLista.ResumeLayout(false);
            tabLista.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmpleados).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabLista;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DataGridView dgvEmpleados;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Label label12;
        private Button btnLimpiar;
        private Button btnGuardar;
        private GroupBox groupBox2;
        private Label label10;
        private Label label9;
        private Label label8;
        private TextBox txtContrasenia;
        private ComboBox cmbRol;
        private TextBox txtUsuario;
        private GroupBox groupBox1;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox txtApellido;
        private ComboBox cmbTipoDoc;
        private TextBox txtNombre;
        private TextBox txtNroDoc;
        private TextBox txtTelefono;
        private TextBox txtEmail;
        private TextBox txtDireccion;
        private Label label13;
        private Label lblTitulo;
    }
}
