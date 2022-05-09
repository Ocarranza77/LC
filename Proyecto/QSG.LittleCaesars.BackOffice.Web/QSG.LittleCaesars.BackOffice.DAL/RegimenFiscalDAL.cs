using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.QSystem.Common.Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPESD.Tools.DBHelper;


namespace QSG.LittleCaesars.BackOffice.DAL
{
    public class RegimenFiscalDAL
    {
        private string _strConnection = string.Empty;

        public RegimenFiscalDAL()
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
        }

        public RegimenFiscalDAL(string strConnection)
        {
            _strConnection = strConnection;
        }

        public List<RegimenFiscal> GetRegimenFiscales(ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<RegimenFiscal>();
            friendlyMessage = "";

            DBHelper dbHelper = new DBHelper(_strConnection);
            
            DataSet ds = dbHelper.ExecuteDataset("getRegimenFiscales");

            //Cliente
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                RegimenFiscal rf;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    rf = new RegimenFiscal();

                    rf.CodRegimenFiscal = row["CodRegimenFiscal"].ToString();
                    rf.Nombre = row["Nombre"].ToString();
                    rf.FisicaMoral = row["FisicaMoral"].ToString();

                    result.Add(rf);
                }
            }

            return result;
        }

    }
}
