namespace CasaRepuestos.Forms
{
    partial class FrmMarca
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
            tabListado = new TabPage();
            groupBox2 = new GroupBox();
            txtBuscar = new TextBox();
            label10 = new Label();
            dgvMarca = new DataGridView();
            btnLimpiar = new Button();
            btnEditar = new Button();
            btnGuardar = new Button();
            lblNombre = new Label();
            txtNombre = new TextBox();
            groupBoxMarcaDatos = new GroupBox();
            label12 = new Label();
            lblTitulo = new Label();
            tabControl1.SuspendLayout();
            tabListado.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMarca).BeginInit();
            groupBoxMarcaDatos.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabListado);
            tabControl1.Font = new Font("Segoe UI", 10F);
            tabControl1.Location = new Point(23, 86);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(874, 650);
            tabControl1.TabIndex = 21;
            // 
            // tabListado
            // 
            tabListado.BackColor = Color.FromArgb(245, 250, 255);
            tabListado.Controls.Add(groupBox2);
            tabListado.Location = new Point(4, 32);
            tabListado.Name = "tabListado";
            tabListado.Padding = new Padding(3);
            tabListado.Size = new Size(866, 614);
            tabListado.TabIndex = 0;
            tabListado.Text = "Listado";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            groupBox2.Controls.Add(txtBuscar);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(dgvMarca);
            groupBox2.Location = new Point(6, 13);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(836, 1101);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Listado de Marcas";
            // 
            // txtBuscar
            // 
            txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBuscar.Location = new Point(120, 30);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(601, 30);
            txtBuscar.TabIndex = 2;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            txtBuscar.Enter += txtBuscar_Enter;
            txtBuscar.Leave += txtBuscar_Leave;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(20, 33);
            label10.Name = "label10";
            label10.Size = new Size(69, 23);
            label10.TabIndex = 8;
            label10.Text = "Buscar: ";
            // 
            // dgvMarca
            // 
            dgvMarca.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMarca.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMarca.BackgroundColor = Color.White;
            dgvMarca.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(200, 230, 255);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgvMarca.DefaultCellStyle = dataGridViewCellStyle1;
            dgvMarca.Location = new Point(10, 70);
            dgvMarca.Name = "dgvMarca";
            dgvMarca.ReadOnly = true;
            dgvMarca.RowHeadersWidth = 51;
            dgvMarca.Size = new Size(776, 1496);
            dgvMarca.TabIndex = 0;
            dgvMarca.CellClick += dgvMarca_CellClick;
            // 
            // btnLimpiar
            // 
            btnLimpiar.BackColor = Color.Orange;
            btnLimpiar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLimpiar.Location = new Point(1394, 372);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(120, 40);
            btnLimpiar.TabIndex = 7;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = false;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnEditar
            // 
            btnEditar.BackColor = Color.FromArgb(255, 193, 7);
            btnEditar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEditar.Location = new Point(1264, 372);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(110, 40);
            btnEditar.TabIndex = 11;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = false;
            btnEditar.Click += btnEditar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(12, 87, 150);
            btnGuardar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGuardar.Location = new Point(1124, 372);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(120, 40);
            btnGuardar.TabIndex = 9;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Segoe UI", 10F);
            lblNombre.Location = new Point(28, 53);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(73, 23);
            lblNombre.TabIndex = 14;
            lblNombre.Text = "Nombre";
            // 
            // txtNombre
            // 
            txtNombre.Font = new Font("Segoe UI", 10F);
            txtNombre.Location = new Point(28, 75);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(400, 30);
            txtNombre.TabIndex = 13;
            // 
            // groupBoxMarcaDatos
            // 
            groupBoxMarcaDatos.BackColor = Color.White;
            groupBoxMarcaDatos.Controls.Add(txtNombre);
            groupBoxMarcaDatos.Controls.Add(lblNombre);
            groupBoxMarcaDatos.Location = new Point(1029, 178);
            groupBoxMarcaDatos.Name = "groupBoxMarcaDatos";
            groupBoxMarcaDatos.Size = new Size(567, 161);
            groupBoxMarcaDatos.TabIndex = 0;
            groupBoxMarcaDatos.TabStop = false;
            groupBoxMarcaDatos.Text = "Datos de la Marca";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label12.Location = new Point(1168, 131);
            label12.Name = "label12";
            label12.Size = new Size(277, 32);
            label12.TabIndex = 12;
            label12.Text = "Agregar / Editar Marca";
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.FromArgb(45, 66, 91);
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Arial Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1608, 83);
            lblTitulo.TabIndex = 22;
            lblTitulo.Text = "MÓDULO DE MARCAS";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmMarca
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1608, 768);
            Controls.Add(lblTitulo);
            Controls.Add(btnEditar);
            Controls.Add(label12);
            Controls.Add(btnGuardar);
            Controls.Add(tabControl1);
            Controls.Add(btnLimpiar);
            Controls.Add(groupBoxMarcaDatos);
            Name = "FrmMarca";
            Text = "Marcas - Casa de Repuestos";
            Load += FrmMarca_Load;
            tabControl1.ResumeLayout(false);
            tabListado.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMarca).EndInit();
            groupBoxMarcaDatos.ResumeLayout(false);
            groupBoxMarcaDatos.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabListado;
        private System.Windows.Forms.GroupBox groupBox2;
 
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView dgvMarca;
        private Button btnLimpiar;
        private Button btnEditar;
        private Button btnGuardar;
        private Label lblNombre;
        private TextBox txtNombre;
        private GroupBox groupBoxMarcaDatos;
        private Label label12;
        private Label lblTitulo;
    }
}
