using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using CasaRepuestos.Models;

namespace CasaRepuestos.Services
{
    public class ArqueoService
    {
        //  Obtener todas las cajas registradas
        public List<Caja> ObtenerTodasLasCajas()
        {
            var cajas = new List<Caja>();

            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                string query = "SELECT idcaja, fecha_apertura, saldo_inicial, fecha_cierre, saldo_final, idempleado FROM cajas ORDER BY fecha_apertura DESC";
                using var cmd = new MySqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cajas.Add(new Caja
                    {
                        IdCaja = reader.GetInt32("idcaja"),
                        FechaApertura = reader.GetDateTime("fecha_apertura"),
                        SaldoInicial = reader.GetDecimal("saldo_inicial"),
                        FechaCierre = reader.IsDBNull("fecha_cierre") ? (DateTime?)null : reader.GetDateTime("fecha_cierre"),
                        SaldoFinal = reader.IsDBNull("saldo_final") ? 0 : reader.GetDecimal("saldo_final"),
                        IdEmpleado = reader.GetInt32("idempleado"),
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener cajas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return cajas;
        }

        // Verificar si hay una caja abierta hoy
        public bool ExisteCajaAbiertaHoy()
        {
            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                string query = "SELECT COUNT(*) FROM cajas WHERE DATE(fecha_apertura) = CURDATE() AND fecha_cierre IS NULL";
                using var cmd = new MySqlCommand(query, conn);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar caja abierta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        //Cerrar automáticamente cajas de días anteriores
        public int CerrarCajasAnterioresAutomaticamente()
        {
            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                // Buscar cajas abiertas de días anteriores al día actual
                string queryBuscar = @"
                    SELECT idcaja, fecha_apertura, saldo_inicial
                    FROM cajas 
                    WHERE DATE(fecha_apertura) < CURDATE() 
                    AND fecha_cierre IS NULL";

                var cajasAnteriores = new List<(int idCaja, DateTime fechaApertura, decimal saldoInicial)>();

                using (var cmd = new MySqlCommand(queryBuscar, conn))
                {
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cajasAnteriores.Add((
                            reader.GetInt32("idcaja"),
                            reader.GetDateTime("fecha_apertura"),
                            reader.GetDecimal("saldo_inicial")
                        ));
                    }
                }

                // Si no hay cajas anteriores abiertas, retornar 0
                if (cajasAnteriores.Count == 0)
                    return 0;

                // Cerrar cada caja del día anterior calculando su saldo final
                int cajasCerradas = 0;
                foreach (var (idCaja, fechaApertura, saldoInicial) in cajasAnteriores)
                {
                    // Calcular el saldo final de la caja
                    decimal saldoFinal = CalcularSaldoFinalCaja(fechaApertura.Date, saldoInicial);

                    // Cerrar la caja con el saldo calculado
                    string updateQuery = @"
                        UPDATE cajas 
                        SET fecha_cierre = DATE_ADD(DATE(fecha_apertura), INTERVAL 1 DAY),
                            saldo_final = @saldoFinal
                        WHERE idcaja = @idCaja";

                    using var cmdUpdate = new MySqlCommand(updateQuery, conn);
                    cmdUpdate.Parameters.AddWithValue("@idCaja", idCaja);
                    cmdUpdate.Parameters.AddWithValue("@saldoFinal", saldoFinal);
                    cmdUpdate.ExecuteNonQuery();
                    cajasCerradas++;
                }

                return cajasCerradas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar cajas anteriores: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1; 
            }
        }

        // Calcular saldo final de una caja específica
        private decimal CalcularSaldoFinalCaja(DateTime fechaCaja, decimal saldoInicial)
        {
            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                // Obtener total de ingresos (facturas + pagos de cuenta corriente)
                decimal totalIngresos = 0;

                // Ingresos por facturas
                string queryFacturas = @"
                    SELECT IFNULL(SUM(total_factura), 0)
                    FROM facturas
                    WHERE DATE(fecha) = @fecha";

                using (var cmd = new MySqlCommand(queryFacturas, conn))
                {
                    cmd.Parameters.AddWithValue("@fecha", fechaCaja);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        totalIngresos += Convert.ToDecimal(result);
                }

                // Ingresos por pagos de cuenta corriente
                string queryPagos = @"
                    SELECT IFNULL(SUM(monto), 0)
                    FROM movimientos_cuentas_corrientes
                    WHERE DATE(fecha_de_pago) = @fecha
                    AND concepto IN ('PAGO TOTAL', 'PAGO PARCIAL')
                    AND metodo_pago IS NOT NULL";

                using (var cmd = new MySqlCommand(queryPagos, conn))
                {
                    cmd.Parameters.AddWithValue("@fecha", fechaCaja);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        totalIngresos += Convert.ToDecimal(result);
                }

                // Obtener total de egresos 
                decimal totalEgresos = 0;

                // Egresos por gastos
                string queryGastos = @"
                    SELECT IFNULL(SUM(monto), 0)
                    FROM gastos
                    WHERE DATE(fecha) = @fecha";

                using (var cmd = new MySqlCommand(queryGastos, conn))
                {
                    cmd.Parameters.AddWithValue("@fecha", fechaCaja);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        totalEgresos += Convert.ToDecimal(result);
                }

                // Calcular saldo final: saldo inicial + ingresos - egresos
                return saldoInicial + totalIngresos - totalEgresos;
            }
            catch (Exception)
            {
                // En caso de error, retornar el saldo inicial 
                return saldoInicial;
            }
        }

        // Verificar si la caja del día está cerrada
        public bool EsCajaDelDiaCerrada()
        {
            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                string query = @"
                    SELECT COUNT(*) 
                    FROM cajas 
                    WHERE DATE(fecha_apertura) = CURDATE() 
                    AND fecha_cierre IS NOT NULL";

                using var cmd = new MySqlCommand(query, conn);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                
                return count > 0; // Retorna true si la caja está cerrada
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar estado de caja: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true; 
            }
        }

        // Verificar si existe alguna caja para hoy (abierta o cerrada)
        public bool ExisteCajaHoy()
        {
            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                string query = "SELECT COUNT(*) FROM cajas WHERE DATE(fecha_apertura) = CURDATE()";
                using var cmd = new MySqlCommand(query, conn);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar caja hoy: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Abrir caja
        public bool AbrirCaja(DateTime fecha, decimal saldoInicial, int idEmpleado)
        {
            try
            {
                // Solo permitir abrir cajas del día actual
                if (fecha.Date != DateTime.Today)
                {
                    MessageBox.Show(
                        "❌ No se puede abrir una caja con fecha diferente al día actual.\n\n" +
                        $"Fecha solicitada: {fecha:dd/MM/yyyy}\n" +
                        $"Fecha actual: {DateTime.Today:dd/MM/yyyy}\n\n" +
                        "Solo se pueden abrir cajas para el día de hoy.",
                        "Fecha Inválida",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return false;
                }

                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                // Verificar si ya existe una caja para hoy (abierta o cerrada)
                string checkQuery = "SELECT COUNT(*) FROM cajas WHERE DATE(fecha_apertura) = @fecha";
                using (var checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@fecha", fecha.Date);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                        return false; // Ya existe una caja para hoy
                }

                string insertQuery = "INSERT INTO cajas (fecha_apertura, saldo_inicial, idempleado) VALUES (@fecha, @saldo, @idEmpleado)";
                using (var cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    cmd.Parameters.AddWithValue("@saldo", saldoInicial);
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir caja: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Cerrar caja
        public bool CerrarCaja(DateTime fecha, decimal saldoFinal)
        {
            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                string updateQuery = @"
                    UPDATE cajas 
                    SET fecha_cierre = @fecha, saldo_final = @saldoFinal
                    WHERE DATE(fecha_apertura) = @fecha AND fecha_cierre IS NULL";

                using var cmd = new MySqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@fecha", fecha.Date);
                cmd.Parameters.AddWithValue("@saldoFinal", saldoFinal);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar caja: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Reabrir caja cerrada
        public bool ReabrirCaja(DateTime fecha)
        {
            try
            {
                // Solo permitir reabrir cajas del día actual
                if (fecha.Date != DateTime.Today)
                {
                    MessageBox.Show(
                        "❌ No se puede reabrir una caja de un día anterior.\n\n" +
                        $"Fecha solicitada: {fecha:dd/MM/yyyy}\n" +
                        $"Fecha actual: {DateTime.Today:dd/MM/yyyy}\n\n" +
                        "Solo se pueden reabrir cajas del día actual.",
                        "Fecha Inválida",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return false;
                }

                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                string updateQuery = @"
                    UPDATE cajas 
                    SET fecha_cierre = NULL, saldo_final = 0
                    WHERE DATE(fecha_apertura) = @fecha AND fecha_cierre IS NOT NULL";

                using var cmd = new MySqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@fecha", fecha.Date);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al reabrir caja: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Abrir caja incluso si ya existe una cerrada
        public bool AbrirCajaForzar(DateTime fecha, decimal saldoInicial, int idEmpleado)
        {
            try
            {
                // Solo permitir abrir cajas del día actual
                if (fecha.Date != DateTime.Today)
                {
                    MessageBox.Show(
                        "❌ No se puede abrir una caja con fecha diferente al día actual.\n\n" +
                        $"Fecha solicitada: {fecha:dd/MM/yyyy}\n" +
                        $"Fecha actual: {DateTime.Today:dd/MM/yyyy}\n\n" +
                        "Solo se pueden abrir cajas para el día de hoy.",
                        "Fecha Inválida",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return false;
                }

                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                // Verificar si ya existe una caja cerrada para hoy
                string checkQuery = "SELECT COUNT(*) FROM cajas WHERE DATE(fecha_apertura) = @fecha AND fecha_cierre IS NOT NULL";
                using (var checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@fecha", fecha.Date);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // Reabrir la caja existente
                        return ReabrirCaja(fecha);
                    }
                }

                // Si no existe ninguna caja, crear una nueva
                string insertQuery = "INSERT INTO cajas (fecha_apertura, saldo_inicial, idempleado) VALUES (@fecha, @saldo, @idEmpleado)";
                using (var cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    cmd.Parameters.AddWithValue("@saldo", saldoInicial);
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir caja: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        
        public List<MovimientoCaja> ObtenerIngresosDelDia(DateTime fecha)
        {
            var ingresos = new List<MovimientoCaja>();

            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                // Obtener todas las facturas del día
                string queryFacturas = @"
            SELECT 
                f.metodo_pago,
                f.total_factura,
                f.fecha,
                f.idfactura,
                f.idpresupuesto
            FROM facturas f
            WHERE DATE(f.fecha) = @fecha
            ORDER BY f.fecha";

                using var cmdFacturas = new MySqlCommand(queryFacturas, conn);
                cmdFacturas.Parameters.AddWithValue("@fecha", fecha.Date);

                using var readerFacturas = cmdFacturas.ExecuteReader();

                while (readerFacturas.Read())
                {
                    string metodoPago = readerFacturas.IsDBNull("metodo_pago") ? "Indefinido" : readerFacturas.GetString("metodo_pago");
                    decimal monto = readerFacturas.GetDecimal("total_factura");
                    DateTime fechaFactura = readerFacturas.GetDateTime("fecha");
                    int idFactura = readerFacturas.GetInt32("idfactura");
                    bool tienePresupuesto = !readerFacturas.IsDBNull("idpresupuesto");

                    string descripcion = ObtenerDescripcionCompletaFactura(idFactura, tienePresupuesto);

                    ingresos.Add(new MovimientoCaja
                    {
                        Descripcion = descripcion,
                        MetodoPago = metodoPago,
                        Monto = monto,
                        Fecha = fechaFactura,
                        Tipo = "INGRESO"
                    });
                }
                readerFacturas.Close();

                //Obtener pagos de cuenta corriente del día
                string queryPagosCuentaCorriente = @"
            SELECT 
                m.monto,
                m.metodo_pago,
                m.fecha_de_pago,
                m.concepto,
                CONCAT(p.nombre, ' ', p.apellido) as cliente_nombre
            FROM movimientos_cuentas_corrientes m
            INNER JOIN cuentas_corrientes cc ON m.idcuentaacorriente = cc.idcuenta_corriente
            INNER JOIN clientes c ON cc.idcliente = c.idcliente
            INNER JOIN personas p ON c.idpersona = p.idpersona
            WHERE DATE(m.fecha_de_pago) = @fecha
            AND m.concepto IN ('PAGO TOTAL', 'PAGO PARCIAL')
            AND m.metodo_pago IS NOT NULL
            ORDER BY m.fecha_de_pago";

                using var cmdPagos = new MySqlCommand(queryPagosCuentaCorriente, conn);
                cmdPagos.Parameters.AddWithValue("@fecha", fecha.Date);

                using var readerPagos = cmdPagos.ExecuteReader();

                while (readerPagos.Read())
                {
                    decimal monto = readerPagos.GetDecimal("monto");
                    string metodoPago = readerPagos.GetString("metodo_pago");
                    string concepto = readerPagos.GetString("concepto");
                    string clienteNombre = readerPagos.GetString("cliente_nombre");
                    DateTime fechaPago = readerPagos.GetDateTime("fecha_de_pago");

                    string descripcion = $"Pago Cta. Cte. - {clienteNombre} ({concepto})";

                    ingresos.Add(new MovimientoCaja
                    {
                        Descripcion = descripcion,
                        MetodoPago = metodoPago,
                        Monto = monto,
                        Fecha = fechaPago,
                        Tipo = "INGRESO"
                    });
                }
                readerPagos.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener ingresos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return ingresos;
        }

        // Método auxiliar para obtener la descripción completa de la factura
        private string ObtenerDescripcionCompletaFactura(int idFactura, bool tienePresupuesto)
        {
            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                if (tienePresupuesto)
                {
                    // Es una reparación - obtener productos de la reparación Y productos adicionales vendidos
                    return ObtenerDescripcionReparacionConProductos(idFactura);
                }
                else
                {
                    // Es una venta directa - solo productos vendidos
                    return ObtenerDescripcionVentaDirecta(idFactura);
                }
            }
            catch (Exception)
            {
                return tienePresupuesto ? "Reparación con productos" : "Venta de productos";
            }
        }

        // Obtener descripción para REPARACIONES (puede incluir productos de reparación y productos vendidos)
        private string ObtenerDescripcionReparacionConProductos(int idFactura)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            var productosReparacion = new List<string>();
            var productosVendidos = new List<string>();

            //  Obtener productos de la REPARACIÓN (del presupuesto)
            string queryReparacion = @"
        SELECT DISTINCT a.nombre
        FROM detalles_presupuestos dp 
        INNER JOIN articulos a ON dp.idarticulo = a.idarticulo 
        INNER JOIN presupuestos p ON dp.idpresupuesto = p.idpresupuesto
        INNER JOIN facturas f ON p.idpresupuesto = f.idpresupuesto
        WHERE f.idfactura = @idFactura
        AND dp.idarticulo IS NOT NULL";

            using (var cmd = new MySqlCommand(queryReparacion, conn))
            {
                cmd.Parameters.AddWithValue("@idFactura", idFactura);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string nombre = reader.IsDBNull("nombre") ? "Producto sin nombre" : reader.GetString("nombre");
                    productosReparacion.Add(nombre);
                }
            }

            //  Obtener productos VENDIDOS ADICIONALES (del detalle_factura)
            string queryProductosVendidos = @"
        SELECT DISTINCT a.nombre
        FROM detalle_factura df 
        INNER JOIN articulos a ON df.idarticulo = a.idarticulo 
        WHERE df.idfactura = @idFactura";

            using (var cmd = new MySqlCommand(queryProductosVendidos, conn))
            {
                cmd.Parameters.AddWithValue("@idFactura", idFactura);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string nombre = reader.IsDBNull("nombre") ? "Producto sin nombre" : reader.GetString("nombre");
                    productosVendidos.Add(nombre);
                }
            }

            //  Construir la descripción completa
            string descripcion = "Reparación";

            if (productosReparacion.Count > 0)
            {
                descripcion += $" con {string.Join(", ", productosReparacion)}";
            }
            else
            {
                descripcion += " (servicio técnico)";
            }

            if (productosVendidos.Count > 0)
            {
                descripcion += $" + venta: {string.Join(", ", productosVendidos)}";
            }

            return descripcion;
        }

        //  Obtener descripción para VENTAS DIRECTAS
        private string ObtenerDescripcionVentaDirecta(int idFactura)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            var productos = new List<string>();

            string queryVenta = @"
        SELECT DISTINCT a.nombre
        FROM detalle_factura df 
        INNER JOIN articulos a ON df.idarticulo = a.idarticulo 
        WHERE df.idfactura = @idFactura";

            using var cmd = new MySqlCommand(queryVenta, conn);
            cmd.Parameters.AddWithValue("@idFactura", idFactura);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string nombre = reader.IsDBNull("nombre") ? "Producto sin nombre" : reader.GetString("nombre");
                productos.Add(nombre);
            }

            return productos.Count > 0
                ? $"Venta: {string.Join(", ", productos)}"
                : "Venta: Productos varios";
        }

        // Obtener egresos del día - USANDO COMPRA Y GASTOS (incluyendo retiros)
        public List<MovimientoCaja> ObtenerEgresosDelDia(DateTime fecha)
        {
            var egresos = new List<MovimientoCaja>();

            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                // Obtener gastos (incluyendo retiros de dinero) 
                string queryGastos = @"
            SELECT 
                descripcion_gasto AS Descripcion,
                metodo_pago AS MetodoPago,
                monto AS Monto,
                fecha AS Fecha
            FROM gastos
            WHERE DATE(fecha) = @fecha
            ORDER BY fecha";

                using (var cmd = new MySqlCommand(queryGastos, conn))
                {
                    cmd.Parameters.AddWithValue("@fecha", fecha.Date);
                    using var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        egresos.Add(new MovimientoCaja
                        {
                            Descripcion = reader.GetString("Descripcion"),
                            MetodoPago = reader.IsDBNull("MetodoPago") ? "Indefinido" : reader.GetString("MetodoPago"),
                            Monto = reader.GetDecimal("Monto"),
                            Fecha = reader.GetDateTime("Fecha"),
                            Tipo = "EGRESO"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener egresos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return egresos;
        }

    
        public decimal ObtenerTotalIngresosPorFecha(DateTime fecha)
        {
            decimal total = 0;

            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                // Sumar todas las facturas del día
                string queryVentas = @"
            SELECT IFNULL(SUM(f.total_factura), 0)
            FROM facturas f
            WHERE DATE(f.fecha) = @fecha";

                using (var cmd = new MySqlCommand(queryVentas, conn))
                {
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        total += Convert.ToDecimal(result);
                    }
                }

                // sumar pagos de cuenta corriente del día
                string queryPagosCuentaCorriente = @"
            SELECT IFNULL(SUM(m.monto), 0)
            FROM movimientos_cuentas_corrientes m
            WHERE DATE(m.fecha_de_pago) = @fecha
            AND m.concepto IN ('PAGO TOTAL', 'PAGO PARCIAL')
            AND m.metodo_pago IS NOT NULL";

                using (var cmd = new MySqlCommand(queryPagosCuentaCorriente, conn))
                {
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        total += Convert.ToDecimal(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener total de ingresos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return total;
        }

        //  Obtener monto inicial de caja 
        public decimal ObtenerMontoInicialCaja(DateTime fecha)
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            string query = @"
                SELECT saldo_inicial 
                FROM cajas 
                WHERE DATE(fecha_apertura) = @fecha 
                AND fecha_cierre IS NULL 
                LIMIT 1";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@fecha", fecha.Date);

            var result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                return Convert.ToDecimal(result);
            }

            return 0m;
        }

        //  Obtener caja abierta hoy 
        public Caja ObtenerCajaAbiertaHoy()
        {
            using var conn = new MySqlConnection(Config.Config.ConnectionString);
            conn.Open();

            string query = @"
                SELECT idcaja, fecha_apertura, saldo_inicial, fecha_cierre, saldo_final, idempleado
                FROM cajas 
                WHERE DATE(fecha_apertura) = @fecha 
                AND fecha_cierre IS NULL 
                LIMIT 1";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@fecha", DateTime.Today);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Caja
                {
                    IdCaja = reader.GetInt32("idcaja"),
                    FechaApertura = reader.GetDateTime("fecha_apertura"),
                    SaldoInicial = reader.GetDecimal("saldo_inicial"),
                    FechaCierre = reader.IsDBNull("fecha_cierre") ? (DateTime?)null : reader.GetDateTime("fecha_cierre"),
                    SaldoFinal = reader.IsDBNull("saldo_final") ? 0m : reader.GetDecimal("saldo_final"),
                    IdEmpleado = reader.GetInt32("idempleado"),
                    Estado = "ABIERTA"
                };
            }

            return null;
        }

        //  Método para registrar retiro de dinero
        public bool RegistrarRetiroDinero(string descripcion, decimal monto, DateTime fecha, string metodoPago)
        {
            try
            {
                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                // Insertar el retiro como un gasto en la tabla gastos
                string insertQuery = @"
                    INSERT INTO gastos (descripcion_gasto, monto, fecha, tipo_gasto, metodo_pago) 
                    VALUES (@descripcion, @monto, @fecha, 'Retiro de Dinero', @metodoPago)";

                using var cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                cmd.Parameters.AddWithValue("@monto", monto);
                cmd.Parameters.AddWithValue("@fecha", fecha);
                cmd.Parameters.AddWithValue("@metodoPago", metodoPago);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar el retiro: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Obtener saldo actual de la caja abierta hoy
        public decimal ObtenerSaldoActualCaja()
        {
            try
            {
                Caja cajaAbierta = ObtenerCajaAbiertaHoy();
                if (cajaAbierta == null)
                {
                    return 0m;
                }

                DateTime fechaCaja = cajaAbierta.FechaApertura.Date;
                decimal montoInicial = cajaAbierta.SaldoInicial;

                // Obtener ingresos y egresos
                var ingresos = ObtenerIngresosDelDia(fechaCaja) ?? new List<MovimientoCaja>();
                var egresos = ObtenerEgresosDelDia(fechaCaja) ?? new List<MovimientoCaja>();
                
                
                decimal totalIngresos = ingresos.Sum(i => i.Monto);
                decimal totalEgresos = egresos.Sum(e => e.Monto);
                
                // Calcular total de ventas y servicios 
                decimal totalVentasYServicios = ObtenerTotalIngresosPorFecha(fechaCaja);

                decimal saldoActual = montoInicial + totalVentasYServicios - totalEgresos;
                return saldoActual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener saldo actual: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0m;
            }
        }

        // Obtener saldo disponible por método de pago
        public Dictionary<string, decimal> ObtenerSaldosPorMetodoPago(DateTime fecha)
        {
            var saldos = new Dictionary<string, decimal>();

            try
            {
                // Métodos de pago comunes
                var metodosPago = new string[] { "EFECTIVO", "TARJETA", "TRANSFERENCIA", "BILLETERA VIRTUAL" };

                // Inicializar saldos en cero
                foreach (var metodo in metodosPago)
                {
                    saldos[metodo] = 0m;
                }

                using var conn = new MySqlConnection(Config.Config.ConnectionString);
                conn.Open();

                // Obtener ingresos por método de pago
                foreach (var metodo in metodosPago)
                {
                    // Ingresos por facturas
                    string queryIngresosFacturas = @"
                        SELECT IFNULL(SUM(total_factura), 0)
                        FROM facturas
                        WHERE DATE(fecha) = @fecha AND metodo_pago = @metodo";

                    decimal ingresosFacturas = 0;
                    using (var cmd = new MySqlCommand(queryIngresosFacturas, conn))
                    {
                        cmd.Parameters.AddWithValue("@fecha", fecha.Date);
                        cmd.Parameters.AddWithValue("@metodo", metodo);
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            ingresosFacturas = Convert.ToDecimal(result);
                    }

                    // Ingresos por pagos de cuenta corriente
                    string queryIngresosCuentaCorriente = @"
                        SELECT IFNULL(SUM(monto), 0)
                        FROM movimientos_cuentas_corrientes
                        WHERE DATE(fecha_de_pago) = @fecha
                        AND concepto IN ('PAGO TOTAL', 'PAGO PARCIAL')
                        AND metodo_pago = @metodo";

                    decimal ingresosCuentaCorriente = 0;
                    using (var cmd = new MySqlCommand(queryIngresosCuentaCorriente, conn))
                    {
                        cmd.Parameters.AddWithValue("@fecha", fecha.Date);
                        cmd.Parameters.AddWithValue("@metodo", metodo);
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            ingresosCuentaCorriente = Convert.ToDecimal(result);
                    }

                    // Egresos por retiros (gastos) del mismo método de pago
                    string queryEgresos = @"
                        SELECT IFNULL(SUM(monto), 0)
                        FROM gastos
                        WHERE DATE(fecha) = @fecha AND metodo_pago = @metodo";

                    decimal egresos = 0;
                    using (var cmd = new MySqlCommand(queryEgresos, conn))
                    {
                        cmd.Parameters.AddWithValue("@fecha", fecha.Date);
                        cmd.Parameters.AddWithValue("@metodo", metodo);
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            egresos = Convert.ToDecimal(result);
                    }

                    // Calcular saldo neto para este método de pago
                    saldos[metodo] = ingresosFacturas + ingresosCuentaCorriente - egresos;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener saldos por método de pago: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return saldos;
        }
    }
}