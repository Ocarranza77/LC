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
using QSG.QSystem.Common.Entities;
using QSG.QSystem.DAL;

namespace QSG.LittleCaesars.BackOffice.DAL
{
    public class CuentaBancoDAL
    {
        private string _strConnection = string.Empty;
        private SqlConnection _cnn;
        private SqlCommand _cmd;
        private string _DBName;

        public CuentaBancoDAL(string dbName)
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
            _DBName = dbName + GeneralApp.schema;
        }

        public CuentaBancoDAL(string dbName, string strConnection)
        {
            _strConnection = strConnection;
            _DBName = dbName + GeneralApp.schema;
        }


        public List<CuentaBanco> GetCuentaBancos(CuentaBanco entidad, int usRqst,  ref string friendlyMessage)
        {
            var result = new List<CuentaBanco>();
            string msg = string.Empty;

            //DBHelper dbHelper = new DBHelper(_strConnection);
            ////dbHelper.CreateParameter<int>("@SucursalID", sucursal.SucursalID);
            
            //SqlParameter prmData = new SqlParameter();
            //prmData.ParameterName = "@prmXML";
            //prmData.SqlDbType = SqlDbType.Xml;
            //prmData.Value = dbHelper.SerializeToXML(entidad);
            //dbHelper.AddParameter(prmData);

            //dbHelper.CreateParameter<string>("@Msg", msg, System.Data.ParameterDirection.Output);
            
            
            //DataSet ds = dbHelper.ExecuteDataset("getCuentaBanco");

            HelperDAL.CreateConnection(_strConnection, _DBName + "getCuentaBanco", ref _cnn, ref _cmd);
            _cmd.Parameters.Add(HelperDAL.CreateParameter("@prmXML", entidad, SqlDbType.Xml));
            _cmd.Parameters.Add(HelperDAL.CreateParameterMsg());
            DataSet ds = HelperDAL.ExecuteDataSet(_cmd);

            friendlyMessage = _cmd.Parameters[1].Value.ToString();

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
             
                CuentaBanco cb;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    cb = new CuentaBanco();
                    cb.CtaBcoID = Convert.ToInt32(row["CtaBcoID"]);
                    cb.Empresa = row["Empresa"].ToString().ToUpper();
                    cb.Banco = new CatalogoTipo() {
                        ID = Convert.ToInt32(row["BancoID"]),
                        Abr = row["BancoAbr"].ToString(),
                        Nombre = row["BancoNom"].ToString()
                    };

                    cb.Moneda = new Moneda()
                    {
                        MonedaID = Convert.ToInt32(row["MonedaID"]),
                        Abr = row["MonedaAbr"].ToString(),
                        Nombre = row["MonedaNom"].ToString()
                    };

                    cb.NoCta = row["NoCta"].ToString();
                    cb.Titular = row["Titular"].ToString();
                    cb.Descripcion = row["Descripcion"].ToString();
                    cb.Notas = row["Notas"].ToString();

                    //cb.Fec = row["Notas"].ToString();

                    result.Add(cb);
                }

            }


            return result;
        }

        public bool SaveCuentaBanco(CuentaBanco entidad, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;

            //DBHelper dbHelper = new DBHelper(_strConnection);

            //SqlParameter prmData = new SqlParameter();
            //prmData.ParameterName = "@prmXML";
            //prmData.SqlDbType = SqlDbType.Xml;
            //prmData.Value = dbHelper.SerializeToXML(entidad);
            //dbHelper.AddParameter(prmData);

            //dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);

            //dbHelper.ExecuteCommand("SaveCuentaBanco");

            HelperDAL.CreateConnection(_strConnection, _DBName + "SaveCuentaBanco", ref _cnn, ref _cmd);
            _cmd.Parameters.Add(HelperDAL.CreateParameter("@prmXML", entidad, SqlDbType.Xml));
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

        public bool SaveCuentaBancos(List<CuentaBanco> lst, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;

            //DBHelper dbHelper = new DBHelper(_strConnection);

            //SqlParameter prmData = new SqlParameter();
            //prmData.ParameterName = "@prmXML";
            //prmData.SqlDbType = SqlDbType.Xml;
            //prmData.Value = dbHelper.SerializeToXML(lst);
            //dbHelper.AddParameter(prmData);

            //dbHelper.CreateParameter<string>("@Entidad", "ArrayOfCuentaBanco/CuentaBanco");


            //dbHelper.ExecuteCommand("SaveCuentaBanco");

            //--
            //SqlConnection cnn = new SqlConnection(_strConnection);
            //var cmd = new SqlCommand("SaveCuentaBanco", cnn);
            //cmd.CommandType = CommandType.StoredProcedure;
            HelperDAL.CreateConnection(_strConnection, _DBName + "SaveCuentaBanco", ref _cnn, ref _cmd);

            //var prmData = HelperDAL.CreateParameter("@prmXML", lst, SqlDbType.Xml);
            //cmd.Parameters.Add(prmData);
            //cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Entidad", SqlDbType = SqlDbType.VarChar, Size = 250, Value = "ArrayOfCuentaBanco/CuentaBanco" });
            //SqlParameter prmMsg = new SqlParameter();
            //prmMsg.ParameterName = "@Msg";
            //prmMsg.Direction = ParameterDirection.Output;
            //prmMsg.SqlDbType = SqlDbType.VarChar;
            //prmMsg.Size = 8000;

            //cmd.Parameters.Add(prmMsg);

            _cmd.Parameters.Add(HelperDAL.CreateParameter("@prmXML", lst, SqlDbType.Xml));
            _cmd.Parameters.Add(HelperDAL.CreateParameter("@Entidad","ArrayOfCuentaBanco/CuentaBanco" ));
            _cmd.Parameters.Add(HelperDAL.CreateParameterMsg());

            //_cnn.Open();
            //_cmd.ExecuteNonQuery();
            //_cnn.Close();

            HelperDAL.ExecuteNonQuery(ref _cnn, ref _cmd);

            Msg = _cmd.Parameters[2].Value.ToString();
            //--

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
