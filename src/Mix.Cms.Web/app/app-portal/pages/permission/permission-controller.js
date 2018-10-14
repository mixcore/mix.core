'use strict';
app.controller('PermissionController',
    ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$location', 'PermissionService', 'CommonService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $location, service, commonService) {
            BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            $scope.request.query = "level=0";
            $scope.columns = [
                {
                    title: 'Keyword',
                    name: 'textKeyword',
                    filter: true,
                    type: 0 // string - ngAppSettings.dataTypes[0]
                },
                {
                    title: 'Default',
                    name: 'textDefault',
                    filter: true,
                    type: 0// string - ngAppSettings.dataTypes[0]
                },
                {
                    title: 'Url',
                    name: 'url',
                    filter: true,
                    type: 0 // string - ngAppSettings.dataTypes[0]
                },
                {
                    title: 'Created Date',
                    name: 'createdDateTime',
                    filter: true,
                    type: 0 // string - ngAppSettings.dataTypes[0]
                },
            ];

            $scope.updateInfos = async function (items) {
                $rootScope.isBusy = true;
                var resp = await service.updateInfos(items);
                if (resp && resp.isSucceed) {
                    $scope.activedPage = resp.data;
                    $rootScope.showMessage('success', 'success');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };

            $scope.updateChildInfos = async function (items) {
                $rootScope.isBusy = true;
                var resp = await service.updateChildInfos(items);
                if (resp && resp.isSucceed) {
                    $scope.activedPage = resp.data;
                    $rootScope.showMessage('success', 'success');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
        }]);
