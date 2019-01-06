'use strict';
app.controller('ThemeController', ['$scope', '$rootScope', 'ngAppSettings', '$routeParams','$location', 'ThemeService','CommonService',
    function ($scope, $rootScope, ngAppSettings, $routeParams,$location, service,commonService) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);  
        $scope.syncTemplates = async function (id) {
            $rootScope.isBusy = true;
            var response = await service.syncTemplates(id);
            if (response.isSucceed) {
                $scope.activedTheme = response.data;
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
        $scope.export = async function (id) {
            $rootScope.isBusy = true;
            var response = await service.export(id);
            if (response.isSucceed) {
                $rootScope.isBusy = false;
                window.open(response.data,'_blank');
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
        $scope.saveCallback = function () {
            commonService.initAllSettings().then(function () {
                $location.path('/portal/theme/list');
            });
        }
        $scope.removeCallback = function () {
            commonService.initAllSettings().then(function () {
                $location.path('/portal/theme/list');
            });
        }
        
    }]);
