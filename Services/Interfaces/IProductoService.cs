using InventarioApi.Models.DTOs;

namespace InventarioApi.Services.Interfaces
{
    public interface IProductoService
    {
        List<ProductoResponseDto> GetAll();
        ProductoResponseDto? GetById(int id);
        void Add(ProductoCreateDto dto);
        void Update(ProductoUpdateDto dto);
        void Delete(int id);
        void AsignarCategoria(int productoId, int categoriaId);  // nuevo
        List<ProductoResponseDto> GetByCategoria(int categoriaId);
        List<ProductoResponseDto> GetByDisponibilidad(bool disponible);
        void ModificarPrecio(int id, decimal nuevoPrecio);
        List<ProductoResponseDto> GetBajoStock(int umbral = 5);
    }
}