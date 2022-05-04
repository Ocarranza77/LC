using System.Collections.Generic;

namespace GenerarXml40
{
    public class ParteC
    {
        public string ClaveProdServ { get; set; } = string.Empty;
        public string NoIdentificacion { get; set; } = string.Empty;
        public decimal Cantidad { get; set; } = 0m;
        public string Unidad { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal ValorUnitario { get; set; } = 0m;
        public decimal Importe { get; set; } = 0m;
        public List<InformacionAduaneraC> InformacionAduanera { get; set; } = new List<InformacionAduaneraC>();

    }
}