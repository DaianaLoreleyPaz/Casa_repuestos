namespace CasaRepuestos.Forms
{
    partial class FrmArqueo
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            panelSuperior = new Panel();
            comboCajas = new ComboBox();
            labelFecha = new Label();
            labelCaja = new Label();
            panelResumen = new Panel();
            cardMontoInicial = new GroupBox();
            lblMontoInicial = new Label();
            cardIngresos = new GroupBox();
            lblTotalIngresos = new Label();
            cardEgresos = new GroupBox();
            lblTotalEgresos = new Label();
            panelPrincipal = new TableLayoutPanel();
            groupIngresos = new GroupBox();
            dgvIngresos = new DataGridView();
            groupEgresos = new GroupBox();
            dgvEgresos = new DataGridView();
            panelInferior = new Panel();
            btnCerrarCaja = new Button();
            flowDetalleMetodos = new FlowLayoutPanel();
            btnContadorBilletes = new Button();
            labelSaldoFinal = new Label();
            btnReabrirCaja = new Button();
            btnRetirarDinero = new Button();
            panelSuperior.SuspendLayout();
            panelResumen.SuspendLayout();
            cardMontoInicial.SuspendLayout();
            cardIngresos.SuspendLayout();
            cardEgresos.SuspendLayout();
            panelPrincipal.SuspendLayout();
            groupIngresos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvIngresos).BeginInit();
            groupEgresos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEgresos).BeginInit();
            panelInferior.SuspendLayout();
            SuspendLayout();
            // 
            // panelSuperior
            // 
            panelSuperior.BackColor = Color.FromArgb(45, 66, 91);
            panelSuperior.Controls.Add(comboCajas);
            panelSuperior.Controls.Add(labelFecha);
            panelSuperior.Controls.Add(labelCaja);
            panelSuperior.Dock = DockStyle.Top;
            panelSuperior.Location = new Point(0, 0);
            panelSuperior.Name = "panelSuperior";
            panelSuperior.Size = new Size(1527, 70);
            panelSuperior.TabIndex = 0;
            // 
            // comboCajas
            // 
            comboCajas.DropDownStyle = ComboBoxStyle.DropDownList;
            comboCajas.Font = new Font("Segoe UI", 10F);
            comboCajas.FormattingEnabled = true;
            comboCajas.Location = new Point(1081, 23);
            comboCajas.Name = "comboCajas";
            comboCajas.Size = new Size(266, 31);
            comboCajas.TabIndex = 3;
            comboCajas.SelectedIndexChanged += comboCajas_SelectedIndexChanged;
            // 
            // labelFecha
            // 
            labelFecha.Font = new Font("Segoe UI", 11F);
            labelFecha.ForeColor = Color.White;
            labelFecha.Location = new Point(830, 23);
            labelFecha.Name = "labelFecha";
            labelFecha.Size = new Size(245, 31);
            labelFecha.TabIndex = 0;
            labelFecha.Text = "Fecha: 21/10/2025";
            labelFecha.TextAlign = ContentAlignment.MiddleRight;
            // 
            // labelCaja
            // 
            labelCaja.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            labelCaja.ForeColor = Color.White;
            labelCaja.Location = new Point(12, 23);
            labelCaja.Name = "labelCaja";
            labelCaja.Size = new Size(187, 28);
            labelCaja.TabIndex = 1;
            labelCaja.Text = "Caja #1 - Activa";
            // 
            // panelResumen
            // 
            panelResumen.Controls.Add(cardMontoInicial);
            panelResumen.Controls.Add(cardIngresos);
            panelResumen.Controls.Add(cardEgresos);
            panelResumen.Dock = DockStyle.Top;
            panelResumen.Location = new Point(0, 70);
            panelResumen.Name = "panelResumen";
            panelResumen.Size = new Size(1527, 94);
            panelResumen.TabIndex = 1;
            // 
            // cardMontoInicial
            // 
            cardMontoInicial.BackColor = Color.LightGray;
            cardMontoInicial.Controls.Add(lblMontoInicial);
            cardMontoInicial.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            cardMontoInicial.ForeColor = Color.FromArgb(45, 66, 91);
            cardMontoInicial.Location = new Point(1, 14);
            cardMontoInicial.Name = "cardMontoInicial";
            cardMontoInicial.Size = new Size(531, 107);
            cardMontoInicial.TabIndex = 0;
            cardMontoInicial.TabStop = false;
            cardMontoInicial.Text = "Monto Inicial";
            // 
            // lblMontoInicial
            // 
            lblMontoInicial.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblMontoInicial.ForeColor = Color.FromArgb(0, 128, 0);
            lblMontoInicial.Location = new Point(18, 22);
            lblMontoInicial.Name = "lblMontoInicial";
            lblMontoInicial.Size = new Size(300, 44);
            lblMontoInicial.TabIndex = 0;
            lblMontoInicial.Text = "$10.000";
            // 
            // cardIngresos
            // 
            cardIngresos.Controls.Add(lblTotalIngresos);
            cardIngresos.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            cardIngresos.ForeColor = Color.FromArgb(45, 66, 91);
            cardIngresos.Location = new Point(538, 20);
            cardIngresos.Name = "cardIngresos";
            cardIngresos.Size = new Size(330, 68);
            cardIngresos.TabIndex = 1;
            cardIngresos.TabStop = false;
            cardIngresos.Text = "Total Ingresos";
            // 
            // lblTotalIngresos
            // 
            lblTotalIngresos.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTotalIngresos.ForeColor = Color.ForestGreen;
            lblTotalIngresos.Location = new Point(18, 22);
            lblTotalIngresos.Name = "lblTotalIngresos";
            lblTotalIngresos.Size = new Size(300, 44);
            lblTotalIngresos.TabIndex = 0;
            lblTotalIngresos.Text = "$0";
            // 
            // cardEgresos
            // 
            cardEgresos.Controls.Add(lblTotalEgresos);
            cardEgresos.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            cardEgresos.ForeColor = Color.FromArgb(45, 66, 91);
            cardEgresos.Location = new Point(874, 19);
            cardEgresos.Name = "cardEgresos";
            cardEgresos.Size = new Size(330, 68);
            cardEgresos.TabIndex = 2;
            cardEgresos.TabStop = false;
            cardEgresos.Text = "Total Egresos";
            // 
            // lblTotalEgresos
            // 
            lblTotalEgresos.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTotalEgresos.ForeColor = Color.IndianRed;
            lblTotalEgresos.Location = new Point(18, 22);
            lblTotalEgresos.Name = "lblTotalEgresos";
            lblTotalEgresos.Size = new Size(300, 44);
            lblTotalEgresos.TabIndex = 0;
            lblTotalEgresos.Text = "$0";
            // 
            // panelPrincipal
            // 
            panelPrincipal.ColumnCount = 2;
            panelPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            panelPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            panelPrincipal.Controls.Add(groupIngresos, 0, 0);
            panelPrincipal.Controls.Add(groupEgresos, 1, 0);
            panelPrincipal.Dock = DockStyle.Fill;
            panelPrincipal.Location = new Point(0, 164);
            panelPrincipal.Name = "panelPrincipal";
            panelPrincipal.RowCount = 1;
            panelPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panelPrincipal.Size = new Size(1527, 418);
            panelPrincipal.TabIndex = 2;
            // 
            // groupIngresos
            // 
            groupIngresos.Controls.Add(dgvIngresos);
            groupIngresos.Dock = DockStyle.Fill;
            groupIngresos.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            groupIngresos.ForeColor = Color.FromArgb(45, 66, 91);
            groupIngresos.Location = new Point(3, 3);
            groupIngresos.Name = "groupIngresos";
            groupIngresos.Size = new Size(757, 412);
            groupIngresos.TabIndex = 0;
            groupIngresos.TabStop = false;
            groupIngresos.Text = "Detalle de Ingresos";
            // 
            // dgvIngresos
            // 
            dgvIngresos.AllowUserToAddRows = false;
            dgvIngresos.AllowUserToDeleteRows = false;
            dgvIngresos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvIngresos.BackgroundColor = Color.White;
            dgvIngresos.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvIngresos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvIngresos.ColumnHeadersHeight = 29;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(45, 66, 91);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvIngresos.DefaultCellStyle = dataGridViewCellStyle2;
            dgvIngresos.Dock = DockStyle.Fill;
            dgvIngresos.Location = new Point(3, 30);
            dgvIngresos.Name = "dgvIngresos";
            dgvIngresos.RowHeadersWidth = 51;
            dgvIngresos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIngresos.Size = new Size(751, 379);
            dgvIngresos.TabIndex = 0;
            // 
            // groupEgresos
            // 
            groupEgresos.Controls.Add(dgvEgresos);
            groupEgresos.Dock = DockStyle.Fill;
            groupEgresos.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupEgresos.ForeColor = Color.FromArgb(45, 66, 91);
            groupEgresos.Location = new Point(766, 3);
            groupEgresos.Name = "groupEgresos";
            groupEgresos.Size = new Size(758, 412);
            groupEgresos.TabIndex = 1;
            groupEgresos.TabStop = false;
            groupEgresos.Text = "Detalle de Egresos";
            // 
            // dgvEgresos
            // 
            dgvEgresos.AllowUserToAddRows = false;
            dgvEgresos.AllowUserToDeleteRows = false;
            dgvEgresos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEgresos.BackgroundColor = Color.White;
            dgvEgresos.BorderStyle = BorderStyle.None;
            dgvEgresos.ColumnHeadersHeight = 29;
            dgvEgresos.Dock = DockStyle.Fill;
            dgvEgresos.Location = new Point(3, 27);
            dgvEgresos.Name = "dgvEgresos";
            dgvEgresos.RowHeadersWidth = 51;
            dgvEgresos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEgresos.Size = new Size(752, 382);
            dgvEgresos.TabIndex = 0;
            // 
            // panelInferior
            // 
            panelInferior.BackColor = Color.FromArgb(240, 240, 240);
            panelInferior.Controls.Add(btnCerrarCaja);
            panelInferior.Controls.Add(flowDetalleMetodos);
            panelInferior.Controls.Add(btnContadorBilletes);
            panelInferior.Controls.Add(labelSaldoFinal);
            panelInferior.Controls.Add(btnReabrirCaja);
            panelInferior.Controls.Add(btnRetirarDinero);
            panelInferior.Dock = DockStyle.Bottom;
            panelInferior.Location = new Point(0, 582);
            panelInferior.Name = "panelInferior";
            panelInferior.Size = new Size(1527, 168);
            panelInferior.TabIndex = 3;
            // 
            // btnCerrarCaja
            // 
            btnCerrarCaja.BackColor = Color.FromArgb(45, 66, 91);
            btnCerrarCaja.FlatAppearance.BorderSize = 0;
            btnCerrarCaja.FlatStyle = FlatStyle.Flat;
            btnCerrarCaja.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnCerrarCaja.ForeColor = Color.White;
            btnCerrarCaja.Location = new Point(1236, 6);
            btnCerrarCaja.Name = "btnCerrarCaja";
            btnCerrarCaja.Size = new Size(95, 150);
            btnCerrarCaja.TabIndex = 1;
            btnCerrarCaja.Text = "Cerrar Caja";
            btnCerrarCaja.UseVisualStyleBackColor = false;
            btnCerrarCaja.Click += btnCerrarCaja_Click;
            // 
            // flowDetalleMetodos
            // 
            flowDetalleMetodos.BackColor = Color.Transparent;
            flowDetalleMetodos.Location = new Point(0, 49);
            flowDetalleMetodos.Name = "flowDetalleMetodos";
            flowDetalleMetodos.Size = new Size(1096, 116);
            flowDetalleMetodos.TabIndex = 0;
            // 
            // btnContadorBilletes
            // 
            btnContadorBilletes.BackColor = Color.Green;
            btnContadorBilletes.FlatAppearance.BorderSize = 0;
            btnContadorBilletes.FlatStyle = FlatStyle.Flat;
            btnContadorBilletes.Font = new Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnContadorBilletes.ForeColor = Color.White;
            btnContadorBilletes.Location = new Point(1102, 6);
            btnContadorBilletes.Name = "btnContadorBilletes";
            btnContadorBilletes.Size = new Size(128, 45);
            btnContadorBilletes.TabIndex = 2;
            btnContadorBilletes.Text = "Contador de billetes";
            btnContadorBilletes.UseVisualStyleBackColor = false;
            btnContadorBilletes.Click += btnContadorBilletes_Click;
            // 
            // labelSaldoFinal
            // 
            labelSaldoFinal.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelSaldoFinal.ForeColor = Color.FromArgb(0, 128, 0);
            labelSaldoFinal.Location = new Point(6, 6);
            labelSaldoFinal.Name = "labelSaldoFinal";
            labelSaldoFinal.Size = new Size(922, 45);
            labelSaldoFinal.TabIndex = 0;
            labelSaldoFinal.Text = "Saldo Final: $10.000";
            // 
            // btnReabrirCaja
            // 
            btnReabrirCaja.BackColor = Color.FromArgb(255, 179, 0);
            btnReabrirCaja.FlatAppearance.BorderSize = 0;
            btnReabrirCaja.FlatStyle = FlatStyle.Flat;
            btnReabrirCaja.Font = new Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnReabrirCaja.ForeColor = Color.White;
            btnReabrirCaja.Location = new Point(1102, 57);
            btnReabrirCaja.Name = "btnReabrirCaja";
            btnReabrirCaja.Size = new Size(128, 45);
            btnReabrirCaja.TabIndex = 4;
            btnReabrirCaja.Text = "Reabrir Caja";
            btnReabrirCaja.UseVisualStyleBackColor = false;
            btnReabrirCaja.Click += btnReabrirCaja_Click;
            // 
            // btnRetirarDinero
            // 
            btnRetirarDinero.BackColor = Color.FromArgb(183, 28, 28);
            btnRetirarDinero.FlatAppearance.BorderSize = 0;
            btnRetirarDinero.FlatStyle = FlatStyle.Flat;
            btnRetirarDinero.Font = new Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRetirarDinero.ForeColor = Color.White;
            btnRetirarDinero.Location = new Point(1102, 111);
            btnRetirarDinero.Name = "btnRetirarDinero";
            btnRetirarDinero.Size = new Size(128, 45);
            btnRetirarDinero.TabIndex = 3;
            btnRetirarDinero.Text = "Retirar Dinero";
            btnRetirarDinero.UseVisualStyleBackColor = false;
            btnRetirarDinero.Click += btnRetirarDinero_Click;
            // 
            // FrmArqueo
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1527, 750);
            Controls.Add(panelPrincipal);
            Controls.Add(panelResumen);
            Controls.Add(panelSuperior);
            Controls.Add(panelInferior);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmArqueo";
            Text = "FrmArqueo";
            panelSuperior.ResumeLayout(false);
            panelResumen.ResumeLayout(false);
            cardMontoInicial.ResumeLayout(false);
            cardIngresos.ResumeLayout(false);
            cardEgresos.ResumeLayout(false);
            panelPrincipal.ResumeLayout(false);
            groupIngresos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvIngresos).EndInit();
            groupEgresos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvEgresos).EndInit();
            panelInferior.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSuperior;
        private System.Windows.Forms.Label labelTitulo;
        private System.Windows.Forms.Label labelFecha;
        private System.Windows.Forms.Label labelCaja;
        private System.Windows.Forms.ComboBox comboCajas;
        private System.Windows.Forms.Panel panelResumen;
        private System.Windows.Forms.GroupBox cardMontoInicial;
        private System.Windows.Forms.Label lblMontoInicial;
        private System.Windows.Forms.GroupBox cardIngresos;
        private System.Windows.Forms.Label lblTotalIngresos;
        private System.Windows.Forms.GroupBox cardEgresos;
        private System.Windows.Forms.Label lblTotalEgresos;
        private System.Windows.Forms.TableLayoutPanel panelPrincipal;
        private System.Windows.Forms.GroupBox groupIngresos;
        private System.Windows.Forms.DataGridView dgvIngresos;
        private System.Windows.Forms.GroupBox groupEgresos;
        private System.Windows.Forms.DataGridView dgvEgresos;
        private System.Windows.Forms.Panel panelInferior;
        private System.Windows.Forms.FlowLayoutPanel flowDetalleMetodos;
        private System.Windows.Forms.Label labelSaldoFinal;
        private System.Windows.Forms.Button btnContadorBilletes;
        private System.Windows.Forms.Button btnRetirarDinero; // Added new button declaration
        private System.Windows.Forms.Button btnReabrirCaja; // 🔹 Added button for reopening cash register
        private System.Windows.Forms.Button btnAbrirCajaForzar; // 🔹 Added button for forced opening
        private System.Windows.Forms.Button btnCerrarCaja; // 🔹 Added button for closing cash register
    }
}