using System;

namespace GenerarXml40
{
    public class Comprobante
    {      
        public Comprobante()
        {
            this.Version = "4.0";
        }

        public string Version { get; set; } = "4.0";
        public string Serie { get; set; } = string.Empty;
        public string Folio { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Sello { get; set; } = string.Empty;
        public string FormaPago { get; set; } = string.Empty;
        public string NoCertificado { get; set; } = string.Empty;
        public string Certificado { get; set; } = string.Empty;
        public string CondicionesDePago { get; set; } = string.Empty;
        public decimal SubTotal { get; set; } = 0m;
        public decimal Descuento { get; set; } = 0m;
        public string Moneda { get; set; } = string.Empty;
        public decimal TipoCambio { get; set; } = 0m;
        public decimal Total { get; set; } = 0m;
        public string TipoDeComprobante { get; set; } = string.Empty;
        public string Exportacion { get; set; } = string.Empty;
        public string MetodoPago { get; set; } = string.Empty;
        public string LugarExpedicion { get; set; } = string.Empty;
        public string Confirmacion { get; set; } = string.Empty;
        public InformacionGlobal InformacionGlobal { get; set; }
        public CfdiRelacionados CfdiRelacionados { get; set; }
        public Emisor Emisor { get; set; }
        public Receptor Receptor { get; set; }
        public Conceptos Conceptos { get; set; }
        public Impuestos Impuestos { get; set; }
        public Complemento Complemento { get; set; }
        public Acuse AcuseCancelacion { get; set; }


        public Addenda Addenda { get; set; }
        /*************** Falta agregar el complemento ADDENDA ********************************/
        public string TotalLetra { get; set; } = string.Empty;
        public bool EstaCancelado { get; set; } = false;

        public string Xml { get; set; } = string.Empty;


    }
}