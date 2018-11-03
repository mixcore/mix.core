
app.component('articleMain', {
    templateUrl: '/app/app-portal/pages/article/components/main/articleMain.html',
    controller: ['$rootScope', '$scope',
        function ($rootScope, $scope) {
            var ctrl = this;
            ctrl.generateSeo = function () {
                if (ctrl.article) {
                    if (ctrl.article.seoName === null || ctrl.article.seoName === '') {
                        ctrl.article.seoName = $rootScope.generateKeyword(ctrl.article.title, '-');
                    }
                    if (ctrl.article.seoTitle === null || ctrl.article.seoTitle === '') {
                        ctrl.article.seoTitle = $rootScope.generateKeyword(ctrl.article.title, '-');
                    }
                    if (ctrl.article.seoDescription === null || ctrl.article.seoDescription === '') {
                        ctrl.article.seoDescription = $rootScope.generateKeyword(ctrl.article.title, '-');
                    }
                    if (ctrl.article.seoKeywords === null || ctrl.article.seoKeywords === '') {
                        ctrl.article.seoKeywords = $rootScope.generateKeyword(ctrl.article.title, '-');
                    }
                }
            }
        }
    ],
    bindings: {
        article: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});