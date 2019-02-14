
app.component('articleMain', {
    templateUrl: '/app/app-portal/pages/article/components/main/articleMain.html',
    controller: ['$rootScope', '$scope',
        function ($rootScope, $scope) {
            var ctrl = this;
            ctrl.translate = $rootScope.translate;            
        }
    ],
    bindings: {
        article: '=',
        generateSeo: '&',
        onUpdate: '&'
    }
});