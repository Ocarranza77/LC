using System.Collections.Generic;

namespace QSG.LittleCaesars.BackOffice.Common.SAT_XML_Entities
{
    public class ImpuestosC
    {
        public List<TrasladoC> Traslados { get; set; } = new List<TrasladoC>();
        public List<RetencionC> Retenciones { get; set; } = new List<RetencionC>();

    }
}