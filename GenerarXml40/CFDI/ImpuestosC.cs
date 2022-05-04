using System.Collections.Generic;

namespace GenerarXml40
{
    public class ImpuestosC
    {
        public List<TrasladoC> Traslados { get; set; } = new List<TrasladoC>();
        public List<RetencionC> Retenciones { get; set; } = new List<RetencionC>();

    }
}