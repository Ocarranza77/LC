using QSG.QSystem.Common.Libreria;
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

namespace QSG.QSystem.DAL
{
    public class HelperDAL
    {
        #region Parameter
        /// <summary>
        /// Crea un Parametro de SQL de tipo Varchar
        /// </summary>
        /// <param name="prmName">Nombre del Parametro</param>
        /// <param name="value">Valor del Parametro, puede ser una clase</param>
        /// <param name="sqlType">De Forma predeterminada es Varchar</param>
        /// <param name="direction">Por defecto es Input</param>
        /// <param name="size">Por defecto es 250</param>
        /// <returns></returns>
/*        public static SqlParameter CreateParameter(string prmName, object value, SqlDbType sqlType = SqlDbType.VarChar, ParameterDirection direction = ParameterDirection.Input, int size = 250)
        {
            SqlParameter prm = new SqlParameter();

            prm.ParameterName = prmName;
            prm.Direction = direction;
            prm.SqlDbType = sqlType;

            if (prm.SqlDbType == SqlDbType.Xml && direction == ParameterDirection.Input)
                prm.Value = SerializeToXML(value);
            else
                prm.Value = value;

            if( sqlType == SqlDbType.Char || sqlType == SqlDbType.VarChar || sqlType == SqlDbType.NVarChar)
                prm.Size = size;

            return prm;
        }
*/

        /// <summary>
        /// Crea un Parametro de SQL de tipo Varchar, Direcction: Input
        /// </summary>
        /// <param name="prmName">Nombre del Parametro</param>
        /// <param name="value">Valor del Parametro, puede ser una clase</param>
        /// <param name="size">Por defecto es 250</param>
        /// <returns></returns>
        public static SqlParameter CreateParameter(string prmName, string value, int size = 250)
        {
            return CreateParameter(prmName, value, SqlDbType.VarChar, ParameterDirection.Input, size);
        }

        /// <summary>
        /// Crea un Parametro de SQL, de tipo Mensaje de Salida
        /// </summary>
        /// <param name="prmName">Nombre del Parametro</param>
        /// <param name="value">Valor a enviar, puede ser una clase</param>
        /// sqlType: Varchar
        /// Direccion: Output
        /// Tamaño: 8,000
        public static SqlParameter CreateParameterMsg(string prmName = "@Msg")
        {
            SqlParameter prm = new SqlParameter();

            prm.ParameterName = "@Msg";
            prm.Direction = ParameterDirection.Output;
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 8000;

            return prm;
        }

        /// <summary>
        /// Crea un Parametro de SQL de tipo Varchar, Direcction: Input
        /// </summary>
        /// <param name="prmName">Nombre del Parametro</param>
        /// <param name="value">Valor del Parametro, puede ser una clase</param>
        /// <param name="sqlType">Tipo de dato en SQL</param>
        /// <returns></returns>
        public static SqlParameter CreateParameter(string prmName, object value, SqlDbType sqlType)
        {
            return CreateParameter(prmName, value, sqlType, ParameterDirection.Input, 250);
        }

        /// <summary>
        /// Crea un Parametro de SQL, todos los valores deben ser enviados.
        /// </summary>
        /// <param name="prmName">Nombre del Parametro</param>
        /// <param name="value">Valor a enviar, puede ser una clase</param>
        /// <param name="sqlType">Tipo de dato en SQL</param>
        /// <param name="direction">Direccion (Input/Output)</param>
        /// <param name="size">Tamaño</param>
        /// <returns></returns>
        public static SqlParameter CreateParameter(string prmName, object value, SqlDbType sqlType, ParameterDirection direction, int size)
        {
            SqlParameter prm = new SqlParameter();

            prm.ParameterName = prmName;
            prm.Direction = direction;
            prm.SqlDbType = sqlType;

            if (prm.SqlDbType == SqlDbType.Xml && direction == ParameterDirection.Input)
                prm.Value = Serializacion.SerializeToXML(value);
            else
                prm.Value = value;

            if (sqlType == SqlDbType.Char || sqlType == SqlDbType.VarChar || sqlType == SqlDbType.NVarChar)
                prm.Size = size;

            return prm;
        }
        #endregion 

        #region Comando
        public static void CreateConnection(string strCnn, string spName, ref SqlConnection cnn, ref SqlCommand cmd)
        {
            cnn = new SqlConnection(strCnn);
            cmd = new SqlCommand(spName, cnn);
            cmd.CommandType = CommandType.StoredProcedure;
        }

        /// <summary>
        /// Ejecucion de SP de tipo Insert, Update, Delete.
        /// </summary>
        /// <param name="cnn">SqlConnection</param>
        /// <param name="cmd">SqlCommand</param>
        public static void ExecuteNonQuery(ref SqlConnection cnn, ref SqlCommand cmd)
        {
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        /// <summary>
        /// Ejecucion de SP de tipo consulta
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(SqlCommand cmd)
        {
            SqlDataAdapter sqlAdapter = new SqlDataAdapter( cmd);
            DataSet ds = new DataSet("Result");

            sqlAdapter.Fill(ds);

            return ds;
        }

        #endregion

        //#region Serializacion
        //public static string SerializeToXML(object entity)
        //{
        //    XmlSerializer serializer;
        //    serializer = new XmlSerializer(entity.GetType());

        //    MemoryStream ms = new MemoryStream();
        //    XmlTextWriter writer = new XmlTextWriter(ms, Encoding.Unicode);

        //    serializer.Serialize(writer, entity);
        //    writer.Close();
        //    ms.Close();

        //    string result = string.Empty;

        //    result = Encoding.Unicode.GetString(ms.GetBuffer());
        //    result = result.Substring(result.IndexOf(Convert.ToChar(60)));
        //    result = result.Substring(0, (result.LastIndexOf(Convert.ToChar(62)) + 1));

        //    return result;
        //}

        //public static T Deserialize<T>(string valor)
        //{
        //    T result = default(T);

        //    if (string.IsNullOrEmpty(valor))
        //        return result;

        //    XmlSerializer ser;
        //    ser = new XmlSerializer(typeof(T));
        //    StringReader stringReader;
        //    stringReader = new StringReader(valor);
        //    XmlTextReader xmlReader;
        //    xmlReader = new XmlTextReader(stringReader);
        //    object obj;
        //    obj = ser.Deserialize(xmlReader);
        //    xmlReader.Close();
        //    stringReader.Close();

        //    result = (T)Convert.ChangeType(obj, typeof(T));

        //    return result;
        //}

        //public static object Deserialize(string value, Type type)
        //{
        //    object result = null;

        //    //xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"

        //    //value = value.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
        //    //value = value.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

        //    Type[] extraTypes = new Type[1];
        //    extraTypes[0] = type;


        //    XmlSerializer ser;
        //    ser = new XmlSerializer(typeof(ArrayList), extraTypes);
        //    StringReader stringReader;
        //    stringReader = new StringReader(value);
        //    XmlTextReader xmlReader;
        //    xmlReader = new XmlTextReader(stringReader);
        //    object obj;
        //    try
        //    {
        //        obj = ser.Deserialize(xmlReader);
        //    }
        //    catch (Exception ex)
        //    {
        //        //TODO: Log the exception...   
        //        throw;
        //    }

        //    xmlReader.Close();
        //    stringReader.Close();

        //    result = obj;
        //    return result;
        //}
        //#endregion

    }
}
