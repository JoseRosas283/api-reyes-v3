using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Service.Interface
{
    public interface IEmpleadoService
    {
        // Obtener todos los empleados
        Task<IEnumerable<EmpleadoEntity>> GetAllAsync();

        // Obtener un empleado específico por clave
        Task<EmpleadoEntity?> GetByIdAsync(string claveEmpleado);

        // Crear un nuevo empleado a partir de un DTO
        Task<EmpleadoEntity> CreateAsync(EmpleadoDTO empleado);

        // Actualizar un empleado existente
        Task UpdateAsync(string claveEmpleado, EmpleadoDTO empleado);

        // Eliminación lógica (la base de datos maneja el estado)
        Task DeleteAsync(string claveEmpleado);
    }
}
