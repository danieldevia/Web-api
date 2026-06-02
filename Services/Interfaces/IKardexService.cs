using InventarioApi.Models;
using InventarioApi.Models.DTOs;

namespace InventarioApi.Services.Interfaces
{
    public interface IKardexService
{
    void RegistrarCompra(CompraDto dto);
    void RegistrarAjuste(AjusteDto dto);
    void RegistrarDevolucion(DevolucionDto dto);
    void RegistrarVenta(int productoId, int cantidad, string observacion = "");

    List<KardexMovimientoDto> ConsultarKardex(int productoId);
    List<KardexMovimientoDto> ConsultarTodos();
    KardexMovimientoDto? UltimoMovimiento(int productoId);
    decimal GetCostoPromedio(int productoId);
    int GetStockActual(int productoId);
    decimal GetValorTotalInventario(int productoId);
    KardexResumenDto GetResumenKardex(int productoId);
}
}