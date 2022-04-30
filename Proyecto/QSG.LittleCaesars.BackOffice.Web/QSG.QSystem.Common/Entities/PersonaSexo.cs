using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities
{
    [Serializable,XmlRoot("PersonaSexo")]
    public class PersonaSexo:BaseEntity
    {
        public int SexoID  {get;set;}
        public string Nombre {get;set;}
        public string Abr { get; set; }

    }
}
