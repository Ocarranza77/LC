using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("PlantillaPolizaIngresoAsiento")]
    public class PlantillaPolizaIngresoAsiento : BaseEntity
    {
        public AsientosPolizaIngreso Asiento { get; set; }

        public string CtaContable { get; set; }
        public string CtaComplementearia { get; set; }

        public string NomCta { get; set; }
        public string NomCtaComp { get; set; }
    }
}
