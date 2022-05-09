using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.QSystem.Common.Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPESD.Tools.DBHelper;

namespace QSG.LittleCaesars.BackOffice.DAL
{
    public class ClienteDAL
    {
        private string _strConnection = string.Empty;

        public ClienteDAL()
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
        }

        public ClienteDAL(string strConnection)
        {
            _strConnection = strConnection;
        }

        public Cliente GetCliente(Cliente cliente, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new Cliente();

            DBHelper dbHelper = new DBHelper(_strConnection);
            dbHelper.CreateParameter<string>("@RFC", cliente.RFC);
            DataSet ds = dbHelper.ExecuteDataset("getCliente");

            //Cliente
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                result.RFC = row["RFC"].ToString();
                result.Calle = row["Calle"].ToString();
                result.NoInt = row["NoInt"].ToString();
                result.NoExt = row["NoExt"].ToString();
                result.Colonia = row["Colonia"].ToString();
                result.Municipio = row["Municipio"].ToString();
                result.Ciudad = row["Ciudad"].ToString();
                result.Contacto = row["Contacto"].ToString();
                result.CP = row["CP"].ToString();
                result.Delegacion = row["Delegacion"].ToString();
                result.Email1 = row["Email1"].ToString();
                result.Email2 = row["Email2"].ToString();
                result.Email3 = row["Email3"].ToString();
                result.Estado = row["Estado"].ToString();
                result.RazonSocial = row["RazonSocial"].ToString();
                result.RegimenFiscal = row["RegimenFiscal"].ToString();

            }

            return result;
        }

        public bool SaveCliente(Cliente cliente, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);
            
            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(cliente);
            dbHelper.AddParameter(prmData);
            
            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            
            dbHelper.ExecuteCommand("SaveCliente");

            result = true;
            if (Msg != String.Empty)
            {
                friendlyMessage = Msg;
                result = false;
            }

            return result;
        }

        public bool SaveClientes(List<Cliente> clientes, ref string friendlyMessage)
        {
            bool result = false; 
            string Msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);
            
            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(clientes);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Entidad", "ArrayOfCliente/Cliente");
            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            
            dbHelper.ExecuteCommand("SaveCliente");

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
