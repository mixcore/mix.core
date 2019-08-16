modules.component('attributeSetForm', {
    templateUrl: '/app/app-portal/components/attribute-set-form/view.html',
    bindings: {
        setId: '=',
        attrDataId: '=',
        attrData: '=?',
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
                if(ctrl.attrDataId){
                    ctrl.data = await service.getSingle('portal', [ctrl.attrDataId, ctrl.setId]);
                    if (ctrl.data) {
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    } else {
                        if (ctrl.data) {
                            $rootScope.showErrors('Failed');
                        }
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                }
                else{
                    ctrl.data = await service.getSingle('portal', [ctrl.defaultId, ctrl.setId]);
                    if (ctrl.data) {
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    } else {
                        $rootScope.showErrors('Failed');
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                }
            };
            ctrl.submit = async function () {
               
                if (ctrl.saveData) {
                    var result = await ctrl.saveData({ data: ctrl.data });
                    if(result.isSucceed){
                        ctrl.data = await service.getSingle('portal', [ctrl.defaultId, ctrl.setId]);
                        if (ctrl.data) {
                            $rootScope.isBusy = false;
                            $scope.$apply();
                        } else {
                            $rootScope.showErrors('Failed');
                            $rootScope.isBusy = false;
                            $scope.$apply();
                        }
                    }
                }
                else {
                    angular.forEach(ctrl.data.values, function(e){                    
                        //Encrypt field before send
                        if(e.field.isEncrypt){
                            var encryptData = $rootScope.encrypt(e.stringValue);
                            e.encryptKey = encryptData.key;
                            e.encryptValue = encryptData.data;
                            e.stringValue = null;
                        }
                    });
                    var saveResult = await service.save(ctrl.data);
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
                if(ctrl.data){
                    var attr =  $rootScope.findObjectByKey(ctrl.data.data, 'attributeName', attributeName);
                    if (!attr){
                        attr = angular.copy($rootScope.findObjectByKey(ctrl.defaultData.data, 'attributeName', attributeName));
                        ctrl.data.data.push(attr);
                    }
                    return attr;
                }
            };
        }]
});