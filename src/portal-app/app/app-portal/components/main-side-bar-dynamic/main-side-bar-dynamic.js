modules.component('mainSideBarDynamic', {
    templateUrl: '/app/app-portal/components/main-side-bar-dynamic/main-side-bar-dynamic.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', 'RoleService', 'TranslatorService', function ($rootScope, $scope, ngAppSettings, roleServices, translatorService) {
        var ctrl = this;
        ctrl.init = function () {
            if (ctrl.roles) {
                ctrl.role = ctrl.roles[0];
            }    
        };
    }],
    bindings: {
        roles: '=',
        activedRole: '=',
        translate: '&'
    }
});