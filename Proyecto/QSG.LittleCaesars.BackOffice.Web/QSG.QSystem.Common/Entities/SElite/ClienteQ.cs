using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities.SElite
{
    [Serializable, XmlRoot("Usuario")]
    public class ClienteQ: BaseEntity
    {
        public int ClienteID { get; set; }
        public string Alias { get; set; }
        public bool Activo { get; set; }
        //public DateTime FechaAlta { get; set; }
        public int DiasAviso { get; set; }
        public string Licencia { get; set; }
        public string LogoPath { get; set; }
        public string Nombre { get; set; }
        public int NoEmpresas { get; set; }

        public List<Empresa> Empresas { get; set; }
        public List<ClienteQApp> Apps { get; set; }

    }
}
