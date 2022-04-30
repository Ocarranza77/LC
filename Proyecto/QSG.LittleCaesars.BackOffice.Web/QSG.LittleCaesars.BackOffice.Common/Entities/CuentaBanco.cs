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
    public class CuentaBanco : BaseEntity
    {
        public int CtaBcoID { get; set; }
        public string Empresa { get; set; }
        public CatalogoTipo Banco { get; set; }
        //public Banco Banco { get; set; }
        public string NoCta { get; set; }
        public Moneda Moneda { get; set; }
        public string Titular { get; set; }
        public string Descripcion { get; set; }
        public string Notas { get; set; }

    }
}
