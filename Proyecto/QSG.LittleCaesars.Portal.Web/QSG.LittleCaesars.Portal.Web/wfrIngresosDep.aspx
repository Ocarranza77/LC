<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfrIngresosDep.aspx.cs" Inherits="QSG.LittleCaesars.Portal.Web.wfrIngresosDep" %>
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

  

     <link href="../../cssBoostrap/bootstrap.min.css" rel="stylesheet">

    <!-- Custom CSS -->
    <link href="../../cssBoostrap/sb-admin.css" rel="stylesheet">

    <!-- Custom Fonts -->
    <link href="../../cssBoostrap/font-awesome.min.css" rel="stylesheet" type="text/css">
    <!--<script type="text/javascript" src="Portal/js/pScript.js" ></script>-->

   <!-- scripts de ña pagina-->
     <!--<script type="text/javascript" src="../../Scripts/CapTickets.js?n=1"></script>-->
   <script type="text/javascript" src="Scripts/wfrIngresosDep.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"] %>"></script>



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
                      
                
                    
                   <input type="button" title="Grabar"  class="btnReport btnSave" onclick="Save();"/>
                    <input type="button" title="Limpiar"  class="btnReport btnClear"  onClick="window.location.reload(); return false;"/>
                    <asp:Button id="Reporte" runat="server"  CssClass="btnReport btnBuscar"  title="Reporte" OnClick="Reporte_Click" />
                   <!--  <asp:Button ID="btnFCts" runat="server" Text="Button" OnClick="btnFCts_Click" />-->


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
                 <span id="cargando"></span>
                     <div class="_section">
                        <div>
                     
                            <label>
                 
                                <span>Usuario
                                  
                                    <select id="SelectUsers" runat="server" disabled="disabled">

                                    </select>
                                </span>
                               
                                <span style="width:180px;">Fecha de Captura(Z's) <img src="../iconos/calendar-icon2.png" title="Seleccionar Fecha" style="padding:0;" onclick="MostCalendar();"/>
                                   <input type="text"  name="Fecha" id="txtFecha" onkeyup="this.value=formateafecha(this.value);"  maxlength="10" placeholder="dd/mm/aaaa" runat="server"/>
                                </span>
                                <span style="background-color:transparent;" >
                                     <asp:Button ID="btnFecha" runat="server" Text="Button" OnClick="btnFecha_Click" style="display:none;" ></asp:Button>
                                    <asp:Button ID="btnChangeFecha" runat="server" style="display:none;"  />
                                    <asp:Calendar ID="ClFechaCap" runat="server" style="position:relative;left:0;margin-left:0;background-color:white;" OnSelectionChanged="ClFechaCap_SelectionChanged" BorderWidth="0"  CellPadding="6" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="12pt" ForeColor="White" Height="180px" Width="200px" Visible="false" Font-Underline="false">
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
                                
                                <!--<span>Sucursal
                                   <select id="cbxSucursales" runat="server"></select>

                                </span>-->
                                

                            </label>
                            
                        </div>

                    </div>

                   <div class="_tabs">
                       <ul class="content_tabs">
                           <li><input type="button" id="DYIng"  class="_tabactive" value="Daily's Ingresos" onclick="clickTab(event);" /></li>
                            <li><input type="button" id="RepDep" value="Reporte de Depositos" onclick="clickTab(event);" /></li>
                       </ul>

                       <div class="content_tab">
                           
                           <div id="content_DYIng">
                               <div class="_section">
                                   
                                   <div class="_table" style="border:none;">
                                 

                                        <div style="display:inline-block;margin:0 auto;width:auto;max-width:100%;height:auto;overflow:auto;">
                                  
                                            <ul class="content_table" id="content_DYIng_ul" runat="server">
                                                    
                                                <li><ul><li class="column_ID">No.</li>
                                                    <li class="column_Check"><img /></li>
                                                    <li class="column_Sucursal"> Sucursal</li>

                                                     <li class="column_IngDaily">Daily's Ingresos<ul>
                                                            <li class="column_Importe">Gross</li>
                                                             <li class="column_ImpTcredito">T.Credito (P)</li>
                                                             <li class="column_ImpTdebito">T.Debito (P)</li>
                                                            <li  class="column_EfectivoDepP">Efectivo (P) a Dep</li>
                                                            <li  class="column_EfectivoDepD">Efectivo (D) a Dep</li>
                                                            <li class="column_FolioServices">Sev.  Blindados (P)</li>
                                                            <li class="column_FolioServicesD">Sev.  Blindados (D)</li>
                                                            <li class="column_CajeroCorto">cajero corto</li>
                                                            <li class="column_Falt">Faltante (P)</li>

                                                            <li class="column_TotDepValPesos">Tot.Dep a Validar (P)</li>
                                                            <li class="column_TotDepValDolares">Tot.Dep a Validar (D)</li>
                                                         </ul> </li>

                                                    

                                                     <li class="column_espacio"></li>

                                                     <li class="column_Depositos">Depositos<ul>
                                                            <li class="column_TotDepPesos">Tot. Depositos (P)</li>
                                                            <li class="column_TotDepDolares">Tot. Depositos (D)</li>

                                                            <li  class="column_TipoCambio">T.C.</li>

                                                            <li class="column_SaldoDepPesos">Saldos X Dep. (P)</li>
                                                            <li class="column_SaldoDepDolares">Saldos X Dep. (D)</li>

                                                            <li class="column_Deudor">&nbsp;Deudor Nombre</li>
                                                            <li class="column_DeudorP">Deudor Importe (P)</li>
                                                             <li class="column_DeudorD">Deudor Importe (D)</li>
                                                        </ul></li>
                                                     

                                                   
                                                   
                                                	 </ul></li>
                                               
                                        </ul>
                                        </div>
                                            

                                       
                                   </div>

                               </div>
                           </div>

                           <div id="content_Depositos">
                               <div class="_section">
                                
                                   <div class="_table" >


                                        <div style="display:inline-block;margin:0 auto;width:auto;max-width:100%;height:auto;overflow:auto;">
                                            <ul class="content_table" id="content_Depositos_ul" runat="server">
                                            <li><ul>
                                                   <li class="column_ID">No.</li>
                                                    <li class="column_Check"><img /></li>
                                                    <li class="column_Sucursal">Sucursal</li>
                                                     <li class="column_Depositos">Depositos<ul>
                                                             <li class="column_NoSec">No Sec</li>
                                                             <li class="column_Banco">Banco</li>
                                                             <li class="column_Moneda">Moneda</li>
                                                             <li class="column_Cuenta">Cuenta</li>
                                                             <li class="column_FolioDep">Folio </li>
                                                             <li class="column_FechaDep">Fecha </li>
                                                             <li class="column_Importe">Importe </li>
                                                             <li class="column_Nota">Notas</li>
                                                          
                                                         </ul>
                                                         
                                                     </li></ul></li>
                                               
                              
                                        </ul>
                                        </div>
                                            

                                       
                                   </div>

                               </div>

                           </div>


                       </div>
                   </div>





                   
                    </ContentTemplate>
                   
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
        <input id="txtCssAnt" value=""  style="display:none;"/>
        <ul>
           <!-- <li onclick="SelectSTT(event);"><input class="STTN" value="N" disabled="disabled" readonly/> <span>Nuevo</span></li>-->
            <li  onclick="SelectSTT(event);"><input class="STTR" value="R" disabled="disabled" readonly/> <span>Rechazado</span></li>
            <li  onclick="SelectSTT(event);"><input class="STTV" value="V" disabled="disabled" readonly/> <span>Validado</span></li>
            <li  onclick="SelectSTT(event);"><input class="STTT" value="T" disabled="disabled" readonly/> <span>Terminado</span></li>
        </ul>
    </div>


     <div class="panelPopud" id="ADDIng"  >
           <!--  <div ><span> </span><!--<img src="../iconos/cancel-26.png" onclick="Close(event);" title="Cerrar"/> <img src="../iconos/save_as-25.png" onclick="Add(event);" title="Aplicar"/></span>-->
                <div >
                    <input type="text" id="txtIndexRow" value="" style="display:none;" />
                    <input type="text" id="txtIndexEditDep" value="" style="display:none;" />
                    <input type="text" id="txtSucursalID" value="" style="display:none;" />
                   <label>
                       <span style="background-color:transparent; width:74.5%;font-weight:bold;">Captura de Depositos por Daily</span>
                       <span style="background-color:transparent;"><img src="iconos/delete.png" onclick="CloseW(event);" /></span>
                   </label>
                     <label >
                        <span style="background-color:transparent;width:49.5%;" ></span>
                         <!--<span  style="background-color:transparent;"  > </span>-->

                         <span style="margin-left:1px;">Tipo de Cambio</span>
                         <span > <input id="txtTC" type="text" value="16.50" disabled="disabled" readonly/></span>
                   
                    </label>
                     <label >
                         <span  ><input id="txtFechaCap" style="width:50%;float:right;" value="a sep 15" disabled="disabled" readonly/>Fecha:</span>
                         <span style="background-color:transparent;" > <input  id="txtstt" onclick="ChangeSTT2(event);"  style="float:right;width:100%;border-radius:16px;-webkit-border-radius:16px;-moz-border-radius:16px;"   readonly/></span>

                         <span>Sucursal</span>
                         <span > <input id="txtsuc"  disabled="disabled" readonly/></span>
                   
                    </label>
                    <label style="margin-top:10px;">
                        <span style="background-color:transparent;"></span>
                        <span style="background-color:transparent;"></span>
                        <span style="background-color:transparent;font-weight:bold;">Capture Deposito</span>
                        <span style="background-color:transparent;"></span>
                    </label>
                    <label >
                         <span  >Gross</span>
                         <span  > <input id="txtImporte" disabled="disabled" readonly/></span>

                         <span>No. Sec</span>
                         <span > <input id="txtNoSec"  onkeypress="return justNumbers(event);" disabled="disabled" readonly/></span>
                   
                    </label>
                     <label >
                         <span >Tarjeta de Credito (P)</span>
                         <span > <input id="txtTCredito" disabled="disabled" readonly/></span>

                         <span >Banco</span>
                         <span ><select id="cbxBanco" onchange="FindCtas(this.value);" ></select></span>
                   
                    </label>
                     <label >
                         <span  >Tarjeta Debito (P)</span>
                         <span  > <input id="txtTDebito" disabled="disabled" readonly/></span>

                         <span >Moneda</span>
                         <span ><!-- <input id="txtMoneda"  />-->
                             <select id="cbxMoneda" ></select>
                         </span>
                   
                    </label>
                     <label >
                         <span  > Efectivo (P) a Depositar</span>
                         <span  > <input id="txtEfectivoPaDep" disabled="disabled" readonly /></span>

                         <span >Cuenta</span>
                         <span > <select id="cbxCta" ></select></span>
                   
                    </label>
                     <label >
                         <span  >Efectivo (D) a Depositar</span>
                         <span  > <input id="txtefectivoDaDep" disabled="disabled" readonly/></span>

                         <span >Folio Deposito</span>
                         <span > <input type="text" id="txtFolioDep"  /></span>
                   
                    </label>
                     <label >
                         <span >Servicio Blindado (P)</span>
                         <span  > <input id="txtServicesP" disabled="disabled" readonly/></span>

                         <span >Fecha Deposito</span>
                         <span > <input type="text" id="txtFechaDep" onkeyup="this.value=formateafecha(this.value);" maxlength="10" placeholder="dd/mm/yyyy"/></span>
                   
                    </label>
                     <label >
                         <span  >Servicio Blindado (D)</span>
                         <span  > <input id="txtServicesD" disabled="disabled" readonly/></span>

                         <span >Importe Deposito</span>
                         <span > <input type="text" id="txtImpDep"  onkeypress="return justNumbers(event);"/></span>
                   
                    </label>
                     <label >
                         <span  >Cajero Corto</span>
                         <span  > <input id="txtCajeroCorto" disabled="disabled" readonly/></span>

                         <span >Nota</span>
                         <span > <input type="text" id="txtNota"  /></span>
                   
                    </label>

                     <label style="margin-top:4px;">
                         <span >Importe Faltante (P)</span>
                         <span  > <input id="txtImpFaltP" disabled="disabled" readonly/></span>

                        <!-- <span style="width:24.5%; height:25px;"></span>
                         <span style="width:24.5%; height:25px;"> <input style="height:100%;" /></span>-->
                         <span style="background-color:transparent;"></span>
                         <span style="background-color:transparent;"><img id="Img1" src="iconos/ic_plus.png" onclick="AddDeposito(event);" onkeypress="EnterCode(event);"/></span>
                   
                    </label>
                     <label >
                         <span  >Total Dep a Validar (P)</span>
                         <span  > <input id="txtTotalDepaValP" disabled="disabled" readonly/></span>

                         <span >Total (P) Depositos</span>
                         <span > <input id="txtTotalPDep" disabled="disabled" readonly/></span>
                   
                    </label>
                     <label >
                         <span  >Total Dep a Validar (D) </span>
                         <span  > <input id="txtTotalDepaValD" disabled="disabled" readonly/></span>

                         <span >Total (D) Depositos</span>
                         <span > <input id="txtTotalDDep"  disabled="disabled" readonly/></span>
                   
                    </label>
                     <label >
                         <span style="color:red;font-weight:bold;" >Deudor Nombre</span>
                         <span> <input id="txtDeudorNom" /></span>

                         <span style="width:49%;text-align:center;" >Diferencia a Depositar</span>
                         <!--<span style="width:24.5%; height:25px;"> </span>-->
                   
                    </label>
                     <label >
                         <span style="color:red;font-weight:bold;" >Deudor Importe P</span>
                         <span  > <input id="txtImpDeudorP" onkeypress="return justNumbers(event);" onkeyup="Saldo(event);"/></span>

                         <span >Pesos</span>
                         <span > <input id="txtPesos"  disabled="disabled" readonly/></span>
                   
                    </label>
                     <label >
                         <span  style="color:red;font-weight:bold;">Deudor Importe D</span>
                         <span  > <input id="txtImpDeudorD" onkeypress="return justNumbers(event);" onkeyup="Saldo(event);"/></span>

                         <span >Dolares</span>
                         <span > <input id="txtDolares" disabled="disabled" readonly/></span>
                         
                   
                    </label>
                    <label style="margin-top:7px;">
                        
                        <span style="background-color:transparent; float:right;height:30px;"><input type="button" id="btnAceptar" value="Aceptar" onclick="ConfirmDep(event);"/></span>
                        <span style="background-color:transparent; float:right;height:30px;"><input type="button" id="btnEliminar" style="background-color: rgba(128,0,0,.8);color:white;font-weight:bold;" onclick="Eliminar(event);" value="Eliminar Deposito"/></span>
                    </label>

                   
                
                </div>
               <!--  <input type="button" value="Aceptar" style="float:right;width:100px;"/>-->
                 <!--</div>-->
         </div>

     <div class="loaderDiv">
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

     </body>


</html>
