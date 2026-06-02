using InventarioApi.Models.DTOs;

namespace InventarioApi.Services.Interfaces
{
    public interface IVentaService
    {
        VentaResponseDto RegistrarVenta(VentaCreateDto dto, int usuarioId);
        VentaResponseDto? GetById(int id);
        List<VentaResponseDto> GetAll();
    }
}