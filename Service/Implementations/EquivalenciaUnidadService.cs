using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;

public class EquivalenciaUnidadService : IEquivalenciaUnidadService
{
    private readonly IEquivalenciaUnidadRepository _repository;

    public EquivalenciaUnidadService(IEquivalenciaUnidadRepository repository)
    {
        _repository = repository;
    }

    // 1. Obtener todas las equivalencias (Mapeo de Entity a DTO)
    public async Task<IEnumerable<EquivalenciaUnidadDTO>> GetAllEquivalenciasAsync()
    {
        var equivalencias = await _repository.GetAllAsync();
        return equivalencias.Select(e => new EquivalenciaUnidadDTO
        {
            claveProducto = e.claveProducto,
            claveUnidadCompra = e.claveUnidadCompra,
            factor_conversion = e.factor_conversion,
            unidad_base = e.unidad_base.ToString()
        });
    }

    // 2. Obtener una específica por su clave compuesta
    public async Task<EquivalenciaUnidadDTO?> GetEquivalenciaByIdAsync(string claveProducto, string claveUnidadCompra)
    {
        var e = await _repository.GetByIdAsync(claveProducto, claveUnidadCompra);
        if (e == null) return null;

        return new EquivalenciaUnidadDTO
        {
            claveProducto = e.claveProducto,
            claveUnidadCompra = e.claveUnidadCompra,
            factor_conversion = e.factor_conversion,
            unidad_base = e.unidad_base.ToString()
        };
    }

    // 3. CREAR: Mapeo directo de DTO a Entity
        public async Task<EquivalenciaUnidadDTO> CreateEquivalenciaAsync(EquivalenciaUnidadDTO dto)
    {
        // Mapeo directo sin conversiones de Enum innecesarias
        var entidad = new EquivalenciaUnidadEntity
        {
            claveProducto = dto.claveProducto.Trim(),
            claveUnidadCompra = dto.claveUnidadCompra.Trim(),
            factor_conversion = dto.factor_conversion,
            unidad_base = dto.unidad_base.Trim() // Pasa como string/varchar directo
        };

        try
        {
            // El Repo se encarga del CALL insertar_equivalencia_unidad
            var resultado = await _repository.CreateAsync(entidad);

            return new EquivalenciaUnidadDTO
            {
                claveProducto = resultado.claveProducto,
                claveUnidadCompra = resultado.claveUnidadCompra,
                factor_conversion = resultado.factor_conversion,
                unidad_base = resultado.unidad_base
            };
        }
        catch (Exception ex)
        {
            // Aquí capturamos los RAISE EXCEPTION del Store Procedure de Postgres
            throw new InvalidOperationException(ex.Message);
        }
    }

    // 4. Actualizar el factor de conversión
    public async Task UpdateEquivalenciaAsync(string claveProducto, string claveUnidadCompra, EquivalenciaUnidadDTO dto)
    {
        if (dto.factor_conversion <= 0)
            throw new ArgumentException("El nuevo factor de conversión debe ser mayor a 0.");

        await _repository.UpdateAsync(claveProducto, claveUnidadCompra, dto);
    }

    // 5. Eliminar la relación
    public async Task DeleteEquivalenciaAsync(string claveProducto, string claveUnidadCompra)
    {
        await _repository.DeleteAsync(claveProducto, claveUnidadCompra);
    }
}