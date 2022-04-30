using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities
{
    [Serializable,XmlRoot("Persona")]
    public  class Persona:BaseEntity
    {
        public int PersonaID {get;set;}
        public string Nombre {get;set;}
        public string Paterno {get;set;}
        public string Materno {get;set;}
        public PersonaTipo PersonaTipo {get;set;}
        public DateTime? FechaNac {get;set;}
        public string RFC {get;set;}
        public string CURP {get;set;}
        public PersonaSexo Sexo {get;set;}
        public Pais Nacionalidad { get; set; }
        public Ciudad CiudadNac {get;set;}
        public Estado EstadoNac {get;set;}
        public Pais PaisNac {get;set;}
        public string Email {get;set;}
        public string RestringidoA {get;set;}
        //public DateTime? FechaAlta {get;set;}

        public List<Domicilio> Domicilios { get; set; }
        public List<Telefono> Telefonos { get; set; }

    }
}
