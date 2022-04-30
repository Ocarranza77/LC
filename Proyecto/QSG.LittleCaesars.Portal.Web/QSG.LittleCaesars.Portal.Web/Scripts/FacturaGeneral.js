/// <reference path="../Acuse.html" />
var _decimales=2;
var _separadorD=".";
var _separadorM=",";
$(function (argument) {
	// body...
//$('.loader1').toggle();



/*

var te="$10,555.40";
te.replace(/[A-Za-z$-]/g,'');
var r=te.replace(/[A-Za-z$-]/g,'');
var r1=r.replace(",","");

alert(parseFloat(r1) );*/
//alert( "$"+formatoNumero("$1253325.56",2,".",","));


});

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

function Facturar(event){
	event.preventDefault();
	var _parent=$(event.target).parent().parent();
	var _index_Li=_parent.parent().index();
	var _id_Row=_parent.attr('id');

	var _ULReg=$('#container_history > li').eq(_index_Li).children('ul');


	var _importe=_ULReg.children('li .column_Xfact').text();
	
	
	if(parseFloat(quitarLetra(_importe)) == 0 || parseFloat(quitarLetra(_importe)) < 0 || isNaN(parseFloat(quitarLetra(_importe)))){return false;}

	if(confirm('¿Esta seguro de FACTURAR como Publico en General?')){
			var ObjectTicket=new Array();
			
			

			ObjectTicket[0]=_ULReg.children('li .column_FolioFact').children('input').val();/*folio fac id*/
			ObjectTicket[1]=_ULReg.children('li .column_FolioSU').children('input').val();/*folio ticket id*/
			ObjectTicket[2]=_ULReg.children('li .column_SucursalID').children('input').val();/*sucursal id*/
			ObjectTicket[3]=parseFloat(quitarLetra(_importe)); /*importe*/
			ObjectTicket[4]=$('#txtFConsumo').val();/*fecha consumo venta*/
			ObjectTicket[5] = $('#SelectUsers').val();
			ObjectTicket[6] = $('#SelectFechaFactura').val(); /* option:selected').text();*/

			$('.loader1').toggle();

			$.ajax({
				type: "POST",
				url: "FacturacionGlobal.aspx/FacturarPG",
				data: JSON.stringify({TicketT:ObjectTicket}),
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (response) {
					if(response.d==""){
						/**/
						var facturado=_ULReg.children('li .column_Facturado').text();
					
						var result=parseFloat(quitarLetra(_importe))+parseFloat(quitarLetra(facturado)); 

						_ULReg.children('li .column_Facturado').text("$ "+formatoNumero(result,_decimales,_separadorD,_separadorM));
						_ULReg.children('li .column_Xfact').text("$ 0.00");
						/*$(event.target).removeClass('ButtonCancelar');
						$(event.target).addClass('ButtonNormal');
						$(event.target).val("Facturado");
						$(event.target).attr("color","black");*/
						$(event.target).attr('src','');
						$(event.target).attr('src','../../iconos/check_icon2.png');
						$(event.target).attr('title','Facturado');

						CalSumatorias(event);

						$('.loader1').toggle();

						//column_Facturado
					}else{
						alert("Error "+response.d);
						$('.loader1').toggle();
					}
					
				},error:function (error){
					alert('Error '+eval(error));
					$('.loader1').toggle();

				}
			});

	}else{return false;}
	//$('.loader1').toggle();

}
function CalSumatorias(event){
	var last=$('#container_history > li').length-1;
	var _totXfact=0;
	var _totFact=0;
	
	$('#container_history > li').each(function (index){

		if(index > 0 && index < last){

			var XFact=$(this).children('ul').children('li .column_Xfact').text();
			var Fact=$(this).children('ul').children('li .column_Facturado').text();
			if(isNaN(parseFloat(quitarLetra(XFact)))){XFact=0; }else{ XFact=parseFloat(quitarLetra(XFact));}
			if(isNaN(parseFloat(quitarLetra(Fact)))){Fact=0;}else{Fact=parseFloat(quitarLetra(Fact));}
			

			_totXfact =_totXfact + XFact;
			_totFact =_totFact + Fact;

		}
	});

	$('#container_history > li').eq(last).children('ul').children('li .column_Xfact').children('input').val("$ "+ formatoNumero(_totXfact,_decimales,_separadorD,_separadorM ));
	$('#container_history > li').eq(last).children('ul').children('li .column_Facturado').children('input').val( "$ "+ formatoNumero(_totFact ,_decimales,_separadorD,_separadorM ));

}

function quitarLetra(monto){
	if(monto==""){return "";}
	monto=monto.replace(/[A-Za-z$-]/g,'');
	monto=monto.replace(',','');
	return monto;
}