using InventarioApi.Models;
using InventarioApi.Repository.Interfaces;

namespace InventarioApi.Repository.Implementations
{
    public class ProductoRepository : IProductoRepository
    {
       private static List<Producto> _productos = new List<Producto>();

        public List<Producto> GetAll()
        {
            return _productos;
        }

        public Producto? GetById(int id)
        {
            return _productos.FirstOrDefault(p => p.Id == id);
        }

        public void Add(Producto producto)
        {
            _productos.Add(producto);
        }

        public void Update(Producto producto)
        {
            var existente = GetById(producto.Id);
            if (existente != null)
            {
                existente.Nombre      = producto.Nombre;
                existente.SKU         = producto.SKU;
                existente.Precio      = producto.Precio;
                existente.Stock       = producto.Stock;
                existente.CategoriaId = producto.CategoriaId;
            }
        }

        public void Delete(int id)
        {
            var producto = GetById(id);
            if (producto != null)
                _productos.Remove(producto);
        }
    }
}