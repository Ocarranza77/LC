using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities.SElite
{
    [Serializable, XmlRoot("Forma")]
    public class Forma
    {
        public string FormaID { get; set; }
        public App App { get; set; }
        public string Descripcion { get; set; }
        public string IconPath { get; set; }
        public string FormPath { get; set; }
        public string Extencion { get; set; }
        public bool Activo { get; set; }

    }
}
