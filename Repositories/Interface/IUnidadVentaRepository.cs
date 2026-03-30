using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Repositories.Interface
{
    public interface IUnidadVentaRepository
    {

        // Consultar todas las unidades de venta
        Task<IEnumerable<UnidadVentaEntity>> GetAllAsync();

        // Consultar una unidad de venta por su clave (Ej: UV001)
        Task<UnidadVentaEntity?> GetByIdAsync(string claveUnidadVenta);

        // Insertar una nueva unidad de venta (Capturando la PK generada)
        Task<UnidadVentaEntity> CreateAsync(UnidadVentaEntity unidadVenta);

        // Actualizar datos de la unidad de venta
        Task UpdateAsync(string claveUnidadVenta,UnidadVentaUpdateEstadoDto unidadVenta);

        // Eliminar por clave
        Task DeleteAsync(string claveUnidadVenta);
    }
}
