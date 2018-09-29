
app.component('productMain', {
    templateUrl: '/app-portal/pages/product/components/main/productMain.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings',
        function ($rootScope, $scope, ngAppSettings) {
            var ctrl = this;
            ctrl.translate = $rootScope.translate;
            ctrl.privacies = ngAppSettings.privacies;
            ctrl.formatPrice = function(price){
                return  price.replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            };
            ctrl.generateSEO = function () {
                if (!ctrl.product.id) {
                    ctrl.product.seoName = $rootScope.generateKeyword(ctrl.product.title, '-');
                }
            };
        }
    ],
    bindings: {
        product: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});