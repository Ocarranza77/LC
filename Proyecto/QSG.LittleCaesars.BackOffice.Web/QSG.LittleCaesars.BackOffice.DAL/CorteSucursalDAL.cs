using QSG.LittleCaesars.BackOffice.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Constants;
using System.Configuration;
using XPESD.Tools.DBHelper;
using System.Data;
using System.Data.SqlClient;
using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Entities;
using QSG.QSystem.DAL;

namespace QSG.LittleCaesars.BackOffice.DAL
{
    public class CorteSucursalDAL
    {
        private string _strConnection = string.Empty;
        private string _DBName;

        public CorteSucursalDAL(string dbName)
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
            _DBName = dbName + GeneralApp.schema;
        }

        public CorteSucursalDAL(string dbName, string strConnection)
        {
            _strConnection = strConnection;
            _DBName = dbName + GeneralApp.schema;
        }

        

        public List<CorteSucursal> GetCorteSucursales(CorteSucursalFilter corteSucursal, int usRqst, ref string friendlyMessage)
        {
            var result = new List<CorteSucursal>();
            string msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(corteSucursal);
            dbHelper.AddParameter(prmData);
            
            dbHelper.CreateParameter<string>("@Msg", msg, System.Data.ParameterDirection.Output);
            dbHelper.CreateParameter<int>("@usRqst", usRqst);

            DataSet ds = dbHelper.ExecuteDataset(_DBName + "getCorteSucursales");

            friendlyMessage = msg;

            FillCorteSucursales(result, ds, null);


            return result;
        }

        public void FillCorteSucursales(List<CorteSucursal> result, DataSet ds, Dictionary<int, int> dic)
        {
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DateTime dt;
                CorteSucursal cs;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    cs = new CorteSucursal();
                    DateTime.TryParse(row["FechaVta"].ToString(), out dt);
                    cs.FechaVta = dt;

                    cs.EfectivoZ = Convert.ToDouble(row["EfectivoZ"]);
                    cs.NoZ = Convert.ToInt32(row["NoZ"]);
                    cs.Total = Convert.ToDouble(row["Total"]);
                    cs.TotalOtraFormaPago = row["TotalOtraFormaPago"] != DBNull.Value ? Convert.ToDouble(row["TotalOtraFormaPago"]) : 0;
                    cs.TotalTCredito = row["TotalTCredito"] != DBNull.Value ? Convert.ToDouble(row["TotalTCredito"]) : 0;
                    cs.TotalTDebito = row["TotalTDebito"] != DBNull.Value ? Convert.ToDouble(row["TotalTDebito"]) : 0;

                    cs.PesosADeposito = row["PesosADeposito"] != DBNull.Value ? Convert.ToDouble(row["PesosADeposito"]) : 0;
                    cs.DolarADeposito = row["DolarADeposito"] != DBNull.Value ? Convert.ToDouble(row["DolarADeposito"]) : 0;
                    cs.TC = row["TC"] != DBNull.Value ? Convert.ToDouble(row["TC"]) : 0;
                    cs.PesosSB = row["PesosSB"] != DBNull.Value ? Convert.ToDouble(row["PesosSB"]) : 0;
                    cs.DolarSB = row["DolarSB"] != DBNull.Value ? Convert.ToDouble(row["DolarSB"]) : 0;
                    cs.Gastos = row["Gastos"] != DBNull.Value ? Convert.ToDouble(row["Gastos"]) : 0;
                    if (row["Ajuste"] != DBNull.Value)
                    {
                        if (Convert.ToDouble(row["Ajuste"]) < 0)
                            cs.Faltante = Convert.ToDouble(row["Ajuste"]) * -1;
                        else
                            cs.Sobrante = Convert.ToDouble(row["Ajuste"]);
                    }
                    //cs.Ajuste = row["Ajuste"] != DBNull.Value ? Convert.ToDouble(row["Ajuste"]) : 0;
                    cs.FolioFactura = row["FolioFactura"].ToString();
                    cs.Comentarios = row["Comentarios"].ToString();
                    cs.CodUsAlta = row["CodUsuario"].ToString();
                    cs.CodUsAltaNombre = row["NomUsuario"].ToString();

                    cs.CajeroCorto = row["CajeroCorto"].ToString();
                    cs.Supervisor = row["Supervisor"].ToString();

                    cs.Stt = row["Stt"] != DBNull.Value ? row["Stt"].ToString() : "";
                    cs.DeudorNombre = row["DeudorNombre"].ToString();
                    cs.DeudorPesos = row["DeudorPesos"] != DBNull.Value ? Convert.ToDouble(row["DeudorPesos"]) : 0;
                    cs.DeudorDolar = row["DeudorDolar"] != DBNull.Value ? Convert.ToDouble(row["DeudorDolar"]) : 0;

                    cs.Sucursal = new Sucursal();
                    cs.Sucursal.SucursalID = (int)row["SucursalID"];
                    cs.Sucursal.Nombre = row["Nombre"].ToString();
                    cs.Sucursal.Abr = row["Abr"].ToString();

                    cs.Sucursal.Empresa = new EmpresaCliente();
                    cs.Sucursal.Empresa.EmpresaID = (int)row["EmpresaID"];
                    cs.Sucursal.Empresa.Nombre = row["EmpresaNombre"].ToString();


                    result.Add(cs);
                    
                    if (dic != null)
                        dic.Add(cs.Sucursal.SucursalID, result.Count - 1);


                }

            }
        }

        public List<CboTipo> GetCorteSucursalStt(DateTime fechaVta, List<int> sucursales)
        {
            var result = new List<CboTipo>();
            string msg = string.Empty;
            int numTable = 0;
            CommonsDAL cDal = new CommonsDAL();


            SqlConnection cnn = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand(_DBName + "GetCorteSucursalStt", cnn);
            cmd.CommandType = CommandType.StoredProcedure;


            SqlParameter prm = new SqlParameter();
            prm.ParameterName = "@FechaVta";
            prm.Direction = ParameterDirection.Input;
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Value = fechaVta.ToString("yyyyMMdd");
            prm.Size = 250;

            cmd.Parameters.Add(prm);

            SqlParameter prmSuc = new SqlParameter();
            prmSuc.ParameterName = "@SucursalID";
            prmSuc.Direction = ParameterDirection.Input;
            prmSuc.SqlDbType = SqlDbType.VarChar;
            prmSuc.Size = 250;
            foreach (int i in sucursales)
                msg += i.ToString() + ',';

            prmSuc.Value = msg.Substring(0, msg.Length - 1);

            cmd.Parameters.Add(prmSuc);

            SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet("Result");

            sqlAdapter.Fill(ds);

            result = cDal.CboFill(ds.Tables[numTable]);

            return result;
        }

        public bool SaveCorteSucursal(CorteSucursal corteSucursal, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;
            DBHelper dbHelper = new DBHelper(_strConnection);
            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(corteSucursal);
            dbHelper.AddParameter(prmData);
            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            dbHelper.ExecuteCommand(_DBName + "SaveCorteSucursal");

            result = true;
            if (Msg != String.Empty)
            {
                friendlyMessage = Msg;
                result = false;
            }

            return result;
        }

        public bool SaveCorteSucursales(List<CorteSucursal> corteSucursal, ref string friendlyMessage)
        {
            bool result = false; 
            string Msg = string.Empty;
            DBHelper dbHelper = new DBHelper(_strConnection);
            
            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(corteSucursal);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Entidad", "ArrayOfCorteSucursal/CorteSucursal");
            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            dbHelper.ExecuteCommand(_DBName + "SaveCorteSucursal");

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
