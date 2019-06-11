'use trick';
modules.directive('paneCollape', function () {
    return {
        require: '^collape-container',
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