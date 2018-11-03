modules.component('confirm', {
    templateUrl: '/app/app-shared/components/confirm/confirm.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', function ($rootScope, $scope) {
        var ctrl = this;
        ctrl.executeFunctionByName =async function (functionName, args, context) {
            var result = await $rootScope.executeFunctionByName(functionName, args, context);
            if (result) {
                $scope.$apply();
            }
        }
    }],
    bindings: {
        message: '='
    }
});