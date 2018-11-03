
app.component('permissionMain', {
    templateUrl: '/app/app-portal/pages/permission/components/main/main.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', '$routeParams',function ($rootScope, $scope, ngAppSettings, $routeParams) {
        var ctrl = this;
        ctrl.settings = $rootScope.globalSettings;
        ctrl.setPageType = function (type) {
            ctrl.page.type = $index;
        };
        ctrl.generateKeyword = function (text) {
            if (!$routeParams.id) {
                ctrl.page.textKeyword = 'portal_' + text.replace(/[^a-zA-Z0-9]+/g, '_')
                    .replace(/([A-Z]+)([A-Z][a-z])/g, '$1-$2')
                    .replace(/([a-z])([A-Z])/g, '$1-$2')
                    .replace(/([0-9])([^0-9])/g, '$1-$2')
                    .replace(/([^0-9])([0-9])/g, '$1-$2')
                    .replace(/-+/g, '_')
                    .toLowerCase();
            }
        };
    }],
    bindings: {
        page: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});