using System;

namespace GenerarXml40
{
    public class TimbreFiscalDigital
    {
        public TimbreFiscalDigital()
        {
            this.Version = "1.1";
        }

        public string Version { get; set; } = "1.1";
        public string UUID { get; set; } = string.Empty;
        public DateTime FechaTimbrado { get; set; }
        public string RfcProvCertif { get; set; } = string.Empty;
        public string Leyenda { get; set; } = string.Empty;
        public string SelloCFD { get; set; } = string.Empty;
        public string NoCertificadoSAT { get; set; } = string.Empty;
        public string SelloSAT { get; set; } = string.Empty;

    }
}