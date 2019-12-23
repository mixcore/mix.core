modules.component('addictionalValue', {
    templateUrl: '/app/app-portal/components/addictional-value/view.html',
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
                $rootScope.showConfirm(ctrl, 'removeAttributeConfirmed', [val, index], null, 'Remove Field', 'Deleted data will not able to recover, are you sure you want to delete this item?');
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