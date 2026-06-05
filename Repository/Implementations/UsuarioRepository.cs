using InventarioApi.Models;
using InventarioApi.Repository.Interfaces;

namespace InventarioApi.Repository.Implementations
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private static List<Usuario> _usuarios = new List<Usuario>()
        {
            new Usuario { Id = 1, Nombre = "Admin",       Email = "admin@sistema.com",       Password = "123", TipoRol = Models.Enums.Rol.Admin,       IsActivo = true  },
            new Usuario { Id = 4, Nombre = "Inactivo",    Email = "inactivo@sistema.com",    Password = "123", TipoRol = Models.Enums.Rol.Empleado,    IsActivo = false }
        };

        public void Add(Usuario usuario)
        {
            _usuarios.Add(usuario);
        }

        public void Update(Usuario usuario)
        {
            var existente = GetById(usuario.Id);
            if (existente != null)
            {
                existente.Nombre   = usuario.Nombre;
                existente.Email    = usuario.Email;
                existente.Password = usuario.Password;
                existente.TipoRol  = usuario.TipoRol;
                existente.IsActivo = usuario.IsActivo;
            }
        }

        public void Delete(int id)
        {
            var usuario = GetById(id);
            if (usuario != null)
                _usuarios.Remove(usuario);
        }

        public Usuario? GetById(int id)
        {
            return _usuarios.FirstOrDefault(u => u.Id == id);
        }

        public List<Usuario> GetAll()
        {
            return _usuarios;
        }
    }
}