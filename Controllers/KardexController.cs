using InventarioApi.Models.DTOs;
using InventarioApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class KardexController : ControllerBase
    {
        private readonly IKardexService _kardexService;

        public KardexController(IKardexService kardexService)
        {
            _kardexService = kardexService;
        }

        // 1. Registrar compra (entrada)
        [HttpPost("compra")]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult RegistrarCompra([FromBody] CompraDto dto)
        {
            if (dto.Cantidad <= 0)
                return BadRequest("La cantidad debe ser mayor que cero.");

            if (dto.CostoUnitario <= 0)
                return BadRequest("El costo unitario debe ser mayor que cero.");

            try
            {
                _kardexService.RegistrarCompra(dto);
                return StatusCode(201, "Compra registrada correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 2. Registrar ajuste
        [HttpPost("ajuste")]
        [Authorize(Roles = "Admin")]
        public ActionResult RegistrarAjuste([FromBody] AjusteDto dto)
        {
            if (dto.Cantidad == 0)
                return BadRequest("La cantidad no puede ser cero.");

            if (string.IsNullOrWhiteSpace(dto.Motivo))
                return BadRequest("El motivo del ajuste es requerido.");

            try
            {
                _kardexService.RegistrarAjuste(dto);
                return StatusCode(201, "Ajuste registrado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 3. Registrar devolución
        [HttpPost("devolucion")]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult RegistrarDevolucion([FromBody] DevolucionDto dto)
        {
            if (dto.Cantidad <= 0)
                return BadRequest("La cantidad debe ser mayor que cero.");

            if (dto.CostoUnitario <= 0)
                return BadRequest("El costo unitario debe ser mayor que cero.");

            try
            {
                _kardexService.RegistrarDevolucion(dto);
                return StatusCode(201, "Devolución registrada correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 4. Resumen completo del kardex (endpoint principal del parcial)
        [HttpGet("{productoId}/resumen")]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult GetResumen(int productoId)
        {
            try
            {
                var resumen = _kardexService.GetResumenKardex(productoId);
                return Ok(resumen);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // 5. Movimientos línea por línea
        [HttpGet("{productoId}")]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult ConsultarKardex(int productoId)
        {
            try
            {
                var kardex = _kardexService.ConsultarKardex(productoId);
                return Ok(kardex);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // 6. Todos los movimientos
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult ConsultarTodos()
        {
            return Ok(_kardexService.ConsultarTodos());
        }

        // 7. Último movimiento / saldo actual
        [HttpGet("{productoId}/ultimo")]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult UltimoMovimiento(int productoId)
        {
            var ultimo = _kardexService.UltimoMovimiento(productoId);
            if (ultimo == null) return NotFound("No hay movimientos para este producto.");
            return Ok(ultimo);
        }

        // 8. Costo promedio ponderado vigente
        [HttpGet("{productoId}/costo-promedio")]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult GetCostoPromedio(int productoId)
        {
            return Ok(new { CostoPromedioPonderado = _kardexService.GetCostoPromedio(productoId) });
        }

        // 9. Valor total del inventario
        [HttpGet("{productoId}/valor-inventario")]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult GetValorInventario(int productoId)
        {
            return Ok(new { ValorTotalInventario = _kardexService.GetValorTotalInventario(productoId) });
        }

        // 10. Stock actual según kardex
        [HttpGet("{productoId}/stock")]
        [Authorize(Roles = "Admin, Empleado")]
        public ActionResult GetStock(int productoId)
        {
            try
            {
                var stock = _kardexService.GetStockActual(productoId);
                return Ok(new { ProductoId = productoId, StockActual = stock });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}