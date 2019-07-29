
modules.component('attributeValuePreview', {
    templateUrl: '/app/app-shared/components/attribute-value-preview/view.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this;
        ctrl.$onInit = function(){
            if(ctrl.data.field.isEncrypt){
                var encryptedData = {
                    key: ctrl.data.encryptKey,
                    data: ctrl.data.encryptValue
                };
                ctrl.data.stringValue = $rootScope.decrypt(encryptedData);
            }
        };
    }
    ],
    bindings: {
        data: '=',
        width: '=',
    }
});