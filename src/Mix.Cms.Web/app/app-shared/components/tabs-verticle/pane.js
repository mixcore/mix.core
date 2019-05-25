'use trick';
modules.directive('paneV', function () {
    return {
        require: '^tabs-v',
        restrict: 'E',
        transclude: true,
        scope: { title: '@' },
        link: function (scope, element, attrs, tabsController) {
            tabsController.addPane(scope);
        },
        template:
            '<div class="tab-pane" ng-class="{active: selected}" ng-transclude>' +
            '</div>',
        replace: true
    };
})