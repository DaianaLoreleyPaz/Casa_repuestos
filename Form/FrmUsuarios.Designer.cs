namespace Casa_repuestos
{
    partial class FrmUsuarios
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtNombre = new TextBox();
            btnGuardar = new Button();
            txtEmail = new TextBox();
            txtContrasena = new TextBox();
            cmbRol = new ComboBox();
            dgvUsuarios = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            clbPermisos = new CheckedListBox();
            btnActualizarPermisos = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            SuspendLayout();
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(396, 81);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(168, 27);
            txtNombre.TabIndex = 0;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(357, 448);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(94, 29);
            btnGuardar.TabIndex = 1;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(342, 50);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(279, 27);
            txtEmail.TabIndex = 2;
            // 
            // txtContrasena
            // 
            txtContrasena.Location = new Point(416, 116);
            txtContrasena.Name = "txtContrasena";
            txtContrasena.Size = new Size(127, 27);
            txtContrasena.TabIndex = 3;
            // 
            // cmbRol
            // 
            cmbRol.FormattingEnabled = true;
            cmbRol.Location = new Point(342, 190);
            cmbRol.Name = "cmbRol";
            cmbRol.Size = new Size(279, 28);
            cmbRol.TabIndex = 4;
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.BackgroundColor = SystemColors.ButtonHighlight;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Location = new Point(12, 483);
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.RowHeadersWidth = 51;
            dgvUsuarios.Size = new Size(996, 188);
            dgvUsuarios.TabIndex = 5;
            dgvUsuarios.CellClick += dgvUsuarios_CellClick;
           
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(215, 52);
            label1.Name = "label1";
            label1.Size = new Size(123, 20);
            label1.TabIndex = 6;
            label1.Text = "Ingrese su email: ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(217, 84);
            label2.Name = "label2";
            label2.Size = new Size(66, 20);
            label2.TabIndex = 7;
            label2.Text = "Usuario: ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(217, 119);
            label3.Name = "label3";
            label3.Size = new Size(90, 20);
            label3.TabIndex = 8;
            label3.Text = "Contraseña: ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(168, 249);
            label4.Name = "label4";
            label4.Size = new Size(168, 20);
            label4.TabIndex = 9;
            label4.Text = "Seleccione los Permisos:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(201, 193);
            label5.Name = "label5";
            label5.Size = new Size(126, 20);
            label5.TabIndex = 10;
            label5.Text = "Seleccione un Rol";
            // 
            // clbPermisos
            // 
            clbPermisos.CheckOnClick = true;
            clbPermisos.FormattingEnabled = true;
            clbPermisos.Items.AddRange(new object[] { "Ventas", "Compras", "Productos", "Inventario", "Reparaciones", "Arqueo", "Configuración" });
            clbPermisos.Location = new Point(342, 249);
            clbPermisos.Name = "clbPermisos";
            clbPermisos.Size = new Size(271, 180);
            clbPermisos.TabIndex = 11;
            // 
            // btnActualizarPermisos
            // 
            btnActualizarPermisos.Location = new Point(488, 448);
            btnActualizarPermisos.Name = "btnActualizarPermisos";
            btnActualizarPermisos.Size = new Size(94, 29);
            btnActualizarPermisos.TabIndex = 12;
            btnActualizarPermisos.Text = "Actualizar";
            btnActualizarPermisos.UseVisualStyleBackColor = true;
            btnActualizarPermisos.Click += btnActualizarPermisos_Click;
            // 
            // FrmUsuarios
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1036, 683);
            Controls.Add(btnActualizarPermisos);
            Controls.Add(clbPermisos);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dgvUsuarios);
            Controls.Add(cmbRol);
            Controls.Add(txtContrasena);
            Controls.Add(txtEmail);
            Controls.Add(btnGuardar);
            Controls.Add(txtNombre);
            Name = "FrmUsuarios";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtNombre;
        private Button btnGuardar;
        private TextBox txtEmail;
        private TextBox txtContrasena;
        private ComboBox cmbRol;
        private DataGridView dgvUsuarios;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private CheckedListBox clbPermisos;
        private Button btnActualizarPermisos;
    }
}
