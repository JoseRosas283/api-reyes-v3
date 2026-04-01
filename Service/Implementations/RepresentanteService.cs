using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;

public class RepresentanteService : IRepresentanteService
{
    private readonly IRepresentanteRepository _repository;
    private readonly IProveedorRepository _proveedorRepository; // Inyectamos para validar existencia

    public RepresentanteService(IRepresentanteRepository repository, IProveedorRepository proveedorRepository)
    {
        _repository = repository;
        _proveedorRepository = proveedorRepository;
    }

    public async Task<IEnumerable<RepresentanteEntity>> GetAllRepresentantesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<RepresentanteEntity?> GetRepresentanteByIdAsync(string claveRepresentante)
    {
        if (string.IsNullOrWhiteSpace(claveRepresentante))
            throw new ArgumentException("La clave del representante es obligatoria.");

        return await _repository.GetByIdAsync(claveRepresentante);
    }

    public async Task<IEnumerable<RepresentanteEntity>> GetRepresentantesByProveedorAsync(string claveProveedor)
    {
        if (string.IsNullOrWhiteSpace(claveProveedor))
            throw new ArgumentException("La clave del proveedor es obligatoria para filtrar.");

        return await _repository.GetByProveedorAsync(claveProveedor);
    }

    public async Task<RepresentanteEntity> CreateRepresentanteAsync(RepresentanteDTO representanteDto)
    {
        // 1. Validaciones de nulos y vacíos (Básicas)
        if (representanteDto == null) throw new ArgumentNullException(nameof(representanteDto));

        if (string.IsNullOrWhiteSpace(representanteDto.nombre) ||
            string.IsNullOrWhiteSpace(representanteDto.apellido_paterno) ||
            string.IsNullOrWhiteSpace(representanteDto.telefono) ||
            string.IsNullOrWhiteSpace(representanteDto.claveProveedor) ||
            string.IsNullOrWhiteSpace(representanteDto.tipo_representante))
        {
            throw new ArgumentException("Error: Todos los campos obligatorios deben estar llenos.");
        }

        // 2. Validar que el proveedor existe antes de intentar el SP
        var proveedorExistente = await _proveedorRepository.GetByIdAsync(representanteDto.claveProveedor);
        if (proveedorExistente == null)
            throw new KeyNotFoundException($"El proveedor con clave {representanteDto.claveProveedor} no existe.");

        // 3. Mapeo a Entidad para enviar al Repo
        var entidad = new RepresentanteEntity
        {
            nombre = representanteDto.nombre.Trim(),
            apellido_paterno = representanteDto.apellido_paterno.Trim(),
            apellido_materno = representanteDto.apellido_materno?.Trim(),
            telefono = representanteDto.telefono.Trim(),
            claveProveedor = representanteDto.claveProveedor.Trim(),
            tipo_representante = representanteDto.tipo_representante.Trim(),
            estado = true
        };

        try
        {
            // El repo ejecuta el CALL "insertar_representante"
            return await _repository.CreateAsync(entidad);
        }
        catch (Exception ex)
        {
            // Captura los RAISE EXCEPTION de Postgres (Compatibilidad de tipos, etc.)
            throw new InvalidOperationException($"Error de Base de Datos: {ex.Message}");
        }
    }

    public async Task<RepresentanteEntity> UpdateRepresentanteAsync(string claveRepresentante, RepresentanteDTO representanteDto)
    {
        if (string.IsNullOrWhiteSpace(claveRepresentante))
            throw new ArgumentException("Clave de representante no válida.");

        var existente = await _repository.GetByIdAsync(claveRepresentante);
        if (existente == null)
            throw new KeyNotFoundException("No se encontró el representante para actualizar.");

        // Mapeo selectivo (solo campos permitidos)
        existente.nombre = representanteDto.nombre;
        existente.apellido_paterno = representanteDto.apellido_paterno;
        existente.apellido_materno = representanteDto.apellido_materno;
        existente.telefono = representanteDto.telefono;
        existente.estado = representanteDto.estado;

        await _repository.UpdateAsync(claveRepresentante, existente);
        return existente;
    }

    public async Task<RepresentanteEntity> DeleteRepresentanteAsync(string claveRepresentante)
    {
        if (string.IsNullOrWhiteSpace(claveRepresentante))
            throw new ArgumentException("Clave de representante no válida.");

        var entidad = await _repository.GetByIdAsync(claveRepresentante);
        if (entidad == null)
            throw new KeyNotFoundException("El registro que intenta eliminar no existe.");

        await _repository.DeleteAsync(claveRepresentante);
        return entidad;
    }
}