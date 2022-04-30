using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("Ticket")]
    public class Ticket: BaseEntity
    {
       
        public int TicketID { get; set; }
        public Sucursal Sucursal { get; set; }
        public int CajaID { get; set; }
        public double Importe { get; set; }
        public DateTime FechaVta { get; set; }
        public string HoraVta { get; set; }
        public string Cajero { get; set; }
        public DateTime FechaCaptura { get; set; }
        public int CodUsuario { get; set; }
        public DateTime? FechaFactura { get; set; }
        public string FolioFactura { get; set; }
        public string RFC { get; set; }
        public Cliente Cliente { get; set; }
        public string Anterior { get; set; }
        public string FacturaXML { get; set; }
        public string UUID { get; set; }
        public DateTime? FechaCancelacion { get; set; }
        public string AcuseXML { get; set; }
        public string MotivoCancelacion { get; set; }
        public MetodoPagoSAT MetodoPago { get; set; }
       /* public string Folio { get; set; }*/
    }
}
