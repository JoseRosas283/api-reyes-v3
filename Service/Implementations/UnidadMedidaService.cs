using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using ReyesAR.Service.Interface;

namespace ReyesAR.Service.Implementations
{
    public class UnidadMedidaService : IUnidadMedidaService
    {


        private readonly IUnidadMedidaRepository _repository;

        // Inyección del repositorio por constructor
        public UnidadMedidaService(IUnidadMedidaRepository repository)
        {
            _repository = repository;
        }

        // GET → Obtener lista completa
        public async Task<IEnumerable<UnidadMedidaEntity>> ListarUnidadesAsync()
        {
            return await _repository.GetAllAsync();
        }

        // GET → Obtener una por su clave
        public async Task<UnidadMedidaEntity?> ObtenerPorIdAsync(string claveUnidad)
        {
            if (string.IsNullOrWhiteSpace(claveUnidad)) return null;
            return await _repository.GetByIdAsync(claveUnidad);
        }

        // POST → Registrar nueva unidad
        public async Task<UnidadMedidaEntity> RegistrarUnidadAsync(UnidadMedidaDTO dto)
        {
            if (dto == null)
                throw new ArgumentException("Los datos de la unidad son obligatorios.");

            if (string.IsNullOrWhiteSpace(dto.nombreUnidad))
                throw new ArgumentException("El nombre de la unidad es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.abreviatura))
                throw new ArgumentException("La abreviatura es obligatoria.");

            var unidad = new UnidadMedidaEntity
            {
                nombreUnidad = dto.nombreUnidad.Trim().ToUpper(),
                abreviatura = dto.abreviatura.Trim(),
                tipoStock = dto.tipoStock,
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
        public async Task UpdateAsync(string claveUnidad, UnidadMedidaUpdateEstadoDto dto)
        {
            if (string.IsNullOrWhiteSpace(claveUnidad))
                throw new ArgumentException("La clave de unidad es obligatoria.");

            if (dto == null || string.IsNullOrWhiteSpace(dto.EstadoTexto))
                throw new ArgumentException("El estado es obligatorio para actualizar.");

            try
            {
                await _repository.UpdateAsync(claveUnidad, dto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // DELETE → Eliminar unidad
        public async Task EliminarUnidadAsync(string claveUnidad)
        {
            if (string.IsNullOrWhiteSpace(claveUnidad))
                throw new ArgumentException("La clave de unidad es obligatoria.");

            try
            {
                await _repository.DeleteAsync(claveUnidad);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }










    }
}
