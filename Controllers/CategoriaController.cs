using InventarioApi.Models.DTOs;
using InventarioApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // 1. Crear categoría
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Post([FromBody] CategoriaCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest("El nombre de la categoría es requerido.");

            try
            {
                _categoriaService.Add(dto);
                return StatusCode(201, "Categoría creada correctamente.");
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message); // 409 si el nombre ya existe
            }
        }

        // 2. Listar categorías
        [HttpGet]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult<List<CategoriaResponseDto>> Get()
        {
            return Ok(_categoriaService.GetAll());
        }

        // 3. Eliminar categoría
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var existente = _categoriaService.GetById(id);
            if (existente == null) return NotFound("Categoría no encontrada.");

            try
            {
                _categoriaService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message); // 409 si tiene productos asociados
            }
        }

        // 4. consultar productos de una categoría
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult<CategoriaResponseDto> GetById(int id)
        {
            var existente = _categoriaService.GetById(id);
            if (existente == null) return NotFound("Categoría no encontrada.");

            var dto = _categoriaService.GetAll().FirstOrDefault(c => c.Id == id);
            return Ok(dto);
        }
    }
}