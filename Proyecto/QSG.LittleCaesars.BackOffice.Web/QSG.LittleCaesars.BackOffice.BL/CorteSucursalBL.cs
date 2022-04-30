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
    public class CorteSucursalBL
    {
        private string _DBName;

        public CorteSucursalBL(string dbName)
        {
            this._DBName = dbName;
        }

        #region Public

        public List<CorteSucursal> GetCorteSucursales(CorteSucursalFilter corteS, int usRqst, ref string FriendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<CorteSucursal>();
            var dal = new CorteSucursalDAL(_DBName);

            result = dal.GetCorteSucursales(corteS, usRqst,  ref msg);
            FriendlyMessage = FriendlyMessage + msg;

            return result;
        }

        public bool SaveCorteSucursal(CorteSucursal corteS, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = false;
            var dal = new CorteSucursalDAL(_DBName);

            msg = SatinizateAlta(corteS);

            if (msg != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo  + msg;

                return result;
            }

            result = dal.SaveCorteSucursal(corteS, ref friendlyMessage);

            return result;
        }

        public bool SaveCorteSucursales(List<CorteSucursal> corteSucursales, ref string friendlyMessage)
        {
            string msg = string.Empty;
            string msgGral = string.Empty;
            int count = 0;
            var result = false;
            var dal = new CorteSucursalDAL(_DBName);

            foreach (CorteSucursal cs in corteSucursales)
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

            result = dal.SaveCorteSucursales(corteSucursales, ref friendlyMessage);

            return result;
        }

        public List<CboTipo> GetCorteSucursalStt(DateTime fechaVta, List<int> sucursales)
        {
            string msg = string.Empty;
            var result = new List<CboTipo>();
            var dal = new CorteSucursalDAL(_DBName);

            result = dal.GetCorteSucursalStt(fechaVta,sucursales);
            
            return result;

        }
        #endregion

        #region Private
        private string SatinizateQuery(CorteSucursal corteS)
        {
            string msg = string.Empty;

            if (corteS.FechaVta != null && corteS.FechaVta.CompareTo(new DateTime(2013, 1, 1)) > 0)
                msg += "Fecha de Venta, ";

            return msg;
        }

        private string SatinizateAlta(CorteSucursal corteS)
        { 
            string msg = string.Empty;

            if (corteS.Sucursal.SucursalID == 0)
                msg += "Sucursal, ";

            if (corteS.Total == 0)
                msg += "Total, ";

            if (corteS.FechaVta != null && corteS.FechaVta.CompareTo(new DateTime(2013, 1, 1)) > 0)
                msg += "Fecha de Venta, ";

            if (corteS.Stt == string.Empty)
                corteS.Stt = "N";

            return msg;
        }

        #endregion 
    }

}
