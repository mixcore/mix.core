
app.component('productMain', {
    templateUrl: '/app/app-portal/pages/product/components/main/productMain.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings',
        function ($rootScope, $scope, ngAppSettings) {
            var ctrl = this;
            ctrl.translate = $rootScope.translate;
            ctrl.privacies = ngAppSettings.privacies;
            ctrl.formatPrice = function (price) {
                return price.replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            };
            ctrl.generateSeo = function () {
                if (ctrl.product) {
                    if (ctrl.product.seoName === null || ctrl.product.seoName === '') {
                        ctrl.product.seoName = $rootScope.generateKeyword(ctrl.product.title, '-');
                    }
                    if (ctrl.product.seoTitle === null || ctrl.product.seoTitle === '') {
                        ctrl.product.seoTitle = $rootScope.generateKeyword(ctrl.product.title, '-');
                    }
                    if (ctrl.product.seoDescription === null || ctrl.product.seoDescription === '') {
                        ctrl.product.seoDescription = $rootScope.generateKeyword(ctrl.product.title, '-');
                    }
                    if (ctrl.product.seoKeywords === null || ctrl.product.seoKeywords === '') {
                        ctrl.product.seoKeywords = $rootScope.generateKeyword(ctrl.product.title, '-');
                    }
                }
            }
        }
    ],
    bindings: {
        product: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});