using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Repositories.Interface
{
    public interface IEquivalenciaUnidadRepository
    {
        // Obtener todas las equivalencias (mapeadas a DTO para vista plana)
        Task<IEnumerable<EquivalenciaUnidadDTO>> GetAllAsync();

        // Obtener una específica por su llave compuesta (Producto + Unidad Compra)
        Task<EquivalenciaUnidadEntity?> GetByIdAsync(string claveProducto, string claveUnidadCompra);

        // Crear usando el Store Procedure (Retorna la Entity creada)
        Task<EquivalenciaUnidadEntity> CreateAsync(EquivalenciaUnidadEntity dto);

        // Actualizar datos de conversión (Promesa)
        Task UpdateAsync(string claveProducto, string claveUnidadCompra, EquivalenciaUnidadDTO dto);

        // Eliminar la relación (Promesa)
        Task DeleteAsync(string claveProducto, string claveUnidadCompra);
    }
}

