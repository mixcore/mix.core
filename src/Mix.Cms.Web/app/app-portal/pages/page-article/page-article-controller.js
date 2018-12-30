'use strict';
app.controller('PageArticleController',
    [
        '$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$location',
        'PageArticleService', 'ArticleService', 'CommonService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $location,
            service, articleService, commonService) {
            BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            $scope.cates = ['Site', 'System'];
            $scope.settings = $rootScope.globalSettings;
            $scope.pageId = $routeParams.id;
            $scope.othersRequest = angular.copy(ngAppSettings.request);
            $scope.othersRequest.query = "&not_page_id=" + $routeParams.id;
            $scope.others = [];
            $scope.getList = async function () {
                $rootScope.isBusy = true;
                var id = $routeParams.id;
                $scope.request.query = '&page_id=' + id;
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
            $scope.remove = function (pageId, articleId) {
                $rootScope.showConfirm($scope, 'removeConfirmed', [pageId, articleId], null, 'Remove', 'Are you sure');
            };

            $scope.removeConfirmed = async function (pageId, articleId) {
                $rootScope.isBusy = true;
                var result = await service.delete(pageId, articleId);
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

            $scope.loadOthers = async function (pageIndex) {
                $scope.othersRequest.pageIndex = pageIndex;
                $scope.others = [];
                var response = await articleService.getList($scope.othersRequest);
                if (response.isSucceed) {
                    angular.forEach(response.data.items, function (e) {
                        $scope.others.push({
                            priority: e.priority,
                            description: e.title,
                            articleId: e.id,
                            categoryId: $scope.pageId,
                            image: e.thumbnailUrl,
                            specificulture: $rootScope.configurationService.get('lang'),
                            article: e,
                            status: 2,
                            isActived: false
                        });
                    });
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    $rootScope.showErrors(response.errors);
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            }
            $scope.saveOthers = async function(){
                var arr = $rootScope.filterArray($scope.others, 'isActived', true);
                var response = await service.saveList(arr);
                if (response.isSucceed) {
                    $scope.loadOthers();
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
