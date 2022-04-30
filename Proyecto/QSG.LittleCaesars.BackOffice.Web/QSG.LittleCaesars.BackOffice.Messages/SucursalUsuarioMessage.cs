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

        public SucursalUsuarioResponse SucursalUsuarioMessage(SucursalUsuarioRequest request)
        {
            ILog _log4net = LogManager.GetLogger("SucursalMessage");

            var response = new SucursalUsuarioResponse();
            var bl = new SucursalUsuarioBL();
            string msg = string.Empty;

            response.ResultType = MessageResultType.Failure;

            if (request.UserIDRqst == 0)
            {
                response.FriendlyMessage = Generales.msgUsuarioRequest;
                return response;
            }

            //if (request.UserIDRqst == 0)
            //{
            //    response.FriendlyMessage = Generales.msgUsuarioRequest;
            //    return response;
            //}

            //if (request.MessageOperationType == MessageOperationType.Query)
            //{
            //    response.SucursalesUsuario = bl.GetSucursalesUsuario(request.SucursalID, request.UsuarioID, ref msg);
            //    response.FriendlyMessage = msg;
            //}

            try
            {
                if (request.MessageOperationType == MessageOperationType.Save)
                {
                    if (request.SucursalesUsuario != null)
                        if (!bl.SaveSucursalesUsuario(request.SucursalesUsuario, ref msg))
                            response.FriendlyMessage += Generales.msgNoGrabo + msg;
                }

                if (request.MessageOperationType == MessageOperationType.Report)
                {
                    response.SucursalesUsuario = bl.GetSucursalesUsuario(request.SucursalID, request.UsuarioID, ref msg);
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
