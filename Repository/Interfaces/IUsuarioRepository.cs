using InventarioApi.Models;

namespace InventarioApi.Repository.Interfaces
{
    public interface IUsuarioRepository
    {
        void Add(Usuario usuario);
        void Update(Usuario usuario);
        void Delete(int id);
        Usuario? GetById(int id);
        List<Usuario> GetAll();
    }
}