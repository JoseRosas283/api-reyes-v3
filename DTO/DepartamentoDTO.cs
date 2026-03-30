
namespace ReyesAR.DTO
{
    public class DepartamentoDTO
    {
        public string nombre { get; set; }

        public string descripcion { get; set; }

        public Boolean Estado { get; set; }
    }


    public class DepartamentoUpdateEstadoDto
    {

        public string EstadoTexto { get; set; }

    }


}
