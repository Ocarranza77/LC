using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace QSG.QSystem.Common.Entities
{
    [Serializable,XmlRoot("Telefono")]
    public class Telefono:BaseEntity
    {
        public int TelefonoID {get;set;}
        public Persona Persona {get;set;}
        public TelefonoTipo TelefonoTipo {get;set;}
        public string Lada {get;set;}
        public string Numero {get;set;}
        public TelefonoCompania CompaniaTelefono {get;set;}
        public string Notas {get;set;}
        //public DateTime FechaAlta {get;set;}

    }
}
