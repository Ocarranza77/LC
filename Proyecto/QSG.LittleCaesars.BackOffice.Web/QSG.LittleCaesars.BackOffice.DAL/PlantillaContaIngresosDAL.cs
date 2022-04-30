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
    public class PlantillaContaIngresosDAL
    {
        private string _strConnection = string.Empty;
        private string _DBName;
        private SqlConnection _cnn;
        private SqlCommand _cmd;


        public PlantillaContaIngresosDAL(string dbName)
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
            _DBName = dbName + GeneralApp.schema;
        }

        public PlantillaContaIngresosDAL(string dbName, string strConnection)
        {
            _strConnection = strConnection;
            _DBName = dbName + GeneralApp.schema;
        }


        public PlantillaPolizaIngreso GetPlantillaContaIngresos(int sucursalID, ref string friendlyMessage)
        {
            var result = new PlantillaPolizaIngreso();
            string msg = string.Empty;
            int numTable = 0;

            var asientos = new PlantillaPolizaIngresoAsiento();
            var ctasBco = new PlantillaPolizaIngresoCtaBco();


            HelperDAL.CreateConnection(_strConnection, _DBName + "getPlantillaContable", ref _cnn, ref _cmd);
            _cmd.Parameters.Add(HelperDAL.CreateParameter("@SucursalID", sucursalID, SqlDbType.Int));
            _cmd.Parameters.Add(HelperDAL.CreateParameterMsg());
            DataSet ds = HelperDAL.ExecuteDataSet(_cmd);

            friendlyMessage = _cmd.Parameters[1].Value.ToString();


            //new CorteSucursalDAL(_DBName.Split(new Char[] { '.' })[0]).FillCorteSucursales(result, ds, idx);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[numTable].Rows != null && ds.Tables[numTable].Rows.Count > 0)
            {
                DataRow row = ds.Tables[numTable].Rows[0];

                result.Sucursal = new Sucursal() { SucursalID = row.Field<int>("SucursalID"), Nombre = row["NomSucursal"].ToString() };
                result.Nombre = row["Nombre"].ToString();
                result.Comentario = row["Comentario"].ToString();
                result.FechaAlta = row.Field<DateTime?>("FechaAlta");
                result.FechauM = row.Field<DateTime?>("FechaUM");
                result.CodUsAlta = row["UsuarioID"].ToString();
                result.CodUsUM = row["UsuarioIDUM"].ToString();
                result.CodUsAltaNombre = row["UsuarioAlta"].ToString();
                result.CodUsUMNombre = row["UsuarioMod"].ToString();

                result.Asientos = new List<PlantillaPolizaIngresoAsiento>();
                result.CtasBancarias = new List<PlantillaPolizaIngresoCtaBco>();
            }

            numTable++;
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[numTable].Rows != null && ds.Tables[numTable].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[numTable].Rows)
                {
                    asientos = new PlantillaPolizaIngresoAsiento();

                    asientos.Asiento = new AsientosPolizaIngreso();
                    asientos.Asiento.AsientoCod = row["AsientoCod"].ToString();
                    asientos.Asiento.MovimientoTipoCargo = row.Field<bool>("MovimientoTipoCargo");
                    asientos.Asiento.CampoRelacionado = row["CampoRelacionado"].ToString();
                    asientos.Asiento.AsientoCod = row["AsientoCod"].ToString();
                    asientos.Asiento.Grupo = row["Grupo"].ToString();
                    asientos.Asiento.MovimientoTipo = row["MovimientoTipo"].ToString();
                    asientos.Asiento.Moneda = new Moneda() { MonedaID = row.Field<int>("MonedaId"), Nombre = row.Field<int>("MonedaId") == 1 ? "Pesos" : "Dolar" };
                    asientos.Asiento.Descripcion = row["Descripcion"].ToString();
                    asientos.Asiento.Referencia = row["Referencia"].ToString();

                    asientos.CtaContable = row["CtaLiga"].ToString();
                    asientos.CtaComplementearia = row["CtaComLiga"].ToString();
                    asientos.NomCta = row["NomCta"].ToString();
                    asientos.NomCtaComp = row["NomCtaComp"].ToString();

                    asientos.CodUsAlta = row["UsuarioID"].ToString();
                    asientos.CodUsUM = row["UsuarioIDUM"].ToString();
                    asientos.CodUsAltaNombre = row["UsuarioAlta"].ToString();
                    asientos.CodUsUMNombre = row["UsuarioMod"].ToString();


                    result.Asientos.Add(asientos);
                }
            }

            numTable++; // Ctas Bancarias ligadas
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[numTable].Rows != null && ds.Tables[numTable].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[numTable].Rows)
                {
                    ctasBco = new PlantillaPolizaIngresoCtaBco();

                    ctasBco.CtaBanco = new CuentaBanco();
                    ctasBco.CtaBanco.CtaBcoID = row.Field<int>("CtaBcoID");
                    ctasBco.CtaBanco.NoCta = row["NoCta"].ToString();
                    ctasBco.CtaBanco.Moneda = new Moneda() { MonedaID = row.Field<int>("MonedaId"), Nombre = row.Field<int>("MonedaId") == 1 ? "Pesos" : "Dolar" };
                    ctasBco.CtaBanco.Banco = new CatalogoTipo() { Nombre = row["BancoNombre"].ToString() };

                    ctasBco.CtaContable = row["CtaLiga"].ToString();
                    ctasBco.CtaComplementearia = row["CtaComLiga"].ToString();
                    ctasBco.NomCta = row["NomCta"].ToString();
                    ctasBco.NomCtaComp = row["NomCtaComp"].ToString();

                    ctasBco.CodUsAlta = row["UsuarioID"].ToString();
                    ctasBco.CodUsUM = row["UsuarioIDUM"].ToString();
                    ctasBco.CodUsAltaNombre = row["UsuarioAlta"].ToString();
                    ctasBco.CodUsUMNombre = row["UsuarioMod"].ToString();

                    ctasBco.MovimientoTipoCargo = false;
                    ctasBco.Grupo = "Bancos";
                    ctasBco.MovimientoTipo = "Depositos";
                    ctasBco.Descripcion = "Deposito Efectivo";
                    ctasBco.Referencia = "Suc+Folio deposito";

                    result.CtasBancarias.Add(ctasBco);
                }
            }

            numTable++; // Cta Banco no ligas.
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[numTable].Rows != null && ds.Tables[numTable].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[numTable].Rows)
                {
                    ctasBco = new PlantillaPolizaIngresoCtaBco();

                    ctasBco.CtaBanco = new CuentaBanco();
                    ctasBco.CtaBanco.CtaBcoID = row.Field<int>("CtaBcoID");
                    ctasBco.CtaBanco.NoCta = row["NoCta"].ToString();
                    ctasBco.CtaBanco.Moneda = new Moneda() { MonedaID = row.Field<int>("MonedaId"), Nombre = row.Field<int>("MonedaId") == 1 ? "Pesos" : "Dolar" };
                    ctasBco.CtaBanco.Banco = new CatalogoTipo() { Nombre = row["BancoNombre"].ToString() };

                    ctasBco.MovimientoTipoCargo = false;
                    ctasBco.Grupo = "Bancos";
                    ctasBco.MovimientoTipo = "Depositos";
                    ctasBco.Descripcion = "Deposito Efectivo";
                    ctasBco.Referencia = "Suc+Folio deposito";

                    result.CtasBancarias.Add(ctasBco);
                }
            }

            return result;
        }

        public List<PlantillaPolizaIngreso> GetPlantillasContaIngresos(ref string friendlyMessage)
        {
            var result = new List<PlantillaPolizaIngreso>();
            var ppi = new PlantillaPolizaIngreso();
            string msg = string.Empty;
            int numTable = 0;


            HelperDAL.CreateConnection(_strConnection, _DBName + "getPlantillasContables", ref _cnn, ref _cmd);
            _cmd.Parameters.Add(HelperDAL.CreateParameterMsg());
            DataSet ds = HelperDAL.ExecuteDataSet(_cmd);

            friendlyMessage = _cmd.Parameters[1].Value.ToString();

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[numTable].Rows != null && ds.Tables[numTable].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[numTable].Rows)
                {
                    ppi = new PlantillaPolizaIngreso();

                    ppi.Sucursal = new Sucursal() { SucursalID = row.Field<int>("SucursalID"), Nombre = row["NomSucursal"].ToString() };
                    ppi.Nombre = row["Nombre"].ToString();
                    ppi.Comentario = row["Comentario"].ToString();
                    ppi.FechaAlta = row.Field<DateTime?>("FechaAlta");
                    ppi.FechauM = row.Field<DateTime?>("FechaUM");
                    ppi.CodUsAlta = row["UsuarioID"].ToString();
                    ppi.CodUsUM = row["UsuarioIDUM"].ToString();
                    ppi.CodUsAltaNombre = row["UsuarioAlta"].ToString();
                    ppi.CodUsUMNombre = row["UsuarioMod"].ToString();

                    result.Add(ppi);
                }
            }
            return result;
        }

        public bool SavePlantillaPolizaContable(PlantillaPolizaIngreso lst, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;

            HelperDAL.CreateConnection(_strConnection, _DBName + "SavePlantillaPolizaIngresos", ref _cnn, ref _cmd);
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

        //public Dictionary<string, List<CboTipo>> IniciarCbos()
        //{

        //    var result = new Dictionary<string, List<CboTipo>>();
        //    string msg = string.Empty;
        //    int numTable = 0;

        //    CommonsDAL cDal = new CommonsDAL();

        //    HelperDAL.CreateConnection(_strConnection, _DBName + "getDepositosIni", ref _cnn, ref _cmd);
        //    DataSet ds = HelperDAL.ExecuteDataSet(_cmd);

        //    // Sucursal
        //    //if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[numTable].Rows != null && ds.Tables[numTable].Rows.Count > 0)
        //    result.Add("Sucursal", cDal.CboFill(ds.Tables[numTable]));
        //    numTable++;

        //    // Banco
        //    result.Add("Banco", cDal.CboFill(ds.Tables[numTable]));
        //    numTable++;

        //    // CuentaBanco
        //    result.Add("CuentaBanco", cDal.CboFill(ds.Tables[numTable]));
        //    numTable++;

        //    // Moneda
        //    result.Add("Moneda", cDal.CboFill(ds.Tables[numTable]));
        //    numTable++;

        //    return result;
        //}

    }


}
