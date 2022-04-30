using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Entities;
using QSG.QSystem.Common.Enums;

namespace QSG.LittleCaesars.BackOffice.BL
{
   public class CuentaBancoBL
    {
        private string _DBName;

        public CuentaBancoBL(string dbName)
        {
            this._DBName = dbName;
        }

       #region Public
       public List<CuentaBanco> GetCuentaBanco (CuentaBanco entidad, int usRqst, ref string friendlyMessage)
       {
           string msg = string.Empty;
           //var result = new Sucursal();
           List<CuentaBanco> result = new List<CuentaBanco>();
           var dal = new CuentaBancoDAL(_DBName);
           /*
           msg = SatinizateQuery(sucursal);

           if (msg != string.Empty)
           {
               friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

               return result;
           }
           */
           result = dal.GetCuentaBancos(entidad, usRqst, ref msg);
           friendlyMessage = friendlyMessage + msg;

           return result;
       }

       public bool SaveCuentaBanco(CuentaBanco entidad, ref string friendlyMessage)
       {
           string msg = string.Empty;
           var result = false;
           var dal = new CuentaBancoDAL(_DBName);

           msg = Satinizate(entidad);
           if (msg != string.Empty)
           {
               friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

               return result;
           }

           result = dal.SaveCuentaBanco(entidad, ref friendlyMessage);

           return result;
       }

       public bool SaveCuentaBancos(List<CuentaBanco> lst, ref string friendlyMessage)
       {
           string msg = string.Empty;
           string msgGral = string.Empty;
           int count = 0;
           var result = false;
           var dal = new CuentaBancoDAL(_DBName);

           foreach (CuentaBanco itm in lst)
           {
               count++;
               msg = Satinizate(itm);

               if (msg != string.Empty)
                   msgGral += " De la Cuenta Banco (" + count.ToString() + "): " + msg + "; ";
           }

           if (msgGral != string.Empty)
           {
               friendlyMessage = friendlyMessage + Generales.msgSigInfo + msgGral;

               return result;
           }

           result = dal.SaveCuentaBancos(lst, ref friendlyMessage);

           return result;
       }

       public List<CboTipo> GetCbo(CuentaBanco itm, int usRqst, ref string friendlyMessage)
       {
           List<CboTipo> result = new List<CboTipo>();
           var lst = GetCuentaBanco(itm, usRqst, ref friendlyMessage);

           foreach (CuentaBanco item in lst)
               result.Add(new CboTipo() { ID = item.CtaBcoID.ToString(), Nombre = item.NoCta, Abr = item.Banco.Nombre });

           return result;
       }

       #endregion

       #region Private
       private string SatinizateQuery(CuentaBanco item)
       {
           string msg = string.Empty;
           
           /*
           if (ticket.TicketID == 0)
               msg += "Numero de Ticket, ";

           if (ticket.Sucursal.SucursalID == 0)
               msg += "Sucursal, ";

           if (ticket.CajaID == 0)
               msg += "Numero de caja, ";

           if (ticket.Importe == 0)
               msg += "Importe";
           */
           return msg;
       }

       private string Satinizate(CuentaBanco item)
       {
           string msg = string.Empty;

           if ((item.OperationType == OperationType.Edit || item.OperationType == OperationType.Delete) 
               && item.CtaBcoID == 0)
               msg += "ID, ";

           if (item.NoCta == string.Empty)
               msg += "Numero de Cuenta, ";


           return msg;
       }

       #endregion

    }
}
