
app.component('pageAdvanced', {
    templateUrl: '/app/app-portal/pages/page/components/page-advanced/view.html',
    bindings: {
        model: '='
    },
    controller: ['$rootScope', '$scope',
        function ($rootScope, $scope, attributeSetService) {
            var ctrl = this;
            ctrl.translate = $rootScope.translate;    
                  
        }
    ]    
});