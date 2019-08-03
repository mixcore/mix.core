'use strict';
app.controller('PositionController', [
    '$scope', '$rootScope', '$location',
    'ngAppSettings', '$routeParams', 'PagePositionService', 'PositionService',      
    function ($scope, $rootScope, $location, 
        ngAppSettings, $routeParams, pagePositionService, service) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
        $scope.defaultAttr = null;
        $scope.getSingleSuccessCallback = async function () {
            var getDefaultAttr = await pagePositionService.getSingle([null, 'portal']);
            if (getDefaultAttr.isSucceed) {
                $scope.defaultAttr = getDefaultAttr.data;
                $scope.defaultAttr.options = [];
            }
            $scope.$apply();
        };
        $scope.saveSuccessCallback = function () {
            $location.url($scope.referrerUrl);
        };
        $scope.saveOthers = async function () {
            var response = await service.saveList($scope.others);
            if (response.isSucceed) {
                $scope.getList();
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        }
    }
]);