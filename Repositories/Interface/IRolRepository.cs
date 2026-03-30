using ReyesAR.Models;

namespace ReyesAR.Repositories.Interface
{
    public interface IRolRepository
    {
        // Crear un nuevo rol
        Task<RolEntity> CreateAsync(RolEntity rol);

        // Obtener un rol por su clave
        Task<RolEntity> GetByIdAsync(string claveRol);

        // Listar todos los roles
        Task<IEnumerable<RolEntity>> GetAllAsync();

        // Actualizar un rol existente
        Task<RolEntity> UpdateAsync(RolEntity rol);

        // Eliminar un rol por su clave
        Task<bool> DeleteAsync(string claveRol);
    }
}
