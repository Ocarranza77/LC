qubicApp.controller('priceListController', function ($scope, $http, $rootScope) {

    var _permisosEspeciales = {};
    var _token = "";
    var _usuario = {};
    var _baseDatos = "";
    $scope.saveAction = true;
    $scope.deleteAction = true;
    $scope.exportAction = true;
    $scope.printAction = true;


    $rootScope.$on("onInitAction", function (event, args) {
        //console.log('Init...')
        //console.log(args);
        _token = args.token;
        _usuario = args.usuario;
        _baseDatos = args.baseDatos;
        _permisosEspeciales = args.permisosEspeciales;

        $scope.saveAction = args.saveAction;
        $scope.deleteAction = args.deleteAction;
        $scope.exportAction = args.exportAction;
        $scope.printAction = args.printAction;

        onInicializate();
/*        loadAllCoins();
        loadAllOffices();
        loadAllPriceLists();*/

    });

    /*
    // No existen en esta pantalla
    $rootScope.$on("onSaveAction", function (event, args) {
        $scope.onSave();
    });


    $rootScope.$on("onClearAction", function (event, args) {
        $scope.onClear();
    });
    */

    //======================================================
    //Variables declarion
    //======================================================
    $scope.priceLists = [];
    $scope.pListSelected = {};
    $scope.monedas = [];
    $scope.selectedCoin = {};
    $scope.offices = [];
    
/*    loadAllCoins();
    loadAllOffices();
    loadAllPriceLists();
*/
    //======================================================
    //Private methods
    //======================================================
    function onInicializate() {
        var request = {};

        request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, BDName: _baseDatos, MessageOperationType: 1 // Consulta
                , CboIniTipo: 1
                , Lista: {}
            }
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/listaPrecios',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                console.log(response.data.Content);

                var cboInis = response.data.Content.CboInis;
                
                for (var i = 0; i < cboInis.length; i++) {
                    if (cboInis[i].Key == 'Moneda') {
                        for (var x = 0; x <= cboInis[i].Value.length - 1; x++)
                            cboInis[i].Value[x].MonedaID = cboInis[i].Value[x].ID;

                        $scope.monedas = cboInis[i].Value;
                    }
                    if (cboInis[i].Key == 'Oficina')
                        $scope.offices = cboInis[i].Value;
                }


                //$scope.productos = response.data.Content.Listas;
                var data = response.data.Content.Listas;
                data = setSelectedOffices(data);
                data = convertDates(data);
                $scope.priceLists = data;

                $('#lblErrorMessage').html(response.data.FriendlyMessage);
                $('#lblErrorMessage').parent().removeClass("alert-danger alert-success alert-info alert-warning").addClass("alert-success");
                $('.errorMessage').show().delay(5000).fadeOut('slow');;
            }
            if (response.data.ResponseCode == "300") {
                console.log(response.data.MessageError);
                $('#lblErrorMessage').html(response.data.FriendlyMessage);
                $('#lblErrorMessage').parent().removeClass("alert-danger alert-success alert-info alert-warning").addClass("alert-warning");
                $('.errorMessage').show().delay(5000).fadeOut('slow');;
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
                $('#lblErrorMessage').html("Error en la capa del Navegador...");
                $('#lblErrorMessage').parent().removeClass("alert-danger alert-success alert-info alert-warning").addClass("alert-danger");
                $('.errorMessage').show().delay(5000).fadeOut('slow');;

            }

        }, function errorCallback(response) {
            console.log('Error....');
            console.log(response);
            $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
            $('#lblErrorMessage').parent().removeClass("alert-danger alert-success alert-info alert-warning").addClass("alert-danger");
            $('.errorMessage').show().delay(5000).fadeOut('slow');;
        });

    };

    

    function loadAllPriceLists() {
        var request = {};

        /*request = {
            Token: "1234-1234578-454545-5545",
            RequestMessage: { CatalogName: "listas" }
        };*/
       /* request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, MessageOperationType: 1 // Consulta
                , CatalogName: "listas"
            }
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/loadCatalog',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                var data = JSON.parse(response.data.Content.CatalogItems);
                data = setSelectedOffices(data);
                data = convertDates(data);
                $scope.priceLists = data;
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
            }

        }, function errorCallback(response) {
            console.log('Error....');
        });
        */

        request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, BDName: _baseDatos, MessageOperationType: 1 // Consulta
                , Lista: {}
            }
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/listaPrecios',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
//                console.log(response.data.Content);

                var data = response.data.Content.Listas;
                data = setSelectedOffices(data);
                data = convertDates(data);
                $scope.priceLists = data;

 //               $('#lblErrorMessage').html(response.data.FriendlyMessage);
 //               $('#lblErrorMessage').parent().removeClass("alert-danger alert-success alert-info alert-warning").addClass("alert-success");
 //               $('.errorMessage').show().delay(5000).fadeOut('slow');;
            }
            if (response.data.ResponseCode == "300") {
                console.log(response.data.MessageError);
                $('#lblErrorMessage').html(response.data.FriendlyMessage);
                $('#lblErrorMessage').parent().removeClass("alert-danger alert-success alert-info alert-warning").addClass("alert-warning");
                $('.errorMessage').show().delay(5000).fadeOut('slow');;
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
                $('#lblErrorMessage').html("Error en la capa del Navegador...");
                $('#lblErrorMessage').parent().removeClass("alert-danger alert-success alert-info alert-warning").addClass("alert-danger");
                $('.errorMessage').show().delay(5000).fadeOut('slow');;

            }

        }, function errorCallback(response) {
            console.log('Error....');
            console.log(response);
            $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
            $('#lblErrorMessage').parent().removeClass("alert-danger alert-success alert-info alert-warning").addClass("alert-danger");
            $('.errorMessage').show().delay(5000).fadeOut('slow');;
        });

    }

    function loadAllCoins() {
        var request = {};

        /*request = {
            Token: "1234-1234578-454545-5545",
            RequestMessage: { CatalogName: "monedas" }
        };*/
        request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, BDName: _baseDatos, MessageOperationType: 1 // Consulta
                , CatalogName: "monedas"
            }
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/loadCatalog',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                $scope.monedas = JSON.parse(response.data.Content.CatalogItems);
                $scope.selectedCoin = $scope.monedas[0];
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
            }

        }, function errorCallback(response) {
            console.log('Error....');
            console.log(response);
        });

    }


    function loadAllOffices() {
        var request = {};

        /*request = {
            Token: "1234-1234578-454545-5545",
            RequestMessage: { CatalogName: "oficinas" }
        };*/
        request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, MessageOperationType: 1 // Consulta
                , CatalogName: "oficinas"
            }
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/loadCatalog',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                $scope.offices = JSON.parse(response.data.Content.CatalogItems);
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
            }

        }, function errorCallback(response) {
            console.log('Error....');
            console.log(response);
        });
    }

    function setCorrelatedOffices(selectedOffices) {

        for (var itm in $scope.offices) {
            $scope.offices[itm].IsSelected = false;
        }

        if (selectedOffices == 'undefined')
            return;

        if (!selectedOffices)
            return;

        for (var office in selectedOffices) {
            for (var itm in $scope.offices) {

                if ($scope.offices[itm].Nombre == selectedOffices[office].Nombre) {
                    $scope.offices[itm].IsSelected = true;
                }
            }            
        }

    }

    function setSelectedOffices(data) {
        var value = "";

        for (var itm in data) {
            value = "";
            for (of in data[itm].Oficinas) {
                value = value + data[itm].Oficinas[of].Abr + ", ";
            }
            value = value.substring(0, value.length - 2);
            data[itm].OficinasList = value;
        }

        return data;
    }

    function convertDates(data) {
        for (var itm in data) {
            data[itm].VigenciaDesde = convertJSONtoDate(data[itm].VigenciaDesde);
            data[itm].VigenciaHasta = convertJSONtoDate(data[itm].VigenciaHasta);
            //data[itm].FechaUM = data[itm].VigenciaHasta;
/*            console.log("Formato directo del servicio");
            console.log(data[itm].FechaAlta);
            console.log("Del match sencillo");
            console.log(data[itm].FechaAlta.match(/\d+/)[0]);
            console.log("Fecha con el match sencillo");
            console.log(new Date(data[itm].FechaAlta.match(/\d+/)[0] * 1));
            console.log("Nuevo metodo personalizado");
            console.log($.parseJSON(data[itm].FechaAlta, true));*/
            data[itm].FechaAlta = convertJSONtoDate(data[itm].FechaAlta);
            //console.log("Fecha UM");
            //console.log(data[itm].FechaUM);
            /*if (data[itm].FechaUM == null)
                data[itm].FechaUM = new Date(0);
            else
                data[itm].FechaUM = new Date(data[itm].FechaUM.match(/\d+/)[0] * 1); */
            data[itm].FechaUM = convertJSONtoDate(data[itm].FechaUM);
            //console.log(data[itm].FechaUM);
            
        }

        return data;
    }

    function convertDatesToJson() {
        $scope.pListSelected.VigenciaDesde = convertDateToJSON($scope.pListSelected.VigenciaDesde);
        $scope.pListSelected.VigenciaHasta = convertDateToJSON($scope.pListSelected.VigenciaHasta);
        $scope.pListSelected.FechaAlta = convertDateToJSON($scope.pListSelected.FechaAlta);
        $scope.pListSelected.FechaUM = convertDateToJSON($scope.pListSelected.FechaUM);
    }

    function collectSelectedOffices() {

        var rowsToUpdate = [];

        for (var itm in $scope.offices) {
            if ($scope.offices[itm].IsSelected) {
                rowsToUpdate.push($scope.offices[itm]);
            }
        }

        return rowsToUpdate;
    }


    //======================================================
    //Controller Events
    //======================================================
    $scope.onAddPriceList = function () {
        $scope.pListSelected = { "OperationType": 1, Moneda: {} };
        $('#mdlEditPriceList').modal('toggle');
    }

    $scope.onPListEdited = function (idx) {
        setCorrelatedOffices($scope.priceLists[idx].Oficinas)
        $scope.priceLists[idx].OperationType=2;
        $scope.pListSelected = $scope.priceLists[idx];
        $('#mdlEditPriceList').modal('toggle');
    }


    $scope.onPListDeleted = function (idx) {
        $scope.pListSelected = $scope.priceLists[idx]
        $scope.pListSelected.OperationType = 3;
        $('#mdlDeletePriceList').modal('toggle');
    }

    $scope.onDelete = function () {
        var request = {};

        /*request = {
            Token: "1234-1234578-454545-5545",
            RequestMessage: { CatalogName: "listas", CatalogItems: JSON.stringify($scope.pListSelected) }
        };*/

        /*
        request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, MessageOperationType: 2 // Save
                , CatalogName: "listas"
                , CatalogItems: JSON.stringify($scope.pListSelected)
            }
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/saveCatalog',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200" || response.data.ResponseCode == "300") {
                
                loadAllPriceLists();

                if (response.data.ResponseCode == "300") {
                    $('#lblAlertMessage').text(response.data.FriendlyMessage);
                    $('#pnlErrorMessage').show().delay(5000).fadeOut();
                }
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
            }

        }, function errorCallback(response) {
            console.log('Error....');
        });
        */

        convertDatesToJson();

        request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, BDName: _baseDatos, MessageOperationType: 2 // Save
                , Listas: [$scope.pListSelected]
            }
        };

        console.log(request);

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/listaPrecios',
            data: JSON.stringify(request),
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                $('#mdlDeletePriceList').modal('toggle');

                loadAllPriceLists();

                $('#lblErrorMessage').html(response.data.FriendlyMessage);
                $('#lblErrorMessage').parent().removeClass("alert-danger alert-success alert-info alert-warning").addClass("alert-success");
                $('.errorMessage').show().delay(5000).fadeOut('slow');;
            }
            if (response.data.ResponseCode == "300") {
                console.log(response.data.MessageError);
                $('#lblErrorMessage').html(response.data.FriendlyMessage);
                $('#lblErrorMessage').parent().removeClass("alert-danger alert-success alert-info alert-warning").addClass("alert-warning");
                $('.errorMessage').show().delay(5000).fadeOut('slow');;
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
                $('#lblErrorMessage').html("Error en la capa del Navegador...");
                $('#lblErrorMessage').parent().removeClass("alert-danger alert-success alert-info alert-warning").addClass("alert-danger");
                $('.errorMessage').show().delay(5000).fadeOut('slow');;

            }

        }, function errorCallback(response) {
            console.log('Error....');
            console.log(response);
            $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
            $('#lblErrorMessage').parent().removeClass("alert-danger alert-success alert-info alert-warning").addClass("alert-danger");
            $('.errorMessage').show().delay(5000).fadeOut('slow');;
        });

        //$('#mdlDeletePriceList').modal('toggle');
    }

    //======================================================
    //Form Events
    //======================================================

    $scope.onSave = function () {
        $scope.pListSelected.Oficinas = collectSelectedOffices();

       // var request = {};

        /*request = {
            Token: "1234-1234578-454545-5545",
            RequestMessage: { CatalogName: "listas", CatalogItems: JSON.stringify($scope.pListSelected) }
        };*/

        /*request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, MessageOperationType: 2 // Save
                , CatalogName: "listas"
                , CatalogItems: JSON.stringify($scope.pListSelected)
            }
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/saveCatalog',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200" || response.data.ResponseCode == "300") {

                loadAllPriceLists();
                $('#mdlEditPriceList').modal('toggle');
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
            }

        }, function errorCallback(response) {
            console.log('Error....');
        });
        */

        convertDatesToJson();
        console.log($scope.pListSelected);
        var request = {};
        request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, BDName: _baseDatos, MessageOperationType: 2 // Grabado
                , Listas: [$scope.pListSelected]
            }
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/listaPrecios',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                loadAllPriceLists();
                $('#mdlEditPriceList').modal('toggle');

                $('#lblErrorMessage').html(response.data.FriendlyMessage);
                $('#lblErrorMessage').parent().removeClass("alert-danger alert-success alert-info alert-warning").addClass("alert-success");
                $('.errorMessage').show().delay(5000).fadeOut('slow');;
            }
            if (response.data.ResponseCode == "300") {
                console.log(response.data.MessageError);
                $('#lblErrorMessagePop').html(response.data.FriendlyMessage);
                $('#lblErrorMessagePop').parent().removeClass("alert-danger alert-success alert-info alert-warning").addClass("alert-warning");
                $('.errorMessage').show().delay(5000).fadeOut('slow');;
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
                $('#lblErrorMessagePop').html("Error en la capa del Navegador...");
                $('#lblErrorMessagePop').parent().removeClass("alert-danger alert-success alert-info alert-warning").addClass("alert-danger");
                $('.errorMessage').show().delay(5000).fadeOut('slow');;

            }

        }, function errorCallback(response) {
            console.log('Error....');
            console.log(response);
            $('#lblErrorMessagePop').html("An error occurs while trying to communicate with the server");
            $('#lblErrorMessagePop').parent().removeClass("alert-danger alert-success alert-info alert-warning").addClass("alert-danger");
            $('.errorMessage').show().delay(5000).fadeOut('slow');;
        });

    }

});
