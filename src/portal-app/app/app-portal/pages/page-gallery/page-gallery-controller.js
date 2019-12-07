'use strict';
app.controller('PageGalleryController',
    [
        '$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$location',
        'PageGalleryService', 'PostService', 'CommonService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $location,
            service, postService, commonService) {
            BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            $scope.cates = ['Site', 'System'];
            $scope.others=[];
            $scope.settings = $rootScope.globalSettings;
            $scope.pageId = $routeParams.id;
            $scope.canDrag = $scope.request.orderBy !== 'Priority' || $scope.request.direction !== '0';
            $scope.getList = async function () {
                $rootScope.isBusy = true;
                var id = $routeParams.id;
                $scope.request.query = '&page_id=' + id;
                var response = await service.getList($scope.request);
                $scope.canDrag = $scope.request.orderBy !== 'Priority' || $scope.request.direction !== '0';
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
            $scope.remove = function (pageId, postId) {
                $rootScope.showConfirm($scope, 'removeConfirmed', [pageId, postId], null, 'Remove', 'Are you sure');
            };

            $scope.removeConfirmed = async function (pageId, postId) {
                $rootScope.isBusy = true;
                var result = await service.delete([pageId, postId]);
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

            $scope.saveSuccessCallback = function () {
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
