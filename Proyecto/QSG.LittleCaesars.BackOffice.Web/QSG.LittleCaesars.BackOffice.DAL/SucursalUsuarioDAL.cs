using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using XPESD.Tools.DBHelper;
using System.Globalization;
using QSG.QSystem.Common.Constants;
using QSG.QSystem.DAL;

namespace QSG.LittleCaesars.BackOffice.DAL
{
    public class SucursalUsuarioDAL
    {
        private string _strConnection = string.Empty;
        private SqlConnection _cnn;
        private SqlCommand _cmd;

        public SucursalUsuarioDAL()
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
        }

        public SucursalUsuarioDAL(string strConnection)
        {
            _strConnection = strConnection;
        }


        public List<SucursalUsuario> GetSucursalesUsuario(int sucursalID, int usuarioID,  ref string friendlyMessage)
        {
            var result = new List<SucursalUsuario>();
            string msg = string.Empty;

            //DBHelper dbHelper = new DBHelper(_strConnection);
            //dbHelper.CreateParameter<int>("@SucursalID", sucursalID);
            //dbHelper.CreateParameter<int>("@UsuarioID", usuarioID);

            //DataSet ds = dbHelper.ExecuteDataset("getSucursalesUsuarios");

            //friendlyMessage = msg;

            HelperDAL.CreateConnection(_strConnection, "getSucursalesUsuarios", ref _cnn, ref _cmd);
//            HelperDAL.CreateConnection(_strConnection, _DBName + "getSucursalesUsuarios", ref _cnn, ref _cmd);
            _cmd.Parameters.Add(HelperDAL.CreateParameter("@SucursalID", sucursalID, SqlDbType.Int));
            _cmd.Parameters.Add(HelperDAL.CreateParameter("@UsuarioID", usuarioID, SqlDbType.Int));
            //_cmd.Parameters.Add(HelperDAL.CreateParameterMsg());
            DataSet ds = HelperDAL.ExecuteDataSet(_cmd);

            friendlyMessage = ""; // _cmd.Parameters[2].Value.ToString();



            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
             
                SucursalUsuario su;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    su = new SucursalUsuario();
                    su.Sucursal = new Sucursal();
                    su.Sucursal.SucursalID = Convert.ToInt32(row["SucursalID"]);
                    su.Sucursal.Nombre = (string)row["SucursalNom"].ToString().ToUpper();
                    su.Sucursal.Abr = (string)row["SucursalAbr"].ToString().ToUpper();

                    su.UsuarioPermisoID = Convert.ToInt32(row["UsuarioPermisoID"]);
                    su.Nombre = (string)row["UsuarioNom"].ToString();

                    result.Add(su);
                }

            }

            return result;
        }

        public bool SaveSucursalesUsuario(List<SucursalUsuario> lst, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;

            //DBHelper dbHelper = new DBHelper(_strConnection);

            //SqlParameter prmData = new SqlParameter();
            //prmData.ParameterName = "@prmXML";
            //prmData.SqlDbType = SqlDbType.Xml;
            //prmData.Value = dbHelper.SerializeToXML(lst);
            //dbHelper.AddParameter(prmData);

            //dbHelper.CreateParameter<string>("@Entidad", "ArrayOfSucursalUsuario/SucursalUsuario");
            //dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);

            //dbHelper.ExecuteCommand("SaveSucursalesUsuarios");



//            HelperDAL.CreateConnection(_strConnection, _DBName + "SaveDepositos", ref _cnn, ref _cmd);
            HelperDAL.CreateConnection(_strConnection, "SaveSucursalesUsuarios", ref _cnn, ref _cmd); //ArrayOfSucursalUsuario/SucursalUsuario
            _cmd.Parameters.Add(HelperDAL.CreateParameter("@prmXML", lst, SqlDbType.Xml));
            _cmd.Parameters.Add(HelperDAL.CreateParameter("@Entidad", "ArrayOfSucursalUsuario/SucursalUsuario"));
            _cmd.Parameters.Add(HelperDAL.CreateParameterMsg());
            HelperDAL.ExecuteNonQuery(ref _cnn, ref _cmd);

            Msg = _cmd.Parameters[2].Value.ToString();


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
