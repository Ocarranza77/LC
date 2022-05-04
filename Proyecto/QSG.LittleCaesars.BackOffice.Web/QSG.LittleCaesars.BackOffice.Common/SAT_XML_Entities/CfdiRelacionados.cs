using System.Collections.Generic;

namespace QSG.LittleCaesars.BackOffice.Common.SAT_XML_Entities
{
    public class CfdiRelacionados
    {
        public string TipoRelacion { get; set; } = string.Empty;
        public List<CfdiRelacionado> CfdiRelacionado { get; set; } = new List<CfdiRelacionado>();

    }
}