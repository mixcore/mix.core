modules.component('addToCartButton', {
    templateUrl: '/app/app-client/components/add-to-cart-button/view.html',
    controller: ['$rootScope', 'localStorageService',
        function ($rootScope, localStorageService) {
            var ctrl = this;
            ctrl.addToCart = function () {
                var current = $rootScope.findObjectByKey(ctrl.cartData.items, 'propertyId', ctrl.propertyId);
                if (current) {
                    current.quantity += parseInt(ctrl.quantity);
                }
                else {
                    var item = {
                        propertyId: ctrl.propertyId,
                        title: ctrl.title,
                        imageUrl: ctrl.imageUrl,
                        price: ctrl.price,
                        quantity: parseInt(ctrl.quantity) || 1
                    };
                    ctrl.cartData.items.push(item);
                    ctrl.cartData.totalItems += 1;
                }
                ctrl.cartData.total+= parseInt(ctrl.price);
                localStorageService.set('shoppingCart', ctrl.cartData);
            }
        }
    ],
    bindings: {
        cartData: '=',
        propertyId: '=',
        title: '=',
        imageUrl: '=',
        price: '=',
        quantity: '=',
    }
});