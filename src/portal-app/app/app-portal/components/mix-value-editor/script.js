
modules.component('mixValueEditor', {
    templateUrl: '/app/app-portal/components/mix-value-editor/view.html',
    bindings: {
        title: '=?',
        isSelect: '=?',
        stringValue: '=',
        type: '='
    },
    controller: ['$rootScope', '$scope', 'ngAppSettings', '$location', 'AttributeSetDataService', 
        function ($rootScope, $scope, ngAppSettings,$location, dataService) {
        var ctrl = this;
        ctrl.icons = ngAppSettings.icons;
        ctrl.refData = [];
        ctrl.refRequest = angular.copy(ngAppSettings.request);
        ctrl.refRequest.pageSize = 100;
        ctrl.dataTypes = $rootScope.globalSettings.dataTypes;
        ctrl.previousId = null;
        ctrl.initData = async function(){
            setTimeout(() => {
                switch (ctrl.dataType) {
                    case 1:
                    case 2:
                    case 3:
                        if (ctrl.dateTimeValue) {
                            ctrl.dateObj = new Date(ctrl.dateTimeValue);
                            $scope.$apply();
                        }
                        break;
                    case 23: // reference
                        if(ctrl.referenceId){
                            dataService.getList('read', ctrl.refRequest, ctrl.referenceId, ctrl.parentType, ctrl.parentId).then(resp=>{
                                if (resp) {
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
                        break;
                    default:
                        if (ctrl.isEncrypt && ctrl.encryptValue) {
                            var encryptedData = {
                                key: ctrl.encryptKey,
                                data: ctrl.encryptValue
                            };
                            ctrl.stringValue = $rootScope.decrypt(encryptedData);
                        }
                        if (!ctrl.stringValue) {
                            ctrl.stringValue = ctrl.defaultValue;
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
                    if (ctrl.dateObj) {
                        ctrl.dateTimeValue = ctrl.dateObj.toISOString();
                        ctrl.stringValue = ctrl.dateTimeValue;
                    }
                    break;
                case 6:
                    if (ctrl.doubleValue) {
                        ctrl.stringValue = ctrl.doubleValue.toString();
                    }
                    break;
                case 18:
                    if (ctrl.booleanValue) {
                        ctrl.stringValue = ctrl.booleanValue.toString();
                    }
                    break;

                default:
                    break;
            }
        };
        ctrl.updateRefData = function(item){
            $location.url('/portal/attribute-set-data/details?dataId='+ item.id +'&attributeSetId=' + item.attributeSetId+'&parentType=' + item.parentType+'&parentId=' + item.parentId);
        };
        ctrl.removeRefData = async function(data){
            $rootScope.showConfirm(ctrl, 'removeRefDataConfirmed', [data.id], null, 'Remove', 'Are you sure');
        };
        ctrl.removeRefDataConfirmed = async function(dataId){
            $rootScope.isBusy = true;
            var result = await dataService.delete(dataId);
            if (result.isSucceed) {
                $rootScope.removeObjectByKey(ctrl.refData, 'id', dataId);
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