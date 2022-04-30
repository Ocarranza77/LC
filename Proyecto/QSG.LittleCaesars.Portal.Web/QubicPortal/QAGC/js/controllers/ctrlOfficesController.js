qubicApp.controller('officesController', function ($scope, $http, $rootScope) {

    var _permisosEspeciales = {};
    var _token = "";
    var _usuario = {};
    var _baseDatos = "";
    $scope.saveAction = true;
    $scope.deleteAction = true;
    $scope.exportAction = true;
    $scope.printAction = true;


    $rootScope.$on("onInitAction", function (event, args) {
        console.log('Init...')
        console.log(args);
        _token = args.token;
        _usuario = args.usuario;
        _baseDatos = args.baseDatos;
        _permisosEspeciales = args.permisosEspeciales;

        $scope.saveAction = args.saveAction;
        $scope.deleteAction = args.deleteAction;
        $scope.exportAction = args.exportAction;
        $scope.printAction = args.printAction;

        loadAllOffices();
    });


    $rootScope.$on("onSaveAction", function (event, args) {
        $scope.onSave();
    });


    $rootScope.$on("onClearAction", function (event, args) {
        $scope.onClear();
    });

    //======================================================
    //Variables declarion
    //======================================================
    $scope.offices = [];
    $scope.officesAdded = [];
    var deleteItem = {};

    //loadAllOffices();

    //======================================================
    //Private methods
    //======================================================
    function loadAllOffices() {
        var request = {};

        request = {
            //Token: "1234-1234578-454545-5545",
            //RequestMessage: { CatalogName: "Sucursales" }
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, BDName: _baseDatos, MessageOperationType: 3 // Reporte
                , ctType: 5 // Oficinas
                , CatalogoTipo: {} //, CatalogName: "oficinas"
            } //Sucursales
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/catalogoTipos',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                console.log(response.data.Content.CatalogoTipos);
                $scope.offices = response.data.Content.CatalogoTipos; //JSON.parse(response.data.Content.CatalogItems);
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
            }

        }, function errorCallback(response) {
            console.log('Error....');
        });

    }

    function collectRowsToUpdate() {

        var rowsToUpdate = [];

        for (var itm in $scope.offices) {
            if ($scope.offices[itm].OperationType != 0) {
                rowsToUpdate.push($scope.offices[itm]);
            }
        }

        for (var itm in $scope.officesAdded) {
            rowsToUpdate.push($scope.officesAdded[itm]);
        }

        return rowsToUpdate;
    }


    //======================================================
    //Controller Events
    //======================================================
    $scope.onAddOffice = function () {
        $scope.officesAdded.push({ "ID": 0, "Nombre": "", "Abr": "", "OperationType": 1 });
    }

    $scope.onOfficeEdited = function (event, idx) {
        var rowSelected = $(event.currentTarget).parent().parent();
        $('.readField', rowSelected).hide();
        $('.editField', rowSelected).show();
        $scope.offices[idx].OperationType = 2;
    }


    $scope.onOfficeDeleted = function (event, idx) {

        $('#mdlDeleteOffice').modal();

        deleteItem = {
            "target": event.currentTarget,
            "idx": idx
        };
    }

    $scope.onDelete = function () {
        //$(event.currentTarget).parent().parent().addClass("danger");
        //$scope.offices[idx].Operation = "Deleted";
        $('#mdlDeleteOffice').modal('toggle');
        $(deleteItem.target).parent().parent().addClass("danger");
        $scope.offices[deleteItem.idx].OperationType = 3;
    }

    //======================================================
    //Form Events
    //======================================================
    /*$rootScope.$on("onSaveAction", function (event, args) {
        $scope.onSave();
    });*/

    $scope.onSave = function () {
        var data = collectRowsToUpdate();

        if (data.length <= 0) {
            return;
        }

        var request = {};

        /*request = {
            Token: "1234-1234578-454545-5545",
            RequestMessage: { CatalogName: "Sucursales", CatalogItems: JSON.stringify(data) }
        };*/
        /*request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, MessageOperationType: 2
                , CatalogName: "oficinas"//"Sucursales" //
                , CatalogItems: JSON.stringify(data)
            }
        };*/
        request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, BDName: _baseDatos, MessageOperationType: 2
                , ctType: 5 // Oficinas
                , CatalogoTipos: data 
            }
        };


        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/catalogoTipos',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200" || response.data.ResponseCode == "300") {
                $scope.onClear();
                console.log(response.data.ResponseCode);

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

    }

    /*$rootScope.$on("onClearAction", function (event, args) {
        $scope.onClear();
    }); */

    $scope.onClear = function () {
        $scope.offices = [];
        $scope.officesAdded = [];
        loadAllOffices();
    }



});
