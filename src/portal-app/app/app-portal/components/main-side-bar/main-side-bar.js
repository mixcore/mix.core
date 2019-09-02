modules.component('mainSideBar', {
    templateUrl: '/app/app-portal/components/main-side-bar/main-side-bar.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', 'TranslatorService', 'CommonService',
        function ($rootScope, $scope, ngAppSettings, translatorService, commonService) {
        var ctrl = this;
        ctrl.items = [];
        ctrl.init = function () {
            var routes = $.parseJSON($('#portal-menus').val());
            ctrl.items = routes.data;
        };
    }],
    bindings: {
    }
});
