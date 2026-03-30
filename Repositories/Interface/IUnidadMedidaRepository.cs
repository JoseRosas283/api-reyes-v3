using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Repositories.Interface
{
    public interface IUnidadMedidaRepository
    {
        // Consultar todas las unidades
        Task<IEnumerable<UnidadMedidaEntity>> GetAllAsync();

        // Consultar una por su clave (Recordando que usas varchar(18) con funciones)
        Task<UnidadMedidaEntity?> GetByIdAsync(string claveUnidad);

        // Insertar
        Task<UnidadMedidaEntity> CreateAsync(UnidadMedidaEntity unidadMedida);

        // Actualizar
        Task UpdateAsync(string claveUnidad, UnidadMedidaUpdateEstadoDto dto);

        // Eliminar
        Task DeleteAsync(string claveUnidad);
    }
}
