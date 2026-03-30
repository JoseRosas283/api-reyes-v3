using System.Text.Json.Serialization;

namespace ReyesAR.Models
{
    public class DepartamentoEntity
    {
        public string claveDepartamento { get; set; }

        public string nombre { get; set; }

        public string descripcion { get; set; }

        public bool estado { get; set; } = true;

        public DateTime fecha_creacion { get; set; } = DateTime.UtcNow;

        [JsonIgnore] // ← e
        public virtual ICollection<CategoriaEntity> Categorias { get; set; } = new List<CategoriaEntity>();
    }
}
