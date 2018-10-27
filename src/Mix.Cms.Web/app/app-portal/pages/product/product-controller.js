'use strict';
app.controller('ProductController', ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', 'ProductService',
    function ($scope, $rootScope, ngAppSettings, $routeParams, service) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
        $scope.preview = function (item) {
            item.editUrl = '/portal/product/details/' + item.id;
            $rootScope.preview('product', item, item.title, 'modal-lg');
        };
    }]);
