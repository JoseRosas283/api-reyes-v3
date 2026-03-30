using ReyesAR.DTO;

namespace ReyesAR.Service.Interface
{
    public interface IProveedorService
    {

        Task<IEnumerable<ProveedorBaseDTO>> GetAllAsync();

        Task<ProveedorBaseDTO?> GetByIdAsync(string idProveedor);

        Task<ProveedorBaseDTO> CreateAsync(ProveedorCreateDTO proveedorDto);

        Task UpdateAsync(string claveUrl, ProveedorCreateDTO proveedorDto);

        Task DeleteAsync(string idProveedor);

    }
}
