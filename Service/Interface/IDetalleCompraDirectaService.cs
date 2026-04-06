using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Service.Interface
{
    public interface IDetalleCompraDirectaService
    {
        // 1. Obtener todos los registros físicos
        Task<IEnumerable<DetalleCompraDirectaEntity>> GetAllAsync();

        // 2. Obtener los detalles de una compra específica (Listado detallado)
        Task<IEnumerable<DetalleCompraDirectaEntity>> GetByCompraAsync(string claveCompra);

        // 3. Buscar un registro por su Llave Primaria Compuesta
        Task<DetalleCompraDirectaEntity?> GetByIdAsync(string claveCompra, string claveProducto);

        // 4. Crear detalle recibiendo DTO (Validaciones + Mapeo + Llamada al SP)
        Task<DetalleCompraDirectaEntity> CreateAsync(DetalleCompraDirectaDTO dto);

        // 5. Actualizar campos permitidos recibiendo DTO
        Task UpdateAsync(string claveCompra, string claveProducto, DetalleCompraDirectaDTO dto);

        // 6. Eliminar un registro
        Task DeleteAsync(string claveCompra, string claveProducto);
    }
}
