using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPESD.Tools.DBHelper;
using QSG.QSystem.Common.Entities;

namespace QSG.QSystem.DAL
{
    public class PaisDAL
    {
        private string _strConnection = string.Empty;
        private string _DBName;

        public PaisDAL(string dbName)
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
            _DBName = dbName + ".dbo.";
        }
        public PaisDAL(string dbName, string srtConnection)
        {
            _strConnection = srtConnection;
            _DBName = dbName + ".dbo.";
        }
        
        public List<Pais> GetPaises(Pais pais, ref string friendlyMessage)
        {
            var result = new List<Pais>();
            string msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(pais);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Msg", msg, System.Data.ParameterDirection.Output);

            DataSet ds = dbHelper.ExecuteDataset(_DBName + "getPaises");
            friendlyMessage = msg;
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                
                Pais p;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    p = new Pais();
                    p.PaisID = Convert.ToInt32(row["PaisID"]);
                    p.Nombre = row["Nombre"].ToString();
                    p.Abr = row["Abr"].ToString();
                    p.Nacionalidad = row["Nacionalidad"].ToString();
                    p.CodigoISOn = row["CodigoISOn"] == DBNull.Value ? 0 : row.Field<int>("CodigoISOn");
                    p.CodigoISOl = row["CodigoISOl"].ToString();
                    p.SimboloMoneda = row["SimboloMoneda"].ToString();
                    p.Moneda = row["Moneda"].ToString();
                    p.FechaAlta = row.Field<DateTime>("FechaAlta");
                    p.UsuarioID = Convert.ToInt32(row["UsuaiorID"]);

                    result.Add(p);
                }
                

            }
            return result;
        }


        public bool SavePais(Pais pais, out int id, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;
            id = 0;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(pais);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            dbHelper.CreateParameter<int>("@ID", id, System.Data.ParameterDirection.Output);

            dbHelper.ExecuteCommand(_DBName + "SavePaises");

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


        public bool SavePaises(List<Pais> paises, out int id, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;
            id = 0;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(paises);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Entidad", "ArrayOfPais/Pais");
            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            dbHelper.CreateParameter<int>("@ID", id, System.Data.ParameterDirection.Output);

            dbHelper.ExecuteCommand(_DBName + "SavePaises");

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
