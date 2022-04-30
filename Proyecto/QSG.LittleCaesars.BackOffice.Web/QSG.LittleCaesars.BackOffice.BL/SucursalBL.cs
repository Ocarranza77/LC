using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Enums;

namespace QSG.LittleCaesars.BackOffice.BL
{
   public class SucursalBL
    {
       #region Public
       public List<Sucursal> GetSucursales(Sucursal sucursal, int usRqst, ref string friendlyMessage)
       {
           string msg = string.Empty;
           //var result = new Sucursal();
           List<Sucursal> result = new List<Sucursal>();
           var dal = new SucursalDAL();
           /*
           msg = SatinizateQuery(sucursal);

           if (msg != string.Empty)
           {
               friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

               return result;
           }
           */
           result = dal.GetSucursales(sucursal, usRqst, ref msg);
           friendlyMessage = friendlyMessage + msg;

           return result;
       }

       public bool SaveSucursal(Sucursal sucursal, ref string friendlyMessage)
       {
           string msg = string.Empty;
           var result = false;
           var dal = new SucursalDAL();

           msg = Satinizate(sucursal);
           if (msg != string.Empty)
           {
               friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

               return result;
           }

           result = dal.SaveSucursal(sucursal, ref friendlyMessage);

           return result;
       }

       public bool SaveSucursales(List<Sucursal> sucursales, ref string friendlyMessage)
       {
           string msg = string.Empty;
           string msgGral = string.Empty;
           int count = 0;
           var result = false;
           var dal = new SucursalDAL();

           foreach (Sucursal suc in sucursales)
           {
               count++;
               msg = Satinizate(suc);

               if (msg != string.Empty)
                   msgGral += " De la Sucursal (" + count.ToString() + "): " + msg + "; ";
           }

           if (msgGral != string.Empty)
           {
               friendlyMessage = friendlyMessage + Generales.msgSigInfo + msgGral;

               return result;
           }

           result = dal.SaveSucursales(sucursales, ref friendlyMessage);

           return result;
       }

       #endregion
       #region Private
       private string SatinizateQuery(Sucursal sucursal)
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

       private string Satinizate(Sucursal sucursal)
       {
           string msg = string.Empty;

           if ((sucursal.OperationType == OperationType.Edit || sucursal.OperationType == OperationType.Delete) 
               && sucursal.SucursalID == 0)
               msg += "SucursalID, ";

           if (sucursal.Nombre == string.Empty)
               msg += "Nombre, ";


           return msg;
       }

       #endregion

    }
}
