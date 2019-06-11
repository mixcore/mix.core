
modules.directive('collapeContainer', function () {
    return {
        restrict: 'E',
        transclude: true,
        scope: {
            addCallback: '&',
            title:'=',
            activedData:'=',
        },        
        controller: function ($scope,$rootScope, $element) {
            var panes = $scope.panes = [];
            $scope.id=$rootScope.generateUUID();
            $scope.select = function (pane) {
                angular.forEach(panes, function (pane) {
                    pane.selected = false;
                });
                pane.selected = true;
                this.addCallback();
            }

            this.addPane = function (pane) {
                if (panes.length === 0) $scope.select(pane);
                panes.push(pane);
            }
        },
        templateUrl: '/app/app-shared/components/collape-container/view.html',
        replace: true
    };
});