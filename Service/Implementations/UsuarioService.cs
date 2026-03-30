using ReyesAR.DTO;
using ReyesAR.Models;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repository;

    public UsuarioService(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    // 1. Obtener todos los usuarios mapeados a DTO
    public async Task<IEnumerable<UsuarioDTO>> GetAllUsuariosAsync()
    {
        var usuarios = await _repository.GetAllAsync();
        return usuarios.Select(u => new UsuarioDTO
        {
            claveUsuario = u.claveUsuario,
            nombre_usuario = u.nombre_usuario,
            estado = u.estado,
            claveEmpleado = u.claveEmpleado
        });
    }

    // 2. Buscar por ID
    public async Task<UsuarioDTO?> GetUsuarioByIdAsync(string claveUsuario)
    {
        if (string.IsNullOrWhiteSpace(claveUsuario)) return null;

        var u = await _repository.GetByIdAsync(claveUsuario);
        if (u == null) return null;

        return new UsuarioDTO
        {
            claveUsuario = u.claveUsuario,
            nombre_usuario = u.nombre_usuario,
            estado = u.estado,
            claveEmpleado = u.claveEmpleado
        };
    }

    // 3. Buscar por Nombre de Usuario (Login)
    public async Task<UsuarioDTO?> GetUsuarioByUsernameAsync(string nombre_usuario)
    {
        if (string.IsNullOrWhiteSpace(nombre_usuario)) return null;

        var u = await _repository.GetByUsernameAsync(nombre_usuario);
        if (u == null) return null;

        return new UsuarioDTO
        {
            claveUsuario = u.claveUsuario,
            nombre_usuario = u.nombre_usuario,
            estado = u.estado,
            claveEmpleado = u.claveEmpleado
        };
    }

    // 4. CREAR: Valida campos y ejecuta el SP "insertar_usuario"
    public async Task<UsuarioDTO> CreateUsuarioAsync(UsuarioDTO usuarioDto)
    {
        // --- VALIDACIONES BÁSICAS DE CAMPOS NULL O VACÍOS ---
        if (string.IsNullOrWhiteSpace(usuarioDto.nombre_usuario))
            throw new ArgumentException("El nombre de usuario es requerido.");

        if (string.IsNullOrWhiteSpace(usuarioDto.contrasena))
            throw new ArgumentException("La contraseña es requerida.");

        if (string.IsNullOrWhiteSpace(usuarioDto.claveEmpleado))
            throw new ArgumentException("Debe asignar un empleado al usuario.");

        // Mapeo a Entity para el repositorio
        var entidad = new UsuarioEntity
        {
            nombre_usuario = usuarioDto.nombre_usuario.Trim(),
            contrasena = usuarioDto.contrasena.Trim(),
            claveEmpleado = usuarioDto.claveEmpleado.Trim()
        };

        try
        {
            // El repositorio llama a: CALL "insertar_usuario"(...)
            var resultado = await _repository.CreateAsync(entidad);

            // Retornamos el DTO con la clave generada por Postgres
            usuarioDto.claveUsuario = resultado.claveUsuario;
            return usuarioDto;
        }
        catch (Exception ex)
        {
            // Aquí capturamos los RAISE EXCEPTION del SP (Longitud, Rol Intendente, Duplicados)
            throw new InvalidOperationException($"No se pudo crear el usuario: {ex.Message}");
        }
    }

    // 5. ACTUALIZAR (Promesa)
    public async Task UpdateUsuarioAsync(string claveUsuario, UsuarioDTO usuarioDto)
    {
        if (string.IsNullOrWhiteSpace(claveUsuario))
            throw new ArgumentException("ID de usuario no proporcionado.");

        var entidad = new UsuarioEntity
        {
            nombre_usuario = usuarioDto.nombre_usuario?.Trim(),
            contrasena = usuarioDto.contrasena?.Trim(),
            estado = usuarioDto.estado,
            claveEmpleado = usuarioDto.claveEmpleado?.Trim()
        };

        await _repository.UpdateAsync(claveUsuario, entidad);
    }

    // 6. ELIMINAR (Promesa)
    public async Task DeleteUsuarioAsync(string claveUsuario)
    {
        if (string.IsNullOrWhiteSpace(claveUsuario))
            throw new ArgumentException("ID de usuario no proporcionado.");

        await _repository.DeleteAsync(claveUsuario);
    }
}