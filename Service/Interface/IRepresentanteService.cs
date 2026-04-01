using ReyesAR.DTO;
using ReyesAR.Models;

public interface IRepresentanteService
{
    // Obtener todos los representantes
    Task<IEnumerable<RepresentanteEntity>> GetAllRepresentantesAsync();

    // Buscar un representante por su clave única
    Task<RepresentanteEntity?> GetRepresentanteByIdAsync(string claveRepresentante);

    // Obtener representantes filtrados por proveedor
    Task<IEnumerable<RepresentanteEntity>> GetRepresentantesByProveedorAsync(string claveProveedor);

    // Crear un representante (Mapea el DTO a Entidad y llama al procedimiento almacenado)
    // Aquí es donde se capturan los RAISE EXCEPTION de Postgres
    Task<RepresentanteEntity> CreateRepresentanteAsync(RepresentanteDTO representanteDto);

    // Actualizar datos del representante
    Task<RepresentanteEntity> UpdateRepresentanteAsync(string claveRepresentante, RepresentanteDTO representanteDto);

    // Eliminar representante (Retorna la entidad eliminada para confirmación)
    Task<RepresentanteEntity> DeleteRepresentanteAsync(string claveRepresentante);
}