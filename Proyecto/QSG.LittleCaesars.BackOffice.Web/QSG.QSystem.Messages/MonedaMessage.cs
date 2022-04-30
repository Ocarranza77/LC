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
        public MonedaResponse MonedaMessage(MonedaRequest request)
        {
            var response = new MonedaResponse();

            if (string.IsNullOrEmpty(request.BDName))
                request.BDName = "Qbic";

            var bl = new MonedaBL(request.BDName);
            string msg = string.Empty;

            response.ResultType = MessageResultType.Failure;

            if(string.IsNullOrEmpty(request.BDName))
                return null;



            if (request.GetCbo)
            {
                response.CboInis = bl.GetCbo(request.Moneda, ref msg);
                response.FriendlyMessage = msg;
            }


            if (request.MessageOperationType == MessageOperationType.Save)
            {
                int estadoID = 0;
                if (request.Moneda != null)
                    if (!bl.SaveMoneda(request.Moneda, out estadoID, ref msg))
                        response.FriendlyMessage += Generales.msgNoGrabo + msg;

                if (request.Monedas != null)
                    if (!bl.SaveMonedas(request.Monedas, out estadoID, ref msg))
                        response.FriendlyMessage += Generales.msgNoGrabo + msg;

                response.MonedaID = estadoID;

                if (request.Moneda == null && request.Monedas == null)
                    response.FriendlyMessage += Generales.msgNoGrabo + Generales.msgNoInfoAGrabar;
            }

            if (request.MessageOperationType == MessageOperationType.Report)
            {
                response.Monedas = bl.GetMonedas(request.Moneda, ref msg);
                response.FriendlyMessage = msg;
            }


            response.ResultType = MessageResultType.Sucess;
            return response;
        }
    }
}
