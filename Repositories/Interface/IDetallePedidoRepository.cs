using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Repositories.Interface
{
    public interface IDetallePedidoRepository
    {
        // Obtener todos los detalles de un pedido (Eager Loading con EF)
        Task<IEnumerable<DetallePedidoEntity>> GetAllAsync();

        Task<DetallePedidoEntity?> GetByIdAsync(string clavePedido, String ClaveProducto);

        // Agregar un detalle a la DB usando el Change Tracker de Entity
        Task<DetallePedidoEntity> CreateAsync(DetallePedidoEntity detalle);

        // Al ser EF, para actualizar necesitamos la terna de llaves
        Task UpdateAsync(string Clavepedido, string Claveproducto, DetallePedidoDTO detalle);

        // Eliminación física del registro en la tabla puente
        Task DeleteAsync(string Clavepedido, string Claveproducto);
    }
}
