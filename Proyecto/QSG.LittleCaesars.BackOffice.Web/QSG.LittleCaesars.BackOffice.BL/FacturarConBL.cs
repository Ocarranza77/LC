using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.LittleCaesars.BackOffice.BL
{
    public class FacturarConBL
    {
        public FacturarCon GetDatos(int sucursalID, ref string friendlyMessage)
        {
            var result = new FacturarCon();
            var dal = new FacturarConDAL();

            string msg = string.Empty;

            if (sucursalID == 0)
            {
                friendlyMessage += "Enviar código de la sucursal, ";
                return result;
            }
            result = dal.GetDatos(sucursalID, ref friendlyMessage);

            return result;
        }
    }
}
