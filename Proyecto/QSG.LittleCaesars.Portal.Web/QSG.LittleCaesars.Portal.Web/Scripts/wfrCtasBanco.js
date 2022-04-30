String.prototype.filename=function(ex){
	 var s= this.replace(/\\/g, '/');
	 s= s.substring(s.lastIndexOf('/')+ 1);
	 return ex? s.replace(/[?#].+$/, ''): s.split('.')[0];
}



function CheckBox(event){
	//var src=$(event.target).attr('src').filename('.png');
	var _ul=$(event.target).parent().parent();
	var cs=$(event.target).attr('class');
	var cs1="";
	var csNew="_unchecked";
	var cssActual=$(event.target).parent().attr('class');
	var cssDif="";
	var result=false;

	cssActual=cssActual.split(' ');


	switch(cssActual[0]){
		case "column_Edit":
		cs1=_ul.find('li.column_Del').children('img').attr('class');
		break;
		case "column_Del":
		cs1=_ul.find('li.column_Edit').children('img').attr('class');
		break;
	}

	if(cs=="_unchecked"){
		result=true;
		csNew="_checked";
	}

	if(csNew=="_checked" && csNew==cs1)
		return false;


	$(event.target).removeClass();
	$(event.target).addClass(csNew);
	return result;
}

function Save(){
	var Cta=new Array();
	var _index=0;

	$('#container_history > li.RowNew').each(function (index) {
		_index=$(this).index();

		$(this).find('li.column_sttReg').children('img').removeClass();
		$(this).find('li.column_sttReg').children('img').addClass('_loanding');

		Cta[0]=$(this).attr('class');
		Cta[1]=$(this).find('li.column_CtaID').children('input').val();
		Cta[2]=$(this).find('li.column_Empresa').children('input').val();
		Cta[3]=$(this).find('li.column_Banco').children('select').val();
		Cta[4]=$(this).find('li.column_Cuenta').children('input').val();
		Cta[5]=$(this).find('li.column_Moneda').children('select').val();
		Cta[6]=$(this).find('li.column_Titular').children('input').val();
		Cta[7]=$(this).find('li.column_Descripcion').children('input').val();
		Cta[8]=$(this).find('li.column_Nota').children('input').val();
		Cta[9]=$(this).index();


		$.ajax({
			type: "POST",
			url: "wfrCtasBanco.aspx/Guardar",
			data: JSON.stringify({Cuenta:Cta}),
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			//async:false, 
			success: function (response) {
				var result=response.d;
				var messege=result.split('|');
				var _operationType="_ok";


				var _type=messege[0];
				var _requestIndex=messege[1];
				var _mensaje=messege[2];

				if(_type!="0"){
					_operationType="_error";

				}else{
					
					$('#container_history > li').eq(_requestIndex).removeClass();
					$('#container_history > li').eq(_requestIndex).addClass('row');
					//$('#container_history > li').eq(_requestIndex).find('li').children('input,select').removeClass();

					$('#container_history > li').eq(_requestIndex).children('ul').find('li').children('input,select').removeClass('active');
					$('#container_history > li').eq(_requestIndex).children('ul').find('li').children('input,select').addClass('inactive');
					$('#container_history > li').eq(_requestIndex).children('ul').find('li').children('input').attr('readonly',true);
					$('#container_history > li').eq(_requestIndex).children('ul').find('li').children('select').attr('disabled',true);

				}

				
				$('#container_history > li').eq(_requestIndex).find('li.column_sttReg').children('img').removeClass();
				$('#container_history > li').eq(_requestIndex).find('li.column_sttReg').children('img').addClass(_operationType);
				$('#container_history > li').eq(_requestIndex).find('li.column_sttReg').children('img').attr('title',_mensaje);


			}
		});

	});

	$('#container_history > li.RowEdit').each(function (index) {
		_index=$(this).index();

		$(this).find('li.column_sttReg').children('img').removeClass();
		$(this).find('li.column_sttReg').children('img').addClass('_loanding');

		Cta[0]=$(this).attr('class');
		Cta[1]=$(this).find('li.column_CtaID').children('input').val();
		Cta[2]=$(this).find('li.column_Empresa').children('input').val();
		Cta[3]=$(this).find('li.column_Banco').children('select').val();
		Cta[4]=$(this).find('li.column_Cuenta').children('input').val();
		Cta[5]=$(this).find('li.column_Moneda').children('select').val();
		Cta[6]=$(this).find('li.column_Titular').children('input').val();
		Cta[7]=$(this).find('li.column_Descripcion').children('input').val();
		Cta[8]=$(this).find('li.column_Nota').children('input').val();
		Cta[9]=$(this).index();


		$.ajax({
			type: "POST",
			url: "wfrCtasBanco.aspx/Guardar",
			data: JSON.stringify({Cuenta:Cta}),
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			//async:false, 
			success: function (response) {
				var result=response.d;
				var messege=result.split('|');

				var _operationType="_ok";

				var _type=messege[0];
				var _requestIndex=messege[1];
				var _mensaje=messege[2];

				if(_type!="0"){
					_operationType="_error";
				}else{

					$('#container_history > li').eq(_requestIndex).removeClass();
					$('#container_history > li').eq(_requestIndex).addClass('row');
					//$('#container_history > li').eq(_requestIndex).find('li').children('input,select').removeClass();

					$('#container_history > li').eq(_requestIndex).children('ul').find('li').children('input,select').removeClass('active');
					$('#container_history > li').eq(_requestIndex).children('ul').find('li').children('input,select').addClass('inactive');
					$('#container_history > li').eq(_requestIndex).children('ul').find('li').children('input').attr('readonly',true);
					$('#container_history > li').eq(_requestIndex).children('ul').find('li').children('select').attr('disabled',true);

				}

				
				$('#container_history > li').eq(_requestIndex).find('li.column_sttReg').children('img').removeClass();
				$('#container_history > li').eq(_requestIndex).find('li.column_sttReg').children('img').addClass(_operationType);
				$('#container_history > li').eq(_requestIndex).find('li.column_sttReg').children('img').attr('title',_mensaje);


			}
		});

	});

$('#container_history > li.RowDelete').each(function (index) {
		_index=$(this).index();

		$(this).find('li.column_sttReg').children('img').removeClass();
		$(this).find('li.column_sttReg').children('img').addClass('_loanding');

		Cta[0]=$(this).attr('class');
		Cta[1]=$(this).find('li.column_CtaID').children('input').val();
		Cta[2]=$(this).find('li.column_Empresa').children('input').val();
		Cta[3]=$(this).find('li.column_Banco').children('select').val();
		Cta[4]=$(this).find('li.column_Cuenta').children('input').val();
		Cta[5]=$(this).find('li.column_Moneda').children('select').val();
		Cta[6]=$(this).find('li.column_Titular').children('input').val();
		Cta[7]=$(this).find('li.column_Descripcion').children('input').val();
		Cta[8]=$(this).find('li.column_Nota').children('input').val();
		Cta[9]=$(this).index();


		$.ajax({
			type: "POST",
			url: "wfrCtasBanco.aspx/Guardar",
			data: JSON.stringify({Cuenta:Cta}),
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			//async:false, 
			success: function (response) {
				var result=response.d;
				var messege=result.split('|');

				var _operationType="_ok";

				var _type=messege[0];
				var _requestIndex=messege[1];
				var _mensaje=messege[2];

				if(_type!="0"){
					_operationType="_error";
				}else{
					$('#container_history > li').eq(_requestIndex).toggle();
					$('#container_history > li').eq(_requestIndex).remove();
				}

				
				$('#container_history > li').eq(_requestIndex).find('li.column_sttReg').children('img').removeClass();
				$('#container_history > li').eq(_requestIndex).find('li.column_sttReg').children('img').addClass(_operationType);
				$('#container_history > li').eq(_requestIndex).find('li.column_sttReg').children('img').attr('title',_mensaje);



			}
		});

	});




}
function AddRow (event) {
	// body...

		var html="";

		html += "<li class='RowNew' >";
		html += "<ul >";
		html += "<li class='column_STTTemp'><input value='RowNew' /></li> ";
		html += "<li class='column_CtaID'><input value='0' /></li> ";
		html += "<li class='column_ID'></li>";
		html += "<li class='column_Edit'><img class='_unchecked' onclick='Editar(event);' /></li>";
		html += "<li class='column_Del'><img class='_unchecked' onclick='Eliminar(event);' /></li>";
		html += "<li class='column_Empresa'> <input  class='active'/> </li>";
		html += "<li class='column_Banco'><select class='active'>";
		html +=GetBancos();
		html +="</select></li>";
		html += "<li class='column_Cuenta' ><input onkeypress='return justNumbers(event);'  maxlength='11' class='active'/></li>";
		html += "<li class='column_Moneda'><select class='active'>";
		html += "<option value='1' >Pesos</option>";
		html += "<option value='2' >Dolares</option>";
		html +=" </select></li>";
		html += "            <li class='column_Titular'><input class='active'/></li>";
		html += "           <li class='column_Descripcion'><input class='active'/></li>";
		html += "           <li class='column_Nota'><input class='active'/></li>";
		html += "           <li class='column_sttReg' ><img  /></li>		 ";
		html += "       </ul> ";
		html += "   </li>";


		$(html).insertAfter('#container_history > li:eq(0)');

}

function GetBancos(){
	var html="";
	$.ajax({
	    type: "POST",
	    url: "wfrCtasBanco.aspx/GetBancos",
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

function Editar(event){
	var _ul=$(event.target).parent().parent();
	var _ulClass=_ul.parent().attr('class');
	var _stt= _ul.find('li.column_STTTemp').children('input').val();

	if(CheckBox(event)){

			if(_ulClass!="RowNew"){
				_ul.parent().removeClass();
				_ul.parent().addClass('RowEdit');
				
				
			}

			_ul.find('li').children('input,select').removeClass('inactive');
			_ul.find('li').children('input,select').addClass('active');
			_ul.find('li').children('input').removeAttr('readonly');
			_ul.find('li').children('select').removeAttr('disabled');
		
	}else{

		if(_ulClass!="RowNew"){
			_ul.parent().removeClass();
			_ul.parent().addClass(_stt);

			
		}
		_ul.find('li').children('input,select').removeClass('active');
		_ul.find('li').children('input,select').addClass('inactive');
		_ul.find('li').children('input').attr('readonly',true);
		_ul.find('li').children('select').attr('disabled',true);
		
	}
	
}

function Eliminar(event){
	var _ul=$(event.target).parent().parent();
	var _ulClass=_ul.parent().attr('class');
	var _stt= _ul.find('li.column_STTTemp').children('input').val();

	if(CheckBox(event)){

			if(_ulClass!="RowNew"){
				_ul.parent().removeClass();
				_ul.parent().addClass('RowDelete');
			}
			_ul.removeClass();
			_ul.addClass('removeRow');
		
	}else{

		if(_ulClass!="RowNew"){
			_ul.parent().removeClass();
			_ul.parent().addClass(_stt);
		}
		_ul.removeClass();
		
	}
}
function Numcta(value){
	var len=value.replace('-','').length;
	var cont=1;
	var result=value;

	if((len/4)==cont){
		result=result+"-";
	}

	return result;
}