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
    public class RegimenFiscalBL
    {
        #region Public
        public List<RegimenFiscal> GetRegimenFiscales(ref string friendlyMessage)
        {
            string msg = string.Empty;
            //var result = new Sucursal();
            List<RegimenFiscal> result = new List<RegimenFiscal>();
            var dal = new RegimenFiscalDAL();
            /*
            msg = SatinizateQuery(sucursal);

            if (msg != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }
            */
            result = dal.GetRegimenFiscales(ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;
        }
        #endregion
    }
}
