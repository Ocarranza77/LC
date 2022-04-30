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

namespace QSG.LittleCaesars.BackOffice.Messages
{
    public partial class ServiceImplementation
    {
        public MenuResponse MenuMessage(MenuRequest request)
        {
            var response = new MenuResponse();
            var bl = new MenuBL();
            string msg = string.Empty;

            response.ResultType = MessageResultType.Failure;

            if (request.UserIDRqst == 0)
            {
                response.FriendlyMessage = Generales.msgUsuarioRequest;
                return response;
            }

            if (request.MessageOperationType == MessageOperationType.Query)
            {

               //response.Menus = bl.GetMenuUser(request.Menu , ref msg);
               // response.Usuario = bl.GetUsuario(request.Usuario, ref msg);

                //response.FriendlyMessage = msg;

                //ToDo: Invocar al BL para hacer la consulta, se acuerdo a lo enviado en el objeto Ticket.
            }

            if (request.MessageOperationType == MessageOperationType.Save)
            {
                //ToDo: Invocar al BL para hacer el grabado y validar la informacion, se acuerdo a lo enviado en el objeto Ticket.
            }

            if (request.MessageOperationType == MessageOperationType.Report)
            {
                response.Menus = bl.GetMenuUser(request.Menu, ref msg);
                // response.Usuario = bl.GetUsuario(request.Usuario, ref msg);

                response.FriendlyMessage = msg;
            }

            //TODO: Llamar al BL para realizar la operacion necesaria.

            response.ResultType = MessageResultType.Sucess;
            return response;
        }





    }
}
