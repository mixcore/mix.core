'use strict';
app.controller('TemplateController', ['$scope', '$rootScope', '$routeParams', 'ngAppSettings', 'TemplateService',
    function ($scope, $rootScope, $routeParams, ngAppSettings, service) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
        $scope.loadParams = async function () {
            $rootScope.isBusy = true;
            $scope.folderType = $routeParams.folderType ? $routeParams.folderType : 'Masters';
            $scope.backUrl = '/portal/template/list/' + $routeParams.themeId;
            $scope.themeId = $routeParams.themeId;
        }
    }]);
