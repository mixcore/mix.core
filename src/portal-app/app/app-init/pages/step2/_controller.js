'use strict';
app.controller('Step2Controller', 
    ['$scope', '$rootScope', '$location', 'Step2Services', 'AuthService',
    function ($scope, $rootScope, $location, services,authService) {
    $scope.user = {
        userName: '',
        email: '',
        password: '',
        confirmPassword: '',
        isAgreed: false
    }
    $scope.register = async function () {
        if (!$scope.user.isAgreed) {
            $rootScope.showMessage('Please agreed with our policy', 'warning');
        } else {
            if ($scope.password !== $scope.confirmPassword) {
                $rootScope.showErrors(['Confirm Password is not matched']);
            } else {
                $rootScope.isBusy = true;
                var result = await services.register($scope.user);
                if (result.isSucceed) {
                    var loginData = {
                        userName: $scope.user.userName,
                        password: $scope.user.password,
                        rememberMe: true
                    }
                    var result = await authService.login(loginData);
                    if (result) {
                        $rootScope.isBusy = false;
                        $location.url('/init/step3')
                        $scope.$apply();
                    }
                    else{
                        if (result) {
                            $rootScope.showErrors(result.errors);
                        }
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                } else {
                    if (result) {
                        $rootScope.showErrors(result.errors);
                    }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            }
        }
    }
}]);