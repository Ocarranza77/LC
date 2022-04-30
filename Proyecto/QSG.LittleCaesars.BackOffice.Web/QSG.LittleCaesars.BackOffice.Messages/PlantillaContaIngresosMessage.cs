using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Enums;
using QSG.LittleCaesars.BackOffice.Messages.Requests;
using QSG.LittleCaesars.BackOffice.Messages.Response;
using QSG.LittleCaesars.BackOffice.BL;
using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Enums;
using log4net;
using QSG.LittleCaesars.BackOffice.Common.Entities;

namespace QSG.LittleCaesars.BackOffice.Messages
{
    public partial class ServiceImplementation
    {

        public PlantillaContaIngresosResponse PlantillaContaIngresosMessage(PlantillaContaIngresosRequest request)
        {
            ILog _log4net = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().Name + GeneralApp.schema + request.BDName);

            var response = new PlantillaContaIngresosResponse();
            var bl = new PlantillaContaIngresosBL(request.BDName);
            string msg = string.Empty;

            response.ResultType = MessageResultType.Failure;

            if (request.UserIDRqst == 0)
            {
                response.FriendlyMessage = Generales.msgUsuarioRequest;
                return response;
            }

            try
            {
                if (request.MessageOperationType == MessageOperationType.Query)
                {
                    if (request.SucursalID != 0)
                    {
                        response.PlantillaContaIngresos = bl.GetPlantillaContaIngresos(request.SucursalID, ref msg);
                        if (!string.IsNullOrEmpty(msg))
                            response.FriendlyMessage += "Ocurrio lo siguiente: " + msg;
                        else
                            response.FriendlyMessage += Generales.msgConsultaExito;
                    }
                }

                if (request.MessageOperationType == MessageOperationType.Save)
                {
                    if (request.PlantillaPolizaIngresos != null)
                        if (!bl.SavePlantillaContaIngresos(request.PlantillaPolizaIngresos, ref msg))
                            response.FriendlyMessage += Generales.msgNoGrabo + msg;

                    //if (request.Clientes != null)
                    //    if(!bl.SaveClientes(request.Clientes, ref msg))
                    //        response.FriendlyMessage += Generales.msgNoGrabo + msg;

                    if (request.PlantillaPolizaIngresos == null) //&& request.Cliente == null)
                        response.FriendlyMessage += Generales.msgNoGrabo + Generales.msgNoInfoAGrabar;
                }

                if (request.MessageOperationType == MessageOperationType.Report)
                {
                    if (request.GetPlantillasContaIngresos)
                    {
                        response.PlantillasContaIngresos = bl.GetPlantillasContaIngresos(ref msg);
                        if (!string.IsNullOrEmpty(msg))
                            response.FriendlyMessage += "Ocurrio lo siguiente: " + msg;
                        else
                            response.FriendlyMessage += Generales.msgConsultaExito;
                    }
                }


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
