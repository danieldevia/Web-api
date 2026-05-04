using InventarioApi.Models;
using InventarioApi.Repository.Interfaces;

namespace InventarioApi.Repository.Implementations
{
    public class ProductoRepository : IProductoRepository
    {
        private static List<Producto> _productos = new List<Producto>()
        {
            new Producto { Id = 1, Nombre = "Camisa",    SKU = "CAM-001", Precio = 25.000m, Stock = 50,  CategoriaId = 1 },
            new Producto { Id = 2, Nombre = "Pantalon",  SKU = "PAN-001", Precio = 45.000m, Stock = 30,  CategoriaId = 1 },
            new Producto { Id = 3, Nombre = "Zapatos",   SKU = "ZAP-001", Precio = 80.000m, Stock = 20,  CategoriaId = 2 },
            new Producto { Id = 4, Nombre = "Bolso",     SKU = "BOL-001", Precio = 60.000m, Stock = 15,  CategoriaId = 2 }
        };

        // Solo trae todos los productos, sin lógica
        public List<Producto> GetAll()
        {
            return _productos;
        }

        // Solo busca por ID, sin lógica
        public Producto? GetById(int id)
        {
            return _productos.FirstOrDefault(p => p.Id == id);
        }

        // Solo guarda, sin calcular ni validar
        public void Add(Producto producto)
        {
            _productos.Add(producto);
        }

        // Solo actualiza los campos, sin validar
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

        // Solo elimina, sin validar
        public void Delete(int id)
        {
            var producto = GetById(id);
            if (producto != null)
                _productos.Remove(producto);
        }
    }
}