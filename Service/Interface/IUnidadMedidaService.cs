using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Service.Interface
{
    public interface IUnidadMedidaService
    {

        // GET → Obtener lista completa
        Task<IEnumerable<UnidadMedidaEntity>> ListarUnidadesAsync();

        // GET → Obtener una por su clave
        Task<UnidadMedidaEntity?> ObtenerPorIdAsync(string claveUnidad);

        // POST → Recibe DTO y devuelve la entidad creada (para obtener la clave generada)
        Task<UnidadMedidaEntity> RegistrarUnidadAsync(UnidadMedidaDTO unidadMedidaDto);

        // PUT → Recibe clave y DTO específico para estado
        Task UpdateAsync(string claveUnidad, UnidadMedidaUpdateEstadoDto dto);

        // DELETE → Recibe clave, devuelve Task (void asíncrono)
        Task EliminarUnidadAsync(string claveUnidad);
    }
}
