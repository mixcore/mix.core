'use strict';
app.controller('AppSettingsController', 
    ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$timeout', '$location', 
'AuthService','CommonService', 'AppSettingsServices',
    function ($scope, $rootScope, ngAppSettings, $routeParams, $timeout, $location, authService, commonService, appSettingsServices) {

        $scope.appSettings = null;
        $scope.errors = [];
        $scope.statuses = ngAppSettings.contentStatuses;
        $scope.getAppSettings = async function (id) {
            $rootScope.isBusy = true;
            var resp = await appSettingsServices.getAppSettings();
            if (resp && resp.isSucceed) {
                $scope.appSettings = resp.data;
                $rootScope.initEditor();
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
        $scope.loadAppSettings = async function () {
            $rootScope.isBusy = true;

            var id = $routeParams.id;
            var response = await appSettingsServices.getAppSettings();
            if (response.isSucceed) {
                $scope.appSettings = response.data;
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }

            var result = await commonService.getSettings();
            if (result.isSucceed) {
                $scope.settings = result.data;
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.isBusy = false;
            }
        };

        $scope.saveAppSettings = async function (appSettings) {
            $rootScope.isBusy = true;
            var resp = await appSettingsServices.saveAppSettings(appSettings);
            if (resp && resp.isSucceed) {
                $scope.appSettings = resp.data;
                $rootScope.showMessage('success', 'success');
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $scope.$apply();
            }
        };

    }]);
