using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using ReyesAR.Service.Interface;

public class DetallePedidoService : IDetallePedidoService
{
    private readonly IDetallePedidoRepository _repository;

    public DetallePedidoService(IDetallePedidoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DetallePedidoEntity>> GetAllDetailsAsync()
    {
        // Llama al repo: GetAllAsync
        return await _repository.GetAllAsync();
    }

    public async Task<DetallePedidoEntity?> GetDetailByIdAsync(string clavePedido, string claveProducto)
    {
        if (string.IsNullOrWhiteSpace(clavePedido) || string.IsNullOrWhiteSpace(claveProducto))
            throw new ArgumentException("Las claves de pedido y producto no pueden estar vacías.");

        // Llama al repo: GetByIdAsync
        return await _repository.GetByIdAsync(clavePedido, claveProducto);
    }

    public async Task<DetallePedidoEntity> CreateDetailAsync(DetallePedidoDTO detalleDto)
    {
        if (detalleDto == null)
            throw new ArgumentNullException(nameof(detalleDto));

        if (string.IsNullOrWhiteSpace(detalleDto.Clavepedido) || string.IsNullOrWhiteSpace(detalleDto.Claveproducto))
            throw new ArgumentException("Datos incompletos: clavePedido y claveProducto son obligatorios.");

        // Mapeo de DTO a Entidad para el repo
        var entidad = new DetallePedidoEntity
        {
            Clavepedido = detalleDto.Clavepedido,
            Claveproducto = detalleDto.Claveproducto,
            cantidad = detalleDto.cantidad,
            descripcion = detalleDto.descripcion,
            tipo_detalle = detalleDto.tipo_detalle
        };

        // Llama al repo: CreateAsync
        return await _repository.CreateAsync(entidad);
    }

    public async Task<DetallePedidoEntity> UpdateDetailAsync(string clavePedido, string claveProducto, DetallePedidoDTO detalleDto)
    {
        if (string.IsNullOrWhiteSpace(clavePedido) || string.IsNullOrWhiteSpace(claveProducto))
            throw new ArgumentException("Las llaves son obligatorias para actualizar.");

        // El repo UpdateAsync es Task (void), así que lo ejecutamos
        await _repository.UpdateAsync(clavePedido, claveProducto, detalleDto);

        // Para cumplir con tu contrato de Service que pide retornar la entidad:
        var actualizado = await _repository.GetByIdAsync(clavePedido, claveProducto);
        if (actualizado == null) throw new Exception("Error al recuperar el registro actualizado.");

        return actualizado;
    }

    public async Task<DetallePedidoEntity> DeleteDetailAsync(string clavePedido, string claveProducto)
    {
        if (string.IsNullOrWhiteSpace(clavePedido) || string.IsNullOrWhiteSpace(claveProducto))
            throw new ArgumentException("Las llaves son obligatorias para eliminar.");

        // Obtenemos la entidad antes de borrarla para poder retornarla
        var entidadAEliminar = await _repository.GetByIdAsync(clavePedido, claveProducto);

        if (entidadAEliminar == null)
            throw new KeyNotFoundException("No se encontró el detalle para eliminar.");

        // Llama al repo: DeleteAsync
        await _repository.DeleteAsync(clavePedido, claveProducto);

        return entidadAEliminar;
    }
}