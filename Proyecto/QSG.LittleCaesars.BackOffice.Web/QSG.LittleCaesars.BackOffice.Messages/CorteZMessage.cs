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
using QSG.QSystem.Common.Libreria;
using log4net;

namespace QSG.LittleCaesars.BackOffice.Messages
{
    public partial class ServiceImplementation
    {
        public CorteZResponse CorteZMessage(CorteZRequest request)
        {
            ILog _log4net = LogManager.GetLogger("CorteZMessage");

            var response = new CorteZResponse();
            var bl = new CorteZBL();
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
                    response.CorteZ = bl.GetTicket(request.CorteZ, request.UserIDRqst, ref msg);
                    response.FriendlyMessage = msg;
                }

                if (request.MessageOperationType == MessageOperationType.Save)
                {
                    if (request.CorteZ != null)
                        if (!bl.SaveCorteZ(request.CorteZ, ref msg))
                            response.FriendlyMessage += Generales.msgNoGrabo + msg;

                    if (request.CorteZs != null)
                        if (!bl.SaveCorteZs(request.CorteZs, ref msg))
                            response.FriendlyMessage += Generales.msgNoGrabo + msg;

                    if (request.CorteZs == null && request.CorteZ == null)
                        response.FriendlyMessage += Generales.msgNoGrabo + Generales.msgNoInfoAGrabar;
                }

                if (request.MessageOperationType == MessageOperationType.Report)
                {
                    response.CorteZs = bl.GetCorteZs(request.Filters, request.UserIDRqst, ref msg);
                    response.FriendlyMessage = msg;

                    if (request.ReturnXML)
                        response.XML = Serializacion.SerializeToXML(response.CorteZs);
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
