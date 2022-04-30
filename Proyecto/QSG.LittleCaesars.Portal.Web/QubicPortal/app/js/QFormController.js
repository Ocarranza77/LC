qubicApp.controller('QFormController', function ($scope, $http, $sce, $templateRequest, $compile, $rootScope) {
    var _token = "";

    $scope.selectedCompany = {};
    $scope.user = {};
    $scope.path = "";

    $scope.saveAction = true;
    $scope.reportAction = false;
    $scope.clearAction = false;
    $scope.searchAction = false;
    $scope.deleteAction = true;
    $scope.exportAction = true;
    $scope.printAction = true;

    $scope.saveEvent = true;
    $scope.reportEvent = true;
    $scope.clearEvent = true;
    $scope.searchEvent = true;
    $scope.deleteEvent = true;
    $scope.exportEvent = true;
    $scope.printEvent = true;

    $scope.spetialPermist = {};

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
        loadFormInfo(_token, values[1], values[2]);

        function loadFormInfo(sid, companyId, formId) {
            request = {
                Token: sid,
                RequestMessage: { ControlID: formId, CompanyID: companyId }
            };

            $http({
                method: 'POST',
                url: urlROOT + '/QubicService.svc/initQbicForm',
                data: request,
                headers: { 'Content-Type': 'application/json' }
            }).then(function successCallback(response) {
                if (response.data.ResponseCode == "403") {
                    //console.log("token no valido...");
                    window.location.href = "Login.html";
                }
                if (response.data.ResponseCode == "200") {
                    $scope.selectedCompany = response.data.Content.Company;

                    for (var itm in response.data.Content.Path) {
                        if (itm < response.data.Content.Path.length - 1) {
                            $scope.path = $scope.path + "<li>" + response.data.Content.Path[itm] + "</li>";
                        }
                        else {
                            $scope.path = $scope.path + "<li class='active'>" + response.data.Content.Path[itm] + "</li>";
                            document.title = response.data.Content.Path[itm];
                        }
                    }

                    $('#pnlPath').html($scope.path);
                    $scope.applyFormSettings(response.data.Content.Permits);
                    $scope.applyFormEvents(response.data.Content.AcctionForm);
                    $scope.spetialPermist = response.data.Content.SpetialPermits;
                    $scope.user = response.data.Content.User;
                    //console.log('load control...' + response.data.Content.ControlName);
                    loadCSS('css/companies/' + $scope.selectedCompany.Logo + '.css');
                    $scope.loadControl(response.data.Content.ControlName);

                }


            }, function errorCallback(response) {
                $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
                $('.errorMessage').show().delay(2000).fadeOut('slow');;
            });

        }

        $scope.loadControl = function (controlName) {

            var templateUrl = $sce.getTrustedResourceUrl(controlName);
            console.log(templateUrl);
            $templateRequest(templateUrl).then(function (template) {
                //console.log(template);
                $compile($("#pnlControlContainer").html(template).contents())($scope);
                // $().delay(500);
                //                $rootScope.$broadcast("onInitAction", { "BaseDatos": $scope.selectedCompany.Logo, "UsuarioID": "16", "toke" : _token, "PermisosEspeciales": $scope.spetialPermist });
                $rootScope.$broadcast("onInitAction",
                    {
                        "baseDatos": $scope.selectedCompany.Logo,
                        "usuario": $scope.user,
                        "token": _token,
                        "permisosEspeciales": $scope.spetialPermist,
                        "saveAction": $scope.saveAction,
                        "deleteAction": $scope.deleteAction,
                        "exportAction": $scope.exportAction,
                        "printAction": $scope.printAction
                    });
            }, function () {
                // An error has occurred
                console.log('Error loading the template');
            });
        }

    }

    $scope.applyFormSettings = function (allowActions) {

        //TODO: Aplicar el encendido de los botones segun lo que regresen.    
        //mnFrm.Permiso_Add, mnFrm.Permiso_Delete, mnFrm.Permiso_Export, mnFrm.Permiso_Print, mnFrm.Permiso_Update 

        for (var itm in allowActions) { // Permisos del usuario

            if (allowActions[itm] === "Save") { // ADD
                $scope.saveAction = false;
            }
            /*if (allowActions[itm] === "Report") {
                $scope.reportAction = false;
            }
            if (allowActions[itm] === "Clear") {
                $scope.clearAction = false;
            }
            if (allowActions[itm] === "Search") {
                $scope.searchAction = false;
            }*/
            if (allowActions[itm] === "Delete") { //Delete
                $scope.deleteAction = false;
            }
            if (allowActions[itm] === "Export") {// Export
                $scope.exportAction = false;
            }
            if (allowActions[itm] === "Print") {// Print
                $scope.printAction = false;
            }
        }

    }

    $scope.applyFormEvents = function (allowEvents) { // Eventos de la forma

        for (var itm in allowEvents) {

            if (allowEvents[itm] === "Save") {
                $scope.saveEvent = false;
            }
            if (allowEvents[itm] === "Report") {
                $scope.reportEvent = false;
            }
            if (allowEvents[itm] === "Clear") {
                $scope.clearEvent = false;
            }
            if (allowEvents[itm] === "Search") {
                $scope.searchEvent = false;
            }
            if (allowEvents[itm] === "Delete") {
                $scope.deleteEvent = false;
            }
            if (allowEvents[itm] === "Export") {
                $scope.exportEvent = false;
            }
            if (allowEvents[itm] === "Print") {
                $scope.printEvent = false;
            }
        }

    }

    $scope.onClearAction = function () {
        $rootScope.$broadcast("onClearAction", { "companyId": "123" });
    }
    $scope.onReportAction = function () {
        $rootScope.$broadcast("onReportAction", {});
    }
    $scope.onSaveAction = function () {
        $rootScope.$broadcast("onSaveAction", {});
    }
    $scope.onSearchAction = function () {
        $rootScope.$broadcast("onSearchAction", {});
    }
    $scope.onDeleteAction = function () {
        $rootScope.$broadcast("onDeleteAction", {});
    }
    $scope.onExportAction = function () {
        $rootScope.$broadcast("onExportAction", {});
    }
    $scope.onPrintAction = function () {
        $rootScope.$broadcast("onPrintAction", {});
    }
    $scope.onCloseAction = function () {
        if (confirm("Seguro de cerrar la pantalla?")) {
            window.opener.focus();
            close();
        }
    }

    loadCSS = function (href) {
        var cssLink = $("<link id='cssXCompany' rel='stylesheet' type='text/css' href='" + href + "'>");
        $("head").append(cssLink);
    };
});