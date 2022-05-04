using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;
using System.Windows.Forms;

namespace GenerarXml40
{
    public class zzGenerarXML
    {
        public zzGenerarXML()
        {
        }

        private string NAMESPACE_CFD = "http://www.sat.gob.mx/cfd/4";
        private string NAMESPACE_PAGOS = "http://www.sat.gob.mx/Pagos20";
        private string NAMESPACE_CARTA_PORTE = "http://www.sat.gob.mx/CartaPorte20";

        private string SCHEMALOCATION_CFD = "http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd";
        private string SCHEMALOCATION_PAGOS = "http://www.sat.gob.mx/Pagos20 http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos20.xsd";
        private string SCHEMALOCATION_CARTA_PORTE = "http://www.sat.gob.mx/CartaPorte20 http://www.sat.gob.mx/sitio_internet/cfd/CartaPorte/CartaPorte20.xsd";
        
        
        
        public string GuardarXMLPorCertificado(Comprobante c, string rutaCertificado, string rutaArchivoClavePrivada, string passwordClavePrivada)
        {
            XmlDocument DOCUMENTO = new XmlDocument();
            
            XmlProcessingInstruction cProcessing = DOCUMENTO.CreateProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
            DOCUMENTO.AppendChild(cProcessing);

            XmlElement ComprobanteElement = DOCUMENTO.CreateElement("cfdi", "Comprobante", NAMESPACE_CFD);
            XmlElement ComprobanteNodo = AgregarNodoComprobante(c, DOCUMENTO, ComprobanteElement, rutaCertificado);

            DOCUMENTO.AppendChild(ComprobanteNodo);

            XmlNodeList elementos = DOCUMENTO.GetElementsByTagName("cfdi:Comprobante");

            string sello = ObtenerSelloPorCertificado(DOCUMENTO, rutaArchivoClavePrivada, passwordClavePrivada);
            (elementos[0] as XmlElement).SetAttribute("Sello", sello);
            
            //por si quiero guardar como archivo
            //DOCUMENTO.Save(rutaXML);

            return DOCUMENTO.OuterXml;
        }
        private XmlElement AgregarNodoComprobante(Comprobante c, XmlDocument documento, XmlElement nComprobante, string rutaCertificado)
        {
            XmlElement nodoComprobante = nComprobante;

            nodoComprobante.SetAttribute("Version", c.Version);

            if (c.Serie != string.Empty)
            {
                nodoComprobante.SetAttribute("Serie", c.Serie);
            }

            if (c.Folio != string.Empty)
            {
                nodoComprobante.SetAttribute("Folio", c.Folio);
            }

            nodoComprobante.SetAttribute("Fecha", c.Fecha.ToString("s"));
            nodoComprobante.SetAttribute("Sello", c.Sello);

            if (c.FormaPago != string.Empty)
            {
                nodoComprobante.SetAttribute("FormaPago", c.FormaPago);
            }

            string certificado, NoCertificado;
            ObtenerCertificadoYNoCertificado(rutaCertificado, out certificado, out NoCertificado);

            nodoComprobante.SetAttribute("NoCertificado", NoCertificado);
            nodoComprobante.SetAttribute("Certificado", certificado);

            if (c.CondicionesDePago != string.Empty)
            {
                nodoComprobante.SetAttribute("CondicionesDePago", c.CondicionesDePago);
            }

            nodoComprobante.SetAttribute("SubTotal", c.SubTotal.ToString("F2"));

            if (c.Descuento > 0m)
            {
                nodoComprobante.SetAttribute("Descuento", c.Descuento.ToString("F2"));
            }

            nodoComprobante.SetAttribute("Moneda", c.Moneda);

            if (c.TipoCambio > 0m)
            {
                nodoComprobante.SetAttribute("TipoCambio", c.TipoCambio.ToString("F2"));
            }

            nodoComprobante.SetAttribute("Total", c.Total.ToString("F2"));
            nodoComprobante.SetAttribute("TipoDeComprobante", c.TipoDeComprobante);

            if (c.MetodoPago != string.Empty)
            {
                nodoComprobante.SetAttribute("MetodoPago", c.MetodoPago);
            }

            nodoComprobante.SetAttribute("LugarExpedicion", c.LugarExpedicion);

            if (c.Confirmacion != string.Empty)
            {
                nodoComprobante.SetAttribute("Confirmacion", c.Confirmacion);
            }

            if (c.Exportacion != string.Empty)
            {
                nodoComprobante.SetAttribute("Exportacion", c.Exportacion);
            }

            XmlAttribute schemaLocation = documento.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            schemaLocation.Value = SCHEMALOCATION_CFD;

            nodoComprobante.SetAttribute("xmlns:cfdi", NAMESPACE_CFD);

            if (c.Complemento != null)
            {
                if (c.Complemento.CartaPorte != null)
                {
                    nodoComprobante.SetAttribute("xmlns:cartaporte20", NAMESPACE_CARTA_PORTE);
                    schemaLocation.Value += " " + SCHEMALOCATION_CARTA_PORTE;
                }

                if (c.Complemento.Pagos != null)
                {
                    nodoComprobante.SetAttribute("xmlns:pago20", NAMESPACE_PAGOS);
                    schemaLocation.Value += " " + SCHEMALOCATION_PAGOS;
                }
            }

            nodoComprobante.SetAttributeNode(schemaLocation);
            
            nodoComprobante.AppendChild(AgregarNodoEmisor(c.Emisor, documento.CreateElement("cfdi", "Emisor", NAMESPACE_CFD)));
            nodoComprobante.AppendChild(AgregarNodoReceptor(c.Receptor, documento.CreateElement("cfdi", "Receptor", NAMESPACE_CFD)));
            nodoComprobante.AppendChild(AgregarNodoConceptos(c.Conceptos, documento));

            XmlElement informacionGlobal = AgregarNodoInformacionGlobal(c.InformacionGlobal, documento);
            if (informacionGlobal != null)
            {
                nodoComprobante.AppendChild(informacionGlobal);
            }
            
            XmlElement impuestos = AgregarNodoImpuestos(c.Impuestos, documento);
            if (impuestos != null)
            {
                nodoComprobante.AppendChild(impuestos);
            }

            XmlElement complemento = AgregarNodoComplemento(c.Complemento, documento);
            if (complemento != null)
            {
                nodoComprobante.AppendChild(complemento);
            }

            return nodoComprobante;
        }
        private XmlElement AgregarNodoEmisor(Emisor emisor, XmlElement nEmisor)
        {
            XmlElement nodoEmisor = nEmisor;
            nodoEmisor.SetAttribute("Rfc", emisor.Rfc);

            if (!string.IsNullOrEmpty(emisor.Nombre))
            {
                nodoEmisor.SetAttribute("Nombre", emisor.Nombre);
            }

            nodoEmisor.SetAttribute("RegimenFiscal", emisor.RegimenFiscal);

            if (!string.IsNullOrEmpty(emisor.FacAtrAdquirente))
            {
                nodoEmisor.SetAttribute("FacAtrAdquirente", emisor.FacAtrAdquirente);
            }

            return nodoEmisor;
        }
        private XmlElement AgregarNodoReceptor(Receptor receptor, XmlElement nReceptor)
        {
            XmlElement nodoReceptor = nReceptor;
            nodoReceptor.SetAttribute("Rfc", receptor.Rfc);

            if (!string.IsNullOrEmpty(receptor.Nombre))
            {
                nodoReceptor.SetAttribute("Nombre", receptor.Nombre);
            }

            if (!string.IsNullOrEmpty(receptor.DomicilioFiscalReceptor))
            {
                nodoReceptor.SetAttribute("DomicilioFiscalReceptor", receptor.DomicilioFiscalReceptor);
            }

            if (!string.IsNullOrEmpty(receptor.ResidenciaFiscal))
            {
                nodoReceptor.SetAttribute("ResidenciaFiscal", receptor.ResidenciaFiscal);
            }

            if (!string.IsNullOrEmpty(receptor.NumRegIdTrib))
            {
                nodoReceptor.SetAttribute("NumRegIdTrib", receptor.NumRegIdTrib);
            }

            if (!string.IsNullOrEmpty(receptor.RegimenFiscalReceptor))
            {
                nodoReceptor.SetAttribute("RegimenFiscalReceptor", receptor.RegimenFiscalReceptor);
            }

            if (!string.IsNullOrEmpty(receptor.UsoCFDI))
            {
                nodoReceptor.SetAttribute("UsoCFDI", receptor.UsoCFDI);
            }

            return nodoReceptor;
        }
        private XmlElement AgregarNodoConceptos(Conceptos conceptos, XmlDocument documento)
        {
            if (conceptos.Concepto.Count() == 0) return null;

            XmlElement nodoConceptos = documento.CreateElement("cfdi", "Conceptos", NAMESPACE_CFD);

            foreach (Concepto concepto in conceptos.Concepto)
            {
                XmlElement nodoConcepto = documento.CreateElement("cfdi", "Concepto", NAMESPACE_CFD);

                if (!string.IsNullOrEmpty(concepto.ClaveProdServ))
                {
                    nodoConcepto.SetAttribute("ClaveProdServ", concepto.ClaveProdServ);
                }

                if (!string.IsNullOrEmpty(concepto.NoIdentificacion))
                {
                    nodoConcepto.SetAttribute("NoIdentificacion", concepto.NoIdentificacion);
                }

                nodoConcepto.SetAttribute("Cantidad", concepto.Cantidad.ToString("F2"));
                nodoConcepto.SetAttribute("ClaveUnidad", concepto.ClaveUnidad);

                if (concepto.Unidad != string.Empty)
                {
                    nodoConcepto.SetAttribute("Unidad", concepto.Unidad);
                }

                nodoConcepto.SetAttribute("Descripcion", concepto.Descripcion);
                nodoConcepto.SetAttribute("ValorUnitario", concepto.ValorUnitario.ToString("F6"));
                nodoConcepto.SetAttribute("Importe", concepto.Importe.ToString("F2"));

                if (concepto.Descuento > 0m)
                {
                    nodoConcepto.SetAttribute("Descuento", concepto.Descuento.ToString("F2"));
                }

                if (!string.IsNullOrEmpty(concepto.ObjetoImp))
                {
                    nodoConcepto.SetAttribute("ObjetoImp", concepto.ObjetoImp);
                }

                XmlElement impuestos = ObtenerNodoImpuestosConcepto(concepto.Impuestos, documento);
                if (impuestos != null)
                {
                    nodoConcepto.AppendChild(impuestos);
                }

                foreach (var item in concepto.CuentaPredial)
                {
                    XmlElement cuentaPredial = ObtenerNodoCuentaPredialC(item, documento);
                    if (cuentaPredial != null)
                    {
                        nodoConcepto.AppendChild(cuentaPredial);
                    }
                }
                
                XmlElement informacionAduanera = ObtenerNodoInformacionAduaneraC(concepto.InformacionAduanera, documento);
                if (informacionAduanera != null)
                {
                    nodoConcepto.AppendChild(informacionAduanera);
                }

                XmlElement parte = ObtenerNodoParteC(concepto.Parte, documento);
                if (parte != null)
                {
                    nodoConcepto.AppendChild(parte);
                }

                nodoConceptos.AppendChild(nodoConcepto);
            }

            return nodoConceptos;
        }
        private XmlElement ObtenerNodoImpuestosConcepto(ImpuestosC impuestos, XmlDocument documento)
        {
            if (impuestos == null) return null;
            if (impuestos.Retenciones.Count == 0 && impuestos.Traslados.Count == 0) return null;

            XmlElement nodoImpuestos = documento.CreateElement("cfdi", "Impuestos", NAMESPACE_CFD);

            if (impuestos.Traslados != null && impuestos.Traslados.Count > 0)
            {
                XmlElement nodoTraslados = documento.CreateElement("cfdi", "Traslados", NAMESPACE_CFD);
                foreach (TrasladoC traslado in impuestos.Traslados)
                {
                    XmlElement nodoTraslado = documento.CreateElement("cfdi", "Traslado", NAMESPACE_CFD);
                    nodoTraslado.SetAttribute("Base", traslado.Basee.ToString("F6"));
                    nodoTraslado.SetAttribute("Impuesto", traslado.Impuesto);
                    nodoTraslado.SetAttribute("TipoFactor", traslado.TipoFactor);

                    if (traslado.TasaOCuota > 0m)
                    {
                        nodoTraslado.SetAttribute("TasaOCuota", traslado.TasaOCuota.ToString("F6"));
                    }

                    if (traslado.Importe > 0m)
                    {
                        nodoTraslado.SetAttribute("Importe", traslado.Importe.ToString("F2"));
                    }

                    nodoTraslados.AppendChild(nodoTraslado);
                }

                nodoImpuestos.AppendChild(nodoTraslados);
            }

            if (impuestos.Retenciones != null && impuestos.Retenciones.Count > 0)
            {
                XmlElement nodoRetenciones = documento.CreateElement("cfdi", "Retenciones", NAMESPACE_CFD);

                foreach (RetencionC retencion in impuestos.Retenciones)
                {
                    XmlElement nodoRetencion = documento.CreateElement("cfdi", "Retencion", NAMESPACE_CFD);
                    nodoRetencion.SetAttribute("Base", retencion.Basee.ToString("F6"));
                    nodoRetencion.SetAttribute("Impuesto", retencion.Impuesto);
                    nodoRetencion.SetAttribute("TipoFactor", retencion.TipoFactor);

                    if (retencion.TasaOCuota > 0m)
                    {
                        nodoRetencion.SetAttribute("TasaOCuota", retencion.TasaOCuota.ToString("F6"));
                    }

                    if (retencion.Importe > 0m)
                    {
                        nodoRetencion.SetAttribute("Importe", retencion.Importe.ToString("F2"));
                    }

                    nodoRetenciones.AppendChild(nodoRetencion);
                }

                nodoImpuestos.AppendChild(nodoRetenciones);
            }
            return nodoImpuestos;
        }
        private XmlElement AgregarNodoInformacionGlobal(InformacionGlobal informacionGlobal, XmlDocument documento) 
        {
            if (informacionGlobal == null) return null;

            XmlElement nodoInformacionGlobal = documento.CreateElement("cfdi", "InformacionGlobal", NAMESPACE_CFD);

            if (!string.IsNullOrEmpty(informacionGlobal.Periodicidad))
            {
                nodoInformacionGlobal.SetAttribute("Periodicidad", informacionGlobal.Periodicidad);
            }

            if (!string.IsNullOrEmpty(informacionGlobal.Meses))
            {
                nodoInformacionGlobal.SetAttribute("Meses", informacionGlobal.Meses);
            }

            if (informacionGlobal.Anio > 0)
            {
                nodoInformacionGlobal.SetAttribute("Año", informacionGlobal.Anio.ToString());
            }

            return nodoInformacionGlobal;
        }
        private XmlElement ObtenerNodoInformacionAduaneraC(List<InformacionAduaneraC> informacionAduanera, XmlDocument documento)
        {
            XmlElement nodoInformacionAduanera = documento.CreateElement("cfdi", "InformacionAduanera", NAMESPACE_CFD);

            if (informacionAduanera == null || informacionAduanera.Count() == 0) return null;

            foreach (InformacionAduaneraC ia in informacionAduanera)
            {
                nodoInformacionAduanera.SetAttribute("NumeroPedimento", ia.NumeroPedimento);
                nodoInformacionAduanera.AppendChild(nodoInformacionAduanera);
            }

            return nodoInformacionAduanera;
        }
        private XmlElement ObtenerNodoCuentaPredialC(CuentaPredialC cuentaPredial, XmlDocument documento)
        {
            if (cuentaPredial == null) return null;

            XmlElement nodoCuentaPredialC = documento.CreateElement("cfdi", "CuentaPredial", NAMESPACE_CFD);
            nodoCuentaPredialC.SetAttribute("NumeroPedimento", cuentaPredial.Numero);
            nodoCuentaPredialC.AppendChild(nodoCuentaPredialC);

            return nodoCuentaPredialC;
        }
        private XmlElement ObtenerNodoParteC(List<ParteC> parte, XmlDocument documento)
        {
            if (parte == null || parte.Count() == 0) return null;

            XmlElement nodoParte = documento.CreateElement("cfdi", "Parte", NAMESPACE_CFD);

            foreach (ParteC p in parte)
            {
                nodoParte.SetAttribute("ClaveProdServ", p.ClaveProdServ);

                if (p.NoIdentificacion != string.Empty)
                {
                    nodoParte.SetAttribute("NoIdentificacion", p.NoIdentificacion);
                }

                nodoParte.SetAttribute("Cantidad", p.Cantidad.ToString("F1"));

                if (p.Unidad != string.Empty)
                {
                    nodoParte.SetAttribute("Unidad", p.Unidad);
                }

                nodoParte.SetAttribute("Descripcion", p.Descripcion);
                nodoParte.SetAttribute("ValorUnitario", p.ValorUnitario.ToString("F2"));

                if (p.Importe > 0m)
                {
                    nodoParte.SetAttribute("Importe", p.Importe.ToString("F2"));
                }

                nodoParte.AppendChild(ObtenerNodoInformacionAduaneraC(p.InformacionAduanera, documento));
            }

            return nodoParte;
        }
        private XmlElement AgregarNodoImpuestos(Impuestos impuestos, XmlDocument documento)
        {
            if (impuestos == null) return null;
            if (impuestos.Retenciones.Count == 0 && impuestos.Traslados.Count == 0) return null;

            XmlElement nodoImpuestos = documento.CreateElement("cfdi", "Impuestos", NAMESPACE_CFD);

            //if (impuestos.TotalImpuestosRetenidos != -1)
            //{
            //    nodoImpuestos.SetAttribute("TotalImpuestosRetenidos", impuestos.TotalImpuestosRetenidos.ToString("F2"));
            //}

            //if (impuestos.TotalImpuestosTrasladados != -1)
            //{
            //    nodoImpuestos.SetAttribute("TotalImpuestosTrasladados", impuestos.TotalImpuestosTrasladados.ToString("F2"));
            //}

            if (impuestos.Traslados != null && impuestos.Traslados.Count > 0)
            {
                nodoImpuestos.SetAttribute("TotalImpuestosTrasladados", impuestos.TotalImpuestosTrasladados.ToString("F2"));

                XmlElement nodoTraslados = documento.CreateElement("cfdi", "Traslados", NAMESPACE_CFD);
                foreach (Traslado traslado in impuestos.Traslados)
                {
                    XmlElement nodoTraslado = documento.CreateElement("cfdi", "Traslado", NAMESPACE_CFD);
                    nodoTraslado.SetAttribute("Base", traslado.Basee.ToString("F6"));
                    nodoTraslado.SetAttribute("Impuesto", traslado.Impuesto);
                    nodoTraslado.SetAttribute("TipoFactor", traslado.TipoFactor);

                    if (traslado.TasaOCuota > 0m)
                    {
                        nodoTraslado.SetAttribute("TasaOCuota", traslado.TasaOCuota.ToString("F6"));
                    }

                    if (traslado.Importe > 0m)
                    {
                        nodoTraslado.SetAttribute("Importe", traslado.Importe.ToString("F2"));
                    }

                    nodoTraslados.AppendChild(nodoTraslado);
                }

                nodoImpuestos.AppendChild(nodoTraslados);
            }

            if (impuestos.Retenciones != null && impuestos.Retenciones.Count > 0)
            {
                nodoImpuestos.SetAttribute("TotalImpuestosRetenidos", impuestos.TotalImpuestosRetenidos.ToString("F2"));

                XmlElement nodoRetenciones = documento.CreateElement("cfdi", "Retenciones", NAMESPACE_CFD);

                foreach (Retencion retencion in impuestos.Retenciones)
                {
                    XmlElement nodoRetencion = documento.CreateElement("cfdi", "Retencion", NAMESPACE_CFD);
                    nodoRetencion.SetAttribute("Impuesto", retencion.Impuesto);

                    if (retencion.Importe > 0m)
                    {
                        nodoRetencion.SetAttribute("Importe", retencion.Importe.ToString("F2"));
                    }

                    nodoRetenciones.AppendChild(nodoRetencion);
                }

                nodoImpuestos.AppendChild(nodoRetenciones);
            }

            return nodoImpuestos;
        }
        private XmlElement AgregarNodoComplemento(Complemento complemento, XmlDocument documento)
        {
            if (complemento == null ) return null;
            if (complemento.Pagos == null && complemento.TimbreFiscalDigital == null && complemento.CartaPorte == null) return null;
            
            XmlElement nodoComplemento = documento.CreateElement("cfdi", "Complemento", NAMESPACE_CFD);
            
            XmlElement pagos = AgregarNodoPagos(complemento.Pagos, documento);
            if (pagos != null)
            {
                nodoComplemento.AppendChild(pagos);
            }

            XmlElement cartaPorte20 = AgregarNodoCartaPorte(complemento.CartaPorte, documento);
            if (cartaPorte20 != null)
            {
                nodoComplemento.AppendChild(cartaPorte20);
            }

            return nodoComplemento;
        }

        
        private XmlElement AgregarNodoPagos(Pagos pagos, XmlDocument documento)
        {
            if (pagos == null) return null;

            XmlElement nodoPagos = documento.CreateElement("pago20", "Pagos", NAMESPACE_PAGOS);
            nodoPagos.SetAttribute("Version", pagos.Version);
            nodoPagos.AppendChild(ObteneNodoPTotales(pagos.Totales, documento));

            if (pagos.Pago != null && pagos.Pago.Count() > 0)
            {
                foreach (Pago pg in pagos.Pago)
                {
                    XmlElement nodoPago = documento.CreateElement("pago20", "Pago", NAMESPACE_PAGOS);
                    nodoPago.SetAttribute("FechaPago", pg.FechaPago.ToString("s"));
                    nodoPago.SetAttribute("FormaDePagoP", pg.FormaDePagoP);
                    nodoPago.SetAttribute("MonedaP", pg.MonedaP);

                    if (pg.TipoCambioP > 0m)
                    {
                        nodoPago.SetAttribute("TipoCambioP", pg.TipoCambioP.ToString());//.ToString("F2")
                    }

                    nodoPago.SetAttribute("Monto", pg.Monto.ToString("F2"));

                    if (!string.IsNullOrEmpty(pg.NumOperacion))
                    {
                        nodoPago.SetAttribute("NumOperacion", pg.NumOperacion);
                    }

                    if (!string.IsNullOrEmpty(pg.RfcEmisorCtaOrd))
                    {
                        nodoPago.SetAttribute("RfcEmisorCtaOrd", pg.RfcEmisorCtaOrd);
                    }

                    if (!string.IsNullOrEmpty(pg.NomBancoOrdExt))
                    {
                        nodoPago.SetAttribute("NomBancoOrdExt", pg.NomBancoOrdExt);
                    }

                    if (!string.IsNullOrEmpty(pg.CtaOrdenante))
                    {
                        nodoPago.SetAttribute("CtaOrdenante", pg.CtaOrdenante);
                    }

                    if (!string.IsNullOrEmpty(pg.RfcEmisorCtaBen))
                    {
                        nodoPago.SetAttribute("RfcEmisorCtaBen", pg.RfcEmisorCtaBen);
                    }

                    if (!string.IsNullOrEmpty(pg.CtaBeneficiario))
                    {
                        nodoPago.SetAttribute("CtaBeneficiario", pg.CtaBeneficiario);
                    }

                    if (!string.IsNullOrEmpty(pg.TipoCadPago))
                    {
                        nodoPago.SetAttribute("TipoCadPago", pg.TipoCadPago);
                    }

                    if (!string.IsNullOrEmpty(pg.CertPago))
                    {
                        nodoPago.SetAttribute("CertPago", pg.CertPago);
                    }

                    if (!string.IsNullOrEmpty(pg.CadPago))
                    {
                        nodoPago.SetAttribute("CadPago", pg.CadPago);
                    }

                    if (!string.IsNullOrEmpty(pg.SelloPago))
                    {
                        nodoPago.SetAttribute("SelloPago", pg.SelloPago);
                    }

                    XmlElement doctoRelacionado = ObtenerNodoPDoctoRelacionado(pg.DoctoRelacionado, documento);
                    if (doctoRelacionado != null)
                    {
                        nodoPago.AppendChild(doctoRelacionado);
                    }

                    XmlElement pImpuestos = ObtenerNodoImpuestosP(pg.Impuestos, documento);
                    if (pImpuestos != null)
                    {
                        nodoPago.AppendChild(pImpuestos);
                    }

                    nodoPagos.AppendChild(nodoPago);
                }
            }
            
            return nodoPagos;
        }
        private XmlElement ObteneNodoPTotales(PTotales totales, XmlDocument documento)
        {
            XmlElement nodoTotales = documento.CreateElement("pago20", "Totales", NAMESPACE_PAGOS);

            if (totales.MontoTotalPagos > 0m)
            {
                nodoTotales.SetAttribute("MontoTotalPagos", totales.MontoTotalPagos.ToString("F2"));
            }

            if (totales.TotalRetencionesIEPS > 0m)
            {
                nodoTotales.SetAttribute("TotalRetencionesIEPS", totales.TotalRetencionesIEPS.ToString("F2"));
            }

            if (totales.TotalRetencionesISR > 0m)
            {
                nodoTotales.SetAttribute("TotalRetencionesISR", totales.TotalRetencionesISR.ToString("F2"));
            }

            if (totales.TotalRetencionesIVA > 0m)
            {
                nodoTotales.SetAttribute("TotalRetencionesIVA", totales.TotalRetencionesIVA.ToString("F2"));
            }

            if (totales.TotalTrasladosBaseIVA0 > 0m)
            {
                nodoTotales.SetAttribute("TotalTrasladosBaseIVA0", totales.TotalTrasladosBaseIVA0.ToString("F2"));
            }

            if (totales.TotalTrasladosBaseIVA16 > 0m)
            {
                nodoTotales.SetAttribute("TotalTrasladosBaseIVA16", totales.TotalTrasladosBaseIVA16.ToString("F2"));
            }

            if (totales.TotalTrasladosBaseIVA8 > 0m)
            {
                nodoTotales.SetAttribute("TotalTrasladosBaseIVA8", totales.TotalTrasladosBaseIVA8.ToString("F2"));
            }

            if (totales.TotalTrasladosBaseIVAExento > 0m)
            {
                nodoTotales.SetAttribute("TotalTrasladosBaseIVAExento", totales.TotalTrasladosBaseIVAExento.ToString("F2"));
            }

            if (totales.TotalTrasladosImpuestoIVA0 > 0m)
            {
                nodoTotales.SetAttribute("TotalTrasladosImpuestoIVA0", totales.TotalTrasladosImpuestoIVA0.ToString("F2"));
            }

            if (totales.TotalTrasladosImpuestoIVA16 > 0m)
            {
                nodoTotales.SetAttribute("TotalTrasladosImpuestoIVA16", totales.TotalTrasladosImpuestoIVA16.ToString("F2"));
            }

            if (totales.TotalTrasladosImpuestoIVA8 > 0m)
            {
                nodoTotales.SetAttribute("TotalTrasladosImpuestoIVA8", totales.TotalTrasladosImpuestoIVA8.ToString("F2"));
            }

            return nodoTotales;
        }
        private XmlElement ObtenerNodoPDoctoRelacionado(List<PDoctoRelacionado> doctoRelacionado, XmlDocument documento)
        {
            if (doctoRelacionado == null && doctoRelacionado.Count() == 0) return null;

            XmlElement nodoPDoctoRelacionado = documento.CreateElement("pago20", "DoctoRelacionado", NAMESPACE_PAGOS);
            
            foreach (PDoctoRelacionado dr in doctoRelacionado)
            {
                nodoPDoctoRelacionado.SetAttribute("IdDocumento", dr.IdDocumento);

                if (dr.Serie != string.Empty)
                {
                    nodoPDoctoRelacionado.SetAttribute("Serie", dr.Serie);
                }

                if (dr.Folio != string.Empty)
                {
                    nodoPDoctoRelacionado.SetAttribute("Folio", dr.Folio);
                }

                nodoPDoctoRelacionado.SetAttribute("MonedaDR", dr.MonedaDR);

                if (dr.EquivalenciaDR > 0m)
                {
                    nodoPDoctoRelacionado.SetAttribute("EquivalenciaDR", dr.EquivalenciaDR.ToString());//.ToString("F2")
                }

                if (dr.NumParcialidad != string.Empty)
                {
                    nodoPDoctoRelacionado.SetAttribute("NumParcialidad", dr.NumParcialidad);
                }

                if (dr.ImpSaldoAnt > 0m)
                {
                    nodoPDoctoRelacionado.SetAttribute("ImpSaldoAnt", dr.ImpSaldoAnt.ToString("F2"));
                }

                if (dr.ImpPagado > 0m)
                {
                    nodoPDoctoRelacionado.SetAttribute("ImpPagado", dr.ImpPagado.ToString("F2"));
                }

                if (dr.ImpSaldoInsoluto > 0m || dr.ImpSaldoInsoluto == 0m)
                {
                    nodoPDoctoRelacionado.SetAttribute("ImpSaldoInsoluto", dr.ImpSaldoInsoluto.ToString("F2"));
                }

                if (!string.IsNullOrEmpty(dr.ObjetoImpDR))
                {
                    nodoPDoctoRelacionado.SetAttribute("ObjetoImpDR", dr.ObjetoImpDR);
                }

                XmlElement impuestosDR = ObtenerNodoImpuestosDR(dr.ImpuestosDR, documento);
                if (impuestosDR != null)
                {
                    nodoPDoctoRelacionado.AppendChild(impuestosDR);
                }
            }

            return nodoPDoctoRelacionado;
        }
        private XmlElement ObtenerNodoImpuestosDR(ImpuestosDR impuestosDR, XmlDocument documento)
        {
            if (impuestosDR == null) return null;

            XmlElement nodoPImpuestosDR = documento.CreateElement("pago20", "ImpuestosDR", NAMESPACE_PAGOS);

            if (impuestosDR.RetencionesDR != null)
            {
                XmlElement nodoPRetenciones = documento.CreateElement("pago20", "RetencionesDR", NAMESPACE_PAGOS);
                foreach (RetencionDR retencion in impuestosDR.RetencionesDR.RetencionDR)
                {
                    XmlElement nodoPRetencion = documento.CreateElement("pago20", "RetencionDR", NAMESPACE_PAGOS);
                    nodoPRetencion.SetAttribute("BaseDR", retencion.BaseDR.ToString("F6"));
                    nodoPRetencion.SetAttribute("ImpuestoDR", retencion.ImpuestoDR);
                    nodoPRetencion.SetAttribute("TipoFactorDR", retencion.TipoFactorDR);
                    nodoPRetencion.SetAttribute("TasaOCuota", retencion.TasaOCuotaDR.ToString("F6"));
                    nodoPRetencion.SetAttribute("ImporteDR", retencion.ImporteDR.ToString("F2"));
                    nodoPRetenciones.AppendChild(nodoPRetencion);
                }

                nodoPImpuestosDR.AppendChild(nodoPRetenciones);
            }

            if (impuestosDR.TrasladosDR != null)
            {
                XmlElement nodoPTraslados = documento.CreateElement("pago20", "TrasladosDR", NAMESPACE_CFD);

                foreach (TrasladoDR traslado in impuestosDR.TrasladosDR.TrasladoDR)
                {
                    XmlElement nodoPTraslado = documento.CreateElement("pago20", "TrasladoDR", NAMESPACE_CFD);
                    nodoPTraslado.SetAttribute("BaseDR", traslado.BaseDR.ToString("F6"));
                    nodoPTraslado.SetAttribute("ImpuestoDR", traslado.ImpuestoDR);
                    nodoPTraslado.SetAttribute("TipoFactorDR", traslado.TipoFactorDR);
                    nodoPTraslado.SetAttribute("TasaOCuota", traslado.TasaOCuotaDR.ToString("F6"));
                    nodoPTraslado.SetAttribute("ImporteDR", traslado.ImporteDR.ToString("F2"));
                    nodoPTraslados.AppendChild(nodoPTraslado);
                }

                nodoPImpuestosDR.AppendChild(nodoPTraslados);
            }

            return nodoPImpuestosDR;
        }
        private XmlElement ObtenerNodoImpuestosP(ImpuestosP impuestosP, XmlDocument documento)
        {
            if (impuestosP == null) return null;

            XmlElement nodoPImpuestos = documento.CreateElement("pago20", "ImpuestosP", NAMESPACE_PAGOS);

            if (impuestosP.RetencionesP != null)
            {
                XmlElement nodoPRetenciones = documento.CreateElement("pago20", "RetencionesP", NAMESPACE_PAGOS);

                foreach (RetencionP retencion in impuestosP.RetencionesP.RetencionP)
                {
                    XmlElement nodoPRetencion = documento.CreateElement("pago20", "RetencionP", NAMESPACE_PAGOS);
                    nodoPRetencion.SetAttribute("ImpuestoP", retencion.ImpuestoP);
                    nodoPRetencion.SetAttribute("ImporteP", retencion.ImporteP.ToString("F2"));
                    nodoPRetenciones.AppendChild(nodoPRetencion);
                }

                nodoPImpuestos.AppendChild(nodoPRetenciones);
            }

            if (impuestosP.TrasladosP != null)
            {
                XmlElement nodoPTraslados = documento.CreateElement("pago20", "Traslados", NAMESPACE_CFD);

                foreach (TrasladoP traslado in impuestosP.TrasladosP.TrasladoP)
                {
                    XmlElement nodoPTraslado = documento.CreateElement("pago20", "Traslado", NAMESPACE_CFD);
                    nodoPTraslado.SetAttribute("BaseP", traslado.BaseP.ToString("F6"));
                    nodoPTraslado.SetAttribute("ImpuestoP", traslado.ImpuestoP);
                    nodoPTraslado.SetAttribute("TipoFactorP", traslado.TipoFactorP);
                    nodoPTraslado.SetAttribute("TasaOCuotaP", traslado.TasaOCuotaP.ToString("F6"));
                    nodoPTraslado.SetAttribute("ImporteP", traslado.ImporteP.ToString("F2"));
                    nodoPTraslados.AppendChild(nodoPTraslado);
                }

                nodoPImpuestos.AppendChild(nodoPTraslados);
            }

            return nodoPImpuestos;
        }


        public XmlElement AgregarNodoCartaPorte(CartaPorte cartaPorte, XmlDocument documento)
        {
            if (cartaPorte == null) return null;

            XmlElement nodoCartaPorte = documento.CreateElement("CartaPorte20", "CartaPorte", NAMESPACE_CARTA_PORTE);
            nodoCartaPorte.SetAttribute("Version", cartaPorte.Version);
            nodoCartaPorte.SetAttribute("TranspInternac", cartaPorte.Version);

            if (string.IsNullOrEmpty(cartaPorte.EntradaSalidaMerc))
            {
                nodoCartaPorte.SetAttribute("EntradaSalidaMerc", cartaPorte.EntradaSalidaMerc);
            }

            if (string.IsNullOrEmpty(cartaPorte.PaisOrigenDestino))
            {
                nodoCartaPorte.SetAttribute("PaisOrigenDestino", cartaPorte.PaisOrigenDestino);
            }

            if (string.IsNullOrEmpty(cartaPorte.PaisOrigenDestino))
            {
                nodoCartaPorte.SetAttribute("ViaEntradaSalida", cartaPorte.ViaEntradaSalida);
            }

            if (string.IsNullOrEmpty(cartaPorte.PaisOrigenDestino))
            {
                nodoCartaPorte.SetAttribute("TotalDistRec", cartaPorte.TotalDistRec.ToString("f2"));
            }

            XmlElement ubicaciones = AgregarNodoUbicaciones(cartaPorte.Ubicaciones, documento);
            if (ubicaciones != null)
            {
                nodoCartaPorte.AppendChild(ubicaciones);
            }

            XmlElement mercancias = AgregarNodoMercancias(cartaPorte.Mercancias, documento);
            if (mercancias != null)
            {
                nodoCartaPorte.AppendChild(mercancias);
            }

            XmlElement figuraTransporte = AgregarNodoFiguraTransporte(cartaPorte.FiguraTransporte, documento);
            if (figuraTransporte != null)
            {
                nodoCartaPorte.AppendChild(figuraTransporte);
            }

            return nodoCartaPorte;
        }
        private XmlElement AgregarNodoUbicaciones(Ubicaciones ubicaciones, XmlDocument documento)
        {
            if (ubicaciones == null) return null;

            XmlElement nodoUbicaciones = documento.CreateElement("cartaporte", "Ubicaciones", NAMESPACE_CARTA_PORTE);

            foreach (Ubicacion ubicacion in ubicaciones.Ubicacion)
            {
                XmlElement nodoUbicacion = documento.CreateElement("cartaporte", "Ubicacion", NAMESPACE_CARTA_PORTE);

                XmlElement nodoDomicilio = AgregarNodoDomicilio(ubicacion.Domicilio, documento);
                if (nodoDomicilio != null)
                {
                    nodoUbicacion.AppendChild(nodoDomicilio);
                }

                nodoUbicacion.SetAttribute("TipoUbicacion", ubicacion.TipoUbicacion);

                if (!string.IsNullOrEmpty(ubicacion.IDUbicacion))
                {
                    nodoUbicacion.SetAttribute("IDUbicacion", ubicacion.IDUbicacion);
                }

                nodoUbicacion.SetAttribute("RFCRemitenteDestinatario", ubicacion.RFCRemitenteDestinatario);

                if (!string.IsNullOrEmpty(ubicacion.NombreRemitenteDestinatario))
                {
                    nodoUbicacion.SetAttribute("NombreRemitenteDestinatario", ubicacion.NombreRemitenteDestinatario);
                }

                if (!string.IsNullOrEmpty(ubicacion.NumRegIdTrib))
                {
                    nodoUbicacion.SetAttribute("NumRegIdTrib", ubicacion.NumRegIdTrib);
                }

                if (!string.IsNullOrEmpty(ubicacion.ResidenciaFiscal))
                {
                    nodoUbicacion.SetAttribute("ResidenciaFiscal", ubicacion.ResidenciaFiscal);
                }

                if (!string.IsNullOrEmpty(ubicacion.NumEstacion))
                {
                    nodoUbicacion.SetAttribute("NumEstacion", ubicacion.NumEstacion);
                }

                if (!string.IsNullOrEmpty(ubicacion.NombreEstacion))
                {
                    nodoUbicacion.SetAttribute("NombreEstacion", ubicacion.NombreEstacion);
                }

                if (!string.IsNullOrEmpty(ubicacion.NavegacionTrafico))
                {
                    nodoUbicacion.SetAttribute("NavegacionTrafico", ubicacion.NavegacionTrafico);
                }

                nodoUbicacion.SetAttribute("FechaHoraSalidaLlegada", ubicacion.FechaHoraSalidaLlegada.ToShortDateString());

                if (!string.IsNullOrEmpty(ubicacion.TipoEstacion))
                {
                    nodoUbicacion.SetAttribute("TipoEstacion", ubicacion.TipoEstacion);
                }

                if (ubicacion.DistanciaRecorrida > 0m)
                {
                    nodoUbicacion.SetAttribute("DistanciaRecorrida", ubicacion.DistanciaRecorrida.ToString());
                }

                nodoUbicaciones.AppendChild(nodoUbicacion);
            }

            return nodoUbicaciones;
        }
        private XmlElement AgregarNodoDomicilio(Domicilio domicilio, XmlDocument documento)
        {
            if (domicilio == null) return null;

            XmlElement nodoDomicilio = documento.CreateElement("cartaporte", "Domicilio", NAMESPACE_CARTA_PORTE);

            if (string.IsNullOrEmpty(domicilio.Calle))
            {
                nodoDomicilio.SetAttribute("Calle", domicilio.Calle);
            }
            if (string.IsNullOrEmpty(domicilio.NumeroExterior))
            {
                nodoDomicilio.SetAttribute("NumeroExterior", domicilio.NumeroExterior);
            }
            if (string.IsNullOrEmpty(domicilio.NumeroInterior))
            {
                nodoDomicilio.SetAttribute("NumeroInterior", domicilio.NumeroInterior);
            }
            if (string.IsNullOrEmpty(domicilio.Colonia))
            {
                nodoDomicilio.SetAttribute("Colonia", domicilio.Colonia);
            }
            if (string.IsNullOrEmpty(domicilio.Localidad))
            {
                nodoDomicilio.SetAttribute("Localidad", domicilio.Localidad);
            }
            if (string.IsNullOrEmpty(domicilio.Referencia))
            {
                nodoDomicilio.SetAttribute("Referencia", domicilio.Referencia);
            }
            if (string.IsNullOrEmpty(domicilio.Municipio))
            {
                nodoDomicilio.SetAttribute("Municipio", domicilio.Municipio);
            }
            if (string.IsNullOrEmpty(domicilio.Estado))
            {
                nodoDomicilio.SetAttribute("Estado", domicilio.Estado);
            }
            if (string.IsNullOrEmpty(domicilio.Pais))
            {
                nodoDomicilio.SetAttribute("Pais", domicilio.Pais);
            }
            if (string.IsNullOrEmpty(domicilio.CodigoPostal))
            {
                nodoDomicilio.SetAttribute("CodigoPostal", domicilio.CodigoPostal);
            }

            return nodoDomicilio;
        }
        private XmlElement AgregarNodoMercancias(Mercancias mercancias, XmlDocument documento)
        {
            if (mercancias == null) return null;

            XmlElement nodoMercancias = documento.CreateElement("cartaporte", "Mercancias", NAMESPACE_CARTA_PORTE);

            foreach (var mercancia in mercancias.Mercancia)
            {
                XmlElement nodoMercancia = AgregarNodoMercancia(mercancia, documento);
                if (nodoMercancia != null)
                {
                    nodoMercancias.AppendChild(nodoMercancia);
                }
            }

            XmlElement nodoAutotransporte = AgregarNodoAutotransporte(mercancias.Autotransporte, documento);
            XmlElement nodoTransporteMaritimo = AgregarNodoTransporteMaritimo(mercancias.TransporteMaritimo, documento);
            XmlElement nodoTransporteAereo = AgregarNodoTransporteAereo(mercancias.TransporteAereo, documento);
            XmlElement nodoTransporteFerroviario = AgregarNodoTransporteFerroviario(mercancias.TransporteFerroviario, documento);

            if (nodoAutotransporte != null)
            {
                nodoMercancias.AppendChild(nodoAutotransporte);
            }
            if (nodoTransporteMaritimo != null)
            {
                nodoMercancias.AppendChild(nodoTransporteMaritimo);
            }
            if (nodoTransporteAereo != null)
            {
                nodoMercancias.AppendChild(nodoTransporteAereo);
            }
            if (nodoTransporteFerroviario != null)
            {
                nodoMercancias.AppendChild(nodoTransporteFerroviario);
            }

            nodoMercancias.SetAttribute("PesoBrutoTotal", mercancias.PesoBrutoTotal.ToString("F2"));
            nodoMercancias.SetAttribute("UnidadPeso", mercancias.UnidadPeso);

            if (mercancias.PesoNetoTotal > 0m)
            {
                nodoMercancias.SetAttribute("PesoNetoTotal", mercancias.PesoNetoTotal.ToString("F2"));
            }
            if (mercancias.NumTotalMercancias > 0)
            {
                nodoMercancias.SetAttribute("NumTotalMercancias", mercancias.NumTotalMercancias.ToString());
            }
            if (mercancias.CargoPorTasacion > 0m)
            {
                nodoMercancias.SetAttribute("CargoPorTasacion", mercancias.CargoPorTasacion.ToString("F2"));
            }

            return nodoMercancias;

        }
        private XmlElement AgregarNodoMercancia(Mercancia mercancia, XmlDocument documento)
        {
            if (mercancia == null) return null;

            XmlElement nodoMercancia = documento.CreateElement("cartaporte", "Mercancia", NAMESPACE_CARTA_PORTE);

            foreach (var pedimento in mercancia.Pedimentos)
            {
                XmlElement nodoPedimentos = AgregarNodoPedimentos(pedimento, documento);
                if (nodoPedimentos != null)
                {
                    nodoMercancia.AppendChild(nodoPedimentos);
                }
            }

            foreach (var guiasIdentificacion in mercancia.GuiasIdentificacion)
            {
                XmlElement nodoGuiasIdentificacion = AgregarNodoGuiasIdentificacion(guiasIdentificacion, documento);
                if (nodoGuiasIdentificacion != null)
                {
                    nodoMercancia.AppendChild(nodoGuiasIdentificacion);
                }
            }

            foreach (var cantidadTransporta in mercancia.CantidadTransporta)
            {
                XmlElement nodoCantidadTransporta = AgregarNodoCantidadTransporta(cantidadTransporta, documento);
                if (nodoCantidadTransporta != null)
                {
                    nodoMercancia.AppendChild(nodoCantidadTransporta);
                }
            }

            XmlElement nodoDetalleMercancia = AgregarNodoDetalleMercancia(mercancia.DetalleMercancia, documento);
            //nodoMercancia.AppendChild(nodoDetalleMercancia); ????????????????????????? lo lleva ??

            nodoMercancia.SetAttribute("BienesTransp", mercancia.BienesTransp);

            if (!string.IsNullOrEmpty(mercancia.ClaveSTCC))
            {
                nodoMercancia.SetAttribute("ClaveSTCC", mercancia.ClaveSTCC);
            }
            if (!string.IsNullOrEmpty(mercancia.Descripcion))
            {
                nodoMercancia.SetAttribute("Descripcion", mercancia.Descripcion);
            }
            if (mercancia.Cantidad > 0m)
            {
                nodoMercancia.SetAttribute("Cantidad", mercancia.Cantidad.ToString("F2"));
            }
            if (!string.IsNullOrEmpty(mercancia.ClaveUnidad))
            {
                nodoMercancia.SetAttribute("ClaveUnidad", mercancia.ClaveUnidad);
            }
            if (!string.IsNullOrEmpty(mercancia.Unidad))
            {
                nodoMercancia.SetAttribute("Unidad", mercancia.Unidad);
            }

            if (!string.IsNullOrEmpty(mercancia.Dimensiones))
            {
                nodoMercancia.SetAttribute("Dimensiones", mercancia.Dimensiones);
            }
            if (!string.IsNullOrEmpty(mercancia.MaterialPeligroso))
            {
                nodoMercancia.SetAttribute("MaterialPeligroso", mercancia.MaterialPeligroso);
            }
            if (!string.IsNullOrEmpty(mercancia.CveMaterialPeligroso))
            {
                nodoMercancia.SetAttribute("CveMaterialPeligroso", mercancia.CveMaterialPeligroso);
            }
            if (!string.IsNullOrEmpty(mercancia.Embalaje))
            {
                nodoMercancia.SetAttribute("Embalaje", mercancia.Embalaje);
            }
            if (!string.IsNullOrEmpty(mercancia.DescripEmbalaje))
            {
                nodoMercancia.SetAttribute("DescripEmbalaje", mercancia.DescripEmbalaje);
            }
            if (mercancia.PesoEnKg > 0m)
            {
                nodoMercancia.SetAttribute("PesoEnKg", mercancia.PesoEnKg.ToString("F3"));
            }
            if (mercancia.ValorMercancia > 0m)
            {
                nodoMercancia.SetAttribute("ValorMercancia", mercancia.ValorMercancia.ToString("F2"));
            }
            if (!string.IsNullOrEmpty(mercancia.Moneda))
            {
                nodoMercancia.SetAttribute("Moneda", mercancia.Moneda);
            }
            if (!string.IsNullOrEmpty(mercancia.FraccionArancelaria))
            {
                nodoMercancia.SetAttribute("FraccionArancelaria", mercancia.FraccionArancelaria);
            }
            if (!string.IsNullOrEmpty(mercancia.UUIDComercioExt))
            {
                nodoMercancia.SetAttribute("UUIDComercioExt", mercancia.UUIDComercioExt);
            }

            return nodoMercancia;
        }
        private XmlElement AgregarNodoPedimentos(Pedimentos pedimentos, XmlDocument documento)
        {
            if (pedimentos == null) return null;

            XmlElement nodoPedimentos = documento.CreateElement("cartaporte", "Pedimentos", NAMESPACE_CARTA_PORTE);
            nodoPedimentos.SetAttribute("Pedimento", pedimentos.Pedimento);

            return nodoPedimentos;
        }
        private XmlElement AgregarNodoGuiasIdentificacion(GuiasIdentificacion guiasIdentificacion, XmlDocument documento)
        {
            if (guiasIdentificacion == null) return null;

            XmlElement nodoGuiasIdentificacion = documento.CreateElement("cartaporte", "GuiasIdentificacion", NAMESPACE_CARTA_PORTE);
            nodoGuiasIdentificacion.SetAttribute("NumeroGuiaIdentificacion", guiasIdentificacion.NumeroGuiaIdentificacion);
            nodoGuiasIdentificacion.SetAttribute("DescripGuiaIdentificacion", guiasIdentificacion.DescripGuiaIdentificacion);
            nodoGuiasIdentificacion.SetAttribute("PesoGuiaIdentificacion", guiasIdentificacion.PesoGuiaIdentificacion.ToString("F3"));

            return nodoGuiasIdentificacion;
        }
        private XmlElement AgregarNodoCantidadTransporta(CantidadTransporta cantidadTransporta, XmlDocument documento)
        {
            if (cantidadTransporta == null) return null;

            XmlElement nodocantidadTransporta = documento.CreateElement("cartaporte", "CantidadTransporta", NAMESPACE_CARTA_PORTE);

            if (cantidadTransporta.Cantidad > 0m)
            {
                nodocantidadTransporta.SetAttribute("NumeroGuiaIdentificacion", cantidadTransporta.Cantidad.ToString("F6"));
            }
            if (string.IsNullOrEmpty(cantidadTransporta.IDOrigen))
            {
                nodocantidadTransporta.SetAttribute("IDOrigen", cantidadTransporta.IDOrigen);
            }
            if (string.IsNullOrEmpty(cantidadTransporta.IDDestino))
            {
                nodocantidadTransporta.SetAttribute("IDDestino", cantidadTransporta.IDDestino);
            }

            return nodocantidadTransporta;
        }
        private XmlElement AgregarNodoDetalleMercancia(DetalleMercancia detalleMercancia, XmlDocument documento)
        {
            if (detalleMercancia == null) return null;

            XmlElement nodoDetalleMercancia = documento.CreateElement("cartaporte", "DetalleMercancia", NAMESPACE_CARTA_PORTE);

            nodoDetalleMercancia.SetAttribute("UnidadPesoMerc", detalleMercancia.UnidadPesoMerc);
            nodoDetalleMercancia.SetAttribute("PesoBruto", detalleMercancia.PesoBruto.ToString("F3"));
            nodoDetalleMercancia.SetAttribute("PesoNeto", detalleMercancia.PesoNeto.ToString("F3"));
            nodoDetalleMercancia.SetAttribute("PesoTara", detalleMercancia.PesoNeto.ToString("F3"));

            if (detalleMercancia.NumPiezas > 0)
            {
                nodoDetalleMercancia.SetAttribute("NumPiezas", detalleMercancia.NumPiezas.ToString());
            }

            return nodoDetalleMercancia;
        }
        private XmlElement AgregarNodoAutotransporte(Autotransporte autotransporte, XmlDocument documento)
        {
            if (autotransporte == null) return null;

            XmlElement nodoAutoTranporte = documento.CreateElement("cartaporte", "Autotransporte", NAMESPACE_CARTA_PORTE);
            XmlElement nodoIdentificacionVehicular = AgregarNodoIdentificacionVehicular(autotransporte.IdentificacionVehicular, documento);
            XmlElement nodoSeguros = AgregarNodoSeguros(autotransporte.Seguros, documento);
            XmlElement nodoRemolques = AgregarNodoRemolques(autotransporte.Remolques, documento);

            if (nodoIdentificacionVehicular != null)
            {
                nodoAutoTranporte.AppendChild(nodoIdentificacionVehicular);
            }
            if (nodoSeguros != null)
            {
                nodoAutoTranporte.AppendChild(nodoSeguros);
            }
            if (nodoRemolques != null)
            {
                nodoAutoTranporte.AppendChild(nodoRemolques);
            }

            nodoAutoTranporte.SetAttribute("PermSCT", autotransporte.PermSCT);
            nodoAutoTranporte.SetAttribute("NumPermisoSCT", autotransporte.NumPermisoSCT);

            return nodoAutoTranporte;
        }
        private XmlElement AgregarNodoIdentificacionVehicular(IdentificacionVehicular identificacionVehicular, XmlDocument documento)
        {
            if (identificacionVehicular == null) return null;

            XmlElement nodoIdentificacionVehicular = documento.CreateElement("cartaporte", "IdentificacionVehicular", NAMESPACE_CARTA_PORTE);
            nodoIdentificacionVehicular.SetAttribute("ConfigVehicular", identificacionVehicular.ConfigVehicular);
            nodoIdentificacionVehicular.SetAttribute("PlacaVM", identificacionVehicular.PlacaVM);
            nodoIdentificacionVehicular.SetAttribute("AnioModeloVM", identificacionVehicular.AnioModeloVM.ToString());

            return nodoIdentificacionVehicular;
        }
        private XmlElement AgregarNodoSeguros(Seguros seguros, XmlDocument documento)
        {
            if (seguros == null) return null;

            XmlElement nodoSeguros = documento.CreateElement("cartaporte", "Seguros", NAMESPACE_CARTA_PORTE);

            nodoSeguros.SetAttribute("AseguraRespCivil", seguros.AseguraRespCivil);
            nodoSeguros.SetAttribute("PolizaRespCivil", seguros.PolizaRespCivil);

            if (string.IsNullOrEmpty(seguros.AseguraMedAmbiente))
            {
                nodoSeguros.SetAttribute("AseguraMedAmbiente", seguros.AseguraMedAmbiente);
            }
            if (string.IsNullOrEmpty(seguros.PolizaMedAmbiente))
            {
                nodoSeguros.SetAttribute("PolizaMedAmbiente", seguros.PolizaMedAmbiente);
            }
            if (string.IsNullOrEmpty(seguros.AseguraCarga))
            {
                nodoSeguros.SetAttribute("AseguraCarga", seguros.AseguraCarga);
            }
            if (string.IsNullOrEmpty(seguros.PolizaCarga))
            {
                nodoSeguros.SetAttribute("PolizaCarga", seguros.PolizaCarga);
            }
            if (seguros.PrimaSeguro > 0m)
            {
                nodoSeguros.SetAttribute("PrimaSeguro", seguros.PrimaSeguro.ToString("F2"));
            }

            return nodoSeguros;
        }
        private XmlElement AgregarNodoRemolques(Remolques remolques, XmlDocument documento)
        {
            if (remolques == null) return null;

            XmlElement nodoRemolques = documento.CreateElement("cartaporte", "Remolques", NAMESPACE_CARTA_PORTE);

            foreach (var remolque in remolques.Remolque)
            {
                XmlElement nodoRemolque = AgregarNodoRemolque(remolque, documento);
                if (nodoRemolque != null)
                {
                    nodoRemolques.AppendChild(nodoRemolque);
                }
            }

            return nodoRemolques;
        }
        private XmlElement AgregarNodoRemolque(Remolque remolque, XmlDocument documento)
        {
            if (remolque == null) return null;

            XmlElement nodoRemolque = documento.CreateElement("cartaporte", "Remolque", NAMESPACE_CARTA_PORTE);
            nodoRemolque.SetAttribute("SubTipoRem", remolque.SubTipoRem);
            nodoRemolque.SetAttribute("Placa", remolque.Placa);

            return nodoRemolque;
        }
        private XmlElement AgregarNodoTransporteMaritimo(TransporteMaritimo transporteMaritimo, XmlDocument documento)
        {
            if (transporteMaritimo == null) return null;

            XmlElement nodoTransporteMaritimo = documento.CreateElement("cartaporte", "TransporteMaritimo", NAMESPACE_CARTA_PORTE);

            foreach (var contenedor in transporteMaritimo.Contenedor)
            {
                XmlElement nodoContenedor = AgregarNodoContenedor(contenedor, documento);
                if (nodoContenedor != null)
                {
                    nodoContenedor.AppendChild(nodoContenedor);
                }
            }

            if (string.IsNullOrEmpty(transporteMaritimo.PermSCT))
            {
                nodoTransporteMaritimo.SetAttribute("PermSCT", transporteMaritimo.PermSCT);
            }
            if (string.IsNullOrEmpty(transporteMaritimo.NumPermisoSCT))
            {
                nodoTransporteMaritimo.SetAttribute("NumPermisoSCT", transporteMaritimo.NumPermisoSCT);
            }
            if (string.IsNullOrEmpty(transporteMaritimo.NombreAseg))
            {
                nodoTransporteMaritimo.SetAttribute("NombreAseg", transporteMaritimo.NombreAseg);
            }
            if (string.IsNullOrEmpty(transporteMaritimo.NumPolizaSeguro))
            {
                nodoTransporteMaritimo.SetAttribute("NumPolizaSeguro", transporteMaritimo.NumPolizaSeguro);
            }
            if (string.IsNullOrEmpty(transporteMaritimo.TipoEmbarcacion))
            {
                nodoTransporteMaritimo.SetAttribute("TipoEmbarcacion", transporteMaritimo.TipoEmbarcacion);
            }
            if (string.IsNullOrEmpty(transporteMaritimo.Matricula))
            {
                nodoTransporteMaritimo.SetAttribute("Matricula", transporteMaritimo.Matricula);
            }
            if (string.IsNullOrEmpty(transporteMaritimo.NumeroOMI))
            {
                nodoTransporteMaritimo.SetAttribute("NumeroOMI", transporteMaritimo.NumeroOMI);
            }
            if (transporteMaritimo.AnioEmbarcacion > 0)
            {
                nodoTransporteMaritimo.SetAttribute("AnioEmbarcacion", transporteMaritimo.AnioEmbarcacion.ToString());
            }
            if (string.IsNullOrEmpty(transporteMaritimo.NombreEmbarc))
            {
                nodoTransporteMaritimo.SetAttribute("NombreEmbarc", transporteMaritimo.NombreEmbarc);
            }
            if (string.IsNullOrEmpty(transporteMaritimo.NacionalidadEmbarc))
            {
                nodoTransporteMaritimo.SetAttribute("NacionalidadEmbarc", transporteMaritimo.NacionalidadEmbarc);
            }

            nodoTransporteMaritimo.SetAttribute("UnidadesDeArqBruto", transporteMaritimo.UnidadesDeArqBruto.ToString("F3"));

            if (string.IsNullOrEmpty(transporteMaritimo.TipoCarga))
            {
                nodoTransporteMaritimo.SetAttribute("TipoCarga", transporteMaritimo.TipoCarga);
            }
            if (string.IsNullOrEmpty(transporteMaritimo.NumCertITC))
            {
                nodoTransporteMaritimo.SetAttribute("NumCertITC", transporteMaritimo.NumCertITC);
            }
            if (transporteMaritimo.Eslora > 0m)
            {
                nodoTransporteMaritimo.SetAttribute("Eslora", transporteMaritimo.Eslora.ToString());
            }
            if (transporteMaritimo.Manga > 0m)
            {
                nodoTransporteMaritimo.SetAttribute("Manga", transporteMaritimo.Manga.ToString());
            }
            if (transporteMaritimo.Calado > 0m)
            {
                nodoTransporteMaritimo.SetAttribute("Calado", transporteMaritimo.Calado.ToString());
            }
            if (string.IsNullOrEmpty(transporteMaritimo.LineaNaviera))
            {
                nodoTransporteMaritimo.SetAttribute("LineaNaviera", transporteMaritimo.LineaNaviera);
            }

            nodoTransporteMaritimo.SetAttribute("NombreAgenteNaviero", transporteMaritimo.NombreAgenteNaviero);
            nodoTransporteMaritimo.SetAttribute("NumAutorizacionNaviero", transporteMaritimo.NumAutorizacionNaviero);

            if (string.IsNullOrEmpty(transporteMaritimo.NumViaje))
            {
                nodoTransporteMaritimo.SetAttribute("NumViaje", transporteMaritimo.NumViaje);
            }
            if (string.IsNullOrEmpty(transporteMaritimo.NumConocEmbarc))
            {
                nodoTransporteMaritimo.SetAttribute("NumConocEmbarc", transporteMaritimo.NumConocEmbarc);
            }

            return nodoTransporteMaritimo;
        }
        private XmlElement AgregarNodoContenedor(Contenedor contenedor, XmlDocument documento)
        {
            if (contenedor == null) return null;

            XmlElement nodoContenedor = documento.CreateElement("cartaporte", "Contenedor", NAMESPACE_CARTA_PORTE);
            nodoContenedor.SetAttribute("MatriculaContenedor", contenedor.MatriculaContenedor);
            nodoContenedor.SetAttribute("TipoContenedor", contenedor.TipoContenedor);

            if (!string.IsNullOrEmpty(contenedor.NumPrecinto))
            {
                nodoContenedor.SetAttribute("NumPrecinto", contenedor.NumPrecinto);
            }

            return nodoContenedor;
        }
        private XmlElement AgregarNodoTransporteAereo(TransporteAereo transporteAereo, XmlDocument documento)
        {
            if (transporteAereo == null) return null;

            XmlElement nodoTransporteAereo = documento.CreateElement("cartaporte", "TransporteAereo", NAMESPACE_CARTA_PORTE);

            if (string.IsNullOrEmpty(transporteAereo.PermSCT))
            {
                nodoTransporteAereo.SetAttribute("PermSCT", transporteAereo.PermSCT);
            }
            if (string.IsNullOrEmpty(transporteAereo.NumPermisoSCT))
            {
                nodoTransporteAereo.SetAttribute("NumPermisoSCT", transporteAereo.NumPermisoSCT);
            }
            if (string.IsNullOrEmpty(transporteAereo.MatriculaAeronave))
            {
                nodoTransporteAereo.SetAttribute("MatriculaAeronave", transporteAereo.MatriculaAeronave);
            }
            if (string.IsNullOrEmpty(transporteAereo.NombreAseg))
            {
                nodoTransporteAereo.SetAttribute("NombreAseg", transporteAereo.NombreAseg);
            }
            if (string.IsNullOrEmpty(transporteAereo.NumPolizaSeguro))
            {
                nodoTransporteAereo.SetAttribute("NumPolizaSeguro", transporteAereo.NumPolizaSeguro);
            }
            if (string.IsNullOrEmpty(transporteAereo.NumeroGuia))
            {
                nodoTransporteAereo.SetAttribute("NumeroGuia", transporteAereo.NumeroGuia);
            }
            if (string.IsNullOrEmpty(transporteAereo.LugarContrato))
            {
                nodoTransporteAereo.SetAttribute("LugarContrato", transporteAereo.LugarContrato);
            }
            if (string.IsNullOrEmpty(transporteAereo.CodigoTransportista))
            {
                nodoTransporteAereo.SetAttribute("CodigoTransportista", transporteAereo.CodigoTransportista);
            }
            if (string.IsNullOrEmpty(transporteAereo.RFCEmbarcador))
            {
                nodoTransporteAereo.SetAttribute("RFCEmbarcador", transporteAereo.RFCEmbarcador);
            }
            if (string.IsNullOrEmpty(transporteAereo.NumRegIdTribEmbarc))
            {
                nodoTransporteAereo.SetAttribute("NumRegIdTribEmbarc", transporteAereo.NumRegIdTribEmbarc);
            }
            if (string.IsNullOrEmpty(transporteAereo.ResidenciaFiscalEmbarc))
            {
                nodoTransporteAereo.SetAttribute("ResidenciaFiscalEmbarc", transporteAereo.ResidenciaFiscalEmbarc);
            }
            if (string.IsNullOrEmpty(transporteAereo.NombreEmbarcador))
            {
                nodoTransporteAereo.SetAttribute("NombreEmbarcador", transporteAereo.NombreEmbarcador);
            }

            return nodoTransporteAereo;
        }
        private XmlElement AgregarNodoTransporteFerroviario(TransporteFerroviario transporteFerroviario, XmlDocument documento)
        {
            if (transporteFerroviario == null) return null;

            XmlElement nodoTransporteFerroviario = documento.CreateElement("cartaporte", "TransporteFerroviario", NAMESPACE_CARTA_PORTE);

            foreach (var derechosDePaso in transporteFerroviario.DerechosDePaso)
            {
                XmlElement nodoDerechosDePaso = AgregarNodoDerechosDePaso(derechosDePaso, documento);
                if (nodoDerechosDePaso != null)
                {
                    nodoTransporteFerroviario.AppendChild(nodoDerechosDePaso);
                }
            }

            foreach (var carro in transporteFerroviario.Carro)
            {
                XmlElement nodoCarro = AgregarNodoCarro(carro, documento);
                if (nodoCarro != null)
                {
                    nodoTransporteFerroviario.AppendChild(nodoCarro);
                }
            }

            nodoTransporteFerroviario.SetAttribute("TipoDeServicio", transporteFerroviario.TipoDeServicio);
            nodoTransporteFerroviario.SetAttribute("TipoDeTrafico", transporteFerroviario.TipoDeTrafico);

            if (!string.IsNullOrEmpty(transporteFerroviario.NombreAseg))
            {
                nodoTransporteFerroviario.SetAttribute("NombreAseg", transporteFerroviario.NombreAseg);
            }
            if (!string.IsNullOrEmpty(transporteFerroviario.NumPolizaSeguro))
            {
                nodoTransporteFerroviario.SetAttribute("NumPolizaSeguro", transporteFerroviario.NumPolizaSeguro);
            }

            return nodoTransporteFerroviario;
        }
        private XmlElement AgregarNodoDerechosDePaso(DerechosDePaso derechosDePaso, XmlDocument documento)
        {
            if (derechosDePaso == null) return null;

            XmlElement nodoDerechosDePaso = documento.CreateElement("cartaporte", "DerechosDePaso", NAMESPACE_CARTA_PORTE);
            nodoDerechosDePaso.SetAttribute("TipoDerechoDePaso", derechosDePaso.TipoDerechoDePaso);
            nodoDerechosDePaso.SetAttribute("KilometrajePagado", derechosDePaso.KilometrajePagado.ToString("F2"));

            return nodoDerechosDePaso;
        }
        private XmlElement AgregarNodoCarro(Carro carro, XmlDocument documento)
        {
            if (carro == null) return null;

            XmlElement nodoCarro = documento.CreateElement("cartaporte", "Carro", NAMESPACE_CARTA_PORTE);

            foreach (var contenedor in carro.Contenedor)
            {
                XmlElement nodoContenedor = AgregarNodoContenedor(contenedor, documento);
                if (nodoContenedor != null)
                {
                    nodoCarro.AppendChild(nodoContenedor);
                }
            }

            nodoCarro.SetAttribute("TipoCarro", carro.TipoCarro);
            nodoCarro.SetAttribute("MatriculaCarro", carro.MatriculaCarro);
            nodoCarro.SetAttribute("GuiaCarro", carro.GuiaCarro);
            nodoCarro.SetAttribute("ToneladasNetasCarro", carro.ToneladasNetasCarro.ToString("F3"));

            return nodoCarro;
        }
        private XmlElement AgregarNodoContenedor(CarroContenedor contenedor, XmlDocument documento)
        {
            if (contenedor == null) return null;

            XmlElement nodoContenedor = documento.CreateElement("cartaporte", "Contenedor", NAMESPACE_CARTA_PORTE);
            nodoContenedor.SetAttribute("TipoContenedor", contenedor.TipoContenedor);
            nodoContenedor.SetAttribute("PesoContenedorVacio", contenedor.PesoContenedorVacio.ToString("F3"));
            nodoContenedor.SetAttribute("PesoNetoMercancia", contenedor.PesoNetoMercancia.ToString("F3"));

            return nodoContenedor;
        }
        private XmlElement AgregarNodoFiguraTransporte(FiguraTransporte figuraTransporte, XmlDocument documento)
        {
            if (figuraTransporte == null) return null;

            XmlElement nodoFiguraTransporte = documento.CreateElement("cartaporte", "FiguraTransporte", NAMESPACE_CARTA_PORTE);

            foreach (var tiposFigura in figuraTransporte.TiposFigura)
            {
                XmlElement nodoPartesTransporte = AgregarNodoTiposFigura(tiposFigura, documento);
                if (nodoPartesTransporte != null)
                {
                    nodoFiguraTransporte.AppendChild(nodoPartesTransporte);
                }
            }

            return nodoFiguraTransporte;
        }
        private XmlElement AgregarNodoTiposFigura(TiposFigura tiposFigura, XmlDocument documento)
        {
            if (tiposFigura == null) return null;

            XmlElement nodoTiposFigura = documento.CreateElement("cartaporte", "TiposFigura", NAMESPACE_CARTA_PORTE);

            foreach (var tipoFigura in tiposFigura.PartesTransporte)
            {
                XmlElement nodoPartesTransporte = AgregarNodoPartesTransporte(tipoFigura, documento);
                if (nodoPartesTransporte != null)
                {
                    nodoTiposFigura.AppendChild(nodoPartesTransporte);
                }
            }

            //Descomentar
            //XmlElement nodoDomicilio = AgregarNodoDomicilio(partesTransporte.Domicilio, documento);
            //if (nodoDomicilio != null)
            //    nodoPartesTransporte.AppendChild(nodoDomicilio);

            nodoTiposFigura.SetAttribute("ParteTransporte", tiposFigura.TipoFigura);

            if (!string.IsNullOrEmpty(tiposFigura.RFCFigura))
            {
                nodoTiposFigura.SetAttribute("RFCFigura", tiposFigura.RFCFigura);
            }
            if (!string.IsNullOrEmpty(tiposFigura.NumLicencia))
            {
                nodoTiposFigura.SetAttribute("NumLicencia", tiposFigura.NumLicencia);
            }
            if (!string.IsNullOrEmpty(tiposFigura.NombreFigura))
            {
                nodoTiposFigura.SetAttribute("NombreFigura", tiposFigura.NombreFigura);
            }
            if (!string.IsNullOrEmpty(tiposFigura.NumRegIdTribFigura))
            {
                nodoTiposFigura.SetAttribute("NumRegIdTribFigura", tiposFigura.NumRegIdTribFigura);
            }
            if (!string.IsNullOrEmpty(tiposFigura.ResidenciaFiscalFigura))
            {
                nodoTiposFigura.SetAttribute("ResidenciaFiscalFigura", tiposFigura.ResidenciaFiscalFigura);
            }

            return nodoTiposFigura;
        }
        private XmlElement AgregarNodoPartesTransporte(PartesTransporte partesTransporte, XmlDocument documento)
        {
            if (partesTransporte == null) return null;

            XmlElement nodoPartesTransporte = documento.CreateElement("cartaporte", "PartesTransporte", NAMESPACE_CARTA_PORTE);
            nodoPartesTransporte.SetAttribute("ParteTransporte", partesTransporte.ParteTransporte);

            return nodoPartesTransporte;
        }



        #region Funciones

        private void ObtenerCertificadoYNoCertificado(string rutaCertificado, out string Certificado, out string NoCertificado)
        {
            try
            {
                X509Certificate2 objCert = new X509Certificate2();
                byte[] bRawData = ReadFile(rutaCertificado);
                objCert.Import(bRawData);
                Certificado = Convert.ToBase64String(bRawData);
                NoCertificado = FormatearSerieCert(objCert.SerialNumber);
            }
            catch
            {
                Certificado = string.Empty;
                NoCertificado = string.Empty;
            }
        }
        private byte[] ReadFile(string strArchivo)
        {
            FileStream f = new FileStream(strArchivo, FileMode.Open, FileAccess.Read);
            int size = Convert.ToInt32(f.Length);
            byte[] data = new byte[size];
            size = f.Read(data, 0, size);
            f.Close();
            return data;
        }
        private string FormatearSerieCert(string Serie)
        {
            string Resultado = "";
            int i;
            for (i = 1; i < Serie.Length; i += 2)
            {
                //Resultado = Serie.Substring(I, 1);
                Resultado = Resultado + Serie.Substring(i, 1);
            }
            return Resultado;
        }

        private string ObtenerSelloPorCertificado(XmlDocument xml, string rutaArchivoClavePrivada, string lPassword)
        {
            byte[] bClavePrivada = File.ReadAllBytes(rutaArchivoClavePrivada);
            string CadenaOriginal = GetCadenaOriginal(xml.InnerXml);

            System.Security.SecureString lSecStr = new System.Security.SecureString();
            lSecStr.Clear();
            SHA256Managed sham = new SHA256Managed();
            
            foreach (char c in lPassword.ToCharArray())
            {
                lSecStr.AppendChar(c);
            }

            RSACryptoServiceProvider lrsa = opensslkey.DecodeEncryptedPrivateKeyInfo(bClavePrivada, lSecStr);
            byte[] bCadenaOriginal = Encoding.UTF8.GetBytes(CadenaOriginal);
            byte[] bytesFirmados = null;
            try
            {
                bytesFirmados = lrsa.SignData(bCadenaOriginal, sham);

            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException("Clave privada incorrecta, revisa que la clave que escribes corresponde a los sellos digitales cargados");
            }

            return Convert.ToBase64String(bytesFirmados);

        }
        private string GetCadenaOriginal(string xmlCFD)
        {
            string rutaXSLT = Application.StartupPath + "\\XSLT\\cadenaoriginal.xslt";
            StringWriter output = new StringWriter();

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xmlCFD);
            XPathNavigator navigator = xmldoc.CreateNavigator();

            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(rutaXSLT);
            xslt.Transform(navigator, null, output);

            return output.ToString();
        }
        
        #endregion
    
    }
}