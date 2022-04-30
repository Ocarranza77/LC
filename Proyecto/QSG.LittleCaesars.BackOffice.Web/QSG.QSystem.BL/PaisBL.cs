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
    public class PaisBL
    {
        private string _DBName;

        public PaisBL(string dbName)
        {
            this._DBName = dbName;
        }

        public List<Pais> GetPaises(Pais pais, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<Pais>();
            var dal = new PaisDAL(this._DBName);

            result = dal.GetPaises(pais, ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;
        }

        public bool SavePais(Pais pais, out int ID, ref string friendlyMessage)
        {
            ID = 0;
            string msg = string.Empty;
            var result = false;
            var dal = new PaisDAL(this._DBName);

            msg = SatinizateAlta(pais);

            if (msg != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }

            result = dal.SavePais(pais, out ID, ref friendlyMessage);

            return result;
        }

        public bool SavePaises(List<Pais> paises, out int id, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = false;
            var dal = new PaisDAL(this._DBName);
            id = 0;

            string msgGral = string.Empty;
            int count = 0;


            foreach (Pais edo in paises)
            {
                count++;
                msg = SatinizateAlta(edo);

                if (msg != string.Empty)
                    msgGral += " Del Pais (" + count.ToString() + "): " + msg + "; ";

            }

            if (msgGral != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msgGral;

                return result;
            }

            result = dal.SavePaises(paises, out id, ref friendlyMessage);

            return result;

        }

        public List<CboTipo> GetCbo(Pais pais, bool nacionalidad, ref string friendlyMessage)
        {
            List<CboTipo> result = new List<CboTipo>();
            var lst = GetPaises(pais, ref friendlyMessage);

            foreach (Pais item in lst)
                result.Add(new CboTipo() { ID = item.PaisID.ToString(), Nombre = nacionalidad ? item.Nacionalidad : item.Nombre, Abr = item.Abr  });

            return result;
        }

        private string SatinizateQuery(int afiliadoID)
        {
            string msg = string.Empty;

            if (afiliadoID == 0)
                msg += "Numero de Identificacion, ";

            return msg;

        }

        private string SatinizateAlta(Pais cd)
        {
            string msg = string.Empty;

            if (string.IsNullOrEmpty(cd.Nombre))
                msg = "Nombre";

            if (string.IsNullOrEmpty(cd.Nacionalidad))
                msg = "Nacionalidad";

            return msg;

        }

    }
}
