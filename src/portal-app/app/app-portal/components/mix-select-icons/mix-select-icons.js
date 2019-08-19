
modules.component('mixSelectIcons', {
    templateUrl: '/app/app-portal/components/mix-select-icons/mix-select-icons.html',
    controller: ['$rootScope', '$scope', '$location', function ($rootScope, $scope, $location) {
        var ctrl = this;
        ctrl.translate = function (keyword) {
            return $rootScope.translate(keyword);
        };
        ctrl.select = function(ico){
            ctrl.data = ico.class;
        }
    }],
    bindings: {
        data: '=',
        prefix: '=',
        options: '=',
    }
});