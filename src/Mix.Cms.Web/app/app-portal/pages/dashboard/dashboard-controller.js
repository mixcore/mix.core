'use strict';
app.controller('DashboardController', ['$scope', '$rootScope', 'ngAppSettings', '$timeout', '$location', 'DashboardServices', function ($scope, $rootScope, ngAppSettings, $timeout, $location, dashboardServices) {
    $scope.pageClass = 'page-dashboard';
    $('.side-nav li').removeClass('active');
    $('.side-nav .page-dashboard').addClass('active');
    $scope.data = {
        totalPage: 0,
        totalArticle: 0,
        totalProduct: 0,
        totalUser: 0
    }
    $scope.users = [];
    $scope.$on('$viewContentLoaded', function () {
        $rootScope.isBusy = false;

    });
    $scope.getDashboardInfo = async function () {
        $rootScope.isBusy = true;
        var response = await dashboardServices.getDashboardInfo();
        if (response.isSucceed) {
            $scope.data = response.data;
            $rootScope.isBusy = false;
            $scope.$apply();
        }
        else {
            $rootScope.showErrors(response.errors);
            $rootScope.isBusy = false;
            $scope.$apply();
        }
    }
}]);
