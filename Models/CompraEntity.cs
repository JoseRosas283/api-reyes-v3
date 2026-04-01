namespace ReyesAR.Models
{
    // Entidad Padre (General)
    public class CompraEntity
    {
        public string ClaveCompra { get; set; } 
        public DateTime fecha_compra { get; set; }
        public string tipo_compra { get; set; } 
        public decimal total { get; set; }
        public string? forma_pago { get; set; }
        public string? ClaveUsuario { get; set; }
        public virtual CompraPedidoEntity? CompraPedido { get; set; }
        public virtual CompraDirectaEntity? CompraDirecta { get; set; }
        public virtual UsuarioEntity Usuario { get; set; }
    }

    // Subentidad: Comprapedido (Especialización)
    public class CompraPedidoEntity
    {
        public string claveCompra { get; set; }
        public string? folio { get; set; }
        public string? factura_proveedor { get; set; }
        public string claveEntrega { get; set; } 
        public string estado_cumplimiento { get; set; }
        public string estado_operativo { get; set; } 

        // Mismo estilo que Proveedor: La subentidad referencia al padre
        public virtual CompraEntity? Compra { get; set; }
        public virtual EntregaEntity? Entrega { get; set; }
        public virtual ICollection<DetalleEntregaEntity> DetalleEntregaProducto { get; set; } = new List<DetalleEntregaEntity>();
    }

    // Subentidad: Compradirecta (Especialización)
    public class CompraDirectaEntity
    {
        public string claveCompra { get; set; } 
        public string? referencia_pago { get; set; }
        public string? documento_referencia { get; set; }
        public string origen { get; set; } 
        public string estado { get; set; }

        // Mismo estilo que Proveedor: La subentidad referencia al padre
        public virtual CompraEntity? Compra { get; set; }
    }
}
