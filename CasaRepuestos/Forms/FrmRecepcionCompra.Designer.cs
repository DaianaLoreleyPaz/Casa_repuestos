namespace CasaRepuestos.Forms
{
    partial class FrmRecepcionCompra
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            groupBox1 = new GroupBox();
            lblFechaEmision = new Label();
            label4 = new Label();
            lblProveedor = new Label();
            label2 = new Label();
            lblNumeroOrden = new Label();
            label1 = new Label();
            groupBox2 = new GroupBox();
            dgvDetallesRecepcion = new DataGridView();
            IdArticulo = new DataGridViewTextBoxColumn();
            NombreArticulo = new DataGridViewTextBoxColumn();
            CantidadRecibida = new DataGridViewTextBoxColumn();
            PrecioFinal = new DataGridViewTextBoxColumn();
            Subtotal = new DataGridViewTextBoxColumn();
            btnConfirmarIngreso = new Button();
            btnCancelar = new Button();
            txtTotalFinal = new TextBox();
            label3 = new Label();
            cmbMetodoPago = new ComboBox();
            label5 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetallesRecepcion).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(lblFechaEmision);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(lblProveedor);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(lblNumeroOrden);
            groupBox1.Controls.Add(label1);
            groupBox1.Font = new Font("Arial", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(12, 15);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(918, 125);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Datos de la Orden de Compra";
            // 
            // lblFechaEmision
            // 
            lblFechaEmision.AutoSize = true;
            lblFechaEmision.Font = new Font("Arial", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFechaEmision.Location = new Point(145, 82);
            lblFechaEmision.Name = "lblFechaEmision";
            lblFechaEmision.Size = new Size(37, 19);
            lblFechaEmision.TabIndex = 5;
            lblFechaEmision.Text = "N/A";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 82);
            label4.Name = "label4";
            label4.Size = new Size(123, 19);
            label4.TabIndex = 4;
            label4.Text = "Fecha Emisión:";
            // 
            // lblProveedor
            // 
            lblProveedor.AutoSize = true;
            lblProveedor.Font = new Font("Arial", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblProveedor.Location = new Point(408, 45);
            lblProveedor.Name = "lblProveedor";
            lblProveedor.Size = new Size(37, 19);
            lblProveedor.TabIndex = 3;
            lblProveedor.Text = "N/A";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(308, 45);
            label2.Name = "label2";
            label2.Size = new Size(89, 19);
            label2.TabIndex = 2;
            label2.Text = "Proveedor:";
            // 
            // lblNumeroOrden
            // 
            lblNumeroOrden.AutoSize = true;
            lblNumeroOrden.Font = new Font("Arial", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNumeroOrden.ForeColor = Color.Firebrick;
            lblNumeroOrden.Location = new Point(111, 45);
            lblNumeroOrden.Name = "lblNumeroOrden";
            lblNumeroOrden.Size = new Size(37, 19);
            lblNumeroOrden.TabIndex = 1;
            lblNumeroOrden.Text = "N/A";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 45);
            label1.Name = "label1";
            label1.Size = new Size(82, 19);
            label1.TabIndex = 0;
            label1.Text = "N° Orden:";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(dgvDetallesRecepcion);
            groupBox2.Font = new Font("Arial", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(12, 159);
            groupBox2.Margin = new Padding(3, 4, 3, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 4, 3, 4);
            groupBox2.Size = new Size(918, 549);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Detalles de la Recepción (Edite la cantidad o precio si es necesario)";
            // 
            // dgvDetallesRecepcion
            // 
            dgvDetallesRecepcion.AllowUserToAddRows = false;
            dgvDetallesRecepcion.AllowUserToDeleteRows = false;
            dgvDetallesRecepcion.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetallesRecepcion.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetallesRecepcion.Columns.AddRange(new DataGridViewColumn[] { IdArticulo, NombreArticulo, CantidadRecibida, PrecioFinal, Subtotal });
            dgvDetallesRecepcion.Dock = DockStyle.Fill;
            dgvDetallesRecepcion.Location = new Point(3, 24);
            dgvDetallesRecepcion.Margin = new Padding(3, 4, 3, 4);
            dgvDetallesRecepcion.Name = "dgvDetallesRecepcion";
            dgvDetallesRecepcion.RowHeadersWidth = 51;
            dgvDetallesRecepcion.RowTemplate.Height = 24;
            dgvDetallesRecepcion.Size = new Size(912, 521);
            dgvDetallesRecepcion.TabIndex = 0;
            // 
            // IdArticulo
            // 
            IdArticulo.HeaderText = "IdArticulo";
            IdArticulo.MinimumWidth = 6;
            IdArticulo.Name = "IdArticulo";
            IdArticulo.Visible = false;
            // 
            // NombreArticulo
            // 
            NombreArticulo.FillWeight = 200F;
            NombreArticulo.HeaderText = "Artículo";
            NombreArticulo.MinimumWidth = 6;
            NombreArticulo.Name = "NombreArticulo";
            NombreArticulo.ReadOnly = true;
            // 
            // CantidadRecibida
            // 
            CantidadRecibida.HeaderText = "Cantidad Recibida";
            CantidadRecibida.MinimumWidth = 6;
            CantidadRecibida.Name = "CantidadRecibida";
            // 
            // PrecioFinal
            // 
            dataGridViewCellStyle1.Format = "C2";
            PrecioFinal.DefaultCellStyle = dataGridViewCellStyle1;
            PrecioFinal.HeaderText = "Precio Unit. Final";
            PrecioFinal.MinimumWidth = 6;
            PrecioFinal.Name = "PrecioFinal";
            // 
            // Subtotal
            // 
            dataGridViewCellStyle2.BackColor = Color.FromArgb(224, 224, 224);
            dataGridViewCellStyle2.Format = "C2";
            Subtotal.DefaultCellStyle = dataGridViewCellStyle2;
            Subtotal.HeaderText = "Subtotal";
            Subtotal.MinimumWidth = 6;
            Subtotal.Name = "Subtotal";
            Subtotal.ReadOnly = true;
            // 
            // btnConfirmarIngreso
            // 
            btnConfirmarIngreso.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnConfirmarIngreso.BackColor = Color.FromArgb(40, 167, 69);
            btnConfirmarIngreso.Font = new Font("Arial", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnConfirmarIngreso.ForeColor = Color.White;
            btnConfirmarIngreso.Location = new Point(737, 722);
            btnConfirmarIngreso.Margin = new Padding(3, 4, 3, 4);
            btnConfirmarIngreso.Name = "btnConfirmarIngreso";
            btnConfirmarIngreso.Size = new Size(193, 76);
            btnConfirmarIngreso.TabIndex = 2;
            btnConfirmarIngreso.Text = "Confirmar Ingreso y Actualizar Stock";
            btnConfirmarIngreso.UseVisualStyleBackColor = false;
            btnConfirmarIngreso.Click += btnConfirmarIngreso_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCancelar.BackColor = Color.Gray;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Font = new Font("Arial", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(15, 728);
            btnCancelar.Margin = new Padding(3, 4, 3, 4);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 64);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            // 
            // txtTotalFinal
            // 
            txtTotalFinal.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            txtTotalFinal.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtTotalFinal.Location = new Point(448, 744);
            txtTotalFinal.Margin = new Padding(3, 4, 3, 4);
            txtTotalFinal.Name = "txtTotalFinal";
            txtTotalFinal.ReadOnly = true;
            txtTotalFinal.Size = new Size(269, 30);
            txtTotalFinal.TabIndex = 4;
            txtTotalFinal.TextAlign = HorizontalAlignment.Right;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(448, 708);
            label3.Name = "label3";
            label3.Size = new Size(115, 24);
            label3.TabIndex = 5;
            label3.Text = "Total Final:";
            // 
            // cmbMetodoPago
            // 
            cmbMetodoPago.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMetodoPago.FormattingEnabled = true;
            cmbMetodoPago.Location = new Point(179, 747);
            cmbMetodoPago.Name = "cmbMetodoPago";
            cmbMetodoPago.Size = new Size(230, 28);
            cmbMetodoPago.TabIndex = 6;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(179, 712);
            label5.Name = "label5";
            label5.Size = new Size(171, 24);
            label5.TabIndex = 7;
            label5.Text = "Metodo de Pago:";
            // 
            // FrmRecepcionCompra
            // 
            AcceptButton = btnConfirmarIngreso;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(942, 806);
            Controls.Add(label5);
            Controls.Add(cmbMetodoPago);
            Controls.Add(label3);
            Controls.Add(txtTotalFinal);
            Controls.Add(btnCancelar);
            Controls.Add(btnConfirmarIngreso);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(960, 853);
            Name = "FrmRecepcionCompra";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Registrar Ingreso de Mercadería";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDetallesRecepcion).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblNumeroOrden;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblProveedor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblFechaEmision;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvDetallesRecepcion;
        private System.Windows.Forms.Button btnConfirmarIngreso;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.TextBox txtTotalFinal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdArticulo;
        private System.Windows.Forms.DataGridViewTextBoxColumn NombreArticulo;
        private System.Windows.Forms.DataGridViewTextBoxColumn CantidadRecibida;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrecioFinal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subtotal;
        private ComboBox cmbMetodoPago;
        private Label label5;
    }
}