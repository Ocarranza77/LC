using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Serialization;
using System.Text;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace MvcApplication1.clasesCFDI
{
    public class claseCFDI
    {
        //variables
        private Comprobante Factura = new Comprobante();

        public void generarEmisor()
        {
            try
            {
                #region EMISOR

                ComprobanteEmisor Emisor = new ComprobanteEmisor();
                string nombreemisor = "FAUSTO ABIEZER FLORES ACOSTA";
                string rfc = "FOAF820313CS7";

                if (Global.ComprobarCampo(nombreemisor))
                {
                    Emisor.nombre = nombreemisor;
                }
                if (Global.ComprobarCampo(rfc))
                {
                    Emisor.rfc = rfc;
                }

                #endregion

                #region Domicilio Fiscal

                t_UbicacionFiscal Domicilio = new t_UbicacionFiscal();

                string callefiscal = "C. PINOS";
                string cpfiscal = "22185";
                string coloniafiscal = "COL. LOMA BONITA";
                string estadofiscal = "BAJA CALIFORNIA";
                string localidadfiscal = "TIJUANA";
                string municipiofiscal = "TIJUANA";
                string noexteriorfiscal = "6009";
                string nointeriorfiscal = "";
                string paisfiscal = "MEXICO";
                string referenciafiscal = "";//frente a tortilleria...

                if (Global.ComprobarCampo(callefiscal))
                {
                    Domicilio.calle = callefiscal;
                }
                if (Global.ComprobarCampo(cpfiscal))
                {
                    Domicilio.codigoPostal = cpfiscal;
                }
                if (Global.ComprobarCampo(coloniafiscal))
                {
                    Domicilio.colonia = coloniafiscal;
                }
                if (Global.ComprobarCampo(estadofiscal))
                {
                    Domicilio.estado = estadofiscal;
                }
                if (Global.ComprobarCampo(localidadfiscal))
                {
                    Domicilio.localidad = localidadfiscal;
                }
                if (Global.ComprobarCampo(municipiofiscal))
                {
                    Domicilio.municipio = municipiofiscal;
                }
                if (Global.ComprobarCampo(noexteriorfiscal))
                {
                    Domicilio.noExterior = noexteriorfiscal;
                }
                if (Global.ComprobarCampo(nointeriorfiscal))
                {
                    Domicilio.noInterior = nointeriorfiscal;
                }
                if (Global.ComprobarCampo(paisfiscal))
                {
                    Domicilio.pais = paisfiscal;
                }
                if (Global.ComprobarCampo(referenciafiscal))
                {
                    Domicilio.referencia = referenciafiscal;
                }

                Emisor.DomicilioFiscal = Domicilio;

                #endregion

                #region Domicilio Expedicion

                t_Ubicacion DomicilioExpedicion = new t_Ubicacion();

                string calleexpedicion = "C. PINOS";
                string cpexpedicion = "22185";
                string coloniaexpedicion = "COL. LOMA BONITA";
                string estadoexpedicion = "BAJA CALIFORNIA";
                string localidadexpedicion = "TIJUANA";
                string municipioexpedicion = "TIJUANA";
                string noexteriorexpedicion = "6009";
                string nointeriorexpedicion = "";
                string paisexpedicion = "MEXICO";
                string referenciaexpedicion = "";//frente a tortilleria...


                if (Global.ComprobarCampo(calleexpedicion))
                {
                    DomicilioExpedicion.calle = calleexpedicion;
                }
                if (Global.ComprobarCampo(cpexpedicion))
                {
                    DomicilioExpedicion.codigoPostal = cpexpedicion;
                }
                if (Global.ComprobarCampo(coloniaexpedicion))
                {
                    DomicilioExpedicion.colonia = coloniaexpedicion;
                }
                if (Global.ComprobarCampo(estadoexpedicion))
                {
                    DomicilioExpedicion.estado = estadoexpedicion;
                }
                if (Global.ComprobarCampo(localidadexpedicion))
                {
                    DomicilioExpedicion.localidad = localidadexpedicion;
                }
                if (Global.ComprobarCampo(municipioexpedicion))
                {
                    DomicilioExpedicion.municipio = municipioexpedicion;
                }
                if (Global.ComprobarCampo(noexteriorexpedicion))
                {
                    DomicilioExpedicion.noExterior = noexteriorexpedicion;
                }
                if (Global.ComprobarCampo(nointeriorexpedicion))
                {
                    DomicilioExpedicion.noInterior = nointeriorexpedicion;
                }
                if (Global.ComprobarCampo(paisexpedicion))
                {
                    DomicilioExpedicion.pais = paisexpedicion;
                }
                if (Global.ComprobarCampo(referenciaexpedicion))
                {
                    DomicilioExpedicion.referencia = referenciaexpedicion;
                }

                Emisor.ExpedidoEn = DomicilioExpedicion;

                #endregion

                #region regimen fiscal

                ComprobanteEmisorRegimenFiscal RegimenFiscal = new ComprobanteEmisorRegimenFiscal();
                RegimenFiscal.Regimen = "REGIMEN DE INCORPORACION FISCAL";
                Emisor.RegimenFiscal = new ComprobanteEmisorRegimenFiscal[] { RegimenFiscal };

                #endregion

                Factura.Emisor = Emisor;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar los datos del emisor, Error: " + ex.Message);
            }
        }
        public void generarReceptor()
        {
            try
            {
                //Receptor
                ComprobanteReceptor Receptor = new ComprobanteReceptor();
                t_Ubicacion Domicilio = new t_Ubicacion();

                string nombre = "PUBLICO EN GENERAL";
                string rfc = "XAXX010101000";

                string calle = "C. CONOCIDO";
                string cp = "22000";
                string colonia = "COL. CONOCIDO";
                string estado = "BAJA CALIFORNIA";
                string localidad = "TIJUANA";
                string municipio = "TIJUANA";
                string noexterior = "CONOCIDO";
                string nointerior = "";
                string pais = "MEXICO";
                string referencia = "";

                if (Global.ComprobarCampo(nombre))
                {
                    Receptor.nombre = nombre;
                }
                if (Global.ComprobarCampo(rfc))
                {
                    Receptor.rfc = rfc;
                }

                if (Global.ComprobarCampo(calle))
                {
                    Domicilio.calle = calle;
                }
                if (Global.ComprobarCampo(cp))
                {
                    Domicilio.codigoPostal = cp;
                }
                if (Global.ComprobarCampo(colonia))
                {
                    Domicilio.colonia = colonia;
                }
                if (Global.ComprobarCampo(estado))
                {
                    Domicilio.estado = estado;
                }
                if (Global.ComprobarCampo(localidad))
                {
                    Domicilio.localidad = localidad;
                }
                if (Global.ComprobarCampo(municipio))
                {
                    Domicilio.municipio = municipio;
                }
                if (Global.ComprobarCampo(noexterior))
                {
                    Domicilio.noExterior = noexterior;
                }
                if (Global.ComprobarCampo(nointerior))
                {
                    Domicilio.noInterior = nointerior;
                }
                if (Global.ComprobarCampo(pais))
                {
                    Domicilio.pais = pais;
                }
                if (Global.ComprobarCampo(referencia))
                {
                    Domicilio.referencia = referencia;
                }

                Receptor.Domicilio = Domicilio;

                Factura.Receptor = Receptor;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar los datos del receptor, Error: " + ex.Message);
            }
        }
        public void generarConceptos()
        {
            try
            {
                ComprobanteConcepto[] Conceptos = new ComprobanteConcepto[2];

                Conceptos[0] = new ComprobanteConcepto();
                Conceptos[0].cantidad = 1m;
                Conceptos[0].unidad = "PIEZA";
                Conceptos[0].noIdentificacion = "001";
                Conceptos[0].descripcion = "MONITOR 30 PULGADAS HP";
                Conceptos[0].valorUnitario = 100.00m;
                Conceptos[0].importe = 100.00m;

                Conceptos[1] = new ComprobanteConcepto();
                Conceptos[1].cantidad = 1m;
                Conceptos[1].unidad = "PIEZA";
                Conceptos[1].noIdentificacion = "002";
                Conceptos[1].descripcion = "MOUSE LOGITECH";
                Conceptos[1].valorUnitario = 100.00m;
                Conceptos[1].importe = 100.00m;

                Factura.Conceptos = Conceptos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar Conceptos, Error: " + ex.Message);
            }
        }
        public void generarImpuestos()
        {
            try
            {
                ComprobanteImpuestos impuestos = new ComprobanteImpuestos();
                impuestos.totalImpuestosRetenidosSpecified = true;
                impuestos.totalImpuestosTrasladadosSpecified = true;

                ////Retenciones
                //ComprobanteImpuestosRetencion[] retenciones = new ComprobanteImpuestosRetencion[2];
                //if (Global.ComprobarCampo(fImpuestos["RetISR_Importe"]))
                //{
                //    retenciones[0] = new ComprobanteImpuestosRetencion();
                //    retenciones[0].impuesto = ComprobanteImpuestosRetencionImpuesto.ISR;
                //    retenciones[0].importe = Decimal.Round(Convert.ToDecimal(fImpuestos["RetISR_Importe"]), 2);
                //    impuestos.totalImpuestosRetenidos = Decimal.Round(Convert.ToDecimal(fImpuestos["RetISR_Importe"]), 2);
                //}

                //if (Global.ComprobarCampo(fImpuestos["RetIVA_Importe"]))
                //{
                //    retenciones[1] = new ComprobanteImpuestosRetencion();
                //    retenciones[1].impuesto = ComprobanteImpuestosRetencionImpuesto.IVA;
                //    retenciones[1].importe = Decimal.Round(Convert.ToDecimal(fImpuestos["RetIVA_Importe"]), 2);
                //    impuestos.totalImpuestosRetenidos = Decimal.Round(Convert.ToDecimal(fImpuestos["RetIVA_Importe"]), 2);
                //}

                //if (Global.ComprobarCampo(fImpuestos["RetISR_Importe"]) && Global.ComprobarCampo(fImpuestos["RetIVA_Importe"]))
                //{
                //    impuestos.totalImpuestosRetenidos = (retenciones[0].importe + retenciones[1].importe);
                //}

                //if (retenciones[0] != null || retenciones[1] != null)
                //{ 
                //    impuestos.Retenciones = retenciones; 
                //}

                //Traslados
                ComprobanteImpuestosTraslado[] traslados = new ComprobanteImpuestosTraslado[1];

                traslados[0] = new ComprobanteImpuestosTraslado();
                traslados[0].impuesto = ComprobanteImpuestosTrasladoImpuesto.IVA;
                traslados[0].tasa = 16;

                traslados[0].importe = 32.00m;//tipo decimal
                impuestos.totalImpuestosTrasladados = 32.00m;//tipo decimal

                impuestos.totalImpuestosRetenidosSpecified = true;
                impuestos.totalImpuestosTrasladadosSpecified = true;

                impuestos.Traslados = traslados;

                Factura.Impuestos = impuestos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar impuestos, Error: " + ex.Message);
            }
        }
        public void generarDatosFactura()
        {
            try
            {
                Factura.version = "3.2";
                Factura.serie = "Z";
                Factura.folio = "1";//folio interno de la empresa
                Factura.fecha = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss"));
                Factura.formaDePago = "PAGO EN UNA SOLA EXHIBICION"; // o PAGO EN PARCIALIDADES
                Factura.condicionesDePago = "CONTADO";
                Factura.subTotal = 200.00m;//tipo decimal
                Factura.total = 232.00m;//tipo decimal
                Factura.metodoDePago = "EFECTIVO"; //efectivo, cheque, no identificado...
                //Factura.NumCtaPago = últimos 4 dígitos del número de cuenta cuando paga con cheque, tarjeta, transferencia
                Factura.NumCtaPago = "NO APLICA";
                Factura.LugarExpedicion = "TIJUANA, B.C. MEXICO";
                Factura.tipoDeComprobante = ComprobanteTipoDeComprobante.ingreso;//cuando emitimos factura el tipo de comprobante es ingreso
                Factura.Moneda = "PESOS";// o DOLAR...
                Factura.TipoCambio = "12.50";

                string rutacertificado = HttpContext.Current.Server.MapPath("~/App_Data/csd00001000000301957024.cer");

                Certificado cert = new Certificado(rutacertificado);
                Factura.noCertificado = cert.Serie;//número del certificado otorgado por el SAT
                Factura.certificado = cert.CertificadoBase64;//Certificado en base64

                //el certificado que te da el SAT tiene vigencia
                //si está caducado te va a dar un error y no va a facturar
                //===puedes monitorear las fechas con:
                //   DateTime iniciocertificado = cert.ValidoDesde;
                //   DateTime fincertificado = cert.ValidoHasta;

            }
            catch (Exception ex)
            {
                throw new Exception("Información general de la factura, Error: " + ex.Message);
            }
        }
        public void generarSello()
        {
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
                    string rutaKey = HttpContext.Current.Server.MapPath("~/App_Data/csdFOAF820313CS7.key");//ruta del archivo llave otorgado por el SAT
                    string claveKey = "FOAF820313hsllcs07";//contraseña del archivo llave

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
                        guardarXML(xmlDoc);
                    }
                    else
                    {
                        throw new Exception("Error al generar Sello, el archivo llave no existe: " + rutaKey);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al generar Sello, Error: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar cadena original, Error: " + ex.Message);
            }
        }
        
        public void timbrar(string rutaxmlportimbrar)
        {
            try
            {
                //leer xml por timbrar: "xmlPath"
                string xmlportimbrar = File.ReadAllText(rutaxmlportimbrar, Encoding.UTF8);

                string nombreXMLtimbrado = HttpContext.Current.Server.MapPath("~/App_Data/Timbre_Factura_" + Factura.folio + ".xml");

                string UsuarioPAC = "FOAF820313CS7";//otorgado por la empresa donde contratamos el servicio de timbrado
                string ContrasenaPAC = "NyM5ceR=";//otorgado por la empresa donde contratamos el servicio de timbrado
                string ReferenciaPAC = Factura.serie + Factura.folio;

                WS_FacturarEnLinea.WS_TFD ServicioFel = new WS_FacturarEnLinea.WS_TFD();
                string[] Respuesta = ServicioFel.TimbrarCFD(UsuarioPAC, ContrasenaPAC, xmlportimbrar, ReferenciaPAC);

                string Res0 = Respuesta[0];//alert
                string Res1 = Respuesta[1];//alert
                string Res2 = Respuesta[2];//alert
                string Res3 = Respuesta[3];//xml timbrado

                if (Res0.Trim().Length == 0 & Res1.Trim().Length == 0 & Res2.Trim().Length == 0)
                {
                    // Create the XmlDocument.
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(Res3);

                    // Save the document to a file. White space is
                    // preserved (no white space).
                    doc.PreserveWhitespace = true;
                    doc.Save(nombreXMLtimbrado);

                }
                else
                {
                    string msgFEL = "Factura NO TIMBRADA";
                    msgFEL += Environment.NewLine + Environment.NewLine;
                    msgFEL += Res0 + Environment.NewLine + Environment.NewLine;
                    msgFEL += Res1 + Environment.NewLine + Environment.NewLine;
                    msgFEL += Res2 + Environment.NewLine + Environment.NewLine;

                    throw new Exception(msgFEL);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al timbrar comprobante, Error: " + ex.Message);
            }
        }
        public void guardarXML(XmlDocument DocumentoXML)
        {
            string nombreXMLportimbrar = HttpContext.Current.Server.MapPath("~/App_Data/Factura_" + Factura.folio + ".xml");

            try
            {
                DocumentoXML.Save(nombreXMLportimbrar);

                //Timbrar
                timbrar(nombreXMLportimbrar);

                //borrar archivo usado para timbrar
                //File.Delete(nombreXMLportimbrar);
            }
            catch (Exception ex)
            {
                File.Delete(nombreXMLportimbrar);

                throw new Exception("Error al guardar comprobante, Error: " + ex.Message);
            }
        }
        
    }
}