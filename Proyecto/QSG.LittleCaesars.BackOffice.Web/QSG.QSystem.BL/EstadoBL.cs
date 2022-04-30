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
    public class EstadoBL
    {
        private string _DBName;

        public EstadoBL(string dbName)
        {
            this._DBName = dbName;
        }

        public List<Estado> GetEstados(Estado edo, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<Estado>();
            var dal = new EstadoDAL(this._DBName);

            result = dal.GetEstados(edo, ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;
        }

        public bool SaveEstado(Estado edo, out int ID, ref string friendlyMessage)
        {
            ID = 0;
            string msg = string.Empty;
            var result = false;
            var dal = new EstadoDAL(this._DBName);

            msg = SatinizateAlta(edo);

            if (msg != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }

            result = dal.SaveEstado(edo, out ID, ref friendlyMessage);

            return result;
        }

        public bool SaveEstados(List<Estado> edos, out int id, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = false;
            var dal = new EstadoDAL(this._DBName);
            id = 0;

            string msgGral = string.Empty;
            int count = 0;


            foreach (Estado edo in edos)
            {
                count++;
                msg = SatinizateAlta(edo);

                if (msg != string.Empty)
                    msgGral += " Del Estado (" + count.ToString() + "): " + msg + "; ";

            }

            if (msgGral != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msgGral;

                return result;
            }

            result = dal.SaveEstados(edos, out id, ref friendlyMessage);

            return result;

        }

        public List<CboTipo> GetCbo(Estado edo, ref string friendlyMessage)
        {
            List<CboTipo> result = new List<CboTipo>();
            var lst = GetEstados(edo, ref friendlyMessage);

            foreach (Estado item in lst)
                result.Add(new CboTipo() { ID = item.EstadoID.ToString(), Nombre = item.Nombre, Abr = item.Abr  });

            return result;
        }

        private string SatinizateQuery(int afiliadoID)
        {
            string msg = string.Empty;

            if (afiliadoID == 0)
                msg += "Numero de Identificacion, ";

            return msg;

        }

        private string SatinizateAlta(Estado cd)
        {
            string msg = string.Empty;

            if (string.IsNullOrEmpty(cd.Nombre))
                msg = "Nombre";

            if (cd.Pais == null || cd.Pais.PaisID == 0)
                msg = "Pais ID";

            return msg;

        }

    }
}
