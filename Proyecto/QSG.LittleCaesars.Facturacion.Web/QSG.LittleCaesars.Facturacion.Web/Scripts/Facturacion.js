$(function () {
	// body...

	$('.content_datos_cap_1').show();
	$('#pizza').hide();
	$('#pizza1').hide();
	$('.lst_info').hide();
	$('.content_title').html('<h2 >Bienvenido a su servicio de facturaci&oacute;n electr&oacute;nica en l&iacute;nea </h2>');
	
	$('.content_datos_cap_1 > label').children('img.img_f').hover(function  (event) {
		// body...
		//alert($(event.target).position().top );
		
			$('.content_ayuda').children('img').attr('src','Images/folio.png');
		
		
		if($(window).width()<480){

			$('.content_ayuda').css({top:(parseInt($(event.target).position().top)-$('.content_ayuda').height()/2),left:(parseInt($(event.target).position().left)-$('.content_ayuda').width())});
		}else{
			$('.content_ayuda').css({top:(parseInt($(event.target).position().top)-$('.content_ayuda').height()/2),left:(parseInt($(event.target).position().left)+20)});
	
		}

	
		$('.content_ayuda').toggle("low");
	});

	$('.content_datos_cap_1 > label').children('img.img_Imp').hover(function  (event) {
		// body...
		//alert($(event.target).position().top );
		
			$('.content_ayuda').children('img').attr('src','Images/importe.png');
		
		
		if($(window).width()<480){

			$('.content_ayuda').css({top:(parseInt($(event.target).position().top)-$('.content_ayuda').height()/2),left:(parseInt($(event.target).position().left)-$('.content_ayuda').width())});
		}else{
			$('.content_ayuda').css({top:(parseInt($(event.target).position().top)-$('.content_ayuda').height()/2),left:(parseInt($(event.target).position().left)+20)});
	
		}

	
		$('.content_ayuda').toggle("low");
	});

$('input[name="btnEditar"]').click(function function_name (argument) {
	// body...
		$('.content_datos_cap_3 > div').children('input[name=empresa]').removeAttr('disabled');
		$('.content_datos_cap_3 > div').children('input[name=calle]').removeAttr('disabled');
		$('.content_datos_cap_3 > div > span').children('input[name=Next]').removeAttr('disabled');
		$('.content_datos_cap_3 > div > span').children('input[name=Nint]').removeAttr('disabled');
		$('.content_datos_cap_3 > div').children('input[name=colonia]').removeAttr('disabled');
		$('.content_datos_cap_3 > div').children('input[name=delegacion]').removeAttr('disabled');
		$('.content_datos_cap_3 > div').children('input[name=ciudad]').removeAttr('disabled');
		$('.content_datos_cap_3 > div').children('input[name=municipio]').removeAttr('disabled');
		$('.content_datos_cap_3 > div').children('input[name=estado]').removeAttr('disabled');
		$('.content_datos_cap_3 > div').children('input[name=codpostal]').removeAttr('disabled');

});
$('.content_nav > h2').hide();

	$('input[name="content1"]').click(function () {
		// body...
		var folio=$('.content_datos_cap_1').children('input[type=text]').first().val().split('#');
		if( folio[0].length ==6 && folio[1].length > 0 && $('.content_datos_cap_1').children('input[type=text]').last().val() > 0){		
		var RFC=$('.content_datos_cap_1').children('input[name=rfc_temp]').val();
		

		$('.content_nav > h2').show();	
		$('.msg_first').hide();
		$('.log_img').hide();
		$('#pizza').show();
		$('#pizza1').show();
		$('.content_datos_cap_1').hide();

		$('.lst_info').append('<li class="l1" style="display:none;"><input type="text" name="rfc2_temp"/></li>');
		$('.lst_info').append('<li class="l1"><input type="hidden" value="'+$('.content_datos_cap_1').children('input')[0].value+'" />Folio: '+$('.content_datos_cap_1').children('input')[0].value +'</li>');
		$('.lst_info').append('<li class="l1"><input type="hidden" value="'+$('#select_sucursal').val() +'" />Sucursal: '+$('#select_sucursal option:selected').text()  +'</li>');
		$('.lst_info').append(' <li class="l1"><input type="hidden" value="'+$('.content_datos_cap_1').children('input')[1].value+'" />Importe: '+$('.content_datos_cap_1').children('input')[1].value+'</li>');
		$('.content_title').html('<h2 >Servicio de facturaci&oacute;n electr&oacute;nica en l&iacute;nea </h2>');
		
		if(RFC!=""){

			GetCliente(RFC.trim());

			$('.content_nav > h2').show();
			$('.content_datos_cap_3').toggle();
			$('.content_datos_cap_2').hide();
			dibujarCanvas(135);
		
			//$('.lst_info > li').find('input[name=rfc2_temp]').val(RFC);
			$('.lst_info').append('<li class="l2"><input type="hidden" value="'+RFC+'" />RFC:'+RFC +'</li>');
			/*
			$('.content_datos_cap_3 > div').children('input[name=empresa]').attr('disabled','disabled');
			$('.content_datos_cap_3 > div').children('input[name=calle]').attr('disabled','disabled');
			$('.content_datos_cap_3 > div > span').children('input[name=Next]').attr('disabled','disabled');
			$('.content_datos_cap_3 > div > span').children('input[name=Nint]').attr('disabled','disabled');
			$('.content_datos_cap_3 > div').children('input[name=delegacion]').attr('disabled','disabled');
			$('.content_datos_cap_3 > div').children('input[name=ciudad]').attr('disabled','disabled');
			$('.content_datos_cap_3 > div').children('input[name=municipio]').attr('disabled','disabled');
			$('.content_datos_cap_3 > div').children('input[name=estado]').attr('disabled','disabled');
			$('.content_datos_cap_3 > div').children('input[name=codpostal]').attr('disabled','disabled');
			*/

		}else{

			
			$('.content_datos_cap_2').toggle();
			dibujarCanvas(45);
			
		
		

		}
		$('.lst_info').show();

		
		
	
		
	}else{
		alert("Por favor ingrese los datos requeridos.");
	}

	});

	$('input[name="back2"]').click(function () {
		$('.content_datos_cap_1').children('input[type=text]').last().focus();
		$('.lst_info li').remove('.l2');
		$('.lst_info li').remove('.l1');
		$('.content_datos_cap_1').toggle();
		$('.content_datos_cap_2').hide();
		$('.msg_first').show();
		$('.content_nav > h2').hide();
		$('.log_img').show();
		$('#pizza').hide();
		$('#pizza1').hide();
		//dibujarCanvas(45);
	
		
	});
	$('input[name="next2"]').click(function () {
		if($('.content_datos_cap_2').children('input[type=text]').val()!="" && $('.content_datos_cap_2').children('input[type=text]').val()!=null ){
			
			if(ValRFC($('.content_datos_cap_2').children('input[type=text]').val().trim())){
			
				GetCliente($('.content_datos_cap_2').children('input[type=text]').val().trim());

			$('.content_nav > h2').show();
			$('.content_datos_cap_3').toggle();
			$('.content_datos_cap_2').hide();
			dibujarCanvas(135);
			
			$('.lst_info').append('<li class="l2"><input type="hidden" value="'+$('.content_datos_cap_2').children('input')[0].value+'" />RFC: '+$('.content_datos_cap_2').children('input')[0].value +'</li>');

			}else{
				alert("RFC Incorrecto,"+"\n"+"Por favor verifique su RFC.");
				$(this).focus();
			}


		}else{
			alert("Por favor ingrese RFC");
			$(this).focus();
		}
	});

	$('input[name="back3"]').click(function () {

		if($('.content_datos_cap_1').children('input[name=rfc_temp]').val()!="" ){
			$('.content_datos_cap_1').children('input[type=text]').last().focus();
			$('.content_datos_cap_1').toggle();
			$('.lst_info li').remove('.l1');
		}else{
			$('.content_datos_cap_2').toggle();
			dibujarCanvas(45);

			$('.lst_info li').remove('.l2');
		
		$('.lst_info li').remove('.l3');
		}
		
		//$('.content_datos_cap_2').toggle();
		$('.content_datos_cap_3').hide();
		


		
	});
	$('input[name="next3"]').click(function () {
		var correos="";
		var bolEmail=true;
		$('.content_datos_cap_3 > div > div > input').each(function  (index) {
			
			if($(this).val()!=""){
				correos+=$(this).val();
				if(index < ($('.content_datos_cap_3 > div > div > input').length-1) ){correos+=";";}

			}
			//validarEmail($(this).val());
		});

		if($('.content_datos_cap_3 > div').children('input')[0].value!="" && $('.content_datos_cap_3 > div').children('input')[8].value!="" && bolEmail==true ){
		
			$('.content_datos_cap_4').toggle();
			$('.content_datos_cap_3').hide();
			dibujarCanvas(225);
			$('.lst_info').append('<li class="l3"><input type="hidden" value="'+$('.content_datos_cap_3 > div').children('input')[0].value+'" />Razon Social: '+$('.content_datos_cap_3 > div').children('input')[0].value +'</li>');
			$('.lst_info').append('<li class="l3"><input type="hidden" value="'+$('.content_datos_cap_3 > div').children('input')[1].value+'" />Calle: '+$('.content_datos_cap_3 > div').children('input')[1].value+'</li>');
			$('.lst_info').append('<li class="l3"><input type="hidden" value="'+$('.content_datos_cap_3 > div > span').children('input')[1].value+'" /> Num  Ext: '+$('.content_datos_cap_3 > div > span').children('input')[0].value+'  Int: '+$('.content_datos_cap_3 > div > span').children('input')[1].value+'</li>');
			$('.lst_info').append('<li class="l3"><input type="hidden" value="'+$('.content_datos_cap_3 > div > span').children('input')[0].value+'" /></li>');
			$('.lst_info').append('<li class="l3"><input type="hidden" value="'+$('.content_datos_cap_3 > div').children('input')[2].value+'" />Colonia: '+$('.content_datos_cap_3 > div').children('input')[2].value+'</li>');
			$('.lst_info').append('<li class="l3"><input type="hidden" value="'+$('.content_datos_cap_3 > div').children('input')[3].value+'" />Delegacion: '+$('.content_datos_cap_3 > div').children('input')[3].value+'</li>');
			$('.lst_info').append('<li class="l3"><input type="hidden" value="'+$('.content_datos_cap_3 > div').children('input')[4].value+'" />Ciudad: '+$('.content_datos_cap_3 > div').children('input')[4].value+'</li>');
			$('.lst_info').append('<li class="l3"><input type="hidden" value="'+$('.content_datos_cap_3 > div').children('input')[5].value+'" />Municipio: '+$('.content_datos_cap_3 > div').children('input')[5].value+'</li>');
			$('.lst_info').append('<li class="l3"><input type="hidden" value="'+$('.content_datos_cap_3 > div').children('input')[6].value+'" />Estado: '+$('.content_datos_cap_3 > div').children('input')[6].value+'</li>');
			$('.lst_info').append('<li class="l3"><input type="hidden" value="'+$('.content_datos_cap_3 > div').children('input')[7].value+'" />CP: '+$('.content_datos_cap_3 > div').children('input')[7].value+'</li>');
			$('.lst_info').append('<li class="l3"><input type="hidden" value="'+$('.content_datos_cap_3 > div').children('input')[8].value+'" />Contacto: '+$('.content_datos_cap_3 > div').children('input')[8].value+'</li>');
			/*
			$('.content_datos_cap_3 > div > div > input').each(function  (index) {
				// body...
				if($(this).val()!=""){
					correos+=$(this).val();
					if(index<$(this).length){correos+=";";}
				}
				
			});
			*/
			$('.lst_info').append('<li class="l3"><input type="hidden" value="'+correos+'" />Correo (s):  '+correos+'</li>');
		}else{
			alert("Para poder continuar es necesario ingresar los datos minimos marcados con (*)");
		}
		
		
	});

	$('input[name="back4"]').click(function () {
		$('.lst_info li').remove('.l3');
		$('.content_datos_cap_3').toggle();
		$('.content_datos_cap_4').hide();
		dibujarCanvas(135);
		
	});

	$('input[name="next4"]').click(function () {
			var text=" <p>Por favor verifique que la cuenta de correo en la que desea recibir su factura sea correcta.En caso contrario modifique antes de aceptar.</p>";
                 text+="<p>Por favor verifique que los datos de alta sean correctos.</p>";
                 text+="<p>Esta seguro que los datos capturados son correctos? </p>";

            var botones="<input type='button' id='aceptar' value='Aceptar' onclick='click_msg(this.id);'/>";
                 botones+="<input type='button' id='modificar'  value='Modificar' onclick='click_msg(this.id);'/>";


			$('.content_msg > div > div .title_msg > span').text('INFORMACION');
			$('.content_msg > div > div .msg > span').html(text);
			$('.content_msg > div > div .btn_msg').html(botones);


			$('.content_msg').toggle();
		
	});

	$('.content_msg > div > img').click(function () {
		// body...

		$('.content_msg').toggle();
	});
	$('input[name="salir"]').click(function () {
		location.reload();
	});

	





	$('.content_datos_cap_4 > input[name=next4]').css({'background-color':'lightblue'});

	$('.content_datos_cap_4 > div > span').children('input[name=chkAviso]').attr('checked',false);
	$('.content_datos_cap_4 > input[name=next4]').attr('disabled','disabled');
	$('.content_datos_cap_4 > div > span').children('input[name=chkAviso]').click(function () {
		// body...
		if($(this).is(':checked')){
			$('.content_datos_cap_4 > input[name=next4]').css({'background-color':'#0080FF','border':'1px solid #7ac9b7'});
			$('.content_datos_cap_4 > input[name=next4]').removeAttr('disabled');
		}else{
			$('.content_datos_cap_4 > input[name=next4]').attr('checked',false);
			$('.content_datos_cap_4 > input[name=next4]').css({'background-color':'lightblue'});
			$('.content_datos_cap_4 > input[name=next4]').attr('disabled','disabled');
		}
		
	});

	$('input.email_input').focusout(function  (event) {

		// body...
		/*if(!validarEmail($(this).val())){
			alert("correo incorrecto");
		}*/

	});
	$('.btnAdd').click(function () {
		// body...
		if($('.content_dat_cap > div input').length <3){
			//alert(validarEmail($('.content_dat_cap > div input').val()));
			if($('.content_dat_cap > div input').val()!="" && validarEmail($('.content_dat_cap > div input').val())){
				$('.content_dat_cap > div').append('<input type="text" placeholder="luis@hotmail.com" class="email_input"/>');
			}
			
		}
	});

	$('.content_privacidad a').click(function () {
		// body...
	//	$(this).attr('href','Files/Proyecto_Facturacion_Electronica_a_Clientes.pdf');
		//$('.viewer_pdf > div').append('<object type="application/pdf"  data="Files/Proyecto_Facturacion_Electronica_a_Clientes.pdf"></object>');
		//$('.viewer_pdf').toggle();
	});
	$('.viewer_pdf img').click(function () {
		// body...
		//$('.viewer_pdf').toggle();
		
	});

	$('.content_dat_cap input').keyup(function  (e) {
		// body...
		if(e.keyCode==13){
			e.preventDefault();

		var inputs=$(this).closest('.content_dat_cap').find('input');
		inputs.eq(inputs.index(this)+1).focus();
		}
		
	});

/*
	$('.content_dat_cap').children('input[type=text]').eq(0).keyup(function  (e) {
		// body...
		e.preventDefault();
		$('.content_dat_cap').children('input[type=text]').eq(1).focus();

	});
	$('.content_dat_cap').children('input[type=text]').eq(1).keyup(function  (e) {
		// body...
		e.preventDefault();
		$('.content_dat_cap').children('input[type=text]').eq(2).focus();
	});
	$('.content_dat_cap').children('input[type=text]').eq(2).keyup(function  (e) {
		// body...
		e.preventDefault();
		$('.content_dat_cap').children('input[type=text]').eq(3).focus();
	});

	$('.content_dat_cap').children('input[type=text]').eq(4).keyup(function  (e) {
		// body...
		e.preventDefault();
		$('.content_dat_cap').children('input[type=text]').eq(5)
	});
*/

	$('.content_datos_cap_1').children('input[type=text]').first().keyup(function  (e) {
		// body...
		e.preventDefault();

		if($(this).val()!=""){
			if(e.keyCode==13){
				$('.img_f').css({'background-color':'#FE9A2E'});
			}
			
		}

		if($(this).val().indexOf('#')<0 || $(this).val()=="" ){
			$(this).focus();
			
		}else{
			var folio=$(this).val().split('#');
			if(e.keyCode ==13){
				e.preventDefault();
				if( folio[0].length ==6 && folio[1].length > 0){
					$('.content_datos_cap_1').children('input[type=text]').last().focus();
					$('.img_f').css({'background-color':'transparent'});
				}else{
					alert("Por favor verifique el folio del ticket en el icono de ayuda ");
					$(this).focus();
					$('.img_f').css({'background-color':'#FE9A2E'});
				}
				
			}
		}
			
			

			

	});
	$('.content_datos_cap_1').children('input[type=text]').first().focusout(function  (e) {
		e.preventDefault();
		if($(this).val().indexOf('#')<0 || $(this).val()=="" ){
			//$(this).focus();
		}else{
			var folio=$(this).val().split('#');
			if(e.keyCode ==13){
				e.preventDefault();
				if( folio[0].length ==6 && folio[1].length > 0){
					$('.content_datos_cap_1').children('input[type=text]').last().focus();
				}else{
					alert("Por favor verifique el folio del ticket en el icono de ayuda");
					$(this).focus();
				}
				
			}
		}
			

	});

	$('.content_datos_cap_1').children('input[type=text]').last().change(function  (e) {
		// body...
		if($(this).val()!=""){
			if(parseFloat($(this).val())>0){
				$('.content_datos_cap_1 > input[name=content1]').css({'background-color':'#0080FF','border':'1px solid #7ac9b7'});
				$('.content_datos_cap_1 > input[name=content1]').removeAttr('disabled');
			}
			
		}else{
			$('.content_datos_cap_1 > input[name=content1]').css({'background-color':'lightblue','border':'1px solid #7ac9b7'});
			$('.content_datos_cap_1 > input[name=content1]').attr('disabled',true);
		}


	});


	$('.content_datos_cap_1').children('input[type=text]').last().keyup(function  (e) {
		// body...
		e.preventDefault();
		
		if($(this).val()==""){
			$(this).focus();
		}else{
			
			/*var folio=$('.content_datos_cap_1').children('input[type=text]').first().val().split('#');*/
			if(e.keyCode==13){
			e.preventDefault();
			/*
				if( folio[0].length ==6 && folio[1].length > 0 && $(this).val()>0){
					GetTicket($('.content_datos_cap_1').children('input[type=text]').first().val(),$(this).val());
					//$("#select_sucursal option[value='"+parseInt(folio[0].substring(0,3))+"']").attr('selected',true);
				

				}else{
					alert("Por favor verifique sus datos, consulte en el icono de ayuda");
				}*/
				$('.content_datos_cap_1').children('input[type=text]').first().focus();
			}
			
		}
		
			
			

	
	});

	$('.content_datos_cap_1').children('input[type=text]').last().focusout(function  (e) {
		e.preventDefault();
		if($(this).val()!=""){

			if(parseFloat($(this).val())>0){
				$(this).val(parseFloat($(this).val()).toFixed(2));
				if($('.content_datos_cap_1').children('input[type=text]').first().val()!="" && $('.content_datos_cap_1').children('input[type=text]').first().val().indexOf('#')>-1){

					var folio=$('.content_datos_cap_1').children('input[type=text]').first().val().split('#');
					if( folio[0].length ==6 && folio[1].length > 0 && $(this).val()>0){
						GetTicket($('.content_datos_cap_1').children('input[type=text]').first().val(),$(this).val());
						//$("#select_sucursal option[value='"+parseInt(folio[0].substring(0,3))+"']").attr('selected',true);
					

					}else{
						alert("Por favor verifique sus datos, consulte en el icono de ayuda");
					}
				}
			}
		}else{
			
			
		}
		
	});

	GetSucursales();
	$('.content_datos_cap_1 > input[name=content1]').css({'background-color':'lightblue','border':'1px solid #7ac9b7'});
	$('.content_datos_cap_1 > input[name=content1]').attr('disabled',true);

});

function dibujarCanvas(valor){
        var canvas = document.getElementById('pizza');
        var contexto = canvas.getContext('2d');
          contexto.fillStyle="white";
          //ctx.fillRect(0,0,300,150);
		  contexto.clearRect(0,0,canvas.width,canvas.height);

           contexto.beginPath();
          // contexto.fillStyle="orange";
          contexto.lineWidth=5;
           contexto.strokeStyle="#F16527";
           contexto.arc(90,90,85,0,Math.PI*2,false);
          // contexto.fill();
           contexto.stroke();
           contexto.closePath();


           for(var i=0;i<=79;i++ ){
				contexto.beginPath();
				contexto.lineWidth=5;
				contexto.strokeStyle="#F16527";
				contexto.arc(90,90,i,(-Math.PI/180)*45,(Math.PI/180)*valor,false);
				contexto.stroke();
				contexto.closePath();
				i=i+3;
           }


        var canvas1 = document.getElementById('pizza1');
        var contexto1 = canvas1.getContext('2d');
          contexto1.fillStyle="white";
          //ctx.fillRect(0,0,300,150);
		  contexto1.clearRect(0,0,canvas.width,canvas.height);

           contexto1.beginPath();
          // contexto.fillStyle="orange";
          contexto1.lineWidth=5;
           contexto1.strokeStyle="#F16527";
           contexto1.arc(90,90,85,0,Math.PI*2,false);
          // contexto.fill();
           contexto1.stroke();
           contexto1.closePath();


           for(var i=0;i<=79;i++ ){
				contexto1.beginPath();
				contexto1.lineWidth=5;
				contexto1.strokeStyle="#F16527";
				contexto1.arc(90,90,i,(-Math.PI/180)*45,(Math.PI/180)*valor,false);
				contexto1.stroke();
				contexto1.closePath();
				i=i+3;
           }

       if(valor==315){

       	 contexto.fillStyle="#F16527";
          contexto.fillRect(60,60,50,50);

       	contexto.font="13pt Corbel";
       	
       	contexto.fillStyle="white";
       	contexto.fillText("Proceso Terminado",20,(canvas.height/2));



       }     


 }

 function GetSucursales() {
 	// body...
 	$('.loader').toggle();
 	$.ajax({
        type:"POST",
        url: "Default.aspx/GetSucursales",
        data:"{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
        	$("#select_sucursal option").remove();
        	$.each(response.d,function (index,reg) {
        		// body...
        		$('#select_sucursal').append(new Option(reg.Nombre ,reg.SucursalID, true, true));
        	});
        	$('#select_sucursal').append(new Option("NOMBRE DE SUCURSAL" ,0, true, true));
        	$('.loader').toggle();
        }
    });

 }

 function  GetTicket (Folio,Importe) {
 	// body...
	$('.loader').toggle();
 	$.ajax({
        type:"POST",
        url: "Default.aspx/ValidarTicket",
        data:"{'cadena':'"+Folio+"','Importe':'"+Importe+"'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
        	$('.content_datos_cap_1 > label.msg_lbl').remove();
        	$("#select_sucursal option").removeAttr('selected');
        	if(response.d.TicketID !=0 ){
        		$("#select_sucursal option[value='"+response.d.Sucursal.SucursalID+"']").attr('selected',true);
        		$('.content_datos_cap_1 > input[name=content1]').css({'background-color':'#0080FF','border':'1px solid #7ac9b7'});
				$('.content_datos_cap_1 > input[name=content1]').removeAttr('disabled');
				
				if(response.d.Cliente.RFC!=null){

					$('.content_datos_cap_1 > input[name=rfc_temp]').val(response.d.Cliente.RFC);
					$('input[name="btnEditar"]').css({'background-color':'lightblue','border':'1px solid #7ac9b7'});
					$('input[name="btnEditar"]').attr('disabled',true);

				}else{
					$('.content_datos_cap_1 > input[name=rfc_temp]').val("");
					$('input[name="btnEditar"]').css({'background-color':'#0080FF','border':'1px solid #7ac9b7'});
					$('input[name="btnEditar"]').removeAttr('disabled');
				}

					
			
				
        	}else{
        		$('.content_datos_cap_1').append('<label class="msg_lbl">Folio del ticket y el importe no coinciden. Por favor verifique sus datos o intente mas tarde.</label>');
        		$('.content_datos_cap_1 > input[name=content1]').css({'background-color':'lightblue','border':'1px solid #7ac9b7'});
				$('.content_datos_cap_1 > input[name=content1]').attr('disabled',true);
        		$("#select_sucursal option[value='0']").attr('selected',true);
        	}
			
			$('.loader').toggle();
        }
    });

 }
 function  GetCliente (RFC) {
 	// body...
 	$('.loader').toggle();
 	$.ajax({
        type:"POST",
        url: "Default.aspx/GetCliente",
        data:"{'RFC':'"+RFC+"'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
        	if(response.d.RFC!=null){
        		
        		$('.lst_info > li').find('input[name=rfc2_temp]').val(response.d.RFC);
				$('.content_dat_cap').children('input')[0].value=response.d.RazonSocial;
				$('.content_dat_cap').children('input')[1].value=response.d.Calle;
			
				$('.content_dat_cap > span').children('input')[0].value=response.d.NoExt;
				$('.content_dat_cap > span').children('input')[1].value=response.d.NoInt;
				$('.content_dat_cap').children('input')[2].value=response.d.Colonia;
				$('.content_dat_cap').children('input')[3].value=response.d.Delegacion;
				$('.content_dat_cap').children('input')[4].value=response.d.Ciudad;
				$('.content_dat_cap').children('input')[5].value=response.d.Municipio;
				$('.content_dat_cap').children('input')[6].value=response.d.Estado;
				$('.content_dat_cap').children('input')[7].value=response.d.CP;
				$('.content_dat_cap').children('input')[8].value=response.d.Contacto;

				if(response.d.Email1!=""){
					$('.content_dat_cap > div').children('input')[0].value=response.d.Email1;
				}
				if(response.d.Email2){
					$('.content_dat_cap > div').append('<input type="text" placeholder="luis@hotmail.com" value="'+response.d.Email2+'"/>');
				}
				if(response.d.Email3){
					$('.content_dat_cap > div').append('<input type="text" placeholder="luis@hotmail.com" value="'+response.d.Email3+'"/>');
				}

				$('.content_datos_cap_3 > div').children('input[name=empresa]').attr('disabled','disabled');
				$('.content_datos_cap_3 > div').children('input[name=calle]').attr('disabled','disabled');
				$('.content_datos_cap_3 > div > span').children('input[name=Next]').attr('disabled','disabled');
				$('.content_datos_cap_3 > div > span').children('input[name=Nint]').attr('disabled','disabled');
				$('.content_datos_cap_3 > div').children('input[name=colonia]').attr('disabled','disabled');
				$('.content_datos_cap_3 > div').children('input[name=delegacion]').attr('disabled','disabled');
				$('.content_datos_cap_3 > div').children('input[name=ciudad]').attr('disabled','disabled');
				$('.content_datos_cap_3 > div').children('input[name=municipio]').attr('disabled','disabled');
				$('.content_datos_cap_3 > div').children('input[name=estado]').attr('disabled','disabled');
				$('.content_datos_cap_3 > div').children('input[name=codpostal]').attr('disabled','disabled');


			}else{

				$('.content_datos_cap_3 > div').children('input[name=empresa]').removeAttr('disabled');
				$('.content_datos_cap_3 > div').children('input[name=calle]').removeAttr('disabled');
				$('.content_datos_cap_3 > div > span').children('input[name=Next]').removeAttr('disabled');
				$('.content_datos_cap_3 > div > span').children('input[name=Nint]').removeAttr('disabled');
				$('.content_datos_cap_3 > div').children('input[name=colonia]').removeAttr('disabled');
				$('.content_datos_cap_3 > div').children('input[name=delegacion]').removeAttr('disabled');
				$('.content_datos_cap_3 > div').children('input[name=ciudad]').removeAttr('disabled');
				$('.content_datos_cap_3 > div').children('input[name=municipio]').removeAttr('disabled');
				$('.content_datos_cap_3 > div').children('input[name=estado]').removeAttr('disabled');
				$('.content_datos_cap_3 > div').children('input[name=codpostal]').removeAttr('disabled');


				$('.lst_info > li').find('input[name=rfc2_temp]').val("");
				$('.content_dat_cap').children('input')[0].value="";
				$('.content_dat_cap').children('input')[1].value="";
			
				$('.content_dat_cap > span').children('input')[0].value="";
				$('.content_dat_cap > span').children('input')[1].value="";
				$('.content_dat_cap').children('input')[2].value="";
				$('.content_dat_cap').children('input')[3].value="";
				$('.content_dat_cap').children('input')[4].value="";
				$('.content_dat_cap').children('input')[5].value="";
				$('.content_dat_cap').children('input')[6].value="";
				$('.content_dat_cap').children('input')[7].value="";
				$('.content_dat_cap').children('input')[8].value="";
				
				if($('.content_dat_cap > div > input').length>1){
					$('.content_dat_cap > div > input').each(function  (index) {
						// body...
						
						if(index>0){$(this).remove(); }
					});
					
				}
				
				$('.content_dat_cap > div').children('input')[0].value="";
				


	
			}

			$('.loader').toggle();
        }
    });

 }
 function click_msg (id) {
 	// body...
	
	$('.loader').toggle(); 	
 	$('.content_msg').toggle();

 	if(id=="aceptar"){
 		
 		
 		//alert(GetDatos(".lst_info:visible > li"));
 		
 		var Client_Info=new Array();
	 	$('.lst_info:visible > li').each(function (index) {
	 		// body...
	 		if(index==3){
	 			Client_Info[index]=parseFloat($(this).children('input').val());
	 		}else{
	 			Client_Info[index]=$(this).children('input').val();
	 		}
	 	});



 		$.ajax({
	        type:"POST",
	        url: "Default.aspx/SaveCliente",
	        data:JSON.stringify({Client_Parameters: Client_Info}),
	        contentType: "application/json; charset=utf-8",
	        dataType: "json",
	        success: function (response) {
	        	$('.loader').toggle();
	        	if(response.d==""){
					$('.content_datos_cap_4').hide();
					$('.content_datos_cap_5').toggle();
					dibujarCanvas(315);
	        	}else{
	        		alert(response.d);
	        	}
	        }, error: function (error) {
	            alert('Error ' + eval(error));
	            console.log(error);
	            $('.loader1').toggle();

	        }
        	
        });
 	}



 	if(id=="modificar"){
 		
 		$('.content_datos_cap_4').hide();
 		$('.content_datos_cap_3').toggle();
 		$('.lst_info li').remove('.l3');
 		$('.loader').toggle(); 
 	}
 	
 }
 function GetDatos () {
 	// body...
 	//var array="";
 	var Client_Info=new Array();
 	$('.lst_info:visible > li').each(function (index) {
 		// body...

 		Client_Info[index]=$(this).children('input').val();

 		
 	});
 	return Client_Info;

 }
  function justNumbers(e) {
     var keynum = window.event ? window.event.keyCode : e.which;
     if ((keynum == 8) || (keynum == 46))
         return true;

     return /\d/.test(String.fromCharCode(keynum));
  }
  function pulsar(e,obj) {
  tecla = (document.all) ? e.keyCode : e.which;
  //alert(tecla);
  if (tecla!="8" && tecla!="0"){
  	obj.value += String.fromCharCode(tecla).toUpperCase();
  	return false;
  }else{
  	return true;
  }
}
function validarEmail( email ) {
    expr = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return expr.test(email);
      // return false;// alert("Error: La dirección de correo " + email + " es incorrecta.");
}
function ValRFC (argument) {
	// body...

	if(argument.length>12){
		expr=/^([A-Z\s]{4})\d{6}([A-Z\w]{3})$/;
		return expr.test(argument);
	}else{
		return true;
	}
	
	
}