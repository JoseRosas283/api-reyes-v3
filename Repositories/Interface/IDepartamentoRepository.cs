using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Repositories.Interface
{
    public interface  IDepartamentoRepository
    {

        // Consultar todos los departamentos
        Task<IEnumerable<DepartamentoEntity>> GetAllAsync();

        // Consultar un departamento por su clave (Ej: DEP001)
        Task<DepartamentoEntity?> GetByIdAsync(string claveDepartamento);

        // Insertar un nuevo departamento (capturando la PK generada)
        Task<DepartamentoEntity> CreateAsync(DepartamentoEntity departamento);

        // Actualizar datos del departamento (ej. estado)
        Task UpdateAsync(string claveDepartamento, DepartamentoUpdateEstadoDto departamento);

        // Eliminar por clave
        Task DeleteAsync(string claveDepartamento);

    }
}
