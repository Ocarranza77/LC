using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QSG.LittleCaesars.Portal.Web
{
    public class Conexion
    {
        private static string cadena = ConfigurationManager.ConnectionStrings["strConnection"].ConnectionString;

        private static SqlConnection conectar;
        public string Error;
        public static bool Inicializar()
        {

            return Conectar();
        }

        private static bool Conectar()
        {
            bool conectado = true;
            // msg = "Conexion Exitoso";
            //MsgServer = "";
            try
            {
                conectar = new SqlConnection(cadena);
                conectar.Open();
            }
            catch (SqlException ex)
            {

                conectado = false;
                conectar.Close();
                //msg = "Error de Conexion";
                //MsgServer = ex.Message;
            }

            return conectado;
        }
        private static void Desconectar()
        {
            conectar.Close(); // cerrar conexion
            //conectar = null;
            conectar.Dispose();

        }
        public static DataTable LeerTabla(string consulta)
        {
            DataTable tabla = new DataTable();
            if (Conectar())
            {
                SqlCommand comando = new SqlCommand(consulta, conectar);
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                try
                {
                    adaptador.Fill(tabla);
                }
                catch (SqlException ex)
                {
                    // msg = "Error en la Consulta SQL";
                    //Msgserver = ex.Message;
                    Desconectar();
                }
                conectar.Close();
                conectar.Dispose();

            }
            return tabla;

        }
        public static DataRow LeerRegistro(string consulta)
        {
            DataRow registro = null;
            DataTable tabla = LeerTabla(consulta);
            if (tabla.Rows.Count > 0)
            {
                registro = tabla.Rows[0];

            }
            return registro;

        }
        public static int GetCodigo(string clave, string referencia, string parametro)
        {

            DataRow dr = LeerRegistro("SELECT " + parametro + " FROM " + referencia + " WHERE Clave='" + clave + "'");
            return dr != null ? Convert.ToInt32(dr[0]) : 0;
        }

        /// <summary>
        /// Ejecuta un comando de SQL no query (Insert, Update, Delete) sencillo (solo texto)
        /// </summary>
        /// <param name="instruccion">Instrucción SQL</param>
        /// <param name="mensaje">Mensaje entendible</param>
        /// <param name="mensajeservidor">Mensaje de SQL Server</param>
        /// <returns>Regresa true si se pudo ejecutar, false si no</returns>
        protected static bool EjecutarComando(string instruccion)
        {
            bool ejecuto = false;
            if (Conectar()) // conectar a Microsoft SQL Server
            {
                SqlCommand comando = new SqlCommand(instruccion, conectar); // comando
                try
                {
                    comando.ExecuteNonQuery(); // ejecutar comando SQL no query
                    Desconectar(); // cerrar conexion
                    ejecuto = true; // si se pudo ejecutar
                    //mensaje = "Instrucción ejecutada correctamente"; // mensaje entendible
                }
                catch (SqlException ex)
                {
                    Desconectar();
                    //mensaje = "Error en la instruccion de SQL"; // mensaje entendible
                    //mensajeservidor = ex.Message; // mensaje del servidor
                }
                comando.Dispose(); comando = null; // destruir objetos
            }
            return ejecuto; // regresar valor
        }


        public static DataTable EjecutarComandoConsulta(SqlCommand comando, out string msg)
        {
            msg = string.Empty;
            DataTable tabla = new DataTable();
            if (Conectar()) // conectar a Microsoft SQL Server
            {
                comando.Connection = conectar; // asignar conexion a comando
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                try
                {
                    adaptador.Fill(tabla);
                    conectar.Close();
                }
                catch (SqlException ex)
                {
                    Desconectar();
                    msg += "Error al ejecutar Command SQL Server " + ex.Message;
                    throw new Exception(ex.Message);
                    // msg = "Error en la Consulta SQL";
                    //Msgserver = ex.Message;
                    // Error = ex.Message;

                }
                conectar.Close();
                conectar.Dispose();
            }
            comando.Dispose(); comando = null; // destruir objetos
            return tabla;
        }

        public static int EjecutarProcedimientoNoConsulta(SqlCommand comando)
        {
            int resultado = 0; //resultado que regresará el stored procedure
            if (Conectar()) // conectar a Microsoft SQL Server
            {
                comando.Connection = conectar; // asignar conexion a comando
                try
                {
                    comando.ExecuteNonQuery(); // ejecutar procedimiento
                    resultado = (int)comando.Parameters["resultado"].Value; // recibir resultado del stored procedure, el stored procedure debe regresar un valor entero para que funcione
                    Desconectar(); // cerrar conexion
                    //mensaje = "Procedimiento almacenado ejecutado exitosamente "; // mensaje entendible
                }
                catch (SqlException ex)//si fue 0 error en la actualizacion
                {
                    resultado = 999; // error en stored procedure
                    Desconectar();
                    //mensaje = "Error en procedimiento almacenado"; // mensaje entendible
                    //mensajeservidor = ex.Message; // mensaje del servidor
                }
            }
            comando.Dispose(); comando = null; // destruir objetos
            return resultado; // regresar valor
        }


    }
}