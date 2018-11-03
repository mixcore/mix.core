
app.component('moduleMain', {
    templateUrl: '/app/app-portal/pages/module/components/main/main.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this;
        ctrl.gennerateName = function () {
            if (!ctrl.module.id || ctrl.module.name === null || ctrl.module.name === '') {
                ctrl.module.name = $rootScope.generateKeyword(ctrl.module.title, '_');
            }
        };
    }],
    bindings: {
        module: '=',
    }
});