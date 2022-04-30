using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities
{
    [Serializable,XmlRoot("Domicilio")]
    public class Domicilio:BaseEntity
    {
        public int DomicilioID {get;set;}
        public Persona Persona {get;set;}
        public string Calle {get;set;}
        public string NoInt {get;set;}
        public string NoExt {get;set;}
        public string Colonia {get;set;}
        public string Delegacion {get;set;}
        public string CP {get;set;}
        public string Municipio {get;set;}
        public DomicilioTipo DomicilioTipo {get;set;}
        public Ciudad Ciudad {get;set;}
        //public DateTime FechaAlta {get;set;}

    }
}
