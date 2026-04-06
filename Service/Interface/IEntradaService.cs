using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Service.Interface
{
    public interface IEntradaService
    {
        // 1. Obtener el historial completo de entradas
        Task<IEnumerable<EntradaEntity>> GetAllAsync();

        // 2. Obtener entradas filtradas por una compra (Folio)
        Task<IEnumerable<EntradaEntity>> GetByCompraAsync(string claveCompra);

        // 3. Buscar una entrada específica por su folio ENT-...
        Task<EntradaEntity?> GetByIdAsync(string claveEntrada);

        // 4. Registrar nueva entrada (Validaciones de negocio + Mapeo + SP)
        Task<EntradaEntity> CreateAsync(EntradaDTO dto);

        // 5. Eliminar un registro de entrada (Solo si es estrictamente necesario)
        Task DeleteAsync(string claveEntrada);
    }
}
