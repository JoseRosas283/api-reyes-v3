using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReyesAR.BasedeDatos;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;

public class RolRepository : IRolRepository
{
    private readonly ReyesARContext _context;

    public RolRepository(ReyesARContext context)
    {
        _context = context;
    }

    // Crear un nuevo rol usando el procedimiento insertar_rol
    public async Task<RolEntity> CreateAsync(RolEntity rol)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "CALL \"insertar_rol\"(@p_nombre, @p_descripcion)",
            new NpgsqlParameter("p_nombre", rol.nombre),
            new NpgsqlParameter("p_descripcion", rol.descripcion)
        );

        var nuevoRol = await _context.Roles
            .FirstOrDefaultAsync(r => r.nombre == rol.nombre);

        if (nuevoRol == null)
            throw new KeyNotFoundException($"No se encontró el rol '{rol.nombre}' recién creado.");

        return nuevoRol;
    }

    // Obtener un rol por su clave
    public async Task<RolEntity> GetByIdAsync(string claveRol)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.ClaveRol == claveRol);
    }

    // Listar todos los roles
    public async Task<IEnumerable<RolEntity>> GetAllAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    // Actualizar un rol existente
    public async Task<RolEntity> UpdateAsync(RolEntity rol)
    {
        var existente = await _context.Roles
            .FirstOrDefaultAsync(r => r.ClaveRol == rol.ClaveRol);

        if (existente == null) return null;

        existente.nombre = rol.nombre;
        existente.descripcion = rol.descripcion;
        existente.activo = rol.activo;

        await _context.SaveChangesAsync();
        return existente;
    }

    // Eliminar un rol por su clave
    public async Task<bool> DeleteAsync(string claveRol)
    {
        var rol = await _context.Roles
            .FirstOrDefaultAsync(r => r.ClaveRol == claveRol);

        if (rol == null) return false;

        _context.Roles.Remove(rol);
        await _context.SaveChangesAsync();
        return true;
    }
}