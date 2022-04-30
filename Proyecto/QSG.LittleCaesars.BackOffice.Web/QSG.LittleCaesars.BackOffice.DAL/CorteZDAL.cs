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

namespace QSG.LittleCaesars.BackOffice.DAL
{
    public class CorteZDAL
    {
        private string _strConnection = string.Empty;

        public CorteZDAL()
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
        }

        public CorteZDAL(string strConnection)
        {
            _strConnection = strConnection;
        }

        
        public CorteZ GetCorteZ(CorteZ corteZ, int usRqst, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new CorteZ();

            DBHelper dbHelper = new DBHelper(_strConnection);
            dbHelper.CreateParameter<int>("@TicketID", corteZ.TicketID);
            dbHelper.CreateParameter<int>("@SucursalID", corteZ.Sucursal.SucursalID);
            dbHelper.CreateParameter<int>("@CajaID", corteZ.CajaID);
            dbHelper.CreateParameter<string>("@FechaVta",corteZ.FechaVta.ToString("yyyy-MM-dd"));
            dbHelper.CreateParameter<int>("@usRqst", usRqst);
            DataSet ds = dbHelper.ExecuteDataset("getCorteZ");

            friendlyMessage = msg;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DateTime dt;
                DataRow row = ds.Tables[0].Rows[0];
                result.TicketID = Convert.ToInt32(row["TicketID"]);
                result.CajaID = Convert.ToInt32(row["CajaID"]);
                DateTime.TryParse(row["FechaVta"].ToString(), out dt);
                result.FechaVta = dt;

                result.Hora = row["Hora"].ToString();
                result.Transacciones = Convert.ToInt32(row["Transacciones"]);
                result.Cajeros = row["Cajeros"].ToString();

                result.Efectivo = Convert.ToDouble(row["Efectivo"]);
                result.TCredito = Convert.ToDouble(row["TCredito"]);
                result.TDebito = row["TDebito"] != DBNull.Value ? Convert.ToDouble(row["TDebito"]) : 0;
                result.OtraFormaPago = Convert.ToDouble(row["OtraFormaPago"]);
                result.Total = Convert.ToDouble(row["Total"]);


                result.CodUsAlta = row["CodUsuario"].ToString();
                result.CodUsAltaNombre = row["NomUsuario"].ToString();

            }
            
            // Sucursal
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[1].Rows != null && ds.Tables[1].Rows.Count > 0)
            {
                var suc = new Sucursal();
                DataRow row = ds.Tables[1].Rows[0];

                suc.SucursalID = Convert.ToInt32(row["SucursalID"]);
                suc.Nombre = row["Nombre"].ToString();
                suc.Abr = row["Abr"].ToString();
                suc.Descripcion = row["Descripcion"].ToString();

                suc.Serie = row["Serie"].ToString();
                suc.Calle = row["Calle"].ToString();
                suc.NoInt = row["NoInt"].ToString();
                suc.NoExt = row["NoExt"].ToString();
                suc.Colonia = row["Colonia"].ToString();
                suc.Delegacion = row["Delegacion"].ToString();
                suc.Ciudad = row["Ciudad"].ToString();
                suc.Municipio = row["Municipio"].ToString();
                suc.Estado = row["Estado"].ToString();
                suc.CP = row["CP"].ToString();
              
                result.Sucursal = suc;
            }

            return result;
        }

        public List<CorteZ> GetCorteZs(CorteZFilter corteZ, int usRqst, ref string friendlyMessage)
        {
            var result = new List<CorteZ>();
            string msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(corteZ);
            dbHelper.AddParameter(prmData);
            
            dbHelper.CreateParameter<string>("@Msg", msg, System.Data.ParameterDirection.Output);
            dbHelper.CreateParameter<int>("@usRqst", usRqst);

            DataSet ds = dbHelper.ExecuteDataset("getCorteZs");

            friendlyMessage = msg;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DateTime dt;
                CorteZ cz;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    cz = new CorteZ();
                    cz.TicketID = Convert.ToInt32(row["TicketID"]);
                    cz.CajaID = Convert.ToInt32(row["CajaID"]);
                    DateTime.TryParse(row["FechaVta"].ToString(), out dt);
                    cz.FechaVta = dt;

                    cz.Hora = row["Hora"].ToString();
                    cz.Transacciones = Convert.ToInt32(row["Transacciones"]);
                    cz.Cajeros = row["Cajeros"].ToString();

                    cz.Efectivo = row["Efectivo"] != DBNull.Value ? Convert.ToDouble(row["Efectivo"]) : 0;
                    cz.TCredito = row["TCredito"] != DBNull.Value ? Convert.ToDouble(row["TCredito"]) : 0;
                    cz.TDebito = row["TDebito"] != DBNull.Value ? Convert.ToDouble(row["TDebito"]) : 0;
                    cz.OtraFormaPago = Convert.ToDouble(row["OtraFormaPago"]);
                    cz.Total = Convert.ToDouble(row["Total"]);


                    cz.CodUsAlta = row["CodUsuario"].ToString();
                    cz.CodUsAltaNombre = row["NomUsuario"].ToString();


                    cz.Sucursal = new Sucursal();
                    cz.Sucursal.SucursalID = (int)row["SucursalID"];
                    cz.Sucursal.Nombre = row["Nombre"].ToString();
                    cz.Sucursal.Abr = row["Abr"].ToString();
                    cz.Sucursal.Empresa = new EmpresaCliente();
                    cz.Sucursal.Empresa.EmpresaID = row.Field<int>("EmpresaID");
                    cz.Sucursal.Empresa.Nombre = row["Empresa"].ToString();


                    result.Add(cz);
                }

            }


            return result;
        }

        public bool SaveCorteZ(CorteZ corteZ, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;
            DBHelper dbHelper = new DBHelper(_strConnection);
            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(corteZ);
            dbHelper.AddParameter(prmData);
            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            dbHelper.ExecuteCommand("SaveCorteZ");

            result = true;
            if (Msg != String.Empty)
            {
                friendlyMessage = Msg;
                result = false;
            }

            return result;
        }

        public bool SaveCorteZs(List<CorteZ> corteZ, ref string friendlyMessage)
        {
            bool result = false; 
            string Msg = string.Empty;
            DBHelper dbHelper = new DBHelper(_strConnection);
            
            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(corteZ);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Entidad","ArrayOfTicket/CorteZ");
            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            dbHelper.ExecuteCommand("SaveCorteZ");

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
