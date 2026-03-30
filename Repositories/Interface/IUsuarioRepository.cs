using ReyesAR.Models;

public interface IUsuarioRepository
{
    // Obtener todos los usuarios (incluyendo datos del empleado)
    Task<IEnumerable<UsuarioEntity>> GetAllAsync();

    // Obtener un usuario por su clave primaria (claveUsuario)
    Task<UsuarioEntity?> GetByIdAsync(string claveUsuario);

    // Obtener un usuario por su nombre de login (Útil para el Auth)
    Task<UsuarioEntity?> GetByUsernameAsync(string nombre_usuario);

    // Obtener el usuario de un empleado específico (Relación 1:1)
    Task<UsuarioEntity?> GetByEmpleadoAsync(string claveEmpleado);

    // Crear un nuevo usuario (PostgreSQL ejecutará generar_clave_usuario)
    Task<UsuarioEntity> CreateAsync(UsuarioEntity usuario);

    // Actualizar datos del usuario (estado, contraseña, etc.)
    Task UpdateAsync(string claveUsuario, UsuarioEntity usuario);

    // Eliminación física o lógica según se requiera
    Task DeleteAsync(string claveUsuario);
}