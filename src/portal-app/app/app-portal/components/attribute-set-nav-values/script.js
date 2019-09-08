modules.component('attributeSetNavValues', {
    templateUrl: '/app/app-portal/components/attribute-set-nav-values/view.html',
    bindings: {
        header: '=',
        data: '=',
        columns: '=?',
        onUpdate:'&?',
        onDelete:'&?',
    },
    controller: ['$rootScope', '$scope', 'RelatedAttributeSetDataService',
        function ($rootScope, $scope, navService) {
            var ctrl = this;
            ctrl.selectedProp = null;
            
            ctrl.settings = $rootScope.globalSettings;
            ctrl.$onInit = function(){
                
            };

            ctrl.update = function(data){
                ctrl.onUpdate({data: data});
                $("html, body").animate({ "scrollTop": "0px" }, 500);
            };
            
            ctrl.delete = function(data){
                ctrl.onDelete({data: data});
            };

            ctrl.filterData = function(item, attributeName){
                return $rootScope.findObjectByKey(item.data, 'attributeName', attributeName);
            };

            ctrl.dragStart = function(index){
                ctrl.dragStartIndex = index;
                ctrl.minPriority = ctrl.data[0].priority;
            };
            ctrl.updateOrders = function(index){
                if(index> ctrl.dragStartIndex){
                    ctrl.data.splice(ctrl.dragStartIndex, 1);
                }
                else{
                    ctrl.data.splice(ctrl.dragStartIndex+1, 1);
                }
                var arrNavs = [];
                angular.forEach(ctrl.data, function(e,i){
                    e.priority = ctrl.minPriority + i;
                    var keys = {
                        parentId: e.parentId,
                        parentType: e.parentType,
                        id: e.id
                    };
                    var properties = {
                        priority: e.priority
                    }
                    arrNavs.push({
                        keys: keys,
                        properties: properties
                    });
                });
                navService.saveProperties('portal', arrNavs).then(resp=>{
                    console.log(resp);
                    $rootScope.isBusy = false;
                    $scope.$apply();
                })
            };
        }]
});