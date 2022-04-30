using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.LittleCaesars.BackOffice.Common.Reports;
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
    public class RepFacturaPGDAL
    {
        private string _strConnection = string.Empty;

        public RepFacturaPGDAL()
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
        }

        public RepFacturaPGDAL(string strConnection)
        {
            _strConnection = strConnection;
        }

        public List<RepFacturaPG> Reporte(DateTime fecha, int  usRqst, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<RepFacturaPG>();


            DBHelper dbHelper = new DBHelper(_strConnection);
            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@FechaVta";
            prmData.SqlDbType = SqlDbType.VarChar;
            prmData.Size = 25;
            prmData.Value = fecha.ToString("yyyyMMdd");
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<int>("@UsRqst", usRqst);
            DataSet ds = dbHelper.ExecuteDataset("GetFacturaPG");
           

            friendlyMessage = msg;


            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                var rf = new RepFacturaPG();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    rf = new RepFacturaPG();

                    rf.FoliosFacturaPG = (string)row["FoliosFacturasPG"].ToString();
                    rf.ImporteFacturaCliente = row["FactCli"] != DBNull.Value ? Convert.ToDouble(row["FactCli"]) : 0;
                    rf.ImporteFacturaClienteCancelada = row["FactCliCan"] != DBNull.Value ? Convert.ToDouble(row["FactCliCan"]) : 0;
                    rf.ImporteFacturaPG = row["FactPG"] != DBNull.Value ? Convert.ToDouble(row["FactPG"]) : 0;
                    rf.ImporteFacturaPGCancelada = row["FactPGCan"] != DBNull.Value ? Convert.ToDouble(row["FactPGCan"]) : 0;
                    rf.NumeroFacturasPG = row["NumFacturasPG"] != DBNull.Value ? Convert.ToInt32(row["NumFacturasPG"]) : 0;
                    rf.Sucursal = (string)row["Abr"].ToString();
                    rf.SucursalID = row["SucursalID"] != DBNull.Value ? Convert.ToInt32(row["SucursalID"]) : 0;
                    rf.TotalVenta = row["Total"] != DBNull.Value ? Convert.ToDouble(row["Total"]) : 0;
                    rf.Empresa = row["Abr"].ToString();

                    result.Add(rf);
                }
                
            }

            return result;
        }

    }
}
