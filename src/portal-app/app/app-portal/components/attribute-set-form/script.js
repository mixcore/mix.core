modules.component('attributeSetForm', {
    templateUrl: '/app/app-portal/components/attribute-set-form/view.html',
    bindings: {
        attrSetId: '=',
        attrSetName: '=',
        attrDataId: '=?',
        attrData: '=?',
        // parentType: '=?', // attribute set = 1 | post = 2 | page = 3 | module = 4
        // parentId: '=?',
        defaultId: '=',
        saveData: '&?'
    },
    controller: ['$rootScope', '$scope', 'AttributeSetDataService',
        function ($rootScope, $scope, service) {
            var ctrl = this;
            ctrl.attributes = [];
            ctrl.selectedProp = null;
            ctrl.settings = $rootScope.globalSettings;
            ctrl.$onInit = async function () {
                ctrl.loadData();
            };
            ctrl.loadData = async function () {
                $rootScope.isBusy = true;
                /*
                    If input is data id => load ctrl.attrData from service and handle it independently
                    Else modify input ctrl.attrData
                */

                if(ctrl.attrDataId){
                    ctrl.attrData = await service.getSingle('portal', [ctrl.attrDataId, ctrl.attrSetId, ctrl.attrSetName]);
                    if (ctrl.attrData) {
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    } else {
                        if (ctrl.attrData) {
                            $rootScope.showErrors('Failed');
                        }
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                }
                else{
                    if(!ctrl.attrData){
                        ctrl.attrData = await service.getSingle('portal', [ctrl.defaultId, ctrl.attrSetId, ctrl.attrSetName]);
                        if (ctrl.attrData) {
                            $rootScope.isBusy = false;
                            $scope.$apply();
                        } else {
                            $rootScope.showErrors('Failed');
                            $rootScope.isBusy = false;
                            $scope.$apply();
                        }
                    }
                }
            };
            ctrl.reload = async function () {
                $rootScope.isBusy = true;
               ctrl.attrData = await service.getSingle('portal', [ctrl.defaultId, ctrl.attrSetId, ctrl.attrSetName]);
                if (ctrl.attrData) {
                    $rootScope.isBusy = false;
                    $scope.$apply();
                } else {
                    $rootScope.showErrors('Failed');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
            ctrl.submit = async function () {
                angular.forEach(ctrl.attrData.values, function(e){                    
                    //Encrypt field before send
                    if(e.field.isEncrypt){
                        var encryptData = $rootScope.encrypt(e.stringValue);
                        e.encryptKey = encryptData.key;
                        e.encryptValue = encryptData.data;
                        e.stringValue = null;
                    }
                });
                if (ctrl.saveData) {
                    var result = await ctrl.saveData({ data: ctrl.attrData });
                    if (result && result.isSucceed) {
                        ctrl.attrData = result.data;
                        $scope.$apply();
                    }
                    else{
                        ctrl.attrData = await service.getSingle('portal', [ctrl.defaultId, ctrl.attrSetId, ctrl.attrSetName]);
                        $scope.$apply();
                    }
                }
                else {
                   
                    var saveResult = await service.save(ctrl.attrData);
                    if (saveResult.isSucceed) {

                    } else {
                        if (saveResult) {
                            $rootScope.showErrors(saveResult.errors);
                        }
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }

                }
            };
           
            ctrl.filterData = function (attributeName) {
                if(ctrl.attrData){
                    var attr =  $rootScope.findObjectByKey(ctrl.attrData.data, 'attributeFieldName', attributeName);
                    if (!attr){
                        attr = angular.copy($rootScope.findObjectByKey(ctrl.defaultData.data, 'attributeFieldName', attributeName));
                        ctrl.attrData.data.push(attr);
                    }
                    return attr;
                }
            };
        }]
});