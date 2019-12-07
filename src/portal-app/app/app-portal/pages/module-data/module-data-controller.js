'use strict';
app.controller('ModuleDataController',
    ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$location', 'ModuleDataService', 'CommonService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $location, service, commonService) {
            BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            $scope.cates = ['Site', 'System'];
            $scope.others = [];
            $scope.settings = $rootScope.globalSettings;    
            $scope.moduleId = $routeParams.moduleId;
            $scope.getList = async function () {
                $rootScope.isBusy = true;
                $scope.request.query = '&module_id='+$scope.moduleId ;
                var response = await service.getList($scope.request);
                if (response.isSucceed) {
                    $scope.data = response.data;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    $rootScope.showErrors(response.errors);
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
            
        $scope.getSingle = async function () {

            $rootScope.isBusy = true;
            var resp = await service.getSingle($routeParams.id, 'portal');
            if (resp && resp.isSucceed) {
                $scope.activedModuleData = resp.data;
                $rootScope.initEditor();
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
            $scope.remove = function (moduleId, dataId) {
                $rootScope.showConfirm($scope, 'removeConfirmed', [moduleId, dataId], null, 'Remove', 'Are you sure');
            };
        
            $scope.removeConfirmed = async function (moduleId, dataId) {
                $rootScope.isBusy = true;
                var result = await service.delete([moduleId, dataId]);
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
           
            $scope.removeCallback = function () {
            }
            
            $scope.saveOthers = async function(){                
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
        }]);
