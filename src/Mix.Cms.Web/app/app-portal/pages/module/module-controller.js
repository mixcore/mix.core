'use strict';
app.controller('ModuleController', ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', 
    'ModuleService', 'ModuleDataService',
    function ($scope, $rootScope, ngAppSettings, $routeParams, moduleServices, moduleDataService) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, moduleServices, 'product');
        $scope.defaultAttr = {
            name: '',
            options: [],
            priority: 0,
            dataType: 0,
            isGroupBy: false,
            isSelect: false,
            isDisplay: true,
            width: 3
        };
        $scope.dataTypes = ngAppSettings.dataTypes;
        $scope.activedData = null;

        $scope.loadModuleDatas = async function () {
            $rootScope.isBusy = true;
            var id = $routeParams.id;
            var response = await moduleServices.getSingle([id, 'portal']);
            if (response.isSucceed) {
                $scope.activedData = response.data;
                $rootScope.initEditor();
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.loadMoreModuleDatas = async function (pageIndex) {
            $scope.request.key = $scope.activedData.id;
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
            $rootScope.isBusy = true;
            var resp = await moduleDataService.getModuleDatas($scope.request);
            if (resp && resp.isSucceed) {

                $scope.activedData.data = resp.data;
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
        $scope.addAttr = function () {
            if ($scope.activedData) {
                var t = angular.copy($scope.defaultAttr);
                $scope.activedData.columns.push(t);
            }
        };

        $scope.addOption = function (col, index) {
            var val = angular.element('#option_' + index).val();
            col.options.push(val);
            angular.element('#option_' + index).val('');
        };

        $scope.generateName = function(col){
            col.name =  $rootScope.generateKeyword(col.title, '_');
        }
        $scope.removeAttr = function (index) {
            if ($scope.activedData) {

                $scope.activedData.columns.splice(index, 1);
            }
        }

        $scope.removeData = function (id) {
            if ($scope.activedData) {
                $rootScope.showConfirm($scope, 'removeDataConfirmed', [id], null, 'Remove Data', 'Are you sure');
            }
        }

        $scope.removeDataConfirmed = async function (id) {
            $rootScope.isBusy = true;
            var result = await moduleDataService.removeModuleData(id);
            if (result.isSucceed) {
                $scope.loadModuleDatas();
            }
            else {
                $rootScope.showMessage('failed');
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        }
        $scope.updateModuleDataField = async function (item, propertyName){
            var result = await moduleDataService.saveFields(item.id, propertyName, item[propertyName]);
            if (result.isSucceed) {
                $scope.loadModuleDatas();
            }
            else {
                $rootScope.showMessage('failed');
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        }
    }]);
