using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Repositories.Interface
{
    public interface IProductoRepository
    {

        // Obtener listado de productos (incluyendo stock y precios)
        Task<IEnumerable<ProductoEntity>> GetAllAsync();

        // Buscar producto por su Clave (ej. PROD-1) o Código de Barras
        Task<ProductoEntity?> GetByIdAsync(string claveProducto);

        // Crear producto enviando la entidad completa
        // El SP se encargará de validar las FK (Categoría, Proveedor, Unidad)
        Task<ProductoEntity> CreateAsync(ProductoDTO producto);

        // Actualizar datos del producto (Nombre, Precio, Stock, etc.)
        Task UpdateAsync(string clave, ProductoDTO producto);

        // Eliminación (Normalmente lógica cambiando el estado)
        Task DeleteAsync(string clave);


    }
}
