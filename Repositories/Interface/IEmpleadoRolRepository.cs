using ReyesAR.Models;

namespace ReyesAR.Repositories.Interface
{
    public interface IEmpleadoRolRepository
    {
        // Obtener todo el historial de roles (Eager Loading)
        Task<IEnumerable<EmpleadoRolEntity>> GetAllAsync();

        // Obtener un registro específico usando la terna de llaves (PK Triple)
        Task<EmpleadoRolEntity?> GetByIdAsync(string claveEmpleado, string claveRol, DateTime fechaInicio);

        // Obtener todos los roles que ha tenido un empleado específico
        Task<IEnumerable<EmpleadoRolEntity>> GetByEmpleadoAsync(string claveEmpleado);

        // Agregar un nuevo rol al historial
        Task<EmpleadoRolEntity> CreateAsync(EmpleadoRolEntity empleadoRol);

        // Actualizar un registro (por ejemplo, para poner la fecha_fin)
        Task UpdateAsync(string claveEmpleado, string claveRol, DateTime fechaInicio, EmpleadoRolEntity empleadoRol);

        // Eliminación física del registro de historial
        Task DeleteAsync(string claveEmpleado, string claveRol, DateTime fechaInicio);
    }
}
