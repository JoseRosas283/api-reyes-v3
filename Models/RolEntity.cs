namespace ReyesAR.Models
{
    public class RolEntity
    {
        public string ClaveRol { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public bool activo { get; set; }
        public DateTime fecha_creacion { get; set; }

        // Colección para ver qué empleados han tenido este rol
        public ICollection<EmpleadoRolEntity> EmpleadosAsignados { get; set; }
    }
}
