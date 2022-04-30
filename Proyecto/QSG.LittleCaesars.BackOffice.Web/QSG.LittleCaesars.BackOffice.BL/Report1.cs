namespace QSG.LittleCaesars.BackOffice.BL
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using BackOffice.BL;
    using BackOffice.Common.Entities;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using System.Xml;
    using System.Xml.Xsl;
    using System.Globalization;
    using System.Threading;
    using ThoughtWorks.QRCode.Codec;

    /// <summary>
    /// Summary description for Report1.
    /// </summary>
    public partial class Report1 : Telerik.Reporting.Report
    {
        public static CultureInfo culture = new CultureInfo("es-MX", true);
        public Report1()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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
        public Report1(ComprobanteEmisor Emisor, Comprobante Factura, Sucursal sucursal, string XMLTimbrado,string _rutaxlst ,String[] D_Matriz,out string FechaTimbrado ,out string msg)
        {

            InitializeComponent();

            txtCadenaOriginalSAT.Value = "";
            txtCant.Value ="";
            txtCd_Estado.Value = "";
            txtcertDig.Value = "";
            txtConcepto.Value = "";
            txtCPag.Value = "";
            txtDomMatriz.Value = "";
            txtDomSuc.Value = "";
            txtefecFiscalPag.Value = "";
            txtEmpresa.Value = "";
            txtExpedidoEn.Value = "";
            txtFechaFactura.Value = "";
            txtFechaHoraTimbrado.Value = "";
            txtfolio.Value = "";
            txtImp.Value = "";
            txtImpIVA.Value = "";
            txtImpLetra.Value = "";
            txtMPag.Value = "";
            txtNCta.Value = "";
            txtObservaciones.Value = "";
            txtPUnit.Value = "";
            txtRecepD.Value = "";
            txtRecepNom.Value = "";
            txtRecepRFC.Value="";
            txtRecepTel.Value="";
            txtRegimenFiscal.Value="";
            txtRFCEmisor.Value = "";
            txtSelloCFDI.Value = "";
            txtSelloSAT.Value = "";
            txtserie.Value = "";
            txtSerieCertSAT.Value = "";
            txtSubT.Value = "";
            txtSuc.Value = "";
            txtSucMatriz.Value = "";
            txtTotal.Value = "";
            txtUnidad.Value = "";
            txtUUID.Value = "";
            

            


           msg = string.Empty;

            Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha = DateTime.Today;

            var _telM = D_Matriz[0];
            var _noInt = D_Matriz[1];
            var _fechaVta = String.Format("{0:dd MMM yyyy}", D_Matriz[2]);
            var _regimenfiscal = D_Matriz[3];

            var _XML = XMLTimbrado;
           
            var _uuid = "";
            var _noCertSAT = "";
            var _fechaTimbrado = "";
            var _selloSAT = "";
            var _selloCFD = "";
            var _certDigital = "";


            var _DomMatriz = "";
            var _DomRecep = "";
            FechaTimbrado = "";

            NumberStyles _NumberStyle = NumberStyles.AllowDecimalPoint;
            IFormatProvider _FormatProvider = System.Globalization.NumberFormatInfo.InvariantInfo;

            try
            {


                XmlDocument xmldoc = new XmlDocument();
                xmldoc.InnerXml = XMLTimbrado;

                //Agregar informacion aduanera
                XmlNodeList nods1 = xmldoc.GetElementsByTagName("cfdi:Comprobante");
                foreach (XmlNode no in nods1)
                {
                    if (no.LocalName == "Comprobante")
                    {
                        XmlAttributeCollection atr = no.Attributes;
                        _certDigital = atr["NoCertificado"].Value;
                        break;
                    }
                }

                XmlNodeList nods = xmldoc.GetElementsByTagName("tfd:TimbreFiscalDigital");
                foreach (XmlNode no in nods)
                {
                    if (no.LocalName == "TimbreFiscalDigital")
                    {
                        XmlAttributeCollection att = no.Attributes;

                        _uuid = att["UUID"].Value;
                        _fechaTimbrado = String.Format("{0:dd MMM yyyy hh:mm t}", att["FechaTimbrado"].Value);
                        _noCertSAT = att["NoCertificadoSAT"].Value;
                        _selloCFD = att["SelloCFD"].Value;
                        _selloSAT = att["SelloSAT"].Value;
                        break;
                    }
                }



                FechaTimbrado = String.Format("{0:dd/MM/yyyy}", _fechaTimbrado);
                XslCompiledTransform transformador = new XslCompiledTransform();
                //transformador.Load(_rutaxlst + "\\cadenaoriginal_TFD_1_0.xslt");
                transformador.Load(_rutaxlst + "\\zzcadenaoriginal_3_3_temp.xslt");

                StringWriter CadenaOriginal = new StringWriter();
                transformador.Transform(xmldoc.CreateNavigator(), null, CadenaOriginal);

                txtEmpresa.Value = Emisor.nombre.ToString().ToUpper();
                txtRFCEmisor.Value = Emisor.rfc.ToString().ToUpper();

                txtSucMatriz.Value = "MATRIZ";

                if (ValidarNull(Emisor.DomicilioFiscal.calle) != "") { _DomMatriz += Emisor.DomicilioFiscal.calle; }
                if (ValidarNull(Emisor.DomicilioFiscal.noExterior) != "") { _DomMatriz += " "+Emisor.DomicilioFiscal.noExterior; }
                if (ValidarNull(Emisor.DomicilioFiscal.noInterior) != "") { _DomMatriz += " INT : " + Emisor.DomicilioFiscal.noInterior; }
                if (ValidarNull(Emisor.DomicilioFiscal.colonia) != "") { _DomMatriz += " " + Emisor.DomicilioFiscal.colonia; }
                if (ValidarNull(Emisor.DomicilioFiscal.localidad) != "") { _DomMatriz += " ," + Emisor.DomicilioFiscal.localidad; }
                if (ValidarNull(Emisor.DomicilioFiscal.municipio) != "") { _DomMatriz += " ," + Emisor.DomicilioFiscal.municipio; }
                if (ValidarNull(Emisor.DomicilioFiscal.estado) != "") { _DomMatriz += " " + Emisor.DomicilioFiscal.estado; }
                if (ValidarNull(Emisor.DomicilioFiscal.codigoPostal) != "") { _DomMatriz += " C.P: " + Emisor.DomicilioFiscal.codigoPostal; }
                if (_telM != "") { _DomMatriz += "\nTel: " + _telM; }


                txtDomMatriz.Value = _DomMatriz;
                //txtDomMatriz.Value =ValidarNull(Emisor.DomicilioFiscal.calle) + " " +  Emisor.DomicilioFiscal.noExterior +" "+ ValidarNull( Emisor.DomicilioFiscal.noInterior)!="" ? "Int: "+Emisor.DomicilioFiscal.noInterior:""  +" " +ValidarNull( Emisor.DomicilioFiscal.colonia) +" "+ ValidarNull(Emisor.DomicilioFiscal.localidad)!=""? ","+ Emisor.DomicilioFiscal.localidad :"" + " " + ValidarNull(Emisor.DomicilioFiscal.municipio) !=""? ","+Emisor.DomicilioFiscal.municipio:"" + " " + ValidarNull(Emisor.DomicilioFiscal.estado) + " C.P " + Emisor.DomicilioFiscal.codigoPostal + " \nTel: " + _telM;


                txtSuc.Value = ValidarNull(sucursal.Abr);
                txtDomSuc.Value = ValidarNull(sucursal.Calle) + " " + sucursal.NoExt + " Int." + sucursal.NoInt + " " + ValidarNull(sucursal.Colonia) + " " + ValidarNull(sucursal.Municipio) + " " + ValidarNull(sucursal.Estado) + " C.P " + sucursal.CP;
                
                txtserie.Value = Factura.serie;
                txtfolio.Value = Factura.folio;
                txtFechaFactura.Value = String.Format("{0:dd MMM yyyy hh:mm t}", Factura.fecha);
                txtExpedidoEn.Value = ValidarNull(Factura.LugarExpedicion);
                txtRecepNom.Value = ValidarNull(Factura.Receptor.nombre);

                txtRecepD.Value = ValidarNull(Factura.Receptor.Domicilio.calle) + " Ext. " + ValidarNull(Factura.Receptor.Domicilio.noExterior) + " Int. " + ValidarNull(Factura.Receptor.Domicilio.noInterior) + " ," + ValidarNull(Factura.Receptor.Domicilio.colonia) + " C.P " + Factura.Receptor.Domicilio.codigoPostal;

                txtCd_Estado.Value = ValidarNull(Factura.Receptor.Domicilio.localidad) + " " + ValidarNull(Factura.Receptor.Domicilio.municipio)+" " + ValidarNull(Factura.Receptor.Domicilio.estado);
                txtRecepRFC.Value = Factura.Receptor.rfc;


                txtFechaHoraTimbrado.Value = _fechaTimbrado;

                /*Conceptos*/
                txtCant.Value = Factura.Conceptos[0].cantidad.ToString();
                txtUnidad.Value =ValidarNull( Factura.Conceptos[0].unidad);
                txtConcepto.Value =ValidarNull( Factura.Conceptos[0].descripcion);
                txtPUnit.Value = Factura.Conceptos[0].valorUnitario.ToString("$0.00");
                txtImp.Value = Factura.Conceptos[0].importe.ToString("$0.00");
                txtSubT.Value = Factura.subTotal.ToString("$0.00");
                txtImpIVA.Value = Factura.Impuestos.Traslados[0].importe.ToString("$0.00");
                txtTotal.Value = Factura.total.ToString("$0.00");

                txtObservaciones.Value = "Fecha de Consumo : " + _fechaVta + " \nNumero de  Ticket: " + Factura.Conceptos[0].noIdentificacion;
                txtImpLetra.Value = ConvertToLetter.ToCardinal(Factura.total);
                txtefecFiscalPag.Value = ("Metodo de Pago " + ValidarNull(Factura.formaDePago));
                txtCPag.Value = Factura.condicionesDePago;
                txtMPag.Value = Factura.metodoDePago;
                txtRegimenFiscal.Value = _regimenfiscal;

                txtCadenaOriginalSAT.Value = CadenaOriginal.ToString();
                txtSelloCFDI.Value = _selloCFD;
                txtSelloSAT.Value = _selloSAT;


                txtUUID.Value = _uuid;
                txtcertDig.Value = Factura.noCertificado.ToString();
                txtSerieCertSAT.Value = _noCertSAT;


                /**********************Generar QR********************/


                string datos = "?re={0}&rr={1}&tt={2}&id={3}";

                string sTotal = "";
                string entero = "";
                string decimales = "";
                string resultado = "";

                sTotal = String.Format("{0:0.000000}", Factura.total).Replace(',', '.');
                entero = decimal.Truncate(Factura.total).ToString().PadLeft(10, Convert.ToChar("0"));
                decimales = sTotal.Split(Convert.ToChar("."))[1];
                resultado = entero + "." + decimales;

                string[] pars = { Emisor.rfc, Factura.Receptor.rfc, resultado, _uuid };
                datos = string.Format(datos, pars);



                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeScale = 4;//4
                qrCodeEncoder.QRCodeVersion = 8;
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                ptBxQR.Value = qrCodeEncoder.Encode(datos);

                /*****************************************/



            }
            catch (Exception ex)
            {
                msg += ex.Message;
            }

           
        }
    }
}