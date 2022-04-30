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
        public UsuarioResponse UsuarioMessage(UsuarioRequest request)
        {
            ILog _log4net = LogManager.GetLogger("UsuarioMessage");

            var response = new UsuarioResponse();
            var bl = new UsuarioBL();
            string msg = string.Empty;

            response.ResultType = MessageResultType.Failure;

           /*
            if (request.UserIDRqst == 0)
            {
                response.FriendlyMessage = Generales.msgUsuarioRequest;
                return response;
            }
            */

            try
            {
                if (request.MessageOperationType == MessageOperationType.Query)
                {
                    response.Usuario = bl.GetUsuario(request.Usuario, ref msg);

                    response.FriendlyMessage = msg;
                }

                if (request.MessageOperationType == MessageOperationType.Save)
                {
                    if (request.Usuario != null)
                        if (!bl.SaveUsuarios(request.Usuario, ref msg))
                            response.FriendlyMessage += Generales.msgNoGrabo + msg;

                    //if (request.Tickets != null)
                    //    if (!bl.SaveTickets(request.Tickets, request.SaveType, ref msg))
                    //        response.FriendlyMessage += Generales.msgNoGrabo + msg;

                    //if (request.Tickets == null && request.Ticket == null)
                    //    response.FriendlyMessage += Generales.msgNoGrabo + Generales.msgNoInfoAGrabar;
                }

                if (request.MessageOperationType == MessageOperationType.Report)
                {
                    response.Usuarios = bl.GetUsuarios(request.Usuario, ref msg);
                    response.FriendlyMessage = msg;
                }


                //TODO: Llamar al BL para realizar la operacion necesaria.

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
