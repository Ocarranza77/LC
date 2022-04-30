<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Portal.aspx.cs" Inherits="QSG.LittleCaesars.Portal.Web.WebForm1" %>
<html>
<head id="Head1"  runat="server">
   <title id="tituloP" runat="server" ></title>
    <!-- meta-->
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- ligas a estilo y scripts-->
    <link rel="stylesheet" type="text/css" href="~/Styles/Style.css" />
    <script type="text/javascript" src="Scripts/jquery-1.8.3.js"></script>  
    <script type="text/javascript" src="Scripts/Portal_inicial.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"] %>"></script>
    <script type="text/javascript" src="Scripts/ScriptGeneral.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"] %>"></script>

    <link href="../../cssBoostrap/bootstrap.min.css" rel="stylesheet">

    <!-- Custom CSS -->
    <link href="../../cssBoostrap/sb-admin.css" rel="stylesheet">

    <!-- Custom Fonts -->
    <link href="../../cssBoostrap/font-awesome.min.css" rel="stylesheet" type="text/css">
    <!--<script type="text/javascript" src="Portal/js/pScript.js" ></script>-->
    
    
    <!-- API Google-->
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

   
    <!--API GOOGLE-->

    <!-- picker-->
    <script type="text/javascript" src="Styles/jquery-ui-1.11.4.custom/jquery-ui.js"></script>

    <link rel="Stylesheet" type="text/css" href="Styles/jquery-ui-1.11.4.custom/jquery-ui.css"  />
     <!-- picker-->

    <!--MAPa -->
    <link rel="Stylesheet" type="text/css" href="mapa/css/style.css" />
     <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?sensor=false"></script>
    <script type="text/javascript" src="mapa/js/map.js"></script>
    


    <SCRIPT type="text/javascript">
        window.history.forward();
        function noBack() { window.history.forward(); }
    </SCRIPT>

</head>
    <body onload="noBack();" onpageshow="if (event.persisted) noBack();" onunload="">
       
         <form id="Form1" runat="server">
         <div id="wrapper">

        <!-- Navigation -->
        <nav class="navbar navbar-inverse navbar-fixed-top" role="navigation">
            <!-- Brand and toggle get grouped for better mobile display -->
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-ex1-collapse">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <div class="logo_empresa"><img src="<%=r_logo %>" /></div><a class="navbar-brand" href="#"><%=Titulo %></a>
            </div>
            <!-- Top Menu Items -->
            <ul class="nav navbar-right top-nav" >
               
                <li class="dropdown" >
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown"><i class="fa fa-bell"></i> <b class="caret"></b></a>
                    <ul class="dropdown-menu alert-dropdown" >
                      
                    </ul>
                </li>
                <li class="dropdown">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown"><i class="fa fa-user"></i> <%=_user %> <b class="caret"></b></a>
                    <ul class="dropdown-menu" >
                        <li>
                            <a href="#" onclick="CrearPanel(event);"><i class="fa fa-fw fa-user"></i> Perfil</a>
                         
                        </li>
                        <li>
                            <a href="#"><i class="fa fa-fw fa-gear"></i> Ajustes</a>
                        </li>
                      
                        <li class="divider"></li>
                        <li>
                            <a href="#" id="salir"><i class="fa fa-fw fa-sign-out"></i> Salir</a>
                        </li>
                    </ul>
                </li>
            </ul>
            <!-- Sidebar Menu Items - These collapse to the responsive navigation menu on small screens -->
            <div class="collapse navbar-collapse navbar-ex1-collapse"  >
                 
                <div id="content_menu" class="side-nav1" runat="server" >
                    <span>Ultimos Accesos</span>
                    <div class="content_lastlinks">
                    
                    </div>
                       
                   
                   <div class="content_apps">
                       <ul class="nav side-nav" id="content_apps_ul" runat="server">
           
                     </ul>
                   </div>
                     <span class="pag_fre" > Mis Favoritos<img src="iconos/ic_plus.png" onclick="open_window(event);" /></span>
                    <div class="content_links">
                       
                        <ul>
                             <li><a href="https://www.google.com" target="_blank"><i class="fa fa-fw fa-globe"></i>www.google.cofm</a><i class="fa fa-fw fa-close" style="float:right;margin-top:2%;cursor:pointer;" onclick="alert();"></i></li>
                        </ul>
                    </div>
                     

                </div>

                   
            </div>
            <!-- /.navbar-collapse -->
        </nav>

        <div id="page-wrapper" >
            
            <div class="container-fluid"  >

               
                <div class="row" id="content_rep_ingresos" runat="server">
                    
                    <div id="content_header_dashboard" class="content_header_dashboard">
                       
                        <div>
                            <span>TABLERO DE INGRESOS / DASHBOARD </span>

                            <span>Sucursal
                                <!--<select>
                                    <option>cucapah</option>
                                </select>-->
                                 <asp:DropDownList ID="DropDownList1" CssClass="classCBX" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                
                            </span>
                            <asp:Button ID="btnClick_Rep" runat="server" Text="Button" OnClick="btnClick_Rep_Click" style="display:none;" />
                           <span >Fecha <img src="../iconos/calendar-icon2.png" title="Seleccionar Fecha" onclick="MostCalendar(event);"/>
                                   <input type="text" id="txtFechaDash" style="width:100px;"  onkeyup="this.value=formateafecha(this.value);"  maxlength="10" placeholder="dd/mm/aaaa" runat="server"/>
                                </span>
                           
                            
                            
                        </div>
                        <img src="iconos/tools.png" title="Configurar Dashboard" />
                        <img src="iconos/filter_report.png"  title="Ir al Reporte Completo"/>
                        <input type="text" name="venta" value="venta" />
                        <input type="text" name="facturado" value="facturado" />
                    </div>

                    <div class="content_dashboard">
                         
                        <div>
                            <div class="content_graficas">
                                <!-- Grafica x Dia-->
                                <asp:Literal ID="ltRepIngDia" runat="server"></asp:Literal>
                                <div>
                                    <div id="content_grafica_dia">
               
                                        <span><%=_titleDia %></span>
                                         <div id="content_rep_dia" ></div>
                                    </div>

                                    <div id="content_pie_dia">
                                        <span>Ventas por localizacion BC-BCS</span>
                                        <div id="dia_pie">

                                        </div>
                                    </div>

                                </div>

                                 <!-- Grafica x semana-->

                          

                                <div>
                                    <div id="content_grafica_semana">
               
                                        <span><%=_titleSemana %></span>
                                         <div id="content_rep_semana" ></div>
                                    </div>

                                    <div id="content_pie_semana">
                                        <span>Ventas por localizacion BC-BCS</span>
                                        <div id="semana_pie">

                                        </div>
                                    </div>

                                </div>
                                 <!-- Grafica x Mes-->
                                <div>
                                    <div id="content_grafica_mes">
               
                                        <span><%=_titleMes %></span>
                                         <div id="content_rep_mes" ></div>
                                    </div>

                                    <div id="content_pie_mes">
                                        <span>Ventas por localizacion BC-BCS</span>
                                        <div id="mes_pie">

                                        </div>
                                    </div>

                                </div>
                                <!--mapa-->
                                 <div>
                                    <div id="content_mapa">
                                         <div id="map-canvas" style="width:100%;height:100%;"></div>
                                    </div>
                                        
                                    
                                     
                              
                                


                                 </div>
                               
                            </div>
                          

                        </div>
                       


                       
                       

                    </div>
                  

                     
                     
               
           
            </div>
               
            <!-- /.container-fluid -->

        </div>
        <!-- /#page-wrapper -->

            

    </div>
           
    <nav class="navbar navbar-inverse navbar-fixed-bottom" style="background-color:transparent;border:none;">
       <div class="footer1">
                     
        <div class="footerDiv">
            <img src="Images/Logo_Qbic.jpg"/>
        </div>
        <span ></span>
    </div>
    <div class="divLinks">
    <a href="#">Administracion</a>
    <a href="#">Ayuda</a>

    </div>
    </nav>
    <!-- /#wrapper -->

         <div id="contetn_button_slider" class="contetn_button_slider" runat="server"> 
              <img id="ocultar"  title="Ocultar"  />
                
            
         </div>
        <div id="content_button_dash" class="content_button_dash" runat="server">
            <img id="ocultar_dash" />
            <span>Ocultar Dashboard</span>
        </div>
                    
        
             <div id="Panel_Links">
                <input /><img onclick="close_addLink(event);"/>
            </div>

        <div class="Panel_password">
            <span>&nbsp;Contraseña Anterior
                <input type="password" id="txtoldpass" />
            </span>
            <span>Nueva Contraseña
                <input type="password" id="txtnewpass" onkeyup="return passwordChanged(event);"  />
            </span>
            <span>Confirme Contraseña
                <input type="password" id="txtconfirpass" onkeyup="return passwordChanged(event);" />
            </span>
           
            <input type="button" id="btnPsw" value="Cambiar" onclick="CambiarPSW();"/>
        </div>

           





         <div class="content_msg" >
             <div>
                 <span>&nbsp;
                     Mensaje
                     <input id="txtTypeMsgID" style="display:none;" readonly/>
                 </span>
                 <textarea disabled="disabled"  readonly>
                     
                    
                 </textarea>
                <input type="button" value="Aceptar" onclick="Aceptar(event);" />
             </div>
         </div>
            
     
       <div class="content_buttos_dash" id="content_buttos_dash" runat="server">
       
           <div>
           
               <ul>
                    <li  onclick="select(event);">ventas</li>
                    <li onclick="select(event);" class="selectDashboard">ingresos</li>
                    <li onclick="select(event);">egresos</li>
                    <li onclick="select(event);">cuenta por pagar</li>
                   <li onclick="select(event);">cash flow</li>
                   <li onclick="select(event);">estados financieros</li>
                   <li onclick="select(event);">inventarios</li>
                   <li onclick="select(event);">recursos humanos</li>
                </ul>
           
           </div>
          
        
       </div>


         </form>
            </body>
    </html>


