using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.Common.Enums;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.QSystem.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.LittleCaesars.BackOffice.BL
{
    public class CorteZBL
    {
        #region Public
        public CorteZ GetTicket(CorteZ corteZ, int usRqst, ref string FriendlyMessage)
        {
            string msg = string.Empty;
            var result = new CorteZ();
            var dal = new CorteZDAL();
            
            msg = SatinizateQuery(corteZ);

            if (msg != string.Empty)
            {
                FriendlyMessage = FriendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }
            
            result = dal.GetCorteZ(corteZ, usRqst, ref msg);
            FriendlyMessage = FriendlyMessage + msg;

            return result;
        }

        public List<CorteZ> GetCorteZs(CorteZFilter corteZ, int usRqst, ref string FriendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<CorteZ>();
            var dal = new CorteZDAL();

            result = dal.GetCorteZs(corteZ, usRqst, ref msg);
            FriendlyMessage = FriendlyMessage + msg;

            return result;
        }

        public bool SaveCorteZ(CorteZ corteZ, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = false;
            var dal = new CorteZDAL();

            msg = SatinizateAlta(corteZ);

            if (msg != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }

            result = dal.SaveCorteZ(corteZ, ref friendlyMessage);

            return result;
        }

        public bool SaveCorteZs(List<CorteZ> corteZs, ref string friendlyMessage)
        {
            string msg = string.Empty;
            string msgGral = string.Empty;
            int count = 0;
            var result = false;
            var dal = new CorteZDAL();

            foreach (CorteZ cz in corteZs)
            {
                count++;
                msg = SatinizateAlta(cz);

                if (msg != string.Empty)
                    msgGral += " Del CorteZ (" + count.ToString() + "): " + msg + "; ";

            }

            if (msgGral != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msgGral;

                return result;
            }

            result = dal.SaveCorteZs(corteZs, ref friendlyMessage);

            return result;
        }

        #endregion

        #region Private
        private string SatinizateQuery(CorteZ corteZ)
        {
            string msg = string.Empty;
            
            if (corteZ.TicketID == 0)
                msg += "Numero de Ticket, ";

            if (corteZ.Sucursal.SucursalID == 0)
                msg += "Sucursal, ";

            if (corteZ.CajaID == 0)
                msg += "Numero de caja, ";

           
            //if (corteZ.Total == 0)
            //    msg += "Total";
            
            return msg;
        }

        private string SatinizateAlta(CorteZ corteZ)
        { 
            string msg = string.Empty;

            if (corteZ.TicketID == 0)
                msg += "Numero de Ticket, ";

            if (corteZ.Sucursal.SucursalID == 0)
                msg += "Sucursal, ";

            if (corteZ.CajaID == 0)
                msg += "Numero de caja, ";

            if (corteZ.Total == 0)
                msg += "Total, ";

            if (corteZ.FechaVta != null && corteZ.FechaVta.CompareTo(new DateTime(2013, 1, 1)) > 0)
                msg += "Fecha de Venta, ";


            return msg;
        }

        #endregion 
    }

}
