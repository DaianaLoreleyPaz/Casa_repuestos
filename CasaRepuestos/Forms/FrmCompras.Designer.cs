// FrmCompras.Designer.cs

namespace CasaRepuestos.Forms
{
    partial class FrmCompras
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
            lblTitulo = new Label();
            tabControl = new TabControl();
            tabOrdenStock = new TabPage();
            label5 = new Label();
            btnBuscarFaltantes = new Button();
            dgvNecesidades = new DataGridView();
            txtTotalOrden = new TextBox();
            lblTotal = new Label();
            groupBox1 = new GroupBox();
            numCantidadStock = new NumericUpDown();
            btnAgregarItemStock = new Button();
            label3 = new Label();
            label2 = new Label();
            cmbArticuloStock = new ComboBox();
            btnCrearOrdenStock = new Button();
            dgvOrdenStock = new DataGridView();
            IdArticuloCol = new DataGridViewTextBoxColumn();
            NombreArticuloCol = new DataGridViewTextBoxColumn();
            CantidadCol = new DataGridViewTextBoxColumn();
            PrecioUnitarioCol = new DataGridViewTextBoxColumn();
            ProveedorCol = new DataGridViewTextBoxColumn();
            SubtotalCol = new DataGridViewTextBoxColumn();
            EliminarCol = new DataGridViewButtonColumn();
            tabOrdenesPendientes = new TabPage();
            label6 = new Label();
            dgvOrdenesPendientes = new DataGridView();
            tabHistorial = new TabPage();
            dgvHistorial = new DataGridView();
            groupBox2 = new GroupBox();
            cmbFiltroTipoHistorial = new ComboBox();
            label7 = new Label();
            btnBuscarHistorial = new Button();
            dtpHastaHistorial = new DateTimePicker();
            label9 = new Label();
            dtpDesdeHistorial = new DateTimePicker();
            label8 = new Label();
            panelHeader = new Panel();
            label1 = new Label();
            tabControl.SuspendLayout();
            tabOrdenStock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNecesidades).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numCantidadStock).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvOrdenStock).BeginInit();
            tabOrdenesPendientes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrdenesPendientes).BeginInit();
            tabHistorial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistorial).BeginInit();
            groupBox2.SuspendLayout();
            panelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(14, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(278, 37);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Módulo de Compras";
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabOrdenStock);
            tabControl.Controls.Add(tabOrdenesPendientes);
            tabControl.Controls.Add(tabHistorial);
            tabControl.Location = new Point(14, 87);
            tabControl.Margin = new Padding(3, 4, 3, 4);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1601, 812);
            tabControl.TabIndex = 1;
            tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
            // 
            // tabOrdenStock
            // 
            tabOrdenStock.BackColor = Color.White;
            tabOrdenStock.Controls.Add(label5);
            tabOrdenStock.Controls.Add(btnBuscarFaltantes);
            tabOrdenStock.Controls.Add(dgvNecesidades);
            tabOrdenStock.Controls.Add(txtTotalOrden);
            tabOrdenStock.Controls.Add(lblTotal);
            tabOrdenStock.Controls.Add(groupBox1);
            tabOrdenStock.Controls.Add(btnCrearOrdenStock);
            tabOrdenStock.Controls.Add(dgvOrdenStock);
            tabOrdenStock.Location = new Point(4, 29);
            tabOrdenStock.Margin = new Padding(3, 4, 3, 4);
            tabOrdenStock.Name = "tabOrdenStock";
            tabOrdenStock.Padding = new Padding(3, 4, 3, 4);
            tabOrdenStock.Size = new Size(1593, 779);
            tabOrdenStock.TabIndex = 0;
            tabOrdenStock.Text = "1. Crear Orden (Stock/Repuestos)";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label5.Location = new Point(900, 99);
            label5.Name = "label5";
            label5.Size = new Size(492, 23);
            label5.TabIndex = 9;
            label5.Text = "Lista consolidada de repuestos faltantes (Mensaje Dinámico)";
            // 
            // btnBuscarFaltantes
            // 
            btnBuscarFaltantes.Location = new Point(947, 31);
            btnBuscarFaltantes.Margin = new Padding(3, 4, 3, 4);
            btnBuscarFaltantes.Name = "btnBuscarFaltantes";
            btnBuscarFaltantes.Size = new Size(251, 39);
            btnBuscarFaltantes.TabIndex = 8;
            btnBuscarFaltantes.Text = "Actualizar Lista de Faltantes";
            btnBuscarFaltantes.UseVisualStyleBackColor = true;
            btnBuscarFaltantes.Click += btnBuscarFaltantes_Click;
            // 
            // dgvNecesidades
            // 
            dgvNecesidades.AllowUserToAddRows = false;
            dgvNecesidades.AllowUserToDeleteRows = false;
            dgvNecesidades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNecesidades.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvNecesidades.Location = new Point(900, 152);
            dgvNecesidades.Margin = new Padding(3, 4, 3, 4);
            dgvNecesidades.Name = "dgvNecesidades";
            dgvNecesidades.ReadOnly = true;
            dgvNecesidades.RowHeadersWidth = 51;
            dgvNecesidades.RowTemplate.Height = 25;
            dgvNecesidades.Size = new Size(673, 599);
            dgvNecesidades.TabIndex = 7;
            dgvNecesidades.CellContentClick += dgvNecesidades_CellContentClick;
            // 
            // txtTotalOrden
            // 
            txtTotalOrden.Location = new Point(623, 707);
            txtTotalOrden.Margin = new Padding(3, 4, 3, 4);
            txtTotalOrden.Name = "txtTotalOrden";
            txtTotalOrden.ReadOnly = true;
            txtTotalOrden.Size = new Size(148, 27);
            txtTotalOrden.TabIndex = 6;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTotal.Location = new Point(544, 708);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(73, 25);
            lblTotal.TabIndex = 5;
            lblTotal.Text = "TOTAL:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(numCantidadStock);
            groupBox1.Controls.Add(btnAgregarItemStock);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(cmbArticuloStock);
            groupBox1.Location = new Point(33, 31);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(841, 91);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Añadir Artículo para Stock (o Repuesto NO Faltante)";
            // 
            // numCantidadStock
            // 
            numCantidadStock.Location = new Point(566, 39);
            numCantidadStock.Margin = new Padding(3, 4, 3, 4);
            numCantidadStock.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numCantidadStock.Name = "numCantidadStock";
            numCantidadStock.Size = new Size(93, 27);
            numCantidadStock.TabIndex = 5;
            numCantidadStock.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnAgregarItemStock
            // 
            btnAgregarItemStock.Location = new Point(702, 30);
            btnAgregarItemStock.Margin = new Padding(3, 4, 3, 4);
            btnAgregarItemStock.Name = "btnAgregarItemStock";
            btnAgregarItemStock.Size = new Size(122, 46);
            btnAgregarItemStock.TabIndex = 4;
            btnAgregarItemStock.Text = "Agregar Item";
            btnAgregarItemStock.UseVisualStyleBackColor = true;
            btnAgregarItemStock.Click += btnAgregarItemStock_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(487, 43);
            label3.Name = "label3";
            label3.Size = new Size(72, 20);
            label3.TabIndex = 2;
            label3.Text = "Cantidad:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(18, 43);
            label2.Name = "label2";
            label2.Size = new Size(64, 20);
            label2.TabIndex = 1;
            label2.Text = "Artículo:";
            // 
            // cmbArticuloStock
            // 
            cmbArticuloStock.FormattingEnabled = true;
            cmbArticuloStock.Location = new Point(83, 39);
            cmbArticuloStock.Margin = new Padding(3, 4, 3, 4);
            cmbArticuloStock.Name = "cmbArticuloStock";
            cmbArticuloStock.Size = new Size(378, 28);
            cmbArticuloStock.TabIndex = 0;
            // 
            // btnCrearOrdenStock
            // 
            btnCrearOrdenStock.BackColor = Color.FromArgb(0, 192, 0);
            btnCrearOrdenStock.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCrearOrdenStock.ForeColor = Color.White;
            btnCrearOrdenStock.Location = new Point(51, 708);
            btnCrearOrdenStock.Margin = new Padding(3, 4, 3, 4);
            btnCrearOrdenStock.Name = "btnCrearOrdenStock";
            btnCrearOrdenStock.Size = new Size(336, 43);
            btnCrearOrdenStock.TabIndex = 1;
            btnCrearOrdenStock.Text = "CREAR ORDEN DE COMPRA";
            btnCrearOrdenStock.UseVisualStyleBackColor = false;
            btnCrearOrdenStock.Click += btnCrearOrdenStock_Click;
            // 
            // dgvOrdenStock
            // 
            dgvOrdenStock.AllowUserToAddRows = false;
            dgvOrdenStock.AllowUserToDeleteRows = false;
            dgvOrdenStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrdenStock.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrdenStock.Columns.AddRange(new DataGridViewColumn[] { IdArticuloCol, NombreArticuloCol, CantidadCol, PrecioUnitarioCol, ProveedorCol, SubtotalCol, EliminarCol });
            dgvOrdenStock.Location = new Point(19, 152);
            dgvOrdenStock.Margin = new Padding(3, 4, 3, 4);
            dgvOrdenStock.Name = "dgvOrdenStock";
            dgvOrdenStock.RowHeadersWidth = 51;
            dgvOrdenStock.RowTemplate.Height = 25;
            dgvOrdenStock.Size = new Size(838, 522);
            dgvOrdenStock.TabIndex = 0;
            dgvOrdenStock.CellContentClick += dgvOrdenStock_CellContentClick;
            dgvOrdenStock.CellEndEdit += dgvOrdenStock_CellEndEdit;
            dgvOrdenStock.CellValueChanged += dgvOrdenStock_CellValueChanged;
            dgvOrdenStock.EditingControlShowing += dgvOrdenStock_EditingControlShowing;
            // 
            // IdArticuloCol
            // 
            IdArticuloCol.DataPropertyName = "IdArticulo";
            IdArticuloCol.HeaderText = "ID";
            IdArticuloCol.MinimumWidth = 6;
            IdArticuloCol.Name = "IdArticuloCol";
            IdArticuloCol.ReadOnly = true;
            // 
            // NombreArticuloCol
            // 
            NombreArticuloCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            NombreArticuloCol.DataPropertyName = "NombreArticulo";
            NombreArticuloCol.HeaderText = "Artículo";
            NombreArticuloCol.MinimumWidth = 6;
            NombreArticuloCol.Name = "NombreArticuloCol";
            NombreArticuloCol.ReadOnly = true;
            // 
            // CantidadCol
            // 
            CantidadCol.DataPropertyName = "Cantidad";
            CantidadCol.HeaderText = "Cantidad";
            CantidadCol.MinimumWidth = 6;
            CantidadCol.Name = "CantidadCol";
            CantidadCol.ReadOnly = true;
            // 
            // PrecioUnitarioCol
            // 
            PrecioUnitarioCol.DataPropertyName = "PrecioUnitarioEstimado";
            dataGridViewCellStyle1.Format = "C2";
            PrecioUnitarioCol.DefaultCellStyle = dataGridViewCellStyle1;
            PrecioUnitarioCol.HeaderText = "P. Unitario";
            PrecioUnitarioCol.MinimumWidth = 6;
            PrecioUnitarioCol.Name = "PrecioUnitarioCol";
            PrecioUnitarioCol.ReadOnly = true;
            // 
            // ProveedorCol
            // 
            ProveedorCol.DataPropertyName = "ProveedorNombre";
            ProveedorCol.HeaderText = "Nombre Proveedor";
            ProveedorCol.MinimumWidth = 6;
            ProveedorCol.Name = "ProveedorCol";
            ProveedorCol.ReadOnly = true;
            // 
            // SubtotalCol
            // 
            SubtotalCol.DataPropertyName = "Subtotal";
            dataGridViewCellStyle2.Format = "C2";
            SubtotalCol.DefaultCellStyle = dataGridViewCellStyle2;
            SubtotalCol.HeaderText = "Subtotal";
            SubtotalCol.MinimumWidth = 6;
            SubtotalCol.Name = "SubtotalCol";
            SubtotalCol.ReadOnly = true;
            // 
            // EliminarCol
            // 
            EliminarCol.HeaderText = "Eliminar";
            EliminarCol.MinimumWidth = 6;
            EliminarCol.Name = "EliminarCol";
            EliminarCol.ReadOnly = true;
            EliminarCol.Text = "X";
            EliminarCol.UseColumnTextForButtonValue = true;
            // 
            // tabOrdenesPendientes
            // 
            tabOrdenesPendientes.BackColor = Color.White;
            tabOrdenesPendientes.Controls.Add(label6);
            tabOrdenesPendientes.Controls.Add(dgvOrdenesPendientes);
            tabOrdenesPendientes.Location = new Point(4, 29);
            tabOrdenesPendientes.Margin = new Padding(3, 4, 3, 4);
            tabOrdenesPendientes.Name = "tabOrdenesPendientes";
            tabOrdenesPendientes.Padding = new Padding(3, 4, 3, 4);
            tabOrdenesPendientes.Size = new Size(1593, 779);
            tabOrdenesPendientes.TabIndex = 2;
            tabOrdenesPendientes.Text = "2. Órdenes Pendientes de Recepción";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label6.Location = new Point(19, 21);
            label6.Name = "label6";
            label6.Size = new Size(477, 28);
            label6.TabIndex = 1;
            label6.Text = "Órdenes de Compra Creadas (Estado PENDIENTE)";
            // 
            // dgvOrdenesPendientes
            // 
            dgvOrdenesPendientes.AllowUserToAddRows = false;
            dgvOrdenesPendientes.AllowUserToDeleteRows = false;
            dgvOrdenesPendientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrdenesPendientes.Location = new Point(19, 67);
            dgvOrdenesPendientes.Margin = new Padding(3, 4, 3, 4);
            dgvOrdenesPendientes.Name = "dgvOrdenesPendientes";
            dgvOrdenesPendientes.ReadOnly = true;
            dgvOrdenesPendientes.RowHeadersWidth = 51;
            dgvOrdenesPendientes.RowTemplate.Height = 25;
            dgvOrdenesPendientes.Size = new Size(1489, 603);
            dgvOrdenesPendientes.TabIndex = 0;
            dgvOrdenesPendientes.CellContentClick += dgvOrdenesPendientes_CellContentClick;
            // 
            // tabHistorial
            // 
            tabHistorial.BackColor = Color.White;
            tabHistorial.Controls.Add(dgvHistorial);
            tabHistorial.Controls.Add(groupBox2);
            tabHistorial.Location = new Point(4, 29);
            tabHistorial.Margin = new Padding(3, 4, 3, 4);
            tabHistorial.Name = "tabHistorial";
            tabHistorial.Padding = new Padding(3, 4, 3, 4);
            tabHistorial.Size = new Size(1593, 779);
            tabHistorial.TabIndex = 3;
            tabHistorial.Text = "3. Historial de Compras Recibidas";
            // 
            // dgvHistorial
            // 
            dgvHistorial.AllowUserToAddRows = false;
            dgvHistorial.AllowUserToDeleteRows = false;
            dgvHistorial.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistorial.Location = new Point(19, 168);
            dgvHistorial.Margin = new Padding(3, 4, 3, 4);
            dgvHistorial.Name = "dgvHistorial";
            dgvHistorial.ReadOnly = true;
            dgvHistorial.RowHeadersWidth = 51;
            dgvHistorial.RowTemplate.Height = 25;
            dgvHistorial.Size = new Size(1458, 508);
            dgvHistorial.TabIndex = 1;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(cmbFiltroTipoHistorial);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(btnBuscarHistorial);
            groupBox2.Controls.Add(dtpHastaHistorial);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(dtpDesdeHistorial);
            groupBox2.Controls.Add(label8);
            groupBox2.Location = new Point(19, 17);
            groupBox2.Margin = new Padding(3, 4, 3, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 4, 3, 4);
            groupBox2.Size = new Size(1458, 127);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Filtros de Historial";
            // 
            // cmbFiltroTipoHistorial
            // 
            cmbFiltroTipoHistorial.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltroTipoHistorial.FormattingEnabled = true;
            cmbFiltroTipoHistorial.Location = new Point(99, 57);
            cmbFiltroTipoHistorial.Margin = new Padding(3, 4, 3, 4);
            cmbFiltroTipoHistorial.Name = "cmbFiltroTipoHistorial";
            cmbFiltroTipoHistorial.Size = new Size(220, 28);
            cmbFiltroTipoHistorial.TabIndex = 6;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(37, 60);
            label7.Name = "label7";
            label7.Size = new Size(42, 20);
            label7.TabIndex = 5;
            label7.Text = "Tipo:";
            // 
            // btnBuscarHistorial
            // 
            btnBuscarHistorial.Location = new Point(1316, 51);
            btnBuscarHistorial.Margin = new Padding(3, 4, 3, 4);
            btnBuscarHistorial.Name = "btnBuscarHistorial";
            btnBuscarHistorial.Size = new Size(109, 31);
            btnBuscarHistorial.TabIndex = 4;
            btnBuscarHistorial.Text = "Buscar";
            btnBuscarHistorial.UseVisualStyleBackColor = true;
            btnBuscarHistorial.Click += btnBuscarHistorial_Click;
            // 
            // dtpHastaHistorial
            // 
            dtpHastaHistorial.Location = new Point(915, 55);
            dtpHastaHistorial.Margin = new Padding(3, 4, 3, 4);
            dtpHastaHistorial.Name = "dtpHastaHistorial";
            dtpHastaHistorial.Size = new Size(328, 27);
            dtpHastaHistorial.TabIndex = 3;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(836, 60);
            label9.Name = "label9";
            label9.Size = new Size(50, 20);
            label9.TabIndex = 2;
            label9.Text = "Hasta:";
            // 
            // dtpDesdeHistorial
            // 
            dtpDesdeHistorial.Location = new Point(452, 55);
            dtpDesdeHistorial.Margin = new Padding(3, 4, 3, 4);
            dtpDesdeHistorial.Name = "dtpDesdeHistorial";
            dtpDesdeHistorial.Size = new Size(362, 27);
            dtpDesdeHistorial.TabIndex = 1;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(370, 62);
            label8.Name = "label8";
            label8.Size = new Size(54, 20);
            label8.TabIndex = 0;
            label8.Text = "Desde:";
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(45, 66, 91);
            panelHeader.Controls.Add(label1);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1627, 70);
            panelHeader.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(617, 12);
            label1.Name = "label1";
            label1.Size = new Size(368, 40);
            label1.TabIndex = 0;
            label1.Text = "MÓDULO DE COMPRAS";
            // 
            // FrmCompras
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1627, 912);
            Controls.Add(panelHeader);
            Controls.Add(tabControl);
            Controls.Add(lblTitulo);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FrmCompras";
            Text = "Gestión de Compras";
            tabControl.ResumeLayout(false);
            tabOrdenStock.ResumeLayout(false);
            tabOrdenStock.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNecesidades).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numCantidadStock).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvOrdenStock).EndInit();
            tabOrdenesPendientes.ResumeLayout(false);
            tabOrdenesPendientes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrdenesPendientes).EndInit();
            tabHistorial.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHistorial).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabOrdenStock;
        private System.Windows.Forms.DataGridView dgvOrdenStock;
        private System.Windows.Forms.Button btnCrearOrdenStock;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAgregarItemStock;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbArticuloStock;
        private System.Windows.Forms.NumericUpDown numCantidadStock;
        private System.Windows.Forms.TextBox txtTotalOrden;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.TabPage tabOrdenesPendientes;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dgvOrdenesPendientes;
        private System.Windows.Forms.TabPage tabHistorial;
        private System.Windows.Forms.DataGridView dgvHistorial;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnBuscarHistorial;
        private System.Windows.Forms.DateTimePicker dtpHastaHistorial;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtpDesdeHistorial;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbFiltroTipoHistorial;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdArticuloCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn NombreArticuloCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn CantidadCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProveedorCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrecioUnitarioCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubtotalCol;
        private System.Windows.Forms.DataGridViewButtonColumn EliminarCol;
        private Label label5;
        private Button btnBuscarFaltantes;
        private DataGridView dgvNecesidades;
        private Panel panelHeader;
        private Label label1;
    }
}