using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.QSystem.Common.Constants;

namespace QSG.LittleCaesars.BackOffice.BL
{
    public class MenuBL
    {
        public List<MenuP> GetMenuUser(MenuP menu, ref string FriendlyMessage)
        {
            string msg = string.Empty;
            //var result = new Usuario();
            var dal = new MenuDAL();
            List<MenuP> lstMenu = new List<MenuP>();

            msg = Satinizate(menu);

            if (msg != string.Empty)
            {
                FriendlyMessage = FriendlyMessage + Generales.msgSigInfo + msg;

                return lstMenu;
            }

            lstMenu = dal.GetMenuUser(menu, ref msg);
            FriendlyMessage = FriendlyMessage + msg;

            return lstMenu;
        }

        private string Satinizate(MenuP menu)
        {
            string msg = string.Empty;

            if (menu.CodUser == "") {
                msg += "Codigo de Usuario";
            }
            return msg;
        }


    }
}
