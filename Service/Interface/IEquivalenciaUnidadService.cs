using ReyesAR.DTO;

public interface IEquivalenciaUnidadService
{
    // 1. Obtener todas las equivalencias (Mapeadas a DTO)
    Task<IEnumerable<EquivalenciaUnidadDTO>> GetAllEquivalenciasAsync();

    // 2. Obtener una específica por su clave compuesta
    Task<EquivalenciaUnidadDTO?> GetEquivalenciaByIdAsync(string claveProducto, string claveUnidadCompra);

    // 3. CREAR: Recibe el DTO y retorna el DTO procesado
    Task<EquivalenciaUnidadDTO> CreateEquivalenciaAsync(EquivalenciaUnidadDTO dto);

    // 4. Actualizar el factor de conversión
    Task UpdateEquivalenciaAsync(string claveProducto, string claveUnidadVenta, EquivalenciaUnidadDTO dto);

    // 5. Eliminar la relación
    Task DeleteEquivalenciaAsync(string claveProducto, string claveUnidadCompra);
    
}