<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="CapTickets1.aspx.cs" Inherits="QSG.LittleCaesars.Portal.Web.WebForm4" %>
<!DOCTYPE html  PUBLIC "-//W3C//DTD XHTML5//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
    <head id="head1" >
        <title id="tituloP"  >CAPTURA DE TICKETS</title>
        <link rel="Stylesheet" type="text/css" href="Styles/Style_caesars_pizza.css"/>
        <script type="text/javascript" src="Scripts/jquery-1.8.3.js"></script>
        <script type="text/javascript" src="Scripts/ScriptGeneral.js"></script>
        <script type="text/javascript" src="Scripts/CapTickets.js"></script>



    </head>

    <body>
       
        <form id="Form1"  runat="server" >
             <div class="content_portal">
                     <header class="content_header">
                        <img src="Images/little.png" title="Home" id="home"/>
                        <input  id="ruta_app" style="color:white;font-size:10pt;border:none;background-color:transparent;margin:10px;width:250px;" runat=server value="Little Caesars" readonly/>
                        <ul>
                            <li><img src="iconos/administrator-25.png" /> <span id="NickName" runat="server"></span></li>
                            <li id="salir" >Salir</li>
                        </ul>
                    </header>
              
           
        <asp:scriptmanager  ID="ScriptManager1" runat="server">


        </asp:scriptmanager>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                 



                   <div class="content_buttons">

                       <asp:Button id="Button2" runat="server" style="background-image:url('../../iconos/save_as-25.png');"  CssClass="btnReport" OnClick="SaveTickets" title="Guardar" />
                       <asp:Button id="Button1" runat="server" style="background-image:url('../../iconos/eraser-25.png');"  CssClass="btnReport"   title="Limpiar" />
                        <asp:Button id="Reporte" runat="server" style="background-image:url('../../iconos/details-25.png');"  CssClass="btnReport" OnClick="GetTickets" title="Reporte" />
                      
                       <!-- <ul>
                            <li> 
                              
                            </li>
                            <li><asp:Button runat="server" Text="Grabar"  CssClass="btnSave" />

                            </li>
                        </ul>
                           -->
                    </div>


                <div class="content_rep_tickets">

                    <div class="content_filtros">
                         <label>
                     <span>Fecha Ticket (Venta)</span>
                     <asp:Button ID="BtnFechaCap" runat="server" OnClick="btnFechaCaP_click" style="float:left;margin-left:5px; width:155px;height:20px; cursor:pointer;padding:0px;background-color:transparent;border:1px solid orange;color:black;text-align:center;" />
                     <asp:Calendar ID="ClFechaCap" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" OnSelectionChanged="ClFechaCap_SelectionChanged" style="position:relative;left:2px;margin-left:5px;" Visible="false" Width="200px">
                         <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                         <DayStyle BorderStyle="Solid" BorderWidth="1px" />
                         <NextPrevStyle VerticalAlign="Bottom" />
                         <OtherMonthDayStyle ForeColor="#808080" />
                         <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                         <SelectorStyle BackColor="#CCCCCC" />
                         <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                         <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                         <WeekendDayStyle BackColor="#FFFFCC" />
                     </asp:Calendar>
                     <!--<input id="fechaCapt" Class="fechaCapt" runat="server" />
                       <!-- <asp:Calendar runat="server" id="fechaC"></asp:Calendar> -->
                     </label>





                        <label><span>Usuario</span> <input id="usuario" Class="usuario" runat="server" disabled /></label>
                        
                        <asp:Button ID="Button3" runat="server" style="background-image:url('../../iconos/ic_plus.png');width:16px;height:16px;" onclick="addTicket"/>


                        <img id="img_add_row" src="iconos/ic_plus.png" onclick="addRow(event);" />

                    </div>
                        
          <div class="content_repT" id="content_repT" runat="server">
                      <ul runat="server" id="container_history" >
                           <li id="Li1" runat="server">
                              
                               <ul id="Ul1" runat="server">
                                   <li id="Li2" runat="server"  class='row_ID'></li>
                                   <li id="Li3" runat="server"  class='column_ID'></li>
                                   <li id="Li4" runat="server"   class='column_Edit'><img class='btnEdit' src='iconos/ic_edit1.png' title='Editar registro'/></li>
                                    <li id="Li5" runat="server"     class='column_Del'><img class='btnDel' src='iconos/ic_delete.png' title='Eliminar registro'/></li>
                                    <li id="Li6" runat="server"  class='column_Sucursal'> Sucursal</li>
                                    <li id="Li7" runat="server" class='column_FechaT'>Fecha Ticket</li>
                                    <li id="Li8" runat="server"   class='column_HoraT' >Hora Ticket</li>
                                    <li id="Li9" runat="server" class='column_FolioT'>Folio Ticket</li>
                                    <li id="Li10" runat="server"   class='column_Caja'># Caja</li>
                                    <li id="Li11" runat="server"  class='column_Cajero'>Nombre de Cajero</li>
                                    <li id="Li12" runat="server"    class='column_Importe'>Importe</li>
                                    <li id="Li13" runat="server"  class='column_UserCap'>Usuario Capturo</li>
                                   <li id="Li14" runat="server"    class='column_stt'></li>		    
                                </ul> 
                               </li>
                      </ul>
                        <label runat="server" id="mostTest"></label>
                        </div>


                </div>
                  </ContentTemplate>

        </asp:UpdatePanel>

        <asp:UpdateProgress ID="UpdateProgress1" runat="server"  AssociatedUpdatePanelID="UpdatePanel1" >
            <ProgressTemplate>

              
                 <div class="loader">
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
                  
        <!--<ul >
            <li><a href="#" style="color:white;">Administrador</a> </li>
                <li><a href="#" style="color:white;">Ayuda</a> </li>
        </ul>-->
</footer>
            <!--
           
            -->
        
        </form>
    </body>    
</html>
