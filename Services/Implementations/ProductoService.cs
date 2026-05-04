using InventarioApi.Models;
using InventarioApi.Models.DTOs;
using InventarioApi.Repository.Interfaces;
using InventarioApi.Services.Interfaces;

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
            var categoria = _categoriaRepository.GetById(p.CategoriaId);
            return new ProductoResponseDto
            {
                Id              = p.Id,
                Nombre          = p.Nombre,
                SKU             = p.SKU,
                Precio          = p.Precio,
                Stock           = p.Stock,
                CategoriaId     = p.CategoriaId,
                CategoriaNombre = categoria?.Nombre ?? "Sin categoría"
            };
        }

        // ── CRUD base ─────────────────────────────────────────────────────────

        public List<ProductoResponseDto> GetAll() =>
            _productoRepository.GetAll().Select(MapToDto).ToList();

        public ProductoResponseDto? GetById(int id)
        {
            var producto = _productoRepository.GetById(id);
            return producto == null ? null : MapToDto(producto);
        }

        public void Add(ProductoCreateDto dto)
        {
            // Validar que la categoría exista
            if (_categoriaRepository.GetById(dto.CategoriaId) == null)
                throw new Exception("La categoría especificada no existe.");

            var todos = _productoRepository.GetAll();

            // Validar SKU duplicado
            if (todos.Any(p => p.SKU == dto.SKU))
                throw new Exception("Ya existe un producto con ese SKU.");

            var nuevo = new Producto
            {
                Id          = todos.Count > 0 ? todos.Max(p => p.Id) + 1 : 1,
                Nombre      = dto.Nombre,
                SKU         = dto.SKU,
                Precio      = dto.Precio,
                Stock       = dto.Stock,
                CategoriaId = dto.CategoriaId
            };

            _productoRepository.Add(nuevo);
        }

        public void Update(ProductoUpdateDto dto)
        {
            var existente = _productoRepository.GetById(dto.Id);
            if (existente == null)
                throw new Exception("Producto no encontrado.");

            // Validar que la categoría exista
            if (_categoriaRepository.GetById(dto.CategoriaId) == null)
                throw new Exception("La categoría especificada no existe.");

            // Validar SKU duplicado excluyendo el propio producto
            var skuDuplicado = _productoRepository.GetAll()
                .Any(p => p.SKU == dto.SKU && p.Id != dto.Id);
            if (skuDuplicado)
                throw new Exception("Ya existe otro producto con ese SKU.");

            var actualizado = new Producto
            {
                Id          = dto.Id,
                Nombre      = dto.Nombre,
                SKU         = dto.SKU,
                Precio      = dto.Precio,
                Stock       = dto.Stock,
                CategoriaId = dto.CategoriaId
            };

            _productoRepository.Update(actualizado);
        }

        public void Delete(int id) => _productoRepository.Delete(id);

        // ── Filtros ───────────────────────────────────────────────────────────

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

        // ── Operaciones especiales ────────────────────────────────────────────

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