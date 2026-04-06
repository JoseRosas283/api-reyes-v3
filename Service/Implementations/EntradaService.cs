using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using ReyesAR.Service.Interface;

public class EntradaService : IEntradaService
{
    private readonly IEntradaRepository _repository;

    public EntradaService(IEntradaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<EntradaEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IEnumerable<EntradaEntity>> GetByCompraAsync(string claveCompra)
    {
        if (string.IsNullOrWhiteSpace(claveCompra))
            throw new ArgumentException("La clave de compra es necesaria para el filtro.");

        return await _repository.GetByCompraAsync(claveCompra.Trim());
    }

    public async Task<EntradaEntity?> GetByIdAsync(string claveEntrada)
    {
        if (string.IsNullOrWhiteSpace(claveEntrada))
            throw new ArgumentException("El folio de entrada es obligatorio.");

        return await _repository.GetByIdAsync(claveEntrada.Trim());
    }

    public async Task<EntradaEntity> CreateAsync(EntradaDTO dto)
    {
        // --- 1. VALIDACIONES DE CAMPOS OBLIGATORIOS ---
        if (dto.cantidad_recibida <= 0)
            throw new ArgumentException("La cantidad recibida debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(dto.tipo_entrada))
            throw new ArgumentException("El tipo de entrada es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.claveProducto))
            throw new ArgumentException("La clave del producto es obligatoria.");

        if (string.IsNullOrWhiteSpace(dto.claveCompra))
            throw new ArgumentException("La clave de la compra es obligatoria.");

        if (string.IsNullOrWhiteSpace(dto.claveUsuario))
            throw new ArgumentException("El usuario que registra la entrada es obligatorio.");

        // --- 2. VALIDACIÓN DE FECHAS ---
        // La fecha de entrada no puede ser en el futuro (opcional, según tu lógica)
        if (dto.fecha > DateTime.Now)
            dto.fecha = DateTime.Now;

        // --- 3. MAPEO DE DTO A ENTIDAD ---
        var entity = new EntradaEntity
        {
            cantidad_recibida = dto.cantidad_recibida,
            fecha = dto.fecha,
            tipo_entrada = dto.tipo_entrada.Trim(),
            claveProducto = dto.claveProducto.Trim(),
            claveCompra = dto.claveCompra.Trim(),
            claveUsuario = dto.claveUsuario.Trim(),
            fecha_caducidad = dto.fecha_caducidad,
            observaciones = dto.observaciones?.Trim()
        };

        // --- 4. LLAMADA AL REPOSITORIO (EJECUCIÓN DEL SP) ---
        // El Repositorio lanzará excepciones si el SP detecta:
        // - Que el usuario no es el dueño de la compra.
        // - Que el producto requiere caducidad y no se envió.
        // - Que la cantidad no coincide con el detalle.
        return await _repository.CreateAsync(entity);
    }

    public async Task DeleteAsync(string claveEntrada)
    {
        if (string.IsNullOrWhiteSpace(claveEntrada))
            throw new ArgumentException("Se requiere el folio para eliminar la entrada.");

        await _repository.DeleteAsync(claveEntrada.Trim());
    }
}