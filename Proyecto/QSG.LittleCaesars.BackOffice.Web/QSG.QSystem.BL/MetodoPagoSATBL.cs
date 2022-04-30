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
    public class MetodoPagoSATBL
    {
        //private string _DBName;

        //public MetodoPagoSATBL(string dbName)
        //{
        //    this._DBName = dbName;
        //}

        public List<MetodoPagoSAT> GetMetodoPagos(ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<MetodoPagoSAT>();
            var dal = new MetodoPagoSATDAL(); //this._DBName

            result = dal.GetMetodoPagos(ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;
        }

        public List<CboTipo> GetCbo(ref string friendlyMessage)
        {
            List<CboTipo> result = new List<CboTipo>();
            var lst = GetMetodoPagos(ref friendlyMessage);

            foreach (MetodoPagoSAT item in lst)
                result.Add(new CboTipo() { ID = item.CodMetodoP, Nombre = item.Descripcion});

            return result;
        }

    }
}
