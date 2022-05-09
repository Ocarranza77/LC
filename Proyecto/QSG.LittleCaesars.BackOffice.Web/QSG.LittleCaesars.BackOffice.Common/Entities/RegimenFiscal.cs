using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("RefimenFiscal")]
    public class RegimenFiscal : BaseEntity
    {
        public string CodRegimenFiscal { get; set; }
        public string Nombre { get; set; }
        public string FisicaMoral { get; set; }

    }
}
