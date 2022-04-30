using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("CorteSucursalDeposito")]
    public class CorteSucursalDeposito:BaseEntity
    {
        //public DateTime FechaVta { get; set; }
        //public Sucursal Sucursal { get; set; }
        public int DepositoID { get; set; }
        public int Consecutivo { get; set; }
        public CuentaBanco CuentaBanco { get; set; }
        public string FolioDeposito { get; set; }
        public DateTime FechaDeposito { get; set; }
        public double Importe { get; set; }
        public string Nota { get; set; }

    }
}
