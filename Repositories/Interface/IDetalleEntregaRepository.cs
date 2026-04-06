namespace ReyesAR.Repositories.Interface
{
    public interface IDetalleEntregaRepository
    {
        // 1. Obtener listado de la tabla física (Entidad)
        Task<IEnumerable<DetalleEntregaEntity>> GetAllAsync();

        // 2. Obtener listado DETALLADO (Desde la VISTA de PostgreSQL)
        // Retorna un DTO con nombres de productos y unidades para el Frontend
        Task<IEnumerable<DetalleEntregaEntity>> GetDetailedListAsync(string claveEntrega);

        // 3. Buscar detalle por su Llave Primaria Compuesta (4 campos)
        Task<DetalleEntregaEntity?> GetByIdAsync(string claveEntrega, string claveProducto, string tipo_detalle, bool extra);

        // 4. Crear detalle enviando el DTO y retornando la Entidad creada
        // Ejecuta el procedimiento almacenado "insertar_detalle_entrega"
        Task<DetalleEntregaEntity> CreateAsync(DetalleEntregaEntity detalle);

        // 5. Actualizar datos del detalle (Requiere los 4 identificadores)
        Task UpdateAsync(string claveEntrega, string claveProducto, string tipo_detalle, bool extra, DetalleEntregaEntity detalle);

        // 6. Eliminación de un registro específico
        Task DeleteAsync(string claveEntrega, string claveProducto, string tipo_detalle, bool extra);
    }
}