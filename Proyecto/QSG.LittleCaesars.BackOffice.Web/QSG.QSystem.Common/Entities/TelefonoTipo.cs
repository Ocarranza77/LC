using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities
{
    [Serializable,XmlRoot("TelefonoTipo")]
    public class TelefonoTipo:BaseEntity
    {
        public int TelefonoTipoID {get;set;}
        public string Nombre {get;set;}
        public string Abr {get;set;}
        public bool Activo {get;set;}
        //public DateTime FechaAlta {get;set;}

    }
}
