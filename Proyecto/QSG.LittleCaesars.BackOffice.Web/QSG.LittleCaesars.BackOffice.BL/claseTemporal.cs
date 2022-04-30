using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using QSG.LittleCaesars.BackOffice.Common.Entities;



namespace QSG.LittleCaesars.BackOffice.BL //VersatilVentaServidor
{
    public class claseTemporal
    {
        /*
              Convencion de Lenguaje:
              //+ Campos usados en "claseCFDIv33" requeridos en la operacion
              //\ Campos usados en "claseCFDIv33" pero de procesos que nosotros no utilizariamos
              // ? Campos que no tenemos claro si los utilizaremos
              // x Campos no usados en "claseCFDIv33" pero que puede que nosotros llegemos a utilizar.
        */

        //login
        //public static bool Login = false;
        public static int IdUsuario = 0;//\
        public static string NombreUsuario = string.Empty;//\
        //public static List<string> ListaPermisos = new List<string>();
        //public static bool Permisos(string Permiso)
        //{
        //    bool success = ListaPermisos.Contains(Permiso);

        //    if (!success)
        //    {
        //        frmAlerta alerta = new frmAlerta();
        //        alerta._Titulo = "Permisos";
        //        alerta._Mensaje = "Usuario No tiene Permiso";
        //        alerta.ShowDialog();
        //    }

        //    return success;
        //}
        //public static bool Permisos(string Permiso1, string Permiso2)
        //{
        //    bool success = ListaPermisos.Contains(Permiso1);

        //    if (!success)
        //    {
        //        success = ListaPermisos.Contains(Permiso2);
        //    }

        //    if (!success)
        //    {
        //        frmAlerta alerta = new frmAlerta();
        //        alerta._Titulo = "Permisos";
        //        alerta._Mensaje = "Usuario No tiene Permiso";
        //        alerta.ShowDialog();
        //    }

        //    return success;
        //}

        //limpiar cadena
        public static string LimpiarString(string CadenaString)
        {
            string palabra = string.Empty;

            palabra = CadenaString.Replace("'", string.Empty).Replace("\"", string.Empty).Replace("|", string.Empty);

            return palabra;
        }

        //truncar
        public static decimal TruncateVersatil(decimal Cantidad, int Decimales)
        {
            char[] separador = { '.' };
            string[] enterodecimales = Cantidad.ToString().Split(separador, 2);
            string sDecimales = string.Empty;

            try
            {
                sDecimales = enterodecimales[1] + "0000";
            }
            catch (Exception)
            {
                sDecimales = "0000";
            }

            string redondeado = enterodecimales[0] + "." + sDecimales.Substring(0, Decimales);

            return Convert.ToDecimal(redondeado);
        }

        //teclado
        //public static string Teclado = string.Empty;

        //respaldos
        //public static bool Respaldo = false;

        //meses
        public static DataTable ListaMeses()
        {
            DataTable dtMeses = new DataTable();
            dtMeses.Columns.Add("mes_id", typeof(Int32));
            dtMeses.Columns.Add("mes_nombre", typeof(string));

            int anio = DateTime.Now.Year;

            dtMeses.Rows.Add(0, "AÑO " + anio);
            dtMeses.Rows.Add(1, "ENERO " + anio);
            dtMeses.Rows.Add(2, "FEBRERO " + anio);
            dtMeses.Rows.Add(3, "MARZO " + anio);
            dtMeses.Rows.Add(4, "ABRIL " + anio);
            dtMeses.Rows.Add(5, "MAYO " + anio);
            dtMeses.Rows.Add(6, "JUNIO " + anio);
            dtMeses.Rows.Add(7, "JULIO " + anio);
            dtMeses.Rows.Add(8, "AGOSTO " + anio);
            dtMeses.Rows.Add(9, "SEPTIEMBRE " + anio);
            dtMeses.Rows.Add(10, "OCTUBRE " + anio);
            dtMeses.Rows.Add(11, "NOVIEMBRE " + anio);
            dtMeses.Rows.Add(12, "DICIEMBRE " + anio);

            return dtMeses;
        }

        //alertas
        public static bool Respuesta; // ?

        //escalas
        //public static int IdEscala = 0;
        //public static decimal MargenEscala = 0m;
        //public static string TipoEscala = string.Empty;

        //cobrar
        public static string IdTicket = string.Empty; // x lo cambie a string por que es el folio de la factura
        //public static string Cantidad = string.Empty;
        //public static string UnidadDeMedida = string.Empty;
        //public static string Descripcion = string.Empty;
        //public static string Precio = string.Empty;
        //public static string PrecioUnitario = string.Empty;
        //public static string Importe = string.Empty;
        //public static string ImporteUnitario = string.Empty;
        //public static decimal Margen = 0m;
        //public static bool IngresarDetalle = false;
        //public static bool VentaCobrada = false;
        //public static decimal CantidadDevolucion = 0m;
        public static string NombreTicket = string.Empty; // x

        //public static decimal CambioMX = 0m;
        //public static decimal CambioUS = 0m;

        //clientes
        public static int IdCliente = 0; //+
        public static string NombreCliente = string.Empty;//\
        //public static bool Seleccionado = false;

        //claves
        //public static int IdClave = 0;

        //empleados
        //public static int IdEmpleado = 0;
        //public static string NombreEmpleado = string.Empty;

        //proveedores
        //public static int IdProveedor = 0;
        //public static string NombreProveedor = string.Empty;
        
        //marcas
        //public static int IdMarca = 0;

        //filtros
        //public static int IdFiltro1 = 0;
        //public static int IdFiltro2 = 0;
        //public static int IdFiltro3 = 0;
        //public static int IdFiltro4 = 0;
        //public static int IdFiltro5 = 0;
        //public static int IdFiltro6 = 0;
        //public static int FiltroPosicion = 0;


        //articulos
        //public static int IdArticulo = 0;
        //public static string CodigoArticulo = string.Empty;
        //public static string NombreArticulo = string.Empty;
        //public static List<int> ListaIdsArticulos = new List<int>();
        ////lo uso por si el código no pertenece a ningún artículo,
        ////es posible que se trate de una etiqueta de báscula
        //public static string CodigoBuscadoArticulo = string.Empty;
        
        //CFDI
        public static int IdCFDI = 0;
        public static string Comentarios = string.Empty;

        public static decimal Subtotal = 0m;//+
        public static decimal IvaTotal = 0m;//+
        public static decimal IepsTotal = 0m;//+
        public static decimal RetencionISR = 0m;//+
        public static decimal RetencionIVA = 0m;//+
        public static decimal RetencionIEP = 0m;//+
        public static decimal Total = 0m;//+

        public static string FormaDePago = string.Empty;
        public static string FormaDePagoClave = string.Empty; //+
        public static string MetodoDePago = string.Empty;
        public static string MetodoDePagoClave =string.Empty;//+
        public static string UsoCfdi = string.Empty;
        public static string UsoCfdiClave = string.Empty;//+
        public static string Moneda = string.Empty;//\
        public static string MonedaClave = string.Empty;//+
        public static string TipoDeCambio = string.Empty;//+
        public static string TipoDeCambioCfdiPadre = string.Empty;
        public static string ReferenciaDeCuenta = string.Empty;
        public static string Condiciones = string.Empty; //+
        public static string CodigoConfirmacion = string.Empty;//+
        
        public static DateTime FechaEmision = DateTime.Now; //+
        public static DateTime FechaPago = DateTime.Now;
        public static string SiguienteFolio = "0"; //+
        public static bool CFDIGenerado = false;//+

        public static bool ConsultarTimbres(string UsuarioPac, string ContraseniaPac)
        {
            bool success = true;

            try
            {
                //leer timbres en fel
                FELv33.WSCFDI33Client ServicioFEL = new FELv33.WSCFDI33Client();
                FELv33.RespuestaCreditos respuestafel = ServicioFEL.ConsultarCreditos(UsuarioPac, ContraseniaPac);
                ServicioFEL.Close();

                if (respuestafel.OperacionExitosa)
                {
                    foreach (FELv33.DetallesPaqueteCreditos detallepaquete in respuestafel.Paquetes)
                    {
                        success = true;

                        if (detallepaquete.EnUso & detallepaquete.Vigente)
                        {
                            int TimbresDisponibles = detallepaquete.TimbresRestantes;
                            string fechavencimiento = Convert.ToDateTime(detallepaquete.FechaVencimiento).ToShortDateString();

                            if (TimbresDisponibles <= 50)
                            {
                                string msg = TimbresDisponibles + " TIMBRES";
                                msg += Environment.NewLine + "VENCIMIENTO " + fechavencimiento;

                                //frmAlerta alerta = new frmAlerta();
                                //alerta._Tipo = "error";
                                //alerta._Titulo = "TIMBRES DISPONIBLES";
                                //alerta._Mensaje = msg;
                                //alerta.ShowDialog();
                            }

                            break;
                        }
                        else
                        {
                            success = false;
                        }
                    }
                }
                else
                {
                    success = false;
                }
            }
            catch (Exception)
            {
                success = false;
            }


            if (!success)
            {
                //frmAlerta alerta = new frmAlerta();
                //alerta._Tipo = "error";
               // alerta._Titulo = "TIMBRES DISPONIBLES";
               // alerta._Mensaje = "0 TIMBRES, HAY INTERNET ??";
                //alerta.ShowDialog(); // TODO: mandar un mensaje de error.
            }

            return success;
        }

        //cotizaciones
        //public static bool ConIva = false;

        //inventarios
        //public static bool ContinuarInventario = false;
        //public static bool PreInventario = false;
        //public static int IdAlmacen = 0;
        //public static string DescripcionAlmacen = string.Empty;
        //public static int IdMotivo = 0;
        //public static string DescripcionMotivo = string.Empty;
        //public static DateTime FechaInventario = DateTime.Now;
        //public static string ObservacionInventario = string.Empty;

        //descuentos
        //public static bool DescuentoGlobal = false;
        //public static decimal ImporteDescontado = 0m;


    }
}