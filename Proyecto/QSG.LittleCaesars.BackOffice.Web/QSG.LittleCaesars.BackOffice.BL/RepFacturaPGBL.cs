using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.LittleCaesars.BackOffice.Common.Reports;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.QSystem.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.LittleCaesars.BackOffice.BL
{
    public class RepFacturaPGBL
    {
        public List<RepFacturaPG> Reporte(DateTime fecha, int usRqst, ref string friendlyMessage)
        {
            string msg = string.Empty;
            
            var result = new List<RepFacturaPG>();
            var dal = new RepFacturaPGDAL();
            
            msg = Satinizate(fecha);

            if (msg != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }
            
            result = dal.Reporte(fecha, usRqst, ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;
        }

        private string Satinizate(DateTime fecha)
        {
            string msg = string.Empty;

            if (fecha < (new DateTime(2012, 01, 01)))
                msg += "fecha valida";

            return msg;
        }
    }
}
