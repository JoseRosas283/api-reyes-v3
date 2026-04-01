namespace ReyesAR.DTO
{
    public class DetalleEntregaDTO
    {
        // Clave Primaria Compuesta (PK)
        public string claveEntrega { get; set; }
        public string claveProducto { get; set; }
        public string tipo_detalle { get; set; }
        public bool extra { get; set; }

        // Columnas de Datos
        public string claveCompra { get; set; }
        public string claveUnidadCompra { get; set; }
        public decimal cantidad { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal subtotal { get; set; }
        public string? observaciones { get; set; }
    }
}
