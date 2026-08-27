using System;
using System.Windows.Forms;
using CasaRepuestos.Models;
using CasaRepuestos.Services;
using System.Globalization;

namespace CasaRepuestos.Forms
{
    public partial class FrmRecepcionCompra : Form
    {
        // ID de la orden de compra a procesar
        private readonly int _idOrdenCompra;
        private readonly ComprasService _comprasService = new ComprasService();// Servicio para manejar compras
        private OrdenCompra _ordenActual;// Orden de compra actual

        public FrmRecepcionCompra(int idOrdenCompra)// Constructor que recibe el ID de la orden de compra
        {
            InitializeComponent();
            _idOrdenCompra = idOrdenCompra;// Almacenar el ID de la orden
            // Suscribir el evento de edición de celda
            dgvDetallesRecepcion.CellEndEdit += dgvDetallesRecepcion_CellEndEdit;
            CargarDatosOrden();// Cargar los datos de la orden en el formulario
            // Configurar el ComboBox de métodos de pago
            cmbMetodoPago.Items.AddRange(new object[] {
                "EFECTIVO",
                "TRANSFERENCIA",
                "TARJETA",
                "BILLETERA VIRTUAL",
                "CUENTA CORRIENTE"
            });
            // Seleccionar el primer método por defecto
            cmbMetodoPago.SelectedIndex = 1;
        }
        // Método para cargar los datos de la orden de compra
        private void CargarDatosOrden()
        {
            _ordenActual = _comprasService.GetOrdenConDetalles(_idOrdenCompra);// Obtener la orden con sus detalles
            if (_ordenActual == null)// Validar que la orden exista
            {
                MessageBox.Show("Error: No se pudo cargar la orden de compra.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            // Mostrar datos de la orden en los controles correspondientes
            lblNumeroOrden.Text = _ordenActual.IdOrdenCompra.ToString();
            lblProveedor.Text = _ordenActual.ProveedorNombre;// Nombre del proveedor
            lblFechaEmision.Text = _ordenActual.FechaCreacion.ToShortDateString();
            // Cargar los detalles en el DataGridView
            dgvDetallesRecepcion.Rows.Clear();// Limpiar filas existentes
            foreach (var detalle in _ordenActual.Detalles)// Recorrer cada detalle de la orden
            {
                // Usamos la cantidad solicitada como cantidad por defecto a recibir
                decimal subtotal = detalle.CantidadSolicitada * detalle.PrecioUnitarioEstimado;
                // Agregar una nueva fila con los datos del detalle
                dgvDetallesRecepcion.Rows.Add(
                    detalle.IdArticulo,
                    detalle.ArticuloNombre,
                    detalle.CantidadSolicitada, 
                    detalle.PrecioUnitarioEstimado, 
                    subtotal  
                );
            }
            CalcularTotal();// Calcular el total inicial
        }

        // Metodo manejador del evento CellEndEdit para validar y recalcular valores
        private void dgvDetallesRecepcion_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;// Validar índice de fila

            DataGridViewRow row = dgvDetallesRecepcion.Rows[e.RowIndex];// Obtener la fila editada
            CultureInfo culture = CultureInfo.GetCultureInfo("es-AR");// Cultura para formato monetario

            // Validaciones y recalculos después de editar una celda
            if (!int.TryParse(row.Cells["CantidadRecibida"].Value?.ToString(), out int cantidad) || cantidad < 0)
            {
                MessageBox.Show("Ingrese una Cantidad Recibida válida (número entero >= 0).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                row.Cells["CantidadRecibida"].Value = 0;// Resetear a 0 si es inválido
                cantidad = 0;// Asegurar que cantidad sea 0
            }

            // Validar Precio Unitario Final 
            string precioText = row.Cells["PrecioFinal"].Value?.ToString();
            if (!decimal.TryParse(precioText, NumberStyles.Currency | NumberStyles.Number, culture, out decimal precio) || precio < 0)// Validar formato y valor
            {
                MessageBox.Show("Ingrese un Precio Unitario Final válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                precio = 0m;
                row.Cells["PrecioFinal"].Value = precio.ToString("C2", culture);// Resetear a 0 formateado
            }

            //  Recalcular Subtotal de la Fila Editada
            decimal nuevoSubtotal = cantidad * precio; 
            row.Cells["Subtotal"].Value = nuevoSubtotal;// Actualizar el subtotal en la celda correspondiente

            // Recalcular Total General
            CalcularTotal();// Llamar al método para recalcular el total
        }

        // Método para calcular el total general de la orden
        private void CalcularTotal()
        {
            decimal total = 0m; // Inicializar total en 0
            CultureInfo culture = CultureInfo.GetCultureInfo("es-AR");// Cultura para formato monetario

            foreach (DataGridViewRow row in dgvDetallesRecepcion.Rows)// Recorrer cada fila del DataGridView
            {
                // Sumar solo si el Subtotal es válido
                if (row.Cells["Subtotal"].Value != null)
                {
                    if (decimal.TryParse(row.Cells["Subtotal"].Value.ToString(), out decimal subtotal))// Intentar parsear directamente
                    {
                        total += subtotal;// Sumar al total
                    }
                    else if (decimal.TryParse(row.Cells["Subtotal"].Value.ToString(), NumberStyles.Currency, culture, out subtotal))// Intentar parsear como moneda
                    {
                        
                        total += subtotal;//    
                    }
                }
            }
            
            txtTotalFinal.Text = total.ToString("C2", culture);//
        }

        private void btnConfirmarIngreso_Click(object sender, EventArgs e)
        {
            // 1. CONFIRMACIÓN DEL USUARIO
            // Muestra un cuadro de diálogo preguntando si está seguro de proceder.
            // Esto es crítico porque la operación modificará el stock permanentemente.
            var confirm = MessageBox.Show("¿Confirma la recepción de estos artículos? El stock será actualizado permanentemente.", "Confirmar Ingreso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Si el usuario presiona "No", se cancela la ejecución y sale de la función.
            if (confirm == DialogResult.No) return;

            // 2. CONFIGURACIÓN REGIONAL
            // Define la cultura argentina para manejar correctamente los formatos de moneda
            // (punto para miles, coma para decimales, símbolo $).
            CultureInfo culture = CultureInfo.GetCultureInfo("es-AR");

            // 3. VALIDACIÓN DEL TOTAL
            // Intenta convertir el texto del Total Final a un número decimal (decimal.TryParse).
            // Usa 'NumberStyles.Currency' para entender el formato de moneda.
            if (!decimal.TryParse(txtTotalFinal.Text, NumberStyles.Currency, culture, out decimal totalFinal))
            {
                // Si falla la conversión (ej: texto vacío o letras), muestra error y detiene el proceso.
                MessageBox.Show("El Total Final no es válido. Recalcule la orden.", "Error de Total", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 4. CREACIÓN DEL OBJETO DE COMPRA
            // Crea una nueva instancia de la clase 'Compra' con los datos generales.
            var compraFinal = new Compra
            {
                Fecha = DateTime.Now,                       // Registra la fecha y hora actual del ingreso.
                IdProveedor = _ordenActual.IdProveedor,     // Hereda el proveedor de la orden original.
                Total = totalFinal,                         // Asigna el total validado arriba.
                Tipo = _ordenActual.Tipo,                   // Mantiene el tipo (ej: Factura A, B, etc.).
                IdOrdenCompra = _ordenActual.IdOrdenCompra, // Vincula esta compra a la orden de origen.
                Detalles = new System.Collections.Generic.List<DetalleCompra>(), // Inicializa la lista vacía para los productos.
                MetodoPago = cmbMetodoPago.SelectedItem.ToString() // Toma el método de pago seleccionado en el combo.
            };

            // 5. PROCESAMIENTO DE LA GRILLA (DETALLES)
            // Recorre cada fila de la grilla visual (dgvDetallesRecepcion) para leer qué productos llegaron.
            foreach (DataGridViewRow row in dgvDetallesRecepcion.Rows)
            {
                // Lee la cantidad que el usuario ingresó en la celda.
                int cantidad = Convert.ToInt32(row.Cells["CantidadRecibida"].Value);
                decimal precio;

                // Obtiene el texto del precio de la celda. Si es nulo, usa "0".
                string precioString = row.Cells["PrecioFinal"].Value?.ToString() ?? "0";

                // Lógica robusta de conversión de precio:
                // Primero intenta parsear considerando formato moneda ($).
                if (!decimal.TryParse(precioString, NumberStyles.Currency, culture, out precio))
                {
                    // Si falla (quizás no tiene el signo $), intenta un parseo genérico.
                    decimal.TryParse(precioString, out precio);
                }

                // Solo agregamos el producto a la lista si la cantidad es mayor a 0 (si llegó algo).
                if (cantidad > 0)
                {
                    // Agrega el detalle a la lista 'Detalles' del objeto 'compraFinal'.
                    compraFinal.Detalles.Add(new DetalleCompra
                    {
                        IdArticulo = Convert.ToInt32(row.Cells["IdArticulo"].Value), // ID del producto.
                        Cantidad = cantidad,                                         // Cantidad real recibida.
                        PrecioUnitario = precio                                      // Precio final validado.
                    });
                }
            }

            // 6. VALIDACIÓN DE CONSISTENCIA
            // Verifica un caso ilógico: Si la lista de detalles está vacía (no llegó nada)
            // pero el total a pagar es mayor a 0, hay un error en los datos.
            if (compraFinal.Detalles.Count == 0 && totalFinal > 0)
            {
                MessageBox.Show("La orden está vacía, pero el total es positivo. Por favor, revise la cantidad y precio en la grilla.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 7. GUARDADO EN BASE DE DATOS Y FINALIZACIÓN
            try
            {
                // LLAMADA AL SERVICIO
                // Envía el objeto completo al servicio para que:
                // a) Guarde la compra en la BD.
                // b) Sume el stock a los artículos correspondientes.
                // c) Marque la orden de compra como "Ingresada".
                _comprasService.RegistrarIngresoDesdeOrden(compraFinal);

                // Si no hubo error, avisa al usuario.
                MessageBox.Show("¡Ingreso registrado y stock actualizado con éxito!.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Cierra este formulario retornando "OK" para que la ventana anterior actualice sus listas.
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                // Si ocurre cualquier error en el servicio (BD caída, error SQL), lo captura y muestra.
                MessageBox.Show("Error al registrar el ingreso: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}