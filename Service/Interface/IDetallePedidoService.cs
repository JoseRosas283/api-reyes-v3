using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Service.Interface
{
    public interface IDetallePedidoService
    {
        // Obtener todos los detalles (mapeados a DTO para la vista)
        Task<IEnumerable<DetallePedidoEntity>> GetAllDetailsAsync();

        // Buscar un detalle específico por su llave compuesta
        Task<DetallePedidoEntity?> GetDetailByIdAsync(string clavePedido, string claveProducto);

        // Crear el detalle (aquí es donde se captura la excepción si el SP falla)
        Task<DetallePedidoEntity> CreateDetailAsync(DetallePedidoDTO detalleDto);

        // Actualizar el detalle (Retorna una promesa de éxito/fallo)
        Task<DetallePedidoEntity> UpdateDetailAsync(string clavePedido, string claveProducto, DetallePedidoDTO detalleDto);

        // Eliminar el detalle (Retorna una promesa de éxito/fallo)
        Task<DetallePedidoEntity> DeleteDetailAsync(string clavePedido, string claveProducto);
    }
}
