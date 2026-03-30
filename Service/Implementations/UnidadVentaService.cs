using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using ReyesAR.Service.Interface;

namespace ReyesAR.Service.Implementations
{
    public class UnidadVentaService : IUnidadVentaService
    {
        private readonly IUnidadVentaRepository _repository;

        // Inyección del repositorio por constructor
        public UnidadVentaService(IUnidadVentaRepository repository)
        {
            _repository = repository;
        }

        // GET → Obtener lista completa
        public async Task<IEnumerable<UnidadVentaEntity>> ListarUnidadesAsync()
        {
            return await _repository.GetAllAsync();
        }

        // GET → Obtener una por su clave
        public async Task<UnidadVentaEntity?> ObtenerPorIdAsync(string claveUnidadVenta)
        {
            if (string.IsNullOrWhiteSpace(claveUnidadVenta)) return null;
            return await _repository.GetByIdAsync(claveUnidadVenta);
        }

        // POST → Registrar nueva unidad
        public async Task<UnidadVentaEntity> RegistrarUnidadAsync(UnidadVentaDTO dto)
        {
            if (dto == null)
                throw new ArgumentException("Los datos de la unidad de venta son obligatorios.");

            if (string.IsNullOrWhiteSpace(dto.nombreUnidad))
                throw new ArgumentException("El nombre de la unidad de venta es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.abreviatura))
                throw new ArgumentException("La abreviatura es obligatoria.");

            if (string.IsNullOrWhiteSpace(dto.tipoValor))
                throw new ArgumentException("El tipo de valor es obligatorio.");

            var unidad = new UnidadVentaEntity
            {
                nombreUnidad = dto.nombreUnidad.Trim().ToUpper(),
                abreviatura = dto.abreviatura.Trim(),
                tipoValor = dto.tipoValor.Trim().ToLower(),
                estado = dto.estado
            };

            try
            {
                return await _repository.CreateAsync(unidad);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // PUT → Actualizar solo el estado
        public async Task UpdateAsync(string claveUnidadVenta, UnidadVentaUpdateEstadoDto dto)
        {
            if (string.IsNullOrWhiteSpace(claveUnidadVenta))
                throw new ArgumentException("La clave de unidad de venta es obligatoria.");

            if (dto == null || string.IsNullOrWhiteSpace(dto.EstadoTexto))
                throw new ArgumentException("El estado es obligatorio para actualizar.");

            try
            {
                await _repository.UpdateAsync(claveUnidadVenta, dto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // DELETE → Eliminar unidad
        public async Task EliminarUnidadAsync(string claveUnidadVenta)
        {
            if (string.IsNullOrWhiteSpace(claveUnidadVenta))
                throw new ArgumentException("La clave de unidad de venta es obligatoria.");

            try
            {
                await _repository.DeleteAsync(claveUnidadVenta);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
