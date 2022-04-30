 $(function () {
     $('#contetn_button_slider #ocultar').attr('src', '../../iconos/back-25.png');
     $('#contetn_button_slider #ocultar').click(function () {
                // body...
         if ($('#contetn_button_slider #ocultar').attr('src') != '../../iconos/back-25.png') {
             $('#contetn_button_slider #ocultar').attr('src', '../../iconos/back-25.png');
             $('#contetn_button_slider #ocultar').attr('title', 'Ocultar Menu');
                  }else{
             $('#contetn_button_slider #ocultar').attr('src', '../../iconos/forward-25.png');
             $('#contetn_button_slider #ocultar').attr('title', 'Mostrar Menu');
                  }


                $('.content_left').animate({width:"toggle"});
            });
            

            $('li').click(function (ev) {
                $(this).find('> ul').slideToggle();
                ev.stopPropagation();
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
          
            $('#Panel_Links').css({top:parseInt($(event.target).position().top)+25,left:parseInt($(event.target).position().left)+25 });
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