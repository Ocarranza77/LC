<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="RepTicketsFac.aspx.cs" Inherits="QSG.LittleCaesars.Portal.Web.WebForm5" %>
<!DOCTYPE html  PUBLIC "-//W3C//DTD XHTML5//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
    <head id="head1" >
        <title id="tituloP"  >REPORTE TICKETS Y FACTURAS</title>
        <link rel="Stylesheet" type="text/css" href="Styles/Style_caesars_pizza.css"/>
        <script type="text/javascript" src="Scripts/jquery-1.8.3.js"></script>
        <script type="text/javascript" src="Scripts/ScriptGeneral.js"></script>
        <script type="text/javascript" src="Scripts/CapTickets.js"></script>
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
                   <!-- <input type="button" title="Limpiar" style="background-image:url('../../iconos/eraser-25.png');" class="btnReport"  onClick="window.location.reload(); return false;"/>-->
                    <asp:Button ID="Button1"  title="Limpiar" style="background-image:url('../../iconos/eraser-25.png');" class="btnReport" runat="server" OnClick="Button1_Click"/>
                        
                    <asp:Button id="Reporte" runat="server" style="background-image:url('../../iconos/details-25.png');"  CssClass="btnReport" OnClick="GetReporte" title="Reporte" />
                </div>

                <div class="content_rep_tickets_fact">
                    <div class="content_filtros">
                        <label>
                            <span>Rango de fechas</span>
                            <span style="margin-left:100px;">
                                <asp:RadioButton ID="FechaTicket" GroupName="fechas" Font-Size="10pt" Height="10"   CssClass="RadioButton"  runat="server" Text="Fecha de Ticket o Recibo" OnCheckedChanged="FechaTicket_CheckedChanged" AutoPostBack="true"  />
                            </span>
                            <span >
                                <asp:RadioButton ID="FechaFact" GroupName="fechas" Font-Size="10pt"  Height="10"  runat="server" Text="Fecha de Facturacion" OnCheckedChanged="FechaFact_CheckedChanged" AutoPostBack="true" />
                            </span>
                            <span >
                                <asp:RadioButton ID="FechaCapturaTick" GroupName="fechas" Font-Size="10pt" Height="10"  runat="server" Text="Fecha de Captura de Ticket" OnCheckedChanged="FechaCapturaTick_CheckedChanged" AutoPostBack="true" />
                            </span>
                            <span style="float:right;">Usuario 
                                <select id="SelectUsers" runat="server">
                                    
                                </select>

                            </span>
                            <span style="float:right;">Sucursal
                               
                                <select id="SelectSucursales" runat="server">
                                 

                                </select>

                            </span>
                        </label>
                        <label>
                            <span style="width:20px;">De</span>
                            <asp:TextBox ID="txtfechaini" runat="server" Width="120px" Height="16px" CssClass="inputTXT" ReadOnly></asp:TextBox>
                            <asp:Button ID="btnFechaIni" runat="server"  OnClick="btnFechaIni_click" style="background-image:url('../../iconos/date-icon.png');background-repeat:no-repeat;border:none;cursor:pointer;float:left;margin-left:2px;" Width="25px" Height="24px"/>

                            
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
                            <span style="width:20px;">A</span>
                            <asp:TextBox ID="txtfechafin" runat="server" Width="120px" Height="16px" CssClass="inputTXT" ReadOnly></asp:TextBox>
                            <asp:Button ID="btnFechafin" runat="server"  OnClick="btnFechafin_click" style="background-image:url('../../iconos/date-icon.png');background-repeat:no-repeat;border:none;cursor:pointer;float:left;margin-left:2px;" Width="25px" Height="24px" />

            
                            <asp:Calendar ID="ClFechaFin" runat="server" style="position:relative;left:2px;margin-left:5px;background-color:transparent;" OnSelectionChanged="ClFechaFin_SelectionChanged" BorderWidth="0"  CellPadding="6" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="12pt" ForeColor="White" Height="180px" Width="200px" Visible="false">
                                 <DayHeaderStyle Font-Bold="True" Font-Size="10pt" CssClass="dayHead2" ForeColor="Black"/>
                                 <DayStyle CssClass="dayStyle" ForeColor="Black" />
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


                    </div>

                     <div class="content_repT" id="content_repT" runat="server">
                       <span>Tickets o Recibos</span><span style='float:right;margin-right:50px;'>Facturas</span>
                        <ul class="container_history" id="container_history" >
                             <li >
                               
                               <ul style="font-size:12pt;">
                                    <li class='column_ID'>No.</li>
                                    <li class='column_Sucursal'> Sucursal</li>
                                    <li class='column_FechaT'> Fecha Ticket</li>
                                    <li class='column_HoraT' >Hora Ticket</li>
                                    <li class='column_FolioT'>Folio Ticket</li>
                                    <li class='column_Caja'># Caja</li>
                                    <li class='column_Cajero'>Nombre de Cajero</li>
                                    <li class='column_Importe'>Importe</li>
                                    <li class='column_vacia'>#</li>
                                    <li class='column_STTF'>#</li>
                                    <li class='column_fechaFact'>Fecha Factura</li>
                                   <li class='column_folioFact'>Folio Factura</li>
                                   <li class='column_RFC1'>RFC</li>
                                   
                                  
                                </ul> 
                            </li>
                            <!--
                             <li class="row" >
                               <ul style="font-size:11pt;">
                                    <li class='column_ID'>1</li>
                                    <li class='column_Sucursal'>fundadores 0021</li>
                                    <li class='column_FechaT'> Fecha Ticket</li>
                                    <li class='column_HoraT' >Hora Ticket</li>
                                    <li class='column_FolioT'>Folio Ticket</li>
                                    <li class='column_Caja'># Caja</li>
                                    <li class='column_Cajero'>Nombre dedfgfdgfdgfdg Cajero </li>
                                    <li class='column_Importe'>Importe</li>
                                
                                    <li class='column_fechaFact'>Fecha Factura</li>
                                   <li class='column_folioFact'>Folio Factura</li>
                                   <li class='column_RFC1'>RFC</li>
                                   
                                  
                                </ul> 
                            </li>
                            -->
                        </ul>
                      


                    </div>

                 </div>




                     </ContentTemplate>
                 </asp:UpdatePanel>

                

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

             <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                     <ProgressTemplate>
                         
                        <div class="loader">
                            <!--
                                <div class="windows8">
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
                                </div>      -->


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




        </form>
        </body>
    </html>
