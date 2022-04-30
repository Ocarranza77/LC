using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using XPESD.Tools.DBHelper;
using QSG.QSystem.Common.Constants;

namespace QSG.LittleCaesars.BackOffice.DAL
{
    public class MenuDAL
    {
        private string _strConnection = string.Empty;

        public MenuDAL()
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
        }

        public MenuDAL(string strConnection)
        {
            _strConnection = strConnection;
        }

        public List<MenuP> GetMenuUser(MenuP menu, ref string FriendlyMessage)
        {
            string msg = string.Empty;
            var result = new MenuP();
            List<MenuP> lstMenu = new List<MenuP>();
            DBHelper dbHelper = new DBHelper(_strConnection);
            dbHelper.CreateParameter<string>("@CodUsuario", menu.CodUser.PadLeft(5,'0'));
            DataSet ds = dbHelper.ExecuteDataset("getMenuUser");




            if (ds != null)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    result = new MenuP();
                    result.CodUser = row["CodUser"].ToString();
                    result.CodAp = (int)row["CodAp"];
                    result.NomAp = row["NomAp"].ToString();
                    result.CodP = (int)row["CodP"];
                    result.NomP = row["NomP"].ToString();
                    result.DescripcionP = row["Descripcion"].ToString();
                    lstMenu.Add(result);
                }


            }

            

            return lstMenu;
        }



    }
}
