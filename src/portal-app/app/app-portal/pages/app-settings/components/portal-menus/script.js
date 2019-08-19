
modules.component('portalMenus', {
    templateUrl: '/app/app-portal/pages/app-settings/components/portal-menus/view.html',
    bindings: {
        'data': '=',
        'allowedTypes': '='
    },
    controller: ['$rootScope', '$scope', '$location', 'CommonService', 'ngAppSettings', 
    function ($rootScope, $scope, $location, commonService, ngAppSettings) {
        var ctrl = this;
        // ctrl.icons = [];
        ctrl.translate = $rootScope.translate;  
        ctrl.init = function(){
            ctrl.icons = ngAppSettings.icons;      
        };
       
    }]
});