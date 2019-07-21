modules.component('attributeSetForm', {
    templateUrl: '/app/app-portal/components/attribute-set-form/view.html',
    bindings: {
        setId: '=',
        data: '=',
        attributes: '=',
        defaultData: '=',
        saveData: '&?'
    },
    controller: ['$rootScope', '$scope', 
        function ($rootScope, $scope) {
            var ctrl = this;
            ctrl.defaultData = {};
            ctrl.attributes = [];
            ctrl.selectedProp = null;
            ctrl.settings = $rootScope.globalSettings;
            
            ctrl.submit = async function () {
                if (ctrl.saveData) {
                    ctrl.saveData({ data: ctrl.data });
                    ctrl.data = ctrl.defaultData;
                }
                else {
                    var saveResult = await service.save(ctrl.data);
                    if (saveResult.isSucceed) {

                    } else {
                        if (saveResult) {
                            $rootScope.showErrors(saveResult.errors);
                        }
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }

                }
            }
            ctrl.$doCheck = function () {
                if (angular.toJson(ctrl.columns) != angular.toJson(ctrl.trackedColumns)) {
                    ctrl.trackedColumns = angular.copy(ctrl.columns);
                    ctrl.trackedProperties = angular.copy(ctrl.properties);
                    ctrl.loadEditors();
                }
            }.bind(ctrl);

            ctrl.loadEditors = function () {
                ctrl.properties = [];
                for (let i = 0; i < ctrl.columns.length; i++) {
                    var col = ctrl.columns[i];
                    var oldObj = $rootScope.findObjectByKey(ctrl.trackedProperties, 'name', col.name) || {};
                    ctrl.properties.push({
                        title: col.title,
                        name: col.name,
                        dataType: col.dataType,
                        value: oldObj.value || col.defaultValue,
                        options: col.options
                    });
                }
            };

            ctrl.addAttr = function () {
                if (ctrl.columns) {
                    var t = angular.copy(ctrl.defaultAttr);
                    ctrl.columns.push(t);
                }
            };

            ctrl.filterData = function (attributeName) {
                var attr =  $rootScope.findObjectByKey(ctrl.data.data, 'attributeName', attributeName);
                if (!attr){
                    attr = angular.copy($rootScope.findObjectByKey(ctrl.defaultData.data, 'attributeName', attributeName));
                    ctrl.data.data.push(attr);
                }
                return attr;
            }
            ctrl.dragStart = function (index) {
                ctrl.dragStartIndex = index;
            };
            ctrl.updateOrders = function (index) {
                if (index > ctrl.dragStartIndex) {
                    ctrl.attributes.splice(ctrl.dragStartIndex, 1);
                }
                else {
                    ctrl.attributes.splice(ctrl.dragStartIndex + 1, 1);
                }
                angular.forEach(ctrl.attributes, function (e, i) {
                    e.priority = i;
                });
            };
        }]
});