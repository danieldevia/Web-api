using InventarioApi.Models;
using InventarioApi.Models.DTOs;
using InventarioApi.Repository.Interfaces;
using InventarioApi.Services.Interfaces;

namespace InventarioApi.Services.Implementations
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IProductoRepository  _productoRepository;

        public CategoriaService(
            ICategoriaRepository categoriaRepository,
            IProductoRepository  productoRepository)
        {
            _categoriaRepository = categoriaRepository;
            _productoRepository  = productoRepository;
        }

        // 2. Listar categorías con conteo de productos asociados
        public List<CategoriaResponseDto> GetAll()
        {
            var productos = _productoRepository.GetAll();

            return _categoriaRepository.GetAll()
                .Select(c => new CategoriaResponseDto
                {
                    Id             = c.Id,
                    Nombre         = c.Nombre,
                    TotalProductos = productos.Count(p => p.CategoriaId == c.Id)
                })
                .ToList();
        }

        public Categoria? GetById(int id) =>
            _categoriaRepository.GetById(id);

        // 1. Crear categoría
        public void Add(CategoriaCreateDto dto)
        {
            // Validar nombre duplicado
            var existe = _categoriaRepository.GetAll()
                .Any(c => c.Nombre.Equals(dto.Nombre, StringComparison.OrdinalIgnoreCase));
            if (existe)
                throw new Exception("Ya existe una categoría con ese nombre.");

            var todas = _categoriaRepository.GetAll();
            var nueva = new Categoria
            {
                Id     = todas.Count > 0 ? todas.Max(c => c.Id) + 1 : 1,
                Nombre = dto.Nombre
            };

            _categoriaRepository.Add(nueva);
        }

        // 3. Eliminar categoría
        public void Delete(int id)
        {
            // Bloquear si tiene productos asociados
            var tieneProductos = _productoRepository.GetAll()
                .Any(p => p.CategoriaId == id);
            if (tieneProductos)
                throw new Exception("No se puede eliminar la categoría porque tiene productos asociados.");

            _categoriaRepository.Delete(id);
        }
    }
}