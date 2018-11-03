'use strict';
app.controller('ConfigurationController',
    ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$location', 'ConfigurationService', 'CommonService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $location, service, commonService) {
            BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            $scope.cates = ['Site', 'System'];
            $scope.dataTypes = ngAppSettings.dataTypes;
            $scope.saveCallback = function () {
                commonService.initAllSettings().then(function () {
                    $location.path('/portal/configuration/list');
                });
            }
            $scope.removeCallback = function () {
                commonService.initAllSettings().then(function () {
                    $location.path('/portal/configuration/list');
                });
            }
        }]);
