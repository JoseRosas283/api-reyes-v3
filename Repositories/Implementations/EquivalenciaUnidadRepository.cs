using Npgsql;
using ReyesAR.BasedeDatos;
using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

public class EquivalenciaUnidadRepository : IEquivalenciaUnidadRepository
{
    private readonly ReyesARContext _context;

    public EquivalenciaUnidadRepository(ReyesARContext context)
    {
        _context = context;
    }

    // 1. Obtener todas las equivalencias (Mapeo a DTO para vista plana)
    public async Task<IEnumerable<EquivalenciaUnidadDTO>> GetAllAsync()
    {
        return await _context.EquivalenciasUnidad
            .Select(e => new EquivalenciaUnidadDTO
            {
                claveProducto = e.claveProducto,
                claveUnidadCompra = e.claveUnidadCompra,
                factor_conversion = e.factor_conversion,
                unidad_base = e.unidad_base.ToString()
            })
            .ToListAsync();
    }

    // 2. Obtener por clave compuesta (Producto + Unidad Compra)
    public async Task<EquivalenciaUnidadEntity?> GetByIdAsync(string claveProducto, string claveUnidadCompra)
    {
        return await _context.EquivalenciasUnidad
            .FirstOrDefaultAsync(e => e.claveProducto == claveProducto
                                   && e.claveUnidadCompra == claveUnidadCompra);
    }

    // 3. Crear recibiendo Entity y ejecutando "insertar_equivalencia_unidad"
    public async Task<EquivalenciaUnidadEntity> CreateAsync(EquivalenciaUnidadEntity entidad)
    {
        // Usamos el casting directo en el SQL para el ENUM de Postgres
        // Aseguramos las comillas dobles en el nombre del procedimiento
        var sql = "CALL \"insertar_equivalencia_unidad\"(@p0, @p1, @p2, @p3::nombre_unidad_enum)";

        await _context.Database.ExecuteSqlRawAsync(sql,
            entidad.claveProducto,       // @p0
            entidad.claveUnidadCompra,   // @p1
            entidad.factor_conversion,   // @p2
            entidad.unidad_base.ToString() // @p3 (Enviado como string para el cast ::)
        );

        // Retornamos la entidad buscando el registro recién insertado para confirmar
        var result = await GetByIdAsync(entidad.claveProducto, entidad.claveUnidadCompra);

        // El "!" indica que confiamos en que el registro existe tras el CALL exitoso
        return result!;
    }

    // 4. Actualizar datos de conversión (Promesa Task)
    public async Task UpdateAsync(string claveProducto, string claveUnidadCompra, EquivalenciaUnidadDTO dto)
    {
        var existente = await GetByIdAsync(claveProducto, claveUnidadCompra);

        if (existente != null)
        {
            existente.factor_conversion = dto.factor_conversion;

            _context.EquivalenciasUnidad.Update(existente);
            await _context.SaveChangesAsync();
        }
    }

    // 5. Eliminar la relación (Promesa Task)
    public async Task DeleteAsync(string claveProducto, string claveUnidadCompra)
    {
        var entidad = await GetByIdAsync(claveProducto, claveUnidadCompra);

        if (entidad != null)
        {
            _context.EquivalenciasUnidad.Remove(entidad);
            await _context.SaveChangesAsync();
        }
    }
}