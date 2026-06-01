namespace InventarioApi.Models.DTOs
{
    public class CompraDto
    {
        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        public decimal CostoUnitario { get; set; }

        public string Observacion { get; set; } = string.Empty;
    }
}