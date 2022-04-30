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
        public CiudadResponse CiudadMessage(CiudadRequest request)
        {
            var response = new CiudadResponse();

            if (string.IsNullOrEmpty(request.BDName))
                request.BDName = "Qbic";

            var bl = new CiudadBL(request.BDName);
            string msg = string.Empty;

            response.ResultType = MessageResultType.Failure;

            if(string.IsNullOrEmpty(request.BDName))
                return null;



            if (request.GetCbo)
            {
                response.CboInis = bl.GetCbo(request.Ciudad, ref msg);
                response.FriendlyMessage = msg;
            }


            if (request.MessageOperationType == MessageOperationType.Save)
            {
                int ciudadID = 0;
                if (request.Ciudad != null)
                    if (!bl.SaveCiudad(request.Ciudad, out ciudadID, ref msg))
                        response.FriendlyMessage += Generales.msgNoGrabo + msg;

                if (request.Ciudades != null)
                    if (!bl.SaveCiudades(request.Ciudades, out ciudadID, ref msg))
                        response.FriendlyMessage += Generales.msgNoGrabo + msg;

                response.CiudadID = ciudadID;

                if (request.Ciudad == null && request.Ciudades == null)
                    response.FriendlyMessage += Generales.msgNoGrabo + Generales.msgNoInfoAGrabar;
            }

            if (request.MessageOperationType == MessageOperationType.Report)
            {
                response.Ciudades = bl.GetCiudades(request.Ciudad, ref msg);
                response.FriendlyMessage = msg;
            }


            response.ResultType = MessageResultType.Sucess;
            return response;
        }
    }
}
