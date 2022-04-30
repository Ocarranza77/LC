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

namespace QSG.LittleCaesars.BackOffice.DAL
{
    public class SucursalDAL
    {
        private string _strConnection = string.Empty;

        public SucursalDAL()
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
        }

        public SucursalDAL(string strConnection)
        {
            _strConnection = strConnection;
        }


        public List<Sucursal> GetSucursales(Sucursal sucursal, int usRqst,  ref string friendlyMessage)
        {
            var result = new List<Sucursal>();
            string msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);
            dbHelper.CreateParameter<int>("@SucursalID", sucursal.SucursalID);
            /*
            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(ticket);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Msg", msg, System.Data.ParameterDirection.Output);
            */
            dbHelper.CreateParameter<int>("@usRqst", usRqst);
            DataSet ds = dbHelper.ExecuteDataset("GetSucursales");

            friendlyMessage = msg;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
             
                Sucursal su;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    su=new Sucursal();
                    su.SucursalID = Convert.ToInt32(row["SucursalID"]);
                    su.Nombre = (string)row["Nombre"].ToString().ToUpper() ;
                    su.Abr =(string)row["Abr"].ToString().ToUpper();
                    su.Descripcion = (string)row["Descripcion"].ToString();

                    su.Calle =(string) row["Calle"].ToString();
                    su.NoInt =(string) row["NoInt"].ToString();
                    su.NoExt =(string) row["NoExt"].ToString();
                    su.Colonia =(string) row["Colonia"].ToString();
                    su.Municipio =(string) row["Municipio"].ToString();
                    su.Ciudad =(string) row["Ciudad"].ToString();
                    su.CP =(string)row["CP"].ToString();
                    su.Delegacion =(string) row["Delegacion"].ToString();

                    su.Iva=row.Field<double>("Iva");

                    //su.Direccion = (string)row["Direccion"];

                    su.Empresa = new EmpresaCliente();
                    su.Empresa.EmpresaID = row.Field<int>("EmpresaID");
                    su.Empresa.Nombre = row["Empresa"].ToString();

                    result.Add(su);
                }

            }


            return result;
        }

        public bool SaveSucursal(Sucursal sucursal, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(sucursal);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);

            dbHelper.ExecuteCommand("SaveSucursal");

            result = true;
            if (Msg != String.Empty)
            {
                friendlyMessage = Msg;
                result = false;
            }

            return result;
        }

        public bool SaveSucursales(List<Sucursal> sucursales, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(sucursales);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Entidad", "ArrayOfSucursal/Sucursal");
            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);

            dbHelper.ExecuteCommand("SaveSucursal");

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
