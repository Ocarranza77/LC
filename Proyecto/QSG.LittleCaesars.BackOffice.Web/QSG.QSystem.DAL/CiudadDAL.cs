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
    public class CiudadDAL
    {
        private string _strConnection = string.Empty;
        private string _DBName;

        public CiudadDAL(string dbName)
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
            _DBName = dbName + ".dbo.";
        }
        public CiudadDAL(string dbName, string srtConnection)
        {
            _strConnection = srtConnection;
            _DBName = dbName + ".dbo.";
        }
        
        public List<Ciudad> GetCiudades(Ciudad cd, ref string friendlyMessage)
        {
            var result = new List<Ciudad>();
            string msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(cd);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Msg", msg, System.Data.ParameterDirection.Output);

            DataSet ds = dbHelper.ExecuteDataset(_DBName + "getCiudades");
            friendlyMessage = msg;
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                
                Ciudad c;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    c = new Ciudad();
                    c.CiudadID = Convert.ToInt32(row["CiudadID"]);
                    c.Nombre = row["Nombre"].ToString();
                    c.Abr = row["Abr"].ToString();
                    c.Estado = new Estado() {EstadoID = row.Field<int>("EstadoID")};
                    c.FechaAlta = Convert.ToDateTime(row["FechaAlta"].ToString());
                    c.UsuarioID = Convert.ToInt32(row["UsuaiorID"]);

                    result.Add(c);
                }
                

            }
            return result;
        }


        public bool SaveCiudad(Ciudad cd, out int id, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;
            id = 0;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(cd);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            dbHelper.CreateParameter<int>("@ID", id, System.Data.ParameterDirection.Output);

            dbHelper.ExecuteCommand(_DBName + "SaveCiudades");

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


        public bool SaveCiudades(List<Ciudad> cds, out int id, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;
            id = 0;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(cds);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Entidad", "ArrayOfCiudad/Ciudad");
            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            dbHelper.CreateParameter<int>("@ID", id, System.Data.ParameterDirection.Output);

            dbHelper.ExecuteCommand(_DBName + "SaveCiudades");

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
