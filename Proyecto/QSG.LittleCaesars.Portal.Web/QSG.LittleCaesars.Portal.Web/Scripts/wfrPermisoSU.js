function check (event) {
	var _ul=$(event.target).parent().parent();
	var cs=$(event.target).attr('class');
	var cs1="";
	var csNew="_unchecked";

	var cl=$(event.target).parent().attr('class');

	//var _oldSU=FindSUcheck(event);

	_ul.parent().removeClass();
	_ul.parent().addClass('RowNew');
	_ul.find('li.column_sttReg').children('img').removeClass();
	_ul.find('li.column_sttReg').children('img').addClass('_editing');
	_ul.removeClass();
	_ul.addClass('editRow');

	
	if(cs=="_unchecked")
		csNew="_checked";

	//console.log(cl);

	if(cl=="column_Check"){
		AllChecked(event,csNew);
	}

	$(event.target).removeClass();
	$(event.target).addClass(csNew);


	if(!AllVer(event)){
		_ul.find('li.column_sttReg').children('img').removeClass();
		_ul.find('li.column_sttReg').children('img').addClass('_lock');
		//_ul.parent().removeClass();
		//_ul.parent().addClass('RowDelete');
		_ul.removeClass();
		_ul.addClass('removeRow');
		//_ul.find('li.column_eject').children('input').val(_oldSU);

		}
	
}

function FindSUcheck(event){
	var result="";
	var _ul=$(event.target).parent().parent();
	var _indexlst=_ul.parent().index();
	var _usuarioID=_ul.find('li.column_codUser').children('input').val();

	_ul.find('li.column_SU').each(function(index){
			var _SucursalID=$(this).children('input').val();
			var _clase=$(this).children('img').attr('class');
			

			if(_clase=="_checked"){
				
				if(result!=""){result+=",";}

				result+=_SucursalID;
				//_index++;
			}

		});

	//result=_usuarioID+"|"+result+"|"+_indexlst;

	return _usuarioID+"|"+result+"|"+_indexlst;
}	


function AllChecked(event,clase){
	var _ul=$(event.target).parent().parent();
	_ul.find('li.column_SU').children('img').removeClass();
	_ul.find('li.column_SU').children('img').addClass(clase);

}
function AllVer(event){
	var _ul=$(event.target).parent().parent();
	var result=false;
	_ul.find('li.column_SU').each(function(index){
		var _clase=$(this).children('img').attr('class');

		if(_clase=="_checked")
			result=true;

	});

	return result;
}
/*
function checkHead(event){
	var ul=$(event.target).parent().parent().parent().parent();
	var src=$(event.target).attr('src');
	var src2='../iconos/check_icon2.png';

	if(src!='../iconos/check_icon2.png'){
			$(event.target).attr('src',src2);
			//src2='../iconos/uncheck_icon2.png';
		}else{
			
			src2='../iconos/uncheck_icon2.png';
			$(event.target).attr('src',src2);
		}

		ul.children('li').each(function (index){
			if(index>0){
				$(this).find('ul > li.column_check').children('img').click();
			}
		});

}
*/

function Save(){
	var _reg=new Array();
	var _SuID="";
	var _index=-1;
/*
	$('#container_history > li.RowDelete').each(function(index){
		_reg=new Array();
		_SuID="";
		_index=0;

		var _indexlst=$(this).index();
		var _css=$(this).attr('class');
		var _usuarioID="";
		var _SucursalID="";

		var _array=$(this).find('li.column_eject').children('input').val().split('|');
		console.log(_array);
		_usuarioID=_array[0];

		var _arraySU=_array[1].split(',');

		$('#container_history > li').eq(_indexlst).children('ul').children('li.column_sttReg').children('img').removeClass();
		$('#container_history > li').eq(_indexlst).children('ul').children('li.column_sttReg').children('img').addClass('_loanding');



		for(var x=0;x < _arraySU.length;x++){
			_reg[_index]=_css+"|"+_usuarioID+"|"+_SucursalID+"|"+_indexlst;
			_index++;
		}

		console.log(_reg);


		$.ajax({
			type: "POST",
			url: "wfrPermisoSU.aspx/UpPermisoSu",
			data: JSON.stringify({registro:_reg}),
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			//async:false, 
			success: function (response) {
				var result=response.d;
				var _dato=result.split('|');

				var clase="_ok";

				if(_dato[0]=="1")
					clase="_error";

				

				$('#container_history > li').eq(_indexlst).children('ul').children('li.column_sttReg').children('img').removeClass();
				$('#container_history > li').eq(_indexlst).children('ul').children('li.column_sttReg').children('img').addClass(clase);
				$('#container_history > li').eq(_indexlst).children('ul').children('li.column_sttReg').children('img').attr('title',_dato[2]);

			}
		});





	});


*/

	$('#container_history > li.RowNew').each(function(index){
		_reg=new Array();
		_SuID="";
		_index=0;

		var _indexlst=$(this).index();
		var _css=$(this).attr('class');

		$('#container_history > li').eq(_indexlst).children('ul').children('li.column_sttReg').children('img').removeClass();
		$('#container_history > li').eq(_indexlst).children('ul').children('li.column_sttReg').children('img').addClass('_loanding');

		var element=$(this).find('li.column_Usuario').children('input');

		if(element.length==0){
			console.log("entro");
			$(this).find('li.column_codUser').children('input').val($(this).find('li.column_Usuario').children('select').val());
		}


		//console.log(element);
		

		var _usuarioID=$(this).find('li.column_codUser').children('input').val();
		var _SucursalID="0";
		var _claseSTT="";

			


		//_reg[_index]=_usuarioID+"|"+_SucursalID+"|"+_indexlst;	

		$(this).find('li.column_SU').each(function(index){
			_SucursalID=$(this).children('input').val();
			_claseSTT=$(this).children('input').attr('class');
			var _clase=$(this).children('img').attr('class');
			
			console.log(_claseSTT);

			if(_clase=="_checked" && _claseSTT=="_noexiste"){
				_reg[_index]= "RowNew|"+_usuarioID+"|"+_SucursalID+"|"+_indexlst;
				_index++;
			}

			if(_clase=="_unchecked" && _claseSTT=="_existe"){
				_reg[_index]= "RowDelete|"+_usuarioID+"|"+_SucursalID+"|"+_indexlst;
				_index++;
			}
			
			

		});
		
		console.log(_reg);

					
					//console.log(_reg);

					//if(_SuID=="")
						//_usuarioID+="|";


		//_reg[_index]=_usuarioID+_SuID;	

		//console.log(_usuarioID+_SuID);

		$.ajax({
			type: "POST",
			url: "wfrPermisoSU.aspx/UpPermisoSu",
			data: JSON.stringify({registro:_reg}),
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			//async:false, 
			success: function (response) {
				var result=response.d;
				var _dato=result.split('|');

				var clase="_ok";
				var _count=0;

				if(_dato[0]=="1")
					clase="_error";





				$('#container_history > li').eq(_indexlst).children('ul').children('li.column_sttReg').children('img').removeClass();
				$('#container_history > li').eq(_indexlst).children('ul').children('li.column_sttReg').children('img').addClass(clase);
				$('#container_history > li').eq(_indexlst).children('ul').children('li.column_sttReg').children('img').attr('title',_dato[2]);

				if(_dato[0]=="0"){
					var _element=$('#container_history > li').eq(_indexlst).children('ul').children('li.column_Usuario').children('select');
					console.log(_element.children('option:selected').text());

					if(_element.length > 0){
						$('#container_history > li').eq(_indexlst).children('ul').children('li.column_Usuario').children('select').remove();
						$('#container_history > li').eq(_indexlst).children('ul').children('li.column_Usuario').append("<input value='"+_element.children('option:selected').text() +"' class='inactive'/>");
						

					}

					$('#container_history > li').eq(_indexlst).children('ul').children('li.column_SU').each(function(index){
							var _cs=$(this).children('img').attr('class');
							if(_cs=="_checked"){
								$(this).children('input').removeClass();
								$(this).children('input').addClass('_existe');
								_count++;
							}
					});



					

					var _typeRow=$('#container_history > li').eq(_indexlst).children('ul').attr('class');
					if(_typeRow=="removeRow" || _count==0 ){
						$('#container_history > li').eq(_indexlst).remove();

					//<input value='" + u.Nombre + "' title='Sucursales: " + _countUser + "/" + _countSU + "' class='inactive' /> 
					}
				}

				

			}
		});

		//_index++;
	});
}

function Getsucursales(){
	var html="";
	$.ajax({
		type: "POST",
		url: "wfrPermisoSU.aspx/GetS",
		data:"{}",// JSON.stringify({registro:_reg}),
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		async:false, 
		success: function (response) {
			html=response.d;
		}
		});
	return html;
}

function GetU(){
var obj=null;
	$.ajax({
		type: "POST",
		url: "wfrPermisoSU.aspx/GetU",
		data:"{}",// JSON.stringify({registro:_reg}),
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		async:false, 
		success: function (response) {
			obj=response.d;
		}
		});
	return obj;

}
function ValidateUser(event){
	var _valor=$(event.target).val();
	var _index=$(event.target).parent().parent().parent().index();
	var result=true;
	$("#container_history > li").each(function(index){
		if($(this).index() > 0){
			var _uID=$(this).find('li.column_codUser').children('input').val(); _uID=parseInt(_uID);

			if(_index!=$(this).index()){
				if(_valor==_uID){
					result=false;
				}
			}

			


		}
	});
	return result;
}
function changeCBX(event){
	if(!ValidateUser(event)){
		event.target.selectedIndex=0;
		alert("Este Usuario ya existe para asignar permiso.");
		return false;
	}
		
}
/*
function removeIndex(event,indexx,valor){
	$("#container_history > li").each(function(index){
		var _index=$(this).index();
		if(_index > 0 && _index !=indexx){
			console.log(_index);

			$(this).find('li.column_Usuario').children('select').each(function (index){
				
				var _valor=$(this).val();
				if(valor==_valor){
					console.log(valor);
					$(this).remove();
					//break;
				}

			});


		}
	});
}
*/

function AddRow(event){

	var _users=GetU();
	var _html="";
	var html="";
	var _code="";

	$("#container_history > li").each(function(index){
		//console.log($(this).index());
		
		if($(this).index() > 0){

		var _uID=$(this).find('li.column_codUser').children('input').val(); _uID=parseInt(_uID);
		//console.log(_uID);

			for(var x=0;x < _users.length; x++){

				if(_uID ==_users[x].CodUsuario){
					_users.splice(x,1);
					break;
				}
				
			}


		}
	
		
	});

	if(_users.length < 1)
		return false;

	for(var y=0; y < _users.length;y++){
		_html+="<option value='"+_users[y].CodUsuario +"'>"+_users[y].Nombre+"</option>";
		
		if(y==0){
			_code=_users[y].CodUsuario;

		}

		//console.log(_users[y].Nombre);
	}


	
	html += "   <li class='RowNew'>";
	html += "   <ul class='editRow'>";
	html += "   <li class='column_codUser'><input value='"+_code+"' /></li>";
	html +="    <li class='column_eject'><input /></li>"
	html += "   <li class='column_ID'></li>";
	html += "   <li class='column_Usuario'><select onchange='return changeCBX(event);' >";
	html += _html;
	html += "</select></li>";
	html += "   <li class='column_Check'><img class='_unchecked' onclick='check(event);' /> </li>";
	html +=Getsucursales();
	html += "<li class='column_sttReg'><img class='_editing'/></li>";
	html += "  </ul></li>";

	$(html).insertAfter('#container_history > li:eq(0)');
	//removeIndex(event,1,_code);
}