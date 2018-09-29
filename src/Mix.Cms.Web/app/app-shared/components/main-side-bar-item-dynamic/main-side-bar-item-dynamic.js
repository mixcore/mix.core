
modules.component('mainSideBarItemDynamic', {
    templateUrl: '/app-shared/components/main-side-bar-item-dynamic/main-side-bar-item-dynamic.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this;
        ctrl.translate = $rootScope.translate;
    }],
    bindings: {
        iconSize: '=',
        linkStyle: '=',
        itemStyle:'=',
        item: '='
    }
});