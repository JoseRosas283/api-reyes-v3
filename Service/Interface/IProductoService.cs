using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Service.Interface
{
    public interface IProductoService
    {

        // GET → Obtener todos los productos (Activos e Inactivos)
        Task<IEnumerable<ProductoEntity>> GetAllAsync();

        // GET → Obtener un producto por su clave única
        Task<ProductoEntity?> GetByClaveAsync(string clave);

        Task<ProductoEntity> CreateAsync(ProductoDTO producto, IFormFile imagen);

        // PUT → Actualizar recibiendo el archivo opcionalmente
        Task<ProductoEntity> UpdateAsync(string clave, ProductoDTO producto, IFormFile? nuevaImagen);

        // DELETE → Eliminar físicamente o desactivar lógicamente según historial
        Task DeleteAsync(string clave);


    }
}
