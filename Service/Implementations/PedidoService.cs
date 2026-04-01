using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using ReyesAR.Service.Interface;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;

    public PedidoService(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<IEnumerable<PedidoDTO>> GetAllAsync()
    {
        var pedidos = await _pedidoRepository.GetAllAsync();
        return pedidos.Select(MapearADTO);              // ✅ Entity → DTO
    }

    public async Task<PedidoDTO?> GetByIdAsync(string clavePedido)
    {
        if (string.IsNullOrWhiteSpace(clavePedido))
            throw new ArgumentException("La clave del pedido no puede estar vacía.");

        var pedido = await _pedidoRepository.GetByIdAsync(clavePedido);
        return pedido == null ? null : MapearADTO(pedido); // ✅ Entity → DTO
    }

    public async Task<PedidoDTO> CreateAsync(PedidoDTO pedidoDto)
    {
        // 1. Validaciones básicas de campos Null o Vacíos
        ValidarPedido(pedidoDto);

        // 2. Mapeo Manual de DTO a Entity (Para el Repositorio)
        var nuevaEntidad = new PedidoEntity
        {
            estado = pedidoDto.estado.Trim(),
            observaciones = pedidoDto.observaciones.Trim(),
            claveUsuario = pedidoDto.claveUsuario.Trim(),
            claveProveedor = pedidoDto.claveProveedor.Trim(),
            tipo_pedido = pedidoDto.tipo_pedido.Trim(),
            total = pedidoDto.total
        };

        // 3. Llamar al repositorio (que ejecuta el SP insertar_pedido)
        var entidadCreada = await _pedidoRepository.CreateAsync(nuevaEntidad);
        return MapearADTO(entidadCreada);              // ✅ Entity → DTO
    }

    public async Task UpdateAsync(string clavePedido, PedidoDTO pedidoDto)
    {
        if (string.IsNullOrWhiteSpace(clavePedido))
            throw new ArgumentException("La clave de pedido es obligatoria para actualizar.");

        ValidarPedido(pedidoDto);
        await _pedidoRepository.UpdateAsync(clavePedido, pedidoDto);
    }

    public async Task DeleteAsync(string clavePedido)
    {
        if (string.IsNullOrWhiteSpace(clavePedido))
            throw new ArgumentException("Debe proporcionar una clave válida para eliminar.");

        await _pedidoRepository.DeleteAsync(clavePedido);
    }

    // ✅ Mapeo privado: corta el ciclo de navegación Entity → DTO
    private static PedidoDTO MapearADTO(PedidoEntity e) => new PedidoDTO
    {
        clavePedido = e.clavePedido,
        fecha_pedido = e.fecha_pedido,
        estado = e.estado,
        observaciones = e.observaciones,
        claveUsuario = e.claveUsuario,
        tipo_pedido = e.tipo_pedido,
        claveProveedor = e.claveProveedor,
        total = e.total
    };

    // --- MODO PRIVADO: Reglas de validación reutilizables ---
    private void ValidarPedido(PedidoDTO dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "El pedido no puede ser nulo.");
        if (string.IsNullOrWhiteSpace(dto.claveUsuario))
            throw new ArgumentException("La clave de usuario es obligatoria.");
        if (string.IsNullOrWhiteSpace(dto.claveProveedor))
            throw new ArgumentException("La clave de proveedor es obligatoria.");
        if (string.IsNullOrWhiteSpace(dto.tipo_pedido))
            throw new ArgumentException("El tipo de pedido es obligatorio.");
        if (dto.total < 0)
            throw new ArgumentException("El total del pedido no puede ser negativo.");
        if (string.IsNullOrWhiteSpace(dto.estado))
            dto.estado = "Pendiente";
    }
}