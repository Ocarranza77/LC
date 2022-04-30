<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="QSG.LittleCaesars.Facturacion.Web._Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

<!DOCTYPE html>
<html lang="en">
<head id="Head1" >
    <meta charset="utf-8" />
    
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
     <meta name = "viewport" content = "initial-scale = 1, user-scalable = no">
   
    <script type="text/javascript"  src="Scripts/jquery-1.8.3.js" ></script>

    <link rel="stylesheet" href="Styles/style.css?n=1" type="text/css" />
    <script type="text/javascript" src="Scripts/Facturacion.js?n=1" ></script>
</head> 
<body>
    <div class="header">
        <div class="header1">        
            <div class="headerDiv">
                <img src="Images/little.jpg" />
            </div>
            <span >Little Caesars</span>
      </div>
    </div>
    <did id="wraper" >
       
        <div id="main">

       <div class="content_title">
          
           <!--<div class="logo_emp"><img /></div> -->
       </div>
    <div class="content_master">
        <div>
         <div class="content_nav1">
            <div class="msg_first">
                 <h3>Estimado cliente</h3>
		        <h3 class="modH">Solo siga las sencillas instrucciones de llenado y en pocos minutos obtendra su factura electronica.</h3>
                 <img style="display:block; width:80%;height:80%;margin:0 auto;" src="Images/SLP1.jpg"/>
		        <h3>Gracias por su preferencia</h3>
            </div>
            <h2>Informacion</h2>
		    <ul class="lst_info">
			   

		    </ul>
		   
	    </div>

	    <div class="content_img_log">
		    <img class="log_img" src="Images/little-caesars-logo.jpg" />
            <canvas id="pizza" width="180" height="180"></canvas>
	    </div>

	    <div class="content_datos_cap_1" id="content_datos_cap_1">
		    <h2>Por favor ingrese :</h2>
		    <label >Folio del ticket<img src="Images/help-25.png" class="img_f"/></label>
		    <input type="text" placeholder="001101#0001"/>	
		    <label>Importe del ticket<img src="Images/help-25.png" class="img_Imp"/></label>
		    <input type="text"  placeholder="$ 0.00" onkeypress="return justNumbers(event);" />
		    <label>Sucursal</label>
            <input type="hidden" name="rfc_temp"/>
		    <select  placeholder="Salinas" id="select_sucursal" disabled></select>


		    <input name="content1" type="button" value="Continuar" />

            

		
	    </div>

          <div class="content_datos_cap_2" id="content_datos_cap_2">
		            <h2>Por favor ingrese los siguientes datos</h2>
		            <label>RFC</label>
		            <input type="text" placeholder="XAXX010101000" onkeypress="return pulsar(event,this)" />	
		            <!--
		            <label>Importe del ticket</label>
		            <input type="text" placeholder="$ 0.00" />-->
		            <input type="button" class="back" name="back2" value="<<    Regresar" />
		            <input type="button" class="next" name="next2" value="Continuar    >>"   />

	        </div>



            <div class="content_datos_cap_3" id="content_datos_cap_3">
		            <div class="content_dat_cap">
                        <label><input type="button" name="btnEditar" value="Editar Domicilio"/></label>
		                <label>Razon social <img src="Images/asterisk.png" /></label>
		                <input type="text" placeholder="Nombre personal o empresa" name="empresa" onkeypress="return pulsar(event,this)"/>	
		                <span style="float:left;width:100%;color:gray;">Direcci&oacute;n Fiscal</span>
		                <label>Calle</label>
		                <input type="text" placeholder="" name="calle" onkeypress="return pulsar(event,this)"/>	
                        <label>Numero</label>
                        <span>Ext.<input type="text" placeholder="" name="Next" onkeypress="return pulsar(event,this)" /></span>
                        <span>Int.<input type="text" placeholder="" name="Nint" onkeypress="return pulsar(event,this)"/></span>
                   	    
                        <label>Colonia</label>
		                <input type="text" placeholder="" name="colonia" onkeypress="return pulsar(event,this)"/>
		                <label>Delegacion</label>
		                <input type="text" placeholder="" name="delegacion" onkeypress="return pulsar(event,this)"/>	
		                <label>Ciudad</label>
		                <input type="text" placeholder="" name="ciudad" onkeypress="return pulsar(event,this)"/>
                        <label>Municipio</label>
		                <input type="text" placeholder="" name="municipio" onkeypress="return pulsar(event,this)"/>		
		                <label>Estado</label>
		                <input type="text" placeholder="" name="estado" onkeypress="return pulsar(event,this)"/>	
		                <label>CP</label>
		                <input type="text" placeholder="" name="codpostal" onkeypress="return pulsar(event,this)"/>	

		                <label>Contacto<img src="Images/asterisk.png" /></label>
		                <input type="text" placeholder="Persona que tramita factura" onkeypress="return pulsar(event,this)"/>	
		                <label style="width:90%;float:left;">Correo(s) electronico(s) para enviar factura<img src="Images/asterisk.png" /></label>
                        <img class="btnAdd" src="Images/ic_plus.png" style="float: left; width: 16px; height:16px;" />
		               <div><input type="text" placeholder="luis@hotmail.com" class="email_input"/></div> 	
		            </div>
		            <input type="button" class="back" name="back3" value="<<    Regresar" />
		            <input type="button" class="next" name="next3" value="Continuar    >>"  />

	        </div>
            
            <div class="content_datos_cap_4" id="content_datos_cap_4">
		
		        <h2>Aviso de privacidad</h2>
		        <div class="content_privacidad">
                 
                    <a href="Files/Política de Privacidad de la Aplicación Web.pdf" target="_blank" title="Aviso de Privacidad" >Click para ver nuestro aviso de privacidad</a>
                    <span><input type="checkbox" name="chkAviso"/> He leido el aviso de privacidad y otorgado mi consentimiento para que mis datos personales sean tratados conforme a lo señalado en el presente, Aviso de Privacidad.</span>
                   
		        </div>
		
		        <input type="button" class="back" name="back4" value="<<    Regresar"  />
		        <input type="button" class="next" name="next4" value="Continuar    >>"  />



		
	        </div>
            <div class="content_datos_cap_5" id="content_datos_cap_5">
		
	            <h2>Emision y Entrega</h2>
	            <div>
                    <h3>Estimado cliente:</h3>
                     <p>Su Factura electronica ha sido enviada al correo electronico que Ud. Ha solicitado.</p>
                     <p>Muchas gracias por su preferencia!</p>
	            </div>
		
	            <input type="button" class="content1" style="float:left;width:100%;" name="salir" value="Salir"  />
	           



		
            </div>


	    <div class="content_nav">
            <div class="msg_first">
                 <h3>Estimado cliente</h3>
		       <h3 class="modH">Solo siga las sencillas instrucciones de llenado y en pocos minutos obtendra su factura electronica.</h3>
                <img style="display:block; width:80%;height:80%;margin:0 auto;" src="Images/SLP1.jpg"/>
		        <h3>Gracias por su preferencia</h3>
            </div>
            <h2>Informacion</h2>
		    <ul class="lst_info">
			   

		    </ul>
		   
	    </div>

         <div class="content_img_log1">
		    <img class="log_img" src="Images/little-caesars-logo.jpg" />
            <canvas id="pizza1" width="180" height="180"></canvas>
	    </div>


        <div class="content_data">
           
        </div>
    </div>
   </div>

   

    </div>
        



 
 </did>
    <div id="footer">
        <div class="footer1">        
            <div class="footerDiv">
                <img src="Images/Logo_Qbic.jpg"/>
            </div>
            <!--<span >Qbic Solutions Group</span>-->
      </div>
    </div>

     <div class="viewer_pdf">
            <img title="Cerrar" />
            <div class="content_view">
               
            </div>
        </div>

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
                </div>
                -->
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

    <div class="content_msg">
        <div>
            <img />
            <div>
                <div class="title_msg">
                    <span></span>
                </div>
                <div class="msg">
                    <span>
                       
                    </span>
                </div>
                <div class="btn_msg">
                    
                </div>
           </div>
        </div>

    </div>
    <div class="content_ayuda">
     
    <img />
   
    </div>



   
</body>
</html>
</asp:Content>

