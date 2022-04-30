<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="FacturacionGlobal.aspx.cs" Inherits="QSG.LittleCaesars.Portal.Web.WebForm8" %>
<!DOCTYPE html  PUBLIC "-//W3C//DTD XHTML5//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
    <head id="head1" >
        <title id="tituloP"  ><%=NomP %></title>
        <link rel="Stylesheet" type="text/css" href="Styles/Style_caesars_pizza.css?n=1"/>
        <script type="text/javascript" src="Scripts/jquery-1.8.3.js"></script>
        <script type="text/javascript" src="Scripts/ScriptGeneral.js"></script>
        <script type="text/javascript" src="Scripts/FacturaGeneral.js?n=1"></script>
    </head>

    <body>
       
        <form id="Form1"  runat="server" >
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
             <div class="content_portal">
                 <header class="content_header">
                        <img src="Images/little.png" title="Home" id="home"/>
                        <input  id="ruta_app" style="color:white;font-size:10pt;border:none;background-color:transparent;margin:10px;width:400px" runat=server value="Little Caesars" readonly/>
                        <ul>
                            <li><img src="iconos/<%=Session["Puesto"] %>.png" /> <span id="NickName" runat="server"></span></li>
                            <li id="salir" >Salir</li>
                        </ul>
                    </header>

                 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                     <ContentTemplate>
                         <div class="content_buttons">
                            <input type="button" title="Grabar" style="background-image:url('../../iconos/save_as-25.png');" class="btnReport"/>
                           <!-- <input type="button" title="Limpiar" style="background-image:url('../../iconos/eraser-25.png');" class="btnReport"  onClick="window.location.reload(); return false;"/>-->
                             <asp:Button ID="Button1" runat="server" title="Limpiar" style="background-image:url('../../iconos/eraser-25.png');" class="btnReport" OnClick="Button1_Click1" />
                            <asp:Button id="Reporte" runat="server" style="background-image:url('../../iconos/details-25.png');"  CssClass="btnReport" title="Reporte" OnClick="Reporte_Click" />
                        </div>
                         <div class="Content_rep_FactPG" >
                            <div class="content_filtros">
                                <label  >
                                  
                                    <span style="width:83px;">Usuario</span>
                                    <select id="SelectUsers" runat="server" disabled></select>

                                </label>

                                <label >
                                    <span >Fecha de consumo</span>
                                    <asp:TextBox ID="txtFConsumo" runat="server" CssClass="inputTXT" Width="100px" Height="18px" ReadOnly></asp:TextBox>
                                    <asp:Button ID="btnFConsumo" runat="server"  Width="25px" Height="24px" OnClick="btnFConsumo_Click" CssClass="btnCalendar" />

                                     <asp:Calendar ID="ClFechaCon" runat="server" style="position:relative;left:2px;margin-left:5px;background-color:transparent;" OnSelectionChanged="ClFechaCon_SelectionChanged" BorderWidth="0"  CellPadding="6" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="12pt" ForeColor="White" Height="180px" Width="200px" Visible="false" Font-Underline="false">
                                        <DayHeaderStyle Font-Bold="True" Font-Size="10pt" CssClass="dayHead2" ForeColor="Black"/>
                                        <DayStyle CssClass="dayStyle" ForeColor="Black" Font-Underline="false"/>
                                        <NextPrevStyle VerticalAlign="Middle" Font-Size="10pt" Font-Bold="True" />
                                        <OtherMonthDayStyle ForeColor="#808080" />
                                        <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                        <SelectorStyle BackColor="#CCCCCC" />
                                        <TitleStyle HorizontalAlign="Center" VerticalAlign="Middle"  Font-Bold="True" CssClass="DayHeadStyle"  />
                                        <TodayDayStyle ForeColor="Black" CssClass="toDayHeadStyle" />
                                        <WeekendDayStyle BackColor="#FFFFCC" CssClass="weekend"/>
                                    </asp:Calendar>

                                </label>
                                <label>

                                    <span>Fecha de Captura&nbsp;&nbsp;</span>
                                    <asp:TextBox ID="txtfechacap" runat="server" CssClass="inputTXT" Width="100px" Height="18px" ReadOnly></asp:TextBox>
                                    <asp:Button ID="btnfechacap" runat="server"  Width="25px" Height="24px" OnClick="btnfechacap_Click" CssClass="btnCalendar" />

                                    <asp:Calendar ID="ClfechaCap" runat="server" style="position:relative;left:2px;margin-left:5px;background-color:transparent;" OnSelectionChanged="ClFechaCap_SelectionChanged" BorderWidth="0"  CellPadding="6" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="12pt" ForeColor="White" Height="180px" Width="200px" Visible="false" Font-Underline="false">
                                        <DayHeaderStyle Font-Bold="True" Font-Size="10pt" CssClass="dayHead2" ForeColor="Black"/>
                                        <DayStyle CssClass="dayStyle" ForeColor="Black" Font-Underline="false"/>
                                        <NextPrevStyle VerticalAlign="Middle" Font-Size="10pt" Font-Bold="True" />
                                        <OtherMonthDayStyle ForeColor="#808080" />
                                        <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                        <SelectorStyle BackColor="#CCCCCC" />
                                        <TitleStyle HorizontalAlign="Center" VerticalAlign="Middle"  Font-Bold="True" CssClass="DayHeadStyle"  />
                                        <TodayDayStyle ForeColor="Black" CssClass="toDayHeadStyle" />
                                        <WeekendDayStyle BackColor="#FFFFCC" CssClass="weekend"/>
                                    </asp:Calendar>

                                </label>
                                <label>

                                    <span>Fecha de Factura&nbsp;&nbsp;</span>
                                    <select id="SelectFechaFactura" runat="server" Width="100px" Height="18px" style="padding:2px" class="inputTXT"></select>

                                </label>
                            </div>


                              <div class="content_repT" id="content_repT" runat="server" >
                                  
                                   <ul class="container_history" id="container_history" >
                                    <li>
                                        <ul>
                                          
                                            <li class="column_Sucursal"> Sucursal</li>
                                            <li class="column_IngDia" >Ingreso del dia (fecha consumo)</li>
                                            <li class="column_FactCli" >Facturado Clientes</li>
                                            <li class="column_Xfact" >Por Facturar en Cierre PG</li>
                                            <li class="column_Facturar" ><img /></li>
                                            <li class="column_Facturado" >Facturado en cierre PG</li>
                                            <li class="column_Folios" >Folios Facturados PG</li>
                                            <li class="column_FactCancel">Facturas Canceladas
                                                <ul>
                                                    <li class="column_FactCancelCli" >de clientes</li>
                                                    <li class="column_FactCancelPG" >de PG</li>
                                                </ul>
                                                
                                            </li>
                                            

                                           
                                        </ul>
                                    </li> 
                                       <!--
                                        <li class="row">
                                            <ul>
                                          
                                                <li class="column_Sucursal"> Sucursal</li>
                                                <li class="column_IngDia" >9</li>
                                                <li class="column_FactCli" >9</li>
                                                <li class="column_Xfact" ></li>
                                                <li class="column_Facturar" ><img src="iconos/uncheck_icon2.png" onclick='Facturar(event);' /></li>
                                                <li class="column_Facturado" >9</li>
                                                <li class="column_Folios" ><a href="#">cf123,</a><a href="#">cf123</a></li>
                                            
                                                <li class="column_FactCancelCli" >=2</li>
                                                <li class="column_FactCancelPG" >2</li>
                                              
                                            

                                           
                                            </ul>
                                    </li> 
                                         <li class="row">
                                            <ul>
                                          
                                                <li class="column_Sucursal"> Sucursal</li>
                                                <li class="column_IngDia" >9</li>
                                                <li class="column_FactCli" >9</li>
                                                <li class="column_Xfact" ></li>
                                                <li class="column_Facturar" ><img src="iconos/uncheck_icon2.png" onclick='Facturar(event);' /></li>
                                                <li class="column_Facturado" >9</li>
                                                <li class="column_Folios" ><a href="#">cf123</a><a href="#">cf123</a></li>
                                            
                                                <li class="column_FactCancelCli" >=2</li>
                                                <li class="column_FactCancelPG" >2</li>
                                              
                                            

                                           
                                            </ul>
                                    </li> 
                                       -->
                                      
                                       <li class="Rowfooter">
                                        <ul >
                                           <li class="column_Sucursal" style='font-weight:bold;'> Totales >></li>
                                                <li class="column_IngDia" ><input readonly/></li>
                                                <li class="column_FactCli" ><input readonly/></li>
                                                <li class="column_Xfact" ><input readonly/></li>
                                                <li class="column_Facturar" ></li>
                                                <li class="column_Facturado" ><input readonly/></li>
                                                <li class="column_Folios" ></li>
                                            
                                                <li class="column_FactCancelCli" ></li>
                                                <li class="column_FactCancelPG" ></li>
                                        </ul>
                                    </li> 
                                      
                                       
                                     
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
            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
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


             <div class="loader1">
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

        </form>
        </body>
    </html>