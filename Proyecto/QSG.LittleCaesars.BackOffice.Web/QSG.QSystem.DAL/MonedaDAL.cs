using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPESD.Tools.DBHelper;

namespace QSG.QSystem.DAL
{
    public class MonedaDAL
    {
        private string _strConnection = string.Empty;
        private string _DBName;

        public MonedaDAL(string dbName)
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
            _DBName = dbName + ".dbo.";
        }
        public MonedaDAL(string dbName, string srtConnection)
        {
            _strConnection = srtConnection;
            _DBName = dbName + ".dbo.";
        }
        
        public List<Moneda> GetMonedas(Moneda entidad, ref string friendlyMessage)
        {
            var result = new List<Moneda>();
            string msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(entidad);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Msg", msg, System.Data.ParameterDirection.Output);

            DataSet ds = dbHelper.ExecuteDataset(_DBName + "getMonedass");
            friendlyMessage = msg;
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                
                Moneda m;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    m = new Moneda();
                    m.MonedaID = Convert.ToInt32(row["MonedaID"]);
                    m.Nombre = row["Nombre"].ToString();
                    m.Abr = row["Abr"].ToString();
                    m.CodigoISO = row["CodigoISO"].ToString();
                    m.Simbolo = row["Simbolo"].ToString();
                    m.FechaAlta = Convert.ToDateTime(row["FechaAlta"].ToString());
                    m.CodUsAlta = row["UsuaiorID"].ToString();

                    result.Add(m);
                }
                

            }
            return result;
        }


        public bool SaveMoneda(Moneda entidad, out int id, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;
            id = 0;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(entidad);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            dbHelper.CreateParameter<int>("@ID", id, System.Data.ParameterDirection.Output);

            dbHelper.ExecuteCommand(_DBName + "SaveMonedas");

            id = dbHelper.GetParameterValue<int>("@ID");
            Msg = dbHelper.GetParameterValue<string>("@Msg");

            result = true;
            if (Msg != String.Empty)
            {
                friendlyMessage = Msg;
                result = false;
            }

            return result;
        }


        public bool SaveMonedas(List<Moneda> lst, out int id, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;
            id = 0;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(lst);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Entidad", "ArrayOfEstado/Moneda");
            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            dbHelper.CreateParameter<int>("@ID", id, System.Data.ParameterDirection.Output);

            dbHelper.ExecuteCommand(_DBName + "SaveMonedas");

            id = dbHelper.GetParameterValue<int>("@ID");
            Msg = dbHelper.GetParameterValue<string>("@Msg");

            result = true;
            if (Msg != String.Empty)
            {
                friendlyMessage = Msg;
                result = false;
            }

            return result;
        }

    }
}
