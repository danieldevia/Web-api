using InventarioApi.Models.Enums;

namespace InventarioApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Rol TipoRol { get; set; } 
    }
}