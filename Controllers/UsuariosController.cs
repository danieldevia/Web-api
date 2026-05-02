using Microsoft.AspNetCore.Mvc;
using InventarioApi.Models;
using InventarioApi.Services.Interfaces;

namespace InventarioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public ActionResult<List<Usuario>> Get()
        {
            return Ok(_usuarioService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Usuario> Get(int id)
        {
            var usuario = _usuarioService.GetById(id);
            if (usuario == null) return NotFound("Usuario no encontrado");
            return Ok(usuario);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Usuario usuario)
        {
            _usuarioService.Add(usuario);
            return CreatedAtAction(nameof(Get), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id) return BadRequest("El ID no coincide");
            
            var existente = _usuarioService.GetById(id);
            if (existente == null) return NotFound();

            _usuarioService.Update(usuario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existente = _usuarioService.GetById(id);
            if (existente == null) return NotFound();

            _usuarioService.Delete(id);
            return NoContent();
        }
    }
}