using iTextSharp.text;
using Org.BouncyCastle.Asn1.Crmf;
using System.Drawing.Printing;
using static System.Net.Mime.MediaTypeNames;

namespace CasaRepuestos.Forms
{
    partial class FrmCuentaCorriente
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


        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabPageCtaCte = new TabPage();
            dataGridViewFacturasPendientes = new DataGridView();
            groupBox4 = new GroupBox();
            label16 = new Label();
            textBoxSaldoaPagar = new TextBox();
            btnGuardarMovCtaCte = new Button();
            comboBoxMetPago = new ComboBox();
            label13 = new Label();
            label18 = new Label();
            textBoxDescripcionMetPago = new TextBox();
            groupBoxDatosCuenta = new GroupBox();
            textBoxSaldoActual = new TextBox();
            label15 = new Label();
            textBoxNombreCompleto = new TextBox();
            label14 = new Label();
            groupBox3 = new GroupBox();
            textBoxClienteConCtaCte = new TextBox();
            dataGridViewCuentasClientes = new DataGridView();
            panel1 = new Panel();
            label11 = new Label();
            tabControl1.SuspendLayout();
            tabPageCtaCte.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewFacturasPendientes).BeginInit();
            groupBox4.SuspendLayout();
            groupBoxDatosCuenta.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCuentasClientes).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPageCtaCte);
            tabControl1.Font = new System.Drawing.Font("Segoe UI", 10F);
            tabControl1.Location = new Point(23, 107);
            tabControl1.Margin = new Padding(3, 4, 3, 4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1515, 880);
            tabControl1.TabIndex = 18;
            // 
            // tabPageCtaCte
            // 
            tabPageCtaCte.Controls.Add(dataGridViewFacturasPendientes);
            tabPageCtaCte.Controls.Add(groupBox4);
            tabPageCtaCte.Controls.Add(groupBoxDatosCuenta);
            tabPageCtaCte.Controls.Add(groupBox3);
            tabPageCtaCte.Location = new Point(4, 32);
            tabPageCtaCte.Name = "tabPageCtaCte";
            tabPageCtaCte.Size = new Size(1507, 844);
            tabPageCtaCte.TabIndex = 2;
            tabPageCtaCte.Text = "Cuenta corriente";
            tabPageCtaCte.UseVisualStyleBackColor = true;
            // 
            // dataGridViewFacturasPendientes
            // 
            dataGridViewFacturasPendientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewFacturasPendientes.Location = new Point(598, 168);
            dataGridViewFacturasPendientes.Name = "dataGridViewFacturasPendientes";
            dataGridViewFacturasPendientes.RowHeadersWidth = 51;
            dataGridViewFacturasPendientes.Size = new Size(880, 523);
            dataGridViewFacturasPendientes.TabIndex = 48;
            dataGridViewFacturasPendientes.CellContentClick += dataGridViewFacturasPendientes_CellClick;
            // 
            // groupBox4
            // 
            groupBox4.BackColor = Color.FromArgb(224, 224, 224);
            groupBox4.Controls.Add(label16);
            groupBox4.Controls.Add(textBoxSaldoaPagar);
            groupBox4.Controls.Add(btnGuardarMovCtaCte);
            groupBox4.Controls.Add(comboBoxMetPago);
            groupBox4.Controls.Add(label13);
            groupBox4.Controls.Add(label18);
            groupBox4.Controls.Add(textBoxDescripcionMetPago);
            groupBox4.Font = new System.Drawing.Font("Segoe UI", 12F, FontStyle.Bold);
            groupBox4.ForeColor = Color.FromArgb(52, 73, 94);
            groupBox4.Location = new Point(598, 697);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(880, 115);
            groupBox4.TabIndex = 47;
            groupBox4.TabStop = false;
            groupBox4.Text = "Método de pago";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
            label16.ForeColor = Color.Black;
            label16.Location = new Point(445, 35);
            label16.Name = "label16";
            label16.Size = new Size(103, 20);
            label16.TabIndex = 42;
            label16.Text = "Saldo a pagar";
            // 
            // textBoxSaldoaPagar
            // 
            textBoxSaldoaPagar.Location = new Point(445, 59);
            textBoxSaldoaPagar.Name = "textBoxSaldoaPagar";
            textBoxSaldoaPagar.Size = new Size(286, 34);
            textBoxSaldoaPagar.TabIndex = 43;
            // 
            // btnGuardarMovCtaCte
            // 
            btnGuardarMovCtaCte.BackColor = Color.FromArgb(46, 204, 113);
            btnGuardarMovCtaCte.FlatAppearance.BorderSize = 0;
            btnGuardarMovCtaCte.FlatStyle = FlatStyle.Flat;
            btnGuardarMovCtaCte.Font = new System.Drawing.Font("Segoe UI", 11F, FontStyle.Bold);
            btnGuardarMovCtaCte.ForeColor = Color.White;
            btnGuardarMovCtaCte.Location = new Point(747, 48);
            btnGuardarMovCtaCte.Name = "btnGuardarMovCtaCte";
            btnGuardarMovCtaCte.Size = new Size(124, 45);
            btnGuardarMovCtaCte.TabIndex = 41;
            btnGuardarMovCtaCte.Text = "$ Pagar";
            btnGuardarMovCtaCte.UseVisualStyleBackColor = false;
            btnGuardarMovCtaCte.Click += btnGuardarMovCtaCte_Click;
            // 
            // comboBoxMetPago
            // 
            comboBoxMetPago.FormattingEnabled = true;
            comboBoxMetPago.Location = new Point(6, 58);
            comboBoxMetPago.Name = "comboBoxMetPago";
            comboBoxMetPago.Size = new Size(237, 36);
            comboBoxMetPago.TabIndex = 32;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
            label13.ForeColor = Color.Black;
            label13.Location = new Point(6, 35);
            label13.Name = "label13";
            label13.Size = new Size(200, 20);
            label13.TabIndex = 26;
            label13.Text = "Seleccione método de pago";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Font = new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
            label18.ForeColor = Color.Black;
            label18.Location = new Point(254, 35);
            label18.Name = "label18";
            label18.Size = new Size(90, 20);
            label18.TabIndex = 30;
            label18.Text = "Descripción";
            // 
            // textBoxDescripcionMetPago
            // 
            textBoxDescripcionMetPago.Location = new Point(254, 59);
            textBoxDescripcionMetPago.Name = "textBoxDescripcionMetPago";
            textBoxDescripcionMetPago.Size = new Size(185, 34);
            textBoxDescripcionMetPago.TabIndex = 31;
            // 
            // groupBoxDatosCuenta
            // 
            groupBoxDatosCuenta.BackColor = Color.FromArgb(224, 224, 224);
            groupBoxDatosCuenta.Controls.Add(textBoxSaldoActual);
            groupBoxDatosCuenta.Controls.Add(label15);
            groupBoxDatosCuenta.Controls.Add(textBoxNombreCompleto);
            groupBoxDatosCuenta.Controls.Add(label14);
            groupBoxDatosCuenta.Font = new System.Drawing.Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxDatosCuenta.ForeColor = Color.FromArgb(52, 73, 94);
            groupBoxDatosCuenta.Location = new Point(598, 16);
            groupBoxDatosCuenta.Name = "groupBoxDatosCuenta";
            groupBoxDatosCuenta.Size = new Size(880, 146);
            groupBoxDatosCuenta.TabIndex = 1;
            groupBoxDatosCuenta.TabStop = false;
            groupBoxDatosCuenta.Text = "Datos de la cuenta";
            // 
            // textBoxSaldoActual
            // 
            textBoxSaldoActual.Enabled = false;
            textBoxSaldoActual.Font = new System.Drawing.Font("Segoe UI", 10.8F, FontStyle.Bold);
            textBoxSaldoActual.Location = new Point(398, 81);
            textBoxSaldoActual.Name = "textBoxSaldoActual";
            textBoxSaldoActual.Size = new Size(333, 31);
            textBoxSaldoActual.TabIndex = 3;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.ForeColor = Color.Black;
            label15.Location = new Point(398, 55);
            label15.Name = "label15";
            label15.Size = new Size(111, 23);
            label15.TabIndex = 2;
            label15.Text = "Saldo Actual";
            // 
            // textBoxNombreCompleto
            // 
            textBoxNombreCompleto.Enabled = false;
            textBoxNombreCompleto.Font = new System.Drawing.Font("Segoe UI", 10.8F, FontStyle.Bold);
            textBoxNombreCompleto.Location = new Point(25, 81);
            textBoxNombreCompleto.Name = "textBoxNombreCompleto";
            textBoxNombreCompleto.Size = new Size(333, 31);
            textBoxNombreCompleto.TabIndex = 1;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.ForeColor = Color.Black;
            label14.Location = new Point(25, 55);
            label14.Name = "label14";
            label14.Size = new Size(160, 23);
            label14.TabIndex = 0;
            label14.Text = "Nombre Completo";
            // 
            // groupBox3
            // 
            groupBox3.BackColor = Color.FromArgb(224, 224, 224);
            groupBox3.Controls.Add(textBoxClienteConCtaCte);
            groupBox3.Controls.Add(dataGridViewCuentasClientes);
            groupBox3.Location = new Point(7, 16);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(585, 812);
            groupBox3.TabIndex = 0;
            groupBox3.TabStop = false;
            groupBox3.Text = "Clientes";
            // 
            // textBoxClienteConCtaCte
            // 
            textBoxClienteConCtaCte.Location = new Point(16, 52);
            textBoxClienteConCtaCte.Name = "textBoxClienteConCtaCte";
            textBoxClienteConCtaCte.PlaceholderText = "Filtrar por Nombre, Apellido o DNI";
            textBoxClienteConCtaCte.Size = new Size(333, 30);
            textBoxClienteConCtaCte.TabIndex = 4;
            textBoxClienteConCtaCte.TextChanged += textBoxClienteConCtaCte_TextChanged;
            // 
            // dataGridViewCuentasClientes
            // 
            dataGridViewCuentasClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCuentasClientes.Location = new Point(16, 95);
            dataGridViewCuentasClientes.Name = "dataGridViewCuentasClientes";
            dataGridViewCuentasClientes.RowHeadersWidth = 51;
            dataGridViewCuentasClientes.Size = new Size(563, 690);
            dataGridViewCuentasClientes.TabIndex = 2;
            dataGridViewCuentasClientes.CellClick += dataGridViewCuentasClientes_CellClick;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(45, 66, 91);
            panel1.Controls.Add(label11);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1561, 80);
            panel1.TabIndex = 17;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new System.Drawing.Font("Segoe UI", 16F, FontStyle.Bold);
            label11.ForeColor = Color.White;
            label11.Location = new Point(34, 24);
            label11.Name = "label11";
            label11.Size = new Size(473, 37);
            label11.TabIndex = 0;
            label11.Text = "MÓDULO DE CUENTAS CORRIENTES";
            // 
            // FrmCuentaCorriente
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightSteelBlue;
            ClientSize = new Size(1561, 1024);
            Controls.Add(panel1);
            Controls.Add(tabControl1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FrmCuentaCorriente";
            Text = "Clientes - Casa de Repuestos";
            tabControl1.ResumeLayout(false);
            tabPageCtaCte.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewFacturasPendientes).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBoxDatosCuenta.ResumeLayout(false);
            groupBoxDatosCuenta.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCuentasClientes).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
            private System.Windows.Forms.Panel panel1;
            private System.Windows.Forms.Label label11;
            private Panel panelHeader;
            private Label lblTitulo;
            private TabPage tabPageCtaCte;
            private GroupBox groupBox3;
            private DataGridView dataGridViewCuentasClientes;
            private GroupBox groupBoxDatosCuenta;
            private TextBox textBoxSaldoActual;
            private Label label15;
            private TextBox textBoxNombreCompleto;
            private Label label14;
            private Button btnGuardarMovCtaCte;
            private TextBox textBoxClienteConCtaCte;
            private GroupBox groupBox4;
            private ComboBox comboBoxMetPago;
            private Label label13;
            private Label label18;
            private TextBox textBoxDescripcionMetPago;
            private DataGridView dataGridViewFacturasPendientes;
            private Label label16;
            private TextBox textBoxSaldoaPagar;
        }
    }