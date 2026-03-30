namespace ReyesAR.Models
{
    public class EquivalenciaUnidadEntity
    {
        public string claveProducto { get; set; }
        public string claveUnidadCompra { get; set; }

        // Atributos
        public decimal factor_conversion { get; set; } // NUMERIC en SQL = decimal en C#
        public string unidad_base { get; set; } // El ENUM de Postgres lo manejamos como string

        // Propiedades de Navegación
        public virtual ProductoEntity Producto { get; set; }
        public virtual UnidadCompraEntity UnidadCompra { get; set; }
    }
}
