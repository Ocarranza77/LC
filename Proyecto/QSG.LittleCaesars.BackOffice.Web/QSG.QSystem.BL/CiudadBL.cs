using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Entities;
using QSG.QSystem.Common.Enums;
using QSG.QSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.QSystem.BL
{
    public class CiudadBL
    {
        private string _DBName;

        public CiudadBL(string dbName)
        {
            this._DBName = dbName;
        }

        public List<Ciudad> GetCiudades(Ciudad cd, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<Ciudad>();
            var dal = new CiudadDAL(this._DBName);

            result = dal.GetCiudades(cd, ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;
        }

        public bool SaveCiudad(Ciudad cd, out int ID, ref string friendlyMessage)
        {
            ID = 0;
            string msg = string.Empty;
            var result = false;
            var dal = new CiudadDAL(this._DBName);

            msg = SatinizateAlta(cd);

            if (msg != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }

            result = dal.SaveCiudad(cd, out ID, ref friendlyMessage);

            return result;
        }

        public bool SaveCiudades(List<Ciudad> cds, out int id, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = false;
            var dal = new CiudadDAL(this._DBName);
            id = 0;

            string msgGral = string.Empty;
            int count = 0;


            foreach (Ciudad cd in cds)
            {
                count++;
                msg = SatinizateAlta(cd);

                if (msg != string.Empty)
                    msgGral += " De la Ciudad (" + count.ToString() + "): " + msg + "; ";

            }

            if (msgGral != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msgGral;

                return result;
            }

            result = dal.SaveCiudades(cds, out id, ref friendlyMessage);

            return result;

        }

        public List<CboTipo> GetCbo(Ciudad cd, ref string friendlyMessage)
        {
            List<CboTipo> result = new List<CboTipo>();
            var lst = GetCiudades(cd, ref friendlyMessage);

            foreach (Ciudad item in lst)
                result.Add(new CboTipo() { ID = item.CiudadID.ToString(), Nombre = item.Nombre, Abr = item.Abr  });

            return result;
        }

        private string SatinizateQuery(int afiliadoID)
        {
            string msg = string.Empty;

            if (afiliadoID == 0)
                msg += "Numero de Identificacion, ";

            return msg;

        }

        private string SatinizateAlta(Ciudad cd)
        {
            string msg = string.Empty;

            if (string.IsNullOrEmpty(cd.Nombre))
                msg = "Nombre";

            if (cd.Estado == null || cd.Estado.EstadoID == 0)
                msg = "Estado ID";

            return msg;

        }

    }
}
