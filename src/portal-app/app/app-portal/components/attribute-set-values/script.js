modules.component('attributeSetValues', {
    templateUrl: '/app/app-portal/components/attribute-set-values/view.html',
    bindings: {
        header: '=',
        data: '=',
        selectedList: '=',
        columns: '=?',
        onApplyList:'&?',
        onUpdate:'&?',
        onDelete:'&?',
    },
    controller: ['$rootScope', '$scope', 'AttributeSetDataService',
        function ($rootScope, $scope, dataService) {
            var ctrl = this;
            ctrl.selectedList = {
                action: 'Delete',
                data: []
            };
            ctrl.actions = ['Delete', 'Export'];

            ctrl.selectedProp = null;
            
            ctrl.settings = $rootScope.globalSettings;
            ctrl.select = function (id, isSelected) {
                if (isSelected) {
                    ctrl.selectedList.data.push(id);
                }
                else {
                    $rootScope.removeObject( ctrl.selectedList.data, id);
                }
            }
            ctrl.selectAll = function (isSelected) {
                ctrl.selectedList.data = [];
                angular.forEach(ctrl.data, function (e) {
                    e.isSelected = isSelected;
                    if (isSelected) {
                        ctrl.selectedList.data.push(e.id);
                    }
                });

            }
            ctrl.apply = async function () {
                ctrl.onApplyList()
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