using InventarioApi.Models;

namespace InventarioApi.Services.Interfaces
{
    public interface IProductoService
    {
        List<Producto> GetAll();
        Producto? GetById(int id);
        void Add(Producto producto);
        void Update(Producto producto);
        void Delete(int id);
    }
}