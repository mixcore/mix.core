'use strict';
app.controller('PageController', 
            ['$scope', '$rootScope', '$routeParams', 'ngAppSettings', 'PageService',
    function ($scope, $rootScope, $routeParams, ngAppSettings, service) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);        
        $scope.request.query = 'level=0';       
        
        $scope.loadPageDatas = async function () {
            $rootScope.isBusy = true;
            var id = $routeParams.id;
            var response = await service.getSingle(id, 'portal');
            if (response.isSucceed) {
                $scope.activedData = response.data;
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.updateInfos = async function (items) {
            $rootScope.isBusy = true;
            var resp = await service.updateInfos(items);
            if (resp && resp.isSucceed) {
                $scope.activedData = resp.data;
                $rootScope.showMessage('success', 'success');
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

    }]);
