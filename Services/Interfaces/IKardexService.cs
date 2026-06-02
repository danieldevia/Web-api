using InventarioApi.Models;
using InventarioApi.Models.DTOs;

namespace InventarioApi.Services.Interfaces
{
    public interface IKardexService
    {
        // Movimientos externos (tienen endpoint en KardexController)
        void RegistrarCompra(CompraDto dto);
        void RegistrarAjuste(AjusteDto dto);
        void RegistrarDevolucion(DevolucionDto dto);

        // Movimiento interno (solo lo llama VentaService)
        void RegistrarVenta(int productoId, int cantidad, string observacion = "");

        // Consultas
        List<Kardex> ConsultarKardex(int productoId);
        List<Kardex> ConsultarTodos();
        Kardex? UltimoMovimiento(int productoId);
        decimal GetCostoPromedio(int productoId);
        int GetStockActual(int productoId);
        decimal GetValorTotalInventario(int productoId);
        KardexResumenDto GetResumenKardex(int productoId);
    }
}