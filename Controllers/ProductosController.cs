using InventarioApi.Models;
using InventarioApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // todos los endpoints requieren token
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Empleado")] // los tres roles pueden ver productos
        public ActionResult<List<Producto>> Get()
        {
            return Ok(_productoService.GetAll());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Empleado")] // los tres roles pueden ver un producto
        public ActionResult<Producto> Get(int id)
        {
            var producto = _productoService.GetById(id);
            if (producto == null) return NotFound("Producto no encontrado.");
            return Ok(producto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Empleado")] // Vendedor no puede crear → 403
        public ActionResult Post([FromBody] Producto producto)
        {
            _productoService.Add(producto);
            return CreatedAtAction(nameof(Get), new { id = producto.Id }, producto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Empleado")] // Vendedor no puede editar → 403
        public ActionResult Put(int id, [FromBody] Producto producto)
        {
            if (id != producto.Id) return BadRequest("El ID no coincide.");
            var existente = _productoService.GetById(id);
            if (existente == null) return NotFound("Producto no encontrado.");
            _productoService.Update(producto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // solo Admin puede eliminar → Vendedor y Almacenista reciben 403
        public ActionResult Delete(int id)
        {
            var existente = _productoService.GetById(id);
            if (existente == null) return NotFound("Producto no encontrado.");
            _productoService.Delete(id);
            return NoContent();
        }
    }
}