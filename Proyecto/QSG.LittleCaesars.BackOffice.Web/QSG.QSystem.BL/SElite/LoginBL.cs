using QSG.QSystem.Common.Entities.SElite;
using QSG.QSystem.DAL.SElite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.QSystem.BL.SElite
{
    public class LoginBL
    {
        private string _DBName;

        public LoginBL(string dbName)
        {
            this._DBName = dbName;
        }

        public List<Usuario> Authentication(string email, string pwd, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<Usuario>();
            var dal = new LoginDAL(this._DBName);

            result = dal.Authentication(email, pwd, ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;
        }

        public List<App> Authorize(int usID, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<App>();
            var dal = new LoginDAL(this._DBName);

            result = dal.Authorize(usID, ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;
        }
    }
}
