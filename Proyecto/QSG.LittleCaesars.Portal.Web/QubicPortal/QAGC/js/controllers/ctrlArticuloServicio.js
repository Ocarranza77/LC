qubicApp.controller('ctrlArticuloServicio', function ($scope, $http, $rootScope) {
    //qubicApp.controller("ctrlArticuloServicio", ["$scope", function ($scope) {

    // --------------------------------------------------------------------------
    // Variables Globales y permisos de la pantalla.  
    // --------------------------------------------------------------------------
    var _permisosEspeciales = {};
    var _token = "";
    var _usuario = {};
    var _baseDatos = "";
    $scope.saveAction = true;
    $scope.deleteAction = true;
    $scope.exportAction = true;
    $scope.printAction = true;

    // ======================================================================
    // Eventos de la Barra de Acciones
    // ======================================================================

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

        // Llamado de las funciones para llenar los datos iniciales de la pantalla
        $scope.onReset();
        onInicializate();
    });


    $rootScope.$on("onSaveAction", function (event, args) {
        //$scope.onUserSaved();
        $scope.onSave();
    });


    $rootScope.$on("onClearAction", function (event, args) {
        $scope.selectedUser = {};
        $scope.selectedPerfil = $scope.perfiles[0];
        $scope.selectedStatus = $scope.status[0];

    });


    // ======================================================================
    // Operativa de la Pantalla
    // ======================================================================
    $scope.filtro = {};
    //$scope.grupo = null;
    //$scope.GrNombre = null;
    //$scope.marca = null
    //$scope.status = "";
    //$scope.tipo = null;

    function onInicializate() {
        var request = {};

        request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, BDName: _baseDatos, MessageOperationType: 1 // Consulta
                , GetCboInis: true
                , Filtro: { }
            }
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/catArticulos',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                console.log(response.data.Content);
                //$scope.users = response.data.Content.Usuarios;
                //console.log($scope.users);

                //$scope.grupos = [{ id: 1, grupo: "Servicio" }, { id: 2, grupo: "Puertas 1/4" }, { id: 3, grupo: "Puertas 3/4" }, { id: 4, grupo: "Marcos" }];
                //$scope.marcas = [{ id: 1, marca: "Marca 1" }, { id: 2, marca: "Marca 2" }, { id: 3, marca: "Marca 3" }, { id: 4, marca: "Ninguna" }]
                //$scope.tipos = [{ id: 1, tipo: "Servicio" }, { id: 2, tipo: "Producto" }];
                var cboInis = response.data.Content.CboInis;
                
                for (var i = 0; i < cboInis.length; i++) {
                    if (cboInis[i].Key == 'ArticuloTipo')
                        $scope.tipos = cboInis[i].Value;
                    if (cboInis[i].Key == 'Grupo')
                        $scope.grupos = cboInis[i].Value;
                    if (cboInis[i].Key == 'Marca')
                        $scope.marcas = cboInis[i].Value;
                    if (cboInis[i].Key == 'Unidad')
                        $scope.unidades = cboInis[i].Value;

                }

                for (var i = 0; i < response.data.Content.Articulos.length; i++)
                    response.data.Content.Articulos[i].habilita = false;

                $scope.productos = response.data.Content.Articulos;
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
            $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
            $('#lblErrorMessage').parent().removeClass("alert-danger alert-success alert-info alert-warning").addClass("alert-danger");
            $('.errorMessage').show().delay(5000).fadeOut('slow');;
        });

    };

    var cont = 0;
    $scope.objEditado = null;


    /*

    $scope.productos = [
        {
            idProd: ++$scope.cont,
            estatus: true,
            tipo: { id: 2, tipo: "Producto" },
            producto: "Puerta x",
            grupo: { id: 2, grupo: "Puertas 1/4" },
            marca: { id: 3, marca: "Marca 1" },
            unidad: "12 Pza",
            codInterno: "fz-10",
            codProducto: "mex001",
            codBarras: 72849,
            garantia: "1 anio",
            habilita: false
        },
        {
            idProd: ++$scope.cont,
            estatus: false,
            tipo: { id: 4, tipo: "Servicio" },
            producto: "Reduccion",
            grupo: { id: 1, grupo: "Servicios" },
            marca: { id: 4, marca: "Ninguna" },
            unidad: "1 Pza",
            codInterno: "MM490",
            codProducto: "",
            codBarras: 111,
            garantia: "0 días",
            habilita: false
        },
        {
            idProd: ++$scope.cont,
            estatus: true,
            tipo: { id: 2, tipo: "Producto" },
            producto: "Cuadro",
            grupo: { id: 2, grupo: "Marcos" },
            marca: { id: 1, marca: "Marca 3" },
            unidad: "2 Pza",
            codInterno: "TL74",
            codProducto: "789",
            codBarras: 654789,
            garantia: "0 días",
            habilita: false
        }
    ];
    */
    var tempProductos = [];
    $scope.edicion = function (item) {
        tempProductos = {
            tipo: { id: item.Tipo.ID, tipo: item.Tipo.Nombre },
            producto: item.Nombre,
            grupo: {id: item.Grupo.ID, grupo: item.Grupo.Nombre },
            marca: { id: item.Marca.ID, marca: item.Marca.Nombre },
            estatus: item.Activo,
            unidad: { id: item.Unidad.ID, unidad: item.Unidad.Nombre },
            codInterno: item.ArticuloID,
            codProducto: item.CodigoProductor,
            codBarras: item.CodigoBarras,
            garantia: item.Garantia,
            habilita: false
        };

        var i = $scope.productos.indexOf(item);
        $scope.productos[i].habilita = !$scope.productos[i].habilita;

        if (!$scope.productos[i].habilita)
            $scope.productos[i].OperationType = 2; // Edit
    };
    $scope.onUndo = function (item) {
        var i = $scope.productos.indexOf(item);
        $scope.productos[i].Tipo = { ID: tempProductos.tipo.id, Nombre: tempProductos.tipo.tipo };
        $scope.productos[i].Nombre = tempProductos.producto;
        $scope.productos[i].Grupo = { ID: tempProductos.tipo.id, Nombre: tempProductos.grupo.tipo };
        $scope.productos[i].Marca = { ID: tempProductos.marca.id, Nombre: tempProductos.marca.marca };
        $scope.productos[i].Unidad = { ID: tempProductos.marca.id, Nombre: tempProductos.unidad.unidad };
        $scope.productos[i].ArticuloID = tempProductos.codInterno;
        $scope.productos[i].CodigoProductor = tempProductos.codProducto;
        $scope.productos[i].CodigoBarras = tempProductos.codBarras;
        $scope.productos[i].Garantia = tempProductos.garantia;
        $scope.productos[i].habilita = false;
        $scope.productos[i].OperationType = 0; // None

    }

    function clearFilds() {
        $scope.tipoo = null;
        $scope.producto = "";
        $scope.grupoo = null;
        $scope.marcaa = null;
        $scope.chkStatus = false;
        $scope.unidad = "";
        $scope.codInterno = "";
        $scope.codProducto = "";
        $scope.codBarras = "";
        $scope.garantia = "";
        $scope.objEditado = null;
    };

    $scope.onAceptModal = function () {
        if ($scope.objEditado == null) {
            $scope.productos.push({
                ArticuloID: 0,
                Activo: $scope.chkStatus,
                Tipo: { ID: $scope.tipoo.ID, Nombre: $scope.tipoo.Nombre },
                Nombre: $scope.producto,
                Grupo:  { ID: $scope.grupoo.ID, Nombre: $scope.grupoo.Nombre },
                Marca: { ID: $scope.marcaa.ID, Nombre: $scope.marcaa.Nombre },
                Unidad: { ID: $scope.unidadd.ID, Nombre: $scope.unidadd.Nombre },
                CodigoProductor: $scope.codProducto,
                CodigoBarras: $scope.codBarras,
                Garantia: $scope.garantia,
                OperationType: 1, // New
                habilita: false
            });
        } else {
            var i = $scope.productos.indexOf($scope.objEditado);
            $scope.productos[i].Activo = $scope.chkStatus;
            $scope.productos[i].Tipo = { ID: $scope.tipoo.ID, Nombre: $scope.tipoo.Nombre },
            $scope.productos[i].Nombre = $scope.producto;
            $scope.productos[i].Grupo = { ID: $scope.grupoo.ID, Nombre: $scope.grupoo.Nombre },
            $scope.productos[i].Marca = { ID: $scope.marcaa.ID, Nombre: $scope.marcaa.Nombre },
            $scope.productos[i].Unidad = { ID: $scope.unidadd.ID, Nombre: $scope.unidadd.Nombre },
            $scope.productos[i].CodigoInterno = $scope.codInterno;
            $scope.productos[i].CodigoProductor = $scope.codProducto;
            $scope.productos[i].CodigoBarras = $scope.codBarras;
            $scope.productos[i].Garantia = $scope.garantia;
            $scope.productos[i].OperationType = 2; // Edit
        }

        $scope.onCloseModal();
    };

    $scope.onSave = function () {
        var toSave = collectRowsToUpdate();

        if (toSave.length <= 0) {
            return;
        }

        var request = {};

        request = {
            Token: _token,
            RequestMessage: {
                UserIDRqst: _usuario.UsuarioID, BDName: _baseDatos, MessageOperationType: 2 // Save
                , Articulos: toSave
            }
        };


        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/catArticulos',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {

            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                onInicializate();
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
            $('#lblErrorMessage').html("An error occurs while trying to communicate with the server");
            $('#lblErrorMessage').parent().removeClass("alert-danger alert-success alert-info alert-warning").addClass("alert-danger");
            $('.errorMessage').show().delay(5000).fadeOut('slow');;
        });

        $('#mdlNew').modal('hide');

    };


    function collectRowsToUpdate() {

        var rowsToUpdate = [];

        for (var i = 0; i < $scope.productos.length; i++) {
            if ($scope.productos[i].OperationType != 0) {
                rowsToUpdate.push($scope.productos[i]);
            }
        }

        return rowsToUpdate;
    }

    $scope.onOpenEdit = function (item) {
        if (item == undefined) {
            clearFilds();
            $("#mdlNew").modal({ backdrop: false });
        }
        else {
            $scope.objEditado = item;

            $scope.tipoo = { ID: item.Tipo.ID, Nombre: item.Tipo.Nombre };
            $scope.producto = item.Nombre;
            $scope.grupoo = { ID: item.Grupo.ID, Nombre: item.Grupo.Nombre };
            $scope.marcaa = { ID: item.Marca.ID, Nombre: item.Marca.Nombre };
            $scope.chkStatus = item.Activo;
            $scope.unidadd = { ID: item.Unidad.ID, Nombre: item.Unidad.Nombre };
            $scope.codInterno = item.ArticuloID;
            $scope.codProducto = item.CodigoProductor;
            $scope.codBarras = item.CodigoBarras;
            $scope.garantia = item.Garantia;
            $("#mdlNew").modal({ backdrop: false });
        }
    };

    $scope.onChangeStatus = function (item) {
        var i = $scope.productos.indexOf(item);
        $scope.productos[i].Activo = !$scope.productos[i].Activo;
        $scope.productos[i].OperationType = 2;
    };

    $scope.onConfirm = function (item) {
        $scope.product = item.Nombre != "" ? item.Nombre : "Sin nombre";
        $scope.itemDeleted = item;
        $("#mdlDelete").modal({ backdrop: false });
    };

    $scope.onDelete = function (item) {
        var i = $scope.productos.indexOf(item);
        $scope.productos[i].OperationType = 3; // Delete
        //$scope.productos.splice(i, 1);

        $("#mdlDelete").modal("hide");
    };

    $scope.onReset = function () {
        $scope.filtro.grupo = null;
        $scope.filtro.marca = null
        $scope.filtro.tipo = null;
        $scope.filtro.status = "";
        $scope.filtro.nombre = "";
    };

    $scope.onCloseModal = function () { $("#mdlNew").modal("hide"); };

    $scope.onAlan = function (x) {
        console.log('Grupo: ');
        console.log(x);
    };

    $scope.filtros = function (item) {

        var mostrar = true;
        if ($scope.filtro.grupo != null && $scope.filtro.grupo.ID != '')
            if (item.Grupo.ID != $scope.filtro.grupo.ID)
                mostrar = false;

        if ($scope.filtro.marca != null && $scope.filtro.marca.ID != '')
            if (item.Marca.ID != $scope.filtro.marca.ID)
                mostrar = false;

        if ($scope.filtro.tipo != null && $scope.filtro.tipo.ID != '')
            if (item.Tipo.ID != $scope.filtro.tipo.ID)
                mostrar = false;

        return mostrar;
    };

});
//}]);