
app.component('moduleAdvanced', {
    templateUrl: '/app/app-portal/pages/module/components/module-advanced/view.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this;
        ctrl.settings = $rootScope.globalSettings;
        ctrl.$onInit = function(){
            ctrl.isAdmin = $rootScope.isAdmin;
        };
    }],
    bindings: {
        model: '=',
    }
});