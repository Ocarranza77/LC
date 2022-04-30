using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace MvcApplication1.clasesCFDI
{
    public class Certificado
    {
        private DateTime _ValidoDesde;
        /// <summary>
        /// Contiene la fecha de inicio de vigencia del certificado
        /// </summary>
        public DateTime ValidoDesde
        {
            get { return _ValidoDesde; }
            set { _ValidoDesde = value; }
        }

        private DateTime _ValidoHasta;
        /// <summary>
        /// Contiene la fecha de vencimiento del certificado
        /// </summary>
        public DateTime ValidoHasta
        {
            get { return _ValidoHasta; }
            set { _ValidoHasta = value; }
        }

        private string _Serie;
        /// <summary>
        /// Contiene el numero de serie del certificado
        /// </summary>
        public string Serie
        {
            get { return _Serie; }
        }

        private string _CertificadoBase64;
        /// <summary>
        /// Contiene el certificado codificado en BASE64
        /// </summary>
        public string CertificadoBase64
        {
            get { return _CertificadoBase64; }
        }

        /// <summary>
        /// Inicializa la clase con la ruta del archivo *.cer
        /// </summary>
        /// <param name="RutaCertificado">Recibe la ruta del archivo *.cer</param>
        public Certificado(string RutaCertificado)
        {
            try
            {
                if (File.Exists(RutaCertificado))
                {
                    X509Certificate cCert = new X509Certificate();
                    string strSerial = string.Empty;

                    cCert = X509Certificate.CreateFromCertFile(RutaCertificado);

                    _ValidoDesde = DateTime.Parse(cCert.GetEffectiveDateString());
                    _ValidoHasta = DateTime.Parse(cCert.GetExpirationDateString());

                    int i;
                    string sn = cCert.GetSerialNumberString();
                    for (i = 0; i < sn.Length; i++)
                    {
                        if (i % 2 != 0)
                        {
                            strSerial = strSerial + sn.Substring(i, 1);
                        }
                    }

                    _Serie = strSerial;
                    _CertificadoBase64 = Convert.ToBase64String(cCert.GetRawCertData());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al leer certificado, Error: " + ex.Message);
            }
        }
    }
}