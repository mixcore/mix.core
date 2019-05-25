'use strict';
app.controller('ModuleGalleryController',
    ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$location', 'ModuleGalleryService', 'CommonService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $location, service, commonService) {
            BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            $scope.cates = ['Site', 'System'];
            $scope.others = [];
            $scope.settings = $rootScope.globalSettings;
            $scope.moduleId = $routeParams.id;
            $scope.canDrag = $scope.request.orderBy !== 'Priority' || $scope.request.direction !== '0';
            $scope.translate = $rootScope.translate;
            $scope.moduleId = $routeParams.id;
            $scope.getList = async function () {
                $rootScope.isBusy = true;
                var id = $routeParams.id;
                $scope.moduleId = $routeParams.id;
                $scope.request.query = '&module_id=' + id;
                $scope.canDrag = $scope.request.orderBy !== 'Priority' || $scope.request.direction !== '0';
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
            $scope.updateInfos = async function (index) {
                $scope.data.items.splice(index, 1);
                $rootScope.isBusy = true;
                var startIndex = $scope.data.items[0].priority-1;
                for (var i = 0; i < $scope.data.items.length; i++) {
                    $scope.data.items[i].priority = startIndex + i + 1;
                }
                var resp = await service.updateInfos($scope.data.items);
                if (resp && resp.isSucceed) {
                    $scope.activedPage = resp.data;
                    $rootScope.showMessage('success', 'success');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            }
        }]);
