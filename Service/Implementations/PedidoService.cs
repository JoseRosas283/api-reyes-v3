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

    public async Task<IEnumerable<PedidoEntity>> GetAllAsync()
    {
        return await _pedidoRepository.GetAllAsync();
    }

    public async Task<PedidoEntity?> GetByIdAsync(string clavePedido)
    {
        if (string.IsNullOrWhiteSpace(clavePedido))
            throw new ArgumentException("La clave del pedido no puede estar vacía.");

        return await _pedidoRepository.GetByIdAsync(clavePedido);
    }

    public async Task<PedidoEntity> CreateAsync(PedidoDTO pedidoDto)
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
        return await _pedidoRepository.CreateAsync(nuevaEntidad);
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
            dto.estado = "Pendiente"; // Valor por defecto si viene vacío
    }
}