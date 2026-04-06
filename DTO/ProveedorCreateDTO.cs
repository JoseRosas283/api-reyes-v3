namespace ReyesAR.DTO
{
    public class ProveedorCreateDTO
    {
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string tipo_proveedor { get; set; } // 'EMPRESA' o 'INDEPENDIENTE'
        public bool estado { get; set; } = true;

        // Campos de Empresa
        public string? razonSocial { get; set; }
        public string? rfc { get; set; }

        // Campos de Independiente
        public string? nombre { get; set; }
        public string? apellidoPaterno { get; set; }
        public string? apellidoMaterno { get; set; }
        public string? tipoServicio { get; set; }
    }


    public class ProveedorBaseDTO
    {
        public string ClaveProveedor { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public bool Estado { get; set; }
        public string TipoProveedor { get; set; }
    }

    public class ProveedorEmpresaDTO : ProveedorBaseDTO
    {
        public string RazonSocial { get; set; }
        public string Rfc { get; set; }
    }

    public class ProveedorIndependienteDTO : ProveedorBaseDTO
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string TipoServicio { get; set; }
    }
}
