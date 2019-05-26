'use strict';
app.controller('RegisterController', 
    ['$scope', '$rootScope', 'RegisterServices', 
    function ($scope, $rootScope, services) {
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
                    $rootScope.isBusy = false;
                    window.location.href = '/security/login';
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