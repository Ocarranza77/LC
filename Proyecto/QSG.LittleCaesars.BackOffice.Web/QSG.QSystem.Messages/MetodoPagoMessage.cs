using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.QSystem.BL;
using QSG.QSystem.Common.Enums;
using QSG.QSystem.Messages.Requests;
using QSG.QSystem.Messages.Response;
using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Enums;
using log4net;

namespace QSG.QSystem.Messages
{
    public partial class ServiceQbic
    {

        public MetodoPagoSATResponse MetodoPagoSATMessage (MetodoPagoSATRequest request)
        {
            ILog _log4net = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().Name + Generales.dbo + request.BDName);

            var response = new MetodoPagoSATResponse();
            var bl = new MetodoPagoSATBL();  //eseeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee
            string msg = string.Empty;

            response.ResultType = MessageResultType.Failure;

            //if (request.UserIDRqst == 0)
            //{
            //    response.FriendlyMessage = Generales.msgUsuarioRequest;
            //    return response;
            //}

            if (request.UserIDRqst == 0)
            {
                response.FriendlyMessage = Generales.msgUsuarioRequest;
                return response;
            }

            try
            {

                if(request.CboIni)
                {
                    response.ListCboTipo = bl.GetCbo(ref msg);
                    response.FriendlyMessage = msg;
                }

                if (request.MessageOperationType == MessageOperationType.Query)
                {
                    /*
                    response.Ticket = bl.GetTicket(request.Ticket, ref msg);
                    response.FriendlyMessage = msg;*/
                }


                if (request.MessageOperationType == MessageOperationType.Report)
                {
                    response.MetodoPagos = bl.GetMetodoPagos(ref msg);
                    response.FriendlyMessage = msg;
                }


                response.ResultType = response.FriendlyMessage == string.Empty ? MessageResultType.Sucess : MessageResultType.Partial;
            }
            catch (Exception ex)
            {
                response.ResultType = MessageResultType.Failure;
                _log4net.Error("MENSAJE: " + ex.Message + Environment.NewLine + "ORIGEN: " + ex.Source + Environment.NewLine + "METODO: " + ex.TargetSite + "USUARIO: " + request.UserIDRqst.ToString(), ex);
                response.FriendlyMessage += Environment.NewLine + "ERROR INESPERADO; Favor de notificar al Administrador del Sistema.";
            }
            return response;
        }


    }
}
