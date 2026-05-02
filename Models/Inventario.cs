
namespace InventarioApi.Models
{
    public class Inventario
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty; // "Entrada" o "Salida"
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Observaciones { get; set; } = string.Empty;
        public Producto? Producto { get; set; }
    }
}