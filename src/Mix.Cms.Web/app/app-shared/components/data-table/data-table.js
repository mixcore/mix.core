modules.component('swDataTable', {
    templateUrl: '/app-shared/components/data-table/data-table.html',
    controller: ['$rootScope', '$scope', '$location', 'ngAppSettings', function ($rootScope, $scope, $location, ngAppSettings) {
        var ctrl = this;
        ctrl.colWidth = 3;
        ctrl.init = function () {
            ctrl.min = ctrl.data.items[0].priority;
            ctrl.colWidth = parseInt(9 / ctrl.columns.length);
            ctrl.lastColWidth = (9 % ctrl.columns.length) > 0 ? 2 : 1;
        };
        ctrl.translate = $rootScope.translate;
        ctrl.selected = null;
        ctrl.updateOrders = function (index, items) {
            items.splice(index, 1);
            for (var i = 0; i < items.length; i++) {
                items[i].priority = ctrl.min + i;
            }
            ctrl.onUpdateInfos({ items: items });
        };

        ctrl.updateChildOrders = function (index, items) {
            items.splice(index, 1);
            for (var i = 0; i < items.length; i++) {
                items[i].priority = ctrl.min + i;
            }
            ctrl.onUpdateChildInfos({ items: items });
        };
        ctrl.dragoverCallback = function (index, item, external, type) {
            //console.log('drop ', index, item, external, type);
        }
        ctrl.insertCallback = function (index, item, external, type) {
            //console.log('insert ', index, item, external, type);
        }
        ctrl.delete = function (id) {
            ctrl.onDelete({ id: id });
        };
        ctrl.goTo = function (id) {
            $location.path(ctrl.editUrl + '/' + id);
        }
    }],
    bindings: {
        data: '=',
        childName: '=',
        canDrag: '=',
        editUrl: '=',
        columns: '=',
        onDelete: '&',
        onUpdateInfos: '&',
        onUpdateChildInfos: '&',
    }
});