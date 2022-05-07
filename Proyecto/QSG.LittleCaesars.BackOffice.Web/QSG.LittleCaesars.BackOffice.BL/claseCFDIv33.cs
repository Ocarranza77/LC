using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Linq;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Permissions;

//using MySql.Data.MySqlClient;

using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

using Telerik.Reporting;


using System.Xml.Serialization;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using SAT_Entities = QSG.LittleCaesars.BackOffice.Common.SAT_XML_Entities; // Para la version 4.0 del XML del SAT

using System.Web;

namespace QSG.LittleCaesars.BackOffice.BL //namespace VersatilVentaServidor
{ 
    public class claseCFDIv33
    {
        
        #region Variables
        
        private DataTable _dtConceptosCfdi;
        private DataTable _dtConceptosCfdiTraslados;
        private DataTable _dtConceptosCfdiRetenciones;

        private DateTime _fechaEmision;
        private string _VersionCFDI;
        private string _SerieCFDI;

        private string _uuid;
        private string _cadenaOriginal;
        private string _selloCFDI;
        private string _selloSAT;
        private DateTime _fechaTimbrado;
        private string _CertificadoSAT;
        private string _xmlTimbrado;

        private string _xmlPorTimbrar = string.Empty;


        //public const string URI_SAT = "http://www.sat.gob.mx/cfd/3";
        //public const string URI_XSI = "http://www.w3.org/2001/XMLSchema-instance";
        //public const string XSI_SCHEMALOCATION = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv33.xsd";

        //Estas variables de nivel de modulo nos facilitan las operaciones en las demas subrutinas
        // private static XmlDocument m_xmlDOM;            // Version 3.3 XML del SAT
        // private static XmlElement ComprobanteCfdi;      // Version 3.3 XML del SAT
        private SAT_Entities.Comprobante ComprobanteCfdi40 = new SAT_Entities.Comprobante();  // Version 4.0 XML del SAT

        // TODO: Reemplazar la ClassEmpresa por la propia, igual que 
        // ClassEmpresa Utilizaremos la nuestra
        EmpresaCliente EmpresaCliente; // = new EmpresaCliente(); // Emisor
        Cliente Cliente; // Receptor
        private string _serieTicket;
        private string _folioTicket;
        private string _importeVtaSinIva;
        private string _traslado;
        private string _ImporteVtaConIva;
        private string _TasaIva;

        private string _FechaCancelacion;
        private string _Sello;

        //"rutacer", "rutakey", "clavekey"
        private string _rutacer;
        private string _rutakey;
        private string _clavekey;

        //claseEmpresa ClassEmpresa = new claseEmpresa();
        //claseClientes ClassCliente = new claseClientes();
        //claseClientesDirecciones ClassClienteDireccion = new claseClientesDirecciones();

        //private string _rutaAcuse; //declaracion global
        //private string _rutaLogoSAT;
        //private string _rutaAcuseHTML;




        #endregion


        //constructores
        public claseCFDIv33()
        {
        }
        public claseCFDIv33(string Instruccion, EmpresaCliente prm_EmpresaCliente, Cliente prm_Cliente, string importeVentaSinIva, string traslado, string serie, string folio, string importeVentaConIva, string tasaIva, string rutacer, string rutakey, string clavekey)
        {
            //"rutacer", "rutakey", "clavekey"
            EmpresaCliente = prm_EmpresaCliente;
            Cliente = prm_Cliente;
            _serieTicket = serie;
            _folioTicket = folio;
            _importeVtaSinIva = importeVentaSinIva;
            _traslado = traslado;            // Impuesto trasladado (iva)
            _ImporteVtaConIva = importeVentaConIva;
            _TasaIva = tasaIva;

            _rutacer = rutacer;
            _rutakey = rutakey;
            _clavekey = clavekey;


            //Inicializamos la variable para que contenga el DOM del CFD
            //m_xmlDOM = new XmlDocument();

            //XmlNode Nodo = m_xmlDOM.CreateProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
            //m_xmlDOM.AppendChild(Nodo);

            //creamos el nodo raiz "Comprobante"
            //ComprobanteCfdi = m_xmlDOM.CreateElement("cfdi", "Comprobante", URI_SAT);


            //ClassEmpresa = new claseEmpresa(1);
            //ClassCliente = new claseClientes(claseTemporal.IdCliente);

            //int idfacturar = ClassClienteDireccion.LeerIdFacturar(claseTemporal.IdCliente); //Esto esta en cliente
            //ClassClienteDireccion = new claseClientesDirecciones(idfacturar);


            //siempre leo de tablas temporales
            // TODO: Llena el DataTable con la informacion que nosotros manejamos
            _dtConceptosCfdi = LeerConceptosCfdi(importeVentaSinIva,importeVentaConIva,folio);
            // TODO: Llena el DataTable con la informacion que nosotros manejamos
            _dtConceptosCfdiTraslados = LeerConceptosCfdiTraslados(importeVentaSinIva, traslado, tasaIva);
            
            //_dtConceptosCfdiRetenciones = LeerConceptosCfdiRetenciones();  // No usamos retenciones en el sistema

        }


        public void CrearAtributosComprobante()
        {
            //_VersionCFDI = "3.3";
            _SerieCFDI = _serieTicket;  //ClassEmpresa._SeriedeFactura;
            _fechaEmision = Convert.ToDateTime(claseTemporal.FechaEmision.ToString("yyyy-MM-ddTHH:mm:ss"));
            /*
            //Inicializamos la variable para que contenga el DOM del CFD            
            m_xmlDOM = new XmlDocument();

            XmlNode Nodo = m_xmlDOM.CreateProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
            m_xmlDOM.AppendChild(Nodo);

            //creamos el nodo raiz "Comprobante"
            ComprobanteCfdi = m_xmlDOM.CreateElement("cfdi", "Comprobante", URI_SAT);

             XmlAttribute xsichemaLocation = m_xmlDOM.CreateAttribute("xsi", "schemaLocation", URI_XSI);
             xsichemaLocation.Value = XSI_SCHEMALOCATION;

             ComprobanteCfdi.SetAttribute("xmlns:cfdi", URI_SAT);
             ComprobanteCfdi.SetAttribute("xmlns:xsi", URI_XSI);
             ComprobanteCfdi.SetAttributeNode(xsichemaLocation);


             ComprobanteCfdi.SetAttribute("Version", "3.3");
            ComprobanteCfdi.SetAttribute("Serie", _SerieCFDI);           
            ComprobanteCfdi.SetAttribute("Folio", claseTemporal.SiguienteFolio);            
            ComprobanteCfdi.SetAttribute("Fecha", claseTemporal.FechaEmision.ToString("yyyy-MM-ddTHH:mm:ss"));            
            //sello
            ComprobanteCfdi.SetAttribute("FormaPago", claseTemporal.FormaDePagoClave);            
            ComprobanteCfdi.SetAttribute("NoCertificado", EmpresaCliente.CertificadoCerSerie);// EmpresaCliente.CertificadoCer); //serie ClassEmpresa._SerieCertificado); //?            
            ComprobanteCfdi.SetAttribute("Certificado", EmpresaCliente.CertificadoCer64bits); // EmpresaCliente.CertificadoKey); // certificado 64ClassEmpresa._CertificadoBase64); //?   
            */
            if (claseTemporal.Condiciones != string.Empty)
            {
                //ComprobanteCfdi.SetAttribute("CondicionesDePago", claseTemporal.Condiciones);
                ComprobanteCfdi40.CondicionesDePago = claseTemporal.Condiciones;
            }
            /*
            ComprobanteCfdi.SetAttribute("SubTotal", claseTemporal.Subtotal.ToString());            
            //descuento
            ComprobanteCfdi.SetAttribute("Moneda", claseTemporal.MonedaClave);            
            ComprobanteCfdi.SetAttribute("TipoCambio", claseTemporal.TipoDeCambio);            
            ComprobanteCfdi.SetAttribute("Total", claseTemporal.Total.ToString());            
            ComprobanteCfdi.SetAttribute("TipoDeComprobante", "I");            
            ComprobanteCfdi.SetAttribute("MetodoPago", claseTemporal.MetodoDePagoClave);            
            ComprobanteCfdi.SetAttribute("LugarExpedicion", EmpresaCliente.CP); // ClassEmpresa._CPExpedicion);            
            */
            if (claseTemporal.CodigoConfirmacion != string.Empty)
            {
                //ComprobanteCfdi.SetAttribute("Confirmacion", claseTemporal.CodigoConfirmacion);
                ComprobanteCfdi40.Confirmacion = claseTemporal.CodigoConfirmacion;
            }


            ComprobanteCfdi40.Serie = _SerieCFDI;
            ComprobanteCfdi40.Folio = claseTemporal.SiguienteFolio;
            ComprobanteCfdi40.Fecha = claseTemporal.FechaEmision;
            ComprobanteCfdi40.FormaPago = claseTemporal.FormaDePagoClave;
            ComprobanteCfdi40.NoCertificado = EmpresaCliente.CertificadoCerSerie;
            ComprobanteCfdi40.Certificado = EmpresaCliente.CertificadoCer64bits;

            ComprobanteCfdi40.SubTotal = claseTemporal.Subtotal;
            ComprobanteCfdi40.Moneda = claseTemporal.MonedaClave;
            // Se comenta porque no es requerido (si se manejan MXN) y se deberia mandar en 1 (uno) pero como es decimal lo manda 1.00 lo que marca error de validacion.
            //ComprobanteCfdi40.TipoCambio = Convert.ToDecimal(claseTemporal.TipoDeCambio);  
            ComprobanteCfdi40.Total = claseTemporal.Total;
            ComprobanteCfdi40.TipoDeComprobante = "I";
            ComprobanteCfdi40.MetodoPago = claseTemporal.MetodoDePagoClave;
            ComprobanteCfdi40.LugarExpedicion = EmpresaCliente.CP;

            ComprobanteCfdi40.Exportacion = "01";  // 4.0 +        01.-No; 02.-Si
            


            //m_xmlDOM.AppendChild(ComprobanteCfdi);
            //IndentarNodo(ComprobanteCfdi);
        }
        public void CrearNodoInformacionGlobal()  // 4.0 +
        {
            if (claseTemporal.InformacionGlobal_Periodicidad == string.Empty)
            {
                return;
            }

            ComprobanteCfdi40.InformacionGlobal = new Common.SAT_XML_Entities.InformacionGlobal();
            
            ComprobanteCfdi40.InformacionGlobal.Periodicidad = claseTemporal.InformacionGlobal_Periodicidad; // ClassEmpresa._RFC);
            ComprobanteCfdi40.InformacionGlobal.Meses = claseTemporal.InformacionGlobal_Meses; 
            ComprobanteCfdi40.InformacionGlobal.Anio = claseTemporal.InformacionGlobal_anio; 

        }
        public void CrearNodoEmisor()
        {
            /*  3.3
            XmlElement Emisor = m_xmlDOM.CreateElement("cfdi", "Emisor", URI_SAT);            
            Emisor.SetAttribute("Rfc", EmpresaCliente.RFC); // ClassEmpresa._RFC);            
            Emisor.SetAttribute("Nombre", EmpresaCliente.Nombre); // ClassEmpresa._RazonSocial);            
            Emisor.SetAttribute("RegimenFiscal", "601"); // ClassEmpresa._ClaveRegimenFiscal);            
            IndentarNodo(Emisor);

            ComprobanteCfdi.AppendChild(Emisor);
            IndentarNodo(ComprobanteCfdi);
            */

            // 4.0 +
            ComprobanteCfdi40.Emisor = new Common.SAT_XML_Entities.Emisor();
            ComprobanteCfdi40.Emisor.Rfc = EmpresaCliente.RFC;
            ComprobanteCfdi40.Emisor.Nombre = EmpresaCliente.Nombre;
            ComprobanteCfdi40.Emisor.RegimenFiscal = "601";
            
        }
        public void CrearNodoReceptor()
        {
            /*  3.3
            XmlElement Receptor = m_xmlDOM.CreateElement("cfdi", "Receptor", URI_SAT);
            
            Receptor.SetAttribute("Rfc", Cliente.RFC); // ClassClienteDireccion._Rfc);            
            Receptor.SetAttribute("Nombre", Cliente.RazonSocial); //ClassClienteDireccion._RazonSocial);
            

            //if (ClassClienteDireccion._Extranjero)
            //{
            //    Receptor.SetAttribute("ResidenciaFiscal", ClassClienteDireccion._PaisExtranjero);
            //    Receptor.SetAttribute("NumRegIdTrib", ClassClienteDireccion._RegistroExtranjero);
            //}
            if (Cliente.RFC == "XAXX010101000")
            {
                Receptor.SetAttribute("UsoCFDI", "P01");
                
            }
            else {
                Receptor.SetAttribute("UsoCFDI", "G03"); // claseTemporal.UsoCfdiClave
                //Receptor.SetAttribute("UsoCFDI", "P01"); // claseTemporal.UsoCfdiClave                
            }
            */

            //IndentarNodo(Receptor);
            //ComprobanteCfdi.AppendChild(Receptor);
            //IndentarNodo(ComprobanteCfdi);


            // 4.0 +
            ComprobanteCfdi40.Receptor = new SAT_Entities.Receptor();
            ComprobanteCfdi40.Receptor.Rfc = Cliente.RFC;
            ComprobanteCfdi40.Receptor.Nombre = Cliente.RazonSocial;
            if (Cliente.RFC == "XAXX010101000")
            {
                ComprobanteCfdi40.Receptor.UsoCFDI = "P01";
            }
            else
            {
                ComprobanteCfdi40.Receptor.UsoCFDI = "G03"; // claseTemporal.UsoCfdiClave
            }

            ComprobanteCfdi40.Receptor.DomicilioFiscalReceptor = Cliente.CP;
            ComprobanteCfdi40.Receptor.RegimenFiscalReceptor = Cliente.RegimenFiscal;


        }
        public void CrearNodoConceptos()
        {
            //XmlElement Conceptos = m_xmlDOM.CreateElement("cfdi", "Conceptos", URI_SAT);
            ComprobanteCfdi40.Conceptos = new SAT_Entities.Conceptos();
            var Concepto = new SAT_Entities.Concepto();
            //IndentarNodo(Conceptos);

            foreach (DataRow drConcepto in _dtConceptosCfdi.Rows)
            {
                /* 3.3
                XmlElement Concepto = m_xmlDOM.CreateElement("cfdi", "Concepto", URI_SAT);
                Concepto.SetAttribute("ClaveProdServ", drConcepto["det_claveprodser"].ToString());               
                Concepto.SetAttribute("NoIdentificacion", drConcepto["det_codigo"].ToString());                
                Concepto.SetAttribute("Cantidad", drConcepto["det_cantidad"].ToString());                
                Concepto.SetAttribute("ClaveUnidad", drConcepto["det_claveum"].ToString());                
                Concepto.SetAttribute("Unidad", drConcepto["det_unidaddemedida"].ToString());                
                Concepto.SetAttribute("Descripcion", drConcepto["det_descripcion"].ToString());                
                Concepto.SetAttribute("ValorUnitario", drConcepto["det_preciounitario"].ToString());                
                Concepto.SetAttribute("Importe", drConcepto["det_importeunitario"].ToString());                

                int idtemp = Convert.ToInt32(drConcepto["det_idtemp"]);
                DataTable dtRetenciones = LeerConceptosCfdiRetenciones(idtemp);  // No manejamos retenciones
                */
                //descuento

                // 4.0 
                var ConceptoItem = new SAT_Entities.Concepto();
                ConceptoItem.ClaveProdServ = drConcepto["det_claveprodser"].ToString();
                ConceptoItem.NoIdentificacion = drConcepto["det_codigo"].ToString();
                ConceptoItem.Cantidad = Convert.ToDecimal(drConcepto["det_cantidad"].ToString());
                ConceptoItem.ClaveUnidad = drConcepto["det_claveum"].ToString();
                ConceptoItem.Unidad = drConcepto["det_unidaddemedida"].ToString();
                ConceptoItem.Descripcion = drConcepto["det_descripcion"].ToString();
                ConceptoItem.ValorUnitario = Convert.ToDecimal(drConcepto["det_preciounitario"].ToString());
                ConceptoItem.ObjetoImp = "02";  // Si Objeto a Impuesto  ;  01 No objeto a impuesto

                ConceptoItem.Importe = Convert.ToDecimal(drConcepto["det_importeunitario"].ToString());

                
                DataTable dtTraslados = LeerConceptosCfdiTraslados(_importeVtaSinIva, _traslado,_TasaIva); 
                ConceptoItem.Impuestos = new SAT_Entities.ImpuestosC();

                if (dtTraslados.Rows.Count > 0 ) // No manejamos retenciones
                {
                    /*  3.3
                    XmlElement Impuestos = m_xmlDOM.CreateElement("cfdi", "Impuestos", URI_SAT);
                    IndentarNodo(Impuestos);
                    */


                    if (dtTraslados.Rows.Count > 0)
                    {
                        /* 3.3
                        XmlElement Traslados = m_xmlDOM.CreateElement("cfdi", "Traslados", URI_SAT);
                        IndentarNodo(Traslados);
                        */

                        var Traslados = new SAT_Entities.TrasladosC();

                        foreach (DataRow drTraslado in dtTraslados.Rows)
                        {
                            /* 3.3
                            XmlElement Traslado = m_xmlDOM.CreateElement("cfdi", "Traslado", URI_SAT);
                            Traslado.SetAttribute("Base", drTraslado["tra_base"].ToString());                            
                            Traslado.SetAttribute("Impuesto", drTraslado["tra_impuesto"].ToString());                            
                            Traslado.SetAttribute("TipoFactor", drTraslado["tra_tipofactor"].ToString());                            
                            Traslado.SetAttribute("TasaOCuota", drTraslado["tra_tasaocuota"].ToString());                            
                            Traslado.SetAttribute("Importe", drTraslado["tra_importe"].ToString());
                            
                            Traslados.AppendChild(Traslado);
                            */

                            // 4.0
                            var Traslado = new SAT_Entities.TrasladoC();
                            Traslado.Basee = Convert.ToDecimal(drTraslado["tra_base"].ToString());
                            Traslado.Impuesto = drTraslado["tra_impuesto"].ToString();
                            Traslado.TipoFactor = drTraslado["tra_tipofactor"].ToString();
                            Traslado.TasaOCuota = Convert.ToDecimal(drTraslado["tra_tasaocuota"].ToString());
                            Traslado.Importe = Convert.ToDecimal(drTraslado["tra_importe"].ToString());


                            ConceptoItem.Impuestos.Traslados.Add(Traslado);
                        }

                        //Impuestos.AppendChild(Traslados);  3.3
                    }

                    //if (dtRetenciones.Rows.Count > 0)
                    //{
                    //    XmlElement Retenciones = m_xmlDOM.CreateElement("cfdi", "Retenciones", URI_SAT);
                    //    IndentarNodo(Retenciones);

                    //    foreach (DataRow drRetencion in dtRetenciones.Rows)
                    //    {
                    //        XmlElement Retencion = m_xmlDOM.CreateElement("cfdi", "Retencion", URI_SAT);
                    //        Retencion.SetAttribute("Base", drRetencion["ret_base"].ToString());
                    //        Retencion.SetAttribute("Impuesto", drRetencion["ret_impuesto"].ToString());
                    //        Retencion.SetAttribute("TipoFactor", drRetencion["ret_tipofactor"].ToString());
                    //        Retencion.SetAttribute("TasaOCuota", drRetencion["ret_tasaocuota"].ToString());
                    //        Retencion.SetAttribute("Importe", drRetencion["ret_importe"].ToString());

                    //        Retenciones.AppendChild(Retencion);
                    //    }

                    //    Impuestos.AppendChild(Retenciones);
                    //}


                    /* 3.3
                    Concepto.AppendChild(Impuestos);
                    IndentarNodo(Impuestos);
                    */
                }


                ComprobanteCfdi40.Conceptos.Concepto.Add(ConceptoItem);
                //Conceptos.AppendChild(Concepto);
                //IndentarNodo(Conceptos);
            }

            //ComprobanteCfdi.AppendChild(Conceptos);
            //IndentarNodo(ComprobanteCfdi);
        }
        public void CrearNodoImpuestos()
        {
            decimal totTraslado = claseTemporal.IvaTotal + claseTemporal.IepsTotal;
            decimal totRetenido = claseTemporal.RetencionISR + claseTemporal.RetencionIVA + claseTemporal.RetencionIEP;

            //XmlElement Impuestos = m_xmlDOM.CreateElement("cfdi", "Impuestos", URI_SAT);  3.3
            var Impuestos = new SAT_Entities.Impuestos();
            if (totTraslado > 0m)
            {
                //Impuestos.SetAttribute("TotalImpuestosTrasladados", totTraslado.ToString()); 3.3
                Impuestos.TotalImpuestosTrasladados = totTraslado;
            }
            if (totRetenido > 0m)
            {
                //Impuestos.SetAttribute("TotalImpuestosRetenidos", totRetenido.ToString());  3.3
                Impuestos.TotalImpuestosRetenidos = totRetenido;
            }
            //IndentarNodo(Impuestos);  3.3


            if (totTraslado > 0m)
            {
                /* 3.3
                XmlElement Traslados = m_xmlDOM.CreateElement("cfdi", "Traslados", URI_SAT);                
                IndentarNodo(Traslados);
                */

                DataTable dtTraslados = LeerConceptosCfdiTraslados(_importeVtaSinIva, _traslado,_TasaIva);//LeerConceptosCfdiTrasladosGlobal(); // Solo manejamos una sola Retencion, hay que usar LeerConceptosCfdiTraslados

                foreach (DataRow drTraslado in dtTraslados.Rows) // TODO: podemos usar lo que tenemos en LeerConceptosCfdiTraslados.
                {
                    /*  3.3
                    XmlElement Traslado = m_xmlDOM.CreateElement("cfdi", "Traslado", URI_SAT);                    
                    Traslado.SetAttribute("Impuesto", drTraslado["tra_impuesto"].ToString());                    
                    Traslado.SetAttribute("TipoFactor", drTraslado["tra_tipofactor"].ToString());                    
                    Traslado.SetAttribute("TasaOCuota", drTraslado["tra_tasaocuota"].ToString());                    
                    Traslado.SetAttribute("Importe", drTraslado["tra_importe"].ToString());
                    
                    Traslados.AppendChild(Traslado);
                    */

                    // 4.0
                    var Traslado = new SAT_Entities.Traslado();
                    Traslado.Impuesto = drTraslado["tra_impuesto"].ToString();
                    Traslado.TipoFactor = drTraslado["tra_tipofactor"].ToString();
                    Traslado.TasaOCuota = Convert.ToDecimal(drTraslado["tra_tasaocuota"].ToString());
                    Traslado.Importe = Convert.ToDecimal(drTraslado["tra_importe"].ToString());
                    Traslado.Basee = Convert.ToDecimal(drTraslado["tra_base"].ToString()); //Traslado.Importe;

                    Impuestos.Traslados.Add(Traslado);
                }

                //Impuestos.AppendChild(Traslados); 3.3
            }


            //if (totRetenido > 0)
            //{
            //    XmlElement Retenciones = m_xmlDOM.CreateElement("cfdi", "Retenciones", URI_SAT);
            //    IndentarNodo(Retenciones);

            //    DataTable dtRetenciones = LeerConceptosCfdiTrasladosGlobal();
            //    foreach (DataRow drRetencion in dtRetenciones.Rows)
            //    {
            //        XmlElement Retencion = m_xmlDOM.CreateElement("cfdi", "Retencion", URI_SAT);
            //        Retencion.SetAttribute("Impuesto", drRetencion["ret_impuesto"].ToString());
            //        Retencion.SetAttribute("Importe", drRetencion["ret_importe"].ToString());

            //        Retenciones.AppendChild(Retencion);
            //    }

            //    Impuestos.AppendChild(Retenciones);
            //}

            /* 3.3
            ComprobanteCfdi.AppendChild(Impuestos);
            IndentarNodo(ComprobanteCfdi);
            */

            // 4.0
            ComprobanteCfdi40.Impuestos = new SAT_Entities.Impuestos();
            ComprobanteCfdi40.Impuestos = Impuestos;
            
        }
        // 3.3   En el  4.0  con la clase de Fausto se sella en  generarxml.GuardarXMLPorCertificado
        public void SellarCfdi()
        {
            string Sello = GenerarSello();

            //ComprobanteCfdi.SetAttribute("Sello", Sello); 3.3
            ComprobanteCfdi40.Sello = Sello;
        }
        public void GuardarCfdi()
        {
            /*
            //_xmlPorTimbrar = @"\XML\" + Cliente.RFC + _serieTicket + _folioTicket + ".xml";//ClassEmpresa._CarpetaLocal + "\\FACTURAS\\Factura.xml";
            _xmlPorTimbrar = HttpContext.Current.Server.MapPath("/XML/" + Cliente.RFC + _serieTicket + _folioTicket + ".xml");

            //agregar prefijo xsi al atributo schemaLocation
            //m_xmlDOM.InnerXml = m_xmlDOM.InnerXml.Replace("schemaLocation", "xsi:schemaLocation");

            m_xmlDOM.PreserveWhitespace = true;
            m_xmlDOM.Save(_xmlPorTimbrar);
            */
            _xmlPorTimbrar = HttpContext.Current.Server.MapPath("/XML/" + Cliente.RFC + _serieTicket + _folioTicket + ".xml");

            SAT_Entities.zzGenerarXML generarxml = new SAT_Entities.zzGenerarXML();
            string xmlcadena = generarxml.GuardarXMLPorCertificado(ComprobanteCfdi40, _rutacer, _rutakey, _clavekey, _xmlPorTimbrar);
            
            
        }
        public void timbrarXML()
        {
            bool success = false;
            

            //leer xml por timbrar
            string xmlportimbrar = File.ReadAllText(_xmlPorTimbrar, Encoding.UTF8);
            string nombreXML = _serieTicket + _folioTicket; //ClassEmpresa._SeriedeFactura + claseTemporal.SiguienteFolio;

            
            #region FEL

            string referenciafel = "factura" + nombreXML;

            FELv33.WSCFDI33Client ServicioFel = new FELv33.WSCFDI33Client();
//            FELv33.RespuestaTFD33 RespuestaFel = ServicioFel.TimbrarCFDI(ClassEmpresa._UsuarioPac, ClassEmpresa._ContrasenaPac, xmlportimbrar, referenciafel);
            FELv33.RespuestaTFD33 RespuestaFel = ServicioFel.TimbrarCFDI(EmpresaCliente.UserPak, EmpresaCliente.ClavePak, xmlportimbrar, referenciafel);
            ServicioFel.Close();

            if (RespuestaFel.OperacionExitosa)
            {
                success = true;
                claseTemporal.CFDIGenerado = true;

                //respaldo para tenerlo en tabla como string
                _xmlTimbrado = RespuestaFel.XMLResultado;

                _TimbradoDatos = new Timbrado();

                _TimbradoDatos.Estado = RespuestaFel.Timbre.Estado;
                _TimbradoDatos.FechaTimbrado = RespuestaFel.Timbre.FechaTimbrado;
                _TimbradoDatos.NumeroCertificadoSAT = RespuestaFel.Timbre.NumeroCertificadoSAT;
                _TimbradoDatos.SelloCFD = RespuestaFel.Timbre.SelloCFD;
                _TimbradoDatos.SelloSAT = RespuestaFel.Timbre.SelloSAT;
                _TimbradoDatos.UUID = RespuestaFel.Timbre.UUID;
                _TimbradoDatos.XML = RespuestaFel.XMLResultado;
                //bandera exito
               

            }
            else
            {
                //CodigoConfirmacion lo regresa el pac en la respuesta
                //me sirve para volver a facturar si importe o tipo de cambio está
                claseTemporal.CodigoConfirmacion = RespuestaFel.CodigoConfirmacion;

                string msgFEL = "FACTURA NO TIMBRADA";

                try
                {
                    msgFEL += Environment.NewLine;
                    msgFEL += RespuestaFel.MensajeError.ToUpper();
                }
                catch (Exception)
                {
                }

                try
                {
                    msgFEL += Environment.NewLine;
                    msgFEL += RespuestaFel.MensajeErrorDetallado.ToUpper();
                }
                catch (Exception)
                {
                }

                //frmAlerta alerta = new frmAlerta();
                //alerta._Titulo = "FACTURAS";
                //alerta._Mensaje = msgFEL;
                //alerta.ShowDialog();
            }

            #endregion
            


            if (success)
            {
                // TODO: Revisar donde quedara este xml para modificar el path
                //                string xmlexitoso = ClassEmpresa._CarpetaLocal + "\\FACTURAS\\Factura_" + nombreXML + ".xml";
                 
                //string xmlexitoso = @"\FACTURAS\" + Cliente.RFC + _serieTicket + _folioTicket + ".xml";
                string xmlexitoso = HttpContext.Current.Server.MapPath("/Facturas/" + Cliente.RFC + _serieTicket + _folioTicket + ".xml");

                //guardo para leerle la info del timbre
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_xmlTimbrado);
                doc.PreserveWhitespace = true;
                doc.Save(xmlexitoso);

                //pasar a variables globales la info
                extraerTimbre(xmlexitoso);


                //borrar xml borrador
                try
                {
                    File.Delete(_xmlPorTimbrar);
                }
                catch (Exception)
                {
                }
                //bandera exito
                claseTemporal.CFDIGenerado = true;
              
            }
        }


        public string Cancelar(string _Clave, string _UserPak, string _ClavePak, string _rutaLogoSAT, string _rutaFacturas, string _RutaPKcs12, string _rutaXML, ComprobanteEmisor Emisor, List<string> LstUUID, string _rutaAcuse, string _rutaAcuseHTML, string rfcReceptor, double totalCfdi, out string msg, out bool success)
        {

            msg = string.Empty;
            var xml = "";


            FELv33.RespuestaCancelacion _respuestaCancelacion = new FELv33.RespuestaCancelacion();
            FELv33.RespuestaTFD33 _respuestaTFD = new FELv33.RespuestaTFD33();
            FELv33.WSCFDI33Client FELService = new FELv33.WSCFDI33Client();
            
            FELv33.DetalleCFDICancelacion DetalleCancelacionSingle = new FELv33.DetalleCFDICancelacion();
            DetalleCancelacionSingle.RFCReceptor = rfcReceptor;
            DetalleCancelacionSingle.Total = Convert.ToDecimal(totalCfdi);  
            DetalleCancelacionSingle.UUID = LstUUID[0];

            FELv33.DetalleCFDICancelacion[] DetalleCancelacion = new FELv33.DetalleCFDICancelacion[1];
            DetalleCancelacion[0] = DetalleCancelacionSingle;

            string certificadoPkcs12 = File.ReadAllText(_RutaPKcs12);

            _respuestaCancelacion = FELService.CancelarCFDIConValidacion(_UserPak, _ClavePak, Emisor.rfc, DetalleCancelacion, certificadoPkcs12, _Clave);
            //_respuestaCancelacion = FELService.CancelarCFDI(_UserPak , _ClavePak, Emisor.rfc, LstUUID.ToArray(), certificadoPkcs12, _Clave);

            List<FELv33.DetalleCancelacion> respuestaDetallada = _respuestaCancelacion.DetallesCancelacion.ToList();
            FELv33.DetalleCancelacion uuidcancelado = respuestaDetallada[0];

            success = false;

            string mensajeresultado = string.Empty;
            try
            {
                mensajeresultado = uuidcancelado.MensajeResultado.ToLower();
            }
            catch (Exception)
            {
            }

            switch (uuidcancelado.CodigoResultado)
            {
                case "201"://cancelado normal
                    {
                        success = true; //si va

                        //GUARDAR ACUSE EN DISCO DURO
                        //LEER INFO DEL ACUSE
                        //CANCELAR FACTURA EN SISTEMA
                        #region XML

                                             

                        XmlDocument acusexml = new XmlDocument();
                        acusexml.LoadXml(_respuestaCancelacion.XMLAcuse);
                        acusexml.PreserveWhitespace = true;
                        acusexml.Save(_rutaAcuse);

                        xml = _respuestaCancelacion.XMLAcuse;
                        //xml = _respuestaTFD.XMLResultado;
                        //msg += "Factura se encuentra cancelada" + _respuestaTFD.Timbre.FechaTimbrado.ToShortDateString() + _respuestaTFD.Timbre.UUID.ToString() + _respuestaTFD.Timbre.SelloSAT +  Emisor.rfc + _rutaLogoSAT + _rutaAcuseHTML; 
                        //msg += SaveAcuse( _respuestaTFD.Timbre.FechaTimbrado.ToShortDateString(), _respuestaTFD.Timbre.UUID.ToString(), _respuestaTFD.Timbre.SelloSAT, Emisor.rfc, _rutaLogoSAT, _rutaAcuseHTML);
                        msg = "";

                        //sacar fecha y sello
                        //LeerAcuseXML(_rutaXML);
                        //actualizar campos de cancelación
                        //ClassCfdi.UpdateAcuseXml(_IdCFDI, _FechaCancelacion, _Sello, _MotivoCancelacion, RespuestaFel.XMLAcuse);
                        
                        #endregion
                        //return xml;
                    } break;
                case "CANC103"://Cancelado Por aceptación del receptor
                case "CANC107"://Cancelado Por plazo vencido
                    {
                        //obtener acuse de cancelación
                        var uuid = LstUUID[0].ToString();
                        _respuestaTFD = FELService.ObtenerAcuseCancelacion(_UserPak, _ClavePak, uuid);
                        

                        if (_respuestaTFD.CodigoRespuesta == "800")
                        {
                            success = true;    //si va

                            #region XML

                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(_respuestaTFD.XMLResultado);
                            doc.PreserveWhitespace = true;
                            doc.Save(_rutaXML);

                            xml = _respuestaCancelacion.XMLAcuse;
                            msg = "";
                            //xml = _respuestaTFD.XMLResultado;
                            //msg += SaveAcuse(_respuestaTFD.Timbre.FechaTimbrado.ToShortDateString(), _respuestaTFD.Timbre.UUID.ToString(), _respuestaTFD.Timbre.SelloSAT, Emisor.rfc, _rutaLogoSAT, _rutaAcuseHTML);

                            
                            //actualizar campos de cancelación
                            //ClassCfdi.UpdateAcuseXml(_IdCFDI, _FechaCancelacion, _Sello, _MotivoCancelacion, respuestaAcuse.XMLResultado);

                            //return xml;
                            #endregion
                        }
                    } break;
                case "CANC999"://Error no clasificado
                    {
                        if (mensajeresultado.Contains("ya se encuentra cancelado"))
                        {
                            //obtener acuse de cancelación
                            //FELv33.WSCFDI33Client servicioFEL = new FELv33.WSCFDI33Client();
                            //FELv33.RespuestaTFD33 respuestaAcuse = ServicioFEL.ObtenerAcuseCancelacion(ClassEmpresa._UsuarioPac, ClassEmpresa._ContrasenaPac, _UUID);
                            var uuid = LstUUID[0].ToString();
                            _respuestaTFD = FELService.ObtenerAcuseCancelacion(_UserPak, _ClavePak, uuid);

                            if (_respuestaTFD.CodigoRespuesta == "800")
                            {

                                success = false;

                                #region XML

                                //XmlDocument doc = new XmlDocument();
                                //doc.LoadXml(_respuestaTFD.XMLResultado);
                                //doc.PreserveWhitespace = true;
                                //doc.Save(_rutaXML);

                                
                                xml = _respuestaTFD.XMLResultado;
                                msg += "Factura se encuentra cancelada" + _respuestaTFD.Timbre.FechaTimbrado.ToShortDateString()+_respuestaTFD.Timbre.UUID.ToString() +_respuestaTFD.Timbre.SelloSAT +_rutaAcuseHTML ; 
                                //msg += SaveAcuse(_respuestaTFD.Timbre.FechaTimbrado.ToShortDateString(), _respuestaTFD.Timbre.UUID.ToString(), _respuestaTFD.Timbre.SelloSAT, Emisor.rfc, _rutaLogoSAT, _rutaAcuseHTML);
                                //return "";
                                #endregion
                            }
                        }
                        else
                        {
                            msg += "FACTURA NO CANCELADA" + Environment.NewLine + mensajeresultado;
                            msg += "error al obtener XMLAcuse" + System.Environment.NewLine;
                            msg += _respuestaTFD.MensajeError;
                            //return "";
                            //ClassCfdi.UpdateComentariosCancelacion(_IdCFDI, mensajeresultado);
                            //claseTemporal.Alerta("alert", "FACTURAS", msgFEL);
                        }
                    } break;
                default:
                    {
                        msg += "FACTURA NO CANCELADA";
                        string msgcancelacion = string.Empty;
                        //return msg;

                        try
                        {
                            msg += Environment.NewLine;
                            msg += _respuestaCancelacion.MensajeError.ToUpper();
                          //  return msg;
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            msg += Environment.NewLine;
                            msg += _respuestaCancelacion.MensajeErrorDetallado.ToUpper();
                            //return msg;
                        }
                        catch (Exception)
                        {
                        }

                        msg += Environment.NewLine + mensajeresultado;
                        //return msg;

                         
                    } break;
            }

            //cerrar servicio
            //ServicioFEL.Close();





            return xml;   



            // en eteoria esto ya no va ========================================================================================================
            //if (_respuestaCancelacion.OperacionExitosa)
            //{
            //    var uuid = LstUUID[0].ToString();
            //    _respuestaTFD = FELService.ObtenerAcuseCancelacion(_UserPak, _ClavePak, uuid);
            //    if (_respuestaTFD.CodigoRespuesta == "800")
            //    {

            //        XmlDocument acusexml = new XmlDocument();
            //        acusexml.LoadXml(_respuestaTFD.XMLResultado);
            //        acusexml.Save(_rutaAcuse);
            //        xml = _respuestaTFD.XMLResultado;

            //        msg += SaveAcuse(_respuestaTFD.Timbre.FechaTimbrado.ToShortDateString(), _respuestaTFD.Timbre.UUID.ToString(), _respuestaTFD.Timbre.SelloSAT, Emisor.rfc, _rutaLogoSAT, _rutaAcuseHTML);


            //    }
            //    else
            //    {
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
                                
        }
        private string SaveAcuse(string fecha, string uuid, string selloSAT, string rfcEmisor, string _rutaLogoSAT, string _rutaAcuseHTML)
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


        private void LeerAcuseXML(string NombreXML)
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
                                                        _FechaCancelacion = fech.ToString("yyyy-MM-ddThh:mm:ss");
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

        #region  QBIC
        //--------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------
        //                                                              Q B I C 
        //--------------------------------------------------------------------------------------------------------------------------

        public Timbrado _TimbradoDatos;

        


        // TODO: Llenar la informacion y las operaciones
        private DataTable LeerConceptosCfdi(string importeVentaSinIva, string importeVentaConIva, string FolioR)
        {
            
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("det_cantidad");
            dt.Columns.Add("det_codigo");
            dt.Columns.Add("det_descripcion");
            dt.Columns.Add("det_claveprodser");
            dt.Columns.Add("det_claveum");
            dt.Columns.Add("det_unidaddemedida");
            dt.Columns.Add("det_precio");
            dt.Columns.Add("det_importe");
            dt.Columns.Add("det_preciounitario");
            dt.Columns.Add("det_importeunitario");
            dt.Columns.Add("det_idarticulo");
            dt.Columns.Add("det_idpaquetereceta");
            dt.Columns.Add("det_espaquete");
            dt.Columns.Add("det_ivas");
            dt.Columns.Add("det_ieps");
            dt.Columns.Add("det_retisr");
            dt.Columns.Add("det_retiva");
            dt.Columns.Add("det_retiep");
            DataRow _ravi = dt.NewRow();
            _ravi["det_cantidad"] = "1";
            _ravi["det_codigo"] = FolioR;
            _ravi["det_descripcion"] = "Establecimientos de comida rapida";
            _ravi["det_claveprodser"] = "90101503";
            _ravi["det_claveum"] = "E48";
            _ravi["det_unidaddemedida"] = "SER";
            _ravi["det_precio"] = importeVentaConIva;
            _ravi["det_importe"] = importeVentaConIva;
            _ravi["det_preciounitario"] = importeVentaSinIva; ;
            _ravi["det_importeunitario"] = importeVentaSinIva;
            _ravi["det_idarticulo"] = "";
            _ravi["det_idpaquetereceta"] = "";
            _ravi["det_espaquete"] = "";
            _ravi["det_ivas"] = "32";
            _ravi["det_ieps"] = "";
            _ravi["det_retisr"] = "0";
            _ravi["det_retiva"] = "0";
            _ravi["det_retiep"] = "0";
            dt.Rows.Add(_ravi);

            return dt;
        }

        // TODO: Llenar la informacion y las operaciones
        private DataTable LeerConceptosCfdiTraslados(string importeVtaSinIva, string traslado, string tasaIva)
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("det_cantidad");
            dt.Columns.Add("tra_id");
            dt.Columns.Add("tra_idtemp");
            dt.Columns.Add("tra_importe");
            dt.Columns.Add("tra_tasa");
            dt.Columns.Add("tra_tasaocuota");
            dt.Columns.Add("tra_tipofactor");
            dt.Columns.Add("tra_impuesto");
            dt.Columns.Add("tra_descripcion");
            dt.Columns.Add("tra_base");
            dt.Columns.Add("tra_tasafactorimpuesto");

            

            DataRow _ravi = dt.NewRow();
            _ravi["det_cantidad"] = "1";
            _ravi["tra_id"] = "1";
            _ravi["tra_idtemp"] = "1";
            _ravi["tra_importe"] =  traslado; //*.16
            _ravi["tra_tasa"] = tasaIva;//"16";

            _ravi["tra_tasaocuota"] = (Convert.ToDecimal(tasaIva) / 100).ToString("0.000000");// "0.160000";

            
            _ravi["tra_tipofactor"] = "Tasa";
            _ravi["tra_impuesto"] = "002";

            _ravi["tra_descripcion"] = String.Format("IVA {0}%", tasaIva); //"IVA 16%";

            _ravi["tra_base"] = importeVtaSinIva;

            _ravi["tra_tasafactorimpuesto"] = String.Format("{0}Tasa002", (Convert.ToDecimal(tasaIva) / 100).ToString("0.000000"));// "0.160000Tasa002";

            dt.Rows.Add(_ravi);

            return dt;
        }

        /// <summary>
        /// Este metodo ejecuta los metodos para emitir una factura en el orden necesario, se deben tener los datos puestos en la "claseTemporal" y pasar los necesarios en el constructor.
        /// </summary>
        public void EjecutarSecuencia()
        {
            CrearAtributosComprobante();

            CrearNodoInformacionGlobal();

            CrearNodoEmisor();

            CrearNodoReceptor();

            CrearNodoConceptos();

            CrearNodoImpuestos();

            // SellarCfdi(); La nueva clase del XML 4.0 ya lo sella en GuardarXMLPorCertificado

            GuardarCfdi();

            timbrarXML();

            //GC.Collect();
        }

        /*  Se ulitizara LeerConceptosCfdiTraslados ya que solo se maneja de uno en uno, de usarse mas se implementaria.
        private DataTable LeerConceptosCfdiTrasladosGlobal()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("tra_importe");
            dt.Columns.Add("tra_id");
            dt.Columns.Add("tra_id");
            dt.Columns.Add("tra_id");
            DataRow _ravi = dt.NewRow();
            _ravi["det_cantidad"] = "";
            dt.Rows.Add(_ravi);
        // Solo manejamos una sola Retencion, hay que usar LeerConceptosCfdiTraslados
        /*
        string query = "SELECT ";
query += "SUM(tra_importe) AS tra_importe, ";
query += "tra_tasaocuota, ";
query += "tra_tipofactor, ";
query += "tra_impuesto ";

            return dt;


        }
*/

#endregion

        #region Ejemplo DataTable

        // Ejemplo 1
        /*   Crea el DataTable
        DataSet customerDS = new DataSet("CustomerOrders");
        DataTable ordersTable = customerDS.Tables.Add("Orders");
        DataColumn pkCol = ordersTable.Columns.Add("OrderID", typeof(Int32));
        ordersTable.Columns.Add("OrderQuantity", typeof(Int32));
        ordersTable.Columns.Add("CompanyName", typeof(string));
        ordersTable.PrimaryKey = new DataColumn[] {pkCol};
        */
        /*  Agrega Registros al DataTable
        DataRow workRow;
        for (int i = 0; i <= 9; i++) 
        {
        workRow = currentTable.NewRow();
        workRow[0] = i; 
        workRow[1] = "CustName" + i.ToString();
        currentTable.Rows.Add(workRow);
        } 
        */
        // Ejemplo 2
        /*  Crea y llena un DataTable
                DataTable dt = new DataTable(); 
        dt.Clear();
        dt.Columns.Add("Name");
        dt.Columns.Add("Marks");
        DataRow _ravi = dt.NewRow();
        _ravi["Name"] = "ravi";
        _ravi["Marks"] = "500";
        dt.Rows.Add(_ravi);

         * 
         * dt.WriteXMLSchema("dtSchemaOrStructure.xml");
         * dt.WriteXML("dtDataxml");
                */


        #endregion


        public bool PDF(ComprobanteEmisor Emisor, Comprobante Factura, Sucursal sucursal, string _rutaXlst, String[] DatosAdicionales, string _rutaFacturas, out string msg)
        {


            var estatus = false;
            string _fechaFacturacion;

            msg = string.Empty;
            try
            {


                // Esta funcion carga el timbrado xml en la variable _XMLTimbrado, en esta clase se maneja la siguiente variable _xmlTimbrado
                //XMLTimbrado(out msg);/* carga xml */  

                //Hashtable deviceInfo = new Hashtable();

                Telerik.Reporting.InstanceReportSource reptoPDF = new InstanceReportSource();
                // Original
                //reptoPDF.ReportDocument = new Report1(Emisor, Factura, sucursal, _XMLTimbrado, _rutaXlst, DatosAdicionales, out _fechaFacturacion, out msg);
                reptoPDF.ReportDocument = new Report1(Emisor, Factura, sucursal, _xmlTimbrado, _rutaXlst, DatosAdicionales, out _fechaFacturacion, out msg);
                /* reptoPDF.ReportDocument = new ReportesCFDI.ReporteAcuseCancelacion(_Fecha, _RFC, _UUID, _Sello);
                 */
                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
                Hashtable deviceInfo = new Hashtable();
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

        //--------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------
        //   FIN                                               Q B I C 
        //--------------------------------------------------------------------------------------------------------------------------

        #region Conceptos CFDI   BASE DE DATOS
        /*
        public bool InsertConceptosCfdi(
            int IdTemp,
            string Cantidad,
            string Codigo,
            string Descripcion,
            string ClaveProdSer,
            string ClaveUm,
            string UnidadDeMedida,
            string Precio,
            string Importe,
            string PrecioUnitario,
            string ImporteUnitario,
            int IdArticulo,
            int IdPaquete,
            int EsPaquete,
            string Ivas,
            string Ieps,
            string Retisr,
            string Retiva,
            string Retiep)
        {
            string instruccionDatos = "INSERT conceptoscfdi ";
            instruccionDatos += "(det_idtemp, ";
            instruccionDatos += "det_cantidad, ";
            instruccionDatos += "det_codigo, ";
            instruccionDatos += "det_descripcion, ";
            instruccionDatos += "det_claveprodser, ";
            instruccionDatos += "det_claveum, ";
            instruccionDatos += "det_unidaddemedida, ";
            instruccionDatos += "det_precio, ";
            instruccionDatos += "det_importe, ";
            instruccionDatos += "det_preciounitario, ";
            instruccionDatos += "det_importeunitario, ";
            instruccionDatos += "det_idarticulo, ";
            instruccionDatos += "det_idpaquetereceta, ";
            instruccionDatos += "det_espaquete, ";
            instruccionDatos += "det_ivas, ";
            instruccionDatos += "det_ieps, ";
            instruccionDatos += "det_retisr, ";
            instruccionDatos += "det_retiva, ";
            instruccionDatos += "det_retiep) ";
            instruccionDatos += "VALUES ";
            instruccionDatos += "(@det_idtemp, ";
            instruccionDatos += "@det_cantidad, ";
            instruccionDatos += "@det_codigo, ";
            instruccionDatos += "@det_descripcion, ";
            instruccionDatos += "@det_claveprodser, ";
            instruccionDatos += "@det_claveum, ";
            instruccionDatos += "@det_unidaddemedida, ";
            instruccionDatos += "@det_precio, ";
            instruccionDatos += "@det_importe, ";
            instruccionDatos += "@det_preciounitario, ";
            instruccionDatos += "@det_importeunitario, ";
            instruccionDatos += "@det_idarticulo, ";
            instruccionDatos += "@det_idpaquetereceta, ";
            instruccionDatos += "@det_espaquete, ";
            instruccionDatos += "@det_ivas, ";
            instruccionDatos += "@det_ieps, ";
            instruccionDatos += "@det_retisr, ";
            instruccionDatos += "@det_retiva, ";
            instruccionDatos += "@det_retiep)";

            MySqlCommand comandoDatos = new MySqlCommand(instruccionDatos);

            //parámetros
            comandoDatos.Parameters.Add("@det_idtemp", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@det_cantidad", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_codigo", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_descripcion", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_claveprodser", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_claveum", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_unidaddemedida", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_precio", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_importe", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_preciounitario", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_importeunitario", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_idarticulo", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@det_idpaquetereceta", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@det_espaquete", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@det_ivas", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_ieps", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_retisr", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_retiva", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_retiep", MySqlDbType.VarChar);

            //pasar atributos de la clase a parámetros
            comandoDatos.Parameters["@det_idtemp"].Value = IdTemp;
            comandoDatos.Parameters["@det_cantidad"].Value = Cantidad;
            comandoDatos.Parameters["@det_codigo"].Value = Codigo;
            comandoDatos.Parameters["@det_descripcion"].Value = Descripcion;
            comandoDatos.Parameters["@det_claveprodser"].Value = ClaveProdSer;
            comandoDatos.Parameters["@det_claveum"].Value = ClaveUm;
            comandoDatos.Parameters["@det_unidaddemedida"].Value = UnidadDeMedida;
            comandoDatos.Parameters["@det_precio"].Value = Precio;
            comandoDatos.Parameters["@det_importe"].Value = Importe;
            comandoDatos.Parameters["@det_preciounitario"].Value = PrecioUnitario;
            comandoDatos.Parameters["@det_importeunitario"].Value = ImporteUnitario;
            comandoDatos.Parameters["@det_idarticulo"].Value = IdArticulo;
            comandoDatos.Parameters["@det_idpaquetereceta"].Value = IdPaquete;
            comandoDatos.Parameters["@det_espaquete"].Value = EsPaquete;
            comandoDatos.Parameters["@det_ivas"].Value = Ivas;
            comandoDatos.Parameters["@det_ieps"].Value = Ieps;
            comandoDatos.Parameters["@det_retisr"].Value = Retisr;
            comandoDatos.Parameters["@det_retiva"].Value = Retiva;
            comandoDatos.Parameters["@det_retiep"].Value = Retiep;

            //ejecutar comando
            return claseConexion.EjecutarComando(comandoDatos);
        }

        public bool UpdateConceptosCfdi(
            int IdTemp,
            string Cantidad,
            string Descripcion,
            string Precio,
            string Importe,
            string PrecioUnitario,
            string ImporteUnitario,
            string Ivas,
            string Ieps,
            string Retisr,
            string Retiva,
            string Retiep)
        {
            string instruccionDatos = "UPDATE conceptoscfdi SET ";
            instruccionDatos += "det_cantidad = @det_cantidad, ";
            instruccionDatos += "det_descripcion = @det_descripcion, ";
            instruccionDatos += "det_precio = @det_precio, ";
            instruccionDatos += "det_importe = @det_importe, ";
            instruccionDatos += "det_preciounitario = @det_preciounitario, ";
            instruccionDatos += "det_importeunitario = @det_importeunitario, ";
            instruccionDatos += "det_ivas = @det_ivas, ";
            instruccionDatos += "det_ieps = @det_ieps, ";
            instruccionDatos += "det_retisr = @det_retisr, ";
            instruccionDatos += "det_retiva = @det_retiva, ";
            instruccionDatos += "det_retiep = @det_retiep ";
            instruccionDatos += "WHERE det_idtemp = @det_idtemp";

            MySqlCommand comandoDatos = new MySqlCommand(instruccionDatos);

            //parámetros
            comandoDatos.Parameters.Add("@det_idtemp", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@det_cantidad", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_descripcion", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_precio", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_importe", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_preciounitario", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_importeunitario", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_ivas", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_ieps", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_retisr", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_retiva", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_retiep", MySqlDbType.VarChar);

            //pasar atributos de la clase a parámetros
            comandoDatos.Parameters["@det_idtemp"].Value = IdTemp;
            comandoDatos.Parameters["@det_cantidad"].Value = Cantidad;
            comandoDatos.Parameters["@det_descripcion"].Value = Descripcion;
            comandoDatos.Parameters["@det_precio"].Value = Precio;
            comandoDatos.Parameters["@det_importe"].Value = Importe;
            comandoDatos.Parameters["@det_preciounitario"].Value = PrecioUnitario;
            comandoDatos.Parameters["@det_importeunitario"].Value = ImporteUnitario;
            comandoDatos.Parameters["@det_ivas"].Value = Ivas;
            comandoDatos.Parameters["@det_ieps"].Value = Ieps;
            comandoDatos.Parameters["@det_retisr"].Value = Retisr;
            comandoDatos.Parameters["@det_retiva"].Value = Retiva;
            comandoDatos.Parameters["@det_retiep"].Value = Retiep;

            //ejecutar comando
            return claseConexion.EjecutarComando(comandoDatos);
        }

        public bool BorrarConceptosCfdi()
        {
            string instruccion = "DELETE FROM conceptoscfdi";

            MySqlCommand comando = new MySqlCommand(instruccion);

            return claseConexion.EjecutarComando(comando);
        }
        public bool BorrarConceptosCfdi(int IdTemp)
        {
            string instruccion = "DELETE FROM conceptoscfdi WHERE det_idtemp = @det_idtemp";

            MySqlCommand comando = new MySqlCommand(instruccion);

            //parámetros
            comando.Parameters.Add("@det_idtemp", MySqlDbType.Int32);

            //pasar atributos de la clase a parámetros
            comando.Parameters["@det_idtemp"].Value = IdTemp;

            return claseConexion.EjecutarComando(comando);
        }
        public DataTable LeerConceptosCfdi()
        {
            string query = "SELECT * FROM conceptoscfdi";
            return claseConexion.LeerTabla(query);
        }
        public DataTable LeerConceptosCfdi(int IdTemp)
        {
            string query = "SELECT * ";
            query += "FROM conceptoscfdi ";
            query += "WHERE det_idtemp = " + IdTemp;
            return claseConexion.LeerTabla(query);
        }

        public int LeerSiguienteIdTemp()
        {
            string query = "SELECT MAX(det_idtemp) AS det_idtemp FROM conceptoscfdi";
            DataTable dtFolio = claseConexion.LeerTabla(query);

            int siguiente = 0;

            foreach (DataRow drRenglon in dtFolio.Rows)
            {
                try
                {
                    siguiente = Convert.ToInt32(drRenglon["det_idtemp"]);
                }
                catch (Exception)
                {
                }
            }

            return siguiente + 1;
        }

        //=============================================
        public bool InsertConceptosCfdiTraslados(
            int IdTraslado,
            int IdTemp,
            decimal Importe,
            string Tasa,
            string TasaoCuota,
            string TipoFactor,
            string Impuesto,
            string Descripcion,
            string BaseImporte,
            string TasaFactorImpuesto)
        {
            string instruccionDatos = "INSERT conceptoscfditraslados ";
            instruccionDatos += "(tra_id, ";
            instruccionDatos += "tra_idtemp, ";
            instruccionDatos += "tra_importe, ";
            instruccionDatos += "tra_tasa, ";
            instruccionDatos += "tra_tasaocuota, ";
            instruccionDatos += "tra_tipofactor, ";
            instruccionDatos += "tra_impuesto, ";
            instruccionDatos += "tra_descripcion, ";
            instruccionDatos += "tra_base, ";
            instruccionDatos += "tra_tasafactorimpuesto) ";
            instruccionDatos += "VALUES ";
            instruccionDatos += "(@tra_id, ";
            instruccionDatos += "@tra_idtemp, ";
            instruccionDatos += "@tra_importe, ";
            instruccionDatos += "@tra_tasa, ";
            instruccionDatos += "@tra_tasaocuota, ";
            instruccionDatos += "@tra_tipofactor, ";
            instruccionDatos += "@tra_impuesto, ";
            instruccionDatos += "@tra_descripcion, ";
            instruccionDatos += "@tra_base, ";
            instruccionDatos += "@tra_tasafactorimpuesto) ";

            MySqlCommand comandoDatos = new MySqlCommand(instruccionDatos);

            //parámetros
            comandoDatos.Parameters.Add("@tra_id", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@tra_idtemp", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@tra_importe", MySqlDbType.Decimal);
            comandoDatos.Parameters.Add("@tra_tasa", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@tra_tasaocuota", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@tra_tipofactor", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@tra_impuesto", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@tra_descripcion", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@tra_base", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@tra_tasafactorimpuesto", MySqlDbType.VarChar);

            //pasar atributos de la clase a parámetros
            comandoDatos.Parameters["@tra_id"].Value = IdTraslado;
            comandoDatos.Parameters["@tra_idtemp"].Value = IdTemp;
            comandoDatos.Parameters["@tra_importe"].Value = Importe;
            comandoDatos.Parameters["@tra_tasa"].Value = Tasa;
            comandoDatos.Parameters["@tra_tasaocuota"].Value = TasaoCuota;
            comandoDatos.Parameters["@tra_tipofactor"].Value = TipoFactor;
            comandoDatos.Parameters["@tra_impuesto"].Value = Impuesto;
            comandoDatos.Parameters["@tra_descripcion"].Value = Descripcion;
            comandoDatos.Parameters["@tra_base"].Value = BaseImporte;
            comandoDatos.Parameters["@tra_tasafactorimpuesto"].Value = TasaFactorImpuesto;

            //ejecutar comando
            return claseConexion.EjecutarComando(comandoDatos);
        }

        public bool UpdateConceptosCfdiTraslados(
            int IdTraslado,
            decimal Importe,
            string BaseImporte)
        {
            string instruccionDatos = "UPDATE conceptoscfditraslados SET ";
            instruccionDatos += "tra_importe = @tra_importe, ";
            instruccionDatos += "tra_base = @tra_base ";
            instruccionDatos += "WHERE tra_id = @tra_id";

            MySqlCommand comandoDatos = new MySqlCommand(instruccionDatos);

            //parámetros
            comandoDatos.Parameters.Add("@tra_id", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@tra_importe", MySqlDbType.Decimal);
            comandoDatos.Parameters.Add("@tra_base", MySqlDbType.VarChar);

            //pasar atributos de la clase a parámetros
            comandoDatos.Parameters["@tra_id"].Value = IdTraslado;
            comandoDatos.Parameters["@tra_importe"].Value = Importe;
            comandoDatos.Parameters["@tra_base"].Value = BaseImporte;

            //ejecutar comando
            return claseConexion.EjecutarComando(comandoDatos);
        }

        public bool BorrarConceptosCfdiTraslados()
        {
            string instruccion = "DELETE FROM conceptoscfditraslados";

            MySqlCommand comando = new MySqlCommand(instruccion);

            return claseConexion.EjecutarComando(comando);
        }
        public bool BorrarConceptosCfdiTraslados(int IdTemp)
        {
            string instruccion = "DELETE FROM conceptoscfditraslados ";
            instruccion += "WHERE tra_idtemp = @tra_idtemp";

            MySqlCommand comando = new MySqlCommand(instruccion);

            //agregar parámetros
            comando.Parameters.Add("@tra_idtemp", MySqlDbType.Int32);

            //dar valores a pararámetros
            comando.Parameters["@tra_idtemp"].Value = IdTemp;

            return claseConexion.EjecutarComando(comando);
        }
        public DataTable LeerConceptosCfdiTraslados()
        {
            string query = "SELECT * FROM conceptoscfditraslados";
            return claseConexion.LeerTabla(query);
        }
        public DataTable LeerConceptosCfdiTraslados(int IdTemp)
        {
            string query = "SELECT * ";
            query += "FROM conceptoscfditraslados ";
            query += "WHERE tra_idtemp = " + IdTemp;
            return claseConexion.LeerTabla(query);
        }
        public DataTable LeerConceptosCfdiTrasladosGlobal()
        {
            string query = "SELECT ";
            query += "SUM(tra_importe) AS tra_importe, ";
            query += "tra_tasaocuota, ";
            query += "tra_tipofactor, ";
            query += "tra_impuesto ";
            query += "FROM conceptoscfditraslados ";
            query += "GROUP BY tra_tasafactorimpuesto";
            return claseConexion.LeerTabla(query);
        }

        public int LeerSiguienteIdTras()
        {
            string query = "SELECT MAX(tra_id) AS tra_id FROM conceptoscfditraslados";
            DataTable dtFolio = claseConexion.LeerTabla(query);

            int siguiente = 0;

            foreach (DataRow drRenglon in dtFolio.Rows)
            {
                try
                {
                    siguiente = Convert.ToInt32(drRenglon["tra_id"]);
                }
                catch (Exception)
                {
                }
            }

            return siguiente + 1;
        }

        //=====================================================
        public bool InsertConceptosCfdiRetenciones(
            int IdRetencion,
            int IdTemp,
            decimal Importe,
            string Tasa,
            string TasaoCuota,
            string TipoFactor,
            string Impuesto,
            string Descripcion,
            string BaseImporte)
        {
            string instruccionDatos = "INSERT conceptoscfdiretenciones ";
            instruccionDatos += "(ret_id, ";
            instruccionDatos += "ret_idtemp, ";
            instruccionDatos += "ret_importe, ";
            instruccionDatos += "ret_tasa, ";
            instruccionDatos += "ret_tasaocuota, ";
            instruccionDatos += "ret_tipofactor, ";
            instruccionDatos += "ret_impuesto, ";
            instruccionDatos += "ret_descripcion, ";
            instruccionDatos += "ret_base) ";
            instruccionDatos += "VALUES ";
            instruccionDatos += "(@ret_id, ";
            instruccionDatos += "@ret_idtemp, ";
            instruccionDatos += "@ret_importe, ";
            instruccionDatos += "@ret_tasa, ";
            instruccionDatos += "@ret_tasaocuota, ";
            instruccionDatos += "@ret_tipofactor, ";
            instruccionDatos += "@ret_impuesto, ";
            instruccionDatos += "@ret_descripcion, ";
            instruccionDatos += "@ret_base)";

            MySqlCommand comandoDatos = new MySqlCommand(instruccionDatos);

            //parámetros
            comandoDatos.Parameters.Add("@ret_id", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@ret_idtemp", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@ret_importe", MySqlDbType.Decimal);
            comandoDatos.Parameters.Add("@ret_tasa", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@ret_tasaocuota", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@ret_tipofactor", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@ret_impuesto", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@ret_descripcion", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@ret_base", MySqlDbType.VarChar);

            //pasar atributos de la clase a parámetros
            comandoDatos.Parameters["@ret_id"].Value = IdRetencion;
            comandoDatos.Parameters["@ret_idtemp"].Value = IdTemp;
            comandoDatos.Parameters["@ret_importe"].Value = Importe;
            comandoDatos.Parameters["@ret_tasa"].Value = Tasa;
            comandoDatos.Parameters["@ret_tasaocuota"].Value = TasaoCuota;
            comandoDatos.Parameters["@ret_tipofactor"].Value = TipoFactor;
            comandoDatos.Parameters["@ret_impuesto"].Value = Impuesto;
            comandoDatos.Parameters["@ret_descripcion"].Value = Descripcion;
            comandoDatos.Parameters["@ret_base"].Value = BaseImporte;

            //ejecutar comando
            return claseConexion.EjecutarComando(comandoDatos);
        }

        public bool UpdateConceptosCfdiRetenciones(
            int IdRetencion,
            decimal Importe,
            string BaseImporte)
        {
            string instruccionDatos = "UPDATE conceptoscfdiretenciones SET ";
            instruccionDatos += "ret_importe = @ret_importe, ";
            instruccionDatos += "ret_base = @ret_base ";
            instruccionDatos += "WHERE ret_id = @ret_id";

            MySqlCommand comandoDatos = new MySqlCommand(instruccionDatos);

            //parámetros
            comandoDatos.Parameters.Add("@ret_id", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@ret_importe", MySqlDbType.Decimal);
            comandoDatos.Parameters.Add("@ret_base", MySqlDbType.VarChar);

            //pasar atributos de la clase a parámetros
            comandoDatos.Parameters["@ret_id"].Value = IdRetencion;
            comandoDatos.Parameters["@ret_importe"].Value = Importe;
            comandoDatos.Parameters["@ret_base"].Value = BaseImporte;

            //ejecutar comando
            return claseConexion.EjecutarComando(comandoDatos);
        }

        public bool BorrarConceptosCfdiRetenciones()
        {
            string instruccion = "DELETE FROM conceptoscfdiretenciones";

            MySqlCommand comando = new MySqlCommand(instruccion);

            return claseConexion.EjecutarComando(comando);
        }
        public bool BorrarConceptosCfdiRetenciones(int IdTemp)
        {
            string instruccion = "DELETE FROM conceptoscfdiretenciones ";
            instruccion += "WHERE ret_idtemp = @ret_idtemp";

            MySqlCommand comando = new MySqlCommand(instruccion);

            //agregar parámetros
            comando.Parameters.Add("@ret_idtemp", MySqlDbType.Int32);

            //dar valores a pararámetros
            comando.Parameters["@ret_idtemp"].Value = IdTemp;

            return claseConexion.EjecutarComando(comando);
        }
        public DataTable LeerConceptosCfdiRetenciones()
        {
            string query = "SELECT * FROM conceptoscfdiretenciones";
            return claseConexion.LeerTabla(query);
        }
        public DataTable LeerConceptosCfdiRetenciones(int IdTemp)
        {
            string query = "SELECT * ";
            query += "FROM conceptoscfdiretenciones ";
            query += "WHERE ret_idtemp = " + IdTemp;
            return claseConexion.LeerTabla(query);
        }
        public DataTable LeerConceptosCfdiRetencionesGlobal()
        {
            string query = "SELECT ";
            query += "SUM(ret_importe) AS ret_importe, ";
            query += "ret_impuesto ";
            query += "FROM conceptoscfdiretenciones ";
            query += "GROUP BY ret_impuesto";
            return claseConexion.LeerTabla(query);
        }

        public int LeerSiguienteIdRet()
        {
            string query = "SELECT MAX(ret_id) AS ret_id FROM conceptoscfdiretenciones";
            DataTable dtFolio = claseConexion.LeerTabla(query);

            int siguiente = 0;

            foreach (DataRow drRenglon in dtFolio.Rows)
            {
                try
                {
                    siguiente = Convert.ToInt32(drRenglon["ret_id"]);
                }
                catch (Exception)
                {
                }
            }

            return siguiente + 1;
        }
*/
        #endregion


        #region Generar CFDIs

        private string GenerarSello()
        {
            //if (File.Exists(ClassEmpresaLocal._LlavePrivada))
            //{

            //}
            //else
            //{
            //    throw new Exception("Error al generar Sello, el archivo llave no existe: " + ClassEmpresaLocal._LlavePrivada);
            //}

            //obtener cadena original
            string CadenaOriginal = GetCadenaOriginal();

            //Cadena original a UTF8
            byte[] CadenaOriginal_UTF8 = Encoding.UTF8.GetBytes(CadenaOriginal);

            //Leer Archivo .key
            //string test = @"/Certificados/" + EmpresaCliente.CertificadoKey;
            string test = HttpContext.Current.Server.MapPath("/Certificados/" + EmpresaCliente.CertificadoKey);
            //byte[] dataKey = File.ReadAllBytes(@"/Certificados/" + EmpresaCliente.CertificadoKey);  //ClassEmpresa._LlavePrivada); 
            byte[] dataKey = File.ReadAllBytes(HttpContext.Current.Server.MapPath("/Certificados/" + EmpresaCliente.CertificadoKey));  //ClassEmpresa._LlavePrivada); 



            //Desencriptar Archivo .key
            //            Org.BouncyCastle.Crypto.AsymmetricKeyParameter KeyDesencriptado = Org.BouncyCastle.Security.PrivateKeyFactory.DecryptKey(ClassEmpresa._ClaveLlavePrivada.ToCharArray(), dataKey);
            Org.BouncyCastle.Crypto.AsymmetricKeyParameter KeyDesencriptado = Org.BouncyCastle.Security.PrivateKeyFactory.DecryptKey(EmpresaCliente.PassKey.ToCharArray(), dataKey);

            //MemoryStream ms = new MemoryStream();
            //TextWriter writer = new StreamWriter(ms);

            System.IO.StringWriter stWrite = new System.IO.StringWriter();

            Org.BouncyCastle.OpenSsl.PemWriter pmw = new PemWriter(stWrite);
            pmw.WriteObject(KeyDesencriptado);
            stWrite.Close();

            ISigner sig = SignerUtilities.GetSigner("SHA256withRSA");

            //SELLAR
            sig.Init(true, KeyDesencriptado);
            sig.BlockUpdate(CadenaOriginal_UTF8, 0, CadenaOriginal_UTF8.Length);
            byte[] signature = sig.GenerateSignature();

            return Convert.ToBase64String(signature);
        }
        private string GetCadenaOriginal()
        {
            StringWriter output = new StringWriter();

            XmlDocument xmldoc = new XmlDocument();

            //xmldoc.LoadXml(m_xmlDOM.InnerXml);
            xmldoc.LoadXml(_xmlPorTimbrar);

            XPathNavigator navigator = xmldoc.CreateNavigator();
            
            //string esquema = Application.StartupPath + @"\XSLT\zzcadenaoriginal_3_3_temp.xslt"; // Importante Tener Archivo en el proyecto y Actualizarlo
            //string esquema =  HttpContext.Current.Server.MapPath("/XSLT/zzcadenaoriginal_3_3_temp.xslt"); // Importante Tener Archivo en el proyecto y Actualizarlo
            string esquema = HttpContext.Current.Server.MapPath("/XSLT/cadenaoriginal.xslt"); // Importante Tener Archivo en el proyecto y Actualizarlo
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(esquema);
            xslt.Transform(navigator, null, output);
            return output.ToString();
        }

        //Formatea el aspecto de los nodos para que sea mas comoda su lectura.
        private void IndentarNodo(XmlNode Nodo)
        {
            //Nodo.AppendChild(m_xmlDOM.CreateTextNode(Environment.NewLine));
        }

        private void extraerTimbre(string rutaXML)
        {
            XmlTextReader FlujoReader = new XmlTextReader(rutaXML);
            FlujoReader.WhitespaceHandling = WhitespaceHandling.None;

            string timbreVersion = string.Empty;
            string rfcPac = string.Empty;

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
                                case "tfd:TimbreFiscalDigital":
                                    {
                                        for (int i = 0; i <= FlujoReader.AttributeCount - 1; i++)
                                        {
                                            FlujoReader.MoveToAttribute(i);

                                            //=====NOMBRE ATRIBUTO 
                                            switch (FlujoReader.Name)
                                            {
                                                case "Version":
                                                    {
                                                        timbreVersion = FlujoReader.Value;
                                                    }break;
                                                case "UUID":
                                                    {
                                                        _uuid = FlujoReader.Value;
                                                    }break;
                                                case "FechaTimbrado":
                                                    {
                                                        _fechaTimbrado = Convert.ToDateTime(FlujoReader.Value);
                                                    }break;
                                                case "RfcProvCertif":
                                                    {
                                                        rfcPac = FlujoReader.Value;
                                                    } break;
                                                case "NoCertificadoSAT":
                                                    {
                                                        _CertificadoSAT = FlujoReader.Value;
                                                    }break;
                                                case "SelloSAT":
                                                    {
                                                        _selloSAT = FlujoReader.Value;
                                                    }break;
                                                case "SelloCFD":
                                                    {
                                                        _selloCFDI = FlujoReader.Value;
                                                    }break;
                                            }
                                        }
                                    }break;
                            }
                        }
                        break;
                }
            }

            //liberar
            FlujoReader.Close();


            //concatenar cadena original del timbre
            _cadenaOriginal = "||" + timbreVersion + "|" + _uuid + "|" + _fechaTimbrado.ToString("s") + "|" + rfcPac + "|" + _selloCFDI + "|" + _CertificadoSAT + "||";
        }
        
        public string QuitarCharInvalidos(string dato)
        {
            //string CharAnterior = string.Empty;
            string CharActual = string.Empty;
            string NewDato = string.Empty;
            dato = dato.Trim();

            for (int i = 0; i < dato.Length; i++)
            {
                CharActual = dato.Substring(i, 1);

                switch (CharActual)
                {
                    case "&":
                        {
                            CharActual = "&amp;";
                        }
                        break;

                    case "\"":
                        {
                            CharActual = "&quot;";
                        }
                        break;
                    case "<":
                        {
                            CharActual = "&lt;";
                        }
                        break;
                    case ">":
                        {
                            CharActual = "&gt;";
                        }
                        break;
                    case "'":
                        {
                            CharActual = "&apos;";
                        }
                        break;
                    default:
                        {
                            char caracter = Convert.ToChar(CharActual);
                            int ascii = caracter;

                            if (ascii == 127)
                            {
                                CharActual = "";
                            }
                            else
                            {
                                if (ascii >= 0 && ascii <= 31)
                                {
                                    CharActual = "";
                                }
                            }
                        }
                        break;
                }

                NewDato += CharActual;
            }

            return NewDato;
        }
        public bool SoloNumerosYLetras(string Campo)
        {
            bool success = false, existenumero = false, existeletra = false;

            int[] Numeros = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, };
            string[] Letras = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "Ñ", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "@", "&" };

            //primera letra del campo
            for (int i = 0; i < Campo.Length; i++)
            {
                //es número?
                for (int j = 0; j < Numeros.Length; j++)
                {
                    if (Campo[i].ToString() == Numeros[j].ToString())
                    {
                        existenumero = true;
                        break;
                    }
                }

                //es letra?
                if (!existenumero)
                {
                    //es letra?
                    for (int k = 0; k < Letras.Length; k++)
                    {
                        string cam = Campo[i].ToString();
                        string let = Letras[k].ToString();

                        if (Campo[i].ToString() == Letras[k].ToString())
                        {
                            existeletra = true;
                            break;
                        }
                    }
                }

                //si no es número y tampoco es letra, oborto todo
                if (!existenumero & !existeletra)
                {
                    success = true;
                    break;
                }
            }

            return success;
        }



        /*
        public DataTable LeerReporteCFDI(int Cancelado)
        {
            string query = "SELECT ";
            query += "cfd_folio, ";
            query += "cfd_fechatimbrado, ";
            query += "cfd_nombrecliente, ";
            query += "cfd_total, ";
            query += "cfd_cancelado, ";
            query += "cfd_fechacancelacion ";
            query += "FROM cfdis ";
            query += "WHERE cfd_cancelado = " + Cancelado;
            query += " ORDER BY cfd_fechatimbrado ASC";

            return claseConexion.LeerTabla(query);
        }

        public DataTable LeerCFDIs(int IdCFDI)
        {
            string query = "SELECT * FROM cfdis WHERE cfd_id = " + IdCFDI;
            return claseConexion.LeerTabla(query);
        }
        public DataTable LeerCFDIs(string Letra, int Cancelado, bool Limite)
        {
            string query = "SELECT * ";
            query += "FROM cfdis ";
            query += "WHERE (cfd_folio LIKE '" + Letra + "%' OR cfd_razonsocialcliente LIKE '" + Letra + "%') ";
            query += "AND cfd_cancelado = " + Cancelado;
            query += " ORDER BY cfd_id DESC ";

            if (Limite)
            {
                query += "LIMIT 5";
            }

            return claseConexion.LeerTabla(query);
        }
        public DataTable LeerCFDIs(int Mes, int IdCliente, string Moneda, int Cancelado)
        {
            string query = "SELECT * ";
            query += "FROM cfdis ";
            query += "WHERE cfd_cancelado = " + Cancelado;
            query += " AND YEAR(cfd_fechaemision) = " + DateTime.Now.Year;

            if (Mes > 0)
            {
                query += " AND MONTH(cfd_fechaemision) = " + Mes;
            }

            if (IdCliente > 0)
            {
                query += " AND cfd_idcliente = " + IdCliente;
            }

            if (Moneda != "todo")
            {
                query += " AND cfd_moneda = '" + Moneda + "' ";
            }

            query += " ORDER BY cfd_id DESC";

            return claseConexion.LeerTabla(query);
        }
        public DataTable LeerCFDIs(int IdCliente, DateTime FechaInicio, DateTime FechaFin, string Moneda, int Cancelado)
        {
            string query = "SELECT * ";
            query += "FROM cfdis ";
            query += "WHERE cfd_cancelado = " + Cancelado;
            query += " AND DATE(cfd_fechaemision) >= '" + FechaInicio.ToString("yyyy-MM-dd") + "' ";
            query += "AND DATE(cfd_fechaemision) <= '" + FechaFin.ToString("yyyy-MM-dd") + "' ";

            if (IdCliente > 0)
            {
                query += " AND cfd_idcliente = " + IdCliente;
            }

            if (Moneda != "todo")
            {
                query += " AND cfd_moneda = '" + Moneda + "' ";
            }

            query += " ORDER BY cfd_id DESC";

            return claseConexion.LeerTabla(query);
        }
        public DataTable LeerCFDIs(DateTime FechaDesde, DateTime FechaHasta, int Cancelado)
        {
            string query = "SELECT * ";
            query += "FROM cfdis ";
            query += "WHERE DATE(cfd_fechatimbrado) >= '" + FechaDesde.ToString("yyyy-MM-dd") + "' ";
            query += "AND DATE(cfd_fechatimbrado) <= '" + FechaHasta.ToString("yyyy-MM-dd") + "' ";
            query += "AND cfd_cancelado = " + Cancelado;
            query += " ORDER BY cfd_id ASC";

            return claseConexion.LeerTabla(query);
        }


        public int UltimoCfdiIngresado()
        {
            string query = "SELECT MAX(cfd_id) AS cfd_id FROM cfdis";
            DataTable dtFolio = claseConexion.LeerTabla(query);

            int Ultimo = 0;

            foreach (DataRow drRenglon in dtFolio.Rows)
            {
                try
                {
                    Ultimo = Convert.ToInt32(drRenglon["cfd_id"]);
                }
                catch (Exception)
                {
                }
            }

            return Ultimo;
        }
        public string LeerSiguienteFactura()
        {
            string query = "SELECT emp_siguientefactura FROM empresa";
            DataTable dtFolio = claseConexion.LeerTabla(query);

            int Folio = 1;

            foreach (DataRow drRenglon in dtFolio.Rows)
            {
                try
                {
                    Folio = Convert.ToInt32(drRenglon["emp_siguientefactura"]);
                }
                catch (Exception)
                {
                }
            }

            return Folio.ToString();
        }
        public bool UpdateSiguienteFactura()
        {
            string instrucciones = "UPDATE empresa SET emp_siguientefactura = emp_siguientefactura + 1 WHERE emp_id = 1";
            MySqlCommand comandoDatos = new MySqlCommand(instrucciones);

            //ejecutar comando
            return claseConexion.EjecutarComando(comandoDatos);
        }

        public decimal LeerSaldoCfdi(int IdCfdi)
        {
            decimal saldo = 0m;

            string query = "SELECT ";
            query += "cfd_saldo ";
            query += "FROM cfdis ";
            query += "WHERE cfd_id = " + IdCfdi;

            DataTable dtSaldo = claseConexion.LeerTabla(query);
            DataRow drSaldo = dtSaldo.Rows[0];

            try
            {
                saldo = Convert.ToDecimal(drSaldo["cfd_saldo"]);
            }
            catch (Exception)
            {
            }

            return saldo;
        }
        public bool UpdateSaldoCfdi(int IdCfdi, decimal Cantidad)
        {
            string instruccionDatos = "UPDATE cfdis SET ";
            instruccionDatos += "cfd_saldo = cfd_saldo + @cantidad ";
            instruccionDatos += "WHERE cfd_id = @cfd_id";


            MySqlCommand comandoDatos = new MySqlCommand(instruccionDatos);

            //parámetros
            comandoDatos.Parameters.Add("@cfd_id", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@cantidad", MySqlDbType.Decimal);

            //pasar atributos de la clase a parámetros
            comandoDatos.Parameters["@cfd_id"].Value = IdCfdi;
            comandoDatos.Parameters["@cantidad"].Value = Cantidad;

            //ejecutar comando
            return claseConexion.EjecutarComando(comandoDatos);
        }

        public bool UpdateAcuseXml(int IdCfdi, string FechaCancelacion, string SelloCancelacion, string Motivo, string AcuseXML)
        {
            string instruccion = "UPDATE cfdis SET ";
            instruccion += "cfd_cancelado = 1, ";
            instruccion += "cfd_fechacancelacion = @cfd_fechacancelacion, ";
            instruccion += "cfd_selloacuse = @cfd_selloacuse, ";
            instruccion += "cfd_motivocancelacion = @cfd_motivocancelacion, ";
            instruccion += "cfd_xmlacuse = @cfd_xmlacuse ";
            instruccion += "WHERE cfd_id = @cfd_id ";

            MySqlCommand Comando = new MySqlCommand(instruccion);

            //agregar parámetros
            Comando.Parameters.Add("@cfd_id", MySqlDbType.Int32);
            Comando.Parameters.Add("@cfd_fechacancelacion", MySqlDbType.VarChar);
            Comando.Parameters.Add("@cfd_selloacuse", MySqlDbType.Text);
            Comando.Parameters.Add("@cfd_motivocancelacion", MySqlDbType.VarChar);
            Comando.Parameters.Add("@cfd_xmlacuse", MySqlDbType.Text);

            //dar valores a pararámetros
            Comando.Parameters["@cfd_id"].Value = IdCfdi;
            Comando.Parameters["@cfd_fechacancelacion"].Value = FechaCancelacion;
            Comando.Parameters["@cfd_selloacuse"].Value = SelloCancelacion;
            Comando.Parameters["@cfd_motivocancelacion"].Value = Motivo;
            Comando.Parameters["@cfd_xmlacuse"].Value = AcuseXML;

            return claseConexion.EjecutarComando(Comando);
        }
        public bool UpdateComentarios(int IdCfdi, string Comentarios)
        {
            string instruccion = "UPDATE cfdis SET ";
            instruccion += "cfd_comentarios = @cfd_comentarios ";
            instruccion += "WHERE cfd_id = @cfd_id";

            MySqlCommand Comando = new MySqlCommand(instruccion);

            //agregar parámetros
            Comando.Parameters.Add("@cfd_id", MySqlDbType.Int32);
            Comando.Parameters.Add("@cfd_comentarios", MySqlDbType.VarChar);

            //dar valores a pararámetros
            Comando.Parameters["@cfd_id"].Value = IdCfdi;
            Comando.Parameters["@cfd_comentarios"].Value = Comentarios;

            return claseConexion.EjecutarComando(Comando);
        }
        */

        #endregion



        #region CFDI MASTER
        /*
        public bool InsertCFDI_Master()
        {
            //info crédito
            int credito = 0;
            decimal saldo = 0.00m;
            if (claseTemporal.MetodoDePagoClave != "PUE")
            {
                credito = 1;
                saldo = claseTemporal.Total;
            }


            return InsertCfdi(
                claseTemporal.IdTicket,
                _VersionCFDI,
                _SerieCFDI,
                claseTemporal.SiguienteFolio,

                credito,
                ClassCliente._DiasCredito,
                claseTemporal.FechaPago,
                saldo,

                ClassEmpresa._NombreEmpresa,
                ClassEmpresa._RazonSocial,
                ClassEmpresa._RFC,
                ClassEmpresa._Telefonos,
                ClassEmpresa._EmailEmpresa,
                ClassEmpresa._LeyendaRegimenFiscal,
                ClassEmpresa._ClaveRegimenFiscal,

                claseTemporal.IdCliente,
                ClassClienteDireccion._RazonSocial,
                ClassClienteDireccion._Rfc,

                claseTemporal.Subtotal.ToString(),
                claseTemporal.IvaTotal.ToString(),
                claseTemporal.IepsTotal.ToString(),
                claseTemporal.RetencionIVA.ToString(),
                claseTemporal.RetencionISR.ToString(),
                claseTemporal.RetencionIEP.ToString(),
                claseTemporal.Total.ToString(),

                claseTemporal.FormaDePago,
                claseTemporal.FormaDePagoClave,
                claseTemporal.MetodoDePago,
                claseTemporal.MetodoDePagoClave,
                claseTemporal.UsoCfdi,
                claseTemporal.UsoCfdiClave,
                claseTemporal.Moneda,
                claseTemporal.MonedaClave,
                claseTemporal.TipoDeCambio,
                claseTemporal.ReferenciaDeCuenta,
                claseTemporal.Condiciones,

                _uuid,
                _fechaEmision,
                _fechaTimbrado,
                _CertificadoSAT,
                ClassEmpresa._SerieCertificado,
                _selloCFDI,
                _selloSAT,
                _cadenaOriginal,
                _xmlTimbrado);
        }




        public bool InsertCfdi(
            int idticket,
            string versioncfd,
            string serie,
            string folio,

            int credito,
            int diascredito,
            DateTime fechaparapago,
            decimal saldo,

            string nombreempresa,
            string razonsocialempresa,
            string rfcempresa,
            string telefonoempresa,
            string emailempresa,
            string regimenfiscalempresa,
            string regimenfiscalempresaclave,

            int idcliente,
            string razonsocialcliente,
            string rfccliente,

            string subtotal,
            string totaliva,
            string totalieps,
            string retencioniva,
            string retecionisr,
            string retecioniep,
            string total,

            string formadepago,
            string formadepagoclave,
            string metododepago,
            string metododepagoclave,
            string usocfdi,
            string usocfdiclave,
            string moneda,
            string monedaclave,
            string tipocambio,
            string referenciacuenta,
            string condiciones,

            string uuidnumero,
            DateTime fechaemision,
            DateTime fechatimbrado,
            string certificadosat,
            string certificadoemisor,
            string sellocfdi,
            string sellosat,
            string cadenaoriginal,
            string xmlcfdi)
        {
            string Instrucciones = "INSERT cfdis ";
            Instrucciones += "(cfd_idticket, ";
            Instrucciones += "cfd_version, ";
            Instrucciones += "cfd_serie, ";
            Instrucciones += "cfd_folio, ";

            Instrucciones += "cfd_credito, ";
            Instrucciones += "cfd_diascredito, ";
            Instrucciones += "cfd_fechaparapago, ";
            Instrucciones += "cfd_saldo, ";

            Instrucciones += "cfd_nombreempresa, ";
            Instrucciones += "cfd_razonsocialempresa, ";
            Instrucciones += "cfd_rfcempresa, ";
            Instrucciones += "cfd_telefonoempresa, ";
            Instrucciones += "cfd_emailempresa, ";
            Instrucciones += "cfd_regimenfiscalempresa, ";
            Instrucciones += "cfd_regimenfiscalempresaclave, ";

            Instrucciones += "cfd_idcliente, ";
            Instrucciones += "cfd_razonsocialcliente, ";
            Instrucciones += "cfd_rfccliente, ";

            Instrucciones += "cfd_subtotal, ";
            Instrucciones += "cfd_totaliva, ";
            Instrucciones += "cfd_totalieps, ";
            Instrucciones += "cfd_retencioniva, ";
            Instrucciones += "cfd_retecionisr, ";
            Instrucciones += "cfd_retecioniep, ";
            Instrucciones += "cfd_total, ";

            Instrucciones += "cfd_formadepago, ";
            Instrucciones += "cfd_formadepagoclave, ";
            Instrucciones += "cfd_metododepago, ";
            Instrucciones += "cfd_metododepagoclave, ";
            Instrucciones += "cfd_usocfdi, ";
            Instrucciones += "cfd_usocfdiclave, ";
            Instrucciones += "cfd_moneda, ";
            Instrucciones += "cfd_monedaclave, ";
            Instrucciones += "cfd_tipocambio, ";
            Instrucciones += "cfd_referenciacuenta, ";
            Instrucciones += "cfd_condiciones, ";

            Instrucciones += "cfd_uuidnumero, ";
            Instrucciones += "cfd_fechaemision, ";
            Instrucciones += "cfd_fechatimbrado, ";
            Instrucciones += "cfd_certificadosat, ";
            Instrucciones += "cfd_certificadoemisor, ";
            Instrucciones += "cfd_sellocfdi, ";
            Instrucciones += "cfd_sellosat, ";
            Instrucciones += "cfd_cadenaoriginal, ";
            Instrucciones += "cfd_xmlcfdi) ";

            Instrucciones += "VALUES ";

            Instrucciones += "(@cfd_idticket, ";
            Instrucciones += "@cfd_version, ";
            Instrucciones += "@cfd_serie, ";
            Instrucciones += "@cfd_folio, ";

            Instrucciones += "@cfd_credito, ";
            Instrucciones += "@cfd_diascredito, ";
            Instrucciones += "@cfd_fechaparapago, ";
            Instrucciones += "@cfd_saldo, ";

            Instrucciones += "@cfd_nombreempresa, ";
            Instrucciones += "@cfd_razonsocialempresa, ";
            Instrucciones += "@cfd_rfcempresa, ";
            Instrucciones += "@cfd_telefonoempresa, ";
            Instrucciones += "@cfd_emailempresa, ";
            Instrucciones += "@cfd_regimenfiscalempresa, ";
            Instrucciones += "@cfd_regimenfiscalempresaclave, ";

            Instrucciones += "@cfd_idcliente, ";
            Instrucciones += "@cfd_razonsocialcliente, ";
            Instrucciones += "@cfd_rfccliente, ";

            Instrucciones += "@cfd_subtotal, ";
            Instrucciones += "@cfd_totaliva, ";
            Instrucciones += "@cfd_totalieps, ";
            Instrucciones += "@cfd_retencioniva, ";
            Instrucciones += "@cfd_retecionisr, ";
            Instrucciones += "@cfd_retecioniep, ";
            Instrucciones += "@cfd_total, ";

            Instrucciones += "@cfd_formadepago, ";
            Instrucciones += "@cfd_formadepagoclave, ";
            Instrucciones += "@cfd_metododepago, ";
            Instrucciones += "@cfd_metododepagoclave, ";
            Instrucciones += "@cfd_usocfdi, ";
            Instrucciones += "@cfd_usocfdiclave, ";
            Instrucciones += "@cfd_moneda, ";
            Instrucciones += "@cfd_monedaclave, ";
            Instrucciones += "@cfd_tipocambio, ";
            Instrucciones += "@cfd_referenciacuenta, ";
            Instrucciones += "@cfd_condiciones, ";

            Instrucciones += "@cfd_uuidnumero, ";
            Instrucciones += "@cfd_fechaemision, ";
            Instrucciones += "@cfd_fechatimbrado, ";
            Instrucciones += "@cfd_certificadosat, ";
            Instrucciones += "@cfd_certificadoemisor, ";
            Instrucciones += "@cfd_sellocfdi, ";
            Instrucciones += "@cfd_sellosat, ";
            Instrucciones += "@cfd_cadenaoriginal, ";
            Instrucciones += "@cfd_xmlcfdi)";


            MySqlCommand comandoDatos = new MySqlCommand(Instrucciones);

            //parámetros
            comandoDatos.Parameters.Add("@cfd_idticket", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@cfd_version", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_serie", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_folio", MySqlDbType.VarChar);

            comandoDatos.Parameters.Add("@cfd_credito", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@cfd_diascredito", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@cfd_fechaparapago", MySqlDbType.DateTime);
            comandoDatos.Parameters.Add("@cfd_saldo", MySqlDbType.Decimal);

            comandoDatos.Parameters.Add("@cfd_nombreempresa", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_razonsocialempresa", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_rfcempresa", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_telefonoempresa", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_emailempresa", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_regimenfiscalempresa", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_regimenfiscalempresaclave", MySqlDbType.VarChar);

            comandoDatos.Parameters.Add("@cfd_idcliente", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@cfd_razonsocialcliente", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_rfccliente", MySqlDbType.VarChar);

            comandoDatos.Parameters.Add("@cfd_subtotal", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_totaliva", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_totalieps", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_retencioniva", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_retecionisr", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_retecioniep", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_total", MySqlDbType.VarChar);

            comandoDatos.Parameters.Add("@cfd_formadepago", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_formadepagoclave", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_metododepago", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_metododepagoclave", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_usocfdi", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_usocfdiclave", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_moneda", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_monedaclave", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_tipocambio", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_referenciacuenta", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_condiciones", MySqlDbType.VarChar);

            comandoDatos.Parameters.Add("@cfd_uuidnumero", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_fechaemision", MySqlDbType.DateTime);
            comandoDatos.Parameters.Add("@cfd_fechatimbrado", MySqlDbType.DateTime);
            comandoDatos.Parameters.Add("@cfd_certificadosat", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_certificadoemisor", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_sellocfdi", MySqlDbType.Text);
            comandoDatos.Parameters.Add("@cfd_sellosat", MySqlDbType.Text);
            comandoDatos.Parameters.Add("@cfd_cadenaoriginal", MySqlDbType.Text);
            comandoDatos.Parameters.Add("@cfd_xmlcfdi", MySqlDbType.Text);

            //pasar atributos de la clase a parámetros
            comandoDatos.Parameters["@cfd_idticket"].Value = idticket;
            comandoDatos.Parameters["@cfd_version"].Value = versioncfd;
            comandoDatos.Parameters["@cfd_serie"].Value = serie;
            comandoDatos.Parameters["@cfd_folio"].Value = folio;

            comandoDatos.Parameters["@cfd_credito"].Value = credito;
            comandoDatos.Parameters["@cfd_diascredito"].Value = diascredito;
            comandoDatos.Parameters["@cfd_fechaparapago"].Value = fechaparapago;
            comandoDatos.Parameters["@cfd_saldo"].Value = saldo;

            comandoDatos.Parameters["@cfd_nombreempresa"].Value = nombreempresa;
            comandoDatos.Parameters["@cfd_razonsocialempresa"].Value = razonsocialempresa;
            comandoDatos.Parameters["@cfd_rfcempresa"].Value = rfcempresa;
            comandoDatos.Parameters["@cfd_telefonoempresa"].Value = telefonoempresa;
            comandoDatos.Parameters["@cfd_emailempresa"].Value = emailempresa;
            comandoDatos.Parameters["@cfd_regimenfiscalempresa"].Value = regimenfiscalempresa;
            comandoDatos.Parameters["@cfd_regimenfiscalempresaclave"].Value = regimenfiscalempresaclave;

            comandoDatos.Parameters["@cfd_idcliente"].Value = idcliente;
            comandoDatos.Parameters["@cfd_razonsocialcliente"].Value = razonsocialcliente;
            comandoDatos.Parameters["@cfd_rfccliente"].Value = rfccliente;

            comandoDatos.Parameters["@cfd_subtotal"].Value = subtotal;
            comandoDatos.Parameters["@cfd_totaliva"].Value = totaliva;
            comandoDatos.Parameters["@cfd_totalieps"].Value = totalieps;
            comandoDatos.Parameters["@cfd_retencioniva"].Value = retencioniva;
            comandoDatos.Parameters["@cfd_retecionisr"].Value = retecionisr;
            comandoDatos.Parameters["@cfd_retecioniep"].Value = retecioniep;
            comandoDatos.Parameters["@cfd_total"].Value = total;

            comandoDatos.Parameters["@cfd_formadepago"].Value = formadepago;
            comandoDatos.Parameters["@cfd_formadepagoclave"].Value = formadepagoclave;
            comandoDatos.Parameters["@cfd_metododepago"].Value = metododepago;
            comandoDatos.Parameters["@cfd_metododepagoclave"].Value = metododepagoclave;
            comandoDatos.Parameters["@cfd_usocfdi"].Value = usocfdi;
            comandoDatos.Parameters["@cfd_usocfdiclave"].Value = usocfdiclave;
            comandoDatos.Parameters["@cfd_moneda"].Value = moneda;
            comandoDatos.Parameters["@cfd_monedaclave"].Value = monedaclave;
            comandoDatos.Parameters["@cfd_tipocambio"].Value = tipocambio;
            comandoDatos.Parameters["@cfd_referenciacuenta"].Value = referenciacuenta;
            comandoDatos.Parameters["@cfd_condiciones"].Value = condiciones;

            comandoDatos.Parameters["@cfd_uuidnumero"].Value = uuidnumero;
            comandoDatos.Parameters["@cfd_fechaemision"].Value = fechaemision;
            comandoDatos.Parameters["@cfd_fechatimbrado"].Value = fechatimbrado;
            comandoDatos.Parameters["@cfd_certificadosat"].Value = _CertificadoSAT;
            comandoDatos.Parameters["@cfd_certificadoemisor"].Value = certificadoemisor;
            comandoDatos.Parameters["@cfd_sellocfdi"].Value = sellocfdi;
            comandoDatos.Parameters["@cfd_sellosat"].Value = sellosat;
            comandoDatos.Parameters["@cfd_cadenaoriginal"].Value = cadenaoriginal;
            comandoDatos.Parameters["@cfd_xmlcfdi"].Value = xmlcfdi;

            //ejecutar comando
            return claseConexion.EjecutarComando(comandoDatos);
        }
        */
        #endregion

        #region CFDI DETALLE     BASES DE DATOS
        /*
        public void InsertCFDI_Detalle()
        {
            int idcfdi = UltimoCfdiIngresado();

            foreach (DataRow drDetalle in _dtConceptosCfdi.Rows)
            {
                int idtemp = Convert.ToInt32(drDetalle["det_idtemp"]);
                string cantidad = drDetalle["det_cantidad"].ToString();
                string codigo = drDetalle["det_codigo"].ToString();
                string descripcion = drDetalle["det_descripcion"].ToString();
                string claveprodser = drDetalle["det_claveprodser"].ToString();
                string claveum = drDetalle["det_claveum"].ToString();
                string unidad = drDetalle["det_unidaddemedida"].ToString();
                string precio = drDetalle["det_precio"].ToString();
                string importe = drDetalle["det_importe"].ToString();
                string preciounitario = drDetalle["det_preciounitario"].ToString();
                string importeunitario = drDetalle["det_importeunitario"].ToString();
                int idarticulo = Convert.ToInt32(drDetalle["det_idarticulo"]);
                int idpaquete = Convert.ToInt32(drDetalle["det_idpaquetereceta"]);
                int espaquete = Convert.ToInt32(drDetalle["det_espaquete"]);
                string ivas = drDetalle["det_ivas"].ToString();
                string ieps = drDetalle["det_ieps"].ToString();
                string retisr = drDetalle["det_retisr"].ToString();
                string retiva = drDetalle["det_retiva"].ToString();
                string retiep = drDetalle["det_retiep"].ToString();

                InsertDetalleCFDI(
                    idcfdi,
                    idtemp,
                    cantidad,
                    codigo,
                    descripcion,
                    claveprodser,
                    claveum,
                    unidad,
                    precio,
                    importe,
                    preciounitario,
                    importeunitario,
                    idarticulo,
                    idpaquete,
                    espaquete,
                    ivas,
                    ieps,
                    retisr,
                    retiva,
                    retiep);
            }
        }

        public bool InsertDetalleCFDI(
            int IdCfdi,
            int Idtemp,
            string Cantidad,
            string Codigo,
            string Descripcion,
            string ClaveProdSer,
            string ClaveUm,
            string UnidadDeMedida,
            string Precio,
            string Importe,
            string PrecioUnitario,
            string ImporteUnitario,
            int IdArticulo,
            int IdPaquete,
            int EsPaquete,
            string Ivas,
            string Ieps,
            string Retisr,
            string Retiva,
            string Retiep)
        {
            string instruccionDatos = "INSERT cfdisdetalle ";
            instruccionDatos += "(det_idcfdi, ";
            instruccionDatos += "det_idtemp, ";
            instruccionDatos += "det_cantidad, ";
            instruccionDatos += "det_codigo, ";
            instruccionDatos += "det_descripcion, ";
            instruccionDatos += "det_claveprodser, ";
            instruccionDatos += "det_claveum, ";
            instruccionDatos += "det_unidaddemedida, ";
            instruccionDatos += "det_precio, ";
            instruccionDatos += "det_importe, ";
            instruccionDatos += "det_preciounitario, ";
            instruccionDatos += "det_importeunitario, ";
            instruccionDatos += "det_idarticulo, ";
            instruccionDatos += "det_idpaquetereceta, ";
            instruccionDatos += "det_espaquete, ";
            instruccionDatos += "det_ivas, ";
            instruccionDatos += "det_ieps, ";
            instruccionDatos += "det_retisr, ";
            instruccionDatos += "det_retiva, ";
            instruccionDatos += "det_retiep) ";
            instruccionDatos += "VALUES ";
            instruccionDatos += "(@det_idcfdi, ";
            instruccionDatos += "@det_idtemp, ";
            instruccionDatos += "@det_cantidad, ";
            instruccionDatos += "@det_codigo, ";
            instruccionDatos += "@det_descripcion, ";
            instruccionDatos += "@det_claveprodser, ";
            instruccionDatos += "@det_claveum, ";
            instruccionDatos += "@det_unidaddemedida, ";
            instruccionDatos += "@det_precio, ";
            instruccionDatos += "@det_importe, ";
            instruccionDatos += "@det_preciounitario, ";
            instruccionDatos += "@det_importeunitario, ";
            instruccionDatos += "@det_idarticulo, ";
            instruccionDatos += "@det_idpaquetereceta, ";
            instruccionDatos += "@det_espaquete, ";
            instruccionDatos += "@det_ivas, ";
            instruccionDatos += "@det_ieps, ";
            instruccionDatos += "@det_retisr, ";
            instruccionDatos += "@det_retiva, ";
            instruccionDatos += "@det_retiep)";

            MySqlCommand comandoDatos = new MySqlCommand(instruccionDatos);

            //parámetros
            comandoDatos.Parameters.Add("@det_idcfdi", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@det_idtemp", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@det_cantidad", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_codigo", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_descripcion", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_claveprodser", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_claveum", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_unidaddemedida", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_precio", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_importe", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_preciounitario", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_importeunitario", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_idarticulo", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@det_idpaquetereceta", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@det_espaquete", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@det_ivas", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_ieps", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_retisr", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_retiva", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_retiep", MySqlDbType.VarChar);

            //pasar atributos de la clase a parámetros
            comandoDatos.Parameters["@det_idcfdi"].Value = IdCfdi;
            comandoDatos.Parameters["@det_idtemp"].Value = Idtemp;
            comandoDatos.Parameters["@det_cantidad"].Value = Cantidad;
            comandoDatos.Parameters["@det_codigo"].Value = Codigo;
            comandoDatos.Parameters["@det_descripcion"].Value = Descripcion;
            comandoDatos.Parameters["@det_claveprodser"].Value = ClaveProdSer;
            comandoDatos.Parameters["@det_claveum"].Value = ClaveUm;
            comandoDatos.Parameters["@det_unidaddemedida"].Value = UnidadDeMedida;
            comandoDatos.Parameters["@det_precio"].Value = Precio;
            comandoDatos.Parameters["@det_importe"].Value = Importe;
            comandoDatos.Parameters["@det_preciounitario"].Value = PrecioUnitario;
            comandoDatos.Parameters["@det_importeunitario"].Value = ImporteUnitario;
            comandoDatos.Parameters["@det_idarticulo"].Value = IdArticulo;
            comandoDatos.Parameters["@det_idpaquetereceta"].Value = IdPaquete;
            comandoDatos.Parameters["@det_espaquete"].Value = EsPaquete;
            comandoDatos.Parameters["@det_ivas"].Value = Ivas;
            comandoDatos.Parameters["@det_ieps"].Value = Ieps;
            comandoDatos.Parameters["@det_retisr"].Value = Retisr;
            comandoDatos.Parameters["@det_retiva"].Value = Retiva;
            comandoDatos.Parameters["@det_retiep"].Value = Retiep;

            //ejecutar comando
            return claseConexion.EjecutarComando(comandoDatos);
        }

        public DataTable LeerCfdiDetalle(int IdCFDI)
        {
            string query = "SELECT * FROM cfdisdetalle WHERE det_idcfdi = " + IdCFDI;
            return claseConexion.LeerTabla(query);
        }
        */
        #endregion

        #region CFDI TRASLADOS    BASE DE DATOS
        /*
        public void InsertCFDI_Traslados()
        {
            int idcfdi = UltimoCfdiIngresado();

            foreach (DataRow drTraslado in _dtConceptosCfdiTraslados.Rows)
            {
                int idtraslado = Convert.ToInt32(drTraslado["tra_id"]);
                int idtemp = Convert.ToInt32(drTraslado["tra_idtemp"]);
                decimal importe = Convert.ToDecimal(drTraslado["tra_importe"]);
                string tasa = drTraslado["tra_tasa"].ToString();
                string tasaocuota = drTraslado["tra_tasaocuota"].ToString();
                string tipofactor = drTraslado["tra_tipofactor"].ToString();
                string impuestoclavesat = drTraslado["tra_impuesto"].ToString();
                string descripcion = drTraslado["tra_descripcion"].ToString();
                string basetras = drTraslado["tra_base"].ToString();
                string tasafactorimpuesto = drTraslado["tra_tasafactorimpuesto"].ToString();

                InsertCfdiTraslados(
                    idcfdi,
                    idtraslado,
                    idtemp,
                    importe,
                    tasa,
                    tasaocuota,
                    tipofactor,
                    impuestoclavesat,
                    descripcion,
                    basetras,
                    tasafactorimpuesto);
            }
        }

        public bool InsertCfdiTraslados(
            int IdCfdi,
            int IdTraslado,
            int IdTemp,
            decimal Importe,
            string Tasa,
            string TasaoCuota,
            string TipoFactor,
            string Impuesto,
            string Descripcion,
            string BaseImporte,
            string TasaFactorImpuesto)
        {
            string instruccionDatos = "INSERT cfdistraslados ";
            instruccionDatos += "(tra_idcfdi, ";
            instruccionDatos += "tra_id, ";
            instruccionDatos += "tra_idtemp, ";
            instruccionDatos += "tra_importe, ";
            instruccionDatos += "tra_tasa, ";
            instruccionDatos += "tra_tasaocuota, ";
            instruccionDatos += "tra_tipofactor, ";
            instruccionDatos += "tra_impuesto, ";
            instruccionDatos += "tra_descripcion, ";
            instruccionDatos += "tra_base, ";
            instruccionDatos += "tra_tasafactorimpuesto) ";
            instruccionDatos += "VALUES ";
            instruccionDatos += "(@tra_idcfdi, ";
            instruccionDatos += "@tra_id, ";
            instruccionDatos += "@tra_idtemp, ";
            instruccionDatos += "@tra_importe, ";
            instruccionDatos += "@tra_tasa, ";
            instruccionDatos += "@tra_tasaocuota, ";
            instruccionDatos += "@tra_tipofactor, ";
            instruccionDatos += "@tra_impuesto, ";
            instruccionDatos += "@tra_descripcion, ";
            instruccionDatos += "@tra_base, ";
            instruccionDatos += "@tra_tasafactorimpuesto)";

            MySqlCommand comandoDatos = new MySqlCommand(instruccionDatos);

            //parámetros
            comandoDatos.Parameters.Add("@tra_idcfdi", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@tra_id", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@tra_idtemp", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@tra_importe", MySqlDbType.Decimal);
            comandoDatos.Parameters.Add("@tra_tasa", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@tra_tasaocuota", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@tra_tipofactor", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@tra_impuesto", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@tra_descripcion", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@tra_base", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@tra_tasafactorimpuesto", MySqlDbType.VarChar);

            //pasar atributos de la clase a parámetros
            comandoDatos.Parameters["@tra_idcfdi"].Value = IdCfdi;
            comandoDatos.Parameters["@tra_id"].Value = IdTraslado;
            comandoDatos.Parameters["@tra_idtemp"].Value = IdTemp;
            comandoDatos.Parameters["@tra_importe"].Value = Importe;
            comandoDatos.Parameters["@tra_tasa"].Value = Tasa;
            comandoDatos.Parameters["@tra_tasaocuota"].Value = TasaoCuota;
            comandoDatos.Parameters["@tra_tipofactor"].Value = TipoFactor;
            comandoDatos.Parameters["@tra_impuesto"].Value = Impuesto;
            comandoDatos.Parameters["@tra_descripcion"].Value = Descripcion;
            comandoDatos.Parameters["@tra_base"].Value = BaseImporte;
            comandoDatos.Parameters["@tra_tasafactorimpuesto"].Value = TasaFactorImpuesto;

            //ejecutar comando
            return claseConexion.EjecutarComando(comandoDatos);
        }

        public DataTable LeerCfdiTraslados(int IdCFDI)
        {
            string query = "SELECT * FROM cfdistraslados WHERE tra_idcfdi = " + IdCFDI;
            return claseConexion.LeerTabla(query);
        }
        */
        #endregion

        #region CFDI RETENCIONES     BASES DE DATOS
        /*
        public void InsertCFDI_Retenciones()
        {
            int idcfdi = UltimoCfdiIngresado();

            foreach (DataRow drTraslado in _dtConceptosCfdiRetenciones.Rows)
            {
                int idretencion = Convert.ToInt32(drTraslado["ret_id"]);
                int idtemp = Convert.ToInt32(drTraslado["ret_idtemp"]);
                decimal importe = Convert.ToDecimal(drTraslado["ret_importe"]);
                string tasa = drTraslado["ret_tasa"].ToString();
                string tasaocuota = drTraslado["ret_tasaocuota"].ToString();
                string tipofactor = drTraslado["ret_tipofactor"].ToString();
                string impuestoclavesat = drTraslado["ret_impuesto"].ToString();
                string descripcion = drTraslado["ret_descripcion"].ToString();
                string basetras = drTraslado["ret_base"].ToString();

                InsertCfdiRetenciones(
                    idcfdi,
                    idretencion,
                    idtemp,
                    importe,
                    tasa,
                    tasaocuota,
                    tipofactor,
                    impuestoclavesat,
                    descripcion,
                    basetras);
            }
        }

        public bool InsertCfdiRetenciones(
            int IdCfdi,
            int IdRetencion,
            int IdTemp,
            decimal Importe,
            string Tasa,
            string TasaoCuota,
            string TipoFactor,
            string Impuesto,
            string Descripcion,
            string BaseImporte)
        {
            string instruccionDatos = "INSERT cfdisretenciones ";
            instruccionDatos += "(ret_idcfdi, ";
            instruccionDatos += "ret_id, ";
            instruccionDatos += "ret_idtemp, ";
            instruccionDatos += "ret_importe, ";
            instruccionDatos += "ret_tasa, ";
            instruccionDatos += "ret_tasaocuota, ";
            instruccionDatos += "ret_tipofactor, ";
            instruccionDatos += "ret_impuesto, ";
            instruccionDatos += "ret_descripcion, ";
            instruccionDatos += "ret_base) ";
            instruccionDatos += "VALUES ";
            instruccionDatos += "(@ret_idcfdi, ";
            instruccionDatos += "@ret_id, ";
            instruccionDatos += "@ret_idtemp, ";
            instruccionDatos += "@ret_importe, ";
            instruccionDatos += "@ret_tasa, ";
            instruccionDatos += "@ret_tasaocuota, ";
            instruccionDatos += "@ret_tipofactor, ";
            instruccionDatos += "@ret_impuesto, ";
            instruccionDatos += "@ret_descripcion, ";
            instruccionDatos += "@ret_base)";

            MySqlCommand comandoDatos = new MySqlCommand(instruccionDatos);

            //parámetros
            comandoDatos.Parameters.Add("@ret_idcfdi", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@ret_id", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@ret_idtemp", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@ret_importe", MySqlDbType.Decimal);
            comandoDatos.Parameters.Add("@ret_tasa", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@ret_tasaocuota", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@ret_tipofactor", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@ret_impuesto", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@ret_descripcion", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@ret_base", MySqlDbType.VarChar);

            //pasar atributos de la clase a parámetros
            comandoDatos.Parameters["@ret_idcfdi"].Value = IdCfdi;
            comandoDatos.Parameters["@ret_id"].Value = IdRetencion;
            comandoDatos.Parameters["@ret_idtemp"].Value = IdTemp;
            comandoDatos.Parameters["@ret_importe"].Value = Importe;
            comandoDatos.Parameters["@ret_tasa"].Value = Tasa;
            comandoDatos.Parameters["@ret_tasaocuota"].Value = TasaoCuota;
            comandoDatos.Parameters["@ret_tipofactor"].Value = TipoFactor;
            comandoDatos.Parameters["@ret_impuesto"].Value = Impuesto;
            comandoDatos.Parameters["@ret_descripcion"].Value = Descripcion;
            comandoDatos.Parameters["@ret_base"].Value = BaseImporte;

            //ejecutar comando
            return claseConexion.EjecutarComando(comandoDatos);
        }

        public DataTable LeerCfdiRetenciones(int IdCFDI)
        {
            string query = "SELECT * FROM cfdisretenciones WHERE ret_idcfdi = " + IdCFDI;
            return claseConexion.LeerTabla(query);
        }
        */
        #endregion

        #region CFDI TRASLADOS RETENCIONES

        public void InsertCFDI_TrasladosRetenciones()  // De forma indirecta usa BD
        {
            // int idcfdi = UltimoCfdiIngresado(); // ojo con esto.
            Dictionary<string, decimal> _TrasladosRetenciones = new Dictionary<string, decimal>();

            //peinar traslados
            foreach (DataRow drTraslado in _dtConceptosCfdiTraslados.Rows)
            {
                decimal importe = Convert.ToDecimal(drTraslado["tra_importe"]);
                string descripcion = drTraslado["tra_descripcion"].ToString();

                
                if (_TrasladosRetenciones.Keys.Contains(descripcion))
                {
                    _TrasladosRetenciones[descripcion] += importe;
                }
                else
                {
                    _TrasladosRetenciones.Add(descripcion, importe);
                }
            }

            //peinar retenciones
            foreach (DataRow drRetencion in _dtConceptosCfdiRetenciones.Rows)
            {
                decimal importe = Convert.ToDecimal(drRetencion["ret_importe"]);
                string descripcion = drRetencion["ret_descripcion"].ToString();

                
                if (_TrasladosRetenciones.Keys.Contains(descripcion))
                {
                    _TrasladosRetenciones[descripcion] += importe;
                }
                else
                {
                    _TrasladosRetenciones.Add(descripcion, importe);
                }
            }


            //guardar todo   // ojo con esto.
            //foreach (KeyValuePair<string, decimal> dEntry in _TrasladosRetenciones)
            //{
            //    InsertTrasladosRetenciones(
            //        idcfdi,
            //        dEntry.Value.ToString(),
            //        dEntry.Key);
            //}
        }
        /*
        public bool InsertTrasladosRetenciones(int IdCfdi, string TasaImporte, string Descripcion)
        {
            string instruccionDatos = "INSERT cfdistrasladosretenciones ";
            instruccionDatos += "(trar_idcfdi, trar_importe, trar_descripcion) ";
            instruccionDatos += "VALUES ";
            instruccionDatos += "(@trar_idcfdi, @trar_importe, @trar_descripcion)";

            MySqlCommand comandoDatos = new MySqlCommand(instruccionDatos);

            //parámetros
            comandoDatos.Parameters.Add("@trar_idcfdi", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@trar_importe", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@trar_descripcion", MySqlDbType.VarChar);

            //pasar atributos de la clase a parámetros
            comandoDatos.Parameters["@trar_idcfdi"].Value = IdCfdi;
            comandoDatos.Parameters["@trar_importe"].Value = TasaImporte;
            comandoDatos.Parameters["@trar_descripcion"].Value = Descripcion;

            //ejecutar comando
            return claseConexion.EjecutarComando(comandoDatos);
        }

        public DataTable LeerCfdiTrasladosRetenciones(int IdCfdi)
        {
            string query = "SELECT * FROM cfdistrasladosretenciones WHERE trar_idcfdi = " + IdCfdi;
            return claseConexion.LeerTabla(query);
        }
        */
        #endregion

        #region CFDI DOMICILIOS
        /*
        public void InsertCFDI_Domicilios()
        {
            int idcfdi = UltimoCfdiIngresado();

            InsertCfdiDomicilios(idcfdi);
        }

        private bool InsertCfdiDomicilios(int IdCFDI)
        {
            string instruccionDatos = "INSERT cfdisdomicilios ";
            instruccionDatos += "(cfdd_idcfdi, ";

            instruccionDatos += "cfdd_callefiscal, ";
            instruccionDatos += "cfdd_numextfiscal, ";
            instruccionDatos += "cfdd_numintfiscal, ";
            instruccionDatos += "cfdd_coloniafiscal, ";
            instruccionDatos += "cfdd_localidadfiscal, ";
            instruccionDatos += "cfdd_estadofiscal, ";
            instruccionDatos += "cfdd_municipiofiscal, ";
            instruccionDatos += "cfdd_paisfiscal, ";
            instruccionDatos += "cfdd_cpfiscal, ";
            instruccionDatos += "cfdd_referenciafiscal, ";

            instruccionDatos += "cfdd_calleexpedicion, ";
            instruccionDatos += "cfdd_numextexpedicion, ";
            instruccionDatos += "cfdd_numintexpedicion, ";
            instruccionDatos += "cfdd_coloniaexpedicion, ";
            instruccionDatos += "cfdd_localidadexpedicion, ";
            instruccionDatos += "cfdd_municipioexpedicion, ";
            instruccionDatos += "cfdd_estadoexpedicion, ";
            instruccionDatos += "cfdd_paisexpedicion, ";
            instruccionDatos += "cfdd_cpexpedicion, ";
            instruccionDatos += "cfdd_referenciaexpedicion, ";

            instruccionDatos += "cfdd_callefacturar, ";
            instruccionDatos += "cfdd_numextfacturar, ";
            instruccionDatos += "cfdd_numintfacturar, ";
            instruccionDatos += "cfdd_coloniafacturar, ";
            instruccionDatos += "cfdd_localidadfacturar, ";
            instruccionDatos += "cfdd_municipiofacturar, ";
            instruccionDatos += "cfdd_estadofacturar, ";
            instruccionDatos += "cfdd_paisfacturar, ";
            instruccionDatos += "cfdd_cpfacturar) ";

            instruccionDatos += "VALUES ";

            instruccionDatos += "(@cfdd_idcfdi, ";

            instruccionDatos += "@cfdd_callefiscal, ";
            instruccionDatos += "@cfdd_numextfiscal, ";
            instruccionDatos += "@cfdd_numintfiscal, ";
            instruccionDatos += "@cfdd_coloniafiscal, ";
            instruccionDatos += "@cfdd_localidadfiscal, ";
            instruccionDatos += "@cfdd_estadofiscal, ";
            instruccionDatos += "@cfdd_municipiofiscal, ";
            instruccionDatos += "@cfdd_paisfiscal, ";
            instruccionDatos += "@cfdd_cpfiscal, ";
            instruccionDatos += "@cfdd_referenciafiscal, ";

            instruccionDatos += "@cfdd_calleexpedicion, ";
            instruccionDatos += "@cfdd_numextexpedicion, ";
            instruccionDatos += "@cfdd_numintexpedicion, ";
            instruccionDatos += "@cfdd_coloniaexpedicion, ";
            instruccionDatos += "@cfdd_localidadexpedicion, ";
            instruccionDatos += "@cfdd_municipioexpedicion, ";
            instruccionDatos += "@cfdd_estadoexpedicion, ";
            instruccionDatos += "@cfdd_paisexpedicion, ";
            instruccionDatos += "@cfdd_cpexpedicion, ";
            instruccionDatos += "@cfdd_referenciaexpedicion, ";

            instruccionDatos += "@cfdd_callefacturar, ";
            instruccionDatos += "@cfdd_numextfacturar, ";
            instruccionDatos += "@cfdd_numintfacturar, ";
            instruccionDatos += "@cfdd_coloniafacturar, ";
            instruccionDatos += "@cfdd_localidadfacturar, ";
            instruccionDatos += "@cfdd_municipiofacturar, ";
            instruccionDatos += "@cfdd_estadofacturar, ";
            instruccionDatos += "@cfdd_paisfacturar, ";
            instruccionDatos += "@cfdd_cpfacturar)";

            MySqlCommand comandoDatos = new MySqlCommand(instruccionDatos);

            //parámetros
            comandoDatos.Parameters.Add("@cfdd_idcfdi", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@cfdd_callefiscal", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_numextfiscal", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_numintfiscal", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_coloniafiscal", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_localidadfiscal", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_estadofiscal", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_municipiofiscal", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_paisfiscal", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_cpfiscal", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_referenciafiscal", MySqlDbType.VarChar);

            comandoDatos.Parameters.Add("@cfdd_calleexpedicion", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_numextexpedicion", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_numintexpedicion", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_coloniaexpedicion", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_localidadexpedicion", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_municipioexpedicion", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_estadoexpedicion", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_paisexpedicion", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_cpexpedicion", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_referenciaexpedicion", MySqlDbType.VarChar);

            comandoDatos.Parameters.Add("@cfdd_callefacturar", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_numextfacturar", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_numintfacturar", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_coloniafacturar", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_localidadfacturar", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_municipiofacturar", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_estadofacturar", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_paisfacturar", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfdd_cpfacturar", MySqlDbType.VarChar);

            //pasar atributos de la clase a parámetros
            comandoDatos.Parameters["@cfdd_idcfdi"].Value = IdCFDI;
            comandoDatos.Parameters["@cfdd_callefiscal"].Value = ClassEmpresa._CalleFiscal;
            comandoDatos.Parameters["@cfdd_numextfiscal"].Value = ClassEmpresa._NumExtFiscal;
            comandoDatos.Parameters["@cfdd_numintfiscal"].Value = ClassEmpresa._NumIntFiscal;
            comandoDatos.Parameters["@cfdd_coloniafiscal"].Value = ClassEmpresa._ColoniaFiscal;
            comandoDatos.Parameters["@cfdd_localidadfiscal"].Value = ClassEmpresa._LocalidadFiscal;
            comandoDatos.Parameters["@cfdd_estadofiscal"].Value = ClassEmpresa._EstadoFiscal;
            comandoDatos.Parameters["@cfdd_municipiofiscal"].Value = ClassEmpresa._MunicipioFiscal;
            comandoDatos.Parameters["@cfdd_paisfiscal"].Value = ClassEmpresa._PaisFiscal;
            comandoDatos.Parameters["@cfdd_cpfiscal"].Value = ClassEmpresa._CPFiscal;
            comandoDatos.Parameters["@cfdd_referenciafiscal"].Value = ClassEmpresa._ReferenciaFiscal;

            comandoDatos.Parameters["@cfdd_calleexpedicion"].Value = ClassEmpresa._CalleExpedicion;
            comandoDatos.Parameters["@cfdd_numextexpedicion"].Value = ClassEmpresa._NumExtExpedicion;
            comandoDatos.Parameters["@cfdd_numintexpedicion"].Value = ClassEmpresa._NumIntExpedicion;
            comandoDatos.Parameters["@cfdd_coloniaexpedicion"].Value = ClassEmpresa._ColoniaExpedicion;
            comandoDatos.Parameters["@cfdd_localidadexpedicion"].Value = ClassEmpresa._LocalidadExpedicion;
            comandoDatos.Parameters["@cfdd_municipioexpedicion"].Value = ClassEmpresa._MunicipioExpedicion;
            comandoDatos.Parameters["@cfdd_estadoexpedicion"].Value = ClassEmpresa._EstadoExpedicion;
            comandoDatos.Parameters["@cfdd_paisexpedicion"].Value = ClassEmpresa._PaisExpedicion;
            comandoDatos.Parameters["@cfdd_cpexpedicion"].Value = ClassEmpresa._CPExpedicion;
            comandoDatos.Parameters["@cfdd_referenciaexpedicion"].Value = ClassEmpresa._ReferenciaExpedicion;

            comandoDatos.Parameters["@cfdd_callefacturar"].Value = ClassClienteDireccion._Calle;
            comandoDatos.Parameters["@cfdd_numextfacturar"].Value = ClassClienteDireccion._Numext;
            comandoDatos.Parameters["@cfdd_numintfacturar"].Value = ClassClienteDireccion._Numint;
            comandoDatos.Parameters["@cfdd_coloniafacturar"].Value = ClassClienteDireccion._Colonia;
            comandoDatos.Parameters["@cfdd_localidadfacturar"].Value = ClassClienteDireccion._Localidad;
            comandoDatos.Parameters["@cfdd_municipiofacturar"].Value = ClassClienteDireccion._Municipio;
            comandoDatos.Parameters["@cfdd_estadofacturar"].Value = ClassClienteDireccion._Estado;
            comandoDatos.Parameters["@cfdd_paisfacturar"].Value = ClassClienteDireccion._Pais;
            comandoDatos.Parameters["@cfdd_cpfacturar"].Value = ClassClienteDireccion._CP;

            //ejecutar comando
            return claseConexion.EjecutarComando(comandoDatos);
        }

        public DataTable LeerCfdiDomicilios(int IdCFDI)
        {
            string query = "SELECT * FROM cfdisdomicilios WHERE cfdd_idcfdi = " + IdCFDI;
            return claseConexion.LeerTabla(query);
        }
        */
        #endregion

        #region CFDI DEUDA     BADE DE DATOS   con algo de operativa
        /*
        public void InsertCFDI_Deuda()
        {
            if (claseTemporal.MetodoDePagoClave != "PUE")
            {
                int idcfdi = UltimoCfdiIngresado();

                switch (claseTemporal.Moneda)
                {
                    case "PESOS":
                        {
                            decimal saldoant = ClassCliente.LeerSaldoCfdiPesos(claseTemporal.IdCliente);
                            decimal saldoact = Math.Round(saldoant + claseTemporal.Total, 2);
                            ClassCliente.UpdateSaldoCfdiPesos(claseTemporal.IdCliente, claseTemporal.Total);


                            //CARGAR DEUDA AL CLIENTE
                            ClassCliente.InsertHistorialCfdiPesos(
                                idcfdi,
                                claseTemporal.FechaEmision,
                                string.Empty,//referencia
                                claseTemporal.IdCliente,
                                claseTemporal.NombreCliente,
                                saldoant.ToString(),
                                claseTemporal.Total.ToString(),
                                saldoact.ToString(),
                                "EFECTIVO",
                                "PESOS",
                                "1",
                                1,//deuda
                                claseTemporal.IdUsuario,
                                claseTemporal.NombreUsuario);
                        } break;
                    case "DOLAR":
                        {
                            decimal saldoant = ClassCliente.LeerSaldoCfdiM2(claseTemporal.IdCliente);
                            decimal saldoact = Math.Round(saldoant + claseTemporal.Total, 2);
                            ClassCliente.UpdateSaldoCfdiM2(claseTemporal.IdCliente, claseTemporal.Total);


                            //CARGAR DEUDA AL CLIENTE
                            ClassCliente.InsertHistorialCfdiM2(
                                idcfdi,
                                claseTemporal.FechaEmision,
                                string.Empty,//referencia
                                claseTemporal.IdCliente,
                                claseTemporal.NombreCliente,
                                saldoant.ToString(),
                                claseTemporal.Total.ToString(),
                                saldoact.ToString(),
                                "EFECTIVO",
                                "DOLAR",
                                claseTemporal.TipoDeCambio,
                                1,//deuda
                                claseTemporal.IdUsuario,
                                claseTemporal.NombreUsuario);
                        } break;
                }
            }
        }
        */
        #endregion




        #region CFDI DEUDA     BASE DE DATOS

        /*
        public bool InsertCFDI_Deuda(
            int IdCFDI,
            int IdTicket,
            string Version,
            string Serie,
            string Folio,
            string TipoComprobante,
            DateTime FechaParaPago,
            int Credito,
            int IdCliente,
            string NombreCliente,
            decimal IvaTasa,
            decimal IvaTotal,
            decimal SubTotal,
            decimal Total,
            decimal RetencionIVA,
            decimal RetencionISR,
            string FormaPago,
            string MetodoPago,
            string ReferenciaDeCuenta,
            string Moneda,
            decimal TipoDeCambio,
            decimal SaldoCredito,
            DateTime FechaTimbrado,
            string RutaXML,
            string RutaPDF,
            string UUID,
            string Comentarios)
        {
            string Instrucciones = "INSERT cfdisdeuda ";
            Instrucciones += "(cfd_id, cfd_idticket, cfd_version, cfd_serie, cfd_folio, ";
            Instrucciones += "cfd_tipo, cfd_fechaparapago, cfd_credito, cfd_idcliente, ";
            Instrucciones += "cfd_nombrecliente, cfd_ivatasa, cfd_ivamporte, ";
            Instrucciones += "cfd_subtotal, cfd_total, cfd_retencioniva, cfd_retecionisr, cfd_fechatimbrado, ";

            Instrucciones += "cfd_formadepago, cfd_metododepago, cfd_referenciacuenta, cfd_moneda, cfd_tipocambio, ";
            Instrucciones += "cfd_saldocredito, ";

            Instrucciones += "cfd_xmlruta, cfd_pdfruta, cfd_uuidnumero, cfd_comentarios) ";
            Instrucciones += "VALUES ";
            Instrucciones += "(@cfd_id, @cfd_idticket, @cfd_version, @cfd_serie, @cfd_folio, ";
            Instrucciones += "@cfd_tipo, @cfd_fechaparapago, @cfd_credito, @cfd_idcliente, ";
            Instrucciones += "@cfd_nombrecliente, @cfd_ivatasa, @cfd_ivamporte, ";
            Instrucciones += "@cfd_subtotal, @cfd_total, @cfd_retencioniva, @cfd_retecionisr, @cfd_fechatimbrado, ";

            Instrucciones += "@cfd_formadepago, @cfd_metododepago, @cfd_referenciacuenta, @cfd_moneda, @cfd_tipocambio, ";
            Instrucciones += "@cfd_saldocredito, ";

            Instrucciones += "@cfd_xmlruta, @cfd_pdfruta, @cfd_uuidnumero, @cfd_comentarios)";

            MySqlCommand comandoDatos = new MySqlCommand(Instrucciones);

            //parámetros
            comandoDatos.Parameters.Add("@cfd_id", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@cfd_idticket", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@cfd_version", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_serie", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_folio", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_tipo", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_fechaparapago", MySqlDbType.DateTime);
            comandoDatos.Parameters.Add("@cfd_credito", MySqlDbType.Int32);

            comandoDatos.Parameters.Add("@cfd_idcliente", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@cfd_nombrecliente", MySqlDbType.VarChar);

            comandoDatos.Parameters.Add("@cfd_ivatasa", MySqlDbType.Decimal);
            comandoDatos.Parameters.Add("@cfd_ivamporte", MySqlDbType.Decimal);
            comandoDatos.Parameters.Add("@cfd_subtotal", MySqlDbType.Decimal);
            comandoDatos.Parameters.Add("@cfd_total", MySqlDbType.Decimal);
            comandoDatos.Parameters.Add("@cfd_retencioniva", MySqlDbType.Decimal);
            comandoDatos.Parameters.Add("@cfd_retecionisr", MySqlDbType.Decimal);
            comandoDatos.Parameters.Add("@cfd_fechatimbrado", MySqlDbType.DateTime);

            comandoDatos.Parameters.Add("@cfd_formadepago", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_metododepago", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_referenciacuenta", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_moneda", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_tipocambio", MySqlDbType.Decimal);
            comandoDatos.Parameters.Add("@cfd_saldocredito", MySqlDbType.Decimal);

            comandoDatos.Parameters.Add("@cfd_xmlruta", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_pdfruta", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_uuidnumero", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@cfd_comentarios", MySqlDbType.VarChar);

            //pasar atributos de la clase a parámetros
            comandoDatos.Parameters["@cfd_id"].Value = IdCFDI;
            comandoDatos.Parameters["@cfd_idticket"].Value = IdTicket;
            comandoDatos.Parameters["@cfd_version"].Value = Version;
            comandoDatos.Parameters["@cfd_serie"].Value = Serie;
            comandoDatos.Parameters["@cfd_folio"].Value = Folio;
            comandoDatos.Parameters["@cfd_tipo"].Value = TipoComprobante;
            comandoDatos.Parameters["@cfd_fechaparapago"].Value = FechaParaPago;
            comandoDatos.Parameters["@cfd_credito"].Value = Credito;

            comandoDatos.Parameters["@cfd_idcliente"].Value = IdCliente;
            comandoDatos.Parameters["@cfd_nombrecliente"].Value = NombreCliente;

            comandoDatos.Parameters["@cfd_ivatasa"].Value = IvaTasa;
            comandoDatos.Parameters["@cfd_ivamporte"].Value = IvaTotal;
            comandoDatos.Parameters["@cfd_subtotal"].Value = SubTotal;
            comandoDatos.Parameters["@cfd_total"].Value = Total;
            comandoDatos.Parameters["@cfd_retencioniva"].Value = RetencionIVA;
            comandoDatos.Parameters["@cfd_retecionisr"].Value = RetencionISR;

            comandoDatos.Parameters["@cfd_formadepago"].Value = FormaPago;
            comandoDatos.Parameters["@cfd_metododepago"].Value = MetodoPago;
            comandoDatos.Parameters["@cfd_referenciacuenta"].Value = ReferenciaDeCuenta;
            comandoDatos.Parameters["@cfd_moneda"].Value = Moneda;
            comandoDatos.Parameters["@cfd_tipocambio"].Value = TipoDeCambio;
            comandoDatos.Parameters["@cfd_saldocredito"].Value = SaldoCredito;

            comandoDatos.Parameters["@cfd_fechatimbrado"].Value = FechaTimbrado;
            comandoDatos.Parameters["@cfd_xmlruta"].Value = RutaXML;
            comandoDatos.Parameters["@cfd_pdfruta"].Value = RutaPDF;
            comandoDatos.Parameters["@cfd_uuidnumero"].Value = UUID;
            comandoDatos.Parameters["@cfd_comentarios"].Value = Comentarios;

            //ejecutar comando
            return claseConexion.EjecutarComando(comandoDatos);
        }

        public bool DeleteCFDI_Deuda(int IdCFDI)
        {
            string instruccion = "DELETE FROM cfdisdeuda ";
            instruccion += "WHERE cfd_id = @cfd_id";

            MySqlCommand Comando = new MySqlCommand(instruccion);

            //agregar parámetros
            Comando.Parameters.Add("@cfd_id", MySqlDbType.Int32);

            //dar valores a pararámetros
            Comando.Parameters["@cfd_id"].Value = IdCFDI;

            return claseConexion.EjecutarComando(Comando);
        }

        public DataTable LeerRangoCFDI_Deuda(int IdCliente, int Rango)
        {
            string query = "SELECT * ";
            query += "FROM cfdisdeuda ";
            query += "WHERE cfd_idcliente = " + IdCliente;
            query += " ORDER BY cfd_fechatimbrado DESC ";
            query += "LIMIT " + Rango;

            return claseConexion.LeerTabla(query);
        }

        public bool ExisteDeuda(int IdCfdi)
        {
            string query = "SELECT * ";
            query += "FROM cfdisdeuda ";
            query += "WHERE cfd_id = " + IdCfdi;

            DataTable dtCfdiDeuda = claseConexion.LeerTabla(query);

            return Convert.ToBoolean(dtCfdiDeuda.Rows.Count);
        }
        public DataTable LeerCFDI_Deuda(int IdCliente)
        {
            string query = "SELECT * ";
            query += "FROM cfdisdeuda ";
            query += "WHERE cfd_idcliente = " + IdCliente;
            query += " ORDER BY cfd_fechatimbrado DESC";

            return claseConexion.LeerTabla(query);
        }
        public DataTable LeerCFDI_Deuda(string Buscando)
        {
            string query = "SELECT * ";

            query += "FROM cfdisdeuda ";
            query += "WHERE (cfd_folio LIKE '" + Buscando + "%' OR cfd_nombrecliente LIKE '" + Buscando + "%') ";
            query += "ORDER BY cfd_fechatimbrado DESC ";
            query += "LIMIT 500";

            return claseConexion.LeerTabla(query);
        }
        public DataTable LeerCFDI_Deuda(DateTime FechaDesde, DateTime FechaHasta)
        {
            string query = "SELECT ";
            query += "cfd_id, ";
            query += "cfd_folio, ";
            query += "cfd_subtotal, ";
            query += "cfd_ivamporte, ";
            query += "cfd_total, ";
            query += "cfd_fechatimbrado, ";
            query += "cfd_nombrecliente ";

            query += "FROM cfdisdeuda ";
            query += "WHERE DATE(cfd_fechatimbrado) >= '" + FechaDesde.ToString("yyyy-MM-dd") + "' ";
            query += "AND DATE(cfd_fechatimbrado) <= '" + FechaHasta.ToString("yyyy-MM-dd") + "' ";
            query += "ORDER BY cfd_fechatimbrado ASC";

            return claseConexion.LeerTabla(query);
        }
        public DataTable LeerCFDI_DeudaPorId(int IdCfdi)
        {
            string query = "SELECT * ";
            query += "FROM cfdisdeuda ";
            query += "WHERE cfd_id = " + IdCfdi;

            return claseConexion.LeerTabla(query);
        }

        public DataTable LeerVentayDetalle(int IdCfdi)
        {
            string query = "SELECT ";
            query += "cfd_id, ";
            query += "cfd_serie, ";
            query += "cfd_folio, ";

            query += "cfd_idcliente, ";
            query += "cfd_nombrecliente, ";
            query += "cfd_fechatimbrado, ";
            query += "cfd_fechaparapago, ";
            query += "cfd_subtotal, ";
            query += "cfd_ivamporte, ";
            query += "cfd_total, ";
            query += "cfd_saldocredito, ";

            query += "det_cantidad, ";
            query += "det_codigo, ";
            query += "det_descripcion, ";
            query += "det_preciounitario, ";
            query += "det_importeunitario ";

            query += "FROM cfdisdeuda ";
            query += "LEFT JOIN cfdisdeudadetalle ";
            query += "ON cfd_id = det_idcfdi ";
            query += "WHERE cfd_id = " + IdCfdi;

            return claseConexion.LeerTabla(query);
        }

        public decimal SaldoCFDI(int IdCFDI)
        {
            decimal saldo = 0;

            string query = "SELECT ";
            query += "cfd_saldocredito ";
            query += "FROM cfdisdeuda ";
            query += "WHERE cfd_id = " + IdCFDI;

            DataTable dtSaldo = claseConexion.LeerTabla(query);
            DataRow drSaldo = dtSaldo.Rows[0];

            try
            {
                saldo = Convert.ToDecimal(drSaldo["cfd_saldocredito"]);
            }
            catch (Exception)
            {
            }

            return saldo;
        }
        public bool UpdateSaldoCFDI(int IdCFDI, decimal Cantidad)
        {
            string instruccionDatos = "UPDATE cfdisdeuda SET ";
            instruccionDatos += "cfd_saldocredito = cfd_saldocredito + @cantidad ";
            instruccionDatos += "WHERE cfd_id = @cfd_id";

            MySqlCommand comandoDatos = new MySqlCommand(instruccionDatos);
            //parámetros
            comandoDatos.Parameters.Add("@cfd_id", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@cantidad", MySqlDbType.Decimal);

            //pasar atributos de la clase a parámetros
            comandoDatos.Parameters["@cfd_id"].Value = IdCFDI;
            comandoDatos.Parameters["@cantidad"].Value = Cantidad;

            //ejecutar comando
            return claseConexion.EjecutarComando(comandoDatos);
        }
        public bool UpdateCreditoApartadoCFDI(int IdCFDI, int Tipo)
        {
            string instruccionDatos = "UPDATE cfdisdeuda SET ";
            instruccionDatos += "cfd_credito = @cfd_credito ";
            instruccionDatos += "WHERE cfd_id = @cfd_id";

            MySqlCommand comandoDatos = new MySqlCommand(instruccionDatos);
            //parámetros
            comandoDatos.Parameters.Add("@cfd_id", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@cfd_credito", MySqlDbType.Int16);

            //pasar atributos de la clase a parámetros
            comandoDatos.Parameters["@cfd_id"].Value = IdCFDI;
            comandoDatos.Parameters["@cfd_credito"].Value = Tipo;

            //ejecutar comando
            return claseConexion.EjecutarComando(comandoDatos);
        }


        public DataTable LeerIdsVentasReporte(
           int IdCliente,
           DateTime FechaInicio,
           DateTime FechaFin,
           string Filtro,
           int Saldo)
        {
            string query = "SELECT ";
            query += "cfd_id ";
            query += "FROM cfdisdeuda ";
            query += "WHERE DATE(cfd_fechatimbrado) >= '" + FechaInicio.ToString("yyyy-MM-dd") + "' ";
            query += "AND DATE(cfd_fechatimbrado) <= '" + FechaFin.ToString("yyyy-MM-dd") + "' ";
            query += "AND cfd_idcliente = " + IdCliente;

            switch (Filtro)
            {
                case "acredito":
                    {
                        query += " AND cfd_credito = 1 ";

                        if (Saldo == 1)
                        {
                            query += " AND cfd_saldocredito > 0 ";
                        }

                    }
                    break;
                case "decontado":
                    {
                        query += " AND cfd_credito = 0 ";
                    }
                    break;
            }

            query += " ORDER BY cfd_fechatimbrado ASC";

            return claseConexion.LeerTabla(query);
        }


        public void InsertCfdiDeudaContado()
        {
            decimal TotalCFDI = 0;
            decimal tipocambio = 0;
            string moneda = string.Empty;
            int IdCliente = 0;
            string NombreCliente = string.Empty;
            DateTime fechatimbrado = DateTime.Now;

            int idcfdi = UltimoCfdiIngresado();

            DataTable dtCfdiSeleccionado = LeerCFDIs(idcfdi);
            DataTable dtCfdiSeleccionadoDetalle = LeerCfdiDetalle(idcfdi);

            //guardar deuda
            foreach (DataRow drCfdi in dtCfdiSeleccionado.Rows)
            {
                int idticket = Convert.ToInt32(drCfdi["cfd_idticket"]);
                string version = drCfdi["cfd_version"].ToString();
                string serie = drCfdi["cfd_serie"].ToString();
                string folio = drCfdi["cfd_folio"].ToString();
                string tipo = drCfdi["cfd_tipo"].ToString();
                DateTime fechaparapago = Convert.ToDateTime(drCfdi["cfd_fechaparapago"]);

                IdCliente = Convert.ToInt32(drCfdi["cfd_idcliente"]);
                NombreCliente = drCfdi["cfd_nombrecliente"].ToString();

                decimal ivatasa = Convert.ToDecimal(drCfdi["cfd_ivatasa"]);
                decimal ivaimporte = Convert.ToDecimal(drCfdi["cfd_ivamporte"]);
                decimal subtotal = Convert.ToDecimal(drCfdi["cfd_subtotal"]);
                TotalCFDI = Convert.ToDecimal(drCfdi["cfd_total"]);
                decimal retencioniva = Convert.ToDecimal(drCfdi["cfd_retencioniva"]);
                decimal retecionisr = Convert.ToDecimal(drCfdi["cfd_retecionisr"]);

                string formadepago = drCfdi["cfd_formadepago"].ToString();
                string metododepago = drCfdi["cfd_metododepago"].ToString();
                string referenciacuenta = drCfdi["cfd_referenciacuenta"].ToString();
                moneda = drCfdi["cfd_moneda"].ToString();
                tipocambio = Convert.ToDecimal(drCfdi["cfd_tipocambio"]);

                string comentarios = drCfdi["cfd_comentarios"].ToString();
                string uuidnumero = drCfdi["cfd_uuidnumero"].ToString();
                fechatimbrado = Convert.ToDateTime(drCfdi["cfd_fechatimbrado"]);
                string xmlruta = drCfdi["cfd_xmlruta"].ToString();
                string pdfruta = drCfdi["cfd_pdfruta"].ToString();


                InsertCFDI_Deuda(
                    idcfdi,
                    idticket,
                    version,
                    serie,
                    folio,
                    tipo,
                    fechaparapago,
                    0,//contado
                    IdCliente,
                    NombreCliente,
                    ivatasa,
                    ivaimporte,
                    subtotal,
                    TotalCFDI,
                    retencioniva,
                    retecionisr,
                    formadepago,
                    metododepago,
                    referenciacuenta,
                    moneda,
                    tipocambio,
                    0,//saldo = 0
                    fechatimbrado,
                    xmlruta,
                    pdfruta,
                    uuidnumero,
                    comentarios);
            }

            //guardar detalle deuda
            foreach (DataRow drDetalle in dtCfdiSeleccionadoDetalle.Rows)
            {
                decimal cantidad = Convert.ToDecimal(drDetalle["det_cantidad"]);
                string codigo = drDetalle["det_codigo"].ToString();
                string descripcion = drDetalle["det_descripcion"].ToString();
                string unidaddemedida = drDetalle["det_unidaddemedida"].ToString();
                decimal preciounitario = Convert.ToDecimal(drDetalle["det_preciounitario"]);
                decimal importeunitario = Convert.ToDecimal(drDetalle["det_importeunitario"]);

                InsertDetalleCFDIDeuda(
                    idcfdi,
                    cantidad,
                    unidaddemedida,
                    descripcion,
                    preciounitario,
                    importeunitario);
            }
        }

        */

        #endregion

        #region CFDI DEUDA DETALLE     BASE DE DATOS
        /*
        public void InsertCFDI_DeudaDetalle(int IdCFDI)
        {
            DataTable dtConceptos = LeerCfdiDetalle(IdCFDI);

            foreach (DataRow drDetalle in dtConceptos.Rows)
            {
                decimal cantidad = Convert.ToDecimal(drDetalle["det_cantidad"]);
                string unidad = drDetalle["det_unidaddemedida"].ToString();
                string descripcion = drDetalle["det_descripcion"].ToString();
                decimal preciounitario = Convert.ToDecimal(drDetalle["det_preciounitario"]);
                decimal importeunitario = Convert.ToDecimal(drDetalle["det_importeunitario"]);

                InsertDetalleCFDIDeuda(
                    IdCFDI,
                    cantidad,
                    unidad,
                    descripcion,
                    preciounitario,
                    importeunitario);
            }
        }

        public bool InsertDetalleCFDIDeuda(
            int IdCFDI,
            decimal Cantidad,
            string UnidadDeMedida,
            string Descripcion,
            decimal PrecioUnitario,
            decimal ImporteUnitario)
        {
            string instruccionDatos = "INSERT cfdisdeudadetalle ";
            instruccionDatos += "(det_idcfdi, det_cantidad, det_codigo, ";
            instruccionDatos += "det_descripcion, det_unidaddemedida, ";
            instruccionDatos += "det_preciounitario, det_importeunitario) ";
            instruccionDatos += "VALUES ";
            instruccionDatos += "(@det_idcfdi, @det_cantidad, @det_codigo, ";
            instruccionDatos += "@det_descripcion, @det_unidaddemedida, ";
            instruccionDatos += "@det_preciounitario, @det_importeunitario)";

            MySqlCommand comandoDatos = new MySqlCommand(instruccionDatos);

            //parámetros
            comandoDatos.Parameters.Add("@det_idcfdi", MySqlDbType.Int32);
            comandoDatos.Parameters.Add("@det_cantidad", MySqlDbType.Decimal);
            comandoDatos.Parameters.Add("@det_codigo", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_descripcion", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_unidaddemedida", MySqlDbType.VarChar);
            comandoDatos.Parameters.Add("@det_preciounitario", MySqlDbType.Decimal);
            comandoDatos.Parameters.Add("@det_importeunitario", MySqlDbType.Decimal);

            //pasar atributos de la clase a parámetros
            comandoDatos.Parameters["@det_idcfdi"].Value = IdCFDI;
            comandoDatos.Parameters["@det_cantidad"].Value = Cantidad;
            comandoDatos.Parameters["@det_codigo"].Value = string.Empty;
            comandoDatos.Parameters["@det_descripcion"].Value = Descripcion;
            comandoDatos.Parameters["@det_unidaddemedida"].Value = UnidadDeMedida;
            comandoDatos.Parameters["@det_preciounitario"].Value = PrecioUnitario;
            comandoDatos.Parameters["@det_importeunitario"].Value = ImporteUnitario;

            //ejecutar comando
            return claseConexion.EjecutarComando(comandoDatos);
        }

        public bool DeleteCFDI_DeudaDetalle(int IdCFDI)
        {
            string instruccion = "DELETE FROM cfdisdeudadetalle ";
            instruccion += "WHERE det_idcfdi = @det_idcfdi";

            MySqlCommand Comando = new MySqlCommand(instruccion);

            //agregar parámetros
            Comando.Parameters.Add("@det_idcfdi", MySqlDbType.Int32);

            //dar valores a pararámetros
            Comando.Parameters["@det_idcfdi"].Value = IdCFDI;

            return claseConexion.EjecutarComando(Comando);
        }


        public DataTable LeerDetalleCFDIDeuda(int IdCFDI)
        {
            string query = "SELECT * FROM cfdisdeudadetalle WHERE det_idcfdi = " + IdCFDI;
            return claseConexion.LeerTabla(query);
        }

        */
        #endregion



        #region Reportes   BASE DE DATOS
        /*
        public DataTable LeerCFDIs(int IdCliente, DateTime FechaDesde, DateTime FechaHasta, string Tipo)
        {
            string query = "SELECT ";
            query += "cfd_id, ";
            query += "cfd_serie, ";
            query += "cfd_folio, ";
            query += "cfd_fechatimbrado, ";
            query += "cfd_idcliente, ";
            query += "cfd_nombrecliente, ";
            query += "cfd_subtotal, ";
            query += "cfd_ivamporte, ";
            query += "cfd_total, ";
            query += "cfd_moneda, ";
            query += "cfd_pagado, ";
            query += "cfd_fechapagado, ";

            query += "det_cantidad, ";
            query += "det_codigo, ";
            query += "det_descripcion, ";
            query += "det_preciounitario, ";
            query += "det_importeunitario ";

            query += "FROM cfdis ";
            query += "LEFT JOIN cfdisdetalle ";
            query += "ON cfd_id = det_idcfdi ";
            query += "WHERE DATE(cfd_fechatimbrado) >= '" + FechaDesde.ToString("yyyy-MM-dd") + "' ";
            query += "AND DATE(cfd_fechatimbrado) <= '" + FechaHasta.ToString("yyyy-MM-dd") + "' ";

            if (IdCliente > 0)
            {
                query += "AND cfd_idcliente = " + IdCliente;
            }

            switch (Tipo)
            {
                case "generados":
                    {
                        query += " AND cfd_cancelado = 0 ";
                    }
                    break;
                case "cancelados":
                    {
                        query += " AND cfd_cancelado = 1 ";
                    }
                    break;
            }



            query += " ORDER BY cfd_id ASC";

            return claseConexion.LeerTabla(query);
        }
        public DataTable LeerCFDIsTotales(int IdCliente, DateTime FechaDesde, DateTime FechaHasta, string Tipo)
        {
            string query = "SELECT ";
            query += "cfd_id, ";
            query += "CONCAT(cfd_serie, ' ', cfd_folio) AS cfd_folio, ";
            query += "cfd_fechatimbrado, ";
            query += "cfd_fechacancelacion, ";
            query += "cfd_idcliente, ";
            query += "cfd_nombrecliente, ";
            query += "cfd_subtotal, ";
            query += "cfd_ivamporte, ";
            query += "cfd_total, ";
            query += "cfd_moneda, ";
            query += "cfd_pagado, ";
            query += "cfd_fechapagado ";

            query += "FROM cfdis ";
            query += "WHERE DATE(cfd_fechatimbrado) >= '" + FechaDesde.ToString("yyyy-MM-dd") + "' ";
            query += "AND DATE(cfd_fechatimbrado) <= '" + FechaHasta.ToString("yyyy-MM-dd") + "' ";

            if (IdCliente > 0)
            {
                query += "AND cfd_idcliente = " + IdCliente;
            }

            switch (Tipo)
            {
                case "generados":
                    {
                        query += " AND cfd_cancelado = 0 ";
                    }
                    break;
                case "cancelados":
                    {
                        query += " AND cfd_cancelado = 1 ";
                    }
                    break;
            }



            query += " ORDER BY cfd_id ASC";

            return claseConexion.LeerTabla(query);
        }
        */
        #endregion




    }
}