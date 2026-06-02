using InventarioApi.Models.DTOs;

namespace InventarioApi.Services.Interfaces
{
    public interface IVentaService
    {
        VentaResponseDto RegistrarVenta(VentaCreateDto dto);
        VentaResponseDto? GetById(int id);
        List<VentaResponseDto> GetAll();
    }
}