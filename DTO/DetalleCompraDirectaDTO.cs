namespace ReyesAR.DTO
{
    public class DetalleCompraDirectaDTO
    {
        public string? claveCompra { get; set; }
        public string claveProducto { get; set; }
        public decimal cantidad { get; set; }
        public double precioUnitario { get; set; }
        public string? impuestos { get; set; }
        public decimal subtotal { get; set; }
        public string claveUnidadCompra { get; set; }
    }
}
