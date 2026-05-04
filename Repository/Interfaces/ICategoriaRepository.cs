using InventarioApi.Models;

namespace InventarioApi.Repository.Interfaces
{
    public interface ICategoriaRepository
    {
        List<Categoria> GetAll();
        Categoria? GetById(int id);
        void Add(Categoria categoria);
        void Delete(int id);
    }
}