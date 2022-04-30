using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("Folios")]
    public class Folios:BaseEntity
    {
        public string Nombre { get; set; }
        public int Folio { get; set; }
        public string Control { get; set; }
    }
}
