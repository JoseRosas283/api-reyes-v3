namespace ReyesAR.DTO
{
    public class EquivalenciaUnidadDTO
    {
        public string claveProducto { get; set; }
        public string claveUnidadCompra { get; set; }

        // Atributos
        public decimal factor_conversion { get; set; } // NUMERIC en SQL = decimal en C#
        public string unidad_base { get; set; } // El ENUM de Postgres lo manejamos como string
    }
}
