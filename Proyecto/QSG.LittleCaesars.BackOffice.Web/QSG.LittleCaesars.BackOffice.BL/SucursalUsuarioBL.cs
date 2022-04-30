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
   public class SucursalUsuarioBL
    {
       #region Public
       public List<SucursalUsuario> GetSucursalesUsuario(int sucursalID, int usuarioID, ref string friendlyMessage)
       {
           string msg = string.Empty;
           List<SucursalUsuario> result = new List<SucursalUsuario>();
           var dal = new SucursalUsuarioDAL();

           /*
           msg = SatinizateQuery(sucursal);

           if (msg != string.Empty)
           {
               friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

               return result;
           }
           */

           result = dal.GetSucursalesUsuario(sucursalID, usuarioID, ref msg);
           friendlyMessage = friendlyMessage + msg;

           return result;
       }

       public bool SaveSucursalesUsuario(List<SucursalUsuario> sucUss, ref string friendlyMessage)
       {
           string msg = string.Empty;
           string msgGral = string.Empty;
           int count = 0;
           var result = false;
           var dal = new SucursalUsuarioDAL();

           foreach (SucursalUsuario su in sucUss)
           {
               count++;
               msg = Satinizate(su);

               if (msg != string.Empty)
                   msgGral += " De la SucursalUsuario (" + count.ToString() + "): " + msg + "; ";
           }

           if (msgGral != string.Empty)
           {
               friendlyMessage = friendlyMessage + Generales.msgSigInfo + msgGral;

               return result;
           }

           result = dal.SaveSucursalesUsuario(sucUss, ref friendlyMessage);

           return result;
       }

       #endregion

       #region Private
       private string SatinizateQuery(SucursalUsuario su)
       {
           string msg = string.Empty;
           
           
           //if (su.Sucursal == null || su.Sucursal.SucursalID == 0)
           //    msg += "Sucursal, ";

           //if (su.UsuarioPermisoID == 0)
           //    msg += "Usuario, ";

           return msg;
       }

       private string Satinizate(SucursalUsuario su)
       {
           string msg = string.Empty;

           if (su.Sucursal == null || su.Sucursal.SucursalID == 0)
               msg += "Sucursal, ";

           if (su.UsuarioPermisoID == 0)
               msg += "Usuario, ";


           return msg;
       }

       #endregion

    }
}
