using ReyesAR.DTO;

namespace ReyesAR.Service.Interface
{
    public interface IEmpleadoRolService
    {
        // Obtener todos los registros del historial mapeados a DTO
        Task<IEnumerable<EmpleadoRolDTO>> GetAllHistorialAsync();

        // Buscar un registro específico por su terna de llaves (PK Triple)
        Task<EmpleadoRolDTO?> GetByKeysAsync(string claveEmpleado, string claveRol, DateTime fechaInicio);

        // Obtener el historial filtrado por un empleado específico
        Task<IEnumerable<EmpleadoRolDTO>> GetHistorialByEmpleadoAsync(string claveEmpleado);

        // Crear el registro (Llamada al SP "asignar_rol_empleado")
        Task<EmpleadoRolDTO> AssignRolAsync(EmpleadoRolDTO empleadoRolDto);

        // Actualizar el registro (Promesa de actualización)
        Task UpdateHistorialAsync(string claveEmpleado, string claveRol, DateTime fechaInicio, EmpleadoRolDTO empleadoRolDto);

        // Eliminar el registro (Promesa de eliminación)
        Task DeleteHistorialAsync(string claveEmpleado, string claveRol, DateTime fechaInicio);
    }
}
