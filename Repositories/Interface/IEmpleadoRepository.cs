using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Repositories.Interface
{
    public interface IEmpleadoRepository
    {
        // Obtener listado de empleados
        Task<IEnumerable<EmpleadoEntity>> GetAllAsync();

        // Buscar empleado por su clave (ej. EMP-1)
        Task<EmpleadoEntity?> GetByIdAsync(string claveEmpleado);

        // Crear un nuevo empleado
        Task<EmpleadoEntity> CreateAsync(EmpleadoDTO empleado);

        // Actualizar datos del empleado (nombre, apellidos, teléfono, etc.)
        Task UpdateAsync(string claveEmpleado, EmpleadoDTO empleado);

        // Eliminación lógica (cambiar estado a inactivo)
        Task DeleteAsync(string claveEmpleado);
    }
}
