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
    public class EmpresaClienteDAL
    {
        private string _strConnection = string.Empty;
        private string _DBName;
        private SqlConnection _cnn;
        private SqlCommand _cmd;

        public EmpresaClienteDAL(string dbName)
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
            _DBName = dbName + GeneralApp.schema;
        }

        public EmpresaClienteDAL(string dbName, string strConnection)
        {
            _DBName = dbName + GeneralApp.schema;
            _strConnection = strConnection;
        }


        public EmpresaCliente GetEmpresa(int empresaID, ref string friendlyMessage)
        {
            var result = new EmpresaCliente();
            string msg = string.Empty;

            //DBHelper dbHelper = new DBHelper(_strConnection);
            //DataSet ds = dbHelper.ExecuteDataset("getEmpresaCliente");
            HelperDAL.CreateConnection(_strConnection, _DBName + "getEmpresaCliente", ref _cnn, ref _cmd);
            _cmd.Parameters.Add(HelperDAL.CreateParameter("@EmpresaID", empresaID, SqlDbType.Int));
            _cmd.Parameters.Add(HelperDAL.CreateParameterMsg());
            DataSet ds = HelperDAL.ExecuteDataSet(_cmd);

            friendlyMessage = msg;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {

                DataRow row = ds.Tables[0].Rows[0];


                FillEmpresaCliente(ref result, row);

            }


            return result;
        }

        public List<EmpresaCliente> GetEmpresas(ref string friendlyMessage)
        {
            var result = new List<EmpresaCliente>();
            string msg = string.Empty;
            EmpresaCliente emp;

            HelperDAL.CreateConnection(_strConnection, _DBName + "getEmpresasCliente", ref _cnn, ref _cmd);
            _cmd.Parameters.Add(HelperDAL.CreateParameterMsg());
            DataSet ds = HelperDAL.ExecuteDataSet(_cmd);

            friendlyMessage = msg;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    emp = new EmpresaCliente();

                    FillEmpresaCliente(ref emp, row);
                    result.Add(emp);
                }

            }


            return result;
        }

        public void FillEmpresaCliente(ref EmpresaCliente result, DataRow row)
        {
            result.Nombre = (string)row["Nombre"].ToString().ToUpper();
            result.RFC = (string)row["RFC"].ToString().ToUpper();
            result.DB = (string)row["DB"];

            result.Calle = row["Calle"].ToString();
            result.NoInt = row["NoInt"].ToString();
            result.NoExt = row["NoExt"].ToString();
            result.Colonia = row["Colonia"].ToString();

            result.Municipio = row["Municipio"].ToString();
            result.Estado = row["Estado"].ToString();
            result.Ciudad = row["Ciudad"].ToString();
            result.CP = row["CP"].ToString();
            result.Delegacion = row["Delegacion"].ToString();

            result.EmpresaContpaqID = row["EmpresaContpaqID"] != DBNull.Value ? row.Field<int>("EmpresaContpaqID") : 0;
            result.EmpresaContpaqName = row["EmpresaContpaqName"].ToString();
            result.EmpresaContpaqRutaBD = row["EmpresaContpaqRutaBD"].ToString();
            result.EmpresaContpaqMascarilla = row["EmpresaContpaqMascarilla"].ToString();

            result.Serao = row["Serao"].ToString();
            result.PassKey = row["PassKey"].ToString();
            result.UserPak = row["UserPak"].ToString();
            result.ClavePak = row["ClavePak"].ToString();
            result.CertificadoCer = row["CertificadoCer"].ToString();
            result.CertificadoKey = row["CertificadoKey"].ToString();
        }

        public bool SaveEmpresa(EmpresaCliente empresa, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(empresa);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);

            dbHelper.ExecuteCommand("SaveEmpresaCliente");

            result = true;
            if (Msg != String.Empty)
            {
                friendlyMessage = Msg;
                result = false;
            }

            return result;
        }

        /* Contable */

        public List<EmpresaCliente> GetEmpresasContpaq(ref string friendlyMessage)
        {
            var result = new List<EmpresaCliente>();
            string msg = string.Empty;
            var emp = new EmpresaCliente();


            HelperDAL.CreateConnection(_strConnection, _DBName + "getEmpresasContpaq", ref _cnn, ref _cmd);
            _cmd.Parameters.Add(HelperDAL.CreateParameterMsg());
            DataSet ds = HelperDAL.ExecuteDataSet(_cmd);


            friendlyMessage = msg;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    emp = new EmpresaCliente();

                    emp.EmpresaContpaqID = row["EmpresaContpaqID"] != DBNull.Value ? row.Field<int>("EmpresaContpaqID") : 0;
                    emp.EmpresaContpaqName = row["EmpresaContpaq"].ToString();
                    emp.EmpresaContpaqRutaBD = row["EmpresaContpaqRutaBD"].ToString();
                    emp.EmpresaContpaqMascarilla = row["EmpresaContpaqMascarilla"].ToString();

                    result.Add(emp);
                }
            }


            return result;
        }

        public List<CatalogoContable> GetCuentasContpaq(string BaseDatos, bool SoloCatalogo, ref List<CatalogoContable> CuentasNoEncontradas, ref string Mascarilla, ref string friendlyMessage)
        {
            var result = new List<CatalogoContable>();
            string msg = string.Empty;
            var cat = new CatalogoContable();
            int numTable = 0;

            CuentasNoEncontradas = new List<CatalogoContable>();
            Mascarilla = "";

            HelperDAL.CreateConnection(_strConnection, _DBName + "getCuentasContpaq", ref _cnn, ref _cmd);
            _cmd.Parameters.Add(HelperDAL.CreateParameter("@BDD", BaseDatos));
            _cmd.Parameters.Add(HelperDAL.CreateParameter("@SoloCat", SoloCatalogo, SqlDbType.Bit));
            _cmd.Parameters.Add(HelperDAL.CreateParameterMsg());
            DataSet ds = HelperDAL.ExecuteDataSet(_cmd);


            friendlyMessage = _cmd.Parameters[2].Value.ToString();

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[numTable].Rows != null && ds.Tables[numTable].Rows.Count > 0)
            {

                foreach (DataRow row in ds.Tables[numTable].Rows)
                {
                    cat = new CatalogoContable();

                    cat.Cuenta = row["Cta"].ToString();
                    cat.Nombre = row["Nombre"].ToString();
                    cat.CtaMayor = row["CtaMayor"] != DBNull.Value ? row.Field<int>("CtaMayor") : 0;
                    cat.Afectable = row["Afectable"] != DBNull.Value ? row.Field<bool>("Afectable") : false;
                    if (!SoloCatalogo)
                        cat.EnUso = row["EnUso"] != DBNull.Value ? row.Field<bool>("EnUso") : false;

                    result.Add(cat);
                }
            }

            if (SoloCatalogo)
                return result;

            numTable++;
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[numTable].Rows != null && ds.Tables[numTable].Rows.Count > 0)
            {

                foreach (DataRow row in ds.Tables[numTable].Rows)
                {
                    cat = new CatalogoContable();

                    cat.Cuenta = row["CtaCont"].ToString();
                    cat.Nombre = row["Nombre"].ToString();

                    CuentasNoEncontradas.Add(cat);
                }
            }


            numTable++;
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[numTable].Rows != null && ds.Tables[numTable].Rows.Count > 0)
            {
                DataRow row = ds.Tables[numTable].Rows[0];
                Mascarilla = row["Mascarilla"].ToString();
            }

            return result;

        }

        public List<PlantillaPolizaIngreso> GetCuentasContpaqUsadasEn(string ctaCont, ref string friendlyMessage)
        {
            var result = new List<PlantillaPolizaIngreso>();
            string msg = string.Empty;
            var pol = new PlantillaPolizaIngreso();
            int numTable = 0;

            HelperDAL.CreateConnection(_strConnection, _DBName + "GetCtaContUsadaEn", ref _cnn, ref _cmd);
            _cmd.Parameters.Add(HelperDAL.CreateParameter("@@CtaCont", ctaCont));
            _cmd.Parameters.Add(HelperDAL.CreateParameterMsg());
            DataSet ds = HelperDAL.ExecuteDataSet(_cmd);


            friendlyMessage = _cmd.Parameters[1].Value.ToString();

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[numTable].Rows != null && ds.Tables[numTable].Rows.Count > 0)
            {

                foreach (DataRow row in ds.Tables[numTable].Rows)
                {
                    pol = new PlantillaPolizaIngreso();

                    pol.Sucursal = new Sucursal() { SucursalID = row.Field<int>("SucursalID"), Nombre = row["SucursalNombre"].ToString() };
                    pol.Nombre = row["NombrePlantilla"].ToString();
                    pol.Comentario = "Usado en: " + row["Tipo"].ToString() + row["NombreTipo"].ToString();
                    result.Add(pol);
                }
            }

            return result;

        }

        public bool CrearLinkContpaq(string servidor, string user, string pwd)
        {
            var result = true;

            HelperDAL.CreateConnection(_strConnection, _DBName + "CreatLinkContpaq", ref _cnn, ref _cmd);
            _cmd.Parameters.Add(HelperDAL.CreateParameter("@Servidor", servidor));
            _cmd.Parameters.Add(HelperDAL.CreateParameter("@DBUser", user));
            _cmd.Parameters.Add(HelperDAL.CreateParameter("@Pwd", pwd));


            try
            {
                HelperDAL.ExecuteNonQuery(ref _cnn, ref _cmd);
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

    }
}
