(function (angular) {
    'use strict';
    app.controller('AppClientController', [ '$rootScope', '$scope', 'ngAppSettings', 'CommonService', 'AuthService'
        , 'TranslatorService', 'ModuleDataService',
        function ($rootScope, $scope, ngAppSettings, commonService, authService, translatorService, moduleDataService) {
            $scope.lang = '';
            $scope.isInit = false;
            $scope.init = async function (lang) {
                if (!$rootScope.isBusy) {
                    $rootScope.isBusy = true;
                    commonService.fillSettings(lang).then(function (response) {
                        $scope.isInit = true;
                        $rootScope.globalSettings = response;
                        if ($rootScope.globalSettings) {
                            $scope.settings = $rootScope.globalSettings;
                            $rootScope.translator.fillTranslator(lang).then(function () {
                                authService.fillAuthData().then(function (response) {
                                    $rootScope.authentication = authService.authentication;
                                });
                                $rootScope.isBusy = false;
                                $scope.$apply();
                            });

                        } else {
                            $rootScope.isBusy = false;
                        }
                    });
                }
            };

            $scope.translate = $rootScope.translate;

            $scope.initModuleForm = async function (name) {
                var resp = null;
                $scope.name = name;
                if ($scope.id) {
                    resp = await moduleDataService.getModuleData($scope.id, $scope.dataId, 'portal');
                }
                else {
                    resp = await moduleDataService.initModuleForm($scope.name);
                }

                if (resp && resp.isSucceed) {
                    $scope.activedModuleData = resp.data;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
            $scope.saveModuleData = async function () {

                var resp = await moduleDataService.saveModuleData($scope.activedModuleData);
                if (resp && resp.isSucceed) {
                    $scope.activedModuleData = resp.data;
                    $rootScope.showMessage('Success', 'success');
                    $rootScope.isBusy = false;
                    $scope.initModuleForm($scope.name);
                    $rootScope.isBusy = false;
                    $scope.$apply();
                    //$location.path('/portal/moduleData/details/' + resp.data.id);
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
        }]);


})(window.angular);