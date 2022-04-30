using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("CorteZ")]
    public class CorteZ:BaseEntity
    {
        public int TicketID { get; set; }
        public Sucursal Sucursal { get; set; }
        public int CajaID { get; set; }
        public DateTime FechaVta { get; set; }
        public string Hora { get; set; }
        public int Transacciones { get; set; }
        public string Cajeros { get; set; }
        public double Efectivo { get; set; }
        public double TCredito { get; set; }
        public double TDebito { get; set; }
        public double OtraFormaPago { get; set; }
        public double Total { get; set; }
        public int CodUsuario { get; set; }
    }
}
