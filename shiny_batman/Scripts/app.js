var phonecatApp = angular.module('phonecatApp', [
    'ngRoute',
    'phonecatControllers',
    'ui.bootstrap'
]);

phonecatApp.factory('TabsData', function () {
    var listaAbas = [];

    return {
        addTab: function (newObj) {
            var found = false;
            for (var i in listaAbas) {
                if (listaAbas[i].id == newObj.id) {
                    listaAbas[i].ativo = true;
                    found = true;
                }
                else
                    listaAbas[i].ativo = false;
            }
            if(!found)
                listaAbas.push(newObj);
        },
        getTabs: function () {
            return listaAbas;
        }
    };
});

phonecatApp.config(['$routeProvider',
function ($routeProvider) {
    $routeProvider.
    when('/', {
        templateUrl: 'Home/Todos',
        controller: 'Tudo'
    }).
    when('/listar/', {
        templateUrl: 'Home/Lista',
        controller: 'Listar'
    }).
    when('/editar/:entityId/', {
        templateUrl: 'Home/Editar',
        controller: 'EditarVisualizar'
    }).
    when('/visualizar/:entidadeId/', {
        templateUrl: 'Home/Visualizar',
        controller: 'EditarVisualizar'
    }).
    otherwise({
        redirectTo: '/'
    });
}]);

var phonecatControllers = angular.module('phonecatControllers', []);



function Lista($http, table) {
    var self = this;    
    self.table = table;
    self.currentPage = 1;

    $http.get('/Data/ListEntities/?model=' + table + '&page=' + self.currentPage + '&maxResults=10&data' + new Date().toLocaleTimeString()).success(function (data) {
        self.fields = data.columns;        
        self.pk = self.fields.filter(function (obj) { return (obj.link==true); })[0].name
        self.values = data.values;
        self.total = data.count;
        self.rows_returned = data.results;
        self.order = data.columns[0].name;
    });
    
    self.setPage = function (pageNo) {
        self.currentPage = pageNo;

        $http.get('/Data/ListEntities/?model=' + self.table + '&page=' + pageNo + '&maxResults=10&data' + new Date().toLocaleTimeString()).success(function (data) {
            self.fields = data.columns;
            self.pk = self.fields.filter(function (obj) { return (obj.link == true); }).name
            self.values = data.values;
            self.total = data.count;
            self.rows_returned = data.results;
            self.order = data.columns[0].name;
        });
    };

    self.changeOrder = function (field) {
        if (field.order) {
            field.order = field.order == 'asc' ? 'desc' : 'asc';
            self.order = self.order == field.name ? '-' + field.name : field.name;
        }
        else {
            for (var i in self.fields) {
                self.fields[i].order = false;
            }
            field.order = 'asc';
            self.order = field.name;
        }
    };

    self.filters = [];
}

function Editar($http, model, id) {
    var self = this;
    self.model = model;
    self.debug = false;
    self.entityId = id;
    // 2. UI models
    $http.get('/Data/GetFormDefinition/?model=' + self.model + '&data' + new Date().toLocaleTimeString()).success(function (data) {
        self.uimodel = data;
        var fields = $scope.self.fields;
        for (var i = 0; i < fields.length; i++) {
            fields[i].modelo = "";
            fields[i].uid = "ids_" + i;
        }
    });

    $http.get('/Data/GetEntity/' + self.entityId + '?model=' + self.model + '&data' + new Date().toLocaleTimeString()).success(function (data) {
        self.entity = data;
    });

    self.saveEntity = function () {        
        $http.post('/Data/SaveEntity/' + self.entityId + '?model=' + self.model + '&data' + new Date().toLocaleTimeString(), { 'jsonData': angular.toJson(self.entity) }).success(function (data) {
            alert('deu certo mano');
        });
    };
}

phonecatApp.controller('Tudo', ['$scope', '$http', '$routeParams', 'TabsData',
    function ($scope, $http, $routeParams, TabsData) {                
        $scope.abas = TabsData.getTabs();
        $scope.abrirAba = function (tipo, model, id) {
          for (var i in $scope.abas)
                $scope.abas[i].ativo = false;

            if (tipo == "lista")
                TabsData.addTab({ "id": "cliente_" + id, "descricao": "Clientes " + id, "tipo": "lista", "ativo": true, "conteudo": new Lista($http, 'tb_pessoa') });
            else if (tipo == "editar")
                TabsData.addTab({ "id": "edit_" + id, "descricao": "Editar " + id, "tipo": "editar", "ativo": true, "conteudo": new Editar($http, model, id) });
        };

        $scope.ativarAba = function (item) {
            for (var i in $scope.abas)
                $scope.abas[i].ativo = false;
            item.ativo = true;
        };

        $scope.removerAba = function (indice) {
            $scope.abas.splice(indice, 1);
            $scope.abas[indice - 1].ativo = true;
        };
    }
]);

phonecatApp.controller('Menu', ['$scope', '$http', 'TabsData',
    function ($scope, $http, TabsData) {
        $http.get('/Menu/Todos?data' + new Date().toLocaleTimeString()).success(function (data) {
            $scope.menus = data;
        });

        $scope.executeAction = function (itemMenu) {
            TabsData.addTab({
                "id": itemMenu.model + "_" + itemMenu.id, "descricao": itemMenu.description,
                "tipo": "lista", "ativo": true, "conteudo": new Lista($http, itemMenu.model)
            });
        };
    }
]);

phonecatControllers.controller('Listar', ['$scope', '$http',
    function ($scope, $http) {
        $http.get('/Data/ListEntities/1?data' + new Date().toLocaleTimeString()).success(function (data) {
            $scope.fields = data.columns;
            $scope.values = data.values;
            $scope.total = data.count;
            $scope.rows_returned = data.results;
            $scope.order = data.columns[0].name;
        });

        $scope.currentPage = 1;
        $scope.setPage = function (pageNo) {
            $scope.currentPage = pageNo;

            $http.get('/Data/ListEntities/' + pageNo + '?data' + new Date().toLocaleTimeString()).success(function (data) {
                $scope.fields = data.columns;
                $scope.values = data.values;
                $scope.total = data.count;
                $scope.rows_returned = data.results;
                $scope.order = data.columns[0].name;
            });
        };

        $scope.changeOrder = function (field) {
            if (field.order) {
                field.order = field.order == 'asc' ? 'desc' : 'asc';
                $scope.order = $scope.order == field.name ? '-' + field.name : field.name;
            }
            else {
                for (var i in $scope.fields) {
                    $scope.fields[i].order = false;
                }
                field.order = 'asc';
                $scope.order = field.name;
            }
        };

        $scope.filters = [];
    }
]);

phonecatControllers.controller('EditarVisualizar', ['$scope', '$http', '$routeParams',
    function ($scope, $http, $routeParams) {

        $scope.entityId = $routeParams.entityId;
        $scope.debug = false;
        // 2. UI models
        $http.get('/Data/GetFormDefinition/1?data' + new Date().toLocaleTimeString()).success(function (data) {
            $scope.uimodel = data;
            var fields = $scope.uimodel.fields;
            for (var i = 0; i < fields.length; i++) {
                fields[i].modelo = "";
                fields[i].uid = "ids_" + i;
            }
        });

        $http.get('/Data/GetEntity/' + $scope.entityId + '?data' + new Date().toLocaleTimeString()).success(function (data) {
            $scope.entity = data;
        });

    }
])