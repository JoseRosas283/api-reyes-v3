using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Service.Interface
{
    public  interface IDepartamentoService
    {


        Task<IEnumerable<DepartamentoEntity>> GetAllAsync();

        // Consultar un departamento por su clave (Ej: DEP001)
        Task<DepartamentoEntity?> GetByIdAsync(string claveDepartamento);

        // Insertar un nuevo departamento (recibiendo DTO)
        Task<DepartamentoEntity> CreateAsync(DepartamentoDTO departamentoDto);

        // Actualizar datos del departamento (ej. estado, recibiendo DTO)
        Task UpdateAsync(string claveDepartamento, DepartamentoUpdateEstadoDto departamentoDto);

        // Eliminar por clave
        Task DeleteAsync(string claveDepartamento);


    }
}
