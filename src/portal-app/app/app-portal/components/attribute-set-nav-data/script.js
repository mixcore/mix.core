modules.component('attributeSetNavData', {
    templateUrl: '/app/app-portal/components/attribute-set-nav-data/view.html',
    bindings: {
        nav:'=',
        parentId:'=',
        parentType:'=',
        onUpdate:'&?',
        onDelete:'&?',
    },
    controller: ['$rootScope', '$scope', 'ngAppSettings', 'RelatedAttributeSetDataService', 'AttributeSetDataService',
        function ($rootScope, $scope, ngAppSettings, navService, dataService) {
            var ctrl = this;
            ctrl.data = [];
            ctrl.selected = null;
            ctrl.navRequest = angular.copy(ngAppSettings.request);            
            ctrl.setRequest = angular.copy(ngAppSettings.request);            
            ctrl.settings = $rootScope.globalSettings;
            ctrl.$onInit = function(){
                navService.getSingle('portal', [ctrl.parentId, ctrl.parentType, 'default']).then(resp=>{
                    ctrl.defaultData = resp;
                    ctrl.selected = angular.copy(ctrl.defaultData);
                    ctrl.loadData();
                });
                
            };
            ctrl.selectPane = function(pane){
            };
            ctrl.loadData = function(){
                navService.getList('portal', ctrl.navRequest, 
                    ctrl.nav.data.id, ctrl.parentType, ctrl.parentId)
                    .then(resp=>{
                    if (resp) 
                    {
                        ctrl.data = resp;
                        $scope.$apply();
                    } else {
                        if (resp) {
                            $rootScope.showErrors('Failed');
                        }
                        $scope.$apply();
                    }
                });
            };
            ctrl.updateData = function(nav){
                ctrl.selected = nav;
                var e = $(".pane-form-" + ctrl.nav.data.id)[0];
                angular.element(e).triggerHandler('click');
                // $location.url('/portal/attribute-set-data/details?dataId='+ item.id +'&attributeSetId=' + item.attributeSetId+'&parentType=' + item.parentType+'&parentId=' + item.parentId);
            };
            ctrl.saveData = function(data){            
                $rootScope.isBusy = true;
                ctrl.selected.data = data;
                dataService.save('portal', data).then(resp=>{
                    if(resp.isSucceed){
                        ctrl.selected.id = resp.data.id;
                        ctrl.selected.attributeSetId = resp.data.attributeSetId;
                        ctrl.selected.data = resp.data;
                        navService.save('portal', ctrl.selected).then(resp=>{
                            if(resp.isSucceed){
                                var tmp = $rootScope.findObjectByKey(ctrl.data, ['parentId', 'parentType', 'id'], 
                                    [resp.data.parentId, resp.data.parentType, resp.data.id]);
                                if(!tmp){
                                    ctrl.data.push(resp.data);
                                    var e = $(".pane-data-" + ctrl.nav.data.id)[0];
                                    angular.element(e).triggerHandler('click');
                                }
                                ctrl.selected = angular.copy(ctrl.defaultData);
                                $rootScope.isBusy = false;
                                $scope.$apply();
                            }else{
                                $rootScope.showMessage('failed');    
                                $rootScope.isBusy = false;
                                $scope.$apply();
                            }
                        })
                        
                    }
                    else{
                        $rootScope.showMessage('failed');    
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                })
            }
            ctrl.removeData = async function(nav){
                $rootScope.showConfirm(ctrl, 'removeDataConfirmed', [nav], null, 'Remove', 'Deleted data will not able to recover, are you sure you want to delete this item?');
            };
            ctrl.removeDataConfirmed = async function(nav){
                $rootScope.isBusy = true;
                var result = await navService.delete([nav.parentId, nav.parentType, nav.id]);
                if (result.isSucceed) {
                    $rootScope.removeObjectByKey(ctrl.data, 'id', nav.id);
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    $rootScope.showMessage('failed');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
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
                    $rootScope.isBusy = false;
                    $scope.$apply();
                });
            };
        }]
});