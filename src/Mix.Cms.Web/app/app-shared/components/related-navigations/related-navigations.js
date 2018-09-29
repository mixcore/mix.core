
modules.component('relatedNavs', {
    templateUrl: '/app-shared/components/related-navigations/related-navigations.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', '$q', function ($rootScope, $scope, ngAppSettings, $q) {
        var ctrl = this;
        ctrl.selected = null;
        ctrl.activeItem = function (item) {
            var currentItem = null;
            $.each(ctrl.navs, function (i, e) {
                if (e.destinationId === item.id) {
                    e.isActived = item.isActived;
                    currentItem = e;
                    return false;
                }
            });
            if (currentItem === null) {
                currentItem = {
                    destinationId: item.id,
                    sourceId: ctrl.sourceId,
                    image: item.imageUrl,
                    description: item.title,
                    specificulture: ctrl.culture,
                    priority: ctrl.navs.length + 1,
                    isActived: true
                };
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
            ctrl.loadData({ pageIndex: ctrl.request.pageIndex }).then(function () {

                angular.forEach(ctrl.data.items, function (value, key) {
                    var deferred = $q.defer();
                    value.isActived = ctrl.checkActived(value) !== undefined;
                    deferred.resolve();
                });
            });
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