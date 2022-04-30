using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.Common.Enums;
using QSG.LittleCaesars.BackOffice.Messages;
using QSG.LittleCaesars.BackOffice.Messages.Requests;
using QSG.LittleCaesars.BackOffice.Messages.Response;
using System.Web.Services;
using QSG.LittleCaesars.BackOffice.BL;
using QSG.LittleCaesars.BackOffice.DAL;
using System.Text.RegularExpressions;
using QSG.QSystem.Common.Enums;

namespace QSG.LittleCaesars.Portal.Web
{
    public partial class WebForm6 : System.Web.UI.Page
    {
       public static CultureInfo culture = new CultureInfo("es-MX", true);
       
       public static CFDI2015 CFDI = new CFDI2015();
       public static claseCFDIv33 CFDIv33 = new claseCFDIv33();


        public string Expcert = "";
        public string QTYTimbres = "";
        public static string _usuarioID;

        protected void Page_Load(object sender, EventArgs e)
        {

            Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha = DateTime.Today;

            List<Creditos> lstCred = new List<Creditos>();
            //Session["pantalla"] = 1;

            if (Session["User"] != null)
            {
                var url = Page.Request.Url.LocalPath.Split(new char[] { '/', '.' });
                var nomp = url[1];
                var credit = 0;




                CFDI._UserPak = "CES070913FQ3"; //"DEMO1409252TA";// "CES070913FQ3";
                CFDI._ClavePak = "TgCSiXi+";//"oA9YK3h1JO=";// "TgCSiXi+";
                _usuarioID = Session["User"].ToString();

                lstCred = CFDI.Creditos();

                if (lstCred.Count > 0)
                {
                    credit = lstCred[0].TimbresRestantes;
                }
                ruta_app.Value = Session["Empresa"].ToString() + " | " + "Cancelacion de Facturas SAT";
                NickName.InnerText = Session["Nombre"].ToString();
              
                //  ClaseCFDI CFDI = new ClaseCFDI();
                var cert = new Certificado(HttpContext.Current.Server.MapPath("~/Certificados/CES070913FQ3_20160603_111637s.cer"));
                
                QTYTimbres = " <span class='btndefault'>Timbres disp.: <input value='" + credit+ "' readonly/></span>";

                if ((((cert.ValidoHasta.Year - DateTime.Today.Year) * 12) + (cert.ValidoHasta.Month - DateTime.Today.Month)) > 3 && (((cert.ValidoHasta.Year - DateTime.Today.Year) * 12) + (cert.ValidoHasta.Month - DateTime.Today.Month)) < 6)
                {
                    Expcert = " <span class='btndefault' style=' background: rgb(223, 117, 20);' title='Vence en : " + DiferenciaFechas(cert.ValidoHasta, DateTime.Today) + "'  id='EXPCert'>Certificado Activo </span>";
                }
                if (((cert.ValidoHasta.Year - cert.ValidoDesde.Year) * 12) + (cert.ValidoHasta.Month - cert.ValidoDesde.Month) < 4)
                {
                    Expcert = " <span class='btndefault' style='background: rgb(202, 60, 60);' title='Vence en : " + DiferenciaFechas(cert.ValidoHasta, DateTime.Today) + "'  id='EXPCert'>Certificado Activo </span>";
                }

                // string mDias = //((cert.ValidoHasta.Year - cert.ValidoDesde.Year) * 12) + (cert.ValidoHasta.Month - cert.ValidoDesde.Month) + " Meses " + cert.ValidoHasta.Subtract(cert.ValidoDesde).Days / (365.25 / 12);
                Expcert = " <span class='btndefault' style='background: rgb(28, 184, 65);' title='Vence en : " + DiferenciaFechas(cert.ValidoHasta, DateTime.Today) + "'  id='EXPCert'>Certificado Activo </span>";
                


                if (!IsPostBack)
                {
                    ClFechaCap.SelectedDate = fecha;
                   // txtfechafin.Text = fecha.ToShortDateString();
                   txtfechaini.Text = fecha.ToShortDateString();

                    btnFechaIni.Text = fecha.Day.ToString();
                   // btnFechafin.Text = fecha.Day.ToString();
                   // FechaTicket.Checked = true;
                    
                    SelectSucursales.Items.Clear();
                    SelectSucursales.DataTextField = "Nombre";
                    SelectSucursales.DataValueField = "SucursalID";
                    SelectSucursales.DataSource = GetSucursales();
                    SelectSucursales.DataBind();
                    SelectSucursales.Items.Add(new ListItem("--- Todos ---", "0", true));
                    SelectSucursales.Items.FindByValue("0").Selected = true;
                    /*
                    SelectUsers.Items.Clear();
                    SelectUsers.DataTextField = "Nombre";
                    SelectUsers.DataValueField = "CodUsuario";
                    SelectUsers.DataSource = GetUsers();
                    SelectUsers.DataBind();
                    SelectUsers.Items.Add(new ListItem("--- Todos ---", "0", true));
                    SelectUsers.Items.FindByValue("0").Selected = true;
                    */

                   // ClaseCFDI CFDI = new ClaseCFDI();
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    
                }


            }
            else
            {
                Session["User"] = null;
                Response.Redirect("~/Account/Login.aspx", true);
            }
        }
        private String DiferenciaFechas(DateTime newdt, DateTime olddt)
        {
            Int32 anios;
            Int32 meses;
            Int32 dias;
            String str = "";

            anios = (newdt.Year - olddt.Year);
            meses = (newdt.Month - olddt.Month);
            dias = (newdt.Day - olddt.Day);

            if (meses < 0)
            {
                anios -= 1;
                meses += 12;
            }
            if (dias < 0)
            {
                meses -= 1;
                dias += DateTime.DaysInMonth(newdt.Year, newdt.Month);
            }

            if (anios < 0)
            {
                return "Fecha Invalida";
            }
            if (anios > 0)
                if (anios == 1) {
                    str = str + anios.ToString() + " año ";
                } else {
                    str = str + anios.ToString() + " años ";
                }
               
            if (meses > 0)
                if (meses == 1)
                {
                    str = str + meses.ToString() + " mes ";
                }
                else
                {
                    str = str + meses.ToString() + " meses ";
                }
                
            if (dias > 0)
                if (dias == 1)
                {
                    str = str + dias.ToString() + " dia ";
                }
                else
                {
                    str = str + dias.ToString() + " dias ";
                }
                

            return str;
        } 

        protected void GetReporte(object sender, EventArgs e)
        {


           content_repT.InnerHtml = GetRepfact(ClFechaCap.SelectedDate, Convert.ToInt32(SelectSucursales.Value));

        }
        protected void ClFechaCap_SelectionChanged(object sender, EventArgs e)
        {

            txtfechaini.Text = ClFechaCap.SelectedDate.ToShortDateString();

            ClFechaCap.Visible = false;
        }
        protected void CancelFact(object sender, EventArgs e)
        {
           
            
        }

        public List<Sucursal> GetSucursales()
        {
            var su = new Sucursal();
            var sv1 = new ServiceImplementation();
            var sr = new SucursalRequest();
            su.SucursalID = 0;
            sr.Sucursal = su;
            sr.UserIDRqst = Convert.ToInt32(_usuarioID);
            sr.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Report;// BackOffice.Common.Enums.MessageOperationType.Report;
            var response = sv1.SucursalMessage(sr);
            return response.Sucursales;
        }
        protected void btnFechaIni_click(object sender, EventArgs e)
        {
            if (ClFechaCap.Visible)
            {
                ClFechaCap.Visible = false;


            }
            else
            {
                ClFechaCap.Visible = true;
            }
            //Expcert = " <span class='btndefault'  id='EXPCert'>Certificado SAT expira en  30 dias</span>";
        }
        
        public static string GetRepfact(DateTime fechafactura,int SucursalID)
        {
            
            var tck = new TicketFilter();
            tck.FechaFactura = DateTime.Parse(fechafactura.ToShortDateString(), new System.Globalization.CultureInfo("es-MX"));
            tck.FechaFacturaHasta = DateTime.Parse(fechafactura.ToShortDateString(), new System.Globalization.CultureInfo("es-MX"));

            tck.Sucursal = new Sucursal() { SucursalID = SucursalID };

            tck.CodUsuario = 0;


            var html = "";

            var sv = new ServiceImplementation();

            TicketResponse tr = sv.TicketMessage(new TicketRequest() { Filters = tck,UserIDRqst=Convert.ToInt32(_usuarioID), MessageOperationType = MessageOperationType.Report });
            
            html+=" <ul id='container_history'>";
            html+="        <li>";
            html+="             <ul>";
            html+="                 <li class='column_ID'>ID</li>";
            html += "                 <li class='column_fechaFact'>Fecha</li>";
            html += "                 <li class='column_FolioT'>Ticket ID</li>";
            html+="                 <li class='column_folioFact'>Folio</li>";
            html += "                 <li class='column_RFCfact'>RFC</li>";
            html+="                  <li class='column_Importe'>Importe</li>";
            html+="                   <li class='column_STTFact'>Estatus</li>";
            html += "                   <li class='column_UUID'>Estatus</li>";
            html += "                   <li class='column_MotC'>Detalle Cancelacion</li>";
            html+="               </ul>";
            html+="           </li>";


           
            int count=0;
            foreach (Ticket ticket in tr.Tickets)
            {

                html += "        <li class='row'>";
                html += "             <ul>";
                html += "                 <li class='column_ID'>" + count++ + "</li>";
                html += "                 <li class='column_fechaFact'>" + String.Format("{0:dd MMM yyyy}", ticket.FechaFactura) + "</li>";
                html += "                 <li class='column_FolioT'>" + ticket.Sucursal.SucursalID.ToString("000") + ticket.CajaID.ToString("000") + "#" + ticket.TicketID + "</li>";
                html += "                 <li class='column_folioFact'>" + ticket.FolioFactura + "</li>";
                html += "                 <li class='column_RFCfact' title='" + ticket.Cliente.RazonSocial + "' style='cursor: pointer;' >" + ticket.Cliente.RFC + "</li>";
                html += "                  <li class='column_Importe'>" + ticket.Importe.ToString("$#,##0.00")+ "<input type='text' style='display:none;' value='" + ticket.Importe + "' reaonly/></li>";

                if (ticket.FechaCancelacion > new DateTime(1900, 01, 01))
                {
                    html += " <li class='column_STTFact'><input type='button' class='ButtonCancelado' value='Cancelado' /></li>";
                    html += " <li class='column_UUID'>" + ticket.UUID + "</li>";
                    html += " <li class='column_MotC'><input type='text'style='border:none;' value='" + ticket.MotivoCancelacion + "' readonly/></li>";
                }
                else
                {
                    html += " <li class='column_STTFact'><input type='button' class='ButtonCancelar' value='Cancelar' onclick='return BntClick(event);'/></li>";
                    html += " <li class='column_UUID'>" + ticket.UUID + "</li>";
                    html += " <li class='column_MotC'><input type='text' value='" + ticket.MotivoCancelacion + "' /></li>";

                }

               
                html += "               </ul>";
                html += "           </li>";

            }
            html += "</ul></li>";

            return html;
        }

        [WebMethod]
        public static string CancelarFactura(String[] Fact_Inf)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            DateTime fechaT = DateTime.Today;
            string msg = string.Empty;
            try
            {


                var info = Fact_Inf[3].Split(new char[] { '#' });
                var tk = new Ticket();
                var sv = new ServiceImplementation();
                var tr = new TicketRequest();
                var tkDal = new TicketDAL();

                

                tk.TicketID = Convert.ToInt32(info[1]);
                tk.Sucursal = new Sucursal() { SucursalID = Convert.ToInt32(info[0].Substring(0, 3)) };
                tk.CajaID = Convert.ToInt32(info[0].Substring(3, 3));
                string imp = String.Format("{0:0.00}", info[2].ToString().Trim());
                tk.Importe = Convert.ToDouble(imp, CultureInfo.InvariantCulture);
                tk.RFC = Fact_Inf[1];

                //tk.FechaCancelacion = DateTime.Now;
                tr.Ticket = tk;
                tr.UserIDRqst = Convert.ToInt32(_usuarioID);
                tr.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Query;// BackOffice.Common.Enums.MessageOperationType.Query;
                var response = sv.TicketMessage(tr);


                var urlBase = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority;

                List<string> _lstUUID = new List<string>();
                _lstUUID.Add(Fact_Inf[0]);

                FacturarConResponse _empresa = sv.FacturarComMessage(new FacturarConRequest() { SucursalID = tk.Sucursal.SucursalID, UserIDRqst = Convert.ToInt32(_usuarioID), MessageOperationType = QSystem.Common.Enums.MessageOperationType.Query /* BackOffice.Common.Enums.MessageOperationType.Query */});
                var empresa = _empresa.Datos.Empresa; //_empresa.Empresa;




                CFDI._Clave = empresa.PassKey;// "CESARMEX1";
                CFDI._UserPak = empresa.UserPak; // "DEMO1409252TA"; //"CES070913FQ3";
                CFDI._ClavePak = empresa.ClavePak; // "oA9YK3h1JO="; //"TgCSiXi+";
                CFDI._rutaLogoSAT = urlBase + "/Images/logo_shcp.jpg";
                CFDI._rutaFacturas = HttpContext.Current.Server.MapPath("/Facturas");
                CFDI._RutaPKcs12 = HttpContext.Current.Server.MapPath("~/Certificados/Files_Pkcs12/"+ empresa.Serao);
                CFDI._rutaXML = HttpContext.Current.Server.MapPath("/XML");/*lleva la misma ruta folder XML*/
                CFDI.Emisor = new ComprobanteEmisor() { rfc =empresa.RFC};  //"CES070913FQ3" 

                
                

                var ser = Regex.Match(response.Ticket.FolioFactura, @"^[A-Za-z]+").Value;
                var fol = Regex.Match(response.Ticket.FolioFactura, @"\d+").Value;

                CFDI.Factura = new Comprobante() { serie = ser, folio = fol, Receptor = new ComprobanteReceptor() { rfc = Fact_Inf[1] } };   //= Fact_Inf[1] };
                CFDI.LstUUID = _lstUUID;

                string _rutaAcuse = CFDI._rutaXML + "\\Acuse_" + CFDI.Factura.Receptor.rfc + CFDI.Factura.serie + CFDI.Factura.folio + ".xml";
                string _rutaAcuseHTML = CFDI._rutaFacturas + "\\Acuse_" + CFDI.Factura.Receptor.rfc + CFDI.Factura.serie + CFDI.Factura.folio + ".html";

                CFDI.GenerarPath(out msg);
              

                var objTicket = response.Ticket;


                
                objTicket.FechaCancelacion = DateTime.Parse(fechaT.ToShortDateString(), new System.Globalization.CultureInfo("es-MX"));  // DateTime.Now;

                //objTicket.AcuseXML = CFDI.Cancelar(out msg);
                bool GrabaTiket = false;
                objTicket.AcuseXML = CFDIv33.Cancelar(CFDI._Clave, CFDI._UserPak, CFDI._ClavePak, CFDI._rutaLogoSAT, CFDI._rutaFacturas, CFDI._RutaPKcs12, CFDI._rutaXML, CFDI.Emisor, CFDI.LstUUID, _rutaAcuse, _rutaAcuseHTML,tk.RFC,tk.Importe, out msg, out GrabaTiket); 
                objTicket.MotivoCancelacion = Fact_Inf[4].ToString();

                if (GrabaTiket == true)
                {
                    if (msg!="")
                    objTicket.MotivoCancelacion += "; ERROR "+ msg;  
 
                        objTicket.OperationType = QSystem.Common.Enums.OperationType.Edit; //BackOffice.Common.Enums.OperationType.Edit;
                        var result = tkDal.SaveTicket(objTicket, ref msg);

                }
            }
            catch (Exception ex)
            {
                msg += ex.Message;
                
            }

            return msg;

        }
        [WebMethod]
        public static string NewTicket(string folio)
        {
            string msg = string.Empty;
            var datos = folio.Split(new char[] { '#' });

            var tk = new Ticket();
            var sv = new ServiceImplementation();
            var tr = new TicketRequest();
            var tkDal = new TicketDAL();

            tk.TicketID = Convert.ToInt32(datos[1]);
            tk.Sucursal = new Sucursal() { SucursalID = Convert.ToInt32(datos[0].Substring(0, 3)) };
            tk.CajaID = Convert.ToInt32(datos[0].Substring(3, 3));
            string imp = String.Format("{0:0.00}", datos[2].ToString().Trim());
            tk.Importe = Convert.ToDouble(imp, CultureInfo.InvariantCulture);

            //tk.FechaCancelacion = DateTime.Now;
            tr.Ticket = tk;
            tr.UserIDRqst = Convert.ToInt32(_usuarioID);
            tr.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Query;// BackOffice.Common.Enums.MessageOperationType.Query;
            var response = sv.TicketMessage(tr);

            var objTicket = new Ticket();
            objTicket.TicketID = -response.Ticket.TicketID;
            objTicket.Sucursal = new Sucursal() { SucursalID = response.Ticket.Sucursal.SucursalID };
            objTicket.CajaID = response.Ticket.CajaID;
            objTicket.Importe = response.Ticket.Importe;
            objTicket.FechaVta = response.Ticket.FechaVta;
            objTicket.HoraVta = response.Ticket.HoraVta;
            objTicket.Cajero = response.Ticket.Cajero;
            objTicket.FechaCaptura = response.Ticket.FechaCaptura;
            objTicket.CodUsuario = response.Ticket.CodUsuario;
            objTicket.OperationType = QSystem.Common.Enums.OperationType.New; //BackOffice.Common.Enums.OperationType.New;
            var result = tkDal.SaveTicket(objTicket, ref msg);
            return msg;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

    }
}