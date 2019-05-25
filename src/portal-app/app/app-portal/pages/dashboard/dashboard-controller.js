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
            // $('#mainSection').removeClass('card');
            $scope.data = response.data;
            $rootScope.isBusy = false;
            $scope.$apply();
            // $scope.getChart();
        }
        else {
            $rootScope.showErrors(response.errors);
            $rootScope.isBusy = false;
            $scope.$apply();
        }
    }
    $scope.getChart = function () {
        var ctx = document.getElementById("myChart");
        var myChart = new Chart(ctx, {
            // type: 'pie',
            // data: {
            //   labels: ["Africa", "Asia", "Europe", "Latin America", "North America"],
            //   datasets: [{
            //     label: "Population (millions)",
            //     backgroundColor: ["#3e95cd", "#8e5ea2","#3cba9f","#e8c3b9","#c45850"],
            //     data: [2478,5267,734,784,433]
            //   }]
            // },
            // options: {
            //   title: {
            //     display: true,
            //     text: 'Predicted world population (millions) in 2050'
            //   }
            // }
        });
    }
}]);