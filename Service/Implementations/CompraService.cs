using ReyesAR.DTO;
using ReyesAR.Service.Interface;

public class CompraService : ICompraService
{
    private readonly ICompraRepository _repository;
    private readonly IImagenService _imagenService;

    public CompraService(ICompraRepository repository, IImagenService imagenService)
    {
        _repository = repository;
        _imagenService = imagenService;
    }

    public async Task<CompraDTO> CreateAsync(CompraDTO compra)
    {
        // 1. Validaciones de Negocio Base
        if (compra.total <= 0)
            throw new ArgumentException("El total de la compra debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(compra.ClaveUsuario))
            throw new ArgumentException("La ClaveUsuario es obligatoria.");

        // 2. Gestión de Imágenes (Flexibilidad Total)
        // No bloqueamos ninguna imagen. Si el archivo viene, se procesa a URL 
        // sea Pedido (Empresa/Independiente) o Directa (Mercado/Centro Comercial).

        if (compra.tipo_compra == "Pedido")
        {
            if (compra.facturaImagen != null && compra.facturaImagen.Length > 0)
            {
                // Si el usuario adjunta algo en Independiente, se guarda igual que en Empresa.
                compra.facturaImagenUrl = await _imagenService.GuardarImagenAsync(compra.facturaImagen);
            }
        }
        else if (compra.tipo_compra == "Directa")
        {
            if (compra.documentoImagen != null && compra.documentoImagen.Length > 0)
            {
                // Si el usuario adjunta algo en Mercado, se guarda igual que en Centro Comercial.
                compra.documentoImagenUrl = await _imagenService.GuardarImagenAsync(compra.documentoImagen);
            }
        }

        // 3. Persistencia
        // El Repositorio enviará la URL generada o NULL al SP "registrar_compra".
        // El SP se encargará de validar la obligatoriedad final según tus reglas de Postgres.
        try
        {
            return await _repository.CreateAsync(compra);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en el registro de compra: {ex.Message}");
        }
    }

    public async Task<IEnumerable<CompraDTO>> GetAllAsync()
    {
        try
        {
            var resultado = await _repository.GetAllAsync();
            return resultado ?? Enumerable.Empty<CompraDTO>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener compras: {ex.Message}");
        }
    }

    public async Task<CompraDTO> GetByIdAsync(string claveCompra)
    {
        if (string.IsNullOrWhiteSpace(claveCompra))
            throw new ArgumentException("La claveCompra no puede estar vacía.");

        var compra = await _repository.GetByIdAsync(claveCompra);
        if (compra == null)
            throw new KeyNotFoundException($"No existe la compra con clave: {claveCompra}");

        return compra;
    }

    public async Task<CompraDTO> UpdateEstadoAsync(string claveCompra, string nuevoEstado)
    {
        if (string.IsNullOrWhiteSpace(claveCompra))
            throw new ArgumentException("La claveCompra es obligatoria para actualizar.");

        return await _repository.UpdateEstadoAsync(claveCompra, nuevoEstado);
    }

    public async Task<CompraDTO> CancelarAsync(string claveCompra)
    {
        if (string.IsNullOrWhiteSpace(claveCompra))
            throw new ArgumentException("La claveCompra es obligatoria para cancelar.");

        return await _repository.CancelarAsync(claveCompra);
    }
}