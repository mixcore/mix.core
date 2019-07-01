
modules.component('fbCustomerChat', {
    templateUrl: '/app/app-client/components/fb-customer-chat/view.html',
    controller: ['$location', function ($location) {
        var ctrl = this;
        this.$onInit = function(){
            setTimeout(() => {
                FB.XFBML.parse();
            }, 200);
        }    
    }],
    bindings: {
        fbPageId:'=',
        themeColor:'=',
        inGreeting:'=',
        outGreeting:'='
    }
});