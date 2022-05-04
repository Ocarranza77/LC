
namespace GenerarXml40
{
    public class RespuestaProdigia
    {
        public RespuestaProdigia()
        {
        }

        public bool TimbradoOk { get; set; } = false;
        public bool StatusOk { get; set; } = false;
        public bool ConsultaOk { get; set; } = false;
        public string Codigo { get; set; } = string.Empty;
        public string CodigoStatusCancelacion { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string Xml { get; set; } = string.Empty;
        public string Saldo { get; set; } = string.Empty;
    }
}