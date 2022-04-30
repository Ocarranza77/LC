using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities.SElite
{
    [Serializable, XmlRoot("ClienteQApp")]
    public class ClienteQApp
    {
        public App Apps { get; set; }
        //public ClienteQ ClienteQ { get; set; }
        public DateTime FechaAlta { get; set; }
        public bool Activo { get; set; }
        public int DiasAviso { get; set; }
        public string Licencia { get; set; }

    }
}
