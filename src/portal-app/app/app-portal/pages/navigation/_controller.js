'use strict';
app.controller('NavigationController',
    [
        '$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$location',
        'NavigationService', 'RelatedAttributeSetDataService', 'CommonService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $location,
            service, navService, commonService) {
            BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            $scope.defaultId = 'default';
            $scope.parentId = null;
            $scope.parentType = null;
            $scope.cates = ['Site', 'System'];
            $scope.others = [];
            $scope.settings = $rootScope.globalSettings;
            $scope.canDrag = $scope.request.orderBy !== 'Priority' || $scope.request.direction !== '0';
            $scope.init = async function () {
                $scope.attributeSetId = $routeParams.attributeSetId;
                $scope.attributeSetName = $routeParams.attributeSetName;
                $scope.dataId = $routeParams.dataId;
                $scope.refParentId = $routeParams.refParentId;
                $scope.refParentType = $routeParams.refParentType;
                if ($scope.refParentId && $scope.refParentType) {
                    $scope.refDataModel = {
                        parentId: $scope.refParentId,
                        parentType: $scope.refParentType
                    };
                }
            };
            $scope.saveSuccessCallback = function () {                
                if ($scope.refDataModel) {
                    $scope.refDataModel.id = $scope.activedData.id;
                    $scope.refDataModel.attributeSetId = $scope.activedData.attributeSetId;
                    $scope.refDataModel.attributeSetName = $scope.activedData.attributeSetName;
                    $scope.refDataModel.specificulture = $scope.activedData.specificulture;
                    $scope.refDataModel.data = $scope.activedData;
                    $rootScope.isBusy = true;
                    navService.save('portal', $scope.refDataModel).then(resp => {
                        if (resp.isSucceed) {
                            $rootScope.isBusy = false;
                            if($scope.parentId){
                                $location.url('/portal/navigation/details?dataId='+ $scope.parentId);
                            }
                            else{
                                $location.url('/portal/navigation/list?attributeSetId='+ $scope.activedData.attributeSetId);                    
                            }
                            $scope.$apply();
                        } else {
                            $rootScope.showMessage('failed');
                            $rootScope.isBusy = false;
                            $scope.$apply();
                        }
                    });
                }
            };
            $scope.getList = async function (page = 0) {
                $rootScope.isBusy = true;
                $scope.attributeSetId = $routeParams.attributeSetId;
                $scope.attributeSetName = $routeParams.attributeSetName;
                if (page != undefined) {
                    $scope.request.pageIndex = page;
                }
                var type = $routeParams.type;
                var parentId = $routeParams.parentId;
                var response = await service.getList('read', $scope.request, $scope.attributeSetId, $scope.attributeSetName, type, parentId);
                $scope.canDrag = $scope.request.orderBy !== 'Priority' || $scope.request.direction !== '0';
                if (response) {
                    $scope.data = response;
                    $scope.count([$routeParams.attributeSetName]);
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
                $scope.attributeSetId = $routeParams.attributeSetId;
                $scope.attributeSetName = $routeParams.attributeSetName;
                var resp = await service.getSingle('portal', [id, $scope.attributeSetId, $scope.attributeSetName]);
                if (resp) {
                    $scope.activedData = resp;
                    $scope.activedData.parentType = $scope.parentType;
                    $scope.activedData.parentId = $scope.parentId;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                } else {
                    if (resp) {
                        $rootScope.showErrors('Failed');
                    }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
            $scope.preview = function (item) {
                item.editUrl = '/portal/post/details/' + item.id;
                $rootScope.preview('post', item, item.title, 'modal-lg');
            };
            $scope.edit = function (data) {
                $scope.goToPath('/portal/attribute-set-data/details?dataId=' + data.id + '&attributeSetId=' + $scope.attributeSetId+ '&attributeSetName=' + $scope.attributeSetId)
            };
            $scope.remove = function (data) {
                $rootScope.showConfirm($scope, 'removeConfirmed', [data.id], null, 'Remove', 'Deleted data will not able to recover, are you sure you want to delete this item?');
            };

            $scope.removeConfirmed = async function (dataId) {
                $rootScope.isBusy = true;
                var result = await service.delete([dataId]);
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
            };

        }]);
