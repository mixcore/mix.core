
app.component('moduleType', {
    templateUrl: '/app/app-portal/pages/module/components/module-type/view.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this;
        ctrl.settings = $rootScope.globalSettings;
    }],
    bindings: {
        model: '=',
    }
});