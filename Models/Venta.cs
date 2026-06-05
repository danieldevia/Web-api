namespace InventarioApi.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public int UsuarioId { get; set; } 
        public decimal Total { get; set; }
         public int VentaId { get; set; }
        public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
    }
}
