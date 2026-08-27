namespace CasaRepuestos.Forms
{
    partial class FrmInventario
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmInventario));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            groupBoxEstadisticas = new GroupBox();
            lblArticulosBajoStock = new Label();
            lblStockGlobal = new Label();
            lblTotalArticulos = new Label();
            tabControl1 = new TabControl();
            tabListado = new TabPage();
            groupBox2 = new GroupBox();
            btnGenerarPDF = new Button();
            pictureBox1 = new PictureBox();
            btnMostrarBajoStock = new Button();
            txtBuscar = new TextBox();
            label10 = new Label();
            dgvArticulo = new DataGridView();
            lblTitulo = new Label();
            groupBoxEstadisticas.SuspendLayout();
            tabControl1.SuspendLayout();
            tabListado.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvArticulo).BeginInit();
            SuspendLayout();
            // 
            // groupBoxEstadisticas
            // 
            groupBoxEstadisticas.BackColor = Color.LightSlateGray;
            groupBoxEstadisticas.Controls.Add(lblArticulosBajoStock);
            groupBoxEstadisticas.Controls.Add(lblStockGlobal);
            groupBoxEstadisticas.Controls.Add(lblTotalArticulos);
            groupBoxEstadisticas.Location = new Point(1090, 120);
            groupBoxEstadisticas.Margin = new Padding(3, 4, 3, 4);
            groupBoxEstadisticas.Name = "groupBoxEstadisticas";
            groupBoxEstadisticas.Padding = new Padding(3, 4, 3, 4);
            groupBoxEstadisticas.Size = new Size(457, 187);
            groupBoxEstadisticas.TabIndex = 4;
            groupBoxEstadisticas.TabStop = false;
            groupBoxEstadisticas.Text = "Estadísticas del Inventario";
            // 
            // lblArticulosBajoStock
            // 
            lblArticulosBajoStock.AutoSize = true;
            lblArticulosBajoStock.Location = new Point(18, 113);
            lblArticulosBajoStock.Name = "lblArticulosBajoStock";
            lblArticulosBajoStock.Size = new Size(201, 20);
            lblArticulosBajoStock.TabIndex = 0;
            lblArticulosBajoStock.Text = "Artículos con bajo stock: N/A";
            // 
            // lblStockGlobal
            // 
            lblStockGlobal.AutoSize = true;
            lblStockGlobal.Location = new Point(18, 73);
            lblStockGlobal.Name = "lblStockGlobal";
            lblStockGlobal.Size = new Size(126, 20);
            lblStockGlobal.TabIndex = 1;
            lblStockGlobal.Text = "Stock global: N/A";
            // 
            // lblTotalArticulos
            // 
            lblTotalArticulos.AutoSize = true;
            lblTotalArticulos.Location = new Point(18, 33);
            lblTotalArticulos.Name = "lblTotalArticulos";
            lblTotalArticulos.Size = new Size(184, 20);
            lblTotalArticulos.TabIndex = 2;
            lblTotalArticulos.Text = "Cantidad de artículos: N/A";
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            tabControl1.Controls.Add(tabListado);
            tabControl1.Font = new Font("Segoe UI", 10F);
            tabControl1.Location = new Point(23, 120);
            tabControl1.Margin = new Padding(3, 4, 3, 4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1046, 853);
            tabControl1.TabIndex = 1;
            // 
            // tabListado
            // 
            tabListado.BackColor = Color.FromArgb(245, 250, 255);
            tabListado.Controls.Add(groupBox2);
            tabListado.Location = new Point(4, 32);
            tabListado.Margin = new Padding(3, 4, 3, 4);
            tabListado.Name = "tabListado";
            tabListado.Padding = new Padding(3, 4, 3, 4);
            tabListado.Size = new Size(1038, 817);
            tabListado.TabIndex = 0;
            tabListado.Text = "Artículos";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnGenerarPDF);
            groupBox2.Controls.Add(pictureBox1);
            groupBox2.Controls.Add(btnMostrarBajoStock);
            groupBox2.Controls.Add(txtBuscar);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(dgvArticulo);
            groupBox2.Location = new Point(9, 8);
            groupBox2.Margin = new Padding(3, 4, 3, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 4, 3, 4);
            groupBox2.Size = new Size(1021, 780);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Listado";
            // 
            // btnGenerarPDF
            // 
            btnGenerarPDF.BackColor = Color.LightSteelBlue;
            btnGenerarPDF.Location = new Point(630, 37);
            btnGenerarPDF.Margin = new Padding(3, 4, 3, 4);
            btnGenerarPDF.Name = "btnGenerarPDF";
            btnGenerarPDF.Size = new Size(160, 40);
            btnGenerarPDF.TabIndex = 0;
            btnGenerarPDF.Text = "Generar PDF";
            btnGenerarPDF.UseVisualStyleBackColor = false;
            btnGenerarPDF.Click += btnGenerarPDF_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Cursor = Cursors.Hand;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(571, 35);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(39, 43);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // btnMostrarBajoStock
            // 
            btnMostrarBajoStock.Location = new Point(457, 37);
            btnMostrarBajoStock.Margin = new Padding(3, 4, 3, 4);
            btnMostrarBajoStock.Name = "btnMostrarBajoStock";
            btnMostrarBajoStock.Size = new Size(91, 40);
            btnMostrarBajoStock.TabIndex = 5;
            btnMostrarBajoStock.Text = "Bajo stock";
            btnMostrarBajoStock.UseVisualStyleBackColor = true;
            btnMostrarBajoStock.Click += btnMostrarBajoStock_Click;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(114, 40);
            txtBuscar.Margin = new Padding(3, 4, 3, 4);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(308, 30);
            txtBuscar.TabIndex = 2;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            txtBuscar.Enter += txtBuscar_Enter;
            txtBuscar.Leave += txtBuscar_Leave;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(23, 44);
            label10.Name = "label10";
            label10.Size = new Size(69, 23);
            label10.TabIndex = 6;
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
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(200, 230, 255);
            dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgvArticulo.DefaultCellStyle = dataGridViewCellStyle1;
            dgvArticulo.Location = new Point(7, 81);
            dgvArticulo.Margin = new Padding(3, 4, 3, 4);
            dgvArticulo.Name = "dgvArticulo";
            dgvArticulo.RowHeadersWidth = 51;
            dgvArticulo.Size = new Size(1007, 691);
            dgvArticulo.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.FromArgb(45, 66, 91);
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Arial Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1561, 83);
            lblTitulo.TabIndex = 14;
            lblTitulo.Text = "MÓDULO DE INVENTARIO";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmInventario
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightSteelBlue;
            ClientSize = new Size(1561, 1024);
            Controls.Add(lblTitulo);
            Controls.Add(tabControl1);
            Controls.Add(groupBoxEstadisticas);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(1168, 784);
            Name = "FrmInventario";
            Text = "Control de Inventario";
            Load += FrmInventario_Load;
            groupBoxEstadisticas.ResumeLayout(false);
            groupBoxEstadisticas.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabListado.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvArticulo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxEstadisticas;
        private System.Windows.Forms.Label lblStockGlobal;
        private System.Windows.Forms.Label lblTotalArticulos;
        private System.Windows.Forms.Label lblArticulosBajoStock;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabListado;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView dgvArticulo;
        private System.Windows.Forms.Button btnMostrarBajoStock;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnGenerarPDF;
        private Label lblTitulo;
    }
}
