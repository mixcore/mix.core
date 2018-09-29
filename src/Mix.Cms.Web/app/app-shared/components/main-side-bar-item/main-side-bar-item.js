
modules.component('mainSideBarItem', {
    templateUrl: '/app-shared/components/main-side-bar-item/main-side-bar-item.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this;
        ctrl.translate = $rootScope.translate;
    }],
    bindings: {
        item: '=',
    }
});