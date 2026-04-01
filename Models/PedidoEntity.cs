namespace ReyesAR.Models
{
    public class PedidoEntity
    {
        // Datos del Pedido
        public string clavePedido { get; set; } = null!;
        public DateTime fecha_pedido { get; set; } = DateTime.Now;
        public string estado { get; set; } = "Pendiente";
        public string observaciones { get; set; } = null!;

        // FK exacta según tu SQL
        public string claveUsuario { get; set; } = null!;
        public string tipo_pedido { get; set; } = null!;
        public string claveProveedor { get; set; } = null!;
        public decimal total { get; set; } = 0;

        // Propiedades de Navegación
        // Asumiendo que tu entidad de usuarios se llama UsuarioEntity
        public virtual UsuarioEntity Usuario { get; set; } = null!;
        public virtual ProveedorEntity Proveedor { get; set; } = null!;
        public virtual ICollection<DetallePedidoEntity> Detalles { get; set; } = new List<DetallePedidoEntity>();
        public virtual EntregaEntity? Entrega { get; set; }
    }
}
