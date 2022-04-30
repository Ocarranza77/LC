using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Security;
using System.Globalization;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.Messages;
using QSG.LittleCaesars.BackOffice.Messages.Requests;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.LittleCaesars.BackOffice.Messages.Response;
using QSG.LittleCaesars.BackOffice.Common.Enums;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using System.Threading;
using System.Text.RegularExpressions;
using QSG.QSystem.Common.Enums;
using QSG.QSystem.Common.Entities;
using QSG.QSystem.Messages;
using QSG.QSystem.Messages.Requests;
using QSG.QSystem.Messages.Response;

namespace QSG.LittleCaesars.Portal.Web
{
    public partial class WebForm2 : System.Web.UI.Page
    {
       public static CultureInfo culture = new CultureInfo("es-MX", true);
       public static string _usuarioID;
       public static List<Sucursal> lstSuc;
       public static List<CboTipo> lstMetodoPagoSAT;
       public static string NombrePantalla;
       public static string Titulo;

       public static string r_logo;
       public static string _user;


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
                     txtFecha.Value = fecha2.ToShortDateString();
                     NombrePantalla = "Captura de Tickets";
                     Titulo = Session["Empresa"].ToString() + " | " + NombrePantalla;
                     _user = Session["Nombre"].ToString();
                     _usuarioID = Session["User"].ToString();
                     User_id.Value = _usuarioID;
                     txtUsuario.Value = _user;
                     GetCBOIni();
                     
                     
                 }
             }
             else
             {
                 Session["User"] = null;
                 Response.Redirect("~/Account/Login.aspx", true);
             }


            /*
            //Session["pantalla"] = 1;
            if (Session["User"] != null)
            {
                var url = Page.Request.Url.LocalPath.Split(new char[]{'/','.'});
                var nomp = url[1];
               

                    ruta_app.Value = Session["Empresa"].ToString() + " | " + "Captura de Tickets";
                    NickName.InnerText = Session["Nombre"].ToString();
                    // fechaCapt.text = fecha.Day.ToString() + "/" + fecha.Month.ToString() + "/" + fecha.Year.ToString();
                    usuario.Value = Session["Nombre"].ToString();
                    User_id.Value = Session["User"].ToString();
                    //content_repT.InnerHtml = GetTickets(fechaCapt.Value);

                    if (!IsPostBack)
                    {
                       
                        _usuarioID = Session["User"] != null ? Convert.ToInt32(Session["User"].ToString()) : 0;
                        GetSuc();
                        ClFechaCap.SelectedDate = fecha2;


                        BtnFechaCap.Text = fecha2.ToShortDateString(); //DateTime.Now.ToShortDateString();
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    }
              

            }
            else
            {
                Session["User"] = null;
                Response.Redirect("~/Account/Login.aspx", true);
            }
             * */
        }

        private void GetCBOIni()
        {
            GetSuc();
            GetMPSAT();
        }

        private void GetMPSAT()
        {
            var sv = new ServiceQbic();

            var response = sv.MetodoPagoSATMessage(new MetodoPagoSATRequest() { 
                BDName = Session["DBName"].ToString(), 
                UserIDRqst = Convert.ToInt32(Session["User"].ToString()),
                MessageOperationType = MessageOperationType.Report,
                CboIni = true
            });

            lstMetodoPagoSAT = response.ListCboTipo;
        }

        protected void btnFechaCaP_click(object sender, EventArgs e)
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
            Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha2 = DateTime.Today;

            if (ClFechaCap.SelectedDate <= fecha2)
            {
                txtFecha.Value = ClFechaCap.SelectedDate.ToShortDateString();
                //BtnFechaCap.Text = ClFechaCap.SelectedDate.ToShortDateString();
                ClFechaCap.Visible = false;
                if (lstSuc.Count > 0)
                {
                    var html = "";
                    html += "  <span style='background-color:transparent;'><span onclick='addRow(event);'>Nuevo Ticket<img /></span></span>";
                    html += " <ul class='content_table' id='container_history' >";




                    container_history.InnerHtml = GetTickets(DateTime.Parse(txtFecha.Value, culture)); //ClFechaCap.SelectedDate);
                    //content_Reptickets.InnerHtml = html + GetTickets(DateTime.Parse(txtFecha.Value, culture)) + "</ul>";
                }

            }
          
        }


        protected void GetTickets(object sender, EventArgs e)
        {/*
            if (lstSuc.Count > 0)
            {
                content_repT.InnerHtml = GetTickets(ClFechaCap.SelectedDate);
            }*/
        }
        protected void GetSuc() {
            var su = new Sucursal();
            var sv1 = new ServiceImplementation();
            var sr = new SucursalRequest();
            su.SucursalID = 0;
            sr.Sucursal = su;
            sr.UserIDRqst = Convert.ToInt32(_usuarioID);
            sr.MessageOperationType = MessageOperationType.Report; //BackOffice.Common.Enums.MessageOperationType.Report;
            var response = sv1.SucursalMessage(sr);
            lstSuc = response.Sucursales;
        }


        [WebMethod]
        public static string GetSucursales() {
            //return lstSuc;
            var html = "";
            foreach (Sucursal su in lstSuc) {
                var _nombre = "";
                var _id = "";

                if (ValidarCampo(su.SucursalID)) { _id = su.SucursalID.ToString(); }
                if (ValidarCampo(su.Nombre)) { _nombre = culture.TextInfo.ToTitleCase(su.Nombre.ToLower()); }

                html += "<option value="+_id+">"+_id+"-"+_nombre +"</option>";
            }
            return html;
            /*
            for (var i = 0; i < sucursales.length; i++)
            {
                html += "<option value='" + sucursales[i].SucursalID + "'>" + ("000" + sucursales[i].SucursalID).slice(-3) + "-" + sucursales[i].Nombre + "</option>";
            }*/
        }

        [WebMethod]
        public static string GetMetodoPagoSAT()
        {
            var html = "";
            foreach (CboTipo item in lstMetodoPagoSAT)
                html += "<option value=" + item.ID + ">" + item.Nombre + "</option>";

            return html;
        }

        [WebMethod]
        public static string SaveTicket(String[] TicketT ) {
            Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha2 = DateTime.Today;

            DateTime _dt;

            var tck = new Ticket();
            var tk = new Ticket();
            var dal = new TicketDAL();
            var sv1 = new ServiceImplementation();
            var tkRequest = new TicketRequest();

            var STT = "Guardado";
            string msg = string.Empty;
            var TypeMsg = 0;
            var result = "";
            var msgError = "";
            var separador=",";

            var _rowID = TicketT[0] != "" ? TicketT[0].ToString().Trim() : "";
            var _sucursalID = TicketT[1] != "" ? Convert.ToInt32(TicketT[1]) : 0;
            var _fechaT = TicketT[2] != "" ? DateTime.Parse(TicketT[2].ToString().Trim(), culture) : fecha2;
            var _horaT = TicketT[3];
            var _folioT = TicketT[4] != "" ? Convert.ToInt32(TicketT[4]) : 0;
            var _cajaID = TicketT[5] != "" ? Convert.ToInt32(TicketT[5]) : 0;
            var _cajero = TicketT[6];
            var _importe = TicketT[7] != "" ? Convert.ToDouble(TicketT[7]) : 0;
            var _codUser = TicketT[8] != "" ? Convert.ToInt32(TicketT[8]) : 0;
            var _codReg = TicketT[9].Split(new char[] { '|' });
            var _codMetodoP = TicketT[10]; 



           


            if (_sucursalID < 1) { msgError = msgError != "" ? separador + "Id Sucursal" : "Id Sucursal"; }
            if (!EsFecha(_fechaT)) { msgError += msgError != "" ? separador + "Fecha Ticket" : "Fecha Ticket"; }
            if (_folioT < 1) { msgError += msgError != "" ? separador + "Folio Ticket" : "Folio Ticket"; }
            if (_cajaID < 1) { msgError += msgError != "" ? separador + "Caja" : "Caja"; }
            if (_importe < 1) { msgError += msgError != "" ? separador + "Importe" : "Importe"; }


            try
            {
                if (msgError == "")
                {
                    tck.Sucursal = new Sucursal() { SucursalID = _sucursalID };//id
                    // DateTime.TryParse(TicketT[2],out dt);
                    tck.FechaVta = _fechaT; //new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0]));//id
                    tck.HoraVta = _horaT; //TicketT[3];
                    tck.TicketID = _folioT; // Convert.ToInt32(TicketT[4]);//id
                    tck.CajaID = _cajaID; //Convert.ToInt32(TicketT[5]);//id
                    tck.Cajero = _cajero; // TicketT[6];
                    tck.Importe = _importe; // imp; //double.Parse(importe , System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);  // Convert.ToDouble(TicketT[7]);//id
                    tck.CodUsuario = _codUser; // Convert.ToInt32(TicketT[8]);
                    tck.MetodoPago = new MetodoPagoSAT() { CodMetodoP = _codMetodoP };
                    //tck.FechaCaptura = DateTime.Now;


                    switch (_rowID)
                    {
                        case "STT0":

                            tck.FechaCaptura = DateTime.Parse(fecha2.ToShortDateString(), culture);//, new System.Globalization.CultureInfo("es-ES"));
                            tck.OperationType =QSystem.Common.Enums.OperationType.New; //BackOffice.Common.Enums.OperationType.New;
                            break;
                        case "STT1":
                            STT = "Editado";
                                var _ticketID_ant = 0;//_codReg[0] != "" ? Convert.ToInt32(_codReg[0]) : 0;
                                var _sucursalID_ant =0;// _codReg[1] != "" ? Convert.ToInt32(_codReg[1]) : 0;
                                var _cajaID_ant = 0;//_codReg[2] != "" ? Convert.ToInt32(_codReg[2]) : 0;
                                var _importe_ant = 0.0;//_codReg[3] != "" ? Convert.ToDouble(_codReg[3]) : 0;
                                var _fecha_ant =DateTime.Parse("01/01/1900", culture);// _codReg[4] != "" ? DateTime.Parse(_codReg[4], culture) : DateTime.Parse("01/01/1900", culture);
                                var _CodMetodoP_ant = string.Empty;



                            if (_codReg.Length == 6)
                            {
                                _ticketID_ant = _codReg[0] != "" ? Convert.ToInt32(_codReg[0]) : 0;
                                _sucursalID_ant = _codReg[1] != "" ? Convert.ToInt32(_codReg[1]) : 0;
                                _cajaID_ant = _codReg[2] != "" ? Convert.ToInt32(_codReg[2]) : 0;
                                _importe_ant = _codReg[3] != "" ? Convert.ToDouble(_codReg[3]) : 0;
                                _fecha_ant = _codReg[4] != "" ? DateTime.Parse(_codReg[4], culture) : DateTime.Parse("01/01/1900", culture);
                                _CodMetodoP_ant = _codReg[5];
                                
                            }


                            tk.TicketID = _ticketID_ant; // Convert.ToInt32(CodeAnterior[0]);
                            tk.Sucursal = new Sucursal() { SucursalID = _sucursalID_ant }; // Convert.ToInt32(CodeAnterior[1]) };
                            tk.CajaID = _cajaID_ant; // Convert.ToInt32(CodeAnterior[2]);
                            tk.Importe = _importe_ant; //Convert.ToDouble(CodeAnterior[3]);
                            tk.FechaVta = _fecha_ant; //new DateTime(Convert.ToInt32(fechaAnt[2]), Convert.ToInt32(fechaAnt[1]), Convert.ToInt32(fechaAnt[0]));
                            tk.MetodoPago = new MetodoPagoSAT() { CodMetodoP = _CodMetodoP_ant };


                            tkRequest.Ticket = tk;
                            tkRequest.UserIDRqst = Convert.ToInt32(_usuarioID);
                            tkRequest.MessageOperationType =QSystem.Common.Enums.MessageOperationType.Query;//BackOffice.Common.Enums.MessageOperationType.Query;

                            var response = sv1.TicketMessage(tkRequest);
                            tck.FechaCaptura = DateTime.Parse(response.Ticket.FechaCaptura.ToShortDateString(), culture);//, new System.Globalization.CultureInfo("es-ES"));
                            tk = new Ticket();
                            tk = response.Ticket;
                            tk.OperationType =QSystem.Common.Enums.OperationType.Delete; //BackOffice.Common.Enums.OperationType.Delete;
                            var tkAnt = dal.SaveTicket(tk, ref msg);
                            if (tkAnt)
                            {
                                tck.OperationType = QSystem.Common.Enums.OperationType.New; //BackOffice.Common.Enums.OperationType.New;
                            }
                            break;
                        case "STT2":
                            STT = "Eliminado";
                            tck.OperationType =QSystem.Common.Enums.OperationType.Delete;// BackOffice.Common.Enums.OperationType.Delete;
                            break;
                    }

                   var tick = dal.SaveTicket(tck, ref msg);

                   result = TypeMsg + "||" + STT + " con Exito" + msg;
                }
                else
                {
                    TypeMsg = 1;
                    result = TypeMsg + "|" + msgError + "|" + msg + " Erro en el " + STT;
                }

            }
            catch (Exception ex) {
                TypeMsg = 1;
                result = TypeMsg + "|" + msgError + "|" + msg + " " + ex.Message.ToString() + " Erro en el " + STT;
            }

            return result;



           // var s = TicketT[0];

            
          /*
            // DateTime dt;
            var fecha = TicketT[2].Split(new char[] { '/' });//dia mes a;o
            var tck = new Ticket();
            var tk = new Ticket();
            var dal = new TicketDAL();
            var sv1 = new ServiceImplementation();
            var tkRequest = new TicketRequest();
            string msg = string.Empty;

            try
            {

                string importe = TicketT[7].ToString();//.Replace(',', '.');
                double imp;
                imp = Convert.ToDouble(importe);
               // Double.TryParse(importe, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo, out imp);



                tck.Sucursal = new Sucursal() { SucursalID = Convert.ToInt32(TicketT[1]) };//id
                // DateTime.TryParse(TicketT[2],out dt);
                tck.FechaVta = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0]));//id
                tck.HoraVta = TicketT[3];
                tck.TicketID = Convert.ToInt32(TicketT[4]);//id
                tck.CajaID = Convert.ToInt32(TicketT[5]);//id
                tck.Cajero = TicketT[6];
                tck.Importe = imp; //double.Parse(importe , System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);  // Convert.ToDouble(TicketT[7]);//id
                tck.CodUsuario = Convert.ToInt32(TicketT[8]);
                //tck.FechaCaptura = DateTime.Now;



                if (TicketT[0] == "STT0")
                {
                    tck.FechaCaptura = DateTime.Parse(fecha2.ToShortDateString(),culture);//, new System.Globalization.CultureInfo("es-ES"));

                    tck.OperationType = BackOffice.Common.Enums.OperationType.New;
                }
                if (TicketT[0] == "STT1")
                {
                    var CodeAnterior = TicketT[9].Split(new char[] { '|' });
                    var fechaAnt = CodeAnterior[4].Split(new char[] { '/' });

                    tk.TicketID = Convert.ToInt32(CodeAnterior[0]);
                    tk.Sucursal = new Sucursal() { SucursalID = Convert.ToInt32(CodeAnterior[1]) };
                    tk.CajaID = Convert.ToInt32(CodeAnterior[2]);
                    tk.Importe = Convert.ToDouble(CodeAnterior[3]);
                    tk.FechaVta = new DateTime(Convert.ToInt32(fechaAnt[2]), Convert.ToInt32(fechaAnt[1]), Convert.ToInt32(fechaAnt[0]));

                    tkRequest.Ticket = tk;
                    tkRequest.MessageOperationType = BackOffice.Common.Enums.MessageOperationType.Query;
                   
                    var response = sv1.TicketMessage(tkRequest);


                    tck.FechaCaptura = DateTime.Parse(response.Ticket.FechaCaptura.ToShortDateString(),culture);//, new System.Globalization.CultureInfo("es-ES"));

                    tk = new Ticket();
                    tk = response.Ticket;


                    tk.OperationType = BackOffice.Common.Enums.OperationType.Delete;
                    var tkAnt = dal.SaveTicket(tk, ref msg);
                    if (tkAnt)
                    {
                        tck.OperationType = BackOffice.Common.Enums.OperationType.New;
                    }

                }
                if (TicketT[0] == "STT2")
                {
                    tck.OperationType = BackOffice.Common.Enums.OperationType.Delete;
                }



                var tick = dal.SaveTicket(tck, ref msg);// .GetTicket(tck, ref msg);
              //return msg;
            }
            catch (Exception err)
            {
                throw new Exception("Ocurrio el siguiente Error: " + err.Message);
                //msg += err.Message.ToString();
                 
            }*/
           

         
        }

        public static string GetTickets(DateTime fecha)
        {

            string html = "";

            //html += " <ul runat='server' id='container_history' ><li >";
            html += "<li><ul>";
            html += "<li class='column_ID'>No.</li>";
            html += "<li class='column_Edit'><img  /></li>";
            html += "<li class='column_Del'><img  /></li>";
            html += "<li class='column_Sucursal'>Sucursal</li>";
            html += "<li class='column_MetodoPago'>Metodo de  Pago</li>";
            html += "<li class='column_FechaT'>Fecha Ticket</li>";
            html += "<li class='column_HoraT' >Hora Ticket</li>";
            html += "<li class='column_FolioT'>Folio Ticket</li>";
            html += "<li class='column_Caja'># Caja</li>";
            html += "<li class='column_Cajero'>Nombre de Cajero</li>";
            html += "<li class='column_Importe'>Importe</li>";
            html += "<li class='column_UserCap'>Usuario Capturo</li>";
            html += "<li class='column_stt'></li>";
            html += "<li class='column_sttReg'></li>";
            html += "</ul> ";
            html += "</li>";



           // var su = new Sucursal();
            var sv1 = new ServiceImplementation();
            //var sr = new SucursalRequest();
           // su.SucursalID = 0;

          //  sr.Sucursal = su;
           // sr.MessageOperationType = BackOffice.Common.Enums.MessageOperationType.Report;
          //  var response = sv1.SucursalMessage(sr);




            var tk = new Ticket();
            var tck = new TicketFilter();
            var ticketRequest = new TicketRequest();




            tck.FechaVta = DateTime.Parse(fecha.ToShortDateString(),culture );//, new System.Globalization.CultureInfo("es-ES")); // new DateTime(2014, 10, 24);
            tck.FechaVtaHasta = DateTime.Parse(fecha.ToShortDateString(),culture);//, new System.Globalization.CultureInfo("es-ES"));
           
            
            var sv = new ServiceImplementation();

            TicketResponse tr = sv.TicketMessage(new TicketRequest() { Filters = tck, UserIDRqst = Convert.ToInt32( _usuarioID), TipoTicketReporte = 1, MessageOperationType = MessageOperationType.Report });

            

            var selec = "";
            var count = 0;

            foreach (Ticket ticket in tr.Tickets)
            {

               // ticketRequest.TicktBitacora = ticket.Anterior;
               // ticketRequest.MessageOperationType = BackOffice.Common.Enums.MessageOperationType.Report;

                count++;
                html += "<li class='row'>";
                html += "    <ul  >";
                html += "    <li class='row_ID'><input  type='hidden' value='STT'  /></li>";
                html += "    <li class='column_ID'>" +count+ "</li>";
                if (ticket.Cliente.RFC != "")
                {
                    html += "    <li class='column_Edit column_fillColor'><img  /></li>";
                    html += "    <li class='column_Del column_fillColor'><img /></li>";
                }
                else {
                    html += "    <li class='column_Edit column_fillColor'><img src='../iconos/checkbox_unchecked (1).png' onclick='Edit_Click(event);' /></li>";
                    html += "    <li class='column_Del column_fillColor'><img src='../iconos/checkbox_unchecked (1).png' onclick='Del_Click(event);'/></li>";
                }
                html += "    <li class='column_Sucursal' ><select  class='inactive' id='select_sucursal' disabled> ";

                foreach (Sucursal s in lstSuc)
                {
                    if (s.SucursalID == ticket.Sucursal.SucursalID)
                    {
                        selec = "selected";
                    }
                    html += "<option " + selec + " value='" + s.SucursalID + "'>" + s.SucursalID.ToString() + "-" + s.Nombre + "</option>";
                    selec = "";
                }

                html += " </select></li>";

                html += "    <li class='column_MetodoPago' ><select  class='inactive' id='select_MetodoPago' disabled> ";
                foreach (CboTipo m in lstMetodoPagoSAT)
                {
                    if (m.ID == ticket.MetodoPago.CodMetodoP)
                    {
                        selec = "selected";
                    }
                    html += "<option " + selec + " value='" + m.ID + "'>" + m.ID.ToString()  + "-" + m.Nombre + "</option>";
                    selec = "";
                }

                html += " </select></li>";
               
 

                html += "<li  class='column_FechaT'><input  class='inactive' value='" + ticket.FechaVta.ToString("dd/MM/yyyy") + "' placeholder='Dia/Mes/Año'  onkeyup='ValDate(event);' onkeypress='return justNumbers(event);' maxlength='10' readonly/></li>";
                html += "<li  class='column_HoraT'><input class='inactive' value='" + ticket.HoraVta + "'  placeholder='HH:MM' onkeyup='valida(event);' onkeypress='return justNumbers(event);' maxlength='5' readonly/> </li>";
                html += "<li  class='column_FolioT'><input  class='inactive' value='" + ticket.TicketID + "' onkeyup='CapFolio(event);' readonly/></li>";
                html += "<li  class='column_Caja'><input  class='inactive' value='" + ticket.CajaID.ToString().PadLeft(3, '0') + "' readonly/>    </li>";
                html += "<li  class='column_Cajero'><input  class='inactive' value='" + ticket.Cajero + "' readonly/>   </li>";
                html += "<li  class='column_Importe'><input  class='inactive' value='" + ticket.Importe.ToString("0.00").Replace(',','.') + "'  readonly/></li>";
                html += "<li  class='column_UserCap'><input  class='inactive' value='"+ticket.CodUsAltaNombre +"' readonly/></li>";
                html += "<li  class='column_stt' ><input type='hidden' value='STT' /></li>";
                html += "<li  class='column_codUser' ><input value='"+ticket.CodUsuario+"'/></li>";

                if (ticket.Cliente.RFC != "")
                {
                    html += "<li  class='column_sttReg' ><img  title='" + ticket.Cliente.RFC + "' src='iconos/icon_factura.png' /></li>";
                }
                else {
                    html += "<li  class='column_sttReg' ><img onclick='Icon_msg(event);'/></li>";
                }

                html += "<li  class='column_CodeReg' ><input /></li>";
                html += "<li  class='column_RFC' ><input value='"+ticket.Cliente.RFC+"' /></li>";
                html += "</ul> ";
                html += "</li>";
            }

            return html; //+ "</ul>";
        }
      
        
        protected void SaveTickets(object sender, EventArgs e)
        {


        }
        protected void addTicket(object sender, EventArgs e) {
            
          
        }

        protected void DataGrid1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Reporte_Click(object sender, EventArgs e)
        {
            container_history.InnerHtml = GetTickets(DateTime.Parse(txtFecha.Value, culture));
            
         // container_history.InnerHtml=  GetTickets(DateTime.Parse(txtFechaTemp.Text ,culture));

         // ClientScript.RegisterStartupScript(typeof(Page), "head", "<script type='text/javascript'> Datepicker(" + txtFechaTemp.Text + "); </script>", true);
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

        protected static bool ValidarCampo(object campo)
        {
            var result = true;

            if (campo != null)
            {
                if (Convert.IsDBNull(campo))
                {
                    result = false;
                }
                else
                {
                    if (campo.ToString().Trim().Length == 0)
                    {
                        result = false;
                    }
                }
            }
            else
            {
                result = false;
            }
            return result;
        }
        protected static bool EsFecha(object value)
        {
            var result = true;
            Regex rgdate = new Regex("^\\d{2}/\\d{2}/\\d{4}");

            if (value != null)
            {
                if (Convert.IsDBNull(value))
                {
                    result = false;
                }
                else
                {
                    if (value.ToString().Trim().Length == 0)
                    {
                        result = false;
                    }
                    else
                    {

                        if (!rgdate.IsMatch(value.ToString()))
                        {
                            result = false;
                        }

                    }
                }
            }
            else
            {
                result = false;
            }
            return result;

        }

    }
}