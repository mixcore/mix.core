modules.component('setAttributeValue', {
    templateUrl: '/app/app-portal/components/set-attribute-value/view.html',
    bindings: {
        title: '=',
        columns: '=',
        properties: '='
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
                if (ctrl.trackedColumns != ctrl.columns) {
                    ctrl.trackedColumns = angular.copy(ctrl.columns);
                    ctrl.loadEditors();
                }
            }.bind(ctrl);
            
            ctrl.loadEditors = function(){
                angular.forEach(ctrl.columns, function(col){
                    var prop = $rootScope.findObjectByKey(ctrl.properties, 'name', col.name)
                    if(prop){
                        prop.dataType = col.dataType;
                        prop.options = col.options
                    }
                    else{
                        ctrl.properties.push({
                            title: col.title,
                            name: col.name,
                            dataType: col.dataType,
                            value: col.defaultValue,
                            options: col.options
                        });
                    }
                })
            };
            
            ctrl.addAttr = function () {
                if (ctrl.columns) {
                    var t = angular.copy(ctrl.defaultAttr);
                    ctrl.columns.push(t);
                }
            };
        }]
});