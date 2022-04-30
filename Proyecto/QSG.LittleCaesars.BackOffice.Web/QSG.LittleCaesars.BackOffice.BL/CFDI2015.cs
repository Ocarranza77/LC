using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Serialization;

using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System.Collections;
using Telerik.Reporting;
using QSG.LittleCaesars.BackOffice.Common.Entities;



namespace QSG.LittleCaesars.BackOffice.BL
{
    public class CFDI2015
    {
        /*parametros*/
        public string _rutaCertKey;/*rutadel archivo key*/
        public string _Clave;/*clave del archivo certKey*/
        public string _RutaPKcs12;/*ruta del archivo pkcs12 para cancelar*/
        public string _UserPak;/*usuario pack*/
        public string _ClavePak;/*clave del pak*/
        public string _RFC;/*no se usa*/
        public string _rutaXML;/*ruta xml x timbrar y timbrado*/
        public string _rutaXlst;/* archivo xlst para obtener cadena original*/
        //public string _rutaPDF;
        public string _rutaFacturas; /*ruta donde se guaardana las facturas pdf*/
      //  public string _FolioFactura;/* folio generado autoamticamnete que sirve como referencia para la factura timbrada al sat y para contro interno de facturas*/
        public List<string> LstUUID;/*lista de folios fiscales para cancelar o timbrar en su defecnto*/
        public Comprobante Factura;/* datos de la factura*/
        public ComprobanteEmisor Emisor;/* datops del ue emite la factura*/
        public Sucursal sucursal;/* sucursal que esta emitiendo al factura*/
        public String[] DatosAdicionales;/* datos temporales y adiciopnales  [0]Tel Matriz [1]no int receptor [2]fechavta [3]regimen fiscal */
        public string XML;/*si ya existe xml*/
        public string _rutaLogoSAT;
       // public string _rutaFacturas;

        /*parametros internos*/
        private string _fechaFacturacion = string.Empty;
        private string _sello = string.Empty;
        private string _XMLTimbrado = string.Empty;

        private string _FolioFactura;
        private string _rutaAcuse;
        private string _rutaAcuseHTML;

        //WSFEL.WSTFDClient FELService = new WSFEL.WSTFDClient();

     //  WS_FacturarEnLinea.WS_TFD ServicioFel = new WS_FacturarEnLinea.WS_TFD();


        public CFDI2015()
        {
           // _FolioFactura = Factura.serie + Factura.folio;
        }


        public bool GenerarPath(out string msg)
        {
            msg = string.Empty;
            try
            {
                _FolioFactura = Factura.serie + Factura.folio;
                _rutaAcuse = _rutaXML + "\\Acuse_" + Factura.Receptor.rfc + _FolioFactura + ".xml";
                _rutaAcuseHTML = _rutaFacturas + "\\Acuse_" + Factura.Receptor.rfc + _FolioFactura + ".html";


                _rutaXML += "\\" + Factura.Receptor.rfc + _FolioFactura + ".xml";
                _rutaFacturas += "\\" + Factura.Receptor.rfc + _FolioFactura + ".pdf";
                
                return true;
            }
            catch (Exception ex)
            {
                msg += ex.Message;
                return false;
            }

        }
        public bool GenerarSello(out string msg)
        {
            bool estatus = false;
            msg = string.Empty;


            try
            {
                //Cargar el xml en memoria
                string esquema = "http://www.sat.gob.mx/sitio_internet/cfd/3/cadenaoriginal_3_0/cadenaoriginal_3_2.xslt";
                string sXML = Factura.Serialize(System.Text.Encoding.UTF8);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(sXML);

                //Generar Cadena Original
                XslCompiledTransform transformador = new XslCompiledTransform();
                transformador.Load(esquema);
                

                StringWriter CadenaOriginal = new StringWriter();
                transformador.Transform(xmlDoc.CreateNavigator(), null, CadenaOriginal);


                //Generar SELLO
                try
                {

                    if (File.Exists(_rutaCertKey))
                    {
                        //string pass = claveKey;
                        byte[] dataKey = File.ReadAllBytes(_rutaCertKey);
                        Org.BouncyCastle.Crypto.AsymmetricKeyParameter asp = Org.BouncyCastle.Security.PrivateKeyFactory.DecryptKey(_Clave.ToCharArray(), dataKey);
                        MemoryStream ms = new MemoryStream();
                        TextWriter writer = new StreamWriter(ms);
                        System.IO.StringWriter stWrite = new System.IO.StringWriter();
                        Org.BouncyCastle.OpenSsl.PemWriter pmw = new PemWriter(stWrite);
                        pmw.WriteObject(asp);
                        stWrite.Close();

                        //ISigner sig = SignerUtilities.GetSigner("MD5WithRSAEncryption");
                        ISigner sig = SignerUtilities.GetSigner("SHA1WithRSA");

                        //' Convertir a UTF8
                        byte[] plaintext = Encoding.UTF8.GetBytes(CadenaOriginal.ToString());

                        //' SELLAR
                        sig.Init(true, asp);
                        sig.BlockUpdate(plaintext, 0, plaintext.Length);
                        byte[] signature = sig.GenerateSignature();

                        object signatureHeader = Convert.ToBase64String(signature);

                        //Asignar sello
                        XmlElement rai = xmlDoc.DocumentElement;
                        XmlAttribute atrSello = xmlDoc.CreateAttribute("sello");
                        atrSello.Value = signatureHeader.ToString();
                        rai.Attributes.Append(atrSello);
                        if (File.Exists(_rutaXML))
                        {
                            File.Delete(_rutaXML);
                        }

                        xmlDoc.Save(_rutaXML);

                       // var resul = Timbrar(out msg);

                        estatus = true;
                    }
                    else
                    {
                        estatus = false;
                        msg += " Error al generar Sello, el archivo llave no existe: " + _rutaCertKey;

                    }
                }
                catch (Exception ex)
                {
                    estatus = false;
                    msg += " Error al generar cadena original, Error: " + ex.Message;

                }
            }
            catch (Exception ex)
            {
                estatus = false;
                msg += " Error al generar cadena original, Error: " + ex.Message;

            }

            return estatus;

        }
        public List<Creditos> Creditos()
        {
            
           // WSFEL.WSTFDClient _servicioTimbrado = new WSFEL.WSTFDClient();
           // WSFEL.RespuestaCreditos _respuestaServicio = new WSFEL.RespuestaCreditos();

          //  List<WSFEL.DetallesPaqueteCreditos> _detalleCreditos = new List<WSFEL.DetallesPaqueteCreditos>();
            List<Creditos> lstcred=new List<Common.Entities.Creditos>();

            //_respuestaServicio = FELService.ConsultarCreditos(_UserPak, _ClavePak);

            //if (_respuestaServicio.OperacionExitosa)
            //{
            //    foreach (WSFEL.DetallesPaqueteCreditos paquete in _respuestaServicio.Paquetes)
            //    {
            //        if (paquete.EnUso)
            //        {
            //            Creditos crt = new Creditos();
            //            crt.EnUso = paquete.EnUso;
            //            crt.FechaActivacion = paquete.FechaActivacion;
            //            crt.FechaVencimiento = paquete.FechaVencimiento;
            //            crt.Paquete = paquete.Paquete;
            //            crt.Timbres = paquete.Timbres;
            //            crt.TimbresRestantes = paquete.TimbresRestantes;
            //            crt.TimbresUsados = paquete.TimbresUsados;
            //            crt.Vigente = paquete.Vigente;
            //            lstcred.Add(crt);
            //        }
            //    }

            //}
            return lstcred; 
          
        }
        public string Cancelar(out string msg)
        {

            msg = string.Empty;
           var xml="";
            //https://timbrado.facturarenlinea.com/WSTFD.svc


            //WSFEL.WSTFDClient _servicioTimbrado = new WSFEL.WSTFDClient();
            
            //WSFEL.RespuestaCancelacion _respuestaCancelacion = new WSFEL.RespuestaCancelacion();
            //WSFEL.RespuestaTFD _respuestaTFD = new WSFEL.RespuestaTFD();

            string certificadoPkcs12 = File.ReadAllText(_RutaPKcs12);
            //_respuestaCancelacion = FELService.CancelarCFDI(_UserPak, _ClavePak, Emisor.rfc, LstUUID.ToArray(), certificadoPkcs12, _Clave);

            //if (_respuestaCancelacion.OperacionExitosa)
            //{
            //    var uuid = LstUUID[0].ToString();
            //   // respdetail = respservice.DetallesCancelacion.ToList();
            //    //_respuestaTFD = FELService.ObtenerAcuseCancelacion(_UserPak, _ClavePak,uuid);
            //    if (_respuestaTFD.CodigoRespuesta == "800")
            //    {

            //        XmlDocument acusexml = new XmlDocument();
            //        acusexml.LoadXml(_respuestaTFD.XMLResultado);
            //        acusexml.Save(_rutaAcuse);
            //        xml = _respuestaTFD.XMLResultado;

            //        msg += SaveAcuse(_respuestaTFD.Timbre.FechaTimbrado.ToShortDateString(), _respuestaTFD.Timbre.UUID.ToString(), _respuestaTFD.Timbre.SelloSAT, Emisor.rfc);


            //    }
            //    else {
            //        msg += "error al obtener XMLAcuse" + System.Environment.NewLine;
            //        msg += _respuestaTFD.MensajeError;
            //    }
            //    return xml;

            //}
            //else
            //{
            //    msg += "Error de cancelacion" + System.Environment.NewLine;
            //    msg += _respuestaCancelacion.MensajeError + System.Environment.NewLine;
            //    msg += _respuestaCancelacion.MensajeErrorDetallado;
            //    return xml;
            //}
            return xml; // para evitar el error




        }
        public Timbrado Timbrar(out string msg)
        {

            Timbrado tr = new Timbrado();
            bool estatus = false;
            msg = string.Empty;
            try
            {

               // string xmlportimbrar = File.ReadAllText(_rutaXML, Encoding.UTF8);
                //https://timbrado.facturarenlinea.com/WSTFD.svc

               // WSFEL.RespuestaTFD _respuestaTimbrado = new WSFEL.RespuestaTFD();


                XmlDocument docxml = new XmlDocument();
                docxml.Load(_rutaXML);

                string strXml;
                strXml = docxml.OuterXml;
              
               // _respuestaTimbrado = _servicioTimbrado.TimbrarCFDI(_UserPak, _ClavePak, strXml, _FolioFactura);
                //_respuestaTimbrado = FELService.TimbrarCFDI(_UserPak, _ClavePak, strXml, _FolioFactura);
                
                //if (_respuestaTimbrado.OperacionExitosa)
                //{
                //    tr.Estado = _respuestaTimbrado.Timbre.Estado;
                //    tr.FechaTimbrado = _respuestaTimbrado.Timbre.FechaTimbrado;
                //    tr.NumeroCertificadoSAT = _respuestaTimbrado.Timbre.NumeroCertificadoSAT;
                //    tr.SelloCFD = _respuestaTimbrado.Timbre.SelloCFD;
                //    tr.SelloSAT = _respuestaTimbrado.Timbre.SelloSAT;
                //    tr.UUID = _respuestaTimbrado.Timbre.UUID;
                //    tr.XML = _respuestaTimbrado.XMLResultado;

                //    docxml.LoadXml(_respuestaTimbrado.XMLResultado);
                //    docxml.Save(_rutaXML);
                //}
                //else
                //{
                //    msg += "Error de Timbrado " + System.Environment.NewLine;
                //    msg += _respuestaTimbrado.CodigoRespuesta + System.Environment.NewLine;
                //    msg += _respuestaTimbrado.MensajeError + System.Environment.NewLine;
                //    msg += _respuestaTimbrado.MensajeErrorDetallado + System.Environment.NewLine;
                    
                //}
                
            }
            catch (Exception ex)
            {
                msg += "Error," + ex.Message;
                estatus = false;
                // throw new Exception("Error al timbrar comprobante, Error: " + ex.Message);
            }


            return tr;
        }
        public bool PrepararXML(out string msg)
        {
            msg = string.Empty;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(XML);
                if (!File.Exists(_rutaXML))
                {
                    xmlDoc.Save(_rutaXML);
                }
                

                return true;
            }
            catch (Exception ex)
            {
                msg += "Erro al preparar XML" + System.Environment.NewLine;
                return false;
            }

        }
        public bool PDF(out string msg)
        {
         
         
            var estatus = false;


            msg = string.Empty;
            try
            {

                XMLTimbrado(out msg);/* carga xml */

                Hashtable deviceInfo = new Hashtable();

                Telerik.Reporting.InstanceReportSource reptoPDF = new InstanceReportSource();
                reptoPDF.ReportDocument = new Report1(Emisor, Factura, sucursal, _XMLTimbrado, _rutaXlst, DatosAdicionales, out _fechaFacturacion, out msg);
                /* reptoPDF.ReportDocument = new ReportesCFDI.ReporteAcuseCancelacion(_Fecha, _RFC, _UUID, _Sello);
                 */
                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
                Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport("PDF", reptoPDF, deviceInfo);
                FileStream fs = new FileStream(_rutaFacturas, FileMode.Create);
                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                fs.Flush();
                fs.Close();
                estatus = true;

            }
            catch (Exception ex)
            {
                msg += "Error al momento generar PDF" + System.Environment.NewLine;
                msg += ex.Message;
                estatus = false;
            }

            return estatus;

        }
        private void XMLTimbrado(out string msg)
        {

           
            msg = string.Empty;
            try
            {
                if (File.Exists(_rutaXML))
                {

                    // string complemento;
                    Comprobante f = new Comprobante();
                    StreamReader objStreamReader = new StreamReader(_rutaXML);
                    XmlSerializer Xml = new XmlSerializer(f.GetType());
                    f = (Comprobante)Xml.Deserialize(objStreamReader);
                    objStreamReader.Close();
                    _XMLTimbrado = f.Complemento.Any[0].OuterXml;

                }

            }
            catch (Exception ex)
            {
                msg += "Error Carga XML " + System.Environment.NewLine;
                msg += ex.Message;
            }


        }
        public string PathXML() { return _rutaXML.ToString(); }
        public string PathPDF() { return _rutaFacturas.ToString(); }
        public string FechaTimbrado() { return _fechaFacturacion.ToString(); }

        private string SaveAcuse( string fecha,string uuid,string selloSAT,string rfcEmisor)
        {
            string msg = string.Empty;
            using (FileStream file = new FileStream(_rutaAcuseHTML, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter write = new StreamWriter(file, Encoding.UTF8))
                {    //HttpContext.Current.Server.MapPath("~/Facturas_HTML/ImagesQR/" + Factura.Receptor.rfc + Factura.serie + Factura.folio + ".jpg")
                    //
                    //string rutalogo = urlShCP + "/Images/logo_shcp.jpg";
                    write.WriteLine(ACuseHTML(_rutaLogoSAT, fecha, uuid, selloSAT, rfcEmisor, out msg));
                    write.Close();
                }
            }
            return msg;
        }
        public static string ACuseHTML(string urlLogo, string fecha, string uuid, string sello, string rfc, out string msg)
        {
            msg = string.Empty;
            StringBuilder html = new StringBuilder();
            try
            {

                html.Append("     <!DOCTYPE html>");
                html.Append("       <html>");
                html.Append("       <head>");
                html.Append("       <title>Acuse de cancelacion CFDI</title>");
                html.Append("       <style type='text/css'>");
                html.Append("       *{padding: 0;");
                html.Append("       margin: 0;     }");
                html.Append("        .content{");
                html.Append("        position: absolute;");
                html.Append("        width: 750px;");
                html.Append("        height: 500px;}");
                /*border:1px solid orange;*/
                //}
                html.Append("       .content > div{");
                html.Append("       float: left;");
                html.Append("      width: 100%;");
                html.Append("       height: 49%;}");
                /*border:1px solid green;*/
                // }
                html.Append("  .content > div:first-child > img{");
                html.Append("   float: left;");
                html.Append("   margin-left: 10px;");
                html.Append("    margin-top:10px;");
                html.Append("   width:200px;");
                html.Append("   height: 200px;}");
                /*border:1px solid red;*/
                // }

                html.Append("   h1,h2{");
                html.Append("    float: left;");

                html.Append("   font-family: 'Corbel';");
                html.Append("    margin-left: 5px;}");
                //   }
                html.Append("  h1{");
                html.Append("   padding:70px 0 5px 2px;}");
                //}
                html.Append("  ul{");
                html.Append("  float: left;");
                html.Append("   width: 100%;");
                html.Append(" height: 100%;}");
                /*border:1px solid blue;*/
                // }
                html.Append(" li{");
                html.Append("  float: left;");
                html.Append("  margin-left: 5px;");
                html.Append("  margin-top: 5px;");
                html.Append("   width: 98%;}");
                /*border:1px solid red;*/
                // }
                html.Append("  li > span{");
                html.Append("  float: left;");
                html.Append("  padding-left: 2px;");
                html.Append("   margin-left: 1px;");
                html.Append("  width: 49%;}");
                /*border:1px solid black;*/
                // }


                html.Append("  </style>");
                html.Append(" </head>");
                html.Append("  <body>");
                html.Append("  <div class='content'>");
                html.Append("   <div class='top_content'>");
                html.Append("  <img src='" + urlLogo + "'/>");
                html.Append(" <h1>Servicio de Administraci&oacute;n Tributaria.</h1>");
                html.Append(" <h2>Acuse de Cancelacion de CFDI.</h2>");
                html.Append(" </div>");
                html.Append("  <div class='bottom_content'>");
                html.Append("  <ul>");
                html.Append("      <li><span>Fecha de solicitud</span><span>" + fecha + " </span></li>");
                html.Append("      <li><span>Fecha de cancelacion</span><span>" + fecha + " </span></li>");
                html.Append("      <li><span>RFC Emisor</span><span>" + rfc + " </span></li>");
                html.Append("      <li></li>");
                html.Append("      <li><span style='padding:2px;background-color:lightgray;'>FOLIO FISCAL</span><span style='padding:2px;background-color:lightgray;'>ESTADO CFDI</span></li>");
                html.Append("      <li><span style='border:1px solid lightgray;font-size:11pt;'>" + uuid + "</span><span style='border:1px solid lightgray;font-size:11pt;'>Cancelado </span></li>");

                html.Append("     <li><span>Sello digital SAT</span></li>");
                html.Append("    <li><span style='float:left;width:99%;font-size:10pt;'>" + sello + "</span></li>");


                html.Append("  </ul>");
                html.Append("  </div>");
                html.Append("  </div>");
                html.Append("  </body>");
                html.Append("  </html>");
            }
            catch (Exception err)
            {
                msg = "Error al generar Acuse HTML  " + err.Message;
            }
            return html.ToString();
        }
     




    }
}
