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
        public CatalogoTipoResponse CatalogoTipoMessage(CatalogoTipoRequest request)
        {
            var response = new CatalogoTipoResponse();

            if (string.IsNullOrEmpty(request.BDName))
                request.BDName = "Qbic";

            var bl = new CatalogoTipoBL(request.BDName);
            string msg = string.Empty;

            response.ResultType = MessageResultType.Failure;

            if(string.IsNullOrEmpty(request.BDName))
                return null;



            if (request.GetCbo)
            {
                response.CboInis = bl.GetCbo(request.CatalogoTipo, request.type, ref msg);
                response.FriendlyMessage = msg;
            }


            if (request.MessageOperationType == MessageOperationType.Save)
            {
                int catalogoTipoID = 0;
                if (request.CatalogoTipo != null)
                    if (!bl.SaveCatalogoTipo(request.CatalogoTipo, request.type, out catalogoTipoID, ref msg))
                        response.FriendlyMessage += Generales.msgNoGrabo + msg;

                if (request.CatalogoTipos != null)
                    if (!bl.SaveCatalogoTipos(request.CatalogoTipos, request.type, out catalogoTipoID, ref msg))
                        response.FriendlyMessage += Generales.msgNoGrabo + msg;

                response.CatalogoTipoID = catalogoTipoID;

                if (request.CatalogoTipos == null && request.CatalogoTipo == null)
                    response.FriendlyMessage += Generales.msgNoGrabo + Generales.msgNoInfoAGrabar;
            }

            if (request.MessageOperationType == MessageOperationType.Report)
            {
                response.CatalogoTipos = bl.GetCatalogoTipos(request.CatalogoTipo, request.type, ref msg);
                response.FriendlyMessage = msg;
            }


            response.ResultType = MessageResultType.Sucess;
            return response;
        }
    }
}
