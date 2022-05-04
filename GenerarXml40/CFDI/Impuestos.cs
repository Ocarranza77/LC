using System.Collections.Generic;

namespace GenerarXml40
{
    public class Impuestos
    {
        public decimal TotalImpuestosRetenidos { get; set; } = -1m;
        public decimal TotalImpuestosTrasladados { get; set; } = -1m;
        public List<Retencion> Retenciones { get; set; } = new List<Retencion>();
        public List<Traslado> Traslados { get; set; } = new List<Traslado>();

    }
}