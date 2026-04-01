using ReyesAR.Models;

namespace ReyesAR.DTO
{
    public class RepresentanteDTO
    {
        public string nombre { get; set; }
        public string apellido_paterno { get; set; }
        public string apellido_materno { get; set; }
        public string telefono { get; set; }
        public bool estado { get; set; } = true;
        public string claveProveedor { get; set; }
        public string tipo_representante { get; set; }
    }
}
