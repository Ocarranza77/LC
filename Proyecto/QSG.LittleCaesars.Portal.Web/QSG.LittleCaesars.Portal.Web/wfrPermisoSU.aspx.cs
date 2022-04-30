using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.Messages;
using QSG.LittleCaesars.BackOffice.Messages.Requests;
using QSG.LittleCaesars.BackOffice.Messages.Response;
using QSG.QSystem.Common.Enums;

namespace QSG.LittleCaesars.Portal.Web
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        public static CultureInfo culture = new CultureInfo("es-MX", true);
        public static ServiceImplementation _services;
        public static string _usuarioID;
        public static List<Sucursal> lstSuc;
        public static List<Usuario> lstUsers;
        public static List<SucursalUsuario> lstUserIN;

        public static string NombrePantalla;
        public static string Titulo;

        public static string r_logo;
        public static string _user;
        public static string DBName;

        protected void Page_Load(object sender, EventArgs e)
        {
             Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha = DateTime.Today;

            //Thread.CurrentThread.CurrentCulture = culture;
            //DateTime fecha2 = DateTime.Today;

            //   txtFecha.Value = fecha2.ToShortDateString();

            r_logo = "Images/little.png";
            if (Session["User"] != null)
            {
                if (!IsPostBack)
                {
                    NombrePantalla = "Permiso a Sucursales";
                    Titulo = Session["Empresa"].ToString() + " | " + NombrePantalla;
                    _user = Session["Nombre"].ToString();
                    _usuarioID = Session["User"].ToString();
                   // txtUserID.Value = _usuarioID;
                    ///txtUsuario.Value = _user;
                    DBName = Session["DBName"].ToString();
                    GetSucursales();
                    GetUsers();
                  

                    Cargar();
                   // CargaInicial();
                    // GetSuc();
                }
            }
            else
            {
                Session["User"] = null;
                Response.Redirect("~/Account/Login.aspx", true);
            }
        }

       
        protected void Cargar()
        {
            var html = "";

            _services = new ServiceImplementation();

            SucursalUsuarioResponse _su = _services.SucursalUsuarioMessage(new SucursalUsuarioRequest()
            {
                BDName=DBName,
                UsuarioID=0,
                SucursalID=0,
                UserIDRqst=Convert.ToInt32( _usuarioID),
                MessageOperationType=QSystem.Common.Enums.MessageOperationType.Report
                

            });

            var lstPermisos = _su.SucursalesUsuario;

            List<SucursalUsuario> lstUsuarios =lstPermisos
                .GroupBy(l => l.UsuarioPermisoID)
                .Select(f => f.First())
                .ToList();




           /* List<string> lstNombres = lstPermisos
                .GroupBy(l => l.UsuarioPermisoID)
                .Select(f => f.First().Nombre).ToList();

            */


            var htmlSuc = "";
            html += " <li>";
            html += " <ul>";
            html += "<li class='column_ID'>ID</li>";

            html += "<li class='column_Usuario'><img class='_new' onclick='AddRow(event);' />Nombre Usuario</li>";
            html += "<li class='column_Check'></li>";
            html += "<li class='column_sucursales'>Sucursales";
            html += "<ul>";

            foreach (Sucursal su in lstSuc)
            {
                html += "<li class='column_SU'>" + su.SucursalID+"</BR>"+su.Nombre + "</li>";

            }

            html += "</ul>";

            html += "</li>";
            html += "<li class='column_sttReg'></li>";
            html += "</ul>";
            html += "</li>";


            var _clase = "_unchecked";
            var _existe = "_noexiste";
            var _all="_unchecked";
            var _countSU = lstSuc.Count;
            var _countUser = 0;
            
            foreach (SucursalUsuario u in lstUsuarios)
            {
                var SuP = lstPermisos.Where(x => x.UsuarioPermisoID == u.UsuarioPermisoID).ToList();

                if (SuP.Count == lstSuc.Count) { _all = "_checked"; }

                    foreach (Sucursal su in lstSuc)
                    {
                        for (var x = 0; x < SuP.Count; x++)
                        {
                            if (SuP[x].Sucursal.SucursalID.ToString() == su.SucursalID.ToString()) {
                                _clase = "_checked";
                                _existe = "_existe";
                                _countUser++;
                                SuP.RemoveAt(x);
                                break;
                            }

                        }

                        htmlSuc += "<li class='column_SU'><img class='" + _clase + "'  onclick='check(event);'/><input class='"+_existe+"'  value='" + su.SucursalID.ToString() + "'  /></li>";
                        _clase = "_unchecked";
                        _existe = "_noexiste";

                    }

              

                html += "   <li class='row'>";
                html += "   <ul>";
                html += "   <li class='column_codUser'><input value='" + u.UsuarioPermisoID.ToString() + "' /></li>";
                html += "   <li class='column_eject'><input /></li>";
                html += "   <li class='column_ID'></li>";
                html += "   <li class='column_Usuario'  > <input value='" + u.Nombre + "' title='Sucursales: " + _countUser + "/" + _countSU + "' class='inactive' /> </li>";
                html += "   <li class='column_Check'><img class='"+_all+"' onclick='check(event);' /> </li>";
                html += htmlSuc;
                html += "<li class='column_sttReg'><img /></li>";
                html += "  </ul></li>";

                htmlSuc = "";
                _all = "_unchecked";
              
                _countUser = 0;
            }



            container_history.InnerHtml = html;

           

        }
        [WebMethod]
        public static string UpPermisoSu(String[]registro)
        {

            Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha = DateTime.Today;
           
            List<SucursalUsuario> lstSup = new List<SucursalUsuario>();

            var _index = 0;
            var _index1=0;
            var TypeMsg = 0;
            var msg = string.Empty;

            

             var _typeOperation="";
            
            var _usrID=0;
            var _suID = 0;


            try
            {
                if (registro.Length > 0)
                {
                    for (var x = 0; x < registro.Length; x++)
                    {
                        var _dato = registro[x].Split(new char[] { '|' });

                        _typeOperation = _dato[0] != "" ? _dato[0].ToString().Trim() : "";
                        _usrID = _dato[1] != "" ? Convert.ToInt32(_dato[1]) : 0;
                        _suID = _dato[2] != "" ? Convert.ToInt32(_dato[2]) : 0;
                        _index = _dato[3] != "" ? Convert.ToInt32(_dato[3]) : 0;
                        SucursalUsuario _usr = new SucursalUsuario();


                        _usr = new SucursalUsuario();
                        _usr.UsuarioPermisoID = _usrID;
                        _usr.Sucursal = new Sucursal() { SucursalID = _suID };
                        _usr.CodUsAlta = _usuarioID;
                        _usr.FechaAlta = fecha;
                        switch (_typeOperation)
                        {
                            case "RowNew":
                                _usr.OperationType = OperationType.New;
                                break;
                            case "RowDelete":
                                _usr.OperationType = OperationType.Delete;
                                break;

                        }

                        lstSup.Add(_usr);


                    }

                    SucursalUsuarioResponse _SU = _services.SucursalUsuarioMessage(new SucursalUsuarioRequest()
                    {
                        BDName = DBName,
                        SucursalesUsuario = lstSup,
                        UserIDRqst = Convert.ToInt32(_usuarioID),
                        MessageOperationType = MessageOperationType.Save

                    });
                    msg += _SU.ResultType.ToString();
                }
               
            }
            catch (Exception ex)
            {
                TypeMsg = 1;
                msg += ex.Message;

            }


            return TypeMsg + "|" + _index + "|" + msg;
        }
        [WebMethod]
        public static string GetS() {
            var html = "";
            foreach (Sucursal s in lstSuc) {

                html += "<li class='column_SU'><img class='_unchecked'  onclick='check(event);'/><input class='_noexiste' value='" + s.SucursalID.ToString()+ "'  /></li>";
            
            }
            return html;
        }
        [WebMethod]
        public static List<Usuario> GetU()
        {
            return lstUsers;
        }
        private void GetSucursales()
        {
            lstSuc = new List<Sucursal>();
            var su = new Sucursal();
            var sv1 = new ServiceImplementation();
            var sr = new SucursalRequest();
            su.SucursalID = 0;
            sr.Sucursal = su;
            sr.UserIDRqst = Convert.ToInt32(_usuarioID);
            sr.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Report; //BackOffice.Common.Enums.MessageOperationType.Report;
            var response = sv1.SucursalMessage(sr);
            lstSuc = response.Sucursales;
            //return response.Sucursales;
        }
        public void GetUsers() {

            lstUsers = new List<Usuario>();

            var u = new Usuario();
     
            var sv = new ServiceImplementation();
            var ur = new UsuarioRequest();
            u.CodUsuario = 0;
            ur.Usuario = u;
            ur.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Report; //BackOffice.Common.Enums.MessageOperationType.Report;
            var  response = sv.UsuarioMessage(ur);
         

            lstUsers = response.Usuarios;
           // return response.Usuarios;
        }

        protected void Reporte_Click(object sender, EventArgs e)
        {
            Cargar();
        }

    }
}