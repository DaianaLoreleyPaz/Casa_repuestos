namespace CasaRepuestos.Forms
{
    partial class FrmReparaciones
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.SplitContainer splitContainerPrincipal;
        private System.Windows.Forms.Panel panelContenedorTitulo;
        private System.Windows.Forms.TabControl tabControlPrincipal;
        private System.Windows.Forms.TabPage tabEsperando;
        private System.Windows.Forms.TabPage tabEnProceso;
        private System.Windows.Forms.DataGridView dgvEsperando;
        private System.Windows.Forms.DataGridView dgvEnProceso;

        private System.Windows.Forms.Panel panelDetalle;
        private System.Windows.Forms.DataGridView dgvDetalles;
        private System.Windows.Forms.Label lblInfoModelo;
        private System.Windows.Forms.Label lblInfoFalla;

    
        private System.Windows.Forms.Button btnMarcarReparado;
      

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            splitContainerPrincipal = new SplitContainer();
            panelContenedorTitulo = new Panel();
            btnProcesarReparacion = new Button();
            tabControlPrincipal = new TabControl();
            tabPendiente = new TabPage();
            dgvPendiente = new DataGridView();
            tabEsperando = new TabPage();
            dgvEsperando = new DataGridView();
            tabEnProceso = new TabPage();
            dgvEnProceso = new DataGridView();
            panelDetalle = new Panel();
            dgvDetalles = new DataGridView();
            groupVisor = new GroupBox();
            lblInfoModelo = new Label();
            lblInfoFalla = new Label();
            panelBotones = new Panel();
            btnMarcarReparado = new Button();
            lblTitulo = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainerPrincipal).BeginInit();
            splitContainerPrincipal.Panel1.SuspendLayout();
            splitContainerPrincipal.Panel2.SuspendLayout();
            splitContainerPrincipal.SuspendLayout();
            panelContenedorTitulo.SuspendLayout();
            tabControlPrincipal.SuspendLayout();
            tabPendiente.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPendiente).BeginInit();
            tabEsperando.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEsperando).BeginInit();
            tabEnProceso.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEnProceso).BeginInit();
            panelDetalle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetalles).BeginInit();
            groupVisor.SuspendLayout();
            panelBotones.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainerPrincipal
            // 
            splitContainerPrincipal.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainerPrincipal.Location = new Point(0, 50);
            splitContainerPrincipal.Name = "splitContainerPrincipal";
            // 
            // splitContainerPrincipal.Panel1
            // 
            splitContainerPrincipal.Panel1.Controls.Add(panelContenedorTitulo);
            // 
            // splitContainerPrincipal.Panel2
            // 
            splitContainerPrincipal.Panel2.Controls.Add(panelDetalle);
            splitContainerPrincipal.Size = new Size(1360, 807);
            splitContainerPrincipal.SplitterDistance = 727;
            splitContainerPrincipal.TabIndex = 0;
            // 
            // panelContenedorTitulo
            // 
            panelContenedorTitulo.Controls.Add(btnProcesarReparacion);
            panelContenedorTitulo.Controls.Add(tabControlPrincipal);
            panelContenedorTitulo.Dock = DockStyle.Fill;
            panelContenedorTitulo.Location = new Point(0, 0);
            panelContenedorTitulo.Name = "panelContenedorTitulo";
            panelContenedorTitulo.Padding = new Padding(10, 60, 10, 10);
            panelContenedorTitulo.Size = new Size(727, 807);
            panelContenedorTitulo.TabIndex = 0;
            // 
            // btnProcesarReparacion
            // 
            btnProcesarReparacion.Location = new Point(465, 39);
            btnProcesarReparacion.Name = "btnProcesarReparacion";
            btnProcesarReparacion.Size = new Size(179, 44);
            btnProcesarReparacion.TabIndex = 2;
            btnProcesarReparacion.Text = "Procesar Reparación";
            btnProcesarReparacion.UseVisualStyleBackColor = true;
            btnProcesarReparacion.Click += btnProcesarReparacion_Click;
            // 
            // tabControlPrincipal
            // 
            tabControlPrincipal.Controls.Add(tabPendiente);
            tabControlPrincipal.Controls.Add(tabEsperando);
            tabControlPrincipal.Controls.Add(tabEnProceso);
            tabControlPrincipal.Dock = DockStyle.Fill;
            tabControlPrincipal.Location = new Point(10, 60);
            tabControlPrincipal.Name = "tabControlPrincipal";
            tabControlPrincipal.SelectedIndex = 0;
            tabControlPrincipal.Size = new Size(707, 737);
            tabControlPrincipal.TabIndex = 0;
            tabControlPrincipal.SelectedIndexChanged += tabControlPrincipal_SelectedIndexChanged;
            // 
            // tabPendiente
            // 
            tabPendiente.Controls.Add(dgvPendiente);
            tabPendiente.Location = new Point(4, 29);
            tabPendiente.Name = "tabPendiente";
            tabPendiente.Size = new Size(699, 704);
            tabPendiente.TabIndex = 2;
            tabPendiente.Text = "PENDIENTE";
            tabPendiente.UseVisualStyleBackColor = true;
            // 
            // dgvPendiente
            // 
            dgvPendiente.AllowUserToAddRows = false;
            dgvPendiente.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPendiente.Dock = DockStyle.Fill;
            dgvPendiente.Location = new Point(0, 0);
            dgvPendiente.MultiSelect = false;
            dgvPendiente.Name = "dgvPendiente";
            dgvPendiente.RowHeadersWidth = 51;
            dgvPendiente.Size = new Size(699, 704);
            dgvPendiente.TabIndex = 0;
            dgvPendiente.CellDoubleClick += dgvPendiente_CellDoubleClick;
            dgvPendiente.SelectionChanged += Grid_SelectionChanged;
            // 
            // tabEsperando
            // 
            tabEsperando.Controls.Add(dgvEsperando);
            tabEsperando.Location = new Point(4, 29);
            tabEsperando.Name = "tabEsperando";
            tabEsperando.Size = new Size(699, 704);
            tabEsperando.TabIndex = 0;
            tabEsperando.Text = "ESPERANDO REPUESTO";
            // 
            // dgvEsperando
            // 
            dgvEsperando.AllowUserToAddRows = false;
            dgvEsperando.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEsperando.ColumnHeadersHeight = 29;
            dgvEsperando.Dock = DockStyle.Fill;
            dgvEsperando.Location = new Point(0, 0);
            dgvEsperando.MultiSelect = false;
            dgvEsperando.Name = "dgvEsperando";
            dgvEsperando.RowHeadersWidth = 51;
            dgvEsperando.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEsperando.Size = new Size(699, 704);
            dgvEsperando.TabIndex = 0;
            dgvEsperando.CellDoubleClick += dgvEsperando_CellDoubleClick;
            dgvEsperando.SelectionChanged += Grid_SelectionChanged;
            // 
            // tabEnProceso
            // 
            tabEnProceso.Controls.Add(dgvEnProceso);
            tabEnProceso.Location = new Point(4, 29);
            tabEnProceso.Name = "tabEnProceso";
            tabEnProceso.Size = new Size(699, 704);
            tabEnProceso.TabIndex = 1;
            tabEnProceso.Text = "EN PROCESO";
            // 
            // dgvEnProceso
            // 
            dgvEnProceso.AllowUserToAddRows = false;
            dgvEnProceso.ColumnHeadersHeight = 29;
            dgvEnProceso.Dock = DockStyle.Fill;
            dgvEnProceso.Location = new Point(0, 0);
            dgvEnProceso.MultiSelect = false;
            dgvEnProceso.Name = "dgvEnProceso";
            dgvEnProceso.RowHeadersWidth = 51;
            dgvEnProceso.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEnProceso.Size = new Size(699, 704);
            dgvEnProceso.TabIndex = 0;
            dgvEnProceso.CellDoubleClick += dgvEnProceso_CellDoubleClick;
            dgvEnProceso.SelectionChanged += Grid_SelectionChanged;
            // 
            // panelDetalle
            // 
            panelDetalle.BackColor = Color.WhiteSmoke;
            panelDetalle.Controls.Add(dgvDetalles);
            panelDetalle.Controls.Add(groupVisor);
            panelDetalle.Controls.Add(panelBotones);
            panelDetalle.Location = new Point(3, 39);
            panelDetalle.Name = "panelDetalle";
            panelDetalle.Padding = new Padding(8);
            panelDetalle.Size = new Size(629, 706);
            panelDetalle.TabIndex = 0;
            // 
            // dgvDetalles
            // 
            dgvDetalles.AllowUserToAddRows = false;
            dgvDetalles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalles.ColumnHeadersHeight = 29;
            dgvDetalles.Dock = DockStyle.Fill;
            dgvDetalles.Location = new Point(8, 118);
            dgvDetalles.Name = "dgvDetalles";
            dgvDetalles.RowHeadersWidth = 51;
            dgvDetalles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalles.Size = new Size(613, 448);
            dgvDetalles.TabIndex = 0;
            // 
            // groupVisor
            // 
            groupVisor.BackColor = Color.White;
            groupVisor.Controls.Add(lblInfoModelo);
            groupVisor.Controls.Add(lblInfoFalla);
            groupVisor.Dock = DockStyle.Top;
            groupVisor.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            groupVisor.Location = new Point(8, 8);
            groupVisor.Name = "groupVisor";
            groupVisor.Size = new Size(613, 110);
            groupVisor.TabIndex = 2;
            groupVisor.TabStop = false;
            groupVisor.Text = "Visor técnico";
            // 
            // lblInfoModelo
            // 
            lblInfoModelo.Font = new Font("Segoe UI", 10F);
            lblInfoModelo.Location = new Point(12, 24);
            lblInfoModelo.Name = "lblInfoModelo";
            lblInfoModelo.Size = new Size(495, 26);
            lblInfoModelo.TabIndex = 0;
            lblInfoModelo.Text = "Marca: -  |  Modelo: -";
            // 
            // lblInfoFalla
            // 
            lblInfoFalla.Font = new Font("Segoe UI", 9.5F, FontStyle.Italic);
            lblInfoFalla.ForeColor = Color.DarkRed;
            lblInfoFalla.Location = new Point(12, 50);
            lblInfoFalla.Name = "lblInfoFalla";
            lblInfoFalla.Size = new Size(500, 57);
            lblInfoFalla.TabIndex = 1;
            lblInfoFalla.Text = "Falla: -";
            // 
            // panelBotones
            // 
            panelBotones.BackColor = Color.Transparent;
            panelBotones.Controls.Add(btnMarcarReparado);
            panelBotones.Dock = DockStyle.Bottom;
            panelBotones.Location = new Point(8, 566);
            panelBotones.Name = "panelBotones";
            panelBotones.Padding = new Padding(8);
            panelBotones.Size = new Size(613, 132);
            panelBotones.TabIndex = 1;
            // 
            // btnMarcarReparado
            // 
            btnMarcarReparado.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnMarcarReparado.BackColor = Color.DarkGreen;
            btnMarcarReparado.ForeColor = SystemColors.ButtonHighlight;
            btnMarcarReparado.Location = new Point(170, 38);
            btnMarcarReparado.Name = "btnMarcarReparado";
            btnMarcarReparado.Size = new Size(288, 42);
            btnMarcarReparado.TabIndex = 6;
            btnMarcarReparado.Text = "FINALIZAR REPARACION";
            btnMarcarReparado.UseVisualStyleBackColor = false;
            btnMarcarReparado.Click += btnMarcarReparado_Click;
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.FromArgb(45, 66, 91);
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Arial Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1360, 83);
            lblTitulo.TabIndex = 14;
            lblTitulo.Text = "MODULO DE REPARACIONES";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmReparaciones
            // 
            ClientSize = new Size(1360, 807);
            Controls.Add(lblTitulo);
            Controls.Add(splitContainerPrincipal);
            Name = "FrmReparaciones";
            Text = "Módulo Reparaciones";
            Load += FrmReparaciones_Load;
            splitContainerPrincipal.Panel1.ResumeLayout(false);
            splitContainerPrincipal.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerPrincipal).EndInit();
            splitContainerPrincipal.ResumeLayout(false);
            panelContenedorTitulo.ResumeLayout(false);
            tabControlPrincipal.ResumeLayout(false);
            tabPendiente.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPendiente).EndInit();
            tabEsperando.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvEsperando).EndInit();
            tabEnProceso.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvEnProceso).EndInit();
            panelDetalle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDetalles).EndInit();
            groupVisor.ResumeLayout(false);
            panelBotones.ResumeLayout(false);
            ResumeLayout(false);
        }
        private Panel panelBotones;
        private GroupBox groupVisor;
        private Label lblTitulo;
        private Button btnProcesarReparacion;
        private TabPage tabPendiente;
        private DataGridView dgvPendiente;
    }
}
