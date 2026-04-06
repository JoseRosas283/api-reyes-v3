using ReyesAR.DTO;
using ReyesAR.Repositories.Interface;
using ReyesAR.Service.Interface;

public class DetalleEntregaService : IDetalleEntregaService
{
    private readonly IDetalleEntregaRepository _repository;

    public DetalleEntregaService(IDetalleEntregaRepository repository)
    {
        _repository = repository;
    }

    public async Task<DetalleEntregaEntity> CreateAsync(DetalleEntregaDTO dto)
    {
        // 1. VALIDACIÓN DE NULOS Y CADENAS VACÍAS (Requisitos del SP)
        if (string.IsNullOrWhiteSpace(dto.claveEntrega))
            throw new ArgumentException("La clave de entrega es obligatoria.");

        if (string.IsNullOrWhiteSpace(dto.claveProducto))
            throw new ArgumentException("La clave de producto es obligatoria.");

        if (string.IsNullOrWhiteSpace(dto.claveCompra))
            throw new ArgumentException("La clave de compra es obligatoria.");

        if (string.IsNullOrWhiteSpace(dto.claveUnidadCompra))
            throw new ArgumentException("La unidad de compra es obligatoria.");

        if (string.IsNullOrWhiteSpace(dto.tipo_detalle))
            throw new ArgumentException("El tipo de detalle (Pedido/Promocion) es obligatorio.");

        // 2. VALIDACIÓN DE VALORES NUMÉRICOS
        if (dto.cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero.");

        // Regla de negocio: Si es regalo, el precio DEBE ser 0. Si no, DEBE ser mayor a 0.
        if (dto.tipo_detalle == "PromocionRegalada" && dto.precioUnitario != 0)
            throw new ArgumentException("Las promociones regaladas deben tener precio 0.");

        if (dto.tipo_detalle != "PromocionRegalada" && dto.precioUnitario <= 0)
            throw new ArgumentException("El precio unitario debe ser mayor a 0 para este tipo de detalle.");

        // 3. MAPEO A ENTIDAD
        var entity = new DetalleEntregaEntity
        {
            claveEntrega = dto.claveEntrega.Trim(),
            claveProducto = dto.claveProducto.Trim(),
            claveCompra = dto.claveCompra.Trim(),
            claveUnidadCompra = dto.claveUnidadCompra.Trim(),
            tipo_detalle = dto.tipo_detalle.Trim(),
            cantidad = dto.cantidad,
            precioUnitario = dto.precioUnitario,
            observaciones = dto.observaciones?.Trim() ?? string.Empty,
            subtotal = dto.cantidad * dto.precioUnitario // Cálculo preventivo
        };

        // 4. PERSISTENCIA (Aquí el SP hará las validaciones de FK y existencias)
        return await _repository.CreateAsync(entity);
    }

    public async Task UpdateAsync(string claveEntrega, string claveProducto, string tipo_detalle, bool extra, DetalleEntregaDTO dto)
    {
        // Validaciones básicas para actualización
        if (dto.cantidad <= 0)
            throw new ArgumentException("La cantidad actualizada debe ser mayor a cero.");

        var entity = new DetalleEntregaEntity
        {
            cantidad = dto.cantidad,
            precioUnitario = dto.precioUnitario,
            observaciones = dto.observaciones?.Trim()
        };

        await _repository.UpdateAsync(claveEntrega, claveProducto, tipo_detalle, extra, entity);
    }

    // --- MÉTODOS DE CONSULTA (Simples pasarelas al Repositorio) ---

    public async Task<IEnumerable<DetalleEntregaEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IEnumerable<DetalleEntregaEntity>> GetDetailedListAsync(string claveEntrega)
    {
        if (string.IsNullOrWhiteSpace(claveEntrega))
            throw new ArgumentException("Se requiere un folio de entrega para listar los detalles.");

        return await _repository.GetDetailedListAsync(claveEntrega);
    }

    public async Task<DetalleEntregaEntity?> GetByIdAsync(string claveEntrega, string claveProducto, string tipo_detalle, bool extra)
    {
        return await _repository.GetByIdAsync(claveEntrega, claveProducto, tipo_detalle, extra);
    }

    public async Task DeleteAsync(string claveEntrega, string claveProducto, string tipo_detalle, bool extra)
    {
        await _repository.DeleteAsync(claveEntrega, claveProducto, tipo_detalle, extra);
    }
}
