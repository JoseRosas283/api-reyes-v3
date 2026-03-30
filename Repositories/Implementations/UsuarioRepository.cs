using Npgsql;
using ReyesAR.BasedeDatos;
using ReyesAR.Models;
using Microsoft.EntityFrameworkCore;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ReyesARContext _context;

    public UsuarioRepository(ReyesARContext context)
    {
        _context = context;
    }

    // Task<IEnumerable<UsuarioEntity>> GetAllAsync();
    public async Task<IEnumerable<UsuarioEntity>> GetAllAsync()
    {
        return await _context.Usuarios
            .Include(u => u.Empleado)
            .ToListAsync();
    }

    // Task<UsuarioEntity?> GetByIdAsync(string claveUsuario);
    public async Task<UsuarioEntity?> GetByIdAsync(string claveUsuario)
    {
        return await _context.Usuarios
            .Include(u => u.Empleado)
            .FirstOrDefaultAsync(u => u.claveUsuario == claveUsuario);
    }

    // Task<UsuarioEntity?> GetByUsernameAsync(string nombre_usuario);
    public async Task<UsuarioEntity?> GetByUsernameAsync(string nombre_usuario)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.nombre_usuario == nombre_usuario);
    }

    // Task<UsuarioEntity?> GetByEmpleadoAsync(string claveEmpleado);
    public async Task<UsuarioEntity?> GetByEmpleadoAsync(string claveEmpleado)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.claveEmpleado == claveEmpleado);
    }

    // Task<UsuarioEntity> CreateAsync(UsuarioEntity usuario);
    // Implementación con el Procedimiento Almacenado "insertar_usuario"
    public async Task<UsuarioEntity> CreateAsync(UsuarioEntity usuario)
    {
        var pNombre = new NpgsqlParameter("p_nombre_usuario", usuario.nombre_usuario);
        var pContrasena = new NpgsqlParameter("p_contrasena", usuario.contrasena);
        var pClaveEmpleado = new NpgsqlParameter("p_claveEmpleado", usuario.claveEmpleado);

        // CALL ejecuta el procedimiento con las validaciones de longitud y rol
        await _context.Database.ExecuteSqlRawAsync(
            "CALL \"insertar_usuario\"(@p_nombre_usuario, @p_contrasena, @p_claveEmpleado)",
            pNombre, pContrasena, pClaveEmpleado
        );

        return usuario;
    }

    // Task UpdateAsync(string claveUsuario, UsuarioEntity usuario);
    public async Task UpdateAsync(string claveUsuario, UsuarioEntity usuario)
    {
        var existente = await _context.Usuarios.FindAsync(claveUsuario);

        if (existente != null)
        {
            existente.nombre_usuario = usuario.nombre_usuario;
            existente.contrasena = usuario.contrasena;
            existente.estado = usuario.estado;
            existente.claveEmpleado = usuario.claveEmpleado;

            _context.Usuarios.Update(existente);
            await _context.SaveChangesAsync();
        }
    }

    // Task DeleteAsync(string claveUsuario);
    public async Task DeleteAsync(string claveUsuario)
    {
        var usuario = await _context.Usuarios.FindAsync(claveUsuario);
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }
}