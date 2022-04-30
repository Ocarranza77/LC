using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.Common.Enums;

namespace QSG.LittleCaesars.BackOffice.Messages.Requests
{
    public class TicketRequest: BaseRequest
    {
        public Ticket Ticket { get; set; }

        public List<Ticket> Tickets { get; set; }

        public TicketSaveType SaveType { get; set; }

        public TicketFilter Filters { get; set; }

        /// <summary>
        /// Este campo es para solicitar el reporte de Ticket para que regrese todos los ticket (0) o que excluya los de publico en general (1)
        /// </summary>
        public int TipoTicketReporte { get; set; }

        /// <summary>
        /// Se envia la "Llave" que es la concatenacion de TicketID  | SucursalID |  CajaID | Importe | FechaVta
        /// </summary>
        public string TicktBitacora { get; set; }
    }
}
