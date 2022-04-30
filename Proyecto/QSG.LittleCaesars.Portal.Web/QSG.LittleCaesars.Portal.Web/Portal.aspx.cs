using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QSG.LittleCaesars.BackOffice.Messages;
using QSG.LittleCaesars.BackOffice.Messages.Requests;
using System.Globalization;
using System.Threading;
using System.Text;
using QSG.LittleCaesars.BackOffice.Common.Reports;
using System.Web.Services;
using System.Data;
using QSG.LittleCaesars.BackOffice.BL;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using QSG.LittleCaesars.BackOffice.Common.Entities;

namespace QSG.LittleCaesars.Portal.Web
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public static string NombrePantalla;
        public static string Titulo;

        public static string r_logo;
        public static string _user;
        public static string _usuarioID;
        public static CultureInfo culture = new CultureInfo("es-MX", true);
        public static List<Sucursal > lstSuc;//= new List<Sucursal>();
        public static string _titleDia;
        public static string _titleSemana;
        public static string _titleMes;
        protected void Page_Load(object sender, EventArgs e)
        {

            Thread.CurrentThread.CurrentCulture = culture;
            DateTime fecha2 = DateTime.Today;

            r_logo = "Images/little.png";
            if (Session["User"] != null)
            {
                NombrePantalla = "Portal";
                Titulo = Session["Empresa"].ToString() + " | " + NombrePantalla;
                _user = Session["Nombre"].ToString();
                _usuarioID = Session["User"].ToString();
               
              
                if (!IsPostBack) {
                   // string script = " $('#loaderDiv').toggle();";
                  //  ClientScript.RegisterStartupScript(this.GetType(), "load", script, true);



                    GetMenu(Session["User"].ToString());
                    txtFechaDash.Value = fecha2.AddDays(-1).ToShortDateString(); // "11/05/2015";

                    if (Convert.ToInt32(Session["DashBoard"].ToString()) > 0)
                    {
                        content_button_dash.Style.Add("display","block");
                        content_menu.Style.Add("width", "0px");
                        contetn_button_slider.Style.Add("left", "0px");


                        content_buttos_dash.Style.Add("display","block");
                        content_rep_ingresos.Style.Add("display","block");


                        GetSuc();
                        GetIngresos(DateTime.Parse(txtFechaDash.Value, culture), DropDownList1.SelectedValue, DropDownList1.SelectedItem.ToString());
                    }
                    else {
                        content_buttos_dash.Style.Add("display", "none");
                        content_rep_ingresos.Style.Add("display", "none");

                        //content_menu.Style.Add("width", "225px");
                    
                    }
                  //  GetIngresos(DateTime.Parse("10/05/2015", culture) );
                }
            }
            else
            {
                Session["User"] = null;
                Response.Redirect("~/Account/Login.aspx", true);
            }
            
            
            
            /*
            if (Session["User"] != null)
            {
                ruta_app.Value = Session["Empresa"] + " | " + tituloP.Text.ToUpper();
                NickName.InnerText = Session["Nombre"].ToString();
                GetMenu(Session["User"].ToString().Trim());
            }
            else
            {
                Session["User"] = null;
                Response.Redirect("~/Account/Login.aspx", true);
            }*/
         }
        [WebMethod]
        public static string CambiarPSW(String[] inf)
        {
            var DT = new DataTable();
            var SHA = new Hash();
            var msg = string.Empty;
            var typeMsg = 0;
            var modo = 3;
            var result = 0;
            var comando = new SqlCommand("GetUsuario");


            var _codUser = Convert.ToInt32(HttpContext.Current.Session["User"].ToString());

            var _oldPsw = inf[0];
            var _newPsw = inf[1];
            var _confPsw = inf[2];

            try
            {

                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.Add(new SqlParameter("@Alias", HttpContext.Current.Session["Email"].ToString()));
                comando.Parameters.Add(new SqlParameter("@Clave", SHA.SHA1(_oldPsw)));
                DT = Conexion.EjecutarComandoConsulta(comando, out msg);//validar usuario existente
                if (DT.Rows.Count > 0)
                {

                    if (_newPsw == _confPsw)
                    {
                        comando = new SqlCommand("UpdatePassUser");
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter prm = new SqlParameter("resultado", SqlDbType.Int, 4);
                        prm.Direction = ParameterDirection.ReturnValue;

                        comando.Parameters.Add(prm);
                        comando.Parameters.Add(new SqlParameter("@codUser", _codUser));
                        comando.Parameters.Add(new SqlParameter("@password", SHA.SHA1(_newPsw)));

                        result = Conexion.EjecutarProcedimientoNoConsulta(comando);

                    }
                    else
                    {
                        msg = "Confirmacion de Contraseña Incorrecta";
                        typeMsg = 1;
                    }
                }
                else
                {
                    msg = "Contraseña Anterior Incorrecta.";
                    typeMsg = 1;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                typeMsg = 1;
            }
            return typeMsg + "|" + modo + "|" + msg;


        }


        protected void GetSuc()
        {
            lstSuc = new List<Sucursal>();
            var su = new Sucursal();
            var sv1 = new ServiceImplementation();
            var sr = new SucursalRequest();
            su.SucursalID = 0;
            sr.Sucursal = su;
            sr.UserIDRqst = Convert.ToInt32(_usuarioID);
            sr.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Report; //BackOffice.Common.Enums.MessageOperationType.Report;
            var response = sv1.SucursalMessage(sr);
            lstSuc.Add(new Sucursal() { SucursalID = 0, Nombre = "Todos" });
            //lstSuc.AddRange(response.Sucursales);

            foreach (Sucursal suc in response.Sucursales) {
                lstSuc.Add(new Sucursal() { SucursalID = suc.SucursalID, Nombre = suc.Abr.ToString() + "-" + suc.Nombre.ToString() });
            }

            
            DropDownList1.DataSource =lstSuc;
            DropDownList1.DataValueField = "SucursalID";
            DropDownList1.DataTextField = "Nombre";
            DropDownList1.DataBind();
            /*
            DropDownList2.DataSource = response.Sucursales;
            DropDownList2.DataValueField = "SucursalID";
            DropDownList2.DataTextField = "Nombre";
            DropDownList2.DataBind();

            DropDownList3.DataSource = response.Sucursales;
            DropDownList3.DataValueField = "SucursalID";
            DropDownList3.DataTextField = "Nombre";
            DropDownList3.DataBind();*/


           // lstSuc = response.Sucursales;
        }
        protected void GetIngresos(DateTime fecha,string sucursalID,string sucursal)

       // [WebMethod]
        //public static string GetIngreso(string sucursalID)
        {
           // DateTime fecha = DateTime.Parse("10/05/2015", culture);
            /*
            String csname1 = "PopupScript";
            String csname2 = "ButtonClickScript";
            Type cstype = this.GetType();

            ClientScriptManager cs = Page.ClientScript;

            // Check to see if the startup script is already registered.
            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                String cstext1 = "alert('Hello World');";
                cs.RegisterStartupScript(cstype, csname1, cstext1, true);
            }

            // Check to see if the client script is already registered.
            if (!cs.IsClientScriptBlockRegistered(cstype, csname2))
            {
                StringBuilder cstext2 = new StringBuilder();
                cstext2.Append("<script type=\"text/javascript\"> function DoClick() {");
                cstext2.Append("Form1.Message.value='Text from client script.'} </");
                cstext2.Append("script>");
                cs.RegisterClientScriptBlock(cstype, csname2, cstext2.ToString(), false);
            }
            */






            //ltRepIngDia.Text = "<script type='text/javascript'>$('#content_rep_dia').HTML='<span>Cargando..</span>'; </script>";
           


            string[] meses = { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };

            var _datos = "";
            var _medidas = "";
            var _msg="<span style=\\'color:red;\\'>No Existen datos para mostrar.</span>";

            var _datSema = "";
            var _datMes = "";
            //var _color = "";
            //var _colorBajo = "#00BFFF";// "#2E9AFE";
            //var _colorAlto = "#0080FF";
            var _ingTotS = 0.0;
            var _ingTotM = 0.0;
            var _facTotS = 0.0;
            var _facTotM = 0.0;
            ///var _contadorDato = 0;
           
            var _ingTotalDia = 0.0;
            var _facTotalDia = 0.0;
            var _BS_SubVenta_dia = 0.0;
            var _BS_SubFact_dia = 0.0;


            var _iniSemana = "";
            var _finSemana = "";
            var _suc = sucursal.ToLower() == "todos" ? " Todas las sucursales " : sucursal;
            var _servicio = new ServiceImplementation();
            var _ingRequest = new RepIngresosDSMRequest();


            _titleDia = "";
            _titleMes = "";
            _titleSemana = "";


            _ingRequest.fecha = fecha;
            _ingRequest.SucursalID=sucursalID!=""?Convert.ToInt32(sucursalID):0;
            _ingRequest.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Report; //BackOffice.Common.Enums.MessageOperationType_old.Report; //.MessageOperationType.Report;
            _ingRequest.UserIDRqst = Convert.ToInt32(_usuarioID);

            var response = _servicio.RepIngresosDSMMessages(_ingRequest);

         

           // content_gxdia.Style.Add("display", "block");

            _medidas = " var width=document.getElementById('content_rep_dia').width;";
            _medidas += " var height=document.getElementById('content_rep_dia').height;";

            var identy = "";

            if (response.Reporte.Dia != null)
            {


                foreach (BaseStrDub BstrD in response.Reporte.Dia)
                {
                    var _nombre = BstrD.strNombre.ToString().Split(new char[] { '|' });
                    identy = "";
                    if (_nombre[1].ToString() == "LP" || _nombre[1].ToString() == "LC" || _nombre[1].ToString() == "PLP")
                    {
                        _BS_SubFact_dia += BstrD.dblValor2;
                        _BS_SubVenta_dia += BstrD.dblValor;
                    }

                    if (_datos != "") { _datos += ","; }

                    if (BstrD.dblValor != BstrD.dblValor2) { identy = "*"; }

                    _datos += "['" + _nombre[1] + identy + "'," + BstrD.dblValor + "," + BstrD.dblValor2 + "]";
                    _ingTotalDia += BstrD.dblValor;
                    _facTotalDia += BstrD.dblValor2;
                }


                _titleDia = "Dia " + String.Format("{0:d MMMM yyyy}", fecha) + " (TODOS) Total Venta: " + _ingTotalDia.ToString("C", culture) + " Facturado: " + _facTotalDia.ToString("C", culture) + " Pesos";


            }

            //var _BN_SubVenta = 0.0;
            //var _BN_SubFact = 0.0;
            var _BS_SubVenta = 0.0;
            var _BS_SubFact = 0.0;

            var _BS_SubVenta_mes = 0.0;
            var _BS_SubFact_mes = 0.0;

            var _SubVenta = 0.0;
            var _Subfact = 0.0;
            var _fecha="";
            var index = 0;

            if (response.Reporte.Semana != null)
            {
                foreach (BaseStrDub Bsrt in response.Reporte.Semana)
                {
                    var _nombre = Bsrt.strNombre.ToString().Split(new char[] { '|' });

                    if (index == 0)
                    {
                        _fecha = Bsrt.strDato.ToString().Trim();
                        _iniSemana = Bsrt.strDato;
                       // index //= response.Reporte.Semana.Count;
                    }
                    index++;
                 

                   

                    if (_fecha != Bsrt.strDato.ToString().Trim())
                    {
                        _ingTotS += _SubVenta;
                        _facTotS += _Subfact;
                        if (_datSema != "") { _datSema += ","; }
                        _datSema += "['" + String.Format("{0:ddd d}", DateTime.Parse(_fecha, culture))  + "'," + _SubVenta + ",'Venta : "+_SubVenta.ToString("C")+"'," + _Subfact + ",'Facturado : "+_Subfact.ToString("C")+"']";
                        _fecha = Bsrt.strDato.ToString().Trim();
                        _SubVenta = 0.0;
                        _Subfact = 0.0;
                        // Regex.Replace(textoOriginal, @"[^a-zA-z0-9 ]+", "");


                    }

                    _SubVenta += Bsrt.dblValor;
                    _Subfact += Bsrt.dblValor2;

                    //if (_nombre[0].ToString().Trim().ToLower() == "forjadores" || _nombre[0].ToString().Trim().ToLower() == "loscabos" || _nombre[0].ToString().Trim().ToLower() == "paseolapaz")
                    if (_nombre[1].ToString() == "LP" || _nombre[1].ToString() == "LC" || _nombre[1].ToString() == "PLP")
                    {
                        _BS_SubVenta += Bsrt.dblValor;
                        _BS_SubFact += Bsrt.dblValor2;
                    }


                    if (index == response.Reporte.Semana.Count)
                    {
                        _finSemana = Bsrt.strDato;

                        _ingTotS += _SubVenta;
                        _facTotS += _Subfact;
                        if (_datSema != "") { _datSema += ","; }
                        _datSema += "['" + String.Format("{0:ddd d}", DateTime.Parse(_fecha, culture)) + "'," + _SubVenta + ",'Venta : "+_SubVenta.ToString("C")+"'," + _Subfact + ",'Facturado : "+_Subfact.ToString("C")+"']";
                        _fecha = Bsrt.strDato.ToString().Trim();
                        _SubVenta = 0.0;
                        _Subfact = 0.0;

                    }

                }

                _titleSemana = "Semana del " + String.Format("{0:dd}", DateTime.Parse(_iniSemana, culture)) + " al " + String.Format("{0:d MMMM yyyy}", DateTime.Parse(_finSemana, culture)) + " (" + _suc + ") Total Venta: " + _ingTotS.ToString("C", culture) + " Facturado: " + _facTotS.ToString("C", culture) + " Pesos";

            }

            index = 0;
           // _BS_SubFact = 0.0;
           // _BS_SubVenta = 0.0;
            _Subfact = 0.0;
            _SubVenta = 0.0;
            if (response.Reporte.Mes!=null) {

                foreach (BaseStrDub Bsrt in response.Reporte.Mes) {
                    var _nombre = Bsrt.strNombre.ToString().Split(new char[] { '|' });
                   

                    if (index == 0)
                    {
                        _fecha = Bsrt.strDato.ToString().Trim();
                        _iniSemana = Bsrt.strDato;
                       // index++;// = response.Reporte.Mes.Count;
                    }
                    index++;

                    if (_fecha != Bsrt.strDato.ToString().Trim())
                    {
                        _ingTotM += _SubVenta;
                        _facTotM += _Subfact;
                        if (_datMes != "") { _datMes += ","; }
                        _datMes += "['" + meses[Convert.ToInt32(_fecha.Substring(4)) - 1] + "'," + _SubVenta + ",'Venta : "+_SubVenta.ToString("C")+"'," + _Subfact + ",'Facturado : "+_Subfact.ToString("C")+"']";
                        _fecha = Bsrt.strDato.ToString().Trim();
                        _SubVenta = 0.0;
                        _Subfact = 0.0;
                    }


                    _SubVenta += Bsrt.dblValor;
                    _Subfact += Bsrt.dblValor2;

                    //if (_nombre[0].ToString().Trim().ToLower() == "forjadores" || _nombre[0].ToString().Trim().ToLower() == "loscabos" || _nombre[0].ToString().Trim().ToLower() == "paseolapaz")
                    if (_nombre[1].ToString() == "LP" || _nombre[1].ToString() == "LC" || _nombre[1].ToString() == "PLP")
                    {
                        _BS_SubVenta_mes += Bsrt.dblValor;
                        _BS_SubFact_mes += Bsrt.dblValor2;
                    }


                    if (index == response.Reporte.Mes.Count) {
                        _finSemana = Bsrt.strDato;
                        _ingTotM += _SubVenta;
                        _facTotM += _Subfact;
                        if (_datMes != "") { _datMes += ","; }
                        _datMes += "['" + meses[Convert.ToInt32(_fecha.Substring(4)) - 1]  + "'," + _SubVenta + ",'Venta : "+_SubVenta.ToString("C")+"'," + _Subfact + ",'Facturado : "+_Subfact.ToString("C")+"']";
                        _fecha = Bsrt.strDato.ToString().Trim();
                        _SubVenta = 0.0;
                        _Subfact = 0.0;
                    }

                }
                _titleMes = meses[Convert.ToInt32(_iniSemana.Substring(4)) - 1] + "-" + _iniSemana.Substring(0, 4) +"  "+ meses[Convert.ToInt32(_finSemana.Substring(4)) - 1] + "-" + _finSemana.Substring(0, 4) + " (" + _suc + ") Total Venta: " + _ingTotM.ToString("C", culture) + " Facturado: " + _facTotM.ToString("C", culture) + " Pesos";
            
            }




            /*
            if (response.Reporte.Semana != null)
            {
               if (response.Reporte.Semana.Count > 0)
                {

                    //content_dashboard_XSemana.Style.Add("display", "inline-block");
                   // content_gxsemana.Style.Add("display", "block");
                  //  _medidas = " var width=document.getElementById('content_rep_semana').width;";
                  //  _medidas += " var height=document.getElementById('content_rep_semana').height;";

                }


               int index = 0;
                foreach (BaseStrDub Bsrt in response.Reporte.Semana)
                {
                    if (index == 0) {
                        _iniSemana = Bsrt.strDato;
                        index = response.Reporte.Semana.Count;
                    }
                    if (index == response.Reporte.Semana.Count) {
                        _finSemana = Bsrt.strDato;
                    }

                    var fe = Bsrt.strDato.ToString();
                    if (_datSema != "") { _datSema += ","; }
                    _datSema += "['" + String.Format("{0:dddd d}", DateTime.Parse(Bsrt.strDato, culture)) +"-"+ String.Format("{0:yy}", DateTime.Parse(Bsrt.strDato, culture)) + "'," + Bsrt.dblValor.ToString() + "," + Bsrt.dblValor2.ToString() + "]";
                    _ingTotS += Bsrt.dblValor;
                    _facTotS += Bsrt.dblValor2;
                }

                _titleSemana = "Semana del " + String.Format("{0:dd}", DateTime.Parse(_iniSemana, culture)) + " al " + String.Format("{0:d MMMM yyyy}", DateTime.Parse(_finSemana, culture)) + " " + _suc + " Total Venta: " + _ingTotS.ToString("C", culture) + " Facturado: " + _facTotS.ToString("C", culture) + " Pesos";
            }



            if (response.Reporte.Mes != null)
            {
                if (response.Reporte.Mes.Count > 0)
                {
                   // content_dashboard_XMes.Style.Add("display", "inline-block");
                   // content_gxmes.Style.Add("display", "block");
                  //  _medidas = " var width=document.getElementById('content_rep_mes').width;";
                  //  _medidas += " var height=document.getElementById('content_rep_mes').height;";

                }
                foreach (BaseStrDub Bsrt in response.Reporte.Mes)
                {
                    var fe = Bsrt.strDato.ToString();
                    if (_datMes != "") { _datMes += ","; }
                    _datMes += "['" + meses[Convert.ToInt32(fe.Substring(4)) - 1] + "-" + fe.Substring(0,4) + "'," + Bsrt.dblValor.ToString() +","+Bsrt.dblValor2.ToString()+ "]";
                    _ingTotM += Bsrt.dblValor;
                    _facTotM += Bsrt.dblValor2;
                }
                _titleMes = " Año " + String.Format("{0:yyyy}", fecha) + " " + _suc + " Total Venta: " + _ingTotM.ToString("C", culture) + " Facturado: " + _facTotM.ToString("C", culture) + " Pesos";
            }

            */

            StringBuilder html = new StringBuilder();

            html.Append("<script type='text/javascript'>");
            html.Append(" google.load('visualization', '1', {packages: ['bar','corechart', 'line'],language:['en']}); google.setOnLoadCallback(drawChart); ");
            html.Append(" function drawChart() {");
            html.Append(" var data_chart_dia = google.visualization.arrayToDataTable([['Sucursales', 'Venta','Facturado'],");
            html.Append(_datos);
            html.Append("]);");
            html.Append("var op_chart_dia = {legend:{position:'none'},chart: {position:'none'},bars: 'vertical', vAxis: {format: 'currency', minValue: 0,logScale: true, textStyle:{fontName:'Corbel',italic:false, bold:false,fontSize:10,left:'0%'}},hAxis:{textStyle:{ fontName:'Corbel',italic:false,bold:false,fontSize:10,left:'0%'}},colors:['#088A29','#DF7401']};");

            html.Append(" var view_chart_dia = new google.charts.Bar(document.getElementById('content_rep_dia')); ");
            html.Append(" view_chart_dia.draw(data_chart_dia, google.charts.Bar.convertOptions(op_chart_dia));");

            html.Append(" var data_pie_dia = google.visualization.arrayToDataTable([['Localidad', 'Venta',{type:'string',role:'tooltip'}],");

            var porcentage1 = Math.Round(((_ingTotalDia - _BS_SubVenta_dia) * 100) / _ingTotalDia, 2);
            var porcentage2 = Math.Round(((_BS_SubVenta_dia * 100) / _ingTotalDia), 2);

            html.Append("['BC'," + (_ingTotalDia - _BS_SubVenta_dia) + ",'" + (_ingTotalDia - _BS_SubVenta_dia).ToString("C", culture) + " (" + porcentage1 + "%)'],['BCS'," + _BS_SubVenta_dia + ",'" + _BS_SubVenta_dia.ToString("C", culture) + " (" + porcentage2 + "%)']");
            html.Append(" ]);");

            html.Append(" var op_pie_dia = {title: '',pieHole: 0.4,legend:{position:'none'},fontSize:10,chartArea: {left:'2%',top:'1%',bottom:'1%',right:'1%',width:'96%',height:'96%'},pieSliceText: 'label',tooltip:{textStyle:{fontSize:12 }}};");
            html.Append(" var content_pie_dia=document.getElementById('dia_pie');");
          
            if (_ingTotalDia > 0.00 || _BS_SubVenta_dia > 0.00)
            {
                html.Append(" var view_pie_dia = new google.visualization.PieChart(content_pie_dia); view_pie_dia.draw(data_pie_dia, op_pie_dia);");
            }
            else
            {
                html.Append("content_pie_dia.innerHTML='" + _msg + "';");
            }


            /**********************************************************************************************/


            html.Append("var data_chart_semana = new google.visualization.DataTable(); data_chart_semana.addColumn('string', 'Dias'); data_chart_semana.addColumn('number', 'Venta'); data_chart_semana.addColumn({type: 'string', role: 'tooltip'});data_chart_semana.addColumn('number', 'Facturado'); data_chart_semana.addColumn({type: 'string', role: 'tooltip'});data_chart_semana.addRows([");
            html.Append(_datSema);
            html.Append("]);");
            html.Append("var op_chart_semana = {legend: { position: 'none' },hAxis: { title: 'Dias',textStyle:{fontSize:9}},vAxis: {title: 'Ingresos',format: 'currency',minValue: 0,logScale: false,textStyle:{fontSize:9}},colors:['#088A29','#DF7401']}; ");
            html.Append(" var view_chart_semana = new google.visualization.LineChart(document.getElementById('content_rep_semana')); ");
            html.Append(" view_chart_semana.draw(data_chart_semana, op_chart_semana);");


            html.Append(" var data_pie_semana = google.visualization.arrayToDataTable([['Localidad', 'Venta',{type:'string',role:'tooltip'}],");


            var por1 = Math.Round(((_ingTotS - _BS_SubVenta) * 100) / _ingTotS, 2);
            var por2 = Math.Round(((_BS_SubVenta * 100) / _ingTotS), 2);

           // html.Append("  var data1 = google.visualization.arrayToDataTable([");
            html.Append(" ['BC'," + (_ingTotS - _BS_SubVenta) + ",'" + (_ingTotS - _BS_SubVenta).ToString("C", culture) + " (" + por1 + "%)'],['BCS'," + _BS_SubVenta + ",'" + _BS_SubVenta.ToString("C", culture) + " (" + por2 + "%)']");
            html.Append(" ]);");

            html.Append("  var op_pie_semana = {title: '',pieHole: 0.4,legend:{position:'none'},fontSize:10,chartArea: {left:'2%',top:'1%',bottom:'1%',right:'1%',width:'96%',height:'96%'},pieSliceText: 'label',tooltip:{textStyle:{fontSize:12 }}}; ");
            html.Append(" var content_pie_semana=document.getElementById('semana_pie');");

            if (_ingTotS > 0.00 || _BS_SubVenta > 0.00)
            {
                html.Append(" var view_pie_semana = new google.visualization.PieChart(content_pie_semana); view_pie_semana.draw(data_pie_semana, op_pie_semana);");
            }
            else
            {
                html.Append("content_pie_semana.innerHTML='" + _msg + "';");
            }



            /**********************************************************************************************/



            html.Append(" var data_chart_mes = new google.visualization.DataTable(); data_chart_mes.addColumn('string', 'Meses'); data_chart_mes.addColumn('number', 'Venta');data_chart_mes.addColumn({type: 'string', role: 'tooltip'});data_chart_mes.addColumn('number', 'Facturado');data_chart_mes.addColumn({type: 'string', role: 'tooltip'}); data_chart_mes.addRows([");
            html.Append(_datMes);
            html.Append("]);");

            html.Append(" var op_chart_mes = {legend: { position: 'none' },hAxis: { title: 'Meses',textStyle:{fontSize:9}},vAxis: {title: 'Ingresos',format: 'currency',minValue: 0,logScale: false,textStyle:{fontSize:9}},colors:['#088A29','#DF7401']}; ");
            html.Append(" var content_chart_mes=document.getElementById('content_rep_mes');");
            html.Append(" var view_chart_mes = new google.visualization.LineChart(content_chart_mes);view_chart_mes.draw(data_chart_mes, op_chart_mes);");

            html.Append(" var data_pie_mes = google.visualization.arrayToDataTable([['Localidad', 'Venta',{type:'string',role:'tooltip'}],");

            por1 = Math.Round(((_ingTotM - _BS_SubVenta_mes) * 100) / _ingTotM, 2);
            por2 = Math.Round(((_BS_SubVenta_mes * 100) / _ingTotM), 2);

            html.Append("['BC'," + (_ingTotM - _BS_SubVenta_mes) + ",'" + (_ingTotM - _BS_SubVenta_mes).ToString("C", culture) + " (" + por1 + "%)'],['BCS'," + _BS_SubVenta_mes + ",'" + _BS_SubVenta_mes.ToString("C", culture) + " (" + por2 + "%)']");
            html.Append(" ]);");

            html.Append("  var op_pie_mes = {title: '',pieHole: 0.4,legend:{position:'none'},fontSize:10,chartArea: {left:'2%',top:'1%',bottom:'1%',right:'1%',width:'96%',height:'96%'},pieSliceText: 'label',tooltip:{textStyle:{fontSize:12 }}}; ");
            html.Append(" var content_pie_mes=document.getElementById('mes_pie');");

            if (_ingTotM > 0.00 || _BS_SubVenta_mes > 0.00)
            {
                html.Append(" var view_pie_mes = new google.visualization.PieChart(content_pie_mes); view_pie_mes.draw(data_pie_mes, op_pie_mes);");
            }
            else
            {
                html.Append("view_pie_mes.innerHTML='" + _msg + "';");
            }


            html.Append("} </script>");


            ltRepIngDia.Text = html.ToString();












           /* if (response.Reporte.Dia.Count > 0)
            {*/
                
                
                
                /*
                html.Append(" <script type='text/javascript'>");
                html.Append(" google.load('visualization', '1', { packages: ['bar','corechart'],language:['en'] });");
                html.Append(" google.setOnLoadCallback(drawChart);");

                html.Append(" function drawChart() {");
                html.Append("    var data = google.visualization.arrayToDataTable([");
                html.Append("      ['Sucursales', 'Venta','Facturado'],");
                html.Append(_datos);
                html.Append("    ]);");
             
                html.Append(_medidas);

                html.Append(" var options = {legend:{position:'none'},");
                html.Append(" chart: {position:'none'},bars: 'vertical',");
                html.Append(" vAxis: {format: 'currency', minValue: 0,logScale: true,");
                html.Append(" textStyle:{fontName:'Corbel',italic:false, bold:false,fontSize:10,left:'0%'}},");
                html.Append(" hAxis:{textStyle:{ fontName:'Corbel',italic:false,bold:false,fontSize:10,left:'0%'}},");
                html.Append(" height:height,colors:['#088A29','#DF7401']};");//colors: ['#2E9AFE', '#81BEF7']
                html.Append(" var chart = new google.charts.Bar(document.getElementById('content_rep_dia'));");
                html.Append("  chart.draw(data, google.charts.Bar.convertOptions(options));}");
                html.Append(" </script>");
                

                html.Append("  <script type='text/javascript'>");
                html.Append(" google.setOnLoadCallback(drawChart);");
                html.Append(" function drawChart() {");
                html.Append("  var data1 = google.visualization.arrayToDataTable([");

                var porcentage1 = Math.Round(((_ingTotalDia - _BS_SubVenta_dia) * 100) / _ingTotalDia, 2);
                var porcentage2 = Math.Round(((_BS_SubVenta_dia * 100) / _ingTotalDia), 2);*/

               // var texto=((_ingTotalDia - _BS_SubVenta_dia)/1000).ToString("C",culture)+" ("+porcentage1+"%)";

               // var texto1 = (_BS_SubVenta_dia/1000).ToString("C", culture) + " (" + porcentage2 + "%)";

              /*  html.Append(" ['Localidad', 'Venta',{type:'string',role:'tooltip'}],['BC'," + (_ingTotalDia - _BS_SubVenta_dia) + ",'" + (_ingTotalDia - _BS_SubVenta_dia).ToString("C",culture) +" ("+porcentage1+ "%)'],['BCS'," + _BS_SubVenta_dia + ",'"+_BS_SubVenta_dia.ToString("C",culture)+" ("+porcentage2+"%)']");
                html.Append(" ]);");
                html.Append("  var options1 = {title: '',pieHole: 0.4,legend:{position:'none'},fontSize:10,chartArea: {left:'2%',top:'1%',bottom:'1%',right:'1%',width:'96%',height:'96%'},pieSliceText: 'label',tooltip:{textStyle:{fontSize:12 }}};");
                html.Append(" var chart = new google.visualization.PieChart(document.getElementById('dia_pie'));");
                html.Append(" chart.draw(data1, options1);}");
                html.Append(" </script>");

                ltRepIngDia.Text = html.ToString();*/
               // txtIngDiatotal.Value = _ingTotalDia.ToString("C", culture)+" Pesos.";
               
          //  }



           // var _script = html.ToString();

           
            //html = new StringBuilder();
            //************************************************************************************
        

          //  html = new StringBuilder();

            //*****************************************************************************************
       
            /*

            html.Append(" <script type='text/javascript'>");
            html.Append(" google.load('visualization', '1.1', {packages: ['line','corechart'],language:['en']});");
            html.Append("   google.setOnLoadCallback(drawChart);");

            html.Append("   function drawChart() {");
           
            
            html.Append("   var data = new google.visualization.DataTable();");
            html.Append("   data.addColumn('string', 'Dias');");
            html.Append("   data.addColumn('number', 'Venta');");
            html.Append("   data.addColumn('number', 'Facturado');");
            html.Append("   data.addRows([");
            html.Append(_datSema);

            html.Append("  ]);");



            html.Append("   var data1 = new google.visualization.DataTable();");
            html.Append("   data1.addColumn('string', 'Meses');");
            html.Append("   data1.addColumn('number', 'Venta');");
            html.Append("   data1.addColumn('number', 'Facturado');");
            html.Append("   data1.addRows([");
            html.Append(_datMes);

            html.Append("  ]);");

         
            html.Append(" var contenedor=document.getElementById('content_rep_semana');");
            html.Append(" var contenedor1=document.getElementById('content_rep_mes');");
            html.Append("  var options = {legend: { position: 'none' },");
         
            html.Append("   width: contenedor.width,");
            html.Append("   height: contenedor.height,");
         
            html.Append(" colors:['#088A29','#DF7401'],");
            html.Append(" vAxis: {format: 'currency',minValue: 0,logScale: true,textStyle:{fontSize:9}},");
            html.Append(" hAxis:{textStyle:{fontSize:9}}");
            html.Append("  };");

            if (response.Reporte.Semana != null)
            {
                if (response.Reporte.Semana.Count > 0)
                {
                    html.Append("   var chart = new google.charts.Line(contenedor);");
                    html.Append("   chart.draw(data,google.charts.Line.convertOptions( options));");
                }
            }
            if (response.Reporte.Mes != null)
            {
                if (response.Reporte.Mes.Count > 0)
                {
                    html.Append("   var chart1 = new google.charts.Line(contenedor1);");
                    html.Append("   chart1.draw(data1,google.charts.Line.convertOptions( options));");
                }
            }

            html.Append("   }");
            html.Append("</script>");


            html.Append("  <script type='text/javascript'>");
            html.Append(" google.setOnLoadCallback(drawChart);");
            html.Append(" function drawChart() {");

            var por1 = Math.Round(((_ingTotS - _BS_SubVenta) * 100) / _ingTotS, 2);
            var por2 = Math.Round(((_BS_SubVenta * 100) / _ingTotS), 2);

            html.Append("  var data1 = google.visualization.arrayToDataTable([");
            html.Append(" ['Localidad', 'Venta',{type:'string',role:'tooltip'}],['BC'," + (_ingTotS - _BS_SubVenta) + ",'" + (_ingTotS - _BS_SubVenta).ToString("C", culture) + " (" + por1 + "%)'],['BCS'," + _BS_SubVenta + ",'" + _BS_SubVenta .ToString("C",culture)+" ("+por2 +"%)']");

            por1 = Math.Round(((_ingTotM - _BS_SubVenta_mes) * 100) / _ingTotM, 2);
            por2 = Math.Round(((_BS_SubVenta_mes * 100) / _ingTotM), 2);

            html.Append(" ]);");
            html.Append("  var data = google.visualization.arrayToDataTable([");
         
            html.Append(" ['Localidad', 'Venta',{type:'string',role:'tooltip'}],['BC'," + (_ingTotM - _BS_SubVenta_mes) + ",'" + (_ingTotM - _BS_SubVenta_mes).ToString("C", culture) + " (" + por1 + "%)'],['BCS'," + _BS_SubVenta_mes + ",'" + _BS_SubVenta_mes.ToString("C", culture) + " (" + por2 + "%)']");

            html.Append(" ]);");
            html.Append("  var options1 = {title: '',pieHole: 0.4,legend:{position:'none'},fontSize:10,chartArea: {left:'2%',top:'1%',bottom:'1%',right:'1%',width:'96%',height:'96%'},pieSliceText: 'label',tooltip:{textStyle:{fontSize:12 }} };");
            html.Append(" var chart = new google.visualization.PieChart(document.getElementById('semana_pie'));");
            html.Append(" chart.draw(data1, options1);");
            html.Append(" var chart1 = new google.visualization.PieChart(document.getElementById('mes_pie'));");
            html.Append(" chart1.draw(data, options1);}");

            html.Append(" </script>");





           ltRepSemana.Text = html.ToString();
        
            */
        }



        protected void GetMenu(string CodUser)
        {
            var men = new MenuP();
            men.CodUser = CodUser;
            

            var sv = new ServiceImplementation();

            var tr = new MenuRequest();

            tr.Menu = men;
            tr.MessageOperationType = QSystem.Common.Enums.MessageOperationType.Report; //BackOffice.Common.Enums.MessageOperationType_old.Report;//.MessageOperationType.Report;
            tr.UserIDRqst = Convert.ToInt32(CodUser);

            var response = sv.MenuMessage(tr);
            int App=0;
            int sub_App=0;
            var html = "";

            foreach (MenuP reg in response.Menus)
            {
                var _aplicacion = "";
                var _nomp = "";
                var _pantalla = "";

                if (ValidarCampo(reg.NomAp)) { _aplicacion = culture.TextInfo.ToTitleCase(reg.NomAp.ToLower()); }
                if (ValidarCampo(reg.NomP)) { _nomp = reg.NomP.ToString().Trim(); }
                if (ValidarCampo(reg.DescripcionP)) { _pantalla = culture.TextInfo.ToTitleCase(reg.DescripcionP.ToLower()); }

                if (App != reg.CodAp)
                {
                    if(html!=""){html+="</ul></li>";}
                    html += "<li><a href='#'><i class='fa fa-fw fa-edit'></i>"+_aplicacion+"</a><ul>";
                    App = reg.CodAp;
                }
                if (_nomp == "Dashboard") { Session["DashBoard"] = "1"; } else { Session["DashBoard"] = "0"; }
                if (_nomp != "Dashboard")
                {
                    html += "<li><a href='../" + _nomp + ".aspx' target='_blank'></i>" + _pantalla + "</a></li>";
                }

            }
            html += "</ul></li>";

            content_apps_ul.InnerHtml = html;



            /*
           // string[] PantallasArray=new string[];

            string ul = "";
            string li = "";
            string ul1 = "";
            string li1 = "";
            string cadena = "<ul id='menu2'>";
            int CodA = 0;
            int CodGpo = 0;
            int cont = 0;
           int index =-1;




           foreach (MenuP reg in response.Menus)
           {
               cont++;
               index++;
               if (CodA != reg.CodAp)
               {
                   cadena += ul1 + li1 + ul + li + "<li><a href='#' id='M" + cont + "' onclick='CambiarClase(this.id);'>" + reg.NomAp.ToUpper() + "</a>";
                   cadena += "<ul  id='sub-menu'>";
                   CodA = reg.CodAp;
                   li = "</li>";
                   ul = "</ul>";
               }
               if (reg.NomP != "")
               {
                   cadena += "<li><a id='" + reg.NomAp + "/" + reg.NomP + "," + reg.CodUser + "," + reg.CodAp + "," + reg.CodP + "'  onclick='AddVisita(this.id);'   href='../" + reg.NomP + ".aspx'   target='_blank'>" + reg.DescripcionP + "</a></li>";
               }
               else
               {
                   cadena += "<li><a id='" + reg.NomAp + "/" + reg.NomP + "," + reg.CodUser + "," + reg.CodAp + "," + reg.CodP + "'  onclick='AddVisita(this.id);'   href='#'   target='_blank'>" + reg.DescripcionP + "</a></li>";
               }

               //PantallasArray[index] = reg.NomP;

           }
           // Session.Add("PantallasUser", PantallasArray);

            //Session["PantallasUser"] = PantallasArray;
            cadena += "</ul></li></ul>";
          //  Aplicaciones.InnerHtml = cadena;
             * 
             */

        }
        protected static bool ValidarCampo(object campo)
        {
            var result = true;

            if (campo != null)
            {
                if (Convert.IsDBNull(campo))
                {
                    result = false;
                }
                else
                {
                    if (campo.ToString().Trim().Length == 0)
                    {
                        result = false;
                    }
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        protected void btnRepDia_Click(object sender, EventArgs e)
        {
            //GetIngresos(DateTime.Parse("10/05/2015", culture));
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetIngresos(DateTime.Parse(txtFechaDash.Value , culture), DropDownList1.SelectedValue, DropDownList1.SelectedItem.ToString());
        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
           // GetIngresos(DateTime.Parse("10/05/2015", culture));
        }

        protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetIngresos(DateTime.Parse("10/05/2015", culture));
        }

        protected void btnFecha_Click(object sender, EventArgs e)
        {
           // ClFechaCap.Visible = true;
           /* if (ClFechaCap.Visible)
            {
                ClFechaCap.Visible = false;
            }
            else {
                ClFechaCap.Visible = true;
            }*/
        }

        protected void ClFechaCap_SelectionChanged(object sender, EventArgs e)
        {

           // txtFechaDash.Value = ClFechaCap.SelectedDate.ToShortDateString();
           // GetIngresos(DateTime.Parse(txtFechaDash.Value, culture), DropDownList1.SelectedValue, DropDownList1.SelectedItem.ToString());
            //ClFechaCap.Visible = false;
        }

        protected void btnClick_Rep_Click(object sender, EventArgs e)
        {
            GetIngresos(DateTime.Parse(txtFechaDash.Value, culture), DropDownList1.SelectedValue, DropDownList1.SelectedItem.ToString());
        }
    }
}