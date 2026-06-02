using InventarioApi.Models;
using InventarioApi.Repository.Interfaces;

namespace InventarioApi.Repository.Implementations
{
    public class VentaRepository : IVentaRepository
    {
        private static List<Venta> _ventas = new();

        public void Add(Venta venta) =>
            _ventas.Add(venta);

        public Venta? GetById(int id) =>
            _ventas.FirstOrDefault(v => v.Id == id);

        public List<Venta> GetAll() =>
            _ventas.OrderByDescending(v => v.Fecha).ToList();
    }
}