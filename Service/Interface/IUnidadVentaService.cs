using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Service.Interface
{
    public interface IUnidadVentaService
    {

        // GET → Obtener lista completa
        Task<IEnumerable<UnidadVentaEntity>> ListarUnidadesAsync();

        // GET → Obtener una por su clave (Ej: UV001)
        Task<UnidadVentaEntity?> ObtenerPorIdAsync(string claveUnidadVenta);

        // POST → Recibe DTO y devuelve la entidad creada (para obtener la clave generada)
        Task<UnidadVentaEntity> RegistrarUnidadAsync(UnidadVentaDTO unidadVentaDto);

        // PUT → Recibe clave y DTO específico para estado
        Task UpdateAsync(string claveUnidadVenta, UnidadVentaUpdateEstadoDto dto);

        // DELETE → Recibe clave, devuelve Task (void asíncrono)
        Task EliminarUnidadAsync(string claveUnidadVenta);
    }
}
