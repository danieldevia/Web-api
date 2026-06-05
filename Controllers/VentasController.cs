using InventarioApi.Models.DTOs;
using InventarioApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VentasController : ControllerBase
    {
        private readonly IVentaService _ventaService;

        public VentasController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }

        // 1. Registrar venta (mueve el kardex automáticamente)
        [HttpPost]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult<VentaResponseDto> Post([FromBody] VentaCreateDto dto)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
                var venta = _ventaService.RegistrarVenta(dto, usuarioId);
                return StatusCode(201, venta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 2. Consultar venta por ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult<VentaResponseDto> GetById(int id)
        {
            var venta = _ventaService.GetById(id);
            if (venta == null) return NotFound("Venta no encontrada.");
            return Ok(venta);
        }

        // 3. Listar todas las ventas
        [HttpGet]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult<List<VentaResponseDto>> GetAll()
        {
            return Ok(_ventaService.GetAll());
        }
    }
}