
app.component('postAdvanced', {
    templateUrl: '/app/app-portal/pages/post/components/post-advanced/view.html',
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