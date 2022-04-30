using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities
{
    [Serializable, XmlRoot("Banco")]
    public class Banco : BaseEntity
    {
        public int BancoID { get; set; }
        public string Nombre { get; set; }
        public string Abr { get; set; }
    }
}
