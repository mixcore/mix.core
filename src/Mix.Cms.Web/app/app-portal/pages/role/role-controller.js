'use strict';
app.controller('RoleController', ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', 'RoleService',
    function ($scope, $rootScope, ngAppSettings, $routeParams, service) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
        $scope.role = { name: '' };
        $scope.createRole = async function () {
            $rootScope.isBusy = true;
            var result = await service.createRole($scope.role.name);
            if (result.isSucceed) {
                $scope.role.name = '';
                $scope.getList();
            }
            else {
                $rootScope.showMessage(result.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

    }]);
