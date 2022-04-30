using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.QSystem.BL;
using QSG.QSystem.BL.SElite;
using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Enums;
using QSG.QSystem.Messages.Requests;
using QSG.QSystem.Messages.Response;

namespace QSG.QSystem.Messages
{
    public partial class ServiceQbic
    {
        public UsuarioResponse UsuarioMessage(UsuarioRequest request)
        {
            var response = new UsuarioResponse();
            var bl = new UsuarioBL(request.BDName);
            string msg = string.Empty;
            int id = 0;

            response.ResultType = MessageResultType.Failure; // ResultType.Failure;

            if (request.MessageOperationType == MessageOperationType.Query)
            {
                response.Usuarios = bl.GetUsuario(request.Usuario,  ref msg);

                response.FriendlyMessage = msg;
            }

            if (request.MessageOperationType == MessageOperationType.Save)
            {
                if (request.Usuario != null)
                    if (!bl.SaveUsuario(request.Usuario, out id, ref msg))
                        response.FriendlyMessage += Generales.msgNoGrabo + msg;

                response.ID = id;

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

            response.ResultType = MessageResultType.Failure;// ResultType.Sucess;
            return response;
        }




    }
}
