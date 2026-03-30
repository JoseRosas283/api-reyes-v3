namespace ReyesAR.DTO
{
    public class EmpleadoDTO
    {
        public string nombre { get; set; }
        public string apellido_paterno { get; set; }
        public string apellido_materno { get; set; }
        public string telefono { get; set; }
        public string curp { get; set; }
        public bool estado { get; set; } = true;
    }
}
