using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace QSG.LittleCaesars.BackOffice.BL
{
   public class Hash
    {
        /// <summary>
        /// Hashea un string utilizando el algoritmo MD5
        /// </summary>
        /// <param name="stringOriginal">string que será hasheado</param>
        /// <returns>string hasheado</returns>
        public  string MD5(string stringOriginal)
        {
            //Hash MD5
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            //Hashar
            byte[] datos = md5Hasher.ComputeHash(Encoding.Default.GetBytes(stringOriginal));
            //crear stringHashado
            StringBuilder stringHashado = new StringBuilder(datos.Length);
            //concatenar bytes de datos a stringHashado
            foreach (byte b in datos) stringHashado.Append(b.ToString("x2"));
            return stringHashado.ToString(); //regresar stringHashado
        }

        /// <summary>
        /// Hashea un string utilizando el algoritmo SHA1
        /// </summary>
        /// <param name="stringOriginal">string que será hasheado</param>
        /// <returns>string hasheado</returns>
        public  string SHA1(string stringOriginal)
        {
            byte[] datos;
            UnicodeEncoding ue = new UnicodeEncoding();
            //convertir string original a arreglo de bytes
            byte[] BytesMensaje = ue.GetBytes(stringOriginal);
            //Hash SHA1
            SHA1Managed shHasher = new SHA1Managed();
            //Hashar
            datos = shHasher.ComputeHash(BytesMensaje);
            //crear stringHashado
            StringBuilder stringHashado = new StringBuilder(datos.Length);
            //concatenar bytes de datos a stringHashado
            foreach (byte b in datos) stringHashado.Append(b.ToString("x2"));
            return stringHashado.ToString(); //regresar stringHashado
        }

    }
}
