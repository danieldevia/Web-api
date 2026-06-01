using InventarioApi.Models;
using InventarioApi.Repository.Interfaces;

namespace InventarioApi.Repository.Implementations
{
    public class ProductoRepository : IProductoRepository
    {
       private static List<Producto> _productos = new List<Producto>()
    {
    // ── Café Preparado ────────────────────────────────────────────────
    new Producto { Id = 1,  Nombre = "Espresso",               SKU = "CAF-001", Precio = 3500,  Stock = 100, CategoriaId = 1 },
    new Producto { Id = 2,  Nombre = "Cappuccino",             SKU = "CAF-002", Precio = 5000,  Stock = 100, CategoriaId = 1 },
    new Producto { Id = 3,  Nombre = "Café Americano",         SKU = "CAF-003", Precio = 4000,  Stock = 100, CategoriaId = 1 },
    new Producto { Id = 4,  Nombre = "Café con Leche",         SKU = "CAF-004", Precio = 4500,  Stock = 100, CategoriaId = 1 },

    // ── Dulces de Café ────────────────────────────────────────────────
    new Producto { Id = 5,  Nombre = "Trufa de Café",          SKU = "DUL-001", Precio = 2500,  Stock = 50,  CategoriaId = 2 },
    new Producto { Id = 6,  Nombre = "Bombón de Café",         SKU = "DUL-002", Precio = 2000,  Stock = 50,  CategoriaId = 2 },
    new Producto { Id = 7,  Nombre = "Brownie de Café",        SKU = "DUL-003", Precio = 4000,  Stock = 30,  CategoriaId = 2 },

    // ── Arequipes ─────────────────────────────────────────────────────
    new Producto { Id = 8,  Nombre = "Arequipe Natural",       SKU = "ARE-001", Precio = 8000,  Stock = 40,  CategoriaId = 3 },
    new Producto { Id = 9,  Nombre = "Arequipe de Café",       SKU = "ARE-002", Precio = 9000,  Stock = 40,  CategoriaId = 3 },
    new Producto { Id = 10, Nombre = "Arequipe de Chocolate",  SKU = "ARE-003", Precio = 9000,  Stock = 40,  CategoriaId = 3 },
    new Producto { Id = 11, Nombre = "Arequipe de Vainilla",   SKU = "ARE-004", Precio = 8500,  Stock = 40,  CategoriaId = 3 },

    // ── Galletas ──────────────────────────────────────────────────────
    new Producto { Id = 12, Nombre = "Galleta de Café",        SKU = "GAL-001", Precio = 3000,  Stock = 60,  CategoriaId = 4 },
    new Producto { Id = 13, Nombre = "Galleta de Avena",       SKU = "GAL-002", Precio = 2500,  Stock = 60,  CategoriaId = 4 },
    new Producto { Id = 14, Nombre = "Galleta de Chocolate",   SKU = "GAL-003", Precio = 3000,  Stock = 60,  CategoriaId = 4 },

    // ── Café en Grano ─────────────────────────────────────────────────
    new Producto { Id = 15, Nombre = "Café en Grano 250g",     SKU = "GRA-001", Precio = 18000, Stock = 25,  CategoriaId = 5 },
    new Producto { Id = 16, Nombre = "Café en Grano 500g",     SKU = "GRA-002", Precio = 32000, Stock = 25,  CategoriaId = 5 },
    new Producto { Id = 17, Nombre = "Café en Grano 1kg",      SKU = "GRA-003", Precio = 60000, Stock = 15,  CategoriaId = 5 },

    // ── Café Molido ───────────────────────────────────────────────────
    new Producto { Id = 18, Nombre = "Café Molido 250g",       SKU = "MOL-001", Precio = 15000, Stock = 30,  CategoriaId = 6 },
    new Producto { Id = 19, Nombre = "Café Molido 500g",       SKU = "MOL-002", Precio = 28000, Stock = 30,  CategoriaId = 6 },
    new() { Id = 20, Nombre = "Café Molido 1kg",               SKU = "MOL-003", Precio = 52000, Stock = 20,  CategoriaId = 6 }
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