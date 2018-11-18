'use strict';
app.controller('AppPortalController', ['$rootScope', '$scope', 'ngAppSettings', '$location'
    , 'CommonService', 'AuthService', 'TranslatorService', 'GlobalSettingsService', 'RoleService',
    function ($rootScope, $scope, ngAppSettings, $location, commonService, authService, translatorService, configurationService, roleServices) {
        $scope.isInit = false;
        $scope.isAdmin = false;
        $scope.translator = translatorService;
        $scope.configurationService = configurationService;
        $scope.lang = '';
        $scope.settings = {};
        $scope.init = function () {
            
            if (!$rootScope.isBusy) {
                $rootScope.isBusy = true;
                $rootScope.configurationService.fillGlobalSettings().then(function (response) {
                    $scope.isInit = true;
                    $rootScope.globalSettings = response;
                    ngAppSettings.globalSettings = response;
                    if ($rootScope.globalSettings) {
                        $rootScope.translator.fillTranslator($rootScope.globalSettings.lang).then(function () {

                            commonService.fillSettings().then(function (response) {

                                authService.fillAuthData().then(function (response) {
                                    $rootScope.authentication = authService.authentication;
                                    if (authService.authentication && authService.authentication.isAuth) {
                                        $scope.isAdmin = authService.authentication.isAdmin;
                                        if (!$scope.isAdmin) {

                                            roleServices.getPermissions().then(function (response) {

                                                if (response && response.isSucceed) {

                                                    $scope.isInit = true;
                                                    $scope.roles = response.data;
                                                    $rootScope.isBusy = false;
                                                    $scope.$apply();
                                                }
                                            });
                                        }
                                    }
                                    else {
                                        window.top.location.href = '/init/login';
                                    }
                                });
                                $rootScope.isBusy = false;
                                $scope.$apply();
                            });
                        });

                    } else {
                        window.top.location.href = '/init/login';
                    }
                });
            }
        };
    }]);
