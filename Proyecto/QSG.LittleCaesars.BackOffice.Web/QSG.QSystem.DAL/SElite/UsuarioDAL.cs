using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Entities;
using QSG.QSystem.Common.Entities.SElite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPESD.Tools.DBHelper;

namespace QSG.QSystem.DAL.SElite
{
    public class UsuarioDAL
    {
        private string _strConnection = string.Empty;
        private string _DBName;

        public UsuarioDAL(string dbName)
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
            _DBName = dbName + ".SElite.";
        }
        public UsuarioDAL(string dbName, string srtConnection)
        {
            _strConnection = srtConnection;
            _DBName = dbName + ".SElite.";
        }

        public List<Usuario> GetUsuarios(Usuario filtro, ref string friendlyMessage)
        {
            var result = new List<Usuario>();
            string msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(filtro);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Msg", msg, System.Data.ParameterDirection.Output);

            DataSet ds = dbHelper.ExecuteDataset(_DBName + "getUsuarios");
            friendlyMessage = msg;
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                
                Usuario u;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    u = new Usuario();
                    u.UsuarioID = Convert.ToInt32(row["UsuarioID"]);
                    u.Email = row["Email"].ToString();
                    u.Nombre = row["Nombre"].ToString();
                    u.Alias = row["Alias"].ToString();
                    u.Titulo = row["Titulo"].ToString();
                    u.Activo = Convert.ToBoolean(row["Activo"]);
                    u.Bloqueo = Convert.ToBoolean(row["Bloqueo"]);
                    u.Tipo = row["Tipo"] == DBNull.Value ? 0 : Convert.ToInt32(row["Tipo"]);
                    u.ShortCutMenuTpe = row["ShortCutMenuType"] == DBNull.Value ? 0 : Convert.ToInt32(row["ShortCutMenuType"]);

                    u.FechaAlta = Convert.ToDateTime(row["FechaAlta"].ToString());
                    u.FechaBaja = Convert.ToDateTime(row["FechaBaja"].ToString());
                    u.PhotoPath = row["PhotoPath"].ToString();

                    u.Persona = new Persona();
                    if(row["PersonaID"] != DBNull.Value && Convert.ToInt32(row["PersonaID"]) > 0 )
                    {
                        u.Persona.Nombre = row.Field<string>("Nombre");
                        //ToDo: Datos regresados de la persona.
                    }

                    u.ClienteQ = new ClienteQ();
                    if(row["ClienteQID"] != DBNull.Value && Convert.ToInt32(row["ClienteQID"]) > 0 )
                    {
                        u.ClienteQ.Nombre = row.Field<string>("Nombre");
                        //ToDo: Datos regresados de la persona.
                    }

                    result.Add(u);
                }
                
            }
            return result;
        }


        public bool SaveUsuario(Usuario entidad, out int id, ref string friendlyMessage)
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

            dbHelper.ExecuteCommand(_DBName + "SaveUsuarios");

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


        public bool SaveUsuarios(List<Usuario> lst, out int id, ref string friendlyMessage)
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

            dbHelper.CreateParameter<string>("@Entidad", "ArrayOfUsuario/Usuario");
            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            dbHelper.CreateParameter<int>("@ID", id, System.Data.ParameterDirection.Output);

            dbHelper.ExecuteCommand(_DBName + "SaveUsuarios");

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

        public bool ChangePwd(Usuario entidad, int all, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(entidad);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            dbHelper.CreateParameter<int>("@All", all);

            dbHelper.ExecuteCommand(_DBName + "SaveChangePwdUsuario");

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
