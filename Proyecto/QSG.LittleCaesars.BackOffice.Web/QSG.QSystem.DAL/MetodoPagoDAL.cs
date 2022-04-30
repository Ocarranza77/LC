using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPESD.Tools.DBHelper;
using System.Globalization;
using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Entities;

namespace QSG.QSystem.DAL
{
    public class MetodoPagoSATDAL
    {
        private string _strConnection = string.Empty;

        public MetodoPagoSATDAL()
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
        }

        public MetodoPagoSATDAL(string strConnection)
        {
            _strConnection = strConnection;
        }

        public List<MetodoPagoSAT> GetMetodoPagos(ref string friendlyMessage) 
        {
            var result = new List<MetodoPagoSAT>();
            string msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);
            DataSet ds = dbHelper.ExecuteDataset("Qbic.dbo.getMetodoPago");

            friendlyMessage = msg;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {

                MetodoPagoSAT mep;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    mep = new MetodoPagoSAT();
                    mep.CodMetodoP = row["CodMetodoP"].ToString();
                    mep.Descripcion = row["Descripcion"].ToString();
                    result.Add(mep);
                }

            }
            return result;   
        }

     
    }
}
