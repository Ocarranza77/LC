<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="CapZDaylis.aspx.cs" Inherits="QSG.LittleCaesars.Portal.Web.WebForm7" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="tituloP" runat="server" ><%=NombrePantalla %></title>
   
    <!-- meta-->
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- ligas a estilo y scripts-->
   
    <link rel="stylesheet" type="text/css" href="../../Styles/Style.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"] %>" />
   
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
   <script type="text/javascript" src="Scripts/CapZDays.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"] %>"></script>



    <SCRIPT type="text/javascript">
        window.history.forward();
        function noBack() { window.history.forward(); }
    </SCRIPT>
</head>
<body onload="noBack();" onpageshow="if (event.persisted) noBack();" onunload="">
          <form id="Form1" runat="server">
        <div id="wrapper" class="wrapDailys" style="padding-left:0;" >

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
                      
                
                    
                   <input type="button" title="Grabar"  class="btnReport btnSave" onclick="SaveIndormacion();"/>
                    <input type="button" title="Limpiar"  class="btnReport btnClear"  onClick="window.location.reload(); return false;"/>
                    <asp:Button id="Reporte" runat="server"  CssClass="btnReport btnBuscar" OnClick="RunReporte" title="Reporte" />
                     <asp:Button ID="btnDownload" runat="server" CssClass="btnReport btnDownload" OnClick="btnDownload_Click"  />


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

                <div class="content_frmCaptura" >
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
                                   <!-- <input id="txtUsuario" runat="server" disabled="disabled"/>
                                    <input type="text" id="User_id" runat="server" style="display:none;"/>-->
                                    <select id="SelectUsers" runat="server" disabled="disabled">

                                    </select>
                                </span>
                               
                                <span style="width:180px;">Fecha de Captura(Z's) <img src="../iconos/calendar-icon2.png" title="Seleccionar Fecha" onclick="MostCalendar();" style="padding:0;"/>
                                   <input type="text"  name="Fecha" id="txtFecha" onkeyup="this.value=formateafecha(this.value);" onkeypress="EnFecha(event);" onclick=" ChangeFecha(event);"  maxlength="10" placeholder="dd/mm/aaaa" runat="server"/>
                                </span>
                                <span style="background-color:transparent;" >
                                     <asp:Button ID="btnFecha" runat="server" Text="Button" OnClick="btnFecha_Click1" style="display:none;" ></asp:Button>
                                    <asp:Button ID="btnChangeFecha" runat="server" OnClick="btnChangeFecha_Click" style="display:none;"  />
                                    <asp:Calendar ID="ClFechaCap" runat="server" style="position:relative;left:0;margin-left:0;background-color:white;" OnSelectionChanged="ClFechaCap_SelectionChanged3" BorderWidth="0"  CellPadding="6" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="12pt" ForeColor="White" Height="180px" Width="200px" Visible="false" Font-Underline="false">
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

                                <span style="float:right; width:85px;">Tipo cambio
                                    <input id="TipoC" value="$15.00" onkeypress="return justNumbers(event);"/>

                                </span>
                            </label>
                            
                        </div>

                         <!--

                        <div class="_table" id="content_Reptickets" >
                          <span style="background-color:transparent;"><span onclick="addRow(event);">Nuevo Ticket<img /></span></span>
                           <ul class="content_table" id="container_history" runat="server" >
                            
                               <li >
                                   <ul >
                                        <li class='column_ID'>No.</li>
                                        <li class='column_Edit'><img /></li>
                                        <li class='column_Del'><img /></li>
                                        <li class='column_Sucursal'> sucursal</li>
                                        <li class='column_FechaT'> Fecha Ticket</li>
                                        <li class='column_HoraT' >Hora Ticket</li>
                                        <li class='column_FolioT'>Folio Ticket</li>
                                        <li class='column_Caja'># Caja</li>
                                        <li class='column_Cajero'>Nombre de Cajero</li>
                                        <li class='column_Importe'>Importe</li>
                                        <li class='column_UserCap'>Usuario Capturo</li>
                                        <li class='column_stt'></li>
                                        <li class='column_sttReg' ><img  /></li>		    
                                    </ul> 
                               </li>

                          </ul>


                        </div>
                         -->

                    </div>

                   <div class="_tabs">
                       <ul class="content_tabs">
                           <li><input type="button" id="RZ"  class="_tabactive" value="Registro de Z's" onclick="clickTab(event);" /></li>
                            <li><input type="button" id="DSummary" value="Daily Cash Summary" onclick="clickTab(event);" /></li>
                       </ul>

                       <div class="content_tab">
                           
                           <div id="content_RegZ">
                               <div class="_section">
                                   
                                   <div class="_table">
                                       <span style="background-color:transparent;"><span onclick="ADDRow(event);">Nueva Z<img /></span></span>

                                        <div style="display:inline-block;margin:0 auto;width:auto;max-width:100%;height:auto;overflow:auto;">
                                  
                                            <ul class="content_table" id="content_RegZ_ul" runat="server">
                                            <li>
                                                 <ul>
                                                    <li class="column_ID">No.</li>
                                                    <li class="column_Edit"><img/></li>
                                                    <li class="column_Del"><img /></li>
                                                    <li class="column_Sucursal"> Sucursal</li>
                                                    <li class="column_FechaT" >Fecha Z</li>
                                                    <li class="column_HoraT">Hora Cierre Z</li>
                                                    <li class="column_FolioT">Folio Z</li>
                                                    <li class="column_Caja">Num. Caja</li>
                                                    <li class="column_NT">Num. Transacc.</li>
                                                    <li class="column_Cajero">Cajero(s)</li>
                                                    <li class="column_ImpEfectivo">Efectivo(cash)</li>
                                                    <li class="column_ImpTcredito">T.Credito(Card)</li>
                                                    <li class="column_ImpTdebito">T.Debito(Card)</li>
                                                    <li class="column_OFPago">Otra Forma de Pago</li>
                                                    <li class="column_Importe">Importe(Grss)</li>
                                                    <li class='column_sttReg'  ></li>		
                                                </ul>

                                            </li>
<!--
                                                <li>
                                                 <ul>
                                                    <li class="column_ID">252</li>
                                                    <li class="column_Edit column_fillColor"><img /></li>
                                                    <li class="column_Del column_fillColor"><img /></li>
                                                    <li class="column_Sucursal"> <select></select></li>
                                                    <li class="column_FechaT" ><input /></li>
                                                    <li class="column_HoraT"> <input /></li>
                                                    <li class="column_FolioT"><input /></li>
                                                    <li class="column_Caja"><input /></li>
                                                    <li class="column_NT"><input /></li>
                                                    <li class="column_Cajero"><input /></li>
                                                    <li class="column_ImpEfectivo"><input /></li>
                                                    <li class="column_ImpTcredito"><input /></li>
                                                     <li class="column_ImpTdebito"><input /></li>
                                                    <li class="column_OFPago"><input /></li>
                                                    <li class="column_Importe"><input /></li>
                                                    <li class='column_sttReg'><img src="../iconos/loanding_min.gif" /></li>		
                                                </ul>

                                            </li>
                                                 <li>
                                                 <ul>
                                                    <li class="column_ID">252</li>
                                                    <li class="column_Edit column_fillColor"><img /></li>
                                                    <li class="column_Del column_fillColor"><img /></li>
                                                    <li class="column_Sucursal"> <select></select></li>
                                                    <li class="column_FechaT" ><input /></li>
                                                    <li class="column_HoraT"> <input /></li>
                                                    <li class="column_FolioT"><input /></li>
                                                    <li class="column_Caja"><input /></li>
                                                    <li class="column_NT"><input /></li>
                                                    <li class="column_Cajero"><input /></li>
                                                    <li class="column_ImpEfectivo"><input /></li>
                                                    <li class="column_ImpTcredito"><input /></li>
                                                     <li class="column_ImpTdebito"><input /></li>
                                                    <li class="column_OFPago"><input /></li>
                                                    <li class="column_Importe"><input /></li>
                                                    <li class='column_sttReg'></li>		
                                                </ul>

                                            </li>-->


                                     <li class="Rowfooter">
                                        <ul >
                                             <li class="column_stt" style="color:white;"></li>
                                             <li class="column_CodeRegZ" style="color:white;"></li>
                                             <li class="row_ID" style="color:white;"></li>
                                            <li class="column_ID" style="color:white;"></li>
                                            <li class="column_Edit" style="background-color:white;" ><img /></li>
                                            <li class="column_Del" style="background-color:white;"><img /></li>
                                            <li class="column_Sucursal"></li>
                                            <li class="column_FechaT" ></li>
                                            <li class="column_HoraT"></li>
                                            <li class="column_FolioT"></li>
                                            <li class="column_Caja"></li>
                                            <li class="column_NT"></li>
                                            <li class="column_Cajero" style="font-weight:bold;">Sub Totales</li>
                                            <li class="column_ImpEfectivo"><input value="0"  readonly/></li>
                                            <li class="column_ImpTcredito"><input value="0" readonly/></li>
                                             <li class="column_ImpTdebito"><input value="0" readonly/></li>
                                            <li class="column_OFPago"><input value="0" readonly/></li>
                                            <li class="column_Importe"><input value="0" readonly/></li>
                                             <li class='column_sttReg' style="color:white;" >[]</li>		
                                        </ul>
                                    </li> 
                                        </ul>
                                        </div>
                                            

                                       
                                   </div>

                               </div>
                           </div>

                           <div id="content_Daily">
                               <div class="_section">
                                   
                                   <div class="_table" >
                                       <!--<span style="background-color:transparent;"><span onclick="ADDRow(event);">Nueva Z<img /></span></span>-->

                                        <div style="display:inline-block;margin:0 auto;width:auto;max-width:100%;height:auto;overflow:auto;">
                                            <ul class="content_table" id="content_Daily_ul" runat="server">
                                            <li>
                                                 <ul>
                                                    <li class="column_ID">No.</li>
                                                    <li class="column_Check"><img /></li>
                                                    <li class="column_Edit"><img class='btnEdit' src='iconos/ic_edit1.png' title='Editar registro'/></li>
                                                    <li class="column_Sucursal">Sucursal</li>
                                                    <li class="column_Supervisor" >Supervisor</li>
                                                    <li class="column_FechaDaily" >Fecha Daily</li>
                                                    <li class="column_NumZ">No.Z</li>
                                                    <li class="column_TotalIng">Total Ingresos</li>

                                                    <li class="column_ImpTcredito">T.Credito(Card)</li>
                                                    <li class="column_ImpTdebito">T.Debito(Card)</li>
                                                    <li class="column_OFPago">Otra Forma de Pago</li>

                                                    <li class="column_TotalEfectivo">Total Efectivo Daily</li>
                                                    <li class="column_TotalEfectZ">Total Efectivo Z </li>
                                                    <li class="column_VariacionDayli">Variacion Daily (Debe ser cero)</li>

                                                    <li  class="column_TipoCambio">T.C.</li>
                                                    <li class="column_EfecDeposito">Efectivo para Deposito
                                                        <ul >
                                                            <li  class="column_EfectivoDepP">Pesos</li>
                                                            <li  class="column_EfectivoDepD">Dolares</li>
                                                 
                                                            <li  class="column_EfectivoDepPConv">Conversion Pesos</li>
                                                        </ul>
                                                    </li>
                                            
                                                    <li class="column_BosalDeposito">Bolsa(s) Servicio Blindado
                                                        <ul>
                                                             <li class="column_BolsaP">Pesos</li>
                                                             <li class="column_BolsaD">Dolares</li>
                                                     
                                                            <li class="column_BolsaPConv">Conversion Pesos</li>
                                                        </ul>
                                                    </li>
                                                    <li class="column_FolioServices">Folio Services</li>
                                                    <li class="column_GastoDeu">Gastos(a deudores)</li>
                                                    <li class="column_Sob">Sobrantes</li>
                                                    <li class="column_CajeroCorto">cajero corto</li>
                                                     <li class="column_Falt">Faltantes</li>

                                            
                                                <li class="column_ComentariosDayli"><img src="iconos/msgNew.png" /></li>

                                        </ul>

                                            </li>

                                          <!--
                                                <li>
                                                 <ul>
                                                    <li class="column_ID">23</li>
                                                       <li class="column_Check column_fillColor"><img class="STTN" onclick="ChangeSTT(event);" /></li>
                                                    <li class="column_Edit column_fillColor"><img /></li>
                                                    <li class="column_Sucursal"><select></select></li>
                                                    <li class="column_Supervisor" ><input value="juan camronas"/></li>
                                                    <li class="column_FechaDaily" ><input /></li>
                                                    <li class="column_NumZ"><input /></li>
                                                    <li class="column_TotalIng"><input /></li>

                                                    <li class="column_ImpTcredito"><input /></li>
                                                    <li class="column_ImpTdebito"><input /></li>
                                                    <li class="column_OFPago"><input /></li>

                                                    <li class="column_TotalEfectivo"><input /></li>
                                                    <li class="column_TotalEfectZ"><input /> </li>
                                                    <li class="column_VariacionDayli"><input /></li>

                                                    <li  class="column_TipoCambio"><input /></li>
                                               
                                               
                                                    <li  class="column_EfectivoDepP"><input /></li>
                                                    <li  class="column_EfectivoDepD"><input /></li>
                                                 
                                                    <li  class="column_EfectivoDepPConv"><input /></li>
                                              
                                             
                                            
                                                 
                                                    <li class="column_BolsaP"><input /></li>
                                                    <li class="column_BolsaD"><input /></li>
                                                     
                                                <li class="column_BolsaPConv"><input /></li>
                                                   
                                                    <li class="column_FolioServices"><input /></li>
                                                    <li class="column_GastoDeu"><input /></li>
                                                    <li class="column_Sob"><input /></li>
                                                    <li class="column_CajeroCorto"><input /></li>
                                                     <li class="column_Falt"><input /></li>

                                            
                                                <li class="column_ComentariosDayli"><img src="iconos/msgNew.png" /></li>

                                        </ul>

                                            </li><!--

                                      <li>
                                                 <ul>
                                                    <li class="column_ID">23</li>
                                                    <li class="column_Check column_fillColor"><img class="STTnuevo" /></li>
                                                    <li class="column_Edit column_fillColor"><img /></li>
                                                    <li class="column_Sucursal"><select></select></input_></li>
                                                    <li class="column_Supervisor" ><input /></li>
                                                    <li class="column_FechaDaily" ><input /></li>
                                                    <li class="column_NumZ"><input /></li>
                                                    <li class="column_TotalIng"><input /></li>

                                                    <li class="column_ImpTcredito"><input /></li>
                                                    <li class="column_ImpTdebito"><input /></li>
                                                    <li class="column_OFPago"><input /></li>

                                                    <li class="column_TotalEfectivo"><input /></li>
                                                    <li class="column_TotalEfectZ"><input /> </li>
                                                    <li class="column_VariacionDayli"><input /></li>

                                                    <li  class="column_TipoCambio"><input /></li>
                                               
                                               
                                                    <li  class="column_EfectivoDepP"><input /></li>
                                                    <li  class="column_EfectivoDepD"><input /></li>
                                                 
                                                    <li  class="column_EfectivoDepPConv"><input /></li>
                                              
                                             
                                            
                                                 
                                                    <li class="column_BolsaP"><input /></li>
                                                    <li class="column_BolsaD"><input /></li>
                                                     
                                                <li class="column_BolsaPConv"><input /></li>
                                                   
                                                    <li class="column_FolioServices"><input /></li>
                                                    <li class="column_GastoDeu"><input /></li>
                                                    <li class="column_Sob"><input /></li>
                                                    <li class="column_CajeroCorto"><input /></li>
                                                     <li class="column_Falt"><input /></li>

                                            
                                                <li class="column_ComentariosDayli"><img src="iconos/msgNew.png" /></li>

                                        </ul>

                                            </li>
                                                
                                        -->
                                <li class="Rowfooter">
                                        <ul>
                                             <li class="column_stt" style="color:white;"></li>
                                             <li class="column_CodeRegZ" style="color:white;"></li>
                                             <li class="row_ID" style="color:white;"></li>
                                            <li class="column_ID" style="color:white;"></li>
                                             <li class="column_Check"><img  /></li>
                                            <li class="column_Edit" style="background-color:white;"></li>

                                            <li class="column_Sucursal" style="font-weight:bold;">
                                               Total Ingreso del dia.
                                            </li>
                                             <li class="column_Supervisor" ></li>
                                            <li class="column_FechaDaily" ></li>
                                            <li class="column_NumZ"></li>

                                            <li class="column_TotalIng"><input  value="0"  readonly="readonly"/></li>
                                           
                                             <li class="column_ImpTcredito"><input  value="0"  readonly/></li>
                                             <li class="column_ImpTdebito"><input value="0" readonly /></li>
                                            <li class="column_OFPago"><input  value="0"  readonly/></li>

                                            <li class="column_TotalEfectivo"></li>
                                            <li class="column_TotalEfectZ"><input value="0"  readonly/></li>
                                            <li class="column_VariacionDayli"></li>

                                            <li class="column_TipoCambio"></li>
                                            <li class="column_EfectivoDepP"><input  value="0" readonly/></li>
                                            <li class="column_EfectivoDepD"><input value="0" readonly/></li>
                                             
                                             <li class="column_EfectivoDepPConv"></li>
                                            
                                            <li class="column_BolsaP"><input  value="0"  readonly/></li>
                                            <li class="column_BolsaD"><input  value="0"  readonly/></li>
                                             
                                             <li class="column_BolsaPConv"></li>
                                            <li class="column_FolioServices"></li>

                                            <li class="column_GastoDeu"><input  value="0"  readonly/></li>
                                            <li class="column_Sob"></li>
                                             <li class="column_CajeroCorto"></li>
                                            <li class="column_Falt"></li>
                                            
                                            <li class="column_ComentariosDayli"></li>

                                        </ul>
                                    </li> 

                                              
                                        </ul>
                                        </div>
                                            

                                       
                                   </div>

                               </div>

                           </div>


                       </div>
                   </div>





                   
                    </ContentTemplate>
                    <Triggers>
                    
                       <asp:PostBackTrigger ControlID="btnDownload"  />
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

    <div class="PanelEstatus">
        <input id="txtSttID" type="text" style="display:none;"/>
        <ul>
            <li onclick="SelectSTT(event);"><img class="STTN" /> <span>Nuevo</span></li>
            <li  onclick="SelectSTT(event);"><img class="STTR" /> <span>Rechazado</span></li>
            <li  onclick="SelectSTT(event);"><img class="STTV" /> <span>Validado</span></li>
            <li  onclick="SelectSTT(event);"><img class="STTT" /> <span>Terminado</span></li>
        </ul>
    </div>

      <div id="WindowComent">
          
              <!-- <span>  <img src="iconos/delete.png" onclick="Close(event);" /> </span> -->
               <span>
                    <img src="iconos/delete.png" onclick="Close(event);" />
                    <textarea  id="textcoment" cols="15" rows="2"></textarea> 

               </span> 
                <span><input type="button" value="Comentar" onclick="Comentar(event);" /> </span> 
       </div>

     </body>


</html>
