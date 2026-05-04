using InventarioApi.Models;
using InventarioApi.Repository.Interfaces;
using InventarioApi.Services.Interfaces;

namespace InventarioApi.Services.Implementations
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repository;

        public ProductoService(IProductoRepository repository)
        {
            _repository = repository;
        }

        public List<Producto> GetAll() => _repository.GetAll();

        public Producto? GetById(int id) => _repository.GetById(id);

        public void Add(Producto producto)
        {
            // lógica de negocio: generar ID
            var todos = _repository.GetAll();
            producto.Id = todos.Count > 0 ? todos.Max(p => p.Id) + 1 : 1;

            // lógica de negocio: validar SKU duplicado
            var skuExiste = todos.Any(p => p.SKU == producto.SKU);
            if (skuExiste)
                throw new Exception("Ya existe un producto con ese SKU.");

            _repository.Add(producto);
        }

        public void Update(Producto producto) => _repository.Update(producto);

        public void Delete(int id) => _repository.Delete(id);
    }
}