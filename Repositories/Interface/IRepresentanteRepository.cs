using ReyesAR.Models;

namespace ReyesAR.Repositories.Interface
{
    public interface IRepresentanteRepository
    {
        // Obtener todos los representantes (Incluyendo la carga del Proveedor si es necesario)
        Task<IEnumerable<RepresentanteEntity>> GetAllAsync();

        // Buscar por su clave única (PK)
        Task<RepresentanteEntity?> GetByIdAsync(string claveRepresentante);

        // Obtener todos los representantes de un proveedor específico
        Task<IEnumerable<RepresentanteEntity>> GetByProveedorAsync(string claveProveedor);

        // Crear usando el Procedimiento Almacenado "insertar_representante"
        // Retorna la entidad para confirmar la creación
        Task<RepresentanteEntity> CreateAsync(RepresentanteEntity representante);

        // Actualizar datos básicos (teléfono, estado, etc.)
        Task UpdateAsync(string claveRepresentante, RepresentanteEntity representante);

        // Eliminación (o cambio de estado si prefieres borrado lógico)
        Task DeleteAsync(string claveRepresentante);
    }
}
