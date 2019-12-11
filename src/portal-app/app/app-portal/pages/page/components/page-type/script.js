
app.component('pageType', {
    templateUrl: '/app/app-portal/pages/page/components/page-type/view.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this;
        ctrl.settings = $rootScope.globalSettings;
    }],
    bindings: {
        model: '=',
    }
});