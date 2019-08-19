
modules.component('portalMenus', {
    templateUrl: '/app/app-portal/pages/app-settings/components/portal-menus/view.html',
    controller: ['$rootScope', '$scope', '$location', function ($rootScope, $scope, $location) {
        var ctrl = this;
        ctrl.data = [];
        ctrl.translate = $rootScope.translate;
        ctrl.$onInit = function(){

        };
    }],
    bindings: {
    }
});