using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("PolizaContaDT")]
    public class PolizaContaDT : BaseEntity
    {
        public int Consecutivo { get; set; }

        public string Cuenta { get; set; }
        public string NomCta { get; set; }
        public string Referencia { get; set; }
        public string Tipo { get; set; }
        public float Importe { get; set; }
        public string Diario { get; set; }
        public string Concepto { get; set; }

    }
}
