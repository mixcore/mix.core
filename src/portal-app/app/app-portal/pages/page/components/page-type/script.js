
app.component('pageType', {
    templateUrl: '/app/app-portal/pages/page/components/page-type/view.html',
    bindings: {
        model: '=',
    },
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this;
        ctrl.settings = $rootScope.globalSettings;
        ctrl.$onInit = function(){
            ctrl.model = parseInt(ctrl.model);
        }
    }],    
});