using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("MenuP")]
    public class MenuP : BaseEntity
    {
        public string CodUser { get; set; }
        public int CodAp { get; set; }
        public int CodP { get; set; }
        public string NomAp { get; set; }
        public string NomP { get; set; }
        public string DescripcionP { get; set; }
    }
}
