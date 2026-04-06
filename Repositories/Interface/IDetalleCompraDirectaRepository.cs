using ReyesAR.Models;

namespace ReyesAR.Repositories.Interface
{
    public interface IDetalleCompraDirectaRepository
    {
        // 1. Obtener todos los detalles (General)
        Task<IEnumerable<DetalleCompraDirectaEntity>> GetAllAsync();

        // 2. Obtener los detalles de una compra específica
        Task<IEnumerable<DetalleCompraDirectaEntity>> GetByCompraAsync(string claveCompra);

        // 3. Buscar un registro específico por su Llave Primaria Compuesta
        Task<DetalleCompraDirectaEntity?> GetByIdAsync(string claveCompra, string claveProducto);

        // 4. Insertar usando el Procedimiento Almacenado "insertar_detalle_compra_directa"
        Task<DetalleCompraDirectaEntity> CreateAsync(DetalleCompraDirectaEntity detalle);

        // 5. Actualizar un registro (Solo campos permitidos como cantidad/precio)
        Task UpdateAsync(string claveCompra, string claveProducto, DetalleCompraDirectaEntity detalle);

        // 6. Eliminar un registro del detalle
        Task DeleteAsync(string claveCompra, string claveProducto);
    }
}
