namespace ReyesAR.Models
{
    public class UnidadMedidaEntity
    {

        public string claveUnidad { get; set; }

        public string nombreUnidad { get; set; }

        public string abreviatura { get; set; }

        public string tipoStock { get; set; }

        public bool estado { get; set; } = true;

        public virtual ICollection<ProductoEntity> Productos { get; set; } = new List<ProductoEntity>();




    }
}
