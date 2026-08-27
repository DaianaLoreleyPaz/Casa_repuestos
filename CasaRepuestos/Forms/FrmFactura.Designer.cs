namespace CasaRepuestos.Forms
{
    partial class FrmFactura
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            groupBox4 = new GroupBox();
            comboBoxMetPago = new ComboBox();
            label2 = new Label();
            label4 = new Label();
            textBoxDescripcionMetPago = new TextBox();
            groupBox3 = new GroupBox();
            btnAddArticulo = new Button();
            textBoxCantidad = new TextBox();
            label3 = new Label();
            comboBoxArticulo = new ComboBox();
            label5 = new Label();
            groupBox2 = new GroupBox();
            btnLimpiarSeleccion = new Button();
            lblModoOperacion = new Label();
            dataGridViewDetallesReparacion = new DataGridView();
            label7 = new Label();
            label6 = new Label();
            btnGuardarVenta = new Button();
            labelTotal = new Label();
            dataGridViewDetallesVenta = new DataGridView();
            groupBox1 = new GroupBox();
            dataGridViewReparacionesCliente = new DataGridView();
            panel1 = new Panel();
            label14 = new Label();
            groupBoxClientes = new GroupBox();
            comboBoxClientes = new ComboBox();
            buttonAgregarNvoCliente = new Button();
            label8 = new Label();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDetallesReparacion).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDetallesVenta).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewReparacionesCliente).BeginInit();
            panel1.SuspendLayout();
            groupBoxClientes.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox4
            // 
            groupBox4.BackColor = Color.FromArgb(224, 224, 224);
            groupBox4.Controls.Add(comboBoxMetPago);
            groupBox4.Controls.Add(label2);
            groupBox4.Controls.Add(label4);
            groupBox4.Controls.Add(textBoxDescripcionMetPago);
            groupBox4.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            groupBox4.ForeColor = Color.FromArgb(52, 73, 94);
            groupBox4.Location = new Point(702, 196);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(827, 115);
            groupBox4.TabIndex = 46;
            groupBox4.TabStop = false;
            groupBox4.Text = "Método de pago";
            // 
            // comboBoxMetPago
            // 
            comboBoxMetPago.Font = new Font("Segoe UI", 12F);
            comboBoxMetPago.FormattingEnabled = true;
            comboBoxMetPago.Location = new Point(20, 58);
            comboBoxMetPago.Name = "comboBoxMetPago";
            comboBoxMetPago.Size = new Size(312, 36);
            comboBoxMetPago.TabIndex = 32;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(20, 35);
            label2.Name = "label2";
            label2.Size = new Size(200, 20);
            label2.TabIndex = 26;
            label2.Text = "Seleccione método de pago";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(357, 35);
            label4.Name = "label4";
            label4.Size = new Size(90, 20);
            label4.TabIndex = 30;
            label4.Text = "Descripción";
            // 
            // textBoxDescripcionMetPago
            // 
            textBoxDescripcionMetPago.Font = new Font("Segoe UI", 12F);
            textBoxDescripcionMetPago.Location = new Point(357, 59);
            textBoxDescripcionMetPago.Name = "textBoxDescripcionMetPago";
            textBoxDescripcionMetPago.Size = new Size(336, 34);
            textBoxDescripcionMetPago.TabIndex = 31;
            // 
            // groupBox3
            // 
            groupBox3.BackColor = Color.FromArgb(224, 224, 224);
            groupBox3.Controls.Add(btnAddArticulo);
            groupBox3.Controls.Add(textBoxCantidad);
            groupBox3.Controls.Add(label3);
            groupBox3.Controls.Add(comboBoxArticulo);
            groupBox3.Controls.Add(label5);
            groupBox3.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            groupBox3.ForeColor = Color.FromArgb(52, 73, 94);
            groupBox3.Location = new Point(702, 317);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(827, 120);
            groupBox3.TabIndex = 45;
            groupBox3.TabStop = false;
            groupBox3.Text = "Añadir artículo";
            // 
            // btnAddArticulo
            // 
            btnAddArticulo.BackColor = Color.FromArgb(52, 152, 219);
            btnAddArticulo.FlatAppearance.BorderSize = 0;
            btnAddArticulo.FlatStyle = FlatStyle.Flat;
            btnAddArticulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAddArticulo.ForeColor = Color.White;
            btnAddArticulo.Location = new Point(543, 54);
            btnAddArticulo.Name = "btnAddArticulo";
            btnAddArticulo.Size = new Size(150, 40);
            btnAddArticulo.TabIndex = 37;
            btnAddArticulo.Text = "Agregar";
            btnAddArticulo.UseVisualStyleBackColor = false;
            btnAddArticulo.Click += btnAddArticulo_Click;
            // 
            // textBoxCantidad
            // 
            textBoxCantidad.Font = new Font("Segoe UI", 12F);
            textBoxCantidad.Location = new Point(357, 60);
            textBoxCantidad.Name = "textBoxCantidad";
            textBoxCantidad.Size = new Size(144, 34);
            textBoxCantidad.TabIndex = 35;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(20, 37);
            label3.Name = "label3";
            label3.Size = new Size(65, 20);
            label3.TabIndex = 28;
            label3.Text = "Artículo";
            // 
            // comboBoxArticulo
            // 
            comboBoxArticulo.Font = new Font("Segoe UI", 12F);
            comboBoxArticulo.FormattingEnabled = true;
            comboBoxArticulo.Location = new Point(20, 60);
            comboBoxArticulo.Name = "comboBoxArticulo";
            comboBoxArticulo.Size = new Size(312, 36);
            comboBoxArticulo.TabIndex = 33;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.ForeColor = Color.Black;
            label5.Location = new Point(357, 37);
            label5.Name = "label5";
            label5.Size = new Size(71, 20);
            label5.TabIndex = 34;
            label5.Text = "Cantidad";
            // 
            // groupBox2
            // 
            groupBox2.BackColor = Color.FromArgb(224, 224, 224);
            groupBox2.Controls.Add(btnLimpiarSeleccion);
            groupBox2.Controls.Add(lblModoOperacion);
            groupBox2.Controls.Add(dataGridViewDetallesReparacion);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(btnGuardarVenta);
            groupBox2.Controls.Add(labelTotal);
            groupBox2.Controls.Add(dataGridViewDetallesVenta);
            groupBox2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            groupBox2.ForeColor = Color.FromArgb(52, 73, 94);
            groupBox2.Location = new Point(0, 443);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1529, 578);
            groupBox2.TabIndex = 44;
            groupBox2.TabStop = false;
            groupBox2.Text = "Detalles de la venta";
            // 
            // btnLimpiarSeleccion
            // 
            btnLimpiarSeleccion.Location = new Point(7, 457);
            btnLimpiarSeleccion.Name = "btnLimpiarSeleccion";
            btnLimpiarSeleccion.Size = new Size(107, 39);
            btnLimpiarSeleccion.TabIndex = 44;
            btnLimpiarSeleccion.Text = "Limpiar";
            btnLimpiarSeleccion.UseVisualStyleBackColor = true;
            btnLimpiarSeleccion.Click += btnLimpiarSeleccion_Click;
            // 
            // lblModoOperacion
            // 
            lblModoOperacion.AutoSize = true;
            lblModoOperacion.Location = new Point(6, 378);
            lblModoOperacion.Name = "lblModoOperacion";
            lblModoOperacion.Size = new Size(17, 28);
            lblModoOperacion.TabIndex = 43;
            lblModoOperacion.Text = ".";
            // 
            // dataGridViewDetallesReparacion
            // 
            dataGridViewDetallesReparacion.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewDetallesReparacion.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewDetallesReparacion.Location = new Point(6, 66);
            dataGridViewDetallesReparacion.Name = "dataGridViewDetallesReparacion";
            dataGridViewDetallesReparacion.RowHeadersWidth = 51;
            dataGridViewDetallesReparacion.Size = new Size(827, 300);
            dataGridViewDetallesReparacion.TabIndex = 42;
            dataGridViewDetallesReparacion.CellClick += DataGridViewDetallesVenta_CellContentClick;
            dataGridViewDetallesReparacion.CellContentClick += DataGridViewDetallesVenta_CellContentClick;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.Black;
            label7.Location = new Point(839, 38);
            label7.Name = "label7";
            label7.Size = new Size(194, 25);
            label7.TabIndex = 41;
            label7.Text = "Artículos adicionales:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.Black;
            label6.Location = new Point(7, 38);
            label6.Name = "label6";
            label6.Size = new Size(93, 25);
            label6.TabIndex = 33;
            label6.Text = "Servicios:";
            // 
            // btnGuardarVenta
            // 
            btnGuardarVenta.BackColor = Color.FromArgb(46, 204, 113);
            btnGuardarVenta.FlatAppearance.BorderSize = 0;
            btnGuardarVenta.FlatStyle = FlatStyle.Flat;
            btnGuardarVenta.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnGuardarVenta.ForeColor = Color.White;
            btnGuardarVenta.Location = new Point(1347, 399);
            btnGuardarVenta.Name = "btnGuardarVenta";
            btnGuardarVenta.Size = new Size(153, 50);
            btnGuardarVenta.TabIndex = 40;
            btnGuardarVenta.Text = "💾 Guardar ";
            btnGuardarVenta.UseVisualStyleBackColor = false;
            btnGuardarVenta.Click += btnGuardarVenta_Click;
            // 
            // labelTotal
            // 
            labelTotal.AutoSize = true;
            labelTotal.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTotal.Location = new Point(839, 399);
            labelTotal.Name = "labelTotal";
            labelTotal.Size = new Size(117, 41);
            labelTotal.TabIndex = 38;
            labelTotal.Text = "TOTAL:";
            // 
            // dataGridViewDetallesVenta
            // 
            dataGridViewDetallesVenta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewDetallesVenta.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(52, 73, 94);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridViewDetallesVenta.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewDetallesVenta.Location = new Point(839, 66);
            dataGridViewDetallesVenta.Name = "dataGridViewDetallesVenta";
            dataGridViewDetallesVenta.RowHeadersWidth = 51;
            dataGridViewDetallesVenta.Size = new Size(684, 300);
            dataGridViewDetallesVenta.TabIndex = 37;
            dataGridViewDetallesVenta.CellContentClick += DataGridViewDetallesVenta_CellContentClick;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.FromArgb(224, 224, 224);
            groupBox1.Controls.Add(dataGridViewReparacionesCliente);
            groupBox1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            groupBox1.ForeColor = Color.FromArgb(52, 73, 94);
            groupBox1.Location = new Point(0, 76);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(696, 361);
            groupBox1.TabIndex = 43;
            groupBox1.TabStop = false;
            groupBox1.Text = "Reparaciones";
            // 
            // dataGridViewReparacionesCliente
            // 
            dataGridViewReparacionesCliente.AllowUserToOrderColumns = true;
            dataGridViewReparacionesCliente.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewReparacionesCliente.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewReparacionesCliente.Location = new Point(6, 49);
            dataGridViewReparacionesCliente.Name = "dataGridViewReparacionesCliente";
            dataGridViewReparacionesCliente.RowHeadersWidth = 51;
            dataGridViewReparacionesCliente.Size = new Size(674, 286);
            dataGridViewReparacionesCliente.TabIndex = 24;
            dataGridViewReparacionesCliente.CellClick += DataGridViewReparacionesCliente_CellClick;
            dataGridViewReparacionesCliente.CellContentClick += DataGridViewReparacionesCliente_CellClick;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(52, 73, 94);
            panel1.Controls.Add(label14);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1902, 70);
            panel1.TabIndex = 42;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            label14.ForeColor = Color.White;
            label14.Location = new Point(380, 15);
            label14.Name = "label14";
            label14.Size = new Size(285, 37);
            label14.TabIndex = 1;
            label14.Text = "MÓDULO DE VENTAS";
            // 
            // groupBoxClientes
            // 
            groupBoxClientes.BackColor = Color.FromArgb(224, 224, 224);
            groupBoxClientes.Controls.Add(comboBoxClientes);
            groupBoxClientes.Controls.Add(buttonAgregarNvoCliente);
            groupBoxClientes.Controls.Add(label8);
            groupBoxClientes.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            groupBoxClientes.ForeColor = Color.FromArgb(52, 73, 94);
            groupBoxClientes.Location = new Point(702, 76);
            groupBoxClientes.Name = "groupBoxClientes";
            groupBoxClientes.Size = new Size(827, 114);
            groupBoxClientes.TabIndex = 47;
            groupBoxClientes.TabStop = false;
            groupBoxClientes.Text = "Clientes";
            // 
            // comboBoxClientes
            // 
            comboBoxClientes.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboBoxClientes.FormattingEnabled = true;
            comboBoxClientes.Location = new Point(11, 58);
            comboBoxClientes.Name = "comboBoxClientes";
            comboBoxClientes.Size = new Size(336, 36);
            comboBoxClientes.TabIndex = 39;
            comboBoxClientes.SelectedIndexChanged += comboBoxClientes_SelectedIndexChanged;
            comboBoxClientes.TextUpdate += comboBoxClientes_TextUpdate;
            // 
            // buttonAgregarNvoCliente
            // 
            buttonAgregarNvoCliente.BackColor = Color.FromArgb(52, 152, 219);
            buttonAgregarNvoCliente.FlatAppearance.BorderSize = 0;
            buttonAgregarNvoCliente.FlatStyle = FlatStyle.Flat;
            buttonAgregarNvoCliente.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            buttonAgregarNvoCliente.ForeColor = Color.White;
            buttonAgregarNvoCliente.Location = new Point(370, 58);
            buttonAgregarNvoCliente.Name = "buttonAgregarNvoCliente";
            buttonAgregarNvoCliente.Size = new Size(172, 40);
            buttonAgregarNvoCliente.TabIndex = 38;
            buttonAgregarNvoCliente.Text = "+ Agregar nuevo";
            buttonAgregarNvoCliente.UseVisualStyleBackColor = false;
            buttonAgregarNvoCliente.Click += buttonAgregarNvoCliente_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.Black;
            label8.Location = new Point(7, 32);
            label8.Name = "label8";
            label8.Size = new Size(56, 20);
            label8.TabIndex = 23;
            label8.Text = "Buscar";
            // 
            // FrmFactura
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1902, 1033);
            Controls.Add(groupBoxClientes);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(panel1);
            Name = "FrmFactura";
            Text = "FrmFactura";
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDetallesReparacion).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDetallesVenta).EndInit();
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewReparacionesCliente).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBoxClientes.ResumeLayout(false);
            groupBoxClientes.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox4;
        private ComboBox comboBoxMetPago;
        private Label label2;
        private Label label4;
        private TextBox textBoxDescripcionMetPago;
        private GroupBox groupBox3;
        private Button btnAddArticulo;
        private TextBox textBoxCantidad;
        private Label label3;
        private ComboBox comboBoxArticulo;
        private Label label5;
        private GroupBox groupBox2;
        private Button btnGuardarVenta;
        private Label labelTotal;
        private DataGridView dataGridViewDetallesVenta;
        private GroupBox groupBox1;
        private DataGridView dataGridViewReparacionesCliente;
        private Panel panel1;
        private Label label14;
        private DataGridView dataGridViewDetallesReparacion;
        private Label label7;
        private Label label6;
        private GroupBox groupBoxClientes;
        private Label label8;
        private Button buttonAgregarNvoCliente;
        private ComboBox comboBoxClientes;
        private Label lblModoOperacion;
        private Button btnLimpiarSeleccion;
    }
}