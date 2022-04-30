$(function  () {
	// body...


})

function Cancel (event) {
	// body...
	event.preventDefault();
	//$('')

}
function OK (event) {
	// body...
}

function BntClick (event) {
	// body...
	
	//$('#fondo-negro').css('display','block');
	//$('#MsgBox').css('display','block');
	var cancelo=false;
	var index1=$(event.target).parent().parent().parent().index();
	var _Fact_Inf=new Array();
	var id=$(event.target).parent().parent().parent().attr('id');

	if($(event.target).val()!="Cancelar"){return false;}

	if($('#container_history > li').eq(index1).children('ul').children('li .column_UUID').text()==""){return false;}

	if($('#container_history > li').eq(index1).children('ul').children('li .column_MotC').children('input').val()!=""){

			if(confirm('Esta seguro que desea cancelar esta factura?')){

					
					_Fact_Inf[2]=$('#container_history > li').eq(index1).children('ul').children('li .column_folioFact').text();
					_Fact_Inf[1]=$('#container_history > li').eq(index1).children('ul').children('li .column_RFCfact').text();
					_Fact_Inf[0]=$('#container_history > li').eq(index1).children('ul').children('li .column_UUID').text();
					_Fact_Inf[3]=$('#container_history > li').eq(index1).children('ul').children('li .column_FolioT').text()+"#"+$('#container_history > li').eq(index1).children('ul').children('li .column_Importe').children('input').val();
					_Fact_Inf[4]=$('#container_history > li').eq(index1).children('ul').children('li .column_MotC').children('input').val();

					//alert($('#container_history > li').eq(index1).children('ul').children('li .column_folioFact').text());

					$("#loader").toggle();

					$.ajax({
					    type: "POST",
					    url: "CancelacionFacturas.aspx/CancelarFactura",
					    data:JSON.stringify({Fact_Inf:_Fact_Inf}),
					    contentType: "application/json; charset=utf-8",
					    dataType: "json",
					    async:false, 
					    success: function (response) {

					    	if(response.d==""){
					    		$('#container_history > li').eq(index1).children('ul').children('li .column_STTFact').children('input[type=button]').removeClass('ButtonCancelar');
					    		$('#container_history > li').eq(index1).children('ul').children('li .column_STTFact').children('input[type=button]').addClass('ButtonCancelado');
					    		$('#container_history > li').eq(index1).children('ul').children('li .column_STTFact').children('input[type=button]').val('Cancelado');
					    		$('#container_history > li').eq(index1).children('ul').children('li .column_MotC').children('input[type=text]').attr('disabled','disables');
					    		cancelo=true;
					    	}else{
					    		alert("Detalle del Error: "+response.d);
					    		_Fact_Inf=null;
					    		$('#container_history > li').eq(index1).children('ul').children('li .column_MotC').children('input').val("");
					    	}
					    	$("#loader").toggle();
					    }

					});

					if(cancelo){

						var mensaje="Su factura con folio ( "+_Fact_Inf[2]+" ) ya fue cancelada ¿Desea reutilizar el ticket? \nDe clic en ACEPTAR solamente si desea emitir una nueva factura porque hubo error en la primera que se esta cancelando \n(el folio nuevo de ticket vendra en negativo y podra utilizarse para una nueva factura). \nDe clic en CANCELAR en caso contrario";
							
							if(confirm(mensaje)){

								$("#loader").toggle();

								$.ajax({
							    type: "POST",
							    url: "CancelacionFacturas.aspx/NewTicket",
							    data:"{folio:'"+_Fact_Inf[3]+"'}",
							    contentType: "application/json; charset=utf-8",
							    dataType: "json",
							    async:false, 
							    success: function (response) {
							    	if(response.d==""){
							    		alert("Nuevo ticket generado con folio negativo.");
							    	}
							     	$("#loader").toggle();
							    }

							});
						
						}
					}
					

			}else{
				return false;
			}
	}else{
		alert("Por Favor Ingrese detalle de cancelacion, Gracias.");
		$('#container_history > li').eq(index1).children('ul').children('li .column_MotC').children('input').focus();
	}

}