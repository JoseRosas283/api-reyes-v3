using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Repositories.Interface
{
    public interface IPedidoRepository
    {
        Task<IEnumerable<PedidoEntity>> GetAllAsync();

        // Buscar pedido por su clave (ej. PED-1)
        Task<PedidoEntity?> GetByIdAsync(string clavePedido);

        // Crear un nuevo pedido con sus detalles
        Task<PedidoEntity> CreateAsync(PedidoEntity pedido);

        // Actualizar datos del pedido (estado, observaciones, total, etc.)
        Task UpdateAsync(string clavePedido, PedidoDTO pedido);

        // Eliminación lógica (cambiar estado a cancelado/inactivo)
        Task DeleteAsync(string clavePedido);
    }
}
