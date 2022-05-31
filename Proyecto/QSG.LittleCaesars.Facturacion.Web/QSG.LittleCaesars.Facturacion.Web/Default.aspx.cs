using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.Messages;
using QSG.LittleCaesars.BackOffice.Messages.Requests;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.QSystem.Common;
using System.IO;
using System.Data;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Threading;
using QSG.LittleCaesars.BackOffice.BL;
using System.Net.Mail;
using System.ComponentModel;
using System.Xml;
using System.Text;
using ThoughtWorks.QRCode.Codec;

using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html.simpleparser;
using System.Net;
using System.Text.RegularExpressions;


//using System.Drawing;
//using System.Drawing.Imaging;




namespace QSG.LittleCaesars.Facturacion.Web
{
    public partial class _Default : Page
    {
       public static CultureInfo culture = new CultureInfo("es-MX", true);
        protected void Page_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            //Response.Redirect("index.aspx",true);

            
           
        }

        [WebMethod]
        public static List<Sucursal> GetSucursales()
        {
            var su = new Sucursal();
            var sv = new ServiceImplementation();
            var sr = new SucursalRequest();
            su.SucursalID = 0;
            sr.Sucursal = su;
            sr.UserIDRqst = 1;
            sr.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Report; //BackOffice.Common.Enums.MessageOperationType.Report;
            var response = sv.SucursalMessage(sr);
            return response.Sucursales;
        }
        [WebMethod]
        public static List<RegimenFiscal> GetRegimenFiscales()
        {
            var rf = new RegimenFiscal();
            var sv = new ServiceImplementation();
            var rfr = new RegimenFiscalRequest();

            rfr.UserIDRqst = 1;
            rfr.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Report; //BackOffice.Common.Enums.MessageOperationType.Report;
            var response = sv.RegimenFiscalMessage(rfr);
            return response.RegimenFiscales;
        }
        [WebMethod]
        public static Ticket ValidarTicket(string cadena, double Importe)
        {
            //101#0001
            //anio mes dia
            var tk = new Ticket();
            var sv = new ServiceImplementation();
            var tr = new TicketRequest();
            var info = cadena.Split(new char[]{'#'});
            var datos = info[0].ToString(); //info[0].ToString().PadLeft(3, '0');
            var folio = info[1].ToString();
            tk.TicketID = Convert.ToInt32(info[1]);
            tk.Sucursal = new Sucursal() { SucursalID = Convert.ToInt32(datos.Substring(0, 3)) };
            tk.CajaID = Convert.ToInt32(datos.Substring(3, 3));

         

            //tk.Folio = Folio.Substring(0, Folio.Length - 8);
            tk.Importe = Importe;
       
            tr.Ticket = tk;
            tr.UserIDRqst = 1;
            tr.MessageOperationType =QSystem.Common.Enums.MessageOperationType.Query;
           
            
            var response = sv.TicketMessage(tr);


            return response.Ticket;



        }
        [WebMethod]
        public static Cliente GetCliente(string RFC) {
            var cl = new Cliente();
            var sv = new ServiceImplementation();
            var cr=new ClienteRequest();

            cl.RFC = RFC;

            cr.Cliente=cl;
            cr.UserIDRqst = 1;
            cr.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Query;
            var response = sv.ClienteMessage(cr);

            return response.Cliente;

        }
        [WebMethod]
        public static string SaveCliente(String[] Client_Parameters)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            DateTime fechaT = DateTime.Today;

            string msg = string.Empty;
            var ms = true;
            decimal Importe = 0;
            double tasa = 0; // Se optiene de Sucursal.
            decimal impSinIVA = 0;
            decimal ImpTraslados = 0;
            


            CFDI2015 cfd = new CFDI2015();
            claseCFDIv33 CFDIv33;


            try
            {
                
                // Aqui estaban declaradas estas variables 20160922
                //string rutacertificado = HttpContext.Current.Server.MapPath("~/Certificados/CES070913FQ3_20160603_111637s.cer");
                //string rutacertificadoKey = HttpContext.Current.Server.MapPath("~/Certificados/CES070913FQ3_20160603_111637.key");
                //string passkey = "CESARMEX1";


                string uuid = "";
                string fechaCert = "";
                string selloSAT = "";
                string NoCertificadoCSD = "";
                string NoCertSAT = "";
                string selloCFDI = "";
                string cadenaoriginal = "";
                string regimenfiscal = "601 General de Ley Personas Morales";
                string fechaVta = "";


                //  var su = new Sucursal();
                //var SuR = new SucursalRequest();



                var fCod = new FacturarCon();
                var FRe = new FacturarConRequest();
                //empresa

                //  var empresa = new EmpresaCliente();
                // var empresa_request = new ClienteRequest();



                //
                var cl = new Cliente();
                var tk = new Ticket();
                var sv = new ServiceImplementation();
                var tr = new TicketRequest();


                var tkDAL = new TicketDAL();
                var dal = new ClienteDAL();
                var correos = Client_Parameters[16].ToString().Split(new char[] { ';' });
                var opcion = Client_Parameters[0].ToString();

                cl.RFC = Client_Parameters[4].ToString();
                cl.RazonSocial = Client_Parameters[5].ToString();
                cl.Calle = Client_Parameters[6].ToString();
                cl.NoInt = Client_Parameters[7].ToString();
                cl.NoExt = Client_Parameters[8].ToString();
                // cl.Municipio
                cl.Colonia = Client_Parameters[9].ToString();
                cl.Delegacion = Client_Parameters[10].ToString();
                cl.Ciudad = Client_Parameters[11].ToString();
                cl.Municipio = Client_Parameters[12].ToString();
                cl.Estado = Client_Parameters[13].ToString();
                cl.CP = Client_Parameters[14].ToString();
                cl.Contacto = Client_Parameters[15].ToString();

                // TODO agegarlo en la pantalla y en el JS.
                cl.RegimenFiscal = Client_Parameters[17].ToString(); //"612"; //Client_Parameters[17].ToString();

                if (correos.Length > 0)
                {

                    if (correos[0] != "")
                    {
                        cl.Email1 = correos[0].ToString().Trim();
                    }

                    if (correos.Length > 1)
                    {
                        if (correos[1] != "")
                        {
                            cl.Email2 = correos[1].ToString().Trim();
                        }
                    }

                    if (correos.Length > 2)
                    {
                        if (correos[2] != "")
                        {
                            cl.Email3 = correos[2].ToString().Trim();
                        }
                    }
                }


                //obtener informacion ticket si es necesaroi actualizar

                var info = Client_Parameters[1].Split(new char[] { '#' });
                var datos = info[0].ToString(); //info[0].ToString().PadLeft(3, '0');
                var folio = info[1].ToString();
                tk.TicketID = Convert.ToInt32(info[1]);
                tk.Sucursal = new Sucursal() { SucursalID = Convert.ToInt32(datos.Substring(0, 3)) };
                tk.CajaID = Convert.ToInt32(datos.Substring(3, 3));
                string imp = String.Format("{0:0.00}", Client_Parameters[3].ToString().Trim());

                double im = Convert.ToDouble(imp, culture);

                tk.Importe = im; //Convert.ToDouble(imp, CultureInfo.InvariantCulture);
                //  tk.Importe = Convert.ToDouble(Client_Parameters[3].ToString().Trim().Replace(',', '.'));

                tr.Ticket = tk;

                

                tr.UserIDRqst = 1;
                tr.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Query;
                var response = sv.TicketMessage(tr);

                Importe = Convert.ToDecimal(tk.Importe); //Convert.ToDecimal(Client_Parameters[3].ToString().Replace(',', '.')); //decimal.Round(Convert.ToDecimal(tk.Importe), 2);

                if (Importe < Convert.ToDecimal(0.00))
                {
                    //ms = false;
                    msg += " Importe debe ser mayor 0";
                }


                fechaVta = String.Format("{0:dd MMM yyyy}", response.Ticket.FechaVta);
                Comprobante Factura = new Comprobante();

                string rutaxmltimbrado = "";
                XmlDocument docxml = new XmlDocument();
                // docxml.Load(pt);


                //var cade = ConvertCadena("||1.0|8BBC79C8-4783-4C48-889B-ECDF534DE6C8|2014-12-03T14:13:06|pwd9NpHoURU7Lj+njZtU+mAi1/Qb2PyWz4okedRZ7PbRT/SmMPkzTew5gzFncvfsy5/U3UrxStaNy+shBgp1yDLVYMSKb1aVAhzbtMxeZDM6qelK/9goICh//Tc2CwhJilO109+01ylh0o7uuJfpLEtAKQ26T5tMK/+crXG+/Qs=|00001000000203220546||", 110);

                var urlBase = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority;  //HttpContext.Current.Request.Url; // ("~/Facturas_HTML/ImagesQR/XAXX010101000PJ2.jpg"); //  Server.MapPath("~/");

              

                    //obtener folio factura

                    FRe.SucursalID = Convert.ToInt32(datos.Substring(0, 3));
                    FRe.UserIDRqst = 1;
                    FRe.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Query;
                    var responseFolio = sv.FacturarComMessage(FRe);
                    var empresa = responseFolio.Datos.Empresa; //responseFolio.Empresa;

                tasa = responseFolio.Sucursal.Iva;  // se saca la tasa de la sucursal que va facturar

                    //string rutacertificado = HttpContext.Current.Server.MapPath("~/Certificados/CES070913FQ3_20160603_111637s.cer");
                    //string rutacertificadoKey = HttpContext.Current.Server.MapPath("~/Certificados/CES070913FQ3_20160603_111637.key");
                    string rutacertificado = HttpContext.Current.Server.MapPath("~/Certificados/" + empresa.CertificadoCer);
                    string rutacertificadoKey = HttpContext.Current.Server.MapPath("~/Certificados/"+ empresa.CertificadoKey);
                    //string passkey = "CESARMEX1";
                    string passkey =empresa.PassKey;


                    //Comprobante Factura = new Comprobante();

                    //quien emite la factura este caso empresa
                    ComprobanteEmisor Emisor = new ComprobanteEmisor();

                    Emisor.nombre = empresa.Nombre; // "CESARMEX S.A DE C.V.";

                    if (ComprobarCampo(empresa.RFC))
                    {
                        Emisor.rfc = empresa.RFC.ToString().Trim();
                    }
                    else
                    {
                        msg += " RFC Emisor,";
                    } 

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

                    var sucursal = response.Ticket.Sucursal;

                

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



                    //cliente
                    ComprobanteReceptor Receptor = new ComprobanteReceptor();
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

                    Receptor.Domicilio = DomicilioReceptor;
                    Factura.Receptor = Receptor;


                    ComprobanteConcepto[] Conceptos = new ComprobanteConcepto[1];


                //impSinIVA = decimal.Round((Importe / 1.16m), 2);
                
                impSinIVA = decimal.Round(Importe / (( Convert.ToDecimal(tasa) / 100) + 1), 2);

                Conceptos[0] = new ComprobanteConcepto();
                    Conceptos[0].cantidad = 1m;
                    Conceptos[0].unidad = "ACT     Clave 90101503";
                    Conceptos[0].noIdentificacion = tk.Sucursal.SucursalID.ToString("000") + tk.CajaID.ToString("000") + "#" + tk.TicketID.ToString();
                    Conceptos[0].descripcion = ("Consumo de Alimentos").ToString().Trim();
                    Conceptos[0].valorUnitario = impSinIVA;
                    Conceptos[0].importe = impSinIVA;

                    Factura.Conceptos = Conceptos;

                    ComprobanteImpuestos impuestos = new ComprobanteImpuestos();
                    impuestos.totalImpuestosRetenidosSpecified = true;
                    impuestos.totalImpuestosTrasladadosSpecified = true;


                    ComprobanteImpuestosTraslado[] traslados = new ComprobanteImpuestosTraslado[1];

                    traslados[0] = new ComprobanteImpuestosTraslado();
                    traslados[0].impuesto = ComprobanteImpuestosTrasladoImpuesto.IVA;
                    traslados[0].tasa = (decimal)tasa;

                    // montImp = valorProd * (tasa / 100);
                    ImpTraslados = (Importe - impSinIVA);
                    traslados[0].importe = decimal.Round(ImpTraslados, 2); //32.00m;//tipo decimal
                    impuestos.totalImpuestosTrasladados = decimal.Round(ImpTraslados, 2);//tipo decimal

                    impuestos.totalImpuestosRetenidosSpecified = true;
                    impuestos.totalImpuestosTrasladadosSpecified = true;

                    impuestos.Traslados = traslados;

                    Factura.Impuestos = impuestos;

                    //generar factura

                    Factura.version = "3.2";// Todo: Revisar si esto no afecta. no debe ser la  3.3?

                    //var entero = Regex.Match("str123", @"\d+").Value;
                   // var cadena = Regex.Match("str123", @"^[A-Za-z]+").Value;
                    if (response.Ticket.FacturaXML == null || response.Ticket.FacturaXML == "")
                    {



                        Factura.serie = responseFolio.Datos.Serie.ToString().ToUpper(); //"Z";
                        Factura.folio = responseFolio.Datos.Folio.ToString(); //"2";//folio interno de la empresa
                    }
                    else
                    {
                        var ser = Regex.Match(response.Ticket.FolioFactura, @"^[A-Za-z]+").Value;
                        var fol = Regex.Match(response.Ticket.FolioFactura, @"\d+").Value;

                        Factura.serie = ser;
                        Factura.folio = fol;
                    }
                    
                    Factura.fecha = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                    Factura.formaDePago = "PUE Pago en una sola exhibicion"; // o PAGO EN PARCIALIDADES
                    Factura.condicionesDePago = "G03 Gastos en general";
                    //Factura.condicionesDePago = "P01 Por Definir";
                    Factura.subTotal = impSinIVA;//tipo decimal
                    Factura.total = impSinIVA + ImpTraslados; //tipo decimal
                    Factura.metodoDePago = response.Ticket.MetodoPago.CodMetodoP + " " + response.Ticket.MetodoPago.Descripcion; //"01";    //"TARJETA DEBITO, #4599";// "No identificado"; //efectivo, cheque, No identificado...
                    //Factura.NumCtaPago = últimos 4 dígitos del número de cuenta cuando paga con cheque, tarjeta, transferencia
                    Factura.NumCtaPago = "NO APLICA";
                    Factura.LugarExpedicion = "TIJUANA, B.C. MEXICO";
                    Factura.tipoDeComprobante = ComprobanteTipoDeComprobante.ingreso;//cuando emitimos factura el tipo de comprobante es ingreso
                    Factura.Moneda = "PESOS";// o DOLAR...
                    Factura.TipoCambio = "1.00";

                    // string rutacertificado = HttpContext.Current.Server.MapPath("~/Certificados/csd00001000000301957024.cer");

                // Comentado para la version 3.3 se pone mas abajo
                    //Certificado cert = new Certificado(rutacertificado);
                    //Factura.noCertificado = cert.Serie;//número del certificado otorgado por el SAT
                    //Factura.certificado = cert.CertificadoBase64;//Certificado en base64

                    //el certificado que te da el SAT tiene vigencia
                    //si está caducado te va a dar un error y no va a facturar
                    //===puedes monitorear las fechas con:
                    //   DateTime iniciocertificado = cert.ValidoDesde;
                    //   DateTime fincertificado = cert.ValidoHasta;
                  
                    
                    /**************************/
                    cfd._rutaCertKey = rutacertificadoKey;

                    
                    //******************************* USUARIOS PARA EL TIMBRADO DE FEL 
                    // Usuario de Preueba 
                    //"DEMO1409252TA";
                    //"oA9YK3h1JO=";
                    
                    // Usuario Valido
                    //"CES070913FQ3";
                    //"TgCSiXi+";
                    //*******************************

                    cfd._Clave = passkey;
                    cfd._UserPak = empresa.UserPak; // "DEMO1409252TA"; //"CES070913FQ3"; //"DEMO1409252TA";
                    cfd._ClavePak = empresa.ClavePak; //"oA9YK3h1JO=";//"TgCSiXi+"; //"oA9YK3h1JO=";
                    cfd._rutaXML = HttpContext.Current.Server.MapPath("/XML");
                   // cfd._rutaXlst = HttpContext.Current.Server.MapPath("/Dats");
                    cfd._rutaXlst = HttpContext.Current.Server.MapPath("/XSLT");
                    cfd._rutaFacturas = HttpContext.Current.Server.MapPath("/Facturas");
                    cfd.XML = response.Ticket.FacturaXML;
                    cfd.Factura = Factura;
                    cfd.Emisor = Emisor;
                    cfd.sucursal = sucursal;
                    cfd.DatosAdicionales = new String[] { "664 103-55-72", cl.NoInt.ToString(), fechaVta, regimenfiscal };
                    


                    var estatus = false;
                    var DatosTimbrado = new Timbrado();

                    //List<Creditos> creditos = new List<Creditos>();
                    
                    estatus = cfd.GenerarPath(out msg);

                ///////////////////////////////////////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++/////////////////
                //  I N I C I O     N U E V O     P R O C E S O     v33
                ///////////////////////////////////////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++/////////////////
                ///////////////////////////////////////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++/////////////////
                Certificado cert = new Certificado(rutacertificado);
                empresa.CertificadoCerSerie = cert.Serie; // número del certificado otorgado por el SAT
                empresa.CertificadoCer64bits = cert.CertificadoBase64; // Certificado en base64
                CFDIv33 = new claseCFDIv33(".", empresa, cl, impSinIVA.ToString() , ImpTraslados.ToString(), Factura.serie, Factura.folio,Factura.total.ToString(), tasa.ToString(), rutacertificado, rutacertificadoKey, passkey);
                // claseTemporal.

                claseTemporal.FechaEmision = DateTime.Now; 
                claseTemporal.CFDIGenerado = false;
        

                claseTemporal.CodigoConfirmacion = string.Empty;
                claseTemporal.IdTicket = Factura.serie + Factura.folio;
                claseTemporal.NombreCliente = Receptor.nombre;
                claseTemporal.Subtotal = impSinIVA;
                claseTemporal.IvaTotal = ImpTraslados;
                claseTemporal.Total= impSinIVA + ImpTraslados;

                claseTemporal.FormaDePago = response.Ticket.MetodoPago.Descripcion;
                claseTemporal.FormaDePagoClave = response.Ticket.MetodoPago.CodMetodoP;
                claseTemporal.MetodoDePago = "Pago en una sola exhibición";
                claseTemporal.MetodoDePagoClave = "PUE";
                claseTemporal.UsoCfdi = "Por definir";
                claseTemporal.UsoCfdiClave = "P01";
                claseTemporal.Moneda = "Peso Mexicano";
                claseTemporal.MonedaClave = "MXN";
                claseTemporal.TipoDeCambio = "1";
                claseTemporal.SiguienteFolio = Factura.folio;

                // TODO: llenar todos los campos necesarios para la clase temporal (los marcados con //+  son obligatorios)

                //cfd._rutaXML = HttpContext.Current.Server.MapPath("/XML");
                //cfd._rutaXlst = HttpContext.Current.Server.MapPath("/Dats");
                //cfd._rutaFacturas = HttpContext.Current.Server.MapPath("/Facturas");
                //cfd.DatosAdicionales = new String[] { "664 103-55-72", cl.NoInt.ToString(), fechaVta, regimenfiscal };
       if (response.Ticket.FacturaXML == null || response.Ticket.FacturaXML == "")
          {
                CFDIv33.EjecutarSecuencia();

                if (claseTemporal.CFDIGenerado)
                {
                    // Grabar la informacion
                    //CFDIv33._TimbradoDatos
                    var objTicket = response.Ticket;
                    objTicket.Cliente = new Cliente() { RFC = Client_Parameters[4].ToString() };
                    objTicket.FechaFactura = DateTime.Parse(CFDIv33._TimbradoDatos.FechaTimbrado.ToShortDateString(), culture);// new System.Globalization.CultureInfo("es-ES"));
                    objTicket.FolioFactura = Factura.serie.ToString().ToUpper() + Factura.folio;
                    objTicket.FechaCancelacion = null;
                    objTicket.UUID = CFDIv33._TimbradoDatos.UUID;


                    objTicket.FacturaXML = CFDIv33._TimbradoDatos.XML;
                    objTicket.OperationType = QSystem.Common.Enums.OperationType.Edit;


                    var mss = tkDAL.SaveTicket(objTicket, ref msg);

                    //LLenamos informacion a la clase de factur para la generacion del PDF

                    Factura.noCertificado = empresa.CertificadoCerSerie;

                    //aqui se genera el pdf
                    estatus = CFDIv33.PDF(Emisor,Factura,sucursal, cfd._rutaXlst, new String[] { "664 103-55-72", cl.NoInt.ToString(),fechaVta, regimenfiscal }, cfd._rutaFacturas, out msg);

                    // aqui se manda el correo 
                    var AttachXml = HttpContext.Current.Server.MapPath("/Facturas") + "\\" + Factura.Receptor.rfc + Factura.serie + Factura.folio + ".xml";
                    var AttachPdf = HttpContext.Current.Server.MapPath("/Facturas") + "\\" + Factura.Receptor.rfc + Factura.serie + Factura.folio + ".pdf";                        

                    msg += SendMail(correos , "Facturacion@little-caesars.com.mx", "Caesars2014", "Little Caesars", "Factura Electronica", "RFC: " + cl.RFC + "\n" + "Razon Social: " + cl.RazonSocial, AttachPdf, AttachXml);

                    //aqui se graba el cliente 

                    if (opcion == "")
                    {
                        cl.OperationType = QSystem.Common.Enums.OperationType.New;
                    }
                    else
                    {
                        cl.OperationType = QSystem.Common.Enums.OperationType.Edit;
                    }

                    ms = dal.SaveCliente(cl, ref msg);
                        
                }
                else
                {
                    msg += "Factura No Timbrada desde la clase claseTemporal.CFDIGenerado;       ";
                    if (!string.IsNullOrEmpty(claseTemporal.MsgError_Timbrado))
                    {
                        msg += claseTemporal.MsgError_Timbrado;
                    }
                }
                ///////////////////////////////////////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++/////////////////
                //  FIN NUEVO PROCESO v33
                ///////////////////////////////////////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++/////////////////
                ///////////////////////////////////////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++/////////////////

                //if (response.Ticket.FacturaXML == null || response.Ticket.FacturaXML == "")
                 //   {

                if (msg == "")
                {
                    //este ya no se usa 3.3
                    // estatus = cfd.GenerarSello(out msg);
                }
                if (msg == "")
                {
                    //DatosTimbrado = cfd.Timbrar(out msg);
                    if (msg == "")
                    {
                                
                        //ESTE PROCESO YA NO SE USARA CAMBIOP VERSION33
                        //var objTicket = response.Ticket;
                        //objTicket.Cliente = new Cliente() { RFC = Client_Parameters[4].ToString() };
                        //objTicket.FechaFactura = DateTime.Parse(DatosTimbrado.FechaTimbrado.ToShortDateString(), culture);// new System.Globalization.CultureInfo("es-ES"));
                        //objTicket.FolioFactura = Factura.serie.ToString().ToUpper() + Factura.folio;
                        //objTicket.FechaCancelacion = null;
                        //objTicket.UUID = DatosTimbrado.UUID;
                        
                        //objTicket.FacturaXML = DatosTimbrado.XML;
                        //objTicket.OperationType = QSystem.Common.Enums.OperationType.Edit;
                        
                        //var mss = tkDAL.SaveTicket(objTicket, ref msg);
                    }
                }
            }
            else
            {

                estatus = cfd.PrepararXML(out msg);
                if (estatus)
                {
                    var AttachXml = HttpContext.Current.Server.MapPath("/Facturas") + "\\" + Factura.Receptor.rfc + Factura.serie + Factura.folio + ".xml";
                    var AttachPdf = HttpContext.Current.Server.MapPath("/Facturas") + "\\" + Factura.Receptor.rfc + Factura.serie + Factura.folio + ".pdf";
                    msg += SendMail(correos, "Facturacion@little-caesars.com.mx", "Caesars2014", "Little Caesars", "Factura Electronica", "RFC: " + cl.RFC + "\n" + "Razon Social: " + cl.RazonSocial, AttachPdf, AttachXml);
                }
            }


            if (msg == "")
            {
                //este proceso se quita por la version 3.3
                //cfd.Factura.metodoDePago =response.Ticket.MetodoPago.CodMetodoP + ' ' + response.Ticket.MetodoPago.Descripcion; // "Efectivo";
                //estatus = cfd.PDF(out msg);
            }

            if (msg == "")
            {
                        
                //este proceso se quita por la nerva version 3.3
                //var AttachXml = cfd.PathXML();
                //var AttachPdf = cfd.PathPDF();

                //msg += SendMail(correos, "Facturacion@little-caesars.com.mx", "Caesars2014", "Little Caesars", "Factura Electronica", "RFC: " + cl.RFC + "\n" + "Razon Social: " + cl.RazonSocial, AttachPdf, AttachXml);
            }

            if (opcion == "")
            {
                //cl.OperationType = QSystem.Common.Enums.OperationType.New;
            }
            else 
            {
                //cl.OperationType = QSystem.Common.Enums.OperationType.Edit;
            }

                //ms = dal.SaveCliente(cl, ref msg);
                    
                /**************************/
        }
        catch (Exception err)
        {
            msg += err.Message.ToString();

        }
        return msg;
        
           
        }
      

        public static string SendMail(string[]emails,string emailEmisor,string passEmisor,string nombreEmisor,string subject,string body,string rutaAttFactura,string rutaAttXml)
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

        public static string GenerarHTML(ComprobanteEmisor Emisor, string matrizTel, Comprobante Factura, Sucursal sucursal, string rutaQR, string rutaLogo, string NoCertificadoCSD, string UUID, string NoCertSAT, string fechaCert, string fechaVta, string selloSAT, string selloCFDI, string cadenaoriginal, string regimenfiscal, string NumInt, out string msg1)
        {
            // string msg1 = string.Empty;
            msg1 = "";
            StringBuilder html = new StringBuilder();
            try
            {
                html.Append(" <html xmlns:v='urn:schemas-microsoft-com:vml'");
                html.Append("\n");
                html.Append("  xmlns:o='urn:schemas-microsoft-com:office:office'");
                html.Append("   xmlns:w='urn:schemas-microsoft-com:office:word'");
                html.Append("   xmlns:m='http://schemas.microsoft.com/office/2004/12/omml'");
                html.Append("   xmlns='http://www.w3.org/TR/REC-html40'>");
                // html.Append("
                html.Append("    <head>");
                html.Append("    <meta http-equiv=Content-Type content='text/html; charset=windows-1252'>");
                html.Append("    <meta name=ProgId content=Word.Document>");
                html.Append("    <meta name=Generator content='Microsoft Word 14'>");
                html.Append("    <meta name=Originator content='Microsoft Word 14'>");
                html.Append("    <link rel=File-List href='cfdi%203.2%20matriz_archivos/filelist.xml'>");
                html.Append("    <title>" + ValidarNull(Factura.Emisor.nombre) + "</title>");


                html.Append("     <link rel=themeData href='cfdi%203.2%20matriz_archivos/themedata.thmx'>");
                html.Append("     <link rel=colorSchemeMapping");
                html.Append("     href='cfdi%203.2%20matriz_archivos/colorschememapping.xml'>");

                html.Append("        <style>");
                html.Append("          <!--");
                html.Append("\n");
                /* Font Definitions */
                html.Append("          @font-face");
                html.Append("            {font-family:Cambria;");
                html.Append("            panose-1:2 4 5 3 5 4 6 3 2 4;");
                html.Append("            mso-font-charset:0;");
                html.Append("            mso-generic-font-family:roman;");
                html.Append("            mso-font-pitch:variable;");
                html.Append("            mso-font-signature:-536870145 1073743103 0 0 415 0;}");
                html.Append("        @font-face");
                html.Append("            {font-family:Calibri;");
                html.Append("            panose-1:2 15 5 2 2 2 4 3 2 4;");
                html.Append("            mso-font-charset:0;");
                html.Append("            mso-generic-font-family:swiss;");
                html.Append("            mso-font-pitch:variable;");
                html.Append("            mso-font-signature:-536870145 1073786111 1 0 415 0;}");
                html.Append("        @font-face");
                html.Append("            {font-family:Tahoma;");
                html.Append("            panose-1:2 11 6 4 3 5 4 4 2 4;");
                html.Append("            mso-font-charset:0;");
                html.Append("            mso-generic-font-family:swiss;");
                html.Append("            mso-font-pitch:variable;");
                html.Append("            mso-font-signature:-520081665 -1073717157 41 0 66047 0;}");
                /* Style Definitions */
                html.Append("          p.MsoNormal, li.MsoNormal, div.MsoNormal");
                html.Append("            {mso-style-unhide:no;");
                html.Append("           mso-style-qformat:yes;");
                html.Append("           mso-style-parent:'';");
                html.Append("           margin-top:0cm;");
                html.Append("           margin-right:0cm;");
                html.Append("           margin-bottom:10.0pt;");
                html.Append("          margin-left:0cm;");
                html.Append("          line-height:115%;");
                html.Append("            mso-pagination:widow-orphan;");
                html.Append("            font-size:11.0pt;");
                html.Append("            font-family:'Calibri','sans-serif';");
                html.Append("            mso-fareast-font-family:'Times New Roman';");
                html.Append("            mso-bidi-font-family:'Times New Roman';");
                html.Append("            mso-fareast-language:EN-US;}");
                html.Append("        a:link, span.MsoHyperlink");
                html.Append("            {mso-style-unhide:no;");
                html.Append("            color:blue;");
                html.Append("            mso-themecolor:hyperlink;");
                html.Append("            text-decoration:underline;");
                html.Append("            text-underline:single;}");
                html.Append("         a:visited, span.MsoHyperlinkFollowed");
                html.Append("             {mso-style-unhide:no;");
                html.Append("             color:purple;");
                html.Append("             mso-themecolor:followedhyperlink;");
                html.Append("              text-decoration:underline;");
                html.Append("             text-underline:single;}");
                html.Append("         strong");
                html.Append("             {mso-style-unhide:no;");
                html.Append("             mso-style-qformat:yes;");
                html.Append("             mso-style-locked:yes;}");
                html.Append("         p.MsoDocumentMap, li.MsoDocumentMap, div.MsoDocumentMap");
                html.Append("             {mso-style-noshow:yes;");
                html.Append("             mso-style-priority:99;");
                html.Append("             mso-style-unhide:no;");
                html.Append("             mso-style-link:'Mapa del documento Car';");
                html.Append("              margin-top:0cm;");
                html.Append("             margin-right:0cm;");
                html.Append("             margin-bottom:10.0pt;");
                html.Append("             margin-left:0cm;");
                html.Append("              line-height:115%;");
                html.Append("              mso-pagination:widow-orphan;");
                html.Append("              background:navy;");
                html.Append("             font-size:10.0pt;");
                html.Append("              font-family:'Tahoma','sans-serif';");
                html.Append("              mso-fareast-font-family:'Times New Roman';");
                html.Append("              mso-fareast-language:EN-US;}");
                html.Append("         p");
                html.Append("             {mso-style-priority:99;");
                html.Append("             mso-style-unhide:no;");
                html.Append("             mso-margin-top-alt:auto;");
                html.Append("             margin-right:0cm;");
                html.Append("             mso-margin-bottom-alt:auto;");
                html.Append("             margin-left:0cm;");
                html.Append("             mso-pagination:widow-orphan;");
                html.Append("              font-size:12.0pt;");
                html.Append("              font-family:'Times New Roman','serif';");
                html.Append("              mso-fareast-font-family:'Times New Roman';}");
                html.Append("          p.MsoAcetate, li.MsoAcetate, div.MsoAcetate");
                html.Append("              {mso-style-noshow:yes;");
                html.Append("              mso-style-priority:99;");
                html.Append("              mso-style-unhide:no;");
                html.Append("              mso-style-link:'Texto de globo Car1';");
                html.Append("  margin:0cm;");
                html.Append("              margin-bottom:.0001pt;");
                html.Append("              mso-pagination:widow-orphan;");
                html.Append("              font-size:8.0pt;");
                html.Append("              font-family:'Tahoma','sans-serif';");
                html.Append("              mso-fareast-font-family:'Times New Roman';");
                html.Append("              mso-fareast-language:EN-US;}");
                html.Append("          span.MapadeldocumentoCar");
                html.Append("              {mso-style-name:'Mapa del documento Car';");
                html.Append("              mso-style-priority:99;");
                html.Append("              mso-style-unhide:no;");
                html.Append("              mso-style-locked:yes;");
                html.Append("              mso-style-link:'Mapa del documento';");
                html.Append("              mso-ansi-font-size:8.0pt;");
                html.Append("              mso-bidi-font-size:8.0pt;");
                html.Append("             font-family:'Tahoma','sans-serif';");
                html.Append("             mso-ascii-font-family:Tahoma;");
                html.Append("              mso-hansi-font-family:Tahoma;");
                html.Append("              mso-bidi-font-family:Tahoma;");
                html.Append("              mso-fareast-language:EN-US;}");
                html.Append("           span.TextodegloboCar");
                html.Append("              {mso-style-name:'Texto de globo Car';");
                html.Append("             mso-style-priority:99;");
                html.Append("             mso-style-unhide:no;");
                html.Append("              mso-style-locked:yes;");
                html.Append("              mso-style-link:'Texto de globo';");
                html.Append("              mso-ansi-font-size:8.0pt;");
                html.Append("              mso-bidi-font-size:8.0pt;");
                html.Append("              font-family:'Tahoma','sans-serif';");
                html.Append("              mso-ascii-font-family:Tahoma;");
                html.Append("              mso-hansi-font-family:Tahoma;");
                html.Append("              mso-bidi-font-family:Tahoma;");
                html.Append("              mso-fareast-language:EN-US;}");
                html.Append("          p.msonospacing0, li.msonospacing0, div.msonospacing0");
                html.Append("              {mso-style-name:msonospacing0;");
                html.Append("              mso-style-priority:1;");
                html.Append("              mso-style-unhide:no;");
                html.Append("              mso-style-qformat:yes;");
                html.Append("              mso-margin-top-alt:auto;");
                html.Append("              margin-right:0cm;");
                html.Append("              mso-margin-bottom-alt:auto;");
                html.Append("              margin-left:0cm;");
                html.Append("             mso-pagination:widow-orphan;");
                html.Append("             font-size:12.0pt;");
                html.Append("            font-family:'Times New Roman','serif';");
                html.Append("             mso-fareast-font-family:'Times New Roman';}");
                html.Append("          p.msolistparagraph0, li.msolistparagraph0, div.msolistparagraph0");
                html.Append("              {mso-style-name:msolistparagraph0;");
                html.Append("              mso-style-priority:34;");
                html.Append("              mso-style-unhide:no;");
                html.Append("              mso-style-qformat:yes;");
                html.Append("              mso-margin-top-alt:auto;");
                html.Append("              margin-right:0cm;");
                html.Append("              mso-margin-bottom-alt:auto;");
                html.Append("              margin-left:0cm;");
                html.Append("              mso-pagination:widow-orphan;");
                html.Append("              font-size:12.0pt;");
                html.Append("              font-family:'Times New Roman','serif';");
                html.Append("              mso-fareast-font-family:'Times New Roman';}");
                html.Append("           p.msolistparagraph0cxspfirst, li.msolistparagraph0cxspfirst, div.msolistparagraph0cxspfirst");
                html.Append("              {mso-style-name:msolistparagraph0cxspfirst;");
                html.Append("              mso-style-priority:34;");
                html.Append("              mso-style-unhide:no;");
                html.Append("              mso-style-qformat:yes;");
                html.Append("             mso-margin-top-alt:auto;");
                html.Append("             margin-right:0cm;");
                html.Append("             mso-margin-bottom-alt:auto;");
                html.Append("              margin-left:0cm;");
                html.Append("              mso-pagination:widow-orphan;");
                html.Append("              font-size:12.0pt;");
                html.Append("             font-family:'Times New Roman','serif';");
                html.Append("              mso-fareast-font-family:'Times New Roman';}");
                html.Append("          p.msolistparagraph0cxspmiddle, li.msolistparagraph0cxspmiddle, div.msolistparagraph0cxspmiddle");
                html.Append("              {mso-style-name:msolistparagraph0cxspmiddle;");
                html.Append("              mso-style-priority:34;");
                html.Append("              mso-style-unhide:no;");
                html.Append("              mso-style-qformat:yes;");
                html.Append("              mso-margin-top-alt:auto;");
                html.Append("              margin-right:0cm;");
                html.Append("              mso-margin-bottom-alt:auto;");
                html.Append("              margin-left:0cm;");
                html.Append("              mso-pagination:widow-orphan;");
                html.Append("              font-size:12.0pt;");
                html.Append("              font-family:'Times New Roman','serif';");
                html.Append("              mso-fareast-font-family:'Times New Roman';}");
                html.Append("           p.msolistparagraph0cxsplast, li.msolistparagraph0cxsplast, div.msolistparagraph0cxsplast");
                html.Append("              {mso-style-name:msolistparagraph0cxsplast;");
                html.Append("              mso-style-priority:34;");
                html.Append("              mso-style-unhide:no;");
                html.Append("              mso-style-qformat:yes;");
                html.Append("              mso-margin-top-alt:auto;");
                html.Append("              margin-right:0cm;");
                html.Append("              mso-margin-bottom-alt:auto;");
                html.Append("              margin-left:0cm;");
                html.Append("              mso-pagination:widow-orphan;");
                html.Append("              font-size:12.0pt;");
                html.Append("              font-family:'Times New Roman','serif';");
                html.Append("             mso-fareast-font-family:'Times New Roman';}");
                html.Append("        p.msonormalcxspmiddle, li.msonormalcxspmiddle, div.msonormalcxspmiddle");
                html.Append("             {mso-style-name:msonormalcxspmiddle;");
                html.Append("             mso-style-priority:99;");
                html.Append("              mso-style-unhide:no;");
                html.Append("              mso-margin-top-alt:auto;");
                html.Append("              margin-right:0cm;");
                html.Append("              mso-margin-bottom-alt:auto;");
                html.Append("              margin-left:0cm;");
                html.Append("             mso-pagination:widow-orphan;");
                html.Append("              font-size:12.0pt;");
                html.Append("              font-family:'Times New Roman','serif';");
                html.Append("              mso-fareast-font-family:'Times New Roman';}");
                html.Append("           p.Default, li.Default, div.Default");
                html.Append("              {mso-style-name:Default;");
                html.Append("              mso-style-priority:99;");
                html.Append("              mso-style-unhide:no;");
                html.Append("             mso-style-parent:'';");
                html.Append("              margin:0cm;");
                html.Append("              margin-bottom:.0001pt;");
                html.Append("             mso-pagination:widow-orphan;");
                html.Append("              mso-layout-grid-align:none;");
                html.Append("              text-autospace:none;");
                html.Append("              font-size:12.0pt;");
                html.Append("              font-family:'Arial','sans-serif';");
                html.Append("              mso-fareast-font-family:'Times New Roman';");
                html.Append("              color:black;}");
                html.Append("          p.msonospacing1, li.msonospacing1, div.msonospacing1");
                html.Append("              {mso-style-name:msonospacing;");
                html.Append("              mso-style-priority:99;");
                html.Append("              mso-style-unhide:no;");
                html.Append("              mso-style-parent:'';");
                html.Append("              margin:0cm;");
                html.Append("             margin-bottom:.0001pt;");
                html.Append("              mso-pagination:widow-orphan;");
                html.Append("              font-size:11.0pt;");
                html.Append("              font-family:'Calibri','sans-serif';");
                html.Append("             mso-fareast-font-family:Calibri;");
                html.Append("             mso-bidi-font-family:'Times New Roman';");
                html.Append("              mso-ansi-language:EN-US;");
                html.Append("              mso-fareast-language:EN-US;}");
                html.Append("          p.msolistparagraph1, li.msolistparagraph1, div.msolistparagraph1");
                html.Append("              {mso-style-name:msolistparagraph;");
                html.Append("              mso-style-priority:99;");
                html.Append("              mso-style-unhide:no;");
                html.Append("             margin-top:0cm;");
                html.Append("             margin-right:0cm;");
                html.Append("              margin-bottom:10.0pt;");
                html.Append("              margin-left:36.0pt;");
                html.Append("              mso-add-space:auto;");
                html.Append("              line-height:115%;");
                html.Append("              mso-pagination:widow-orphan;");
                html.Append("              font-size:11.0pt;");
                html.Append("              font-family:'Calibri','sans-serif';");
                html.Append("              mso-fareast-font-family:'Times New Roman';");
                html.Append("              mso-bidi-font-family:'Times New Roman';");
                html.Append("             mso-fareast-language:EN-US;}");
                html.Append("         p.msolistparagraph1CxSpFirst, li.msolistparagraph1CxSpFirst, div.msolistparagraph1CxSpFirst");
                html.Append("             {mso-style-name:msolistparagraphCxSpFirst;");
                html.Append("           mso-style-priority:99;");
                html.Append("             mso-style-unhide:no;");
                html.Append("              mso-style-type:export-only;");
                html.Append("              margin-top:0cm;");
                html.Append("              margin-right:0cm;");
                html.Append("              margin-bottom:0cm;");
                html.Append("              margin-left:36.0pt;");
                html.Append("             margin-bottom:.0001pt;");
                html.Append("            mso-add-space:auto;");
                html.Append("           line-height:115%;");
                html.Append("           mso-pagination:widow-orphan;");
                html.Append("            font-size:11.0pt;");
                html.Append("            font-family:'Calibri','sans-serif';");
                html.Append("            mso-fareast-font-family:'Times New Roman';");
                html.Append("             mso-bidi-font-family:'Times New Roman';");
                html.Append("             mso-fareast-language:EN-US;}");
                html.Append("        p.msolistparagraph1CxSpMiddle, li.msolistparagraph1CxSpMiddle, div.msolistparagraph1CxSpMiddle");
                html.Append("            {mso-style-name:msolistparagraphCxSpMiddle;");
                html.Append("            mso-style-priority:99;");
                html.Append("         mso-style-unhide:no;");
                html.Append("            mso-style-type:export-only;");
                html.Append("            margin-top:0cm;");
                html.Append("             margin-right:0cm;");
                html.Append("            margin-bottom:0cm;");
                html.Append("             margin-left:36.0pt;");
                html.Append("              margin-bottom:.0001pt;");
                html.Append("             mso-add-space:auto;");
                html.Append("             line-height:115%;");
                html.Append("             mso-pagination:widow-orphan;");
                html.Append("              font-size:11.0pt;");
                html.Append("             font-family:'Calibri','sans-serif';");
                html.Append("              mso-fareast-font-family:'Times New Roman';");
                html.Append("             mso-bidi-font-family:'Times New Roman';");
                html.Append("            mso-fareast-language:EN-US;}");
                html.Append("         p.msolistparagraph1CxSpLast, li.msolistparagraph1CxSpLast, div.msolistparagraph1CxSpLast");
                html.Append("             {mso-style-name:msolistparagraphCxSpLast;");
                html.Append("             mso-style-priority:99;");
                html.Append("              mso-style-unhide:no;");
                html.Append("              mso-style-type:export-only;");
                html.Append("              margin-top:0cm;");
                html.Append("              margin-right:0cm;");
                html.Append("             margin-bottom:10.0pt;");
                html.Append("             margin-left:36.0pt;");
                html.Append("            mso-add-space:auto;");
                html.Append("            line-height:115%;");
                html.Append("            mso-pagination:widow-orphan;");
                html.Append("            font-size:11.0pt;");
                html.Append("            font-family:'Calibri','sans-serif';");
                html.Append("            mso-fareast-font-family:'Times New Roman';");
                html.Append("            mso-bidi-font-family:'Times New Roman';");
                html.Append("            mso-fareast-language:EN-US;}");
                html.Append("        span.TextodegloboCar1");
                html.Append("            {mso-style-name:'Texto de globo Car1';");
                html.Append("            mso-style-noshow:yes;");
                html.Append("           mso-style-unhide:no;");
                html.Append("          mso-style-locked:yes;");
                html.Append("          mso-style-link:'Texto de globo';");
                html.Append("           mso-ansi-font-size:8.0pt;");
                html.Append("           mso-bidi-font-size:8.0pt;");
                html.Append("            font-family:'Tahoma','sans-serif';");
                html.Append("            mso-ascii-font-family:Tahoma;");
                html.Append("            mso-hansi-font-family:Tahoma;");
                html.Append("             mso-bidi-font-family:Tahoma;}");
                html.Append("         .MsoChpDefault");
                html.Append("            {mso-style-type:export-only;");
                html.Append("            mso-default-props:yes;");
                html.Append("             font-size:10.0pt;");
                html.Append("            mso-ansi-font-size:10.0pt;");
                html.Append("            mso-bidi-font-size:10.0pt;}");
                html.Append("        @page WordSection1");
                html.Append("            {size:612.0pt 792.0pt;");
                html.Append("            margin:72.0pt 54.0pt 72.0pt 54.0pt;");
                html.Append("            mso-header-margin:35.4pt;");
                html.Append("             mso-footer-margin:35.4pt;");
                html.Append("            mso-paper-source:0;}");
                html.Append("         div.WordSection1");
                html.Append("            {page:WordSection1;}");
                html.Append("         -->");
                html.Append("        </style>");



                html.Append("\n");
                html.Append("     </head>");

                html.Append("     <body lang=ES-MX link=blue vlink=purple style='tab-interval:35.4pt'>");

                html.Append("     <div class=WordSection1>");

                html.Append("      <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("      normal'><b><span style='font-size:8.0pt;font-family:'Arial','sans-serif';");
                html.Append("      color:gray'><o:p>&nbsp;</o:p></span></b></p>");

                html.Append("      <div align=center>");

                html.Append("      <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=718");
                html.Append("       style='border-collapse:collapse;mso-table-layout-alt:fixed;mso-yfti-tbllook:");
                html.Append("      1184;mso-padding-alt:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("       <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'>");
                html.Append("       <td width=146 rowspan=3 valign=top style='width:109.4pt;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("        <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("       normal'><b style='mso-bidi-font-weight:normal'><span style='color:gray;");
                html.Append("       mso-themecolor:background1;mso-themeshade:128'><img style='float:left;width:100px;height:100px;background-repeat: no-repeat;' src='" + rutaLogo + "'/><o:p></o:p></span></b></p>");
                html.Append("       </td>");

                html.Append("       <td  rowspan=1 colspan=2   style='width:290.95pt;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("       <p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("      text-align:center;line-height:normal'><b><span style='font-size:14.0pt;");
                html.Append("     mso-bidi-font-size:11.0pt;color:#404040;mso-themecolor:text1;mso-themetint:");
                html.Append("      191'>" + ValidarNull(Emisor.nombre) + "<o:p></o:p></span></b></p>");

                html.Append("     <p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("     text-align:center;line-height:normal'><b><span style='color:#404040;font-size:10pt;");
                html.Append("     mso-themecolor:text1;mso-themetint:191;'>R.F.C: <span style='mso-spacerun:yes;font-size:10pt;'></span> " + ValidarNull(Emisor.rfc) + "<o:p></o:p></span></b><b");
                html.Append("      style='mso-bidi-font-weight:normal'><span style='font-size:9.0pt;font-family:");
                html.Append("     'Arial','sans-serif';color:#404040;mso-themecolor:text1;mso-themetint:191'><o:p></o:p></span></b></p>");

                html.Append("    </td>");

                html.Append("    <td width=184 valign=top style='width:138.15pt;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("     <p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("      text-align:center;line-height:normal'><b style='mso-bidi-font-weight:normal'><span");
                html.Append("       style='font-size:9.0pt;color:#404040;");
                html.Append("       mso-themecolor:text1;mso-themetint:191'>FACTURA<o:p></o:p></span></b></p>");
                html.Append("      </td>");
                html.Append("      </tr>");

                html.Append("      <tr style='mso-yfti-irow:1'>");

                html.Append("<td width=388 rowspan=2 col style='width:290.95pt;padding:0cm 5.4pt 0cm 5.4pt;' >");
                html.Append("\n");
                html.Append("            <p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt; text-align:center;line-height:normal'>");
                html.Append("           <b><span style='color:#404040;mso-themecolor:text1;mso-themetint:191;font-size:8pt;'>MATRIZ</span></b>");
                html.Append("           <b style='mso-bidi-font-weight:normal'>");
                html.Append("           <span style='font-size:9.0pt;color:#404040;mso-themecolor:text1;mso-themetint:191'><o:p></o:p> </span>");
                html.Append("           </b>");

                html.Append("            </p>");


                html.Append("           <p class=Default align=center style='text-align:center'><span");
                html.Append("          style='font-size:8.0pt;color:#404040;mso-themecolor:text1;mso-themetint:191'>" + ValidarNull(Emisor.DomicilioFiscal.calle) + " No. " + ValidarNull(Emisor.DomicilioFiscal.noExterior) + " " + ValidarNull(Emisor.DomicilioFiscal.colonia) + " " + ValidarNull(Emisor.DomicilioFiscal.municipio) + " " + ValidarNull(Emisor.DomicilioFiscal.estado) + " C.P " + ValidarNull(Emisor.DomicilioFiscal.codigoPostal) + "</span>");
                //html.Append("          </span><span");
                html.Append("         <span style='font-size:9.0pt;color:#404040;mso-themecolor:text1;mso-themetint:191;");
                html.Append("        mso-fareast-language:EN-US'><o:p></o:p>");
                html.Append("         </span></p>");

                html.Append("         <p class=Default align=center style='text-align:center'><b><span");
                html.Append("         style='font-size:9.0pt;color:#404040;mso-themecolor:text1;mso-themetint:191'>Tel.");
                html.Append("          </span></b><span style='font-size:8.0pt;color:#404040;mso-themecolor:text1;");
                html.Append("       mso-themetint:191'>" + ValidarNull(matrizTel) + "</span> ");
                html.Append("         <span style='font-size:9.0pt;color:#404040;");
                html.Append("       mso-themecolor:text1;mso-themetint:191'><o:p></o:p></span></p>");


                html.Append("        </td>");


                html.Append("          <td width=388 rowspan=2>");
                html.Append("          <p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt; text-align:center;line-height:normal'>");
                html.Append("          <b><span style='color:#404040;mso-themecolor:text1;mso-themetint:191;font-size:8pt;'>" + ValidarNull(sucursal.Abr) + "</span></b>");
                html.Append("         <b style='mso-bidi-font-weight:normal'>");
                html.Append("          <span style='font-size:9.0pt;color:#404040;mso-themecolor:text1;mso-themetint:191'><o:p></o:p> </span>");
                html.Append("         </b>");

                html.Append("         </p>");


                html.Append("         <p class=Default align=center style='text-align:center'><span");
                html.Append("          style='font-size:8.0pt;color:#404040;mso-themecolor:text1;mso-themetint:191;font-size:8pt;'>");
                html.Append("          " + ValidarNull(Emisor.ExpedidoEn.calle) + " " + ValidarNull(Emisor.ExpedidoEn.noExterior) + " " + ValidarNull(Emisor.ExpedidoEn.colonia) + " " + ValidarNull(Emisor.ExpedidoEn.municipio) + " " + ValidarNull(Emisor.ExpedidoEn.localidad) + " " + ValidarNull(Emisor.ExpedidoEn.estado) + " CP: " + ValidarNull(Emisor.ExpedidoEn.codigoPostal) + " </span><span");
                html.Append("         style='font-size:9.0pt;color:#404040;mso-themecolor:text1;mso-themetint:191;");
                html.Append("          mso-fareast-language:EN-US'><o:p></o:p>");


                html.Append("           </span></p>");
                html.Append("         <p class=Default align=center style='text-align:center'><b><span");
                html.Append("         style='font-size:9.0pt;color:#404040;mso-themecolor:text1;mso-themetint:191'>");
                html.Append("          </span></b><span style='font-size:9.0pt;color:#404040;mso-themecolor:text1;");
                html.Append("       mso-themetint:191'></span> ");
                html.Append("         <span style='font-size:9.0pt;color:#404040;");
                html.Append("       mso-themecolor:text1;mso-themetint:191'><o:p></o:p></span></p>");

                html.Append("           </td>");





                html.Append("       <td width=184 valign=top style='width:138.15pt;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("       <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("      normal'><b style='mso-bidi-font-weight:normal'><span style='font-size:9.0pt;");
                html.Append("     font-family:'Arial','sans-serif';color:#404040;mso-themecolor:text1;");
                html.Append("       mso-themetint:191'>Serie:</span></b><span style='font-size:9.0pt;font-family:");
                html.Append("     'Arial','sans-serif';color:#404040;mso-themecolor:text1;mso-themetint:191'> </span><b");
                html.Append("     style='mso-bidi-font-weight:normal'><span style='mso-bidi-font-size:9.0pt;");
                html.Append("     font-family:Arial,sans-serif;color:#404040;mso-themecolor:text1;");
                html.Append("     mso-themetint:191'>" + ValidarNull(Factura.serie) + "</span></b><span style='font-size:9.0pt;");
                html.Append("     color:#404040;mso-themecolor:text1;");
                html.Append("    mso-themetint:191'><o:p></o:p></span></p>");
                html.Append("     <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("     normal'><b style='mso-bidi-font-weight:normal'><span style='font-size:9.0pt;");
                html.Append("     font-family:'Arial','sans-serif';color:#404040;mso-themecolor:text1;");
                html.Append("     mso-themetint:191'>Folio:</span></b><span style='font-size:9.0pt;font-family:");
                html.Append("    'Arial','sans-serif';color:#404040;mso-themecolor:text1;mso-themetint:191'> </span><b");
                html.Append("    style='mso-bidi-font-weight:normal'><span style='mso-bidi-font-size:9.0pt;");
                html.Append("   font-family:Arial,sans-serif;color:#404040;mso-themecolor:text1;");
                html.Append("     mso-themetint:191'>" + ValidarNull(Factura.folio) + "</span></b><span style='font-size:9.0pt;");
                html.Append("    color:#404040;mso-themecolor:text1;");
                html.Append("   mso-themetint:191'><o:p></o:p></span></p>");
                html.Append("    <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("    normal'><b style='mso-bidi-font-weight:normal'><span lang=EN-US");
                html.Append("     style='font-size:7.0pt;font-family:Arial,sans-serif;color:#404040;");
                html.Append("    mso-themecolor:text1;mso-themetint:191;mso-ansi-language:EN-US'>" + String.Format("{0:dd MMM yyyy hh:mm t}", Factura.fecha) + "<span");
                html.Append("    style='mso-spacerun:yes'>   </span><span style='mso-bidi-font-weight:bold'><o:p></o:p></span></span></b></p>");
                html.Append("     </td>");
                html.Append("     </tr>");
                html.Append("     <tr style='mso-yfti-irow:2;mso-yfti-lastrow:yes;height:21.65pt'>");
                html.Append("       <td width=184 style='width:138.15pt;padding:0cm 5.4pt 0cm 5.4pt;height:21.65pt'>");
                html.Append("     <p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("      text-align:center;line-height:normal'><b style='mso-bidi-font-weight:normal'><span");
                html.Append("      style='font-size:7.0pt;font-family:Arial,sans-serif;color:#404040;");
                html.Append("     mso-themecolor:text1;mso-themetint:191;'>" + ValidarNull(Factura.LugarExpedicion) + "<span");
                html.Append("     style='mso-bidi-font-weight:bold'><o:p></o:p></span></span></b></p>");
                html.Append("     </td>");
                html.Append("     </tr>");
                html.Append("   </table>");

                html.Append("    </div>");

                html.Append("   <p class=Default><span style='font-size:8.0pt;mso-bidi-font-size:9.5pt;");
                html.Append("     color:gray'><o:p>&nbsp;</o:p></span></p>");

                html.Append("     <div align=center>");

                html.Append("     <table class=MsoTableGrid border=1 cellspacing=0 cellpadding=0 width=718");
                html.Append("     style='width:19.0cm;border-collapse:collapse;border:none;mso-border-alt:solid #A6A6A6 .5pt;");
                html.Append("    mso-border-themecolor:background1;mso-border-themeshade:166;mso-yfti-tbllook:");
                html.Append("    1184;mso-padding-alt:0cm 5.4pt 0cm 5.4pt;mso-border-insideh:.5pt solid #A6A6A6;");
                html.Append("     mso-border-insideh-themecolor:background1;mso-border-insideh-themeshade:166;");
                html.Append("    mso-border-insidev:.5pt solid #A6A6A6;mso-border-insidev-themecolor:background1;");
                html.Append("     mso-border-insidev-themeshade:166'>");
                html.Append("      <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes;height:14.75pt'>");
                html.Append("     <td width=451 style='width:338.55pt;border:solid #A6A6A6 1.0pt;mso-border-themecolor:");
                html.Append("     background1;mso-border-themeshade:166;mso-border-alt:solid #A6A6A6 .5pt;");
                html.Append("     mso-border-themecolor:background1;mso-border-themeshade:166;background:#D9D9D9;");
                html.Append("     mso-background-themecolor:background1;mso-background-themeshade:217;");
                html.Append("    padding:0cm 5.4pt 0cm 5.4pt;height:14.75pt'>");
                html.Append("    <p class=Default><b style='mso-bidi-font-weight:normal'><span");
                html.Append("   style='font-size:8.0pt;color:#262626'>Receptor del Comprobante Fiscal<o:p></o:p></span></b></p>");
                html.Append("     </td>");
                html.Append("    <td width=235 style='width:176.25pt;border:solid #A6A6A6 1.0pt;mso-border-themecolor:");
                html.Append("     background1;mso-border-themeshade:166;border-left:none;mso-border-left-alt:");
                html.Append("    solid #A6A6A6 .5pt;mso-border-left-themecolor:background1;mso-border-left-themeshade:");
                html.Append("   166;mso-border-alt:solid #A6A6A6 .5pt;mso-border-themecolor:background1;");
                html.Append("    mso-border-themeshade:166;background:#D9D9D9;mso-background-themecolor:background1;");
                html.Append("    mso-background-themeshade:217;padding:0cm 5.4pt 0cm 5.4pt;height:14.75pt'>");
                html.Append("    <p class=Default align=center style='text-align:center'><b style='mso-bidi-font-weight:");
                html.Append("     normal'><span style='font-size:7.0pt;mso-bidi-font-size:8.0pt;color:#262626'>Folio");
                html.Append("    Fiscal<o:p></o:p></span></b></p>");
                html.Append("    </td>");
                html.Append("     </tr>");
                html.Append("     <tr style='mso-yfti-irow:1;height:14.75pt'>");
                html.Append("     <td width=451 rowspan=6 style='width:338.55pt;border:solid #A6A6A6 1.0pt;");
                html.Append("    mso-border-themecolor:background1;mso-border-themeshade:166;border-top:none;");
                html.Append("    mso-border-top-alt:solid #A6A6A6 .5pt;mso-border-top-themecolor:background1;");
                html.Append("   mso-border-top-themeshade:166;mso-border-alt:solid #A6A6A6 .5pt;mso-border-themecolor:");
                html.Append("    background1;mso-border-themeshade:166;padding:0cm 5.4pt 0cm 5.4pt;height:");
                html.Append("   14.75pt'>");
                html.Append("    <p class=Default><span style='font-size:9.0pt;mso-bidi-font-size:8.0pt;");
                html.Append("    color:#595959'>NOMBRE: " + ValidarNull(Factura.Receptor.nombre) + "<span style='mso-bidi-font-weight:");
                html.Append("     bold'><o:p></o:p></span></span></p>");
                html.Append("     <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("     normal'><span style='font-size:9.0pt;mso-bidi-font-size:8.0pt;");
                html.Append("     color:#595959'>DIRECCION : " + ValidarNull(Factura.Receptor.Domicilio.calle) + "  #" + ValidarNull(Factura.Receptor.Domicilio.noExterior) + " Int. " + ValidarNull(NumInt));
                html.Append("     " + ValidarNull(Factura.Receptor.Domicilio.colonia) + "<span style='mso-spacerun:yes'>   </span>C.P.");
                html.Append("     " + ValidarNull(Factura.Receptor.Domicilio.codigoPostal) + "<span style='mso-spacerun:yes'>      </span><o:p></o:p></span></p>");
                html.Append("    <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("    normal'><span style='font-size:9.0pt;mso-bidi-font-size:8.0pt;");
                html.Append("   color:#595959'>CIUDAD: " + ValidarNull(Factura.Receptor.Domicilio.municipio) + "<span");
                html.Append("    style='mso-spacerun:yes'>    </span>" + ValidarNull(Factura.Receptor.Domicilio.estado) + "<span");
                html.Append("    style='mso-spacerun:yes'>        </span><o:p></o:p></span></p>");
                html.Append("    <p class=MsoNormal style='line-height:normal'><span style='font-size:9.0pt;");
                html.Append("    mso-bidi-font-size:8.0pt;mso-bidi-font-family:Calibri;color:#595959'>R.F.C.<span");
                html.Append("    style='mso-spacerun:yes'>  </span>" + ValidarNull(Factura.Receptor.rfc) + " <span");
                html.Append("    style='mso-spacerun:yes'>     </span>TELEFONO: </span><span style='font-size:");
                html.Append("   10.0pt;mso-bidi-font-size:9.5pt;mso-bidi-font-family:Calibri;color:#595959'></span><span");
                html.Append("  style='font-size:10.0pt;mso-bidi-font-size:9.5pt;mso-bidi-font-family:Calibri;");
                html.Append("    color:gray'> </span><b style='mso-bidi-font-weight:normal'><span");
                html.Append("    style='font-size:8.0pt;color:gray'><o:p></o:p></span></b></p>");
                html.Append("    </td>");
                html.Append("     <td width=235 style='width:176.25pt;border-top:none;border-left:none;");
                html.Append("     border-bottom:solid #A6A6A6 1.0pt;mso-border-bottom-themecolor:background1;");
                html.Append("     mso-border-bottom-themeshade:166;border-right:solid #A6A6A6 1.0pt;mso-border-right-themecolor:");
                html.Append("     background1;mso-border-right-themeshade:166;mso-border-top-alt:solid #A6A6A6 .5pt;");
                html.Append("     mso-border-top-themecolor:background1;mso-border-top-themeshade:166;");
                html.Append("     mso-border-left-alt:solid #A6A6A6 .5pt;mso-border-left-themecolor:background1;");
                html.Append("     mso-border-left-themeshade:166;mso-border-alt:solid #A6A6A6 .5pt;mso-border-themecolor:");
                html.Append("    background1;mso-border-themeshade:166;padding:0cm 5.4pt 0cm 5.4pt;height:");
                html.Append("     14.75pt'>");
                html.Append("    <p class=Default align=center style='text-align:center'><span");
                html.Append("    style='font-size:7.0pt;mso-bidi-font-size:8.0pt;font-family:Courier New;");
                html.Append("    color:#595959'>" + UUID + "</span><span style='font-size:7.0pt;mso-bidi-font-size:");
                html.Append("    8.0pt;color:#595959'><o:p></o:p></span></p>");
                html.Append("     </td>");
                html.Append("    </tr>");
                html.Append("    <tr style='mso-yfti-irow:2;height:14.6pt'>");
                html.Append("    <td width=235 style='width:176.25pt;border-top:none;border-left:none;");
                html.Append("     border-bottom:solid #A6A6A6 1.0pt;mso-border-bottom-themecolor:background1;");
                html.Append("    mso-border-bottom-themeshade:166;border-right:solid #A6A6A6 1.0pt;mso-border-right-themecolor:");
                html.Append("    background1;mso-border-right-themeshade:166;mso-border-top-alt:solid #A6A6A6 .5pt;");
                html.Append("    mso-border-top-themecolor:background1;mso-border-top-themeshade:166;");
                html.Append("    mso-border-left-alt:solid #A6A6A6 .5pt;mso-border-left-themecolor:background1;");
                html.Append("   mso-border-left-themeshade:166;mso-border-alt:solid #A6A6A6 .5pt;mso-border-themecolor:");
                html.Append("   background1;mso-border-themeshade:166;background:#EEECE1;mso-background-themecolor:");
                html.Append("    background2;padding:0cm 5.4pt 0cm 5.4pt;height:14.6pt'>");
                html.Append("    <p class=Default align=center style='text-align:center'><b style='mso-bidi-font-weight:");
                html.Append("     normal'><span style='font-size:7.0pt;mso-bidi-font-size:8.0pt;color:#262626'>No.");
                html.Append("    Certificado Digital<o:p></o:p></span></b></p>");
                html.Append("     </td>");
                html.Append("     </tr>");
                html.Append("     <tr style='mso-yfti-irow:3;height:14.6pt'>");
                html.Append("     <td width=235 style='width:176.25pt;border-top:none;border-left:none;");
                html.Append("    border-bottom:solid #A6A6A6 1.0pt;mso-border-bottom-themecolor:background1;");
                html.Append("   mso-border-bottom-themeshade:166;border-right:solid #A6A6A6 1.0pt;mso-border-right-themecolor:");
                html.Append("    background1;mso-border-right-themeshade:166;mso-border-top-alt:solid #A6A6A6 .5pt;");
                html.Append("    mso-border-top-themecolor:background1;mso-border-top-themeshade:166;");
                html.Append("    mso-border-left-alt:solid #A6A6A6 .5pt;mso-border-left-themecolor:background1;");
                html.Append("    mso-border-left-themeshade:166;mso-border-alt:solid #A6A6A6 .5pt;mso-border-themecolor:");
                html.Append("   background1;mso-border-themeshade:166;padding:0cm 5.4pt 0cm 5.4pt;height:");
                html.Append("     14.6pt'>");
                html.Append("    <p class=Default align=center style='text-align:center'><span");
                html.Append("    style='font-size:7.0pt;mso-bidi-font-size:8.0pt;font-family:Courier New;color:#595959'>" + NoCertificadoCSD + "<o:p></o:p></span></p>");
                html.Append("    </td>");
                html.Append("   </tr>");
                html.Append("  <tr style='mso-yfti-irow:4;height:14.6pt'>");
                html.Append("   <td width=235 style='width:176.25pt;border-top:none;border-left:none;");
                html.Append("   border-bottom:solid #A6A6A6 1.0pt;mso-border-bottom-themecolor:background1;");
                html.Append("   mso-border-bottom-themeshade:166;border-right:solid #A6A6A6 1.0pt;mso-border-right-themecolor:");
                html.Append("   background1;mso-border-right-themeshade:166;mso-border-top-alt:solid #A6A6A6 .5pt;");
                html.Append("  mso-border-top-themecolor:background1;mso-border-top-themeshade:166;");
                html.Append("    mso-border-left-alt:solid #A6A6A6 .5pt;mso-border-left-themecolor:background1;");
                html.Append("   mso-border-left-themeshade:166;mso-border-alt:solid #A6A6A6 .5pt;mso-border-themecolor:");
                html.Append("   background1;mso-border-themeshade:166;background:#EEECE1;mso-background-themecolor:");
                html.Append("   background2;padding:0cm 5.4pt 0cm 5.4pt;height:14.6pt'>");
                html.Append("   <p class=Default align=center style='text-align:center'><strong><span");
                html.Append("   style='font-size:7.0pt;mso-bidi-font-size:8.0pt;color:#262626;mso-bidi-font-weight:");
                html.Append("    normal'>No de Serie del Certificado del SAT</span></strong><span");
                html.Append("    style='font-size:7.0pt;mso-bidi-font-size:8.0pt;color:#262626'><o:p></o:p></span></p>");
                html.Append("   </td>");
                html.Append("   </tr>");
                html.Append("   <tr style='mso-yfti-irow:5;height:13.25pt'>");
                html.Append("    <td width=235 style='width:176.25pt;border-top:none;border-left:none;");
                html.Append("   border-bottom:solid #A6A6A6 1.0pt;mso-border-bottom-themecolor:background1;");
                html.Append("   mso-border-bottom-themeshade:166;border-right:solid #A6A6A6 1.0pt;mso-border-right-themecolor:");
                html.Append("    background1;mso-border-right-themeshade:166;mso-border-top-alt:solid #A6A6A6 .5pt;");
                html.Append("    mso-border-top-themecolor:background1;mso-border-top-themeshade:166;");
                html.Append("    mso-border-left-alt:solid #A6A6A6 .5pt;mso-border-left-themecolor:background1;");
                html.Append("   mso-border-left-themeshade:166;mso-border-alt:solid #A6A6A6 .5pt;mso-border-themecolor:");
                html.Append("    background1;mso-border-themeshade:166;padding:0cm 5.4pt 0cm 5.4pt;height:");
                html.Append("     13.25pt'>");
                html.Append("    <p class=Default align=center style='text-align:center'><span");
                html.Append("    style='font-size:7.0pt;mso-bidi-font-size:8.0pt;mso-bidi-font-family:Times New Roman;font-family:Courier New;");
                html.Append("   color:#595959'>" + NoCertSAT + "</span><span style='font-size:7.0pt;");
                html.Append("   mso-bidi-font-size:8.0pt;color:#595959'><o:p></o:p></span></p>");
                html.Append("   </td>");
                html.Append("    </tr>");
                html.Append("    <tr style='mso-yfti-irow:6;mso-yfti-lastrow:yes;height:13.25pt'>");
                html.Append("     <td width=235 style='width:176.25pt;border-top:none;border-left:none;");
                html.Append("    border-bottom:solid #A6A6A6 1.0pt;mso-border-bottom-themecolor:background1;");
                html.Append("    mso-border-bottom-themeshade:166;border-right:solid #A6A6A6 1.0pt;mso-border-right-themecolor:");
                html.Append("    background1;mso-border-right-themeshade:166;mso-border-top-alt:solid #A6A6A6 .5pt;");
                html.Append("   mso-border-top-themecolor:background1;mso-border-top-themeshade:166;");
                html.Append("    mso-border-left-alt:solid #A6A6A6 .5pt;mso-border-left-themecolor:background1;");
                html.Append("    mso-border-left-themeshade:166;mso-border-alt:solid #A6A6A6 .5pt;mso-border-themecolor:");
                html.Append("    background1;mso-border-themeshade:166;padding:0cm 5.4pt 0cm 5.4pt;height:");
                html.Append("   13.25pt'>");
                html.Append("    <p class=Default align=center style='text-align:center'><strong><span");
                html.Append("   style='font-size:7.0pt;mso-bidi-font-size:8.0pt;color:gray;mso-bidi-font-weight:");
                html.Append("    normal'>Fecha/hora certificación: </span></strong><span style='font-size:");
                html.Append("    7.0pt;mso-bidi-font-size:8.0pt;mso-bidi-font-family:Times New Roman;font-family:Times New Roman;");
                html.Append("     color:#595959'>" + fechaCert + "</span><span style='font-size:7.0pt;mso-bidi-font-size:");
                html.Append("    8.0pt;color:gray'><o:p></o:p></span></p>");
                html.Append("    </td>");
                html.Append("   </tr>");
                html.Append("   </table>");

                html.Append("   </div>");

                html.Append("   <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("    normal'><b style='mso-bidi-font-weight:normal'><span style='font-size:8.0pt;");
                html.Append("    font-family:'Arial','sans-serif';color:gray'><o:p>&nbsp;</o:p></span></b></p>");

                html.Append("      <div align=center>");

                html.Append("     <table class=MsoTableGrid border=1 cellspacing=0 cellpadding=0 width=718");
                html.Append("     style='border-collapse:collapse;mso-table-layout-alt:fixed;border:none;");
                html.Append("     mso-border-alt:solid windowtext .5pt;mso-yfti-tbllook:1184;mso-padding-alt:");
                html.Append("      0cm 5.4pt 0cm 5.4pt;mso-border-insideh:none;mso-border-insidev:none'>");
                html.Append("    <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'>");
                html.Append("     <td width=70 valign=top style='width:52.7pt;border-top:solid windowtext 1.0pt;");
                html.Append("    border-left:solid windowtext 1.0pt;border-bottom:none;border-right:none;");
                html.Append("      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;");
                html.Append("     background:black;mso-background-themecolor:text1;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("     <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("     normal'><b style='mso-bidi-font-weight:normal'><span style='font-size:8.0pt;");
                html.Append("    mso-bidi-font-size:10.0pt;color:white;");
                html.Append("    mso-themecolor:background1;color:white;'>CANTIDAD<o:p></o:p></span></b></p>");
                html.Append("    </td>");
                html.Append("   <td width=76 valign=top style='width:2.0cm;border:none;border-top:solid windowtext 1.0pt;");
                html.Append("   mso-border-top-alt:solid windowtext .5pt;background:black;mso-background-themecolor:");
                html.Append("    text1;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("     <p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("     text-align:center;line-height:normal'><b style='mso-bidi-font-weight:normal'><span");
                html.Append("    style='font-size:8.0pt;mso-bidi-font-size:10.0pt;");
                html.Append("     color:white;mso-themecolor:background1;color:white;'>UNIDAD<o:p></o:p></span></b></p>");
                html.Append("    </td>");
                html.Append("    <td width=378 valign=top style='width:10.0cm;border:none;border-top:solid windowtext 1.0pt;");
                html.Append("   mso-border-top-alt:solid windowtext .5pt;background:black;mso-background-themecolor:");
                html.Append("  text1;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("   <p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("  text-align:center;line-height:normal'><b style='mso-bidi-font-weight:normal'><span");
                html.Append("   style='font-size:8.0pt;mso-bidi-font-size:10.0pt;");
                html.Append("  color:white;mso-themecolor:background1;color:white;'>CONCEPTO<o:p></o:p></span></b></p>");
                html.Append("   </td>");
                html.Append("   <td width=97 valign=top style='width:72.45pt;border:none;border-top:solid windowtext 1.0pt;");
                html.Append("   mso-border-top-alt:solid windowtext .5pt;background:black;mso-background-themecolor:");
                html.Append("   text1;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("    <p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("    text-align:center;line-height:normal'><b style='mso-bidi-font-weight:normal'><span");
                html.Append("   style='font-size:8.0pt;mso-bidi-font-size:10.0pt;");
                html.Append("   color:white;mso-themecolor:background1;color:white;'>P.UNITARIO<o:p></o:p></span></b></p>");
                html.Append("   </td>");
                html.Append("  <td width=98 valign=top style='width:73.15pt;border-top:solid windowtext 1.0pt;");
                html.Append("  border-left:none;border-bottom:none;border-right:solid windowtext 1.0pt;");
                html.Append("  mso-border-top-alt:solid windowtext .5pt;mso-border-right-alt:solid windowtext .5pt;");
                html.Append("  background:black;mso-background-themecolor:text1;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("  <p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("  text-align:center;line-height:normal'><b style='mso-bidi-font-weight:normal'><span");
                html.Append("   style='font-size:8.0pt;mso-bidi-font-size:10.0pt;");
                html.Append("  color:white;mso-themecolor:background1;'>IMPORTE<o:p></o:p></span></b></p>");
                html.Append("   </td>");
                html.Append("  </tr>");
                html.Append("   <tr style='mso-yfti-irow:1'>");
                html.Append("   <td width=70 valign=top style='width:52.7pt;border:none;border-left:solid windowtext 1.0pt;");
                html.Append("   mso-border-left-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("  <p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("  text-align:center;line-height:normal'><span style='font-size:8.0pt;");
                html.Append("   font-family:'Arial','sans-serif';color:black;mso-themecolor:text1;margin-top:2px;'>" + Factura.Conceptos[0].cantidad + "<o:p></o:p></span></p>");
                html.Append("   </td>");
                html.Append("  <td width=76 valign=top style='width:2.0cm;border:none;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("   <p class=Default><span style='font-size:8.0pt;color:black;mso-themecolor:");
                html.Append("  text1'>" + Factura.Conceptos[0].unidad + "<span style='mso-bidi-font-weight:bold'><o:p></o:p></span></span></p>");
                html.Append("  </td>");
                html.Append("  <td width=378 valign=top style='width:10.0cm;border:none;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append(" <p class=Default><span style='font-size:8.0pt;color:black;mso-themecolor:");
                html.Append("  text1;mso-bidi-font-weight:bold'>" + ValidarNull(Factura.Conceptos[0].descripcion) + " </span><span");
                html.Append("  style='font-size:8.0pt;color:black;mso-themecolor:text1'><span");
                html.Append("  style='mso-bidi-font-weight:bold'><o:p></o:p></span></span></p>");
                html.Append("   </td>");
                html.Append("    <td width=97 valign=top style='width:72.45pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("   <p class=MsoNormal align=right style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("    text-align:right;line-height:normal'><span style='font-size:8.0pt;font-family:");
                html.Append("  'Arial','sans-serif';color:black;mso-themecolor:text1'>" + Factura.Conceptos[0].valorUnitario.ToString(CultureInfo.InvariantCulture) + "<o:p></o:p></span></p>");
                html.Append("    </td>");
                html.Append("   <td width=98 valign=top style='width:73.15pt;border:none;border-right:solid windowtext 1.0pt;");
                html.Append("    mso-border-right-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("   <p class=MsoNormal align=right style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("   text-align:right;line-height:normal'><span style='font-size:8.0pt;font-family:");
                html.Append("    'Arial','sans-serif';color:black;mso-themecolor:text1'>" + Factura.Conceptos[0].importe.ToString(CultureInfo.InvariantCulture) + "<o:p></o:p></span></p>");
                html.Append("    </td>");
                html.Append("   </tr>");
                html.Append("   <tr style='mso-yfti-irow:2;mso-yfti-lastrow:yes'>");
                html.Append("    <td width=70 valign=top style='width:52.7pt;border-top:none;border-left:solid windowtext 1.0pt;");
                html.Append("    border-bottom:solid windowtext 1.0pt;border-right:none;mso-border-left-alt:");
                html.Append("    solid windowtext .5pt;mso-border-bottom-alt:solid windowtext .5pt;padding:");
                html.Append("    0cm 5.4pt 0cm 5.4pt'>");
                html.Append("    <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("   normal'><span style='font-size:8.0pt;");
                html.Append("   color:gray'><o:p>&nbsp;</o:p></span></p>");
                html.Append("  </td>");
                html.Append("    <td width=454 colspan=2 valign=top style='width:12.0cm;border:none;");
                html.Append("    border-bottom:solid windowtext 1.0pt;mso-border-bottom-alt:solid windowtext .5pt;");
                html.Append("   padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("   <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("   normal'><span style='font-size:8.0pt;");
                html.Append("   color:gray'><o:p>&nbsp;</o:p></span></p>");
                html.Append("   <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("   normal'><span style='font-size:8.0pt;");
                html.Append("   color:gray'><o:p>&nbsp;</o:p></span></p>");
                html.Append("   <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("   normal'><span style='font-size:8.0pt;");
                html.Append("   color:gray'><o:p>&nbsp;</o:p></span></p>");
                html.Append("  </td>");
                html.Append("   <td width=97 valign=top style='width:72.45pt;border:none;border-bottom:solid windowtext 1.0pt;");
                html.Append("    mso-border-bottom-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("    <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("    normal'><span style='font-size:8.0pt;");
                html.Append("    color:gray'><o:p>&nbsp;</o:p></span></p>");
                html.Append("    </td>");
                html.Append("    <td width=98 valign=top style='width:73.15pt;border-top:none;border-left:");
                html.Append("    none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;");
                html.Append("    mso-border-bottom-alt:solid windowtext .5pt;mso-border-right-alt:solid windowtext .5pt;");
                html.Append("    padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("    <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("    normal'><span style='font-size:8.0pt;");
                html.Append("     color:gray'><o:p>&nbsp;</o:p></span></p>");
                html.Append("     </td>");
                html.Append("    </tr>");
                html.Append("    </table>");

                html.Append("    </div>");

                html.Append("   <p class=MsoNormal align=center style='text-align:center;tab-stops:45.1pt'><span");
                html.Append("   style='font-size:4.0pt;mso-bidi-font-size:1.0pt;line-height:115%;");
                html.Append("   color:#EEECE1;mso-themecolor:background2'>");
                html.Append("    <o:p></o:p></span></p>");

                html.Append("   <div align=center>");

                html.Append("    <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=718");
                html.Append("     style='width:19.0cm;border-collapse:collapse;mso-yfti-tbllook:160;mso-padding-alt:");
                html.Append("     0cm 5.4pt 0cm 5.4pt'>");
                html.Append("    <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'>");
                html.Append("     <td width=428 rowspan=7 valign=top style='width:321.3pt;border:solid #A6A6A6 1.0pt;");
                html.Append("      mso-border-themecolor:background1;mso-border-themeshade:166;mso-border-alt:");
                html.Append("     solid #A6A6A6 .5pt;mso-border-themecolor:background1;mso-border-themeshade:");
                html.Append("     166;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("     <p class=Default><b style='mso-bidi-font-weight:normal'><span");
                html.Append("      style='font-size:9.5pt;color:#404040;mso-themecolor:text1;mso-themetint:191'>Observación: </span></b><span");
                html.Append("      style='font-size:8.0pt;color:#404040;mso-themecolor:text1;mso-themetint:191'>  Fecha de Consumo:  " + fechaVta + ", Ticket " + Factura.Conceptos[0].noIdentificacion);
                html.Append("      <o:p></o:p></span></p>");
                html.Append("     <p class=Default><span style='font-size:8.0pt;color:#404040;mso-themecolor:");
                html.Append("      text1;mso-themetint:191'></span><span");
                html.Append("     style='font-size:9.5pt;color:gray'><o:p></o:p></span></p>");
                html.Append("      </td>");
                html.Append("      <td width=146 valign=top style='width:109.35pt;border:none;border-top:solid #A6A6A6 1.0pt;");
                html.Append("      mso-border-top-themecolor:background1;mso-border-top-themeshade:166;");
                html.Append("       mso-border-left-alt:solid windowtext .5pt;mso-border-top-alt:solid #A6A6A6 .5pt;");
                html.Append("      mso-border-top-themecolor:background1;mso-border-top-themeshade:166;");
                html.Append("     mso-border-left-alt:solid windowtext .5pt;background:white;mso-background-themecolor:");
                html.Append("    background1;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("     <p class=MsoNormal align=right style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("    text-align:right;line-height:normal'><b style='mso-bidi-font-weight:normal'><span");
                html.Append("     style='font-size:10.0pt;color:gray'>SUB TOTAL:<o:p></o:p></span></b></p>");
                html.Append("     </td>");
                html.Append("     <td width=112 valign=top style='width:84.15pt;border-top:solid #A6A6A6 1.0pt;");
                html.Append("      mso-border-top-themecolor:background1;mso-border-top-themeshade:166;");
                html.Append("     border-left:none;border-bottom:none;border-right:solid #A6A6A6 1.0pt;");
                html.Append("    mso-border-right-themecolor:background1;mso-border-right-themeshade:166;");
                html.Append("  mso-border-top-alt:solid #A6A6A6 .5pt;mso-border-top-themecolor:background1;");
                html.Append("    mso-border-top-themeshade:166;mso-border-right-alt:solid #A6A6A6 .5pt;");
                html.Append("  mso-border-right-themecolor:background1;mso-border-right-themeshade:166;");
                html.Append("    padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("   <p class=MsoNormal align=right style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("   text-align:right;line-height:normal'><span style='font-size:10.0pt;");
                html.Append("    color:#595959'>" + Factura.subTotal.ToString(CultureInfo.InvariantCulture) + "<o:p></o:p></span></p>");
                html.Append("    </td>");
                html.Append("   </tr>");

                html.Append("  </tr>");
                html.Append("   <tr style='mso-yfti-irow:3'>");
                html.Append("   <td width=146 valign=top style='width:109.35pt;border:none;mso-border-left-alt:");
                html.Append("   solid windowtext .5pt;background:white;mso-background-themecolor:background1;");
                html.Append("   padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("   <p align=right style='text-align:right'><b><span style='font-size:10.0pt;");
                html.Append("  font-family:'Calibri','sans-serif';color:gray'>IVA:</span></b><b");
                html.Append("   style='mso-bidi-font-weight:normal'><span style='font-size:10.0pt;font-family:");
                html.Append("  'Calibri','sans-serif';color:gray'><o:p></o:p></span></b></p>");
                html.Append("   </td>");
                html.Append("  <td width=112 valign=top style='width:84.15pt;border:none;border-right:solid #A6A6A6 1.0pt;");
                html.Append("   mso-border-right-themecolor:background1;mso-border-right-themeshade:166;");
                html.Append("  mso-border-right-alt:solid #A6A6A6 .5pt;mso-border-right-themecolor:background1;");
                html.Append("   mso-border-right-themeshade:166;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("   <p class=MsoNormal align=right style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("  text-align:right;line-height:normal'><span style='color:#595959'>" + Factura.Impuestos.Traslados[0].importe.ToString(CultureInfo.InvariantCulture) + "<o:p></o:p></span></p>");
                html.Append("    </td>");

                html.Append("  <tr style='mso-yfti-irow:6;mso-yfti-lastrow:yes'>");
                html.Append("    <td width=146 valign=top style='width:109.35pt;border:none;border-bottom:");
                html.Append("   solid #A6A6A6 1.0pt;mso-border-bottom-themecolor:background1;mso-border-bottom-themeshade:");
                html.Append("   166;mso-border-left-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;");
                html.Append("  mso-border-bottom-alt:solid #A6A6A6 .5pt;mso-border-bottom-themecolor:background1;");
                html.Append(" mso-border-bottom-themeshade:166;background:black;mso-background-themecolor:");
                html.Append("  text1;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("  <p class=MsoNormal align=right style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("  text-align:right;line-height:normal'><b style='mso-bidi-font-weight:normal'><span");
                html.Append("   style='font-size:10.0pt;color:white;mso-themecolor:background1'>TOTAL:<o:p></o:p></span></b></p>");
                html.Append("   </td>");
                html.Append("  <td width=112 valign=top style='width:84.15pt;border-top:none;border-left:");
                html.Append("   none;border-bottom:solid #A6A6A6 1.0pt;mso-border-bottom-themecolor:background1;");
                html.Append("   mso-border-bottom-themeshade:166;border-right:solid #A6A6A6 1.0pt;mso-border-right-themecolor:");
                html.Append("   background1;mso-border-right-themeshade:166;mso-border-bottom-alt:solid #A6A6A6 .5pt;");
                html.Append("  mso-border-bottom-themecolor:background1;mso-border-bottom-themeshade:166;");
                html.Append("   mso-border-right-alt:solid #A6A6A6 .5pt;mso-border-right-themecolor:background1;");
                html.Append("  mso-border-right-themeshade:166;background:#D9D9D9;mso-background-themecolor:");
                html.Append("  background1;mso-background-themeshade:217;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("   <p class=MsoNormal align=right style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("  text-align:right;line-height:normal'><span style='font-size:10.0pt;");
                html.Append("   color:#262626'>" + Factura.total.ToString(CultureInfo.InvariantCulture) + "<o:p></o:p></span></p>");
                html.Append("   </td>");
                html.Append("  </tr>");
                html.Append("   </table>");

                html.Append("   </div>");

                html.Append("  <p class=MsoNormal style='mso-outline-level:1'><b><span style='font-size:1.0pt;");
                html.Append("  line-height:115%;font-family:'Arial','sans-serif';color:gray'><o:p>&nbsp;</o:p></span></b></p>");

                html.Append("  <div align=center>");

                html.Append(" <table class=MsoTableGrid border=1 cellspacing=0 cellpadding=0 width=718");
                html.Append("   style='width:19.0cm;border-collapse:collapse;border:none;mso-border-alt:solid #A6A6A6 .5pt;");
                html.Append("  mso-border-themecolor:background1;mso-border-themeshade:166;mso-yfti-tbllook:");
                html.Append("   1184;mso-padding-alt:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("  <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'>");
                html.Append("  <td width=168 rowspan=5 valign=top style='width:125.9pt;border:solid #A6A6A6 1.0pt;");
                html.Append("   mso-border-themecolor:background1;mso-border-themeshade:166;mso-border-alt:");
                html.Append("   solid #A6A6A6 .5pt;mso-border-themecolor:background1;mso-border-themeshade:");
                html.Append("   166;background:white;mso-background-themecolor:background1;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("  <p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("   text-align:center;tab-stops:45.1pt'><b><span style='font-size:10.0pt;");
                html.Append("   line-height:115%;font-family:'Arial','sans-serif';color:gray'><img style='float:left;width:100px;height:100px;background-repeat: no-repeat;'  src='" + rutaQR + "'/></span></b><b");
                html.Append("   style='mso-bidi-font-weight:normal'><span style='font-size:8.0pt;line-height:");
                html.Append("  115%;font-family:'Arial','sans-serif';color:white;mso-themecolor:background1'><o:p></o:p></span></b></p>");
                html.Append("    </td>");
                html.Append("   <td width=519 style='width:388.9pt;border:solid #A6A6A6 1.0pt;mso-border-themecolor:");
                html.Append("   background1;mso-border-themeshade:166;border-left:none;mso-border-left-alt:");
                html.Append("  solid #A6A6A6 .5pt;mso-border-left-themecolor:background1;mso-border-left-themeshade:");
                html.Append("   166;mso-border-alt:solid #A6A6A6 .5pt;mso-border-themecolor:background1;");
                html.Append("    mso-border-themeshade:166;background:black;mso-background-themecolor:text1;");
                html.Append("  padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("  <p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("  text-align:center;tab-stops:45.1pt'><b style='mso-bidi-font-weight:normal'><span");
                html.Append("   style='font-size:8.0pt;line-height:115%;font-family:Arial,sans-serif;");
                html.Append("    color:white;mso-themecolor:background1'>IMPORTE CON LETRA</span></b><span");
                html.Append("    style='font-size:7.0pt;line-height:115%;font-family:'Arial','sans-serif';");
                html.Append("    color:white;mso-themecolor:background1;color:white;'><o:p></o:p></span></p>");
                html.Append("   </td>");
                html.Append("    </tr>");
                html.Append("  <tr style='mso-yfti-irow:1'>");
                html.Append("   <td width=519 style='width:388.9pt;border-top:none;border-left:none;");
                html.Append("    border-bottom:solid #A6A6A6 1.0pt;mso-border-bottom-themecolor:background1;");
                html.Append("     mso-border-bottom-themeshade:166;border-right:solid #A6A6A6 1.0pt;mso-border-right-themecolor:");
                html.Append("     background1;mso-border-right-themeshade:166;mso-border-top-alt:solid #A6A6A6 .5pt;");
                html.Append("     mso-border-top-themecolor:background1;mso-border-top-themeshade:166;");
                html.Append("     mso-border-left-alt:solid #A6A6A6 .5pt;mso-border-left-themecolor:background1;");
                html.Append("    mso-border-left-themeshade:166;mso-border-alt:solid #A6A6A6 .5pt;mso-border-themecolor:");
                html.Append("     background1;mso-border-themeshade:166;background:white;mso-background-themecolor:");
                html.Append("     background1;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("     <p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("    text-align:center;tab-stops:45.1pt'><span style='font-size:8.0pt;line-height:");
                html.Append("   115%;font-family:'Arial','sans-serif';color:#595959'>" + ConvertToLetter.ToCardinal(Factura.total).ToString().ToUpper() + "</span><b");
                html.Append("   style='mso-bidi-font-weight:normal'><span style='font-size:8.0pt;line-height:");
                html.Append("   115%;font-family:'Arial','sans-serif';color:white;mso-themecolor:background1'><o:p></o:p></span></b></p>");
                html.Append("    </td>");
                html.Append("  </tr>");
                html.Append("   <tr style='mso-yfti-irow:2'>");
                html.Append("    <td width=519 style='width:388.9pt;border-top:none;border-left:none;");
                html.Append("    border-bottom:solid #A6A6A6 1.0pt;mso-border-bottom-themecolor:background1;");
                html.Append("    mso-border-bottom-themeshade:166;border-right:solid #A6A6A6 1.0pt;mso-border-right-themecolor:");
                html.Append("   background1;mso-border-right-themeshade:166;mso-border-top-alt:solid #A6A6A6 .5pt;");
                html.Append("    mso-border-top-themecolor:background1;mso-border-top-themeshade:166;");
                html.Append("   mso-border-left-alt:solid #A6A6A6 .5pt;mso-border-left-themecolor:background1;");
                html.Append("     mso-border-left-themeshade:166;mso-border-alt:solid #A6A6A6 .5pt;mso-border-themecolor:");
                html.Append("     background1;mso-border-themeshade:166;background:white;mso-background-themecolor:");
                html.Append("    background1;padding:0cm 5.4pt 0cm 5.4pt'></td>");
                html.Append("   </tr>");
                html.Append("    <tr style='mso-yfti-irow:3'>");
                html.Append("     <td width=519 style='width:388.9pt;border-top:none;border-left:none;");
                html.Append("     border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;");
                html.Append("     mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid #A6A6A6 .5pt;");
                html.Append("     mso-border-left-themecolor:background1;mso-border-left-themeshade:166;");
                html.Append("     mso-border-alt:solid windowtext .5pt;mso-border-left-alt:solid #A6A6A6 .5pt;");
                html.Append("    mso-border-left-themecolor:background1;mso-border-left-themeshade:166;");
                html.Append("    background:black;mso-background-themecolor:text1;padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("    <p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt;");
                html.Append("    text-align:center;tab-stops:45.1pt'><b style='mso-bidi-font-weight:normal'><span");
                html.Append("   style='font-size:8.0pt;line-height:115%;mso-bidi-font-family:Calibri;");
                html.Append("   color:white;mso-themecolor:background1'>ESTE DOCUMENTO ES UNA REPRESENTACION");
                html.Append("   IMPRESA DE UN CFDI</span></b><b style='mso-bidi-font-weight:normal'><span");
                html.Append("   style='font-size:7.0pt;line-height:115%;font-family:'Arial','sans-serif';");
                html.Append("  color:#595959'><o:p></o:p></span></b></p>");
                html.Append("    </td>");
                html.Append("    </tr>");
                html.Append("    <tr style='mso-yfti-irow:4;mso-yfti-lastrow:yes'>");
                html.Append("     <td width=519 style='width:388.9pt;border-top:none;border-left:none;");
                html.Append("       border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;");
                html.Append("       mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid #A6A6A6 .5pt;");
                html.Append("     mso-border-left-themecolor:background1;mso-border-left-themeshade:166;");
                html.Append("      mso-border-alt:solid windowtext .5pt;mso-border-left-alt:solid #A6A6A6 .5pt;");
                html.Append("       mso-border-left-themecolor:background1;mso-border-left-themeshade:166;");
                html.Append("        padding:0cm 5.4pt 0cm 5.4pt'>");
                html.Append("        <p class=Default align=center style='text-align:center'><span");
                html.Append("        style='font-size:8.0pt;mso-bidi-font-size:9.5pt'>*EFECTOS FISCALES AL");
                html.Append("        PAGO<span style='mso-spacerun:yes'>  ");
                html.Append("         </span>* " + ValidarNull(Factura.formaDePago) + "</span><span style='font-size:9.5pt'><o:p></o:p></span></p>");
                html.Append("         <p class=Default align=center style='text-align:center'><b style='mso-bidi-font-weight:");
                html.Append("         normal'><span style='font-size:9.5pt'>Condiciones de pago: </span></b><span");
                html.Append("         style='font-size:9.5pt'>  " + ValidarNull(Factura.condicionesDePago) + "<o:p></o:p></span></p>");
                html.Append("       <p class=Default align=center style='text-align:center'><b style='mso-bidi-font-weight:");
                html.Append("       normal'><span style='font-size:9.5pt'>Método de pago: </span></b><span");
                html.Append("        style='font-size:9.5pt'> " + ValidarNull(Factura.metodoDePago) + "<o:p></o:p></span></p>");
                html.Append("        <p class=Default align=center style='text-align:center'><b style='mso-bidi-font-weight:");
                html.Append("      normal'><span style='font-size:9.5pt'>Numero de Cta.:</span></b><span");
                html.Append("    style='font-size:9.5pt'> " + ValidarNull(Factura.NumCtaPago) + " <o:p></o:p></span></p>");
                html.Append("     <p class=Default align=center style='text-align:center'><b style='mso-bidi-font-weight:");
                html.Append("      normal'><span style='font-size:9.5pt'>Régimen Fiscal:</span></b><span");
                html.Append("     style='font-size:9.5pt'>");
                html.Append("       " + regimenfiscal + "</span><b><span");
                html.Append("      style='font-size:7.0pt;mso-bidi-font-size:8.0pt;color:gray'><o:p></o:p></span></b></p>");
                html.Append("     </td>");
                html.Append("     </tr>");
                html.Append("    </table>");

                html.Append("     </div>");

                html.Append("      <p class=MsoNormal style='mso-outline-level:1'><b><span style='font-size:1.0pt;");
                html.Append("     line-height:115%;font-family:Arial,sans-serif;color:gray'><o:p>&nbsp;</o:p></span></b></p>");

                html.Append("      <div align=center>");

                html.Append("      <table class=MsoTableGrid border=1 cellspacing=0 cellpadding=0 width=718 style='width:718px;'");
                html.Append("        style='width:19.0cm;border-collapse:collapse;border:none;mso-border-alt:solid windowtext .5pt;");
                html.Append("       mso-yfti-tbllook:1184;mso-padding-alt:0cm 5.4pt 0cm 5.4pt;mso-border-insideh:");
                html.Append("       .5pt solid windowtext;mso-border-insidev:.5pt solid windowtext'>");
                html.Append("      <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes;height:16.7pt'>");
                html.Append("       <td width=519 style='width:388.9pt;border:solid windowtext 1.0pt;mso-border-alt:");
                html.Append("      solid windowtext .5pt;background:#EEECE1;mso-background-themecolor:background2;");
                html.Append("       padding:0cm 5.4pt 0cm 5.4pt;height:16.7pt'>");
                html.Append("      <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("     normal;mso-outline-level:1'><b style='mso-bidi-font-weight:normal'><span");
                html.Append("     style='font-size:9.0pt;mso-ascii-font-family:Calibri;mso-ascii-theme-font:");
                html.Append("      minor-latin;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin;");
                html.Append("      mso-bidi-font-family:Calibri;color:#262626'>Cadena original del complemento");
                html.Append("       de certificación digital del SAT</span></b><b><span style='font-size:7.0pt;");
                html.Append("       mso-bidi-font-size:1.0pt;font-family:'Arial','sans-serif';color:#262626'><o:p></o:p></span></b></p>");
                html.Append("       </td>");
                html.Append("      </tr>");
                html.Append("     <tr style='mso-yfti-irow:1;height:4.25pt'>");
                html.Append("        <td width=519 style='width:388.9pt;border:solid windowtext 1.0pt;border-top:");
                html.Append("       none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;");
                html.Append("        padding:0cm 5.4pt 0cm 5.4pt;height:4.25pt'>");
                html.Append("       <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("        normal;mso-outline-level:1'><b style='mso-bidi-font-weight:normal'><span");
                html.Append("       style='font-size:4.0pt;font-family:Courier New;color:#595959'> " + cadenaoriginal.ToString() + "</span></b><b><span");
                html.Append("      style='font-size:7.0pt;mso-bidi-font-size:8.0pt;font-family:'Arial','sans-serif';");
                html.Append("        color:#595959'><o:p></o:p></span></b></p>");
                html.Append("        </td>");
                html.Append("      </tr>");
                html.Append("    <tr style='mso-yfti-irow:2;height:4.2pt'>");
                html.Append("       <td width=519 style='width:388.9pt;border:solid windowtext 1.0pt;border-top:");
                html.Append("       none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;");
                html.Append("      background:#EEECE1;mso-background-themecolor:background2;padding:0cm 5.4pt 0cm 5.4pt;");
                html.Append("       height:4.2pt'>");
                html.Append("       <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("       normal;mso-outline-level:1'><b style='mso-bidi-font-weight:normal'><span");
                html.Append("       style='font-size:7.0pt;mso-bidi-font-size:9.0pt;font-family:'Arial','sans-serif';");
                html.Append("        color:#262626'>Sello digital del CFDI</span></b><span style='font-size:7.0pt;");
                html.Append("     mso-bidi-font-size:9.0pt;font-family:Courier New;color:#262626'><o:p></o:p></span></p>");
                html.Append("        </td>");
                html.Append("     </tr>");
                html.Append("   <tr style='mso-yfti-irow:3;height:4.2pt'>");
                html.Append("   <td width=519 style='width:388.9pt;border:solid windowtext 1.0pt;border-top:");
                html.Append("  none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;");
                html.Append("    padding:0cm 5.4pt 0cm 5.4pt;height:4.2pt'>");
                html.Append("    <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("       normal;mso-outline-level:1'><b style='mso-bidi-font-weight:normal'><span");
                html.Append("      style='font-size:4.0pt;mso-bidi-font-size:4.0pt;font-family:Courier New;");
                html.Append("      color:#595959'>" + selloCFDI + "<o:p></o:p></span></b></p>");
                html.Append("     </td>");
                html.Append("    </tr>");
                html.Append("   <tr style='mso-yfti-irow:4;height:16.7pt'>");
                html.Append("      <td width=519 style='width:388.9pt;border:solid windowtext 1.0pt;border-top:");
                html.Append("      none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;");
                html.Append("      background:#EEECE1;mso-background-themecolor:background2;padding:0cm 5.4pt 0cm 5.4pt;");
                html.Append("     height:16.7pt'>");
                html.Append("    <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("   normal'><b style='mso-bidi-font-weight:normal'><span style='font-size:7.0pt;");
                html.Append("    mso-bidi-font-size:9.0pt;font-family:'Arial','sans-serif';color:#262626'>Sello");
                html.Append("   digital del SAT</span></b><span style='font-size:7.0pt;mso-bidi-font-size:");
                html.Append("   8.0pt;font-family:'Arial','sans-serif';color:#262626'><o:p></o:p></span></p>");
                html.Append("     </td>");
                html.Append("    </tr>");
                html.Append("     <tr style='mso-yfti-irow:5;mso-yfti-lastrow:yes;height:10.7pt'>");
                html.Append("     <td width=519 style='width:388.9pt;border:solid windowtext 1.0pt;border-top:");
                html.Append("    none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;");
                html.Append("   padding:0cm 5.4pt 0cm 5.4pt;height:10.7pt'>");
                html.Append("   <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:");
                html.Append("     normal'><b style='mso-bidi-font-weight:normal'><span style='font-size:4.0pt;");
                html.Append("    mso-bidi-font-size:4.0pt;font-family:Courier New;color:#595959'>" + selloSAT + "<o:p></o:p></span></b></p>");
                html.Append("    </td>");
                html.Append("     </tr>");
                html.Append("    </table>");

                html.Append("     </div>");

                html.Append("      <p class=MsoNormal style='mso-outline-level:1'><b><span style='font-size:8.0pt;");
                html.Append("      line-height:115%;font-family:'Arial','sans-serif';color:gray'><o:p>&nbsp;</o:p></span></b></p>");

                html.Append("      <p class=MsoNormal><span style='font-size:8.0pt;line-height:115%;font-family:");
                html.Append("       'Arial','sans-serif';color:white'><o:p>&nbsp;</o:p></span></p>");

                html.Append("      <p class=MsoNormal><span style='mso-bidi-font-size:8.0pt;line-height:115%'><o:p>&nbsp;</o:p></span></p>");

                html.Append("      </div>");

                html.Append("     </body>");

                html.Append("     </html>");

            }
            catch (Exception err)
            {
                msg1 = "Error al momento de generar Factura HTML  detalle: " + err.Message;
            }
            return html.ToString();
        }




        /*
        static bool mailSent = false;
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            mailSent = true;
        }
        public bool EnviarCorreo(string fromE,string nameFrom,string body,string subject ,string toE)
        {
            string msg = "";
            // Command line argument must the the SMTP host.
            SmtpClient client = new SmtpClient();
            // Specify the e-mail sender.
            // Create a mailing address that includes a UTF8 character
            // in the display name.
            MailAddress from = new MailAddress(fromE,
               nameFrom + (char)0xD8 + " Clayton",
            System.Text.Encoding.UTF8);
            // Set destinations for the e-mail message.
            MailAddress to = new MailAddress(toE);
            // Specify the message content.
            MailMessage message = new MailMessage(from, to);
            message.Body =body;
            // Include some non-ASCII characters in body and subject.
            string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
            message.Body += Environment.NewLine + someArrows;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = "test message 1" + someArrows;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            // Set the method that is called back when the send operation ends.
            client.SendCompleted += new
            SendCompletedEventHandler(SendCompletedCallback);
            // The userState can be any object that allows your callback 
            // method to identify this send operation.
            // For this example, the userToken is a string constant.
            string userState = "test message1";
            client.SendAsync(message, userState);
            Console.WriteLine("Sending message... press c to cancel mail. Press any other key to exit.");
            string answer = Console.ReadLine();
            // If the user canceled the send, and mail hasn't been sent yet,
            // then cancel the pending operation.
            if (answer.StartsWith("c") && mailSent == false)
            {
                client.SendAsyncCancel();
            }
            // Clean up.
            message.Dispose();
            Console.WriteLine("Goodbye.");


            return true;
        }
         * */



        public static string CrearQR(int scala,string ruta, string[] Datos_Factura,decimal tot)
        {
            string msg = "";
            string datos = "?re={0}&rr={1}&tt={2}&id={3}";
            decimal total =0;
            string sTotal = "";
            string entero = "";
            string decimales = "";
            string resultado = "";

            string RFC_Emisor = "";
            string RFC_Receptor = "";
            string UUID = "";

          
            try
            {
                if (Datos_Factura.Length == 3)
                {
                    RFC_Emisor = Datos_Factura[0];
                    RFC_Receptor = Datos_Factura[1];
                    total = tot;
                    UUID = Datos_Factura[2];





                    sTotal = String.Format("{0:0.000000}", total).Replace(',', '.');
                    entero = decimal.Truncate(total).ToString().PadLeft(10, Convert.ToChar("0"));
                    decimales = sTotal.Split(Convert.ToChar("."))[1];
                    resultado = entero + "." + decimales;


                    QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    qrCodeEncoder.QRCodeScale = scala;//4
                    qrCodeEncoder.QRCodeVersion = 8;
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                    // = qrCodeEncoder.Encode(datos);
                    System.Drawing.Bitmap bm = qrCodeEncoder.Encode(datos);
                    bm.Save(ruta,System.Drawing.Imaging.ImageFormat.Jpeg);
                    

                }
                else
                {
                    msg += "Informacion Factura Erroneo ";
                   
                }
            }
            catch (Exception ex)
            {
                msg += ex.Message;
            }
            return msg;

        }

        protected static string ValidarNull(object Campo)
        {

            if (Campo == null)
            {
                return ""; //  bRes = false;
            }
            else
            {
                return Campo.ToString().ToUpper();
            }
            
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
        protected static bool ComprobarRFC(object rfc)
        {
            var valida = @"^(([A-Z]|[a-z]|\s){1})(([A-Z]|[a-z]){3})([0-9]{6})((([A-Z]|[a-z]|[0-9]){3}))";
            //@"^([A-Z\s]{4})\d{6}([A-Z\w]{3})$"
            return Regex.IsMatch(rfc.ToString(), valida);
        }
        protected static string ConvertCadena(string cadena,int tamano){
        //  string cadena1 = "";
          //  string cadena2="";
          /*  decimal div = 0;
      
            if(cadena.Trim()!=""){
                if (cadena.Length >= tamano)
                {
                    div = Convert.ToInt32(cadena.Length) / tamano;

                    for (int i = 1; i == Convert.ToInt32(Math.Truncate(div)); i++)
                    {

                        cadena.Insert(i * 120, Environment.NewLine);

                    }

                    return cadena;

                }
                else {
                   // return cadena;
                }
                return cadena;
            }*/
            return cadena;
           
           
        }
        protected static void ConvertHTMLToPDF(string HTMLCode, string rutaPDF)
        {

           
          



            
            /*
            String htmlText = HTMLCode.ToString();
            Document document = new Document(iTextSharp.text.PageSize.A4, 10f, 10f, 10f, 0.0f);
            string filePath = rutaPDF;// HostingEnvironment.MapPath("~/Content/Pdf/");
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            document.Open();
          
            iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);
            hw.Parse(new StringReader(htmlText));
            document.Close();    
             */
        }


        
    }
}