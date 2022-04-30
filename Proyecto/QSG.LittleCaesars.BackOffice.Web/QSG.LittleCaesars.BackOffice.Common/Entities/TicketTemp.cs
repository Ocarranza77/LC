using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    public class TicketTemp
    {
        public string row_ID { get; set; }
        public int TicketID { get; set; }
        public int SucursalID { get; set; }
        public int CajaID { get; set; }
        public double Importe { get; set; }
        public DateTime FechaVta { get; set; }
        public string HoraVta { get; set; }
        public string Cajero { get; set; }
        public DateTime FechaCaptura { get; set; }
        public int CodUsuario { get; set; }
        public DateTime FechaFactura { get; set; }
        public string FolioFactura { get; set; }
        public string RFC { get; set; }

    }
}
