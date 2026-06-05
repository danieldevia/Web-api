namespace InventarioApi.Models.DTOs
{
    public class AjusteDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }   // positivo suma, negativo resta
        public string Motivo { get; set; } = string.Empty;
    }

    public class DevolucionDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
         public int VentaId { get; set; }
        public string Observacion { get; set; } = string.Empty;
    }

    public class KardexMovimientoDto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty;

        // Entradas
        public int CantidadEntrada { get; set; }
        public decimal CostoUnitarioEntrada { get; set; }
        public decimal CostoTotalEntrada { get; set; }

        // Salidas
        public int CantidadSalida { get; set; }
        public decimal CostoUnitarioSalida { get; set; }
        public decimal CostoTotalSalida { get; set; }

        // Venta
        public decimal PrecioVenta { get; set; }
        public decimal Utilidad { get; set; }
        public decimal IngresoTotal { get; set; }

        // Saldo
        public int SaldoCantidad { get; set; }
        public decimal CostoPromedio { get; set; }
        public decimal SaldoValor { get; set; }

        public string Observacion { get; set; } = string.Empty;
    }

    public class KardexResumenDto
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;

        // Stock
        public int StockActual { get; set; }
        public decimal CostoPromedioPonderado { get; set; }
        public decimal ValorTotalInventario { get; set; }

        // Totales entradas
        public int TotalUnidadesEntradas { get; set; }
        public decimal TotalCostoEntradas { get; set; }

        // Totales salidas
        public int TotalUnidadesSalidas { get; set; }
        public decimal TotalCostoSalidas { get; set; }

        // Utilidad
        public decimal UtilidadTotal { get; set; }
        public decimal TotalIngresos { get; set; }

        public List<KardexMovimientoDto> Movimientos { get; set; } = new();
    }
}