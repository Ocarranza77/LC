<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="QSG.LittleCaesars.Portal.Web.Account.Login" %>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>
 <html >
<head id="Head1" runat="server">
    <meta name = "viewport" content = "initial-scale = 1, user-scalable = no">
    <meta name="viewport" content="width=device-width,initial-sacle=1.0"
    <title></title>
   <!-- <link rel="stylesheet" type="text/css" href="../Styles/Style_caesars_pizza.css" />-->
     <link rel="stylesheet" type="text/css" href="../Styles/Style.css?v=1" />
    <script type="text/javascript" src="../Portal_Js/jquery-1.8.3.js"></script>
    <script>
     
    </script>
    <SCRIPT type="text/javascript">
        window.history.forward();
        function noBack() { window.history.forward(); }
    </SCRIPT>
 </head>
<body  onload="noBack();" onpageshow="if (event.persisted) noBack();" onunload="">     
    
        <form id="forma" runat=server>
           
            <div id="FrmLogin" class="FrmLogin" >
                <h2 class="txt_title">Sistema de Administraci&oacute;n</h2>

                <div class="content_logo">
                    <div class="img_logo">
                        <img src="../Images/little-caesars-logo.jpg" />

                    </div>
                </div>
                <span class="QbicTittle" id="meseage"> <h3>Q-system ver 2.0.0 </h3> </span>
                <div class="content_login">
                        <label runat="server" id="Usuario">Usuario</label>
                        <input type="text" id="UserName" runat="server"  placeholder="ejemplo@little-caesars.com.mx" runat=server />
                        <label>Contrase&ntilde;a</label>
                        <input type="Password" id="Password" runat="server" placeholder="Contraseña" runat=server runat=server />
                         <span id="msgError" runat="server" ></span>
                    <asp:Button ID="LoginButton"  runat="server" CommandName="Login" Text="Iniciar" 
                        onclick="LoginButton_Click" />
                       
                </div>

               
                

            </div>

            </form>
 

</body>
</html>
