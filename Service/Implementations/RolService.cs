using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using ReyesAR.Service.Interface;

public class RolService : IRolService
{
    private readonly IRolRepository _rolRepository;
    public RolService(IRolRepository rolRepository)
    {
        _rolRepository = rolRepository;
    }

    // Crear un nuevo rol
    public async Task<RolDTO> CreateAsync(RolDTO rolDto)
    {
        var entity = new RolEntity
        {
            nombre = rolDto.nombre,
            descripcion = rolDto.descripcion,
            activo = rolDto.activo,
            fecha_creacion = rolDto.fecha_creacion
        };
        var creado = await _rolRepository.CreateAsync(entity);
        return new RolDTO
        {
            ClaveRol = creado.ClaveRol,
            nombre = creado.nombre,
            descripcion = creado.descripcion,
            activo = creado.activo,
            fecha_creacion = creado.fecha_creacion
        };
    }

    // Obtener un rol por clave
    public async Task<RolDTO> GetByIdAsync(string claveRol)
    {
        var rol = await _rolRepository.GetByIdAsync(claveRol);
        if (rol == null) return null;
        return new RolDTO
        {
            ClaveRol = rol.ClaveRol,
            nombre = rol.nombre,
            descripcion = rol.descripcion,
            activo = rol.activo,
            fecha_creacion = rol.fecha_creacion
        };
    }

    // Listar todos los roles
    public async Task<IEnumerable<RolDTO>> GetAllAsync()
    {
        var roles = await _rolRepository.GetAllAsync();
        return roles.Select(r => new RolDTO
        {
            ClaveRol = r.ClaveRol,
            nombre = r.nombre,
            descripcion = r.descripcion,
            activo = r.activo,
            fecha_creacion = r.fecha_creacion
        });
    }

    // Actualizar un rol
    public async Task<RolDTO> UpdateAsync(RolDTO rolDto)
    {
        var entity = new RolEntity
        {
            ClaveRol = rolDto.ClaveRol,
            nombre = rolDto.nombre,
            descripcion = rolDto.descripcion,
            activo = rolDto.activo,
            fecha_creacion = rolDto.fecha_creacion
        };
        var actualizado = await _rolRepository.UpdateAsync(entity);
        if (actualizado == null) return null;
        return new RolDTO
        {
            ClaveRol = actualizado.ClaveRol,
            nombre = actualizado.nombre,
            descripcion = actualizado.descripcion,
            activo = actualizado.activo,
            fecha_creacion = actualizado.fecha_creacion
        };
    }

    // Eliminar un rol
    public async Task<bool> DeleteAsync(string claveRol)
    {
        return await _rolRepository.DeleteAsync(claveRol);
    }
}