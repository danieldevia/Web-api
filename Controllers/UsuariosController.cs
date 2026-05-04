using InventarioApi.Models;
using InventarioApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // 1. Crear usuario
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Post([FromBody] Usuario usuario)
        {
            try
            {
                _usuarioService.Add(usuario);
                return CreatedAtAction(nameof(Get), new { id = usuario.Id }, usuario);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message); // 409 si el email ya existe
            }
        }

        // 2. Actualizar usuario
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Put(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id) return BadRequest("El ID no coincide.");

            var existente = _usuarioService.GetById(id);
            if (existente == null) return NotFound("Usuario no encontrado.");

            try
            {
                _usuarioService.Update(usuario);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message); // 409 si el email ya existe
            }
        }

        // 3. Eliminar usuario
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var existente = _usuarioService.GetById(id);
            if (existente == null) return NotFound("Usuario no encontrado.");
            _usuarioService.Delete(id);
            return NoContent();
        }

        // 4. Consultar usuario por ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<Usuario> Get(int id)
        {
            var usuario = _usuarioService.GetById(id);
            if (usuario == null) return NotFound("Usuario no encontrado.");
            return Ok(usuario);
        }

        // 5. Listar usuarios
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<Usuario>> Get()
        {
            return Ok(_usuarioService.GetAll());
        }
    }
}