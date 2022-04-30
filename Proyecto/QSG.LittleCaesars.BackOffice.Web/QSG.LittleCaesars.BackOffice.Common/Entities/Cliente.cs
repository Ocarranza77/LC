using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
   [Serializable, XmlRoot("Cliente")]
   public  class Cliente:BaseEntity
    {
       public string RFC { get; set; }
       public string RazonSocial { get; set; }
       public string Calle { get; set; }
       public string NoInt { get; set; }
       public string NoExt { get; set; }
       public string Colonia { get; set; }
       public string Delegacion { get; set; }
       public string Ciudad { get; set; }
       public string Municipio { get; set; }
       public string Estado { get; set; }
       public string CP { get; set; }
       public string Contacto { get; set; }
       public string Email1 { get; set; }
       public string Email2 { get; set; }
       public string Email3 { get; set; }
    }
}
