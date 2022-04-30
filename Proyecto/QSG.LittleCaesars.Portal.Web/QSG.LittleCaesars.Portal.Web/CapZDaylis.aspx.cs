using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Xsl;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.Common.Enums;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.LittleCaesars.BackOffice.Messages;
using QSG.LittleCaesars.BackOffice.Messages.Requests;
using QSG.LittleCaesars.BackOffice.Messages.Response;
using QSG.QSystem.Common.Entities;
using QSG.QSystem.Common.Enums;

namespace QSG.LittleCaesars.Portal.Web
{
    public partial class WebForm7 : System.Web.UI.Page
    {
        public string NombrePantalla;
        public static CultureInfo culture = new CultureInfo("es-MX", true);
        public static string _usuarioID;
        public static List<Sucursal> lstSuc;
        public static string r_logo;
        public static string _user;
        public static string Titulo;
        public static string DBName;
        public static XmlDocument xml;
        public static XmlDocument xmlA;
        public static DataSet dtxml;


        //public static XmlDocument xmlA;
        protected void Page_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha = DateTime.Today;



            if (Session["User"] != null)
            {
               
                _usuarioID = Session["User"].ToString();
                lstSuc = new List<Sucursal>();
                lstSuc = LlenarSucursales(_usuarioID);
                if (!IsPostBack)
                {
                    ClFechaCap.SelectedDate = fecha;
                    NombrePantalla = "Captura Z's y Daily's.";
                    r_logo = "Images/little.png";
                    _user = Session["Nombre"].ToString();

                    Titulo = Session["Empresa"].ToString() + " | " + NombrePantalla;
                    DBName = Session["DBName"].ToString();


                    txtFecha.Value = fecha.ToShortDateString();

                    
                    SelectUsers.Items.Clear();
                    SelectUsers.DataTextField = "Nombre";
                    SelectUsers.DataValueField = "CodUsuario";
                    SelectUsers.DataSource = GetUsers();
                    SelectUsers.DataBind();
                    SelectUsers.Items.Add(new ListItem("--- Todos ---", "0", true));
                    SelectUsers.Items.FindByValue(Session["User"].ToString()).Selected = true;


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
        [WebMethod]
        public static string GetSucursales()
        {
            var html = "";
            foreach (Sucursal su in lstSuc) {

                var _nom = "";
                if (su.Nombre != null) { _nom = culture.TextInfo.ToTitleCase(su.Nombre.ToLower()); }
                
                html += "<option value='"+su.SucursalID+"'>"+su.SucursalID+"-"+_nom+"</option>";
            
            }
            return html;
         
        }
        private List<Sucursal> LlenarSucursales(string code) {
            var su = new Sucursal();
            var sv1 = new ServiceImplementation();
            var sr = new SucursalRequest();
            su.SucursalID = 0;
            sr.Sucursal = su;
            sr.UserIDRqst = Convert.ToInt32(code);
            sr.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Report; //BackOffice.Common.Enums.MessageOperationType.Report;
            var response = sv1.SucursalMessage(sr);
            return response.Sucursales;
           // lstSuc = new List<Sucursal>();
           // lstSuc = response.Sucursales;
            ///return response.Sucursales;
        
        }

        protected void btnFecha_Click(object sender, EventArgs e)
        {
            if (ClFechaCap.Visible)
            {
                ClFechaCap.Visible = false;
            }
            else
            {
                ClFechaCap.Visible = true;

            }
        }

        protected void ClFechaCap_SelectionChanged(object sender, EventArgs e)
        {
            /*Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha = DateTime.Today;
            if (ClFechaCap.SelectedDate <= fecha)
            {
                txtFecha.Text = ClFechaCap.SelectedDate.ToShortDateString();
                ClFechaCap.Visible = false;
               /* content_repT.InnerHtml = GetTickets(ClFechaCap.SelectedDate);*/

            /*}*/
        }
        protected void RunReporte(object sender, EventArgs e)
        {
            content_RegZ_ul.InnerHtml = GetDatos(txtFecha.Value);
            content_Daily_ul.InnerHtml = GetAcumulado(txtFecha.Value);

          // content_repT.InnerHtml = GetDatos(ClFechaCap.SelectedDate.ToShortDateString());
           // content_repT2.InnerHtml = GetAcumulado(ClFechaCap.SelectedDate.ToShortDateString());

         
        }
        private void clear() {

            var html = "";
            //  html += "  <span class='CSSHeader' >Registro de Z's</span><ul  runat='server' id='container_history' >";
            html += "     <li>";
            html += "        <ul>";

            html += "            <li class='column_ID'>No.</li>";
            html += "            <li class='column_Edit'><img/></li>";
            html += "            <li class='column_Del'><img /></li>";
            html += "            <li class='column_Sucursal'> Sucursal</li>";
            html += "            <li class='column_FechaT'>Fecha Z</li>";
            html += "            <li class='column_HoraT'>Hora Cierre Z</li>";
            html += "            <li class='column_FolioT'>Folio Z</li>";
            html += "            <li class='column_Caja'>Num. Caja</li>";
            html += "            <li class='column_NT'>Num. Transacc</li>";
            html += "            <li class='column_Cajero'>Cajero(s)</li>";
            html += "            <li class='column_ImpEfectivo'>Efectivo(cash)</li>";
            html += "            <li class='column_ImpTcredito'>T.Credito(Card)</li>";
            html += "            <li class='column_ImpTdebito'>T.Debito(Card)</li>";
            html += "            <li class='column_OFPago'>Otra Forma de Pago</li>";
            html += "            <li class='column_Importe'>Importe(Grss)</li>";
            html += "             <li class='column_sttReg' ></li>	";
            html += "         </ul>";
            html += "     </li> ";


            html += "<li class='Rowfooter'>";
            html += "       <ul >";
            html += "         <li class='column_stt' style='color:white;'></li>";
            html += "         <li class='column_CodeRegZ' style='color:white;'></li>";
            html += "         <li class='row_ID' style='color:white;'></li>";
            html += "         <li class='column_ID' style='color:white;'></li>";
            html += "          <li class='column_Edit' style='background-color:white;' ><img /></li>";
            html += "           <li class='column_Del' style='background-color:white;'><img /></li>";
            html += "          <li class='column_Sucursal'></li>";
            html += "          <li class='column_FechaT' ></li>";
            html += "           <li class='column_HoraT'></li>";
            html += "           <li class='column_FolioT'></li>";
            html += "           <li class='column_Caja'></li>";
            html += "           <li class='column_NT'></li>";
            html += "           <li class='column_Cajero' style='font-weight:bold;'>Sub Totales</li>";
            html += "          <li class='column_ImpEfectivo'><input value='0'  readonly/></li>";
            html += "          <li class='column_ImpTcredito'><input value='0' readonly/></li>";
            html += "          <li class='column_ImpTdebito'><input value='0' readonly/></li>";
            html += "          <li class='column_OFPago'><input value='0' readonly/></li>";
            html += "          <li class='column_Importe'><input value='0' readonly/></li>";
            html += "          <li class='column_sttReg'  ></li>";
            html += "          </ul>";
            html += "       </li> ";

            content_RegZ_ul.InnerHtml = html;

            html = "";

            html += "                         <li>";
            html += "                           <ul>";

            html += "                                <li class='column_ID'>No.</li>";
            html += "                               <li class='column_Check><img   /></li>";
            html += "                               <li class='column_Edit'><img class='btnEdit' src='iconos/ic_edit1.png'  title='Editar registro' /></li>";
            html += "                             <li class='column_Sucursal'>Sucursal</li>";
            html += "                              <li class='column_Supervisor' >Fecha Daily</li>";
            html += "                              <li class='column_FechaDaily' >Fecha Daily</li>";
            html += "                             <li class='column_NumZ'>No.Z</li>";
            html += "                             <li class='column_TotalIng'>Total Ingresos</li>";


            /*add columnas*/
            html += "                             <li class='column_ImpTcredito'>T.Credito</li>";
            html += "                             <li class='column_ImpTdebito'>T.Credito</li>";
            html += "                             <li class='column_OFPago'>Otra Forma de Pago</li>";

            html += "                            <li class='column_TotalEfectivo'>Total Efectivo Daily </li>";
            html += "                            <li class='column_TotalEfectZ'>Total Efectivo Z </li>";
            html += "                           <li class='column_VariacionDayli'>Variacion Daily(Debe ser Cero)</li>";

            html += "                                     <li  class='column_TipoCambio'>T.C.</li>";
            html += "                             <li class='column_EfecDeposito'>Efectivo para Deposito";
            html += "                                 <ul >";
            html += "                                     <li  class='column_EfectivoDepP'>Pesos</li>";
            html += "                                     <li  class='column_EfectivoDepD'>Dolares</li>";

            html += "                                     <li  class='column_EfectivoDepPConv'>Conversion Pesos</li>";
            html += "                                </ul>";
            html += "                           </li>";

            html += "                           <li class='column_BosalDeposito'>Bolsa(s) Servicio Blindado";
            html += "                              <ul>";
            html += "                                    <li class='column_BolsaP'>Pesos</li>";
            html += "                                    <li class='column_BolsaD'>Dolares</li>";

            html += "                                    <li class='column_BolsaPConv'>Conversion Pesos</li>";
            html += "                                </ul>";
            html += "                           </li>";
            html += "                           <li class='column_FolioServices'>Folio Services</li>";
            html += "                           <li class='column_GastoDeu'>Gastos(a deudores)</li>";
            html += "                            <li class='column_Sob'>Sobrantes</li>";
            html += "                            <li class='column_CajeroCorto'>Faltantes</li>";
            html += "                            <li class='column_Falt'>Faltantes</li>";


            html += "                            <li class='column_ComentariosDayli'><img src='iconos/msgNew.png' /></li>";

            html += "                     </ul>";
            html += "                  </li> ";




            html += " <li class='Rowfooter'>";
            html += "                     <ul>";
            html += "                         <li class='column_stt' style='color:white;'></li>";
            html += "                         <li class='column_CodeRegZ' style='color:white;'></li>";
            html += "                         <li class='row_ID' style='color:white;'></li>";
            html += "                         <li class='column_ID' style='color:white;'></li>";
            html += "                        <li class='column_Edit' style='background-color:white;'></li>";
            //html += "
            html += "                   <li class='column_Sucursal' style='font-weight:bold;'> Total Ingreso del dia. </li>";
            //  html += "                      Total Ingreso del dia.
            // html += "                   </li>
            html += "                   <li class='column_Supervisor' ></li>";
            html += "                   <li class='column_FechaDaily' ></li>";
            html += "                   <li class='column_NumZ'></li>";
            html += "                   <li class='column_TotalIng'><input value='0'  readonly/></li>";
            html += "                   <li class='column_ImpTcredito'><input value='0'  readonly/></li>";
            html += "                   <li class='column_ImpTdebito'><input value='0'  readonly/></li>";
            html += "                   <li class='column_OFPago'><input  value='0' readonly/></li>";

            html += "               <li class='column_TotalEfectivo'></li>";
            html += "               <li class='column_TotalEfectZ'><input value='0'  readonly/></li>";
            html += "               <li class='column_VariacionDayli'></li>";

            html += "                   <li class='column_TipoCambio'></li>";
            //  html += "                  
            html += "                    <li class='column_EfectivoDepP'><input value='0'  readonly/></li>";
            html += "                   <li class='column_EfectivoDepD'><input  value='0' readonly/></li>";

            html += "                   <li class='column_EfectivoDepPConv'></li>";
            //html += "                   <li class='column_FolioServices'></li>";
            html += "                   <li class='column_BolsaP'><input  value=0' readonly/></li>";
            html += "                   <li class='column_BolsaD'><input  value='0' readonly/></li>";
            //html += "                   <li class='column_TipoCambio'></li>";
            html += "                   <li class='column_BolsaPConv'></li>";
            html += "                   <li class='column_FolioServices'></li>";
            // html += "
            html += "                <li class='column_GastoDeu'></li>";
            html += "                <li class='column_Sob'></li>";
            html += "                <li class='column_CajeroCorto'></li>";
            html += "                <li class='column_Falt'></li>";

            // html += "               <li class='column_TotalEfectZ'><input value='$" + String.Format(CultureInfo.InvariantCulture, "{0:#,0.00}", _totImp) + "'  readonly/></li>";
            // html += "               <li class='column_VariacionDayli'></li>";
            html += "              <li class='column_ComentariosDayli'></li>";

            html += "         </ul>";
            html += "     </li> ";

            content_Daily_ul.InnerHtml = html;

        }
        public static string GetDatos(string fecha)
        {
            Thread.CurrentThread.CurrentCulture = culture;
           // DateTime fecha = DateTime.Today;


           

            var su = new Sucursal();
            var sv = new ServiceImplementation();
            var sr = new SucursalRequest();
            su.SucursalID = 0;

            sr.Sucursal = su;
            sr.UserIDRqst = Convert.ToInt32(_usuarioID);
            sr.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Report; //BackOffice.Common.Enums.MessageOperationType.Report;
            var respSuc = sv.SucursalMessage(sr);


            var CrtFilter = new CorteZFilter();
            var CrtSucFilter = new CorteSucursalFilter();

           
            sv = new ServiceImplementation();
            
            CrtFilter.FechaVta = DateTime.Parse(fecha.ToString(), culture);
            CrtFilter.FechaVtaHasta = DateTime.Parse(fecha.ToString(), culture);
           // CrtSucFilter.FechaVtaHasta = DateTime.Parse(fecha.ToString(), culture);

            CorteZResponse CrtResp = sv.CorteZMessage(new CorteZRequest() { Filters = CrtFilter,UserIDRqst=Convert.ToInt32(_usuarioID), ReturnXML=true, MessageOperationType = MessageOperationType.Report });
           // CorteSucursalResponse CrtSucResp = sv.CorteSucursalMessage(new CorteSucursalRequest() { Filters = CrtSucFilter, MessageOperationType = MessageOperationType.Report });
            xml = new XmlDocument();
            xml.InnerXml = CrtResp.XML;
           
          /*
            dtxml = new DataSet();
            dtxml.ReadXml(new StringReader(CrtResp.XML));*/

            var selec = "";
            var count = 0;

            double _totEfec = 0;
            double _totTCredit = 0;
            double _totTDebito = 0;
            double _otrFPag = 0;
            double _totImp = 0;







            var html = "";
          //  html += "  <span class='CSSHeader' >Registro de Z's</span><ul  runat='server' id='container_history' >";
            html += "     <li>";
            html += "        <ul>";
           
            html += "            <li class='column_ID'>No.</li>";
            html += "            <li class='column_Edit'><img/></li>";
            html += "            <li class='column_Del'><img /></li>";
            html += "            <li class='column_Sucursal'> Sucursal</li>";
            html += "            <li class='column_FechaT'>Fecha Z</li>";
            html += "            <li class='column_HoraT'>Hora Cierre Z</li>";
            html += "            <li class='column_FolioT'>Folio Z</li>";
            html += "            <li class='column_Caja'>Num. Caja</li>";
            html += "            <li class='column_NT'>Num. Transacc</li>";
            html += "            <li class='column_Cajero'>Cajero(s)</li>";
            html += "            <li class='column_ImpEfectivo'>Efectivo(cash)</li>";
            html += "            <li class='column_ImpTcredito'>T.Credito(Card)</li>";
            html += "            <li class='column_ImpTdebito'>T.Debito(Card)</li>";
            html += "            <li class='column_OFPago'>Otra Forma de Pago</li>";
            html += "            <li class='column_Importe'>Importe(Grss)</li>";
            html += "             <li class='column_sttReg' ></li>	";
            html += "         </ul>";
            html += "     </li> ";

            foreach (CorteZ crt in CrtResp.CorteZs)
            {


              


                count++;
                html += "<li class='row'><ul >";
                html += "<li class='column_stt'><input value='ST' /></li>";
                html += "<li class='column_CodeRegZ'><input /></li>";
                html += "<li class='row_ID' ><input value='ST'/></li>";
                html += "<li class='column_ID'>" + count + "</li>";
                html += "<li class='column_Edit column_fillColor'><img class='btnEdit' src='iconos/checkbox_unchecked (1).png' title='Editar' onclick='Editar(event);' registro'/></li>";
                html += "<li class='column_Del column_fillColor'><img class='btnDel' src='iconos/checkbox_unchecked (1).png' title='Eliminar registro' onclick='Eliminar(event);'/></li>";
                html += "<li class='column_Sucursal'>";
                html += "<select id='select_sucursal' class='inactive' autofocus disabled>";
                
                foreach (Sucursal s in lstSuc) //respSuc.Sucursales)
                {
                    if (s.SucursalID == crt.Sucursal.SucursalID)
                    {
                        selec = "selected";
                    }
                    html += "<option " + selec + " value='" + s.SucursalID + "'>" + s.SucursalID.ToString() + "-" + s.Nombre + "</option>";
                    selec = "";
                }
                


                html += "</select></li>";
                html += "<li class='column_FechaT' ><input value='" + crt.FechaVta.ToString("dd/MM/yyyy") + "' class='inactive' readonly/></li>";
                html += "<li class='column_HoraT'><input value='" + crt.Hora + "' placeholder='HH:MM' onkeyup='valida(event);'  onkeypress='return justNumbers(event);' maxlength='5' class='inactive' readonly/></li>";
                html += "<li class='column_FolioT'><input value='" + crt.TicketID + "' onkeypress='return justNumbers(event);' class='inactive' readonly/></li>";
                html += "<li class='column_Caja'><input value='" + crt.CajaID + "' onkeypress='return justNumbers(event);'  class='inactive' readonly/></li>";
                html += "<li class='column_NT'><input value='" + crt.Transacciones + "' onkeypress='return justNumbers(event);' class='inactive' readonly/></li>";
                html += "<li class='column_Cajero'><input value='" + crt.Cajeros + "'  class='inactive' readonly/></li>";
                html += "<li class='column_ImpEfectivo'><input value='" +crt.Efectivo.ToString("C")  + "' onkeypress='return justNumbers(event);' onkeyup='verificar(event);'  class='inactive' readonly/></li>";
                html += "<li class='column_ImpTcredito'><input value='" +  crt.TCredito.ToString("C") + "' onkeypress='return justNumbers(event);' onkeyup='verificar(event);' class='inactive' readonly/></li>";
                html += "<li class='column_ImpTdebito'><input value='" + crt.TDebito.ToString("C")  + "' onkeypress='return justNumbers(event);' onkeyup='verificar(event);' class='inactive' readonly/></li>";
                html += "<li class='column_OFPago'><input value='"+ crt.OtraFormaPago.ToString("C") + "' onkeypress='return justNumbers(event);' onkeyup='verificar(event);'  class='inactive' readonly/></li>";
                html += "<li class='column_Importe'><input value='"+crt.Total.ToString("C") + "' class='inactive' readonly/></li><li class='column_sttReg'><img onclick='Add_Z(event);' /></li> <li class='column_codUser'><input value='" + crt.CodUsAlta + "'/></li></ul></li> ";

                _totEfec += crt.Efectivo;
                _totTCredit += crt.TCredito;
                _totTDebito += crt.TDebito;
                _otrFPag += crt.OtraFormaPago;
                _totImp += crt.Total;


            }

            html += "<li class='Rowfooter'>";
            html += "       <ul >";
            html += "         <li class='column_stt' style='color:white;'></li>";
            html += "         <li class='column_CodeRegZ' style='color:white;'></li>";
            html += "         <li class='row_ID' style='color:white;'></li>";
            html += "         <li class='column_ID' style='color:white;'></li>";
            html += "          <li class='column_Edit' style='background-color:white;' ><img /></li>";
            html += "           <li class='column_Del' style='background-color:white;'><img /></li>";
            html += "          <li class='column_Sucursal'></li>";
            html += "          <li class='column_FechaT' ></li>";
            html += "           <li class='column_HoraT'></li>";
            html += "           <li class='column_FolioT'></li>";
            html += "           <li class='column_Caja'></li>";
            html += "           <li class='column_NT'></li>";
            html += "           <li class='column_Cajero' style='font-weight:bold;'>Sub Totales</li>";
            html += "          <li class='column_ImpEfectivo'><input value='" + _totEfec.ToString("C") + "'  readonly/></li>";
            html += "          <li class='column_ImpTcredito'><input value='" +  _totTCredit.ToString("C") + "' readonly/></li>";
            html += "          <li class='column_ImpTdebito'><input value='" + _totTDebito.ToString("C") + "' readonly/></li>";
            html += "          <li class='column_OFPago'><input value='" +  _otrFPag.ToString("C") + "' readonly/></li>";
            html += "          <li class='column_Importe'><input value='" + _totImp.ToString("C") + "' readonly/></li>";
            html += "          <li class='column_sttReg'  ></li>";
            html += "          </ul>";
            html += "       </li> ";

            return html;// +"</ul>";
        }

        public static string GetAcumulado(string fecha)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            //DateTime fecha = DateTime.Today;


            var su = new Sucursal();
            var sv = new ServiceImplementation();
            var sr = new SucursalRequest();
            su.SucursalID = 0;

            sr.Sucursal = su;
            sr.UserIDRqst = Convert.ToInt32(_usuarioID);
            sr.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Report; //BackOffice.Common.Enums.MessageOperationType.Report;
            var respSuc = sv.SucursalMessage(sr);


            //var CrtFilter = new CorteZFilter();
            var CrtSucFilter = new CorteSucursalFilter();


            sv = new ServiceImplementation();

            CrtSucFilter.FechaVta = DateTime.Parse(fecha.ToString(), culture);
            CrtSucFilter.FechaVtaHasta = DateTime.Parse(fecha.ToString(), culture);

            //CorteZResponse CrtResp = sv.CorteZMessage(new CorteZRequest() { Filters = CrtFilter, MessageOperationType = MessageOperationType.Report });
            CorteSucursalResponse CrtSucResp = sv.CorteSucursalMessage(new CorteSucursalRequest() { Filters = CrtSucFilter,UserIDRqst=Convert.ToInt32(_usuarioID),ReturnXML=true, MessageOperationType = MessageOperationType.Report });
            xmlA = new XmlDocument();
            xmlA.InnerXml = CrtSucResp.XML;
            /*
            DataSet dt2 = new DataSet();
           // dt2.ReadXml(new StringReader( CrtSucResp.XML));
            XmlDocument xdes=new XmlDocument();
            xdes.InnerXml=CrtSucResp.XML;

            XmlNode xde = xml["ArrayOfCorteZ"];
            XmlElement xmor = (XmlElement)xdes["ArrayOfCorteSucursal"].ChildNodes[0];
            XmlNode xmlnodes = xdes.ImportNode(xmor, true);
            xde.AppendChild(xmlnodes);

            var x = xml;
          
            //xml.InnerXml = dtxml.GetXml();
            /*

            try
            {
                XmlTextReader xmlreader1 = new XmlTextReader("C:\\Books2.xml");
                XmlTextReader xmlreader2 = new XmlTextReader("C:\\Books1.xml");

                DataSet ds = new DataSet();
                ds.ReadXml(xmlreader1);
                DataSet ds2 = new DataSet();
                ds2.ReadXml(xmlreader2);
                ds.Merge(ds2) ;
                ds.WriteXml("C:\\Books.xml");
                Console.WriteLine("Completed merging XML documents");
            }
            catch (System.Exception ex)
            {
                Console.Write(ex.Message);
            }
            Console.Read();	

            */
















            var selec = "";
            var count = 0;

            double _totEfecP = 0;
            double _totEfecD = 0;
            double _totBP = 0;
            double _totBD = 0;
            double _totTCredit = 0;
            double _totTDebito = 0;
            double _otrFPag = 0;
            double _totImp = 0;
            double _totIng = 0;

            var html = "";
           // html += "  <span class='CSSHeader'>Daily Cash Summary</span> <ul runat='server' id='container_history1' >";
            html += "                         <li>";
            html += "                           <ul>";
    
            html += "                                <li class='column_ID'>No.</li>";
            html += "                               <li class='column_Check'><img   /></li>";
            html += "                               <li class='column_Edit'><img class='btnEdit' src='iconos/ic_edit1.png'  title='Editar registro' /></li>";
            html += "                             <li class='column_Sucursal'>Sucursal</li>";
            html += "                              <li class='column_Supervisor' >Supervisor</li>";
            html += "                              <li class='column_FechaDaily' >Fecha Daily</li>";
            html += "                             <li class='column_NumZ'>No.Z</li>";
            html += "                             <li class='column_TotalIng'>Total Ingresos</li>";


            /*add columnas*/
            html += "                             <li class='column_ImpTcredito'>T.Credito</li>";
            html += "                             <li class='column_ImpTdebito'>T.Debito</li>";
            html += "                             <li class='column_OFPago'>Otra Forma de Pago</li>";

            html += "                            <li class='column_TotalEfectivo'>Total Efectivo Daily </li>";
            html += "                            <li class='column_TotalEfectZ'>Total Efectivo Z </li>";
            html += "                           <li class='column_VariacionDayli'>Variacion Daily(Debe ser Cero)</li>";

            html += "                                     <li  class='column_TipoCambio'>T.C.</li>";
            html += "                             <li class='column_EfecDeposito'>Efectivo para Deposito";
            html += "                                 <ul >";
            html += "                                     <li  class='column_EfectivoDepP'>Pesos</li>";
            html += "                                     <li  class='column_EfectivoDepD'>Dolares</li>";
          
            html += "                                     <li  class='column_EfectivoDepPConv'>Conversion Pesos</li>";
            html += "                                </ul>";
            html += "                           </li>";
        
            html += "                           <li class='column_BosalDeposito'>Bolsa(s) Servicio Blindado";
            html += "                              <ul>";
            html += "                                    <li class='column_BolsaP'>Pesos</li>";
            html += "                                    <li class='column_BolsaD'>Dolares</li>";
         
            html += "                                    <li class='column_BolsaPConv'>Conversion Pesos</li>";
            html += "                                </ul>";
            html += "                           </li>";
            html += "                           <li class='column_FolioServices'>Folio Services</li>";
            html += "                           <li class='column_GastoDeu'>Gastos(a deudores)</li>";
            html += "                            <li class='column_Sob'>Sobrantes</li>";
            html += "                            <li class='column_CajeroCorto'>Cajero Corto</li>";
            html += "                            <li class='column_Falt'>Faltantes</li>";

       
            html += "                            <li class='column_ComentariosDayli'><img src='iconos/msgNew.png' /></li>";
    
            html += "                     </ul>";
            html += "                  </li> ";




            foreach (CorteSucursal crtS in CrtSucResp.CorteSucursales)
            {
                double _totEfectivo = 0;
                double _variacion = 0;






                var _stt = crtS.Stt != "" ? crtS.Stt.Trim().ToUpper() : "";


                var disabled = "";
                var style = "";


                if (_stt == "V" || _stt == "T") { disabled = "disabled='disabled'"; }

                _totEfectivo = crtS.PesosADeposito + ConvertMoneda(crtS.DolarADeposito, crtS.TC, 1) + crtS.PesosSB + ConvertMoneda(crtS.DolarSB, crtS.TC, 1);

                _variacion = CalcVariacion(_totEfectivo, crtS.EfectivoZ, crtS.Gastos, crtS.Sobrante, crtS.Faltante);

                if (_variacion < 0) {
                    style = "style='color:red;'";
                }
                count++;
                html += "<li class='row'>";
                html += "<ul>";
                html += "<li class='column_stt' ><input value='ST' /></li>";
                html += "<li class='column_CodeRegZ'><input /></li>";
                html += "<li class='row_ID' ><input value='ST'/></li>";
                html += "<li class='column_ID'>" + count + "</li>";
                html += "<li class='column_Check'><input class='STT" + _stt + "' value='"+_stt+"'  disabled='disabled' readonly /></li>";
                html += "<li class='column_Edit column_fillColor'><img class='btnEdit'  src='iconos/checkbox_unchecked (1).png' onclick='EditarDaily(event);' title='Editar registro' " + disabled + " /></li>";
                html += "<li class='column_Sucursal'>";
                html += "<select id='select_sucursal2' disabled=true class='inactive' autofocus>";

                foreach (Sucursal s in lstSuc) // respSuc.Sucursales)
                {
                    if (s.SucursalID == crtS.Sucursal.SucursalID)
                    {
                        selec = "selected";
                    }
                    html += "<option " + selec + " value='" + s.SucursalID + "'>" + s.SucursalID.ToString() + "-" + s.Nombre + "</option>";
                    selec = "";
                }

                html += "</select>";
                html += "</li>";
                html += "<li class='column_Supervisor' ><input value='" + crtS.Supervisor + "' class='inactive'  readonly/></li>";
                html += "<li class='column_FechaDaily' ><input value='" + crtS.FechaVta.ToString("dd/MM/yyyy") + "' class='inactive'  readonly/></li>";
                html += "<li class='column_NumZ'><input value='" + crtS.NoZ + "' class='inactive'  readonly/></li>";
                html += "<li class='column_TotalIng'><input value='" + crtS.Total.ToString("C") + "'class='inactive'  readonly/></li>";

                /*ad columnas*/
                html += "<li class='column_ImpTcredito' ><input value='"+crtS.TotalTCredito.ToString("C") + "' class='inactive'  readonly/></li>";
                html += "<li class='column_ImpTdebito' ><input value='" + crtS.TotalTDebito.ToString("C") + "' class='inactive'  readonly/></li>";
               
                
                html += "<li class='column_OFPago' ><input value='" + crtS.TotalOtraFormaPago.ToString("C") + "' class='inactive'  readonly/></li>";

                html += "<li class='column_TotalEfectivo'><input value='" + _totEfectivo.ToString("C") + "' class='inactive'  readonly/></li>";
                html += "<li class='column_TotalEfectZ' style='background-color: lightblue;'><input value='" + crtS.EfectivoZ.ToString("C") + "' class='inactive'  readonly/></li>";
                html += "<li class='column_VariacionDayli'><input " + style + " value='" + _variacion.ToString("C") + "' class='inactive'  readonly/></li>";

                html += "<li class='column_TipoCambio'><input value='" +  crtS.TC.ToString("C") + "' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);' class='inactive'  readonly/></li>";

                html += "<li class='column_EfectivoDepP' style='background-color: lightblue;'><input value='" +  crtS.PesosADeposito.ToString("C") + "' onkeypress='return justNumbers(event);'  onkeyup='CalcSummary(event);' class='inactive'  readonly/></li>";
                html += "<li class='column_EfectivoDepD' ><input value='" +  crtS.DolarADeposito.ToString("C") + "' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);' class='inactive'  readonly/></li>";

                html += "<li class='column_EfectivoDepPConv' style='background-color: lightblue;'><input value='" + ConvertMoneda(crtS.DolarADeposito, crtS.TC, 1).ToString("C") + "' class='inactive'  readonly/></li>";
                //html += "<li class='column_FolioServices'><input value='0' /></li>";
                html += "<li class='column_BolsaP' style='background-color: lightblue;'><input  value='" +  crtS.PesosSB.ToString("C") + "' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);' class='inactive'  readonly/></li>";
                html += "<li class='column_BolsaD'><input  value='" +  crtS.DolarSB.ToString("C") + "' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);' class='inactive'  readonly/></li>";
                // html += "<li class='column_TipoCambio'><input  value='$" + String.Format(CultureInfo.InvariantCulture, "{0:#,0.00}", crtS.TC) + "' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);' class='inactive'  readonly/></li>";
                html += "<li class='column_BolsaPConv' style='background-color: lightblue;'><input  value='" + ConvertMoneda(crtS.DolarSB, crtS.TC, 1).ToString("C") + "' class='inactive'  readonly/></li>";


                html += "<li class='column_FolioServices'><input value='" + crtS.FolioFactura + "' class='inactive' /></li>";


                html += "<li class='column_GastoDeu' style='background-color: lightblue;'><input  value='" +  crtS.Gastos.ToString("C") + "' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);' class='inactive'  readonly/></li>";
                html += "<li class='column_Sob' style='background-color: lightblue;'><input  value='" +  crtS.Sobrante.ToString("C") + "' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);' class='inactive'  readonly/></li>";
                html += "<li class='column_CajeroCorto'><input  value='"+crtS.CajeroCorto + "' class='inactive'  readonly/></li>";
                
                html += "<li class='column_Falt' style='background-color: lightblue;'><input  value='" +  crtS.Faltante.ToString("C") + "' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);' class='inactive'  readonly/></li>";

                //html += "<li class='column_TotalEfectZ' style='background-color: lightblue;'><input value='$" + String.Format(CultureInfo.InvariantCulture, "{0:#,0.00}", crtS.EfectivoZ) + "' class='inactive'  readonly/></li>";
                //html += "<li class='column_VariacionDayli'><input value='0' class='inactive'  readonly/></li>";
                html += "<li class='column_ComentariosDayli'><img src='iconos/msgNew.png' onclick='VerComent(event);'/></li><li class='column_codUser'><input  value='" + crtS.CodUsAlta + "' /></li><li class='column_coment'><input value='" + crtS.Comentarios + "' /></li> </ul>";


                _totIng += crtS.Total;

                _totTCredit += crtS.TotalTCredito;
                _totTDebito += crtS.TotalTDebito;
                _otrFPag += crtS.TotalOtraFormaPago;
                _totEfecP += crtS.PesosADeposito;
                _totEfecD += crtS.DolarADeposito;
                _totBP += crtS.PesosSB;
                _totBD += crtS.DolarSB;

                _totImp += crtS.EfectivoZ;





            }

            html += " <li class='Rowfooter'>";
            html += "                     <ul>";
            html += "                         <li class='column_stt' style='color:white;'></li>";
            html += "                         <li class='column_CodeRegZ' style='color:white;'></li>";
            html += "                         <li class='row_ID' style='color:white;'></li>";
            html += "                         <li class='column_ID' style='color:white;'></li>";
            html += "                        <li class='column_Check' style='background-color:white;'></li>";
            html += "                        <li class='column_Edit' style='background-color:white;'></li>";
            //html += "
            html += "                   <li class='column_Sucursal' style='font-weight:bold;'> Total Ingreso del dia. </li>";
            //  html += "                      Total Ingreso del dia.
            // html += "                   </li>
            html += "                   <li class='column_Supervisor' ></li>";
            html += "                   <li class='column_FechaDaily' ></li>";
            html += "                   <li class='column_NumZ'></li>";
            html += "                   <li class='column_TotalIng'><input value='" +  _totIng.ToString("C") + "'  readonly/></li>";
            html += "                   <li class='column_ImpTcredito'><input value='" +  _totTCredit.ToString("C") + "'  readonly/></li>";
            html += "                   <li class='column_ImpTdebito'><input value='" +_totTDebito.ToString("C")  + "'  readonly/></li>";
            html += "                   <li class='column_OFPago'><input  value='" +  _otrFPag.ToString("C") + "' readonly/></li>";

            html += "               <li class='column_TotalEfectivo'></li>";
            html += "               <li class='column_TotalEfectZ'><input value='" +  _totImp.ToString("C") + "'  readonly/></li>";
            html += "               <li class='column_VariacionDayli'></li>";

            html += "                   <li class='column_TipoCambio'></li>";
            //  html += "                  
            html += "                    <li class='column_EfectivoDepP'><input value='" +  _totEfecP.ToString("C") + "'  readonly/></li>";
            html += "                   <li class='column_EfectivoDepD'><input  value='" +  _totEfecD.ToString("C") + "' readonly/></li>";
          
            html += "                   <li class='column_EfectivoDepPConv'></li>";
            //html += "                   <li class='column_FolioServices'></li>";
            html += "                   <li class='column_BolsaP'><input  value='" +  _totBP.ToString("C") + "' readonly/></li>";
            html += "                   <li class='column_BolsaD'><input  value='" + _totBD.ToString("C") + "' readonly/></li>";
            //html += "                   <li class='column_TipoCambio'></li>";
            html += "                   <li class='column_BolsaPConv'></li>";
            html += "                   <li class='column_FolioServices'></li>";
            // html += "
            html += "                <li class='column_GastoDeu'></li>";
            html += "                <li class='column_Sob'></li>";
            html += "                <li class='column_CajeroCorto'></li>";
            html += "                <li class='column_Falt'></li>";

           // html += "               <li class='column_TotalEfectZ'><input value='$" + String.Format(CultureInfo.InvariantCulture, "{0:#,0.00}", _totImp) + "'  readonly/></li>";
           // html += "               <li class='column_VariacionDayli'></li>";
            html += "              <li class='column_ComentariosDayli'></li>";

            html += "         </ul>";
            html += "     </li> ";



            return html;// + "</ul>";
        }
        public static List<CboTipo> GetSTT(DateTime fecha, int sucursalID)
        {

            var sv = new ServiceImplementation();
            var _DalCrtSucursal = new CorteSucursalDAL(DBName);
            List<int> lstID = new List<int>();
            lstID.Add(sucursalID);

            var result = _DalCrtSucursal.GetCorteSucursalStt(fecha, lstID);

            return result;

           // DateTime _fecha;
           // _fecha = DateTime.Parse(fecha, culture);
         //   DataRow dr = Conexion.LeerRegistro("select Stt from LittleCaesarDev.dbo.CorteSucursal where FechaVta='" + fecha.ToShortDateString() + "' and SucursalID=" + sucursalID);
           // return dr["Stt"].ToString();
        }
        [WebMethod]
        public static string SaveZ(String[] DatosZ)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha = DateTime.Today;

            string msg = string.Empty;

            var CorteZ = new CorteZ();
            var CorteDAL = new CorteZDAL();

            var CrtRequest = new CorteZRequest();
            var sv = new ServiceImplementation();

            var CrtSucursal = new CorteSucursal();
            var CrtDalSucursal = new CorteSucursalDAL(DBName);



            var TypeMsg = 0;
            try
            {

                //  NumberStyles _NumberStyle = NumberStyles.AllowDecimalPoint;
                //  IFormatProvider _FormatProvider=System.Globalization.NumberFormatInfo.InvariantInfo;

                //double _monto;

                var _codeTemp = DatosZ[0].Split(new char[] { '|' }); /*sucursalid;caja;ticketid;fechavta*/
                var _rowEstatus = DatosZ[1];
                var _sucursalID = DatosZ[2] != "" ? Convert.ToInt32(DatosZ[2]) : 0;
                var _fechaZ = DateTime.Parse(DatosZ[3], culture);
                var _horaZ = DatosZ[4];
                var _folioZ = Convert.ToInt32(DatosZ[5]) ;
                var _cajaID = Convert.ToInt32(DatosZ[6]);
                var _NT = Convert.ToInt32(DatosZ[7]);
                var _cajero = DatosZ[8];
                var _efectivo = StrToDouble(DatosZ[9], culture);//, _NumberStyle, _FormatProvider);
                var _TCredit = StrToDouble(DatosZ[10], culture);//, _NumberStyle, _FormatProvider);
                var _oFpag = StrToDouble(DatosZ[11], culture);//, _NumberStyle, _FormatProvider);
                // _monto = Double.Parse(DatosZ[12] != "" ? DatosZ[12].ToString().Trim() : "0", NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, culture); //, @"^\d+$").Value;
                // var _total = Double.Parse(_monto);//, _NumberStyle, _FormatProvider);
                var _codUser = Convert.ToInt32(DatosZ[13]);

                /**/

                var _STT = DatosZ[24] != "" ? DatosZ[24].ToString().ToUpper() : "";

               var _SttLast = GetSTT(_fechaZ, _sucursalID);

               if (_SttLast != null && _SttLast.Count > 0)
               {

                   if ((_STT != _SttLast[0].Nombre.ToString().ToUpper()) && _SttLast[0].Nombre.ToString().ToUpper() == "V" || _SttLast[0].Nombre.ToString().ToUpper() == "T")
                   {
                       TypeMsg = 1;
                       msg = "Es necesario ejecutar el reporte nuevamente , el daily para el codigo sucursal ( " + _sucursalID + " ) fue modificado Validado o Terminado.";
                   }


               }


                var _efecDepP = StrToDouble(DatosZ[14], culture);//, _NumberStyle, _FormatProvider);
                var _efecDepD = StrToDouble(DatosZ[15], culture);//, _NumberStyle, _FormatProvider);

                var _bolsaP = StrToDouble(DatosZ[16], culture);//, _NumberStyle, _FormatProvider);
                var _bolsaD = StrToDouble(DatosZ[17], culture);//, _NumberStyle, _FormatProvider);
                var _folioServices = DatosZ[18];
                var _gasto = StrToDouble(DatosZ[19], culture);//, _NumberStyle, _FormatProvider);
                var _sob = StrToDouble(DatosZ[20], culture);//, _NumberStyle, _FormatProvider);

                var _tipoCambio = StrToDouble(DatosZ[21], culture);//, _NumberStyle, _FormatProvider);
                var _coment = DatosZ[22] != null ? DatosZ[22] : "";
                var _falt = StrToDouble(DatosZ[23], culture);//, _NumberStyle, _FormatProvider);

                var _Supervisor = DatosZ[25] != null ? DatosZ[25] : "";
                var _CajeroCorto = DatosZ[26] != null ? DatosZ[26] : "";

                var _TDebit = StrToDouble(DatosZ[27], culture);

              



/*

                if ((_STT.ToUpper() != _SttLast[0].Nombre.ToString().ToUpper()) && _SttLast[0].ToString().ToUpper() == "V" || _SttLast[0].ToString().ToUpper() == "T")
                {

                    TypeMsg = 1;
                    msg = "Es necesario recargar los datos ,este daily ya fue modificado en la pantalla de ingresos";
                }
                else
                {
                */

                    //if (_rowEstatus != "" && _rowEstatus != "ST1")
                    //{


                    /*
                     switch (DatosZ[24])
                     {
                         case "STTR":
                             _STT = "R";
                             break;
                         case "STTV":
                             _STT = "V";
                             break;
                         case "STTT":
                             _STT = "T";
                             break;
                     }
                 */

                if (TypeMsg < 1)
                {

                    CrtSucursal.Sucursal = new Sucursal { SucursalID = _sucursalID };
                    CrtSucursal.FechaVta = _fechaZ;  //_fechaDaily;
                    CrtSucursal.PesosADeposito = _efecDepP;
                    CrtSucursal.DolarADeposito = _efecDepD;
                    CrtSucursal.TC = _tipoCambio;
                    CrtSucursal.FolioFactura = _folioServices;
                    CrtSucursal.PesosSB = _bolsaP;
                    CrtSucursal.DolarSB = _bolsaD;
                    CrtSucursal.Gastos = _gasto;
                    //  CrtSucursal.Ajuste = _sobFalt;
                    CrtSucursal.Sobrante = _sob;
                    CrtSucursal.Faltante = _falt;
                    CrtSucursal.Comentarios = _coment;
                    CrtSucursal.CodUsuario = _codUser;
                    CrtSucursal.Supervisor = _Supervisor;
                    CrtSucursal.CajeroCorto = _CajeroCorto;
                    // CrtSucursal.Stt = _STT;

                    // }








                    CorteZ.CajaID = _cajaID;
                    CorteZ.Sucursal = new Sucursal() { SucursalID = _sucursalID };
                    CorteZ.FechaVta = _fechaZ;
                    CorteZ.Hora = _horaZ;
                    CorteZ.TicketID = _folioZ;
                    CorteZ.CajaID = _cajaID;
                    CorteZ.Transacciones = _NT;
                    CorteZ.Cajeros = _cajero;
                    CorteZ.Efectivo = _efectivo;
                    CorteZ.TCredito = _TCredit;
                    CorteZ.OtraFormaPago = _oFpag;
                    CorteZ.Total = _efectivo + _TCredit + _TDebit + _oFpag; // _total;
                    CorteZ.CodUsuario = _codUser;
                    CorteZ.TDebito = _TDebit;






                    switch (_rowEstatus)
                    {
                        case "ST0":
                            CorteZ.OperationType = QSystem.Common.Enums.OperationType.New; //BackOffice.Common.Enums.OperationType.New;
                            break;
                        case "ST1"://edicion elimina la z anterior y agrga una nueva

                            var GetCortZ = new CorteZ();
                            GetCortZ.Sucursal = new Sucursal { SucursalID = Convert.ToInt32(_codeTemp[1]) };
                            GetCortZ.CajaID = Convert.ToInt32(_codeTemp[2]);
                            GetCortZ.TicketID = Convert.ToInt32(_codeTemp[0]);
                            GetCortZ.FechaVta = DateTime.Parse(_codeTemp[3].ToString(), culture);
                            GetCortZ.Total = Convert.ToDouble(_codeTemp[4]);

                            CrtRequest.CorteZ = GetCortZ;
                            CrtRequest.UserIDRqst = _codUser;
                            CrtRequest.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Query;// BackOffice.Common.Enums.MessageOperationType.Query;
                            var response = sv.CorteZMessage(CrtRequest);

                            GetCortZ = new CorteZ();
                            GetCortZ = response.CorteZ;
                            GetCortZ.OperationType = QSystem.Common.Enums.OperationType.Delete; //BackOffice.Common.Enums.OperationType.Delete;

                            var result = CorteDAL.SaveCorteZ(GetCortZ, ref msg);
                            if (result)
                            {
                                CorteZ.OperationType = QSystem.Common.Enums.OperationType.New;// BackOffice.Common.Enums.OperationType.New;
                            }

                            break;
                        case "ST2":
                            CorteZ.OperationType = QSystem.Common.Enums.OperationType.Delete; //BackOffice.Common.Enums.OperationType.Delete;
                            break;
                    }



                    var res = CorteDAL.SaveCorteZ(CorteZ, ref msg);



                    // msg = "Grabo Z";
                    msg += System.Environment.NewLine;
                    if (res)
                    {

                        CrtSucursal.OperationType = QSystem.Common.Enums.OperationType.Edit; //BackOffice.Common.Enums.OperationType.Edit;

                        var result = CrtDalSucursal.SaveCorteSucursal(CrtSucursal, ref msg);


                        //  msg += "Actualizo consolidado";
                    }
                }
            }
            catch (Exception ex)
            {
                msg =  ex.Message;
                TypeMsg = 1;
            }
            return TypeMsg + "|" + msg;
        }
        [WebMethod]
        public static string SaveSummary(String[] Summary)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha = DateTime.Today;


           // NumberStyles _NumberStyle = NumberStyles.AllowDecimalPoint;

            //IFormatProvider _FormatProvider = System.Globalization.NumberFormatInfo.InvariantInfo;

            string msg = string.Empty;
            var TypeMsg = 0;


            try
            {
                var _tipoCambio = StrToDouble(Summary[12], culture);//, _NumberStyle, _FormatProvider);
                var _codeTemp = Summary[0];
                var _rowEstatus = Summary[1];
                var _sucursalID = Convert.ToInt32(Summary[2]);
                var _fechaDaily = DateTime.Parse(Summary[3], culture); ;
                //var _numz = Convert.ToInt32(Summary[4]);
                // var _TotalIng = Double.Parse(Summary[5], _NumberStyle, _FormatProvider);
                // var _Tcredit = Double.Parse(Summary[6], _NumberStyle, _FormatProvider);
                // var _OTF = Double.Parse(Summary[7], _NumberStyle, _FormatProvider);

                var _efecDepP = StrToDouble(Summary[4] , culture);//, _NumberStyle, _FormatProvider);
                var _efecDepD = StrToDouble(Summary[5], culture);//, _NumberStyle, _FormatProvider);

                var _bolsaP = StrToDouble(Summary[6], culture);//, _NumberStyle, _FormatProvider);
                var _bolsaD = StrToDouble(Summary[7], culture);//, _NumberStyle, _FormatProvider);
                var _folioServices = Summary[8];
                var _gasto = StrToDouble(Summary[9], culture);//, _NumberStyle, _FormatProvider);
                //var _sobFalt = Double.Parse(Summary[10].Replace('-','-') , _NumberStyle, _FormatProvider);
                var _sob = StrToDouble(Summary[10], culture);//, CultureInfo.InvariantCulture);


                //var _totEfec = Double.Parse(Summary[13], _NumberStyle, _FormatProvider);
                //var _variacion = Double.Parse(Summary[14], _NumberStyle, _FormatProvider);
                var _codUser = Convert.ToInt32(Summary[11]);
                var _coment = Summary[13];
                var _falt = StrToDouble(Summary[14], culture);//, CultureInfo.InvariantCulture);

                var _STT = Summary[15] != "" ? Summary[15].ToString().ToUpper() : "";
              /*  switch (Summary[15])
                {
                    case "STTR":
                        _STT = "R";
                        break;
                    case "STTV":
                        _STT = "V";
                        break;
                    case "STTT":
                        _STT = "T";
                        break;
                }
                */
                var _Supervisor = Summary[16];
                var _CajeroCorto = Summary[17];

                var _SttLast = GetSTT(_fechaDaily, _sucursalID);

                if (_SttLast != null && _SttLast.Count > 0)
                {

                    if ((_STT != _SttLast[0].Nombre.ToString().ToUpper()) && _SttLast[0].Nombre.ToString().ToUpper() == "V" || _SttLast[0].Nombre.ToString().ToUpper() == "T")
                    {
                        TypeMsg = 1;
                       // msg = "Es necesario recargar los datos ,este daily ya fue modificado en la pantalla de ingresos";
                        msg = "Es necesario ejecutar el reporte nuevamente , el daily para el codigo sucursal ( " + _sucursalID + " ) fue modificado Validado o Terminado.";
                    }


                }




                var CrtSucursal = new CorteSucursal();
                var CrtDalSucursal = new CorteSucursalDAL(DBName);


                if (TypeMsg < 1)
                {

                    CrtSucursal.Sucursal = new Sucursal { SucursalID = _sucursalID };
                    CrtSucursal.FechaVta = _fechaDaily;
                    // CrtSucursal.NoZ = _numz;
                    //CrtSucursal.Total = _TotalIng;
                    //CrtSucursal.TotalTCredigo = _Tcredit;
                    // CrtSucursal.TotalOtraFormaPago = _OTF;
                    CrtSucursal.PesosADeposito = _efecDepP;
                    CrtSucursal.DolarADeposito = _efecDepD;
                    CrtSucursal.TC = _tipoCambio;
                    CrtSucursal.PesosSB = _bolsaP;
                    CrtSucursal.DolarSB = _bolsaD;
                    CrtSucursal.FolioFactura = _folioServices;
                    CrtSucursal.Gastos = _gasto;
                    // CrtSucursal.Ajuste = _sobFalt;
                    CrtSucursal.Sobrante = _sob;
                    CrtSucursal.Faltante = _falt;
                    CrtSucursal.Comentarios = _coment;
                    CrtSucursal.CodUsuario = _codUser;
                    CrtSucursal.Supervisor = _Supervisor;
                    CrtSucursal.CajeroCorto = _CajeroCorto;
                    CrtSucursal.Stt = _STT;



                    /*
                    switch (_rowEstatus)
                    {
                        case "ST0":
                            CrtSucursal.OperationType = BackOffice.Common.Enums.OperationType.New;
                            break;
                        case "ST1":
                            CrtSucursal.OperationType = BackOffice.Common.Enums.OperationType.Edit;
                            break;
                    }
                    */
                    CrtSucursal.OperationType = QSystem.Common.Enums.OperationType.Edit; //BackOffice.Common.Enums.OperationType.Edit;

                    var result = CrtDalSucursal.SaveCorteSucursal(CrtSucursal, ref msg);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                TypeMsg = 1;
            }
            return TypeMsg + "|" + msg;

        }
        protected static Double CalcVariacion(double efectivo,double efectivoDaily, double gastos,double sobrante,double faltante){

            double _resultTot = 0;
           // var result = 0;
            double _variacion = 0;

          //  result=Datos.EfectivoP + Convertidor(Datos.EfectivoD,Datos.TC,1) + Datos.BolsaP + Convertidor(Datos.BolsaD,Datos.TC,1) + Datos.Gastos;
            _variacion = (efectivo + gastos) - efectivoDaily;
            
            
            if (sobrante > 0)
            {
                _resultTot = _variacion - sobrante;
                _resultTot = _resultTot + faltante;
            }
            else
            {
                _resultTot = _variacion + faltante;
            }

            return _resultTot;
        }
        protected static Double ConvertMoneda(double monto, double tipoCambio, int conversion)
        {
            Double result = 0.0;
            if (conversion == 0)
            {
                result = (monto / tipoCambio);//convetrir dolares
            }
            if (conversion == 1)
            {
                result = (monto * tipoCambio);//convertoir pesos
            }
            return result;

        }
        protected static Double StrToDouble(string value, CultureInfo cultura)
        {
            string ms = string.Empty;
            // _monto = Double.Parse(DatosZ[12] != "" ? DatosZ[12].ToString().Trim() : "0", NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, culture); //, @"^\d+$").Value;
            Double result = 0;
            try
            {
                value = value != "" && value != null ? value.Trim().Replace("$", "") : "0";
                result = Double.Parse(value, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, cultura);
            }
            catch (Exception ex)
            {
                //ms += System.Environment.NewLine;
                ms += " " + value + " : " + ex.Message;
                throw new Exception(ms);

            }

            return result;
            //, @"^\d+$").Value;
        }
        protected void ClFechaCap_SelectionChanged1(object sender, EventArgs e)
        {

        }

        protected void ClFechaCap_SelectionChanged2(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha = DateTime.Today;
            if (ClFechaCap.SelectedDate <= fecha)
            {
                //txtFecha.Text = ClFechaCap.SelectedDate.ToShortDateString();
                ClFechaCap.Visible = false;
                /* content_repT.InnerHtml = GetTickets(ClFechaCap.SelectedDate);*/

            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void btnFecha_Click1(object sender, EventArgs e)
        {
            if (ClFechaCap.Visible)
            {
                ClFechaCap.Visible = false;
            }
            else
            {
                ClFechaCap.Visible = true;
            }
        }

        protected void ClFechaCap_SelectionChanged3(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha = DateTime.Today;


            if (ClFechaCap.SelectedDate <= fecha)
            {
                txtFecha.Value = ClFechaCap.SelectedDate.ToShortDateString();
            }

            clear();
          
            ClFechaCap.Visible = false;
        }

        protected void btnChangeFecha_Click(object sender, EventArgs e)
        {
            clear();
        }

        protected bool down(string _rutaxsl, string rutaxml, XmlDocument xml)
        {
            var result = false;
            XslCompiledTransform xslt = new XslCompiledTransform();
            if (xmlA != null)
            {


                xslt.Load(_rutaxsl);

                StringWriter XMLOUT = new StringWriter();
                xslt.Transform(xml.CreateNavigator(), null, XMLOUT);

                using (FileStream file = new FileStream(rutaxml, FileMode.Create, FileAccess.ReadWrite))
                {
                    var write = new StreamWriter(file);
                    write.Write(XMLOUT.ToString());
                    write.Close();
                }


                var fileInf = new FileInfo(rutaxml);

                if (fileInf.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileInf.Name);
                    Response.AddHeader("Content-Length", fileInf.Length.ToString());
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.TransmitFile(fileInf.FullName);

                    // Response.WriteFile(fileInf.FullName);
                    Response.End();
                    result = true;
                }


            }
            return result;

        }


        protected void btnDownload_Click(object sender, EventArgs e)
        {







            string FechaActual = DateTime.Now.Day.ToString("00") + "" + DateTime.Now.Month.ToString("00") + "" + DateTime.Now.Year;

            XmlDocument newxml = new XmlDocument();
            var _rutaXML = "";//AppDomain.CurrentDomain.BaseDirectory + "XMLDownloads\\CorteZ" + FechaActual + "_" + _usuarioID + ".xml";
           // var _rutaXMLA = ""; //AppDomain.CurrentDomain.BaseDirectory + "XMLDownloads\\CorteZAcumulado" + FechaActual + "_" + _usuarioID + ".xml";
            var _rutaXSL = "";//AppDomain.CurrentDomain.BaseDirectory + "XMLDownloads\\stylesheet_crtz.xsl";
           // var _rutaXSLA = "";// AppDomain.CurrentDomain.BaseDirectory + "XMLDownloads\\stylesheet_crtSU.xsl";

            for (var x = 0; x < 2; x++) {
                if (x == 0) {
                    _rutaXML = AppDomain.CurrentDomain.BaseDirectory + "XMLDownloads\\CorteZ" + FechaActual + "_" + _usuarioID + ".xml";
                    _rutaXSL = AppDomain.CurrentDomain.BaseDirectory + "XMLDownloads\\stylesheet_crtz.xsl";
                    newxml = xml;
                }
                if (x == 1) {
                    _rutaXML = AppDomain.CurrentDomain.BaseDirectory + "XMLDownloads\\CorteZAcumulado" + FechaActual + "_" + _usuarioID + ".xml";
                    _rutaXSL = AppDomain.CurrentDomain.BaseDirectory + "XMLDownloads\\stylesheet_crtSU.xsl";
                    newxml = xmlA;
                }

                var result = down(_rutaXSL , _rutaXML, newxml);
                
            }



            /*

           // xml = new XmlDocument();
            XslCompiledTransform xslt = new XslCompiledTransform();
            XslCompiledTransform xsltA = new XslCompiledTransform();

            
            //if (xml != null)
           // {


                xslt.Load(_rutaXSL);
                xsltA.Load(_rutaXSLA);

                StringWriter XMLOUT = new StringWriter();
                xslt.Transform(xml.CreateNavigator(), null, XMLOUT);

                StringWriter XMLOUTA = new StringWriter();
                xsltA.Transform(xmlA.CreateNavigator(), null, XMLOUTA);

                using (FileStream file = new FileStream(_rutaXML, FileMode.Create, FileAccess.ReadWrite))
                {
                    var write = new StreamWriter(file);
                    write.Write(XMLOUT.ToString());
                    write.Close();
                }

                using (FileStream file = new FileStream(_rutaXMLA, FileMode.Create, FileAccess.ReadWrite))
                {
                    var write = new StreamWriter(file);
                    write.Write(XMLOUTA.ToString());
                    write.Close();
                }



                var fileInf = new FileInfo(_rutaXML);

                if (fileInf.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileInf.Name);
                    Response.AddHeader("Content-Length", fileInf.Length.ToString());
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.TransmitFile(fileInf.FullName);

                    // Response.WriteFile(fileInf.FullName);
                    Response.End();

                }

              fileInf = new FileInfo(_rutaXMLA);

                if (fileInf.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileInf.Name);
                    Response.AddHeader("Content-Length", fileInf.Length.ToString());
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.TransmitFile(fileInf.FullName);

                    // Response.WriteFile(fileInf.FullName);
                    Response.End();

                }

                
          //  }
       /*
            if (xmlA != null)
            {


                xslt.Load(_rutaXSLA);

                StringWriter XMLOUT = new StringWriter();
                xslt.Transform(xmlA.CreateNavigator(), null, XMLOUT);

                using (FileStream file = new FileStream(_rutaXMLA, FileMode.Create, FileAccess.ReadWrite))
                {
                    var write = new StreamWriter(file);
                    write.Write(XMLOUT.ToString());
                    write.Close();
                }


                var fileInf = new FileInfo(_rutaXMLA);

                if (fileInf.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileInf.Name);
                    Response.AddHeader("Content-Length", fileInf.Length.ToString());
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.TransmitFile(fileInf.FullName);

                    // Response.WriteFile(fileInf.FullName);
                    Response.End();

                }


            }*/

        }



    }
}