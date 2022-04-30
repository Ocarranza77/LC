 $(function () {

     $('#txtFechaDash').datepicker({
        onSelect:function(date){
            //alert(date);
            $('#btnClick_Rep').click();
        }
    });

      $(window).on('scroll',function(){
        var top=$(window).scrollTop();

    });

    var _widthMenu=parseInt($('#content_menu').css('width'));

   
    $('#content_button_dash').children('img').attr('src','../../iconos/forward-25.png');
    $('#content_button_dash').click(function(){
        
       if($(this).children('span').text()!="Mostrar Dashboard"){
             $(this).children('span').text("Mostrar Dashboard");
              $(this).animate({
                marginLeft:'-180px'
            });

       }else{
             $(this).children('span').text("Ocultar Dashboard");
              $(this).animate({
                marginLeft:'-35px'
             });
               $(this).children('img').attr('src','../../iconos/back-25.png');
       }


        $('#content_rep_ingresos').animate({
            marginRight:'toggle'
        },"fast");
        $('#content_buttos_dash').toggle();
    
    });

    console.log(_widthMenu);

    $("#wrapper").css('padding-left',_widthMenu);
       
        if(_widthMenu < 225){
            ///console.log("entro < 225");
           //// $('#content_rep_ingresos').css({'display':'block'});
          //   $('#content_buttos_dash').css({'display':'block'});
              $('#ocultar').attr('src', '../../iconos/forward-25.png');
     }else{
          //console.log($('#content_menu').css('width'));
     /*
         $('#content_rep_ingresos').css({'display':'none'});
         $('#content_buttos_dash').css({'display':'none'});*/
          $('#ocultar').attr('src', '../../iconos/back-25.png');
     }


   


     $('#ocultar').click(function () {

         if ($('#ocultar').attr('src') != '../../iconos/back-25.png') {
               
                $('#ocultar').attr('src', '../../iconos/back-25.png');
                $('#ocultar').attr('title', 'Ocultar Menu');
                $('#content_menu').css('width','225px');
               //$('#content_menu').width("225px");

            }else{
                $('#ocultar').attr('src','../../iconos/forward-25.png');
                $('#ocultar').attr('title', 'Mostrar Menu');
                 $('#content_menu').css('width','0px');

            }

            _widthMenu=parseInt($('#content_menu').css('width'));

            
            $("#wrapper").css('padding-left',_widthMenu);
            $('#contetn_button_slider').css('left',_widthMenu);
           // $('#contetn_button_slider').animate({left:$('#content_menu').css('width')},"slow");
            // $('#content_menu').animate({width:"toggle"},"fast");
            
            $('#content_menu').animate({width:_widthMenu},"fast");

          
            });
            

           

            $("#linksV li").mouseover(function () {
                $(this).children("img").css({ "display": "block" });
            });
            $("#linksV li").mouseout(function () {
                $(this).children("img").css({ "display": "none" });
            });


            $("#FrmLink input").show();
            $("#linksV li").children("img").css({ "display": "none" });
           // $("#FrmLink img").css({'background-image':'url("../iconos/right_round-26.png")'});
            $("#FrmLink img").click(function(){
                 $("#FrmLink input").focus();

                if (parseInt( $("#FrmLink input").width()) == 0) {
                   $("#FrmLink input").css({ "width":"250px"});
                    $("#FrmLink img").css({'background-image':'url("../iconos/left_round-26.png")'});
                } else {
                    
                    $("#FrmLink input").css({ "width": "0px" });
                    $("#FrmLink img").css({'background-image':'url("../iconos/right_round-26.png")'});
                }

            });

            $("#AddLink").children("img").click(function () {
                //$("#FrmLink input").slideToggle();
                $("#FrmLink input").focus();

                if (parseInt( $("#FrmLink input").width()) == 0) {
                    
                    $("#FrmLink input").css({ "width": "250px" });

                } else {
                    
                    $("#FrmLink input").css({ "width": "0px" });

                }
            });
            $("#FrmLink input").keypress(function (event) {

                if (event.keyCode == 13) {
                    Addlink($("#FrmLink input").val());
                    $("#FrmLink input").val("");
                }
            });


              
            


        });
 function h_over(){
    
 }

 function select(event){
    var ul=$(event.target).parent();
    ul.children('li').removeClass('selectDashboard');
    $(event.target).addClass("selectDashboard");
 }

 function MostCalendar(event){
   // $("#btnFecha").click();
   $('#txtFechaDash').datepicker("show");
}

        function SelectDia(event){
           var suID=$(event.target).val();
            $.ajax({
                    type: "POST",
                    url: "Portal.aspx/GetIngreso",
                    data: "{'sucursalID':'"+suID+"'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    //async:false, 
                    success: function (response) {
                        eval(response.d);

                    }
                });
        }

        function li_over(event) {
            // body...
            $('#content_links li').eq($(event.target).parent().index()).children('img').attr('src','../../iconos/delete.png');
            // $(event.target).children('img').attr('disabled','disabled');
        }
         function li_out(event) {
            // body...
           $('#content_links li').eq($(event.target).parent().index()).children('img').removeAttr('src');
             //$(event.target).children('img').removeAttr('disabled');
        }

        function close_addLink (event) {
            // body...
             $('#Panel_Links').toggle('slow');

        }
        function open_window(event) {
            // body...
          console.log($(event.target).position().top);
            $('#Panel_Links').css({top:parseInt($(event.target).position().top)+($(event.target).height()+50),left:parseInt($(event.target).position().left)+25 });
            $('#Panel_Links').toggle('slow');
        }
        function Slide(id) {
            $("#linksV #" + id).slideUp();
            EliminarLink(id);
        
        }
        function CambiarClase(id) {
            $("ul#menu2 li a").removeClass("CambiarColor");
            $("ul#menu2 li a #" + id).addClass("CambiarColor");
        }
        function AddVisita(parameter) {
            /*
            parameter = parameter.split(',');
            $.ajax({
                cache: false,
                timeout: 30000,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                url: "Portal.aspx/AddVisita",
                async: false,
                data: "{'coduser':'" + parameter[1].toString() + "','codAp':" + parameter[2].toString() + ",'codp':" + parameter[3].toString() + "}",
                success: function (data) {
                    location.reload(true);
                },
                error: function (response) {
                }
            });
            */

        }
        function Addlink(link) {
            $.ajax({
                cache: false,
                timeout: 30000,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                url: "Portal.aspx/AddLink",
                async: false,
                data: "{'link':'" + link + "'}",
                success: function (data) {
                    location.reload(true);
                },
                error: function (response) {
                }
            });
        }
        function EliminarLink(id) {
            $.ajax({
                cache: false,
                timeout: 30000,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                url: "Portal.aspx/Eliminar",
                async: false,
                data: "{'ID':'" + id + "'}",
                success: function (data) {
                   
                },
                error: function (response) {
                }
            });

        }
       