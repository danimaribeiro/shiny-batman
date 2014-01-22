var phonecatApp = angular.module('phonecatApp', [
    'ngRoute',
    'phonecatControllers'
]);

phonecatApp.config(['$routeProvider',
function ($routeProvider) {
    $routeProvider.
    when('/listar', {
        templateUrl: 'Home/Lista',
        controller: 'Listar'
    }).
    when('/editar/:entityId', {
        templateUrl: 'Home/Editar',
        controller: 'EditarVisualizar'
    }).
    when('/visualizar/:entidadeId', {
        templateUrl: 'Home/Visualizar',
        controller: 'EditarVisualizar'
    }).
    otherwise({
        redirectTo: '/listar'
    });
}]);

var phonecatControllers = angular.module('phonecatControllers', []);

phonecatControllers.controller('Listar', ['$scope', '$http',
    function ($scope, $http) {
        $scope.fields = [
            {"Description":"#", "Name":"Id", "Link":true, "Order":"desc"},
            {"Description": "Nome", "Name":"Nome", "Link":false, "Order":false}, 
            {"Description": "Telefone", "Name":"Telefone", "Link":false, "Order":false }, 
            { "Description": "Estado", "Name":"Estado", "Link":false, "Order":false}
        ];
        $scope.values = [
            {"Id":"1", "Nome":"Danimar Ribeiro", "Telefone":"(48) 9801-6226", "Estado":"Santa Catarina"},
            { "Id": "2", "Nome": "Beltrano e cicrano", "Telefone": "(48) 9801-6226", "Estado": "Parana" },
            { "Id": "3", "Nome": "Fulano de tal", "Telefone": "(48) 9801-6226", "Estado": "Rio Grande do Sul" },
        ];

        $scope.Order = "Id";
        $scope.changeOrder = function (field) {
            if (field.Order) {
                field.Order = field.Order == 'asc' ? 'desc' : 'asc';
                $scope.Order = $scope.Order == field.Name ? '-' + field.Name : field.Name;
            }
            else {
                for (var i in $scope.fields) {
                    $scope.fields[i].Order = false;
                }
                field.Order = 'asc';
                $scope.Order = field.Name;
            }
        };

        $scope.filters = [];
    }
]);

phonecatControllers.controller('EditarVisualizar', ['$scope', '$http','$routeParams',
    function ($scope, $http, $routeParams) {

        $scope.entityId = $routeParams.entityId;
        $scope.debug = false;
        // 2. UI models
        $http.get('/Data/GetFormDefinition/1').success(function (data) {            
            $scope.uimodel = data;
            var fields = $scope.uimodel.fields;
            for (var i = 0; i < fields.length; i++) {
                fields[i].modelo = "";
                fields[i].uid = "ids_" + i;
            }
        });

        $http.get('/Data/GetEntity/' + $scope.entityId).success(function (data) {
            $scope.entity = data;            
        });

    }
])