'use strict';
app.controller('Step2Controller', ['$scope', '$rootScope', 'ngAppSettings', '$timeout', '$location', '$http', 'CommonService', 'Step2Services'
    , function ($scope, $rootScope, ngAppSettings, $timeout, $location, $http, commonService, services) {
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
            }
            else {
                if ($scope.password !== $scope.confirmPassword) {
                    $rootScope.showErrors(['Confirm Password is not matched']);
                }
                else {
                    $rootScope.isBusy = true;
                    var result = await services.register($scope.user);
                    if (result.isSucceed) {
                        $rootScope.isBusy = false;
                        window.location.href = '/portal';
                    } else {
                        if (result) { $rootScope.showErrors(result.errors); }
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                }
            }
        }
    }]);
