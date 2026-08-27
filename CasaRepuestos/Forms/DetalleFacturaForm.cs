using CasaRepuestos.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CasaRepuestos.Forms
{
    public partial class DetalleFacturaForm : Form
    {
        // Form para mostrar los detalles de una factura específica
        public DetalleFacturaForm(int idFactura, List<DetalleFacturaAMostrar> detalles)
        {
            InitializeComponent();
            this.Text = $"Detalles de Factura N° {idFactura}";

            // Asignar y configurar DataGridView
            dgvDetalles.DataSource = detalles;
            dgvDetalles.ReadOnly = true;
            dgvDetalles.AllowUserToAddRows = false;
            dgvDetalles.AllowUserToDeleteRows = false;
            dgvDetalles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }


    }
}
