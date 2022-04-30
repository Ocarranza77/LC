using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("TicketFilter")]
    public class TicketFilter: Ticket
    {
        /// <summary>
        /// Se mandan cocatenados separados por coma los codigos de las sucurdales
        /// </summary>
        public string Sucursales { get; set; }

        /// <summary>
        /// Se mandan concatenados separados por coma los codigos de las cajas
        /// </summary>
        public string Cajas { get; set; }

        public double ImporteHasta { get; set; }
        public DateTime? FechaVtaHasta { get; set; }
        public DateTime? FechaCapturaHasta { get; set; }
        public DateTime? FechaFacturaHasta { get; set; }
        public DateTime? FechaCancelacionHasta { get; set; }
        public string FolioFacturaHasta { get; set; }

    }
}
