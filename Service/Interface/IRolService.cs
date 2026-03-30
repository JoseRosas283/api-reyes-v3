using ReyesAR.DTO;

namespace ReyesAR.Service.Interface
{
    public interface IRolService
    {
        // Crear un nuevo rol a partir de un DTO
        Task<RolDTO> CreateAsync(RolDTO rolDto);

        // Obtener un rol por su clave
        Task<RolDTO> GetByIdAsync(string claveRol);

        // Listar todos los roles
        Task<IEnumerable<RolDTO>> GetAllAsync();

        // Actualizar un rol existente
        Task<RolDTO> UpdateAsync(RolDTO rolDto);

        // Eliminar un rol por su clave
        Task<bool> DeleteAsync(string claveRol);
    }
}