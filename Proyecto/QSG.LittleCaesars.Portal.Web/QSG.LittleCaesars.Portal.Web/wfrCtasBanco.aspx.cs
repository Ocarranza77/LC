using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.LittleCaesars.BackOffice.Messages;
using QSG.LittleCaesars.BackOffice.Messages.Requests;
using QSG.LittleCaesars.BackOffice.Messages.Response;
using QSG.QSystem.Common.Entities;

namespace QSG.LittleCaesars.Portal.Web
{
    public partial class wfrCtasBanco : System.Web.UI.Page
    {
        public static CultureInfo culture = new CultureInfo("es-MX", true);
        public static string _usuarioID;
        public static List<Sucursal> lstSuc;
    
        public static string NombrePantalla;
        public static string Titulo;

        public static string r_logo;
        public static string _user;
        public static string DBName;


        public static List<CboTipo> lstBancos;
        public static ServiceImplementation services;

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
                    NombrePantalla = "Cuentas Bancarias";
                    Titulo = Session["Empresa"].ToString() + " | " + NombrePantalla;
                    _user = Session["Nombre"].ToString();
                    _usuarioID = Session["User"].ToString();
                    txtUserID.Value = _usuarioID;
                    txtUsuario.Value = _user;
                    DBName = Session["DBName"].ToString();
                    CargaInicial();
                   // GetSuc();
                }
            }
            else
            {
                Session["User"] = null;
                Response.Redirect("~/Account/Login.aspx", true);
            }
        }
        private void CargaInicial()
        {

            DataTable dt = Conexion.LeerTabla("select *from Qbic.dbo.Bancos");

            lstBancos = new List<CboTipo>();
            foreach (DataRow row in dt.Rows) {
                CboTipo cbo = new CboTipo();
                cbo.ID = row["BancoID"].ToString();
                cbo.Nombre = row["Nombre"] != DBNull.Value ? row["Nombre"].ToString() : "";
                lstBancos.Add(cbo);
                
            }

        }
        [WebMethod]
        public static string GetBancos()
        {
            var html = "";
            foreach (CboTipo bc in lstBancos)
            {
                html += "<option value='" + bc.ID + "'>" + bc.Nombre + "</option>";

            }
            return html;
        }

        [WebMethod]
        public static string Guardar(String[]Cuenta, string dbName){

            services = new ServiceImplementation();
            List<CuentaBanco> lstCta = new List<CuentaBanco>();
            var _ctaDAL = new CuentaBancoDAL(dbName);


             Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha = DateTime.Today;

            var TypeMsg = 0;
            var msg = "Operacion Finalizada ";
            var _index = 0;
            



            try
            {
                var _OperationType = Cuenta[0];
                var _CtaId = Cuenta[1] != "" ? Convert.ToInt32(Cuenta[1]) : 0;
                var _empresa = Cuenta[2];
                var _bancoID = Cuenta[3] != "" ? Convert.ToInt32(Cuenta[3]) : 0;
                var _NoCta = Cuenta[4];
                var _monedaID = Cuenta[5] != "" ? Convert.ToInt32(Cuenta[5]) : 0;
                var _titular = Cuenta[6];
                var _desc = Cuenta[7];
                var _nota = Cuenta[8];

                _index = Cuenta[9] != "" ? Convert.ToInt32(Cuenta[9]) : 0;

                var _usuarioReqst = Convert.ToInt32(_usuarioID);


                CuentaBanco _cta = new CuentaBanco();
                _cta.CtaBcoID = _CtaId;
                _cta.Empresa = _empresa;
                _cta.Banco = new CatalogoTipo() { ID = _bancoID };
                _cta.NoCta = _NoCta;
                _cta.Moneda = new Moneda() { MonedaID = _monedaID };
                _cta.Titular = _titular;
                _cta.Descripcion = _desc;
                _cta.Notas = _nota;
                

                switch (_OperationType)
                {
                    case "RowNew":
                        _cta.FechaAlta = DateTime.Parse(fecha.ToShortDateString(), culture);
                        _cta.FechauM = DateTime.Parse(fecha.ToShortDateString(), culture);
                        _cta.OperationType = QSystem.Common.Enums.OperationType.New;
                        break;
                    case "RowEdit":
                        _cta.FechauM = DateTime.Parse(fecha.ToShortDateString(),culture);
                        _cta.OperationType = QSystem.Common.Enums.OperationType.Edit;
                        break;
                    case "RowDelete":
                        _cta.OperationType = QSystem.Common.Enums.OperationType.Delete;
                        break;
                }

                lstCta.Add(_cta);

                if (_bancoID > 0 && _monedaID > 0)
                {
                    CuentaBancoResponse _ctaResp = services.CuentaBancoMessage(new CuentaBancoRequest()
                    {
                        BDName = DBName,
                        CuentaBancos=lstCta,
                        UserIDRqst = _usuarioReqst,
                        MessageOperationType = QSystem.Common.Enums.MessageOperationType.Save
                    });

                    msg += _ctaResp.ResultType.ToString();

                   // var _ctaResp = _ctaDAL.SaveCuentaBancos(lstCta, ref msg);

                   
                }
                else
                {
                    TypeMsg = 1;
                    msg += "\n" + "Por Favor verifique Banco o tipo de Moneda.";
                }
               
                //result = TypeMsg + "|" + msg;

            }
            catch (Exception ex)
            {
                TypeMsg = 1;
                msg += "\n" + ex.Message;

            }

            return TypeMsg + "|" + _index + "|" + msg;
        }

        protected void Reporte_Click(object sender, EventArgs e)
        {
            var html = "";
            var select = "";

            services = new ServiceImplementation();
            CuentaBancoResponse _ctaResp = services.CuentaBancoMessage(new CuentaBancoRequest()
           {
               BDName = DBName,
               CuentaBanco = new CuentaBanco() { },
               UserIDRqst = Convert.ToInt32(_usuarioID),
               MessageOperationType = QSystem.Common.Enums.MessageOperationType.Report
           });




            html += " <li >";
            html += "<ul >";
            html += "<li class='column_ID'>No.</li>";
            html += "<li class='column_Edit'><img /></li>";
            html += "<li class='column_Del'><img /></li>";
            html += "<li class='column_Empresa'> Empresa</li>";
            html += "<li class='column_Banco'> Banco</li>";
            html += "<li class='column_Cuenta' >Cuenta</li>";
            html += "<li class='column_Moneda'>Moneda</li>";
            html += "<li class='column_Titular'>Titular</li>";
            html += "<li class='column_Descripcion'>Descripcion</li>";
            html += "<li class='column_Nota'>Notas</li>";
            html += "<li class='column_sttReg' ><img  /></li>";
            html += "</ul> ";
            html += "</li>";



            foreach (CuentaBanco cta in _ctaResp.CuentaBancos)
            {

                select = "";

                html += "<li class='row' >";
                html += "<ul >";
                html += "<li class='column_STTTemp'><input value='row' /></li> ";
                html += "<li class='column_CtaID'><input value='" + cta.CtaBcoID.ToString() + "' /></li> ";
                html += "<li class='column_ID'></li>";
                html += "<li class='column_Edit column_fillColor'><img class='_unchecked'  onclick='Editar(event);' /></li>";
                html += "<li class='column_Del column_fillColor'><img class='_unchecked'  onclick='Eliminar(event);' /></li>";
                html += "<li class='column_Empresa'> <input value='" + cta.Empresa + "' class='inactive' readonly/> </li>";

                html += "<li class='column_Banco'><select class='inactive' disabled>";

                foreach (CboTipo b in lstBancos)
                {
                    if (b.ID == cta.Banco.ID.ToString()) { select = "selected"; }
                    html += "<option value='" + b.ID + "' " + select + ">" + b.Nombre + "</option>";
                    select = "";
                }
                html += "</select></li>";


                html += "<li class='column_Cuenta' ><input  onkeypress='return justNumbers(event);'  maxlength='11' value='" + cta.NoCta + "' class='inactive' readonly/></li>";
                html += "<li class='column_Moneda'><select class='inactive' disabled>";

                select = "";
                switch (cta.Moneda.MonedaID)
                {
                    case 1:
                        html += "<option value='1' selected>Pesos</option>";
                        html += "<option value='2' >Dolares</option>";
                        break;
                    case 2:
                        html += "<option value='1' >Pesos</option>";
                        html += "<option value='2' selected>Dolares</option>";
                        break;
                }

                html += " </select></li>";
                html += "<li class='column_Titular'><input value='" + cta.Titular + "' class='inactive' readonly/></li>";
                html += "<li class='column_Descripcion'><input value='" + cta.Descripcion + "' class='inactive' readonly/></li>";
                html += "<li class='column_Nota'><input value='" + cta.Notas + "' class='inactive' readonly/></li>";
                html += "<li class='column_sttReg' ><img  /></li>";
                html += "</ul> ";
                html += "</li>";
            }


            container_history.InnerHtml = html.ToString();



        }
    }
}