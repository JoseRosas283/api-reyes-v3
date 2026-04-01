using ReyesAR.DTO;

namespace ReyesAR.Service.Interface
{
    public interface IEntregaService
    {
        Task<IEnumerable<EntregaEntity>> GetAllEntregasAsync();

        Task<EntregaEntity> GetEntregaByClaveAsync(string claveEntrega);

        Task<EntregaEntity> CreateEntregaAsync(EntregaDTO entregaDTO);

        Task<EntregaEntity> UpdateEntregaAsync(string claveEntrega, EntregaDTO entregaDTO);

        Task<bool> DeleteEntregaAsync(string claveEntrega);
    }
}
