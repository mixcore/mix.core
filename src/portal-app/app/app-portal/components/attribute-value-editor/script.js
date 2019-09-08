
modules.component('attributeValueEditor', {
    templateUrl: '/app/app-portal/components/attribute-value-editor/view.html',
    bindings: {
        attributeValue: '=?',
        parentType: '=?',
        parentId: '=?',
        isShowTitle: '=?',
    },
    controller: ['$rootScope', '$scope', 'ngAppSettings', '$location', 'RelatedAttributeSetDataService', 'AttributeSetDataService', 
        function ($rootScope, $scope, ngAppSettings,$location, navService,dataService) {
        var ctrl = this;
        ctrl.icons = ngAppSettings.icons;
        ctrl.refData = [];
        ctrl.defaultDataModel = null;
        ctrl.refDataModel = {
            id: null,
            data: null
        };
        ctrl.refRequest = angular.copy(ngAppSettings.request);
        ctrl.refRequest.pageSize = 100;
        ctrl.dataTypes = $rootScope.globalSettings.dataTypes;
        ctrl.previousId = null;
        ctrl.$doCheck = function () {
            if (ctrl.attributeValue && ctrl.previousId !== ctrl.attributeValue.id) {
                ctrl.previousId = ctrl.attributeValue.id;
                ctrl.initData();
            }
        }.bind(ctrl);
        ctrl.$onInit = function () {
        };
        ctrl.initData = async function(){
            setTimeout(() => {
                switch (ctrl.attributeValue.field.dataType) {
                    case 1:
                    case 2:
                    case 3:
                        if (ctrl.attributeValue.datetimeValue) {
                            ctrl.attributeValue.dateObj = new Date(ctrl.attributeValue.datetimeValue);
                            $scope.$apply();
                        }
                        break;
                    case 23: // reference
                        if(ctrl.attributeValue.field.referenceId){
                            navService.getSingle('portal', [ctrl.parentId, ctrl.parentType, 'default']).then(resp=>{
                                ctrl.defaultDataModel = resp;
                                ctrl.refDataModel = angular.copy(ctrl.defaultDataModel);
                            });
                            ctrl.loadRefData();
                        }
                        break;
                    default:
                        if (ctrl.attributeValue.field.isEncrypt && ctrl.attributeValue.encryptValue) {
                            var encryptedData = {
                                key: ctrl.attributeValue.encryptKey,
                                data: ctrl.attributeValue.encryptValue
                            };
                            ctrl.attributeValue.stringValue = $rootScope.decrypt(encryptedData);
                        }
                        if (!ctrl.attributeValue.stringValue) {
                            ctrl.attributeValue.stringValue = ctrl.attributeValue.field.defaultValue;
                            $scope.$apply();
                        }
                        break;
                }
            }, 200);
        };
        ctrl.updateStringValue = async function (dataType) {
            switch (dataType) {
                case 1:
                case 2:
                case 3:
                    if (ctrl.attributeValue.dateObj) {
                        ctrl.attributeValue.datetimeValue = ctrl.attributeValue.dateObj.toISOString();
                        ctrl.attributeValue.stringValue = ctrl.attributeValue.datetimeValue;
                    }
                    break;
                case 6:
                    if (ctrl.attributeValue.doubleValue) {
                        ctrl.attributeValue.stringValue = ctrl.attributeValue.doubleValue.toString();
                    }
                    break;
                case 18:
                    if (ctrl.attributeValue.booleanValue) {
                        ctrl.attributeValue.stringValue = ctrl.attributeValue.booleanValue.toString();
                    }
                    break;

                default:
                    break;
            }
        };
        ctrl.loadRefData = function(){
            navService.getList('portal', ctrl.refRequest, 
                ctrl.attributeValue.field.referenceId, ctrl.parentType, ctrl.parentId)
                .then(resp=>{
                if (resp) 
                {
                    ctrl.refData = resp;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                } else {
                    if (resp) {
                        $rootScope.showErrors('Failed');
                    }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            });
        }
        ctrl.updateRefData = function(nav){
            ctrl.refDataModel = nav;
            // $location.url('/portal/attribute-set-data/details?dataId='+ item.id +'&attributeSetId=' + item.attributeSetId+'&parentType=' + item.parentType+'&parentId=' + item.parentId);
        };
        ctrl.saveRefData = function(data){            
            $rootScope.isBusy = true;
            ctrl.refDataModel.data = data;
            dataService.save('portal', data).then(resp=>{
                if(resp.isSucceed){
                    ctrl.refDataModel.id = resp.data.id;
                    ctrl.refDataModel.data = resp.data;
                    navService.save('portal', ctrl.refDataModel).then(resp=>{
                        if(resp.isSucceed){
                            var tmp = $rootScope.findObjectByKey(ctrl.refData, ['parentId', 'parentType', 'id'], 
                                [resp.data.parentId, resp.data.parentType, resp.data.id]);
                            if(!tmp){
                                ctrl.refData.push(resp.data);
                            }
                            ctrl.refDataModel = angular.copy(ctrl.defaultDataModel);
                            $rootScope.isBusy = false;
                            $scope.$apply();
                        }else{
                            $rootScope.showMessage('failed');    
                            $rootScope.isBusy = false;
                            $scope.$apply();
                        }
                    })
                    
                }
                else{
                    $rootScope.showMessage('failed');    
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                console.log(resp);
            })
        }
        ctrl.removeRefData = async function(nav){
            $rootScope.showConfirm(ctrl, 'removeRefDataConfirmed', [nav], null, 'Remove', 'Are you sure');
        };
        ctrl.removeRefDataConfirmed = async function(nav){
            $rootScope.isBusy = true;
            var result = await navService.delete([nav.parentId, nav.parentType, nav.id]);
            if (result.isSucceed) {
                $rootScope.removeObjectByKey(ctrl.refData, 'id', nav.id);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.showMessage('failed');
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
    }]
});