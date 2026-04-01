using ReyesAR.Models;

namespace ReyesAR.Models
{
    public class ProveedorEntity
    {
        public string claveProveedor { get; set; }

        public string direccion { get; set; }

        public string tipo_proveedor { get; set; }

        public string telefono { get; set; }

        public bool estado { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public virtual ICollection<ProductoEntity> Productos { get; set; } = new List<ProductoEntity>();

        public virtual EmpresaEntity Empresa { get; set; }
        public virtual IndependienteEntity Independiente { get; set; }

        public virtual ICollection<PedidoEntity> Pedidos { get; set; } = new List<PedidoEntity>();

        public virtual ICollection<RepresentanteEntity> Representantes { get; set; } = new List<RepresentanteEntity>();
        public virtual ICollection<EntregaEntity> Entregas { get; set; } = new List<EntregaEntity>();

    }
}
    public class EmpresaEntity
    {
        public string claveProveedor { get; set; }
        public string razonSocial { get; set; }
        public string rfc { get; set; }

        public ProveedorEntity Proveedor { get; set; }
    }

    public class IndependienteEntity
    {
        public string claveProveedor { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string tipoServicio { get; set; }

        public ProveedorEntity Proveedor { get; set; }
    }








