'use strict';
app.controller('PositionController', [
    '$scope', '$rootScope', '$location',
    'ngAppSettings', '$routeParams', 'PagePositionService', 'PositionService',      
    function ($scope, $rootScope, $location, 
        ngAppSettings, $routeParams, pagePositionService, service) {
        BaseODataCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
        $scope.defaultAttr = null;
        $scope.defaultData = null;
        $scope.request.selects = 'id,description';
        $scope.orders = [{ title: 'Id', value: 'id' }, { title: 'Description', value: 'description' }];
        $scope.request.orderBy = 'id';

        $scope.$on('$viewContentLoaded', function () {            
        });
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
        };
    }
]);