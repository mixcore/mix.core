
app.component('articleMain', {
    templateUrl: '/app-portal/pages/article/components/main/articleMain.html',
    controller: ['$rootScope', '$scope',
        function ($rootScope, $scope) {
            var ctrl = this;
            ctrl.generateSEO = function () {
                if (!ctrl.article.id) {
                    ctrl.article.seoName = $rootScope.generateKeyword(ctrl.article.title, '-');
                }
                if (!ctrl.article.id) {
                    ctrl.article.urlAlias.alias = $rootScope.generateKeyword(ctrl.article.title, '-');
                }
                $scope.$apply();
            };
        }
    ],
    bindings: {
        article: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});