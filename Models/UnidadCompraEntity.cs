namespace ReyesAR.Models
{
    public class UnidadCompraEntity
    {

        public string claveUnidadCompra { get; set; }

        public string nombre_unidad { get; set; }

        public string abreviatura { get; set; }

        public string tipo_stock { get; set; }

        public bool estado { get; set; } = true;

        public virtual ICollection<EquivalenciaUnidadEntity> EquivalenciasUnidad { get; set; } = new List<EquivalenciaUnidadEntity>();
    }
}
