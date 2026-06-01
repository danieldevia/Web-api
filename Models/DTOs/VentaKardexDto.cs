namespace InventarioApi.Models.DTOs
{
    public class VentaKardexDto
    {
        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        public string Observacion { get; set; } = string.Empty;
    }
}