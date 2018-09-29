
modules.component('swPaging', {
    templateUrl: '/app-shared/components/paging/paging.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this;
        ctrl.loadData = function (index) {
            ctrl.callback({ pageIndex: index });
        };        
        ctrl.range = $rootScope.range;
    }],
    bindings: {
        data: '=',
        activeClass: '=',
        page: '=',
        totalPage: '=',
        callback: '&'
    }
});