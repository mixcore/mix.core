modules.component('booking', {
    templateUrl: '/app/app-client/components/booking/index.html',
    controller: [
        '$rootScope', 'CommonService', 
        function ($rootScope, commonService) {
            var ctrl = this;
            ctrl.submitted = false;
            ctrl.isShow = false;
            ctrl.order = {
                name:'',
                propertyId:'',
                price:'',
                quantity: 1
            };
            ctrl.edm = 'Url: <a href="[url]">View Tour</a> <br/>Name: [name] <br/>'
                        + 'Phone: [phone]<br/>'
                        + 'Email: [email]<br/>'
                        + 'Quantity: [quantity]<br/>'
                        + 'Message: [message] <br/>'
                        + 'property: [property] <br/>Price: [price] <br/>';
            ctrl.init = function () {
                if (!$rootScope.isInit) {
                    setTimeout(function () { ctrl.init(); }, 500);
                } else {
                    ctrl.order.propertyId = ctrl.propertyId;
                    ctrl.order.price = ctrl.price;
                    ctrl.order.quantity = ctrl.quantity;
                }
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
            }
        }
    ],
    bindings: {
        propertyId: '=',
        propertyName: '=',
        price: '=',
        quantity: '=',
    }
});