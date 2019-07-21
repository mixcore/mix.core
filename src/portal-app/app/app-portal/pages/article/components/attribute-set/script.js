
app.component('articleAttributeSet', {
    templateUrl: '/app/app-portal/pages/article/components/attribute-set/view.html',
    controller: ['$rootScope', '$scope', 'AttributeDataService', function ($rootScope, $scope, service) {
        var ctrl = this;
        ctrl.dataTypes = $rootScope.globalSettings.dataTypes;
        ctrl.activedData = null;
        ctrl.defaultData = null;
        ctrl.$onInit = async function () {
            var getData = await service.getSingle(['post', ctrl.set.attributeSetId, 'portal']);
            if (getData.isSucceed) {
                ctrl.defaultData = getData.data;
                if (!ctrl.activedData) {
                    ctrl.activedData = angular.copy(ctrl.defaultData);
                }
                $rootScope.isBusy = false;
                $scope.$apply();
            } else {
                if (getData) {
                    $rootScope.showErrors(getData.errors);
                }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
        ctrl.update = function (data) {
            ctrl.activedData = data;
        };
        ctrl.saveData = function (data) {
            if(!data.id && !data.isAdded){
                data.isAdded = true;                
                ctrl.set.attributeSet.articleData.items.push(data);
            }
            ctrl.activedData = angular.copy(ctrl.defaultData);
        };
    }],
    bindings: {
        set: '=',
    }
});