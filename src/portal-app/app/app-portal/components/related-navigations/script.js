
modules.component('relatedNavs', {
    templateUrl: '/app/app-portal/components/related-navigations/view.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', '$q', function ($rootScope, $scope, ngAppSettings, $q) {
        var ctrl = this;
        ctrl.selected = null;
        ctrl.activeItem = function (item) {            
            var currentItem = $rootScope.findObjectByKey(ctrl.navs, ['sourceId', 'destinationId']
                            , [ctrl.sourceId, item.id]);
            if (currentItem === null) {
                currentItem = item;
                currentItem.priority = ctrl.navs.length + 1;
                ctrl.navs.push(currentItem);
            }
        };
        ctrl.updateOrders = function (index) {
            ctrl.data.splice(index, 1);
            for (var i = 0; i < ctrl.data.length; i++) {
                ctrl.data[i].priority = i + 1;
            }
        };
        ctrl.load = function (pageIndex) {
            if (pageIndex) {
                ctrl.request.pageIndex = pageIndex;
            }
            ctrl.loadData({ pageIndex: ctrl.request.pageIndex });
        };
        ctrl.checkActived = function (item) {
            if (ctrl.navs) {
                return ctrl.navs.find(function (nav) {
                    return nav.destinationId === item.id;
                });
            }
        };
    }],
    bindings: {
        request: '=',
        prefix: '=',
        sourceId: '=',
        culture: '=',
        navs: '=',
        data: '=',
        loadData: '&'
    }
});