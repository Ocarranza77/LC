using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Libreria
{
    public class Serializacion
    {
        #region Serializacion
        public static string SerializeToXML(object entity)
        {
            XmlSerializer serializer;
            serializer = new XmlSerializer(entity.GetType());

            MemoryStream ms = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(ms, Encoding.Unicode);

            serializer.Serialize(writer, entity);
            writer.Close();
            ms.Close();

            string result = string.Empty;

            result = Encoding.Unicode.GetString(ms.GetBuffer());
            result = result.Substring(result.IndexOf(Convert.ToChar(60)));
            result = result.Substring(0, (result.LastIndexOf(Convert.ToChar(62)) + 1));

            return result;
        }

        public static T Deserialize<T>(string valor)
        {
            T result = default(T);

            if (string.IsNullOrEmpty(valor))
                return result;

            XmlSerializer ser;
            ser = new XmlSerializer(typeof(T));
            StringReader stringReader;
            stringReader = new StringReader(valor);
            XmlTextReader xmlReader;
            xmlReader = new XmlTextReader(stringReader);
            object obj;
            obj = ser.Deserialize(xmlReader);
            xmlReader.Close();
            stringReader.Close();

            result = (T)Convert.ChangeType(obj, typeof(T));

            return result;
        }

        public static object Deserialize(string value, Type type)
        {
            object result = null;

            //xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"

            //value = value.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
            //value = value.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            Type[] extraTypes = new Type[1];
            extraTypes[0] = type;


            XmlSerializer ser;
            ser = new XmlSerializer(typeof(ArrayList), extraTypes);
            StringReader stringReader;
            stringReader = new StringReader(value);
            XmlTextReader xmlReader;
            xmlReader = new XmlTextReader(stringReader);
            object obj;
            try
            {
                obj = ser.Deserialize(xmlReader);
            }
            catch (Exception ex)
            {
                //TODO: Log the exception...   
                throw;
            }

            xmlReader.Close();
            stringReader.Close();

            result = obj;
            return result;
        }
        #endregion

    }
}
