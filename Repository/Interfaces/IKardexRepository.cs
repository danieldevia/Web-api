using InventarioApi.Models;

namespace InventarioApi.Services.Interfaces
{

    public interface IKardexRepository
    {
        List<Kardex> GetAll();

        List<Kardex> GetByProducto(int productoId);

        Kardex? GetUltimoMovimiento(int productoId);

        void Add(Kardex kardex);
    }
}
