modules.component('attributeSetValues', {
    templateUrl: '/app/app-portal/components/attribute-set-values/view.html',
    bindings: {
        title: '=',
        attributes: '='
    },
    controller: ['$rootScope', '$scope',
        function ($rootScope, $scope) {
            var ctrl = this;
            ctrl.defaultAttr = {
                title: '',
                name: '',
                default: null,
                options: [],
                priority: 0,
                dataType: 7,
                isGroupBy: false,
                isSelect: false,
                isDisplay: true,
                width: 3
            };
            ctrl.selectedProp = null;
            ctrl.settings = $rootScope.globalSettings;

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
        }]
});