using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities.SElite
{
    [Serializable, XmlRoot("App")]
    public class App
    {
        public string AppID { get; set; }
        public string Descripcion { get; set; }
        public string Abr { get; set; }
        public string Version { get; set; }
        public string LastVerReq { get; set; }
        public string Notas { get; set; }
        public bool Activo { get; set; }
        public string IconPath { get; set; }

        public List<Menu> Menus { get; set; }
    }
}
