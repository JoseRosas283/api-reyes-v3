using ReyesAR.DTO;

public interface ICompraService
{
    // 1. Obtener listado mapeado
    Task<IEnumerable<CompraDTO>> GetAllAsync();

    // 2. Obtener detalle por clave
    Task<CompraDTO> GetByIdAsync(string claveCompra);

    // 3. El método principal: Procesa imágenes y llama al Repo
    Task<CompraDTO> CreateAsync(CompraDTO compra);

    // 4. Actualizar estados
    Task<CompraDTO> UpdateEstadoAsync(string claveCompra, string nuevoEstado);

    // 5. Cancelación
    Task<CompraDTO> CancelarAsync(string claveCompra);
}