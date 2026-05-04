using InventarioApi.Models.DTOs;

namespace InventarioApi.Services.Interfaces
{
    public interface IProductoService
    {
        // CRUD base
        List<ProductoResponseDto> GetAll();
        ProductoResponseDto? GetById(int id);
        void Add(ProductoCreateDto dto);
        void Update(ProductoUpdateDto dto);
        void Delete(int id);

        // Filtros
        List<ProductoResponseDto> GetByCategoria(int categoriaId);
        List<ProductoResponseDto> GetByDisponibilidad(bool disponible);

        // Operaciones especiales
        void ModificarPrecio(int id, decimal nuevoPrecio);
        List<ProductoResponseDto> GetBajoStock(int umbral = 5);
    }
}