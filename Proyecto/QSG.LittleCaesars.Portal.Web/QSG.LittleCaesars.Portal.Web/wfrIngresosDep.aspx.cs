using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
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
using QSG.QSystem.Common.Enums;

namespace QSG.LittleCaesars.Portal.Web
{
    public partial class wfrIngresosDep : System.Web.UI.Page
    {
        public string NombrePantalla;
        public static CultureInfo culture = new CultureInfo("es-MX", true);
        public static string _usuarioID;
        public static List<Sucursal> lstSuc;
        public static List<CboTipo> lstMoneda;
        public static List<CboTipo> lstCtaBanco;
        public static List<CboTipo> lstBancos;
        public static List<CuentaBanco> lstCtas;
        public static string r_logo;
        public static string _user;
        public static string Titulo;
        public static string DBName;

        private DepositoResponse _DepResponse = new DepositoResponse();
        protected void Page_Load(object sender, EventArgs e)
        {
             Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha = DateTime.Today;



            if (Session["User"] != null)
            {

                if (!IsPostBack)
                {
                    _usuarioID = Session["User"].ToString();
                    NombrePantalla = "Control de Ingresos y Depositos Bancarios";
                    r_logo = "Images/little.png";
                    _user = Session["Nombre"].ToString();

                    Titulo = Session["Empresa"].ToString() + " | " + NombrePantalla;
                    DBName = Session["DBName"].ToString();
                    txtFecha.Value = fecha.ToShortDateString();
                    lstSuc = LlenarSucursales(_usuarioID);
                    CargaInicial();


                  

                }
            }
            else
            {
                Session["User"] = null;
                Response.Redirect("~/Account/Login.aspx", true);

            }

        }
        public List<Usuario> GetUsers()
        {
            var u = new Usuario();
            var sv = new ServiceImplementation();
            var ur = new UsuarioRequest();
            u.CodUsuario = 0;
            ur.Usuario = u;
            // ur.UserIDRqst = Convert.ToInt32(_usuarioID);
            ur.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Report; //BackOffice.Common.Enums.MessageOperationType.Report;
            var response = sv.UsuarioMessage(ur);
            return response.Usuarios;


        }
        private List<Sucursal> LlenarSucursales(string code)
        {
            var su = new Sucursal();
            var sv1 = new ServiceImplementation();
            var sr = new SucursalRequest();
            su.SucursalID = 0;
            sr.Sucursal = su;
            sr.UserIDRqst =1;// Convert.ToInt32(code);
            sr.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Report; //BackOffice.Common.Enums.MessageOperationType.Report;
            var response = sv1.SucursalMessage(sr);
            return response.Sucursales;
        }
        private void CargaInicial() {
            lstBancos = new List<CboTipo>();
            lstCtaBanco = new List<CboTipo>();
            lstMoneda = new List<CboTipo>();

            lstCtas = new List<CuentaBanco>();

            var Ser = new ServiceImplementation();
            _DepResponse = Ser.DepositoMessage(new DepositoRequest()
            {
                BDName=DBName,
                GetCbo=true

            });
            /*
            DataTable dtctas = Conexion.LeerTabla("select *from LittleCaesarDev.dbo.CuentaBanco");
         

            foreach (DataRow row in dtctas.Rows)
            {
                CuentaBanco cta = new CuentaBanco();
                cta.CtaBcoID = row["CtaBcoID"] != DBNull.Value ? Convert.ToInt32(row["CtaBcoID"]) : 0;
                cta.Banco = new CatalogoTipo() { ID = Convert.ToInt32(row["BancoID"]) };
                cta.Moneda = new Moneda() { MonedaID = Convert.ToInt32(row["MonedaID"]) };
                cta.NoCta = row["NoCta"].ToString();

                lstCtas.Add(cta);
            }

            */

            CuentaBancoResponse _ctaResp = Ser.CuentaBancoMessage(new CuentaBancoRequest() {
                BDName=DBName,
                CuentaBanco=new CuentaBanco(){},
                UserIDRqst=Convert.ToInt32( _usuarioID),
                MessageOperationType=QSystem.Common.Enums.MessageOperationType.Report
            });

            lstCtas = _ctaResp.CuentaBancos;
           

            lstBancos.Add(new CboTipo() { ID = "0", Nombre = " seleccionar " });
            lstCtaBanco.Add(new CboTipo() { ID = "0", Nombre = " seleccionar" });
            lstMoneda.Add(new CboTipo() { ID = "0", Nombre = " seleccionar" });


            lstBancos.AddRange(_DepResponse.CboInis["Banco"]);
            lstCtaBanco.AddRange( _DepResponse.CboInis["CuentaBanco"]);
            lstMoneda.AddRange(_DepResponse.CboInis["Moneda"]);
            


            /*
            lstBancos.Add(new CboTipo() { ID = "1", Nombre = "Bancomer S.A" });
            lstBancos.Add(new CboTipo() { ID = "2", Nombre = "Serfin S.A" });

            lstCtaBanco.Add(new CuentaBanco() { CtaBcoID = 1, Banco = new CatalogoTipo() { ID = 1 }, NoCta = "123456" });
            lstCtaBanco.Add(new CuentaBanco() { CtaBcoID = 2, Banco = new CatalogoTipo() { ID = 2 }, NoCta = "123" });

            lstMoneda.Add(new Moneda() { MonedaID = 1, Nombre = "Pesos" });
            lstMoneda.Add(new Moneda() { MonedaID = 2, Nombre = "Dolares" });*/

           
            /*
            cbxSucursales.Items.Clear();
            cbxSucursales.DataTextField = "Nombre";
            cbxSucursales.DataValueField = "sucursalID";
            cbxSucursales.DataSource = lstSuc; // GetUsers();
            cbxSucursales.DataBind();
            */
            SelectUsers.Items.Clear();
            SelectUsers.DataTextField = "Nombre";
            SelectUsers.DataValueField = "CodUsuario";
            SelectUsers.DataSource = GetUsers();
            SelectUsers.DataBind();
            SelectUsers.Items.Add(new ListItem("--- Todos ---", "0", true));
            SelectUsers.Items.FindByValue(Session["User"].ToString()).Selected = true;
        
        }

        private void GetCorteSucursales(string fecha,string _sucursalID) {


            double _TotDepAValP = 0;
            double _TotDepAValD = 0;

            double _TotImporte = 0;
            double _TotTCredito = 0;
            double _TotTDebito = 0;
            double _TotEfecPaDep = 0;
            double _TotEfecDaDep = 0;
            double _TotPesosSB = 0;
            double _TotDolaresSB = 0;
            double _TotFaltante = 0;
            double _TotalDepAValP = 0;
            double _TotalDepAValD = 0;
            double _TotDepP = 0;
            double _TotDepD = 0;
            double _TotSaldoP = 0;
            double _TotSaldoD = 0;
            double _TotDeudorP = 0;
            double _TotDeudorD = 0;


            StringBuilder html = new StringBuilder();
            StringBuilder htmlDep = new StringBuilder();
            var sv = new ServiceImplementation();
            /*
            var CrtSucFilter = new CorteSucursalFilter();
            //var CrtDep=new CorteSucursalDeposito();

            var sv = new ServiceImplementation();

            sv = new ServiceImplementation();

            CrtSucFilter.FechaVta = DateTime.Parse(fecha.ToString(), culture);
            CrtSucFilter.FechaVtaHasta = DateTime.Parse(fecha.ToString(), culture);
          
            CorteSucursalResponse CrtSucResp = sv.CorteSucursalMessage(new CorteSucursalRequest() { Filters = CrtSucFilter, UserIDRqst = Convert.ToInt32(_usuarioID), MessageOperationType = MessageOperationType.Report });
            */

            DepositoResponse DepResponse = sv.DepositoMessage(new DepositoRequest() { BDName=DBName, Fecha = DateTime.Parse(fecha.ToString(), culture), UserIDRqst=Convert.ToInt32(_usuarioID) ,MessageOperationType=MessageOperationType.Report });

                html.Append("<li><ul><li class='column_ID'>No.</li>");
                html.Append("<li class='column_Check'><img /></li>");
                html.Append("<li class='column_Sucursal'> Sucursal</li>");
                html.Append("<li class='column_IngDaily'>Daily's Ingresos<ul>");
                html.Append("<li class='column_Importe'>Gross</li>");
                html.Append("<li class='column_ImpTcredito'>T. Credito (P)</li>");
                html.Append("<li class='column_ImpTdebito'>T. Debito (P)</li>");
                html.Append("<li class='column_EfectivoDepP'>Efectivo (P) a Dep</li>");
                html.Append("<li class='column_EfectivoDepD'>Efectivo (D) a Dep</li>");
                html.Append("<li class='column_FolioServices'>Ser. Blindados (P)</li>");
                html.Append("<li class='column_FolioServicesD'>Ser. Blindados (D)</li>");
                html.Append("<li class='column_CajeroCorto'>cajero corto</li>");
                html.Append("<li class='column_Falt'>Faltante (P)</li>");
                html.Append("<li class='column_TotDepValPesos'>Tot. Dep a Validar (P)</li>");
                html.Append("<li class='column_TotDepValDolares'>Tot. Dep a Validar (D)</li>");
                html.Append("</ul> </li>");
                html.Append("<li class='column_espacio'></li>");
                html.Append("<li class='column_Depositos'>Depositos<ul>");
                html.Append("<li class='column_TotDepPesos'>Tot. Depositos (P)</li>");
                html.Append("<li class='column_TotDepDolares'>Tot. Depositos (D)</li>");
                html.Append("<li class='column_TipoCambio'>T.C.</li>");
                html.Append("<li class='column_SaldoDepPesos'>Saldos x Dep. (P)</li>");
                html.Append("<li class='column_SaldoDepDolares'>Saldos x Dep. (D)</li>");
                html.Append("<li class='column_Deudor'>&nbsp;Deudor Nombre</li>");
                html.Append("<li class='column_DeudorP'>Deudor Importe (P)</li>");
                html.Append("<li class='column_DeudorD'>Deudor Importe (D)</li>");
                html.Append("</ul></li>");               
                html.Append("</ul></li>");



                htmlDep.Append("<li><ul>");
                htmlDep.Append("<li class='column_ID'>No.</li>");
                htmlDep.Append("<li class='column_Check'><img /></li>");
                htmlDep.Append("<li class='column_Sucursal'>Sucursal</li>");
                htmlDep.Append("<li class='column_Depositos'>Depositos<ul>");
                htmlDep.Append("<li class='column_NoSec'>No Sec</li>");
                htmlDep.Append("<li class='column_Banco'>Banco</li>");
                htmlDep.Append("<li class='column_Moneda'>Moneda</li>");
                htmlDep.Append("<li class='column_Cuenta'>Cuenta</li>");
                htmlDep.Append("<li class='column_FolioDep'>Folio </li>");
                htmlDep.Append("<li class='column_FechaDep'>Fecha </li>");
                htmlDep.Append("<li class='column_Importe'>Importe</li>");
                htmlDep.Append("<li class='column_Nota'>Notas</li>");
                htmlDep.Append("</ul></li></ul></li>");


                foreach (CorteSucursal crt in DepResponse.CorteSucursales)
                {
                    var _stt = crt.Stt != null ? crt.Stt.Trim().ToUpper() : "";
                    var selec = "";
                    var disabled = "";


                    if (_stt != "" && _stt != "N" && _stt != "R" && _stt!="V") {
                        disabled = "disabled='disabled'";
                    }

                    _TotDepAValP = (crt.PesosADeposito + crt.PesosSB);
                    _TotDepAValD = (crt.DolarADeposito + crt.DolarSB);



                   

                    html.Append("<li class='row'><ul>");
                    html.Append("<li class='column_SuID'><input value='" + crt.Sucursal.SucursalID.ToString() + "'/></li>");
                    html.Append("<li class='column_fecha'><input value='" + crt.FechaVta.ToShortDateString() + "'/></li>");
                    html.Append("<li class='column_ID'></li>");
                    html.Append("<li class='column_Check'><input class='STT" + _stt + "' value='" + _stt + "'  onclick='ChangeSTT(event);' " + disabled + " readonly/></li>");
                    html.Append("<li class='column_Sucursal'>");
                    html.Append("<input class='inactive' value='" + crt.Sucursal.SucursalID.ToString() + "-" + crt.Sucursal.Nombre.ToString() + "' disabled='disabled'   readonly/>");
                        //<select disabled='disabled'>");

                   /* foreach (Sucursal s in lstSuc) // respSuc.Sucursales)
                    {
                        if (s.SucursalID == crt.Sucursal.SucursalID)
                        {
                            selec = "selected";
                        }
                        html.Append("<option " + selec + " value='" + s.SucursalID + "'>" + s.SucursalID.ToString() + "-" + s.Nombre + "</option>");
                        selec = "";
                    }
                    */

                    //html.Append("<li class='column_IngDaily'>Daily's Ingresos<ul>");
                   // html.Append("</select>
                       html.Append("</li><li class='column_Importe'><input class='inactive'  ondblclick='ADDIngreso(event);' value='" + crt.Total.ToString("C").Replace("$", "") + "'  "+disabled+"  readonly/></li>");
                    html.Append("<li class='column_ImpTcredito'><input class='inactive' ondblclick='ADDIngreso(event);' value='" + crt.TotalTCredito.ToString("C").Replace("$", "") + "' " + disabled + " readonly/></li>");
                    html.Append("<li class='column_ImpTdebito'><input class='inactive' ondblclick='ADDIngreso(event);' value='" + crt.TotalTDebito.ToString("C").Replace("$", "") + "' " + disabled + " readonly/></li>");
                    html.Append("<li class='column_EfectivoDepP'><input class='inactive' ondblclick='ADDIngreso(event);' value='" + crt.PesosADeposito.ToString("C").Replace("$", "") + "' " + disabled + " readonly/></li>");
                    html.Append("<li class='column_EfectivoDepD'><input class='inactive' ondblclick='ADDIngreso(event);' value='" + crt.DolarADeposito.ToString("C").Replace("$", "") + "' " + disabled + " readonly/></li>");
                    html.Append("<li class='column_FolioServices'><input class='inactive' ondblclick='ADDIngreso(event);' value='" + crt.PesosSB.ToString("C").Replace("$", "") + "' " + disabled + " readonly/></li>");
                    html.Append("<li class='column_FolioServicesD'><input class='inactive' ondblclick='ADDIngreso(event);' value='" + crt.DolarSB.ToString("C").Replace("$", "") + "' " + disabled + " readonly/></li>");
                    html.Append("<li class='column_CajeroCorto'><input class='inactive' ondblclick='ADDIngreso(event);' value='" + crt.CajeroCorto + "' " + disabled + " readonly/></li>");
                    html.Append("<li class='column_Falt'><input class='inactive' ondblclick='ADDIngreso(event);' value='" + crt.Faltante.ToString("C").Replace("$", "") + "' " + disabled + " readonly/></li>");
                    html.Append("<li class='column_TotDepValPesos'><input class='inactive' ondblclick='ADDIngreso(event);' value='" + _TotDepAValP.ToString("C").Replace("$", "") + "' " + disabled + " readonly/></li>");
                    html.Append("<li class='column_TotDepValDolares'><input class='inactive' ondblclick='ADDIngreso(event);' value='" + _TotDepAValD.ToString("C").Replace("$", "") + "' " + disabled + " readonly/></li>");
                    // html.Append("</ul> </li>");
                    html.Append("<li class='column_espacio'></li>");
                    // html.Append("<li class='column_Depositos'>Depositos<ul>");
                    html.Append("<li class='column_TotDepPesos'><input class='inactive' ondblclick='ADDIngreso(event);' value='" + crt.TotalDepositosP.ToString("C").Replace("$", "") + "' " + disabled + " readonly/></li>");
                    html.Append("<li class='column_TotDepDolares'><input class='inactive' ondblclick='ADDIngreso(event);' value='" + crt.TotalDepositosD.ToString("C").Replace("$", "") + "' " + disabled + " readonly/></li>");
                    html.Append("<li class='column_TipoCambio'><input class='inactive' value='" + crt.TC.ToString("C").Replace("$", "") + "' readonly/></li>");
                    html.Append("<li class='column_SaldoDepPesos'><input class='inactive' ondblclick='ADDIngreso(event);' value='" + crt.TotalPorDepositarP.ToString("C").Replace("$", "") + "' " + disabled + " readonly/></li>");
                    html.Append("<li class='column_SaldoDepDolares'><input class='inactive' ondblclick='ADDIngreso(event);' value='" + crt.TotalPorDepositarD.ToString("C").Replace("$", "") + "' " + disabled + " readonly/></li>");
                    html.Append("<li class='column_Deudor'><input class='inactive' ondblclick='ADDIngreso(event);' value='" + crt.DeudorNombre + "' readonly/></li>");
                    html.Append("<li class='column_DeudorP'><input class='inactive' ondblclick='ADDIngreso(event);' value='" + crt.DeudorPesos.ToString("C").Replace("$", "") + "' " + disabled + " readonly/></li>");
                    html.Append("<li class='column_DeudorD'><input class='inactive' ondblclick='ADDIngreso(event);' value='" + crt.DeudorDolar.ToString("C").Replace("$", "") + "' " + disabled + " readonly/></li>");
                    html.Append("<li class='column_sttReg'><img /></li>");
                    // html.Append("</ul></li>");
                    html.Append(" </ul></li>");

                    //sumatorias

                    _TotImporte += crt.Total;
                    _TotTCredito += crt.TotalTCredito;
                    _TotTDebito += crt.TotalTDebito;
                    _TotEfecPaDep += crt.PesosADeposito;
                    _TotEfecDaDep += crt.DolarADeposito;
                    _TotPesosSB += crt.PesosSB;
                    _TotDolaresSB += crt.DolarSB;
                    _TotFaltante += crt.Faltante;
                    _TotalDepAValP += _TotDepAValP;
                    _TotalDepAValD += _TotDepAValD;
                    _TotDepP += crt.TotalDepositosP;
                    _TotDepD += crt.TotalDepositosD;
                    _TotSaldoP += crt.TotalPorDepositarP;
                    _TotSaldoD += crt.TotalPorDepositarD;
                    _TotDeudorP += crt.DeudorPesos;
                    _TotDeudorD += crt.DeudorDolar;


                    if (crt.Depositos != null)
                    {
                        foreach (CorteSucursalDeposito dep in crt.Depositos)
                        {

                            htmlDep.Append("");

                            htmlDep.Append("<li class='row'>");
                            htmlDep.Append("<ul>");
                            htmlDep.Append("<li class='column_eject'><input /></li>");
                            htmlDep.Append("<li class='column_DepositoID'><input value='" + dep.DepositoID.ToString() + "'/></li>");
                            htmlDep.Append("<li class='column_fecha'><input value='" + crt.FechaVta.ToShortDateString() + "'/></li>");
                            htmlDep.Append("<li class='column_SuID'><input value='" + crt.Sucursal.SucursalID.ToString() + "'/></li>");
                            htmlDep.Append("<li class='column_BanID'><input value='" + dep.CuentaBanco.Banco.ID.ToString() + "'/></li>");
                            htmlDep.Append("<li class='column_CtaID'><input value='" + dep.CuentaBanco.CtaBcoID + "'/></li>");
                            htmlDep.Append("<li class='column_MonID'><input value='" + dep.CuentaBanco.Moneda.MonedaID.ToString() + "'/></li>");
                            htmlDep.Append("<li class='column_ID'>No.</li>");
                            htmlDep.Append("<li class='column_Check'><input class='STT" + _stt + "' value='" + _stt + "' disabled='disabled' readonly/></li>");
                            htmlDep.Append("<li class='column_Sucursal'>");

                            htmlDep.Append("<input value='" + crt.Sucursal.SucursalID.ToString() + "-" + crt.Sucursal.Nombre.ToString() + "' class='inactive' ondblclick='EditDep(event);' " + disabled + " readonly/>");
                            htmlDep.Append("</li>");
                            htmlDep.Append("<li class='column_NoSec'><input value='" + dep.Consecutivo.ToString() + "' class='inactive' disabled='disabled' " + disabled + " readonly/></li>");
                            htmlDep.Append("<li class='column_Banco'>");
                            htmlDep.Append("<input value='" + dep.CuentaBanco.Banco.Nombre + "' class='inactive' ondblclick='EditDep(event);' " + disabled + " readonly/>");
                            htmlDep.Append("</li>");
                            htmlDep.Append("<li class='column_Moneda'>");

                            htmlDep.Append("<input value='" + dep.CuentaBanco.Moneda.Nombre + "' class='inactive' ondblclick='EditDep(event);' " + disabled + " readonly/>");
                            htmlDep.Append("</li>");
                            htmlDep.Append("<li class='column_Cuenta'>");
                            htmlDep.Append("<input value='" + dep.CuentaBanco.NoCta + "' class='inactive' ondblclick='EditDep(event);' " + disabled + " readonly/>");
                            htmlDep.Append("</li>");
                            htmlDep.Append("<li class='column_FolioDep'><input value='" + dep.FolioDeposito + "'  class='inactive' ondblclick='EditDep(event);' " + disabled + "  readonly/></li>");
                            htmlDep.Append("<li class='column_FechaDep'><input value='" + dep.FechaDeposito.ToShortDateString() + "' class='inactive' ondblclick='EditDep(event);' " + disabled + " readonly/></li>");
                            htmlDep.Append("<li class='column_Importe'><input value='" + dep.Importe.ToString("C").Replace("$", "") + "' class='inactive' ondblclick='EditDep(event);' " + disabled + " readonly/></li>");
                            htmlDep.Append("<li class='column_Nota'><input value='" + dep.Nota + "' class='inactive' disabled='disabled' readonly/></li>");
                            htmlDep.Append("<li class='column_sttReg'><img /></li>");
                            htmlDep.Append("</ul></li>");


                        }
                    }




                }
                
                
                content_DYIng_ul.InnerHtml=html.ToString();
                content_Depositos_ul.InnerHtml=htmlDep.ToString();


                //return html.ToString();
        
        }
        private string GetSTT(string fecha,string sucursalID) {
            DateTime _fecha;
            _fecha = DateTime.Parse(fecha, culture);
            DataRow dr = Conexion.LeerRegistro("select Stt from LittleCaesarDev.dbo.CorteSucursal where FechaVta='" + _fecha + "' and SucursalID=" + sucursalID);
            return dr["Stt"].ToString();
        }

        [WebMethod]
        public static string NextID(string fecha, string ID)
        {
            var _nextID = "0";
            DateTime _fecha;
            _fecha = DateTime.Parse(fecha, culture);
            DataRow dr = Conexion.LeerRegistro("select MAX(Consecutivo)+1 AS NextID from LittleCaesarDev.dbo.CorteSucursalDeposito where FechaVta='" + _fecha.ToShortDateString() + "' and SucursalID=" + ID);
            if (dr["NextID"] != DBNull.Value) { _nextID = dr["NextID"].ToString(); }

            return _nextID;

        }

        [WebMethod]
        public static string GetCtasB(string BcoID)
        {
            var html = "";
          
            var result = lstCtas.Where(x => x.Banco.ID == Convert.ToInt32(BcoID));
            html += "<option value='0'> seleccionar </option>";
            foreach (CuentaBanco row in result.ToList())
            {
                html += "<option value='" + row.CtaBcoID + "'>" + row.NoCta + "</option>";
            }



            return html.ToString();

        }
        [WebMethod]
        public static string GetBancosB(string CtaID) {

            var html = "";

            var result = lstCtas.Where(x => x.CtaBcoID == Convert.ToInt32(CtaID));
            html += "<option value='0'>seleccionar</option>";
            foreach (CuentaBanco row in lstCtas.ToList()) {
                html += "<option value='" + row.Banco.ID.ToString() + "'>" + row.Banco.Nombre + "</option>";
            
            }

            return html.ToString();
        }

       

        [WebMethod]
        public static string GetCtas(string parameter)
        {
            var html = "";
            var select = "";
            foreach (CboTipo cta in lstCtaBanco)
            {
                if (cta.ID.ToString().Trim() == parameter.Trim()) { select = "selected='selected'"; }
                html += "<option value='" + cta.ID + "' " + select + " >" + cta.Nombre + "</option>";
                select = "";
            }
            return html;
          
        }
        [WebMethod]
        public static string GetBancos(string parameter)
        {
            var html = "";
            var select = "";
            foreach (CboTipo b in lstBancos) {
                if (b.ID.Trim() == parameter.Trim()) { select = "selected='selected'"; }
                html += "<option value='"+b.ID+"' "+select+" >"+b.Nombre+"</option>";
                select = "";
            }
            return html;
        }
        [WebMethod]
        public static string GetMoneda(string parameter)
        {
            var html = "";
            var select = "";
            foreach (CboTipo m in lstMoneda)
            {
                if (m.ID.ToString().Trim() == parameter.Trim()) { select = "selected='selected'"; }
                    html += "<option value='" + m.ID + "' " + select + ">" + m.Nombre + "</option>";
                select = "";
            }
            return html;
        }
        [WebMethod]
        public static string GetSucursales(string parameter)
        {
            var html = "";
            var select = "";
            foreach (Sucursal su in lstSuc)
            {

                var _nom = "";
                if (su.Nombre != null) { _nom = culture.TextInfo.ToTitleCase(su.Nombre.ToLower()); }
                if (su.SucursalID.ToString() == parameter) { select = "selected='selected'"; }
                html += "<option value='" + su.SucursalID + "' " + select + ">" + su.SucursalID + "-" + _nom + "</option>";
                select = "";

            }
            return html;

        }
        [WebMethod]
        public static string UpdateCorte(String[] corte)
        {

            var msg = string.Empty;
            var TypeMsg = 0;
  

            List<CorteSucursal> lstCorte = new List<CorteSucursal>();
            List<CorteSucursalDeposito> lstDeposios = new List<CorteSucursalDeposito>();

            var _corte = new CorteSucursal();
            var _deposito = new CorteSucursalDeposito();
            var _DalDep = new DepositoDAL(DBName);

            try
            {
                var _fechaVta = DateTime.Parse(corte[0],culture);
                var _sucursalID = Convert.ToInt32(corte[1]);

                var _deudor = corte[2];
                var _deudorP = corte[3];
                var _deudorD = corte[4];
                var _stt = corte[6].ToString().Trim();

                var _depositos = corte[5].Split(new char[] { ';' });


                for (var x = 0; x < _depositos.Length; x++)
                {
                    if (_depositos[x] != "")
                    {
                        var _dep = _depositos[x].Split(new char[] { '|' });
                        _deposito = new CorteSucursalDeposito();
                        _deposito.DepositoID = Convert.ToInt32(_dep[0]);
                        _deposito.Consecutivo = Convert.ToInt32(_dep[1]);

                        _deposito.CuentaBanco = new CuentaBanco()
                        {
                            CtaBcoID = Convert.ToInt32(_dep[3]),
                            Moneda = new Moneda() { MonedaID = Convert.ToInt32(_dep[4]) }
                            //Banco = new CatalogoTipo() { ID = Convert.ToInt32(_dep[2]) },


                        };

                        /*
                        _deposito.CuentaBanco.Banco.ID = Convert.ToInt32(_dep[2]);// = new CatalogoTipo() { ID = Convert.ToInt32(_dep[2]) };
                        _deposito.CuentaBanco = new CuentaBanco() { CtaBcoID = Convert.ToInt32(_dep[3]) };
                        _deposito.CuentaBanco.Moneda  = new Moneda() { MonedaID = Convert.ToInt32(_dep[4]) };
                         * */

                        _deposito.FolioDeposito = _dep[5];
                        _deposito.FechaDeposito = DateTime.Parse(_dep[6], culture);
                        _deposito.Importe = Convert.ToDouble(_dep[7]);
                        _deposito.Nota = _dep[8];
                       

                        switch (_dep[9].ToString().Trim())
                        {
                            case "RowEdit":
                                _deposito.OperationType = QSystem.Common.Enums.OperationType.Edit;
                                break;
                            case "RowNew":
                                _deposito.OperationType = QSystem.Common.Enums.OperationType.New;
                                break;
                            case "RowDelete":
                                _deposito.OperationType = QSystem.Common.Enums.OperationType.Delete;
                                break;

                        }

                        lstDeposios.Add(_deposito);
                    }
                }

                _corte.Stt = _stt;
                _corte.FechaVta = _fechaVta;
                _corte.Sucursal = new Sucursal() { SucursalID = _sucursalID };
                _corte.DeudorNombre = _deudor;
                _corte.DeudorPesos = Convert.ToDouble(_deudorP);
                _corte.DeudorDolar = Convert.ToDouble(_deudorD);
                _corte.Depositos = lstDeposios;
                _corte.OperationType = QSystem.Common.Enums.OperationType.Edit;

                lstCorte.Add(_corte);


                var result = _DalDep.SaveDepositos(lstCorte, ref msg);

                msg = TypeMsg + "||" + msg;
            }
            catch (Exception ex)
            {
                TypeMsg = 1;
                msg = TypeMsg + "||" + ex.Message.ToString();
            }
              
               // msg = "0||correcto";
            return msg;
        }

      /*  public static string NuevoDep(String[] deposito)
        {
            var msg = string.Empty;

            List<CorteSucursalDeposito> lstDeposito = new List<CorteSucursalDeposito>();
            var _deposito = new CorteSucursalDeposito();
            var _DalDep = new DepositoDAL(DBName);




           // var result = _DalDep.SaveDepositos( ref msg);










            return msg;
        }
        9*/
        protected void btnFecha_Click(object sender, EventArgs e)
        {
            if (ClFechaCap.Visible)
            {
                ClFechaCap.Visible = false;
            }
            else {
                ClFechaCap.Visible = true;
            }
        }

        protected void ClFechaCap_SelectionChanged(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha = DateTime.Today;


            if (ClFechaCap.SelectedDate <= fecha)
            {
                txtFecha.Value = ClFechaCap.SelectedDate.ToShortDateString();
            }

           // clear();

            ClFechaCap.Visible = false;

        }

        protected void Reporte_Click(object sender, EventArgs e)
        {
           // content_DYIng_ul.InnerHtml = GetCorteSucursales(txtFecha.Value);

            GetCorteSucursales(txtFecha.Value,cbxSucursales.Value);
        }

        protected void btnFCts_Click(object sender, EventArgs e)
        {

           // var result = lstCtas.Where(x => x.Banco.ID == Convert.ToInt32(BcoID));
        }
    }
}