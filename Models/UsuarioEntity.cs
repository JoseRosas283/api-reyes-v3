namespace ReyesAR.Models
{
    public class UsuarioEntity
    {
        public string claveUsuario { get; set; }
        public string nombre_usuario { get; set; }
        public string contrasena { get; set; }
        public bool? estado { get; set; }
        public string claveEmpleado { get; set; }

        // Relación 1:1
        public EmpleadoEntity Empleado { get; set; }

        public virtual ICollection<PedidoEntity> Pedidos { get; set; } = new List<PedidoEntity>();
        public virtual ICollection<EntregaEntity> Entregas { get; set; } = new List<EntregaEntity>();
    }
}
