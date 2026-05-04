using InventarioApi.Models;
using InventarioApi.Repository.Interfaces;
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
            var todos = _repository.GetAll();
            usuario.Id = todos.Count > 0 ? todos.Max(u => u.Id) + 1 : 1;

            var emailExiste = todos.Any(u => u.Email == usuario.Email);
            if (emailExiste)
                throw new Exception("Ya existe un usuario con ese email.");

            _repository.Add(usuario);
        }

        public void Update(Usuario usuario)
        {
            // Validar email duplicado excluyendo el propio usuario
            var emailDuplicado = _repository.GetAll()
                .Any(u => u.Email == usuario.Email && u.Id != usuario.Id);
            if (emailDuplicado)
                throw new Exception("Ya existe otro usuario con ese email.");

            _repository.Update(usuario);
        }

        public void Delete(int id) => _repository.Delete(id);
    }
}