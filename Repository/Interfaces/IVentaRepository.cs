using InventarioApi.Models;

namespace InventarioApi.Repository.Interfaces
{
    public interface IVentaRepository
    {
        void Add(Venta venta);
        Venta? GetById(int id);
        List<Venta> GetAll();
    }
}