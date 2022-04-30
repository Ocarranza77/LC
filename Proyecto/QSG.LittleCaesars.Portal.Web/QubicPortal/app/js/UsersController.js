
//**************************************************************************************************************************
// USERS CONTROLLER
//**************************************************************************************************************************

qubicApp.controller('usersController', function ($scope, $http, $rootScope) {

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


        loadAllUsers();
    });


    $rootScope.$on("onSaveAction", function (event, args) {
        console.log('Save...')
        $scope.onUserSaved();
    });


    $rootScope.$on("onClearAction", function (event, args) {
        console.log('Clear User...')
        $scope.selectedUser = {};
        $scope.selectedPerfil = $scope.perfiles[0];
        $scope.selectedStatus = $scope.status[0];

    });

    $scope.users = [];
    $scope.selectedUser = {};
    $scope.selectedPerfil = {};
    $scope.selectedStatus = {};

    $scope.perfiles = [{ "id": -1, "name": "- Seleccionar un Perfil -" }, { "id": 1, "name": "Administrador" }, {"id":2, "name":"Usuario"}];
    $scope.status = [{ "id": -1, "name": "- Seleccionar un status -" }, { "id": 1, "name": "Activo" }, { "id": 0, "name": "Inactivo" }];

    $scope.selectedPerfil = $scope.perfiles[0];
    $scope.selectedStatus = $scope.status[0];

    //loadAllUsers();

    function loadAllUsers() {
        var request = {};

        request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, MessageOperationType: 3
                , Usuario: { ClienteQ: { ClienteID: _usuario.ClienteQ.ClienteID } }
            }
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/users',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                $scope.users = response.data.Content.Usuarios;
                console.log($scope.users);
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
            }

        }, function errorCallback(response) {
            console.log('Error....');
            $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
            $('.errorMessage').show().delay(2000).fadeOut('slow');;
        });

    }

    $scope.onUserEdited = function (id) {

        for (var idx in $scope.users) {
            if ($scope.users[idx].UsuarioID == id) {
                $scope.selectedUser = $scope.users[idx];

                $scope.selectedPerfil = $scope.perfiles[$scope.selectedUser.Tipo];
                if($scope.selectedUser.Activo)
                    $scope.selectedStatus = $scope.status[1];
                else
                    $scope.selectedStatus = $scope.status[2];
                break;
            }
        }

        $('#pnlUserModal').modal();
    }

    $scope.onUserDeleted = function (id) {


        for (var idx in $scope.users) {
            if ($scope.users[idx].UsuarioID == id) {
                $scope.selectedUser = $scope.users[idx];

                //$scope.selectedPerfil = $scope.perfiles[$scope.selectedUser.Perfil];
                //$scope.selectedStatus = $scope.status[$scope.selectedUser.Status];
                break;
            }
        }

        $scope.selectedUser.Activo = false;
        $scope.selectedUser.OperationType = 3; //  Eliminar

        request = {
            Token: _token,
            RequestMessage: {
                 UserIDRqst: _usuario.UsuarioID, MessageOperationType: 2 // Grabado
                , Usuario: $scope.selectedUser
             }
        };

        /*var request = {
            Token: _token,
            RequestMessage: { "Usuario": $scope.selectedUser }
        };*/


        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/deleteuser',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {

            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                loadAllUsers();
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
            }

        }, function errorCallback(response) {
            console.log('Error....');
            $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
            $('.errorMessage').show().delay(2000).fadeOut('slow');;
        });



    }

    $scope.onUserSaved = function () {

        $scope.selectedUser.Tipo = $scope.selectedPerfil.id;
        if ($scope.selectedStatus.id = 1 )
            $scope.selectedUser.Activo = true;
        else
            $scope.selectedUser.Activo = false;

        if ($scope.selectedUser.UsuarioID == undefined || $scope.selectedUser.UsuarioID == 0) {
            $scope.selectedUser.ClienteQ = { ClienteID: _usuario.ClienteQ.ClienteID };
            $scope.selectedUser.OperationType = 1;
        }
        else
            $scope.selectedUser.OperationType = 2;

        console.log('Save user: ');
        console.log($scope.selectedUser);

        request = {
            Token: _token,
            RequestMessage: //{ "Usuario": $scope.selectedUser }
             {
                UserIDRqst: _usuario.UsuarioID, MessageOperationType: 2
                , Usuario: $scope.selectedUser
            }
        };


        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/user',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {

            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                $('#pnlUserModal').modal('toggle');
                loadAllUsers();
                $scope.selectedUser = {};
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
            }

        }, function errorCallback(response) {
            console.log('Error....');
            $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
            $('.errorMessage').show().delay(2000).fadeOut('slow');;
        });

    }


    $scope.onNewUser = function () {
        $scope.selectedUser = {};

        $scope.selectedPerfil = $scope.perfiles[0];
        $scope.selectedStatus = $scope.status[0];

        $('#pnlUserModal').modal();

    }


});



//**************************************************************************************************************************
// USERS CONTROLLER
//**************************************************************************************************************************

qubicApp.controller('pwdController', function ($scope, $http, $rootScope) {

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
        //loadAllUsers();
    });


    $rootScope.$on("onSaveAction", function (event, args) {
        console.log('Save...')
        $scope.onSaveProfile();
    });

    $rootScope.$on("onClearAction", function (event, args) {
        console.log('Clearing pwd...')
        console.log(args);
        $scope.onClear();
    });

    $rootScope.$on("onSearchAction", function (event, args) {
        console.log('Search User...')
        $scope.onSearch();
    });

    $scope.users = [];
    $scope.selectedUser = {};
    $scope.selectedPerfil = {};
    $scope.resetSpecs = {};
    $scope.filtro = "Buscar por...";
    $scope.perfiles = [{ "id": 0, "name": "- Seleccionar un Perfil -" }, { "id": 1, "name": "Administrador" }, { "id": 2, "name": "Usuario" }];
    $scope.contrasenaTipo = 'none';

    $scope.selectedPerfil = $scope.perfiles[0];

//    loadAllUsers();

    function loadAllUsers() {
        var request = {};

        request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, MessageOperationType: 3
                , Usuario: { ClienteQ: { ClienteID: _usuario.ClienteQ.ClienteID } }
            }
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/users',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            console.log(response);

            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                console.log('call success...');
                console.log(response.data.Content);
                $scope.users = response.data.Content.Usuarios;
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
            }

        }, function errorCallback(response) {
            console.log('Error....');
            $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
            $('.errorMessage').show().delay(2000).fadeOut('slow');;
        });

    }

    //OnSearch
    $scope.onSearch = function () {
        var request = {};
        $('#pnlReset').hide();

        $scope.selectedUser.ClienteQ = _usuario.ClienteQ;

        request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, MessageOperationType: 3
                , Usuario:  $scope.selectedUser
            }
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/searchusers',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                $scope.users = response.data.Content.Usuarios;
                $('#pnlResultSearch').show();
                if ($scope.users.lenght > 0) {
                    $('#cmdSearch').hide();
                }
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
            }
        }, function errorCallback(response) {
            console.log('Error....');
            $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
            $('.errorMessage').show().delay(2000).fadeOut('slow');;
        });

    }

    //OnUserSelected
    $scope.onUserSelected = function (id) {
        for (var idx in $scope.users) {
            if ($scope.users[idx].UsuarioID == id) {
                $scope.selectedUser = $scope.users[idx];

                $scope.selectedPerfil = $scope.perfiles[$scope.selectedUser.Tipo];
                $scope.selectedUser.ContrasenaXAdmin = false;
                $scope.selectedUser.ContrasenaXSistema = false;
                break;
            }
        }

        $('#cmdSearch').hide();
        $('#pnlResultSearch').hide();
        $('#pnlReset').show();
        $scope.filtro = "Usuario Seleccionado.";
    }

    //OnSaveProfile
    $scope.onSaveProfile = function () {
        var request = {};


        if ($scope.selectedUser.Bloqueo == false && $scope.selectedUser.UsPwd != $scope.selectedUser.Confirmacion) {
            $('#lblErrorMessage').html("El Password y la Confirmacion no coinciden");
            $('.errorMessage').show().delay(5000).fadeOut('slow');;
  
            return;
        }

        if ($scope.selectedUser.Bloqueo == false) {
            if ($scope.contrasenaTipo == 'XAdmin')
                $scope.selectedUser.ContrasenaXAdmin = true;
            if ($scope.contrasenaTipo == 'XSistema')
                $scope.selectedUser.ContrasenaXSistema = true;
        }

        $scope.selectedUser.OperationType = 2;
        request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, MessageOperationType: 2
                , "Usuario": $scope.selectedUser
            }
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/resetpassword',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                console.log('Sucess when password was reset...');
                $scope.onClear();
                alert("Informacion Actualizada.");
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
            }
        }, function errorCallback(response) {
            console.log('Error....');
            $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
            $('.errorMessage').show().delay(2000).fadeOut('slow');;
        });


    }

    $scope.onClear = function () {
        $scope.selectedUser = {};
        $scope.selectedPerfil = $scope.perfiles[0];
        $('#cmdSearch').show();
        $('#pnlResultSearch').hide();
        $('#pnlReset').hide();
        $scope.filtro = "Buscar por...";
        $scope.contrasenaTipo = 'none';
    }


});


//**************************************************************************************************************************
// SECURITY CONTROLLER
//**************************************************************************************************************************
qubicApp.controller('securityController', function ($scope, $http, $rootScope) {

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


        loadAllUsers();
    });


    //ACTION BAR BUTTONS....
    $rootScope.$on("onSaveAction", function (event, args) {
        console.log('Save...')
        $scope.onSavePermisos();
    });

    $rootScope.$on("onClearAction", function () {
        $scope.onClear();
    });

    $scope.users = [];
    $scope.selectedUser = {};
    $scope.selectedPerfil = {};

    $scope.filters = {};
    $scope.selectedCompany = {};
    $scope.selectedApplication = {};
    $scope.securityInfo = [];

    $scope.selectedRow = {};

    $scope.ShowUserProfile = false;
    $scope.perfiles = [{ "id": 0, "name": "- Seleccionar un Perfil -" }, { "id": 1, "name": "Administrador" }, { "id": 2, "name": "Usuario" }];

    $scope.selectedPerfil = $scope.perfiles[0];

    //loadAllUsers();

    function loadAllUsers() {
        var request = {};

        request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, MessageOperationType: 3
            } //, Usuario: { ClienteQ: { ClienteID: _usuario.ClienteQ.ClienteID }
                   };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/users',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            console.log(response);

            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                console.log('call success...');
                console.log(response.data.Content);
                $scope.users = response.data.Content.Usuarios;
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
            }

        }, function errorCallback(response) {
            console.log('Error....');
            $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
            $('.errorMessage').show().delay(2000).fadeOut('slow');;
        });

    }


    //OnSearch
    $scope.onSearch = function () {
        var request = {};
        $('#pnlReset').hide();

        request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, MessageOperationType: 3,
                Usuario: $scope.selectedUser
            }
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/searchusers',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                $scope.users = response.data.Content.Usuarios;
                $('#pnlResultUsersSearch').show();
                if ($scope.users.lenght > 0) {
                    $('#cmdSearchUsers').hide();
                }
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
            }
        }, function errorCallback(response) {
            console.log('Error....');
            $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
            $('.errorMessage').show().delay(2000).fadeOut('slow');;
        });

    }

    $scope.showRow = function (companyid, applicationid, idxRow) {
        //console.log("Company Id: " + companyid + ", application id: " + applicationid);
        //filters.ShowUserProfile
        //$scope.showRowProfile

        var result = false;

        if ($scope.selectedCompany.Id == -1 && $scope.selectedApplication.Id == -1) {
            result = true;
        }
        else if ($scope.selectedCompany.Id == -1 && $scope.selectedApplication.Id != -1) {
            if ($scope.selectedApplication.Id == applicationid) {
                result = true;
            }
            else {
                result = false;
            }
        }
        else if ($scope.selectedCompany.Id != -1 && $scope.selectedApplication.Id == -1) {
            if ($scope.selectedCompany.Id == companyid) {
                result = true;
            }
            else {
                result = false;
            }
        }
        else if ($scope.selectedCompany.Id != -1 && $scope.selectedApplication.Id != -1) {
            if ($scope.selectedCompany.Id == companyid && $scope.selectedApplication.Id == applicationid) {
                result = true;
            }
            else {
                result = false;
            }
        }

        console.log('check for user access: [' + $scope.ShowUserProfile + "],  " + result);
        if ($scope.ShowUserProfile && result) {
            return $scope.showRowProfile(idxRow);
        }
        else {
            return result;
        }



        //if ($scope.selectedCompany.Id == companyid) {// && $scope.selectedApplication.Id == applicationid) {
        //    return true;
        //}
        //else {
        //    return false;
        //}

    }

    $scope.showRowProfile = function (idxRow) {

        if ($scope.securityInfo[idxRow].Read || $scope.securityInfo[idxRow].Write || $scope.securityInfo[idxRow].Print ||
            $scope.securityInfo[idxRow].Delete || $scope.securityInfo[idxRow].Export) {
            return true;
        }
        else {
            return false;
        }

    }


    //OnUserSelected
    $scope.onUserSelected = function (id) {
        for (var idx in $scope.users) {
            if ($scope.users[idx].Codigo == id) {
                $scope.selectedUser = $scope.users[idx];

                $scope.selectedPerfil = $scope.perfiles[$scope.selectedUser.Perfil];
                break;
            }
        }

        $('#cmdSearchUsers').hide();
        $('#pnlResultUsersSearch').hide();
        $('#pnlFiltros').show();
        $('#pnlFacultades').show();

        request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, MessageOperationType: 3, //Report
                UserIndentificator: $scope.selectedUser.Codigo
            }
        };


        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/getsecurityfilters',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                console.log('get security filter sucess...');

                $scope.filters = response.data.Content.Filters;
                $scope.selectedCompany = response.data.Content.Filters.Companies[0];
                $scope.selectedApplication = response.data.Content.Filters.Applications[0];
                //$scope.onSelectedApplication();
                loadSecurityInfo();
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
            }
        }, function errorCallback(response) {
            console.log('Error....');
            $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
            $('.errorMessage').show().delay(2000).fadeOut('slow');;
        });

    }

    function loadSecurityInfo() {
        console.log('loading security Info...');

        var request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, MessageOperationType: 1, //Query
                //"UserIdentificator": $scope.selectedUser.Codigo,
                "CompanyId": $scope.selectedCompany.Id,
                "ApplicationId": $scope.selectedApplication.Id
            }
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/getsecurityinfo',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                console.log('security info sucess...');
                console.log(response.data.Content.SecurityInfo);
                console.log(JSON.parse(response.data.Content.SecurityInfo));
                $scope.securityInfo = JSON.parse(response.data.Content.SecurityInfo);
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
            }
        }, function errorCallback(response) {
            console.log('Error....');
            $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
            $('.errorMessage').show().delay(2000).fadeOut('slow');;
        });

    }

    $scope.onSpecialAccess = function (idx) {
        console.log('Idx Selected: ' + idx);
        $scope.selectedRow = $scope.securityInfo[idx];

        console.log('---------------');
        console.log($scope.selectedRow);
        console.log('---------------');

        $('#pnlSpecialAccess').modal();
    }

    $scope.onAcceptAccess = function () {

        var result = "";

        for (var idx in $scope.selectedRow.Others) {
            if ($scope.selectedRow.Others[idx].Granted)
                result += $scope.selectedRow.Others[idx].Id + ", ";
        }

        result = result.substring(0, result.length - 2);

        $scope.selectedRow.OthersValue = result;

        //formService.setErrorMessage('This is a sample message of an error triggered from security form...');

        $('#pnlSpecialAccess').modal('toggle');;
    }

    //$scope.onSelectedCompany = function () {
    //    $scope.selectedApplication = $scope.selectedCompany.Applications[0];
    //    //console.log("Application selected:");
    //    //console.log($scope.selectedApplication);

    //    $scope.onSelectedApplication();
    //}

    //$scope.onSelectedApplication = function () {
    //    console.log('Application selected...');
    //    loadSecurityInfo();
    //}


    $scope.onClear = function () {
        $scope.selectedUser = {};
        $scope.selectedPerfil = $scope.perfiles[0];
        $('#cmdSearchUsers').show();
        $('#pnlResultUsersSearch').hide();
        $('#pnlFiltros').hide();
        $('#pnlFacultades').hide();

    }

    $scope.onSavePermisos = function () {
        $scope.onClear();
    }

});


//Save??


