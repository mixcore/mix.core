
app.component('initSiteConfigurations', {
    templateUrl: '/app/app-init/pages/step3/components/site-configurations/view.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this; 
        ctrl.data = [];
        ctrl.$onInit = function(){
            ctrl.data = $rootScope.filterArray(ctrl.configurations, ['category'], ['Site_Common']);
        } 
    }],
    bindings: {
        configurations: '=',
    }
});