using InventarioApi.Models;
using InventarioApi.Models.DTOs;
using InventarioApi.Repository.Interfaces;
using InventarioApi.Services.Interfaces;
using Mapster;

namespace InventarioApi.Services.Implementations
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository  _productoRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        public ProductoService(
            IProductoRepository  productoRepository,
            ICategoriaRepository categoriaRepository)
        {
            _productoRepository  = productoRepository;
            _categoriaRepository = categoriaRepository;
        }

        // ── Helper privado ────────────────────────────────────────────────────
          private ProductoResponseDto MapToDto(Producto p)
        {
            var dto = p.Adapt<ProductoResponseDto>();
            if (p.CategoriaId.HasValue)
            {
                var categoria = _categoriaRepository.GetById(p.CategoriaId.Value);
                dto.CategoriaNombre = categoria?.Nombre ?? "Sin categoría";
            }
            return dto;
        }

        public List<ProductoResponseDto> GetAll() =>
            _productoRepository.GetAll().Select(MapToDto).ToList();

        public ProductoResponseDto? GetById(int id)
        {
            var producto = _productoRepository.GetById(id);
            return producto == null ? null : MapToDto(producto);
        }

         public void Add(ProductoCreateDto dto)
        {
            // Validar categoría solo si se envió
            if (dto.CategoriaId.HasValue && _categoriaRepository.GetById(dto.CategoriaId.Value) == null)
                throw new Exception("La categoría especificada no existe.");

            var todos = _productoRepository.GetAll();

            if (todos.Any(p => p.SKU == dto.SKU))
                throw new Exception("Ya existe un producto con ese SKU.");

            var nuevo = dto.Adapt<Producto>();
            nuevo.Id = todos.Count > 0 ? todos.Max(p => p.Id) + 1 : 1;

            _productoRepository.Add(nuevo);
        }

       public void Update(ProductoUpdateDto dto)
{
    var existente = _productoRepository.GetById(dto.Id)
        ?? throw new Exception("Producto no encontrado.");

    if (dto.CategoriaId.HasValue && _categoriaRepository.GetById(dto.CategoriaId.Value) == null)
        throw new Exception("La categoría especificada no existe.");

    var skuDuplicado = _productoRepository.GetAll()
        .Any(p => p.SKU == dto.SKU && p.Id != dto.Id);
    if (skuDuplicado)
        throw new Exception("Ya existe otro producto con ese SKU.");

    var actualizado = dto.Adapt<Producto>();
    actualizado.Stock = existente.Stock; // ← stock no se toca, solo el kardex lo mueve
    _productoRepository.Update(actualizado);
}

         public void AsignarCategoria(int productoId, int categoriaId)
        {
            var producto = _productoRepository.GetById(productoId)
                ?? throw new Exception("Producto no encontrado.");

            if (_categoriaRepository.GetById(categoriaId) == null)
                throw new Exception("La categoría especificada no existe.");

            producto.CategoriaId = categoriaId;
            _productoRepository.Update(producto);
        }

        public void Delete(int id) => _productoRepository.Delete(id);

        public List<ProductoResponseDto> GetByCategoria(int categoriaId) =>
            _productoRepository.GetAll()
                .Where(p => p.CategoriaId == categoriaId)
                .Select(MapToDto)
                .ToList();

        public List<ProductoResponseDto> GetByDisponibilidad(bool disponible) =>
            _productoRepository.GetAll()
                .Where(p => disponible ? p.Stock > 0 : p.Stock == 0)
                .Select(MapToDto)
                .ToList();

        public void ModificarPrecio(int id, decimal nuevoPrecio)
        {
            if (nuevoPrecio <= 0)
                throw new Exception("El precio debe ser mayor que cero.");

            var producto = _productoRepository.GetById(id);
            if (producto == null)
                throw new Exception("Producto no encontrado.");

            producto.Precio = nuevoPrecio;
            _productoRepository.Update(producto);
        }

        public List<ProductoResponseDto> GetBajoStock(int umbral = 5) =>
            _productoRepository.GetAll()
                .Where(p => p.Stock <= umbral)
                .OrderBy(p => p.Stock)
                .Select(MapToDto)
                .ToList();
    }
}