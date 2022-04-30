using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Messages.Requests;
using QSG.LittleCaesars.BackOffice.Messages.Response;
using QSG.LittleCaesars.BackOffice.Common.Enums;
using QSG.LittleCaesars.BackOffice.BL;
using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Enums;
using log4net;

namespace QSG.LittleCaesars.BackOffice.Messages
{
    public partial class ServiceImplementation
    {
        public TicketResponse TicketMessage(TicketRequest request)
        {
            ILog _log4net = LogManager.GetLogger("TicketMessage");

            var response = new TicketResponse();
            var bl = new TicketBL();
            string msg = string.Empty;

            response.ResultType = MessageResultType.Failure;

            if (request.UserIDRqst == 0)
            {
                response.FriendlyMessage = Generales.msgUsuarioRequest;
                return response;
            }

            try
            {
                if (request.MessageOperationType == MessageOperationType.Query)
                {
                    response.Ticket = bl.GetTicket(request.Ticket, request.UserIDRqst, ref msg);
                    response.FriendlyMessage = msg;
                }

                if (request.MessageOperationType == MessageOperationType.Save)
                {
                    if (request.Ticket != null)
                        if (!bl.SaveTicket(request.Ticket, request.SaveType, ref msg))
                            response.FriendlyMessage += Generales.msgNoGrabo + msg;

                    if (request.Tickets != null)
                        if (!bl.SaveTickets(request.Tickets, request.SaveType, ref msg))
                            response.FriendlyMessage += Generales.msgNoGrabo + msg;

                    if (request.Tickets == null && request.Ticket == null)
                        response.FriendlyMessage += Generales.msgNoGrabo + Generales.msgNoInfoAGrabar;
                }

                if (request.MessageOperationType == MessageOperationType.Report)
                {
                    if (!string.IsNullOrEmpty(request.TicktBitacora))
                    {
                        response.Tickets = bl.GetBitacoraTicket(request.TicktBitacora, ref msg);
                        response.FriendlyMessage = msg;

                    }
                    else
                    {
                        response.Tickets = bl.GetTickets(request.Filters, request.UserIDRqst, ref msg, request.TipoTicketReporte);
                        response.FriendlyMessage = msg;
                    }
                }


                response.ResultType = MessageResultType.Sucess;
            }
            catch (Exception ex)
            {
                response.ResultType = MessageResultType.Failure;
                _log4net.Error("MENSAJE: " + ex.Message + Environment.NewLine + "ORIGEN: " + ex.Source + Environment.NewLine + "METODO: " + ex.TargetSite, ex);
                response.FriendlyMessage += Environment.NewLine + "ERROR INESPERADO; Favor de notificar al Administrador del Sistema.";
            }
            
            return response;
        }
       

    }
}
