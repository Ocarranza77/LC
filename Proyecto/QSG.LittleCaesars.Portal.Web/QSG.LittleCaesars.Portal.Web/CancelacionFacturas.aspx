<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="CancelacionFacturas.aspx.cs" Inherits="QSG.LittleCaesars.Portal.Web.WebForm6" %>
<!DOCTYPE html  PUBLIC "-//W3C//DTD XHTML5//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
    <head id="head1" >
        <meta http-equiv="pragma" content="no-cache" />
        <title id="tituloP"  >Cancelacion de Facturas al SAT</title>
        <link rel="Stylesheet" type="text/css" href="Styles/Style_caesars_pizza.css"/>
        <!--<link rel="Stylesheet"  type="text/css" href="Styles/dataTables.bootstrap.css"/>
        <link rel="Stylesheet" type="text/css" href="Styles/bootstrap.css"/>-->
        <script type="text/javascript" src="Scripts/jquery-1.8.3.js"></script>
        <script type="text/javascript" src="Scripts/ScriptGeneral.js"></script>
        <script type="text/javascript" src="Scripts/CancelacionFact.js?n=1" ></script>
      
    </head>

    <body>
       
        <form id="Form1"  runat="server" >
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
           
              <div class="content_portal">
                   <header class="content_header">
                        <img src="Images/little.png" title="Home" id="home"/>
                        <input  id="ruta_app" style="color:white;font-size:10pt;border:none;background-color:transparent;margin:10px;width:250px;" runat=server value="Little Caesars" readonly/>
                        <ul>
                            <li><img src="iconos/<%=Session["Puesto"] %>.png" /> <span id="NickName" runat="server"></span></li>
                            <li id="salir" >Salir</li>
                        </ul>
                    </header>

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <div class="content_buttons">
                    <!--<input type="button" title="Grabar" style="background-image:url('../../iconos/save_as-25.png');" class="btnReport" onclick="SaveP();"/>-->
                    <!--<input type="button" title="Limpiar" style="background-image:url('../../iconos/eraser-25.png');" class="btnReport"  onClick="window.location.reload(); return false;"/>-->
                        <asp:Button ID="Button1" runat="server" title="Limpiar" style="background-image:url('../../iconos/eraser-25.png');" class="btnReport" OnClick="Button1_Click"/>
                    <asp:Button id="Reporte" runat="server" style="background-image:url('../../iconos/details-25.png');"  CssClass="btnReport" OnClick="GetReporte" title="Reporte" />
                    </div>
                        <div class="Content_rep_facturas">
                            <div class="content_filtros">
                                <label>
                            <span style="width:150px;">Fecha de Facturacion</span>
                            <asp:TextBox ID="txtfechaini" runat="server" Width="120px" Height="16px" CssClass="inputTXT" ReadOnly></asp:TextBox>
                            <asp:Button ID="btnFechaIni" runat="server"   OnClick="btnFechaIni_click" style="background-image:url('../../iconos/date-icon.png');background-repeat:no-repeat;border:none;cursor:pointer;float:left;margin-left:2px;" Width="25px" Height="24px" BorderStyle="None" />

                            
                            <asp:Calendar ID="ClFechaCap" runat="server" style="position:relative;left:2px;margin-left:5px;background-color:transparent;" OnSelectionChanged="ClFechaCap_SelectionChanged" BorderWidth="0"  CellPadding="6" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="12pt" ForeColor="White" Height="180px" Width="200px" Visible="false">
                                 <DayHeaderStyle Font-Bold="True" Font-Size="10pt" CssClass="dayHead2" ForeColor="Black"/>
                                 <DayStyle CssClass="dayStyle" ForeColor="Black"/>
                                 <NextPrevStyle VerticalAlign="Middle" Font-Size="10pt" Font-Bold="True" />
                                 <OtherMonthDayStyle ForeColor="#808080" />
                                 <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                 <SelectorStyle BackColor="#CCCCCC" />
                             <TitleStyle 
                                 HorizontalAlign="Center" VerticalAlign="Middle"  Font-Bold="True" CssClass="DayHeadStyle"  />
                                 <TodayDayStyle ForeColor="Black" CssClass="toDayHeadStyle" />
                                 <WeekendDayStyle BackColor="#FFFFCC" CssClass="weekend"/>
                            </asp:Calendar>

                            
                           

                            </label>
                                <label>
                                    <span style="float:right;">Sucursal
                               
                                             <select id="SelectSucursales" runat="server">
                                 

                                                 </select>

                            </span>
                                </label>
                                
                            </div>
                       
                        <%=Expcert  %> 
                            <%=QTYTimbres %>
                           
                               <div class="content_repT" id="content_repT" runat="server">
                            
                                



                                 
                              <ul id="container_history">
                                <li>
                                    <ul>
                                        <li class="column_ID">ID</li>
                                        <li class="column_fechaFact">Fecha</li>
                                        <li class="column_FolioT">Ticket ID</li>
                                        <li class="column_folioFact">Folio</li>
                                        <li class="column_RFCfact">RFC</li>
                                        <li class="column_Importe">Importe</li>
                                        <li class="column_STTFact">Estatus</li>
                                        <li class="column_UUID">UUID</li>
                                         <li class='column_MotC'>Detalle Cancelacion</li>
                                    </ul>
                                </li>
                                



                            </ul>



                        </div>

                        </div>

           




                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                    <div class="loader">
                       <!-- <div class="windows8">
                        <div class="wBall" id="wBall_1">
                        <div class="wInnerBall">
                        </div>
                        </div>
                        <div class="wBall" id="wBall_2">
                        <div class="wInnerBall">
                        </div>
                        </div>
                        <div class="wBall" id="wBall_3">
                        <div class="wInnerBall">
                        </div>
                        </div>
                        <div class="wBall" id="wBall_4">
                        <div class="wInnerBall">
                        </div>
                        </div>
                        <div class="wBall" id="wBall_5">
                        <div class="wInnerBall">
                        </div>
                        </div>
                        </div>   -->
                        
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
            <footer >
            <div class="footer1">
                     
                <div class="footerDiv">
                    <img src="Images/Logo_Qbic.jpg"/>
                </div>
           
            </div>
                <div class="divLinks">
                    <a href="#">Administraci&oacute;n</a>
                    <a href="#">Ayuda</a>

                </div>           
       
           </footer>
        </form>
        <div id="fondo-negro">
           <div id="MsgBox">
		        <div id="MsgTit">
			        <input type="text" value="Atencion" readonly/>
			        <input type="button" value="X">
		        </div>
		        <div id="MsgContent1">
                    <img border="0"/>
			        <textarea onfocus="this.blur()" readonly></textarea>
		        </div>
			
		        <input type="button" id="Cancel" value="Cancelar" onclick="Cancel();" />
		        <input type="button" id="OK" value="OK" onclick="OK();"/>

        </div>
</body>
</html>