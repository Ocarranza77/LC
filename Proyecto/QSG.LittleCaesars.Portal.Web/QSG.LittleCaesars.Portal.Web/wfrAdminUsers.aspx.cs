using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QSG.LittleCaesars.Portal.Web
{
    public partial class wfrAdminUsers : System.Web.UI.Page
    {
        public static CultureInfo culture = new CultureInfo("es-MX", true);
        public static string _usuarioID;
        //public static List<Sucursal> lstSuc;

        public static string NombrePantalla;
        public static string Titulo;

        public static string r_logo;
        public static string _user;
        public static string DBName;
        protected void Page_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha2 = DateTime.Today;

            //   txtFecha.Value = fecha2.ToShortDateString();

            r_logo = "Images/little.png";
            if (Session["User"] != null)
            {
                if (!IsPostBack)
                {
                    NombrePantalla = "Administracion de Usuarios";
                    Titulo = Session["Empresa"].ToString() + " | " + NombrePantalla;
                    _user = Session["Nombre"].ToString();
                    _usuarioID = Session["User"].ToString();
                    txtUserID.Value = _usuarioID;
                    txtUsuario.Value = _user;
                    DBName = Session["DBName"].ToString();
                  //  CargaInicial();
                    // GetSuc();
                }
            }
            else
            {
                Session["User"] = null;
                Response.Redirect("~/Account/Login.aspx", true);
            }
        }
    }
}