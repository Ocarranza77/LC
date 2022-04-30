


$(document).ready(function(){

	$('#txtNota').blur(function(){
		//$('#btnAdd').focus();
		$('#btnAdd').focus();
	});

})
$(document).click(function(event){

	if(event.target.tagName!="INPUT"){
		$('.PanelEstatus').hide();
	}
	
});

var _now=new Date();	
var _currentDate=("0"+_now.getDate()).slice(-2) +"/"+("0"+(_now.getMonth()+1)).slice(-2) +"/"+_now.getFullYear();



function clickTab(event){
	var id=$(event.target).val();
	var element=event.target;
	$(event.target).parent().parent().children('li').children('input').removeClass('_tabactive');
	//$(event.target).removeClass('_tabInactive');
	element.className='_tabactive';
	$(event.target).addClass('_tabactive');


	$('.content_tab > div').css({'display':'none'});
	
	switch($(event.target).attr('id')){
		case "DYIng":
			$('#content_DYIng').css({'display':'block'});
		break;
		case "RepDep":
			$('#content_Depositos').css({'display':'block'});
		break;
		
	}	
}
function FindCtas(parameter){
	var html="";
	$.ajax({
	    type: "POST",
	    url: "wfrIngresosDep.aspx/GetCtasB",
	    data:"{'BcoID':'"+parameter+"'}",
	    contentType: "application/json; charset=utf-8",
	    dataType: "json", 
	    async:false, 
	    success: function (response) {
	    	html=response.d;
	    	$('#cbxCta option').remove();
	    	$('#cbxCta').append(html);
	    }
	});
	return html;
}
function FindBanco(parameter){
	var html="";
	$.ajax({
	    type: "POST",
	    url: "wfrIngresosDep.aspx/GetBancosB",
	    data:"{'parameter':'"+CtaID+"'}",
	    contentType: "application/json; charset=utf-8",
	    dataType: "json", 
	    async:false, 
	    success: function (response) {
	    	html=response.d;

	    }
	});
	return html;
}

function FillSucursales(parameter){
	var html="";
	$.ajax({
	    type: "POST",
	    url: "wfrIngresosDep.aspx/GetSucursales",
	    data:"{'parameter':'"+parameter+"'}",
	    contentType: "application/json; charset=utf-8",
	    dataType: "json", 
	    async:false, 
	    success: function (response) {
	    	html=response.d;

	    }
	});
	return html;

}
function FillBancos(parameter){
	var html="";
	$.ajax({
	    type: "POST",
	    url: "wfrIngresosDep.aspx/GetBancos",
	    data:"{'parameter':'"+parameter+"'}",
	    contentType: "application/json; charset=utf-8",
	    dataType: "json", 
	    async:false, 
	    success: function (response) {
	    	html=response.d;

	    }
	});
	return html;

}
function FillMoneda(parameter){
	var html="";
	$.ajax({
	    type: "POST",
	    url: "wfrIngresosDep.aspx/GetMoneda",
	    data:"{'parameter':'"+parameter+"'}",
	    contentType: "application/json; charset=utf-8",
	    dataType: "json", 
	    async:false, 
	    success: function (response) {
	    	html=response.d;

	    }
	});
	return html;

}
function FillCtas(parameter){
	var html="";
	$.ajax({
	    type: "POST",
	    url: "wfrIngresosDep.aspx/GetCtas",
	    data:"{'parameter':'"+parameter+"'}",
	    contentType: "application/json; charset=utf-8",
	    dataType: "json", 
	    async:false, 
	    success: function (response) {
	    	html=response.d;

	    }
	});
	return html;

}

function Save(){

	var _codeCrt="";
	var _codeDEp="";

	//var termina=true;
	//v//ar count=0;
	//var result="";

	var xhr=null;	


	$('#content_DYIng_ul > li.RowEdit').each(function(index){

		var _corte=new Array();
		var indexList=$(this).index();

		
		$('#content_DYIng_ul > li').eq(indexList).children('ul').children('li.column_sttReg').children('img').attr('src','../iconos/loanding_min.gif');

		_corte[0]=$(this).find('ul > li.column_fecha').children('input').val();
		_corte[1]=$(this).find('ul > li.column_SuID').children('input').val();

		//_corte[2]=$(this).find('ul > li.column_TotDepPesos').children('input').val();
		//_corte[3]=$(this).find('ul > li.column_TotDepDolares').children('input').val();
		//_corte[4]=$(this).find('ul > li.column_SaldoDepPesos').children('input').val();
		//_corte[5]=$(this).find('ul > li.column_SaldoDepDolares').children('input').val();
		_corte[2]=$(this).find('ul > li.column_Deudor').children('input').val();
		_corte[3]=$(this).find('ul > li.column_DeudorP').children('input').val();
		_corte[4]=$(this).find('ul > li.column_DeudorD').children('input').val();



		_corte[6]=$(this).find('ul > li.column_Check').children('input').val();


		_codeCrt=_corte[0]+""+_corte[1];


		_corte[5]="";
		var indexList2=0;
		$('#content_Depositos_ul > li.RowNew').each(function (index){

		

			indexList2=$(this).index();
			_codeDEp=$(this).find('li.column_fecha').children('input').val()+""+$(this).find('li.column_SuID').children('input').val();

			if(_codeCrt==_codeDEp){

				//$('#content_Depositos_ul > li').eq(indexList2).children('ul').children('li.column_sttReg').children('img').attr('src','../iconos/loanding_min.gif');

				if(_corte[5]!=""){_corte[5]+=";";}
				
				_corte[5]+=$(this).find('li.column_DepositoID').children('input').val();//0
				//_corte[5]+="|"+$(this).find('li.column_fecha').children('input').val();
				_corte[5]+="|"+$(this).find('li.column_NoSec').children('input').val();//1
				//_corte[5]+="|"+$(this).find('li.column_SuID').children('input').val();
				_corte[5]+="|"+$(this).find('li.column_BanID').children('input').val();//2
				_corte[5]+="|"+$(this).find('li.column_CtaID').children('input').val();//3
				_corte[5]+="|"+$(this).find('li.column_MonID').children('input').val();//4
				_corte[5]+="|"+$(this).find('li.column_FolioDep').children('input').val();//5
				_corte[5]+="|"+$(this).find('li.column_FechaDep').children('input').val();//6
				_corte[5]+="|"+$(this).find('li.column_Importe').children('input').val();//7
				_corte[5]+="|"+$(this).find('li.column_Nota').children('input').val();//8
				_corte[5]+="|"+$(this).attr('class');//9
				_corte[5]+="|"+indexList2;
			}
		});

		$('#content_Depositos_ul > li.RowEdit').each(function(index){

	
			indexList2=$(this).index();
			_codeDEp=$(this).find('li.column_fecha').children('input').val()+""+$(this).find('li.column_SuID').children('input').val();
			if(_codeCrt==_codeDEp){

				//$('#content_Depositos_ul > li').eq(indexList2).children('ul').children('li.column_sttReg').children('img').attr('src','../iconos/loanding_min.gif');

				if(_corte[5]!=""){_corte[5]+=";";}
				_corte[5]+=$(this).find('li.column_DepositoID').children('input').val();
				//_corte[5]+="|"+$(this).find('li.column_fecha').children('input').val();
				_corte[5]+="|"+$(this).find('li.column_NoSec').children('input').val();
				//_corte[5]+="|"+$(this).find('li.column_SuID').children('input').val();
				_corte[5]+="|"+$(this).find('li.column_BanID').children('input').val();
				_corte[5]+="|"+$(this).find('li.column_CtaID').children('input').val();
				_corte[5]+="|"+$(this).find('li.column_MonID').children('input').val();
				_corte[5]+="|"+$(this).find('li.column_FolioDep').children('input').val();
				_corte[5]+="|"+$(this).find('li.column_FechaDep').children('input').val();
				_corte[5]+="|"+$(this).find('li.column_Importe').children('input').val();
				_corte[5]+="|"+$(this).find('li.column_Nota').children('input').val();
				_corte[5]+="|"+$(this).attr('class');
				_corte[5]+="|"+indexList2;

			}

		});


		$('#content_Depositos_ul > li.RowDelete').each(function(index){

	
			indexList2=$(this).index();
			_codeDEp=$(this).find('li.column_fecha').children('input').val()+""+$(this).find('li.column_SuID').children('input').val();
			if(_codeCrt==_codeDEp){

				//$('#content_Depositos_ul > li').eq(indexList2).children('ul').children('li.column_sttReg').children('img').attr('src','../iconos/loanding_min.gif');

				if(_corte[5]!=""){_corte[5]+=";";}
				_corte[5]+=$(this).find('li.column_DepositoID').children('input').val();
				//_corte[5]+="|"+$(this).find('li.column_fecha').children('input').val();
				_corte[5]+="|"+$(this).find('li.column_NoSec').children('input').val();
				//_corte[5]+="|"+$(this).find('li.column_SuID').children('input').val();
				_corte[5]+="|"+$(this).find('li.column_BanID').children('input').val();
				_corte[5]+="|"+$(this).find('li.column_CtaID').children('input').val();
				_corte[5]+="|"+$(this).find('li.column_MonID').children('input').val();
				_corte[5]+="|"+$(this).find('li.column_FolioDep').children('input').val();
				_corte[5]+="|"+$(this).find('li.column_FechaDep').children('input').val();
				_corte[5]+="|"+$(this).find('li.column_Importe').children('input').val();
				_corte[5]+="|"+$(this).find('li.column_Nota').children('input').val();
				_corte[5]+="|"+$(this).attr('class');
				_corte[5]+="|"+indexList2;

			}

		});
		


	$('.loaderDiv').show();
	$.ajax({
	    type: "POST",
	    url: "wfrIngresosDep.aspx/UpdateCorte",
	    data: JSON.stringify({corte:_corte}),
	    contentType: "application/json; charset=utf-8",
	    dataType: "json",
	   async:false, 
	  // beforeSend:function(){$('.loaderDiv').css({'display':'block'});},
	    success: function (response) {
	    	var result=response.d;
	    	var msg=result.split("|");
	    	var src="../iconos/ok-24.png";


	    	//typemsg=1;//parseInt(msg[0]);
	    	//result=msg[2];
	    	console.log(result);

	    	if(msg[0]=="1"){
	    		src="../iconos/Icon-warning.png";
	    	}

	    	$('#content_DYIng_ul > li').eq(indexList).children('ul').children('li.column_sttReg').children('img').attr('src',src);
	    	$('#content_DYIng_ul > li').eq(indexList).children('ul').children('li.column_sttReg').children('img').attr('title',msg[2]);
	    	$('#content_DYIng_ul > li').eq(indexList).removeClass();
	    	$('#content_DYIng_ul > li').eq(indexList).addClass('row');
	    	$('.loaderDiv').hide(0);
	    	
	    	

	    }//,complete:function(){$('.loaderDiv').css({'display':'none'});}
		});

	});




}

function validate(str){
 var fvalue = parseFloat(str);
  return !isNaN(fvalue) && fvalue != 0; 
} 


function ValidaNegativo(value){
	//var value=$(event.target).val();
	//var patron = /^\d*$/;  
	var resul=true;
/*
	console.log((!isNaN(parseFloat(value)) && 0 <= ~~ parseFloat value));

	if(!(!isNaN(parseFloat(value)) && 0 <= ~~parseFloat(value))){
		resul=false;
	}
	
*/
	return resul;
}

function MostCalendar () {
	// body...
	$("#btnFecha").click();
}

function NexID(_fecha,_sucursalID){
	/*var _ID=0;
	$.ajax({
	    type: "POST",
	    url: "wfrIngresosDep.aspx/GetCtasB",
	    data:"{'fecha':'"+_fecha+"',ID:'"+_sucursalID+"'}",
	    contentType: "application/json; charset=utf-8",
	    dataType: "json", 
	    async:false, 
	    success: function (response) {
	    	_ID=response.d;
	    	
	    }
	});

	return _ID;
*/
	var contador=0;
	$('#content_Depositos_ul > li').each(function(index){
		var _sucursalID2=$(this).find('li.column_SuID').children('input').val();
		var _fecha2=$(this).find('li.column_fecha').children('input').val();

		if(index > 0 && $(this).attr('class')!="RowDelete" ){
			
			if(_fecha+""+_sucursalID==_fecha2+""+_sucursalID2){
				contador++;
			}

		}
		
	
	});
	return contador+=1;
}
function ADDIngreso (event) {
	// body...
	console.log(event.target);
	var _ul=$(event.target).parent().parent();
	var _index=_ul.parent().index();
	_ul.addClass('activeUL');
	$('.PanelEstatus').hide();
	var _stt=_ul.find('li.column_Check').children('input').val();
	var _css=_ul.find('li.column_Check').children('input').attr('class');

	//if(_stt!="" && _stt!="N" && _stt!="R"){
	//	alert('No se permite modificar un corte VALIDADO o TERMINADO.');
	//	return false;
	//}

	if(_ul.parent().attr('class')!="RowEdit" ){
		_ul.parent().removeClass('row');
		_ul.parent().addClass('RowEdit');
	}
	

	$('#txtIndexRow').val(_index);
	$('#btnEliminar').css({'display':'none'});

	 //.children('img').attr('class');

	var _sucursal=_ul.find('li.column_Sucursal').children('input').val(); //.children('select').find('option:selected').text();
	var _sucursalID=_ul.find('li.column_SuID').children('input').val(); //_scucursal=_scucursal.replace(" ","");
	
	var _TC=_ul.find('li.column_TipoCambio').children('input').val(); // _TC=quitarLetra(_TC); _TC=parseFloat(_TC);
	var _fecha=_ul.find('li.column_fecha').children('input').val();
	var _Importe=_ul.find('li.column_Importe').children('input').val(); 
	var _TCredito=_ul.find('li.column_ImpTcredito').children('input').val();
	var _TDebito=_ul.find('li.column_ImpTdebito').children('input').val();
	var _EPaDep=_ul.find('li.column_EfectivoDepP').children('input').val();
	var _EDaDep=_ul.find('li.column_EfectivoDepD').children('input').val();

	var _ServicesP=_ul.find('li.column_FolioServices').children('input').val();
	var _ServicesD=_ul.find('li.column_FolioServicesD').children('input').val();
	var _CajeroC=_ul.find('li.column_CajeroCorto').children('input').val();
	var _ImpFaltP=_ul.find('li.column_Falt').children('input').val();
	var _TotDepaValP=_ul.find('li.column_TotDepValPesos').children('input').val();
	var _TotDepaValD=_ul.find('li.column_TotDepValDolares').children('input').val();

	var _TotDepP=_ul.find('li.column_TotDepPesos').children('input').val();
	var _TotDepD=_ul.find('li.column_TotDepDolares').children('input').val();

	var _SalPesos=_ul.find('li.column_SaldoDepPesos').children('input').val();
	var _SalDolares=_ul.find('li.column_SaldoDepDolares').children('input').val();
	var _deudor=_ul.find('li.column_Deudor').children('input').val();
	var _deudorP=_ul.find('li.column_DeudorP').children('input').val();
	var _deudorD=_ul.find('li.column_DeudorD').children('input').val();

	

	$('#txtNoSec').val(NexID(_fecha,_sucursalID));

	$('#txtstt').removeClass();
	$('#txtstt').addClass(_css);
	$('#txtstt').val(_stt);

	$('#txtTC').val(_TC);
	$('#txtsuc').val(_sucursal);
	$('#txtSucursalID').val(_sucursalID);
	$('#txtFechaCap').val(_fecha);
	$('#txtImporte').val(_Importe);
	$('#txtTCredito').val(_TCredito);
	$('#txtTDebito').val(_TDebito);
	$('#txtEfectivoPaDep').val(_EPaDep);
	$('#txtefectivoDaDep').val(_EDaDep);
	$('#txtServicesP').val(_ServicesP);
	$('#txtServicesD').val(_ServicesD);
	$('#txtCajeroCorto').val(_CajeroC);
	$('#txtImpFaltP').val(_ImpFaltP);
	$('#txtTotalDepaValP').val(_TotDepaValP);
	$('#txtTotalDepaValD').val(_TotDepaValD);

	$('#txtTotalPDep').val(_TotDepP);
	$('#txtTotalDDep').val(_TotDepD);

	$('#txtPesos').val(_SalPesos);
	$('#txtDolares').val(_SalDolares);

	$('#txtFechaDep').val(_currentDate);
	$('#txtDeudorNom').val(_deudor);
	$('#txtImpDeudorP').val(_deudorP);
	$('#txtImpDeudorD').val(_deudorD);


	//$('#cbxBanco option').remove();

	$('#cbxBanco').html(FillBancos("0"));
	$('#cbxMoneda').html(FillMoneda("0"));
	$('#cbxCta').html(FillCtas("0"));



	$("#ADDIng").toggle(); //.animate({"height":"toggle"},"fast");
	$('#txtNoSec').focus();

}

function Saldo(event){

	var id=$(event.target).attr('id');
	var _value=$(event.target).val();_value=quitarLetra(_value); _value=parseFloat(_value);

	var _DepValP=$('#txtTotalDepaValP').val(); _DepValP=quitarLetra(_DepValP); _DepValP=parseFloat(_DepValP);
	var _DepValD=$('#txtTotalDepaValD').val(); _DepValD=quitarLetra(_DepValD); _DepValD=parseFloat(_DepValD);

	var _TotDepP=$('#txtTotalPDep').val(); _TotDepP=quitarLetra(_TotDepP); _TotDepP=parseFloat(_TotDepP);
	var _TotDepD=$('#txtTotalDDep').val(); _TotDepD=quitarLetra(_TotDepD); _TotDepD=parseFloat(_TotDepD);

	//var _deudorP=$('#txtImpDeudorP').val();
	//var _deudorD=$('#txtImpDeudorD').val();


	var result=0;

	switch(id){
		case "txtImpDeudorP":

			/*if(!ValidaNegativo(_TotDepP)){
				alert("Por Favor revise las fichas de deposito,"+"\n"+"Sus depositos son mayores a los ingresos,no es posible ajustar.");
				return false;
			}
				
*/
			result=CalSaldo(_DepValP,_TotDepP,_value);
			result=formatoNumero(result,2,".",",");

			$('#txtPesos').val(result);

		break;
		case "txtImpDeudorD":

			/*if(!ValidaNegativo(_TotDepD)){
				alert("Por Favor revise las fichas de deposito,"+"\n"+"Sus depositos son mayores a los ingresos,no es posible ajustar.");
				return false;
			}
*/
				result=CalSaldo(_DepValD,_TotDepD,_value);
				result=formatoNumero(result,2,".",",");

			$('#txtDolares').val(result);
		break;
	}

	console.log(result);
	
}

function CalSaldo(TotalV,TotDep,Deudor){
	var result=0;
	console.log(parseInt(TotalV-TotDep));
		if(parseInt(TotalV-TotDep) < 0){
			result= (TotalV-TotDep)+Deudor;
		}else{
			result= (TotalV-TotDep)-Deudor;
		}
	return result;
}
function Eliminar(event){
	var _index=$('#txtIndexEditDep').val();
	var _indexRow=$('#txtIndexRow').val();

	var _ul=$('#content_Depositos_ul > li').eq(_index).children('ul');
	_ul.removeClass('activeUL');
	_ul.addClass('removeRow');

	$('#content_Depositos_ul > li').eq(_index).removeClass();
	$('#content_Depositos_ul > li').eq(_index).addClass('RowDelete');
	_ul.find('li').children().attr('disabled',true);
	//_ul.find('li').children().addClass('inactive');
	//$('#content_Depositos_ul > li').eq(_index).attr('disabled',true);

	//_ul.find('li.column_eject').children('input').val('RowDelete');


	//var _indexRow=$('#txtIndexRow').val();
	//var _indexDep=$('#txtIndexEditDep').val();

	//var _stt=$('#txtstt').val();
	//var _noS=$('#txtNoSec').val();
	//var _Importe=$('#txtImpDep').val();   _Importe=quitarLetra(_Importe);   _Importe=parseFloat(_Importe);
	//var _monedaID=$('#cbxMoneda').val();  _monedaID=parseInt(_monedaID);
	//var _BancoID=$('#cbxBanco').val();
	//var _CtaID=$('#cbxCta').val();
	//var _folio=$('#txtFolioDep').val();
	//var _fechaDep=$('#txtFechaDep').val();


	var _DepP=$('#txtTotalPDep').val();
	var _DepD=$('#txtTotalDDep').val();

	var _SalP=$('#txtPesos').val();
	var _SalD=$('#txtDolares').val();

	var _ImpDeuP=$('#txtImpDeudorP').val(); _ImpDeuP=quitarLetra(_ImpDeuP); _ImpDeuP=parseFloat(_ImpDeuP);

	var _ImpDeuD=$('#txtImpDeudorD').val(); _ImpDeuD=quitarLetra(_ImpDeuD); _ImpDeuD=parseFloat(_ImpDeuD);


		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_TotDepPesos').children('input').val(_DepP);
		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_TotDepDolares').children('input').val(_DepD);

		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_SaldoDepPesos').children('input').val(_SalP);
		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_SaldoDepDolares').children('input').val(_SalD);

		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_Deudor').children('input').val($('#txtDeudorNom').val());
		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_DeudorP').children('input').val(formatoNumero(_ImpDeuP,2,".",","));
		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_DeudorD').children('input').val(formatoNumero(_ImpDeuD,2,".",","));






	Clear();


	//if($('#content_DYIng_ul > li').eq($('#txtIndexRow').val()).attr('class')=="row")
	//$('#content_DYIng_ul > li').eq($('#txtIndexRow').val()).children('ul').removeClass('activeUL');

	$('#txtIndexEditDep').val(0);
	$('#txtIndexRow').val(0);

	$(event.target).parent().parent().parent().parent().toggle(); //.animate({"height":"toggle"},"fast");
}

function EditDep(event){
	var _ul=$(event.target).parent().parent();
	var _index=_ul.parent().index();
	_ul.addClass('activeUL');

	var _stt=_ul.find('li.column_Check').children('input').val();

	if(_stt!="" && _stt!="N" && _stt!="R" && _stt!="V"){
		//$(event.target).attr('disabled','disabled');
		alert('No se permite modificar un corte VALIDADO o TERMINADO.');
		return false;
	}

	if(_ul.parent().attr('class')=="row"){

		_ul.parent().removeClass('row');
		_ul.parent().addClass('RowEdit');

	}
	
	
	$('#txtIndexEditDep').val(_index);

	var _sucursalID_Dep=_ul.find('li.column_SuID').children('input').val();
	var _fecha_Vta=_ul.find('li.column_fecha').children('input').val();
	var _Ban_Dep=_ul.find('li.column_BanID').children('input').val();
	var _Mon_Dep=_ul.find('li.column_MonID').children('input').val();
	var _Cta_Dep=_ul.find('li.column_CtaID').children('input').val();
	
	var _NoS_Dep=_ul.find('li.column_NoSec').children('input').val();
	var _folio_Dep=_ul.find('li.column_FolioDep').children('input').val();
	var _fecha_Dep=_ul.find('li.column_FechaDep').children('input').val();
	var _Imp_Dep=_ul.find('li.column_Importe').children('input').val(); _Imp_Dep=quitarLetra(_Imp_Dep); _Imp_Dep=parseFloat(_Imp_Dep);
	var _Nota_Dep=_ul.find('li.column_Nota').children('input').val();


	if(_index > 0){

		$('#btnEliminar').css({'display':'block'});
	}else{
		$('#btnEliminar').css({'display':'none'});
	}

	$('#content_DYIng_ul > li').each(function(index){
		var _sucursalID=$(this).find('ul > li.column_SuID').children('input').val(); //.children('select').val();
		var _fecha=$(this).find('ul > li.column_fecha').children('input').val();


		if(_fecha_Vta+""+_sucursalID_Dep==_fecha+""+_sucursalID){

				$('#txtIndexRow').val(index);

				if($(this).attr('class')=="row"){
					$(this).removeClass('row');
					$(this).addClass('RowEdit');
				}
				

				var _stt=$(this).find('li.column_Check').children('input').val(); //.children('img').attr('class');

				var _sucursal=$(this).find('li.column_Sucursal').children('input').val(); //.children('select').find('option:selected').text();
				var _sucursalID=$(this).find('li.column_SuID').children('input').val(); //_scucursal=_scucursal.replace(" ","");
				
				var _TC=$(this).find('li.column_TipoCambio').children('input').val(); // _TC=quitarLetra(_TC); _TC=parseFloat(_TC);
				var _fecha=$(this).find('li.column_fecha').children('input').val();
				var _Importe=$(this).find('li.column_Importe').children('input').val(); 
				var _TCredito=$(this).find('li.column_ImpTcredito').children('input').val();
				var _TDebito=$(this).find('li.column_ImpTdebito').children('input').val();
				var _EPaDep=$(this).find('li.column_EfectivoDepP').children('input').val();
				var _EDaDep=$(this).find('li.column_EfectivoDepD').children('input').val();

				var _ServicesP=$(this).find('li.column_FolioServices').children('input').val();
				var _ServicesD=$(this).find('li.column_FolioServicesD').children('input').val();
				var _CajeroC=$(this).find('li.column_CajeroCorto').children('input').val();
				var _ImpFaltP=$(this).find('li.column_Falt').children('input').val();

				var _TotDepaValP=$(this).find('li.column_TotDepValPesos').children('input').val(); _TotDepaValP=quitarLetra(_TotDepaValP); _TotDepaValP=parseFloat(_TotDepaValP);
				var _TotDepaValD=$(this).find('li.column_TotDepValDolares').children('input').val(); _TotDepaValD=quitarLetra(_TotDepaValD); _TotDepaValD=parseFloat(_TotDepaValD);

				var _TotDepP=$(this).find('li.column_TotDepPesos').children('input').val(); _TotDepP=quitarLetra(_TotDepP); _TotDepP=parseFloat(_TotDepP);
				var _TotDepD=$(this).find('li.column_TotDepDolares').children('input').val(); _TotDepD=quitarLetra(_TotDepD); _TotDepD=parseFloat(_TotDepD); 

				

				var _SalPesos=$(this).find('li.column_SaldoDepPesos').children('input').val();     _SalPesos=quitarLetra(_SalPesos);    _SalPesos=parseFloat(_SalPesos);
				var _SalDolares=$(this).find('li.column_SaldoDepDolares').children('input').val(); _SalDolares=quitarLetra(_SalDolares); _SalDolares=parseFloat(_SalDolares);


				var _deudor=$(this).find('li.column_Deudor').children('input').val();
				var _deudorP=$(this).find('li.column_DeudorP').children('input').val(); _deudorP=quitarLetra(_deudorP); _deudorP=parseFloat(_deudorP);
				var _deudorD=$(this).find('li.column_DeudorD').children('input').val(); _deudorD=quitarLetra(_deudorD); _deudorD=parseFloat(_deudorD);


			

				if(parseInt(_Mon_Dep)==1){
					_TotDepP=_TotDepP-_Imp_Dep;

					/*	if(parseInt(_SalPesos) < 0){

						 _SalPesos=_SalPesos+_Imp_Dep;

						}else{

						_SalPesos=_SalPesos-_Imp_Dep;

						}*/
						_SalPesos=CalSaldo(_TotDepaValP,_TotDepP,_deudorP);

				}

				if(parseInt(_Mon_Dep)==2){
					_TotDepD=_TotDepD-_Imp_Dep;

					/*if(parseInt(_SalDolares)< 0){

					_SalDolares=_SalDolares+_Imp_Dep;

					}else{

						_SalDolares=_SalDolares-_Imp_Dep;

					}*/

					_SalDolares=CalSaldo(_TotDepaValD,_TotDepD,_deudorD);

				}

				
					console.log(_Mon_Dep);
				console.log(_TotDepaValP);
				console.log(_TotDepP);
				

				_TotDepaValP=formatoNumero(_TotDepaValP,2,".",",");
				_TotDepaValD=formatoNumero(_TotDepaValD,2,".",",");

				_TotDepP=formatoNumero(_TotDepP,2,".",",");
				_TotDepD=formatoNumero(_TotDepD,2,".",",");

				_SalPesos=formatoNumero(_SalPesos,2,".",",");
				_SalDolares=formatoNumero(_SalDolares,2,".",",");

				_deudorP=formatoNumero(_deudorP,2,".",",");
				_deudorD=formatoNumero(_deudorD,2,".",",");



	
				$('#txtstt').val(_stt);
				$('#txtstt').removeClass();
				$('#txtstt').addClass('STT'+_stt);

				$('#txtTC').val(_TC);

				$('#txtsuc').val(_sucursal);
				$('#txtSucursalID').val(_sucursalID);

				$('#txtFechaCap').val(_fecha);

				$('#txtImporte').val(_Importe);
				$('#txtTCredito').val(_TCredito);
				$('#txtTDebito').val(_TDebito);
				$('#txtEfectivoPaDep').val(_EPaDep);
				$('#txtefectivoDaDep').val(_EDaDep);
				$('#txtServicesP').val(_ServicesP);
				$('#txtServicesD').val(_ServicesD);
				$('#txtCajeroCorto').val(_CajeroC);
				$('#txtImpFaltP').val(_ImpFaltP);

				$('#txtTotalDepaValP').val(_TotDepaValP);
				$('#txtTotalDepaValD').val(_TotDepaValD);



				$('#txtTotalPDep').val(_TotDepP);
				$('#txtTotalDDep').val(_TotDepD);

				$('#txtPesos').val(_SalPesos);
				$('#txtDolares').val(_SalDolares);

				$('#txtFechaDep').val(_currentDate);
				$('#txtDeudorNom').val(_deudor);
				$('#txtImpDeudorP').val(_deudorP);
				$('#txtImpDeudorD').val(_deudorD);


				//$('#cbxBanco option').remove();

				$('#cbxBanco').html(FillBancos(_Ban_Dep));
				$('#cbxMoneda').html(FillMoneda(_Mon_Dep));
				$('#cbxCta').html(FillCtas(_Cta_Dep));

				$('#txtNoSec').val(_NoS_Dep);
				$('#txtFolioDep').val(_folio_Dep);
				$('#txtFechaDep').val(_fecha_Dep);
				$('#txtImpDep').val(_Imp_Dep);
				$('#txtNota').val(_Nota_Dep);



				$("#ADDIng").toggle(); //.animate({"height":"toggle"},"fast");
				$('#txtNoSec').focus();
		}

	});
}
var change=false;
function CloseW(event){
	Clear();

	if($('#content_DYIng_ul > li').eq($('#txtIndexRow').val()).attr('class')=="row"){
		$('#content_DYIng_ul > li').eq($('#txtIndexRow').val()).children('ul').removeClass('activeUL');
	}
	

	

	$('#txtIndexRow').val(0);
	$('#txtIndexEditDep').val(0);

	$(event.target).parent().parent().parent().parent().toggle(); //.animate({"height":"toggle"},"fast");

}

function EnterCode(event){
	if(event.keyCode===13){
		AddDeposito(event);
	}
}

function ConfirmDep(event){
	var _indexRow=$('#txtIndexRow').val();
	var _indexDep=$('#txtIndexEditDep').val();

	var _stt=$('#txtstt').val();
	var _noS=$('#txtNoSec').val();
	var _Importe=$('#txtImpDep').val();   _Importe=quitarLetra(_Importe);   _Importe=parseFloat(_Importe);
	var _monedaID=$('#cbxMoneda').val();  _monedaID=parseInt(_monedaID);
	var _BancoID=$('#cbxBanco').val();
	var _CtaID=$('#cbxCta').val();
	var _folio=$('#txtFolioDep').val();
	var _fechaDep=$('#txtFechaDep').val();


	var _DepP=$('#txtTotalPDep').val();
	var _DepD=$('#txtTotalDDep').val();

	var _SalP=$('#txtPesos').val();
	var _SalD=$('#txtDolares').val();

	var _ImpDeuP=$('#txtImpDeudorP').val(); _ImpDeuP=quitarLetra(_ImpDeuP); _ImpDeuP=parseFloat(_ImpDeuP);

	var _ImpDeuD=$('#txtImpDeudorD').val(); _ImpDeuD=quitarLetra(_ImpDeuD); _ImpDeuD=parseFloat(_ImpDeuD);

	$('.PanelEstatus').hide();
	console.log(_folio);
	console.log(_BancoID);
	console.log(_indexDep);

	//if(_BancoID=="" || _monedaID=="" || isNaN(_monedaID) || _CtaID=="" || _folio=="" || _fechaDep=="" || _Importe < 0.01){
	if(_indexDep!="" && parseInt(_indexDep) > 0 && $('#txtDeudorNom').val()!=""){	
		change=true;

		//$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_TotDepPesos').children('input').val(_DepP);
		//$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_TotDepDolares').children('input').val(_DepD);


		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_Check').children('input').val(_stt);
		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_Check').children('input').removeClass();
		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_Check').children('input').addClass("STT"+_stt);
		


		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_SaldoDepPesos').children('input').val(_SalP);
		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_SaldoDepDolares').children('input').val(_SalD);

		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_Deudor').children('input').val($('#txtDeudorNom').val());
		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_DeudorP').children('input').val(formatoNumero(_ImpDeuP,2,".",","));
		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_DeudorD').children('input').val(formatoNumero(_ImpDeuD,2,".",","));


		//if(_indexDep > 0){
			var _f=$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_SuID').children('input').val();
			var _sID=$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_Sucursal').children('input').val();

			$('#content_Depositos_ul > li').each(function (index){
				
				if(index > 0){

					var _f1=$(this).find('li.column_Sucursal').children('input').val();
					var _sID1=$(this).find('li.column_SuID').children('input').val();

					if(_f+""+_sID==_f1+""+_sID1){

						$(this).find('li.column_Check').children('input').val(_stt);
						$(this).find('li.column_Check').children('input').removeClass();
						$(this).find('li.column_Check').children('input').addClass("STT"+_stt);
						console.log(_stt);
					}
				}
			});
			
		//}
		//$('#txtIndexEditDep').val(0);

		$("#ADDIng").hide();
	}//else{

	if(AddDeposito(event)){
		$("#ADDIng").hide();
	}

	//}


}

function AddDeposito(event){

	var html="";
	var _indexRow=$('#txtIndexRow').val();
	var _indexDep=$('#txtIndexEditDep').val();
	var _TC=$('#txtTC').val();_TC=quitarLetra(_TC); _TC=parseFloat(_TC);

	var _stt=$('#txtstt').val();

	var _fecha=$('#txtFechaCap').val();//fechaVTA
	var _noS=$('#txtNoSec').val();
	var _sucursalID=$('#txtSucursalID').val();
	
	var _Importe=$('#txtImpDep').val();   _Importe=quitarLetra(_Importe);   _Importe=parseFloat(_Importe);
	
	var _monedaID=$('#cbxMoneda').val();  _monedaID=parseInt(_monedaID);
	var _BancoID=$('#cbxBanco').val();
	var _CtaID=$('#cbxCta').val();

	var _sucursal=$('#txtsuc').val();
	var _moneda=$('#cbxMoneda option:selected').text(); // _monedaID=parseFloat(_monedaID);
	var _Banco=$('#cbxBanco option:selected').text();
	var _Cta=$('#cbxCta option:selected').text();

	var _folio=$('#txtFolioDep').val();
	var _fechaDep=$('#txtFechaDep').val();
	var _nota=$('#txtNota').val();

	var _TotDepP=$('#txtTotalPDep').val(); _TotDepP=quitarLetra(_TotDepP); _TotDepP=parseFloat(_TotDepP);
	var _TotDepD=$('#txtTotalDDep').val(); _TotDepD=quitarLetra(_TotDepD); _TotDepD=parseFloat(_TotDepD);


	var _TotDepPaVal=$('#txtTotalDepaValP').val(); _TotDepPaVal=quitarLetra(_TotDepPaVal); _TotDepPaVal=parseFloat(_TotDepPaVal);
	var _TotDepDaVal=$('#txtTotalDepaValD').val(); _TotDepDaVal=quitarLetra(_TotDepDaVal); _TotDepDaVal=parseFloat(_TotDepDaVal);

	var _ImpDeuP=$('#txtImpDeudorP').val(); _ImpDeuP=quitarLetra(_ImpDeuP); _ImpDeuP=parseFloat(_ImpDeuP);
	var _ImpDeuD=$('#txtImpDeudorD').val(); _ImpDeuD=quitarLetra(_ImpDeuD); _ImpDeuD=parseFloat(_ImpDeuD);


	console.log(_folio);

	if(_BancoID=="" || _monedaID=="" || isNaN(_monedaID) || _CtaID=="" || _folio.length < 1 || _folio=="0" || _fechaDep=="" || _Importe < 0.01){
		$('#txtNoSec').focus();
		alert("Por Favor verifique sus datos.");
		return false;
	}



	change=true;

	var _SubTotalP=0;
	var _SubTotalD=0;
	var _SubDifP=0;
	var _SubDifD=0;

	//var _ImpDeudor=_ImpDeuP+Convertidor(_ImpDeuD,_TC,1);

	//if(_Importe < 0.00 )
	//	return false;


	//console.log(_TotDepPaVal);
	switch(_monedaID){
		case 1:
		_SubTotalP=_TotDepP+_Importe;
		_SubDifP=CalSaldo(_TotDepPaVal,_SubTotalP,_ImpDeuP);//   (_TotDepPaVal-_SubTotalP)-_ImpDeuP;


		_SubTotalP=formatoNumero(_SubTotalP,2,".",",");
		_SubDifP=formatoNumero(_SubDifP,2,".",",");

		$('#txtTotalPDep').val(_SubTotalP);
		$('#txtPesos').val(_SubDifP);

		//$('#txtDolares').val(formatoNumero((_TotDepDaVal-_TotDepD)-_ImpDeuD,2,".",","));


		//$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_TotDepPesos').children('input').val(_SubTotal);
		//$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_SaldoDepPesos').children('input').val(_SubDif);

		//$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_SaldoDepDolares').children('input').val( formatoNumero((_TotDepDaVal-_TotDepD)-_ImpDeuD,2,".",",") );

		break;
		case 2:
		_SubTotalD=_TotDepD+_Importe;
		_SubDifD=CalSaldo(_TotDepDaVal,_SubTotalD,_ImpDeuD);

		_SubTotalD=formatoNumero(_SubTotalD,2,".",",");
		_SubDifD=formatoNumero(_SubDifD,2,".",",");

		$('#txtTotalDDep').val(_SubTotalD);

		//$('#txtPesos').val(formatoNumero((_TotDepPaVal-_ImpDeuP)-_ImpDeuP,2,".",",") );

		$('#txtDolares').val(_SubDifD);

		//$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_TotDepDolares').children('input').val(_SubTotal);

		//$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_SaldoDepPesos').children('input').val( formatoNumero((_TotDepPaVal-_ImpDeuP)-_ImpDeuP,2,".",",") );
		//$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_SaldoDepDolares').children('input').val(_SubDif);
		break;
		}

		console.log($('#txtTotalPDep').val());
		console.log($('#txtTotalDDep').val());

		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_TotDepPesos').children('input').val($('#txtTotalPDep').val());
		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_SaldoDepPesos').children('input').val($('#txtPesos').val());

		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_TotDepDolares').children('input').val($('#txtTotalDDep').val());
		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_SaldoDepDolares').children('input').val($('#txtDolares').val());

		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_Deudor').children('input').val($('#txtDeudorNom').val());
		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_DeudorP').children('input').val(formatoNumero(_ImpDeuP,2,".",","));
		$('#content_DYIng_ul > li').eq(_indexRow).children('ul').find('li.column_DeudorD').children('input').val(formatoNumero(_ImpDeuD,2,".",","));

		if(_indexDep!="" && parseInt(_indexDep) > 0){

			var _ul=$('#content_Depositos_ul > li').eq(_indexDep).children('ul');

			_ul.find('li.column_Check').children('input').val(_stt);
			_ul.find('li.column_Check').children('input').removeClass();
			_ul.find('li.column_Check').children('input').addClass("STT"+_stt);

			_ul.find('li.column_SuID').children('input').val(_sucursalID);
			_ul.find('li.column_BanID').children('input').val(_BancoID);
			_ul.find('li.column_CtaID').children('input').val(_CtaID);
			_ul.find('li.column_MonID').children('input').val(_monedaID);

			_ul.find('li.column_NoSec').children('input').val(_noS);
			_ul.find('li.column_Sucursal').children('input').val(_sucursal);
			_ul.find('li.column_Banco').children('input').val(_Banco);
			_ul.find('li.column_Moneda').children('input').val(_moneda);
			_ul.find('li.column_Cuenta').children('input').val(_Cta);
			_ul.find('li.column_FolioDep').children('input').val(_folio);
			_ul.find('li.column_FechaDep').children('input').val(_fechaDep);
			_ul.find('li.column_Importe').children('input').val(formatoNumero(_Importe,2,".",","));
			_ul.find('li.column_Nota').children('input').val(_nota);
			$('#txtIndexEditDep').val(0);
			
		}else{

		html+=" <li class='RowNew'>";
		html+="	<ul>";
		html+="	<li class='column_eject'><input value='0'/></li>";
		html+="	<li class='column_DepositoID'><input value='0'/></li>";
		html+="	<li class='column_fecha'><input value='"+_fecha+"'/></li>";
		html+="	<li class='column_SuID'><input value='"+_sucursalID+"'/></li>";
		html+="	<li class='column_BanID'><input value='"+_BancoID+"'/></li>";
		html+="	<li class='column_CtaID'><input value='"+_CtaID+"'/></li>";
		html+="	<li class='column_MonID'><input value='"+_monedaID+"'/></li>";
		html+="	<li class='column_ID'>No.</li>";
		html+="	<li class='column_Check'><input class='STT"+_stt+"' value='"+_stt+"' disabled='disabled' readonly/></li>";
		html+="	<li class='column_Sucursal'>";
		
		html+="<input value='"+_sucursal+"' class='inactive' ondblclick='EditDep(event);' readonly/>"
		html+="</li>";
		html+="	<li class='column_NoSec'><input value='"+_noS+"' class='inactive' disabled='disabled' readonly/></li>";
		html+="	<li class='column_Banco'>";
		html+="<input value='"+_Banco+"' class='inactive' ondblclick='EditDep(event);' readonly/>"
		html +="</li>";
		html+="	<li class='column_Moneda'>";
	
		html+="<input value='"+_moneda+"' class='inactive' ondblclick='EditDep(event);' readonly/>";	
		html+="</li>";
		html+="	<li class='column_Cuenta'>";
		html+="<input value='"+_Cta+"' class='inactive' ondblclick='EditDep(event);' readonly/>";
		html+="</li>";
		html+="	<li class='column_FolioDep'><input value='"+_folio+"'  class='inactive' ondblclick='EditDep(event);'  readonly/></li>";
		html+="	<li class='column_FechaDep'><input value='"+_fechaDep+"' class='inactive' ondblclick='EditDep(event);'  readonly/></li>";
		html+="	<li class='column_Importe'><input value='"+formatoNumero(_Importe,2,".",",")+"' class='inactive' ondblclick='EditDep(event);' readonly/></li>";
		html+="	<li class='column_Nota'><input value='"+_nota+"' class='inactive' disabled='disabled' readonly/></li>";
		html+="	<li class='column_sttReg'><img /></li>";
		html+="	</ul></li>";

		$(html).insertAfter('#content_Depositos_ul > li:eq(0)');

		$('#txtNoSec').focus();
	}

		console.log(_indexRow+"-"+_indexDep);

		$('#txtNoSec').val(NexID(_fecha,_sucursalID));
		$('#cbxBanco').html(FillBancos('0'));
		$('#cbxCta').html(FillCtas('0'));
		$('#cbxMoneda').html(FillMoneda('0'));

		$('#txtFolioDep').val("");
		$('#txtFechaDep').val(_currentDate);
		$('#txtImpDep').val(0);
		$('#txtNota').val("");


		return true;
}

function CapImporte(event){


}

function Clear(){


	$('#txtTC').val("");
	$('#txtsuc').val("");
	$('#txtFechaCap').val(_currentDate);
	$('#txtImporte').val(0.00);
	$('#txtTCredito').val(0.00);
	$('#txtTDebito').val(0.00);
	$('#txtEfectivoPaDep').val(0.00);
	$('#txtefectivoDaDep').val(0.00);
	$('#txtServicesP').val(0.00);
	$('#txtServicesD').val(0.00);
	$('#txtCajeroCorto').val("");
	$('#txtImpFaltP').val(0.00);
	$('#txtTotalDepaValP').val(0.00);
	$('#txtTotalDepaValD').val(0.00);

	$('#txtTotalPDep').val(0.00);
	$('#txtTotalDDep').val(0.00);

	$('#txtDeudorNom').val("");
	$('#txtImpDeudorP').val("");
	$('#txtImpDeudorD').val("");

	$('#txtPesos').val(0.00);
	$('#txtDolares').val(0.00);
	$('#txtNoSec').val("");
	$('#txtBanco').val("");
	$('#txtCuenta').val("");
	$('#txtFolioDep').val("");
	$('#txtFechaDep').val(_currentDate);
	$('#txtImpDep').val(0);
	$('#txtNota').val();
}

 function quitarLetra(monto){
  	if(typeof monto==="undefined"){
  		monto=0;
  	}else{
  		if(monto!=""){
  			monto=monto.replace(/$/g,'');
  			monto=monto.replace(' ','');

  			//monto=monto.replace(/[A-Za-z$-]/g,'');
			monto=monto.replace(/,/g,'');
  		}else{

  			monto=0;
  		}
  	}
  
	return monto;
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
function ChangeSTT2(event){
	//var _ul=$(event.target).parent().parent();
	var _stt=$(event.target).val();
	//var _index=$(event.target).parent().parent().parent().index();
	var _top=$(event.target).position().top;
	var _left=$(event.target).position().left;

	var _Div_top=$('.panelPopud > div').position().top-240;//-($('.panelPopud > div').height()/2);
	var _Div_left=$('.panelPopud > div').position().left+50;//-($('.panelPopud > div').width()/2);


	/*
	if(_ul.parent().attr('class')=="row"){
		_ul.parent().removeClass('row');
		_ul.parent().addClass('RowEdit');
	}
	
*/
	


	_top=_Div_top;
	_left=_Div_left;

	console.log($('.panelPopud > div').height()/2);
	console.log($('.panelPopud > div').position().top);

	$(".PanelEstatus").css({'top':_top+10});
	$(".PanelEstatus").css({'left':_left+25});
	$(".PanelEstatus").toggle("fast");
	$("#txtSttID").val(0);
	$('#txtCssAnt').val(_stt);
	console.log(_stt);

}

function ChangeSTT(event) {
	// body...
	var _ul=$(event.target).parent().parent();
	var _stt=$(event.target).val();
	var _index=$(event.target).parent().parent().parent().index();
	var _top=$(event.target).position().top;
	var _left=$(event.target).position().left;


	
	if(_ul.parent().attr('class')=="row"){
		_ul.parent().removeClass('row');
		_ul.parent().addClass('RowEdit');
	}
	

	$(".PanelEstatus").css({'top':_top+10});
	$(".PanelEstatus").css({'left':_left+25});
	$(".PanelEstatus").toggle("fast");
	$("#txtSttID").val(_index);


}
function SelectSTT (event) {
	// body...
	
	var _index=$("#txtSttID").val();
	var _claseAnt="";

	if(_index > 0){
		 _claseAnt=$("#content_DYIng_ul > li").eq(_index).children('ul').children('li .column_Check').children('input').attr('class');
	}else{
		_claseAnt="STT"+$('#txtCssAnt').val();
	}
	
	var _claseNew=$(event.target).parent().children('input').attr('class');
	var msg="";
	//console.log($(event.target).parent().children('img').attr('class'));
	//console.log(_claseAnt);
	//console.log(_claseNew);

	//if(_claseAnt=="STTR" && _claseNew=="STTN")
	//	return false;
		var _ImpVP=0;
		var _ImpVD=0;

		var _DepP=0;
		var _DepD=0;

		var _DeuP=0;
		var _DeuD=0;


		if(_index > 0 ){

			var _ImpVP=$("#content_DYIng_ul > li").eq(_index).children('ul').children('li .column_TotDepValPesos').children('input').val(); _ImpVP=quitarLetra(_ImpVP); _ImpVP=parseFloat(_ImpVP);
			var _ImpVD=$("#content_DYIng_ul > li").eq(_index).children('ul').children('li .column_TotDepValDolares').children('input').val(); _ImpVD=quitarLetra(_ImpVD); _ImpVD=parseFloat(_ImpVD);

			var _DepP=$("#content_DYIng_ul > li").eq(_index).children('ul').children('li .column_TotDepPesos').children('input').val(); _DepP=quitarLetra(_DepP);_DepP=parseFloat(_DepP);
			var _DepD=$("#content_DYIng_ul > li").eq(_index).children('ul').children('li .column_TotDepDolares').children('input').val(); _DepD=quitarLetra(_DepD);_DepD=parseFloat(_DepD);

			var _DeuP=$("#content_DYIng_ul > li").eq(_index).children('ul').children('li .column_DeudorP').children('input').val(); _DeuP=quitarLetra(_DeuP); _DeuP=parseFloat(_DeuP);
			var _DeuD=$("#content_DYIng_ul > li").eq(_index).children('ul').children('li .column_DeudorD').children('input').val(); _DeuD=quitarLetra(_DeuD); _DeuD=parseFloat(_DeuD);

		}else{
		
			_ImpVP=$('#txtTotalDepaValP').val();_ImpVP=quitarLetra(_ImpVP); _ImpVP=parseFloat(_ImpVP);
			_ImpVD=$('#txtTotalDepaValD').val();_ImpVD=quitarLetra(_ImpVD); _ImpVD=parseFloat(_ImpVD);

			_DepP=$('#txtTotalPDep').val(); _DepP=quitarLetra(_DepP);_DepP=parseFloat(_DepP);
			_DepD=$('#txtTotalDDep').val(); _DepD=quitarLetra(_DepD);_DepD=parseFloat(_DepD);

			_DeuP=$('#txtImpDeudorP').val(); _DeuP=quitarLetra(_DeuP); _DeuP=parseFloat(_DeuP);
			_DeuD=$('#txtImpDeudorD').val();  _DeuD=quitarLetra(_DeuD); _DeuD=parseFloat(_DeuD);
		}

		if(_claseNew=="STTT"){

			if(CalSaldo(_ImpVP,_DepP,_DeuP) > 0){msg="Pesos"}

			if(CalSaldo(_ImpVD,_DepD,_DeuD) > 0){if(msg!=""){msg+=",";}msg+="Dolares";}

			if(msg!=""){
				alert("Existen inconsistencias en los ingresos "+ msg +"\n"+"Por Favor verifique,Gracias.");
				return false;
			}
		}


		if( _index > 0){

			$("#content_DYIng_ul > li").eq(_index).children('ul').children('li .column_Check').children('input').removeClass(_claseAnt);	
			$("#content_DYIng_ul > li").eq(_index).children('ul').children('li .column_Check').children('input').addClass(_claseNew);	
			$("#content_DYIng_ul > li").eq(_index).children('ul').children('li .column_Check').children('input').val($(event.target).parent().children('input').val());
		}else{

			$('#txtstt').removeClass(_claseAnt);
			$('#txtstt').addClass(_claseNew);
			$('#txtstt').val($(event.target).parent().children('input').val());
		}
		console.log($('#txtCssAnt').val());

		$(".PanelEstatus").toggle("fast");
		 if(_index > 0){
		 	 $("#txtSttID").val(0);
		 }
		

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
