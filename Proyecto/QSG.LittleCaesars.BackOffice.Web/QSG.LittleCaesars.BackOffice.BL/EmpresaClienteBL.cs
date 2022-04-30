using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.QSystem.Common.Constants;

namespace QSG.LittleCaesars.BackOffice.BL
{
    public class EmpresaClienteBL
    {

        private string _DBName;

        public EmpresaClienteBL(string dbName)
        {
            this._DBName = dbName;
        }

        #region Public
        public EmpresaCliente GetEmpresa(int empresaID, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new EmpresaCliente();
            var dal = new EmpresaClienteDAL(_DBName);
            /*
            msg = SatinizateQuery(sucursal);

            if (msg != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }
            */
            result = dal.GetEmpresa(empresaID, ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;
        }

        public List<EmpresaCliente> GetEmpresas(ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<EmpresaCliente>();
            var dal = new EmpresaClienteDAL(_DBName);
            /*
            msg = SatinizateQuery(sucursal);

            if (msg != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }
            */
            result = dal.GetEmpresas(ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;
        }

        public bool SaveEmpresa(EmpresaCliente empresa, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = false;
            var dal = new EmpresaClienteDAL(_DBName);

            msg = Satinizate(empresa);
            if (msg != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }

            result = dal.SaveEmpresa(empresa, ref friendlyMessage);

            return result;
        }

        public List<EmpresaCliente> GetEmpresasContpaq(ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<EmpresaCliente>();
            var dal = new EmpresaClienteDAL(_DBName);

            result = dal.GetEmpresasContpaq(ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;
        }

        public List<CatalogoContable> GetCuentasContpaq(string BaseDatos, bool SoloCatalogo, ref List<CatalogoContable> CuentasNoEncontradas, ref string Mascarilla, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<CatalogoContable>();
            var dal = new EmpresaClienteDAL(_DBName);

            if (string.IsNullOrEmpty(BaseDatos))
                msg += "Empresa Contpag";

            if(string.IsNullOrEmpty(msg))
                return result;

            result = dal.GetCuentasContpaq(BaseDatos, SoloCatalogo, ref CuentasNoEncontradas, ref Mascarilla, ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;
        }

        public List<PlantillaPolizaIngreso> GetCuentasContpaqUsadasEn(string ctaCont, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<PlantillaPolizaIngreso>();
            var dal = new EmpresaClienteDAL(_DBName);

            if (string.IsNullOrEmpty(ctaCont))
            {
                friendlyMessage = "Favor de indicar la Cuenta Contable";
                return result;
            }

            result = dal.GetCuentasContpaqUsadasEn(ctaCont, ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;
        }

        public bool CrearLinkContpaq(string servidor, string user, string pwd, ref string friendlyMessage)
        {
            var result = false;
            var dal = new EmpresaClienteDAL(_DBName);


            if (string.IsNullOrEmpty(servidor) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pwd))
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + "Servidor, Usuario y Password son requeridos";

                return result;
            }

            result = dal.CrearLinkContpaq(servidor, user, pwd);

            return result;
        }

        #endregion

        #region Private

        //private string SatinizateQuery(EmpresaCliente item)
        //{
        //    string msg = string.Empty;

        //    /*
        //    if (ticket.TicketID == 0)
        //        msg += "Numero de Ticket, ";

        //    if (ticket.Sucursal.SucursalID == 0)
        //        msg += "Sucursal, ";

        //    if (ticket.CajaID == 0)
        //        msg += "Numero de caja, ";

        //    if (ticket.Importe == 0)
        //        msg += "Importe";
        //    */
        //    return msg;
        //}

        private string Satinizate(EmpresaCliente item)
        {
            string msg = string.Empty;

            if ( item.EmpresaContpaqID != 0 & ( string.IsNullOrEmpty(item.EmpresaContpaqRutaBD) || string.IsNullOrEmpty(item.EmpresaContpaqMascarilla)  ))
                msg += "Mascarilla y/o Base de datos Contpaq, ";


            return msg;
        }

        #endregion

    }
}
