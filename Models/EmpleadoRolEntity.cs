namespace ReyesAR.Models
{
    public class EmpleadoRolEntity
    {
        public string claveEmpleado { get; set; }
        public string claveRol { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime? fecha_fin { get; set; }
        public EmpleadoEntity Empleado { get; set; }
        public RolEntity Rol { get; set; }
    }
}
