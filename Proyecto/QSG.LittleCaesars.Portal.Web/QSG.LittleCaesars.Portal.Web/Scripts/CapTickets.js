var sucursales=new Array();
var now = new Date();

$(function  () {
	// body... #container_history > li > ul > li.column_FechaT


	

	//$('#txtFecha').datepicker();
	//$('#txtFecha').datepicker("option","dateFormat","dd/mm/yy");

      var FechaActualddmmyyyy=now.getDay()+"/"+now.getMonth()+"/"+now.getYear();
    // $('#txtFecha').val(FechaActualddmmyyyy);
      $('.column_FechaT').children('input').keyup(function  (event) {
      	// body...
      	//alert();
      });

	$('.row').children('ul').children('li').children('input').keyup(function  (e) {
		// body...
		if(e.keyCode==13){
			e.preventDefault();

		var inputs=$('.row').children('ul').children('li').find('input');
		inputs.eq(inputs.index(this)+1).focus();
		}
		
	});

	//var suc=new Array();
	/*
	$.ajax({
	    type: "POST",
	    url: "CapTickets.aspx/GetSucursales",
	    data:"{}",
	    contentType: "application/json; charset=utf-8",
	    dataType: "json",
	    async:false, 
	    success: function (response) {
	    	$.each(response.d,function (index,sucursal) {
	    		sucursales[index]=sucursal;

	    	});
	    }
	});*/

//alert(sucursales[0].Abr);


	
	

	$(document).keypress(function(e) { if(e.keyCode === 13) { e.preventDefault(); return false; } });
/*
	$('#btSpan,img').hover(function (event) {

		// body...
		$('#btSpan').removeClass('bntAdd');
		$('#btSpan').addClass('bntAdd2');
	});

	$('#btSpan,img').mouseout(function (event) {
		$('#btSpan').removeClass('bntAdd2');
		$('#btSpan').addClass('bntAdd');
	});
	
*/


});
function FillSuc(){
	var html="";
	$.ajax({
	    type: "POST",
	    url: "CapTickets.aspx/GetSucursales",
	    data:"{}",
	    contentType: "application/json; charset=utf-8",
	    dataType: "json",
	    async:false, 
	    success: function (response) {
	    	html=response.d;
	    	console.log(html);
	    	/*$.each(response.d,function (index,sucursal) {
	    		sucursales[index]=sucursal;

	    	});*/
	    }
	});
	return html;
}

function FillMetodoPago(){
	var html="";
	$.ajax({
	    type: "POST",
	    url: "CapTickets.aspx/GetMetodoPagoSAT",
	    data:"{}",
	    contentType: "application/json; charset=utf-8",
	    dataType: "json",
	    async:false, 
	    success: function (response) {
	    	html=response.d;
	    	console.log(html);
	    }
	});
	return html;
}








function Datepicker(fecha){
	$('#txtFecha').datepicker();
	$('#txtFecha').datepicker("option","dateFormat","dd/mm/yy");
	console.log(fecha);
	//$('#txtFecha').val(fecha);
}

function PassFecha(event){
	document.getElementById('txtFechaTemp').value=$(event.target).val();

}
function MostCalendar(event){
	$("#btnFecha").click();
}
function Mhover() {
	// body...
	$('#btSpan').removeClass('bntAdd');
	$('#btSpan').addClass('bntAdd2');
}
function Mout() {
	// body...
	$('#btSpan').removeClass('bntAdd2');
	$('#btSpan').addClass('bntAdd');
}

function SaveP() {
	// body...


	$('#container_history > li.rowM').each(function (index) {
		var ticket=new Array();
		var indexList=$(this).index();
		$('#container_history > li').eq(indexList).children('ul').children('li.column_sttReg').children('img').attr('src','../iconos/loanding_min.gif');
		

		ticket[0]=$(this).find('ul > li.row_ID').children('input').val();
		ticket[1]=$(this).find('ul > li.column_Sucursal').children('select').val();
		ticket[2]=$(this).find('ul > li.column_FechaT').children('input').val();
		ticket[3]=$(this).find('ul > li.column_HoraT').children('input').val();
		ticket[4]=$(this).find('ul > li.column_FolioT').children('input').val();
		ticket[5]=$(this).find('ul > li.column_Caja').children('input').val();
		ticket[6]=$(this).find('ul > li.column_Cajero').children('input').val();
		ticket[7]=$(this).find('ul > li.column_Importe').children('input').val();
		ticket[8]=$(this).find('ul > li.column_codUser').children('input').val();
		ticket[9]=$(this).find('ul > li.column_CodeReg').children('input').val();//column_CodeReg
		ticket[10]=$(this).find('ul > li.column_MetodoPago').children('select').val();


		

		$.ajax({
			    type: "POST",
			    url: "CapTickets.aspx/SaveTicket",
			    data: JSON.stringify({TicketT:ticket}),
			    contentType: "application/json; charset=utf-8",
			    dataType: "json",
			    //async:false, 
			    success: function (response) {
			    	var _result=response.d;
			    	var _msg=_result.split('|');//0 tipo 1 faltaltes 2 msg

			    	var _msgResult="";

			    	if(_msg[1]!=""){_msgResult="Datos Faltantes "+_msg[1];}
			    	_msgResult+=_msg[2];

			    	
			    	//$(this).find('ul > li.column_sttReg').children('img').attr('title',_msgResult);
			    	$('#container_history > li').eq(indexList).children('ul').children('li .column_sttReg').children('img').attr('title',_msgResult);

			    	if(_msg[0]=="0"){

			    		$(this).removeClass('rowM');
			    		$(this).addClass('row');

			    		//$(this).children('ul').children('li.column_sttReg').children('img').attr('src','../../iconos/ok-24.png');

			    		$('#container_history > li').eq(indexList).children('ul').children('li').children('input').addClass('inactive');
			    		$('#container_history > li').eq(indexList).children('ul').children('li').children('select').addClass('inactive');

			    		$('#container_history > li').eq(indexList).children('ul').children('li').children('input').attr('readonly',true);
						$('#container_history > li').eq(indexList).children('ul').children('li').children('select').attr('disabled',true);


			    		$('#container_history > li').eq(indexList).children('ul').children('li.column_sttReg').children('img').attr('src','../iconos/ok-24.png');
			    		
			    	}else{

			    		$('#container_history > li').eq(indexList).children('ul').children('li.column_sttReg').children('img').attr('src','../iconos/Icon-warning.png');
			    	}
			    	console.log(_msg[0]);

			    }
			});

	});



	
/*

	$('.loader').toggle();
	var ObjectTicket;
	var ObjectArray=new Array();
	var valida=false;
	var ExistError=false;
	var ErrorArray=new Array();
	var Errores="";
	$('#container_history > li.rowM').each(function (index) {
		
		Errores="";
		var indexList=$(this).index();
		valida=true;
		ObjectTicket=new Array();
		ObjectTicket[0]=$(this).find('ul > li.row_ID').children('input').val();/*row_ID*/
	/*	if(ObjectTicket[0]==""){valida=false; Errores+="ID desconocido,"; }
		ObjectTicket[1]=$(this).find('ul > li.column_Sucursal').children('select').val();/*SucursalID*/
	/*	if(ObjectTicket[1]==""){valida=false;Errores+="Sucursal desconocido,";}
		ObjectTicket[2]=$(this).find('ul > li.column_FechaT').children('input').val();/*FechaVta*/
		
	/*	if(ObjectTicket[2]==""){
			valida=false;
			Errores+="Fecha Ticket vacio,";
		}

		if(ObjectTicket[2]!=""){
			var v2=true;
			var fechat=ObjectTicket[2].toString().split('/');

			if(isNaN(parseInt(fechat[0])) || fechat[0]==""){
				v2=false;
			}
			if(isNaN(parseInt(fechat[1])) || fechat[1]==""){
				v2=false;
			}
			if(isNaN(parseInt(fechat[2])) || fechat[2]==""){
				v2=false;
			}
			if(!v2){
				valida=v2;
				Errores+="Formato Fecha Incorrecta,";
			}

		}	
		
		ObjectTicket[3]=$(this).find('ul > li.column_HoraT').children('input').val();/*HoraVta*/
	/*	if(ObjectTicket[3]==""){valida=false;Errores+="Hora de Venta vacia,";}
		

		ObjectTicket[4]=$(this).find('ul > li.column_FolioT').children('input').val();/*TicketID*/
	/*	if(ObjectTicket[4]==""){valida=false;Errores+="Folio Ticket vacio,";}
		ObjectTicket[5]=$(this).find('ul > li.column_Caja').children('input').val();/*CajaID*/
	/*	if(ObjectTicket[5]==""){valida=false;Errores+="Numero de caja vacio,";}
		ObjectTicket[6]=$(this).find('ul > li.column_Cajero').children('input').val();/*Cajero*/
	/*	if(ObjectTicket[6]==""){valida=false;Errores+="Nombre cajero vacio,";}
		ObjectTicket[7]=parseFloat($(this).find('ul > li.column_Importe').children('input').val().toString().replace(',','.') ).toFixed(2);/*Importe*/
	/*	if(isNaN(ObjectTicket[7])){
			valida=false;
			Errores+="Importe Incorrecto,";
		}

		if(ObjectTicket[7]==""){
			valida=false;
			Errores+="Importe vacio,";
		}
		
		if(ObjectTicket[7]=="0.00"){
			valida=false;
			Errores+="Importe debe ser mayor 0 ,";

		}
		
		ObjectTicket[8]=$(this).find('ul > li.column_codUser').children('input').val();/*CodUsuario*/
	/*	if(ObjectTicket[8]==""){valida=false;Errores+="Error codigo de usuario faltante";}
		ObjectTicket[9]=$(this).find('ul > li.column_CodeReg').children('input').val();/*codereg*/
		


	/*	if(valida){
			
			$.ajax({
			    type: "POST",
			    url: "CapTickets.aspx/SaveTicket",
			    data: JSON.stringify({TicketT:ObjectTicket}),
			    contentType: "application/json; charset=utf-8",
			    dataType: "json",
			    async:false, 
			    success: function (response) {
			    	//ErrorArray[index]=response.d;
			    	if(response.d==""){

			    		$(this).removeClass('rowM');
			    		$(this).addClass('row');

			    		$(this).attr('src','../../iconos/checkbox_unchecked (1).png');
			    		$('#container_history > li').eq(indexList).children('ul').children('li').children('input').addClass('inactive');
			    		$('#container_history > li').eq(indexList).children('ul').children('li').children('select').addClass('inactive');

			    		$('#container_history > li').eq(indexList).children('ul').children('li').children('input').attr('readonly',true);
						$('#container_history > li').eq(indexList).children('ul').children('li').children('select').attr('disabled',true);


			    		$('#container_history > li').eq(indexList).children('ul').children('li .column_sttReg').children('img').attr('src','../../iconos/ok-24.png');
			    		$('#container_history > li').eq(indexList).children('ul').children('li .column_sttReg').children('img').attr('title','Operacion exitosa');
			    	}

			    	if(response.d!=""){

			    		$('#container_history > li').eq(indexList).children('ul').children('li .column_sttReg').children('img').attr('src','../../iconos/warning-24.png');
			    		$('#container_history > li').eq(indexList).children('ul').children('li .column_sttReg').children('img').attr('title','Error Servidor : '+response.d);
			    	}


			    }
			});

		}
		else
		{
			$('#container_history > li').eq(indexList).children('ul').children('li .column_sttReg').children('img').attr('src','../../iconos/warning-24.png');
			$('#container_history > li').eq(indexList).children('ul').children('li .column_sttReg').children('img').attr('title','Errores : '+ Errores);

		}

	});

	
	$('.loader').toggle();*/
	
}



/*
function GetTicktes () {
		// body...

	$.ajax({
	    type: "POST",
	    url: "CapTickets.aspx/GetTickets",
	    data:"{}",
	    contentType: "application/json; charset=utf-8",
	    dataType: "json",
	    success: function (response) {
	    	$("#container_history li").remove();
	    	
	    	
	    	$("#container_history").append(response.d);
	    	//$('.loading').toggle();
	    }
});
*/

function CapFolio (event) {
	// body...
	if($(event.target).val().indexOf('#')>-1){
		var folio=$(event.target).val().split('#');
			if(event.keyCode ==13){
				event.preventDefault();
				if( folio[0].length ==6 && folio[1].length > 0){
					
					$('#container_history > li').eq($(event.target).parent().parent().parent().index()).children('ul').children('li .column_Sucursal').find("#select_sucursal option[value='"+parseInt(folio[0].substring(0,3)) +"']").attr('selected',true);
					$('#container_history > li').eq($(event.target).parent().parent().parent().index()).children('ul').children('li .column_Caja').children('input').val(folio[0].substring(3,6));
					$(event.target).val(folio[1]);
				}else{
					alert("Por favor ingrese folio valido");
				}
			}
	}
}
function Icon_msg (event) {
	// body...
	/*
	var index1=$(event.target).parent().parent().parent().index();
	if($(event.target).attr('src')=='../../iconos/ok-24.png'){
		$('#container_history > li').eq(index1).toggle('fast');
		$('#container_history > li').eq(index1).remove();
	}
	*/
}
function Del_Click (event) {
	// body...
	event.preventDefault();
	var index1=$(event.target).parent().parent().parent().index();

		var id=$(event.target).parent().parent().parent().attr('id');

		if(index1!=0){

			if($(event.target).attr('src')!='../../iconos/checkbox_checked.png'){
				$(event.target).attr('src','../../iconos/checkbox_checked.png');
				$('#container_history > li').eq(index1).children('ul').children('li').addClass('removeRow');
				$('#container_history > li').eq(index1).children('ul').children('li .column_Edit').children('img').attr('disabled','disabled');
				
				$('#container_history > li').eq(index1).children('ul').children('li .row_ID').children('input').val('STT2');
				$(event.target).parent().parent().parent().removeClass('row');
				$(event.target).parent().parent().parent().addClass('rowM');
			}else{
				$(event.target).attr('src','../../iconos/checkbox_unchecked (1).png');
				$('#container_history > li').eq(index1).children('ul').children('li').removeClass('removeRow');
				$('#container_history > li').eq(index1).children('ul').children('li .column_Edit').children('img').removeAttr('disabled');
				$('#container_history > li').eq(index1).children('ul').children('li .row_ID').children('input').val($('#container_history > li').eq(index1).children('ul').children('li .column_stt').children('input').val());
				$(event.target).parent().parent().parent().removeClass('rowM');
				$(event.target).parent().parent().parent().addClass('row');
			}	

		}



}
//nada STT nuevo STT0 edit STT1 eliminar STT2
function Edit_Click(event) {
	// body...
	event.preventDefault();
	var index1=$(event.target).parent().parent().parent().index();

		var id=$(event.target).parent().parent().parent().attr('id');
		if(index1!=0){
			//alert($(event.target).attr('src'));
			

			if($(event.target).attr('src')!='../../iconos/checkbox_checked.png'){
				$(event.target).attr('src','../../iconos/checkbox_checked.png');
				
				var TicketID=$('#container_history > li').eq(index1).children('ul').children('li .column_FolioT').children('input').val();
				var SucursalID=$('#container_history > li').eq(index1).children('ul').children('li .column_Sucursal').children('select').val();
				var CajaID=$('#container_history > li').eq(index1).children('ul').children('li .column_Caja').children('input').val();
				var Importe=$('#container_history > li').eq(index1).children('ul').children('li .column_Importe').children('input').val();
				var FechaVta=$('#container_history > li').eq(index1).children('ul').children('li .column_FechaT').children('input').val();
				var MetodoPago=$('#container_history > li').eq(index1).children('ul').children('li .column_MetodoPago').children('select').val();	

				$('#container_history > li').eq(index1).children('ul').children('li').children('input').removeClass('inactive');
				$('#container_history > li').eq(index1).children('ul').children('li').children('select').removeClass('inactive');

				$('#container_history > li').eq(index1).children('ul').children('li').children('input').removeAttr('readonly');
				$('#container_history > li').eq(index1).children('ul').children('li').children('select').removeAttr('disabled');

				$('#container_history > li').eq(index1).children('ul').children('li .column_Del').children('img').attr('disabled','disabled');
				$(event.target).parent().parent().parent().removeClass('row');
				$(event.target).parent().parent().parent().addClass('rowM');

				if($('#container_history > li').eq(index1).children('ul').children('li .row_ID').children('input').val()!="STT0" ){
					$('#container_history > li').eq(index1).children('ul').children('li .row_ID').children('input').val('STT1');
					$('#container_history > li').eq(index1).children('ul').children('li .column_CodeReg').children('input').val(TicketID+"|"+SucursalID+"|"+CajaID+"|"+Importe+"|"+FechaVta+"|"+MetodoPago);
				}	
				
				$('#container_history > li').eq(index1).children('ul').children('li .column_UserCap').children('input').addClass('inactive');
				$('#container_history > li').eq(index1).children('ul').children('li .column_UserCap').children('input').attr('readonly',true);


					/*
				if(id!='STT0'){
					$(event.target).parent().parent().parent().attr('id','STT1');
				}

						*/


			}else{

				var ValTemp=$('#container_history > li').eq(index1).children('ul').children('li .column_CodeReg').children('input').val();
				var Valores=ValTemp.split(';');
				$(event.target).attr('src','../../iconos/checkbox_unchecked (1).png');

				$('#container_history > li').eq(index1).children('ul').children('li').children('input').addClass('inactive');
				$('#container_history > li').eq(index1).children('ul').children('li').children('select').addClass('inactive');

				$('#container_history > li').eq(index1).children('ul').children('li').children('input').attr('readonly',true);
				$('#container_history > li').eq(index1).children('ul').children('li').children('select').attr('disabled',true);

				$('#container_history > li').eq(index1).children('ul').children('li .column_Del').children('img').removeAttr('disabled');

				$('#container_history > li').eq(index1).children('ul').children('li .row_ID').children('input').val($('#container_history > li').eq(index1).children('ul').children('li .column_stt').children('input').val());
				//$(event.target).parent().parent().parent().attr('id',$('#container_history > li').eq(index1).children('ul').children('li .column_stt').children('input').first().val());
				$(event.target).parent().parent().parent().removeClass('rowM');
				$(event.target).parent().parent().parent().addClass('row');

				$('#container_history > li').eq(index1).children('ul').children('li .column_Sucursal').find("#select_sucursal option[value='"+Valores[1]+"']").attr('selected',true);
				$('#container_history > li').eq(index1).children('ul').children('li .column_FolioT').children('input').val(Valores[0]);
				//$('#container_history > li').eq(index1).children('ul').children('li .column_Sucursal').children('select').val(Valores[1]);
				$('#container_history > li').eq(index1).children('ul').children('li .column_Caja').children('input').val(Valores[2]);
				$('#container_history > li').eq(index1).children('ul').children('li .column_Importe').children('input').val(Valores[3]);
				$('#container_history > li').eq(index1).children('ul').children('li .column_FechaT').children('input').val(Valores[4]);
				$('#container_history > li').eq(index1).children('ul').children('li .column_MetodoPago').find("#select_MetodoPago option[value='"+Valores[5]+"']").attr('selected',true);


			}	

		}

}

function valida (e) {
	// body...

	if(e.keyCode!=8){
			var hora=e.target.value;
			if(hora.length ==2){

				if(parseInt(hora)>24){
					hora=24;				  
				}
				e.target.value= hora+":";
			}
			if(hora.length>4){
				if(parseInt(hora.substring(3,hora.length))>59){
					e.target.value=hora.substring(0,3)+"59";
				}
			}
		
		
	}
	
		
}

function ValDate (e) {
	// body... dia mes año
	var date=new Date();
	var date1=new Date();
	var fechaG="";

	if(e.keyCode!=8){
	
		
		
		var fecha=e.target.value;
		if(fecha.length==2){
			if(parseInt(fecha)>31){
				fecha=31;
			}
			e.target.value=fecha+"/";
		}
		if(fecha.length==5){
			if(parseInt(fecha.substring(3,fecha.length))>12){
				fecha=fecha.substring(0,3)+"12";

			}
			e.target.value=fecha+"/";
		}
		if(fecha.length==10){
			
			
			if(parseInt(fecha.substring(6,fecha.length))> date.getFullYear()) {
				fecha=fecha.substring(0,6)+date.getFullYear();
			}
			fechaG=fecha.split('/');
			date1=new Date(fechaG[2],(fechaG[1]-1),fechaG[0]);
			//alert(date1);
			if(date1>date){
				fecha=("0"+date.getDate()).slice(-2)+"/"+("0"+(date.getMonth()+1) ).slice(-2)+"/"+date.getFullYear();
			}
		
			
			e.target.value=fecha;
		}
	}

}

function Run_Report (event) {
	// body...
	if(event.keyCode==13){
		addRow(event);
		$('#container_history > li').eq(1).children('ul').children('li .column_Sucursal').children('select').focus();
		$('#btSpan').removeClass('bntAdd2');
		$('#btSpan').addClass('bntAdd');
	} 

}
function  Ejecutar(event) {
	// body...

	if(event.keyCode==13){
		$('#btSpan').removeClass('bntAdd');
		$('#btSpan').addClass('bntAdd2');
		$('#inputTemp_runReport').focus();

	}
}

function addRow (event) {
	// body...
	//event.preventDefault();
// alert($('#BtnFechaCap').val());
	//if(sucursales.length>0){
	//var fechaE=$('#BtnFechaCap').val().toString().split('/');
	//<img src='../iconos/uncheck_icon3.png' onclick='Edit_Click(event);' />
	var html="";
		html += "	<li class='rowM' >";
		html += "    <ul >";
		html += "    <li  class='row_ID'><input value='STT0'/></li>";
		html += "    <li  class='column_ID'></li>";
		html += "    <li  class='column_Edit column_fillColor'><img src='../iconos/checkbox_unchecked (1).png' onclick='Edit_Click(event);'/></li>";
		html += "    <li  class='column_Del column_fillColor'><img src='../iconos/checkbox_unchecked (1).png' onclick='Del_Click(event);'/></li>";
		
		html += "    <li  class='column_Sucursal' ><select id='select_sucursal' autofocus> ";
		html +=FillSuc();
		/*
		for (var i =0;i<sucursales.length;i++) {
			html+="<option value='"+sucursales[i].SucursalID +"'>"+("000"+sucursales[i].SucursalID).slice(-3)+"-"+sucursales[i].Nombre +"</option>";
		}
		
	*/
		html+=" </select></li>";

		html += "    <li  class='column_MetodoPago' ><select id='select_MetodoPago' autofocus> ";
		html +=FillMetodoPago();
		/*
		for (var i =0;i<sucursales.length;i++) {
			html+="<option value='"+sucursales[i].SucursalID +"'>"+("000"+sucursales[i].SucursalID).slice(-3)+"-"+sucursales[i].Nombre +"</option>";
		}
		
	*/
		html+=" </select></li>";

		html += "<li class='column_FechaT'><input onkeyup='this.value=formateafecha(this.value,30,0);'  maxlength='10' placeholder='dd/mm/aaaa' value='"+$('#txtFecha').val()+"'  /></li>";
		html += "<li class='column_HoraT'><input  placeholder='HH:MM' onkeyup='valida(event);' onkeypress='return justNumbers(event);' maxlength='5' /> </li>";
		html += "<li  class='column_FolioT'><input onkeyup='CapFolio(event);' /></li>";
		html += "<li  class='column_Caja'><input  />    </li>";
		html += "<li  class='column_Cajero'><input    />   </li>";
		html += "<li  class='column_Importe'><input  onkeyup='Ejecutar(event);' onkeypress='return justNumbers(event);' /></li>";
		html += "<li  class='column_UserCap'><input class='inactive'  value='"+$('#txtUsuario').val()+"' readonly/> </li>";
		html += "<li  class='column_stt'><input  value='STT0' /></li>";
		html += "<li  class='column_codUser' ><input  value='"+$('#User_id').val()+"' /></li>";
		html += "<li  class='column_sttReg' ><img></li>";
		html += "<li  class='column_CodeReg' ><input /></li>";
		html += "<li  class='column_RFC' ><input /></li>";
		html += "</ul> ";
		html += "</li>";

	
		$(html).insertAfter('#container_history > li:eq(0)');

		$('#container_history > li').each(function (index) {
			// body...
		
			//$(this).attr('id',(index).toString());
			if(index!=0){
				$(this).find('ul > li.column_ID').text((index).toString());
			}
			

		});
	//}

}

function justNumbers(e) {
     var keynum = window.event ? window.event.keyCode : e.which;
     if ((keynum == 8) || (keynum == 46  ))
         return true;

     return /\d/.test(String.fromCharCode(keynum));
  }

  