using ReyesAR.DTO;
using ReyesAR.Models;

namespace ReyesAR.Repositories.Interface
{
    public  interface IProveedorRepository
    {


        Task<IEnumerable<ProveedorBaseDTO>> GetAllAsync();
        Task<ProveedorBaseDTO?> GetByIdAsync(string idProveedor);
        Task<ProveedorBaseDTO> CreateAsync(ProveedorCreateDTO proveedorDto);
        Task UpdateAsync(string claveUrl, ProveedorCreateDTO proveedorDto);
        Task DeleteAsync(string idProveedor);
    }

}

