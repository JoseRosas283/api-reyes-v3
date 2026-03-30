using ReyesAR.DTO;
using ReyesAR.Repositories.Interface;
using ReyesAR.Service.Interface;

namespace ReyesAR.Service.Implementations
{
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepository _repository;

        public ProveedorService(IProveedorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProveedorBaseDTO>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ProveedorBaseDTO?> GetByIdAsync(string idProveedor)
        {
            if (string.IsNullOrWhiteSpace(idProveedor))
                throw new ArgumentException("La clave del proveedor no puede ser nula o vacía.");

            return await _repository.GetByIdAsync(idProveedor);
        }

        public async Task<ProveedorBaseDTO> CreateAsync(ProveedorCreateDTO proveedorDto)
        {
            // Validaciones comunes
            if (string.IsNullOrWhiteSpace(proveedorDto.direccion))
                throw new ArgumentException("La dirección es obligatoria.");

            if (string.IsNullOrWhiteSpace(proveedorDto.telefono) || !System.Text.RegularExpressions.Regex.IsMatch(proveedorDto.telefono, @"^\d{10}$"))
                throw new ArgumentException("El teléfono debe contener exactamente 10 dígitos.");

            if (string.IsNullOrWhiteSpace(proveedorDto.tipo_proveedor) ||
                !(proveedorDto.tipo_proveedor.Equals("EMPRESA", StringComparison.OrdinalIgnoreCase) ||
                  proveedorDto.tipo_proveedor.Equals("INDEPENDIENTE", StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("El tipo de proveedor debe ser EMPRESA o INDEPENDIENTE.");

            // Validaciones específicas
            if (proveedorDto.tipo_proveedor.Equals("EMPRESA", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(proveedorDto.razonSocial) || string.IsNullOrWhiteSpace(proveedorDto.rfc))
                    throw new ArgumentException("Razón Social y RFC son obligatorios para proveedores EMPRESA.");

                if (!(proveedorDto.rfc.Length == 12 || proveedorDto.rfc.Length == 13))
                    throw new ArgumentException("El RFC debe tener 12 o 13 caracteres.");

                if (!string.IsNullOrWhiteSpace(proveedorDto.nombre) ||
                    !string.IsNullOrWhiteSpace(proveedorDto.apellidoPaterno) ||
                    !string.IsNullOrWhiteSpace(proveedorDto.apellidoMaterno) ||
                    !string.IsNullOrWhiteSpace(proveedorDto.tipoServicio))
                    throw new ArgumentException("Un proveedor EMPRESA no debe registrar nombre, apellidos ni tipo de servicio.");
            }
            else // INDEPENDIENTE
            {
                if (string.IsNullOrWhiteSpace(proveedorDto.nombre) ||
                    string.IsNullOrWhiteSpace(proveedorDto.apellidoPaterno) ||
                    string.IsNullOrWhiteSpace(proveedorDto.tipoServicio))
                    throw new ArgumentException("Nombre, Apellido Paterno y Tipo de Servicio son obligatorios para proveedores INDEPENDIENTES.");

                if (!string.IsNullOrWhiteSpace(proveedorDto.razonSocial) || !string.IsNullOrWhiteSpace(proveedorDto.rfc))
                    throw new ArgumentException("Un proveedor INDEPENDIENTE no debe registrar razón social ni RFC.");
            }

            // Si pasa todas las validaciones, se delega al repository
            return await _repository.CreateAsync(proveedorDto);
        }

        public async Task UpdateAsync(string claveUrl, ProveedorCreateDTO proveedorDto)
        {
            if (string.IsNullOrWhiteSpace(claveUrl))
                throw new ArgumentException("La clave del proveedor no puede ser nula o vacía.");

            // Reutilizamos las mismas validaciones que en Create
            await CreateAsync(proveedorDto);

            await _repository.UpdateAsync(claveUrl, proveedorDto);
        }

        public async Task DeleteAsync(string idProveedor)
        {
            if (string.IsNullOrWhiteSpace(idProveedor))
                throw new ArgumentException("La clave del proveedor no puede ser nula o vacía.");

            await _repository.DeleteAsync(idProveedor);
        }
    }

}
