using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using ReyesAR.Service.Interface;

namespace ReyesAR.Service.Implementations
{

    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        // Listar todas las categorías
        public async Task<IEnumerable<CategoriaEntity>> ListarCategoriasAsync()
        {
            var entidades = await _repository.GetAllAsync();

            if (entidades == null || !entidades.Any())
                throw new InvalidOperationException("No hay categorías registradas.");

            return entidades;
        }

        // Obtener categoría por clave
        public async Task<CategoriaEntity?> ObtenerPorIdAsync(string clave)
        {
            if (string.IsNullOrWhiteSpace(clave))
                throw new ArgumentException("La clave es obligatoria");

            var entidad = await _repository.GetByIdAsync(clave);

            if (entidad == null)
                throw new InvalidOperationException("Categoría no encontrada");

            return entidad;
        }

        // Registrar nueva categoría (recibe DTO y retorna Entity con PK)
        public async Task<CategoriaEntity> RegistrarCategoriaAsync(CategoriaDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.nombre))
                throw new ArgumentException("El NOMBRE de la categoría es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.descripcion))
                throw new ArgumentException("La DESCRIPCIÓN de la categoría es obligatoria.");

            if (string.IsNullOrWhiteSpace(dto.claveDepartamento))
                throw new ArgumentException("La CLAVE DEPARTAMENTO es obligatoria.");

            var nuevaEntidad = new CategoriaEntity
            {
                nombre = dto.nombre.Trim(),
                descripcion = dto.descripcion.Trim(),
                claveDepartamento = dto.claveDepartamento.Trim(),
                estado = dto.estado // ← nuevo campo
            };

            return await _repository.CreateAsync(nuevaEntidad);
        }

        // Actualizar categoría existente
        public async Task ActualizarCategoriaAsync(string clave, CategoriaDTO dto)
        {
            if (string.IsNullOrWhiteSpace(clave))
                throw new ArgumentException("La clave es requerida.");

            var existe = await _repository.GetByIdAsync(clave);
            if (existe == null)
                throw new InvalidOperationException("La categoría no existe.");

            var entidadActualizada = new CategoriaEntity
            {
                claveCategoria = clave.Trim(),
                nombre = dto.nombre.Trim(),
                descripcion = dto.descripcion.Trim(),
                claveDepartamento = dto.claveDepartamento.Trim(),
                estado = dto.estado
            };

            await _repository.UpdateAsync(clave, entidadActualizada);
        }

        // Eliminar categoría
        public async Task EliminarCategoriaAsync(string clave)
        {
            if (string.IsNullOrWhiteSpace(clave))
                throw new ArgumentException("Se requiere una clave válida.");

            await _repository.DeleteAsync(clave);
        }
    }


}
