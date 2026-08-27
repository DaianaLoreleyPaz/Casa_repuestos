namespace CasaRepuestos.Forms
{
    partial class FrmArticulo
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
            this.Load += new System.EventHandler(this.FrmArticulo_Load);

            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            tabControl1 = new TabControl();
            tabListado = new TabPage();
            groupBox2 = new GroupBox();
            txtBuscar = new TextBox();
            label10 = new Label();
            dgvArticulo = new DataGridView();
            btnEditar = new Button();
            label12 = new Label();
            btnGuardar = new Button();
            btnLimpiar = new Button();
            groupBoxArticuloDatos = new GroupBox();
            numPorcentajeGanancia = new NumericUpDown();
            cmbIVA = new ComboBox();
            label2 = new Label();
            label1 = new Label();
            dgvProveedoresCosto = new DataGridView();
            label15 = new Label();
            cmbMarca = new ComboBox();
            txtStock = new TextBox();
            label14 = new Label();
            label13 = new Label();
            txtPrecio = new TextBox();
            lblPrecio = new Label();
            txtNombre = new TextBox();
            lblNombre = new Label();
            panel1 = new Panel();
            lblTitulo = new Label();
            tabControl1.SuspendLayout();
            tabListado.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvArticulo).BeginInit();
            groupBoxArticuloDatos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPorcentajeGanancia).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvProveedoresCosto).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            tabControl1.Controls.Add(tabListado);
            tabControl1.Font = new Font("Segoe UI", 10F);
            tabControl1.Location = new Point(0, 76);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1055, 674);
            tabControl1.TabIndex = 17;
            // 
            // tabListado
            // 
            tabListado.BackColor = Color.White;
            tabListado.Controls.Add(groupBox2);
            tabListado.Location = new Point(4, 32);
            tabListado.Name = "tabListado";
            tabListado.Padding = new Padding(3);
            tabListado.Size = new Size(1047, 638);
            tabListado.TabIndex = 0;
            tabListado.Text = "Listado";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(txtBuscar);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(dgvArticulo);
            groupBox2.Location = new Point(22, 18);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(992, 614);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Listado de Artículos";
            // 
            // txtBuscar
            // 
            txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBuscar.BackColor = SystemColors.Menu;
            txtBuscar.Location = new Point(95, 30);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(844, 30);
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
            // dgvArticulo
            // 
            dgvArticulo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvArticulo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvArticulo.BackgroundColor = Color.White;
            dgvArticulo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(200, 230, 255);
            dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgvArticulo.DefaultCellStyle = dataGridViewCellStyle1;
            dgvArticulo.Location = new Point(6, 66);
            dgvArticulo.Name = "dgvArticulo";
            dgvArticulo.ReadOnly = true;
            dgvArticulo.RowHeadersWidth = 51;
            dgvArticulo.Size = new Size(980, 510);
            dgvArticulo.TabIndex = 0;
            dgvArticulo.CellClick += dgvArticulo_CellClick;
            // 
            // btnEditar
            // 
            btnEditar.BackColor = Color.FromArgb(255, 193, 7);
            btnEditar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEditar.Location = new Point(85, 548);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(110, 40);
            btnEditar.TabIndex = 11;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = false;
            btnEditar.Click += btnEditar_Click;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label12.Location = new Point(1097, 108);
            label12.Name = "label12";
            label12.Size = new Size(299, 32);
            label12.TabIndex = 12;
            label12.Text = "Agregar / Editar Artículo";
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(12, 87, 150);
            btnGuardar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGuardar.Location = new Point(232, 548);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(120, 40);
            btnGuardar.TabIndex = 9;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.BackColor = Color.Orange;
            btnLimpiar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLimpiar.Location = new Point(369, 552);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(120, 40);
            btnLimpiar.TabIndex = 7;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = false;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // groupBoxArticuloDatos
            // 
            groupBoxArticuloDatos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            groupBoxArticuloDatos.BackColor = Color.White;
            groupBoxArticuloDatos.Controls.Add(numPorcentajeGanancia);
            groupBoxArticuloDatos.Controls.Add(cmbIVA);
            groupBoxArticuloDatos.Controls.Add(label2);
            groupBoxArticuloDatos.Controls.Add(label1);
            groupBoxArticuloDatos.Controls.Add(dgvProveedoresCosto);
            groupBoxArticuloDatos.Controls.Add(btnLimpiar);
            groupBoxArticuloDatos.Controls.Add(btnEditar);
            groupBoxArticuloDatos.Controls.Add(btnGuardar);
            groupBoxArticuloDatos.Controls.Add(label15);
            groupBoxArticuloDatos.Controls.Add(cmbMarca);
            groupBoxArticuloDatos.Controls.Add(txtStock);
            groupBoxArticuloDatos.Controls.Add(label14);
            groupBoxArticuloDatos.Controls.Add(label13);
            groupBoxArticuloDatos.Controls.Add(txtPrecio);
            groupBoxArticuloDatos.Controls.Add(lblPrecio);
            groupBoxArticuloDatos.Controls.Add(txtNombre);
            groupBoxArticuloDatos.Controls.Add(lblNombre);
            groupBoxArticuloDatos.Location = new Point(1074, 168);
            groupBoxArticuloDatos.Name = "groupBoxArticuloDatos";
            groupBoxArticuloDatos.Size = new Size(533, 598);
            groupBoxArticuloDatos.TabIndex = 0;
            groupBoxArticuloDatos.TabStop = false;
            groupBoxArticuloDatos.Text = "Datos del Artículo";
            // 
            // numPorcentajeGanancia
            // 
            numPorcentajeGanancia.Location = new Point(194, 153);
            numPorcentajeGanancia.Name = "numPorcentajeGanancia";
            numPorcentajeGanancia.Size = new Size(103, 27);
            numPorcentajeGanancia.TabIndex = 36;
            numPorcentajeGanancia.ValueChanged += numPorcentajeGanancia_ValueChanged;
            // 
            // cmbIVA
            // 
            cmbIVA.FormattingEnabled = true;
            cmbIVA.Location = new Point(381, 154);
            cmbIVA.Name = "cmbIVA";
            cmbIVA.Size = new Size(117, 28);
            cmbIVA.TabIndex = 35;
            cmbIVA.SelectedIndexChanged += cmbIVA_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(332, 157);
            label2.Name = "label2";
            label2.Size = new Size(31, 20);
            label2.TabIndex = 34;
            label2.Text = "IVA";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(42, 157);
            label1.Name = "label1";
            label1.Size = new Size(143, 20);
            label1.TabIndex = 32;
            label1.Text = "Porcentaje Ganancia";
            // 
            // dgvProveedoresCosto
            // 
            dgvProveedoresCosto.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProveedoresCosto.Location = new Point(23, 215);
            dgvProveedoresCosto.Name = "dgvProveedoresCosto";
            dgvProveedoresCosto.RowHeadersWidth = 51;
            dgvProveedoresCosto.Size = new Size(489, 210);
            dgvProveedoresCosto.TabIndex = 30;
            dgvProveedoresCosto.CellEndEdit += dgvProveedoresCosto_CellEndEdit;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(42, 192);
            label15.Name = "label15";
            label15.Size = new Size(77, 20);
            label15.TabIndex = 25;
            label15.Text = "Proveedor";
            // 
            // cmbMarca
            // 
            cmbMarca.FormattingEnabled = true;
            cmbMarca.Location = new Point(130, 104);
            cmbMarca.Name = "cmbMarca";
            cmbMarca.Size = new Size(359, 28);
            cmbMarca.TabIndex = 22;
            // 
            // txtStock
            // 
            txtStock.Location = new Point(102, 474);
            txtStock.Name = "txtStock";
            txtStock.Size = new Size(334, 27);
            txtStock.TabIndex = 20;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(42, 104);
            label14.Name = "label14";
            label14.Size = new Size(50, 20);
            label14.TabIndex = 26;
            label14.Text = "Marca";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(39, 481);
            label13.Name = "label13";
            label13.Size = new Size(45, 20);
            label13.TabIndex = 27;
            label13.Text = "Stock";
            // 
            // txtPrecio
            // 
            txtPrecio.Location = new Point(139, 441);
            txtPrecio.Name = "txtPrecio";
            txtPrecio.Size = new Size(297, 27);
            txtPrecio.TabIndex = 18;
            // 
            // lblPrecio
            // 
            lblPrecio.AutoSize = true;
            lblPrecio.Location = new Point(39, 448);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.Size = new Size(85, 20);
            lblPrecio.TabIndex = 28;
            lblPrecio.Text = "Precio Final";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(130, 45);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(359, 27);
            txtNombre.TabIndex = 13;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(42, 45);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(64, 20);
            lblNombre.TabIndex = 29;
            lblNombre.Text = "Nombre";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(45, 66, 91);
            panel1.Controls.Add(lblTitulo);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1633, 70);
            panel1.TabIndex = 16;
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.FromArgb(45, 66, 91);
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Arial Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1633, 73);
            lblTitulo.TabIndex = 20;
            lblTitulo.Text = "MÓDULO DE PRODUCTOS";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmArticulo
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1633, 768);
            Controls.Add(panel1);
            Controls.Add(label12);
            Controls.Add(tabControl1);
            Controls.Add(groupBoxArticuloDatos);
            MinimumSize = new Size(1024, 600);
            Name = "FrmArticulo";
            Text = "Artículos - Casa de Repuestos";
            Load += FrmArticulo_Load;
            tabControl1.ResumeLayout(false);
            tabListado.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvArticulo).EndInit();
            groupBoxArticuloDatos.ResumeLayout(false);
            groupBoxArticuloDatos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPorcentajeGanancia).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvProveedoresCosto).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabListado;
        private System.Windows.Forms.GroupBox groupBox2;

        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView dgvArticulo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.GroupBox groupBoxArticuloDados; // legacy name not used
        private System.Windows.Forms.GroupBox groupBoxArticuloDatos;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtPrecio;
        private System.Windows.Forms.Label lblPrecio;
        private System.Windows.Forms.TextBox txtStock;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmbMarca;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel panel1;
        private Label lblTitulo;
        private DataGridView dgvProveedoresCosto;
        private Label label1;
        private Label label2;
        private NumericUpDown numPorcentajeGanancia;
        private ComboBox cmbIVA;
    }
}
