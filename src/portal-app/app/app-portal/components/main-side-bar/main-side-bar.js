modules.component('mainSideBar', {
    templateUrl: '/app/app-portal/components/main-side-bar/main-side-bar.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', 'TranslatorService', 'CommonService',
        function ($rootScope, $scope, ngAppSettings, translatorService, commonService) {
        var ctrl = this;
        ctrl.items = [];
        ctrl.init = async function () {
            commonService.loadJArrayData('portal-menus.json').then(resp=>{
                var root = resp.data[0];
                ctrl.items = root.data;
                $rootScope.isBusy = false;
                $scope.$apply();
            });
        };
    }],
    bindings: {
    }
});
