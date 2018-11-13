'use strict'
function BaseCtrl($scope, $rootScope, $routeParams, ngAppSettings, service) {

    $scope.request = angular.copy(ngAppSettings.request);
    $scope.contentStatuses = angular.copy(ngAppSettings.contentStatuses);
    $scope.activedData = null;
    $scope.data = null;
    $scope.isInit = false;
    $scope.errors = [];
    $scope.saveCallbackArgs = [];
    $scope.removeCallbackArgs = [];
    $scope.range = $rootScope.range;
    
    $scope.getSingle = async function () {
        $rootScope.isBusy = true;
        var id = $routeParams.id;
        var resp = await service.getSingle([id, 'portal']);
        if (resp && resp.isSucceed) {
            $scope.activedData = resp.data;
            $rootScope.isBusy = false;
            $scope.$apply();
        }
        else {
            if (resp) { $rootScope.showErrors(resp.errors); }
            $rootScope.isBusy = false;
            $scope.$apply();
        }
    };
    $scope.getList = async function (pageIndex) {
        if (pageIndex !== undefined) {
            $scope.request.pageIndex = pageIndex;
        }
        if ($scope.request.fromDate !== null) {
            var d = new Date($scope.request.fromDate);
            $scope.request.fromDate = d.toISOString();
        }
        if ($scope.request.toDate !== null) {
            var d = new Date($scope.request.toDate);
            $scope.request.toDate = d.toISOString();
        }
        var resp = await service.getList($scope.request);
        if (resp && resp.isSucceed) {

            ($scope.data = resp.data);
            $.each($scope.data.items, function (i, data) {

                $.each($scope.activedDatas, function (i, e) {
                    if (e.dataId === data.id) {
                        data.isHidden = true;
                    }
                });
            });
            $rootScope.isBusy = false;
            $scope.$apply();
        }
        else {
            if (resp) { $rootScope.showErrors(resp.errors); }
            $rootScope.isBusy = false;
            $scope.$apply();
        }
    };

    $scope.remove = function (id) {
        $rootScope.showConfirm($scope, 'removeConfirmed', [id], null, 'Remove', 'Are you sure');
    };

    $scope.removeConfirmed = async function (id) {
        $rootScope.isBusy = true;
        var result = await service.delete(id);
        if (result.isSucceed) {
            if ($scope.removeCallback) {
                $rootScope.executeFunctionByName('removeCallback', $scope.removeCallbackArgs, $scope)
            }
            $scope.getList();
        }
        else {
            $rootScope.showMessage('failed');
            $rootScope.isBusy = false;
            $scope.$apply();
        }
    };

    $scope.save = async function (data) {
        $rootScope.isBusy = true;
        var resp = await service.save(data);
        if (resp && resp.isSucceed) {
            $scope.activedData = resp.data;
            $rootScope.showMessage('success', 'success');
            
            if ($scope.saveCallback) {
                $rootScope.executeFunctionByName('saveCallback', $scope.saveCallbackArgs, $scope)
            }
            $rootScope.isBusy = false;
            $scope.$apply();
        }
        else {
            if (resp) { $rootScope.showErrors(resp.errors); }
            $rootScope.isBusy = false;
            $scope.$apply();
        }
    };

}