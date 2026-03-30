using ReyesAR.Models;

namespace ReyesAR.Repositories.Interface
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<CategoriaEntity>> GetAllAsync();

        // Obtener una categoría específica por su clave primaria
        Task<CategoriaEntity?> GetByIdAsync(string clave);

        // Crear una nueva categoría recibiendo la entidad ya mapeada
        Task<CategoriaEntity> CreateAsync(CategoriaEntity categoria);

        // Actualizar una categoría existente usando el objeto entidad
        Task UpdateAsync(string clave, CategoriaEntity categoria);

        // Eliminar una categoría de la base de datos
        Task DeleteAsync(string clave);


    }
}
