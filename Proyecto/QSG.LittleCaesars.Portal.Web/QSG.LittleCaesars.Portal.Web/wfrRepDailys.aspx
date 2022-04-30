<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="wfrRepDailys.aspx.cs" Inherits="QSG.LittleCaesars.Portal.Web.WebForm9" %>
<!DOCTYPE html  PUBLIC "-//W3C//DTD XHTML5//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
    <head id="head1" >
        <title id="tituloP"  ><%=NomP %></title>
        <link rel="Stylesheet" type="text/css" href="Styles/Style_caesars_pizza.css?n=1"/>
        <script type="text/javascript" src="Scripts/jquery-1.8.3.js"></script>
        <script type="text/javascript" src="Scripts/ScriptGeneral.js"></script>
        <script type="text/javascript" src="Scripts/CapZDays.js?n=1"></script>
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
                            <input type="button" title="Grabar" style="background-image:url('../../iconos/save_as-25.png');" class="btnReport" onclick="SaveIndormacion();"/>
                            <input type="button" title="Limpiar" style="background-image:url('../../iconos/eraser-25.png');" class="btnReport"  onClick="window.location.reload(); return false;"/>
                            <asp:Button id="Reporte" runat="server" style="background-image:url('../../iconos/details-25.png');"  CssClass="btnReport" title="Reporte" />
                        </div>
                         <div class="Content_rep_Days" >
                            <div class="content_filtros" style="max-width:92%;">
                                <label>

                                </label>
                            </div>

                                                          <div class="content_repT2" id="content_repT2" runat="server">
                                 <span class="CSSHeader">Daily Cash Summary</span> 
                                 <ul runat="server" id="container_history1" >
                                      <li>
                                        <ul>
                                            <li class="column_ID">No.</li>
                                            <li class="column_Edit"><img class='btnEdit' src='iconos/ic_edit1.png' title='Editar registro'/></li>
                                            <li class="column_Sucursal">Sucursal</li>
                                            <li class="column_FechaDaily" >Fecha Daily</li>
                                            <li class="column_NumZ">No.Z</li>
                                            <li class="column_TotalIng">Total Ingresos</li>

                                            <li class="column_ImpTcredito">T.Credito(Card)</li>
                                            <li class="column_OFPago">Otra Forma de Pago</li>

                                            <li class="column_TotalEfectivo">Total Efectivo Z</li>
                                            <li class="column_TotalEfectZ">Total Efectivo Daily </li>
                                            <li class="column_VariacionDayli">Variacion en Daily</li>

                                            <li  class="column_TipoCambio">Tipo de Cambio</li>
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
                                             <li class="column_Falt">Faltantes</li>

                                            
                                            <li class="column_ComentariosDayli"><img src="iconos/msgNew.png" /></li>

                                        </ul>
                                    </li> 
                                      


                                       <li class="Rowfooter">
                                        <ul>
                                            <li class="column_ID" style="color:white;"></li>
                                            <li class="column_Edit" style="background-color:white;"></li>

                                            <li class="column_Sucursal" style="font-weight:bold;">
                                               Total Ingreso del dia.
                                            </li>
                                            <li class="column_FechaDaily" ></li>
                                            <li class="column_NumZ"></li>
                                            <li class="column_TotalIng"><input  value="0"  readonly/></li>
                                           
                                             <li class="column_ImpTcredito"><input  value="0"  readonly/></li>
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
                                            <li class="column_Falt"></li>
                                            
                                            <li class="column_ComentariosDayli"></li>

                                        </ul>
                                    </li> 


                                  </ul>
                             </div>

                             </div>



                       </ContentTemplate>
                   </asp:UpdatePanel>

                   </div>

        </form>
</body>
    </html>
