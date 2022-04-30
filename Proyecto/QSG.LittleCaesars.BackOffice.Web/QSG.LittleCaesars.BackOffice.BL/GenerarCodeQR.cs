using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;


namespace QSG.LittleCaesars.BackOffice.BL
{
    public class GenerarCodeQR
    {
        /*



        //Datos_factura RFC_emisor,RFC_Recpetor Total, Clave UUID
        public QRCodeEncoder CrearQR( int scala, string[] Datos_Factura)
        {
            string msg="";
            string datos = "?re={0}&rr={1}&tt={2}&id={3}";
            decimal total = 0;
            string sTotal = "";
            string entero = "";
            string decimales = "";
            string resultado = "";

            string RFC_Emisor = "";
            string RFC_Receptor = "";
            string UUID = "";

            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            
            try
            {
                if (Datos_Factura.Length == 4)
                {
                    RFC_Emisor = Datos_Factura[0];
                    RFC_Receptor = Datos_Factura[1];
                    total = Convert.ToDecimal(Datos_Factura[2]);
                    UUID = Datos_Factura[3];


                   


                    sTotal = String.Format("{0:0.000000}", total);
                    entero = decimal.Truncate(total).ToString().PadLeft(10, Convert.ToChar("0"));
                    decimales = sTotal.Split(Convert.ToChar("."))[1];
                    resultado = entero + "." + decimales;


                   // QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    qrCodeEncoder.QRCodeScale =scala;//4
                    qrCodeEncoder.QRCodeVersion = 8;
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                    qrCodeEncoder.Encode(datos);
                 
                    

                }
                else {
                    msg += "Informacion Factura Erroneo ";
                }





            }
            catch(Exception ex) {
               
            }
            return qrCodeEncoder;
           
        }
        */

    }
}
