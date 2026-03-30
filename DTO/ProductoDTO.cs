namespace ReyesAR.DTO
{
    public class ProductoDTO
    {

        public string nombre { get; set; }
        public string descripcion { get; set; }
        public decimal precio { get; set; }
        public decimal margen_ganancia { get; set; } = 1.30m;

        public decimal precio_venta { get; set; }
        public string? codigo_barras { get; set; }
        public string tipo_producto { get; set; }
        public decimal cantidad { get; set; }
        public bool caducidad { get; set; }
        public bool estado { get; set; } = true;

        public IFormFile? Imagen { get; set; }

        public string? ImagenUrl { get; set; }


        // Campos de relaciones (Foreign Keys)
        public string claveCategoria { get; set; }
        public string claveUnidadMedida { get; set; }
        public string claveUnidadVenta { get; set; }
        public string claveProveedor { get; set; }


    }
}
