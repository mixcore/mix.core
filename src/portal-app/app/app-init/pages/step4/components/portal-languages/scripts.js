
app.component('initPortalLanguages', {
    templateUrl: '/app/app-init/pages/step4/components/portal-languages/view.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this; 
        ctrl.data = [];
        ctrl.$onInit = function(){
            ctrl.data = $rootScope.filterArray(ctrl.languages, 'category', 'Portal');
        } 
    }],
    bindings: {
        languages: '=',
    }
});