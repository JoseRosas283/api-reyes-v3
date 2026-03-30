using Npgsql;
using ReyesAR.BasedeDatos;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

public class EmpleadoRolRepository : IEmpleadoRolRepository
{
    private readonly ReyesARContext _context;

    public EmpleadoRolRepository(ReyesARContext context)
    {
        _context = context;
    }

    // Task<IEnumerable<EmpleadoRolEntity>> GetAllAsync();
    public async Task<IEnumerable<EmpleadoRolEntity>> GetAllAsync()
    {
        return await _context.EmpleadoRol
            .Include(er => er.Empleado)
            .Include(er => er.Rol)
            .ToListAsync();
    }

    // Task<EmpleadoRolEntity?> GetByIdAsync(string claveEmpleado, string claveRol, DateTime fechaInicio);
    public async Task<EmpleadoRolEntity?> GetByIdAsync(string claveEmpleado, string claveRol, DateTime fechaInicio)
    {
        return await _context.EmpleadoRol
            .Include(er => er.Empleado)
            .Include(er => er.Rol)
            .FirstOrDefaultAsync(er => er.claveEmpleado == claveEmpleado &&
                                       er.claveRol == claveRol &&
                                       er.fecha_inicio.Date == fechaInicio.Date);
    }

    // Task<IEnumerable<EmpleadoRolEntity>> GetByEmpleadoAsync(string claveEmpleado);
    public async Task<IEnumerable<EmpleadoRolEntity>> GetByEmpleadoAsync(string claveEmpleado)
    {
        return await _context.EmpleadoRol
            .Include(er => er.Rol)
            .Where(er => er.claveEmpleado == claveEmpleado)
            .OrderByDescending(er => er.fecha_inicio)
            .ToListAsync();
    }

    // Task<EmpleadoRolEntity> CreateAsync(EmpleadoRolEntity empleadoRol);
    public async Task<EmpleadoRolEntity> CreateAsync(EmpleadoRolEntity empleadoRol)
    {
        var pClaveEmpleado = new NpgsqlParameter("p_claveEmpleado", empleadoRol.claveEmpleado);
        var pClaveRol = new NpgsqlParameter("p_claveRol", empleadoRol.claveRol);
        var pFechaInicio = new NpgsqlParameter("p_fecha_inicio", empleadoRol.fecha_inicio);

        // Ejecución del procedimiento almacenado con nomenclatura exacta
        await _context.Database.ExecuteSqlRawAsync(
            "CALL \"asignar_rol_empleado\"(@p_claveEmpleado, @p_claveRol, @p_fecha_inicio)",
            pClaveEmpleado, pClaveRol, pFechaInicio
        );

        return empleadoRol;
    }

    // Task UpdateAsync(string claveEmpleado, string claveRol, DateTime fechaInicio, EmpleadoRolEntity empleadoRol);
    public async Task UpdateAsync(string claveEmpleado, string claveRol, DateTime fechaInicio, EmpleadoRolEntity empleadoRol)
    {
        // Buscamos por la terna de llaves para asegurar que editamos el registro correcto del historial
        var existente = await _context.EmpleadoRol
            .FirstOrDefaultAsync(er => er.claveEmpleado == claveEmpleado &&
                                       er.claveRol == claveRol &&
                                       er.fecha_inicio.Date == fechaInicio.Date);

        if (existente != null)
        {
            // Solo actualizamos fecha_fin, ya que cambiar las llaves rompería el historial
            existente.fecha_fin = empleadoRol.fecha_fin;

            _context.EmpleadoRol.Update(existente);
            await _context.SaveChangesAsync();
        }
    }

    // Task DeleteAsync(string claveEmpleado, string claveRol, DateTime fechaInicio);
    public async Task DeleteAsync(string claveEmpleado, string claveRol, DateTime fechaInicio)
    {
        var registro = await _context.EmpleadoRol
            .FirstOrDefaultAsync(er => er.claveEmpleado == claveEmpleado &&
                                       er.claveRol == claveRol &&
                                       er.fecha_inicio.Date == fechaInicio.Date);

        if (registro != null)
        {
            _context.EmpleadoRol.Remove(registro);
            await _context.SaveChangesAsync();
        }
    }
}