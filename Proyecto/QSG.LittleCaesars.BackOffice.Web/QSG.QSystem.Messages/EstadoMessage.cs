using QSG.QSystem.BL;
using QSG.QSystem.Messages.Requests;
using QSG.QSystem.Messages.Response;
using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.QSystem.Messages
{
    public partial class ServiceQbic
    {
        public EstadoResponse EstadoMessage(EstadoRequest request)
        {
            var response = new EstadoResponse();

            if (string.IsNullOrEmpty(request.BDName))
                request.BDName = "Qbic";

            var bl = new EstadoBL(request.BDName);
            string msg = string.Empty;

            response.ResultType = MessageResultType.Failure;

            if(string.IsNullOrEmpty(request.BDName))
                return null;



            if (request.GetCbo)
            {
                response.CboInis = bl.GetCbo(request.Estado, ref msg);
                response.FriendlyMessage = msg;
            }


            if (request.MessageOperationType == MessageOperationType.Save)
            {
                int estadoID = 0;
                if (request.Estado != null)
                    if (!bl.SaveEstado(request.Estado, out estadoID, ref msg))
                        response.FriendlyMessage += Generales.msgNoGrabo + msg;

                if (request.Estados != null)
                    if (!bl.SaveEstados(request.Estados, out estadoID, ref msg))
                        response.FriendlyMessage += Generales.msgNoGrabo + msg;

                response.EstadoID = estadoID;

                if (request.Estado == null && request.Estados == null)
                    response.FriendlyMessage += Generales.msgNoGrabo + Generales.msgNoInfoAGrabar;
            }

            if (request.MessageOperationType == MessageOperationType.Report)
            {
                response.Estados = bl.GetEstados(request.Estado, ref msg);
                response.FriendlyMessage = msg;
            }


            response.ResultType = MessageResultType.Sucess;
            return response;
        }
    }
}
