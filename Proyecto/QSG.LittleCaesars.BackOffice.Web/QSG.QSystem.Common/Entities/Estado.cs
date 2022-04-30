using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities
{
    [Serializable,XmlRoot("Estado")]
    public class Estado:BaseEntity
    {
        public int EstadoID {get;set;}
        public Pais Pais {get;set;}
        public string Nombre {get;set;}
        public string Abr {get;set;}
        //public DateTime FechaAlta {get;set;}

    }
}
