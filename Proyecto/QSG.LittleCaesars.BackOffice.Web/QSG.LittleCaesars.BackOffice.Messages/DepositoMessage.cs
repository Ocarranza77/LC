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
        public DepositoResponse DepositoMessage(DepositoRequest request)
        {
            ILog _log4net = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().Name + GeneralApp.schema + request.BDName);

            var response = new DepositoResponse();
            var bl = new DepositoBL(request.BDName);
            string msg = string.Empty;

            response.ResultType = MessageResultType.Failure;

          
            //if (request.UserIDRqst == 0)
            //{
            //    response.FriendlyMessage = Generales.msgUsuarioRequest;
            //    return response;
            //}

            if (string.IsNullOrEmpty(request.BDName))
            {
                response.FriendlyMessage = Generales.msgFaltaDBName;
                return response;
            }

            try
            {
                if (request.GetCbo)
                {
                    response.CboInis = bl.GetCbo(ref msg);
                    response.FriendlyMessage = msg;
                }

                if (request.MessageOperationType == MessageOperationType.Save)
                {
                    _log4net.Info("Grabado de Depositos;  Usuario: " + request.UserIDRqst.ToString());

                    if (request.CorteSucursales != null)
                        if (!bl.SaveDepositos(request.CorteSucursales, ref msg))
                            response.FriendlyMessage += Generales.msgNoGrabo + msg;

                    if (request.CorteSucursales == null)
                        response.FriendlyMessage += Generales.msgNoGrabo + Generales.msgNoInfoAGrabar;
                }

                if (request.MessageOperationType == MessageOperationType.Report)
                {
                    _log4net.Info("Consulta de Depositos;  Usuario: " + request.UserIDRqst.ToString() + " Fecha Solicitada: " + request.Fecha.ToShortDateString());
                    response.CorteSucursales = bl.GetDepositos(request.Fecha, ref msg);
                    response.FriendlyMessage = msg;
                }

                if (!(request.MessageOperationType == MessageOperationType.Save && response.FriendlyMessage != string.Empty))
                    response.ResultType = MessageResultType.Sucess;
            }
            catch (Exception ex)
            {
                response.ResultType = MessageResultType.Failure;
                _log4net.Error("MENSAJE: " + ex.Message + Environment.NewLine + "ORIGEN: " + ex.Source + Environment.NewLine + "METODO: " + ex.TargetSite + Environment.NewLine + "Usuario ID: " + request.UserIDRqst.ToString(), ex);
                response.FriendlyMessage += Environment.NewLine + "ERROR INESPERADO; Favor de notificar al Administrador del Sistema.";
            }
            
            return response;
        }
       

    }
}
