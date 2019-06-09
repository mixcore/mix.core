'use strict';
app.controller('AttributeSetController', [
    '$scope', '$rootScope', '$location',
    'ngAppSettings', '$routeParams', 'AttributeFieldService', 'AttributeSetService',      
    function ($scope, $rootScope, $location, 
        ngAppSettings, $routeParams, attributeFieldService, service) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
        $scope.defaultAttr = null;
        $scope.getSingleSuccessCallback = async function () {
            var getDefaultAttr = await attributeFieldService.getSingle([null, 'portal']);
            if (getDefaultAttr.isSucceed) {
                $scope.defaultAttr = getDefaultAttr.data;
                $scope.defaultAttr.options = [];
            }
            $scope.$apply();
        }
        $scope.saveCallback = function () {
            $location.url($scope.referrerUrl);
        };
    }
]);