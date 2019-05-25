
app.component('appSettingsGeneral', {
    templateUrl: '/app/app-portal/pages/app-settings/components/general/view.html',
    controller: ['ngAppSettings', function (ngAppSettings) {
        var ctrl = this;  
    }],
    bindings: {
        appSettings: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});