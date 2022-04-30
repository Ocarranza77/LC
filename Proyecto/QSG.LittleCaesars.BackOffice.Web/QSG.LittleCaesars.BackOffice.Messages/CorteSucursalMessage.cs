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
using QSG.QSystem.Common.Libreria;
using log4net;

namespace QSG.LittleCaesars.BackOffice.Messages
{
    public partial class ServiceImplementation
    {
        public CorteSucursalResponse CorteSucursalMessage(CorteSucursalRequest request)
        {
            ILog _log4net = LogManager.GetLogger("CorteSucursalMessage");

            var response = new CorteSucursalResponse();
            var bl = new CorteSucursalBL(request.BDName);
            string msg = string.Empty;

            response.ResultType = MessageResultType.Failure;

            if (request.UserIDRqst == 0)
            {
                response.FriendlyMessage = Generales.msgUsuarioRequest;
                return response;
            }

            try
            {
                if (request.FechaVta != null && request.SucursalesID != null && request.SucursalesID.Count > 0)
                    response.CorteSucursalesStt = bl.GetCorteSucursalStt(request.FechaVta, request.SucursalesID);

                if (request.MessageOperationType == MessageOperationType.Save)
                {

                    if (request.CorteSucursales != null)
                        if (!bl.SaveCorteSucursales(request.CorteSucursales, ref msg))
                            response.FriendlyMessage += Generales.msgNoGrabo + msg;

                    if (request.CorteSucursales == null)
                        response.FriendlyMessage += Generales.msgNoGrabo + Generales.msgNoInfoAGrabar;
                }

                if (request.MessageOperationType == MessageOperationType.Report)
                {
                    response.CorteSucursales = bl.GetCorteSucursales(request.Filters, request.UserIDRqst, ref msg);
                        response.FriendlyMessage = msg;


                        if (request.ReturnXML)
                            response.XML = Serializacion.SerializeToXML(response.CorteSucursales);
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
