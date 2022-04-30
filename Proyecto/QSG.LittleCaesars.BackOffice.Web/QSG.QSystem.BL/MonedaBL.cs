using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Entities;
using QSG.QSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.QSystem.BL
{
    public class MonedaBL
    {
        private string _DBName;

        public MonedaBL(string dbName)
        {
            this._DBName = dbName;
        }

        public List<Moneda> GetMonedas(Moneda entidad, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<Moneda>();
            var dal = new MonedaDAL(this._DBName);

            result = dal.GetMonedas(entidad, ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;
        }

        public bool SaveMoneda(Moneda entidad, out int ID, ref string friendlyMessage)
        {
            ID = 0;
            string msg = string.Empty;
            var result = false;
            var dal = new MonedaDAL(this._DBName);

            msg = SatinizateAlta(entidad);

            if (msg != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }

            result = dal.SaveMoneda(entidad, out ID, ref friendlyMessage);

            return result;
        }

        public bool SaveMonedas(List<Moneda> lst, out int id, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = false;
            var dal = new MonedaDAL(this._DBName);
            id = 0;

            string msgGral = string.Empty;
            int count = 0;


            foreach (Moneda ett in lst)
            {
                count++;
                msg = SatinizateAlta(ett);

                if (msg != string.Empty)
                    msgGral += " De la Moneda (" + count.ToString() + "): " + msg + "; ";

            }

            if (msgGral != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msgGral;

                return result;
            }

            result = dal.SaveMonedas(lst, out id, ref friendlyMessage);

            return result;

        }

        public List<CboTipo> GetCbo(Moneda itm, ref string friendlyMessage)
        {
            List<CboTipo> result = new List<CboTipo>();
            var lst = GetMonedas(itm, ref friendlyMessage);

            foreach (Moneda item in lst)
                result.Add(new CboTipo() { ID = item.MonedaID.ToString(), Nombre = item.Nombre, Abr = item.Abr  });

            return result;
        }

        private string SatinizateQuery(int id)
        {
            string msg = string.Empty;

            if (id == 0)
                msg += "Numero de Identificacion, ";

            return msg;

        }

        private string SatinizateAlta(Moneda cd)
        {
            string msg = string.Empty;

            if (string.IsNullOrEmpty(cd.Nombre))
                msg = "Nombre";

            return msg;

        }

    }
}
