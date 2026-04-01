namespace ReyesAR.Repositories.Interface
{
    public interface IEntregaRepository
    {
        Task<IEnumerable<EntregaEntity>> GetAllAsync();

        Task<EntregaEntity> GetByClaveAsync(string claveEntrega);

        Task<EntregaEntity> AddAsync(EntregaEntity entrega);

        Task<EntregaEntity> UpdateAsync(EntregaEntity entrega);

        Task<EntregaEntity> DeleteAsync(string claveEntrega);
    }
}
