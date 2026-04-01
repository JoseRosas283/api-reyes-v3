using ReyesAR.BasedeDatos;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

public class RepresentanteRepository : IRepresentanteRepository
{
    private readonly ReyesARContext _context;

    public RepresentanteRepository(ReyesARContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RepresentanteEntity>> GetAllAsync()
    {
        return await _context.Set<RepresentanteEntity>()
            .Include(r => r.Proveedor) // Carga Eager para ver a qué empresa pertenecen
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<RepresentanteEntity?> GetByIdAsync(string claveRepresentante)
    {
        return await _context.Set<RepresentanteEntity>()
            .Include(r => r.Proveedor)
            .FirstOrDefaultAsync(r => r.claveRepresentante == claveRepresentante);
    }

    public async Task<IEnumerable<RepresentanteEntity>> GetByProveedorAsync(string claveProveedor)
    {
        return await _context.Set<RepresentanteEntity>()
            .Where(r => r.claveProveedor == claveProveedor)
            .ToListAsync();
    }

    public async Task<RepresentanteEntity> CreateAsync(RepresentanteEntity representante)
    {
        // Ejecución del Procedimiento Almacenado con comillas y nomenclatura exacta
        // El orden coincide con: p_nombre, p_paterno, p_materno, p_telefono, p_claveProv, p_tipoRep
        await _context.Database.ExecuteSqlRawAsync(
            "CALL \"insertar_representante\"({0}, {1}, {2}, {3}, {4}, {5}::\"tipo_representante_enum\")",
            representante.nombre,
            representante.apellido_paterno,
            representante.apellido_materno,
            representante.telefono,
            representante.claveProveedor,
            representante.tipo_representante
        );

        // Retornamos la entidad. Nota: La claveRepresentante se genera en DB, 
        // si necesitas el ID exacto tras insertar, tendrías que consultar el último registro o 
        // ajustar el SP para que sea una función que retorne el ID.
        return representante;
    }

    public async Task UpdateAsync(string claveRepresentante, RepresentanteEntity representante)
    {
        var existente = await _context.Set<RepresentanteEntity>()
            .FirstOrDefaultAsync(r => r.claveRepresentante == claveRepresentante);

        if (existente != null)
        {
            existente.nombre = representante.nombre;
            existente.apellido_paterno = representante.apellido_paterno;
            existente.apellido_materno = representante.apellido_materno;
            existente.telefono = representante.telefono;
            existente.estado = representante.estado;
            // Normalmente la claveProveedor y el tipo no se cambian una vez asignados por lógica de negocio

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(string claveRepresentante)
    {
        var entidad = await _context.Set<RepresentanteEntity>()
            .FirstOrDefaultAsync(r => r.claveRepresentante == claveRepresentante);

        if (entidad != null)
        {
            _context.Set<RepresentanteEntity>().Remove(entidad);
            await _context.SaveChangesAsync();
        }
    }
}