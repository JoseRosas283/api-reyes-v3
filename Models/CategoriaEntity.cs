using System.Text.Json.Serialization;

namespace ReyesAR.Models
{
    public class CategoriaEntity
    {
        public string claveCategoria { get; set; }

        public string nombre { get; set; }

        public string descripcion { get; set; }

        public bool estado { get; set; } = true;

        public string claveDepartamento { get; set; }

        [JsonIgnore] // ← e
        public virtual DepartamentoEntity Departamento { get; set; }

        public virtual ICollection<ProductoEntity> Productos { get; set; } = new List<ProductoEntity>();
    }
}
