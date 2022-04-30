using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;

namespace QSG.LittleCaesars.BackOffice.Messages.Response
{
    public class TicketResponse: BaseResponse
    {
        public Ticket Ticket { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}
