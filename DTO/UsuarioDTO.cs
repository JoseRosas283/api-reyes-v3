using ReyesAR.Models;

namespace ReyesAR.DTO
{
    public class UsuarioDTO
    {
        public string? claveUsuario { get; set; }
        public string nombre_usuario { get; set; }
        public string contrasena { get; set; }
        public bool? estado { get; set; }
        public string claveEmpleado { get; set; }

        // Relación 1:1
        public EmpleadoEntity Empleado { get; set; }
    }
}
