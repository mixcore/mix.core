'use strict';
app.controller('ThemeController', ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', 'ThemeService',
    function ($scope, $rootScope, ngAppSettings, $routeParams, service) {
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

        
    }]);
