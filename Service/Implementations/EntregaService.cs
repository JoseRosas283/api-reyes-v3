using ReyesAR.DTO;
using ReyesAR.Repositories.Interface;
using ReyesAR.Service.Interface;

public class EntregaService : IEntregaService
{
    private readonly IEntregaRepository _repository;

    public EntregaService(IEntregaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<EntregaEntity>> GetAllEntregasAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<EntregaEntity?> GetEntregaByClaveAsync(string claveEntrega)
    {
        if (string.IsNullOrWhiteSpace(claveEntrega)) return null;
        return await _repository.GetByClaveAsync(claveEntrega);
    }

    public async Task<EntregaEntity?> CreateEntregaAsync(EntregaDTO entregaDTO)
    {
        // Validaciones de negocio: No nulos, no vacíos
        if (entregaDTO == null ||
            string.IsNullOrWhiteSpace(entregaDTO.estado) ||
            string.IsNullOrWhiteSpace(entregaDTO.descripcion) ||
            string.IsNullOrWhiteSpace(entregaDTO.tipo_entrega) ||
            string.IsNullOrWhiteSpace(entregaDTO.clavePedido) ||
            string.IsNullOrWhiteSpace(entregaDTO.claveUsuario) ||
            string.IsNullOrWhiteSpace(entregaDTO.claveProveedor) ||
            entregaDTO.total <= 0)
        {
            throw new ArgumentException("Error: Faltan datos obligatorios o el total es inválido.");
        }

        // Mapeo manual de DTO a Entity
        var entregaEntity = new EntregaEntity
        {
            estado = entregaDTO.estado.Trim(),
            total = entregaDTO.total,
            descripcion = entregaDTO.descripcion.Trim(),
            tipo_entrega = entregaDTO.tipo_entrega.Trim().ToUpper(),
            clavePedido = entregaDTO.clavePedido.Trim(),
            claveUsuario = entregaDTO.claveUsuario.Trim(),
            claveProveedor = entregaDTO.claveProveedor.Trim(),
            claveRepresentante = string.IsNullOrWhiteSpace(entregaDTO.claveRepresentante)
                                 ? null : entregaDTO.claveRepresentante.Trim()
        };

        return await _repository.AddAsync(entregaEntity);
    }

    public async Task<EntregaEntity?> UpdateEntregaAsync(string claveEntrega, EntregaDTO entregaDTO)
    {
        if (string.IsNullOrWhiteSpace(claveEntrega)) return null;

        var existingEntrega = await _repository.GetByClaveAsync(claveEntrega);
        if (existingEntrega == null) return null;

        // Validar que los campos de actualización no vengan vacíos
        if (string.IsNullOrWhiteSpace(entregaDTO.estado) ||
            string.IsNullOrWhiteSpace(entregaDTO.descripcion))
        {
            throw new ArgumentException("Error: El estado y la descripción son obligatorios para actualizar.");
        }

        // Actualizamos solo los campos permitidos
        existingEntrega.estado = entregaDTO.estado.Trim();
        existingEntrega.descripcion = entregaDTO.descripcion.Trim();
        // El total y las llaves (Pedido/Proveedor) normalmente no se cambian en una entrega ya hecha

        return await _repository.UpdateAsync(existingEntrega);
    }

    public async Task<bool> DeleteEntregaAsync(string claveEntrega)
    {
        if (string.IsNullOrWhiteSpace(claveEntrega)) return false;

        var deleted = await _repository.DeleteAsync(claveEntrega);
        return deleted != null;
    }
}