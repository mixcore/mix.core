'use strict';
app.controller('ArticleController', ['$scope', '$rootScope', '$location','$filter',
    'ngAppSettings', '$routeParams', 'ArticleService',
    function ($scope, $rootScope, $location,$filter, ngAppSettings, $routeParams, service) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
        $scope.preview = function (item) {
            item.editUrl = '/portal/article/details/' + item.id;
            $rootScope.preview('article', item, item.title, 'modal-lg');
        };
        $scope.saveCallback = function () {
            $location.url('/portal/article/list');
        }
        $scope.getSingleSuccessCallback = function () {
            $scope.activedData.publishedDateTime = $filter('utcToLocalTime')($scope.activedData.publishedDateTime);
        }
    }
]);