
modules.component('qrCode', {
    templateUrl: '/app/app-client/components/qr-code/view.html',
    controller: ['$location', function ($location) {
        var ctrl = this;
        ctrl.$onInit= function(){
            if(ctrl.model){
                ctrl.generate();
            }
        }
        ctrl.generate = function () {            
            $('#qr-output').empty();
            $('#qr-output').qrcode(ctrl.model);
        };
    }],
    bindings: {
        model: '='
    }
});