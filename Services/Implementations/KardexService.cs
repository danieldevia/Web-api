using InventarioApi.Models;
using InventarioApi.Models.DTOs;
using InventarioApi.Repository.Interfaces;
using InventarioApi.Services.Interfaces;
using Mapster;

namespace InventarioApi.Services.Implementations
{
    public class KardexService : IKardexService
    {
        private readonly IKardexRepository   _kardexRepository;
        private readonly IProductoRepository _productoRepository;

        public KardexService(
            IKardexRepository   kardexRepository,
            IProductoRepository productoRepository)
        {
            _kardexRepository   = kardexRepository;
            _productoRepository = productoRepository;
        }

        // ── Helper ID ─────────────────────────────────────────────────────────
        private int NuevoId()
        {
            var todos = _kardexRepository.GetAll();
            return todos.Count > 0 ? todos.Max(k => k.Id) + 1 : 1;
        }

        // ── Compra ────────────────────────────────────────────────────────────
        public void RegistrarCompra(CompraDto dto)
        {
            var producto = _productoRepository.GetById(dto.ProductoId)
                ?? throw new Exception("Producto no encontrado.");

            if (dto.Cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor que cero.");

            if (dto.CostoUnitario <= 0)
                throw new Exception("El costo unitario debe ser mayor que cero.");

            var ultimo            = _kardexRepository.GetUltimoMovimiento(dto.ProductoId);
            var saldoCantAnterior = ultimo?.SaldoCantidad ?? producto.Stock;
            var saldoValAnterior  = ultimo?.SaldoValor    ?? 0;

            var costoTotalEntrada  = dto.Cantidad * dto.CostoUnitario;
            var nuevoSaldoCant     = saldoCantAnterior + dto.Cantidad;
            var nuevoCostoPromedio = (saldoValAnterior + costoTotalEntrada) / nuevoSaldoCant;
            var nuevoSaldoValor    = nuevoSaldoCant * nuevoCostoPromedio;

            _kardexRepository.Add(new Kardex
            {
                Id                   = NuevoId(),
                ProductoId           = dto.ProductoId,
                Fecha                = DateTime.Now,
                TipoMovimiento       = "Compra",
                CantidadEntrada      = dto.Cantidad,
                CostoUnitarioEntrada = dto.CostoUnitario,
                CostoTotalEntrada    = costoTotalEntrada,
                CantidadSalida       = 0,
                CostoUnitarioSalida  = 0,
                CostoTotalSalida     = 0,
                PrecioVenta          = 0,
                Utilidad             = 0,
                CostoPromedio        = Math.Round(nuevoCostoPromedio, 2),
                SaldoCantidad        = nuevoSaldoCant,
                SaldoValor           = Math.Round(nuevoSaldoValor, 2),
                Observacion          = dto.Observacion
            });

            producto.Stock = nuevoSaldoCant;
            _productoRepository.Update(producto);
        }

        // ── Venta (solo llamado desde VentaService) ───────────────────────────
        public void RegistrarVenta(int productoId, int cantidad, int ventaId, string observacion = "")
        {
            var producto = _productoRepository.GetById(productoId)
                ?? throw new Exception("Producto no encontrado.");

            if (cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor que cero.");

            if (producto.Stock < cantidad)
                throw new Exception($"Stock insuficiente. Stock actual: {producto.Stock}.");

            var ultimo = _kardexRepository.GetUltimoMovimiento(productoId)
                ?? throw new Exception("El producto no tiene movimientos de compra registrados.");

            var costoPromedio    = ultimo.CostoPromedio;
            var costoTotalSalida = cantidad * costoPromedio;
            var nuevoSaldoCant   = ultimo.SaldoCantidad - cantidad;
            var nuevoSaldoValor  = nuevoSaldoCant * costoPromedio;
            var utilidad         = (producto.Precio - costoPromedio) * cantidad;
            var ingresoTotal     = producto.Precio * cantidad;

            _kardexRepository.Add(new Kardex
            {
                Id                   = NuevoId(),
                ProductoId           = productoId,
                Fecha                = DateTime.Now,
                TipoMovimiento       = "Venta",
                VentaId              = ventaId,
                CantidadEntrada      = 0,
                CostoUnitarioEntrada = 0,
                CostoTotalEntrada    = 0,
                CantidadSalida       = cantidad,
                CostoUnitarioSalida  = costoPromedio,
                CostoTotalSalida     = Math.Round(costoTotalSalida, 2),
                PrecioVenta          = producto.Precio,
                Utilidad             = Math.Round(utilidad, 2),
                IngresoTotal         = Math.Round(ingresoTotal, 2),
                CostoPromedio        = costoPromedio,
                SaldoCantidad        = nuevoSaldoCant,
                SaldoValor           = Math.Round(nuevoSaldoValor, 2),
                Observacion          = observacion
            });

            producto.Stock = nuevoSaldoCant;
            _productoRepository.Update(producto);
        }

        // ── Ajuste ────────────────────────────────────────────────────────────
        public void RegistrarAjuste(AjusteDto dto)
        {
            var producto = _productoRepository.GetById(dto.ProductoId)
                ?? throw new Exception("Producto no encontrado.");

            if (dto.Cantidad == 0)
                throw new Exception("La cantidad no puede ser cero.");

            var ultimo            = _kardexRepository.GetUltimoMovimiento(dto.ProductoId);
            var saldoCantAnterior = ultimo?.SaldoCantidad ?? producto.Stock;
            var saldoValAnterior  = ultimo?.SaldoValor    ?? 0;
            var costoPromedio     = ultimo?.CostoPromedio ?? 0;

            var nuevoSaldoCant = saldoCantAnterior + dto.Cantidad;
            if (nuevoSaldoCant < 0)
                throw new Exception("El ajuste dejaría el stock en negativo.");

            decimal nuevoCostoPromedio;
            decimal nuevoSaldoValor;

            if (dto.Cantidad > 0)
            {
                // Ajuste positivo: recalcula costo promedio
                var costoTotalEntrada = dto.Cantidad * costoPromedio;
                nuevoCostoPromedio    = (saldoValAnterior + costoTotalEntrada) / nuevoSaldoCant;
                nuevoSaldoValor       = nuevoSaldoCant * nuevoCostoPromedio;
            }
            else
            {
                // Ajuste negativo: mantiene costo promedio vigente
                nuevoCostoPromedio = costoPromedio;
                nuevoSaldoValor    = nuevoSaldoCant * costoPromedio;
            }

            _kardexRepository.Add(new Kardex
            {
                Id                   = NuevoId(),
                ProductoId           = dto.ProductoId,
                Fecha                = DateTime.Now,
                TipoMovimiento       = "Ajuste",
                CantidadEntrada      = dto.Cantidad > 0 ? dto.Cantidad            : 0,
                CostoUnitarioEntrada = dto.Cantidad > 0 ? costoPromedio           : 0,
                CostoTotalEntrada    = dto.Cantidad > 0 ? dto.Cantidad * costoPromedio : 0,
                CantidadSalida       = dto.Cantidad < 0 ? Math.Abs(dto.Cantidad)  : 0,
                CostoUnitarioSalida  = dto.Cantidad < 0 ? costoPromedio           : 0,
                CostoTotalSalida     = dto.Cantidad < 0 ? Math.Abs(dto.Cantidad) * costoPromedio : 0,
                PrecioVenta          = 0,
                Utilidad             = 0,
                CostoPromedio        = Math.Round(nuevoCostoPromedio, 2),
                SaldoCantidad        = nuevoSaldoCant,
                SaldoValor           = Math.Round(nuevoSaldoValor, 2),
                Observacion          = dto.Motivo
            });

            producto.Stock = nuevoSaldoCant;
            _productoRepository.Update(producto);
        }

        // ── Devolución ────────────────────────────────────────────────────────
        public void RegistrarDevolucion(DevolucionDto dto)
{
    var producto = _productoRepository.GetById(dto.ProductoId)
        ?? throw new Exception("Producto no encontrado.");

    var ultimo = _kardexRepository.GetUltimoMovimiento(dto.ProductoId)
        ?? throw new Exception("El producto no tiene movimientos registrados.");

    // Buscar la venta específica en el kardex
    var ventaKardex = _kardexRepository.GetByProducto(dto.ProductoId)
        .FirstOrDefault(m => m.TipoMovimiento == "Venta" && m.VentaId == dto.VentaId)
        ?? throw new Exception($"No se encontró la venta #{dto.VentaId} para este producto.");

    if (dto.Cantidad > ventaKardex.CantidadSalida)
        throw new Exception($"No puedes devolver más unidades de las vendidas. Vendidas: {ventaKardex.CantidadSalida}.");

    var costoPromedio     = ultimo.CostoPromedio;
    var precioVenta       = ventaKardex.PrecioVenta;  // ← precio de esa venta específica
    var costoTotalEntrada = dto.Cantidad * costoPromedio;
    var nuevoSaldoCant    = ultimo.SaldoCantidad + dto.Cantidad;
    var nuevoSaldoValor   = nuevoSaldoCant * costoPromedio;
    var utilidad          = (precioVenta - costoPromedio) * dto.Cantidad * -1;

    _kardexRepository.Add(new Kardex
    {
        Id                   = NuevoId(),
        ProductoId           = dto.ProductoId,
        Fecha                = DateTime.Now,
        TipoMovimiento       = "Devolucion",
        CantidadEntrada      = dto.Cantidad,
        CostoUnitarioEntrada = costoPromedio,
        CostoTotalEntrada    = Math.Round(costoTotalEntrada, 2),
        CantidadSalida       = 0,
        CostoUnitarioSalida  = 0,
        CostoTotalSalida     = 0,
        PrecioVenta          = precioVenta,
        Utilidad             = Math.Round(utilidad, 2),
        SaldoCantidad        = nuevoSaldoCant,
        CostoPromedio        = costoPromedio,
        SaldoValor           = Math.Round(nuevoSaldoValor, 2),
        Observacion          = $"Devolución venta #{dto.VentaId} - {dto.Observacion}"
    });

    producto.Stock = nuevoSaldoCant;
    _productoRepository.Update(producto);
}

        // ── Consultas ─────────────────────────────────────────────────────────
       public List<KardexMovimientoDto> ConsultarKardex(int productoId)
{
    _ = _productoRepository.GetById(productoId)
        ?? throw new Exception("Producto no encontrado.");

    return _kardexRepository.GetByProducto(productoId).Select(m =>
    {
        var dto = m.Adapt<KardexMovimientoDto>();
        dto.ProductoNombre = _productoRepository.GetById(m.ProductoId)?.Nombre ?? "Sin nombre";
        return dto;
    }).ToList();
}

public List<KardexMovimientoDto> ConsultarTodos()
{
    return _kardexRepository.GetAll().Select(m =>
    {
        var dto = m.Adapt<KardexMovimientoDto>();
        dto.ProductoNombre = _productoRepository.GetById(m.ProductoId)?.Nombre ?? "Sin nombre";
        return dto;
    }).ToList();
}

public KardexMovimientoDto? UltimoMovimiento(int productoId)
{
    var m = _kardexRepository.GetUltimoMovimiento(productoId);
    if (m == null) return null;

    var dto = m.Adapt<KardexMovimientoDto>();
    dto.ProductoNombre = _productoRepository.GetById(m.ProductoId)?.Nombre ?? "Sin nombre";
    return dto;
}

public KardexResumenDto GetResumenKardex(int productoId)
{
    var producto = _productoRepository.GetById(productoId)
        ?? throw new Exception("Producto no encontrado.");

    var movimientos = _kardexRepository.GetByProducto(productoId);
    var ultimo      = movimientos.LastOrDefault();

    var resumen = new KardexResumenDto
    {
        ProductoId             = producto.Id,
        ProductoNombre         = producto.Nombre,
        SKU                    = producto.SKU,
        StockActual            = ultimo?.SaldoCantidad    ?? producto.Stock,
        CostoPromedioPonderado = ultimo?.CostoPromedio    ?? 0,
        ValorTotalInventario   = ultimo?.SaldoValor       ?? 0,
        TotalUnidadesEntradas  = movimientos.Sum(m => m.CantidadEntrada),
        TotalCostoEntradas     = movimientos.Sum(m => m.CostoTotalEntrada),
        TotalUnidadesSalidas   = movimientos.Sum(m => m.CantidadSalida),
        TotalCostoSalidas      = movimientos.Sum(m => m.CostoTotalSalida),
        UtilidadTotal          = movimientos.Sum(m => m.Utilidad),
        TotalIngresos          = movimientos.Sum(m => m.IngresoTotal),
        Movimientos            = movimientos.Select(m =>
        {
            var dto = m.Adapt<KardexMovimientoDto>();
            dto.ProductoNombre = producto.Nombre;
            return dto;
        }).ToList()
    };

    return resumen;
}

        public decimal GetCostoPromedio(int productoId) =>
            _kardexRepository.GetUltimoMovimiento(productoId)?.CostoPromedio ?? 0;

        public int GetStockActual(int productoId) =>
            _kardexRepository.GetUltimoMovimiento(productoId)?.SaldoCantidad
                ?? _productoRepository.GetById(productoId)?.Stock ?? 0;

        public decimal GetValorTotalInventario(int productoId) =>
            _kardexRepository.GetUltimoMovimiento(productoId)?.SaldoValor ?? 0;
      
    }
}