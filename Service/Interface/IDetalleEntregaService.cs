using ReyesAR.DTO;

namespace ReyesAR.Service.Interface
{
    public interface IDetalleEntregaService
    {
        Task<IEnumerable<DetalleEntregaEntity>> GetAllAsync();

        Task<IEnumerable<DetalleEntregaEntity>> GetDetailedListAsync(string claveEntrega);

        Task<DetalleEntregaEntity> GetByIdAsync(string claveEntrega, string claveProducto, string tipo_detalle, bool extra);

        // Recibe DTO para creación
        Task<DetalleEntregaEntity> CreateAsync(DetalleEntregaDTO dto);

        // Recibe DTO para actualización
        Task UpdateAsync(string claveEntrega, string claveProducto, string tipo_detalle, bool extra, DetalleEntregaDTO dto);

        Task DeleteAsync(string claveEntrega, string claveProducto, string tipo_detalle, bool extra);
    }
}
