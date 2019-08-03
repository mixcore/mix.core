
app.component('postAttributeSet', {
    templateUrl: '/app/app-portal/pages/post/components/attribute-set/view.html',
    bindings: {
        set: '=',
    },
    controller: ['$rootScope', '$scope', 'AttributeDataService', 'PostAttributeDataService',
        function ($rootScope, $scope, service, dataService) {
            var ctrl = this;
            ctrl.dataTypes = $rootScope.globalSettings.dataTypes;
            ctrl.activedData = null;
            ctrl.defaultData = null;
            ctrl.$onInit = async function () {
                
            };
            ctrl.update = function (data) {
                ctrl.activedData = data;
            };
            ctrl.removeValue = function (data) {
                $rootScope.showConfirm(ctrl, 'removeValueConfirmed', [data], null, 'Remove', 'Are you sure');
            };

            ctrl.removeValueConfirmed = async function (data) {
                if (data.id) {
                    $rootScope.isBusy = true;
                    var result = await dataService.delete(data.id);
                    if (result.isSucceed) {
                        $rootScope.removeObjectByKey(ctrl.set.attributeSet.postData.items, 'id', data.id);
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    } else {
                        $rootScope.showMessage('failed');
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                }
                else {
                    var i = ctrl.set.attributeSet.postData.items.indexOf(data);                    
                    if(i >=0){
                        ctrl.set.attributeSet.postData.items.splice(i,1);
                    }
                }
            };

            ctrl.saveData = async function (data) {                
                if (!data.id && !data.isAdded) {
                    data.isAdded = true;
                    ctrl.set.attributeSet.postData.items.push(data);
                }
                return { isSucceed: true }; 
            };
        }]
});