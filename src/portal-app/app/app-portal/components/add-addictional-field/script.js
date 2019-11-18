modules.component('addAddictionalField', {
    templateUrl: '/app/app-portal/components/add-addictional-field/view.html',
    bindings: {                
        model: '='
    },
    controller: ['$rootScope', '$scope', 'BaseService',
        function ($rootScope, $scope, baseService ) {
            var ctrl = this;
            var valueService = angular.copy(baseService);
            valueService.init('attribute-set-value');
            ctrl.value = {};
            ctrl.field = { dataType: 7 };
            ctrl.selectedCol = null;
            ctrl.settings = $rootScope.globalSettings;
            ctrl.$onInit = async function () {
            };
            ctrl.addAttr = function () {
                
                if (ctrl.field.name) {
                    var current = $rootScope.findObjectByKey(ctrl.model.attributeData.data.values, 'attributeFieldName', ctrl.field.name);
                    if (current) { 
                        $rootScope.showErrors(['Field ' + ctrl.field.name + ' existed!']);
                    }
                    else {
                        var t = angular.copy(ctrl.field);
                        t.priority = ctrl.model.attributeData.data.values.length + 1;
                        ctrl.value.attributeFieldName = ctrl.field.name;
                        ctrl.value.dataType = ctrl.field.dataType;
                        ctrl.value.priority = t.priority;
                        ctrl.value.field = t;
                        ctrl.model.attributeData.data.values.push(ctrl.value);

                        //reset field option
                        ctrl.field.title = '';
                        ctrl.field.name = '';
                        ctrl.field.dataType = 0;
                    }
                }
                else {
                    $rootScope.showErrors(['Please Add Field Name']);
                }
            };

            
            ctrl.generateName = function (col) {
                col.name = $rootScope.generateKeyword(col.title, '_');
            };

            ctrl.updateOrders = function (index) {
                if (index > ctrl.dragStartIndex) {
                    ctrl.fields.splice(ctrl.dragStartIndex, 1);
                }
                else {
                    ctrl.fields.splice(ctrl.dragStartIndex + 1, 1);
                }
                angular.forEach(ctrl.fields, function (e, i) {
                    e.priority = i;
                });
            };

            ctrl.dragStart = function (index) {
                ctrl.dragStartIndex = index;
            };

            ctrl.removeAttribute = function (val, index) {
                $rootScope.showConfirm(ctrl, 'removeAttributeConfirmed', [val, index], null, 'Remove Field', 'Are you sure');
            };
            ctrl.removeAttributeConfirmed = async function (val, index) {
                if(val.id){
                    $rootScope.isBusy = true;
                    var result = await valueService.delete([val.id]);
                    if(result.isSucceed){
                        ctrl.model.attributeData.data.values.splice(index, 1);
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                    else{
                        $rootScope.showErrors(result.errors);
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                }
                else{
                    ctrl.model.attributeData.data.values.splice(index, 1);
                }
            };

        }]

});