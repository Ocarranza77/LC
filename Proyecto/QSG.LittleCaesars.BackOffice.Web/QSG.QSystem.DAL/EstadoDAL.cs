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
    public class EstadoDAL
    {
        private string _strConnection = string.Empty;
        private string _DBName;

        public EstadoDAL(string dbName)
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
            _DBName = dbName + ".dbo.";
        }
        public EstadoDAL(string dbName, string srtConnection)
        {
            _strConnection = srtConnection;
            _DBName = dbName + ".dbo.";
        }
        
        public List<Estado> GetEstados(Estado edo, ref string friendlyMessage)
        {
            var result = new List<Estado>();
            string msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(edo);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Msg", msg, System.Data.ParameterDirection.Output);

            DataSet ds = dbHelper.ExecuteDataset(_DBName + "getEstados");
            friendlyMessage = msg;
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                
                Estado e;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    e = new Estado();
                    e.EstadoID = Convert.ToInt32(row["EstadoID"]);
                    e.Nombre = row["Nombre"].ToString();
                    e.Abr = row["Abr"].ToString();
                    e.Pais = new Pais() {PaisID = row.Field<int>("PaisID")};
                    e.FechaAlta = Convert.ToDateTime(row["FechaAlta"].ToString());
                    e.UsuarioID = Convert.ToInt32(row["UsuaiorID"]);

                    result.Add(e);
                }
                

            }
            return result;
        }


        public bool SaveEstado(Estado edo, out int id, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;
            id = 0;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(edo);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            dbHelper.CreateParameter<int>("@ID", id, System.Data.ParameterDirection.Output);

            dbHelper.ExecuteCommand(_DBName + "SaveEstados");

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


        public bool SaveEstados(List<Estado> edos, out int id, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;
            id = 0;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(edos);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Entidad", "ArrayOfEstado/Estado");
            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            dbHelper.CreateParameter<int>("@ID", id, System.Data.ParameterDirection.Output);

            dbHelper.ExecuteCommand(_DBName + "SaveEstados");

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
