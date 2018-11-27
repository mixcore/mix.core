
modules.component('navs', {
    templateUrl: '/app/app-shared/components/navigations/navigations.html',
    controller: ['$scope', '$location', function ($scope, $location) {
        var ctrl = this;
        ctrl.selected = null;
        ctrl.updateOrders = function (index) {
            ctrl.data.splice(index, 1);
            for (var i = 0; i < ctrl.data.length; i++) {
                ctrl.data[i].priority = i + 1;
            }
        };
        ctrl.goToDetails = async function (nav) {
            $location.path(ctrl.detailsUrl + nav[ctrl.key]);
        };
    }],
    bindings: {
        prefix: '=',
        detailsUrl: '=',
        key: '=',
        data: '=',
        callback: '&'
    }
});