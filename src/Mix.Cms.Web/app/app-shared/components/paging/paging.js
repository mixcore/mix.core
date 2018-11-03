
modules.component('mixPaging', {
    templateUrl: '/app/app-shared/components/paging/paging.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this;
        ctrl.loadData = function (index) {
            ctrl.pagingAction({ pageIndex: index });
        };        
        ctrl.range = $rootScope.range;
    }],
    bindings: {
        data: '=',
        activeClass: '=',
        ulClass: '=',
        page: '=',
        pageSize: '=',
        total: '=',
        totalPage: '=',
        pagingAction: '&'
    }
});