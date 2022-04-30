using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using QSG.LittleCaesars.BackOffice.BL;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.Common.Reports;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.LittleCaesars.BackOffice.Messages;
using QSG.LittleCaesars.BackOffice.Messages.Requests;
using QSG.LittleCaesars.BackOffice.Messages.Response;

namespace QSG.LittleCaesars.Portal.Web
{
    public partial class WebForm8 : System.Web.UI.Page
    {
        public string NomP;
        public static CultureInfo culture = new CultureInfo("es-MX", true);
        public static string _usuarioID;
        protected void Page_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha = DateTime.Now;
            NomP = "Facturacion del dia (Publico en general)";

           if (Session["User"] != null)
            {
                var url = Page.Request.Url.LocalPath.Split(new char[] { '/', '.' });
                var nomp = url[1];


                ruta_app.Value = Session["Empresa"].ToString() + " | " + NomP;
                NickName.InnerText = Session["Nombre"].ToString();
                _usuarioID = Session["User"].ToString();
            
                if (!IsPostBack)
                {
                    //ClFechaCap.SelectedDate = fecha;
                    ClfechaCap.SelectedDate = fecha;
                    ClFechaCon.SelectedDate = fecha;

                    txtFConsumo.Text = fecha.ToShortDateString();
                    txtfechacap.Text = fecha.ToShortDateString();
                        //DateTime.Now.ToShortDateString();
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);



                    SelectUsers.Items.Clear();
                    SelectUsers.DataTextField = "Nombre";
                    SelectUsers.DataValueField = "CodUsuario";
                    SelectUsers.DataSource = GetUsers();
                    SelectUsers.DataBind();
                    SelectUsers.Items.Add(new ListItem("--- Todos ---", "0", true));
                    SelectUsers.Items.FindByValue(Session["User"].ToString()).Selected = true;

                    SelectFechaFactura.Items.Clear();
                    SelectFechaFactura.Items.Add(new ListItem(fecha.ToShortDateString(), fecha.ToString(), true));
                    SelectFechaFactura.Items.Add(new ListItem(fecha.AddDays(-1).ToShortDateString(), fecha.AddDays(-1).ToString(), true));

                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                }
            }
            else
            {
                Session["User"] = null;
                Response.Redirect("~/Account/Login.aspx", true);
            }
            
        }
        public static string SendMail(List<string> emails,string emailEmisor,string passEmisor,string nombreEmisor,string subject,string body,string rutaAttFactura,string rutaAttXml)
        {
            string msg = "";
            try
            {
                //Configuración del Mensaje
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
                mail.From = new MailAddress(emailEmisor, nombreEmisor, Encoding.UTF8);
                //Aquí ponemos el asunto del correo
                mail.Subject = subject;
                //Aquí ponemos el mensaje que incluirá el correo
                mail.Body =body;

                mail.IsBodyHtml = false;
                mail.Priority = MailPriority.High;

                //Especificamos a quien enviaremos el Email, no es necesario que sea Gmail, puede ser cualquier otro proveedor

                foreach (string m in emails)
                {
                    if (m != "") { mail.To.Add(m); }
                }
              //  mail.To.Add("carlos.villafane6@gmail.com");
                //Si queremos enviar archivos adjuntos tenemos que especificar la ruta en donde se encuentran
                mail.Attachments.Add(new Attachment(rutaAttXml));
                mail.Attachments.Add(new Attachment(rutaAttFactura)); //new Attachment(HttpContext.Current.Server.MapPath("~/Facturas_HTML/Factura_Timbrado_3.html")));
                
                //Configuracion del SMTP
                SmtpServer.Port = 587; //Puerto que utiliza Gmail para sus servicios
                //Especificamos las credenciales con las que enviaremos el mail

                SmtpServer.Credentials = new System.Net.NetworkCredential(emailEmisor,passEmisor);
               
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
               
            }
            catch (Exception ex)
            {
                msg +="No se envio la factura por la razon : "+ ex.Message;
            }
            return msg;
        }

        [WebMethod]
        public static string FacturarPG(String[] TicketT)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha2 = DateTime.Today;
            List<string> correo = new List<string>();
            
            var result = false;
            var _sucursalID = Convert.ToInt32(TicketT[2]);
           // var serie = Regex.Match(TicketT[0], @"^[A-Za-z]+").Value;
            //var folio = Regex.Match(TicketT[0], @"\d+").Value;
            var _ticketID = (Convert.ToInt32(TicketT[1]) + 1) * -1;/*folio ticket*/
            var _imp = TicketT[3];
            var _fechaVenta = DateTime.Parse(TicketT[4], culture);
            var _cajero =  HttpContext.Current.Session["Nombre"].ToString();
            var _coduser =Convert.ToInt32(HttpContext.Current.Session["User"].ToString());
            double _importe = 0;
            string fechaVta = "";
            DateTime fechaFactura = Convert.ToDateTime(Convert.ToDateTime(TicketT[6]).ToString("yyyy-MM-ddTHH:mm:ss"));

            double tasa = 0; // se pone la nueva variable para definir la tasa

            //Movido 20160923
            //string rutacertificado = HttpContext.Current.Server.MapPath("~/Certificados/CES070913FQ3_20160603_111637s.cer");
            //string rutacertificadoKey = HttpContext.Current.Server.MapPath("~/Certificados/CES070913FQ3_20160603_111637.key");
            //string passkey = "CESARMEX1";
            //string regimenfiscal = "Regimen General de Ley Personas Morales";

            //double _importe=0;
            double.TryParse(_imp, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo, out _importe);
  
            var tk = new Ticket();
            var dal = new TicketDAL();
            var sv = new ServiceImplementation();
            var tr = new TicketRequest();

            var Factura = new Comprobante();
            var Emisor = new ComprobanteEmisor();
            var Receptor = new ComprobanteReceptor();
            var Conceptos=new ComprobanteConcepto[1];
            var impuestos = new ComprobanteImpuestos();
            var traslados = new ComprobanteImpuestosTraslado[1];



            

            string msg = string.Empty;

            tk.Sucursal = new Sucursal() { SucursalID = _sucursalID };
            tk.FechaVta = _fechaVenta; //DateTime.Parse(txtFConsumo.Tex, culture);
            tk.HoraVta = String.Format("{0:HH:MM}", DateTime.Now);
            tk.TicketID = _ticketID ;
            tk.CajaID = 0;
            tk.Cajero = _cajero;
            tk.Importe = _importe;
            tk.CodUsuario = _coduser;
            tk.FechaCaptura = DateTime.Parse(fecha2.ToShortDateString(), culture);
            tk.FechaFactura = fechaFactura;
            tk.OperationType = QSystem.Common.Enums.OperationType.New;//BackOffice.Common.Enums.OperationType.New;

            result = dal.SaveTicket(tk, ref msg);
            if (result)
            {
                /*consultar ticket a editar*/
               tk = new Ticket();
                tk.Sucursal = new Sucursal() { SucursalID = _sucursalID };
                tk.TicketID = _ticketID;
                tk.CajaID = 0;
                tk.FechaVta = _fechaVenta;
                string imptemp = String.Format("{0:0.00}", _importe);
                tk.Importe = _importe;//Convert.ToDouble(imptemp,CultureInfo.InvariantCulture);

                


                TicketResponse tkresponse = sv.TicketMessage(new TicketRequest() { Ticket = tk, UserIDRqst = Convert.ToInt32(_usuarioID), MessageOperationType =QSystem.Common.Enums.MessageOperationType.Query /* BackOffice.Common.Enums.MessageOperationType.Query*/ });


                fechaVta = String.Format("{0:dd MMM yyyy}", tkresponse.Ticket.FechaVta);
                /*
                /*consultar sucursal*/
                var su = new Sucursal();
                su.SucursalID = _sucursalID;
                
                SucursalResponse _suResponse = sv.SucursalMessage(new SucursalRequest() { Sucursal = su,UserIDRqst=Convert.ToInt32(_usuarioID), MessageOperationType =QSystem.Common.Enums.MessageOperationType.Report /* BackOffice.Common.Enums.MessageOperationType.Report*/ });


                /*consultar cliente*/

              var cliente = new Cliente();
                cliente.RFC = "XAXX010101000";
                ClienteResponse _clResponse = sv.ClienteMessage(new ClienteRequest() { Cliente = cliente,UserIDRqst=Convert.ToInt32(_usuarioID), MessageOperationType =QSystem.Common.Enums.MessageOperationType.Query /*BackOffice.Common.Enums.MessageOperationType.Query*/ });
                var cl = _clResponse.Cliente;

                if (cl.Email1 != "") { correo.Add(cl.Email1); }
                if (cl.Email2 != "") {  correo.Add( cl.Email2); }
                if (cl.Email3 != "") {  correo.Add(cl.Email3); }

                
                /*consultar empresa emisora de la factura*/

                
               var paramFact = new FacturarCon();

                FacturarConResponse _empresa = sv.FacturarComMessage(new FacturarConRequest() { SucursalID = _sucursalID,UserIDRqst=Convert.ToInt32(_usuarioID), MessageOperationType =QSystem.Common.Enums.MessageOperationType.Query /* BackOffice.Common.Enums.MessageOperationType.Query */});
                var empresa = _empresa.Datos.Empresa; //_empresa.Empresa;
                var sucursal = _empresa.Sucursal;
                /*88888888888888888888888888888888*/
                /*Emisor*/
                // ComprobanteEmisor Emisor = new ComprobanteEmisor();
                
                //Movido aqui el  20160923
                string rutacertificado = HttpContext.Current.Server.MapPath("~/Certificados/" + empresa.CertificadoCer);
                string rutacertificadoKey = HttpContext.Current.Server.MapPath("~/Certificados/" + empresa.CertificadoKey);
                string passkey = empresa.PassKey;
                string regimenfiscal = "601 General de Ley Personas Morales";

                tasa = sucursal.Iva; //se asigna la tasa del iva depende de la sucursal



              Emisor.nombre = empresa.Nombre; // "CESARMEX S.A DE C.V.";

                if (ComprobarCampo(empresa.RFC)) { Emisor.rfc = empresa.RFC.ToString().Trim(); }
                else { msg += " RFC Emisor,"; } //"FOAF820313CS7";

                //empresa 
                t_UbicacionFiscal domicilioEmisorFiscal = new t_UbicacionFiscal();

                if (ComprobarCampo(empresa.Calle)) { domicilioEmisorFiscal.calle = empresa.Calle.ToString().Trim(); } else { msg += " Calle Fiscal Emisor,"; }
                if (ComprobarCampo(empresa.CP)) { domicilioEmisorFiscal.codigoPostal = empresa.CP.ToString().Trim(); } else { msg += " CP Fiscal Emisor,"; } // "22185";
                if (ComprobarCampo(empresa.Colonia)) { domicilioEmisorFiscal.colonia = empresa.Colonia.ToString().Trim(); } else { } //"COL. LOMA BONITA";
                if (ComprobarCampo(empresa.Estado)) { domicilioEmisorFiscal.estado = empresa.Estado.ToString().Trim(); } else { msg += " Estado Fiscal Emisor,"; } // "BAJA CALIFORNIA";
                if (ComprobarCampo(empresa.Delegacion)) { domicilioEmisorFiscal.localidad = empresa.Delegacion.ToString().Trim(); } else { }// "TIJUANA";
                if (ComprobarCampo(empresa.Municipio)) { domicilioEmisorFiscal.municipio = empresa.Municipio.ToString().Trim(); } else { msg += "Municipio Fiscal Emisor,"; }// "TIJUANA";
                if (ComprobarCampo(empresa.NoExt)) { domicilioEmisorFiscal.noExterior = empresa.NoExt.ToString().Trim(); } else { } //"6009";
                if (ComprobarCampo(empresa.NoInt)) { domicilioEmisorFiscal.noInterior = empresa.NoInt.ToString().Trim(); } else { }
                // domicilioEmisorFiscal.noInterior = "";
                if (ComprobarCampo("MEXICO")) { domicilioEmisorFiscal.pais = ("MEXICO").ToString().Trim(); } else { msg += " Pais Fiscal Emisor,"; }
                // domicilioEmisorFiscal.referencia = "";//frente a tortilleria...

                Emisor.DomicilioFiscal = domicilioEmisorFiscal;

                t_Ubicacion DomicilioEmisorExp = new t_Ubicacion();

                if (ComprobarCampo(sucursal.Calle)) { DomicilioEmisorExp.calle = sucursal.Calle.ToString().Trim(); } else { }// ("C. PINOS").ToString().Trim();
                if (ComprobarCampo(sucursal.CP)) { DomicilioEmisorExp.codigoPostal = sucursal.CP.ToString().Trim(); } else { } //("22185").ToString().Trim();
                if (ComprobarCampo(sucursal.Colonia)) { DomicilioEmisorExp.colonia = sucursal.Colonia.ToString().Trim(); } else { }//("COL. LOMA BONITA").ToString().Trim();
                if (ComprobarCampo(sucursal.Estado)) { DomicilioEmisorExp.estado = sucursal.Estado.ToString().Trim(); } else { } // ("BAJA CALIFORNIA").ToString().Trim();
                if (ComprobarCampo(sucursal.Ciudad)) { DomicilioEmisorExp.localidad = sucursal.Ciudad.ToString().Trim(); } else { }// ("TIJUANA").ToString().Trim();
                if (ComprobarCampo(sucursal.Municipio)) { DomicilioEmisorExp.municipio = sucursal.Municipio.ToString().Trim(); } else { } //("TIJUANA").ToString().Trim();

                if (ComprobarCampo(sucursal.NoExt)) { DomicilioEmisorExp.noExterior = sucursal.NoExt.ToString().Trim(); } else { }// ("6009").ToString().Trim();
                if (ComprobarCampo(sucursal.NoInt)) { DomicilioEmisorExp.noInterior = sucursal.NoInt.ToString().Trim(); } else { }
                
                // DomicilioEmisorExp.noInterior = "";
                if (ComprobarCampo("MEXICO")) { DomicilioEmisorExp.pais = ("MEXICO").ToString().Trim(); } else { msg += " Pais Exp. Emisor,"; }
                // DomicilioEmisorExp.referencia = "";//frente a tortilleria...
                Emisor.ExpedidoEn = DomicilioEmisorExp;

                ComprobanteEmisorRegimenFiscal RegimenFiscal = new ComprobanteEmisorRegimenFiscal();
                RegimenFiscal.Regimen = "601 General de Ley Personas Morales";
                Emisor.RegimenFiscal = new ComprobanteEmisorRegimenFiscal[] { RegimenFiscal };

                Factura.Emisor = Emisor;

                Receptor.nombre = cl.RazonSocial; // "PUBLICO EN GENERAL";

                if (ComprobarCampo(cl.RFC))
                {

                    Receptor.rfc = cl.RFC.ToString().Trim();

                }
                else { msg += " RFC Receptor,"; }



                t_Ubicacion DomicilioReceptor = new t_Ubicacion();
                string pais = "MEXICO";



                if (ComprobarCampo(cl.Calle)) { DomicilioReceptor.calle = cl.Calle; } else { } // "C. CONOCIDO";

                if (ComprobarCampo(cl.CP)) { DomicilioReceptor.codigoPostal = cl.CP.ToString().Trim(); } else { }// "22000";
                if (ComprobarCampo(cl.Colonia)) { DomicilioReceptor.colonia = cl.Colonia.ToString().Trim(); } else { } //"COL. CONOCIDO";
                if (ComprobarCampo(cl.Estado)) { DomicilioReceptor.estado = cl.Estado.ToString().Trim(); } else { } // "BAJA CALIFORNIA";
                if (ComprobarCampo(cl.Delegacion)) { DomicilioReceptor.localidad = cl.Delegacion.ToString().Trim(); } else { } // "TIJUANA";
                if (ComprobarCampo(cl.Municipio)) { DomicilioReceptor.municipio = cl.Municipio.ToString().Trim(); } else { } // "TIJUANA";
                if (ComprobarCampo(cl.NoExt)) { DomicilioReceptor.noExterior = cl.NoExt; } else { } // "CONOCIDO";
                if (ComprobarCampo(cl.NoInt)) { DomicilioReceptor.noInterior = cl.NoInt; } else { }  
                // if (cl.NoInt.Trim().Length > 0) { DomicilioReceptor.noInterior = cl.NoInt; } else { ms = false; } // "";
                if (ComprobarCampo(pais)) { DomicilioReceptor.pais = pais.Trim(); } else { msg += " Pais, "; }
                //if (cl.Contacto.Trim().Length > 0) { DomicilioReceptor.referencia = cl.Contacto; } else { ms = false; }


                //modificacion de iva deacuerda a la sucursal
                //decimal valorunitario = decimal.Round((Convert.ToDecimal(_importe) / 1.16m), 2);
                decimal valorunitario  = decimal.Round(Convert.ToDecimal(_importe) / ((Convert.ToDecimal(tasa) / 100) + 1), 2);

                

                Receptor.Domicilio = DomicilioReceptor;
                Factura.Receptor = Receptor;

                Conceptos[0] = new ComprobanteConcepto();
                Conceptos[0].cantidad = 1m;
                Conceptos[0].unidad = "ACT Clave 90101503";
                Conceptos[0].noIdentificacion = _sucursalID.ToString("000") + "0" + _ticketID; //tk.Sucursal.SucursalID.ToString("000") + tk.CajaID.ToString("000") + "#" + tk.TicketID.ToString();
                Conceptos[0].descripcion = ("Venta").ToString().Trim();
                Conceptos[0].valorUnitario = valorunitario;
                Conceptos[0].importe = valorunitario;

                Factura.Conceptos = Conceptos;

                impuestos.totalImpuestosRetenidosSpecified = true;
                impuestos.totalImpuestosTrasladadosSpecified = true;

                traslados[0] = new ComprobanteImpuestosTraslado();
                traslados[0].impuesto = ComprobanteImpuestosTrasladoImpuesto.IVA;
                traslados[0].tasa = (decimal)tasa;

                var ImpTraslados = (Convert.ToDecimal(_importe) - valorunitario);
                traslados[0].importe = decimal.Round(ImpTraslados, 2); //32.00m;//tipo decimal
                impuestos.totalImpuestosTrasladados = decimal.Round(ImpTraslados, 2);//tipo decimal

                impuestos.totalImpuestosRetenidosSpecified = true;
                impuestos.totalImpuestosTrasladadosSpecified = true;

                impuestos.Traslados = traslados;

                Factura.Impuestos = impuestos;

                //generar factura

                Factura.version = "3.2";




                Factura.serie = _empresa.Datos.Serie.ToString().ToUpper(); // responseFolio.Datos.Serie.ToString().ToUpper(); //"Z";
                Factura.folio = _empresa.Datos.Folio.ToString(); //responseFolio.Datos.Folio.ToString(); //"2";//folio interno de la empresa


                //Factura.fecha = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                Factura.fecha = fechaFactura;
                Factura.formaDePago = "PUE PAGO EN UNA SOLA EXHIBICION"; // o PAGO EN PARCIALIDADES
                Factura.condicionesDePago = "P01 Por Definir";
                Factura.subTotal = valorunitario;//tipo decimal
                Factura.total = valorunitario + ImpTraslados; //tipo decimal
                Factura.metodoDePago = "01";//"No Identificado";//"01";//20160604: se debe mandar de acuerdo a un catalogo expuesto por sat. //"No Identificado"; //efectivo, cheque, no identificado...
                //Factura.NumCtaPago = últimos 4 dígitos del número de cuenta cuando paga con cheque, tarjeta, transferencia
                Factura.NumCtaPago = "NO APLICA";
                Factura.LugarExpedicion = "TIJUANA, B.C. MEXICO";
                Factura.tipoDeComprobante = ComprobanteTipoDeComprobante.ingreso;//cuando emitimos factura el tipo de comprobante es ingreso
                Factura.Moneda = "PESOS";// o DOLAR...
                Factura.TipoCambio = "1.00";

                // string rutacertificado = HttpContext.Current.Server.MapPath("~/Certificados/csd00001000000301957024.cer");

                Certificado cert = new Certificado(rutacertificado);
                Factura.noCertificado = cert.Serie;//número del certificado otorgado por el SAT
                Factura.certificado = cert.CertificadoBase64;//Certificado en base64


                CFDI2015 cfd = new CFDI2015();
                cfd._rutaCertKey = rutacertificadoKey;

                //************************************USUARIOS DE TIMBRADO PARA FEL
                //USUARIO VALIDO
                // "CES070913FQ3";
                //"TgCSiXi+";

                //USUARIO PRUEBA
                //"DEMO1409252TA";
                // "oA9YK3h1JO=";
                //************************************USUARIOS DE TIMBRADO PARA FEL

                cfd._Clave = passkey;
                cfd._UserPak = empresa.UserPak; // "DEMO1409252TA";//"CES070913FQ3"; //"DEMO1409252TA";//DEMO1409252TA //CES070913FQ3
                cfd._ClavePak = empresa.ClavePak;//"oA9YK3h1JO="; //"TgCSiXi+";// "oA9YK3h1JO=";//oA9YK3h1JO=  //TgCSiXi+
                cfd._rutaXML = HttpContext.Current.Server.MapPath("/XML");
                cfd._rutaXlst = HttpContext.Current.Server.MapPath("/XSLT");
                cfd._rutaFacturas = HttpContext.Current.Server.MapPath("/Facturas");
                cfd.XML = ""; //response.Ticket.FacturaXML;
                cfd.Factura = Factura;
                cfd.Emisor = Emisor;
                cfd.sucursal = sucursal;
                cfd.DatosAdicionales = new String[] { "664 103-55-72", cl.NoInt.ToString(), String.Format("{0:dd MMM yyyy}", _fechaVenta), regimenfiscal };

                ///////////////////////////////////////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++/////////////////
                //  I N I C I O     N U E V O     P R O C E S O     v33 2017 12  18 
                ///////////////////////////////////////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++/////////////////
                ///////////////////////////////////////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++/////////////////
                claseCFDIv33 CFDIv33;
               

                 // Certificado cert = new Certificado(rutacertificado);
                empresa.CertificadoCerSerie = cert.Serie; // número del certificado otorgado por el SAT
                empresa.CertificadoCer64bits = cert.CertificadoBase64; // Certificado en base64
                CFDIv33 = new claseCFDIv33("." , empresa, cl, valorunitario.ToString(), ImpTraslados.ToString(), Factura.serie, Factura.folio, Factura.total.ToString(), tasa.ToString());
                // claseTemporal.
                claseTemporal.FechaEmision = fechaFactura;
                claseTemporal.CFDIGenerado = false;
                claseTemporal.CodigoConfirmacion = string.Empty;
                claseTemporal.IdTicket = Factura.serie + Factura.folio;
                claseTemporal.NombreCliente = Receptor.nombre;
                claseTemporal.Subtotal = valorunitario;
                claseTemporal.IvaTotal = ImpTraslados;
                claseTemporal.Total = valorunitario + ImpTraslados;

                claseTemporal.FormaDePago = "EFECTIVO";
                claseTemporal.FormaDePagoClave = Factura.metodoDePago;
                claseTemporal.MetodoDePago = "Pago en una sola exhibición";
                claseTemporal.MetodoDePagoClave = "PUE";
                claseTemporal.UsoCfdi = "Por definir";
                claseTemporal.UsoCfdiClave = "P01";
                claseTemporal.Moneda = "Peso Mexicano";
                claseTemporal.MonedaClave = "MXN";
                claseTemporal.TipoDeCambio = "1";
                claseTemporal.SiguienteFolio = Factura.folio;



                CFDIv33.EjecutarSecuencia();
                if (claseTemporal.CFDIGenerado)
                {
                    // Grabar la informacion
                    //CFDIv33._TimbradoDatos
                    var estatus = false;
                    var objTicket = tkresponse.Ticket;

                    objTicket.Cliente = new Cliente() { RFC = cl.RFC };
                    objTicket.FechaFactura = DateTime.Parse(CFDIv33._TimbradoDatos.FechaTimbrado.ToShortDateString(), culture);// new System.Globalization.CultureInfo("es-ES"));
                    objTicket.FolioFactura = Factura.serie.ToString().ToUpper() + Factura.folio;
                    objTicket.FechaCancelacion = null;
                    objTicket.UUID = CFDIv33._TimbradoDatos.UUID;


                    objTicket.FacturaXML = CFDIv33._TimbradoDatos.XML;
                    objTicket.OperationType = QSystem.Common.Enums.OperationType.Edit;


                    var mss = dal.SaveTicket(objTicket, ref msg);

                    //LLenamos informacion a la clase de factur para la generacion del PDF

                    Factura.noCertificado = empresa.CertificadoCerSerie;

                    //aqui se genera el pdf
                    //cfd._rutaFacturas
                    estatus = cfd.GenerarPath(out msg);
                    estatus = CFDIv33.PDF(Emisor,Factura,sucursal, cfd._rutaXlst, new String[] { "664 103-55-72", cl.NoInt.ToString(), fechaVta, regimenfiscal }, cfd._rutaFacturas, out msg);

                    // aqui se manda el correo 
                    var AttachXml = HttpContext.Current.Server.MapPath("/Facturas") + "\\" + Factura.Receptor.rfc + Factura.serie + Factura.folio + ".xml";
                    var AttachPdf = HttpContext.Current.Server.MapPath("/Facturas") + "\\" + Factura.Receptor.rfc + Factura.serie + Factura.folio + ".pdf";


                    msg += SendMail(correo, "Facturacion@little-caesars.com.mx", "Caesars2014", "Little Caesars", "Factura Electronica", "RFC: " + cl.RFC + "\n" + "Razon Social: " + cl.RazonSocial, AttachPdf, AttachXml);

                   

                   
                }
                else
                {

                    msg += "Factura No Tibrada desde la clase claseTemporal.CFDIGenerado ";
                }
                ///////////////////////////////////////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++/////////////////
                //  FIN NUEVO PROCESO v33
                ///////////////////////////////////////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++/////////////////
                ///////////////////////////////////////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++/////////////////


                
                var DatosTimbrado = new Timbrado();



                //estatus = cfd.GenerarPath(out msg);

                if (msg == "")
                { //estatus = cfd.GenerarSello(out msg);
                }
                    if (msg == "")
                {
                        //DatosTimbrado = cfd.Timbrar(out msg);
                        if (msg == "")
                    {



                            //var objTicket = tkresponse.Ticket;
                            // objTicket.Cliente = new Cliente() { RFC = cl.RFC };

                            // objTicket.FechaFactura = DateTime.Parse(DatosTimbrado.FechaTimbrado.ToShortDateString(), culture);
                            // objTicket.FechaCancelacion = null;
                            //  objTicket.FolioFactura = Factura.serie.ToString() + Factura.folio;
                            // objTicket.UUID = DatosTimbrado.UUID;
                            //   objTicket.FacturaXML = DatosTimbrado.XML;
                            //  objTicket.OperationType = QSystem.Common.Enums.OperationType.Edit; //BackOffice.Common.Enums.OperationType.Edit;

                            // var res = dal.SaveTicket(objTicket, ref msg);


                        }
                    }
                if (msg == "")
                    { //estatus = cfd.PDF(out msg);
                    }


                        if (msg == "")
                {

                            //var AttachXml = cfd.PathXML();
                            // cfd.Factura.metodoDePago = "EFECTIVO";
                            // var AttachPdf = cfd.PathPDF();

                            // msg += SendMail(correo, "Facturacion@little-caesars.com.mx", "Caesars2014", "Little Caesars", "Factura Electronica", "RFC: " + cl.RFC + "\n" + "Razon Social: " + cl.RazonSocial, AttachPdf, AttachXml);


                        }
                    }

            return msg;


        }
        private bool Facturar() {


            return true;
        }
        public List<Usuario> GetUsers()
        {
            var u = new Usuario();
            var sv = new ServiceImplementation();
            var ur = new UsuarioRequest();
            u.CodUsuario = 0;
            ur.Usuario = u;
            ur.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Report; //BackOffice.Common.Enums.MessageOperationType.Report;
            var response = sv.UsuarioMessage(ur);
            return response.Usuarios;


        }
        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void btnFConsumo_Click(object sender, EventArgs e)
        {

            if (ClFechaCon.Visible)
            {
                ClFechaCon.Visible = false;

            }
            else {
                ClfechaCap.Visible=false;
                ClFechaCon.Visible = true;

               
            }
        }

        protected void Reporte_Click(object sender, EventArgs e)
        {
            var FactPG = new RepFacturaPG();
            var sv = new ServiceImplementation();
            var msg = "";
            //var FactPGRequest = new RepFacturaPGRequest();
            try
            {
                RepFacturaPGResponse response = sv.RepFacturaPGMessage(new RepFacturaPGRequest() { fecha = DateTime.Parse(txtFConsumo.Text, culture),UserIDRqst=Convert.ToInt32(_usuarioID), MessageOperationType =QSystem.Common.Enums.MessageOperationType.Report/* BackOffice.Common.Enums.MessageOperationType.Report*/ });
                var signo = "$ ";
                double _totIng = 0;
                double _totFacCli = 0;
                double _totXFac = 0;
                double _totFact = 0;

                var html = "";
                html += " <ul class='container_history' id='container_history' ><li><ul>";

                html += "                               <li class='column_Sucursal'> Sucursal</li>";
                html += "                              <li class='column_IngDia' >Ingreso del dia (fecha consumo)</li>";
                html += "                              <li class='column_FactCli' >Facturacion Clientes</li>";
                html += "                              <li class='column_Xfact' >Por Facturar en Cierre PG</li>";
                html += "                            <li class='column_Facturar' ><img /></li>";
                html += "                             <li class='column_Facturado' >Facturado en cierre PG</li>";
                html += "                             <li class='column_Folios' >Folios Facturados PG</li>";
                html += "                           <li class='column_FactCancel'>Facturas Canceladas";
                html += "                               <ul>";
                html += "                                  <li class='column_FactCancelCli' >de clientes</li>";
                html += "                                  <li class='column_FactCancelPG' >de PG</li>";
                html += "                              </ul>";
                //html += "                              
                html += "                          </li>";



                html += "                     </ul>";
                html += "                 </li> ";


                foreach (RepFacturaPG fac in response.Reporte)
                {
                    var clase = "ButtonNormal";
                    var function = "";
                    var img = "iconos/check_icon2.png";
                    var folios = "";
                    var separador = "";
                    double xfact = fac.TotalVenta - (fac.ImporteFacturaCliente + fac.ImporteFacturaPG);

                    if (xfact > 0) { clase = "ButtonCancelar"; function = "onclick='Facturar(event);'"; img = "iconos/uncheck_icon2.png"; }


                    foreach (string folio in fac.FoliosFacturaPG.ToString().Trim().Split(new char[] { ',' }))
                    {
                        if (folio != "")
                        {
                            if (folios != "") { separador = " ;"; }
                            folios += "<a href='/Facturas/xaxx010101000" + folio.Trim() + ".pdf' target='_blank'>" + folio + separador + "</a>";
                        }

                    }

                    //fac.NumeroFacturasPG
                    html += "<li class='row'> <ul>";
                    html += "   <li class='column_FolioFact'><input value='" + fac.FoliosFacturaPG + "' /></li>";
                    html += "   <li class='column_FolioSU'><input value='" + fac.NumeroFacturasPG + "' /></li>";
                    html += "   <li class='column_SucursalID'><input value='" + fac.SucursalID + "' /></li>";
                    html += "    <li class='column_Sucursal'> " + fac.Sucursal + "</li>";
                    html += "    <li class='column_IngDia' >" + signo + String.Format(CultureInfo.InvariantCulture, "{0:#,0.00}", fac.TotalVenta) + "</li>";
                    html += "   <li class='column_FactCli' >" + signo + String.Format(CultureInfo.InvariantCulture, "{0:#,0.00}", fac.ImporteFacturaCliente) + "</li>";
                    html += "   <li class='column_Xfact' >" + signo + String.Format(CultureInfo.InvariantCulture, "{0:#,0.00}", xfact) + "</li>";
                    html += "    <li class='column_Facturar' ><img src='" + img + "' " + function + " title='Facturar' /></li>";
                    html += "    <li class='column_Facturado' >" + signo + String.Format(CultureInfo.InvariantCulture, "{0:#,0.00}", fac.ImporteFacturaPG) + " </li>";

                    html += "    <li class='column_Folios' >" + folios + "</li>";

                    html += "     <li class='column_FactCancelCli' >" + fac.ImporteFacturaClienteCancelada + "</li>";
                    html += "    <li class='column_FactCancelPG' >" + fac.ImporteFacturaPGCancelada + "</li>";

                    html += " </ul></li> ";


                    _totIng += fac.TotalVenta;
                    _totFacCli += fac.ImporteFacturaCliente;
                    _totXFac += xfact;
                    _totFact += fac.ImporteFacturaPG;

                }

                html += "<li class='Rowfooter'> <ul>";
                // html += "   <li class='column_FolioFact'><input value='" + fac.FoliosFacturaPG + "' /></li>";
                // html += "   <li class='column_FolioSU'><input value='" + fac.NumeroFacturasPG + "' /></li>";
                // html += "   <li class='column_SucursalID'><input value='" + fac.SucursalID + "' /></li>";
                html += "    <li class='column_Sucursal' style='font-weight:bold;'> Totales >></li>";
                html += "    <li class='column_IngDia' ><input value='" + signo + String.Format(CultureInfo.InvariantCulture, "{0:#,0.00}", _totIng) + "' readonly/> </li>";
                html += "   <li class='column_FactCli' ><input value='" + signo + String.Format(CultureInfo.InvariantCulture, "{0:#,0.00}", _totFacCli) + "' readonly/></li>";
                html += "   <li class='column_Xfact' ><input value='" + signo + String.Format(CultureInfo.InvariantCulture, "{0:#,0.00}", _totXFac) + "' readonly/></li>";
                html += "    <li class='column_Facturar' ></li>";
                html += "    <li class='column_Facturado' ><input value='" + signo + String.Format(CultureInfo.InvariantCulture, "{0:#,0.00}", _totFact) + " ' readonly/></li>";

                html += "    <li class='column_Folios' ></li>";

                html += "     <li class='column_FactCancelCli' ></li>";
                html += "    <li class='column_FactCancelPG' ></li>";

                html += " </ul></li> ";

                content_repT.InnerHtml = html + "</ul>";
                msg = response.FriendlyMessage.ToString();
            }
            catch (Exception ex)
            {
               // throw new Exception();
                msg = "Error : " + ex.Message;

            }
           

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "alert("+msg+");", true);
        }

        protected void ClFechaCap_SelectionChanged(object sender, EventArgs e)
        {
          //  txtfechacap.Text = ClfechaCap.SelectedDate.ToShortDateString();
            //ClfechaCap.Visible = false;
        }

        protected void ClFechaCon_SelectionChanged(object sender, EventArgs e)
        {
            txtFConsumo.Text = ClFechaCon.SelectedDate.ToShortDateString();
            ClFechaCon.Visible = false;
        }

        protected void btnfechacap_Click(object sender, EventArgs e)
        {/*
            if (ClfechaCap.Visible) { 
                ClfechaCap.Visible=false;
            } else {
                ClFechaCon.Visible = false;
                ClfechaCap.Visible = true;
            }*/
        }
        protected static bool ComprobarCampo(object Campo)
        {
            bool bRes = true;
            if (Campo != null)
            {
                if (Convert.IsDBNull(Campo))
                {
                    bRes = false;
                }
                else
                {
                    if (Campo.ToString().Trim().Length == 0)
                    {
                        bRes = false;
                    }
                }
            }
            else
            {
                bRes = false;
            }
            return bRes;
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
    }
}