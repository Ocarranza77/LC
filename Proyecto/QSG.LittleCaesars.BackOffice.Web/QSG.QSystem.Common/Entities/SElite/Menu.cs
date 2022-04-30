using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities.SElite
{
    [Serializable, XmlRoot("Menu")]
    public class Menu
    {
        public int MenuID { get; set; }
        // En este caso AppID no es una clase por la recursividad de la misma clase (Menu).
        public string AppID { get; set; }
        public string Descripcion { get; set; }
        public int PadreID { get; set; }
        public Forma Forma { get; set; }

        public bool Activo { get; set; }
        public int Orden { get; set; }

        public List<Menu> Hijos { get; set; }
    }
}
