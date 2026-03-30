using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Service.Interface
{
    public interface ICategoriaService
    {

        Task<IEnumerable<CategoriaEntity>> ListarCategoriasAsync();

        // Devuelve la entidad completa buscada por clave
        Task<CategoriaEntity?> ObtenerPorIdAsync(string clave);

        // Recibe datos limpios (DTO) pero devuelve la entidad creada con su nueva clave
        Task<CategoriaEntity> RegistrarCategoriaAsync(CategoriaDTO dto);

        // Operaciones de acción que no requieren retorno de objeto
        Task ActualizarCategoriaAsync(string clave, CategoriaDTO dto);

        Task EliminarCategoriaAsync(string clave);




    }
}
