using System.Collections.Generic;

namespace GenerarXml40
{
    public class CfdiRelacionados
    {
        public string TipoRelacion { get; set; } = string.Empty;
        public List<CfdiRelacionado> CfdiRelacionado { get; set; } = new List<CfdiRelacionado>();

    }
}