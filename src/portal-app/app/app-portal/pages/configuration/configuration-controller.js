'use strict';
app.controller('ConfigurationController',
    ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$location', 'ConfigurationService', 'CommonService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $location, service, commonService) {
            BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            $scope.cates = ngAppSettings.enums.configuration_types;
            $scope.settings = $rootScope.globalSettings;
            $scope.saveSuccessCallback = function () {
                commonService.initAllSettings().then(function () {
                    $location.url($scope.referrerUrl);
                });
            }
            $scope.removeCallback = function () {
                commonService.initAllSettings().then(function () {
                    $location.url($scope.referrerUrl);
                });
            }
        }]);
