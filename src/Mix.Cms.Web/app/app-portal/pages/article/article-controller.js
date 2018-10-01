'use strict';
app.controller('ArticleController', ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', 'ArticleService',
    function ($scope, $rootScope, ngAppSettings, $routeParams, service) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
        $scope.preview = function (item) {
            $rootScope.preview('article', item, item.title, 'modal-lg');
        };
    }]);
