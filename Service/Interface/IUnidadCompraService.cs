using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Service.Interface
{
    public interface IUnidadCompraService
    {
        // GET → Obtener lista completa
        Task<IEnumerable<UnidadCompraEntity>> ListarUnidadesAsync();

        // GET → Obtener una por su clave (Ej: UC001)
        Task<UnidadCompraEntity?> ObtenerPorIdAsync(string claveUnidadCompra);

        // POST → Recibe DTO y devuelve la entidad creada (para obtener la clave generada)
        Task<UnidadCompraEntity> RegistrarUnidadAsync(UnidadCompraDTO unidadCompraDto);

        // PUT → Recibe clave y DTO específico para estado
        Task UpdateAsync(string claveUnidadCompra, UnidadCompraUpdateEstadoDto dto);

        // DELETE → Recibe clave, devuelve Task (void asíncrono)
        Task EliminarUnidadAsync(string claveUnidadCompra);
    }
}
