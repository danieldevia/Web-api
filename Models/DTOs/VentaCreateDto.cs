namespace InventarioApi.Models.DTOs
{
    public class DetalleVentaCreateDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }

    public class VentaCreateDto
    {
        public List<DetalleVentaCreateDto> Detalles { get; set; } = new();
    }

    public class VentaResponseDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioId { get; set; }
        public decimal Total { get; set; }
        public List<DetalleVentaResponseDto> Detalles { get; set; } = new();
    }

    public class DetalleVentaResponseDto
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal Utilidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}