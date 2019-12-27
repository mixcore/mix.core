
app.component('postMain', {
    templateUrl: '/app/app-portal/pages/post/components/main/view.html',
    controller: ['$rootScope', '$scope',
        function ($rootScope, $scope, attributeSetService) {
            var ctrl = this;
            ctrl.translate = $rootScope.translate;
            ctrl.generateSeo = function () {
                if (ctrl.post) {
                    if (ctrl.post.seoName === null || ctrl.post.seoName === '') {
                        ctrl.post.seoName = $rootScope.generateKeyword(ctrl.post.title, '-');
                    }
                    if (ctrl.post.seoTitle === null || ctrl.post.seoTitle === '') {
                        ctrl.post.seoTitle = ctrl.post.title
                    }
                    if (ctrl.post.seoDescription === null || ctrl.post.seoDescription === '') {
                        ctrl.post.seoDescription = ctrl.post.excerpt
                    }
                    if (ctrl.post.seoKeywords === null || ctrl.post.seoKeywords === '') {
                        ctrl.post.seoKeywords = ctrl.post.title
                    }
                }
            };
        }
    ],
    bindings: {
        post: '=',
        generateSeo: '&',
        onUpdate: '&'
    }
});