using InventarioApi.Models;
using InventarioApi.Repository.Interfaces;

namespace InventarioApi.Repository.Implementations
{
    public class CategoriaRepository : ICategoriaRepository
    {
       private static List<Categoria> _categorias = new List<Categoria>()
    {
    new Categoria { Id = 1, Nombre = "Café Preparado" },
    new Categoria { Id = 2, Nombre = "Dulces de Café" },
    new Categoria { Id = 3, Nombre = "Arequipes" },
    new Categoria { Id = 4, Nombre = "Galletas" },
    new Categoria { Id = 5, Nombre = "Café en Grano" },
    new Categoria { Id = 6, Nombre = "Café Molido" }
    };

        public List<Categoria> GetAll() => _categorias;

        public Categoria? GetById(int id) =>
            _categorias.FirstOrDefault(c => c.Id == id);

        public void Add(Categoria categoria) =>
            _categorias.Add(categoria);

        public void Delete(int id)
        {
            var categoria = GetById(id);
            if (categoria != null)
                _categorias.Remove(categoria);
        }
    }
}