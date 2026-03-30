using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Repositories.Interface
{
    public interface IUnidadCompraRepository
    {

        // Consultar todas las unidades de compra
        Task<IEnumerable<UnidadCompraEntity>> GetAllAsync();

        // Consultar una unidad de compra por su clave (Ej: UC001)
        Task<UnidadCompraEntity?> GetByIdAsync(string claveUnidadCompra);

        // Insertar una nueva unidad de compra (capturando la PK generada)
        Task<UnidadCompraEntity> CreateAsync(UnidadCompraEntity unidadCompra);

        // Actualizar datos de la unidad de compra
        Task UpdateAsync(string claveUnidadCompra, UnidadCompraUpdateEstadoDto unidadCompra);

        // Eliminar por clave
        Task DeleteAsync(string claveUnidadCompra);
    }
}
