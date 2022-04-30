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
    public class RepIngresosDSMDAL
    {
        private string _strConnection = string.Empty;

        public RepIngresosDSMDAL()
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
        }

        public RepIngresosDSMDAL(string strConnection)
        {
            _strConnection = strConnection;
        }

        public RepIngresosDSM Reporte(DateTime fecha, int sucursalID, int empresaID, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new RepIngresosDSM();
            BaseStrDub baseSD;
            DateTime dt;

            int NumTbl = 0;

            DBHelper dbHelper = new DBHelper(_strConnection);
            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@Dia";
            prmData.SqlDbType = SqlDbType.Date;
            prmData.Value = fecha;

            dbHelper.AddParameter(prmData);

            SqlParameter prmSucID = new SqlParameter();
            prmSucID.ParameterName = "@SucursalID";
            prmSucID.SqlDbType = SqlDbType.Int;
            prmSucID.Value = sucursalID;

            dbHelper.AddParameter(prmSucID);

            SqlParameter prmEmpID = new SqlParameter();
            prmEmpID.ParameterName = "@EmpresaID";
            prmEmpID.SqlDbType = SqlDbType.Int;
            prmEmpID.Value = empresaID;

            dbHelper.AddParameter(prmEmpID);

            DataSet ds = dbHelper.ExecuteDataset("RepIngresosDSM");

            friendlyMessage = msg;


            // Dia
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[NumTbl].Rows != null && ds.Tables[NumTbl].Rows.Count > 0)
            {
                result.Dia = new List<BaseStrDub>();
                
                foreach (DataRow row in ds.Tables[NumTbl].Rows)
                {
                    baseSD = new BaseStrDub();

                    baseSD.strNombre = row["Nombre"].ToString(); 
                    //DateTime.TryParse(row["Fecha"].ToString(), out dt);
                    //baseSD.strDato = dt.ToShortDateString();
                    baseSD.dblValor = Convert.ToDouble(row["Venta"]); // (double)row["Importe"];
                    baseSD.dblValor2 = Convert.ToDouble(row["Factura"]);

                    result.Dia.Add(baseSD);
                }

                
            }

            // Semana
            NumTbl = 1;
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[NumTbl].Rows != null && ds.Tables[NumTbl].Rows.Count > 0)
            {
                
                result.Semana = new List<BaseStrDub>();
                foreach (DataRow row in ds.Tables[NumTbl].Rows)
                {
                    baseSD = new BaseStrDub();

                    baseSD.strNombre = row["Nombre"].ToString();
                    DateTime.TryParse(row["Fecha"].ToString(), out dt);
                    baseSD.strDato = dt.ToShortDateString();

                    baseSD.dblValor = Convert.ToDouble(row["Venta"]); // (double)row["Importe"];
                    baseSD.dblValor2 = Convert.ToDouble(row["Factura"]);

                    result.Semana.Add(baseSD);
                }
            }

            // Mes
            NumTbl = 2;
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[NumTbl].Rows != null && ds.Tables[NumTbl].Rows.Count > 0)
            {
                
                result.Mes = new List<BaseStrDub>();
                foreach (DataRow row in ds.Tables[NumTbl].Rows)
                {
                    baseSD = new BaseStrDub();

                    baseSD.strNombre = row["Nombre"].ToString();
                    //DateTime.TryParse(row["Fecha"].ToString(), out dt);
                    baseSD.strDato = row["Fecha"].ToString();// dt.ToShortDateString();

                    baseSD.dblValor = Convert.ToDouble(row["Venta"]); // (double)row["Importe"];
                    baseSD.dblValor2 = Convert.ToDouble(row["Factura"]);

                    result.Mes.Add(baseSD);
                }
            }

            return result;
        }

    }
}
