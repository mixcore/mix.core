cart.component('shoppingCart', {
    templateUrl: '/app/app-client/components/shopping-cart/view.html',
    controller: [
        '$rootScope','localStorageService', 'CommonService', 
        function ($rootScope, localStorageService, commonService) {
            var ctrl = this;
            ctrl.submitted = false;
            ctrl.isShow = false;
            
            ctrl.edm = 'Url: <a href="[url]">View Tour</a> <br/>Name: [name] <br/>'
                        + 'Phone: [phone]<br/>'
                        + 'Email: [email]<br/>'
                        + 'Quantity: [quantity]<br/>'
                        + 'Message: [message] <br/>'
                        + 'property: [property] <br/>Price: [price] <br/>';
            ctrl.init = function () {
                
            };
            ctrl.showShoppingCart = function(){
                $('#modal-shopping-cart').modal('show');
            }
            ctrl.calculate = function(){
                ctrl.cartData.total = 0;
                ctrl.cartData.totalItems = ctrl.cartData.items.length;
                angular.forEach(ctrl.cartData.items, function(e){
                    ctrl.cartData.total+= (parseInt(e.price) * e.quantity);
                });
                localStorageService.set('shoppingCart', ctrl.cartData);
            };
            ctrl.removeItem = function(index){
                ctrl.cartData.items.splice(index,1);
                ctrl.calculate();
            }
            ctrl.book = function(){
                ctrl.edm = ctrl.edm.replace(/\[url\]/g,window.top.location.href);
                ctrl.edm = ctrl.edm.replace(/\[name\]/g,ctrl.order.name);
                ctrl.edm = ctrl.edm.replace(/\[phone\]/g,ctrl.order.phone);
                ctrl.edm = ctrl.edm.replace(/\[email\]/g,ctrl.order.email);
                ctrl.edm = ctrl.edm.replace(/\[message\]/g,ctrl.order.message);
                ctrl.edm = ctrl.edm.replace(/\[property\]/g,ctrl.order.propertyId);
                ctrl.edm = ctrl.edm.replace(/\[price\]/g,ctrl.order.price);
                ctrl.edm = ctrl.edm.replace(/\[quantity\]/g,ctrl.order.quantity);
                
                commonService.sendMail('Booking - ' + ctrl.propertyName, ctrl.edm);
                ctrl.submitted = true;
                setTimeout(() => {
                    ctrl.submitted = false;
                }, 1000);
                ctrl.cartData = {
                    items: [],
                    totalItems:0,
                    total:0,
                };
                localStorageService.set('shoppingCart', ctrl.cartData);
            }
        }
    ],
    bindings: {
        cartData: '='
    }
});