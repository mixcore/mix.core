modules.component('attributeSetNavs', {
    templateUrl: '/app/app-portal/components/attribute-set-navs/view.html',
    bindings: {
        parentId:'=',
        parentType:'=',
        onUpdate:'&?',
        onDelete:'&?',
    },
    controller: ['$rootScope', '$scope', 'ngAppSettings', 'RelatedAttributeSetService', 'AttributeSetService',
        function ($rootScope, $scope, ngAppSettings, navService, setService) {
            var ctrl = this;
            ctrl.data = [];
            ctrl.selected = {};
            ctrl.defaultData = null;
            ctrl.navRequest = angular.copy(ngAppSettings.request);            
            ctrl.setRequest = angular.copy(ngAppSettings.request);            
            ctrl.settings = $rootScope.globalSettings;
            ctrl.$onInit = function(){
                navService.getSingle('portal', [ctrl.parentId, ctrl.parentType, 0]).then(resp=>{
                    ctrl.defaultData = resp;
                    ctrl.loadData();
                });
            };
            ctrl.goToPath = $rootScope.goToPath;
            ctrl.selectPane = function(pane){
                console.log(pane);
            };
            ctrl.loadData = async function(){
                var resp = await navService.getList('portal', ctrl.navRequest, ctrl.parentType, ctrl.parentId);
                if (resp) 
                {
                    angular.forEach(resp, e => {
                        e.isActived = true;
                        ctrl.data.push(e);
                    });
                } else {
                    if (resp) {
                        $rootScope.showErrors('Failed');
                    }
                }
                ctrl.setRequest.filter = 'type eq ' + ctrl.parentType;
                var setResult = await setService.getList('portal', ctrl.setRequest);
                if (setResult) 
                {
                    angular.forEach(setResult, element => {
                        
                        var e = $rootScope.findObjectByKey(ctrl.data, 'id', element.id);
                        if(!e){
                            e = angular.copy(ctrl.defaultData);
                            e.id = element.id;
                            e.data = element;
                            e.isActived = false;
                            ctrl.data.push(e);
                        }                        
                    });
                } else {
                    if (setResult) {
                        $rootScope.showErrors('Others Failed');
                    }
                }
                $scope.$apply();
            };  
            ctrl.save = async function(nav){
                $rootScope.isBusy = true;
                var result;
                if(nav.isActived){
                    result = await navService.save('portal', nav);
                }
                else{
                    result = await navService.delete([nav.parentId, nav.parentType, nav.id]);
                    $('.pane-container-'+ nav.data.id).parent().remove();
                }
                if (result.isSucceed) {
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    $rootScope.showMessage('failed');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            }
            
            ctrl.update = function(data){
                ctrl.onUpdate({data: data});                
            };
            
            ctrl.delete = function(data){
                ctrl.onDelete({data: data});
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