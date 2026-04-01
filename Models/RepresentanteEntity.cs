using System.ComponentModel.DataAnnotations.Schema;

namespace ReyesAR.Models
{
    public class RepresentanteEntity
    {
        public string claveRepresentante { get; set; }
        public string nombre { get; set; }
        public string apellido_paterno { get; set; }
        public string apellido_materno { get; set; }
        public string telefono { get; set; }
        public bool estado { get; set; } = true;
        public string claveProveedor { get; set; }
        public string tipo_representante { get; set; }
        public virtual ProveedorEntity Proveedor { get; set; }
        public virtual ICollection<EntregaEntity> Entregas { get; set; } = new List<EntregaEntity>();
    }
}
