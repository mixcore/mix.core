
app.component('postMain', {
    templateUrl: '/app/app-portal/pages/post/components/main/view.html',
    controller: ['$rootScope', '$scope',
        function ($rootScope, $scope) {
            var ctrl = this;
            ctrl.translate = $rootScope.translate;            
        }
    ],
    bindings: {
        post: '=',
        generateSeo: '&',
        onUpdate: '&'
    }
});