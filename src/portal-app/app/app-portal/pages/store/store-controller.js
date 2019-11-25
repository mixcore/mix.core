'use strict';
app.controller('StoreController',
    ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$location', 'StoreService', 'CommonService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $location, service, commonService) {
            BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            $scope.cates = ngAppSettings.enums.configuration_types;
            $scope.settings = $rootScope.globalSettings;
            $scope.saveSuccessCallback = function () {
                commonService.initAllSettings().then(function () {
                    $location.url($scope.referrerUrl);
                });
            }
            $scope.removeCallback = function () {
                commonService.initAllSettings().then(function () {
                    $location.url($scope.referrerUrl);
                });
            }

            $scope.getList = function(){
                $scope.data.items = [];
            };

            // $scope.items = [];
            // $scope.init = function () {
            //     var req = {
            //         method: 'GET',
            //         url: 'https://api.github.com/repos/mixcore/mix.core/contributors'
            //     };
            //     $scope.getGithubApiResult(req);
            // };

            // $scope.getGithubApiResult = async function (req) {
            //     return $http(req).then(function (resp) {
            //         if (resp.status == '200') {
            //             $scope.items = resp.data;
            //         }
            //         else {
            //             console.log(resp);

            //         }
            //     },
            //         function (error) {
            //             return { isSucceed: false, errors: [error.statusText || error.status] };
            //         });
            // };
        }]);
