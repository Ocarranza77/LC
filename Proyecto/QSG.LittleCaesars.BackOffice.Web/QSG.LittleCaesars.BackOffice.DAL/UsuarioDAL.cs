using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using XPESD.Tools.DBHelper;
using System.Data.SqlClient;
using System.Security.Policy;
using QSG.QSystem.Common.Constants;

namespace QSG.LittleCaesars.BackOffice.DAL
{
   public class UsuarioDAL
    {

       private string _strConnection = string.Empty;

        public UsuarioDAL()
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
        }

        public UsuarioDAL(string strConnection)
        {
            _strConnection = strConnection;
        }

        public Usuario GetUsuario(Usuario user, ref string FriendlyMessage)
        {
            string msg = string.Empty;
            var result = new Usuario();
           

            DBHelper dbHelper = new DBHelper(_strConnection);
            dbHelper.CreateParameter<string>("@Alias", user.Alias.Trim());
            dbHelper.CreateParameter<string>("@Clave",  user.Clave.Trim());
            /*dbHelper.CreateParameter<int>("@CajaID", ticket.CajaID);
            dbHelper.CreateParameter<double>("@Importe", ticket.Importe);
           */
            DataSet ds = dbHelper.ExecuteDataset("GetUsuario");



            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
             
                DataRow row = ds.Tables[0].Rows[0];
                result.CodUsuario = Convert.ToInt32(row["CodUsuario"]);
                result.Alias = (string)row["Alias"];
                result.Clave = (string)row["Clave"];
                result.Nombre = (string)row["Nombre"];
                result.Puesto = (string)row["Puesto"];

               
            }else{
                result = null;
            }

            

            return result;
        }

        public List<Usuario> GetUsuarios(Usuario user, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<Usuario>();


            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(user);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Msg", msg, System.Data.ParameterDirection.Output);

            DataSet ds = dbHelper.ExecuteDataset("GetUsuarios");

            friendlyMessage = msg;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                Usuario usuario;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    usuario = new Usuario();
                    usuario.CodUsuario = Convert.ToInt32(row["CodUsuario"]);
                    usuario.Alias = (string)row["Alias"];
                    usuario.Clave = (string)row["Clave"];
                    usuario.Nombre = (string)row["Nombre"];
                    usuario.Puesto = (string)row["Puesto"];
                    result.Add(usuario);
                }
            }

            return result;
        }

        public bool SaveUsuario(Usuario usuario, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);
            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(usuario);
            dbHelper.AddParameter(prmData);
            
            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            
            dbHelper.ExecuteCommand("SaveUsuario");

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
