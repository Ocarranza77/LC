var sucursales=new Array();
var _decimales=2;
var _separadorD=".";
var _separadorM=",";
//var html="";


//$(function () { $("#txtFechaVenta").datepicker();});

/*
$(document).keypress(function(e) {
	if(e.keyCode === 13) { 

		e.preventDefault();
		console.log($(e.target).attr('id'));
		//if($(e.target).attr('id')!="txtFecha"){
		//	return false;
		//}
	 	
	  }

 });*/

//function mostCalendar(){ $('#txtFechaVenta').datepicker("show");}

function EnFecha (event) {
	// body...
	console.log(event.keyCode);

	if(event.keyCode==13){
		$("#Reporte").focus();
		$("#Reporte").click();
	}
	
}
function ChangeFecha (event) {
	// body...
	
	//$("#btnChangeFecha").click();
	//$("#txtFecha").focus();
}

function FillSucursales(){
	var html="";
	$.ajax({
	    type: "POST",
	    url: "CapZDaylis.aspx/GetSucursales",
	    data:"{}",
	    contentType: "application/json; charset=utf-8",
	    dataType: "json", 
	    async:false, 
	    success: function (response) {
	    	html=response.d;

	    }
	});
	return html;

}

function clickTab(event){
	var id=$(event.target).val();
	var element=event.target;
	$(event.target).parent().parent().children('li').children('input').removeClass('_tabactive');
	//$(event.target).removeClass('_tabInactive');
	element.className='_tabactive';
	$(event.target).addClass('_tabactive');


	$('.content_tab > div').css({'display':'none'});
	
	switch($(event.target).val()){
		case "Registro de Z's":
			$('#content_RegZ').css({'display':'block'});
		break;
		case "Daily Cash Summary":
			$('#content_Daily').css({'display':'block'});
		break;
		
	}	
}
function MostCalendar(){
	$("#btnFecha").click();
}

function SaveIndormacion(){
	var _hizoCambio=false;
	var msg=false;
	var msgText="";
	var msgCont=0;

	var ID_Array=new Array();
	var _indexSucursales=0;
	var _suID="";
	
	$('#content_RegZ_ul > li.rowM').each(function (index) {

		//console.log($('#container_history > li').eq($(this).index()).children('ul').children('li .column_sttReg').children('img').attr('src'));

		if($(this).find('ul > li.column_sttReg').children('img').attr('src')=='../../iconos/sign_down.png' ){_hizoCambio=true;}
		/*
		var _SiD=$(this).find('li.column_Sucursal').children('select').val();
		var _f=$(this).find('li.column_FechaT').children('input').val();
		if(_suID != _SiD){

			ID_Array[_indexSucursales]=_SiD;
			ID_Array[_indexSucursales]+="|"+_f;
			_indexSucursales++;

		}
		*/

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

	var countIndex=$('#content_RegZ_ul > li.rowM').length;
	console.log(countIndex);
	if(countIndex > 0){
		//eliminar nuevos que no estan grabados
		$('#content_RegZ_ul > li.row').each(function (index) {
			//console.log($(this).find('ul > li.row_ID').children('input').val());
			if($(this).find('ul > li.row_ID').children('input').val()=="ST2"){
				$(this).remove();
			}
		});

	$('#content_RegZ_ul > li.rowM').each(function (index) {
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
			ObjArray[9]=$(this).find('ul > li.column_ImpEfectivo').children('input').val();/*efectivo z*/
			ObjArray[10]=$(this).find('ul > li.column_ImpTcredito').children('input').val();/*hTcredit z*/
			ObjArray[11]=$(this).find('ul > li.column_OFPago').children('input').val();/*ofPagoz*/
			ObjArray[12]=$(this).find('ul > li.column_Importe').children('input').val();/*Importe z*/
			//console.log($(this).find('ul > li.column_Importe').children('input').val()+"x");
			ObjArray[13]=$(this).find('ul > li.column_codUser').children('input').val();/*Importe z*/

			/*recororero corte sucursal*/

			$('#content_Daily_ul > li').each(function (index) {
			var indexList=$(this).index();
				//ObjArraySuc=new Array();
				var _SucId=$(this).find('ul > li.column_Sucursal').children('select').val();
				var _FDaily=$(this).find('ul > li.column_FechaDaily').children('input').val(); 

				if(ObjArray[2]+""+ObjArray[3] == _SucId+""+_FDaily){

						ObjArray[14]=$(this).find('ul > li.column_EfectivoDepP').children('input').val();
						ObjArray[15]=$(this).find('ul > li.column_EfectivoDepD').children('input').val();
						//ObjArray[8]=$(this).find('ul > li.column_FolioServices').children('input').val();
						ObjArray[16]=$(this).find('ul > li.column_BolsaP').children('input').val();
						ObjArray[17]=$(this).find('ul > li.column_BolsaD').children('input').val();
						ObjArray[18]=$(this).find('ul > li.column_FolioServices').children('input').val();

						ObjArray[19]=$(this).find('ul > li.column_GastoDeu').children('input').val();
						ObjArray[20]=$(this).find('ul > li.column_Sob').children('input').val();
					
						ObjArray[21]=$(this).find('ul > li.column_TipoCambio').children('input').val();//$("#TipoC").val();
						ObjArray[22]=$(this).find('ul > li.column_coment').children('input').val();
						ObjArray[23]=$(this).find('ul > li.column_Falt').children('input').val();


						ObjArray[24]=$(this).find('ul > li.column_Check').children('input').val();  // .attr('class');
						ObjArray[25]=$(this).find('ul > li.column_Supervisor').children('input').val();
						ObjArray[26]=$(this).find('ul > li.column_CajeroCorto').children('input').val();




				}


			});

			ObjArray[27]=$(this).find('ul > li.column_ImpTdebito').children('input').val();


			/**********************/
			
			$.ajax({
			    type: "POST",
			    url: "CapZDaylis.aspx/SaveZ",
			    data: JSON.stringify({DatosZ:ObjArray}),
			    contentType: "application/json; charset=utf-8",
			    dataType: "json",
			    async:false, 
			    success: function (response) {
			    	var _res=response.d;
			    	var result=_res.split('|');

			    	console.log(result);

			    	if(parseInt(result[0])==0){

						$('#content_RegZ_ul > li').eq(indexList).removeClass('rowM');
						$('#content_RegZ_ul > li').eq(indexList).addClass('row');

						if($('#content_RegZ_ul > li').eq(indexList).children('ul .row_ID').children('li').children('input').val()!="ST2" ){
							//row_ID
							//$(this).attr('src','../../iconos/checkbox_unchecked (1).png');
							$('#content_RegZ_ul > li').eq(indexList).children('ul').children('li').children('input').addClass('inactive');
							$('#content_RegZ_ul > li').eq(indexList).children('ul').children('li').children('select').addClass('inactive');

							$('#content_RegZ_ul > li').eq(indexList).children('ul').children('li').children('input').attr('readonly',true);
							$('#content_RegZ_ul > li').eq(indexList).children('ul').children('li').children('select').attr('disabled',true);


							$('#content_RegZ_ul > li').eq(indexList).children('ul').children('li .column_sttReg').children('img').attr('src','../../iconos/ok-24.png');
							$('#content_RegZ_ul > li').eq(indexList).children('ul').children('li .column_sttReg').children('img').attr('title',result[1]);

							$('#content_RegZ_ul > li').eq(indexList).children('ul').children('li .column_Edit').children('img').attr('src','../../iconos/checkbox_unchecked (1).png');

							//$(event.target).attr('src','../../iconos/checkbox_unchecked (1).png');

							$('#content_RegZ_ul > li').eq(indexList).children('ul').children('li .row_ID').children('input').val("ST");
							//_ULRegZ.parent().removeClass('rowM');
							//_ULRegZ.parent().addClass('row'); 
							//}

							
					    	//$('.loader').toggle();

						}else{

							$('#content_RegZ_ul > li').eq(indexList).remove();
						}
						
			    	}else{
			    		//alert(response.d);
			    		$('#content_RegZ_ul > li').eq(indexList).children('ul').children('li .column_sttReg').children('img').attr('src','../../iconos/exclamation.png');
						$('#content_RegZ_ul > li').eq(indexList).children('ul').children('li .column_sttReg').children('img').attr('title',result[1]);
						msgText += "\n"+result[1];
						msgCont +=parseInt(result[0]);
			    	}
			    },
			    error:function(response){
			    	
			    	$('.loader').toggle();
			    }
			});
			

			

			//}
	});
	
	if(msgCont > 0){
		var text="Se detectaron los siguientes errores : ";
		if(msgCont==1){text="Se detecto el siguiente Error : ";}
		alert(text+"\n"+msgText);
	}else{
		alert("Operacion finalizada con exito");
	}
		
		$('.loader').toggle();

	
	}else{


	/*   grabar summary de sucursales*/
	if($('#content_Daily_ul > li.rowM').length > 0){

	$('#content_Daily_ul > li.rowM').each(function (index) {
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


			ObjArray[4]=$(this).find('ul > li.column_EfectivoDepP').children('input').val();
			ObjArray[5]=$(this).find('ul > li.column_EfectivoDepD').children('input').val();
			//ObjArray[8]=$(this).find('ul > li.column_FolioServices').children('input').val();
			ObjArray[6]=$(this).find('ul > li.column_BolsaP').children('input').val();
			ObjArray[7]=$(this).find('ul > li.column_BolsaD').children('input').val();
			ObjArray[8]=$(this).find('ul > li.column_FolioServices').children('input').val();

			ObjArray[9]=$(this).find('ul > li.column_GastoDeu').children('input').val();
			ObjArray[10]=$(this).find('ul > li.column_Sob').children('input').val();
			//ObjArray[15]=$(this).find('ul > li.column_TotalEfectZ').children('input').val();
			//ObjArray[16]=$(this).find('ul > li.column_VariacionDayli').children('input').val();
			ObjArray[11]=$(this).find('ul > li.column_codUser').children('input').val();
		
			ObjArray[12]=$(this).find('ul > li.column_TipoCambio').children('input').val()//$("#TipoC").val();//column_TipoCambio
			ObjArray[13]=$(this).find('ul > li.column_coment').children('input').val();
			ObjArray[14]=$(this).find('ul > li.column_Falt').children('input').val();

			ObjArray[15]=$(this).find('ul > li.column_Check').children('input').val(), //.children('img').attr('class');
			ObjArray[16]=$(this).find('ul > li.column_Supervisor').children('input').val();
			ObjArray[17]=$(this).find('ul > li.column_CajeroCorto').children('input').val();

			$.ajax({
			    type: "POST",
			    url: "CapZDaylis.aspx/SaveSummary",
			    data: JSON.stringify({Summary:ObjArray}),
			    contentType: "application/json; charset=utf-8",
			    dataType: "json",
			    async:false, 
			    success: function (response) {
			    	var _res=response.d;
			    	var result=_res.split('|');



			    	if(parseInt(result[0]) ==0){
				    	$('#content_Daily_ul > li').eq(indexList).removeClass('rowM');
						$('#content_Daily_ul > li').eq(indexList).addClass('row');

						$('#content_Daily_ul > li').eq(indexList).children('ul').children('li').children('input').addClass('inactive');
						$('#content_Daily_ul > li').eq(indexList).children('ul').children('li').children('select').addClass('inactive');

						$('#content_Daily_ul > li').eq(indexList).children('ul').children('li').children('input').attr('readonly',true);
						$('#content_Daily_ul > li').eq(indexList).children('ul').children('li').children('select').attr('disabled',true);

						$('#content_Daily_ul > li').eq(indexList).children('ul').children('li .column_Edit').children('img').attr('src','../../iconos/checkbox_unchecked (1).png');
						$('#content_Daily_ul > li').eq(indexList).children('ul').removeClass('active');
					}else{
					//alert(response.d);
					msgCont +=result[0];
					msgText +="\n"+result[1];
					}

			    }

			   });

		});

	if(msgCont > 0){
		var text="Se detectaron los siguientes errores : ";
		if(msgCont==1){text="Se detecto el siguiente Error : ";}
		alert(text+"\n"+msgText);
	}else{
		alert("Operacion finalizada con exito");
	}
		$('.loader').toggle();

			}
		}

	
	}else{

		return false;
	}

}




//console.log(FillSucursales());

function ADDRow(event){
	//event.preventDefault();

	//var fechaE=$('#txtFecha').val().toString().split('/');

	var html="<li class='rowM'><ul >";
		html +="<li class='column_stt'><input value='ST0' /></li>"
		html +="<li class='column_CodeRegZ'><input /></li>";//column_CodeReg //column_CodeRegZ
		html +="<li class='row_ID' ><input value='ST0'/></li>";
		html +="<li class='column_ID'>1</li>";
		html +="<li class='column_Edit column_fillColor'><img  src='iconos/checkbox_unchecked (1).png' title='Editar' onclick='Editar(event);'/></li>";
		html +="<li class='column_Del column_fillColor'><img  src='iconos/checkbox_unchecked (1).png' title='Eliminar registro' onclick='Eliminar(event);'/></li>";
		html +="<li class='column_Sucursal'>";
		html +="<select class='active' id='select_sucursal'   autofocus>";

		html += FillSucursales();
		/*for (var i =0;i<sucursales.length;i++) {
			
			html+="<option value='"+sucursales[i].SucursalID +"'>"+("000"+sucursales[i].SucursalID).slice(-3)+"-"+sucursales[i].Nombre +"</option>";
		
		}*/

		html +="</select></li>";
		html +="<li class='column_FechaT' ><input class='active' value='"+$("#txtFecha").val()+"' onkeyup='this.value=formateafecha(this.value);' maxlength='10' placeholder='dd/mm/aaaa' /></li>";
		html +="<li class='column_HoraT'><input class='active'  placeholder='HH:MM' onkeyup='valida(event);'  onkeypress='return justNumbers(event);' maxlength='5' /></li>";
		html +="<li class='column_FolioT'><input class='active'   onkeypress='return justNumbers(event);' /></li>";
		html +="<li class='column_Caja'><input class='active' onkeypress='return justNumbers(event);'/></li>";
		html +="<li class='column_NT'><input class='active' onkeypress='return justNumbers(event);' /></li>";
		html +="<li class='column_Cajero'><input class='active'  /></li>";
		html +="<li class='column_ImpEfectivo'><input class='active' onkeypress='return justNumbers(event);' onkeyup='verificar(event);' value='0' /></li>";
		html +="<li class='column_ImpTcredito'><input class='active' onkeypress='return justNumbers(event);' onkeyup='verificar(event);' value='0' /></li>";
		html +="<li class='column_ImpTdebito'><input class='active' onkeypress='return justNumbers(event);' onkeyup='verificar(event);' value='0' /></li>";
		html +="<li class='column_OFPago'><input class='active' onkeypress='return justNumbers(event);' onkeyup='verificar(event);' value='0' /></li>";
		html +="<li class='column_Importe'><input  value='0' class='inactive'  readonly/></li><li class='column_sttReg'><img onclick='Add_Z(event);' src='../../iconos/sign_down.png' /></li> <li class='column_codUser'><input value='"+$("#SelectUsers").val()+"'/></li></ul></li> ";

		$(html).insertAfter('#content_RegZ_ul > li:eq(0)');

		$('#content_RegZ_ul > li').each(function (index) {
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
				if(parseInt(hora.substring(3,hora.length))> 59){
					e.target.value=hora.substring(0,3)+"59";
				}
			}
		
		
	}


}

function ChangeSTT(event) {
	// body...
	var _ul=$(event.target).parent().parent();
	var _stt=$(event.target).attr('class');
	var _index=$(event.target).parent().parent().parent().index();

	var _variacion=_ul.children('li .column_VariacionDayli').val();

	if(_stt!="STT" && _stt!="STTN" && _stt!="STTR")
		return false;

	$(".PanelEstatus").toggle("fast");
	$("#txtSttID").val(_index);
	console.log(_index);

}
function SelectSTT (event) {
	// body...
	var _index=$("#txtSttID").val();
	var _claseAnt=$("#content_Daily_ul > li").eq(_index).children('ul').children('li .column_Check').children('img').attr('class');
	var _claseNew=$(event.target).parent().children('img').attr('class');
	console.log($(event.target).parent().children('img').attr('class'));
	console.log(_claseAnt);
	console.log(_claseNew);

	$("#content_Daily_ul > li").eq(_index).children('ul').children('li .column_Check').children('img').removeClass(_claseAnt);	
	$("#content_Daily_ul > li").eq(_index).children('ul').children('li .column_Check').children('img').addClass(_claseNew);	

	$(".PanelEstatus").toggle("fast");
	$("#txtSttID").val(0);

}

function ValidaSTTDaily(event){
	var result=false;
	var msg="No se puede alterar un Daily Terminado o Validado";

	var _UL=$(event.target).parent().parent();
	var _stt=_UL.find('li.column_Check').children('input').val();

	console.log(_stt);
	if(_stt=="R" ||_stt=="N" || _stt==""){
		msg="";
		result= true;
	}


	if(msg!=""){
		alert(msg);
	}

	return result;
}

function ValidateSTT(event) {
	var result=false;
	event.preventDefault();
	var _UL=$(event.target).parent().parent().parent().parent();
	var _index_li=$(event.target).parent().parent().parent().index();

	var _sucursalID=_UL.children('li').eq(_index_li).children('ul').children('li .column_Sucursal').children('select').val();
	var _fecha=_UL.children('li').eq(_index_li).children('ul').children('li .column_FechaT').children('input').val();

	$("#content_Daily_ul > li").each(function  (index) {
		// body...
		if(index > 0){

			var _SID=$(this).find('ul > li.column_Sucursal').children('select').val();
			var _FeD=$(this).find('ul > li.column_FechaDaily').children('input').val();

			result=true;

			if(_SID+""+_FeD==_sucursalID+""+_fecha){

				var clase =$(this).find('ul > li.column_Check').children('input').val(); //.children('img').attr('class');
				console.log(clase);
				if(clase=="N" || clase=="R" || clase==""){
					result=true;

				}else{
					result=false;
				}

				return false;
			}
		}
		
	});

	//alert(_sucursalID);

	return result;
}

function EditarDaily(event){
	event.preventDefault();
	var _parent=$(event.target).parent().parent();
	var _index_Li=_parent.parent().index();
	var _id_Row=_parent.attr('id');

//alert();
	var _ULRegZ=$('#content_Daily_ul > li').eq(_index_Li).children('ul');

	console.log(_index_Li);

	if(_index_Li==0)
		return false;

	if(!ValidaSTTDaily(event))
		return false;

	if($(event.target).attr('src')!='../../iconos/checkbox_checked.png'){
		$(event.target).attr('src','');
		$(event.target).attr('src','../../iconos/checkbox_checked.png');

		_ULRegZ.addClass('active');
		if(_ULRegZ.children('ul').children('li .row_ID').children('input').val() != "ST0"){
			_ULRegZ.parent().removeClass('row');
			_ULRegZ.parent().addClass('rowM'); 
			
		}
		_ULRegZ.find('li.column_Supervisor').children().removeClass('inactive');
		_ULRegZ.find('li.column_Supervisor').children().attr('readonly',false);

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

		_ULRegZ.find('li.column_CajeroCorto').children().removeClass('inactive');
		_ULRegZ.find('li.column_CajeroCorto').children().attr('readonly',false);

		_ULRegZ.find('li.column_Falt').children().removeClass('inactive');
		_ULRegZ.find('li.column_Falt').children().attr('readonly',false);

		//_ULRegZ.find('li input').attr('readonly',false);
	//	_ULRegZ.find('li select').attr('disabled',false);


	}else{
		
		$(event.target).attr('src','../../iconos/checkbox_unchecked (1).png');

			_ULRegZ.removeClass('active');
			if(_ULRegZ.children('ul').children('li .row_ID').children('input').val() != "ST0"){
				_ULRegZ.parent().removeClass('rowM');
				_ULRegZ.parent().addClass('row');

			}

			_ULRegZ.find('li.column_Supervisor').children().addClass('inactive');
		_ULRegZ.find('li.column_Supervisor').children().attr('readonly',true);

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

		_ULRegZ.find('li.column_CajeroCorto').children().addClass('inactive');
		_ULRegZ.find('li.column_CajeroCorto').children().attr('readonly',true);

		_ULRegZ.find('li.column_Falt').children().addClass('inactive');
		_ULRegZ.find('li.column_Falt').children().attr('readonly',true);


	}

}
var _indexDailyEdit=0;
function Editar(event){
	event.preventDefault();
	var _parent=$(event.target).parent().parent();
	var _index_Li=_parent.parent().index();
	var _id_Row=_parent.attr('id');




//alert();
	var _ULRegZ=$('#content_RegZ_ul > li').eq(_index_Li).children('ul');

	if(_index_Li==0)
		return false;


	if(!ValidateSTT(event)){
		alert("Intenta Editar un registro Z que ya fue validado.");
		return false;
	}


	var _SucId=_ULRegZ.children('li .column_Sucursal').children('select').val();
	var _FechaZ=_ULRegZ.children('li .column_FechaT').children('input').val();
	var _folioZ=_ULRegZ.children('li .column_FolioT').children('input').val();
	var _CodCaja=_ULRegZ.children('li .column_Caja').children('input').val();



	var _TCredito=_ULRegZ.children('li .column_ImpTcredito').children('input').val(); _TCredito=quitarLetra(_TCredito);_TCredito=parseFloat(_TCredito);
	var _TDebito=_ULRegZ.children('li .column_ImpTdebito').children('input').val(); _TDebito=quitarLetra(_TDebito);_TDebito=parseFloat(_TDebito);
	var _OFPago=_ULRegZ.children('li .column_OFPago').children('input').val(); _OFPago=quitarLetra(_OFPago); _OFPago=parseFloat(_OFPago);

	var _Importe=_ULRegZ.children('li .column_Importe').children('input').val(); _Importe=quitarLetra(_Importe); _Importe=parseFloat(_Importe);  //parseFloat(quitarLetra(_ULRegZ.children('li .column_Importe').children('input').val()));
	var _ImpEfectivo=_ULRegZ.children('li .column_ImpEfectivo').children('input').val(); _ImpEfectivo=quitarLetra(_ImpEfectivo); _ImpEfectivo=parseFloat(_ImpEfectivo);  //parseFloat(quitarLetra(_ULRegZ.children('li .column_ImpEfectivo').children('input').val()));

	var _codeAnt=_folioZ+"|"+_SucId+"|"+_CodCaja+"|"+_FechaZ +"|"+_Importe;
	//$('#container_history > li').eq(index1).children('ul').children('li .column_CodeRegZ').children('input').val(
	var _SttAnt=_ULRegZ.children('ul').children('li .column_stt').children('input').val(); 	

	_ULRegZ.children('li .column_CodeRegZ').children('input').val(_codeAnt);	

	if($(event.target).attr('src')!='../../iconos/checkbox_checked.png'){
	
		if(confirm('Esta consciente que un cambio en el registro de Zs ,hara necesario revizar los datos del Daily Cash Summary.\n¿Esta seguro de continuar?')){

			$(event.target).attr('src','');
			$(event.target).attr('src','../../iconos/checkbox_checked.png');

			_ULRegZ.children('li .column_sttReg').children('img').attr('src','../../iconos/sign_down.png');

			/*habilitar los texbox para editar*/
			_ULRegZ.find('li').children().removeClass('inactive');
			_ULRegZ.find('li input').attr('readonly',false);
			_ULRegZ.find('li select').attr('disabled',false);

			_ULRegZ.children('li .column_Importe').children('input').attr('readonly',true);
			_ULRegZ.children('li .column_Importe').children('input').addClass('inactive');

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
			$('#content_Daily_ul > li').each(function (index) {

					if(index > 0){
						var SucId=$(this).find('ul > li.column_Sucursal').children('select').val();
						var FDaily=$(this).find('ul > li.column_FechaDaily').children('input').val();

						if(_SucId+""+_FechaZ == SucId+""+FDaily){
							
							var _TotIng =$(this).find('li.column_TotalIng').children('input').val(); _TotIng=quitarLetra(_TotIng); _TotIng=parseFloat(_TotIng); //parseFloat(quitarLetra($(this).find('li.column_TotalIng').children('input').val()));
							var _NoZ = $(this).find('li.column_NumZ').children('input').val(); _NoZ=parseFloat(_NoZ); //parseInt($(this).find('li.column_NumZ').children('input').val());
							var _TotEfectZ =$(this).find('li.column_TotalEfectZ').children('input').val(); _TotEfectZ=quitarLetra(_TotEfectZ); _TotEfectZ=parseFloat(_TotEfectZ); //parseFloat(quitarLetra($(this).find('li.column_TotalEfectZ').children('input').val()));

							var _TC=$(this).find('li.column_ImpTcredito').children('input').val(); _TC=quitarLetra(_TC); _TC=parseFloat(_TC);
							var _TD=$(this).find('li.column_ImpTdebito').children('input').val(); _TD=quitarLetra(_TD); _TD=parseFloat(_TD);
							var _OFP=$(this).find('li.column_OFPago').children('input').val(); _OFP=quitarLetra(_OFP); _OFP=parseFloat(_OFP);

							var SubTIng=_TotIng > 0 ? _TotIng - _Importe:0;
							var SubNoz=_NoZ > 0 ?_NoZ -1:0;
							var SubTEfZ=_TotEfectZ > 0 ? _TotEfectZ - _ImpEfectivo:0;
							var SubTC=_TC > 0 ? _TC - _TCredito:0;
							var SubTD=_TD > 0 ?_TD -_TDebito:0;
							var SubOFP=_OFP > 0 ? _OFP-_OFPago:0;

							$(this).find('li.column_TotalIng').children('input').val("$"+ formatoNumero(SubTIng,_decimales,_separadorD,_separadorM));
							$(this).find('li.column_NumZ').children('input').val(SubNoz);
							$(this).find('li.column_TotalEfectZ').children('input').val("$"+ formatoNumero( SubTEfZ,_decimales,_separadorD,_separadorM));

							$(this).find('li.column_ImpTcredito').children('input').val("$"+ formatoNumero( SubTC ,_decimales,_separadorD,_separadorM));
							$(this).find('li.column_ImpTdebito').children('input').val("$"+ formatoNumero(SubTD ,_decimales,_separadorD,_separadorM));
							$(this).find('li.column_OFPago').children('input').val("$"+ formatoNumero(SubOFP ,_decimales,_separadorD,_separadorM));

							$(this).removeClass('row')
							$(this).addClass('rowM');
							
							if(SubNoz == 0 ){
								_indexDailyEdit=index;
								$(this).children('ul').addClass('removeRow');
								$(this).children('ul > li.row_ID').children('input').val('ST2');
								//$(this).hide();

							}else{
								$(this).children('ul').removeClass('removeRow');
								$(this).children('ul > li.row_ID').children('input').val('ST1');
							}

							return false;
						}	
					}
				});
				
			}else{
				_indexDailyEdit=0;
				return false;
			}


	}else{
			$(event.target).attr('src','../../iconos/checkbox_unchecked (1).png');

			_ULRegZ.children('li .column_Del').children('img').removeAttr('disabled');
			_ULRegZ.children('li .column_sttReg').children('img').attr('src','');
			//_ULRegZ.children('ul').children('li .row_ID').children('input').val(_SttAnt);

			_ULRegZ.find('li').children().addClass('inactive')
			_ULRegZ.find('li input').attr('readonly',true);
			_ULRegZ.find('li select').attr('disabled',true);


			//_ULRegZ.children('ul').removeClass('removeRow');
			if(_indexDailyEdit > 0){

				$('#content_Daily_ul > li').eq(_indexDailyEdit).children('ul').removeClass('removeRow');

				var _TotIng=$('#content_Daily_ul > li').eq(_indexDailyEdit).children('ul').children('li .column_TotalIng').children('input').val();    _TotIng=quitarLetra(_TotIng); _TotIng=parseFloat(_TotIng);
				var _NoZ=$('#content_Daily_ul > li').eq(_indexDailyEdit).children('ul').children('li .column_NumZ').children('input').val();     _NoZ=parseFloat(_NoZ);
				var _TotEfectZ=$('#content_Daily_ul > li').eq(_indexDailyEdit).children('ul').children('li .column_TotalEfectZ').children('input').val(); _TotEfectZ=quitarLetra(_TotEfectZ); _TotEfectZ=parseFloat(_TotEfectZ);
				var _TC=$('#content_Daily_ul > li').eq(_indexDailyEdit).children('ul').children('li .column_ImpTcredito').children('input').val(); _TC=quitarLetra(_TC); _TC=parseFloat(_TC);
				var _TD=$('#content_Daily_ul > li').eq(_indexDailyEdit).children('ul').children('li .column_ImpTdebito').children('input').val();  _TD=quitarLetra(_TD); _TD=parseFloat(_TD);
				var _OFP=$('#content_Daily_ul > li').eq(_indexDailyEdit).children('ul').children('li .column_OFPago').children('input').val();      _OFP=quitarLetra(_OFP); _OFP=parseFloat(_OFP);



				$('#content_Daily_ul > li').eq(_indexDailyEdit).children('ul').children('li .column_TotalIng').children('input').val("$"+ formatoNumero(_TotIng+_Importe,_decimales,_separadorD,_separadorM));    
				$('#content_Daily_ul > li').eq(_indexDailyEdit).children('ul').children('li .column_NumZ').children('input').val(_NoZ+1);       
				$('#content_Daily_ul > li').eq(_indexDailyEdit).children('ul').children('li .column_TotalEfectZ').children('input').val( "$"+ formatoNumero(_TotEfectZ+_ImpEfectivo,_decimales,_separadorD,_separadorM)); 
				$('#content_Daily_ul > li').eq(_indexDailyEdit).children('ul').children('li .column_ImpTcredito').children('input').val("$"+ formatoNumero(_TC +_TCredito,_decimales,_separadorD,_separadorM)); 
				$('#content_Daily_ul > li').eq(_indexDailyEdit).children('ul').children('li .column_ImpTdebito').children('input').val( "$"+ formatoNumero(_TD+_TDebito,_decimales,_separadorD,_separadorM) );  
				$('#content_Daily_ul > li').eq(_indexDailyEdit).children('ul').children('li .column_OFPago').children('input').val("$"+ formatoNumero(_OFP+_OFPago,_decimales,_separadorD,_separadorM) );   



			}
			
			if(_ULRegZ.children('li .row_ID').children('input').val() != "ST0"){
				_ULRegZ.children('li .row_ID').children('input').val(_SttAnt);
				_ULRegZ.parent().removeClass('rowM');
				_ULRegZ.parent().addClass('row');
				
			}else{
			//	_ULRegZ.parent().removeClass('row');
			//	_ULRegZ.parent().addClass('rowM');

			}





			_indexDailyEdit=0;
			
	}



}

function Eliminar(event){
	event.preventDefault();
	var _parent=$(event.target).parent().parent();
	var _index_Li=_parent.parent().index();
	var _id_Row=_parent.attr('id');

	var _lastIndex=$('#content_Daily_ul > li').length-1;
	var _ULRegZ=$('#content_RegZ_ul > li').eq(_index_Li).children('ul');

	if(_index_Li==0)
		return false;

	if(!ValidateSTT(event)){
		alert("Intenta Eliminar un registro Z que ya fue validado.");
		return false;
	}

		var _SttAnt=_ULRegZ.children('li .column_stt').children('input').val(); //$('#container_history > li').eq(index1).children('ul').children('li .column_stt').children('input').val()

		var _SucId=_ULRegZ.children('li .column_Sucursal').children('select').val();
  		var _FechaZ=_ULRegZ.children('li .column_FechaT').children('input').val();
  		
  		var _Importe=_ULRegZ.children('li .column_Importe').children('input').val();           _Importe=quitarLetra(_Importe);           _Importe=parseFloat(_Importe); //parseFloat(quitarLetra(_ULRegZ.children('li .column_Importe').children('input').val()));
  		var _ImpEfectivo=_ULRegZ.children('li .column_ImpEfectivo').children('input').val();   _ImpEfectivo =quitarLetra(_ImpEfectivo);  _ImpEfectivo=parseFloat(_ImpEfectivo); //parseFloat(quitarLetra(_ULRegZ.children('li .column_ImpEfectivo').children('input').val()));
  		var _ImpTC=_ULRegZ.children('li .column_ImpTcredito').children('input').val();         _ImpTC=quitarLetra(_ImpTC);               _ImpTC=parseFloat(_ImpTC); //parseFloat(quitarLetra(_ULRegZ.children('li .column_ImpTcredito').children('input').val()));
  		var _ImpTD=_ULRegZ.children('li .column_ImpTdebito').children('input').val();          _ImpTD=quitarLetra(_ImpTD);               _ImpTD=parseFloat(_ImpTD);
  		var _ImpOFP=_ULRegZ.children('li .column_OFPago').children('input').val();             _ImpOFP=quitarLetra(_ImpOFP);             _ImpOFP=parseFloat(_ImpOFP); //parseFloat(quitarLetra(_ULRegZ.children('li .column_OFPago').children('input').val()));


  		var _tot_Ing=$('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_TotalIng').children('input').val();        _tot_Ing=quitarLetra(_tot_Ing);    _tot_Ing=parseFloat(_tot_Ing); //parseFloat(quitarLetra($('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_TotalIng').children('input').val()));
  		var _tot_TC=$('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_ImpTcredito').children('input').val();      _tot_TC=quitarLetra(_tot_TC);      _tot_TC=parseFloat(_tot_TC); //parseFloat(quitarLetra($('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_ImpTcredito').children('input').val()));
  		var _tot_TD=$('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_ImpTdebito').children('input').val();       _tot_TD=quitarLetra(_tot_TD);      _tot_TD=parseFloat(_tot_TD); //parseFloat(quitarLetra($('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_ImpTdebito').children('input').val()));
  		var _tot_OFP=$('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_OFPago').children('input').val();          _tot_OFP=quitarLetra(_tot_OFP);    _tot_OFP=parseFloat(_tot_OFP); //parseFloat(quitarLetra($('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_OFPago').children('input').val()));
  		var _tot_EZ=$('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_TotalEfectZ').children('input').val();      _tot_EZ=quitarLetra(_tot_EZ);      _tot_EZ=parseFloat(_tot_EZ); // parseFloat(quitarLetra($('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_TotalEfectZ').children('input').val()));


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

			$('#content_Daily_ul > li').each(function (index) {
				if(index > 0){
					var SucId=$(this).find('ul > li.column_Sucursal').children('select').val();
					var FDaily=$(this).find('ul > li.column_FechaDaily').children('input').val();

					if(_SucId+""+_FechaZ == SucId+""+FDaily){

						var _TotIng =$(this).find('li.column_TotalIng').children('input').val();       _TotIng=quitarLetra(_TotIng);       _TotIng=parseFloat(_TotIng); //parseFloat(quitarLetra($(this).find('li.column_TotalIng').children('input').val()));
						var _TotTC =$(this).find('li.column_ImpTcredito').children('input').val();     _TotTC=quitarLetra(_TotTC);         _TotTC=parseFloat(_TotTC); //parseFloat(quitarLetra($(this).find('li.column_ImpTcredito').children('input').val()));
						var _TotTD =$(this).find('li.column_ImpTdebito').children('input').val();      _TotTD=quitarLetra(_TotTD);         _TotTD=parseFloat(_TotTD);  //parseFloat(quitarLetra($(this).find('li.column_ImpTdebito').children('input').val()));
						var _TotOFP =$(this).find('li.column_OFPago').children('input').val();         _TotOFP=quitarLetra(_TotOFP);       _TotOFP=parseFloat(_TotOFP);  //parseFloat(quitarLetra($(this).find('li.column_OFPago').children('input').val()));
						var _NoZ =$(this).find('li.column_NumZ').children('input').val();              _NoZ=quitarLetra(_NoZ);             _NoZ=parseFloat(_NoZ); //parseFloat(quitarLetra($(this).find('li.column_NumZ').children('input').val()));
						var _TotEfectZ =$(this).find('li.column_TotalEfectZ').children('input').val(); _TotEfectZ=quitarLetra(_TotEfectZ); _TotEfectZ=parseFloat(_TotEfectZ);  //parseFloat(quitarLetra($(this).find('li.column_TotalEfectZ').children('input').val()));

						var SubTIng=_TotIng - _Importe;
						var SubTC=_TotTC - _ImpTC;
						var SubTD=_TotTD - _ImpTD;
						var SubOFP=_TotOFP -_ImpOFP;
						var SubNoz=_NoZ-1;
						var SubTEfZ=_TotEfectZ - _ImpEfectivo;

						$(this).find('li.column_TotalIng').children('input').val("$"+ formatoNumero( SubTIng ,_decimales,_separadorD,_separadorM));
						$(this).find('li.column_NumZ').children('input').val(SubNoz);
						$(this).find('li.column_TotalEfectZ').children('input').val("$"+ formatoNumero( SubTEfZ,_decimales,_separadorD,_separadorM));
						$(this).find('li.column_ImpTcredito').children('input').val("$"+ formatoNumero( SubTC,_decimales,_separadorD,_separadorM));
						$(this).find('li.column_ImpTdebito').children('input').val("$"+ formatoNumero( SubTD,_decimales,_separadorD,_separadorM));
						$(this).find('li.column_OFPago').children('input').val("$"+ formatoNumero( SubOFP,_decimales,_separadorD,_separadorM));


						$('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_TotalIng').children('input').val("$"+ formatoNumero(  _tot_Ing -_Importe,_decimales,_separadorD,_separadorM));
						$('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_ImpTcredito').children('input').val("$"+ formatoNumero( _tot_TC - _ImpTC,_decimales,_separadorD,_separadorM));
						$('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_ImpTdebito').children('input').val("$"+ formatoNumero( _tot_TD - _ImpTD,_decimales,_separadorD,_separadorM));
						$('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_OFPago').children('input').val("$"+ formatoNumero( _tot_OFP - _ImpOFP,_decimales,_separadorD,_separadorM));
						$('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_TotalEfectZ').children('input').val("$"+ formatoNumero( _tot_EZ - _ImpEfectivo,_decimales,_separadorD,_separadorM));


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
			

			$('#content_Daily_ul > li').each(function(index){

				if(index > 0){
					var SucId=$(this).find('ul > li.column_Sucursal').children('select').val();
					var FDaily=$(this).find('ul > li.column_FechaDaily').children('input').val();

					if(_SucId+""+_FechaZ == SucId+""+FDaily){
						

						var _TotIng =$(this).find('li.column_TotalIng').children('input').val();           _TotIng=quitarLetra(_TotIng);       _TotIng=parseFloat(_TotIng);  //parseFloat(quitarLetra($(this).find('li.column_TotalIng').children('input').val()));
						var _NoZ =$(this).find('li.column_NumZ').children('input').val();                                                      _NoZ=parseFloat(_NoZ); //parseInt($(this).find('li.column_NumZ').children('input').val());
						var _TotEfectZ =$(this).find('li.column_TotalEfectZ').children('input').val();     _TotEfectZ=quitarLetra(_TotEfectZ); _TotEfectZ=parseFloat(_TotEfectZ); //parseFloat(quitarLetra($(this).find('li.column_TotalEfectZ').children('input').val()));

						var _TotTC =$(this).find('li.column_ImpTcredito').children('input').val();         _TotTC=quitarLetra(_TotTC);         _TotTC=parseFloat(_TotTC); //parseFloat(quitarLetra($(this).find('li.column_ImpTcredito').children('input').val()));
						var _TotTD =$(this).find('li.column_ImpTdebito').children('input').val();          _TotTD=quitarLetra(_TotTD);         _TotTD=parseFloat(_TotTD); //parseFloat(quitarLetra($(this).find('li.column_ImpTdebito').children('input').val()));
						var _TotOFP =$(this).find('li.column_OFPago').children('input').val();             _TotOFP=quitarLetra(_TotOFP);       _TotOFP=parseFloat(_TotOFP);  //parseFloat(quitarLetra($(this).find('li.column_OFPago').children('input').val()));

						var SubTIng=_TotIng + _Importe;
						var SubNoz=_NoZ+1;
						var SubTEfZ=_TotEfectZ + _ImpEfectivo;
						var SubTC=_TotTC + _ImpTC;
						var SubTD=_TotTD + _ImpTD;
						var SubOFP=_TotOFP + _ImpOFP;

						$(this).find('li.column_TotalIng').children('input').val("$"+ formatoNumero( SubTIng,_decimales,_separadorD,_separadorM));
						$(this).find('li.column_NumZ').children('input').val(SubNoz)
						$(this).find('li.column_TotalEfectZ').children('input').val("$"+ formatoNumero( SubTEfZ,_decimales,_separadorD,_separadorM));
						$(this).find('li.column_ImpTcredito').children('input').val("$"+ formatoNumero( SubTC,_decimales,_separadorD,_separadorM ));
						$(this).find('li.column_ImpTdebito').children('input').val("$"+ formatoNumero( SubTD,_decimales,_separadorD,_separadorM ));
						$(this).find('li.column_OFPago').children('input').val("$"+ formatoNumero( SubOFP,_decimales,_separadorD,_separadorM ));

						$('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_TotalIng').children('input').val("$"+ formatoNumero( _tot_Ing +_Importe,_decimales,_separadorD,_separadorM));
						$('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_ImpTcredito').children('input').val("$"+ formatoNumero(_tot_TC + _ImpTC,_decimales,_separadorD,_separadorM));
						$('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_ImpTdebito').children('input').val("$"+ formatoNumero(_tot_TD + _ImpTD,_decimales,_separadorD,_separadorM));
						$('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_OFPago').children('input').val("$"+ formatoNumero( _tot_OFP + _ImpOFP,_decimales,_separadorD,_separadorM));
						$('#content_Daily_ul > li').eq(_lastIndex).children('ul').children('li .column_TotalEfectZ').children('input').val( "$"+ formatoNumero(_tot_EZ + _ImpEfectivo,_decimales,_separadorD,_separadorM));

						$(this).children('ul').removeClass('removeRow');

						return false;
					}

				}
			});

		}


}


function Variacion(event,Datos,indexCol){
	//event.preventDefault();
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
	//event.preventDefault();
	var _parent=$(event.target).parent().parent();
	var _index_Li=_parent.parent().index();
	var _id_Row=_parent.attr('id');
	var _UlId=_parent.parent().parent().attr('id');

	//console.log("index"+$(event.target).parent().index());

	/*obtener subtotales del renglon*/
	var SubTotalRow=0;
	var _total=0;
	var ArrayTotales=new Array(ArrayColIndex.length);
	var _lastIndex=$("#"+_UlId+" > li").length-1;

	//console.log(_lastIndex);
	for(var i=0;i< ArrayTotales.length;i++){
		ArrayTotales[i]=0;
	}

	if(IndexCol > -1){

		for(var i=0; i < ArrayColIndex.length; i++ ){
			//console.log($("#"+_UlId+" > li").eq(_index_Li).children('ul').children('li:visible').eq(ArrayColIndex[i]).children('input').val());
			var _indexCol=ArrayColIndex[i];

			var value= $("#"+_UlId+" > li").eq(_index_Li).children('ul').children('li').eq(_indexCol).children('input').val();
			value=quitarLetra(value);

			//var value=parseFloat(quitarLetra( $("#"+_UlId+" > li").eq(_index_Li).children('ul').children('li').eq(ArrayColIndex[i]).children('input').val()));
			
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

		$("#"+_UlId+" > li").eq(_index_Li).children('ul').children('li').eq(IndexCol).children('input').val("$"+ formatoNumero( SubTotalRow,_decimales,_separadorD,_separadorM));
	}

	$("#"+_UlId+" > li").each(function(index){

		if(index > 0 && index < _lastIndex){

			for(var i=0; i < ArrayTotales.length;i++){

				var _indexCol=ArrayColIndex[i];

				var value=$(this).children('ul').children('li').eq(_indexCol).children('input').val(); 
				value=quitarLetra(value);

				//var value=parseFloat(quitarLetra( $(this).children('ul').children('li:visible').eq(_indexCol).children('input').val()));

			//var value=$(this).children('ul').children('li').eq(ArrayColIndex[i]).children('input').val();
			//ArrayColIndex[i]
				
				if(value !=""){
					if(isNaN(parseFloat(value))){
						value=0;
					}else{
						value=parseFloat(value);
					}
				}else{
					value=0;
				}
				
				ArrayTotales[i]=ArrayTotales[i] + value;
				console.log(ArrayTotales);
				//$("#"+_UlId+" > li").eq(_lastIndex).children('ul').children('li').eq(_indexCol).children('input').val("$"+ formatoNumero( ArrayTotales[i],_decimales,_separadorD,_separadorM));
			}

			//console.log(ArrayTotales);
		}


	});

	//console.log(ArrayColIndex);
	for(var i=0; i < ArrayColIndex.length; i++){
		_total = (_total + ArrayTotales[i]);

		var _indexCol=ArrayColIndex[i];
		//console.log(_indexCol);
		//console.log($("#"+_UlId+" > li").eq(_index_Li).children('ul').children('li').eq(_indexCol).children('input').val());
		

		$("#"+_UlId+" > li").eq(_lastIndex).children('ul').children('li').eq(_indexCol).children('input').val("$"+ formatoNumero( ArrayTotales[i],_decimales,_separadorD,_separadorM));
		
	}


	//console.log(ArrayTotales);

	//$("#"+_UlId+" > li").eq(_lastIndex).children('ul').children('li').eq(IndexCol).children('input').val("$"+ formatoNumero( _total,_decimales,_separadorD,_separadorM));


	if(IndexCol > -1){
		/*sutotal en renglon*/
		//$("#"+_UlId+" > li").eq(_index_Li).children('ul').children('li').eq(IndexCol).children('input').val("$"+ formatoNumero( SubTotalRow,_decimales,_separadorD,_separadorM));
		

		/*total general cordenada la columan de total de cada renglon y el ultimo renglon */

		$("#"+_UlId+" > li").eq(_lastIndex).children('ul').children('li').eq(IndexCol).children('input').val("$"+ formatoNumero( _total,_decimales,_separadorD,_separadorM));
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
  //event.preventDefault();

	var index1=$(event.target).parent().parent().parent().index();
	var id=$(event.target).parent().parent().parent().attr('id');

	var Error=false;
	var ErrCampos="";
	//alert($(event.target).attr('disabled') );
//console.log("entro");
	if($(event.target).attr('readonly')!='readonly'){

	/*	Error=Field($('#content_RegZ_ul > li').eq(index1).children('ul').children('li .column_Sucursal').children('select').val());
		
		if(Error){ErrCampos +="\n"+ $('#content_RegZ_ul > li').eq(0).children('ul').children('li .column_Sucursal').text();}

		Error=Field($('#content_RegZ_ul > li').eq(index1).children('ul').children('li .column_FechaT').children('input').val());
		
		if(Error){ErrCampos +="\n"+ $('#content_RegZ_ul > li').eq(0).children('ul').children('li .column_FechaT').text();}

		Error=Field($('#content_RegZ_ul > li').eq(index1).children('ul').children('li .column_HoraT').children('input').val());
	
		if(Error){
			ErrCampos +="\n"+ $('#content_RegZ_ul > li').eq(0).children('ul').children('li .column_HoraT').text();
		}
		
		Error=Field($('#content_RegZ_ul > li').eq(index1).children('ul').children('li .column_FolioT').children('input').val());
		
		if(Error){ErrCampos +="\n"+ $('#content_RegZ_ul > li').eq(0).children('ul').children('li .column_FolioT').text();}

		Error=Field($('#content_RegZ_ul > li').eq(index1).children('ul').children('li .column_Caja').children('input').val());
		
		if(Error){ErrCampos +="\n"+ $('#content_RegZ_ul > li').eq(0).children('ul').children('li .column_Caja').text();}

		Error=Field($('#content_RegZ_ul > li').eq(index1).children('ul').children('li .column_NT').children('input').val());
		
		if(Error){ErrCampos +="\n"+ $('#content_RegZ_ul > li').eq(0).children('ul').children('li .column_NT').text();}

		Error=Field($('#content_RegZ_ul > li').eq(index1).children('ul').children('li .column_Cajero').children('input').val());
		if(Error){ErrCampos +="\n"+ $('#content_RegZ_ul > li').eq(0).children('ul').children('li .column_Cajero').children('input').val();}

		Error=Field($('#content_RegZ_ul > li').eq(index1).children('ul').children('li .column_ImpEfectivo').children('input').val());
		
		if(Error){ErrCampos +="\n"+ $('#content_RegZ_ul > li').eq(0).children('ul').children('li .column_ImpEfectivo').text();}

		if(isNaN(parseFloat($('#content_RegZ_ul > li').eq(index1).children('ul').children('li .column_ImpEfectivo').children('input').val())) ){
			ErrCampos +="\n"+"Cash debe ser Numerico.";
		}else{
			if(parseFloat($('#content_RegZ_ul > li').eq(index1).children('ul').children('li .column_ImpEfectivo').children('input').val())==0 ){
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
			
			$('#content_RegZ_ul > li').eq(index1).children('ul').children('li .column_sttReg').children('img').attr('src','../../iconos/sign_down.png');
			$('#content_RegZ_ul > li').eq(index1).children('ul').children('li .column_sttReg').children('img').attr('title','Agregar Z al Daily');
			  console.log("ver");
			//CalcImporte(event);
			*/
			CalcSumatorias(event,[13,14,15,16],17);
		
		/*}else{
			
			$('#content_RegZ_ul > li').eq(index1).children('ul').children('li .column_sttReg').children('img').attr('src','../../iconos/exclamation.png');
			$('#content_RegZ_ul > li').eq(index1).children('ul').children('li .column_sttReg').children('img').attr('title','Error En los siguientes campos: ' + ErrCampos);
		}*/

	}
	return ErrCampos;


  }
  function ValidateDATA (event) {
  	// body...	
  	var result=true;
  	var msg="";
  	var _UL=$(event.target).parent().parent().parent().parent();
  	var _index=$(event.target).parent().parent().parent().index();

  	console.log(ValidateSTT(event));
	  	if(!ValidateSTT(event)){
	  		alert("No se puede alterar un Daily que ya fue Validado o Terminado");
	  		return false;
	  	}
  		

  		var _sucursal=_UL.children('li').eq(_index).children('ul').children('li .column_Sucursal').children('select').find('option:selected').text();

  		msg ="Existen inconvenientes: renglon #"+_index+ " "+_sucursal.replace(" ","").split('-')[1]+"\n";

		var _fecha=_UL.children('li').eq(_index).children('ul').children('li .column_FechaT').children('input').val();
		var _hora=_UL.children('li').eq(_index).children('ul').children('li .column_HoraT').children('input').val();
		var _folio=_UL.children('li').eq(_index).children('ul').children('li .column_FolioT').children('input').val();
		var _caja=_UL.children('li').eq(_index).children('ul').children('li .column_Caja').children('input').val();
		var _num=_UL.children('li').eq(_index).children('ul').children('li .column_NT').children('input').val();
		var _cajero=_UL.children('li').eq(_index).children('ul').children('li .column_Cajero').children('input').val();
		
		var _imp=_UL.children('li').eq(_index).children('ul').children('li .column_Importe').children('input').val();

		 var RegExPattern = /^\d{1,2}\/\d{1,2}\/\d{2,4}$/;
		 console.log(_fecha);
	      if (!_fecha.match(RegExPattern)){
	      	msg+="Fecha: incorrecta";
	      	result=false;
	      }
	     	

		//var fmt = format.toUpperCase();
		if(_hora.length < 5 || _hora.length > 5){
			
			result=false;
			msg += "\n"+"Hora: debe contener 4 caracteres.";
		}else{
			var _valor=_hora.split(':');
			if(_valor.length < 2 || _valor.length > 2){
				msg+= "\n"+"Hora: Formato incorrecto";
				result=false;
			}else{

				if(parseFloat(_valor[0]) < 1 || parseFloat(_valor[0]) > 24 || parseFloat(_valor[1]) > 59){
					result=false;
					msg +="\n"+"Hora: incorrecta";
				}
			}
		}

		if(_folio=="" || _folio=="0"){
			msg += "\n"+"Folio: Faltante";
			result=false;
		}

		if(_caja=="" || _caja=="0"){
			msg += "\n"+"Caja: Faltante";
			result=false;
		}

		if(_num=="" || _num=="0"){
			msg += "\n"+"Num. Caja: Faltante";
			result=false;
		}

		if(_cajero=="" ){
			msg += "\n"+"Cajero: Faltante";
			result=false;
		}

		if(_imp=="" || _imp =="0"){
			msg +="\n"+"Importe(GRSS): No puede ser 0";
			result=false;
		}


		if(!result)
		alert(msg);


		return result;
		
  }

  function Add_Z(event){
  		//event.preventDefault();
  		var _parent=$(event.target).parent().parent();
  		var _index_Li=_parent.parent().index();
  		var _id_Row=_parent.attr('id');

  		var _content_z=$("#content_RegZ_ul > li");
  		var _content_daily=$("#content_Daily_ul > li");

  		var _lastIndex=_content_daily.length-1;/*ultimo index de  sucursal consolidado*/

  		//console.log(_lastIndex);
  		var _ULRegZ=_content_z.eq(_index_Li).children('ul');

  		/* datos*/
  		/**/

  		var _SucId=_ULRegZ.children('li .column_Sucursal').children('select').val();
  		var _FechaZ=_ULRegZ.children('li .column_FechaT').children('input').val();
  		var _Importe=parseFloat(quitarLetra(_ULRegZ.children('li .column_Importe').children('input').val()));
  		var _ImpEfectivo=parseFloat(quitarLetra(_ULRegZ.children('li .column_ImpEfectivo').children('input').val()));
  		var _TCredito=parseFloat(quitarLetra(_ULRegZ.children('li .column_ImpTcredito').children('input').val()));
  		var _TDebito=parseFloat(quitarLetra(_ULRegZ.children('li .column_ImpTdebito').children('input').val()));
  		var _OFPago=parseFloat(quitarLetra(_ULRegZ.children('li .column_OFPago').children('input').val()));


  		//console.log(_SucId);

  		/*totales*/

  		var _totIng=parseFloat(quitarLetra(_content_daily.eq(_lastIndex).children('ul').children('li .column_TotalIng').children('input').val()));
  		var _totimpTc=parseFloat(quitarLetra(_content_daily.eq(_lastIndex).children('ul').children('li .column_ImpTcredito').children('input').val()));
  		var _totimpTD=parseFloat(quitarLetra(_content_daily.eq(_lastIndex).children('ul').children('li .column_ImpTdebito').children('input').val()));
  		var _totOFP=parseFloat(quitarLetra(_content_daily.eq(_lastIndex).children('ul').children('li .column_OFPago').children('input').val()));
  		/*
  		var _totEDP=$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_EfectivoDepP').children('input').val();
  		var _totEDD=$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_EfectivoDepD').children('input').val();
  		var _totBP=$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_BolsaP').children('input').val();
  		var _totBD=$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_BolsaD').children('input').val();
  		var _totG=$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_GastoDeu').children('input').val();
  		var _totSob=$('#container_history1 > li').eq(_lastIndex).children('ul').children('li .column_SobFalt').children('input').val();
  		*/
  		var _totEfZ=parseFloat(quitarLetra(_content_daily.eq(_lastIndex).children('ul').children('li .column_TotalEfectZ').children('input').val()));
  		
  		//console.log(_totIng);
  		
  		/*variables*/
  		var encontro=false;

  		if(!ValidateDATA(event))
  			return false;


  			if($(event.target).attr('src')=='../../iconos/sign_down.png'){/* condicion si esta habilitado para agrgar  al daily cash sumary*/
  				
  				$(event.target).attr('src','../../iconos/document-save-as.png');
  				$(event.target).attr('title',"Z Agregado con exito.");
		  		/*******buscar sucursal fecha**************/
				_content_daily.each(function (index) {


					var SucId=$(this).find('ul > li.column_Sucursal').children('select').val();
					var FDaily=$(this).find('ul > li.column_FechaDaily').children('input').val();
					var _indexActual=$(this).index();


					if(_SucId+""+_FechaZ == SucId+""+FDaily){
					
						console.log("eneee");
						var _TotIng =parseFloat(quitarLetra($(this).find('li.column_TotalIng').children('input').val()));
						var _NoZ =parseFloat($(this).find('li.column_NumZ').children('input').val());
						var _TotEfectZ =parseFloat(quitarLetra($(this).find('li.column_TotalEfectZ').children('input').val()));
						var _TCr =parseFloat(quitarLetra($(this).find('li.column_ImpTcredito').children('input').val()));
						var _TDb =parseFloat(quitarLetra($(this).find('li.column_ImpTdebito').children('input').val()));
						var _OFP =parseFloat(quitarLetra($(this).find('li.column_OFPago').children('input').val()));


						var SubTIng=_TotIng + _Importe;
						var SubNoz=_NoZ+1;
						var SubTEfZ=_TotEfectZ + _ImpEfectivo;
						var ImpTcredit=_TCr +_TCredito;
						var ImpDeb=_TDb+_TDebito;
						var ImpOTF=_OFP + _OFPago;


						$(this).find('li.column_TotalIng').children('input').val("$"+ formatoNumero( SubTIng,_decimales,_separadorD,_separadorM));
						$(this).find('li.column_NumZ').children('input').val(SubNoz)
						$(this).find('li.column_TotalEfectZ').children('input').val("$"+ formatoNumero( SubTEfZ,_decimales,_separadorD,_separadorM));
						$(this).find('li.column_ImpTcredito').children('input').val( "$"+ formatoNumero(ImpTcredit,_decimales,_separadorD,_separadorM));
						$(this).find('li.column_ImpTdebito').children('input').val( "$"+ formatoNumero(ImpDeb,_decimales,_separadorD,_separadorM));
						$(this).find('li.column_OFPago').children('input').val("$"+ formatoNumero( ImpOTF,_decimales,_separadorD,_separadorM));

						_content_daily.eq(_indexActual).children('ul').children('li .column_TipoCambio').children('input').keyup();
						//CalcSummary($('#container_history1 > li').eq(_indexActual).children('ul').children('li .column_TipoCambio').children('input').keyup());

						$(this).children('ul').removeClass('removeRow');
						encontro=true;	

						_content_daily.eq(_lastIndex).children('ul').children('li .column_TotalIng').children('input').val("$"+ formatoNumero( _totIng + _Importe ,_decimales,_separadorD,_separadorM));
						_content_daily.eq(_lastIndex).children('ul').children('li .column_ImpTcredito').children('input').val("$"+ formatoNumero( _totimpTc +_TCredito,_decimales,_separadorD,_separadorM));
						_content_daily.eq(_lastIndex).children('ul').children('li .column_ImpTdebito').children('input').val("$"+ formatoNumero( _totimpTD +_TDebito,_decimales,_separadorD,_separadorM));
						_content_daily.eq(_lastIndex).children('ul').children('li .column_OFPago').children('input').val("$"+ formatoNumero( _totOFP + _OFPago,_decimales,_separadorD,_separadorM));
						

						_content_daily.eq(_lastIndex).children('ul').children('li .column_TotalEfectZ').children('input').val("$"+ formatoNumero( _totEfZ + _ImpEfectivo,_decimales,_separadorD,_separadorM));

						

						return false;
					}

				});


				if(!encontro){
					var Datos={Fecha:_FechaZ,NumZ:1,TotalIng:_Importe,TCredit:_TCredito,TDebit:_TDebito, OFPago:_OFPago,TotalEfectZ:_ImpEfectivo,SucursalID:_SucId,TipoCambio:$('#TipoC').val() };
					AddDaily(Datos);


				}
				_ULRegZ.find('li').children().removeClass('active');

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
  function AddDaily(ArrayDatos){

		var _content_z=$("#content_RegZ_ul > li");
  		var _content_daily=$("#content_Daily_ul > li");

	var html ="<li class='rowM'>";
		html +="<ul>";
		html +="<li class='column_stt' ><input value='ST0' /></li>"
		html +="<li class='column_CodeRegZ'><input /></li>";
		html +="<li class='row_ID' ><input value='ST0'/></li>";
		html +="<li class='column_ID'>No.</li>";
		html +="<li class='column_Check'><input class='STTN' value='N' disabled='disabled' readonly/></li>";
		html +="<li class='column_Edit column_fillColor'><img class='btnEdit' src='iconos/checkbox_unchecked (1).png' onclick='EditarDaily(event);' title='Editar registro'/></li>";
		html +="<li class='column_Sucursal'>";
		html +="<select id='select_sucursal2' disabled=true class='inactive' autofocus>";
		html +=FillSucursales();
		/*
		for (var i =0;i<ArraySuc.length;i++) {




			html+="<option value='"+ArraySuc[i].SucursalID +"'>"+("000"+ArraySuc[i].SucursalID).slice(-3)+"-"+ArraySuc[i].Nombre +"</option>";
		}
		*/
		html +="</select>";
		html +="</li>";
		html +="<li class='column_Supervisor' ><input /></li>";
		html +="<li class='column_FechaDaily' ><input value='"+ArrayDatos.Fecha +"' class='inactive'   readonly='readonly'/></li>";
		html +="<li class='column_NumZ'><input value='"+ArrayDatos.NumZ +"' class='inactive'  readonly='readonly'/></li>";
		html +="<li class='column_TotalIng'><input value='"+ArrayDatos.TotalIng+"' class='inactive'  readonly='readonly'/></li>";
		/*add columns*/
		html +="<li class='column_ImpTcredito' ><input value='"+ArrayDatos.TCredit +"' class='inactive'  readonly='readonly'/></li>";
		html +="<li class='column_ImpTdebito' ><input value='"+ArrayDatos.TDebit+"' class='inactive'  readonly='readonly'/></li>";
		html +="<li class='column_OFPago' ><input value='"+ArrayDatos.OFPago +"'  class='inactive'  readonly='readonly'/></li>";

		html +="<li class='column_TotalEfectivo'><input value='0' class='inactive' readonly='readonly'/></li>";
		html +="<li class='column_TotalEfectZ' style='background-color: lightblue;'><input value='"+ArrayDatos.TotalEfectZ +"' class='inactive' readonly='readonly'/></li>";
		html +="<li class='column_VariacionDayli'><input value='0' class='inactive' readonly='readonly'/></li>";

		html +="<li class='column_TipoCambio'><input value='"+ArrayDatos.TipoCambio+"' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);' /></li>";

		html +="<li class='column_EfectivoDepP' style='background-color: lightblue;'><input value='0' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);' /></li>";
		html +="<li class='column_EfectivoDepD' ><input value='0' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);' /></li>";
		
		html +="<li class='column_EfectivoDepPConv' style='background-color: lightblue;'><input value='0' class='inactive'  readonly='readonly'/></li>";

		//html +="<li class='column_FolioServices'><input /></li>";
		html +="<li class='column_BolsaP' style='background-color: lightblue;'><input  value='0' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);'/></li>";
		html +="<li class='column_BolsaD'><input  value='0' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);'/></li>";
		//html +="<li class='column_TipoCambio'><input  value='"+ArrayDatos.TipoCambio+"' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);'/></li>";
		html +="<li class='column_BolsaPConv' style='background-color: lightblue;'><input  value='0' class='inactive'  readonly='readonly'/></li>";


		html +="<li class='column_FolioServices'><input /></li>";

		html +="<li class='column_GastoDeu' style='background-color: lightblue;'><input  value='0' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);'/></li>";
		html +="<li class='column_Sob' style='background-color: lightblue;'><input  value='0' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);'/></li>";
		html +="<li class='column_CajeroCorto' ><input /></li>";
		html +="<li class='column_Falt' style='background-color: lightblue;'><input  value='0' onkeypress='return justNumbers(event);' onkeyup='CalcSummary(event);'/></li>";
		
		//html +="<li class='column_TotalEfectZ' style='background-color: lightblue;'><input value='"+ArrayDatos.TotalEfectZ +"' readonly/></li>";
		//html +="<li class='column_VariacionDayli'><input value='0' readonly/></li>";
		html +="<li class='column_ComentariosDayli'><img src='iconos/msgNew.png' onclick='VerComent(event);'/></li><li class='column_codUser'><input value='"+$("#SelectUsers").val()+"'/></li><li class='column_coment'><input /></li> </ul>";

		$(html).insertAfter($("#content_Daily_ul > li").eq(0)); //+' > li:eq(0)');

		$("#content_Daily_ul > li").eq(1).children('ul').children('li .column_Sucursal').find("#select_sucursal2 option[value='"+ArrayDatos.SucursalID+"']").attr('selected',true);

		$("#content_Daily_ul > li").each(function (index) {

				if(index!=0){
					$(this).find('ul > li.column_ID').text((index).toString());

				}


		});


		/******************************/

			//var _parent=$(event.target).parent().parent();
		var _index_Li=1;//_parent.parent().index();
		//var _id_Row=_parent.attr('id');
		//var _UlId=_parent.parent().parent().attr('id');
		//efectivos +servicios blindado(pesos) )-total efectivo)-sobrantes)+gastos=variacion

		var result=0;
		var _variacion=0;
		var _resultTot=0;


		var _lastIndex=$("#content_Daily_ul > li").length-1;

		var _totIng=quitarLetra($("#content_Daily_ul > li").eq(_lastIndex).children('ul').children('li .column_TotalIng').children('input').val()); _totIng=parseFloat(_totIng);

  		var _totimpTc=quitarLetra($("#content_Daily_ul > li").eq(_lastIndex).children('ul').children('li .column_ImpTcredito').children('input').val()); _totimpTc=parseFloat(_totimpTc);

  		var _totimpTD=quitarLetra($("#content_Daily_ul > li").eq(_lastIndex).children('ul').children('li .column_ImpTdebito').children('input').val()); _totimpTD=parseFloat(_totimpTD);

  		var _totOFP=quitarLetra($("#content_Daily_ul > li").eq(_lastIndex).children('ul').children('li .column_OFPago').children('input').val()); _totOFP=parseFloat(_totOFP);


  		var _totEfZ=quitarLetra($("#content_Daily_ul > li").eq(_lastIndex).children('ul').children('li .column_TotalEfectZ').children('input').val()); _totEfZ=parseFloat(_totEfZ);



		var _EfecDepP =$("#content_Daily_ul > li").eq(_index_Li).children('ul').children('li .column_EfectivoDepP').children('input').val(); _EfecDepP=quitarLetra(_EfecDepP); _EfecDepP=parseFloat(_EfecDepP);

		var _EfecDepD =$("#content_Daily_ul > li").eq(_index_Li).children('ul').children('li .column_EfectivoDepD').children('input').val(); _EfecDepD=quitarLetra(_EfecDepD); _EfecDepD=parseFloat(_EfecDepD);

		var _EfecBolP =$("#content_Daily_ul > li").eq(_index_Li).children('ul').children('li .column_BolsaP').children('input').val(); _EfecBolP=quitarLetra(_EfecBolP); _EfecBolP=parseFloat(_EfecBolP);

		var _EfecBolD =$("#content_Daily_ul > li").eq(_index_Li).children('ul').children('li .column_BolsaD').children('input').val(); _EfecBolD=quitarLetra(_EfecBolD); _EfecBolD=parseFloat(_EfecBolD);

		var _EfecTotZ =$("#content_Daily_ul > li").eq(_index_Li).children('ul').children('li .column_TotalEfectZ').children('input').val(); _EfecTotZ=quitarLetra(_EfecTotZ); _EfecTotZ=parseFloat(_EfecTotZ);

		var _sob =$("#content_Daily_ul > li").eq(_index_Li).children('ul').children('li .column_Sob').children('input').val(); _sob=quitarLetra(_sob); _sob=parseFloat(_sob);

		var _falt=$("#content_Daily_ul > li").eq(_index_Li).children('ul').children('li .column_Falt').children('input').val(); _falt=quitarLetra(_falt); _falt=parseFloat(_falt);

		var _gast=$("#content_Daily_ul > li").eq(_index_Li).children('ul').children('li .column_GastoDeu').children('input').val(); _gast=quitarLetra(_gast); _gast=parseFloat(_gast);

		var _tipoCambio=quitarLetra(ArrayDatos.TipoCambio); _tipoCambio=parseFloat(_tipoCambio); //_content_daily.children('li .column_TipoCambio').children('input').val(); _tipoCambio=quitarLetra(_tipoCambio); _tipoCambio=parseFloat(_tipoCambio);

		var _ConvEfecD_P=Convertidor(_EfecDepD,_tipoCambio ,1);

		var _convBolD_P=Convertidor(_EfecBolD,_tipoCambio,1);


		result=_EfecDepP  + _ConvEfecD_P  + _EfecBolP + _convBolD_P  + _gast;

		_variacion=(result-_EfecTotZ);

		$("#content_Daily_ul > li").eq(_index_Li).children('ul').children('li.column_TotalEfectivo').children('input').val( "$"+ formatoNumero( result,_decimales,_separadorD,_separadorM));

		$("#content_Daily_ul > li").eq(_index_Li).children('ul').children('li.column_EfectivoDepPConv').children('input').val("$"+ formatoNumero(_ConvEfecD_P ,_decimales,_separadorD,_separadorM ));

		$("#content_Daily_ul > li").eq(_index_Li).children('ul').children('li.column_BolsaPConv').children('input').val("$"+ formatoNumero(_convBolD_P,_decimales,_separadorD,_separadorM ));

		//si es sobrante sera signo negativo

		if( _sob > 0 ){

		 _resultTot=_variacion - _sob;
		 _resultTot=_resultTot + _falt;

		}else{
			_resultTot=_variacion + _falt;
		}

		$("#content_Daily_ul > li").eq(_index_Li).children('ul').children('li.column_VariacionDayli').children('input').val("$"+ formatoNumero( _resultTot,_decimales,_separadorD,_separadorM));
		
		if(_resultTot < 0){
			$("#content_Daily_ul > li").eq(_index_Li).children('ul').children('li.column_VariacionDayli').children('input').css({'color':'red'});
		}else{
			$("#content_Daily_ul > li").eq(_index_Li).children('ul').children('li.column_VariacionDayli').children('input').css({'color':'black'});
		}



		$("#content_Daily_ul > li").eq(_lastIndex).children('ul').children('li .column_TotalIng').children('input').val("$"+ formatoNumero( _totIng + ArrayDatos.TotalIng,_decimales,_separadorD,_separadorM ));
		$("#content_Daily_ul > li").eq(_lastIndex).children('ul').children('li .column_ImpTcredito').children('input').val("$"+ formatoNumero( _totimpTc + ArrayDatos.TCredit,_decimales,_separadorD,_separadorM));
		$("#content_Daily_ul > li").eq(_lastIndex).children('ul').children('li .column_ImpTdebito').children('input').val("$"+ formatoNumero( _totimpTD + ArrayDatos.TDebit ,_decimales,_separadorD,_separadorM));
		$("#content_Daily_ul > li").eq(_lastIndex).children('ul').children('li .column_OFPago').children('input').val("$"+ formatoNumero( _totOFP + ArrayDatos.OFPago,_decimales,_separadorD,_separadorM));
		$("#content_Daily_ul > li").eq(_lastIndex).children('ul').children('li .column_TotalEfectZ').children('input').val("$"+ formatoNumero( _totEfZ + ArrayDatos.TotalEfectZ,_decimales,_separadorD,_separadorM));
		
		

  }

function SummayNew(event){

}

  function CalcSummary(event){

  		var _lastIndex=$("#content_Daily_ul > li").length-1;
  		var _index=$(event.target).parent().parent().parent().index();
  		var _indexL=$(event.target).parent().index();
  		


  		//$("#"+_UlId+" > li").eq(_index_Li).children('ul').children('li:visible').eq(ArrayColIndex[i]).children('input').val()

  		/****************variacion***************************/

  		var _EfecDepP =$("#content_Daily_ul > li").eq(_index).children('ul').children('li .column_EfectivoDepP').children('input').val(); _EfecDepP=quitarLetra(_EfecDepP); _EfecDepP=parseFloat(_EfecDepP);

		var _EfecDepD =$("#content_Daily_ul > li").eq(_index).children('ul').children('li .column_EfectivoDepD').children('input').val(); _EfecDepD=quitarLetra(_EfecDepD); _EfecDepD=parseFloat(_EfecDepD);

		var _EfecBolP =$("#content_Daily_ul > li").eq(_index).children('ul').children('li .column_BolsaP').children('input').val(); _EfecBolP=quitarLetra(_EfecBolP); _EfecBolP=parseFloat(_EfecBolP);

		var _EfecBolD =$("#content_Daily_ul > li").eq(_index).children('ul').children('li .column_BolsaD').children('input').val(); _EfecBolD=quitarLetra(_EfecBolD); _EfecBolD=parseFloat(_EfecBolD);

		var _EfecTotZ =$("#content_Daily_ul > li").eq(_index).children('ul').children('li .column_TotalEfectZ').children('input').val(); _EfecTotZ=quitarLetra(_EfecTotZ); _EfecTotZ=parseFloat(_EfecTotZ);

		var _sob =$("#content_Daily_ul > li").eq(_index).children('ul').children('li .column_Sob').children('input').val(); _sob=quitarLetra(_sob); _sob=parseFloat(_sob);

		var _falt=$("#content_Daily_ul > li").eq(_index).children('ul').children('li .column_Falt').children('input').val(); _falt=quitarLetra(_falt); _falt=parseFloat(_falt);

		var _gast=$("#content_Daily_ul > li").eq(_index).children('ul').children('li .column_GastoDeu').children('input').val(); _gast=quitarLetra(_gast); _gast=parseFloat(_gast);

		var _tipoCambio=$("#content_Daily_ul > li").eq(_index).children('ul').children('li .column_TipoCambio').children('input').val();_tipoCambio=quitarLetra(_tipoCambio);  _tipoCambio=parseFloat(_tipoCambio); //_content_daily.children('li .column_TipoCambio').children('input').val(); _tipoCambio=quitarLetra(_tipoCambio); _tipoCambio=parseFloat(_tipoCambio);

		var _ConvEfecD_P=Convertidor(_EfecDepD,_tipoCambio ,1);

		var _convBolD_P=Convertidor(_EfecBolD,_tipoCambio,1);


		result=_EfecDepP  + _ConvEfecD_P  + _EfecBolP + _convBolD_P  + _gast;

		_variacion=(result-_EfecTotZ);

		$("#content_Daily_ul > li").eq(_index).children('ul').children('li.column_TotalEfectivo').children('input').val( "$"+ formatoNumero( result,_decimales,_separadorD,_separadorM));

		$("#content_Daily_ul > li").eq(_index).children('ul').children('li.column_EfectivoDepPConv').children('input').val("$"+ formatoNumero(_ConvEfecD_P ,_decimales,_separadorD,_separadorM ));

		$("#content_Daily_ul > li").eq(_index).children('ul').children('li.column_BolsaPConv').children('input').val("$"+ formatoNumero(_convBolD_P,_decimales,_separadorD,_separadorM ));

		//si es sobrante sera signo negativo

		if( _sob > 0 ){

		 _resultTot=_variacion - _sob;
		 _resultTot=_resultTot + _falt;

		}else{
			_resultTot=_variacion + _falt;
		}

		$("#content_Daily_ul > li").eq(_index).children('ul').children('li.column_VariacionDayli').children('input').val("$"+ formatoNumero( _resultTot,_decimales,_separadorD,_separadorM));
		
		if(_resultTot < 0){
			$("#content_Daily_ul > li").eq(_index).children('ul').children('li.column_VariacionDayli').children('input').css({'color':'red'});
		}else{
			$("#content_Daily_ul > li").eq(_index).children('ul').children('li.column_VariacionDayli').children('input').css({'color':'black'});
		}

		CalcSumatorias(event,[10,11,12,13,18,19,21,22,25],-1);


		//CalcSumatorias(event,[18,19,21,22,25],-1);

  		/******************************************/



  		//Variacion(event,_datos,12);//10
			  	//CalcSumatorias(event,[5,6,7,9,12,13,14,15,17],-1);
			  	//CalcSumatorias(event,[7,8,9,10,12,15,16,18,19,22],-1);

  		//event.preventDefault();
  		//console.log("entro");
  		

  		/*
  		var _parent=$(event.target).parent().parent();
  		var _index_Li=_parent.parent().index();
  		var _id_Row=_parent.attr('id');
  	
  		var _ULRegZ=$("#content_Daily_ul > li").eq(_index_Li).children('ul');

		
		if($(event.target).attr('readonly')!='readonly'){

				var EP=0;if (!isNaN(parseFloat(quitarLetra(_ULRegZ.children('li .column_EfectivoDepP').children('input').val())))){EP=parseFloat(quitarLetra( _ULRegZ.children('li .column_EfectivoDepP').children('input').val())); }
				var ED=0;if (!isNaN(parseFloat(quitarLetra(_ULRegZ.children('li .column_EfectivoDepD').children('input').val())))) {ED=parseFloat(quitarLetra( _ULRegZ.children('li .column_EfectivoDepD').children('input').val()));}
				var BP=0;if (!isNaN(parseFloat(quitarLetra(_ULRegZ.children('li .column_BolsaP').children('input').val())))) {BP=parseFloat(quitarLetra(_ULRegZ.children('li .column_BolsaP').children('input').val()));}
				var BD=0;if (!isNaN(parseFloat(quitarLetra(_ULRegZ.children('li .column_BolsaD').children('input').val())))) {BD=parseFloat(quitarLetra(_ULRegZ.children('li .column_BolsaD').children('input').val()));}
				var TE=0;if (!isNaN(parseFloat(quitarLetra(_ULRegZ.children('li .column_TotalEfectZ').children('input').val())))) {TE=parseFloat(quitarLetra(_ULRegZ.children('li .column_TotalEfectZ').children('input').val()));}
				var S=0;if (!isNaN(parseFloat(quitarLetra(_ULRegZ.children('li .column_Sob').children('input').val())))) {S=parseFloat(quitarLetra(_ULRegZ.children('li .column_Sob').children('input').val()));}
				var F=0;if (!isNaN(parseFloat(quitarLetra(_ULRegZ.children('li .column_Falt').children('input').val())))) {F=parseFloat(quitarLetra(_ULRegZ.children('li .column_Falt').children('input').val()));}
				var G=0;if (!isNaN(parseFloat(quitarLetra(_ULRegZ.children('li .column_GastoDeu').children('input').val())))) {G=parseFloat( quitarLetra(_ULRegZ.children('li .column_GastoDeu').children('input').val()));}
				var TC=0;if (!isNaN(parseFloat(quitarLetra(_ULRegZ.children('li .column_TipoCambio').children('input').val())))) {TC=parseFloat(quitarLetra(_ULRegZ.children('li .column_TipoCambio').children('input').val()));}

				//console.log(_ULRegZ.children('li .column_Sob').children('input').val()+"valor");

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


				Variacion(event,_datos,12);//10
			  	//CalcSumatorias(event,[5,6,7,9,12,13,14,15,17],-1);
			  	//CalcSumatorias(event,[7,8,9,10,12,15,16,18,19,22],-1);
	  	}*/
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
  	//event.preventDefault();
	$("#WindowComent").toggle();

  	var _content_z=$("#content_RegZ_ul > li");
  		var _content_daily=$("#content_Daily_ul > li");
  	var index1=$(event.target).parent().parent().parent().index();

	var id=$(event.target).parent().parent().parent().attr('id');
	indextemp=null;
	indextemp=index1;

	//$("#WindowComent").hide();
	//$("#textcoment").text("");
	document.getElementById("textcoment").value="";
	
	/*
	if($('#container_history1 > li').eq(index1).hasClass('row')){
		$('#container_history1 > li').eq(index1).removeClass('row');
		$('#container_history1 > li').eq(index1).addClass('rowM');
	}*/
	
	var comentario=$("#content_Daily_ul > li").eq(index1).children('ul').children('li .column_coment').children('input').val();
	

	//$("#textcoment").text(comentario);
	document.getElementById("textcoment").value=comentario;

	console.log($(event.target).offset().top);

  	$("#WindowComent").css({'top': $(event.target).position().top-(parseFloat($("#WindowComent").height())/4)}); //+(parseFloat($("#WindowComent").height())/2) });
  	$("#WindowComent").css({'left': $(event.target).position().left-($("#WindowComent").width())});
  	
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
  	$("#content_Daily_ul > li").eq(indextemp).children('ul').children('li .column_coment').children('input').val("");
 	$("#content_Daily_ul > li").eq(indextemp).children('ul').children('li .column_coment').children('input').val($("#textcoment").val());
 	$("#WindowComent").toggle("fast");
  }

  function quitarLetra(monto){
  	//console.log(monto);
  	if(typeof monto==="undefined"){
  		monto=0;
  	}else{
  		if(monto!=""){
  			monto=monto.replace('$','');
  			monto=monto.replace(' ','');

  			//monto=monto.replace(/[A-Za-z$-]/g,'');
			monto=monto.replace(',','');
  		}else{

  			monto=0;
  		}
  	}
  
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