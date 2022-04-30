using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("PlantillaPolizaIngresoCtaBco")]
    public class PlantillaPolizaIngresoCtaBco : BaseEntity
    {
        public CuentaBanco CtaBanco { get; set; }

        public string CtaContable { get; set; }
        public string CtaComplementearia { get; set; }
        public string NomCta { get; set; }
        public string NomCtaComp { get; set; }

        public bool MovimientoTipoCargo { get; set; }
        public string Grupo { get; set; }
        public string MovimientoTipo { get; set; }
        public string Descripcion { get; set; }
        public string Referencia { get; set; }

    }
}
