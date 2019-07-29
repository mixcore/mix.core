
app.component('articleAttributeSet', {
    templateUrl: '/app/app-portal/pages/article/components/attribute-set/view.html',
    bindings: {
        set: '=',
    },
    controller: ['$rootScope', '$scope', 'AttributeDataService', 'ArticleAttributeDataService',
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
                        $rootScope.removeObjectByKey(ctrl.set.attributeSet.articleData.items, 'id', data.id);
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    } else {
                        $rootScope.showMessage('failed');
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                }
                else {
                    var i = ctrl.set.attributeSet.articleData.items.indexOf(data);                    
                    if(i >=0){
                        ctrl.set.attributeSet.articleData.items.splice(i,1);
                    }
                }
            };

            ctrl.saveData = async function (data) {                
                if (!data.id && !data.isAdded) {
                    data.isAdded = true;
                    ctrl.set.attributeSet.articleData.items.push(data);
                }
                return { isSucceed: true }; 
            };
        }]
});