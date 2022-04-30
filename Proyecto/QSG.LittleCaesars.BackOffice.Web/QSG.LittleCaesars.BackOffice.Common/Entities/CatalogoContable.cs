using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("CuentaBanco")]
    public class CatalogoContable
    {
        public string Cuenta { get; set; }
        public string Nombre { get; set; }
        public int CtaMayor { get; set; }
        public bool Afectable { get; set; }
        public bool EnUso { get; set; }
    }
}
