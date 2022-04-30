using log4net;
using QSG.LittleCaesars.BackOffice.BL;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.Common.Enums;
using QSG.LittleCaesars.BackOffice.Messages.Requests;
using QSG.LittleCaesars.BackOffice.Messages.Response;
using QSG.QSystem.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
using QSG.LittleCaesars.BackOffice.Messages.Requests;
using QSG.LittleCaesars.BackOffice.Messages.Response;
using QSG.LittleCaesars.BackOffice.Common.Enums;
using QSG.LittleCaesars.BackOffice.BL;
using QSG.LittleCaesars.BackOffice.Common.Constants;*/

namespace QSG.LittleCaesars.BackOffice.Messages
{
    public partial class ServiceImplementation
    {
        public FacturarConResponse FacturarComMessage(FacturarConRequest request)
        {
            ILog _log4net = LogManager.GetLogger("FacturarComMessage");

            var response = new FacturarConResponse();
            var bl = new FacturarConBL();
            var sucBL = new SucursalBL();
            var EmprCliBL = new EmpresaClienteBL(request.BDName);

            string msg = string.Empty;

            response.ResultType = MessageResultType.Failure;

            try
            {
                response.Datos = bl.GetDatos(request.SucursalID, ref msg);
                response.FriendlyMessage = msg;

                response.Sucursal = sucBL.GetSucursales(new Sucursal() { SucursalID = request.SucursalID }, request.UserIDRqst, ref msg)[0];
                response.FriendlyMessage = msg;
                
                //Cometado 20160923
                //response.Empresa = EmprCliBL.GetEmpresa(ref msg);
                //response.FriendlyMessage = msg;

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
