
modules.component('attributeValueEditor', {
    templateUrl: '/app/app-portal/components/attribute-value-editor/view.html',
    bindings: {
        attributeValue: '=',
        attribute: '=',
        inputClass: '=',
        isShowTitle: '=',
        title: '='
    },
    controller: ['$rootScope', '$scope', 'ngAppSettings', function ($rootScope, $scope, ngAppSettings) {
        var ctrl = this;
        ctrl.icons = ngAppSettings.icons;
        ctrl.dataTypes = $rootScope.globalSettings.dataTypes;
        ctrl.$onInit = function(){
            setTimeout(() => {
                ctrl.attributeValue.priority = ctrl.attribute.priority;                
                console.log(ctrl.encryptedData, ctrl.decryptedData);
                switch(ctrl.attribute.dataType){
                    case 1:
                    case 2:
                    case 3:
                        if(ctrl.attributeValue.datetimeValue){
                            ctrl.attributeValue.dateObj = new Date(ctrl.attributeValue.datetimeValue);
                            $scope.$apply();
                        }
                        break;
                    default:
                        if(ctrl.attribute.isEncrypt){
                            var encryptedData = {
                                key: ctrl.attributeValue.encryptKey,
                                data: ctrl.attributeValue.encryptValue
                            };
                            ctrl.attributeValue.stringValue = $rootScope.decrypt(encryptedData);
                        }
                        if(!ctrl.attributeValue.stringValue){
                            ctrl.attributeValue.stringValue = ctrl.attribute.defaultValue;
                            $scope.$apply();
                        }
                        break;
                }
            }, 200);
        };
        ctrl.updateStringValue = function(dataType){
            switch (dataType) {
                case 1:
                case 2:
                case 3:
                    if(ctrl.attributeValue.dateObj){
                        ctrl.attributeValue.datetimeValue = ctrl.attributeValue.dateObj.toISOString();
                        ctrl.attributeValue.stringValue = ctrl.attributeValue.datetimeValue;
                    }
                    break;
                case 6:
                    if(ctrl.attributeValue.doubleValue){
                        ctrl.attributeValue.stringValue = ctrl.attributeValue.doubleValue.toString();
                    }
                    break;
                case 18:
                    if(ctrl.attributeValue.booleanValue){
                        ctrl.attributeValue.stringValue = ctrl.attributeValue.booleanValue.toString();
                    }
                    break;
            
                default:
                    break;
            }
            console.log(ctrl.attributeValue);
        }
    }]
});