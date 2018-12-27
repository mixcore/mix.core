
modules.component('jumbotrons', {
    templateUrl: '/app/app-portal/components/jumbotrons/jumbotrons.html',
    controller: ['$rootScope', '$scope', '$location', function ($rootScope, $scope, $location) {
        var ctrl = this;
        ctrl.translate = function (keyword) {
            return $rootScope.translate(keyword);
        };
        // ctrl.back = function () {
        //     ctrl.backUrl = ctrl.backUrl || '/admin';
        //     $location.path(ctrl.backUrl);
        // };
    }],
    bindings: {
        tagName: '=',
        tagType: '=',
    }
});