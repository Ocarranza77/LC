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

        public EmpresaClienteResponse EmpresaClienteMessage(EmpresaClienteRequest request)
        {
            ILog _log4net = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().Name + GeneralApp.schema + request.BDName);

            var response = new EmpresaClienteResponse();
            var bl = new EmpresaClienteBL(request.BDName);
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
                    if (request.EmpresaID > 0)
                    {
                        response.EmpresaCliente = bl.GetEmpresa(request.EmpresaID, ref msg);
                        if (!string.IsNullOrEmpty(msg))
                            response.FriendlyMessage += "Ocurrio lo siguiente: " + msg;
                        else
                            response.FriendlyMessage += "GetEmpresaCliente: " + Generales.msgConsultaExito;
                    }

                    if (request.GetEmpresasCliente)
                    {
                        response.EmpresasCliente = bl.GetEmpresas(ref msg);
                        if (!string.IsNullOrEmpty(msg))
                            response.FriendlyMessage += "Ocurrio lo siguiente: " + msg;
                        else
                            response.FriendlyMessage += "GetEmpresasCliente: " + Generales.msgConsultaExito;
                    }

                    if (request.GetEmpresasContpaq)
                    {
                        response.EmpresasContpaq = bl.GetEmpresasContpaq(ref msg);
                        if (!string.IsNullOrEmpty(msg))
                            response.FriendlyMessage += "Ocurrio lo siguiente: " + msg;
                        else
                            response.FriendlyMessage += "GetEmpresasContpaq: " + Generales.msgConsultaExito;
                    }

                    if (request.GetCatalogoCuentasContpaq)
                    {
                        var cuentasNoEncontradas = new List<CatalogoContable>();
                        var mascarilla = string.Empty;

                        response.CatalogoCuentasContpaq = bl.GetCuentasContpaq(request.BaseDatos, request.SoloCatalogo, ref cuentasNoEncontradas, ref mascarilla, ref msg);
                        response.CuentasNoEncontradas = cuentasNoEncontradas;
                        response.Mascarilla = mascarilla;

                        if (!string.IsNullOrEmpty(msg))
                            response.FriendlyMessage += "Ocurrio lo siguiente: " + msg;
                        else
                            response.FriendlyMessage += "GetCatalogoCuentasContpaq: " + Generales.msgConsultaExito;
                    }

                    if (!string.IsNullOrEmpty(request.CuentaContableUsadaEn))
                    {
                        response.CuentasContpaqUsadasEn = bl.GetCuentasContpaqUsadasEn(request.CuentaContableUsadaEn, ref msg);
                        if (!string.IsNullOrEmpty(msg))
                            response.FriendlyMessage += "Ocurrio lo siguiente: " + msg;
                        else
                            response.FriendlyMessage += "GetCatalogoCuentasContpaq: " + Generales.msgConsultaExito;
                    }

                    if (request.CrearLinkContpaq)
                    {
                        msg = string.Empty;
                        response.LinkContpaqCreado = bl.CrearLinkContpaq(request.ServidorContpaq, request.UsuarioContpaq, request.PasswordUsuarioContpaq, ref msg);

                        if (!string.IsNullOrEmpty(msg))
                            response.FriendlyMessage += "Ocurrio lo siguiente: " + msg;
                        else
                            response.FriendlyMessage += "CrearLinkContpaq: " + Generales.msgConsultaExito;
                    }


                    //response.FriendlyMessage = msg;
                }

                if (request.MessageOperationType == MessageOperationType.Save)
                {
                    if (request.EmpresaCliente != null)
                        if (!bl.SaveEmpresa(request.EmpresaCliente, ref msg))
                            response.FriendlyMessage += Generales.msgNoGrabo + msg;

                    //if (request.Clientes != null)
                    //    if(!bl.SaveClientes(request.Clientes, ref msg))
                    //        response.FriendlyMessage += Generales.msgNoGrabo + msg;

                    if (request.EmpresaCliente == null) //&& request.Cliente == null)
                        response.FriendlyMessage += Generales.msgNoGrabo + Generales.msgNoInfoAGrabar;
                }

                if (request.MessageOperationType == MessageOperationType.Report)
                {
                    //ToDo: Implementar reporteria Sencilla
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
