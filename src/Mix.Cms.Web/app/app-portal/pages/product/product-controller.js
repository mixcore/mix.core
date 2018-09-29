'use strict';
app.controller('ProductController', ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', 'ProductService',
    function ($scope, $rootScope, ngAppSettings, $routeParams, service) {
        service.init('product');
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service, 'product');
        $scope.preview = function (item) {
            $rootScope.preview('product', item, item.title, 'modal-lg');
        };
    }]);
