
app.component('postAttributeSet', {
    templateUrl: '/app/app-portal/pages/post/components/attribute-set/view.html',
    bindings: {
        set: '=',
        parentType: '=?',
        parentId: '=?',
    },
    controller: ['$rootScope', '$scope', 'RelatedAttributeSetDataService', 'AttributeSetDataService',
        function ($rootScope, $scope, navService, dataService) {
            var ctrl = this;
            ctrl.dataTypes = $rootScope.globalSettings.dataTypes;            
            ctrl.activedData = null;
            ctrl.defaultData = null;
            ctrl.$onInit = async function () {
                navService.getSingle('portal', [ctrl.parentId, ctrl.parentType, 'default']).then(resp=>{
                    ctrl.defaultData = resp;
                    ctrl.activedData = angular.copy(ctrl.defaultData);
                });
            };
            ctrl.update = function (nav) {
                ctrl.activedData = nav;
                var e = $(".pane-form-" + ctrl.set.attributeSet.id)[0];
                angular.element(e).triggerHandler('click');
            };

            ctrl.removeValue = function (nav) {
                $rootScope.showConfirm(ctrl, 'removeValueConfirmed', [nav], null, 'Remove', 'Are you sure');
            };

            ctrl.removeValueConfirmed = async function (nav) {
                $rootScope.isBusy = true;
                var result = await navService.delete([nav.parentId, nav.parentType, nav.id]);
                if (result.isSucceed) {
                    $rootScope.removeObjectByKey(ctrl.set.attributeSet.postData.items, 'id', nav.id);
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    $rootScope.showMessage('failed');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }

                // if (data.id) {
                //     $rootScope.isBusy = true;
                //     var result = await dataService.delete(data.id);
                //     if (result.isSucceed) {
                //         $rootScope.removeObjectByKey(ctrl.set.attributeSet.postData.items, 'id', data.id);
                //         $rootScope.isBusy = false;
                //         $scope.$apply();
                //     } else {
                //         $rootScope.showMessage('failed');
                //         $rootScope.isBusy = false;
                //         $scope.$apply();
                //     }
                // }
                // else {
                //     var i = ctrl.set.attributeSet.postData.items.indexOf(data);                    
                //     if(i >=0){
                //         ctrl.set.attributeSet.postData.items.splice(i,1);
                //     }
                // }
            };

            ctrl.saveData = async function (data) {                
                $rootScope.isBusy = true;
                ctrl.activedData.data = data;
                dataService.save('portal', data).then(resp=>{
                    if(resp.isSucceed){
                        ctrl.activedData.id = resp.data.id;
                        ctrl.activedData.data = resp.data;
                        navService.save('portal', ctrl.activedData).then(resp=>{
                            if(resp.isSucceed){
                                var tmp = $rootScope.findObjectByKey(ctrl.set.attributeSet.postData.items, ['parentId', 'parentType', 'id'], 
                                    [resp.data.parentId, resp.data.parentType, resp.data.id]);
                                if(!tmp){
                                    ctrl.set.attributeSet.postData.items.push(resp.data);
                                    var e = $(".pane-data-" + ctrl.set.attributeSet.id)[0];
                                    angular.element(e).triggerHandler('click');
                                }
                                ctrl.activedData = angular.copy(ctrl.defaultData);
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
                });
            };
        }]
});