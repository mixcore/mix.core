'use strict';
app.controller('AttributeSetDataController',
    [
        '$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$location',
        'AttributeSetDataService', 'CommonService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $location,
            service, commonService) {
            BaseODataCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            $scope.defaultId = 'default';
            $scope.parentId = null;
            $scope.parentType = null;
            $scope.cates = ['Site', 'System'];
            $scope.others=[];
            $scope.settings = $rootScope.globalSettings;
            
            $scope.canDrag = $scope.request.orderBy !== 'Priority' || $scope.request.direction !== '0';
            $scope.init= async function(){
                $scope.attributeSetId = $routeParams.attributeSetId;
                $scope.dataId = $routeParams.dataId;
                
            };
            $scope.saveSuccessCallback = function () {
                if($scope.parentId){
                    $location.url('/portal/attribute-set-data/details?dataId='+ $scope.parentId);
                }
                else{
                    $location.url('/portal/attribute-set-data/list?attributeSetId='+ $scope.activedData.attributeSetId);                    
                }
            };
            $scope.getList = async function () {
                $rootScope.isBusy = true;
                $scope.attributeSetId = $routeParams.attributeSetId;
                var attrSetId = $routeParams.attributeSetId;
                var type = $routeParams.type;
                var parentId = $routeParams.parentId;
                var response = await service.getList('read', $scope.request, attrSetId, type, parentId);
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
                $scope.attributeSetId = $routeParams.attributeSetId;
                var resp = await service.getSingle('portal', [id, $scope.attributeSetId]);
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
            $scope.edit= function(data){
                $scope.goToPath('/portal/attribute-set-data/details?dataId='+ data.id +'&attributeSetId=' + $scope.attributeSetId)
            }
            $scope.remove = function (data) {
                $rootScope.showConfirm($scope, 'removeConfirmed', [data.id], null, 'Remove', 'Are you sure');
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
