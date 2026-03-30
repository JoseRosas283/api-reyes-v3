using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using ReyesAR.Service.Interface;

public class EmpleadoRolService : IEmpleadoRolService
{
    private readonly IEmpleadoRolRepository _repository;

    public EmpleadoRolService(IEmpleadoRolRepository repository)
    {
        _repository = repository;
    }

    // 1. OBTENER TODO EL HISTORIAL
    public async Task<IEnumerable<EmpleadoRolDTO>> GetAllHistorialAsync()
    {
        var entidades = await _repository.GetAllAsync();
        // Mapeo manual de Entidad a DTO
        return entidades.Select(e => new EmpleadoRolDTO
        {
            claveEmpleado = e.claveEmpleado,
            claveRol = e.claveRol,
            fecha_inicio = e.fecha_inicio,
            fecha_fin = e.fecha_fin
        });
    }

    // 2. BUSCAR POR CLAVE TRIPLE (EMPLEADO, ROL, FECHA)
    public async Task<EmpleadoRolDTO?> GetByKeysAsync(string claveEmpleado, string claveRol, DateTime fechaInicio)
    {
        // Validación básica de parámetros
        if (string.IsNullOrWhiteSpace(claveEmpleado) || string.IsNullOrWhiteSpace(claveRol))
            throw new ArgumentException("Las claves de búsqueda no pueden estar vacías.");

        var e = await _repository.GetByIdAsync(claveEmpleado, claveRol, fechaInicio);

        if (e == null) return null;

        return new EmpleadoRolDTO
        {
            claveEmpleado = e.claveEmpleado,
            claveRol = e.claveRol,
            fecha_inicio = e.fecha_inicio,
            fecha_fin = e.fecha_fin
        };
    }

    // 3. ASIGNAR ROL (Llama al SP "asignar_rol_empleado")
    public async Task<EmpleadoRolDTO> AssignRolAsync(EmpleadoRolDTO dto)
    {
        // --- VALIDACIONES BÁSICAS DE CAMPOS ---
        if (string.IsNullOrWhiteSpace(dto.claveEmpleado))
            throw new ArgumentException("La clave del empleado es obligatoria.");

        if (string.IsNullOrWhiteSpace(dto.claveRol))
            throw new ArgumentException("La clave del rol es obligatoria.");

        if (dto.fecha_inicio == default)
            dto.fecha_inicio = DateTime.Now; // Si no viene fecha, asignamos hoy

        // Mapeo de DTO a Entidad para el Repositorio
        var entidad = new EmpleadoRolEntity
        {
            claveEmpleado = dto.claveEmpleado.Trim().ToUpper(),
            claveRol = dto.claveRol.Trim().ToUpper(),
            fecha_inicio = dto.fecha_inicio
        };

        try
        {
            // El Repositorio ejecuta el CALL "asignar_rol_empleado"
            await _repository.CreateAsync(entidad);
            return dto;
        }
        catch (Exception ex)
        {
            // Aquí capturamos los RAISE EXCEPTION de tu procedimiento de PostgreSQL
            // Ejemplo: "El empleado X ya tiene un rol activo..."
            throw new InvalidOperationException($"Error de Base de Datos: {ex.Message}");
        }
    }

    // 4. ACTUALIZAR (Normalmente para cerrar un periodo con fecha_fin)
    public async Task UpdateHistorialAsync(string claveEmpleado, string claveRol, DateTime fechaInicio, EmpleadoRolDTO dto)
    {
        if (string.IsNullOrWhiteSpace(claveEmpleado)) throw new ArgumentException("ID Empleado requerido.");

        var entidadActualizada = new EmpleadoRolEntity
        {
            fecha_fin = dto.fecha_fin // Solo permitimos actualizar la fecha de cierre
        };

        await _repository.UpdateAsync(claveEmpleado, claveRol, fechaInicio, entidadActualizada);
    }

    // 5. ELIMINAR
    public async Task DeleteHistorialAsync(string claveEmpleado, string claveRol, DateTime fechaInicio)
    {
        if (string.IsNullOrWhiteSpace(claveEmpleado)) throw new ArgumentException("ID Empleado requerido.");

        await _repository.DeleteAsync(claveEmpleado, claveRol, fechaInicio);
    }

    // 6. HISTORIAL POR EMPLEADO
    public async Task<IEnumerable<EmpleadoRolDTO>> GetHistorialByEmpleadoAsync(string claveEmpleado)
    {
        if (string.IsNullOrWhiteSpace(claveEmpleado)) return Enumerable.Empty<EmpleadoRolDTO>();

        var lista = await _repository.GetByEmpleadoAsync(claveEmpleado);
        return lista.Select(e => new EmpleadoRolDTO
        {
            claveEmpleado = e.claveEmpleado,
            claveRol = e.claveRol,
            fecha_inicio = e.fecha_inicio,
            fecha_fin = e.fecha_fin
        });
    }
}
