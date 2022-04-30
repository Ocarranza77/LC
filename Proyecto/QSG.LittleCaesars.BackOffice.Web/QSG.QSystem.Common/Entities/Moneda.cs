using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities
{
    [Serializable, XmlRoot("Moneda")]
    public class Moneda : BaseEntity
    {
        public int MonedaID { get; set; }
        public string Nombre { get; set; }
        public string Abr { get; set; }
        public string CodigoISO { get; set; }
        public string Simbolo { get; set; }
    }
}
