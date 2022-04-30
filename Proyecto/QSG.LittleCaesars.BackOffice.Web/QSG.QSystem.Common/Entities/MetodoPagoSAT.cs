using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities
{
    [Serializable, XmlRoot("MetodoPagoSAT")]
    public class MetodoPagoSAT : BaseEntity
    {
        public string CodMetodoP {get;set;}
        public string Descripcion { get; set; }


    }
}
