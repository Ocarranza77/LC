using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities
{
    [Serializable,XmlRoot("PersonaTipo")]
    public class PersonaTipo:BaseEntity
    {
        public int PersonaTipoID {get;set;}
        public string Nombre {get;set;}
        public string Abr { get; set; }

    }
}
