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
    public class DepositoDAL
    {
        private string _strConnection = string.Empty;
        private string _DBName;
        private SqlConnection _cnn;
        private SqlCommand _cmd;


        public DepositoDAL(string dbName)
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
            _DBName = dbName + GeneralApp.schema;
        }

        public DepositoDAL(string dbName, string strConnection)
        {
            _strConnection = strConnection;
            _DBName = dbName + GeneralApp.schema;
        } 

        public List<CorteSucursal> GetDepositos(DateTime fecha, ref string friendlyMessage)
        {
            var result = new List<CorteSucursal>();
            string msg = string.Empty;
            Dictionary<int, int> idx = new Dictionary<int, int>();
            int numTable = 1;

            HelperDAL.CreateConnection(_strConnection, _DBName + "GetRepDepositos", ref _cnn, ref _cmd);
            _cmd.Parameters.Add(HelperDAL.CreateParameter("@Fecha", fecha.ToString("yyyyMMdd")));
            _cmd.Parameters.Add(HelperDAL.CreateParameterMsg());
            DataSet ds = HelperDAL.ExecuteDataSet(_cmd);

            friendlyMessage = _cmd.Parameters[1].Value.ToString();


            new CorteSucursalDAL(_DBName.Split(new Char [] {'.'})[0]).FillCorteSucursales(result, ds, idx);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[numTable].Rows != null && ds.Tables[numTable].Rows.Count > 0)
            {
                DateTime dt;
                CorteSucursalDeposito dep;
                foreach (DataRow row in ds.Tables[numTable].Rows)
                {
                    dep = new CorteSucursalDeposito();
                    dep.DepositoID = Convert.ToInt32(row["DepositosID"]);
                    dep.Consecutivo = Convert.ToInt32(row["Consecutivo"]);
                    dep.FolioDeposito = row["FolioDeposito"].ToString();
                    DateTime.TryParse(row["FechaDeposito"].ToString(), out dt);
                    dep.FechaDeposito = dt;
                    dep.Importe = row["Importe"] != DBNull.Value ? Convert.ToDouble(row["Importe"]) : 0;
                    dep.Nota = row["Nota"].ToString();
            
                    DateTime.TryParse(row["FechaAlta"].ToString(), out dt);
                    dep.FechaAlta = dt;
                    dep.CodUsAlta = row["UsuarioID"].ToString();

                    if(row["CtaBcoID"] != DBNull.Value)
                    {
                        dep.CuentaBanco = new CuentaBanco();
                        dep.CuentaBanco.CtaBcoID = Convert.ToInt32(row["CtaBcoID"]);
                        dep.CuentaBanco.Descripcion = row["CBDescripcion"].ToString();
                        dep.CuentaBanco.NoCta = row["CBNoCta"].ToString();
                        dep.CuentaBanco.Titular = row["CBTitular"].ToString();

                        if (row["BcoNombre"] != DBNull.Value)
                        {
                            dep.CuentaBanco.Banco = new CatalogoTipo();
                            dep.CuentaBanco.Banco.Nombre = row["BcoNombre"].ToString();
                            dep.CuentaBanco.Banco.Abr = row["BcoAbr"].ToString();
                            dep.CuentaBanco.Banco.ID = Convert.ToInt32(row["BcoID"]);
                        }

                        if (row["MonedaID"] != DBNull.Value)
                        {
                            dep.CuentaBanco.Moneda = new Moneda();
                            dep.CuentaBanco.Moneda.MonedaID = Convert.ToInt32(row["MonedaID"]);
                            dep.CuentaBanco.Moneda.Nombre = row["MonedaNombre"].ToString();
                            dep.CuentaBanco.Moneda.Abr = row["MonedaAbr"].ToString();
                        }
                    }

                    var cs = result[idx[Convert.ToInt32(row["SucursalID"])]];

                    if (cs.Depositos == null)
                        cs.Depositos = new List<CorteSucursalDeposito>();

                    cs.TotalDepositosD += dep.CuentaBanco.Moneda.MonedaID == 2 ? dep.Importe : 0;
                    cs.TotalDepositosP += dep.CuentaBanco.Moneda.MonedaID == 1 ? dep.Importe : 0;
                   // cs.TotalPorDepositarP

                   /* cs.TotalDeudorEnPesos = (cs.PesosADeposito + (cs.DolarADeposito * cs.TC))  
                        - (cs.TotalDepositosP + (cs.TotalDepositosD * cs.TC))
                        - (cs.DeudorPesos + (cs.DeudorDolar * cs.TC));*/

                    //result[idx[Convert.ToInt32(row["SucursalID"])]].Depositos.Add(dep);
                    cs.Depositos.Add(dep);
                }
            }

            return result;
        }

        public bool SaveDepositos(List<CorteSucursal> lst, ref string friendlyMessage)
        {
            bool result = false; 
            string Msg = string.Empty;
            
            HelperDAL.CreateConnection(_strConnection, _DBName + "SaveDepositos", ref _cnn, ref _cmd);
            _cmd.Parameters.Add(HelperDAL.CreateParameter("@prmXML", lst, SqlDbType.Xml));
            _cmd.Parameters.Add(HelperDAL.CreateParameterMsg());
            HelperDAL.ExecuteNonQuery(ref _cnn, ref _cmd);

            Msg = _cmd.Parameters[1].Value.ToString();



            result = true;
            if (Msg != String.Empty)
            {
                friendlyMessage = Msg; 
                result = false;
            }

            return result;
        }

        public Dictionary<string, List<CboTipo>> IniciarCbos()
        {
            
            var result = new Dictionary<string, List<CboTipo>>();
            string msg = string.Empty;
            int numTable = 0;
            
            CommonsDAL cDal = new CommonsDAL();

            HelperDAL.CreateConnection(_strConnection, _DBName + "getDepositosIni", ref _cnn, ref _cmd);
            DataSet ds = HelperDAL.ExecuteDataSet(_cmd);

            // Sucursal
            //if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[numTable].Rows != null && ds.Tables[numTable].Rows.Count > 0)
            result.Add("Sucursal", cDal.CboFill(ds.Tables[numTable]));
            numTable++;

            // Banco
            result.Add("Banco", cDal.CboFill(ds.Tables[numTable]));
            numTable++;

            // CuentaBanco
            result.Add("CuentaBanco", cDal.CboFill(ds.Tables[numTable]));
            numTable++;

            // Moneda
            result.Add("Moneda", cDal.CboFill(ds.Tables[numTable]));
            numTable++;

            return result;
        }
        
    }


}
