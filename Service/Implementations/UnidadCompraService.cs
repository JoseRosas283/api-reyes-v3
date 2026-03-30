using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using ReyesAR.Service.Interface;

namespace ReyesAR.Service.Implementations
{
    public class UnidadCompraService : IUnidadCompraService
    {
        private readonly IUnidadCompraRepository _repository;

        // Inyección del repositorio por constructor
        public UnidadCompraService(IUnidadCompraRepository repository)
        {
            _repository = repository;
        }

        // GET → Obtener lista completa
        public async Task<IEnumerable<UnidadCompraEntity>> ListarUnidadesAsync()
        {
            return await _repository.GetAllAsync();
        }

        // GET → Obtener una por su clave
        public async Task<UnidadCompraEntity?> ObtenerPorIdAsync(string claveUnidadCompra)
        {
            if (string.IsNullOrWhiteSpace(claveUnidadCompra)) return null;
            return await _repository.GetByIdAsync(claveUnidadCompra);
        }

        // POST → Registrar nueva unidad
        public async Task<UnidadCompraEntity> RegistrarUnidadAsync(UnidadCompraDTO dto)
        {
            if (dto == null)
                throw new ArgumentException("Los datos de la unidad de compra son obligatorios.");

            if (string.IsNullOrWhiteSpace(dto.nombre_unidad))
                throw new ArgumentException("El nombre de la unidad de compra es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.abreviatura))
                throw new ArgumentException("La abreviatura es obligatoria.");

            if (string.IsNullOrWhiteSpace(dto.tipo_stock))
                throw new ArgumentException("El tipo de stock es obligatorio.");

            var unidad = new UnidadCompraEntity
            {
                nombre_unidad = dto.nombre_unidad.Trim(),
                abreviatura = dto.abreviatura.Trim().ToLower(),
                tipo_stock = dto.tipo_stock.Trim().ToLower(),
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
        public async Task UpdateAsync(string claveUnidadCompra, UnidadCompraUpdateEstadoDto dto)
        {
            if (string.IsNullOrWhiteSpace(claveUnidadCompra))
                throw new ArgumentException("La clave de unidad de compra es obligatoria.");

            if (dto == null || string.IsNullOrWhiteSpace(dto.EstadoTexto))
                throw new ArgumentException("El estado es obligatorio para actualizar.");

            try
            {
                await _repository.UpdateAsync(claveUnidadCompra, dto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // DELETE → Eliminar unidad
        public async Task EliminarUnidadAsync(string claveUnidadCompra)
        {
            if (string.IsNullOrWhiteSpace(claveUnidadCompra))
                throw new ArgumentException("La clave de unidad de compra es obligatoria.");

            try
            {
                await _repository.DeleteAsync(claveUnidadCompra);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
