using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Serialization;

using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

//using Telerik.Reporting;
using System.Collections;

namespace QSG.LittleCaesars.BackOffice.BL
{
    public class ClaseCFDI
    {
        private string _Fecha = string.Empty;
        private string _Sello = string.Empty;
        private string _RFC = string.Empty;
        private string[] _uuid = new string[1];
        //no es neceraio los certificados una ves generado el archivo pem
        public string CancelarCFDI(string passllaveprivada, string RFCEmisor, string folio, string UUID, string rutaPkcs12, string rutaXML, string R_AcuseHTML, string urlImgSat, out string msg)
        {
            msg = string.Empty;
            string xmlReturn = string.Empty;
            _uuid[0] = UUID;
            _RFC = RFCEmisor;
            string usuariopac = "CES070913FQ3";  // "FOAF820313CS7";//otorgado por la empresa donde contratamos el servicio de timbrado
            string contrasenapac = "TgCSiXi+";// "NyM5ceR=";//otorgado por la empresa donde contratamos el servicio de timbrado
            string certificadoPkcs12 = File.ReadAllText(rutaPkcs12);
            rutaXML += _RFC + folio + ".xml";
            R_AcuseHTML += _RFC + folio + ".html";

           
            
            WS_FacturarEnLinea.WS_TFD serviciofel = new WS_FacturarEnLinea.WS_TFD();
            string[] Respuesta = serviciofel.CancelarCFDI(usuariopac, contrasenapac, "CES070913FQ3", _uuid, certificadoPkcs12, passllaveprivada);

            bool cancelado = Respuesta[1].Contains("|201|");

            if (cancelado)
            {
                //  _RutaxmlAcuse = HttpContext.Current.Server.MapPath("~/App_Data/Acuse_" + _Folio + ".xml");
                //  _RutapdfAcuse = HttpContext.Current.Server.MapPath("~/App_Data/Acuse_" + _Folio + ".pdf");


                //guardar acuse xml
                string acuse = Respuesta[2];
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(acuse);
                doc.PreserveWhitespace = true;
                doc.Save(rutaXML);
                xmlReturn = acuse;

                //sacar fecha y sello
                LeerXML(rutaXML, out msg);

                //guardar acuse pdf
                // ExportToPDF(_RutapdfAcuse);
                if (msg == "")
                {
                    msg += SaveAcuse(R_AcuseHTML, urlImgSat);
                }


                //abrir acuse pdf
                //Process.Start(_RutapdfAcuse);
                // msg = "";
            }
            else
            {
                //mostrar mensajes informando
                //porqué no se canceló...
                string Mensaje1 = Respuesta[0];
                string Mensaje2 = Respuesta[1];

                msg = Mensaje1 + " " + Mensaje2;
            }
            
            return xmlReturn;
        }
        public void ExportToPDF(string NombreAcuse)
        {
            try
            {
                if (File.Exists(NombreAcuse))
                {
                    File.Delete(NombreAcuse);
                }

                Hashtable deviceInfo = new Hashtable();
                /*
                Telerik.Reporting.InstanceReportSource reptoPDF = new InstanceReportSource();
                reptoPDF.ReportDocument = new ReportesCFDI.ReporteAcuseCancelacion(_Fecha, _RFC, _UUID, _Sello);

                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
                Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport("PDF", reptoPDF, deviceInfo);
                FileStream fs = new FileStream(NombreAcuse, FileMode.Create);
                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                fs.Flush();
                fs.Close();*/
            }
            catch (Exception ex)
            {
                throw new Exception("Error al exportar a PDF v3, Error: " + ex.Message);
            }
        }


        public string ObtenerTimbres(string usuario, string password)
        {

            WS_FacturarEnLinea.WS_TFD ServicioFel = new WS_FacturarEnLinea.WS_TFD();
            string[] Respuesta = ServicioFel.ConsultarCreditos(usuario, password);
            if (Respuesta[0].Trim().Length == 0 && Respuesta[1].Trim().Length == 0 && Respuesta[2].Trim().Length == 0 && Respuesta[4].Trim().Length == 0)
            {
                return Respuesta[3];

            }
            else
            {
                return "Surgieron Errores :" + Respuesta[0] + Respuesta[1] + Respuesta[2] + Respuesta[3] + Respuesta[4];
            }

        }
        public string generarSello(Comprobante Factura,string RutaCertKey,string key,string RutaXMLXT,string RutaXMLTimb)
        {

          
           //bool ejecuto = false;
            string msg = string.Empty;
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
                    string rutaKey = RutaCertKey;// HttpContext.Current.Server.MapPath("~/App_Data/csdFOAF820313CS7.key");//ruta del archivo llave otorgado por el SAT
                    string claveKey = key; //"FOAF820313hsllcs07";//contraseña del archivo llave

                    if (File.Exists(rutaKey))
                    {
                        string pass = claveKey;
                        byte[] dataKey = File.ReadAllBytes(rutaKey);
                        Org.BouncyCastle.Crypto.AsymmetricKeyParameter asp = Org.BouncyCastle.Security.PrivateKeyFactory.DecryptKey(pass.ToCharArray(), dataKey);
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

                        //Guardar en disco todo el xml, para timbrarlo
                        //ruta xml por timbrar HttpContext.Current.Server.MapPath("~/App_Data/Factura_" + Factura.folio + ".xml");
                        //string rutaXMLXT = pathXML;  // HttpContext.Current.Server.MapPath("~/App_Data/Factura_" + Factura.folio + ".xml");
                        msg = guardarXML(xmlDoc, RutaXMLXT, RutaXMLTimb, Factura.serie, Factura.folio);
                    }
                    else
                    {
                        msg += " Error al generar Sello, el archivo llave no existe: " + rutaKey;
                        throw new Exception("\nError al generar Sello, el archivo llave no existe: " + rutaKey);
                    }
                }
                catch (Exception ex)
                {
                    msg += " Error al generar cadena original, Error: " + ex.Message;
                    throw new Exception("\nError al generar Sello " + ex.Message );
                }
            }
            catch (Exception ex)
            {

                msg += " Error al generar cadena original, Error: " + ex.Message;
                throw new Exception("Se presentaron los siguientes Errores \nGenerar Cadena Original " + ex.Message );
            }

            
            return msg;
        }

        private string timbrar(string rutaxmlportimbrar,string RutaXmlTimbrado,string serie,string folio)
        {
           // bool ejecuto = false;
            string msg = string.Empty;
            try
            {
                //leer xml por timbrar: "xmlPath"
                string xmlportimbrar = File.ReadAllText(rutaxmlportimbrar, Encoding.UTF8);

                string nombreXMLtimbrado = RutaXmlTimbrado; //HttpContext.Current.Server.MapPath("~/App_Data/Timbre_Factura_" + Factura.folio + ".xml");

                string UsuarioPAC ="CES070913FQ3"; //"FOAF820313CS7";//otorgado por la empresa donde contratamos el servicio de timbrado
                string ContrasenaPAC = "TgCSiXi+";// "NyM5ceR=";//otorgado por la empresa donde contratamos el servicio de timbrado
                string ReferenciaPAC = serie + folio; // Factura.serie + Factura.folio;

                WS_FacturarEnLinea.WS_TFD ServicioFel = new WS_FacturarEnLinea.WS_TFD();
                //Convert.ToDateTime("2014/88/99");
                //ServicioFel.Timeout = 1500;
                
                string[] Respuesta = ServicioFel.TimbrarCFD(UsuarioPAC, ContrasenaPAC, xmlportimbrar, ReferenciaPAC);

                string Res0 = Respuesta[0];//alert
                string Res1 = Respuesta[1];//alert
                string Res2 = Respuesta[2];//alert
                string Res3 = Respuesta[3];//xml timbrado
               // msg += Res0 + Res1 + Res2 + Res3;
                
                if (Res0.Trim().Length == 0 & Res1.Trim().Length == 0 & Res2.Trim().Length == 0)
                {
                    // Create the XmlDocument.
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(Res3);

                    // Save the document to a file. White space is
                    // preserved (no white space).
                    doc.PreserveWhitespace = true;
                    doc.Save(nombreXMLtimbrado);
                   // ejecuto = true;
                }
                else
                {
                    string msgFEL = "Factura NO TIMBRADA";
                    msgFEL += Environment.NewLine + Environment.NewLine;
                    msgFEL += Res0 + Environment.NewLine + Environment.NewLine;
                    msgFEL += Res1 + Environment.NewLine + Environment.NewLine;
                    msgFEL += Res2 + Environment.NewLine + Environment.NewLine;
                  //  ejecuto = false;
                    msg += msgFEL;
                    throw new Exception(msgFEL);
                    
                }
                
               // return msg;
            }
            catch (Exception ex)
            {
                msg += "Error al timbrar comprobante, Error: " + ex.Message;
                throw new Exception("\nError con el servicio SAT " + ex.Message + "\nPor Favor intente mas tarde.Gracias.");
            }
            return msg;
        }
        private string guardarXML(XmlDocument DocumentoXML, string rutaXMLporTimbrar,string RutaXMLTimbrado, string serieF, string folioF)
        {
            //bool ejecuto = false;
            string msg = string.Empty;
           string nombreXMLportimbrar = rutaXMLporTimbrar;// + "Facturas/Factura_" + Factura.folio + ".xml"; // HttpContext.Current.Server.MapPath("~/App_Data/Factura_" + Factura.folio + ".xml");

           try
           {
               DocumentoXML.Save(nombreXMLportimbrar);

               //Timbrar
               msg = timbrar(nombreXMLportimbrar, RutaXMLTimbrado, serieF, folioF);

               //borrar archivo usado para timbrar
               //File.Delete(nombreXMLportimbrar);

               // ejecuto = true;
           }
           catch (Exception ex)
           {
               File.Delete(nombreXMLportimbrar);
               //  ejecuto = false;
               msg += "Error al guardar comprobante, Error: " + ex.Message;
               throw new Exception("\nError al guardar comprobante [" + ex.Message + "].");
           }
            return msg;
        }


        public string cadenaOriginal(string rutaXML,string rutaxlst)
        {
            string msg = "";
            try
            {
                string complemento;
                //Deserialize text file to a new object.
                Comprobante Factura = new Comprobante();
                StreamReader objStreamReader = new StreamReader(rutaXML);
                XmlSerializer Xml = new XmlSerializer(Factura.GetType());
                Factura = (Comprobante)Xml.Deserialize(objStreamReader);
                objStreamReader.Close();

                if (Factura.Complemento.Any != null && Factura.Complemento.Any.Length > 0)
                { 
                    complemento = Factura.Complemento.Any[0].OuterXml;
                    /*
                    Stream s = new MemoryStream(ASCIIEncoding.Default.GetBytes(complemento));

                    TimbreFiscalDigital timbre = new TimbreFiscalDigital();

                    StreamReader objStreamReader2 = new StreamReader(s);
                    Xml = new XmlSerializer(timbre.GetType());
                    timbre = (TimbreFiscalDigital)Xml.Deserialize(objStreamReader2);
                    objStreamReader2.Close();
                    */
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.InnerXml = complemento;

                    XslCompiledTransform transformador = new XslCompiledTransform();
                    transformador.Load(rutaxlst);//System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\cadenaoriginal_TFD_1_0.xslt");
                    StringWriter CadenaOriginal = new StringWriter();
                    transformador.Transform(xmldoc.CreateNavigator(), null, CadenaOriginal);
                    msg = CadenaOriginal.ToString();
                   // _uuid = timbre.UUID;
                   // _fechaTimbrado = timbre.FechaTimbrado;
                   // _noCertificadoSAT = timbre.noCertificadoSAT;
                   // _selloSAT = timbre.selloSAT;
                    //_pdf = _sPath + "\\FacturasElectronicas\\" + nombrePDF + ".pdf";

                  //  ReporteFactura factura = new ReporteFactura(Factura, timbre, CadenaOriginal.ToString(), _LeyendaRegimenFiscal, _LeyendaLibre);
                   // ExportToPDF(_pdf, factura);
                }
            }
            catch (Exception ex)
            {
               
                throw new Exception("Cadena original  Error: " + ex.Message);
            }
            return msg;
        }


        private void LeerXML(string NombreXML,out string msg)
        {

            msg = string.Empty;
            try
            {
                XmlTextReader FlujoReader = new XmlTextReader(NombreXML);
                FlujoReader.WhitespaceHandling = WhitespaceHandling.None;

                //analizar el fichero y presentar cada nodo
                while (FlujoReader.Read())
                {
                    //=====TIPO NODO
                    switch (FlujoReader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                //=====NOMBRE NODO
                                switch (FlujoReader.Name)
                                {
                                    case "Acuse":
                                        {
                                            for (int i = 0; i <= FlujoReader.AttributeCount - 1; i++)
                                            {
                                                FlujoReader.MoveToAttribute(i);

                                                //=====NOMBRE ATRIBUTO
                                                switch (FlujoReader.Name)
                                                {
                                                    case "Fecha":
                                                        {
                                                            DateTime fech = Convert.ToDateTime(FlujoReader.Value);
                                                            _Fecha = fech.ToShortDateString() + " " + fech.ToString("HH:mm:ss");
                                                        } break;
                                                }
                                            }
                                        } break;
                                    case "SignatureValue":
                                        {
                                            _Sello = FlujoReader.ReadString();
                                        } break;
                                }
                            } break;
                    }
                }

            }
            catch (Exception err)
            {
                msg += " Error XML Detalle : " + err.Message;

            }



        }

        private string SaveAcuse(string rutaAcuseHTML, string urlShCP)
        {
            string msg = string.Empty;
            using (FileStream file = new FileStream(rutaAcuseHTML, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter write = new StreamWriter(file, Encoding.UTF8))
                {    //HttpContext.Current.Server.MapPath("~/Facturas_HTML/ImagesQR/" + Factura.Receptor.rfc + Factura.serie + Factura.folio + ".jpg")
                    //
                    //string rutalogo = urlShCP + "/Images/logo_shcp.jpg";
                    write.WriteLine(ACuseHTML(urlShCP, _Fecha, _uuid[0], _Sello, _RFC, out msg));
                    write.Close();
                }
            }
            return msg;
        }
        public static string ACuseHTML(string urlLogo, string fecha, string uuid, string sello, string rfc,out string msg)
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
                msg ="Error al generar Acuse HTML  "+ err.Message;
            }
            return html.ToString();
        }



    }
}
