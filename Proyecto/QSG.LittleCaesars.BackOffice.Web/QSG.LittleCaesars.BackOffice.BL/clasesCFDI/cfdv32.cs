using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml;

namespace MvcApplication1.clasesCFDI
{

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sat.gob.mx/cfd/3", IsNullable = false)]
    public partial class Comprobante
    {
        [System.Xml.Serialization.XmlAttribute("schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string xsiSchemaLocation = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";

        private ComprobanteEmisor emisorField;

        private ComprobanteReceptor receptorField;

        private ComprobanteConcepto[] conceptosField;

        private ComprobanteImpuestos impuestosField;

        private ComprobanteComplemento complementoField;

        private ComprobanteAddenda addendaField;

        private string versionField;

        private string serieField;

        private string folioField;

        private System.DateTime fechaField;

        private string selloField;

        private string formaDePagoField;

        private string noCertificadoField;

        private string certificadoField;

        private string condicionesDePagoField;

        private decimal subTotalField;

        private decimal descuentoField;

        private bool descuentoFieldSpecified;

        private string motivoDescuentoField;

        private string tipoCambioField;

        private string monedaField;

        private decimal totalField;

        private ComprobanteTipoDeComprobante tipoDeComprobanteField;

        private string metodoDePagoField;

        private string lugarExpedicionField;

        private string numCtaPagoField;

        private string folioFiscalOrigField;

        private string serieFolioFiscalOrigField;

        private System.DateTime fechaFolioFiscalOrigField;

        private bool fechaFolioFiscalOrigFieldSpecified;

        private decimal montoFolioFiscalOrigField;

        private bool montoFolioFiscalOrigFieldSpecified;

        private static System.Xml.Serialization.XmlSerializer serializer;

        public Comprobante()
        {
            this.versionField = "3.2";
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public ComprobanteEmisor Emisor
        {
            get
            {
                return this.emisorField;
            }
            set
            {
                this.emisorField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public ComprobanteReceptor Receptor
        {
            get
            {
                return this.receptorField;
            }
            set
            {
                this.receptorField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Concepto", IsNullable = false)]
        public ComprobanteConcepto[] Conceptos
        {
            get
            {
                return this.conceptosField;
            }
            set
            {
                this.conceptosField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public ComprobanteImpuestos Impuestos
        {
            get
            {
                return this.impuestosField;
            }
            set
            {
                this.impuestosField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public ComprobanteComplemento Complemento
        {
            get
            {
                return this.complementoField;
            }
            set
            {
                this.complementoField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public ComprobanteAddenda Addenda
        {
            get
            {
                return this.addendaField;
            }
            set
            {
                this.addendaField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string serie
        {
            get
            {
                return this.serieField;
            }
            set
            {
                this.serieField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string folio
        {
            get
            {
                return this.folioField;
            }
            set
            {
                this.folioField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime fecha
        {
            get
            {
                return this.fechaField;
            }
            set
            {
                this.fechaField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string sello
        {
            get
            {
                return this.selloField;
            }
            set
            {
                this.selloField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string formaDePago
        {
            get
            {
                return this.formaDePagoField;
            }
            set
            {
                this.formaDePagoField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string noCertificado
        {
            get
            {
                return this.noCertificadoField;
            }
            set
            {
                this.noCertificadoField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string certificado
        {
            get
            {
                return this.certificadoField;
            }
            set
            {
                this.certificadoField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string condicionesDePago
        {
            get
            {
                return this.condicionesDePagoField;
            }
            set
            {
                this.condicionesDePagoField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal subTotal
        {
            get
            {
                return this.subTotalField;
            }
            set
            {
                this.subTotalField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal descuento
        {
            get
            {
                return this.descuentoField;
            }
            set
            {
                this.descuentoField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool descuentoSpecified
        {
            get
            {
                return this.descuentoFieldSpecified;
            }
            set
            {
                this.descuentoFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string motivoDescuento
        {
            get
            {
                return this.motivoDescuentoField;
            }
            set
            {
                this.motivoDescuentoField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string TipoCambio
        {
            get
            {
                return this.tipoCambioField;
            }
            set
            {
                this.tipoCambioField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Moneda
        {
            get
            {
                return this.monedaField;
            }
            set
            {
                this.monedaField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal total
        {
            get
            {
                return this.totalField;
            }
            set
            {
                this.totalField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ComprobanteTipoDeComprobante tipoDeComprobante
        {
            get
            {
                return this.tipoDeComprobanteField;
            }
            set
            {
                this.tipoDeComprobanteField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string metodoDePago
        {
            get
            {
                return this.metodoDePagoField;
            }
            set
            {
                this.metodoDePagoField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string LugarExpedicion
        {
            get
            {
                return this.lugarExpedicionField;
            }
            set
            {
                this.lugarExpedicionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string NumCtaPago
        {
            get
            {
                return this.numCtaPagoField;
            }
            set
            {
                this.numCtaPagoField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string FolioFiscalOrig
        {
            get
            {
                return this.folioFiscalOrigField;
            }
            set
            {
                this.folioFiscalOrigField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SerieFolioFiscalOrig
        {
            get
            {
                return this.serieFolioFiscalOrigField;
            }
            set
            {
                this.serieFolioFiscalOrigField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime FechaFolioFiscalOrig
        {
            get
            {
                return this.fechaFolioFiscalOrigField;
            }
            set
            {
                this.fechaFolioFiscalOrigField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FechaFolioFiscalOrigSpecified
        {
            get
            {
                return this.fechaFolioFiscalOrigFieldSpecified;
            }
            set
            {
                this.fechaFolioFiscalOrigFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal MontoFolioFiscalOrig
        {
            get
            {
                return this.montoFolioFiscalOrigField;
            }
            set
            {
                this.montoFolioFiscalOrigField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MontoFolioFiscalOrigSpecified
        {
            get
            {
                return this.montoFolioFiscalOrigFieldSpecified;
            }
            set
            {
                this.montoFolioFiscalOrigFieldSpecified = value;
            }
        }

        private static System.Xml.Serialization.XmlSerializer Serializer
        {
            get
            {
                if ((serializer == null))
                {
                    serializer = new System.Xml.Serialization.XmlSerializer(typeof(Comprobante));
                }
                return serializer;
            }
        }

        public virtual string Serialize(System.Text.Encoding encoding)
        {
            System.IO.StreamReader streamReader = null;
            System.IO.MemoryStream memoryStream = null;
            try
            {
                //Name spaces
                XmlSerializerNamespaces xmlNameSpace = new XmlSerializerNamespaces();
                xmlNameSpace.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xmlNameSpace.Add("cfdi", "http://www.sat.gob.mx/cfd/3");

                memoryStream = new System.IO.MemoryStream();
                System.Xml.XmlWriterSettings xmlWriterSettings = new System.Xml.XmlWriterSettings();
                xmlWriterSettings.Encoding = encoding;
                System.Xml.XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);

                //Agregar el xmlNameSpace
                Serializer.Serialize(xmlWriter, this, xmlNameSpace);

                memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                streamReader = new System.IO.StreamReader(memoryStream);
                return streamReader.ReadToEnd();
            }
            finally
            {
                if ((streamReader != null))
                {
                    streamReader.Dispose();
                }
                if ((memoryStream != null))
                {
                    memoryStream.Dispose();
                }
            }
        }

        public virtual void SaveToFile(string fileName, System.Text.Encoding encoding)
        {
            System.IO.StreamWriter streamWriter = null;
            try
            {
                string xmlString = Serialize(encoding);
                streamWriter = new System.IO.StreamWriter(fileName, false, Encoding.UTF8);
                streamWriter.WriteLine(xmlString);
                streamWriter.Close();
            }
            finally
            {
                if ((streamWriter != null))
                {
                    streamWriter.Dispose();
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteEmisor
    {
        private t_UbicacionFiscal domicilioFiscalField;

        private t_Ubicacion expedidoEnField;

        private ComprobanteEmisorRegimenFiscal[] regimenFiscalField;

        private string rfcField;

        private string nombreField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public t_UbicacionFiscal DomicilioFiscal
        {
            get
            {
                return this.domicilioFiscalField;
            }
            set
            {
                this.domicilioFiscalField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public t_Ubicacion ExpedidoEn
        {
            get
            {
                return this.expedidoEnField;
            }
            set
            {
                this.expedidoEnField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("RegimenFiscal", Order = 2)]
        public ComprobanteEmisorRegimenFiscal[] RegimenFiscal
        {
            get
            {
                return this.regimenFiscalField;
            }
            set
            {
                this.regimenFiscalField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string rfc
        {
            get
            {
                return this.rfcField;
            }
            set
            {
                this.rfcField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nombre
        {
            get
            {
                return this.nombreField;
            }
            set
            {
                this.nombreField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sat.gob.mx/cfd/3")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sat.gob.mx/cfd/3", IsNullable = true)]
    public partial class t_UbicacionFiscal
    {

        private string calleField;

        private string noExteriorField;

        private string noInteriorField;

        private string coloniaField;

        private string localidadField;

        private string referenciaField;

        private string municipioField;

        private string estadoField;

        private string paisField;

        private string codigoPostalField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string calle
        {
            get
            {
                return this.calleField;
            }
            set
            {
                this.calleField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string noExterior
        {
            get
            {
                return this.noExteriorField;
            }
            set
            {
                this.noExteriorField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string noInterior
        {
            get
            {
                return this.noInteriorField;
            }
            set
            {
                this.noInteriorField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string colonia
        {
            get
            {
                return this.coloniaField;
            }
            set
            {
                this.coloniaField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string localidad
        {
            get
            {
                return this.localidadField;
            }
            set
            {
                this.localidadField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string referencia
        {
            get
            {
                return this.referenciaField;
            }
            set
            {
                this.referenciaField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string municipio
        {
            get
            {
                return this.municipioField;
            }
            set
            {
                this.municipioField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string estado
        {
            get
            {
                return this.estadoField;
            }
            set
            {
                this.estadoField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pais
        {
            get
            {
                return this.paisField;
            }
            set
            {
                this.paisField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codigoPostal
        {
            get
            {
                return this.codigoPostalField;
            }
            set
            {
                this.codigoPostalField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sat.gob.mx/cfd/3")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sat.gob.mx/cfd/3", IsNullable = true)]
    public partial class t_InformacionAduanera
    {

        private string numeroField;

        private System.DateTime fechaField;

        private string aduanaField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string numero
        {
            get
            {
                return this.numeroField;
            }
            set
            {
                this.numeroField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime fecha
        {
            get
            {
                return this.fechaField;
            }
            set
            {
                this.fechaField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string aduana
        {
            get
            {
                return this.aduanaField;
            }
            set
            {
                this.aduanaField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sat.gob.mx/cfd/3")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sat.gob.mx/cfd/3", IsNullable = true)]
    public partial class t_Ubicacion
    {

        private string calleField;

        private string noExteriorField;

        private string noInteriorField;

        private string coloniaField;

        private string localidadField;

        private string referenciaField;

        private string municipioField;

        private string estadoField;

        private string paisField;

        private string codigoPostalField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string calle
        {
            get
            {
                return this.calleField;
            }
            set
            {
                this.calleField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string noExterior
        {
            get
            {
                return this.noExteriorField;
            }
            set
            {
                this.noExteriorField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string noInterior
        {
            get
            {
                return this.noInteriorField;
            }
            set
            {
                this.noInteriorField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string colonia
        {
            get
            {
                return this.coloniaField;
            }
            set
            {
                this.coloniaField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string localidad
        {
            get
            {
                return this.localidadField;
            }
            set
            {
                this.localidadField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string referencia
        {
            get
            {
                return this.referenciaField;
            }
            set
            {
                this.referenciaField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string municipio
        {
            get
            {
                return this.municipioField;
            }
            set
            {
                this.municipioField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string estado
        {
            get
            {
                return this.estadoField;
            }
            set
            {
                this.estadoField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pais
        {
            get
            {
                return this.paisField;
            }
            set
            {
                this.paisField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codigoPostal
        {
            get
            {
                return this.codigoPostalField;
            }
            set
            {
                this.codigoPostalField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteEmisorRegimenFiscal
    {

        private string regimenField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Regimen
        {
            get
            {
                return this.regimenField;
            }
            set
            {
                this.regimenField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteReceptor
    {

        private t_Ubicacion domicilioField;

        private string rfcField;

        private string nombreField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public t_Ubicacion Domicilio
        {
            get
            {
                return this.domicilioField;
            }
            set
            {
                this.domicilioField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string rfc
        {
            get
            {
                return this.rfcField;
            }
            set
            {
                this.rfcField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nombre
        {
            get
            {
                return this.nombreField;
            }
            set
            {
                this.nombreField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteConcepto
    {

        private object[] itemsField;

        private decimal cantidadField;

        private string unidadField;

        private string noIdentificacionField;

        private string descripcionField;

        private decimal valorUnitarioField;

        private decimal importeField;

        [System.Xml.Serialization.XmlElementAttribute("ComplementoConcepto", typeof(ComprobanteConceptoComplementoConcepto), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("CuentaPredial", typeof(ComprobanteConceptoCuentaPredial), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("InformacionAduanera", typeof(t_InformacionAduanera), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Parte", typeof(ComprobanteConceptoParte), Order = 0)]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal cantidad
        {
            get
            {
                return this.cantidadField;
            }
            set
            {
                this.cantidadField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string unidad
        {
            get
            {
                return this.unidadField;
            }
            set
            {
                this.unidadField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string noIdentificacion
        {
            get
            {
                return this.noIdentificacionField;
            }
            set
            {
                this.noIdentificacionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string descripcion
        {
            get
            {
                return this.descripcionField;
            }
            set
            {
                this.descripcionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal valorUnitario
        {
            get
            {
                return this.valorUnitarioField;
            }
            set
            {
                this.valorUnitarioField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal importe
        {
            get
            {
                return this.importeField;
            }
            set
            {
                this.importeField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteConceptoComplementoConcepto
    {
        private System.Xml.XmlElement[] anyField;

        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteConceptoCuentaPredial
    {
        private string numeroField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string numero
        {
            get
            {
                return this.numeroField;
            }
            set
            {
                this.numeroField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteConceptoParte
    {
        private t_InformacionAduanera[] informacionAduaneraField;

        private decimal cantidadField;

        private string unidadField;

        private string noIdentificacionField;

        private string descripcionField;

        private decimal valorUnitarioField;

        private bool valorUnitarioFieldSpecified;

        private decimal importeField;

        private bool importeFieldSpecified;

        [System.Xml.Serialization.XmlElementAttribute("InformacionAduanera", Order = 0)]
        public t_InformacionAduanera[] InformacionAduanera
        {
            get
            {
                return this.informacionAduaneraField;
            }
            set
            {
                this.informacionAduaneraField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal cantidad
        {
            get
            {
                return this.cantidadField;
            }
            set
            {
                this.cantidadField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string unidad
        {
            get
            {
                return this.unidadField;
            }
            set
            {
                this.unidadField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string noIdentificacion
        {
            get
            {
                return this.noIdentificacionField;
            }
            set
            {
                this.noIdentificacionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string descripcion
        {
            get
            {
                return this.descripcionField;
            }
            set
            {
                this.descripcionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal valorUnitario
        {
            get
            {
                return this.valorUnitarioField;
            }
            set
            {
                this.valorUnitarioField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valorUnitarioSpecified
        {
            get
            {
                return this.valorUnitarioFieldSpecified;
            }
            set
            {
                this.valorUnitarioFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal importe
        {
            get
            {
                return this.importeField;
            }
            set
            {
                this.importeField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool importeSpecified
        {
            get
            {
                return this.importeFieldSpecified;
            }
            set
            {
                this.importeFieldSpecified = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteImpuestos
    {
        private ComprobanteImpuestosRetencion[] retencionesField;

        private ComprobanteImpuestosTraslado[] trasladosField;

        private decimal totalImpuestosRetenidosField;

        private bool totalImpuestosRetenidosFieldSpecified;

        private decimal totalImpuestosTrasladadosField;

        private bool totalImpuestosTrasladadosFieldSpecified;

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Retencion", IsNullable = false)]
        public ComprobanteImpuestosRetencion[] Retenciones
        {
            get
            {
                return this.retencionesField;
            }
            set
            {
                this.retencionesField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Traslado", IsNullable = false)]
        public ComprobanteImpuestosTraslado[] Traslados
        {
            get
            {
                return this.trasladosField;
            }
            set
            {
                this.trasladosField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal totalImpuestosRetenidos
        {
            get
            {
                return this.totalImpuestosRetenidosField;
            }
            set
            {
                this.totalImpuestosRetenidosField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool totalImpuestosRetenidosSpecified
        {
            get
            {
                return this.totalImpuestosRetenidosFieldSpecified;
            }
            set
            {
                this.totalImpuestosRetenidosFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal totalImpuestosTrasladados
        {
            get
            {
                return this.totalImpuestosTrasladadosField;
            }
            set
            {
                this.totalImpuestosTrasladadosField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool totalImpuestosTrasladadosSpecified
        {
            get
            {
                return this.totalImpuestosTrasladadosFieldSpecified;
            }
            set
            {
                this.totalImpuestosTrasladadosFieldSpecified = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteImpuestosRetencion
    {
        private ComprobanteImpuestosRetencionImpuesto impuestoField;

        private decimal importeField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ComprobanteImpuestosRetencionImpuesto impuesto
        {
            get
            {
                return this.impuestoField;
            }
            set
            {
                this.impuestoField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal importe
        {
            get
            {
                return this.importeField;
            }
            set
            {
                this.importeField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
    public enum ComprobanteImpuestosRetencionImpuesto
    {
        ISR,
        IVA,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteImpuestosTraslado
    {
        private ComprobanteImpuestosTrasladoImpuesto impuestoField;

        private decimal tasaField;

        private decimal importeField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ComprobanteImpuestosTrasladoImpuesto impuesto
        {
            get
            {
                return this.impuestoField;
            }
            set
            {
                this.impuestoField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal tasa
        {
            get
            {
                return this.tasaField;
            }
            set
            {
                this.tasaField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal importe
        {
            get
            {
                return this.importeField;
            }
            set
            {
                this.importeField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
    public enum ComprobanteImpuestosTrasladoImpuesto
    {
        IVA,
        IEPS,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteComplemento
    {
        private System.Xml.XmlElement[] anyField;

        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteAddenda
    {
        private System.Xml.XmlElement[] anyField;

        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
    public enum ComprobanteTipoDeComprobante
    {
        ingreso,
        egreso,
        traslado,
    }

}