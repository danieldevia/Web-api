using InventarioApi.Models.DTOs;
using InventarioApi.Models;

namespace InventarioApi.Services.Interfaces
{

public interface IKardexService
{
    void RegistrarCompra(CompraDto dto);

    void RegistrarVenta(VentaKardexDto dto);

    List<Kardex> ConsultarKardex(int productoId);
}
}