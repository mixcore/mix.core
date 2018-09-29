'use strict';
app.controller('ConfigurationController', ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$timeout', '$location', 'AuthService', 'ConfigurationServices','GlobalSettingsService',
    function ($scope, $rootScope, ngAppSettings, $routeParams, $timeout, $location, authService, configurationServices, GlobalSettingsService) {
        $scope.request = angular.copy(ngAppSettings.request);

        $scope.request.contentStatuses = [
            'Site',
            'System'
        ];
        $scope.request.status = '0';
        $scope.configurationFile = {
            file: null,
            fullPath: '',
            folder: 'Configuration',
            title: '',
            description: ''
        };
        $scope.cates = [
            'Site',
            'System'
        ];
        $scope.dataTypes = ngAppSettings.editorConfigurations.dataTypes;
        $scope.activedConfiguration = null;
        $scope.relatedConfigurations = [];
        $rootScope.isBusy = false;
        $scope.data = {
            pageIndex: 0,
            pageSize: 1,
            totalItems: 0
        };
        $scope.errors = [];

        $scope.range = function (max) {
            var input = [];
            for (var i = 1; i <= max; i += 1) input.push(i);
            return input;
        };
        $scope.initEditors = function () {
            $rootScope.initEditor();
        };

        $scope.getConfiguration = async function (id) {
            $rootScope.isBusy = true;
            var resp = await configurationServices.getConfiguration(id, 'portal');
            if (resp && resp.isSucceed) {
                $scope.activedConfiguration = resp.data;
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
        $scope.loadConfiguration = async function () {
            $rootScope.isBusy = true;
            var id = $routeParams.id;
            var response = await configurationServices.getConfiguration(id, 'portal');
            if (response.isSucceed) {
                $scope.activedConfiguration = response.data;
                $rootScope.initEditor();
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.loadConfigurations = async function (pageIndex) {
            if (pageIndex !== undefined) {
                $scope.request.pageIndex = pageIndex;
            }
            $rootScope.isBusy = true;
            var resp = await configurationServices.getConfigurations($scope.request);
            if (resp && resp.isSucceed) {

                $scope.data = resp.data;
                $.each($scope.data.items, function (i, configuration) {

                    $.each($scope.activedConfigurations, function (i, e) {
                        if (e.configurationId === configuration.id) {
                            configuration.isHidden = true;
                        }
                    });
                });
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.removeConfiguration = function (id) {
            $rootScope.showConfirm($scope, 'removeConfigurationConfirmed', [id], null, 'Remove Configuration', 'Are you sure');
        };

        $scope.removeConfigurationConfirmed = async function (id) {
            $rootScope.isBusy = true;
            var result = await configurationServices.removeConfiguration(id);
            if (result.isSucceed) {
                GlobalSettingsService.reset($rootScope.globalSettings.lang).then(function () {
                    window.location.href = '/portal/configuration/list';
                });
            }
            else {
                $rootScope.showErrors(result.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.saveConfiguration = async function (configuration) {
            configuration.content = $('.editor-content').val();
            configuration.dataType = configuration.property.dataType;
            $rootScope.isBusy = true;
            var resp = await configurationServices.saveConfiguration(configuration);
            if (resp && resp.isSucceed) {
                $scope.activedConfiguration = resp.data;
                $rootScope.showMessage('Thành công', 'success');
                
                GlobalSettingsService.reset($rootScope.globalSettings.lang).then(function () {
                    window.location.href = '/portal/configuration/list';
                });
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

    }]);
