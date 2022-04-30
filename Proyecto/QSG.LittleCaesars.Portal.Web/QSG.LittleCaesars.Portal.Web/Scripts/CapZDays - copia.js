var sucursales=new Array();
var _decimales=2;
var _separadorD=".";
var _separadorM=",";
$(function () {
	// body...
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


	$.ajax({
	    type: "POST",
	    url: "CapZDaylis.aspx/GetSucursales",
	    data:"{}",
	    contentType: "application/json; charset=utf-8",
	    dataType: "json",
	    async:false, 
	    success: function (response) {
	    	$.each(response.d,function (index,sucursal) {
	    		sucursales[index]=sucursal;

	    	});
	    }
	});
})


function SaveIndormacion(){
	var _hizoCambio=false;
	var msg=false;
	
	$('#container_history > li.rowM').each(function (index) {

		//console.log($('#container_history > li').eq($(this).index()).children('ul').children('li .column_sttReg').children('img').attr('src'));

		if($(this).find('ul > li.column_sttReg').children('img').attr('src')=='../../iconos/sign_down.png' ){_hizoCambio=true;}
	});


	//$('#container_history > li').eq(index1).children('ul').children('li .column_sttReg').children('img').attr('src','../../iconos/sign_down.png')
	
	if(_hizoCambio){
		msg=confirm('Existen cortes Z modificados que no tienen corte sucursal,La informacion del corte se perdera ¿Desea Continuar? ');
	}else{
		msg=true;
	}

//	console.log(_hizoCambio);
	//console.log($(this).find('ul > li .column_sttReg').children('img').attr('src'));
	
	if(msg){

	$('.loader').toggle();
	var ObjArray=new Array();
	var ObjArraySuc=new Array();
	var Error=false;
	var MsgError="";
	var ArraySave=new Array();

	$('.loader').toggle();

	var countIndex=$('#container_history > li.rowM').length;
	console.log(countIndex);
	if(countIndex > 0){
		//eliminar nuevos que no estan grabados
		$('#container_history > li.row').each(function (index) {
			//console.log($(this).find('ul > li.row_ID').children('input').val());
			if($(this).find('ul > li.row_ID').children('input').val()=="ST2"){
				$(this).remove();
			}
		});

	$('#container_history > li.rowM').each(function (index) {
		var indexList=$(this).index();
			ObjArray=new Array();


			ObjArray[0]=$(this).find('ul > li.column_CodeRegZ').children('input').val();/*code registro z*/
			ObjArray[1]=$(this).find('ul > li.row_ID').children('input').val();/*row_ID*/

			ObjArray[2]=$(this).find('ul > li.column_Sucursal').children('select').val();/*code sucursal*/

			ObjArray[3]=$(this).find('ul > li.column_FechaT').children('input').val();/* fecha z*/

			ObjArray[4]=$(this).find('ul > li.column_HoraT').children('input').val();/*hora z*/
			ObjArray[5]=$(this).find('ul > li.column_FolioT').children('input').val();/*folio z*/
			ObjArray[6]=$(this).find('ul > li.column_Caja').children('input').val();/*caja z*/
			ObjArray[7]=$(this).find('ul > li.column_NT').children('input').val();/*numero z*/
			ObjArray[8]=$(this).find('ul > li.column_Cajero').children('input').val();/*cejeroz*/
			ObjArray[9]=parseFloat(quitarLetra($(this).find('ul > li.column_ImpEfectivo').children('input').val()));/*efectivo z*/
			ObjArray[10]=parseFloat(quitarLetra($(this).find('ul > li.column_ImpTcredito').children('input').val()));/*hTcredit z*/
			ObjArray[11]=parseFloat(quitarLetra($(this).find('ul > li.column_OFPago').children('input').val()));/*ofPagoz*/
			ObjArray[12]=parseFloat(quitarLetra($(this).find('ul > li.column_Importe').children('input').val()));/*Importe z*/
			ObjArray[13]=$(this).find('ul > li.column_codUser').children('input').val();/*Importe z*/

			/*recororero corte sucursal*/

			$('#container_history1 > li').each(function (index) {
			var indexList=$(this).index();
				//ObjArraySuc=new Array();
				var _SucId=$(this).find('ul > li.column_Sucursal').children('select').val();
				var _FDaily=$(this).find('ul > li.column_FechaDaily').children('input').val(); 

				if(ObjArray[2]+""+ObjArray[3] == _SucId+""+_FDaily){
						ObjArray[14]=parseFloat(quitarLetra($(this).find('ul > li.column_EfectivoDepP').children('input').val()));
						ObjArray[15]=parseFloat(quitarLetra($(this).find('ul > li.column_EfectivoDepD').children('input').val()));
						//ObjArray[8]=$(this).find('ul > li.column_FolioServices').children('input').val();
						ObjArray[16]=parseFloat(quitarLetra($(this).find('ul > li.column_BolsaP').children('input').val()));
						ObjArray[17]=parseFloat(quitarLetra($(this).find('ul > li.column_BolsaD').children('input').val()));
						ObjArray[18]=$(this).find('ul > li.column_FolioServices').children('input').val();

						ObjArray[19]=parseFloat(quitarLetra($(this).find('ul > li.column_GastoDeu').children('input').val()));
						ObjArray[20]=parseFloat(quitarLetra($(this).find('ul > li.column_Sob').children('input').val()));
					
						ObjArray[21]=parseFloat(quitarLetra($(this).find('ul > li.column_TipoCambio').children('input').val()));//$("#TipoC").val();
						ObjArray[22]=$(this).find('ul > li.column_coment').children('input').val();
						ObjArray[23]=parseFloat(quitarLetra($(this).find('ul > li.column_Falt').children('input').val()));

				}


			});

			/**********************/
			
			$.ajax({
			    type: "POST",
			    url: "CapZDaylis.aspx/SaveZ",
			    data: JSON.stringify({DatosZ:ObjArray}),
			    contentType: "application/json; charset=utf-8",
			    dataType: "json",
			    async:false, 
			    success: function (response) {

			    	if(response.d==""){

						$('#container_history > li').eq(indexList).removeClass('rowM');
						$('#container_history > li').eq(indexList).addClass('row');

						if($('#container_history > li').eq(indexList).children('ul .row_ID').children('li').children('input').val()!="ST2" ){
							//row_ID
							//$(this).attr('src','../../iconos/checkbox_unchecked (1).png');
							$('#container_history > li').eq(indexList).children('ul').children('li').children('input').addClass('inactive');
							$('#container_history > li').eq(indexList).children('ul').children('li').children('select').addClass('inactive');

							$('#container_history > li').eq(indexList).children('ul').children('li').children('input').attr('readonly',true);
							$('#container_history > li').eq(indexList).children('ul').children('li').children('select').attr('disabled',true);


							$('#container_history > li').eq(indexList).children('ul').children('li .column_sttReg').children('img').attr('src','../../iconos/ok-24.png');
							$('#container_history > li').eq(indexList).children('ul').children('li .column_sttReg').children('img').attr('title','Operacion exitosa');

							$('#container_history > li').eq(indexList).children('ul').children('li .column_Edit').children('img').attr('src','../../iconos/checkbox_unchecked (1).png');

							//$(event.target).attr('src','../../iconos/checkbox_unchecked (1).png');

							$('#container_history > li').eq(indexList).children('ul').children('li .row_ID').children('input').val("ST");
							//_ULRegZ.parent().removeClass('rowM');
							//_ULRegZ.parent().addClass('row'); 
							//}

							
					    	//$('.loader').toggle();

						}else{

							$('#container_history > li').eq(indexList).remove();
						}
						
			    	}else{
			    		//alert(response.d);
			    		$('#container_history > li').eq(indexList).children('ul').children('li .column_sttReg').children('img').attr('src','../../iconos/Icon-warning.png');
						$('#container_history > li').eq(indexList).children('ul').children('li .column_sttReg').children('img').attr('title','No fue posible grabar ');
			    	}
			    },
			    error:function(response){
			    	
			    	$('.loader').toggle();
			    }
			});
			

			

			//}
	});
	
		alert("Operacion finalizada con exito");
		$('.loader').toggle();

	
	}else{


	/*   grabar summary de sucursales*/
	if($('#container_history1 > li.rowM').length > 0){

	$('#container_history1 > li.rowM').each(function (index) {
		var indexList=$(this).index();
			ObjArray=new Array();

			ObjArray[0]=$(this).find('ul > li.column_CodeRegZ').children('input').val();
			ObjArray[1]=$(this).find('ul > li.row_ID').children('input').val();
			ObjArray[2]=$(this).find('ul > li.column_Sucursal').children('select').val();
			ObjArray[3]=$(this).find('ul > li.column_FechaDaily').children('input').val();
			//ObjArray[4]=$(this).find('ul > li.column_NumZ').children('input').val();
			//ObjArray[5]=$(this).find('ul > li.column_TotalIng').children('input').val();
			/*ad columns*/
			//ObjArray[6]=$(this).find('ul > li.column_EfectivoDepP').children('input').val();
			//ObjArray[7]=$(this).find('ul > li.column_EfectivoDepP').children('input').val();


			ObjArray[4]=parseFloat(quitarLetra($(this).find('ul > li.column_EfectivoDepP').children('input').val()));
			ObjArray[5]=parseFloat(quitarLetra($(this).find('ul > li.column_EfectivoDepD').children('input').val()));
			//ObjArray[8]=$(this).find('ul > li.column_FolioServices').children('input').val();
			ObjArray[6]=parseFloat(quitarLetra($(this).find('ul > li.column_BolsaP').children('input').val()));
			ObjArray[7]=parseFloat(quitarLetra($(this).find('ul > li.column_BolsaD').children('input').val()));
			ObjArray[8]=$(this).find('ul > li.column_FolioServices').children('input').val();

			ObjArray[9]=parseFloat(quitarLetra($(this).find('ul > li.column_GastoDeu').children('input').val()));
			ObjArray[10]=parseFloat(quitarLetra($(this).find('ul > li.column_Sob').children('input').val()));
			//ObjArray[15]=$(this).find('ul > li.column_TotalEfectZ').children('input').val();
			//ObjArray[16]=$(this).find('ul > li.column_VariacionDayli').children('input').val();
			ObjArray[11]=$(this).find('ul > li.column_codUser').children('input').val();
		
			ObjArray[12]=$("#TipoC").val();
			ObjArray[13]=$(this).find('ul > li.column_coment').children('input').val();
			ObjArray[14]=parseFloat(quitarLetra($(this).find('ul > li.column_Falt').children('input').val()));

			$.ajax({
			    type: "POST",
			    url: "CapZDaylis.aspx/SaveSummary",
			    data: JSON.stringify({Summary:ObjArray}),
			    contentType: "application/json; charset=utf-8",
			    dataType: "json",
			    async:false, 
			    success: function (response) {
			    	
			    	if(response.d==""){
				    	$('#container_history1 > li').eq(indexList).removeClass('rowM');
						$('#container_history1 > li').eq(indexList).addClass('row');

						$('#container_history1 > li').eq(indexList).children('ul').children('li').children('input').addClass('inactive');
						$('#container_history1 > li').eq(indexList).children('ul').children('li').children('select').addClass('inactive');

						$('#container_history1 > li').eq(indexList).children('ul').children('li').children('input').attr('readonly',true);
						$('#container_history1 > li').eq(indexList).children('ul').children('li').children('select').attr('disabled',true);

						$('#container_history1 > li').eq(indexList).children('ul').children('li .column_Edit').children('img').attr('src','../../iconos/checkbox_unchecked (1).png');
					}else{
					//alert(response.d);
					}

			    }

			   });

		});

		alert("Operacion finalizada con exito");
		$('.loader').toggle();

			}
		}

	
	}else{

		return false;
	}

}






function ADDRow(event){
	event.preventDefault();

	var fechaE=$('#txtFecha').val().toString().split('/');

	var html="<li class='rowM'><ul >";
		html +="<li class='column_stt'><input value='ST0' /></li>"
		html +="<li class='column_CodeRegZ'><input /></li>";
		html +="<li class='row_ID' ><input value='ST0'/></li>";
		html +="<li class='column_ID'>1</li>";
		html +="<li class='column_Edit'><img class='btnEdit' src='iconos/checkbox_unchecked (1).png' title='Editar' onclick='Editar(event);'/></li>";
		html +="<li class='column_Del'><img class='btnDel' src='iconos/checkbox_unchecked (1).png' title='Eliminar registro' onclick='Eliminar(event);'/></li>";
		html +="<li class='column_Sucursal'>";
		html +="<select id='select_sucursal' onchange='verificar(event);'  autofocus>";

		for (var i =0;i<sucursales.length;i++) {
			
			html+="<option value='"+sucursales[i].SucursalID +"'>"+("000"+sucursales[i].SucursalID).slice(-3)+"-"+sucursales[i].Nombre +"</option>";
		
		}

		html +="</select></li>";
		html +="<li class='column_FechaT' ><input value='"+$("#txtFecha").val()+"' /></li>";
		html +="<li class='column_HoraT'><input  placeholder='HH:MM' onkeyup='valida(event);'  onkeypress='return justNumbers(event);' maxlength='5' /></li>";
		html +="<li class='column_FolioT'><input onkeyup='verificar(event);' /></li>";
		html +="<li class='column_Caja'><input onkeypress='return justNumbers(event);' onkeyup='verificar(event);'/></li>";
		html +="<li class='column_NT'><input onkeypress='return justNumbers(event);' onkeyup='verificar(event);'/></li>";
		html +="<li class='column_Cajero'><input onkeyup='verificar(event);' /></li>";
		html +="<li class='column_ImpEfectivo'><input onkeypress='return justNumbers(event);' onkeyup='verificar(event);' value='0' /></li>";
		html +="<li class='column_ImpTcredito'><input onkeypress='return justNumbers(event);' onkeyup='verificar(event);' value='0' /></li>";
		html +="<li class='column_OFPago'><input onkeypress='return justNumbers(event);' onkeyup='verificar(event);' value='0' /></li>";
		html +="<li class='column_Importe'><input value='0' class='inactive' readonly/></li><li class='column_sttReg'><img onclick='Add_Z(event);' /></li> <li class='column_codUser'><input value='"+$("#SelectUsers").val()+"'/></li></ul></li> ";

		$(html).insertAfter('#container_history > li:eq(0)');

		$('#container_history > li').each(function (index) {
			// body...
		
			//$(this).attr('id',(index).toString());
			if(index!=0){
				$(this).find('ul > li.column_ID').text((index).toString());
			}
			

		});

}
/*
function Calcular(event){
	var index1=$(event.target).parent().parent().parent().index();

	var id=$(event.target).parent().parent().parent().attr('id');

	var SucursalID=parseInt( $('#container_history > li').eq(index1).children('ul').children('li .column_Sucursal').children('select').val());
	var CajaID=$('#container_history > li').eq(index1).children('ul').children('li .column_Caja').children('input').val();
	var FolioT=$('#container_history > li').eq(index1).children('ul').children('li .column_FolioT').children('input').val();
	var FechaT=$('#container_history > li').eq(index1).children('ul').children('li .column_FechaT').children('input').val();
	var Importe=$('#container_history > li').eq(index1).children('ul').children('li .column_ImpEfectivo').children('input').val();


	var Efectivo=0;
	var countZ=0;
	var encontro=false;
	var index2=null;
	var ingreso=0;
	var change=false;

	///var Error=false;

	//Error=Field($('#container_history > li').eq(index1).children('ul').children('li .column_Sucursal').children('select').val());
	//Error=Field($('#container_history > li').eq(index1).children('ul').children('li .column_FechaT').children('input').val());
	//Error=Field($('#container_history > li').eq(index1).children('ul').children('li .column_HoraT').children('input').val());
	//Error=Field($('#container_history > li').eq(index1).children('ul').children('li .column_FolioT').children('input').val());
	//Error=Field($('#container_history > li').eq(index1).children('ul').children('li .column_Caja').children('input').val());
	//Error=Field($('#container_history > li').eq(index1).children('ul').children('li .column_NT').children('input').val());
	//=Field($('#container_history > li').eq(index1).children('ul').children('li .column_ImpEfectivo').children('input').val());

	

	if(verificar(event)==""){

		

				}
}
*/


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
	
	verificar(event);	
}
function EditarDaily(event){
	event.preventDefault();
	var _parent=$(event.target).parent().parent();
	var _index_Li=_parent.parent().index();
	var _id_Row=_parent.attr('id');

//alert();
	var _ULRegZ=$('#container_history1 > li').eq(_index_Li).children('ul');

	if(_index_Li==0)
		return false;


	if($(event.target).attr('src')!='../../iconos/checkbox_checked.png'){
		$(event.target).attr('src','');
		$(event.target).attr('src','../../iconos/checkbox_checked.png');


		if(_ULRegZ.children('ul').children('li .row_ID').children('input').val() != "ST0"){
			_ULRegZ.parent().removeClass('row');
			_ULRegZ.parent().addClass('rowM'); 
		}

		_ULRegZ.find('li.column_TipoCambio').children().removeClass('inactive');
		_ULRegZ.find('li.column_TipoCambio').children().attr('readonly',false);

		_ULRegZ.find('li.column_EfectivoDepP').children().removeClass('inactive');
		_ULRegZ.find('li.column_EfectivoDepP').children().attr('readonly',false);

		_ULRegZ.find('li.column_EfectivoDepD').children().removeClass('inactive');
		_ULRegZ.find('li.column_EfectivoDepD').children().attr('readonly',false);

		_ULRegZ.find('li.column_BolsaP').children().removeClass('inactive');
		_ULRegZ.find('li.column_BolsaP').children().attr('readonly',false);

		_ULRegZ.find('li.column_BolsaD').children().removeClass('inactive');
		_ULRegZ.find('li.column_BolsaD').children().attr('readonly',false);

		_ULRegZ.find('li.column_GastoDeu').children().removeClass('inactive');
		_ULRegZ.find('li.column_GastoDeu').children().attr('readonly',false);

		_ULRegZ.find('li.column_Sob').children().removeClass('inactive');
		_ULRegZ.find('li.column_Sob').children().attr('readonly',false);

		_ULRegZ.find('li.column_Falt').children().removeClass('inactive');
		_ULRegZ.find('li.column_Falt').children().attr('readonly',false);

		//_ULRegZ.find('li input').attr('readonly',false);
	//	_ULRegZ.find('li select').attr('disabled',false);


	}else{
		
		$(event.target).attr('src','../../iconos/checkbox_unchecked (1).png');

			if(_ULRegZ.children('ul').children('li .row_ID').children('input').val() != "ST0"){
				_ULRegZ.parent().removeClass('rowM');
				_ULRegZ.parent().addClass('row'); 
			}

			_ULRegZ.find('li.column_TipoCambio').children().addClass('inactive');
		_ULRegZ.find('li.column_TipoCambio').children().attr('readonly',true);

		_ULRegZ.find('li.column_EfectivoDepP').children().addClass('inactive');
		_ULRegZ.find('li.column_EfectivoDepP').children().attr('readonly',true);

		_ULRegZ.find('li.column_EfectivoDepD').children().addClass('inactive');
		_ULRegZ.find('li.column_EfectivoDepD').children().attr('readonly',true);

		_ULRegZ.find('li.column_BolsaP').children().addClass('inactive');
		_ULRegZ.find('li.column_BolsaP').children().attr('readonly',true);

		_ULRegZ.find('li.column_BolsaD').children().addClass('inactive');
		_ULRegZ.find('li.column_BolsaD').children().attr('readonly',true);

		_ULRegZ.find('li.column_GastoDeu').children().addClass('inactive');
		_ULRegZ.find('li.column_GastoDeu').children().attr('readonly',true);

		_ULRegZ.find('li.column_Sob').children().addClass('inactive');
		_ULRegZ.find('li.column_Sob').children().attr('readonly',true);

		_ULRegZ.find('li.column_Falt').children().addClass('inactive');
		_ULRegZ.find('li.column_Falt').children().attr('readonly',true);


	}

}

function Editar(event){
	event.preventDefault();
	var _parent=$(event.target).parent().parent();
	var _index_Li=_parent.parent().index();
	var _id_Row=_parent.attr('id');

//alert();
	var _ULRegZ=$('#container_history > li').eq(_index_Li).children('ul');

	if(_index_Li==0)
		return false;

	var _SucId=_ULRegZ.children('li .column_Sucursal').children('select').val();
	var _FechaZ=_ULRegZ.children('li .column_FechaT').children('input').val();
	var _folioZ=_ULRegZ.children('li .column_FolioT').children('input').val();
	var _CodCaja=_ULRegZ.children('li .column_Caja').children('input').val();
	var _Importe=parseFloat(quitarLetra(_ULRegZ.children('li .column_Importe').children('input').val()));
	var _ImpEfectivo=parseFloat(quitarLetra(_ULRegZ.children('li .column_ImpEfectivo').children('input').val()));

	var _codeAnt=_folioZ+"|"+_SucId+"|"+_CodCaja+"|"+_FechaZ +"|"+_Importe;
	//$('#container_history > li').eq(index1).children('ul').children('li .column_CodeRegZ').children('input').val(
	var _SttAnt=_ULRegZ.children('ul').children('li .column_stt').children('input').val(); 	

	_ULRegZ.children('li .column_CodeRegZ').children('input').val(_codeAnt);	

	if($(event.target).attr('src')!='../../iconos/checkbox_checked.png'){
	
		if(confirm('Esta consciente que un cambio en el registro de Zs ,hara necesario revizar los datos del Daily Cash Summary.\n¿Esta seguro de continuar?')){

			$(event.target).attr('src','');
			$(event.target).attr('src','../../iconos/checkbox_checked.png');

			/*habilitar los texbox para editar*/
			_ULRegZ.find('li').children().removeClass('inactive');
			_ULRegZ.find('li input').attr('readonly',false);
			_ULRegZ.find('li select').attr('disabled',false);

			_ULRegZ.children('li .column_Del').children('img').attr('disabled','disabled');
			
			if(_ULRegZ.children('li .row_ID').children('input').val() != "ST0"){
				_ULRegZ.children('li .row_ID').children('input').val("ST1");
				_ULRegZ.parent().removeClass('row');
				_ULRegZ.parent().addClass('rowM'); 
				//_ULRegZ.parent().removeClass('rowM');
			//	_ULRegZ.parent().addClass('row');
			}else{
				//_ULRegZ.parent().removeClass('row');
				//_ULRegZ.parent().addClass('rowM');

			}


			/* descontar esta sucursal del daily cash summary*/
			$('#container_history1 > li').each(function (index) {
					if(index > 0){
						var SucId=$(this).find('ul > li.column_Sucursal').children('select').val();
						var FDaily=$(this).find('ul > li.column_FechaDaily').children('input').val();

						if(_SucId+""+_FechaZ == SucId+""+FDaily){
							
							var _TotIng =parseFloat(quitarLetra($(this).find('li.column_TotalIng').children('input').val()));
							var _NoZ =parseInt($(this).find('li.column_NumZ').children('input').val());
							var _TotEfectZ =parseFloat(quitarLetra($(this).find('li.column_TotalEfectZ').children('input').val()));

							var SubTIng=_TotIng - _Importe;
							var SubNoz=_NoZ-1;
							var SubTEfZ=_TotEfectZ - _ImpEfectivo;

							$(this).find('li.column_TotalIng').children('input').val("$"+ formatoNumero(SubTIng,_decimales,_separadorD,_separadorM));
							$(this).find('li.column_NumZ').children('input').val(SubNoz);
							$(this).find('li.column_TotalEfectZ').children('input').val("$"+ formatoNumero( SubTEfZ,_decimales,_separadorD,_separadorM));

							$(this).removeClass('row')
							$(this).addClass('rowM');
							
							if(SubNoz == 0 ){
								
								$(this).children('ul').addClass('removeRow');
								$(this).children('ul > li.row_ID').children('input').val('ST2');

							}else{
								$(this).children('ul').removeClass('removeRow');
								$(this).children('ul > li.row_ID').children('input').val('ST1');
							}

							return false;
						}	
					}
				});
				
			}else{

				return false;
			}


	}else{
			$(event.target).attr('src','../../iconos/checkbox_unchecked (1).png');

			_ULRegZ.children('li .column_Del').children('img').removeAttr('disabled');
			//_ULRegZ.children('ul').children('li .row_ID').children('input').val(_SttAnt);

			_ULRegZ.find('li').children().addClass('inactive')
			_ULRegZ.find('li input').attr('readonly',true);
			_ULRegZ.find('li select').attr('disabled',true);
			
			if(_ULRegZ.children('li .row_ID').children('input').val() != "ST0"){
				_ULRegZ.children('li .row_ID').children('input').val(_SttAnt);
				_ULRegZ.parent().removeClass('rowM');
				_ULRegZ.parent().addClass('row');
			}else{
			//	_ULRegZ.parent().removeClass('row');
			//	_ULRegZ.parent().addClass('rowM');

			}
			
	}



}

function Eliminar(event){
	event.preventDefault();
	var _parent=$(event.target).parent().parent();
	var _index_Li=_parent.parent().index();
	var _id_Row=_parent.attr('id');

	var _lastIndex=$('#container_history1 > li').length-1;
	var _ULRegZ=$('#container_history > li').eq(_index_Li).children('ul');

	if(_index_Li==0)
		return false;

		var _SttAnt=_ULRegZ.children('li .column_stt').children('input').val(); //$('#container_history > li').eq(index1).children('ul').children('li .column_stt').children('input').val()

		var _SucId=_ULRegZ.children('li .column_Sucursal').children('select').val();
  		var _FechaZ=_ULRegZ.children('li .column_FechaT').children('input').val();
  		var _Importe=parseFloat(quitarLetra(_ULRegZ.children('li .column_Importe').children('input').val()));
  		var _ImpEfectivo=parseFloat(quitarLetra(_ULRegZ.children('li .column_ImpEfectivo').children('input').val()));
  		var _ImpTC=parseFloat(quitarLetra(_ULRegZ.children('li .column_ImpTcredito').children('input').val()));
  		var _ImpOFP=parseFloat(quitarLetra(_ULRegZ.children('li .column_OFPago').children('input').val()));


  		var _tot_Ing=parseFloat(quitarLetra($('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_TotalIng').children('input').val()));
  		var _tot_TC=parseFloat(quitarLetra($('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_ImpTcredito').children('input').val()));
  		var _tot_OFP=parseFloat(quitarLetra($('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_OFPago').children('input').val()));
  		var _tot_EZ=parseFloat(quitarLetra($('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_TotalEfectZ').children('input').val()));


		if($(event.target).attr('src')!='../../iconos/checkbox_checked.png'){
			if(confirm('Esta seguro eliminar el registro Z ?')){
			
			$(event.target).attr('src','');
			$(event.target).attr('src','../../iconos/checkbox_checked.png');

			_ULRegZ.addClass('removeRow');
			_ULRegZ.children('li .column_Edit').children('img').attr('disabled','disabled');

			//console.log(_ULRegZ.children('li .row_ID').children('input').val());
			if(_ULRegZ.children('li .row_ID').children('input').val() !="ST0" ){

				_ULRegZ.children('li .row_ID').children('input').val('ST2');

				_ULRegZ.parent().removeClass('row');
				_ULRegZ.parent().addClass('rowM');
				console.log(_ULRegZ.children('li .row_ID').children('input').val());

			}else{
				_ULRegZ.parent().removeClass('rowM');
				_ULRegZ.parent().addClass('row');
			}

			$('#container_history1 > li').each(function (index) {
				if(index > 0){
					var SucId=$(this).find('ul > li.column_Sucursal').children('select').val();
					var FDaily=$(this).find('ul > li.column_FechaDaily').children('input').val();

					if(_SucId+""+_FechaZ == SucId+""+FDaily){
						var _TotIng =parseFloat(quitarLetra($(this).find('li.column_TotalIng').children('input').val()));
						var _TotTC =parseFloat(quitarLetra($(this).find('li.column_ImpTcredito').children('input').val()));
						var _TotOFP =parseFloat(quitarLetra($(this).find('li.column_OFPago').children('input').val()));
						var _NoZ =parseFloat(quitarLetra($(this).find('li.column_NumZ').children('input').val()));
						var _TotEfectZ =parseFloat(quitarLetra($(this).find('li.column_TotalEfectZ').children('input').val()));

						var SubTIng=_TotIng - _Importe;
						var SubTC=_TotTC - _ImpTC;
						var SubOFP=_TotOFP -_ImpOFP;
						var SubNoz=_NoZ-1;
						var SubTEfZ=_TotEfectZ - _ImpEfectivo;

						$(this).find('li.column_TotalIng').children('input').val("$"+ formatoNumero( SubTIng ,_decimales,_separadorD,_separadorM));
						$(this).find('li.column_NumZ').children('input').val(SubNoz);
						$(this).find('li.column_TotalEfectZ').children('input').val("$"+ formatoNumero( SubTEfZ,_decimales,_separadorD,_separadorM));
						$(this).find('li.column_ImpTcredito').children('input').val("$"+ formatoNumero( SubTC,_decimales,_separadorD,_separadorM));
						$(this).find('li.column_OFPago').children('input').val("$"+ formatoNumero( SubOFP,_decimales,_separadorD,_separadorM));


						$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_TotalIng').children('input').val("$"+ formatoNumero(  _tot_Ing -_Importe,_decimales,_separadorD,_separadorM));
						$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_ImpTcredito').children('input').val("$"+ formatoNumero( _tot_TC - _ImpTC,_decimales,_separadorD,_separadorM));
						$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_OFPago').children('input').val("$"+ formatoNumero( _tot_OFP - _ImpOFP,_decimales,_separadorD,_separadorM));
						$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_TotalEfectZ').children('input').val("$"+ formatoNumero( _tot_EZ - _ImpEfectivo,_decimales,_separadorD,_separadorM));


						$(this).removeClass('row')
						$(this).addClass('rowM');

						if(SubNoz == 0 ){
							
							$(this).children('ul').addClass('removeRow');
							$(this).children('ul > li.row_ID').children('input').val('ST2');

						}else{
							$(this).children('ul > li.row_ID').children('input').val('ST1');
						}

						return false;
					}

					
				}


			});

		}else{return false;}

		}else{

			$(event.target).attr('src','../../iconos/checkbox_unchecked (1).png');

			_ULRegZ.removeClass('removeRow');
			_ULRegZ.children('li .column_Edit').children('img').removeAttr('disabled');
			//_ULRegZ.children('ul').children('li .row_ID').children('input').val(_SttAnt);
			
			if(_ULRegZ.children('li .row_ID').children('input').val() != "ST0"){
				_ULRegZ.children('li .row_ID').children('input').val(_SttAnt);
				_ULRegZ.parent().removeClass('rowM');
				_ULRegZ.parent().addClass('row');
			}else{
				_ULRegZ.parent().removeClass('rowM');
				_ULRegZ.parent().addClass('row');
			}
			

			$('#container_history1 > li').each(function(index){

				if(index > 0){
					var SucId=$(this).find('ul > li.column_Sucursal').children('select').val();
					var FDaily=$(this).find('ul > li.column_FechaDaily').children('input').val();

					if(_SucId+""+_FechaZ == SucId+""+FDaily){
						var _TotIng =parseFloat(quitarLetra($(this).find('li.column_TotalIng').children('input').val()));
						var _NoZ =parseInt($(this).find('li.column_NumZ').children('input').val());
						var _TotEfectZ =parseFloat(quitarLetra($(this).find('li.column_TotalEfectZ').children('input').val()));

						var _TotTC =parseFloat(quitarLetra($(this).find('li.column_ImpTcredito').children('input').val()));
						var _TotOFP =parseFloat(quitarLetra($(this).find('li.column_OFPago').children('input').val()));

						var SubTIng=_TotIng + _Importe;
						var SubNoz=_NoZ+1;
						var SubTEfZ=_TotEfectZ + _ImpEfectivo;
						var SubTC=_TotTC + _ImpTC;
						var SubOFP=_TotOFP + _ImpOFP;

						$(this).find('li.column_TotalIng').children('input').val("$"+ formatoNumero( SubTIng,_decimales,_separadorD,_separadorM));
						$(this).find('li.column_NumZ').children('input').val(SubNoz)
						$(this).find('li.column_TotalEfectZ').children('input').val("$"+ formatoNumero( SubTEfZ,_decimales,_separadorD,_separadorM));
						$(this).find('li.column_ImpTcredito').children('input').val("$"+ formatoNumero( SubTC,_decimales,_separadorD,_separadorM ));
						$(this).find('li.column_OFPago').children('input').val("$"+ formatoNumero( SubOFP,_decimales,_separadorD,_separadorM ));

						$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_TotalIng').children('input').val("$"+ formatoNumero( _tot_Ing +_Importe,_decimales,_separadorD,_separadorM));
						$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_ImpTcredito').children('input').val("$"+ formatoNumero(_tot_TC + _ImpTC,_decimales,_separadorD,_separadorM));
						$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_OFPago').children('input').val("$"+ formatoNumero( _tot_OFP + _ImpOFP,_decimales,_separadorD,_separadorM));
						$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_TotalEfectZ').children('input').val( "$"+ formatoNumero(_tot_EZ + _ImpEfectivo,_decimales,_separadorD,_separadorM));

						$(this).children('ul').removeClass('removeRow');

						return false;
					}

				}
			});

		}


}


function Variacion(event,Datos,indexCol){
	event.preventDefault();
	var _parent=$(event.target).parent().parent();
	var _index_Li=_parent.parent().index();
	var _id_Row=_parent.attr('id');
	var _UlId=_parent.parent().parent().attr('id');
	//efectivos +servicios blindado(pesos) )-total efectivo)-sobrantes)+gastos=variacion

	var result=0;
	var _variacion=0;
	var _resultTot=0;

	//result=(((Datos.EfectivoP+Convertidor(Datos.EfectivoD,Datos.TC,1)+Datos.BolsaP+Convertidor(Datos.BolsaD,Datos.TC,1)) - Datos.TotalEfectivo) - Datos.Sobrantes) + Datos.Gastos;

	result=Datos.EfectivoP + Convertidor(Datos.EfectivoD,Datos.TC,1) + Datos.BolsaP + Convertidor(Datos.BolsaD,Datos.TC,1) + Datos.Gastos;
	_variacion=result-Datos.TotalEfectivo;

	$("#"+_UlId+" > li").eq(_index_Li).children('ul').children('li.column_TotalEfectivo').children('input').val( "$"+ formatoNumero( result,_decimales,_separadorD,_separadorM));

	$("#"+_UlId+" > li").eq(_index_Li).children('ul').children('li.column_EfectivoDepPConv').children('input').val("$"+ formatoNumero(Convertidor(Datos.EfectivoD,Datos.TC,1),_decimales,_separadorD,_separadorM ));

	$("#"+_UlId+" > li").eq(_index_Li).children('ul').children('li.column_BolsaPConv').children('input').val("$"+ formatoNumero( Convertidor(Datos.BolsaD,Datos.TC,1),_decimales,_separadorD,_separadorM ));

	//si es sobrante sera signo negativo

	if(Datos.Sobrantes > 0 ){
	 _resultTot=_variacion - Datos.Sobrantes;
	 _resultTot=_resultTot + Datos.Faltantes;
	}else{
		_resultTot=_variacion + Datos.Faltantes;
	}

	

	console.log(Datos.Sobrantes);
	//console.log(Convertidor(Datos.BolsaD,Datos.TC,1) );
	$("#"+_UlId+" > li").eq(_index_Li).children('ul').children('li:visible').eq(indexCol).children('input').val("$"+ formatoNumero( _resultTot,_decimales,_separadorD,_separadorM));
	
	if(_resultTot < 0){
		$("#"+_UlId+" > li").eq(_index_Li).children('ul').children('li:visible').eq(indexCol).children('input').css({'color':'red'});
	}else{
		$("#"+_UlId+" > li").eq(_index_Li).children('ul').children('li:visible').eq(indexCol).children('input').css({'color':'black'});
	}

}
function Convertidor(monto,tipoCambio,tipoconversion){
	/* tipo de converion 0=Pesos a dolares, 1 Dolares a pesos*/

	var result=0;
	if(isNaN(parseFloat(tipoCambio))){return false;}
	if(!isNaN(parseFloat(monto))){
		if(tipoconversion==0){
			result =(monto / tipoCambio);
		}
		if (tipoconversion==1 ) {
			result =(monto * tipoCambio);
		}

	}

	return result;
}
function CalcSumatorias(event,ArrayColIndex,IndexCol){
	//event.preventDefault();
	event.preventDefault();
	var _parent=$(event.target).parent().parent();
	var _index_Li=_parent.parent().index();
	var _id_Row=_parent.attr('id');
	var _UlId=_parent.parent().parent().attr('id');

	/*obtener subtotales del renglon*/
	var SubTotalRow=0;
	var _total=0;
	var ArrayTotales=new Array(ArrayColIndex.length);
	var _lastIndex=$("#"+_UlId+" > li").length-1;

	for(var i=0;i< ArrayTotales.length;i++){
		ArrayTotales[i]=0;
	}

	for(var i=0; i < ArrayColIndex.length; i++ ){

		var value=parseFloat(quitarLetra( $("#"+_UlId+" > li").eq(_index_Li).children('ul').children('li:visible').eq(ArrayColIndex[i]).children('input').val()));
			if(value !=""){
				if(isNaN(parseFloat(value))){
					value=0;
				}else{
					value=parseFloat(value);
				}
			}else{
				value=0;
			}


		SubTotalRow =SubTotalRow + value;
	}


	$("#"+_UlId+" > li").each(function(index){

		if(index > 0 && index < _lastIndex){

			for(var i=0; i < ArrayTotales.length;i++){

				var value=parseFloat(quitarLetra( $(this).children('ul').children('li:visible').eq(ArrayColIndex[i]).children('input').val()));
				
				if(value !=""){
					if(isNaN(parseFloat(value))){
						value=0;
					}else{
						value=parseFloat(value);
					}
				}else{
					value=0;
				}
				
				ArrayTotales[i]=ArrayTotales[i] +value;
			}
		}


	});


	for(var i=0; i< ArrayColIndex.length;i++ ){
		_total = _total + ArrayTotales[i];
		$("#"+_UlId+" > li").eq(_lastIndex).children('ul').children('li:visible').eq(ArrayColIndex[i]).children('input').val("$"+ formatoNumero( ArrayTotales[i],_decimales,_separadorD,_separadorM));
	}

	if(IndexCol > -1){
		/*sutotal en renglon*/
		$("#"+_UlId+" > li").eq(_index_Li).children('ul').children('li:visible').eq(IndexCol).children('input').val("$"+ formatoNumero( SubTotalRow,_decimales,_separadorD,_separadorM));
		

		/*total general cordenada la columan de total de cada renglon y el ultimo renglon */

		$("#"+_UlId+" > li").eq(_lastIndex).children('ul').children('li:visible').eq(IndexCol).children('input').val("$"+ formatoNumero( _total,_decimales,_separadorD,_separadorM));
	}
}


function justNumbers(e) {
     var keynum = window.event ? window.event.keyCode : e.which;
     if ((keynum == 8) || (keynum == 46 ))
         return true;

     return /\d/.test(String.fromCharCode(keynum));
  }
  function justNumbers2(e) {
     var keynum = window.event ? window.event.keyCode : e.which;
     if ((keynum == 8) || (keynum == 46 )  || (keynum==45))
         return true;

     return /\d/.test(String.fromCharCode(keynum));
  }

  function Field(value){
  	
  	if(value!=null){

	  	if(value.length > 0){
	  		return false;
	  	}else{
	  		return true;
	  	}
  	
  	}else{
  		
  		return true;
  	}

  }


  function verificar(event){
  event.preventDefault();
	var index1=$(event.target).parent().parent().parent().index();
	var id=$(event.target).parent().parent().parent().attr('id');
	var Error=false;
	var ErrCampos="";
	//alert($(event.target).attr('disabled') );

	if($(event.target).attr('readonly')!='readonly'){

		Error=Field($('#container_history > li').eq(index1).children('ul').children('li .column_Sucursal').children('select').val());
		
		if(Error){ErrCampos +="\n"+ $('#container_history > li').eq(0).children('ul').children('li .column_Sucursal').text();}

		Error=Field($('#container_history > li').eq(index1).children('ul').children('li .column_FechaT').children('input').val());
		
		if(Error){ErrCampos +="\n"+ $('#container_history > li').eq(0).children('ul').children('li .column_FechaT').text();}

		Error=Field($('#container_history > li').eq(index1).children('ul').children('li .column_HoraT').children('input').val());
	
		if(Error){
			ErrCampos +="\n"+ $('#container_history > li').eq(0).children('ul').children('li .column_HoraT').text();
		}
		
		Error=Field($('#container_history > li').eq(index1).children('ul').children('li .column_FolioT').children('input').val());
		
		if(Error){ErrCampos +="\n"+ $('#container_history > li').eq(0).children('ul').children('li .column_FolioT').text();}

		Error=Field($('#container_history > li').eq(index1).children('ul').children('li .column_Caja').children('input').val());
		
		if(Error){ErrCampos +="\n"+ $('#container_history > li').eq(0).children('ul').children('li .column_Caja').text();}

		Error=Field($('#container_history > li').eq(index1).children('ul').children('li .column_NT').children('input').val());
		
		if(Error){ErrCampos +="\n"+ $('#container_history > li').eq(0).children('ul').children('li .column_NT').text();}

		Error=Field($('#container_history > li').eq(index1).children('ul').children('li .column_Cajero').children('input').val());
		if(Error){ErrCampos +="\n"+ $('#container_history > li').eq(0).children('ul').children('li .column_Cajero').children('input').val();}

		Error=Field($('#container_history > li').eq(index1).children('ul').children('li .column_ImpEfectivo').children('input').val());
		
		if(Error){ErrCampos +="\n"+ $('#container_history > li').eq(0).children('ul').children('li .column_ImpEfectivo').text();}

		if(isNaN(parseFloat($('#container_history > li').eq(index1).children('ul').children('li .column_ImpEfectivo').children('input').val())) ){
			ErrCampos +="\n"+"Cash debe ser Numerico.";
		}else{
			if(parseFloat($('#container_history > li').eq(index1).children('ul').children('li .column_ImpEfectivo').children('input').val())==0 ){
				ErrCampos +="\n"+"Cash debe ser mayor a 0";
			}else{
				//ErrCampos="";
			}
		}

		//Error=Field($('#container_history > li').eq(index1).children('ul').children('li .column_ImpTcredito').children('input').val());
		//if(Error){ErrCampos +="\n"+$('#container_history > li').eq(0).children('ul').children('li .column_ImpTcredito').text();}

		//Error=Field($('#container_history > li').eq(index1).children('ul').children('li .column_OFPago').children('input').val());
		//if(Error){ErrCampos +="\n"+$('#container_history > li').eq(0).children('ul').children('li .column_OFPago').text();}
		
		//var count=ErrCampos.replace(/\s/g,'').length;
		//alert(ErrCampos);

		if(ErrCampos.replace(/\s/g,'')=="") {
			
			$('#container_history > li').eq(index1).children('ul').children('li .column_sttReg').children('img').attr('src','../../iconos/sign_down.png');
			$('#container_history > li').eq(index1).children('ul').children('li .column_sttReg').children('img').attr('title','Agregar Z al Daily');

			//CalcImporte(event);
			CalcSumatorias(event,[10,11,12],13);
		
		}else{
			
			$('#container_history > li').eq(index1).children('ul').children('li .column_sttReg').children('img').attr('src','../../iconos/exclamation.png');
			$('#container_history > li').eq(index1).children('ul').children('li .column_sttReg').children('img').attr('title','Error En los siguientes campos: ' + ErrCampos);
		}

	}
	return ErrCampos;


  }

  function Add_Z(event){
  		event.preventDefault();
  		var _parent=$(event.target).parent().parent();
  		var _index_Li=_parent.parent().index();
  		var _id_Row=_parent.attr('id');

  		var _lastIndex=$('#container_history1 > li').length-1;/*ultimo index de  sucursal consolidado*/

  		console.log(_lastIndex);
  		var _ULRegZ=$('#container_history > li').eq(_index_Li).children('ul');

  		/* datos*/
  		/**/

  		var _SucId=_ULRegZ.children('li .column_Sucursal').children('select').val();
  		var _FechaZ=_ULRegZ.children('li .column_FechaT').children('input').val();
  		var _Importe=parseFloat(quitarLetra(_ULRegZ.children('li .column_Importe').children('input').val()));
  		var _ImpEfectivo=parseFloat(quitarLetra(_ULRegZ.children('li .column_ImpEfectivo').children('input').val()));
  		var _TCredito=parseFloat(quitarLetra(_ULRegZ.children('li .column_ImpTcredito').children('input').val()));
  		var _OFPago=parseFloat(quitarLetra(_ULRegZ.children('li .column_OFPago').children('input').val()));

  		/*totales*/

  		var _totIng=parseFloat(quitarLetra($('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_TotalIng').children('input').val()));
  		var _totimpTc=parseFloat(quitarLetra($('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_ImpTcredito').children('input').val()));
  		var _totOFP=parseFloat(quitarLetra($('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_OFPago').children('input').val()));
  		/*
  		var _totEDP=$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_EfectivoDepP').children('input').val();
  		var _totEDD=$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_EfectivoDepD').children('input').val();
  		var _totBP=$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_BolsaP').children('input').val();
  		var _totBD=$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_BolsaD').children('input').val();
  		var _totG=$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_GastoDeu').children('input').val();
  		var _totSob=$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_SobFalt').children('input').val();
  		*/
  		var _totEfZ=parseFloat(quitarLetra($('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_TotalEfectZ').children('input').val()));
  		
  		console.log(_totIng);
  		
  		/*variables*/
  		var encontro=false;


  			if($(event.target).attr('src')=='../../iconos/sign_down.png'){/* condicion si esta habilitado para agrgar  al daily cash sumary*/
  				
  				$(event.target).attr('src','../../iconos/document-save-as.png');
  				$(event.target).attr('title',"Z Agregado con exito.");
		  		/*******buscar sucursal fecha**************/
				$('#container_history1 > li').each(function (index) {


					var SucId=$(this).find('ul > li.column_Sucursal').children('select').val();
					var FDaily=$(this).find('ul > li.column_FechaDaily').children('input').val();



					if(_SucId+""+_FechaZ == SucId+""+FDaily){
						
						//var _index=$(this).index();
						
						var _TotIng =parseFloat(quitarLetra($(this).find('li.column_TotalIng').children('input').val()));
						var _NoZ =parseFloat($(this).find('li.column_NumZ').children('input').val());
						var _TotEfectZ =parseFloat(quitarLetra($(this).find('li.column_TotalEfectZ').children('input').val()));
						var _TCr =parseFloat(quitarLetra($(this).find('li.column_ImpTcredito').children('input').val()));
						var _OFP =parseFloat(quitarLetra($(this).find('li.column_OFPago').children('input').val()));


						var SubTIng=_TotIng + _Importe;
						var SubNoz=_NoZ+1;
						var SubTEfZ=_TotEfectZ + _ImpEfectivo;
						var ImpTcredit=_TCr +_TCredito;
						var ImpOTF=_OFP + _OFPago;


						$(this).find('li.column_TotalIng').children('input').val("$"+ formatoNumero( SubTIng,_decimales,_separadorD,_separadorM));
						$(this).find('li.column_NumZ').children('input').val(SubNoz)
						$(this).find('li.column_TotalEfectZ').children('input').val("$"+ formatoNumero( SubTEfZ,_decimales,_separadorD,_separadorM));
						$(this).find('li.column_ImpTcredito').children('input').val( "$"+ formatoNumero(ImpTcredit,_decimales,_separadorD,_separadorM));
						$(this).find('li.column_OFPago').children('input').val("$"+ formatoNumero( ImpOTF,_decimales,_separadorD,_separadorM));

						$(this).children('ul').removeClass('removeRow');
						encontro=true;	

						$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_TotalIng').children('input').val("$"+ formatoNumero( _totIng + _Importe ,_decimales,_separadorD,_separadorM));
						$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_ImpTcredito').children('input').val("$"+ formatoNumero( _totimpTc +_TCredito,_decimales,_separadorD,_separadorM));
						$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_OFPago').children('input').val("$"+ formatoNumero( _totOFP + _OFPago,_decimales,_separadorD,_separadorM));
						/*
						$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_EfectivoDepP').children('input').val();
						$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_EfectivoDepD').children('input').val();
						$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_BolsaP').children('input').val();
						$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_BolsaD').children('input').val();
						$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_GastoDeu').children('input').val();
						$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_SobFalt').children('input').val();*/

						$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_TotalEfectZ').children('input').val("$"+ formatoNumero( _totEfZ + _ImpEfectivo,_decimales,_separadorD,_separadorM));

						return false;
					}

				});


				if(!encontro){

					var Datos={Fecha:_FechaZ,NumZ:1,TotalIng:_Importe,TCredit:_TCredito,OFPago:_OFPago,TotalEfectZ:_ImpEfectivo,SucursalID:_SucId,TipoCambio:$('#TipoC').val() };
					AddDaily(sucursales,Datos);
				}

				_ULRegZ.find('li').children().addClass('inactive');
				_ULRegZ.find('li input').attr('readonly',true);
				_ULRegZ.find('li select').attr('disabled',true);

				/*
			_ULRegZ.children('input').addClass('inactive');
			_ULRegZ.children('select').addClass('inactive');

			_ULRegZ.children('input').attr('readonly',true);
			_ULRegZ.children('select').attr('disabled',true);
			*/

			}	

  		/********************/

//alert(SucursalID);


  		//var _indexRow=_parent.index();
  		

  		//var index1=$(event.target).parent().parent().parent().index();
	//var id=$(event.target).parent().parent().parent().attr('id');


  }
  function AddDaily(ArraySuc,ArrayDatos){

	var html ="<li class='rowM'>";
		html +="<ul>";
		html +="<li class='column_stt' ><input value='ST0' /></li>"
		html +="<li class='column_CodeRegZ'><input /></li>";
		html +="<li class='row_ID' ><input value='ST0'/></li>";
		html +="<li class='column_ID'>No.</li>";
		html +="<li class='column_Edit'><img class='btnEdit' src='iconos/checkbox_unchecked (1).png' onclick='EditarDaily(event);' title='Editar registro'/></li>";
		html +="<li class='column_Sucursal'>";
		html +="<select id='select_sucursal2' disabled=true class='inactive' autofocus>";

		for (var i =0;i<ArraySuc.length;i++) {
			html+="<option value='"+ArraySuc[i].SucursalID +"'>"+("000"+ArraySuc[i].SucursalID).slice(-3)+"-"+ArraySuc[i].Nombre +"</option>";
		}

		html +="</select>";
		html +="</li>";
		html +="<li class='column_FechaDaily' ><input value='"+ArrayDatos.Fecha +"' class='inactive'  readonly=true/></li>";
		html +="<li class='column_NumZ'><input value='"+ArrayDatos.NumZ +"' class='inactive'  readonly/></li>";
		html +="<li class='column_TotalIng'><input value='"+ArrayDatos.TotalIng+"' class='inactive'  readonly/></li>";
		/*add columns*/
		html +="<li class='column_ImpTcredito' ><input value='"+ArrayDatos.TCredit +"' class='inactive'  readonly/></li>";
		html +="<li class='column_OFPago' ><input value='"+ArrayDatos.OFPago +"'  class='inactive'  readonly/></li>";

		html +="<li class='column_TotalEfectivo'><input value='0' class='inactive' readonly/></li>";
		html +="<li class='column_TotalEfectZ' style='background-color: lightblue;'><input value='"+ArrayDatos.TotalEfectZ +"' class='inactive' readonly/></li>";
		html +="<li class='column_VariacionDayli'><input value='0' class='inactive' readonly/></li>";

		html +="<li class='column_TipoCambio'><input value='"+ArrayDatos.TipoCambio+"' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);' /></li>";

		html +="<li class='column_EfectivoDepP' style='background-color: lightblue;'><input value='0' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);' /></li>";
		html +="<li class='column_EfectivoDepD' ><input value='0' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);' /></li>";
		
		html +="<li class='column_EfectivoDepPConv' style='background-color: lightblue;'><input value='0' class='inactive'  readonly/></li>";

		//html +="<li class='column_FolioServices'><input /></li>";
		html +="<li class='column_BolsaP' style='background-color: lightblue;'><input  value='0' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);'/></li>";
		html +="<li class='column_BolsaD'><input  value='0' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);'/></li>";
		//html +="<li class='column_TipoCambio'><input  value='"+ArrayDatos.TipoCambio+"' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);'/></li>";
		html +="<li class='column_BolsaPConv' style='background-color: lightblue;'><input  value='0' class='inactive'  readonly/></li>";


		html +="<li class='column_FolioServices'><input /></li>";

		html +="<li class='column_GastoDeu' style='background-color: lightblue;'><input  value='0' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);'/></li>";
		html +="<li class='column_Sob' style='background-color: lightblue;'><input  value='0' onkeypress='return justNumbers2(event);' onkeyup='CalcSummary(event);'/></li>";
		html +="<li class='column_Falt' style='background-color: lightblue;'><input  value='0' onkeypress='return justNumbers2(event);' onkeyup='CalcSummary(event);'/></li>";
		
		//html +="<li class='column_TotalEfectZ' style='background-color: lightblue;'><input value='"+ArrayDatos.TotalEfectZ +"' readonly/></li>";
		//html +="<li class='column_VariacionDayli'><input value='0' readonly/></li>";
		html +="<li class='column_ComentariosDayli'><img src='iconos/msgNew.png' onclick='VerComent(event);'/></li><li class='column_codUser'><input value='"+$("#SelectUsers").val()+"'/></li><li class='column_coment'><input /></li> </ul>";

		$(html).insertAfter('#container_history1 > li:eq(0)');

		$('#container_history1 > li').eq(1).children('ul').children('li .column_Sucursal').find("#select_sucursal2 option[value='"+ArrayDatos.SucursalID+"']").attr('selected',true);

		$('#container_history1 > li').each(function (index) {

				if(index!=0){
					$(this).find('ul > li.column_ID').text((index).toString());

				}


		});
		var _lastIndex=$('#container_history1 > li').length-1;
		var _totIng=parseFloat(quitarLetra($('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_TotalIng').children('input').val()));
  		var _totimpTc=parseFloat(quitarLetra($('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_ImpTcredito').children('input').val()));
  		var _totOFP=parseFloat(quitarLetra($('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_OFPago').children('input').val()));
  		var _totEfZ=parseFloat(quitarLetra($('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_TotalEfectZ').children('input').val()));

		$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_TotalIng').children('input').val("$"+ formatoNumero( _totIng + ArrayDatos.TotalIng,_decimales,_separadorD,_separadorM ));
		$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_ImpTcredito').children('input').val("$"+ formatoNumero( _totimpTc +ArrayDatos.TCredit,_decimales,_separadorD,_separadorM));
		$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_OFPago').children('input').val("$"+ formatoNumero( _totOFP + ArrayDatos.OFPago,_decimales,_separadorD,_separadorM));
		$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_TotalEfectZ').children('input').val("$"+ formatoNumero( _totEfZ + ArrayDatos.TotalEfectZ,_decimales,_separadorD,_separadorM));
		//CalcSummary(event);

  }

  function CalcSummary(event){
  		event.preventDefault();
  		var _parent=$(event.target).parent().parent();
  		var _index_Li=_parent.parent().index();
  		var _id_Row=_parent.attr('id');
  		

  		var _ULRegZ=$('#container_history1 > li').eq(_index_Li).children('ul');

  //	((Datos.EfectivoP+Convertidor(Datos.EfectivoD,Datos.TC,1)+Datos.BolsaP+Convertidor(Datos.BolsaD,Datos.TC,1)) - Datos.TotalEfectivo) - Datos.Sobrantes)+Datos.Gastos
	//console.log(_ULRegZ.children('li .column_SobFalt').children('input').val());
	
		if($(event.target).attr('readonly')!='readonly'){
				//var TC1=quitarLetra(_ULRegZ.children('li .column_TipoCambio').children('input').val());
				//_ULRegZ.children('li .column_TipoCambio').children('input').val(TC1);

				//$('#TipoC').val(TC1);
				var EP=0;if (!isNaN(parseFloat(quitarLetra(_ULRegZ.children('li .column_EfectivoDepP').children('input').val())))){EP=parseFloat(quitarLetra( _ULRegZ.children('li .column_EfectivoDepP').children('input').val())); }
				var ED=0;if (!isNaN(parseFloat(quitarLetra(_ULRegZ.children('li .column_EfectivoDepD').children('input').val())))) {ED=parseFloat(quitarLetra( _ULRegZ.children('li .column_EfectivoDepD').children('input').val()));}
				var BP=0;if (!isNaN(parseFloat(quitarLetra(_ULRegZ.children('li .column_BolsaP').children('input').val())))) {BP=parseFloat(quitarLetra(_ULRegZ.children('li .column_BolsaP').children('input').val()));}
				var BD=0;if (!isNaN(parseFloat(quitarLetra(_ULRegZ.children('li .column_BolsaD').children('input').val())))) {BD=parseFloat(quitarLetra(_ULRegZ.children('li .column_BolsaD').children('input').val()));}
				var TE=0;if (!isNaN(parseFloat(quitarLetra(_ULRegZ.children('li .column_TotalEfectZ').children('input').val())))) {TE=parseFloat(quitarLetra(_ULRegZ.children('li .column_TotalEfectZ').children('input').val()));}
				var S=0;if (!isNaN(parseFloat(quitarLetra(_ULRegZ.children('li .column_Sob').children('input').val())))) {S=parseFloat(quitarLetra(_ULRegZ.children('li .column_Sob').children('input').val()));}
				var F=0;if (!isNaN(parseFloat(quitarLetra(_ULRegZ.children('li .column_Falt').children('input').val())))) {F=parseFloat(quitarLetra(_ULRegZ.children('li .column_Falt').children('input').val()));}
				var G=0;if (!isNaN(parseFloat(quitarLetra(_ULRegZ.children('li .column_GastoDeu').children('input').val())))) {G=parseFloat( quitarLetra(_ULRegZ.children('li .column_GastoDeu').children('input').val()));}
				var TC=0;if (!isNaN(parseFloat(quitarLetra(_ULRegZ.children('li .column_TipoCambio').children('input').val())))) {TC=parseFloat(quitarLetra(_ULRegZ.children('li .column_TipoCambio').children('input').val()));}

				var _datos={
				'EfectivoP':EP,
				'EfectivoD':ED,
				'BolsaP':BP,
				'BolsaD':BD,
				'TotalEfectivo':TE,
				'Sobrantes':S,
				'Faltantes':F,
				'Gastos':G,
				'TC':TC
				}


				Variacion(event,_datos,10);
			  	CalcSumatorias(event,[5,6,7,9,12,13,14,15,17],-1);
	  }
  }

 /*
  function CalcTipoCambio (event) {
  	// body...
  	event.preventDefault();
  	var index1=$(event.target).parent().parent().parent().index();

	var id=$(event.target).parent().parent().parent().attr('id');

	var EfePesos=0;
	var EfecDolares=0;
	var TCambio=0;
	var TotEfecZ=0;
	var Sobrante=0;

	var ConverPesos=0;


	EfePesos =parseFloat(quitarLetra($('#container_history1 > li').eq(index1).children('ul').children('li.column_EfectivoDepP').children('input').val()));
	EfecDolares =parseFloat(quitarLetra($('#container_history1 > li').eq(index1).children('ul').children('li.column_EfectivoDepD').children('input').val()));
	TCambio =parseFloat($('#TipoC').val());
	TotEfecZ =parseFloat(quitarLetra( $('#container_history1 > li').eq(index1).children('ul').children('li.column_TotalEfectZ').children('input').val()));
	Sobrante =parseFloat(quitarLetra($('#container_history1 > li').eq(index1).children('ul').children('li.column_SobFalt').children('input').val()));

	if(isNaN(EfePesos)){EfePesos=0;}
	if(isNaN(EfecDolares)){EfecDolares=0;}
	if(isNaN(Sobrante)){Sobrante=0;}
	
	if(EfecDolares>0){
		ConverPesos=EfecDolares*TCambio;
	}
	//"$"+ formatoNumero(
	var res=((ConverPesos+EfePesos)+TotEfecZ)-Sobrante;

	$('#container_history1 > li').eq(index1).children('ul').children('li.column_VariacionDayli').children('input').val("$"+ formatoNumero( res,_decimales,_separadorD,_separadorM));

  }
*/
  var indextemp;

  function VerComent(event){
  	event.preventDefault();
  	var index1=$(event.target).parent().parent().parent().index();

	var id=$(event.target).parent().parent().parent().attr('id');
	indextemp=null;
	indextemp=index1;

	$("#WindowComent").hide();
	$("#textcoment").val("");
	if($('#container_history1 > li').eq(index1).hasClass('row')){
		$('#container_history1 > li').eq(index1).removeClass('row');
		$('#container_history1 > li').eq(index1).addClass('rowM');
	}
	
	$("#textcoment").val($('#container_history1 > li').eq(index1).children('ul').children('li .column_coment').children('input').val());

  	$("#WindowComent").css({'top': $(event.target).position().top});
  	$("#WindowComent").css({'left': $(event.target).position().left});
  	$("#WindowComent").toggle("slow");
  }

  function Close(event){
  //	$(event.target).toggle("fast");
  $("#WindowComent").toggle("fast");
  indextemp=null;
  }
  function Comentar(event){
  	event.preventDefault();
  	console.log(indextemp);
  	console.log($("#textcoment").val());
  	$('#container_history1 > li').eq(indextemp).children('ul').children('li .column_coment').children('input').val("");
 	$('#container_history1 > li').eq(indextemp).children('ul').children('li .column_coment').children('input').val($("#textcoment").val());
 	$("#WindowComent").toggle("fast");
  }

  function quitarLetra(monto){
  
  	if(monto=="undefined"){return "";}
	if(monto==""){return "";}
	monto=monto.replace(/[A-Za-z$-]/g,'');
	monto=monto.replace(',','');
	return monto;
}

function formatoNumero(numero, decimales, separadorDecimal, separadorMiles) {
    var partes, array;
    if ( !isFinite(numero) || isNaN(numero = parseFloat(numero)) ) {
        return "";
    }
    if (typeof separadorDecimal==="undefined") {
        separadorDecimal = ",";
    }
    if (typeof separadorMiles==="undefined") {
        separadorMiles = "";
    }

    // Redondeamos
    if ( !isNaN(parseInt(decimales)) ) {
        if (decimales >= 0) {
            numero = numero.toFixed(decimales);
        } else {
            numero = (
                Math.round(numero / Math.pow(10, Math.abs(decimales))) * Math.pow(10, Math.abs(decimales))
            ).toFixed();
        }
    } else {
        numero = numero.toString();
    }

    // Damos formato
    partes = numero.split(".", 2);
    array = partes[0].split("");
    for (var i=array.length-3; i>0 && array[i-1]!=="-"; i-=3) {
        array.splice(i, 0, separadorMiles);
    }
    numero = array.join("");

    if (partes.length>1) {
        numero += separadorDecimal + partes[1];
    }

    return numero;
}