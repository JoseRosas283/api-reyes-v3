namespace ReyesAR.Models
{
    public class UnidadVentaEntity
    {
        public string claveUnidadVenta { get; set; }
        public string nombreUnidad { get; set; }
        public string abreviatura { get; set; }
        public string tipoValor { get; set; }
        public bool estado { get; set; } = true;

        public virtual ICollection<ProductoEntity> Productos { get; set; } = new List<ProductoEntity>();
        public virtual ICollection<EquivalenciaUnidadVentaEntity> EquivalenciasUnidadVenta { get; set; } = new List<EquivalenciaUnidadVentaEntity>();
    }
}
