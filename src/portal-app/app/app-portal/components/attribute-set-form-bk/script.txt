modules.component('attributeSetForm', {
    templateUrl: '/app/app-portal/components/attribute-set-form/view.html',
    bindings: {
        setId: '=',
        data: '=',
        attributes: '=',
        defaultData: '=?',
        saveData: '&?'
    },
    controller: ['$rootScope', '$scope', 'AttributeDataService',
        function ($rootScope, $scope, service) {
            var ctrl = this;
            ctrl.attributes = [];
            ctrl.selectedProp = null;
            ctrl.settings = $rootScope.globalSettings;
            ctrl.$onInit = async function () {
                if (!ctrl.defaultData) {
                    var getData = await service.getSingle(['post', ctrl.setId, 'portal']);
                    if (getData.isSucceed) {
                        ctrl.defaultData = getData.data;
                        if (!ctrl.data) {
                            ctrl.data = angular.copy(ctrl.defaultData);
                        }
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    } else {
                        if (getData) {
                            $rootScope.showErrors(getData.errors);
                        }
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }

                }else{
                    if (!ctrl.data) {
                        ctrl.data = angular.copy(ctrl.defaultData);
                    }
                }
            };
            ctrl.submit = async function () {
               
                if (ctrl.saveData) {
                    var result = await ctrl.saveData({ data: ctrl.data });
                    if(result.isSucceed){
                        ctrl.data = angular.copy(ctrl.defaultData);
                    }
                }
                else {
                    angular.forEach(ctrl.data.data, function(e){                    
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