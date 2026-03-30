using ReyesAR.DTO;
public interface IUsuarioService
{
    // 1. Obtener todos los usuarios mapeados a DTO
    Task<IEnumerable<UsuarioDTO>> GetAllUsuariosAsync();

    // 2. Buscar un usuario por su clave primaria (claveUsuario)
    Task<UsuarioDTO?> GetUsuarioByIdAsync(string claveUsuario);

    // 3. Buscar un usuario por su nombre de usuario (Login)
    Task<UsuarioDTO?> GetUsuarioByUsernameAsync(string nombre_usuario);

    // 4. Crear el usuario (Aquí se captura la excepción si el SP "insertar_usuario" falla)
    Task<UsuarioDTO> CreateUsuarioAsync(UsuarioDTO usuarioDto);

    // 5. Actualizar el usuario (Contraseña, estado, etc.)
    Task UpdateUsuarioAsync(string claveUsuario, UsuarioDTO usuarioDto);

    // 6. Eliminar el usuario del sistema
    Task DeleteUsuarioAsync(string claveUsuario);
}