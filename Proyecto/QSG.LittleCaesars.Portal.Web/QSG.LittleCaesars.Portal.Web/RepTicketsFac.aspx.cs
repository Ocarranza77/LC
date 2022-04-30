using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Threading;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.Messages;
using QSG.LittleCaesars.BackOffice.Messages.Requests;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.LittleCaesars.BackOffice.Messages.Response;
using QSG.LittleCaesars.BackOffice.Common.Enums;
using System.Web.UI.HtmlControls;
using QSG.QSystem.Common.Enums;

namespace QSG.LittleCaesars.Portal.Web
{
    public partial class WebForm5 : System.Web.UI.Page
    {
        public static CultureInfo culture = new CultureInfo("es-MX", true);
        public static int _usuarioID;
        public static List<Usuario> lstUser;
        public static List<Sucursal> lstSuc;
        protected void Page_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha = DateTime.Today;
            //Session["pantalla"] = 1;

            if (Session["User"] != null)
            {
                var url = Page.Request.Url.LocalPath.Split(new char[] { '/', '.' });
                var nomp = url[1];


                ruta_app.Value = Session["Empresa"].ToString() + " | " + "Reporte de Tickets y Facturas.";
                NickName.InnerText = Session["Nombre"].ToString();
                

                if (!IsPostBack)
                {

                    _usuarioID = Session["User"] !=null ? Convert.ToInt32(Session["User"].ToString()) : 0;



                    ClFechaCap.SelectedDate = fecha;
                    ClFechaFin.SelectedDate = fecha;

                    txtfechafin.Text = fecha.ToShortDateString();
                    txtfechaini.Text = fecha.ToShortDateString();

                    btnFechaIni.Text = fecha.Day.ToString();
                    btnFechafin.Text = fecha.Day.ToString();
                    FechaTicket.Checked = true;
                    /*
                    lstSuc = new List<Sucursal>();
                    lstUser = new List<Usuario>();

                    lstSuc.Add(new Sucursal() { SucursalID = 0, Nombre = "--- Todos ---" });

                    lstUser.Add(new Usuario() { CodUsuario = 0, Nombre = "--- Todos ---" });

                    lstSuc.AddRange(GetSucursales());
                    lstUser.AddRange(GetUsers());
                    */

                    GetSucursales();
                    GetUsers();

                    SelectSucursales.Items.Clear();
                    SelectSucursales.DataTextField = "Nombre";
                    SelectSucursales.DataValueField = "SucursalID";
                    SelectSucursales.DataSource = lstSuc;
                    SelectSucursales.DataBind();

                    if (lstSuc.Count < 1)
                    {
                        SelectSucursales.Items.Add(new ListItem("No existen sucursales", "", true));
                    }


                    //SelectSucursales.Items.Add(new ListItem("--- Todos ---", "0", true));
                   // SelectSucursales.Items.FindByValue("0").Selected = true;

                    SelectUsers.Items.Clear();
                    SelectUsers.DataTextField = "Nombre";
                    SelectUsers.DataValueField = "CodUsuario";
                    SelectUsers.DataSource = lstUser;
                    SelectUsers.DataBind();
                    /*
                    SelectUsers.Items.FindByValue(SelectUsers.Value).Selected = false;
                    if (SelectUsers.Items.FindByValue(_usuarioID.ToString()) != null) {
                        SelectUsers.Items.FindByValue(_usuarioID.ToString()).Selected = true;
                    }
                    */

                   // SelectUsers.Items.Add(new ListItem("--- Todos ---", "0", true));
                   // SelectUsers.Items.FindByValue("0").Selected = true;

                    Response.Cache.SetCacheability(HttpCacheability.NoCache);

                }
              


            }
            else
            {
                Session["User"] = null;
                Response.Redirect("~/Account/Login.aspx", true);
            }

        }
      
        protected void GetReporte(object sender, EventArgs e)
        {
            var t = 0;
            if (FechaTicket.Checked ) { t = 1; }
            if (FechaCapturaTick.Checked ) { t = 2; }
            if (FechaFact.Checked) { t = 3; }

            if (SelectSucursales.Value != "")
            {
                content_repT.InnerHtml = GetReporte(ClFechaCap.SelectedDate, ClFechaFin.SelectedDate, Convert.ToInt32(SelectSucursales.Value), Convert.ToInt32(SelectUsers.Value), _usuarioID, t, txtfechaini.Text, txtfechafin.Text);
            }

            
        }
        protected void ClFechaCap_SelectionChanged(object sender, EventArgs e)
        {

            txtfechaini.Text = ClFechaCap.SelectedDate.ToShortDateString();
           
            ClFechaCap.Visible = false;
        }
        protected void ClFechaFin_SelectionChanged(object sender, EventArgs e)
        {

            txtfechafin.Text = ClFechaFin.SelectedDate.ToShortDateString();

            ClFechaFin.Visible = false;
        
        }
        protected void btnFechaIni_click(object sender, EventArgs e)
        {
            ClFechaFin.Visible = false;
            if (ClFechaCap.Visible)
            {
                ClFechaCap.Visible = false;


            }
            else
            {
                ClFechaCap.Visible = true;
            }
        }
        protected void btnFechafin_click(object sender, EventArgs e)
        {
            ClFechaCap.Visible = false;
            if (ClFechaFin.Visible )
            {
               
                ClFechaFin.Visible = false;
            }
            else
            {
                ClFechaFin.Visible = true;
            }

        }

        protected void FechaTicket_CheckedChanged(object sender, EventArgs e)
        {
            if (FechaTicket.Checked)
            {
                if (SelectSucursales.Value != "")
                {
                    content_repT.InnerHtml = GetReporte(ClFechaCap.SelectedDate, ClFechaFin.SelectedDate, Convert.ToInt32(SelectSucursales.Value), Convert.ToInt32(SelectUsers.Value),_usuarioID, 1, txtfechaini.Text, txtfechafin.Text);

                }
            }
        }

        protected void FechaFact_CheckedChanged(object sender, EventArgs e)
        {
            if (FechaFact.Checked)
            {
                if (SelectSucursales.Value != "")
                {
                    content_repT.InnerHtml = GetReporte(ClFechaCap.SelectedDate, ClFechaFin.SelectedDate, Convert.ToInt32(SelectSucursales.Value), Convert.ToInt32(SelectUsers.Value),_usuarioID, 3, txtfechaini.Text, txtfechafin.Text);
                }
            }

        }

        protected void FechaCapturaTick_CheckedChanged(object sender, EventArgs e)
        {
            if (FechaCapturaTick.Checked)
            {
                if (SelectSucursales.Value != "")
                {
                    content_repT.InnerHtml = GetReporte(ClFechaCap.SelectedDate, ClFechaFin.SelectedDate, Convert.ToInt32(SelectSucursales.Value), Convert.ToInt32(SelectUsers.Value),_usuarioID, 2, txtfechaini.Text, txtfechafin.Text);
                }
            }

        }
        public void GetSucursales()
        {
            lstSuc = new List<Sucursal>();
            var su = new Sucursal();
            var sv1 = new ServiceImplementation();
            var sr = new SucursalRequest();
            su.SucursalID = 0;
            sr.Sucursal = su;
            sr.UserIDRqst = _usuarioID;
            sr.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Report;// BackOffice.Common.Enums.MessageOperationType.Report;
            var response = sv1.SucursalMessage(sr);

            if (response.Sucursales.Count > 0)
            {
                lstSuc.Add(new Sucursal() { SucursalID = 0, Nombre = "--- Todos ---" });
                lstSuc.AddRange(response.Sucursales);
            }
            else {

              
            }



           // return response.Sucursales;
        }
       public void GetUsers(){
           lstUser = new List<Usuario>();
           
           var u = new Usuario();
            var sv = new ServiceImplementation();
            var ur = new UsuarioRequest();
            u.CodUsuario = 0;
            ur.Usuario = u;
            ur.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Report; //BackOffice.Common.Enums.MessageOperationType.Report;
            var response = sv.UsuarioMessage(ur);
           // return response.Usuarios;
            if (response.Usuarios.Count > 0)
            {
                lstUser.Add(new Usuario() { CodUsuario = 0, Nombre = "--- Todos ---" });
                lstUser.AddRange(response.Usuarios);
            }
        
        }

       public static string GetReporte(DateTime fechaIni, DateTime fechaFin, int SucursalID, int CodUsuario,int usrReq, int tipo, string fi, string ff)
       {
           var html = "";
           var tck = new TicketFilter();
           //tck.Sucursal = new Sucursal() { SucursalID = SucursalID };
           //tck.CodUsuario = CodUsuario;
          

           if (tipo == 1)
           {
               //fecha ticktet orecibo
               tck.FechaVta = DateTime.Parse(fechaIni.ToShortDateString(),culture);// new System.Globalization.CultureInfo("es-ES")); // new DateTime(2014, 10, 24);
               tck.FechaVtaHasta = DateTime.Parse(fechaFin.ToShortDateString(), culture);// new System.Globalization.CultureInfo("es-ES"));
           }
           if (tipo == 2)
           {
               //fecha captura ticket
               tck.FechaCaptura = DateTime.Parse(fechaIni.ToShortDateString(),culture);// new System.Globalization.CultureInfo("es-ES"));
               tck.FechaCapturaHasta = DateTime.Parse(fechaFin.ToShortDateString(), culture);// new System.Globalization.CultureInfo("es-ES"));
           }
           if (tipo == 3)
           {
               //fecha factuiracion
               tck.FechaFactura = DateTime.Parse(fechaIni.ToShortDateString(), culture);// new System.Globalization.CultureInfo("es-ES"));
               tck.FechaFacturaHasta = DateTime.Parse(fechaFin.ToShortDateString(), culture);// new System.Globalization.CultureInfo("es-ES"));
           }

           tck.Sucursal = new Sucursal() { SucursalID = SucursalID };
           tck.CodUsuario = CodUsuario;
         //  tck.CodUsuario = CodUsuario;
          
           


           var sv = new ServiceImplementation();

           TicketResponse tr = sv.TicketMessage(new TicketRequest() { Filters = tck,UserIDRqst=usrReq, TipoTicketReporte = 1, MessageOperationType = MessageOperationType.Report });
           html += "<span>Tickets o Recibos</span><span style='float:right;margin-right:50px;'>Facturas</span>";
           html += " <ul id='container_history' >";
           html += " <li >";
           html += " <ul style='font-size:12pt;'>";
           html += " <li class='column_ID'>No.</li>";
           html += " <li class='column_Sucursal'> Sucursal</li>";
           html += " <li class='column_FechaT'> Fecha Ticket</li>";
           html += " <li class='column_HoraT' >Hora Ticket</li>";
           html += " <li class='column_FolioT'>Folio Ticket</li>";
           html += " <li class='column_Caja'># Caja</li>";
           html += " <li class='column_Cajero'>Nombre de Cajero</li>";
           html += " <li class='column_Importe'>Importe</li>";
           html += "<li class='column_vacia'>#</li>";
           html += "<li class='column_STTF'>#</li>";
           html += " <li class='column_fechaFact'>Fecha Factura</li>";
           html += " <li class='column_folioFact'>Folio Factura</li>";
           html += " <li class='column_RFC1'>RFC</li>";
           html += " </ul> ";
           html += "</li>";

           int cont = 0;
           int contador = 0;
           int countF = 0;
           int countFCan = 0;
           double montoT = 0;
           double montFac = 0;
           double montFacCancel = 0;


           foreach (Ticket ticket in tr.Tickets)
           {
               var fechaFactura = "";
               var foliofact = "";
               var rfcfact = "";
               
               
               contador++;

               html += " <li class='row'>";
               html += " <ul style='font-size:11pt;'>";
               html += " <li class='column_ID'>" + contador + "</li>";
               html += " <li class='column_Sucursal'>" + ticket.Sucursal.SucursalID.ToString() + "-" + ticket.Sucursal.Nombre + "</li>";
               html += " <li class='column_FechaT'>" + string.Format("{0:dd MMM yyyy}", ticket.FechaVta) + "</li>";
               html += " <li class='column_HoraT' >" + ticket.HoraVta + "</li>";
               html += " <li class='column_FolioT'>" + ticket.TicketID + "</li>";
               html += " <li class='column_Caja'>" + ticket.CajaID.ToString().PadLeft(3, '0') + "</li>";
               html += " <li class='column_Cajero'>" + ticket.Cajero + "</li>";
               html += " <li class='column_Importe'>  $" + ticket.Importe.ToString("#,##0.00")+ "</li>";
               html += "<li class='column_vacia'>#</li>";

               if (ticket.FechaFactura > new DateTime(1900, 01, 01))
               {
                   fechaFactura = String.Format("{0:dd/MM/yyyy}", ticket.FechaFactura);
                   if (ticket.FechaCancelacion > new DateTime(1900, 01, 01))
                   {
                       montFacCancel += ticket.Importe;
                       countFCan++;
                       html += "<li class='column_STTF'> <a href='/Facturas/Acuse_" + ticket.Cliente.RFC + ticket.FolioFactura + ".html' target='_blank' ><img src='iconos/Status-tray-busy-icon.png' title='Factura Cancelada' /></a></li>";
                   }
                   else
                   {
                       montFac += ticket.Importe;
                       countF++;
                       html += "<li class='column_STTF'><img src='iconos/Status-tray-online-icon.png'/></li>";
                       cont++;
                       montoT += ticket.Importe;
                   }
               }
               else
               {
                   html += "<li class='column_STTF'><img src='iconos/Status-tray-away-icon.png'/></li>";
                  // montoT += ticket.Importe;
                   cont++;
                   montoT += ticket.Importe;
               }
              

               //  cont++;

               /*
               if (tipo > 2)
               {
                   if (fechaFactura != "")
                   {
                          
                       if (ticket.FechaCancelacion > new DateTime(1900, 01, 01))
                       {
                          // html += "<li class='column_STTF'><img src='iconos/Status-tray-online-icon.png'/></li>";
                         //  html += "<li class='column_STTF'> <a href='/AcuseHTML/" + ticket.Cliente.RFC + ticket.FolioFactura + ".html' target='_blank' ><img src='iconos/Status-tray-busy-icon.png' title='Factura Cancelada' /></a></li>";
                       }
                       else
                       {
                          // html += "<li class='column_STTF'><img src='iconos/Status-tray-online-icon.png'/></li>";

                           if (ticket.FechaFactura > new DateTime(1900, 01, 01))
                           {
                              // html += "<li class='column_STTF'><img src='iconos/Status-tray-online-icon.png'/></li>";
                           }
                           else {
                               html += "<li class='column_STTF'><img src='iconos/Status-tray-away-icon.png'/></li>";
                           }
                       }

                       html += " <li class='column_fechaFact'>" + fechaFactura + "</li>";
                       html += " <li class='column_folioFact'>" + ticket.FolioFactura + "</li>";
                       html += " <li class='column_RFC1'><a href='/LCFacturacion/Facturas_HTML/" + ticket.Cliente.RFC + ticket.FolioFactura + ".html'  target='_blank' >" + ticket.Cliente.RFC + "</a></li>";
                       html += " </ul> ";
                       html += "</li>";

                           
                   }

               }
               else {
                   */
               /*  cont++;
                 html += " <li class='row'>";
                 html += " <ul style='font-size:11pt;'>";
                 html += " <li class='column_ID'>" + cont + "</li>";
                 html += " <li class='column_Sucursal'>" + ticket.Sucursal.Abr + "</li>";
                 html += " <li class='column_FechaT'>" + ticket.FechaVta.ToString("dd/MM/yyyy") + "</li>";
                 html += " <li class='column_HoraT' >" + ticket.HoraVta + "</li>";
                 html += " <li class='column_FolioT'>" + ticket.TicketID + "</li>";
                 html += " <li class='column_Caja'>" + ticket.CajaID.ToString().PadLeft(3,'0') + "</li>";
                 html += " <li class='column_Cajero'>" + ticket.Cajero + "</li>";
                 html += " <li class='column_Importe'>  $" + ticket.Importe.ToString("#,##0.00").Replace(',', '.') + "</li>";
                 html += "<li class='column_vacia'>#</li>";
                * */

               /*
                        
                        
               if (ticket.FechaCancelacion > new DateTime(1900, 01, 01))
               {
                  // html += "<li class='column_STTF'><img src='iconos/Status-tray-online-icon.png'/></li>";

                   html += "<li class='column_STTF'><a href='/AcuseHTML/" + ticket.Cliente.RFC + ticket.FolioFactura + ".html' target='_blank' ><img src='iconos/Status-tray-busy-icon.png'  title='Factura Cancelada'/></a></li>";
               }
               else
               {
                   if (ticket.FechaFactura > new DateTime(1900, 01, 01))
                   {
                       html += "<li class='column_STTF'><img src='iconos/Status-tray-online-icon.png'/></li>";
                   }
                   else
                   {
                       html += "<li class='column_STTF'><img src='iconos/Status-tray-away-icon.png'/></li>";
                   }

                  // html += "<li class='column_STTF'><img src='iconos/Status-tray-busy-icon.png'/></li>";
               }



               /*
               html += " <li class='column_fechaFact'>" + fechaFactura + "</li>";
               html += " <li class='column_folioFact'>" + ticket.FolioFactura + "</li>";
               html += " <li class='column_RFC1'> <a href='/LCFacturacion/Facturas_HTML/" + ticket.Cliente.RFC + ticket.FolioFactura + ".html'  target='_blank' >" + ticket.Cliente.RFC + "</a></li>";
               html += " </ul> ";
               html += "</li>";
              */




               //  }


               html += " <li class='column_fechaFact'>" + fechaFactura + "</li>";
               html += " <li class='column_folioFact'>" + ticket.FolioFactura + "</li>";
               html += " <li class='column_RFC1'><a href='/Facturas/" + ticket.Cliente.RFC + ticket.FolioFactura + ".pdf'  target='_blank' >" + ticket.Cliente.RFC + "</a></li>";
               html += " </ul> ";
               html += "</li>";

           
           }



           html += "</ul>";

           html += "  <div class='div_resumen'>";
           html += "             <div>";
           html += "<span style='float:left;width:98%;height:auto;margin-left:5px;margin-top:10px;font-weight:bold;'>RESUMEN</span>";
           html += "<span style='float:left;width:98%;height:auto;margin-left:5px;margin-top:10px;font-weight:italic;'>Rango de Fechas : <span style='font-weight:bold;color:black;'>" + String.Format("{0:dd MMM yyyy}", fechaIni) + " A " + String.Format("{0:dd MMM yyyy}",fechaFin) +"</span></span>";
           html += "               <ul>";
           html += "                    <li style='list-style:none;'><span>No. de tickets activos:</span><span name='qty'>" + cont + "</span><span>Monto:</span><span name='monto'>$" + montoT.ToString("#,##0.00") + "</span></li>";
           html += "                    <li style='list-style:none;'><span>No. de Facturas</span><span name='qty'>" + countF + "</span><span>Monto:</span><span name='monto'>$" + montFac.ToString("#,##0.00") + "</span></li>";
           html += "                    <li style='list-style:none;'><span>No. de Facturas Canceladas </span><span name='qty'>" + countFCan + "</span><span>Monto:</span><span name='monto'>$" + montFacCancel.ToString("#,##0.00") + "</span></li>";
           html += "               </ul>";
           html += "            </div>";
           html += "       </div>";


           return html + tr.FriendlyMessage.ToString();


       }

       protected void Button1_Click(object sender, EventArgs e)
       {
           Page.Response.Redirect(Page.Request.Url.ToString(), true);
       }

       
    }
}