using ReyesAR.DTO;
using ReyesAR.Models;

public interface ICompraRepository
{
    // 1. Retorna el listado mapeado a DTO para vista de tablas
    Task<IEnumerable<CompraDTO>> GetAllAsync();

    // 2. Retorna el detalle mapeado a DTO (incluye URLs de imágenes)
    Task<CompraDTO> GetByIdAsync(string claveCompra);

    // 3. Ejecuta el SP "registrar_compra" y retorna la Entidad resultante
    // Usamos el DTO de entrada que ya trae las URLs procesadas
    Task<CompraDTO> CreateAsync(CompraDTO compra);

    // 4. Cambia el estado y retorna la entidad actualizada
    Task<CompraDTO> UpdateEstadoAsync(string claveCompra, string nuevoEstado);

    // 5. Realiza la cancelación lógica y retorna la entidad con el nuevo estado
    Task<CompraDTO> CancelarAsync(string claveCompra);
}