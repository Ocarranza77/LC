using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.Services;

namespace QSG.LittleCaesars.Portal.Web
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();


            Session["User"] = null;
            Session["TipoUser"] = 0;
            Session["Nombre"] = null;
            Session["Empresa"] = null;
            Session.Clear();
            Session.RemoveAll();
            //Response.Redirect("~/Default.aspx");
            Response.Redirect("~/Account/Login.aspx", true);
          //  Response.Redirect("~/Portal.aspx", true);
           // Server.Transfer("~/Portal.aspx");
          //  base.OnLoad(e);
        }
    }
}