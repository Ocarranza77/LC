using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.QSystem.BL;
using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Enums;
using QSG.QSystem.Messages.Requests;
using QSG.QSystem.Messages.Response;
using QSG.QSystem.BL.SElite;

namespace QSG.QSystem.Messages
{
    public partial class ServiceQbic
    {
        public LoginResponse LoginMessage(LoginRequest request)
        {
            var response = new LoginResponse();
            

            if (string.IsNullOrEmpty(request.BDName))
                request.BDName = "Qbic";

            var bl = new LoginBL(request.BDName);

            string msg = string.Empty;
            response.FriendlyMessage = msg;

            response.ResultType = MessageResultType.Failure;

            if (string.IsNullOrEmpty(request.BDName))
            {
                response.FriendlyMessage = Generales.msgFaltaDBName;
                return response;
            }

            //if (request.MessageOperationType == MessageOperationType.Query)
            //{
            //    response.Usuario = bl.GetUsuario(request.Usuario, ref msg);

            //    response.FriendlyMessage = msg;
            //}

            //if (request.MessageOperationType == MessageOperationType.Save)
            //{
            //    if (request.Usuario != null)
            //        if (!bl.SaveUsuarios(request.Usuario, ref msg))
            //            response.FriendlyMessage += Generales.msgNoGrabo + msg;

            //    //if (request.Tickets != null)
            //    //    if (!bl.SaveTickets(request.Tickets, request.SaveType, ref msg))
            //    //        response.FriendlyMessage += Generales.msgNoGrabo + msg;

            //    //if (request.Tickets == null && request.Ticket == null)
            //    //    response.FriendlyMessage += Generales.msgNoGrabo + Generales.msgNoInfoAGrabar;
            //}

            if (request.MessageOperationType == MessageOperationType.Report)
            {
                if(!string.IsNullOrEmpty( request.Email) && !string.IsNullOrEmpty(request.Password))
                response.Usuarios = bl.Authentication(request.Email, request.Password, ref msg);
                
                //Autoizacion

                response.FriendlyMessage = msg;

                if (request.UsuarioID != 0)
                    response.Apps = bl.Authorize(request.UsuarioID, ref msg);

                response.FriendlyMessage = response.FriendlyMessage + msg;
            }


            //TODO: Llamar al BL para realizar la operacion necesaria.

            response.ResultType = MessageResultType.Failure;// ResultType.Sucess;
            return response;
        }




    }
}
