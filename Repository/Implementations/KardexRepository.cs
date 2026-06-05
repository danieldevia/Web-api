using InventarioApi.Models;
using InventarioApi.Repository.Interfaces;

namespace InventarioApi.Repository.Implementations
{
    public class KardexRepository : IKardexRepository
    {
        private static List<Kardex> _kardex = new();

        public List<Kardex> GetAll() =>
            _kardex.OrderBy(k => k.Fecha).ToList();

        public List<Kardex> GetByProducto(int productoId) =>
            _kardex
                .Where(k => k.ProductoId == productoId)
                .OrderBy(k => k.Fecha)
                .ToList();

        public Kardex? GetUltimoMovimiento(int productoId) =>
            _kardex
                .Where(k => k.ProductoId == productoId)
                .OrderBy(k => k.Fecha)
                .LastOrDefault();

        public void Add(Kardex kardex) =>
            _kardex.Add(kardex);
    }
}