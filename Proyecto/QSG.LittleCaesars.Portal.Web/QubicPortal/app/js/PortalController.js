// JavaScript source code

var qubicApp = angular.module('qubicApp', []);
var urlROOT = "";

//**************************************************************************************************************************
// ACCOUNTS CONTROLLER
//**************************************************************************************************************************
qubicApp.controller('accountController', function ($scope, $http, $window) {

    var _token = "";

    $scope.cleanSession = false;
    $scope.notifications = [];
    $scope.userLogged = {};
    $scope.menuOptions = [];
    $scope.selectedCompany = {};
    $scope.brokerSelected = {};
    $scope.AuthenticateRequest = {
        "Email": "",
        "Password": "",
        "PIN": ""
    };
    $scope.companies = [];

    loadInitValues();
    loadingQueryStringValues();

    function loadingQueryStringValues() {
        var urlParams;
        (window.onpopstate = function () {
            var match,
                pl = /\+/g,  // Regex for replacing addition symbol with a space
                search = /([^&=]+)=?([^&]*)/g,
                decode = function (s) { return decodeURIComponent(s.replace(pl, " ")); },
                query = window.location.search.substring(1);

            urlParams = {};
            while (match = search.exec(query))
                urlParams[decode(match[1])] = decode(match[2]);
        })();

        if (!urlParams.rq)
            return;


        var values = urlParams.rq.toString().split('_');
        _token = values[0];
        //console.log(_token);
        loadCompanies(_token, values[1]);

        function loadCompanies(sid, userid) {
            //console.log(userid);

            request = {
                Token: sid,
                RequestMessage: { UsuarioID: userid, GetEmpresas: 1, GetAutorize: 1 }
            };

            $http({
                method: 'POST',
                url: urlROOT + '/QubicService.svc/Authorize',
                data: request,
                headers: { 'Content-Type': 'application/json' }
            }).then(function successCallback(response) {
                if (response.data.ResponseCode == "403") {
                    //console.log("token no valido...");
                    window.location.href = "Login.html";
                }
                if (response.data.ResponseCode == "200") {

                    $scope.cleanSession = true;
                    $scope.selectedCompany = response.data.Content.Empresas[0];

                    if (response.data.Content.Empresas.length > 1) {
                        for (var x = 1; x < response.data.Content.Empresas.length; x++) {
                            $scope.companies.push(response.data.Content.Empresas[x]);
                        }
                    }

                    $scope.menuOptions = response.data.Content.Apps;
                    loadCSS('css/companies/' + $scope.selectedCompany.NombreBD + '.css');
                    $scope.loggedUser = response.data.Content.Usuarios[0];
                    $scope.notifications = response.data.Content.Notificaciones;
                }


            }, function errorCallback(response) {
                $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
                $('.errorMessage').show().delay(2000).fadeOut('slow');;
            });

        }

    }

    loadCSS = function (href) {
        var cssLink = $("<link id='cssXCompany' rel='stylesheet' type='text/css' href='" + href + "'>");
        $("head").append(cssLink);
    };

    removeCSS = function () {
        $("#cssXCompany").remove();
    };


    $scope.getUrl = function (action, menuType) {
        //console.log('Action:' + action + " - " + menuType);
        //console.log('Company Selected: ' + $scope.selectedCompany.Id);

        if (action == null)
            return "javascript:;";
        else
            return "qubicform.html?rq=" + _token + "_" + $scope.selectedCompany.EmpresaID + "_" + action;
    }

    $scope.sendToUrl = function (action, menuType) {
        if (menuType == 1) {
            //console.log('sending to...' + action);
            window.open("qubicform.html?rq=" + _token + "_" + $scope.selectedCompany.EmpresaID + "_" + action, '_blank');
        }
    }


    function loadInitValues() {
        var url = urlROOT + '/QubicService.svc/Initialize';

        $http({ method: 'GET', url: url }).
            then(function successCallback(response) {
                $scope.InitConfiguration = {};
                $scope.InitConfiguration.Version = response.data.Content.Version;
                $scope.InitConfiguration.HidePIN = response.data.Content.HidePIN;

            }, function errorCallback(response) {
                console.log('Faild where try to get Parent skills from server');
            });


    }

    $scope.onAuthenticate = function () {
        var request = {
            RequestMessage: $scope.AuthenticateRequest
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/Authenticate',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                $('#lblErrorMessage').html(response.data.FriendlyMessage);
                $('.errorMessage').show().delay(2000).fadeOut('slow');;
            }
            if (response.data.ResponseCode == "200") {
                this._token = response.data.Content.Token;
                if (response.data.Content.Brokers.length == 1) {
                    window.location.href = "Qubicportal.html?rq=" + response.data.Content.Token + "_" + response.data.Content.Brokers[0].UserIdentificator;
                }
                else {
                    response.data.Content.Brokers.splice(0, 0, { ID: 0, "Name": "Seleccionar un cliente - Select a broker" });;
                    $scope.Brokers = response.data.Content.Brokers;
                    $scope.brokerSelected = $scope.Brokers[0];
                    $('#pnlBrokers').show();
                }

            }

        }, function errorCallback(response) {
            $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
            $('.errorMessage').show().delay(2000).fadeOut('slow');;
        });

    }

    $scope.onBrokerSelected = function () {
        window.location.href = "Qubicportal.html?rq=" + brokerSelected.UserIdentificator;
    }

    $scope.onCompanySelected = function (id) {
        $scope.companies.unshift($scope.selectedCompany);

        for (var idx in $scope.companies) {
            if ($scope.companies[idx].Id == id) {
                $scope.selectedCompany = $scope.companies[idx];
                break;
            }
        }

        if (idx > 0) {
            $scope.companies.splice(idx, 1);
        }

        removeCSS();
        loadCSS($scope.selectedCompany.CssFile);

        var request = {
            Token: _token,
            RequestMessage: { UsuarioID: userid, GetAutorize: 1, EmpresaID: id }
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/GetMenuForCompany',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                //console.log("token no valido...");
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {

                $scope.menuOptions = response.data.Content.Apps;
                loadCSS($scope.selectedCompany.CssFile);
                //$scope.loggedUser = response.data.Content.LoggedUser;
            }


        }, function errorCallback(response) {
            $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
            $('.errorMessage').show().delay(2000).fadeOut('slow');;
        });

    }

    var toggleSideBar = false;

    $scope.onToggleSideBar = function () {

        if (toggleSideBar) {
            toggleSideBar = false;
            $('.side-nav').show();
            $('#cmdCloseMenu').children().removeClass('glyphicon-menu-right').addClass('glyphicon-menu-left');
            $('#cmdCloseMenu').css('margin-left', '0px');
        }
        else {
            toggleSideBar = true;
            $('.side-nav').hide();
            $('#cmdCloseMenu').children().removeClass('glyphicon-menu-left').addClass('glyphicon-menu-right');
            $('#cmdCloseMenu').css('margin-left', '-230px');

        }

    }


    var toogleDashboard = true;

    $scope.onToggleDashboard = function () {

        if (toogleDashboard) {
            toogleDashboard = false;
            $('#pnlDashboard').show();
            $('#cmdShowDashboard').children().next().show();
            $('#cmdShowDashboard').css('width', 200);
            $('#cmdShowDashboard').children('.glyphicon').removeClass('glyphicon-menu-left').addClass('glyphicon-menu-right');

        }
        else {
            toogleDashboard = true;
            $('#pnlDashboard').hide();
            $('#cmdShowDashboard').css('width', 50);
            $('#cmdShowDashboard').children('.glyphicon').removeClass('glyphicon-menu-right').addClass('glyphicon-menu-left');
            $('#cmdShowDashboard').children().next().hide();

        }

    }

    $scope.onCloseSession = function () {

        var url = urlROOT + '/QubicService.svc/CloseSession';

        $http({ method: 'GET', url: url }).
            then(function successCallback(response) {
                window.location.href = "Login.html";
            }, function errorCallback(response) {
                console.log('Faild where try to get Parent skills from server');
            });
    }

    $scope.onExit = function () {
        var url = urlROOT + '/QubicService.svc/CloseSession';

        $http({ method: 'GET', url: url }).
            then(function successCallback(response) {
                window.location.href = "Login.html";
            }, function errorCallback(response) {
                console.log('Faild where try to get Parent skills from server');
            });
    };

    $window.onbeforeunload = $scope.onExit; 

    $scope.onClickLinkAdmin = function () {
        //alert("habilitar Admin.");
    };
});

