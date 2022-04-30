
    qubicApp.directive("mdlDelete", function () {
        return {
            restrict: "E",
            templateUrl: "QAGC/views/ArticuloServicios/mdlDelete.html"
        }
    });


    qubicApp.directive("mdlNew", function () {
        return {
            restrict: "E",
            templateUrl: "QAGC/views/ArticuloServicios/mdlNew.html"
        }
    });

    qubicApp.directive("mdlSucursal", function () {
        return {
            restrict: "E",
            templateUrl: "QAGC/views/AdmonListaPrecios/mdlSucursal.html"
        }
    });

    qubicApp.directive("mdlEditarPrecio", function () {
        var r = {};
        r.restrict = "E";
        r.templateUrl = "QAGC/views/AdmonListaPrecios/mdlEditarPrecio.html"
        return r;
    });

    qubicApp.directive("mdlEliminar", function () {
        return {
            restrict: "E",
            templateUrl: "QAGC/views/AdmonListaPrecios/mdlEliminar.html"
        }
    });