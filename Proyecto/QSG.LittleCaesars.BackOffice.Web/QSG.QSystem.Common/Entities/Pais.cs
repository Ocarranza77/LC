using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities
{
    [Serializable,XmlRoot("Pais")]
    public class Pais:BaseEntity
    {
        public int PaisID { get; set; }
        public string Nombre { get; set; }
        public string Abr { get; set; }
        public string Nacionalidad { get; set; }
        public int CodigoISOn { get; set; }
        public  string  CodigoISOl { get; set; }
        public  string SimboloMoneda { get; set; }
        public string Moneda { get; set; }
        //public DateTime FechaAlta { get; set; }

    }
}
