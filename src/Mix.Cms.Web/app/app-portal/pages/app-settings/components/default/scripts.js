
app.component('appSettingsDefault', {
    templateUrl: '/app/app-portal/pages/app-settings/components/default/view.html',
    controller: ['$rootScope', 'ngAppSettings', function ($rootScope, ngAppSettings) {
        var ctrl = this;
        ctrl.$onInit = function(){
            ctrl.properties = $.parseJSON(ctrl.appSettings.GlobalSettings.DefaultArticleAttr);
            ctrl.trackedProperties = $.parseJSON(ctrl.appSettings.GlobalSettings.DefaultArticleAttr);
            ctrl.dataTypes = $rootScope.globalSettings.dataTypes;
        }
        ctrl.addArticleProperty = function () {
            ctrl.properties.push({
                priority: 0,
                name: '',
                value: null,
                dataType: '7'
            });
            
        };
        ctrl.$doCheck = function () {
            if (ctrl.trackedProperties != ctrl.properties) {
                ctrl.trackedProperties = angular.copy(ctrl.properties);
                ctrl.appSettings.GlobalSettings.DefaultArticleAttr = JSON.stringify(ctrl.properties);
            }
        }.bind(ctrl);
    }],
    bindings: {
        appSettings: '=',
        cultures: '=',
        statuses: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});