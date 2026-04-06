namespace ReyesAR.Models
{
    public class EntradaEntity
    {
        public string claveEntrada { get; set; }

        public decimal cantidad_recibida { get; set; }

        public DateTime fecha { get; set; }

        public string tipo_entrada { get; set; }

        public DateTime? fecha_caducidad { get; set; }

        public string? observaciones { get; set; }

        public bool? extra { get; set; }

        public string claveCompra { get; set; }

        public string claveProducto { get; set; }

        public string claveUsuario { get; set; }

        // Propiedades de Navegación
        public virtual CompraEntity Compra { get; set; }
        public virtual ProductoEntity Producto { get; set; }
        public virtual UsuarioEntity Usuario { get; set; }
    }
}
