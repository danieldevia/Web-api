namespace InventarioApi.Models.DTOs
{
    public class ProductoCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int CategoriaId { get; set; }
    }

    public class ProductoUpdateDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int CategoriaId { get; set; }
    }

    public class ProductoResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Disponible => Stock > 0;
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
    }

    public class ProductoPrecioDto
    {
        public decimal NuevoPrecio { get; set; }
    }
}