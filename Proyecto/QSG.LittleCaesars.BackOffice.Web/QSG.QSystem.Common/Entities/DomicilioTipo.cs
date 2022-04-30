using QSG.QSystem.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities
{
    [Serializable,XmlRoot("DomicilioTipo")]
    public class DomicilioTipo:BaseEntity, ICatalogoTipo
    {
        public int DomicilioTipoID {get;set;}
        public string Nombre {get;set;}
        public string Abr {get;set;}
        public bool Activo {get;set;}
        //public DateTime FechaAlta {get;set;}

        public int ID  { get; set; }
        
    }
}
