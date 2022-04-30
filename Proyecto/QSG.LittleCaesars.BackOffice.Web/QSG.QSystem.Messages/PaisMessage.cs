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
        public PaisResponse PaisMessage(PaisRequest request)
        {
            var response = new PaisResponse();

            if (string.IsNullOrEmpty(request.BDName))
                request.BDName = "Qbic";

            var bl = new PaisBL(request.BDName);
            string msg = string.Empty;

            response.ResultType = MessageResultType.Failure;

            if(string.IsNullOrEmpty(request.BDName))
                return null;



            if (request.GetCbo || request.GetCboNacionalidad)
            {
                response.CboInis = bl.GetCbo(request.Pais, request.GetCboNacionalidad, ref msg);
                response.FriendlyMessage = msg;
            }


            if (request.MessageOperationType == MessageOperationType.Save)
            {
                int paisID = 0;
                if (request.Pais != null)
                    if (!bl.SavePais(request.Pais, out paisID, ref msg))
                        response.FriendlyMessage += Generales.msgNoGrabo + msg;

                if (request.Paises != null)
                    if (!bl.SavePaises(request.Paises, out paisID, ref msg))
                        response.FriendlyMessage += Generales.msgNoGrabo + msg;

                response.PaisID = paisID;

                if (request.Pais == null && request.Paises == null)
                    response.FriendlyMessage += Generales.msgNoGrabo + Generales.msgNoInfoAGrabar;
            }

            if (request.MessageOperationType == MessageOperationType.Report)
            {
                response.Paises = bl.GetPaises(request.Pais, ref msg);
                response.FriendlyMessage = msg;
            }


            response.ResultType = MessageResultType.Sucess;
            return response;
        }
    }
}
