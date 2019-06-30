
app.component('initSysConfigurations', {
    templateUrl: '/app/app-init/pages/step2/components/site-configurations/view.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this; 
        ctrl.data = [];
        ctrl.$onInit = function(){
            ctrl.data = $rootScope.filterArray(ctrl.configurations, 'category', 'System');
        } 
    }],
    bindings: {
        configurations: '=',
    }
});