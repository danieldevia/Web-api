using InventarioApi.Models;
using InventarioApi.Repositories.Interfaces; // Asegúrate de tener esta carpeta
using InventarioApi.Services.Interfaces;

namespace InventarioApi.Services.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public List<Usuario> GetAll() => _repository.GetAll();

        public Usuario? GetById(int id) => _repository.GetById(id);

        public void Add(Usuario usuario) 
        {
            _repository.Add(usuario);
        }

        public void Update(Usuario usuario) => _repository.Update(usuario);

        public void Delete(int id) => _repository.Delete(id);
    }
}