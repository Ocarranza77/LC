<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="wfrPermisoSU.aspx.cs" Inherits="QSG.LittleCaesars.Portal.Web.WebForm4" %>
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
   <script type="text/javascript" src="Scripts/wfrPermisoSU.js"></script>



    <SCRIPT type="text/javascript">
        window.history.forward();
        function noBack() { window.history.forward(); }
    </SCRIPT>
</head>
<body onload="noBack();" onpageshow="if (event.persisted) noBack();" onunload="">
          <form id="Form1" runat="server">
        <div id="wrapper" class="wrapSucursales" style="padding-left:0;" >

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

                 <div class="content_buttons" >
                      
                
                    
                      <input type="button" title="Grabar"  class="btnReport btnSave" onclick="Save();"/>
                    <input type="button" title="Limpiar"  class="btnReport btnClear"  onClick="window.location.reload(); return false;"/>
                    <asp:Button id="Reporte" runat="server"  CssClass="btnReport btnBuscar" title="Reporte" OnClick="Reporte_Click" />


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
                     <!--   <div>
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

                         <!--   <label>

                                <span>Usuario
                                    <input id="txtUsuario" runat="server" disabled="disabled"/>
                                    <input type="text" id="txtUserID" runat="server" style="display:none;"/>
                                </span>
                               
                                
                                <span style="width:170px;">Fecha ticket(venta) <img src="../iconos/calendar-icon2.png" title="Seleccionar Fecha" onclick="MostCalendar(event);"/>
                                   <input type="text" id="txtFecha" onkeyup="this.value=formateafecha(this.value);"  maxlength="10" placeholder="dd/mm/aaaa" runat="server"/>
                                </span>
                                <span style="background-color:transparent;" >
                                     <asp:Button ID="btnFecha" runat="server" Text="Button"  style="display:none;" ></asp:Button>
                                    <asp:Calendar ID="ClFechaCap" runat="server" style="position:relative;left:0;margin-left:0;background-color:white;"  BorderWidth="0"  CellPadding="6" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="12pt" ForeColor="White" Height="180px" Width="200px" Visible="false" Font-Underline="false">
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
                         -->
                        <div class="_table" id="content_Reptickets" >
                         <!-- <span style="background-color:transparent;"><span onclick="AddRow(event);">Nueva Cuenta<img /></span></span>-->
                           <ul class="content_table" id="container_history" runat="server" >
                            
                               <li >
                                   <ul>
                                       <li class='column_ID'>No.</li>
                                       <li class="column_Usuario"><img class='_new' onclick='AddRow(event);' />Nombre Usuario</li>
                                       <li class="column_Check"></li>
                                       <li class="column_sucursales"> Sucursales
                                           <ul>
                                               <li class="column_SU"> 01-cucapah</li>
                                                <li class="column_SU"> 02-pajarita</li>
                                                <li class="column_SU"> 03-paseo 2000</li>
                                                <li class="column_SU"> 04-papalote</li>
                                                <li class="column_SU"> 04-fuentes</li>
                                                <li class="column_SU"> 04-diaz ordaz</li>
                                                <li class="column_SU"> 04-prado</li>
                                                <li class="column_SU"> 04-forjadores</li>
                                               <li class="column_SU"> 04-madero</li>
                                               <li class="column_SU"> 04-fundadores</li>
                                               <li class="column_SU"> 04-salinas</li>
                                               <li class="column_SU"> 04-paseo la paz</li>
                                               <li class="column_SU"> 04-los cabos</li>
                                           </ul>
                                       </li>
                                       <li class='column_sttReg' > <select onblur></select></li>
                                    </ul> 

                                   <!--
                                   <li class="row">
                                   <ul>
                                       <li class='column_ID'>No.</li>
                                       <li class="column_Usuario"><input value="carlos godines dominguez"/></li>
                                       <li class="column_Check" ><img class="_unchecked" onclick="check(event);" /></li>
                                        <li class="column_SU"><img class="_unchecked"  onclick="check(event);"/><input value="1"  /> </li>
                                        <li class="column_SU"><img class="_unchecked"  onclick="check(event);"/><input value="2" /></li>
                                        <li class="column_SU"> <img class="_unchecked"  onclick="check(event);"/><input value="3" /></li>
                                        <li class="column_SU"> <img class="_unchecked"  onclick="check(event);"/><input value="4" /></li>
                                        <li class="column_SU"> <img class="_unchecked" onclick="check(event);" /><input value="5" /></li>
                                        <li class="column_SU"><img class="_unchecked"  onclick="check(event);"/><input value="6" /></li>
                                        <li class="column_SU"> <img class="_unchecked"  onclick="check(event);"/><input value="7" /></li>
                                        <li class="column_SU"><img class="_unchecked"  onclick="check(event);"/><input value="8" /></li>
                                        <li class="column_SU"> <img class="_unchecked"  onclick="check(event);"/><input value="9" /></li>
                                        <li class="column_SU"> <img class="_unchecked"  onclick="check(event);"/><input value="10" /></li>
                                        <li class="column_SU"><img class="_unchecked"  onclick="check(event);"/><input value="11" /></li>
                                        <li class="column_SU"> <img class="_unchecked" onclick="check(event);" /><input value="12" /></li>
                                        <li class="column_SU"> <img class="_unchecked"  onclick="check(event);"/><input value="13" /></li>
                                          
                                    </ul> 


                               </li>

                                  <li class="row">
                                   <ul>
                                       <li class='column_ID'>No.</li>
                                       <li class="column_Usuario"><input value="carlos godines dominguez"/></li>
                                       <li class="column_Check"><img class="_unchecked" onclick="check(event);" /></li>
                                        <li class="column_SU"><img class="_unchecked"  onclick="check(event);"/><input value="1"  /> </li>
                                        <li class="column_SU"><img class="_unchecked"  onclick="check(event);"/><input value="2" /></li>
                                        <li class="column_SU"> <img class="_unchecked"  onclick="check(event);"/><input value="3" /></li>
                                        <li class="column_SU"> <img class="_unchecked"  onclick="check(event);"/><input value="4" /></li>
                                        <li class="column_SU"> <img class="_unchecked" onclick="check(event);" /><input value="5" /></li>
                                        <li class="column_SU"><img class="_unchecked"  onclick="check(event);"/><input value="6" /></li>
                                        <li class="column_SU"> <img class="_unchecked"  onclick="check(event);"/><input value="7" /></li>
                                        <li class="column_SU"><img class="_unchecked"  onclick="check(event);"/><input value="8" /></li>
                                        <li class="column_SU"> <img class="_unchecked"  onclick="check(event);"/><input value="9" /></li>
                                        <li class="column_SU"> <img class="_unchecked"  onclick="check(event);"/><input value="10" /></li>
                                        <li class="column_SU"><img class="_unchecked"  onclick="check(event);"/><input value="11" /></li>
                                        <li class="column_SU"> <img class="_unchecked" onclick="check(event);" /><input value="12" /></li>
                                        <li class="column_SU"> <img class="_unchecked"  onclick="check(event);"/><input value="13" /></li>
                                          
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

