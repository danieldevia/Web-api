namespace InventarioApi.Models
{
    public class Kardex
    {
        public int Id { get; set; }

        // Relación con Producto
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        // Información del movimiento
        public DateTime Fecha { get; set; } = DateTime.Now;

        public string TipoMovimiento { get; set; } = string.Empty;
        // Compra, Venta, Ajuste, Devolucion

        // Entradas
        public int CantidadEntrada { get; set; }
        public decimal CostoUnitarioEntrada { get; set; }
        public decimal CostoTotalEntrada { get; set; }

        // Salidas
        public int CantidadSalida { get; set; }
        public decimal CostoUnitarioSalida { get; set; }
        public decimal CostoTotalSalida { get; set; }

        // Saldo después del movimiento
        public int SaldoCantidad { get; set; }

        public decimal CostoPromedio { get; set; }

        public decimal SaldoValor { get; set; }

        public string Observacion { get; set; } = string.Empty;
    }
}