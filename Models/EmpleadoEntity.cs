namespace ReyesAR.Models
{
    public class EmpleadoEntity
    {
        public string claveEmpleado { get; set; }
        public string nombre { get; set; }
        public string apellido_paterno { get; set; }
        public string apellido_materno { get; set; }
        public string telefono { get; set; }
        public string curp { get; set; }
        public bool estado { get; set; } = true;

        // Colección para el historial de roles
        public ICollection<EmpleadoRolEntity> RolesHistorial { get; set; }

        // Relación inversa para el 1:1 con Usuario
        public UsuarioEntity Usuario { get; set; }
    }
}
