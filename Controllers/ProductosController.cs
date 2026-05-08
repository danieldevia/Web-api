using InventarioApi.Models.DTOs;
using InventarioApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        // 1. Crear producto
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Post([FromBody] ProductoCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.SKU))
                return BadRequest("Nombre y SKU son requeridos.");

            if (dto.Precio <= 0)
                return BadRequest("El precio debe ser mayor que cero.");

            try
            {
                _productoService.Add(dto);
                return StatusCode(201, "Producto creado correctamente.");
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        // 2. Actualizar producto
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Put(int id, [FromBody] ProductoUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest("El ID de la ruta no coincide con el del cuerpo.");

            try
            {
                _productoService.Update(dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // 3. Eliminar producto
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var existente = _productoService.GetById(id);
            if (existente == null) return NotFound("Producto no encontrado.");
            _productoService.Delete(id);
            return NoContent();
        }

        // 4. Consultar producto por ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult<ProductoResponseDto> GetById(int id)
        {
            var producto = _productoService.GetById(id);
            if (producto == null) return NotFound("Producto no encontrado.");
            return Ok(producto);
        }

        // 5. Listar productos
        [HttpGet]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult<List<ProductoResponseDto>> Get()
        {
            return Ok(_productoService.GetAll());
        }

        // 6. Filtrar por categoría
        [HttpGet("categoria/{categoriaId}")]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult<List<ProductoResponseDto>> GetByCategoria(int categoriaId)
        {
            return Ok(_productoService.GetByCategoria(categoriaId));
        }

        // 7. Filtrar por disponibilidad
        [HttpGet("disponibles")]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult<List<ProductoResponseDto>> GetDisponibles()
        {
            return Ok(_productoService.GetByDisponibilidad(true));
        }

        [HttpGet("no-disponibles")]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult<List<ProductoResponseDto>> GetNoDisponibles()
        {
            return Ok(_productoService.GetByDisponibilidad(false));
        }

        // 8. Modificar precio
        [HttpPatch("{id}/precio")]
        [Authorize(Roles = "Admin")]
        public ActionResult PatchPrecio(int id, [FromBody] ProductoPrecioDto dto)
        {
            if (dto.NuevoPrecio <= 0)
                return BadRequest("El precio debe ser mayor que cero.");

            try
            {
                _productoService.ModificarPrecio(id, dto.NuevoPrecio);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // 9. Consultar productos con bajo stock
        [HttpGet("bajo-stock/{umbral}")]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult<List<ProductoResponseDto>> GetBajoStock(int umbral = 5)
        {
            if (umbral < 0) return BadRequest("El umbral no puede ser negativo.");
            return Ok(_productoService.GetBajoStock(umbral));
        }
    }
}