using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("AsientosPolizaIngresos")]
    public class AsientosPolizaIngreso : BaseEntity
    {
        public string AsientoCod { get; set; }

        public bool MovimientoTipoCargo { get; set; }
        public string CampoRelacionado { get; set; }
        public string Grupo { get; set; }
        public string MovimientoTipo { get; set; }
        public Moneda Moneda { get; set; }
        public string Descripcion { get; set; }
        public string Referencia { get; set; }

    }
}
