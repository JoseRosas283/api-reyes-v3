using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Service.Interface
{
    public interface IPedidoService
    {
        // Obtener todos los pedidos con sus detalles
        Task<IEnumerable<PedidoEntity>> GetAllAsync();

        // Obtener un pedido específico por clave
        Task<PedidoEntity?> GetByIdAsync(string clavePedido);

        // Crear un nuevo pedido a partir de un DTO
        Task<PedidoEntity> CreateAsync(PedidoDTO pedido);

        // Actualizar un pedido existente
        Task UpdateAsync(string clavePedido, PedidoDTO pedido);

        // Eliminación lógica (solo confirma con mensaje)
        Task DeleteAsync(string clavePedido); // este va a ser mi contrato de service
    }
}
