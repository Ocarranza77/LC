using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QSG.LittleCaesars.BackOffice.Messages;
using QSG.LittleCaesars.BackOffice.Messages.Requests;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.BL;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;
using QSG.LittleCaesars.Portal.Web.Resources;






namespace QSG.LittleCaesars.Portal.Web.Account
{
    public partial class Login : Page
    {
      //  public CultureInfo culture = new CultureInfo("es-ES", true);
        protected void Page_Load(object sender, EventArgs e)
        {
            //Thread.CurrentThread.CurrentCulture = culture;
           // DateTime d = new DateTime();
            //d = Convert.ToDateTime("2015-03-06T12:29:41");
          //  string F1 = d.ToShortDateString();
           // string f = Convert.ToDateTime("2015-03-06T12:29:41").ToShortDateString();
            
            
            /*
            RegisterHyperLink.NavigateUrl = "Register.aspx";
            OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];

            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }*/

            //Usuario.InnerText = resTest.lblNombre;
        }
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            var tck = new Usuario();
            var SHA = new Hash();
            var msg = "";
            var Error = false;
            
            
            tck.CodUsuario = 0;



            if (!Regex.IsMatch(UserName.Value.Trim(), @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" + @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$"))
            {
                msg += "Usuario Incorrecto ";
                Error = true;
            }

            if (!Regex.IsMatch(Password.Value.Trim(), @"^(?=.{7,})(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$"))
            {
                if (msg != "") {
                    msg += " y ";
                }
                msg += " Contraseña Incorrecta.";
                Error = true;
            }



            if (!Error)
            {

                tck.Alias = UserName.Value.Trim();
                tck.Clave = SHA.SHA1(Password.Value.Trim());
                
                var sv = new ServiceImplementation();

                var tr = new UsuarioRequest();

                tr.Usuario = tck;
                tr.MessageOperationType = QSG.QSystem.Common.Enums.MessageOperationType.Query;  //BackOffice.Common.Enums.MessageOperationType.Query;
                //tr.MessageOperationType = QSG.LittleCaesars.BackOffice.Common.Enums.MessageOperationType_old.Query;

                var response = sv.UsuarioMessage(tr);

                if (response.FriendlyMessage.ToString() == "")
                {
                    if (response.Usuario != null)
                    {
                        Session["User"] = response.Usuario.CodUsuario;
                        Session["Email"] = UserName.Value.Trim();
                        Session["TipoUser"] = 0;
                        Session["Nombre"] = response.Usuario.Nombre;
                        Session["Empresa"] = "Little Caesars";
                        Session["DBName"] = "LittleCaesarDev";
                        Session["Puesto"] = response.Usuario.Puesto;
                        Response.Redirect("~/Portal.aspx", true);

                    }
                    else
                    {
                        Session["User"] = null;
                        msg = "No existe el perfil,  Por favor contacte a un Administrador";
                    }
                }
                else
                {
                    Session["User"] = null;
                    msg = response.FriendlyMessage.ToString();
                }
            }

            msgError.InnerText = msg;
        }



    }
}