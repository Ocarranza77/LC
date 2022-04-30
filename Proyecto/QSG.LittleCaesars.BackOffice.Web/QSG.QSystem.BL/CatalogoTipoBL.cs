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
    public class CatalogoTipoBL
    {
        private string _DBName;

        public CatalogoTipoBL(string dbName)
        {
            this._DBName = dbName;
        }

        public List<CatalogoTipo> GetCatalogoTipos(CatalogoTipo cat, CatalogoType type, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<CatalogoTipo>();
            var dal = new CatalogoTipoDAL(this._DBName, GetShema(type));

            result = dal.GetCatalogoTipo(cat, GetTableName(type), GetPkName(type), ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;
        }

        public bool SaveCatalogoTipo(CatalogoTipo cat, CatalogoType type, out int ID, ref string friendlyMessage)
        {
            ID = 0;
            string msg = string.Empty;
            var result = false;
            var dal = new CatalogoTipoDAL(this._DBName, GetShema(type));

            msg = SatinizateAlta(cat);

            if (msg != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }

            result = dal.SaveCatalogoTipo(cat, GetSPName(type), out ID, ref friendlyMessage);

            return result;
        }

        public bool SaveCatalogoTipos(List<CatalogoTipo> cats, CatalogoType type, out int id, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = false;
            var dal = new CatalogoTipoDAL(this._DBName, GetShema(type));
            id = 0;

            string msgGral = string.Empty;
            int count = 0;


            foreach (CatalogoTipo cat in cats)
            {
                count++;
                msg = SatinizateAlta(cat);

                if (msg != string.Empty)
                    msgGral += " Del " + Enum.GetName(typeof(CatalogoTipo), (int)type) + " (" + count.ToString() + "): " + msg + "; ";

            }

            if (msgGral != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msgGral;

                return result;
            }

            result = dal.SaveCatalogoTipos(cats, GetSPName(type), out id, ref friendlyMessage);

            return result;

        }

        public List<CboTipo> GetCbo(CatalogoTipo cat, CatalogoType type, ref string friendlyMessage)
        {
            List<CboTipo> result = new List<CboTipo>();
            var lst = GetCatalogoTipos(cat, type, ref friendlyMessage);

            foreach (CatalogoTipo item in lst)
                result.Add(new CboTipo() { ID = item.ID.ToString(), Nombre = item.Nombre, Abr = item.Abr  });

            return result;
        }

        private string SatinizateQuery(int afiliadoID)
        {
            string msg = string.Empty;

            if (afiliadoID == 0)
                msg += "Numero de Identificacion, ";

            return msg;

        }

        private string SatinizateAlta(CatalogoTipo cat)
        {
            string msg = string.Empty;

            if (string.IsNullOrEmpty(cat.Nombre))
                msg = "Nombre";

            return msg;

        }

        private string GetShema(CatalogoType type)
        {
            switch ((int)type)
            {
                case 0-4: return ".dbo.";
                default: return ".dbo.";
            }
        }

        private string GetTableName(CatalogoType type)
        {
            switch ((int)type)
            {
                case 0: return "DomicilioTipo";
                case 1: return "PersonaSexo";
                case 2: return "PersonaTipo";
                case 3: return "TelefonoCompania";
                case 4: return "TelefonoTipo";
                case 5: return "Bancos";
                default: return "";
            }
        }

        private string GetPkName(CatalogoType type)
        {
            switch ((int)type)
            {
                case 0: return "DomicilioTipoID";
                case 1: return "SexoID";
                case 2: return "PersonaTipoID";
                case 3: return "TelefonoCompaniaID";
                case 4: return "TelefonoTipoID";
                case 5: return "BancoID";
                default: return "";
            }
        }

        private string GetSPName(CatalogoType type)
        {
            switch ((int)type)
            {
                case 0: return "SaveDomicilioTipo";
                case 1: return "SavePersonaSexo";
                case 2: return "SavePersonaTipo";
                case 3: return "SaveTelefonoCompania";
                case 4: return "SaveTelefonoTipo";
                default: return "";
            }
        }
    }
}
