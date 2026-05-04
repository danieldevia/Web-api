using InventarioApi.Models;
using InventarioApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // todos los endpoints requieren token
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // solo Admin puede ver todos los usuarios
        public ActionResult<List<Usuario>> Get()
        {
            return Ok(_usuarioService.GetAll());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")] // solo Admin puede ver un usuario
        public ActionResult<Usuario> Get(int id)
        {
            var usuario = _usuarioService.GetById(id);
            if (usuario == null) return NotFound("Usuario no encontrado.");
            return Ok(usuario);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // solo Admin puede crear usuarios
        public ActionResult Post([FromBody] Usuario usuario)
        {
            _usuarioService.Add(usuario);
            return CreatedAtAction(nameof(Get), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // solo Admin puede editar usuarios
        public ActionResult Put(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id) return BadRequest("El ID no coincide.");
            var existente = _usuarioService.GetById(id);
            if (existente == null) return NotFound("Usuario no encontrado.");
            _usuarioService.Update(usuario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // solo Admin puede eliminar usuarios
        public ActionResult Delete(int id)
        {
            var existente = _usuarioService.GetById(id);
            if (existente == null) return NotFound("Usuario no encontrado.");
            _usuarioService.Delete(id);
            return NoContent();
        }
    }
}