using ReyesAR.Models;

namespace ReyesAR.DTO
{
    public class EmpleadoRolDTO
    {
        public string claveEmpleado { get; set; }
        public string claveRol { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime? fecha_fin { get; set; }
        
    }
}
