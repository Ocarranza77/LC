 $(function () {
        $("#home").click(function () {
           // window.location = "/Portal.aspx";
            window.open('/Portal.aspx','_parent','');
            window.close();
        });

        $("#salir").click(function () {
            //window.location = "/LogOut.aspx";
             console.log("entro");
             window.close();
             window.open('/LogOut.aspx','_parent','');
            // window.close();
            
        });

            $('.side-nav li').click(function (ev) {
                $(this).find('> ul').slideToggle();
                ev.stopPropagation();
            });

             $('.top-nav li').click(function (ev) {
                $(this).find('> ul').slideToggle();
                ev.stopPropagation();
            });



    });

 document.onkeydown = doKey;

function doKey(event) {
    if (event.keyCode == 13){
        console.log(event);
        console.log(event.target.id);
         if(event.target.id !="txtFecha" && event.target.id !="btnAdd"){ return false;}
    }
   
    
}

function money(e){
    var _symbol="$";
     var keynum = window.event ? window.event.keyCode : e.which;
     if ((keynum == 8) || (keynum == 46))
         return true;
    return /\d/.test(String.fromCharCode(keynum));
    
}
 function justNumbers(e) {
     var keynum = window.event ? window.event.keyCode : e.which;
   console.log(keynum);
     if ((keynum == 8) || (keynum == 46))
         return true;

     return /\d/.test(String.fromCharCode(keynum));
    }
     function  DownOP(event) {
            // body...
          /*  event.preventDefault();    
            $('.content_panel_options').css({top:parseInt($(event.target).position().top)+parseInt($(event.target).height())+1 ,left:parseInt($(event.target).position().left)-parseInt($(event.target).width()/2)});
          

            $('.content_panel_options').toggle();*/
        }



function IsNumeric(valor)
{
    
var log=valor.length; var sw="S";
for (x=0; x<log; x++)
{ v1=valor.substr(x,1);
v2 = parseInt(v1);
//Compruebo si es un valor numérico
if (isNaN(v2)) { sw= "N";}
}
if (sw=="S") {return true;} else {return false; }
}

var primerslap=false;
var segundoslap = false;
function formateafecha(fecha, dPasados, dFuturos) {
    var long = fecha.length;
    var dia;
    var mes;
    var ano;
    let fechaValida = true;
    let fechaHoy = new Date();


    if ((long >= 2) && (primerslap == false)) {
        dia = fecha.substr(0, 2);
        if ((IsNumeric(dia) == true) && (dia <= 31) && (dia != "00")) { fecha = fecha.substr(0, 2) + "/" + fecha.substr(3, 7); primerslap = true; }
        else { fecha = ""; primerslap = false; }
    }
    else {
        dia = fecha.substr(0, 1);
        if (IsNumeric(dia) == false)
        { fecha = ""; }
        if ((long <= 2) && (primerslap = true)) { fecha = fecha.substr(0, 1); primerslap = false; }
    }
    if ((long >= 5) && (segundoslap == false)) {
        mes = fecha.substr(3, 2);
        if ((IsNumeric(mes) == true) && (mes <= 12) && (mes != "00")) { fecha = fecha.substr(0, 5) + "/" + fecha.substr(6, 4); segundoslap = true; }
        else { fecha = fecha.substr(0, 3);; segundoslap = false; }
    }
    else { if ((long <= 5) && (segundoslap = true)) { fecha = fecha.substr(0, 4); segundoslap = false; } }
    if (long >= 7) {
        ano = fecha.substr(6, 4);
        if (IsNumeric(ano) == false) { fecha = fecha.substr(0, 6); }
        else { if (long == 10) { if ((ano == 0) || (ano < 1900) || (ano > 2100)) { fecha = fecha.substr(0, 6); } } }
    }

    if (long >= 10) {
        fecha = fecha.substr(0, 10);
        dia = fecha.substr(0, 2);
        mes = fecha.substr(3, 2);
        ano = fecha.substr(6, 4);
        // Año no viciesto y es febrero y el dia es mayor a 28
        if ((ano % 4 != 0) && (mes == 02) && (dia > 28)) { fecha = fecha.substr(0, 2) + "/"; }
    }

    if (fecha.length == 10) {


        if (dPasados != undefined) {
            if ((ano + mes + dia) < formatoYYYYMMDD(sumarDias(fechaHoy, dPasados, true))) {
                fechaValida = false;


            }
            //console.log("Fecha: " + fecha + "  FechaValidar " + sumarDias(fechaHoy, dPasados, true) + " FechaFinalr: " + formatoFechaMX(sumarDias(fechaHoy, dPasados, true)));
            console.log("Fecha: " + (ano + mes + dia));

        }
        if (dFuturos != undefined) {
            if ((ano + mes + dia) > formatoYYYYMMDD(sumarDias(fechaHoy, dFuturos)))
                fechaValida = false;
        }
    }
    if (fechaValida == false) {
        fecha = formatoFechaMX(fechaHoy);
    }
    return (fecha);
}

/* Función que suma o resta días a una fecha, si el parámetro
   días es negativo restará los días*/
function sumarDias(fecha, dias, Resta) {
    let fc = new Date();
    if (Resta != undefined)
        fc.setDate(fecha.getDate() - dias);
    else
        fc.setDate(fecha.getDate() + dias);
    console.log("FechaValidar: " + fc);
    return fc;
}
function formatoFechaMX(date) {
    //return myDate.getDate() + "/" + (myDate.getMonth() + 1) + "/" + myDate.getFullYear();
    var year = date.getFullYear();
    var month = (1 + date.getMonth()).toString(); month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString(); day = day.length > 1 ? day : '0' + day;
    console.log("Fecha Final: " + day + '/' + month + '/' + year);
    return day + '/' + month + '/' + year;

}
function formatoYYYYMMDD(date) {
    var year = date.getFullYear();
    var month = (1 + date.getMonth()).toString(); month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString(); day = day.length > 1 ? day : '0' + day;
    console.log("Fecha YYYYMMDD: " + year + month + day);
    return year + month + day;
}
//function formateafecha(fecha)
//{
//var long = fecha.length;
//var dia;
//var mes;
//var ano;

//if ((long>=2) && (primerslap==false)) { dia=fecha.substr(0,2);
//if ((IsNumeric(dia)==true) && (dia<=31) && (dia!="00")) { fecha=fecha.substr(0,2)+"/"+fecha.substr(3,7); primerslap=true; }
//else { fecha=""; primerslap=false;}
//}
//else
//{ dia=fecha.substr(0,1);
//if (IsNumeric(dia)==false)
//{fecha="";}
//if ((long<=2) && (primerslap=true)) {fecha=fecha.substr(0,1); primerslap=false; }
//}
//if ((long>=5) && (segundoslap==false))
//{ mes=fecha.substr(3,2);
//if ((IsNumeric(mes)==true) &&(mes<=12) && (mes!="00")) { fecha=fecha.substr(0,5)+"/"+fecha.substr(6,4); segundoslap=true; }
//else { fecha=fecha.substr(0,3);; segundoslap=false;}
//}
//else { if ((long<=5) && (segundoslap=true)) { fecha=fecha.substr(0,4); segundoslap=false; } }
//if (long>=7)
//{ ano=fecha.substr(6,4);
//if (IsNumeric(ano)==false) { fecha=fecha.substr(0,6); }
//else { if (long==10){ if ((ano==0) || (ano<1900) || (ano>2100)) { fecha=fecha.substr(0,6); } } }
//}

//if (long>=10)
//{
//fecha=fecha.substr(0,10);
//dia=fecha.substr(0,2);
//mes=fecha.substr(3,2);
//ano=fecha.substr(6,4);
//// Año no viciesto y es febrero y el dia es mayor a 28
//if ( (ano%4 != 0) && (mes ==02) && (dia > 28) ) { fecha=fecha.substr(0,2)+"/"; }
//}
//return (fecha);
//} 

function CrearPanel(event){
    var content=$('.dropdown');
    var _left=content.position().left;
    var _top=$(event.target).position().top;
    var _panel= $('.Panel_password');

    console.log(_panel.width());
     _panel.css({'left':(_left-_panel.width())});
     $('.Panel_password').toggle();
}

function CambiarPSW(){
    var obj=new Array();
    var url="Portal.aspx/CambiarPSW"; 
    
    if(document.getElementById('txtoldpass').value!=""){


        
        obj[0]=document.getElementById('txtoldpass').value;
        obj[1]=document.getElementById('txtnewpass').value;
        obj[2]=document.getElementById('txtconfirpass').value;

        if(obj[1] == obj[2]){

            $('.loaderDiv').toggle();
            $.ajax({
                type: "POST",
                url:url,
                data: JSON.stringify({'inf':obj}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var str=response.d;
                    var color="";
                   var m=str.split('|');

                    msg(m[2],m[0],m[1]);


                    document.getElementById('txtoldpass').value="";
                    document.getElementById('txtnewpass').value="";
                    document.getElementById('txtconfirpass').value="";

                    if(m[0] == 0){
                         $('.Panel_password').toggle();
                    }
                   
                },error:function (){
                    $('.loaderDiv').toggle();
                }
            });
        }else{
            alert("La confirmacion de contraseña es incorrecta.");
        }

    }else{

        $('#txtoldpass').css({'border-color':'red'}); 
    }
 
}

function msg(texto,tipo,modo){
    
    var color="";
    var _modo="";
    var _msg="";
    switch(parseInt(modo)){
        case -1:
        _modo="Consultado";
        break;
        case 0:
        _modo="Guardado";
        break;
        case 1:
        _modo="Actualizado";
        break;
        case 2:
        _modo="Eliminado";
        break;
        case 3:
        _modo=" Cambio de Contraseña";
        break;
    }

    if(tipo=="0"){
        color="rgba(0, 92, 44, 0.6)";//verde
        if(_modo<3){
             _msg ="Registro "+_modo+" con exito.";
        }else{
            _msg=_modo+"  con exito.";
        }
       
    }else{
        color="rgba(215, 40, 40, 0.6)";
        _msg="Ocurrio el siguiente inconveniente durante el "+_modo+"\n"+texto; 
    }

    $('.content_msg > div > span').css({'background-color':color});
    $('.content_msg > div').css({'border':'1px solid '+ color});

    $('.content_msg > div').children('textarea').text(_msg);
    $('.content_msg').toggle();
            
}

function Aceptar(event){
    $(event.target).parent().parent().toggle();
    var MsgID=$('.content_msg').children('div').children('span').children('input').val();
    $('.loaderDiv').toggle();
    $('.loader').hide();
    console.log(MsgID);
    if(MsgID!=""){
        if(parseInt(MsgID) == 0){
            $("#btnBuscar").click();
        }
    }
    
    //location.reload();
}
function passwordChanged(event) {
var _input=$(event.target);
//var strength = document.getElementById(‘strength’);
var strongRegex = new RegExp("^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$","g"); //RegExp("^(?=.{9,})(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*\W).*$", "g");
var mediumRegex = new RegExp("^(?=.{7,})(((?=.*[A-Z])(?=.*[a-z]))|((?=.*[A-Z])(?=.*[0-9]))|((?=.*[a-z])(?=.*[0-9]))).*$", "g");
var enoughRegex = new RegExp("(?=.{6,}).*", "g");
var pwd =_input; // document.getElementById("password");

if (pwd.val().length==0) {

} else if (false == enoughRegex.test(pwd.val())) {
     _input.css({'border-color':'red'});

      $('#btnPsw').attr('disabled','disabled');
    } else if (strongRegex.test(pwd.val())) {

        _input.css({'border-color':'green'});
        $('#btnPsw').removeAttr('disabled');

    } else if (mediumRegex.test(pwd.val())) {
        _input.css({'border-color':'orange'});
          $('#btnPsw').removeAttr('disabled');
    } else {

     _input.css({'border-color':'red'});
      $('#btnPsw').attr('disabled','disabled');
    }


}