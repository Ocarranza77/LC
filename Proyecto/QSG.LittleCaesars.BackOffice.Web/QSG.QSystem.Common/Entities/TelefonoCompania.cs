using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities
{
    [Serializable,XmlRoot("TelefonoCompania")]
    public class TelefonoCompania:BaseEntity
    {
        public int TelefonoCompaniaID {get;set;}
        public string Nombre {get;set;}
        public string Abr {get;set;}
        public string Formato {get;set;}
        public bool Activo {get;set;}
        //public DateTime FechaAlta {get;set;}

    }
}
