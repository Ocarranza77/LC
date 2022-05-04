using System;
using System.Windows.Forms;

namespace GenerarXml40
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            //clase principal
            Comprobante COMPROBANTE = new Comprobante();

            //Agregamos los atributos del comprobante
            COMPROBANTE.Version = string.Empty;
            COMPROBANTE.Serie = string.Empty;
            COMPROBANTE.Folio = string.Empty;
            COMPROBANTE.TipoDeComprobante = string.Empty;
            COMPROBANTE.Fecha = DateTime.Now;
            COMPROBANTE.SubTotal = 0m;
            COMPROBANTE.Descuento = 0m;
            COMPROBANTE.Total = 0m;
            COMPROBANTE.Moneda = string.Empty;
            COMPROBANTE.TipoCambio = 0m;
            COMPROBANTE.Exportacion = string.Empty;
            COMPROBANTE.FormaPago = string.Empty;
            COMPROBANTE.MetodoPago = string.Empty;
            COMPROBANTE.CondicionesDePago = string.Empty;
            COMPROBANTE.LugarExpedicion = string.Empty;
            COMPROBANTE.Confirmacion = string.Empty;

            //Agregamos cfdis relacionados
            CfdiRelacionados relacionados = new CfdiRelacionados();
            relacionados.TipoRelacion = string.Empty;
            CfdiRelacionado relacionado = new CfdiRelacionado();
            relacionado.UUID = string.Empty;
            relacionados.CfdiRelacionado.Add(relacionado);
            COMPROBANTE.CfdiRelacionados = relacionados;

            //Agregamos el emisor
            Emisor emisor = new Emisor();
            emisor.Nombre = string.Empty;
            emisor.Rfc = string.Empty;
            emisor.RegimenFiscal = string.Empty;
            emisor.FacAtrAdquirente = string.Empty;
            COMPROBANTE.Emisor = emisor;

            //Agregamos el Receptor
            Receptor receptor = new Receptor();
            receptor.Nombre = string.Empty;
            receptor.Rfc = string.Empty;
            receptor.DomicilioFiscalReceptor = string.Empty;
            receptor.RegimenFiscalReceptor = string.Empty;
            receptor.NumRegIdTrib = string.Empty;
            receptor.ResidenciaFiscal = string.Empty;
            receptor.UsoCFDI = string.Empty;
            COMPROBANTE.Receptor = receptor;


            //Iniciamos los conceptos
            COMPROBANTE.Conceptos = new Conceptos();

            #region conceptos

            Concepto concepto = new Concepto();
            concepto.ClaveProdServ = string.Empty;
            concepto.ClaveUnidad = string.Empty;
            concepto.Unidad = string.Empty;
            concepto.NoIdentificacion = string.Empty;
            concepto.Descripcion = string.Empty;
            concepto.Descuento = 0m;
            concepto.Importe = 0m;
            concepto.ValorUnitario = 0m;
            concepto.Cantidad = 0m;
            concepto.ObjetoImp = string.Empty;

            //iniciar impuestos
            concepto.Impuestos = new ImpuestosC();

            string objimpclave = "claveobjetodeimpuesto";
            switch (objimpclave)
            {
                case "01"://No objeto de impuesto.
                    {

                    } break;
                case "02"://Sí objeto de impuesto.
                    {
                        //Creo el traslado para el concepto
                        TrasladoC trasladoConcepto = new TrasladoC();
                        trasladoConcepto.Basee = 0m;
                        trasladoConcepto.TasaOCuota = 0m;
                        trasladoConcepto.Importe = 0m;
                        trasladoConcepto.TipoFactor = string.Empty;
                        trasladoConcepto.Impuesto = string.Empty;

                        concepto.Impuestos.Traslados.Add(trasladoConcepto);


                        //Creo la retención para el concepto
                        RetencionC retencionConcepto = new RetencionC();
                        retencionConcepto.Basee = 0m;
                        retencionConcepto.TasaOCuota = 0m;
                        retencionConcepto.Importe = 0m;
                        retencionConcepto.TipoFactor = string.Empty;
                        retencionConcepto.Impuesto = string.Empty;

                        concepto.Impuestos.Retenciones.Add(retencionConcepto);
                    } break;
                case "03"://Sí objeto del impuesto y no obligado al desglose.
                    {

                    } break;
            }


            //pedimentos
            InformacionAduaneraC infoaduanera = new InformacionAduaneraC();
            infoaduanera.NumeroPedimento = string.Empty;
            concepto.InformacionAduanera.Add(infoaduanera);


            //Agregamos el concepto
            COMPROBANTE.Conceptos.Concepto.Add(concepto);

            #endregion

            //iniciamos impuestos del comprobante
            Impuestos impuestos = new Impuestos();

            Traslado traslado = new Traslado();
            traslado.Basee = 0m;
            traslado.Importe = 0m;
            traslado.Impuesto = string.Empty;
            traslado.TasaOCuota = 0m;
            traslado.TipoFactor = string.Empty;
            impuestos.Traslados.Add(traslado);

            Retencion retencion = new Retencion();
            retencion.Impuesto = string.Empty;
            retencion.Importe = 0m;
            impuestos.Retenciones.Add(retencion);

            //total impuestos
            impuestos.TotalImpuestosTrasladados = 0m;
            impuestos.TotalImpuestosRetenidos = 0m;
                        

            //Agregamos impuestos al comprobante
            COMPROBANTE.Impuestos = new Impuestos();
            COMPROBANTE.Impuestos = impuestos;


            //COMPLEMENTO CARTA PORTE...



            //Creo el xml 
            zzGenerarXML generarxml = new zzGenerarXML();
            string xmlcadena = generarxml.GuardarXMLPorCertificado(COMPROBANTE, "rutacer", "rutakey", "clavekey");

        }

    }
}