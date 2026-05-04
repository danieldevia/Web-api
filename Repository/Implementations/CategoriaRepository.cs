using InventarioApi.Models;
using InventarioApi.Repository.Interfaces;

namespace InventarioApi.Repository.Implementations
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private static List<Categoria> _categorias = new List<Categoria>()
        {
            new Categoria { Id = 1, Nombre = "Ropa" },
            new Categoria { Id = 2, Nombre = "Accesorios" }
        };

        // Solo trae todas las categorías, sin lógica
        public List<Categoria> GetAll() => _categorias;

        // Solo busca por ID, sin lógica
        public Categoria? GetById(int id) =>
            _categorias.FirstOrDefault(c => c.Id == id);

        // Solo guarda, sin lógica
        public void Add(Categoria categoria) =>
            _categorias.Add(categoria);

        // Solo elimina, sin lógica
        public void Delete(int id)
        {
            var categoria = GetById(id);
            if (categoria != null)
                _categorias.Remove(categoria);
        }
    }
}