
app.component('appSettingsDefault', {
    templateUrl: '/app/app-portal/pages/app-settings/components/default/view.html',
    controller: ['ngAppSettings', function (ngAppSettings) {
        var ctrl = this;  
    }],
    bindings: {
        appSettings: '=',
        cultures: '=',
        statuses: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});