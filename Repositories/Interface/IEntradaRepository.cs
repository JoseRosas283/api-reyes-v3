using ReyesAR.Models;

namespace ReyesAR.Repositories.Interface
{
    public interface IEntradaRepository
    {
        // 1. Obtener todas las entradas (Historial general)
        Task<IEnumerable<EntradaEntity>> GetAllAsync();

        // 2. Obtener entradas de una compra específica (Pedido o Directa)
        Task<IEnumerable<EntradaEntity>> GetByCompraAsync(string claveCompra);

        // 3. Obtener una entrada por su folio único (ENT-...)
        Task<EntradaEntity?> GetByIdAsync(string claveEntrada);

        // 4. Insertar usando el Procedimiento Almacenado "insertar_entrada"
        // Este método recibirá la entidad con los datos capturados y el ID del usuario
        Task<EntradaEntity> CreateAsync(EntradaEntity entrada);

        // 5. Eliminar una entrada (en caso de error administrativo)
        Task DeleteAsync(string claveEntrada);
    }
}
