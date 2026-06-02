using InventarioApi.Models;
using InventarioApi.Repository.Interfaces;

namespace InventarioApi.Repository.Implementations
{
    public class KardexRepository : IKardexRepository
    {
        private static List<Kardex> _kardex = new()
        {
            // ── Café Preparado ─────────────────────────────────────────────
            new Kardex { Id=1,  ProductoId=1,  Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=100, CostoUnitarioEntrada=2000, CostoTotalEntrada=200000, SaldoCantidad=100, CostoPromedio=2000, SaldoValor=200000, Observacion="Stock inicial" },
            new Kardex { Id=2,  ProductoId=2,  Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=100, CostoUnitarioEntrada=3000, CostoTotalEntrada=300000, SaldoCantidad=100, CostoPromedio=3000, SaldoValor=300000, Observacion="Stock inicial" },
            new Kardex { Id=3,  ProductoId=3,  Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=100, CostoUnitarioEntrada=2500, CostoTotalEntrada=250000, SaldoCantidad=100, CostoPromedio=2500, SaldoValor=250000, Observacion="Stock inicial" },
            new Kardex { Id=4,  ProductoId=4,  Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=100, CostoUnitarioEntrada=2800, CostoTotalEntrada=280000, SaldoCantidad=100, CostoPromedio=2800, SaldoValor=280000, Observacion="Stock inicial" },

            // ── Dulces de Café ─────────────────────────────────────────────
            new Kardex { Id=5,  ProductoId=5,  Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=50,  CostoUnitarioEntrada=1500, CostoTotalEntrada=75000,  SaldoCantidad=50,  CostoPromedio=1500, SaldoValor=75000,  Observacion="Stock inicial" },
            new Kardex { Id=6,  ProductoId=6,  Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=50,  CostoUnitarioEntrada=1200, CostoTotalEntrada=60000,  SaldoCantidad=50,  CostoPromedio=1200, SaldoValor=60000,  Observacion="Stock inicial" },
            new Kardex { Id=7,  ProductoId=7,  Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=30,  CostoUnitarioEntrada=2500, CostoTotalEntrada=75000,  SaldoCantidad=30,  CostoPromedio=2500, SaldoValor=75000,  Observacion="Stock inicial" },

            // ── Arequipes ──────────────────────────────────────────────────
            new Kardex { Id=8,  ProductoId=8,  Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=40,  CostoUnitarioEntrada=5000, CostoTotalEntrada=200000, SaldoCantidad=40,  CostoPromedio=5000, SaldoValor=200000, Observacion="Stock inicial" },
            new Kardex { Id=9,  ProductoId=9,  Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=40,  CostoUnitarioEntrada=5500, CostoTotalEntrada=220000, SaldoCantidad=40,  CostoPromedio=5500, SaldoValor=220000, Observacion="Stock inicial" },
            new Kardex { Id=10, ProductoId=10, Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=40,  CostoUnitarioEntrada=5500, CostoTotalEntrada=220000, SaldoCantidad=40,  CostoPromedio=5500, SaldoValor=220000, Observacion="Stock inicial" },
            new Kardex { Id=11, ProductoId=11, Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=40,  CostoUnitarioEntrada=5200, CostoTotalEntrada=208000, SaldoCantidad=40,  CostoPromedio=5200, SaldoValor=208000, Observacion="Stock inicial" },

            // ── Galletas ───────────────────────────────────────────────────
            new Kardex { Id=12, ProductoId=12, Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=60,  CostoUnitarioEntrada=1800, CostoTotalEntrada=108000, SaldoCantidad=60,  CostoPromedio=1800, SaldoValor=108000, Observacion="Stock inicial" },
            new Kardex { Id=13, ProductoId=13, Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=60,  CostoUnitarioEntrada=1500, CostoTotalEntrada=90000,  SaldoCantidad=60,  CostoPromedio=1500, SaldoValor=90000,  Observacion="Stock inicial" },
            new Kardex { Id=14, ProductoId=14, Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=60,  CostoUnitarioEntrada=1800, CostoTotalEntrada=108000, SaldoCantidad=60,  CostoPromedio=1800, SaldoValor=108000, Observacion="Stock inicial" },

            // ── Café en Grano ──────────────────────────────────────────────
            new Kardex { Id=15, ProductoId=15, Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=25,  CostoUnitarioEntrada=12000, CostoTotalEntrada=300000, SaldoCantidad=25,  CostoPromedio=12000, SaldoValor=300000, Observacion="Stock inicial" },
            new Kardex { Id=16, ProductoId=16, Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=25,  CostoUnitarioEntrada=22000, CostoTotalEntrada=550000, SaldoCantidad=25,  CostoPromedio=22000, SaldoValor=550000, Observacion="Stock inicial" },
            new Kardex { Id=17, ProductoId=17, Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=15,  CostoUnitarioEntrada=42000, CostoTotalEntrada=630000, SaldoCantidad=15,  CostoPromedio=42000, SaldoValor=630000, Observacion="Stock inicial" },

            // ── Café Molido ────────────────────────────────────────────────
            new Kardex { Id=18, ProductoId=18, Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=30,  CostoUnitarioEntrada=10000, CostoTotalEntrada=300000, SaldoCantidad=30,  CostoPromedio=10000, SaldoValor=300000, Observacion="Stock inicial" },
            new Kardex { Id=19, ProductoId=19, Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=30,  CostoUnitarioEntrada=19000, CostoTotalEntrada=570000, SaldoCantidad=30,  CostoPromedio=19000, SaldoValor=570000, Observacion="Stock inicial" },
            new Kardex { Id=20, ProductoId=20, Fecha=DateTime.Now, TipoMovimiento="Compra", CantidadEntrada=20,  CostoUnitarioEntrada=36000, CostoTotalEntrada=720000, SaldoCantidad=20,  CostoPromedio=36000, SaldoValor=720000, Observacion="Stock inicial" },
        };

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