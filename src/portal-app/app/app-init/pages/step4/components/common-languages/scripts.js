
app.component('initCommonLanguages', {
    templateUrl: '/app/app-init/pages/step3/components/common-languages/view.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this; 
        ctrl.data = [];
        ctrl.$onInit = function(){
            ctrl.data = $rootScope.filterArray(ctrl.languages, 'category', 'Common');
        } 
    }],
    bindings: {
        languages: '=',
    }
});