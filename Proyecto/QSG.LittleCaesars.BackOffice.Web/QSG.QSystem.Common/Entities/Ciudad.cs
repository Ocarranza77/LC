using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities
{
    [Serializable, XmlRoot("Ciudad")]
    public class Ciudad:BaseEntity
    {
        public int CiudadID { get; set; }
        public Estado Estado { get; set; }
        public string Nombre { get; set; }
        public string Abr { get; set; }
        //public DateTime FechaAlta { get; set; }

    }
}
