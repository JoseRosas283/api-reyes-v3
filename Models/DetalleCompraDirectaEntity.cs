namespace ReyesAR.Models
{
    public class DetalleCompraDirectaEntity
    {
        public string claveCompra { get; set; }
        public string claveProducto { get; set; }
        public decimal cantidad { get; set; }
        public double precioUnitario { get; set; }
        public string? impuestos { get; set; }
        public decimal subtotal { get; set; }
        public string claveUnidadCompra { get; set; }

        public virtual CompraDirectaEntity CompraDirecta { get; set; }
        public virtual EquivalenciaUnidadEntity EquivalenciaUnidad { get; set; }

        // Relación lógica hacia el maestro de productos
        public virtual ProductoEntity Producto { get; set; }
    }
}
