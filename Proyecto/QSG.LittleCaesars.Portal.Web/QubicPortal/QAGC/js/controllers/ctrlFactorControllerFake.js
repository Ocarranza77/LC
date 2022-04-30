qubicApp.controller('factorController', function ($scope, $http, $rootScope) {

    //======================================================
    //Variables declarion
    //======================================================

    $scope.factors = [];
    $scope.factorsReport = [];

    $scope.statusList = ["Activo", "Inactivo"];
    $scope.typeList = ["Valor", "Regla"];
    $scope.valueTypeList = ["Importe", "Porcentaje"];

    $scope.clonedFactors = [];
    $scope.clonedFactorsReport = [];

    $scope.selectedFactor = {};

    loadAllFactors();
    $scope.lastFormulaValue = "";
    $scope.formulaValues = [];

    $scope.isDirty = false;

    var deleteItem = 0;

    //$('#txtFormula').on('keydown', function (e) {
    $('#txtFormula').on('input', function (e) {

        //console.log('event ....' + e.wich);
        var currentValue = e.target.value;
        checkIfCodsExistInFormula(currentValue);

        //if (e.which == 8 || e.which == 46) {
        //    checkIfCodsExistInFormula(currentValue);
        //}
    });



    //======================================================
    //Private methods
    //======================================================
    function loadAllFactors() {
        var request = {};

        request = {
            Token: "1234-1234578-454545-5545"
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/factores',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {

                for (var f in response.data.Content.Factores) {

                    var fid = "cod" + response.data.Content.Factores[f].FactorID;

                    if (response.data.Content.Factores[f].Regla) {
                        response.data.Content.Factores[f].ValorTipo = "Regla";
                        response.data.Content.Factores[f].Valor = "";
                        response.data.Content.Factores[f].ValorFinal = "";
                        calculateFormulaValues(response.data.Content.Factores[f]);
                    } else {
                        response.data.Content.Factores[f].Abr = "$";
                        response.data.Content.Factores[f].ValorTipo = "Valor";
                        response.data.Content.Factores[f].ValorFinal = response.data.Content.Factores[f].Valor;
                    }

                    console.log(response.data.Content.Factores);

                    $scope.factors[fid] = response.data.Content.Factores[f];
                    $scope.factorsReport.push($scope.factors[fid]);

                    for (var itm in $scope.factors) {
                        if (checkUserReferences($scope.factors[itm].FactorID)) {
                            $scope.factors[itm].isUsed = true;
                        }
                    }
                    
                }
            }
            if (response.data.ResponseCode == "500") {
                console.log('Error interno...');
            }

        }, function errorCallback(response) {
            console.log('Error....');
        });

    }

    function createFactorsReport() {

        $scope.factorsReport = [];

        for (var itm in $scope.factors) {
            $scope.factorsReport.push($scope.factors[itm]);
        }
    }


    function calculateFormulaValues(factor) {

        console.log('Calculating...' + factor.Formula);

        formulaTreated = factor.Formula.replace(/(cod[\d]+)/g, ' $1 ');
        var pieces = formulaTreated.split(' ');

        var msg = "";
        var ope = "";


        for (var x in pieces) {
            if (pieces[x].indexOf('cod') > -1) {
                msg += $scope.factors[pieces[x]].Nombre;
                ope += $scope.factors[pieces[x]].ValorFinal;
            } else {
                msg += pieces[x].toString();
                ope += pieces[x].toString();
            }
        }

        factor.FormulaDesc = msg;
        var operation = new Function('return ' + ope);
        factor.ValorFinal = operation();
    }

    function cloneFactorList() {

        $scope.clonedFactors = [];
        $scope.clonedFactorsReport = [];

        //Clone only active factors        
        for (var itm in $scope.factors) {
            $scope.clonedFactors[itm] = cloneFactor($scope.factors[itm]);
        }

        createClonedFactorsReport();
    }

    function createClonedFactorsReport() {

        $scope.clonedFactorsReport = [];

        for (var itm in $scope.clonedFactors) {
            $scope.clonedFactorsReport.push($scope.clonedFactors[itm]);
        }
    }


    function cloneFactor(source) {
        result = {
            "FactorID": source.FactorID,
            "Orden": source.Orden,
            "Activo": source.Activo,
            "Nombre": source.Nombre,
            "ValorTipo": source.ValorTipo,
            "Abr": source.Abr,
            "Valor": source.Valor,
            "Formula": source.Formula,
            "FormulaDesc": source.FormulaDesc,
            "ValorFinal": source.ValorFinal,
            "isRegla": source.Regla,
            "isUsed": source.isUsed,
            "UserID": source.UserID,
            "CDate": source.CDate
        };

        //console.log('Status: ' + result.Activo);

        return result;
    }


    function reindexValues() {
        var orderAlreadyExist = false;

        //Check if the order already exist
        for (var itm in $scope.clonedFactors) {
            if ($scope.clonedFactors[itm].Orden == $scope.selectedFactor.Orden) {
                orderAlreadyExist = true;
                break;
            }
        }

        var previousCode = "";

        if (orderAlreadyExist) {
            //If exist alter current order...
            for (var itm in $scope.clonedFactors) {
                if ($scope.clonedFactors[itm].Orden == $scope.selectedFactor.Orden) {
                    previousCode = itm;
                    $scope.clonedFactors[itm].Orden++;
                    $scope.clonedFactors[itm].OperationType = 2;
                } else if ($scope.clonedFactors[itm].Orden > $scope.selectedFactor.Orden) {
                    if ($scope.clonedFactors[itm].Orden == $scope.clonedFactors[previousCode].Orden) {
                        previousCode = itm;
                        $scope.clonedFactors[itm].Orden++;
                        $scope.clonedFactors[itm].OperationType = 2;
                    } else {
                        break;
                    }
                }
            }
        }

        $scope.clonedFactors["cod" + $scope.selectedFactor.FactorID] = $scope.selectedFactor;
    }

    function sortClonedFactors() {

        var swapped;
        do {
            swapped = false;

            for (var i = 0; i < $scope.clonedFactorsReport.length - 1; i++) {
                if ($scope.clonedFactorsReport[i].Orden > $scope.clonedFactorsReport[i + 1].Orden) {
                    var temp = cloneFactor($scope.clonedFactorsReport[i]);
                    $scope.clonedFactorsReport[i] = cloneFactor($scope.clonedFactorsReport[i + 1]);
                    $scope.clonedFactorsReport[i + 1] = temp;
                    swapped = true;
                }
            }
        } while (swapped);
    }

    function doCalculations() {

        $('#lblError').text('').hide();

        if ($scope.selectedFactor.ValorTipo == 'Valor') {
            if ($scope.selectedFactor.valueType == 'Importe') {
                $scope.selectedFactor.ValorFinal = $scope.selectedFactor.Valor;
            } else {
                $scope.selectedFactor.ValorFinal = $scope.selectedFactor.Valor / 100;
            }
            setButtonStatus(true);
        } else {

            if (!checkIfRulesFactorsAreLower()) {
                $('#lblError').text('No se puede realizar la operacion, uno de los factores utilizados tiene un orden superior.').show();
                return;
            }

            try {
                calculateFormulaValues($scope.selectedFactor);
                setButtonStatus(true);
            }
            catch (err) {
                $('#lblError').text('No se puede realizar la operacion, revisar que este escrita correctamente.').show();
            }

        }
    }

    function checkIfCodsExistInFormula(currentValue) {

        console.log('checking if exist...');
        //console.log("current value: " + currentValue + ", Last Value: " + $scope.lastFormulaValue );

        if (currentValue == "")
            return;

        var formulaValuesFound = [];
        var formulaValuesNotFound = [];

        for (var itm in $scope.formulaValues) {
            if (currentValue.indexOf($scope.formulaValues[itm]) > -1) {
                formulaValuesFound.push($scope.formulaValues[itm]);
            } else {
                formulaValuesNotFound.push($scope.formulaValues[itm]);
            }

        }

        //console.log(formulaValuesNotFound);

        if (formulaValuesNotFound.length > 0) {

            var acceptableValue = $scope.lastFormulaValue;
            $scope.formulaValues = formulaValuesFound;

            for (var itm in formulaValuesNotFound) {
                acceptableValue = acceptableValue.replace(formulaValuesNotFound[itm], '');
            }

            //console.log('Acceptable Value: ' + acceptableValue);

            $scope.lastFormulaValue = acceptableValue;
            $('#txtFormula').val(acceptableValue);


        }
    }

    function checkIfRulesFactorsAreLower() {

        for (var itm in $scope.formulaValues) {
            if ($scope.clonedFactors[$scope.formulaValues[itm]].Orden > $scope.selectedFactor.Orden) {
                result = false;
                break;
            }
                
        }
        return result;
    }

    function setButtonStatus(isEnabled) {

        if (!isEnabled) {
            $('#cmdSaveFactor').prop('disabled', true);
        }
        else
        {
            $('#cmdSaveFactor').removeAttr('disabled');
        }
    }

    function collectFactorsToSave() {
        result = [];

        for (var itm in $scope.clonedFactors) {
            if ($scope.clonedFactors[itm].OperationType) {
                result.push($scope.clonedFactors[itm]);
            }
        }

        console.log(result);
        return result;
    }

    function checkUserReferences(factorId) {

        var result = false;
        var cod = "cod" + factorId;

        for (var itm in $scope.factors) {
            if ($scope.factors[itm].Formula && $scope.factors[itm].Formula != "") {
                if ($scope.factors[itm].Formula.indexOf(cod) > -1) {
                    result = true;
                    break;
                }
            }
        }

        return result;
    }

    //======================================================
    //Controller Events
    //======================================================
    $scope.onAddNewFactor = function () {
        //console.log('Adding new factor...');
        cloneFactorList();

        $scope.lastFormulaValue = "";

        $scope.selectedFactor = {
            "FactorID": 0,
            "Orden": 0,
            "Nombre": "",
            "Activo": $scope.statusList[0],
            "ValorTipo": $scope.typeList[0],
            "valueType": $scope.valueTypeList[0],
            "Formula": "",
            "Valor": 0,
            "OperationType": 1,
            "UserID": "16",
            "CDate": new Date().toDateString()
        };

        setButtonStatus(false);

        $scope.$watch('selectedFactor.Orden', function (newValue, oldValue) { setButtonStatus(false); });
        $scope.$watch('selectedFactor.Nombre', function (newValue, oldValue) { setButtonStatus(false); });
        $scope.$watch('selectedFactor.ValorTipo', function (newValue, oldValue) { setButtonStatus(false); });
        $scope.$watch('selectedFactor.valueType', function (newValue, oldValue) { setButtonStatus(false); });
        $scope.$watch('selectedFactor.Formula', function (newValue, oldValue) { setButtonStatus(false); });
        $scope.$watch('selectedFactor.Valor', function (newValue, oldValue) { setButtonStatus(false); });

    }

    $scope.onAddFactorToFormula = function (factorId) {
        $('#txtFormula').val($('#txtFormula').val() + "cod" + factorId);
        $scope.lastFormulaValue = $('#txtFormula').val();
        $scope.formulaValues.push("cod" + factorId);
        $scope.selectedFactor.Formula = $scope.lastFormulaValue;
    }

    $scope.onCalculate = function () {

        if ($scope.selectedFactor.Nombre == "") {
            $('#lblError').text('El nombre del concepto es un valore requerido, favor de capturar').show();
            return;
        }

        cloneFactorList();
        doCalculations();
        reindexValues();
        createClonedFactorsReport();
        sortClonedFactors();
    }

    $scope.onSaveFactor = function () {
        var data = collectFactorsToSave();

        if (data && data.length > 0) {
            var request = {};

            request = {
                Token: "1234-1234578-454545-5545",
                RequestMessage: { Factores: JSON.stringify(data) }
            };

            $http({
                method: 'POST',
                url: urlROOT + '/QubicService.svc/savefactores',
                data: request,
                headers: { 'Content-Type': 'application/json' }
            }).then(function successCallback(response) {
                if (response.data.ResponseCode == "403") {
                    window.location.href = "Login.html";
                }
                if (response.data.ResponseCode == "200") {
                    $scope.factors = [];
                    $scope.factorsReport = [];

                    for (var f in response.data.Content.Factores) {

                        var fid = "cod" + response.data.Content.Factores[f].FactorID;

                        if (response.data.Content.Factores[f].Regla) {
                            response.data.Content.Factores[f].ValorTipo = "Regla";
                            response.data.Content.Factores[f].Valor = "";
                            response.data.Content.Factores[f].ValorFinal = "";
                            calculateFormulaValues(response.data.Content.Factores[f]);
                        } else {
                            response.data.Content.Factores[f].Abr = "$";
                            response.data.Content.Factores[f].ValorTipo = "Valor";
                            response.data.Content.Factores[f].ValorFinal = response.data.Content.Factores[f].Valor;
                        }

                        $scope.factors[fid] = response.data.Content.Factores[f];
                        $scope.factorsReport.push($scope.factors[fid]);
                    }

                    $('#mdlAltaModFactores').modal('toggle');
                }
                if (response.data.ResponseCode == "500") {
                    $('#lblError').text('Ha sucesido un error interno al momento de grabar.').show();
                }

            }, function errorCallback(response) {
                console.log('Error....');
            });

            
        }
    }

    $scope.onFactorDeleted = function (factorId) {

        $('#mdlDeleteFactor').modal();
        deleteItem = factorId;
    }

    $scope.onDelete = function () {

        var request = {};

        request = {
            Token: "1234-1234578-454545-5545",
            RequestMessage: { FactorID: deleteItem }
        };

        $http({
            method: 'POST',
            url: urlROOT + '/QubicService.svc/deletefactor',
            data: request,
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            if (response.data.ResponseCode == "403") {
                window.location.href = "Login.html";
            }
            if (response.data.ResponseCode == "200") {
                var cod = 'cod' + deleteItem;
                $('#mdlDeleteFactor').modal('toggle');
                delete $scope.factors[cod];
                createFactorsReport();
            }
            if (response.data.ResponseCode == "500") {
                $('#lblError').text('Ha sucesido un error interno al momento de grabar.').show();
            }

        }, function errorCallback(response) {
            console.log('Error....');
        });
    }

    $scope.onFactorEdited = function (factorId) {

        cloneFactorList();
        $scope.lastFormulaValue = "";

        $scope.selectedFactor = cloneFactor($scope.factors["cod" + factorId]);
        $scope.selectedFactor.OperationType = 2;

        $scope.selectedFactor.isUsed = checkUserReferences(factorId);
       

        if ($scope.selectedFactor.Activo) {
            $scope.selectedFactor.Activo = 'Activo';
        } else {
            $scope.selectedFactor.Activo = 'Inactivo';
        }

        if ($scope.selectedFactor.Abr == '$') {
            $scope.selectedFactor.valueType = 'Importe';
        } else {
            $scope.selectedFactor.valueType = 'Porcentaje';
        }

        setButtonStatus(false);

        $scope.$watch('selectedFactor.Orden', function (newValue, oldValue) { setButtonStatus(false); });
        $scope.$watch('selectedFactor.Nombre', function (newValue, oldValue) { setButtonStatus(false); });
        $scope.$watch('selectedFactor.ValorTipo', function (newValue, oldValue) { setButtonStatus(false); });
        $scope.$watch('selectedFactor.valueType', function (newValue, oldValue) { setButtonStatus(false); });
        $scope.$watch('selectedFactor.Formula', function (newValue, oldValue) { setButtonStatus(false); });
        $scope.$watch('selectedFactor.Valor', function (newValue, oldValue) { setButtonStatus(false); });


        $('#mdlAltaModFactores').modal();
    }


});


function onFormulaChange(evt) {
    //Accepted Values....
    console.log('formulachange...' + evt.keyCode);

    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57) && !isFormulaAcceptedValue(charCode)) {
        return false;
    }
    return true;
}

function isFormulaAcceptedValue(char) {
    var isAcceptable = false;
    
    //40 (, 41 ), 43 +, 45 -, 42 *, 47 /, 
    var acceptedValues = "()+=*/ ";
    var value = String.fromCharCode(char);

    if (acceptedValues.indexOf(value) > -1)
        isAcceptable = true;

    return isAcceptable;
}

