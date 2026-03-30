using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using ReyesAR.Service.Interface;

namespace ReyesAR.Service.Implementations
{
    public class DepartamentoService : IDepartamentoService
    {
        private readonly IDepartamentoRepository _departamentoRepository;

        public DepartamentoService(IDepartamentoRepository departamentoRepository)
        {
            _departamentoRepository = departamentoRepository;
        }

        public async Task<IEnumerable<DepartamentoEntity>> GetAllAsync()
        {
            return await _departamentoRepository.GetAllAsync();
        }

        public async Task<DepartamentoEntity?> GetByIdAsync(string claveDepartamento)
        {
            if (string.IsNullOrWhiteSpace(claveDepartamento))
                throw new ArgumentException("La clave del departamento no puede estar vacía.");

            return await _departamentoRepository.GetByIdAsync(claveDepartamento);
        }

        public async Task<DepartamentoEntity> CreateAsync(DepartamentoDTO departamentoDto)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(departamentoDto.nombre))
                throw new ArgumentException("El nombre del departamento es obligatorio.");

            if (string.IsNullOrWhiteSpace(departamentoDto.descripcion))
                throw new ArgumentException("La descripción del departamento es obligatoria.");

            // Mapear DTO a Entity
            var entity = new DepartamentoEntity
            {
                nombre = departamentoDto.nombre.Trim(),
                descripcion = departamentoDto.descripcion.Trim(),
                estado = departamentoDto.Estado
                // claveDepartamento y fecha_creacion los maneja la BD
            };

            return await _departamentoRepository.CreateAsync(entity);
        }

        public async Task UpdateAsync(string claveDepartamento, DepartamentoUpdateEstadoDto departamentoDto)
        {
            if (string.IsNullOrWhiteSpace(claveDepartamento))
                throw new ArgumentException("La clave del departamento no puede estar vacía.");

            if (string.IsNullOrWhiteSpace(departamentoDto.EstadoTexto))
                throw new ArgumentException("El estado es obligatorio. Use 'Activado' o 'Desactivado'.");

            await _departamentoRepository.UpdateAsync(claveDepartamento, departamentoDto);
        }

        public async Task DeleteAsync(string claveDepartamento)
        {
            if (string.IsNullOrWhiteSpace(claveDepartamento))
                throw new ArgumentException("La clave del departamento no puede estar vacía.");

            await _departamentoRepository.DeleteAsync(claveDepartamento);
        }
    }

}
