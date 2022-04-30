<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="CapTickets.aspx.cs" Inherits="QSG.LittleCaesars.Portal.Web.WebForm2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="tituloP" runat="server" ><%=NombrePantalla %></title>
   
    <!-- meta-->
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- ligas a estilo y scripts-->
   
    <link rel="stylesheet" type="text/css" href="../../Styles/Style.css" />
   
    <script type="text/javascript" src="../../Scripts/jquery-1.8.3.js"></script>
    
       
    <!--<script type="text/javascript" src="../../Scripts/Portal_inicial.js"></script>-->
    <script type="text/javascript" src="../../Scripts/ScriptGeneral.js"></script>


    <link rel="stylesheet" type="text/css" href="Styles/jquery-ui-1.11.4.custom/jquery-ui.css" />
    <script type="text/javascript" src="Styles/jquery-ui-1.11.4.custom/jquery-ui.js"></script>

  

     <link href="cssBoostrap/bootstrap.min.css" rel="stylesheet">

    <!-- Custom CSS -->
    <link href="cssBoostrap/sb-admin.css" rel="stylesheet">

    <!-- Custom Fonts -->
    <link href="cssBoostrap/font-awesome.min.css" rel="stylesheet" type="text/css">
    <!--<script type="text/javascript" src="Portal/js/pScript.js" ></script>-->

   <!-- scripts de ña pagina-->
     <!--<script type="text/javascript" src="../../Scripts/CapTickets.js?n=1"></script>-->
   <script type="text/javascript" src="Scripts/CapTickets.js"></script>



    <SCRIPT type="text/javascript">
        window.history.forward();
        function noBack() { window.history.forward(); }
    </SCRIPT>
</head>
<body onload="noBack();" onpageshow="if (event.persisted) noBack();" onunload="">
          <form id="Form1" runat="server">
        <div id="wrapper" class="wrapCatalogos" style="padding-left:0;" >

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
                <div class="logo_empresa"><img src="<%=r_logo %>" id="home"/></div><a class="navbar-brand" href="#"><%=Titulo %></a>
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
                    <ul class="dropdown-menu">
                        <li>
                            <a href="#"><i class="fa fa-fw fa-user"></i> Perfil</a>
                        </li>
                        <li>
                            <a href="#"><i class="fa fa-fw fa-gear"></i> Ajustes</a>
                        </li>
                       <!-- <li>
                            <a href="#"><i class="fa fa-fw fa-gear"></i> Settings</a>
                        </li>-->
                        <li class="divider"></li>
                        <li>
                            <a href="#" id="salir"><i class="fa fa-fw fa-sign-out"></i> Salir</a>
                        </li>
                    </ul>
                </li>
            </ul>
            <!-- Sidebar Menu Items - These collapse to the responsive navigation menu on small screens -->
            <div class="collapse navbar-collapse navbar-ex1-collapse" ></div><!--mantener cerrada-->
                 
           

                   
        
            <!-- /.navbar-collapse -->
        </nav>

        <div id="page-wrapper" >
            
            <div class="container-fluid"  >
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>

                 <div class="content_buttons">
                      
                
                    
                      <input type="button" title="Grabar"  class="btnReport btnSave" onclick="SaveP();"/>
                    <input type="button" title="Limpiar"  class="btnReport btnClear"  onClick="window.location.reload(); return false;"/>
                    <asp:Button id="Reporte" runat="server"  CssClass="btnReport btnBuscar" OnClick="Reporte_Click" title="Reporte" />


                </div>
                        <!--
                 <div class="content_filtros" style="border:1px solid blue;">
                    <label>
                        <span>codigo de afiliado
                            <input />
                        </span>
                         <span>sucursales
                            <select>
                                <optgroup>sdfdds</optgroup>
                            </select>
                        </span>
                    </label>            
                 </div>-->

                <div class="content_frmCaptura">
                   <!--  <input id="txtCodeAfiliadoTemp" runat="server" style="width:100px;display:none;" />
                     <input id="txtNombreTemp" runat="server" style="width:100px;display:none;" />
                     <input id="txtPaternoTemp" runat="server" style="width:100px;display:none;" />
                     <input id="txtMaternoTemp" runat="server" style="width:100px;display:none;" />-->
                   
                     <div class="_section">
                        <div>
                      <!--
                            <label>
                                <span class="Cod" >Codigo
                                    <input id="txtCodeAfiliado" runat="server" />
                                </span>
                                <span class="Clave">Clave
                                    <input id="txtClave" runat="server" />
                                </span>
                            <span style="width:26%;">Nombre
                            <input id="txtNombre" runat="server" />
                            </span>
                            <span style="width:26%;">Apellido Paterno
                            <input id="txtApellidoPat" runat="server" />
                            </span>
                            <span style="width:26%">Apellido Materno
                            <input id="txtApellidoMat" runat="server" />
                            </span>
                            </label>
                            -->
                            <label>
                                <!--<asp:TextBox ID="txtFTemp" runat="server"></asp:TextBox>-->
                                <span>Usuario
                                    <input id="txtUsuario" runat="server" disabled="disabled"/>
                                    <input type="text" id="User_id" runat="server" style="display:none;"/>
                                </span>
                               
                                <span style="width:170px;">Fecha ticket(venta) <img src="../iconos/calendar-icon2.png" title="Seleccionar Fecha" onclick="MostCalendar(event);"/>
                                   <input type="text" id="txtFecha" onkeyup="this.value=formateafecha(this.value);"  maxlength="10" placeholder="dd/mm/aaaa" runat="server"/>
                                </span>
                                <span style="background-color:transparent;" >
                                     <asp:Button ID="btnFecha" runat="server" Text="Button" OnClick="btnFecha_Click" style="display:none;" ></asp:Button>
                                    <asp:Calendar ID="ClFechaCap" runat="server" style="position:relative;left:0;margin-left:0;background-color:white;"  OnSelectionChanged="ClFechaCap_SelectionChanged" BorderWidth="0"  CellPadding="6" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="12pt" ForeColor="White" Height="180px" Width="200px" Visible="false" Font-Underline="false">
                                         <DayHeaderStyle Font-Bold="True" Font-Size="10pt" CssClass="dayHead2" ForeColor="Black"/>
                                         <DayStyle CssClass="dayStyle" ForeColor="Black" Font-Underline="false"/>
                                         <NextPrevStyle VerticalAlign="Middle" Font-Size="10pt" Font-Bold="True" />
                                         <OtherMonthDayStyle ForeColor="#808080" />
                                         <SelectedDayStyle BackColor="#6666666" Font-Bold="True" ForeColor="#F16527" />
                                         <SelectorStyle BackColor="#CCCCCC" />
                                     <TitleStyle 
                                         HorizontalAlign="Center" VerticalAlign="Middle"  Font-Bold="True" CssClass="DayHeadStyle"  />
                                         <TodayDayStyle ForeColor="Black" CssClass="toDayHeadStyle" />
                                         <WeekendDayStyle BackColor="#FFFFCC" CssClass="weekend"/>
                                    </asp:Calendar>
                                </span>

                            </label>
                            
                        </div>

                        <div class="_table" id="content_Reptickets" >
                          <span style="background-color:transparent;"><span onclick="addRow(event);">Nuevo Ticket<img /></span></span>
                           <ul class="content_table" id="container_history" runat="server" >
                            
                               <li >
                                   <ul >
                                        <li class='column_ID'>No.</li>
                                        <li class='column_Edit'><img /></li>
                                        <li class='column_Del'><img /></li>
                                        <li class='column_Sucursal'> sucursal</li>
                                        <li class='column_MetodoPago'> Metodo de Pago</li>
                                        <li class='column_FechaT'> Fecha Ticket</li>
                                        <li class='column_HoraT' >Hora Ticket</li>
                                        <li class='column_FolioT'>Folio Ticket</li>
                                        <li class='column_Caja'># Caja</li>
                                        <li class='column_Cajero'>Nombre de Cajero</li>
                                        <li class='column_Importe'>Importe</li>
                                        <li class='column_UserCap'>Usuario Capturo</li>
                                        <li class='column_stt'></li>
                                        <li class='column_sttReg' ><img /></li>		    
                                    </ul> 
                               </li>

                      <!--
                              <li class="rowM">
                               <ul >
                                    <li class='column_ID'>No.</li>
                                    <li class='column_Edit column_fillColor'><img /></li>
                                    <li class='column_Del column_fillColor'><img /></li>
                                    <li class='column_Sucursal'><select><option>sucursal</option></select></li>
                                    <li class='column_FechaT'><input value="01/01/01" /></li>
                                    <li class='column_HoraT' ><input /></li>
                                    <li class='column_FolioT'><input value="dfdfdsfdsfdsfsdfsd" /></li>
                                    <li class='column_Caja'><input /></li>
                                    <li class='column_Cajero'><input /></li>
                                    <li class='column_Importe'><input /></li>
                                    <li class='column_UserCap'><input /></li>
                                    <li class='column_stt'><input /></li>
                                    <li class='column_sttReg' ><img src="../iconos/loanding_min.gif"/></li>		    
                                </ul> 
                               </li>
                               
                               
                               
                               
                                <!-- <li>
                                  <ul>
                                        <li class="column_ID" style="background-color:lightgray;">#</li>
                                        <li class="column_Code">Codigo</li>
                                      <li class="column_clave">Clave</li>
                                        <li class="column_nombre">Nombre </li>
                                        <li class="column_domicilio">Domicilio</li>
                                        <li class="column_telefonos">Telefonos</li>
                                        <li class="column_correos">Correos</li>
                                        <li class="column_abogadoGes"> Gestor</li>
                                       <li class="column_representante">Representante</li>
                                       <li class="column_estatus">Estatus</li>
                                    </ul>

                              </li>
                               <!--
                                <li class="_tableRow">
                                  <ul>
                                     <li class="column_ID" onclick='SelectRow(event);' onmouseout='OutRow(event);' onmouseover='HoverRow(event);'>1</li>
                                       <li class="column_Code" onclick="SelectRow(event);"> 1234</li>
                                        <li class="column_nombre" onclick="SelectRow(event);">carlos gonzales ceuvas</li>
                                        <li class="column_domicilio" onclick="SelectRow(event);">dfdfdsfdsfdsfsdfdsfdsfdsfdsfsdfsdfsdf</li>
                                        <li class="column_telefonos" onclick="SelectRow(event);">123 45 46,123 45 46</li>
                                        <li class="column_correos" onclick="SelectRow(event);">dadasdasdasd@.com.mx,dadasdasdasd@.com.mx</li>
                                        <li class="column_abogadoGes" onclick="SelectRow(event);">carlos bustamnete</li>
                                       <li class="column_representante" onclick="SelectRow(event);">carlos bustamante</li>
                                       <li class="column_estatus" onclick="SelectRow(event);">activo</li>
                                       <li class="column_Temp" ><input value="carlos" /><input value="gonzales" /><input value="ceuvas" /></li>
                                    </ul>

                              </li>
                               <li class="_tableRow">
                                  <ul>
                                     <li class="column_ID">2</li>
                                       <li class="column_Code" onclick="SelectRow(event);"> 555</li>
                                        <li class="column_nombre" onclick="SelectRow(event);">juan vill santiago</li>
                                        <li class="column_domicilio" onclick="SelectRow(event);">dfdfdsfdsfdsfsdfdsfdsfdsfdsfsdfsdfsdf</li>
                                        <li class="column_telefonos" onclick="SelectRow(event);">123 45 46,123 45 46</li>
                                        <li class="column_correos" onclick="SelectRow(event);">dadasdasdasd@.com.mx,dadasdasdasd@.com.mx</li>
                                        <li class="column_abogadoGes" onclick="SelectRow(event);">carlos bustamnete</li>
                                       <li class="column_representante" onclick="SelectRow(event);">carlos bustamante</li>
                                       <li class="column_estatus" onclick="SelectRow(event);">activo</li>
                                     <li class="column_Temp" ><input value="juan" /><input value="vill" /><input value="santiago" /></li>
                                    </ul>

                              </li>
                            -->
                          </ul>


                        </div>

                    </div>

                    </ContentTemplate>
                    <Triggers>
                       
                    </Triggers>
                </asp:UpdatePanel>
               
            </div>

           


            <!-- /.container-fluid -->

        </div>
        <!-- /#page-wrapper -->

            <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                <ProgressTemplate>
                   <div class="loader">
                        <div id="floatingBarsG">
                                    <div class="blockG" id="rotateG_01">
                                    </div>
                                    <div class="blockG" id="rotateG_02">
                                    </div>
                                    <div class="blockG" id="rotateG_03">
                                    </div>
                                    <div class="blockG" id="rotateG_04">
                                    </div>
                                    <div class="blockG" id="rotateG_05">
                                    </div>
                                    <div class="blockG" id="rotateG_06">
                                    </div>
                                    <div class="blockG" id="rotateG_07">
                                    </div>
                                    <div class="blockG" id="rotateG_08">
                                    </div>
                                </div>    
                    </div>
                            
                </ProgressTemplate>
            </asp:UpdateProgress>

    </div>
    <nav class="navbar navbar-inverse navbar-fixed-bottom" style="background-color:transparent;border:none;">
       <div class="footer1">
                     
        <div class="footerDiv">
            <img src="../Images/Logo_Qbic.jpg"/>
        </div>
        <span ></span>
    </div>
    <div class="divLinks">
    <a href="#">Administracion</a>
    <a href="#">Ayuda</a>

    </div>
    </nav>
    <!-- /#wrapper -->

    </form>

     </body>

</html>
