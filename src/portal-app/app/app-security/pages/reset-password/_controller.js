'use strict';
app.controller('ResetPasswordController',
    ['$scope', '$rootScope', 'AuthService',
        function ($scope, $rootScope, service) {
            $scope.user = {
                email: '',
                password: '',
                confirmPassword: '',
                code: '',
            }
            $scope.init = function(){

            };
            $scope.submit = async function () {

                if ($scope.password !== $scope.confirmPassword) {
                    $rootScope.showErrors(['Confirm Password is not matched']);
                } else {
                    $rootScope.isBusy = true;
                    var result = await service.resetPassword($scope.user);
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
        }]);