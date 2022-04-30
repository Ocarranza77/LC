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
    public class CheckListTipoDAL
    {
        private string _strConnection = string.Empty;
        private string _DBName;

        public CheckListTipoDAL(string dbName, string schema)
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
            _DBName = dbName + schema;
        }
        public CheckListTipoDAL(string dbName, string schema, string srtConnection)
        {
            _strConnection = srtConnection;
            _DBName = dbName + schema;
        }
        
        public List<CheckListTipo> GetCheckListTipo(CheckListTipo cklTipo, string tlName, string pkName, ref string friendlyMessage)
        {
            var result = new List<CheckListTipo>();
            string msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(cklTipo);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Msg", msg, System.Data.ParameterDirection.Output);
            dbHelper.CreateParameter<string>("@PkName", pkName);
            dbHelper.CreateParameter<string>("@TablaName", _DBName + tlName);

            DataSet ds = dbHelper.ExecuteDataset(_DBName + "getCheckListTipo");
            friendlyMessage = msg;
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                
                CheckListTipo cat;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    cat = new CheckListTipo();
                    cat.ID = Convert.ToInt32(row[pkName]);
                    cat.Grupo = row["Grupo"].ToString();
                    cat.Elemento = row["Elemento"].ToString();
                    cat.Activo = (bool)row["Activo"];
                    cat.FechaAlta = Convert.ToDateTime(row["FechaAlta"].ToString());
                    //cat.UsuarioID = row["UsuarioID"] != DBNull.Value ? Convert.ToInt32(row["UsuarioID"]) : 0;
                    cat.CodUsAlta = row["UsuarioID"].ToString();

                    result.Add(cat);
                }
                

            }
            return result;
        }


        public bool SaveCheckListTipo(CheckListTipo cklTipo, string spName, out int id, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;
            id = 0;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(cklTipo);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            dbHelper.CreateParameter<int>("@ID", id, System.Data.ParameterDirection.Output);

            dbHelper.ExecuteCommand(_DBName + spName);

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


        public bool SaveCheckListTipos(List<CheckListTipo> cklTipo, string spName, out int id, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;
            id = 0;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(cklTipo);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Entidad", "ArrayOfCheckListTipo/CheckTipo");
            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            dbHelper.CreateParameter<int>("@ID", id, System.Data.ParameterDirection.Output);

            dbHelper.ExecuteCommand(_DBName + spName);

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
