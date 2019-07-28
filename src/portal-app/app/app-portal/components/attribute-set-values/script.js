modules.component('attributeSetValues', {
    templateUrl: '/app/app-portal/components/attribute-set-values/view.html',
    bindings: {
        header: '=',
        data: '=',
        columns: '=?',
        onUpdate:'&?',
        onDelete:'&?',
    },
    controller: ['$rootScope', '$scope',
        function ($rootScope, $scope) {
            var ctrl = this;
            ctrl.selectedProp = null;
            
            ctrl.settings = $rootScope.globalSettings;
            ctrl.$onInit = function(){
                if(ctrl.data.length && !ctrl.columns){
                    ctrl.columns = ctrl.data[0].data;
                }
            };

            ctrl.update = function(data){
                ctrl.onUpdate({data: data});
            };
            
            ctrl.delete = function(data){
                ctrl.onDelete({data: data});
            };

            ctrl.filterData = function(item, attributeName){
                return $rootScope.findObjectByKey(item.data, 'attributeName', attributeName);
            };

            ctrl.dragStart = function(index){
                ctrl.dragStartIndex = index;
            };
            ctrl.updateOrders = function(index){
                if(index> ctrl.dragStartIndex){
                    ctrl.data.splice(ctrl.dragStartIndex, 1);
                }
                else{
                    ctrl.data.splice(ctrl.dragStartIndex+1, 1);
                }
                angular.forEach(ctrl.data, function(e,i){
                    e.priority = i;
                });
            };
        }]
});