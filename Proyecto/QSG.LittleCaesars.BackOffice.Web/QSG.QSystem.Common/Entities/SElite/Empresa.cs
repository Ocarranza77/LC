using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities.SElite
{
    [Serializable, XmlRoot("Empresa")]
    public class Empresa: BaseEntity
    {
        public int EmpresaID { get; set; }
        public ClienteQ ClienteQ { get; set; }
        public string Nombre { get; set; }
        public string Abr { get; set; }
        public bool Activo { get; set; }
        public string NombreBD { get; set; }
        public int NoEmpresa { get; set; }
        public int EmpresaIDD { get; set; }

    }
}
