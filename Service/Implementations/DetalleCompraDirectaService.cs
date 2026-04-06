using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using ReyesAR.Service.Interface;

public class DetalleCompraDirectaService : IDetalleCompraDirectaService
{
    private readonly IDetalleCompraDirectaRepository _repository;

    public DetalleCompraDirectaService(IDetalleCompraDirectaRepository repository)
    {
        _repository = repository;
    }

    // 1. Obtener todos los registros
    public async Task<IEnumerable<DetalleCompraDirectaEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    // 2. Obtener detalles por folio de compra
    public async Task<IEnumerable<DetalleCompraDirectaEntity>> GetByCompraAsync(string claveCompra)
    {
        if (string.IsNullOrWhiteSpace(claveCompra))
            throw new ArgumentException("El folio de la compra es requerido.");

        return await _repository.GetByCompraAsync(claveCompra.Trim());
    }

    // 3. Buscar un registro específico
    public async Task<DetalleCompraDirectaEntity?> GetByIdAsync(string claveCompra, string claveProducto)
    {
        if (string.IsNullOrWhiteSpace(claveCompra) || string.IsNullOrWhiteSpace(claveProducto))
            throw new ArgumentException("Las llaves de búsqueda no pueden estar vacías.");

        return await _repository.GetByIdAsync(claveCompra.Trim(), claveProducto.Trim());
    }

    // 4. Crear registro (Validación + Mapeo)
    public async Task<DetalleCompraDirectaEntity> CreateAsync(DetalleCompraDirectaDTO dto)
    {
        // --- VALIDACIONES DE NULOS Y VACÍOS ---
        if (string.IsNullOrWhiteSpace(dto.claveCompra))
            throw new ArgumentException("La clave de compra es obligatoria.");

        if (string.IsNullOrWhiteSpace(dto.claveProducto))
            throw new ArgumentException("La clave del producto es obligatoria.");

        if (string.IsNullOrWhiteSpace(dto.claveUnidadCompra))
            throw new ArgumentException("La unidad de compra es obligatoria.");

        // --- VALIDACIONES DE NEGOCIO ---
        if (dto.cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero.");

        if (dto.precioUnitario <= 0)
            throw new ArgumentException("El precio unitario debe ser mayor a cero.");

        // --- MAPEO DE DTO A ENTIDAD ---
        var entity = new DetalleCompraDirectaEntity
        {
            claveCompra = dto.claveCompra.Trim(),
            claveProducto = dto.claveProducto.Trim(),
            cantidad = dto.cantidad,
            precioUnitario = dto.precioUnitario,
            impuestos = dto.impuestos?.Trim() ?? "No tiene",
            claveUnidadCompra = dto.claveUnidadCompra.Trim()
        };

        // El Repositorio ejecutará el CALL "insertar_detalle_compra_directa"
        return await _repository.CreateAsync(entity);
    }

    // 5. Actualizar registro
    public async Task UpdateAsync(string claveCompra, string claveProducto, DetalleCompraDirectaDTO dto)
    {
        if (dto.cantidad <= 0 || dto.precioUnitario <= 0)
            throw new ArgumentException("Los valores numéricos deben ser positivos.");

        var entity = new DetalleCompraDirectaEntity
        {
            cantidad = dto.cantidad,
            precioUnitario = dto.precioUnitario,
            impuestos = dto.impuestos?.Trim()
        };

        await _repository.UpdateAsync(claveCompra.Trim(), claveProducto.Trim(), entity);
    }

    // 6. Eliminar registro
    public async Task DeleteAsync(string claveCompra, string claveProducto)
    {
        if (string.IsNullOrWhiteSpace(claveCompra) || string.IsNullOrWhiteSpace(claveProducto))
            throw new ArgumentException("Se requieren las llaves para eliminar el registro.");

        await _repository.DeleteAsync(claveCompra.Trim(), claveProducto.Trim());
    }
}