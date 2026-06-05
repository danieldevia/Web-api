using InventarioApi.Models;
using InventarioApi.Models.DTOs;
using InventarioApi.Repository.Interfaces;
using InventarioApi.Services.Interfaces;

namespace InventarioApi.Services.Implementations
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository    _ventaRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IKardexService      _kardexService;
        private readonly IUsuarioRepository  _usuarioRepository; 

        public VentaService(
            IVentaRepository    ventaRepository,
            IProductoRepository productoRepository,
            IKardexService      kardexService,
            IUsuarioRepository  usuarioRepository)
        {
            _ventaRepository    = ventaRepository;
            _productoRepository = productoRepository;
            _kardexService      = kardexService;
            _usuarioRepository  = usuarioRepository;
        }

        public VentaResponseDto RegistrarVenta(VentaCreateDto dto, int UsuarioId)
        {
            if (dto.Detalles == null || dto.Detalles.Count == 0)
                throw new Exception("La venta debe tener al menos un producto.");

            // Validar stock de todos antes de registrar nada
            foreach (var item in dto.Detalles)
            {
                var prod = _productoRepository.GetById(item.ProductoId)
                    ?? throw new Exception($"Producto {item.ProductoId} no encontrado.");

                if (item.Cantidad <= 0)
                    throw new Exception($"La cantidad de {prod.Nombre} debe ser mayor que cero.");

                if (prod.Stock < item.Cantidad)
                    throw new Exception($"Stock insuficiente para {prod.Nombre}. Stock actual: {prod.Stock}.");
            }

            // Construir venta
            var todas = _ventaRepository.GetAll();
            var venta = new Venta
            {
                Id        = todas.Count > 0 ? todas.Max(v => v.Id) + 1 : 1,
                Fecha     = DateTime.Now,
                UsuarioId = UsuarioId,
                Detalles  = new List<DetalleVenta>()
            };

            decimal total = 0;
            var detallesResponse = new List<DetalleVentaResponseDto>();

            foreach (var item in dto.Detalles)
            {
                var producto      = _productoRepository.GetById(item.ProductoId)!;
                var costoPromedio = _kardexService.GetCostoPromedio(item.ProductoId);
                var subtotal      = producto.Precio * item.Cantidad;
                var utilidad      = (producto.Precio - costoPromedio) * item.Cantidad;

                venta.Detalles.Add(new DetalleVenta
                {
                    Id             = item.ProductoId,
                    VentaId        = venta.Id,
                    ProductoId     = item.ProductoId,
                    Cantidad       = item.Cantidad,
                    PrecioUnitario = producto.Precio,
                    Subtotal       = subtotal
                });

                // Mueve el kardex automáticamente
                _kardexService.RegistrarVenta(item.ProductoId, item.Cantidad, venta.Id,
                    $"Venta #{venta.Id}");

                total += subtotal;

                detallesResponse.Add(new DetalleVentaResponseDto
                {
                    ProductoId     = item.ProductoId,
                    ProductoNombre = producto.Nombre,
                    Cantidad       = item.Cantidad,
                    PrecioUnitario = producto.Precio,
                    CostoUnitario  = costoPromedio,
                    Utilidad       = Math.Round(utilidad, 2),
                    Subtotal       = subtotal
                });
            }

            venta.Total = total;
            _ventaRepository.Add(venta);

            return new VentaResponseDto
            {
                Id        = venta.Id,
                Fecha     = venta.Fecha,
                UsuarioId = venta.UsuarioId,
                UsuarioNombre = _usuarioRepository.GetById(UsuarioId)?.Nombre ?? "Sin nombre",
                Total     = total,
                Detalles  = detallesResponse
            };
        }

        public VentaResponseDto? GetById(int id)
        {
            var venta = _ventaRepository.GetById(id);
            if (venta == null) return null;

            return new VentaResponseDto
            {
                Id        = venta.Id,
                Fecha     = venta.Fecha,
                UsuarioId = venta.UsuarioId,
                UsuarioNombre = _usuarioRepository.GetById(venta.UsuarioId)?.Nombre ?? "Sin nombre",
                Total     = venta.Total,
                Detalles  = venta.Detalles.Select(d =>
                {
                    var producto = _productoRepository.GetById(d.ProductoId);
                    return new DetalleVentaResponseDto
                    {
                        ProductoId     = d.ProductoId,
                        ProductoNombre = producto?.Nombre ?? "Sin nombre",
                        Cantidad       = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        CostoUnitario  = _kardexService.GetCostoPromedio(d.ProductoId),
                        Utilidad       = Math.Round((d.PrecioUnitario - _kardexService.GetCostoPromedio(d.ProductoId)) * d.Cantidad, 2),
                        Subtotal       = d.Subtotal
                    };
                }).ToList()
            };
        }

        public List<VentaResponseDto> GetAll() =>
            _ventaRepository.GetAll()
                .Select(v => GetById(v.Id)!)
                .ToList();
    }
}