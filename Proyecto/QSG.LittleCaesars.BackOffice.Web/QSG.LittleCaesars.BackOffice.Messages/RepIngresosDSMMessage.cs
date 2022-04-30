using log4net;
using QSG.LittleCaesars.BackOffice.BL;
using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.LittleCaesars.BackOffice.Messages.Requests;
using QSG.LittleCaesars.BackOffice.Messages.Response;
using QSG.QSystem.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.LittleCaesars.BackOffice.Messages
{
    public partial class ServiceImplementation
    {
        public RepIngresosDSMResponse RepIngresosDSMMessages(RepIngresosDSMRequest request)
        {
            ILog _log4net = LogManager.GetLogger("RepIngresosDSMMessages");

            var response = new RepIngresosDSMResponse();
            var bl = new RepIngresosDSMBL();
            var msg = string.Empty;

            if (request.UserIDRqst == 0)
            {
                response.FriendlyMessage = Generales.msgUsuarioRequest;
                return response;
            }

            try
            {
                response.Reporte = bl.Reporte(request.fecha, request.SucursalID,request.EmpresaID, ref msg);
                response.FriendlyMessage = msg;
            }
            catch (Exception ex)
            {
                
                _log4net.Error("MENSAJE: " + ex.Message + Environment.NewLine + "ORIGEN: " + ex.Source + Environment.NewLine + "METODO: " + ex.TargetSite, ex);
                response.FriendlyMessage += Environment.NewLine + "ERROR INESPERADO; Favor de notificar al Administrador del Sistema.";
            }

            return response;
        }
    }
}
