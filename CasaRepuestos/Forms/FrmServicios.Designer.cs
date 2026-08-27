namespace CasaRepuestos.Forms
{
    partial class FrmServicios
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            panelContainer = new Panel();
            splitContainer = new SplitContainer();
            panelLista = new Panel();
            dgvServicios = new DataGridView();
            panelBusqueda = new Panel();
            txtBuscar = new TextBox();
            lblBuscar = new Label();
            panelForm = new Panel();
            btnLimpiarArticulo = new Button();
            cmbArticuloAsociado = new ComboBox();
            lblArticuloAsociado = new Label();
            btnCancelar = new Button();
            btnGuardar = new Button();
            txtPrecio = new TextBox();
            lblPrecio = new Label();
            txtDescripcion = new TextBox();
            lblDescripcion = new Label();
            lblTituloForm = new Label();
            panelHeader = new Panel();
            lblTitulo = new Label();
            panelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            panelLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvServicios).BeginInit();
            panelBusqueda.SuspendLayout();
            panelForm.SuspendLayout();
            panelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // panelContainer
            // 
            panelContainer.BackColor = Color.FromArgb(240, 245, 249);
            panelContainer.Controls.Add(splitContainer);
            panelContainer.Controls.Add(panelHeader);
            panelContainer.Dock = DockStyle.Fill;
            panelContainer.Location = new Point(0, 0);
            panelContainer.Name = "panelContainer";
            panelContainer.Size = new Size(1365, 729);
            panelContainer.TabIndex = 0;
            // 
            // splitContainer
            // 
            splitContainer.BackColor = Color.LightSteelBlue;
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 80);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(panelLista);
            splitContainer.Panel1.Controls.Add(panelBusqueda);
            splitContainer.Panel1MinSize = 800;
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(panelForm);
            splitContainer.Size = new Size(1365, 649);
            splitContainer.SplitterDistance = 913;
            splitContainer.SplitterWidth = 5;
            splitContainer.TabIndex = 2;
            // 
            // panelLista
            // 
            panelLista.Controls.Add(dgvServicios);
            panelLista.Dock = DockStyle.Fill;
            panelLista.Location = new Point(0, 65);
            panelLista.Name = "panelLista";
            panelLista.Padding = new Padding(10, 11, 10, 11);
            panelLista.Size = new Size(913, 584);
            panelLista.TabIndex = 1;
            // 
            // dgvServicios
            // 
            dgvServicios.AllowUserToAddRows = false;
            dgvServicios.AllowUserToDeleteRows = false;
            dgvServicios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvServicios.BackgroundColor = Color.White;
            dgvServicios.BorderStyle = BorderStyle.None;
            dgvServicios.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(45, 66, 91);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.Padding = new Padding(5);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(45, 66, 91);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvServicios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvServicios.ColumnHeadersHeight = 40;
            dgvServicios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(45, 66, 91);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(200, 220, 240);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(45, 66, 91);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvServicios.DefaultCellStyle = dataGridViewCellStyle2;
            dgvServicios.Dock = DockStyle.Fill;
            dgvServicios.EnableHeadersVisualStyles = false;
            dgvServicios.GridColor = Color.FromArgb(220, 220, 220);
            dgvServicios.Location = new Point(10, 11);
            dgvServicios.Name = "dgvServicios";
            dgvServicios.ReadOnly = true;
            dgvServicios.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(240, 245, 249);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(45, 66, 91);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(200, 220, 240);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(45, 66, 91);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvServicios.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvServicios.RowHeadersWidth = 40;
            dgvServicios.RowTemplate.Height = 35;
            dgvServicios.Size = new Size(893, 562);
            dgvServicios.TabIndex = 0;
            // 
            // panelBusqueda
            // 
            panelBusqueda.BackColor = Color.White;
            panelBusqueda.Controls.Add(txtBuscar);
            panelBusqueda.Controls.Add(lblBuscar);
            panelBusqueda.Dock = DockStyle.Top;
            panelBusqueda.Location = new Point(0, 0);
            panelBusqueda.Name = "panelBusqueda";
            panelBusqueda.Padding = new Padding(10, 11, 10, 11);
            panelBusqueda.Size = new Size(913, 65);
            panelBusqueda.TabIndex = 0;
            // 
            // txtBuscar
            // 
            txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBuscar.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtBuscar.Location = new Point(101, 15);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(793, 31);
            txtBuscar.TabIndex = 1;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBuscar.ForeColor = Color.FromArgb(45, 66, 91);
            lblBuscar.Location = new Point(15, 19);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(75, 25);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar:";
            // 
            // panelForm
            // 
            panelForm.BackColor = Color.White;
            panelForm.BorderStyle = BorderStyle.FixedSingle;
            panelForm.Controls.Add(btnLimpiarArticulo);
            panelForm.Controls.Add(cmbArticuloAsociado);
            panelForm.Controls.Add(lblArticuloAsociado);
            panelForm.Controls.Add(btnCancelar);
            panelForm.Controls.Add(btnGuardar);
            panelForm.Controls.Add(txtPrecio);
            panelForm.Controls.Add(lblPrecio);
            panelForm.Controls.Add(txtDescripcion);
            panelForm.Controls.Add(lblDescripcion);
            panelForm.Controls.Add(lblTituloForm);
            panelForm.Dock = DockStyle.Fill;
            panelForm.Location = new Point(0, 0);
            panelForm.Name = "panelForm";
            panelForm.Padding = new Padding(21, 20, 21, 20);
            panelForm.Size = new Size(447, 649);
            panelForm.TabIndex = 1;
            // 
            // btnLimpiarArticulo
            // 
            btnLimpiarArticulo.BackColor = Color.LightCoral;
            btnLimpiarArticulo.FlatAppearance.BorderSize = 0;
            btnLimpiarArticulo.FlatStyle = FlatStyle.Flat;
            btnLimpiarArticulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLimpiarArticulo.ForeColor = Color.White;
            btnLimpiarArticulo.Location = new Point(396, 174);
            btnLimpiarArticulo.Name = "btnLimpiarArticulo";
            btnLimpiarArticulo.Size = new Size(39, 29);
            btnLimpiarArticulo.TabIndex = 10;
            btnLimpiarArticulo.Text = "X";
            btnLimpiarArticulo.UseVisualStyleBackColor = false;
            btnLimpiarArticulo.Click += btnLimpiarArticulo_Click;
            // 
            // cmbArticuloAsociado
            // 
            cmbArticuloAsociado.FormattingEnabled = true;
            cmbArticuloAsociado.Location = new Point(191, 175);
            cmbArticuloAsociado.Name = "cmbArticuloAsociado";
            cmbArticuloAsociado.Size = new Size(199, 28);
            cmbArticuloAsociado.TabIndex = 20;
            // 
            // lblArticuloAsociado
            // 
            lblArticuloAsociado.AutoSize = true;
            lblArticuloAsociado.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblArticuloAsociado.Location = new Point(2, 178);
            lblArticuloAsociado.Name = "lblArticuloAsociado";
            lblArticuloAsociado.Size = new Size(164, 25);
            lblArticuloAsociado.TabIndex = 19;
            lblArticuloAsociado.Text = "Articulo Asociado";
            // 
            // btnCancelar
            // 
            btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCancelar.BackColor = Color.FromArgb(108, 117, 125);
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(270, 561);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 45);
            btnCancelar.TabIndex = 18;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnGuardar.BackColor = Color.FromArgb(76, 132, 200);
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(120, 561);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(120, 45);
            btnGuardar.TabIndex = 17;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            // 
            // txtPrecio
            // 
            txtPrecio.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtPrecio.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPrecio.Location = new Point(150, 335);
            txtPrecio.Name = "txtPrecio";
            txtPrecio.Size = new Size(257, 31);
            txtPrecio.TabIndex = 16;
            // 
            // lblPrecio
            // 
            lblPrecio.AutoSize = true;
            lblPrecio.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPrecio.ForeColor = Color.FromArgb(45, 66, 91);
            lblPrecio.Location = new Point(79, 339);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.Size = new Size(70, 25);
            lblPrecio.TabIndex = 15;
            lblPrecio.Text = "Precio:";
            // 
            // txtDescripcion
            // 
            txtDescripcion.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDescripcion.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtDescripcion.Location = new Point(152, 246);
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(257, 31);
            txtDescripcion.TabIndex = 14;
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDescripcion.ForeColor = Color.FromArgb(45, 66, 91);
            lblDescripcion.Location = new Point(37, 250);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(116, 25);
            lblDescripcion.TabIndex = 13;
            lblDescripcion.Text = "Descripción:";
            // 
            // lblTituloForm
            // 
            lblTituloForm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblTituloForm.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloForm.ForeColor = Color.FromArgb(45, 66, 91);
            lblTituloForm.Location = new Point(21, 51);
            lblTituloForm.Name = "lblTituloForm";
            lblTituloForm.Size = new Size(408, 80);
            lblTituloForm.TabIndex = 12;
            lblTituloForm.Text = "REGISTRO SERVICIO";
            lblTituloForm.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(45, 66, 91);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1365, 80);
            panelHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Arial Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(564, 21);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(384, 40);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "MÓDULO DE SERVICIOS";
            // 
            // FrmServicios
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1365, 729);
            Controls.Add(panelContainer);
            MinimumSize = new Size(1365, 766);
            Name = "FrmServicios";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestión de Servicios";
            Load += FrmServicios_Load;
            panelContainer.ResumeLayout(false);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            panelLista.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvServicios).EndInit();
            panelBusqueda.ResumeLayout(false);
            panelBusqueda.PerformLayout();
            panelForm.ResumeLayout(false);
            panelForm.PerformLayout();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelContainer;
        private SplitContainer splitContainer;
        private Panel panelLista;
        private DataGridView dgvServicios;
        private Panel panelBusqueda;
        private TextBox txtBuscar;
        private Label lblBuscar;
        private Panel panelForm;
        private Button btnCancelar;
        private Button btnGuardar;
        private TextBox txtPrecio;
        private Label lblPrecio;
        private TextBox txtDescripcion;
        private Label lblDescripcion;
        private Label lblTituloForm;
        private Button btnLimpiarArticulo;
        private ComboBox cmbArticuloAsociado;
        private Label lblArticuloAsociado;
        private Panel panelHeader;
        private Label lblTitulo;
    }
}