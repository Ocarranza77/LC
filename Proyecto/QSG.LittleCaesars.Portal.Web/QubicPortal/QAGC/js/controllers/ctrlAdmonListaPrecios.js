qubicApp.controller("ctrlAdmonListaPrecios", ["$scope", function ($scope) {

    $scope.grupos = [{ id: 1, grupo: "Servicio" }, { id: 2, grupo: "Puertas 1/4" }, { id: 3, grupo: "Puertas 3/4" }, { id: 4, grupo: "Marcos" }];
    $scope.marcas = [{ id: 1, marca: "Marca 1" }, { id: 2, marca: "Marca 2" }, { id: 3, marca: "Marca 3" }, { id: 4, marca: "Ninguna" }]
    $scope.tipos = [{ id: 1, tipo: "Servicio" }, { id: 2, tipo: "Producto" }];
    $scope.sucursales = [{ id: 1, sucursal: "Tijuana" }, { id: 2, sucursal: "DF" }, { id: 3, sucursal: "Monterrey" }, { id: 4, sucursal: "Sonora" }];


    $scope.prodPrecios = [
    {
        idProd: ++$scope.cont,
        isImportante: true,
        estatus: false,
        tipo: { id: 2, tipo: "Producto" },
        producto: "Puerta X",
        grupo: { id: 2, grupo: "Puertas 1/4" },
        marca: { id: 3, marca: "Marca 2" },
        sucursal: { id: 1, sucursal: "Tijuana" },
        unidad: "12 Pza",
        precioUnitario: 14.90,
        codInterno: "fz-10",
        codProducto: "mex001",
        codBarras: 72849,
        garantia: "1 anio"
        //habilita: false
    },
    {
        idProd: ++$scope.cont,
        isImportante: false,
        estatus: true,
        tipo: { id: 1, tipo: "Servicio" },
        producto: "Reduccion",
        grupo: { id: 5, grupo: "Servicio" },
        marca: { id: 4, marca: "Ninguna" },
        sucursal: { id: 1, sucursal: "Tijuana" },
        unidad: "1 Pza",
        precioUnitario: 320,
        codInterno: "789456",
        codProducto: "SON0934",
        codBarras: 852147,
        garantia: "N/A"
        //habilita: false
    },
     {
         idProd: ++$scope.cont,
         isImportante: false,
         estatus: true,
         tipo: { id: 3, tipo: "Producto" },
         producto: "Marco Y",
         grupo: { id: 4, grupo: "Marcos" },
         marca: { id: 1, marca: "Marca 4" },
         sucursal: { id: 1, sucursal: "Tijuana" },
         unidad: "1 Pza",
         precioUnitario: 680,
         codInterno: "am90",
         codProducto: "CAMAE",
         codBarras: 3212,
         garantia: "1 SEMANA"
         //habilita: false
     }
    ];

    $scope.onChangeImportant = function (item) {
        var i = $scope.prodPrecios.indexOf(item);
        $scope.prodPrecios[i].isImportante = !$scope.prodPrecios[i].isImportante;
    };
    
    $scope.onOpenSucursales = function (item) {
        $scope.sucursalName = item.sucursal;
        $scope.precios = [];
        for (var i = 0; i < $scope.prodPrecios.length; i++) {
            $scope.precios.push({
                producto: $scope.prodPrecios[i].producto,
                sucursal: { id: $scope.prodPrecios[i].sucursal.id, sucursal: $scope.prodPrecios[i].sucursal },
                precioUnitario: $scope.prodPrecios[i].precioUnitario
            });
        }
       
        $scope.prodTemp = $scope.precios;

        $("#mdlSucursal").modal({ backdrop: false });
    }

    $scope.onSave = function () {
        $('#mdlSucursal').modal('hide');
    };
    

    $scope.precioNuevo;
    $scope.onEdit = function (producto) {
        $scope.precioNuevo = "";
        $scope.tempProducto = producto.producto;
        $scope.tempPrecio = producto.precioUnitario;
        $scope.tempoCompleto = producto;
        $scope.provedor = producto.proveedor;
        $scope.proveedorTel = producto.telProveedor;
        $scope.fechaAct = producto.actualizacion;

        $("#mdlEditarPrecio").modal({ backdrop: false });
    };
    
    $scope.onRefreshPrice = function (item) {
        var i = $scope.prodPrecios.indexOf(item);
        $scope.prodPrecios[i].precioUnitario = $scope.tempPrecio;
        $("#mdlEditarPrecio").modal("hide");
    };

    $scope.onReset = function () {
        $scope.grupo = "";
        $scope.marca = "";
        $scope.estatus = "";
        $scope.tipo = "";
        $scope.producto = "";
    };

    $scope.onConfirm = function (item) {
        $scope.product = item.producto;
        $scope.itemDeleted = item;
        $("#mdlEliminar").modal({ backdrop: false });
    };
    $scope.onDelete = function (item) {
        var i = $scope.prodPrecios.indexOf(item);
        $scope.prodPrecios.splice(i, 1);

        $("#mdlEliminar").modal("hide");
    };


    // Fin controller.
}]);