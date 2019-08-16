'use strict';
app.controller('AttributeSetDataController',
    [
        '$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$location',
        'AttributeSetDataService', 'CommonService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $location,
            service, commonService) {
            BaseODataCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            $scope.defaultId = 'default';
            $scope.cates = ['Site', 'System'];
            $scope.others=[];
            $scope.settings = $rootScope.globalSettings;
            
            $scope.canDrag = $scope.request.orderBy !== 'Priority' || $scope.request.direction !== '0';
            $scope.getList = async function () {
                $rootScope.isBusy = true;
                $scope.attributeSetId = $routeParams.attributeSetId;
                var type = $routeParams.type;
                var parentId = $routeParams.parentId;
                var attrSetId = $routeParams.attributeSetId;
                $scope.request.filter  = '';
                if(attrSetId){
                    $scope.request.filter += 'attributeSetId eq ' + attrSetId;
                }
                if(type){
                    if($scope.request.filter){
                        $scope.request.filter += ' and ';
                    }
                    $scope.request.filter += 'parentType eq ' + type;
                }
                if(parentId){
                    if($scope.request.filter){
                        $scope.request.filter += ' and ';
                    }
                    $scope.request.filter += 'parentId eq ' + parentId;
                }
                
                var response = await service.getList('read', $scope.request);
                $scope.canDrag = $scope.request.orderBy !== 'Priority' || $scope.request.direction !== '0';
                if (response) {
                    $scope.data = response;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    $rootScope.showErrors('Failed');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
            $scope.getSingle = async function () {
                $rootScope.isBusy = true;
                var id = $routeParams.id || $scope.defaultId;
                var attributeSetId = $routeParams.attributeSetId;
                var resp = await service.getSingle('portal', [id, attributeSetId]);
                if (resp) {
                    $scope.activedData = resp;
                    if ($scope.getSingleSuccessCallback) {
                        $scope.getSingleSuccessCallback();
                    }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                } else {
                    if (resp) {
                        $rootScope.showErrors('Failed');
                    }
                    if ($scope.getSingleFailCallback) {
                        $scope.getSingleFailCallback();
                    }
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
