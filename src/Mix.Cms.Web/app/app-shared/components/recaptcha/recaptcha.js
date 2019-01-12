
modules.component('recaptcha', {
    templateUrl: '/app/app-shared/components/recaptcha/recaptcha.html',
    controller: ['$rootScope', 'CommonService',
        function ($rootScope, commonService) {
            var ctrl = this;
            ctrl.shortenString = '';
            ctrl.previousContentId = undefined;
            ctrl.recaptcha_key = null;
            ctrl.token= null;
            this.$onInit = () => { 
                ctrl.recaptcha_key = $rootScope.globalSettings.data.Recaptcha_Key;
                ctrl.recaptcha_secret = $rootScope.globalSettings.data.Recaptcha_Secret;
                grecaptcha.ready(function() {
                    grecaptcha.execute(ctrl.recaptcha_key, {action: ctrl.action})
                    .then(function(token) {
                       ctrl.token = token;
                    });
                });
            };
            ctrl.verify = function(){
                var  url = 'https://www.google.com/recaptcha/api/siteverify';
                var data = {
                    secret: ctrl.recaptcha_secret,
                    response: ctrl.token                    
                };
                var req = {
                    method: 'POST',
                    url: url,
                    data: data
                };
                return commonService.getApiResult(req).then(function (response) {
                    if(response.success){
                        ctrl.callback();
                    }
                    else{
                        angular.forEach(response.error-codes, function(err){
                            commonService.showAlertMsg('Recaptcha Error', err);
                        });
                        
                    }
                });                       
            }
        }],
    bindings: {
        action: '=',
        callback: '&',
    }
});