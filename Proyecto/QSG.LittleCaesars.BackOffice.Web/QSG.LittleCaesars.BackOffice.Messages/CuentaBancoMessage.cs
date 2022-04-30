using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.BL;
using QSG.LittleCaesars.BackOffice.Common.Enums;
using QSG.LittleCaesars.BackOffice.Messages.Requests;
using QSG.LittleCaesars.BackOffice.Messages.Response;
using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Enums;
using log4net;

namespace QSG.LittleCaesars.BackOffice.Messages
{
    public partial class ServiceImplementation
    {

        public CuentaBancoResponse CuentaBancoMessage(CuentaBancoRequest request)
        {
            ILog _log4net = LogManager.GetLogger("CorteZMessage");

            var response = new CuentaBancoResponse();
            var bl = new CuentaBancoBL(request.BDName);
            string msg = string.Empty;

            response.ResultType = MessageResultType.Failure;

            if (string.IsNullOrEmpty(request.BDName))
            {
                response.FriendlyMessage = Generales.msgFaltaDBName;
                return response;
            }

            if (request.UserIDRqst == 0)
            {
                response.FriendlyMessage = Generales.msgUsuarioRequest;
                return response;
            }

            try
            {

                if (request.CboInis)
                {
                    response.CboInis = bl.GetCbo(request.CuentaBanco, request.UserIDRqst, ref msg);
                    response.FriendlyMessage = msg;
                }


                if (request.MessageOperationType == MessageOperationType.Query)
                {
                    /*
                    response.Ticket = bl.GetTicket(request.Ticket, ref msg);
                    response.FriendlyMessage = msg;*/
                }

                if (request.MessageOperationType == MessageOperationType.Save)
                {
                    if (request.CuentaBanco != null)
                        if (!bl.SaveCuentaBanco(request.CuentaBanco, ref msg))
                            response.FriendlyMessage += Generales.msgNoGrabo + msg;

                    if (request.CuentaBancos != null)
                        if (!bl.SaveCuentaBancos(request.CuentaBancos, ref msg))
                            response.FriendlyMessage += Generales.msgNoGrabo + msg;

                    if (request.CuentaBancos == null && request.CuentaBanco == null)
                        response.FriendlyMessage += Generales.msgNoGrabo + Generales.msgNoInfoAGrabar;

                }

                if (request.MessageOperationType == MessageOperationType.Report)
                {
                    response.CuentaBancos = bl.GetCuentaBanco(request.CuentaBanco, request.UserIDRqst, ref msg);
                    response.FriendlyMessage = msg;
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
