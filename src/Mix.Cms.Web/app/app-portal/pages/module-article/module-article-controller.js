'use strict';
app.controller('ModuleArticleController',
    ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$location', 'ModuleArticleService', 'CommonService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $location, service, commonService) {
            BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            $scope.cates = ['Site', 'System'];
            $scope.others = [];
            $scope.settings = $rootScope.globalSettings;    
            $scope.moduleId = $routeParams.id;
            $scope.getList = async function () {
                $rootScope.isBusy = true;
                var id = $routeParams.id;
                $scope.request.query = '&module_id='+id;
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
            $scope.remove = function (moduleId, articleId) {
                $rootScope.showConfirm($scope, 'removeConfirmed', [moduleId, articleId], null, 'Remove', 'Are you sure');
            };
        
            $scope.removeConfirmed = async function (moduleId, articleId) {
                $rootScope.isBusy = true;
                var result = await service.delete(moduleId, articleId);
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
        
            $scope.saveCallback = function () {               
            }
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
