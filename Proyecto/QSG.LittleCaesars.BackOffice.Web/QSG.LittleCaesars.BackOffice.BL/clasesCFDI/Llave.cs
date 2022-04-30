using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.IO;
using System.Text;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;

namespace MvcApplication1.clasesCFDI
{
    public class Llave
    {
        private string _RutaKey;
        private string _Password;

        /// <summary>
        /// Inicializa la clase indicando la ruta del archivo *.key y su password
        /// </summary>
        /// <param name="RutaLlave">Especifica la ruta del archivo *.key</param>
        /// <param name="Password">Especifica el password para abrir el archivo *.key</param>
        public Llave(string RutaLlave, string Password)
        {
            _RutaKey = RutaLlave;
            _Password = Password;

            if (File.Exists(RutaLlave))
            {
                byte[] dataKey = File.ReadAllBytes(RutaLlave);
                try
                {
                    Org.BouncyCastle.Crypto.AsymmetricKeyParameter asp = Org.BouncyCastle.Security.PrivateKeyFactory.DecryptKey(Password.ToCharArray(), dataKey);
                }
                catch (Exception ex)
                {
                    throw new Exception("La clave del archivo no es válida. Error: " + ex.Message);
                }
            }
            else
            {
                throw new Exception("El archivo llave no existe en el directorio especificado");
            }
        }

        /// <summary>
        /// Genera el sello a patir de una cadena original
        /// </summary>
        /// <param name="CadenaOriginal">Recibe el parametro de la cadena original</param>
        /// <returns>Regresa el sello digital</returns>
        public string GenerarSello(string CadenaOriginal)
        {
            try
            {
                byte[] dataKey = File.ReadAllBytes(_RutaKey);
                Org.BouncyCastle.Crypto.AsymmetricKeyParameter asp = Org.BouncyCastle.Security.PrivateKeyFactory.DecryptKey(_Password.ToCharArray(), dataKey);
                MemoryStream ms = new MemoryStream();
                TextWriter writer = new StreamWriter(ms);
                System.IO.StringWriter stWrite = new System.IO.StringWriter();
                Org.BouncyCastle.OpenSsl.PemWriter pmw = new PemWriter(stWrite);
                pmw.WriteObject(asp);
                stWrite.Close();

                //ISigner sig = SignerUtilities.GetSigner("MD5WithRSAEncryption");
                ISigner sig = SignerUtilities.GetSigner("SHA1WithRSA");

                //' Convertir a UTF8
                byte[] plaintext = Encoding.UTF8.GetBytes(CadenaOriginal);

                //' SELLAR
                sig.Init(true, asp);
                sig.BlockUpdate(plaintext, 0, plaintext.Length);
                byte[] signature = sig.GenerateSignature();

                object signatureHeader = Convert.ToBase64String(signature);
                return signatureHeader.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al intentar generar el sello. Error: " + ex.Message);
            }
        }
    }
}