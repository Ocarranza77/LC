using System.Collections.Generic;

namespace QSG.LittleCaesars.BackOffice.Common.SAT_XML_Entities
{
    public class Impuestos
    {
        public decimal TotalImpuestosRetenidos { get; set; } = -1m;
        public decimal TotalImpuestosTrasladados { get; set; } = -1m;
        public List<Retencion> Retenciones { get; set; } = new List<Retencion>();
        public List<Traslado> Traslados { get; set; } = new List<Traslado>();

    }
}