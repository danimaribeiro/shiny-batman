var phonecatApp = angular.module('phonecatApp', [
    'ngRoute',
    'phonecatControllers'
]);

phonecatApp.config(['$routeProvider',
    function ($routeProvider) {
        $routeProvider.
        when('/phones', {
            templateUrl: 'Home/Contact',
            controller: 'PhoneListCtrl'
        }).
        when('/phones/:phoneId', {
            templateUrl: 'Home/About',
            controller: 'PhoneDetailCtrl'
        }).
        otherwise({
            redirectTo: '/phones'
        });
    }]);


var phonecatControllers = angular.module('phonecatControllers', []);

phonecatControllers.controller('PhoneListCtrl', ['$scope', '$http',
function ($scope, $http) {
    $http.get('Scripts/phones.js').success(function (data) {
        $scope.phones = data;
    });

    $scope.log = [{"msg":"Primeira mensagem"}, {"msg":"Segunda mensagem"}, {"msg":"Terceira mensagem"}]
    $scope.orderProp = 'age';
}]);

phonecatControllers.controller('PhoneDetailCtrl', ['$scope', '$routeParams',
    function ($scope, $routeParams) {
        $scope.phoneId = $routeParams.phoneId;
    }
]);