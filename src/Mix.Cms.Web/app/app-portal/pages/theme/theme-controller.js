'use strict';
app.controller('ThemeController', ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$timeout', '$location', 'AuthService', 'ThemeServices',
    function ($scope, $rootScope, ngAppSettings, $routeParams, $timeout, $location, authService, themeServices) {
        $scope.request = {
            pageSize: '10',
            pageIndex: 0,
            status: '2',
            orderBy: 'Priority',
            direction: '0',
            fromDate: null,
            toDate: null,
            keyword: ''
        };

        $scope.activedTheme = null;

        $scope.relatedThemes = [];

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

        $scope.getTheme = async function (id) {
            $rootScope.isBusy = true;
            var resp = await themeServices.getTheme(id, 'portal');
            if (resp && resp.isSucceed) {
                $scope.activedTheme = resp.data;
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

        $scope.syncTemplates = async function (id) {
            $rootScope.isBusy = true;
            var response = await themeServices.syncTemplates(id);
            if (response.isSucceed) {
                $scope.activedTheme = response.data;
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.loadTheme = async function () {
            $rootScope.isBusy = true;
            var id = $routeParams.id;
            var response = await themeServices.getTheme(id, 'portal');
            if (response.isSucceed) {
                $scope.activedTheme = response.data;
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
        $scope.loadThemes = async function (pageIndex) {
            if (pageIndex != undefined) {
                $scope.request.pageIndex = pageIndex;
            }
            if ($scope.request.fromDate != null) {
                var d = new Date($scope.request.fromDate);
                $scope.request.fromDate = d.toISOString();
            }
            if ($scope.request.toDate != null) {
                var d = new Date($scope.request.toDate);
                $scope.request.toDate = d.toISOString();
            }
            $rootScope.isBusy = true;
            var resp = await themeServices.getThemes($scope.request);
            if (resp && resp.isSucceed) {

                ($scope.data = resp.data);
                //$("html, body").animate({ "scrollTop": "0px" }, 500);
                $.each($scope.data.items, function (i, theme) {

                    $.each($scope.activedThemes, function (i, e) {
                        if (e.themeId == theme.id) {
                            theme.isHidden = true;
                        }
                    })
                })
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.saveTheme = async function (theme) {
            $rootScope.isBusy = true;
            var resp = await themeServices.saveTheme(theme);
            if (resp && resp.isSucceed) {
                $scope.activedTheme = resp.data;
                $rootScope.showMessage('Thành công', 'success');
                $rootScope.isBusy = false;
                $rootScope.updateSettings();
                $location.path('/portal/theme/list');
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.removeTheme = function (id) {
            $rootScope.showConfirm($scope, 'removeThemeConfirmed', [id], null, 'Remove Theme', 'Are you sure');
        };

        $scope.removeThemeConfirmed = async function (id) {
            $rootScope.isBusy = true;
            var result = await themeServices.removeTheme(id);
            if (result.isSucceed) {
                $scope.loadThemes();
            }
            else {
                $rootScope.showMessage('failed');
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        }
    }]);
