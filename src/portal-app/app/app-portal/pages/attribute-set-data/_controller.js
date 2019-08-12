'use strict';
app.controller('AttributeSetDataController',
    [
        '$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$location',
        'PagePostService', 'PostService', 'CommonService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $location,
            service, postService, commonService) {
            BaseODataCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            $scope.cates = ['Site', 'System'];
            $scope.others=[];
            $scope.settings = $rootScope.globalSettings;
            $scope.attributeSetId = $routeParams.id;
            $scope.canDrag = $scope.request.orderBy !== 'Priority' || $scope.request.direction !== '0';
            $scope.getList = async function () {
                $rootScope.isBusy = true;
                var id = $routeParams.id;
                $scope.request.query = '&attribute_set_id=' + id;
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
            $scope.preview = function (item) {
                item.editUrl = '/portal/post/details/' + item.id;
                $rootScope.preview('post', item, item.title, 'modal-lg');
            };
            $scope.remove = function (attributeSetId, postId) {
                $rootScope.showConfirm($scope, 'removeConfirmed', [attributeSetId, postId], null, 'Remove', 'Are you sure');
            };

            $scope.removeConfirmed = async function (attributeSetId, postId) {
                $rootScope.isBusy = true;
                var result = await service.delete(attributeSetId, postId);
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
            };
        }]);
