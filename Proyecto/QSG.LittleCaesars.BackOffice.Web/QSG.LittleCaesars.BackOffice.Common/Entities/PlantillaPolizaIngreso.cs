using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("PlantillaPolizaIngreso")]
    public class PlantillaPolizaIngreso : BaseEntity
    {
        public Sucursal Sucursal { get; set; }

        public string Nombre { get; set; }
        public string Comentario { get; set; }

        public List<PlantillaPolizaIngresoAsiento> Asientos { get; set; }
        public List<PlantillaPolizaIngresoCtaBco> CtasBancarias { get; set; }

    }
}
