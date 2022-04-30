using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.QSystem.Common.Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPESD.Tools.DBHelper;

namespace QSG.LittleCaesars.BackOffice.DAL
{
    public class FacturarConDAL
    {
        private string _strConnection = string.Empty;

        public FacturarConDAL()
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
        }

        public FacturarConDAL(string strConnection)
        {
            _strConnection = strConnection;
        }


        public FacturarCon GetDatos(int sucursalID, ref string friendlyMessage)
        {
            var result = new FacturarCon();
            string msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);
            dbHelper.CreateParameter<int>("@SucID", sucursalID);

            //dbHelper.CreateParameter<string>("@Msg", msg, System.Data.ParameterDirection.Output);

            DataSet ds = dbHelper.ExecuteDataset("GetFolioFactura");

            friendlyMessage = msg;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                //result.RFC = (string)row["RFC"].ToString();
                result.Serie = (string)row["Serie"].ToString();
                result.Folio = Convert.ToInt32(row["Folio"]);
                
                var empCliDAL = new EmpresaClienteDAL("");
                var empCli = new EmpresaCliente();

                empCliDAL.FillEmpresaCliente(ref empCli, row);
                result.Empresa = empCli;
            }


            return result;
        }

    }
}
