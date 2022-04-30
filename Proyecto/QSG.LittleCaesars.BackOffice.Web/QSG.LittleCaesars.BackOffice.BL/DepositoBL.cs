using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.Common.Enums;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.LittleCaesars.BackOffice.BL
{
    public class DepositoBL
    {
        private string _DBName;

        public DepositoBL(string dbName)
        {
            this._DBName = dbName;
        }

        #region Public

        public List<CorteSucursal> GetDepositos(DateTime fecha, ref string FriendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<CorteSucursal>();
            var dal = new DepositoDAL(_DBName);

            result = dal.GetDepositos(fecha, ref msg);
            FriendlyMessage = FriendlyMessage + msg;

            return result;
        }

        public bool SaveDepositos(List<CorteSucursal> lst, ref string friendlyMessage)
        {
            string msg = string.Empty;
            string msgGral = string.Empty;
            int count = 0;
            var result = false;
            var dal = new DepositoDAL(_DBName);

            foreach (CorteSucursal cs in lst)
            {
                count++;
                msg = SatinizateAlta(cs);

                if (msg != string.Empty)
                    msgGral += " Del CorteSucursal (" + count.ToString() + "): " + msg + "; ";

            }

            if (msgGral != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msgGral;

                return result;
            }

            result = dal.SaveDepositos(lst, ref friendlyMessage);

            return result;
        }

        #endregion

        #region Private
        //private string SatinizateQuery(CorteSucursal corteS)
        //{
        //    string msg = string.Empty;

        //    foreach(CorteSucursalDeposito dep in corteS.Depositos)
        //    {
        //        if (dep.FechaDeposito != null && dep.FechaDeposito.CompareTo(new DateTime(2013, 1, 1)) > 0)
        //            msg += "Fecha del Deposito, ";

        //        if(dep.CuentaBanco == null || (dep.CuentaBanco != null && dep.CuentaBanco.CtaBcoID != 0))
        //            msg += "Cuenta Bancaria, ";

        //        if(dep.Importe == 0)
        //            msg += "Importe, ";

        //        if(dep.FolioDeposito == string.Empty)
        //            msg += "Folio del Deposito, ";

        //        msg = "Del Deposito (" + dep.Consecutivo.ToString() + "): " + msg + "; ";
        //    }

        //    return msg;
        //}

        private string SatinizateAlta(CorteSucursal corteS)
        { 
            string msg = string.Empty;

            foreach (CorteSucursalDeposito dep in corteS.Depositos)
            {
                if (dep.FechaDeposito != null && dep.FechaDeposito.CompareTo(new DateTime(2013, 1, 1)) > 0)
                    msg += "Fecha del Deposito, ";

                if (dep.CuentaBanco == null || (dep.CuentaBanco != null && dep.CuentaBanco.CtaBcoID != 0))
                    msg += "Cuenta Bancaria, ";

                if (dep.Importe == 0)
                    msg += "Importe, ";

                if (dep.FolioDeposito == string.Empty)
                    msg += "Folio del Deposito, ";

                msg = "Del Deposito (" + dep.Consecutivo.ToString() + "): " + msg + "; ";
            }


            return msg;
        }

        #endregion 
    
        public Dictionary<string, List<CboTipo>> GetCbo(ref string friendlyMessage)
       {
           var result = new Dictionary<string, List<CboTipo>>();
           var dal = new DepositoDAL(this._DBName);

           result = dal.IniciarCbos();


           return result;
       }
    }

}
